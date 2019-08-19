using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GameJam
{
    interface ICVar
    {
        string Serialize();
        bool Deserialize(string val);
    }

    class CVar<T> : ICVar
    {
        public string Name
        {
            get;
            private set;
        }
        public T Value;

        public CVar(string name, T val)
        {
            Name = name;
            Value = val;
        }

        public bool Deserialize(string value)
        {
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
            return Name + " = " + Value;
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
                ini += _cvars[key].Serialize() + Environment.NewLine;
            }

            return ini;
        }
        private static bool DeserializeAll(string iniSet)
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
                if(_cvars[name].Deserialize(value))
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

        private static void Create<T>(string name, T value)
        {
            _cvars.Add(name, new CVar<T>(name, value));
        }
        
        public static void Initialize()
        {
            CreateDefaultCVars();
            SynchronizeFromFile();
        }

        private static bool Load()
        {
            string path = GetSavePath();
            if(File.Exists(path))
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);
                    return DeserializeAll(reader.ReadToEnd());
                }
            }

            return false;
        }
        private static void Save()
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
            if(!Load())
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
