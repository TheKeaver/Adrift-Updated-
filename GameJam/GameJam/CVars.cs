using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam
{
    interface ICVar
    {
        string Serialize();
        void Deserialize(string val);
    }

    class CVar<T> : ICVar
    {
        public T Value;

        public CVar(T val)
        {
            Value = val;
        }

        public void Deserialize(string val)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }

    static class CVars
    {
        static Dictionary<string, ICVar> _cvars = new Dictionary<string, ICVar>();

        public static string SerializeAll()
        {
            return "";
        }
        public static void DeserializeAll(string iniSet)
        {

        }

        public static ref T Get<T>(string name)
        {
            return ref (_cvars[name] as CVar<T>).Value;
        }

        private static void Create(string name, ICVar cvar)
        {
            _cvars.Add(name, cvar);
        }
        
        public static void Initialize()
        {
            CreateDefaultCVars();
        }

        private static void CreateDefaultCVars()
        {
            Create("test", new CVar<string>("Hi!"));
        }
    }
}
