namespace TGamePlay.TBattle
{
    public abstract class Property
    {
        protected Property(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}

