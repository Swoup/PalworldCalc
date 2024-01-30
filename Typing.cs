namespace PalworldCalculator
{
    public sealed record Typing : IEquatable<Typing>, IComparable<Typing>, IComparable
    {
        private static readonly Dictionary<PalTypes, (PalTypes[], PalTypes)> strengthAndWeaknesses = new()
    {
        {PalTypes.Dark, (new PalTypes[]{PalTypes.Normal}, PalTypes.Dragon)},
        {PalTypes.Normal, (new PalTypes[]{}, PalTypes.Dark)},
        {PalTypes.Dragon, (new PalTypes[]{PalTypes.Dark}, PalTypes.Ice)},
        {PalTypes.Ice, (new PalTypes[]{PalTypes.Dark}, PalTypes.Fire)},
        {PalTypes.Fire, (new PalTypes[]{PalTypes.Ice, PalTypes.Leaf}, PalTypes.Water)},
        {PalTypes.Water, (new PalTypes[]{PalTypes.Fire}, PalTypes.Electricity)},
        {PalTypes.Electricity, (new PalTypes[]{PalTypes.Water}, PalTypes.Earth)},
        {PalTypes.Earth, (new PalTypes[]{PalTypes.Water}, PalTypes.Leaf)},
        {PalTypes.Leaf, (new PalTypes[]{PalTypes.Earth}, PalTypes.Fire)}
    };

        private readonly PalTypes type;
        private readonly PalTypes weaknesses;
        private readonly PalTypes strengths;

        public Typing(PalTypes a, PalTypes b)
        {
            type = a | b;
            strengths = strengthAndWeaknesses[a].Item1.Aggregate(PalTypes.None, (x, y) => x | y) | b;
            weaknesses = strengthAndWeaknesses[a].Item2 | b;
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