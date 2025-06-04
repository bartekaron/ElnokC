using System;
using System.Collections.Generic;

class Program
{
   
    public static void PushElements(Stack<int> verem, params int[] elements)
    {
        foreach (var element in elements)
        {
            verem.Push(element);
        }
    }

    
    public static void PopAndPrintTop(Stack<int> verem)
    {
        if (verem.Count > 0)
        {
            int legfelso = verem.Pop();
            Console.WriteLine($"Legfelső elem leszedve: {legfelso}");
        }
        else
        {
            Console.WriteLine("A verem üres, nem lehet leszedni a legfelsőt.");
        }
    }

    
    public static void ClearStack(Stack<int> verem)
    {
        verem.Clear();
        Console.WriteLine("A verem kiürítve.");
    }

    
    public static void PrintStack(Stack<int> verem)
    {
        if (verem.Count > 0)
        {
            Console.WriteLine("Verem elemei:");
            foreach (var element in verem)
            {
                Console.WriteLine(element);
            }
        }
        else
        {
            Console.WriteLine("A verem üres.");
        }
    }

    
    public static bool IsStackEmpty(Stack<int> verem)
    {
        return verem.Count == 0;
    }

    static void Main(string[] args)
    {
       
        Stack<int> verem = new Stack<int>();


        PushElements(verem, 5, 10, 15, 20);
        PopAndPrintTop(verem);
        PushElements(verem, 25, 30);
        PrintStack(verem);
        ClearStack(verem);
        PrintStack(verem);

        
        bool üres = IsStackEmpty(verem);
        Console.WriteLine($"Üres a verem? {üres}");

        Console.ReadKey();
    }
}