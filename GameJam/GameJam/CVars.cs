using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace GameJam
{
    static class CVarFlags
    {
        public static int PRESERVE = 0x1;
        public static int DEV_PRESERVE = 0x2;
        public static int LIVE_RELOAD = 0x4;
    }

    interface ICVar
    {
        string Serialize();
        string SerializeDefault();
        bool Deserialize(string val);

        void Reset();

        int Flags
        {
            get;
        }

        Type UnderlyingType
        {
            get;
        }
    }

    class CVar<T> : ICVar
    {
        public string Name
        {
            get;
            private set;
        }
        public T Value;
        public readonly T DefaultValue;

        public int Flags
        {
            get;
            private set;
        } = 0;

        Type ICVar.UnderlyingType
        {
            get
            {
                return typeof(T);
            }
        }

        public CVar(string name, T val, int flags = 0)
        {
            Name = name;
            Value = val;
            DefaultValue = val;
            Flags = flags;
        }

        public bool Deserialize(string value)
        {
            if(typeof(T) == typeof(Color))
            {
                string[] rgb_S = value.Split(',');
                if(rgb_S.Length < 3)
                {
                    return false;
                }
                try
                {
                    Value = (T)Activator.CreateInstance(typeof(T), int.Parse(rgb_S[0]), int.Parse(rgb_S[1]), int.Parse(rgb_S[2]));
                } catch (Exception)
                {
                    return false;
                }
                return true;
            }

            try
            {
                Value = (T)Convert.ChangeType(value, typeof(T));
            } catch (Exception)
            {
                return false;
            }
            return true;
        }

        public string Serialize()
        {
            if (typeof(T) == typeof(Color))
            {
                Color color = (Color)Activator.CreateInstance(typeof(Color), Value, 0);
                return string.Format("{0},{1},{2}", color.R, color.G, color.B);
            }

            return Value.ToString();
        }
        public string SerializeDefault()
        {
            if (typeof(T) == typeof(Color))
            {
                Color color = (Color)Activator.CreateInstance(typeof(Color), DefaultValue, 0);
                return string.Format("{0},{1},{2}", color.R, color.G, color.B);
            }

            return DefaultValue.ToString();
        }

        public void Reset()
        {
            Value = DefaultValue;
        }
    }

    static partial class CVars
    {
        static Dictionary<string, ICVar> _cvars = new Dictionary<string, ICVar>();

        private static string SerializeAll()
        {
            string ini = "";

            foreach (string key in _cvars.Keys)
            {
                if ((_cvars[key].Flags & CVarFlags.PRESERVE) == 0
#if DEBUG
                    && (_cvars[key].Flags & CVarFlags.DEV_PRESERVE) == 0
#endif
                    )
                {
                    continue;
                }
                ini += key + " = " + _cvars[key].Serialize() + Environment.NewLine;
            }

            return ini;
        }
        private static bool DeserializeAll(string iniSet, bool live)
        {
            string[] lines = iniSet.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<string> updatedCVars = new List<string>();
            foreach(string line in lines)
            {
                if(line.Trim().Length == 0)
                {
                    continue;
                }
                string[] s = line.Split('=');
                string name = s[0].Trim();
                string value = s[1].Trim();
                if(!_cvars.ContainsKey(name))
                {
                    Console.WriteLine("'{0}' is not a valid CVar name. Does it exist?", name);
                    continue;
                }

                if((_cvars[name].Flags & CVarFlags.PRESERVE) != 0
#if DEBUG
                    || (_cvars[name].Flags & CVarFlags.DEV_PRESERVE) != 0
#endif
                    )
                {
                    if(live && (_cvars[name].Flags & CVarFlags.LIVE_RELOAD) == 0)
                    {
                        Console.WriteLine("WARNING: CVar `{0}` has not been as a live reloadable cvar. It may requite a game state change or game restart.", name);
                    }
                    if (_cvars[name].Deserialize(value))
                    {
                        updatedCVars.Add(name);
                    }
                } else
                {
                    updatedCVars.Add(name);
                }
            }

            return updatedCVars.Count == _cvars.Count;
        }

        public static ref T Get<T>(string name)
        {
            if(!_cvars.ContainsKey(name))
            {
                throw new Exception(string.Format("CVar `{0}` does not exist.", name));
            }
            CVar<T> cvar = _cvars[name] as CVar<T>;
            if(cvar == null)
            {
                throw new Exception(string.Format("Cannot cast `{0}` into the type requested. Does it match the type in the CVar defaults?", name));
            }
            return ref cvar.Value;
        }

        public static ICVar RawGet(string name)
        {
            if (!_cvars.ContainsKey(name))
            {
                throw new Exception(string.Format("CVar `{0}` does not exist.", name));
            }
            return _cvars[name];
        }

        public static string[] GetNames()
        {
            return _cvars.Keys.ToArray();
        }

        private static void Create<T>(string name, T value, int flags = 0)
        {
            _cvars.Add(name, new CVar<T>(name, value, flags));
        }
        
        public static void Initialize()
        {
            CreateDefaultCVars();
            SynchronizeFromFile(false);
        }

        private static bool Load(bool live)
        {
            string path = GetSavePath();
            if(File.Exists(path))
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);
                    return DeserializeAll(reader.ReadToEnd(), live);
                }
            }

            return false;
        }
        public static void Save()
        {
            string path = GetSavePath();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(SerializeAll());
                writer.Flush();
            }
        }

        public static void SynchronizeFromFile()
        {
            SynchronizeFromFile(true);
        }
        private static void SynchronizeFromFile(bool live)
        {
            if(!Load(live))
            {
                Save();
            }
        }

        private static string GetSavePath()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            string path;
            switch (pid)
            {
                case PlatformID.WinCE:
                case PlatformID.Win32S:
                case PlatformID.Win32NT:
                case PlatformID.Win32Windows:
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                       "AdriftGame");
                    break;
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    {
                        IntPtr buf = IntPtr.Zero;
                        try
                        {
                            buf = Marshal.AllocHGlobal(8192);
                            if (uname(buf) == 0)
                            {
                                string un = Marshal.PtrToStringAnsi(buf);
                                if (un == "Darwin")
                                {
                                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                                       "Library", "Application Support", "com.teamequinox.adrift");
                                }
                                else
                                {
                                    path = Directory.GetCurrentDirectory();
                                }
                            }
                            else
                            {
                                path = Directory.GetCurrentDirectory();
                            }
                        }
                        catch
                        {
                            path = Directory.GetCurrentDirectory();
                        }
                        finally
                        {
                            if (buf != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(buf);
                            }
                        }
                    }
                    break;
                default:
                    path = Directory.GetCurrentDirectory();
                    break;
            }
            path = Path.Combine(path, "cvars.ini");

            return path;
        }

        [DllImport("libc")]
        static extern int uname(IntPtr buf);
    }
}
