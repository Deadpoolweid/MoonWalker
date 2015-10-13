using System;

namespace MoonWalker
{
    class Program
    {
        static void Main()
        {
            Start:
            try
            {
                Output output = new Output();
                Control control = new Control();
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
