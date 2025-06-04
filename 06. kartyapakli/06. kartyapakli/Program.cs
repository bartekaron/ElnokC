using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
 
        List<int> pakli = KevertPakli(52); 
        Stack<int> pakliVerem = new Stack<int>(pakli);

 
        int jatekos1 = 0;
        int jatekos2 = 0;


       
        while (pakliVerem.Count > 1) 
        {
            int jatekos1Kartya = pakliVerem.Pop();
            int jatekos2Kartya = pakliVerem.Pop();

            Console.WriteLine($"Játékos 1 húzta: {jatekos1Kartya}, Játékos 2 húzta: {jatekos2Kartya}");

            if (jatekos1Kartya > jatekos2Kartya)
            {
                jatekos1++;
                Console.WriteLine("Játékos 1 nyert ebben a körben!");
            }
            else if (jatekos2Kartya > jatekos1Kartya)
            {
                jatekos2++;
                Console.WriteLine("Játékos 2 nyert ebben a körben!");
            }
            else
            {
                Console.WriteLine("Döntetlen ebben a körben!");
            }
        }

     
        Console.WriteLine("\nJáték vége!");
        Console.WriteLine($"Játékos 1 pontszáma: {jatekos1}");
        Console.WriteLine($"Játékos 2 pontszáma: {jatekos2}");

        if (jatekos1 > jatekos2)
        {
            Console.WriteLine("Játékos 1 nyert!");
        }
        else if (jatekos2 > jatekos1)
        {
            Console.WriteLine("Játékos 2 nyert!");
        }
        else
        {
            Console.WriteLine("Döntetlen!");
        }

        Console.ReadKey();
    }

 
    static List<int> KevertPakli(int osszesKartya)
    {
        Random random = new Random();
        List<int> kartyak = new List<int>();
        for (int i = 0; i < osszesKartya; i++)
        {
            kartyak.Add((i % 13) + 1); 
        }
        
        
        for (int i = kartyak.Count - 1; 0 < i; i--)
        {
            int j = random.Next(i + 1);
            int g = kartyak[i];
            kartyak[i] = kartyak[j];
            kartyak[j] = g;
        }

        return kartyak;
    }
}
