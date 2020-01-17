using System;
using System.Text;

namespace Osu_Map_Recovery
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private static void ConsoleSetup()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.SetWindowSize(80, 23);
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            Console.Title = "Osu Map Recovery";
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("||                                                                           ||");
            Console.WriteLine("||                           Made by Joel Helbling                           ||");
            Console.WriteLine("||               https://github.com/camperguy/osu-map-recovery               ||");
            Console.WriteLine("||                                                                           ||");
            Console.WriteLine("-------------------------------------------------------------------------------");
            System.Threading.Thread.Sleep(2000);
        }
    }
}
