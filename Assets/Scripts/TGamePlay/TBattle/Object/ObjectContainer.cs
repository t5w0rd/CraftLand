using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class ObjectContainer : IObjectContainer
    {
        private readonly Dictionary<int, Object> objects = new();

        public ObjectContainer() { }

        public int ObjectCount => objects.Count;

        public void AddObject(Object obj)
        {
            objects.Add(obj.ID, obj);
        }

        public Object FindObject(int id)
        {
            bool ok = objects.TryGetValue(id, out Object obj);
            return ok ? obj : null; ;
        }

        public T FindObject<T>(int id) where T : Object
        {
            return FindObject(id) as T;
        }

        public void RemoveObject(int id)
        {
            _ = objects.Remove(id);
        }
    }
}
