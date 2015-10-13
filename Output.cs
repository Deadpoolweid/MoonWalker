using System;

namespace MoonWalker
{
    internal class Output
    {
        public void main(Action action)
        {
            // Выводим на экран действие
            Console.WriteLine(action);
        }
    }
}