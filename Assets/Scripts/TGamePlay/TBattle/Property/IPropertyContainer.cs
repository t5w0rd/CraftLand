namespace TGamePlay.TBattle
{
    public interface IPropertyContainer
    {
        void AddProperty(string name, Property prop, bool overwrite = true);

        void RemoveProperty(string name);

        Property FindProperty(string name);
        T FindProperty<T>(string name) where T : Property;

        int PropertyCount { get; }
    }
}
