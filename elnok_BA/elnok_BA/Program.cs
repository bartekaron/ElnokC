using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace elnok_BA
{
    class President
    {
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Party { get; set; }
        public int BirthYear { get; set; }
        public int DeathYear { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "elnokok.txt"; 
            List<President> elnokok = new List<President>();

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('|');
                if (parts.Length == 6)
                {
                    elnokok.Add(new President
                    {
                        Name = parts[0],
                        StartYear = int.Parse(parts[1]),
                        EndYear = int.Parse(parts[2]),
                        Party = parts[3],
                        BirthYear = int.Parse(parts[4]),
                        DeathYear = int.Parse(parts[5])
                    });
                }
            }

            // 1
            var elsoElnok = elnokok.OrderBy(p => p.StartYear).FirstOrDefault();
            Console.WriteLine($"Az első amerikai elnök: {elsoElnok.Name}");

            // 2
            Console.WriteLine("Adj meg egy elnök nevet:");
            string nev = Console.ReadLine();
            var elnok = elnokok.FirstOrDefault(p => p.Name.Equals(nev));

            if (elnok != null)
            {
                Console.WriteLine($"Név: {elnok.Name}, Hivatalban: {elnok.StartYear}-{elnok.EndYear}, Párt: {elnok.Party}, Született: {elnok.BirthYear}, Elhunyt: {elnok.DeathYear}");
            }
            else
            {
                Console.WriteLine("Nincs ilyen elnök.");
            }

            // 3
            var elnokFuggetlensegi = elnokok.FirstOrDefault(p => p.StartYear <= 1861 && p.EndYear >= 1865);
            Console.WriteLine($"Az elnök 1861 és 1865 között: {elnokFuggetlensegi.Name}");

            // 4
            var partStatisztika = elnokok.GroupBy(p => p.Party).Select(g => new { Party = g.Key, Count = g.Count() });
            Console.WriteLine("Pártok statisztikája:");
            foreach (var stat in partStatisztika)
            {
                Console.WriteLine($"{stat.Party}: {stat.Count} elnök");
            }

            // 5
            var elsoDemokrata = elnokok.Where(p => p.Party == "Demokrata").OrderBy(p => p.StartYear).FirstOrDefault();
            Console.WriteLine($"Az első demokrata párti elnök: {elsoDemokrata.Name}");

            // 6
            var nemEgymasUtan = elnokok.GroupBy(p => p.Name)
                                                    .Where(g => g.Count() > 1)
                                                    .Select(g => g.Key)
                                                    .FirstOrDefault();
            Console.WriteLine($"Az elnök, aki két nem egymást követő ciklusban szolgált: {nemEgymasUtan}");

            // 7
            var rovidElnok = elnokok.OrderBy(p => p.EndYear - p.StartYear).FirstOrDefault();
            Console.WriteLine($"A legrövidebb ideig hivatalban lévő elnök: {rovidElnok.Name}");

            // 8
            var masodikVhElnok = elnokok.FirstOrDefault(p => p.StartYear <= 1945 && p.EndYear >= 1945);
            Console.WriteLine($"Az elnök a második világháború végén: {masodikVhElnok.Name}");

            // 9
            var elnok2001 = elnokok.FirstOrDefault(p => p.StartYear <= 2001 && p.EndYear >= 2008);
            Console.WriteLine($"Az elnök 2001 és 2008 között: {elnok2001.Name}");

            // 10
            var elnok20Szazad = elnokok.Where(p => p.BirthYear >= 1901).OrderBy(p => p.BirthYear).FirstOrDefault();
            Console.WriteLine($"Az első elnök, aki a 20. században született: {elnok20Szazad.Name}");

            Console.ReadLine();
        }
    }
}