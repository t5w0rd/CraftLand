namespace TGamePlay.TBattle
{
    public struct CoefValue
    {
        private Coefficient coef;

        public CoefValue(int value)
        {
            ValueRaw = value;
            coef = new Coefficient();
        }

        public int ValueRaw { get; set; }

        public int Value => coef.Value(ValueRaw);

        public Coefficient Coef { get => coef; }

        public void ChangeCoefA(int delta) => coef.ChangeA(delta);

        public void ChangeCoefB(int delta) => coef.ChangeB(delta);
    }
}
