namespace PalworldCalculator
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> GetKCombsWithRept<T>(this IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombsWithRept(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(this IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), 
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }


        public static int GetSetBitCount(this PalTypes lValue)
        {
                int iCount = 0;
                while (lValue != 0)
                {
                    lValue &= (lValue - 1);
                    iCount++;
                }
                return iCount;
         }

         public static PalTypes GetCoverage(this IEnumerable<Typing> source, Func<Typing, PalTypes> selector) => source.Select(selector).Aggregate((a, b) => a | b);
    }
}