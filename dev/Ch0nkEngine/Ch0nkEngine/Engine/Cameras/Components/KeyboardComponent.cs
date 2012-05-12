using System.Collections.Generic;
using System.Windows.Forms;
using SlimDX.RawInput;
using System;
using SlimDX;
using Ch0nkEngine.Composition;

namespace Ch0nkEngine.Cameras.Components
{
    public class KeyboardComponent : Component
    {
        private float speed = 100; //units per second

        private readonly Keys[] usedKeys = { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.C };
        private Dictionary<Keys, KeyState> _keyStates = new Dictionary<Keys, KeyState>();

        private Camera camera;

        public override void Load()
        {
            Device.RegisterDevice(SlimDX.Multimedia.UsagePage.Generic, SlimDX.Multimedia.UsageId.Keyboard, DeviceFlags.None);
            Device.KeyboardInput += new EventHandler<KeyboardInputEventArgs>(HandleKeyboardEvent);

            camera = ((Camera)Container);
            foreach (Keys a in usedKeys)
            {
                _keyStates.Add(a, KeyState.Released);
            }
        }

        public void HandleKeyboardEvent(object sender, KeyboardInputEventArgs e)
        {
            if(_keyStates.ContainsKey(e.Key))
            {
                _keyStates[e.Key] = e.State;
            }
        }


        public override void Update(GameTime gameTime)
        {
            Vector3 deltaMovement = Vector3.Zero;

            if (_keyStates[Keys.W] == KeyState.Pressed)
                deltaMovement.Y += 1;
            if (_keyStates[Keys.S] == KeyState.Pressed)
                deltaMovement.Y -= 1;
            if (_keyStates[Keys.A] == KeyState.Pressed)
                deltaMovement.X -= 1;
            if (_keyStates[Keys.D] == KeyState.Pressed)
                deltaMovement.X += 1;

            if (deltaMovement == Vector3.Zero)
                return;


            Vector3 cameraDirection = camera.Direction;
            cameraDirection.Normalize();

            Vector3 cameraNormalDirection = Vector3.Cross(cameraDirection, camera.UpVector);
            cameraNormalDirection.Normalize();

            Vector3 offset = cameraDirection * ((speed * 1.0f/gameTime.ElapsedMiliseconds) * deltaMovement.Y);
            offset += cameraNormalDirection * ((speed * 1.0f/gameTime.ElapsedMiliseconds) * deltaMovement.X);

            camera.Target += offset;
            camera.Position += offset;
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
    }
}
