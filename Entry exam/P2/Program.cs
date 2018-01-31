using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2
{
    class Program
    {
        static void Main(string[] args)
        {
            var cubicFootIce = 195;
            var costOfFootIce = 1900;
            var requiredHeight = 30;


            var wallParts = Console.ReadLine()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Where(i => i < requiredHeight)
                .ToList();

            var usedIcePerDay = new List<int>();
            var wallToBuild = true;
            while (wallToBuild)
            {
                wallToBuild = false;
                var usedIceToday = 0;
                for (int i = 0; i < wallParts.Count; i++)
                {
                    if (wallParts[i] < requiredHeight)
                    {
                        wallParts[i]++;
                        usedIceToday += cubicFootIce;
                        wallToBuild = true;
                    }
                }

                if (usedIceToday != 0)
                {
                    usedIcePerDay.Add(usedIceToday);
                }
            }

            Console.WriteLine(string.Join(", ", usedIcePerDay));
            Console.WriteLine($"{usedIcePerDay.Sum() * costOfFootIce} coins");
        }
    }
}
