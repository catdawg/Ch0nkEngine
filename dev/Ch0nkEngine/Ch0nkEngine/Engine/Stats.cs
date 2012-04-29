using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Composition;

namespace Ch0nkEngine
{
    class Stats : Component
    {
        private float frameAccumulator;
        private int frameCount;
        public override void Load()
        {
            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Render(GameTime time)
        {
            base.Render(time);

            
            frameAccumulator += time.ElapsedMiliseconds;
            ++frameCount;
            if (frameAccumulator >= 1000000.0f)
            {
                Master.I.form.Text = "Ch0nkEngineRenderer : fps:" + (int)((frameCount / frameAccumulator) * 100000);

                frameAccumulator = 0.0f;
                frameCount = 0;
            }
             
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }
    }
}
