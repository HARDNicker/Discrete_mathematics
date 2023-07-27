using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
enum TokenType
{
    Variable,
    Operation
}

struct Token
{
    private string value;
    private TokenType type;

    public string Value { get { return value; } }
    public TokenType Type { get { return type; } }

    public Token(string value, TokenType type)
    {
        this.value = value;
        this.type = type;
    }
}

static class Arithmetic
{
    static List<int> listtableVariables = new List<int>();
    static List<int> solutionF = new List<int>();
    public static List<Token> Calculate(string expression)
    {
        MatchCollection collection = Regex.Matches(expression, @"\(|\)|[A-Z]|7|v|\^|->|<->");

        Regex variables = new Regex(@"[A-Z]");
        Regex operations = new Regex(@"7|v|\^|->|<->");
        Regex brackets = new Regex(@"\(|\)");
        string[] priority = { "7", "^", "v", "->", "<->" };

        Stack<string> stack = new Stack<string>();
        List<Token> list = new List<Token>();
        foreach (Match match in collection)
        {
            Match temp = variables.Match(match.Value);
            if (temp.Success) { list.Add(new Token(temp.Value, TokenType.Variable)); continue; }
            temp = brackets.Match(match.Value);
            if (temp.Success)
            {
                if (temp.Value == "(") { stack.Push(temp.Value); continue; }
                string operation = stack.Pop();
                while (operation != "(")
                {
                    list.Add(new Token(operation, TokenType.Operation));
                    operation = stack.Pop();
                }
                continue;
            }
            temp = operations.Match(match.Value);
            if (temp.Success)
            {
                if (stack.Count != 0)
                    while (Array.IndexOf(priority, temp.Value) > Array.IndexOf(priority, stack.Peek()))
                    {
                        if (stack.Peek() == "(") break;
                        list.Add(new Token(stack.Pop(), TokenType.Operation));
                    }
                stack.Push(temp.Value);
            }
        }
        while (stack.Count != 0)
            list.Add(new Token(stack.Pop(), TokenType.Operation));

        return list;
    }


    public static bool Calculate(List<Token> rpn, Dictionary<string, bool> variables)
    {
        Stack<bool> result = new Stack<bool>();
        foreach (Token token in rpn)
        {
            if (token.Type == TokenType.Variable) result.Push(variables[token.Value]);
            if (token.Type == TokenType.Operation)
                switch (token.Value)
                {
                    case "7": result.Push(!result.Pop()); break;
                    case "^": result.Push(result.Pop() & result.Pop()); break;
                    case "v": result.Push(result.Pop() | result.Pop()); break;
                    case "->": result.Push(result.Pop() | !result.Pop()); break;
                    case "<->": result.Push(!(result.Pop() ^ result.Pop())); break;
                }
        }
        return result.Pop();
    }


    public static Dictionary<string, bool> GetVariables(List<Token> rpn)
    {
        string[] variables = rpn.Where(x => x.Type == TokenType.Variable).Distinct().Select(x => x.Value).Cast<string>().ToArray();
        Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
        foreach (string variable in variables)
            dictionary[variable] = false;
        return dictionary;
    }


    public static void GetVariables(int value, Dictionary<string, bool> variables)
    {
        string binary = Convert.ToString(value, 2);
        for (int i = 1; i < binary.Length; i++)
        {
            variables[variables.ElementAt(i - 1).Key] = binary[i] == '0' ? false : true;
            listtableVariables.Add(binary[i] == '0' ? 0 : 1);
        }
    }
    public static List<int> ReturnLists(ref List<int> _listtableVariables, ref List<int> _solutionF, bool option)
    {
        if (option == true)
            return _listtableVariables = listtableVariables;
        else
            return _solutionF = solutionF;
    }
    public static void PrintTable(List<Token> rpn, Dictionary<string, bool> variables)
    {
        int count = (int)Math.Pow(2, variables.Count);
        Console.Write("┌");
        foreach (var value in variables)
            Console.Write("─┬");
        Console.WriteLine("─┐");
        foreach (var value in variables)
            Console.Write("│{0}", value.Key);
        Console.WriteLine("│f│");
        Console.Write("├");
        foreach (var value in variables)
            Console.Write("─┼");
        Console.WriteLine("─┤");
        for (int i = 0; i < count; i++)
        {
            GetVariables(i + count, variables);
            foreach (var value in variables)
                Console.Write(value.Value ? "│1" : "│0");
            Console.WriteLine("│{0}│", Calculate(rpn, variables) ? "1" : "0");
            solutionF.Add(Calculate(rpn, variables) ? 1 : 0);
        }
        Console.Write("└");
        foreach (var value in variables)
            Console.Write("─┴");
        Console.WriteLine("─┘");
    }
}

