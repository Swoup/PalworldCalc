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


var typesByPalNames = pals.ToLookup(x => new Typing(x.Type1.GetValueOrDefault(), x.Type2.GetValueOrDefault()), x => x.Name);
var actualPalTypes = typesByPalNames.Select(x => x.Key);
var theoricalTeams = actualPalTypes.GetKCombs(5).ToList();

var bestTeams = theoricalTeams
    .OrderByDescending(CountStrengths)
    .ThenBy(CountWeaknesses)
    .ToList();

var exampleBestTeam = bestTeams.First();

var veryBestTeams = bestTeams
    .Where(x => CountStrengths(x) == CountStrengths(exampleBestTeam) && CountWeaknesses(x) == CountWeaknesses(exampleBestTeam))
    .ToList();

foreach(var bestTeam in veryBestTeams)
{
    Console.WriteLine();
    StringBuilder sb = new();
    StringBuilder candidates = new();
    foreach (var type in bestTeam) 
    {
        
        sb.Append('[').Append(type.Type).Append(']');
        candidates.Append($"Candidate for type {type} are {string.Join("," , typesByPalNames[type])} \n");
    }
    Console.WriteLine($"{sb} with offensive coverage {GetCoverage(bestTeam, x => x.Strengths)} and defensive coverage {GetCoverage(bestTeam, x => x.Weaknesses)}");
    Console.WriteLine($"{candidates}");
}


static int CountStrengths(IEnumerable<Typing> source) => CountBits(source, y => y.Strengths);
static int CountWeaknesses(IEnumerable<Typing> source) => CountBits(source, y => y.Weaknesses);
static int CountBits(IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => GetCoverage(source, selector).GetSetBitCount();
static PalTypes GetCoverage(IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => source.Select(selector).Aggregate((a, b) => a | b);
