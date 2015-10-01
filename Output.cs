using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonWalker
{
    class Output
    {
        public void main(Data data)
        {
            Console.WriteLine();
            Console.WriteLine("Координаты лунохода: ({0};{1})", data.XY.X,data.XY.Y);
            Console.WriteLine("Направление движения: {0}", data.d);
            Console.WriteLine("Карта: ");
            int x = 0, y;
            foreach (var VARIABLE in data.map)
            {
                if (x == 14)
                {
                    Console.WriteLine(VARIABLE);
                    x = 0;
                }
                else
                {
                    Console.Write(VARIABLE);
                    x++;
                }
            }
        }
    }
}
