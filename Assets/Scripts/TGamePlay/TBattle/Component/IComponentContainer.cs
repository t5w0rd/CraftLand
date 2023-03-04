namespace TGamePlay.TBattle
{
    public interface IComponentContainer
    {
        T AddComponent<T>(T c) where T : Component;

        T GetComponent<T>() where T : Component;

        void RemoveComponent(Component c);
    }
}
