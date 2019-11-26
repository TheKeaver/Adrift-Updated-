using System.Diagnostics;

namespace System
{
    public static class Console
    {
        public static void WriteLine(string s, params object[] o)
        {
            Debug.WriteLine(string.Format(s, o));
        }

        static void WriteLine<T>(T t)
        {
            WriteLine(t.ToString());
        }
    }
}