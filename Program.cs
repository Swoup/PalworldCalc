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
var bestCombos = ordered.Take(1).SelectMany(x => x).ToList();
var palTeams = bestCombos.Select(x => x.Select(y => (typing : y, pal : palsByType[y].OrderByDescending(pal => pal.Attack).ThenByDescending(pal => pal.Defence).First()))).ToList();
var palByName = pals.ToDictionary(x => x.Name, y => y);
var mountedTeams = palTeams.Where(x => x.Any(y => palByName[y.pal!.Name].Mounted >= 1100)).ToList();
var mountedTeamsOrdered = mountedTeams.OrderByDescending(x => x.Max(y => palByName[y.pal!.Name].Mounted)).ToList();
var bestTeams = mountedTeamsOrdered.OrderByDescending(x => x.Average(pick => pick.pal.Attack));

Print(bestTeams.Take(100).ToList());

static int CountStrengths(IEnumerable<Typing> source) => CountBits(source, y => y.Strengths);
static int CountWeaknesses(IEnumerable<Typing> source) => CountBits(source, y => y.Weaknesses);
static int CountBits(IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => source.GetCoverage(selector).GetSetBitCount();

static void Print(List<IEnumerable<(Typing typing, Pal pal)>> bestTeams)
{
    if(bestTeams.Count == 0) 
        Console.WriteLine("No team satisfying requirements found");
        
    foreach(var bestTeam in bestTeams)
    {
        StringBuilder sb = new();
        PalTypes strengths = PalTypes.None;
        PalTypes weakness = PalTypes.None;
        foreach ((Typing typing, Pal pal) in bestTeam)
        {
            sb.Append(string.Join(", " , $"#{pal.Number} {pal.Name}, "));
            strengths |= typing.Strengths;
            weakness |= typing.Weaknesses;
        }
        sb.Append($"strong against {(strengths.GetSetBitCount() == 9 ? "all types" : strengths)} and weak against {weakness} and average attack of {bestTeam.Average(x => x.pal.Attack)} and a top mount speed of {bestTeam.Max(x => x.pal.Mounted)} \n");
        Console.WriteLine(sb);
    }
    Console.WriteLine();
}