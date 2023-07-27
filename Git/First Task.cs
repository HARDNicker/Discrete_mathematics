using System;
using System.Linq;

namespace Лаба__1
{
    internal class Program
    {
        static string[] Ex()
        {
            Console.WriteLine("Введите начальную границу");
            int limit1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите конечную границу");
            int limit2 = int.Parse(Console.ReadLine());
            string[] result = new string[Math.Abs(limit1) + Math.Abs(limit2) + 1];
            Console.WriteLine("Выберите вариант постановки скобок 1.(x,x], 2.[x,x), 3.(x,x), 4.[x,x]");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        if (limit1 < 0)
                            limit1++;
                        else
                            limit1--;
                        for (int i = 0, j = limit1; j <= limit2; i++, j++)
                        {
                            result[i] = j.ToString();
                        }
                        return result;
                    }
                case "2":
                    {
                        for (int i = 0, j = limit1; j < limit2; i++, j++)
                        {
                            result[i] = j.ToString();
                        }
                        return result;
                    }
                case "3":
                    {
                        if (limit1 < 0)
                            limit1++;
                        else
                            limit1--;
                        for (int i = 0, j = limit1; j < limit2; i++, j++)
                        {
                            result[i] = j.ToString();
                        }
                        return result;
                    }
                case "4":
                    {
                        for (int i = 0, j = limit1; j <= limit2; i++, j++)
                        {
                            result[i] = j.ToString();
                        }
                        return result;
                    }
                default:
                    Console.WriteLine("Неверное значенение");
                    break;
            }
            return result;
        }

        static void Boolean(int[] a, int n)
        {
            if (n == 0)
            {
                Console.Write("0");
                return;
            }
            int i = 0;
            while (n > 0)
            {
                if ((n & 1) == 1)
                    Console.Write("" + a[i] + ",");
                i++;
                n >>= 1;
            }
            Console.Write("\b");

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите множество A");
            string[] A = Ex();
            Console.WriteLine("Введите множество B");
            string[] B = Ex();
            Console.WriteLine("Введите множество C");
            string[] C = Ex();
            Console.WriteLine("Введите множество D");
            string[] D = Ex();
            Console.WriteLine("Введите множество E");
            string[] E = Ex();
            Console.Clear();

            //Задание 1
            var result1_1 = A.Intersect(B); // Пересечение
            var result2_1 = C.Union(E); // Обьеденение
            var result3_1 = result1_1.Except(result2_1); // Разность
            Console.Write("Ответ на первое задание : ");
            if (result3_1.Count() == 0)
                Console.Write("0 \n");
            else
                Console.WriteLine($"[{result3_1.First()},{result3_1.Last()}]");
            //Задание 2
            var result1_2 = B.Intersect(D);
            var result2_2 = E.Union(A);
            var result3_2 = C.Union(result1_2);
            var result4_2 = result2_2.Except(result3_2);
            Console.WriteLine("Ответ на второе задание : ");
            Console.WriteLine($"[{result4_2.First()},{int.Parse(result4_2.Last()) + 2}]");
            if (result4_2.Count() == 0)
                Console.Write("0 \n");
            else
                Console.WriteLine("\n");

            //Задание 3
            Console.WriteLine("Введите множество U");
            string[] U = Ex();
            var result1_3 = U.Except(A);
            string[] result2_3 = result1_3.ToArray();
            Console.WriteLine("Ответ на третье задание : ");
            Console.WriteLine($"[{result2_3.First()},{int.Parse(result2_3[result2_3.Length / 2 - 1]) + 1}) ({int.Parse(result2_3[result2_3.Length / 2]) - 1},{result2_3.Last()}]");
            if (result4_2.Count() == 0)
                Console.Write("0 \n");
            else
                Console.WriteLine("\n");

            //Задание 4
            var Сortege_A = new[] { "a,", "b,", "c,", "d,", "e,", "f," };
            var Сortege_B = new[] { ("1,2"), ("2,4"), ("4,5") };
            var product = from first in Сortege_A from second in Сortege_B select new[] { first, second };
            Console.WriteLine("Ответ на четвертое задание : ");
            Console.Write("{");
            foreach (var s in product)
            {
                Console.Write("(");
                foreach (var ss in s)
                    Console.Write($"{ss} ");
                Console.Write("),");
            }
            Console.Write("\b} \n");

            //Задание 5
            int N = 3;
            int[] a = { 1, 2, 3 };
            int r, i;
            r = 1 << N;
            Console.WriteLine("Ответ на пятое задание : ");
            Console.Write("{ ");
            for (i = 0; i < r; i++)
            {
                if (i == 3 || i == 5 || i == 6 || i == 7)
                    Console.Write("(");
                Boolean(a, i);
                if (i == 3 || i == 5 || i == 6 || i == 7)
                    Console.Write(")");
                Console.Write(",");
            }
            Console.Write("\b}");
            Console.ReadLine();
        }
    }
}
