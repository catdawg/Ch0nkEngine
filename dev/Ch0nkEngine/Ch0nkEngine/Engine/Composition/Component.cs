using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch0nkEngine.Engine.Composition
{
    public abstract class Component
    {
        internal Container Container;

        public virtual void Render(GameTime time) { }
        public virtual void Update(GameTime time) { }
        public virtual void Load() { }
        public virtual void Unload() { }
    }
}
