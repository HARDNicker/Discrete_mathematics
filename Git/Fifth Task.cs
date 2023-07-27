using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;

namespace Задание
{
    internal class Program
    {
        public static List<List<T>> ShowAllCombinations<T>(IList<T> arr, List<List<T>> list = null, List<T> current = null)
        {
            if (list == null) list = new List<List<T>>();
            if (current == null) current = new List<T>();
            if (arr.Count == 0) //если все элементы использованы, выводим на консоль получившуюся строку и возвращаемся
            {
                list.Add(current);
                return list;
            }
            for (int i = 0; i < arr.Count; i++) //в цикле для каждого элемента прибавляем его к итоговой строке, создаем новый список из оставшихся элементов, и вызываем эту же функцию рекурсивно с новыми параметрами.
            {
                List<T> lst = new List<T>(arr);
                lst.RemoveAt(i);
                var newlst = new List<T>(current);
                newlst.Add(arr[i]);
                ShowAllCombinations(lst, list, newlst);
            }
            return list;
        }

        static IEnumerable<String> CombinationsWithRepetition(IEnumerable<string> input, int length)
        {
            if (length <= 0)
                yield return "";
            else
            {
                foreach (var i in input)
                    foreach (var c in CombinationsWithRepetition(input, length - 1))
                        yield return i.ToString() + c;
            }
        }
        static IEnumerable<IEnumerable<T>> CombinationsWithoutRepetition<T>(IEnumerable<T> input, int length)
        {
            int i = 0;
            foreach (var item in input)
            {
                if (length == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in CombinationsWithoutRepetition(input.Skip(i + 1), length - 1))
                        yield return new T[] { item }.Concat(result);
                }
                ++i;
            }
        }
        static void Placement(int index,int k,int n,ref string[] Akn,ref string[] arr)
        {
            if (index >= k)
            {
                Print(arr);
            }
            else
            {
                for (int i = index; i < n; i++)
                {
                    arr[index] = Akn[i];
                    Swap(ref Akn[i], ref Akn[index]);
                    Placement(index + 1,k,n,ref Akn,ref arr);
                    Swap(ref Akn[i], ref Akn[index]);
                }
            }
        }

        private static void Placement_R(int index, int k, int n, ref string[] Akn, ref string[] arr)
        {
            if (index >= k)
            {
                Print(arr);
                return;
            }

            for (int i = 0; i < n; i++)
            {
                arr[index] = Akn[i];
                Placement_R(index + 1,k,n,ref Akn,ref arr);
            }
        }

        private static void Swap<T>(ref T v1, ref T v2)
        {
            T old = v1;
            v1 = v2;
            v2 = old;
        }

        private static void Print<T>(T[] arr)
        {
            Console.WriteLine("{" + string.Join(", ", arr) + "}");
        }

        public static long Factorial(long n)
        {
            if (n == 0)
                return 1;
            else
                return n * Factorial(n - 1);
        }

