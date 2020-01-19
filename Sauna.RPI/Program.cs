using System;
using System.Threading.Tasks;

namespace Sauna.RPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var sauna = new SaunaController();
            sauna.Init();

            Console.ReadLine();
        }

    }
}