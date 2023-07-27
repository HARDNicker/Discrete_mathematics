// Составить алгоритм и написать программу генерации таблицы истинности. 

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
    public static List<Token> Calculate(string expression)
    {
        MatchCollection collection = Regex.Matches(expression, @"\(|\)|[A-Z]|7|v|\^|->|<->");

        Regex variables = new Regex(@"[A-Z]");
        Regex operations = new Regex(@"7|v|\^|->|<->");
        Regex brackets = new Regex(@"\(|\)");
        string[] priority = { "7", "^", "v", "->", "<->" };

        Stack<string> stack = new();
        List<Token> list = new();
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
            variables[variables.ElementAt(i - 1).Key] = binary[i] == '0' ? false : true;
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
        }
        Console.Write("└");
        foreach (var value in variables)
            Console.Write("─┴");
        Console.WriteLine("─┘");
    }
}


class Program
{
    static void Main()
    {
        Console.WriteLine("Справочник по вводу выражения : \n 7 - отрицание\t ^ - конъюнкция\t " +
                    "v - дизъюнкция\t -> - импликация\t <-> - эквивалентность");
        Console.Write("Введите логическое выражение : ");
        string input = Console.ReadLine();
        List<Token> rpn = Arithmetic.Calculate(input);
        Dictionary<string, bool> variables = Arithmetic.GetVariables(rpn);
        Arithmetic.PrintTable(rpn, variables);
        Console.Read();
    }
}