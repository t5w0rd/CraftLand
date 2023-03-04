namespace TGamePlay.TBattle
{
    public struct CoefFloatMaxValue
    {
        private float value;
        private CoefFloatValue maxValue;

        public CoefFloatMaxValue(float maxValue)
        {
            if (maxValue < 1)
            {
                maxValue = 1;
            }
            this.maxValue = new(maxValue);
            value = maxValue;
        }

        public float MaxValue
        {
            get
            {
                float m = maxValue.Value;
                return m >= 1 ? m : 1;
            }

        }

        public float MaxValueRaw
        {
            get => maxValue.ValueRaw;
            set
            {
                float oldMaxValue = MaxValue;
                maxValue.ValueRaw = value >= 1 ? value : 1;
                Value = this.value * MaxValue / oldMaxValue;
            }
        }

        public float Value
        {
            get => value;
            set
            {
                float m = MaxValue;
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
