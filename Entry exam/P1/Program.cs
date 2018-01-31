using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1
{
    class Program
    {
        static void Main(string[] args)
        {
            var startInterval = int.Parse(Console.ReadLine());
            var endInterval = int.Parse(Console.ReadLine());
            var magicNumber = int.Parse(Console.ReadLine());

            var combination = 0;
            for (int i = startInterval; i <= endInterval; i++)
            {
                for (int j = startInterval; j <= endInterval; j++)
                {
                    combination++;
                    if (i + j == magicNumber)
                    {
                        Console.WriteLine($"Combination N:{combination} ({i} + {j} = {magicNumber})");
                        return;
                    }
                }
            }

            Console.WriteLine($"{combination} combinations - neither equals {magicNumber}");
        }
    }
}
