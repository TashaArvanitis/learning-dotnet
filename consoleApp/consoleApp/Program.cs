using System;

namespace consoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            Console.WriteLine("Counting...");
            for (int i = 0; i<30; i++)
            {
                Console.Write(count + " ");
                count += 1;
                if (i % 3 == 0) {
                    Console.Write("fizz");
                }
                if (i % 5 == 0) {
                    Console.Write("buzz");
                }
                Console.WriteLine();
            }
        }
    }
}
