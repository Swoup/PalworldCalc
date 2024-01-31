namespace PalworldCalculator
{
    public sealed record Typing : IEquatable<Typing>, IComparable<Typing>, IComparable
    {
        private static readonly Dictionary<PalTypes, PalTypes[]> strong = new()
        {
            {PalTypes.Dark, new PalTypes[]{PalTypes.Normal}},
            {PalTypes.Normal, new PalTypes[]{}},
            {PalTypes.Dragon, new PalTypes[]{PalTypes.Dark}},
            {PalTypes.Ice, new PalTypes[]{PalTypes.Dark}},
            {PalTypes.Fire, new PalTypes[]{PalTypes.Ice, PalTypes.Leaf}},
            {PalTypes.Water, new PalTypes[]{PalTypes.Fire}},
            {PalTypes.Electricity, new PalTypes[]{PalTypes.Water}},
            {PalTypes.Earth, new PalTypes[]{PalTypes.Water}},
            {PalTypes.Leaf, new PalTypes[]{PalTypes.Earth}}
        };

        private static readonly Dictionary<PalTypes, PalTypes> weak = new()
        {
            {PalTypes.Normal, PalTypes.Dark},
            {PalTypes.Dark, PalTypes.Dragon},
            {PalTypes.Dragon, PalTypes.Ice},
            {PalTypes.Ice, PalTypes.Fire},
            {PalTypes.Fire, PalTypes.Water},
            {PalTypes.Water, PalTypes.Electricity},
            {PalTypes.Electricity, PalTypes.Earth},
            {PalTypes.Earth, PalTypes.Leaf},
            {PalTypes.Leaf, PalTypes.Fire}
        };

        private readonly PalTypes type;
        private readonly PalTypes weaknesses;
        private readonly PalTypes strengths;

        public Typing(PalTypes a, PalTypes b)
        {
            type = a | b;
            strengths = strong[a].Aggregate(PalTypes.None, (x, y) => x | y) | b;
            if (b == PalTypes.None)
                weaknesses = weak[a];
            else
                weaknesses = weak[a] ^ Array.Find(strong[b], x => x == weak[a]) | weak[b] ^ Array.Find(strong[a], x => x == weak[a]);

        }

        public PalTypes Type => type;
        public PalTypes Weaknesses => weaknesses;
        public PalTypes Strengths => strengths;
        public int CompareTo(Typing? other) => Type.CompareTo(other?.Type);

        public int CompareTo(object? obj) => obj is Typing ? CompareTo((Typing?)obj) : -1;

        public bool Equals(Typing? other)
        {
            if (other is null) return false;
            return Type == other.Type;
        }
        public override int GetHashCode() => Type.GetHashCode();
        public override string ToString() => Type.ToString();
    }
}