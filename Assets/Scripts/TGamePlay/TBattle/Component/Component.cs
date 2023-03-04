namespace TGamePlay.TBattle
{
    public abstract class Component
    {
        private IComponentContainer container;

        public Component() { }

        public IComponentContainer Container { get => container; }

        public void AddToContainer(IComponentContainer container)
        {
            this.container = container;
            OnAdd();
        }

        public void RemoveFromContainer()
        {
            OnRemove();
            container = null;
        }

        public T GetComponent<T>() where T : Component => container.GetComponent<T>();

        protected virtual void OnAdd() { }
        protected virtual void OnRemove() { }
    }
}
