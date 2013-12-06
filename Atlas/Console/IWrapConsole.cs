using System;

namespace Atlas.Console
{
    internal interface IWrapConsole
    {
        ConsoleKey ReadKey();
        void WriteLine(string message);
        void WriteLine(string format, params object[] arg);
    }
}