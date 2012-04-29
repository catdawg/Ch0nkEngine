using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch0nkEngine.Composition
{
    public abstract class Container : Component
    {
        protected readonly Dictionary<String, Component> components = new Dictionary<String, Component>();

        public void AddComponent(Component component)
        {
            component.Container = this;
            components.Add(component.GetType().Name, component);
        }

        public T GetComponent<T>()
        {
            return (T)(Object)components[typeof(T).Name];
        }

        public void RenderComponents(GameTime time)
        {
            foreach (Component component in components.Values)
            {
                component.Render(time);
            }
        }
        public void UpdateComponents(GameTime time)
        {
            foreach (Component component in components.Values)
            {
                component.Update(time);
            }
        }
        public void LoadComponents()
        {
            foreach (Component component in components.Values)
            {
                component.Load();
            }
        }
        public void UnloadComponents()
        {
            foreach (Component component in components.Values)
            {
                component.Unload();
            }
        }
    }
}
