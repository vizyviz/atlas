using System;

namespace Atlas.Console
{
    internal class ConsoleWrapper : IWrapConsole
    {
        public ConsoleKey ReadKey()
        {
            return System.Console.ReadKey().Key;
        }

        public void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        public void WriteLine(string format, params object[] arg)
        {
            System.Console.WriteLine(format, arg);
        }
    }
}
