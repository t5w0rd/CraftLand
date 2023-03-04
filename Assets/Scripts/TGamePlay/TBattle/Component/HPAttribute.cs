namespace TGamePlay.TBattle
{
    public class HPAttribute : Component
    {
        private CoefMaxValue hp;

        public HPAttribute(int maxHP)
        {
            hp = new(maxHP);
        }

        public int MaxHPRaw
        {
            get => hp.MaxValueRaw;
            set => hp.MaxValueRaw = value;
        }

        public int MaxHP => hp.MaxValue;

        public int HP
        {
            get => hp.Value;
            set => hp.Value = value;
        }

        public Coefficient MaxHPCoef { get => hp.MaxValueCoef; }

        public void ChangeMaxHPCoefA(int delta) => hp.ChangeMaxValueCoefA(delta);

        public void ChangeMaxHPCoefB(int delta) => hp.ChangeMaxValueCoefB(delta);
    }
}
