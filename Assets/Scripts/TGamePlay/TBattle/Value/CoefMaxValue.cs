namespace TGamePlay.TBattle
{
    public struct CoefMaxValue
    {
        private int value;
        private CoefValue maxValue;

        public CoefMaxValue(int maxValue)
        {
            if (maxValue < 1)
            {
                maxValue = 1;
            }
            this.maxValue = new(maxValue);
            value = maxValue;
        }

        public int MaxValue
        {
            get
            {
                int m = maxValue.Value;
                return m >= 1 ? m : 1;
            }

        }

        public int MaxValueRaw
        {
            get => maxValue.ValueRaw;
            set
            {
                int oldMaxValue = MaxValue;
                maxValue.ValueRaw = value >= 1 ? value : 1;
                Value = this.value * MaxValue / oldMaxValue;
            }
        }

        public int Value
        {
            get => value;
            set
            {
                int m = MaxValue;
                if (value < 0)
                {
                    this.value = 0;
                }
                else if (value > m)
                {
                    this.value = m;
                }
                else
                {
                    this.value = value;
                }
            }
        }

        public Coefficient MaxValueCoef { get => maxValue.Coef; }

        public void ChangeMaxValueCoefA(int delta) => maxValue.Coef.ChangeA(delta);

        public void ChangeMaxValueCoefB(int delta) => maxValue.Coef.ChangeB(delta);
    }
}
