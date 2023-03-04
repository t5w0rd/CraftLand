using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class PropertyContainer : IPropertyContainer
    {
        private readonly Dictionary<string, Property> properties = new();

        public PropertyContainer() { }

        public void AddProperty(string name, Property prop, bool overwrite = true)
        {
            if (!overwrite && properties.ContainsKey(name))
            {
                return;
            }
            properties.Add(name, prop);
        }

        public void RemoveProperty(string name)
        {
            _ = properties.Remove(name);
        }

        public Property FindProperty(string name)
        {
            bool ok = properties.TryGetValue(name, out Property prop);
            return ok ? prop : null;
        }

        public T FindProperty<T>(string name) where T : Property
        {
            return FindProperty(name) as T;
        }

        int IPropertyContainer.PropertyCount => properties.Count;
    }
}
