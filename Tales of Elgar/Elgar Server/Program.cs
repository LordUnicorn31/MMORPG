using System.Threading;
using System;

namespace Elgar_Server
{
    class Program
    {
        private static Thread consoleThread;
        private static General general;

        static void Main(string[] args)
        {
            general = new General();
            consoleThread = new Thread(new ThreadStart(ConsoleThread));
            consoleThread.Start();
            general.InitializeServer();

        }

        static void ConsoleThread()
        {
            Console.ReadLine();
        }
    }
}
