using System;
using System.Collections.Generic;

class Program
{
    static Stack<char> verem = new Stack<char>();

    public static void PushSymbolsToStack(Stack<char> verem, string symbols)
    {
        foreach (var symbol in symbols)
        {
            verem.Push(symbol);
        }
    }

   
    public static bool CheckSymbols(string megadott)
    {
        

        foreach (char c in megadott)
        {
            
            if (c == '(' || c == '[' || c == '{')
            {
                verem.Push(c);
            }
            
            else if (c == ')' || c == ']' || c == '}')
            {
                if (verem.Count == 0) 
                {
                    return false;
                }

                char legfelso = verem.Pop();
                if (!IsMatchingPair(legfelso, c))
                {
                    return false;
                }
            }
        }


        return verem.Count == 0;
    }

    private static bool IsMatchingPair(char nyito, char zaro)
    {
        return (nyito == '(' && zaro == ')') ||
               (nyito == '[' && zaro == ']') ||
               (nyito == '{' && zaro == '}');
    }

    static void Main(string[] args)
    {
      
        string helyes = "({[()]})"; 
        string helytelen = "({[()]}";  
 
        Console.WriteLine($"Input: {helyes}");
        Console.WriteLine(CheckSymbols(helyes) ? "A zárójelek megfelelően párosítva." : "A zárójelek nincsenek megfelelően párosítva.");

        Console.WriteLine($"Input: {helytelen}");
        Console.WriteLine(CheckSymbols(helytelen) ? "A zárójelek megfelelően párosítva." : "A zárójelek nincsenek megfelelően párosítva.");

        Console.ReadKey();
    }
}