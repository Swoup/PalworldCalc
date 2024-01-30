namespace PalworldCalculator
{
    [Flags]
    public enum PalTypes
    {
        None = 0b_0000_0000_0000,
        Normal = 0b_0000_0000_0001,
        Dark = 0b_0000_0000_0010,
        Dragon = 0b_0000_0000_0100,
        Ice = 0b_0000_0000_1000,
        Fire = 0b_0000_0001_0000,
        Water = 0b_0000_0010_0000,
        Electricity = 0b_0000_0100_0000,
        Earth = 0b_0000_1000_0000,
        Leaf = 0b_0001_0000_0000
    }
}