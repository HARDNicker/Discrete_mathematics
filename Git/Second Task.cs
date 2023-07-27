using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Web;

namespace Задание_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Задание 1
            Console.WriteLine("Введите колличество элементов множества A :");
            int count_A = int.Parse(Console.ReadLine());
            string[] Сortege_A = new string[count_A];
            Console.WriteLine("Введите элементы множества A :");
            for (int i = 0; i < count_A; i++)
                Сortege_A[i] = Console.ReadLine();
            Console.WriteLine("Введите колличество элементов множества B :");
            int count_B = int.Parse(Console.ReadLine());
            string[] Сortege_B = new string[count_B];
            Console.WriteLine("Введите элементы множества B :");
            for (int i = 0; i < count_B; i++)
                Сortege_B[i] = Console.ReadLine();
            var product = from first in Сortege_A from second in Сortege_B select new[] { first, second };
            string[][] solution1 = product.ToArray();
            string[][] solution2 = product.ToArray();
            Console.WriteLine("Декартово произведение имеет вид:");
            Console.Write("{");
            foreach (var s in product)
            {
                Console.Write("(");
                
                foreach (var ss in s)
                    Console.Write($"{ss},");
                Console.Write("\b");
                Console.Write("),");
            }
            Console.Write("\b} \n");
            Console.Write("\n");
            bool flag_ref = true;
            for (int i = 0; i < solution1.Length; i++)
                for(int j =0; j < solution1[i].Length-1; j++)
                {
                    if (solution1[i][j] == solution1[i][j + 1])
                    {
                        if (flag_ref)
                        {
                            Console.WriteLine("Рефлексивность :");
                            flag_ref= false;
                        }
                        Console.Write($"({solution1[i][j]},{solution1[i][j + 1]})");
                    }
                }
            if (flag_ref == true)
                Console.Write("Рефлексивность отсутствует ");
            Console.WriteLine("\n");
            bool flag_sim = true;
            bool flag_simIn = true;
            int[] temp = new int[solution1.Length];
            for (int i = 0; i < solution1.Length; i++)
                for (int j = 0; j < solution1[i].Length - 1; j++)
                {
                    for (int ii = 0; ii < solution2.Length; ii++)
                        for (int jj = 0; jj < solution2[i].Length - 1; jj++)
                        {
                            if (solution1[i][j] == solution2[ii][jj+1] && solution1[i][j+1] == solution2[ii][jj])
                            {
                                if (flag_sim)
                                {
                                    Console.WriteLine("Симметричность :");
                                    flag_sim=false;
                                }
                                if (solution1[i][j] != solution1[i][j + 1] && temp[ii]!=i)
                                {
                                    flag_simIn = false;
                                    Console.WriteLine($"({solution1[i][j]},{solution1[i][j + 1]}) & ({solution2[ii][jj]},{solution2[ii][jj + 1]})");
                                    temp[i] = ii;
                                }
                            }
                        }
                }
            if(flag_sim==true || flag_simIn==true)
                Console.Write("Симметричность отсутствует ");
            Console.Write("\n");
            bool flag_tranz = true;
            for (int i = 0; i < solution1.Length; i++)
                for (int j = 0; j < solution1[i].Length - 1; j++)
                {
                    for (int ii = 0; ii < solution2.Length; ii++)
                        for (int jj = 0; jj < solution2[i].Length - 1; jj++)
                        {
                            if (solution1[i][j+1] == solution2[ii][jj])
                            {
                                if (flag_tranz)
                                {
                                    Console.WriteLine("Транзитивность :");
                                    flag_tranz = false;
                                }
                                if (solution1[i][j] != solution1[i][j + 1] && solution2[ii][jj]!= solution2[ii][jj+1])
                                    Console.WriteLine($"({solution1[i][j]},{solution1[i][j + 1]}) & ({solution2[ii][jj]},{solution2[ii][jj + 1]}) = ({solution1[i][j]},{solution2[ii][jj + 1]})");
                            }
                        }
                }
            if (flag_tranz == true)
                Console.Write("Транзитивность отсутствует ");
            Console.Write("\n");
            Console.ReadLine();
            // Задание 2
            Console.WriteLine("Выберете вариант задания : \n 0)X делится на Y \n 1)X делится на Y с остатком 1");
            int flagInt = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите колличество элементов множества X :");
            int count_X = int.Parse(Console.ReadLine());
            int[] X = new int[count_X];
            Console.WriteLine("Введите элементы множества X :");
            for (int i = 0; i < count_X; i++)
                X[i] = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите колличество элементов множества Y :");
            int count_Y = int.Parse(Console.ReadLine());
            int[] Y = new int[count_Y];
            Console.WriteLine("Введите элементы множества Y :");
            for (int i = 0; i < count_Y; i++)
                Y[i] = int.Parse(Console.ReadLine());
            Console.WriteLine(" ");
            int[] usesIn_X = new int[count_X];
            int[] usesIn_Y = new int[count_Y];
            for (int i = 0; i < count_X; i++)
                for (int j = 0; j < count_Y; j++)
                    if (X[i] % Y[j] == flagInt)
                    {
                        usesIn_X[i]++;
                        usesIn_Y[j]++;
                        Console.WriteLine($"{X[i]} -> {Y[j]}");
                    }

            int usesX = 0, usesY = 0;
            for (int i = 0; i < count_X; i++)
                if (usesIn_X[i] >= 1)
                    usesX++;
            for (int i = 0; i < count_Y; i++)
                if (usesIn_Y[i] >= 1)
                    usesY++;
            if (usesX == count_X)
                Console.WriteLine("Всюду определенное ");
            if (usesY == count_Y)
                Console.WriteLine("Cюрьективное");

            int Func = 0;
            for (int i = 0; i < count_X; i++)
                if (usesIn_X[i] <= 1)
                    Func++;
            if (Func == count_X)
                Console.WriteLine("Функциональное");

            int inect = 0;
            for (int i = 0; i < count_Y; i++)
                if (usesIn_Y[i] <= 1)
                    inect++;
            if (inect == count_Y)
                Console.WriteLine("Инъективное");
            if (!(usesX == count_X) && !(usesY == count_Y) && !(Func == count_X) && !(inect == count_Y))
                Console.WriteLine("Никакими свойствами не обладает");
            Console.ReadLine();
        }
    }
}
