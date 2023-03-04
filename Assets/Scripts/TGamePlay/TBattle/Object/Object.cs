namespace TGamePlay.TBattle
{
    public abstract class Object
    {
        public Object(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; }
        public string Name { get; set; }

        private static int curID = 1000;
        public static int GenLocalID()
        {
            return curID++;
        }
    }
}
