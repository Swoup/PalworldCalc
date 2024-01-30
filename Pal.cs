namespace PalworldCalculator
{
    public record Pal(string Number, string Name, PalTypes? Type1, PalTypes? Type2, bool IsActiveAtNight, byte Kindling, byte Watering, byte Planting, byte Electric, byte Handiwork, byte Gathering, byte Lumbering, byte Mining, byte Medicine, byte Cooling, byte Transporting, byte Farming, byte Food, ushort BreedPWR, ushort HP, ushort Melee, ushort Shot, ushort Defence, ushort Price, ushort Stamina, ushort Walking, ushort Running, ushort? Mounted, ushort? Transport, double CaptureMulti, byte MaleRate);
}