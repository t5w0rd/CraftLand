namespace TGamePlay.TBattle
{
    public class RValueProperty<T> : Property
    {
        public RValueProperty(string name, T v) : base(name)
        {
            Value = v;
        }

        public T Value { get; set; }
    }

    public class IntegerProperty : RValueProperty<int>
    {
        public IntegerProperty(string name, int v) : base(name, v) { }
    }

    public class FloatProperty : RValueProperty<string>
    {
        public FloatProperty(string name, string v) : base(name, v) { }
    }

    public class StringProperty : RValueProperty<string>
    {
        public StringProperty(string name, string v) : base(name, v) { }
    }

    public class CoefficientProperty : RValueProperty<Coefficient>
    {
        public CoefficientProperty(string name, Coefficient v) : base(name, v) { }
    }
}
