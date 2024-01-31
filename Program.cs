using System.Globalization;
using System.Text;
using CsvHelper;
using PalworldCalculator;

List<Pal> pals;
using (var reader = new StreamReader("Pals.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    pals = csv.GetRecords<Pal>().ToList();
}


var palsByType= pals.ToLookup(x => new Typing(x.Type1.GetValueOrDefault(), x.Type2.GetValueOrDefault()), x => x);
var actualPalTypes = palsByType.Select(x => x.Key);
var theoricalCombos = actualPalTypes.GetKCombs(5).ToList();

var bestTypesCombo = theoricalCombos
    .OrderByDescending(CountStrengths)
    .ThenBy(CountWeaknesses)
    .ToList();

var exampleBestCombo = bestTypesCombo.First();
var veryBestTypesCombo = bestTypesCombo
    .Where(x => CountStrengths(x) == CountStrengths(exampleBestCombo) && CountWeaknesses(x) == CountWeaknesses(exampleBestCombo))
    .ToLookup(x => x.OrderBy(y => y.Type))
    .Select(x => x.Key)
    .ToList();

var palByName = pals.ToDictionary(x => x.Name, y => y);

var palTeams = veryBestTypesCombo.Select(x => x.Select(y => (typing : y, pal : palsByType[y].OrderByDescending(pal => pal.Melee > pal.Shot ? pal.Melee : pal.Shot).ThenByDescending(pal => pal.Defence).First())));
var speedTeams = palTeams.OrderByDescending(x => x.Max(y => palByName[y.pal!.Name].Mounted)).ThenByDescending(x => x.Average(pick => pick.pal.Melee > pick.pal.Shot ? pick.pal.Melee : pick.pal.Shot)).ToList();

foreach(var bestTeam in speedTeams.Take(10))
{
    Console.WriteLine();
    StringBuilder sb = new();
    StringBuilder candidates = new();
    foreach ((Typing typing, Pal? pal) pick in bestTeam) 
    {
        sb.Append("[").Append(pick.typing).Append("]").Append(" and ");
        candidates.Append($"Pal picked for type {pick.typing} is {string.Join(", " , $"#{palByName[pick.pal.Name].Number} {pick.pal.Name}")} \n");
    }
    sb.Remove(sb.Length -3, 3);
    Console.WriteLine($"{sb} with offensive coverage {GetCoverage(bestTeam.Select(x => x.typing), x => x.Strengths)} and defensive weaknesses {GetCoverage(bestTeam.Select(x => x.typing), x => x.Weaknesses)}");
    Console.WriteLine($"{candidates}");
}


static int CountStrengths(IEnumerable<Typing> source) => CountBits(source, y => y.Strengths);
static int CountWeaknesses(IEnumerable<Typing> source) => CountBits(source, y => y.Weaknesses);
static int CountBits(IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => GetCoverage(source, selector).GetSetBitCount();
static PalTypes GetCoverage(IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => source.Select(selector).Aggregate((a, b) => a | b);
