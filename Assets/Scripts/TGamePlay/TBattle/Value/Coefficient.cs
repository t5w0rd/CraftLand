namespace TGamePlay.TBattle
{
    public struct Coefficient
    {
        private int a;
        private int b;

        public Coefficient(int a, int b = 0)
        {
            this.a = a;
            this.b = b;
        }

        public int A => a;

        public int B => b;

        public int Value(int x)
        {
            return ((a + 100) * x / 100) + b;
        }

        public float Value(float x)
        {
            return ((a + 100) * x / 100) + b;
        }

        public int ChangeA(int delta)
        {
            a += delta;
            return a;
        }

        public int ChangeB(int delta)
        {
            b += delta;
            return b;
        }
    }
}
