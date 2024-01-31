namespace PalworldCalculator
{
    public sealed record Typing : IEquatable<Typing>, IComparable<Typing>, IComparable
    {
        private static readonly Dictionary<PalTypes, PalTypes> strong = new()
        {
            {PalTypes.Dark, PalTypes.Normal},
            {PalTypes.Normal, PalTypes.None},
            {PalTypes.Dragon, PalTypes.Dark},
            {PalTypes.Ice, PalTypes.Dragon},
            {PalTypes.Fire, PalTypes.Ice | PalTypes.Leaf},
            {PalTypes.Water, PalTypes.Fire},
            {PalTypes.Electricity, PalTypes.Water},
            {PalTypes.Earth, PalTypes.Electricity},
            {PalTypes.Leaf, PalTypes.Earth}
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
            if (b == PalTypes.None)
            {
                weaknesses = weak[a];
                strengths = strong[a];
                type = a;
                return;
            }
            strengths = strong[a] | strong[b];
            weaknesses = weak[a] & (~(strong[b] | b)) | weak[b] & (~(strong[a] | a));
            type = a | b;
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