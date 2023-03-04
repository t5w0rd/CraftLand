namespace TGamePlay.TBattle
{
    public struct CoefFloatValue
    {
        private Coefficient coef;

        public CoefFloatValue(float value)
        {
            ValueRaw = value;
            coef = new Coefficient();
        }

        public float ValueRaw { get; set; }

        public float Value => coef.Value(ValueRaw);

        public Coefficient Coef { get => coef; }

        public void ChangeCoefA(int delta) => coef.ChangeA(delta);

        public void ChangeCoefB(int delta) => coef.ChangeB(delta);
    }
}
