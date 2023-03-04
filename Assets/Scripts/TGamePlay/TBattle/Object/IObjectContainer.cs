namespace TGamePlay.TBattle
{
    public interface IObjectContainer
    {
        void AddObject(Object prop);

        void RemoveObject(int id);

        Object FindObject(int id);
        T FindObject<T>(int id) where T : Object;

        int ObjectCount { get; }
    }
}
