namespace TGamePlay.TBattle
{
    public struct DamageInfo
    {
        public DamageInfo(Unit source, int value)
        {
            Source = source;
            Value = value;
        }

        public static DamageInfo invalid = new(null, 0);

        public Unit Source { get; }
        public int Value { get; set; }
    }
}
