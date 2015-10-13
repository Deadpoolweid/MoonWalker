using System;

namespace MoonWalker
{
    internal class Program
    {
        private static void Main()
        {
            Start:
            try
            {
                var output = new Output();
                var control = new Control();
                output.main(control.main(Input.Core()));
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Возникла ошибка!");

                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            Console.Clear();
            goto Start;
        }
    }
}