namespace Задание_1
{
    internal class Program
    {

        static void Solution(ref string[] array_exp, ref string[] order, ref int count_Order, string sign, ref int counter)
        {
            for (int i = counter + 1; i < array_exp.Length; i++)
            {
                if (array_exp[i] == sign && array_exp[i - 1] != ")")
                {
                    Console.Write($"{array_exp[i - 1]}{array_exp[i]}{array_exp[i + 1]}");
                    order[count_Order] = array_exp[i - 1];
                    order[count_Order + 1] = array_exp[i];
                    order[count_Order + 2] = array_exp[i + 1];
                    count_Order += 3;
                    if (array_exp[i + 1] == "!")
                    {
                        Console.Write($"{array_exp[i + 2]}");
                        order[count_Order] = array_exp[i + 2];
                        order[count_Order + 1] = ".";
                        count_Order += 2;
                        Console.WriteLine("");
                        break;
                    }
                    order[count_Order] = ".";
                    count_Order++;
                    Console.WriteLine("");
                }
                if (array_exp[i - 1] == ")" && array_exp[i] == sign)
                {
                    int temp = 0;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (array_exp[j] == "(")
                            temp = j;
                    }
                    for (int c = temp; c < i; c++)
                    {
                        Console.Write(array_exp[c]);
                        order[count_Order] = array_exp[c];
                        count_Order++;
                    }
                    if (array_exp[i + 1] == "!")
                    {
                        Console.Write(array_exp[i] + array_exp[i + 1] + array_exp[i + 2]);
                        order[count_Order] = array_exp[i];
                        order[count_Order + 1] = array_exp[i + 1];
                        order[count_Order + 2] = array_exp[i + 2];
                        order[count_Order + 3] = ".";
                        count_Order += 4;
                    }
                    else
                    {
                        Console.Write(array_exp[i] + array_exp[i + 1]);
                        order[count_Order] = array_exp[i];
                        order[count_Order + 1] = array_exp[i + 1];
                        order[count_Order + 2] = ".";
                        count_Order += 3;
                        Console.WriteLine("");
                    }
                }
            }
        }
        static void Solution1(ref string[] array_exp, ref string[] order, ref int count_Order, string sign, ref int i)
        {
            for (int j = i; j < array_exp.Length; j++)
            {
                if (array_exp[j] == ")")
                    break;
                if (array_exp[j] == sign)
                {
                    if (j > 2 && array_exp[j - 2] == "!")
                    {
                        Console.Write($"{array_exp[j - 2]}");
                        order[count_Order] = array_exp[j - 2];
                        count_Order++;
                    }
                    Console.Write($"{array_exp[j - 1]}{array_exp[j]}{array_exp[j + 1]}");
                    order[count_Order] = array_exp[j - 1];
                    order[count_Order + 1] = array_exp[j];
                    order[count_Order + 2] = array_exp[j + 1];
                    order[count_Order + 3] = ".";
                    count_Order += 4;
                    if (array_exp[j + 1] == "!")
                    {
                        Console.Write($"{array_exp[j + 2]}");
                        order[count_Order - 1] = array_exp[j + 2];
                        order[count_Order] = array_exp[j + 3];
                        order[count_Order + 1] = ".";
                        count_Order += 2;
                    }
                    Console.WriteLine("");
                }
            }
        }
        static bool Check(ref string[] order, int count_O, ref int count_Order, ref string[] array_exp, ref bool flag)
        {
            int tempCount = 0;
            for (int i = 0; i < order.Length; i++)
                if (order[i] == ".")
                    tempCount++;
            if (tempCount == count_O - 1)
            {
                for (int i = 0; i < array_exp.Length; i++, count_Order++)
                {
                    order[count_Order] = array_exp[i];
                    Console.Write(order[count_Order]);
                }
                order[count_Order] = ".";
                flag = true;
                return true;
            }
            if (tempCount == count_O)
            {
                flag = true;
                return true;
            }
            else
                return false;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество логических переменных: ");
            int count_M = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите колличество логических операций: ");
            int count_O = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите выражение используя следующие логические операции: 1) ! (отрицание); 2) ^ (конъюнкция); 3) v (дизъюнкция); 4) -> (импликация); 5) <-> (эквиваленция)");
            Console.WriteLine("Вводите выражение использую пробел через каждый символ ");
            string expression = Console.ReadLine();
            string[] operation = { " " };
            string[] array_exp = expression.Split(operation, StringSplitOptions.None);
            string[] order = new string[array_exp.Length * 3];
            int count_Order = 0;
            bool flag = false;
            Console.WriteLine("Получилось выражение : ");
            for (int j = 0; j < array_exp.Length; j++)
                Console.Write(array_exp[j]);
            Console.WriteLine("\nРазберем выражение по действиям:");
            for (int i = 0; i < array_exp.Length; i++)
            {
                if (array_exp[i] == "(")
                {
                    for (int j = i; j < array_exp.Length; j++)
                    {
                        if (array_exp[j] == ")")
                            break;
                        if (array_exp[j] == "!")
                        {
                            Console.Write($"{array_exp[j]}{array_exp[j + 1]}\n");
                            order[count_Order] = array_exp[j];
                            order[count_Order + 1] = array_exp[j + 1];
                            order[count_Order + 2] = ".";
                            count_Order += 3;
                        }
                    }
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution1(ref array_exp, ref order, ref count_Order, "^", ref i);
                    else break;
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution1(ref array_exp, ref order, ref count_Order, "v", ref i);
                    else break;
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution1(ref array_exp, ref order, ref count_Order, "->", ref i);
                    else break;
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution1(ref array_exp, ref order, ref count_Order, "<->", ref i);
                    else break;
                }
            }
            if (flag == false)
            {
                for (int i = 0; i < array_exp.Length; i++)
                {
                    if (array_exp[i] == "(")
                        while (array_exp[i] != ")")
                            i++;
                    for (int j = i; j < array_exp.Length; j++)
                    {
                        if (array_exp[j] == "!")
                        {
                            Console.Write($"{array_exp[j]}{array_exp[j + 1]}\n");
                            order[count_Order] = array_exp[j];
                            order[count_Order + 1] = array_exp[j + 1];
                            order[count_Order + 2] = ".";
                            count_Order += 3;
                        }
                    }
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution(ref array_exp, ref order, ref count_Order, "^", ref i);
                    else break;
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution(ref array_exp, ref order, ref count_Order, "v", ref i);
                    else break;
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution(ref array_exp, ref order, ref count_Order, "->", ref i);
                    else break;
                    if (!Check(ref order, count_O, ref count_Order, ref array_exp, ref flag))
                        Solution(ref array_exp, ref order, ref count_Order, "<->", ref i);
                    else break;
                }
            }

            Console.WriteLine("\nЕго таблица истинности : ");
            string input = String.Concat(array_exp);
            input = input.Replace('!', '7');
            List<Token> rpn = Arithmetic.Calculate(input);
            Dictionary<string, bool> variables = Arithmetic.GetVariables(rpn);
            Arithmetic.PrintTable(rpn, variables);
            List<int> listtableVariables = new List<int>();
            List<int> solutionF = new List<int>();
            listtableVariables = Arithmetic.ReturnLists(ref listtableVariables, ref solutionF, true);
            solutionF = Arithmetic.ReturnLists(ref listtableVariables, ref solutionF, false);

            string[] names = new string[count_M];
            int[] valuesT = new int[listtableVariables.Count];
            int[] valuesF = new int[solutionF.Count];
            variables.Keys.CopyTo(names, 0);
            listtableVariables.CopyTo(valuesT);
            solutionF.CopyTo(valuesF);

            string SDNF = "";
            string SKNF = "";
            int counter = 0;
            for (int i = 0; i < valuesF.Length; i++)
            {
                
                if (valuesF[i] == 1)
                {
                    SDNF += "(";
                    for (int j = 0; j < count_M; j++)
                    {
                        if (valuesT[j + counter] == 1) 
                        {
                            SDNF += names[j] + "^";
                        }
                        else
                        {
                            SDNF += "!" + names[j] + "^";
                        }
                    }
                    counter += count_M;
                    SDNF = SDNF.Remove(SDNF.Length - 1);
                    SDNF += ")v";
                }
                else
                {
                    SKNF += "(";
                    for (int j = 0; j < count_M; j++)
                    {
                        if (valuesT[j + counter] == 0) 
                        {
                            SKNF += names[j] + "v";
                        }
                        else
                        {
                            SKNF += "!" + names[j] + "v";
                        }
                    }
                    counter += count_M;
                    SKNF = SKNF.Remove(SKNF.Length - 1);
                    SKNF += ")^";
                }
            }
            SDNF = SDNF.Remove(SDNF.Length - 1);
            SKNF = SKNF.Remove(SKNF.Length - 1);

            Console.WriteLine($"Получившаяся СДНФ {SDNF}");
            Console.WriteLine($"Получившаяся СКНФ {SKNF}");
            Console.Read();
            Console.ReadLine();
        }
    }
}
