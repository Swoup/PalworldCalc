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

var theoricalCombosByStrengthAndWeaknesses = theoricalCombos.ToLookup(x => new {strength = CountStrengths(x), weakness = CountWeaknesses(x)});
var ordered = theoricalCombosByStrengthAndWeaknesses.OrderByDescending(x => x.Key.strength).ThenBy(x => x.Key.weakness);
var bestCombos = ordered.First();
var palTeams = bestCombos.Select(x => x.Select(y => (typing : y, pal : palsByType[y].OrderByDescending(pal => (pal.Melee + pal.Shot) / 2).ThenByDescending(pal => pal.Defence).First()))).ToList();
var palByName = pals.ToDictionary(x => x.Name, y => y);
var mountedTeams = palTeams.Where(x => x.Any(y => palByName[y.pal!.Name].Mounted != null)).ToList();
var mountedTeamsOrdered = mountedTeams.OrderByDescending(x => x.Max(y => palByName[y.pal!.Name].Mounted)).ToList();
var bestTeams = mountedTeamsOrdered.OrderByDescending(x => x.Average(pick => (pick.pal.Melee + pick.pal.Shot) / 2)).ToList();

Print(bestTeams, palByName);

static int CountStrengths(IEnumerable<Typing> source) => CountBits(source, y => y.Strengths);
static int CountWeaknesses(IEnumerable<Typing> source) => CountBits(source, y => y.Weaknesses);
static int CountBits(IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => source.GetCoverage(selector).GetSetBitCount();

static void Print(List<IEnumerable<(Typing typing, Pal pal)>> bestTeams, Dictionary<string, Pal>  palByName)
{
    foreach(var bestTeam in bestTeams.Take(20))
    {
        StringBuilder sb = new();
        foreach (var pal in bestTeam.Select(x => x.pal))
            sb.Append(string.Join(", " , $"#{pal.Number} {pal.Name}, "));
        Console.WriteLine(sb);
    }

    foreach(var bestTeam in bestTeams.Take(10))
    {
        Console.WriteLine();
        StringBuilder sb = new();
        StringBuilder candidates = new();
        foreach ((Typing typing, Pal? pal) pick in bestTeam) 
        {
            sb.Append("[").Append(pick.typing).Append("]").Append(" and ");
            candidates.Append($"Pal picked for type {pick.typing} is {string.Join(", " , $"#{palByName[pick.pal!.Name].Number} {pick.pal.Name}")} \n");
        }
        sb.Remove(sb.Length -3, 3);
        Console.WriteLine($"{sb} with offensive coverage {bestTeam.Select(x => x.typing).GetCoverage(x => x.Strengths)} and defensive weaknesses {bestTeam.Select(x => x.typing).GetCoverage(x => x.Weaknesses)}");
        Console.WriteLine($"{candidates}");
    }
    Console.WriteLine();
}