using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class ComponentContainer : IComponentContainer
    {
        private readonly List<Component> components = new();

        public T AddComponent<T>(T c) where T : Component
        {
            components.Add(c);
            c.AddToContainer(this);
            return c;
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < components.Count; i++)
            {
                Component c = components[i];
                if (c is T)
                {
                    return c as T;
                }
            }
            return null;
        }

        public void RemoveComponent(Component c)
        {
            c.RemoveFromContainer();
            components.Remove(c);
        }
    }
}