        public static long PowerNumber(int number,int power)
        {
            int solution = number;
            for(int i = 1; i < power; i++)
                solution *= number;
            return solution;
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1) Размещение без повторений(Akn); 2) Размещение с повторениями(!Akn);");
                Console.WriteLine("3) Сочетание без повторений(Ckn); 4) Сочетание с повторениями(!Ckn);");
                Console.WriteLine("5) Перестановки без повторений(Pn); 6) Бином Ньютона; 7) Выход.");
                Console.Write("\nВыберите необходимое задание от 1 до 7 : ");
                int number = int.Parse(Console.ReadLine());
                switch (number)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("Введите степень k: ");
                        int Ak = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер n: ");
                        int An = int.Parse(Console.ReadLine());
                        string[] arrayof_A = new string[An];
                        string[] freeArrayof_A= new string[Ak];

                        for (int i = 0; i < An; i++)
                        {
                            Console.Write($"Введите {i + 1} элемент: ");
                            arrayof_A[i] = Console.ReadLine();
                        }

                        Placement(0,Ak,An,ref arrayof_A, ref freeArrayof_A);
                        long Akn = Factorial(An) / Factorial(An - Ak);
                        Console.WriteLine($"Ответ = {Akn}\n");
                        break;
                   case 2:
                        Console.Clear();
                        Console.Write("Введите степень k: ");
                        int _Ak = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер n: ");
                        int _An = int.Parse(Console.ReadLine());
                        string[] arrayof_A_R = new string[_An];
                        string[] freeArrayof_A_R = new string[_Ak];

                        for (int i = 0; i < _An; i++)
                        {
                            Console.Write($"Введите {i + 1} элемент: ");
                            arrayof_A_R[i] = Console.ReadLine();
                        }

                        Placement_R(0, _Ak, _An, ref arrayof_A_R, ref freeArrayof_A_R);
                        long _Akn = PowerNumber(_An, _Ak);
                        Console.WriteLine($"Ответ = {_Akn}\n");
                        break;
                    case 3:
                        Console.Clear();
                        Console.Write("Введите степень k: ");
                        int Ck = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер n: ");
                        int Cn = int.Parse(Console.ReadLine());
                        string[] arrayof_C = new string[Cn];

                        for (int i = 0; i < Cn; i++)
                        {
                            Console.Write($"Введите {i + 1} элемент: ");
                            arrayof_C[i] = Console.ReadLine();
                        }

                        var resultofC = CombinationsWithoutRepetition(arrayof_C, Ck);
                        foreach (var perm in resultofC)
                        {
                            foreach (var c in perm)
                            {
                                Console.Write(c + " ");
                            }
                            Console.WriteLine();
                        }

                        long Ckn = Factorial(Cn) / (Factorial(Ck) * Factorial(Cn - Ck));
                        Console.WriteLine($"Ответ = {Ckn}\n");
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write("Введите степень k: ");
                        int _Ck = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер n: ");
                        int _Cn = int.Parse(Console.ReadLine());
                        string[] arrayofC_R = new string[_Cn];

                        for (int i = 0; i < _Cn; i++)
                        {
                            Console.Write($"Введите {i + 1} элемент: ");
                            arrayofC_R[i] = Console.ReadLine();
                        }

                        foreach (var c in CombinationsWithRepetition(arrayofC_R, _Ck))
                            Console.WriteLine(c);

                        long _Ckn =Factorial(_Cn+_Ck-1)/ (Factorial(_Ck) * Factorial(_Cn - 1));
                        Console.WriteLine($"Ответ = {_Ckn}\n");
                        break;
                    case 5:
                        Console.Clear();
                        Console.Write("Введите номер n: ");
                        int n = int.Parse(Console.ReadLine());
                        string[] arrayof_P= new string[n];

                        for(int jj=0; jj<n; jj++)
                        {
                            Console.Write($"Введите {jj+1} элемент: ");
                            arrayof_P[jj]=Console.ReadLine();
                        }

                        object[] arr = arrayof_P.ToArray();
                        var resultofP = ShowAllCombinations(arr);
                        foreach (List<object> lst in resultofP)
                        {
                            foreach (object obj in lst)
                            {
                                Console.Write(obj + " ");
                            }
                            Console.WriteLine();
                        }

                        long Pn = Factorial(n);
                        Console.WriteLine($"Количество перестановок = {Pn}\n");
                        break;
                    case 6:
                        Console.Clear();
                        Console.Write("Введите порядок бинома от 1 до 13 k = ");
                        int k= int.Parse(Console.ReadLine());
                        if(k>0 && k<14)
                        {
                            int[,] array = new int[14,14];
                            array[0, 0] = 1;
                            array[1,0] = 1;
                            array[1, 1] = 1;
                            for (int i = 2; i <= k; i++)
                            {
                                array[i, 0] = 1;
                                for (int j = 1; j <= i; j++)
                                {
                                    if (j == i)
                                        array[i, j] = 1;
                                    else
                                        array[i, j] = array[i - 1, j - 1] + array[i - 1, j];
                                }
                            }
                            Console.WriteLine("Коэффициенты бинома : ");
                            for (int j = 0; j <= k; j++)
                                Console.Write(array[k, j] + "     ");
                            Console.WriteLine("\nОтвет : ");
                            for (int j = 0; j <= k; j++)
                            {
                                Console.Write(array[k, j] + $"x^{k - j} ");
                                if(k-j!=0)
                                    Console.Write("+ ");
                            }
                            
                            Console.WriteLine("\n");
                        }
                        else
                            Console.WriteLine("Введено неверное число\n");
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Вы ввели неправильный номер. Попробуйте еще раз \n");
                        break;
                }
            }

        }
    }
}
