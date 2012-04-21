
using System;
using SlimDX.DirectInput;
using System.Windows.Forms;
using System.Drawing;
using SlimDX;
using SlimDX.RawInput;
using Ch0nkEngine.Engine.Composition;


namespace Ch0nkEngine.Cameras.Components
{
    public class MouseComponent : Component
    {
        private float mouseSensivity = 1f;

        private float mouseYInverted = 1; // mouse YAxis movement is inverted if -1

        private Vector2 mouseDelta = Vector2.Zero;

        private Camera camera;



        public override void Load()
        {
            camera = ((Camera)Container);

            SlimDX.RawInput.Device.RegisterDevice(SlimDX.Multimedia.UsagePage.Generic, SlimDX.Multimedia.UsageId.Mouse, SlimDX.RawInput.DeviceFlags.None);
            SlimDX.RawInput.Device.MouseInput += new EventHandler<MouseInputEventArgs>(MouseEventHandle);
        }

        public void MouseEventHandle(object sender, MouseInputEventArgs e)
        {
            mouseDelta.X += e.X;
            mouseDelta.Y += e.Y;
        }

        public override void Update(GameTime gameTime)
        {
            if (mouseDelta == Vector2.Zero)
                return;

            Vector3 cameraDirection = camera.Direction;
            float length = cameraDirection.Length();
            cameraDirection.Normalize();

            Vector3 cameraNormalDirection = Vector3.Cross(camera.UpVector, cameraDirection);

            Vector3 cameraTargetNormalDirectionUp = Vector3.Cross(cameraDirection, cameraNormalDirection);
            cameraTargetNormalDirectionUp.Normalize();

            Vector3 newTargetRelative = cameraDirection;

            if (mouseDelta.X != 0)
                newTargetRelative -= cameraNormalDirection * mouseDelta.X * mouseSensivity * 1.0f / gameTime.ElapsedMiliseconds;

            if (mouseDelta.Y != 0)
                newTargetRelative -= 
                    cameraTargetNormalDirectionUp * mouseDelta.Y * 
                    mouseSensivity * mouseYInverted * 1.0f/gameTime.ElapsedMiliseconds;

            newTargetRelative = Vector3.Multiply(newTargetRelative, length);
            camera.Target = camera.Position + newTargetRelative;

            Cursor.Position = Master.I.form.PointToScreen(new Point(Master.I.form.ClientSize.Width / 2, Master.I.form.ClientSize.Height / 2));
            Cursor.Hide();
            mouseDelta = Vector2.Zero;
        }


        public float MouseSensivity
        {
            get { return mouseSensivity; }
            set { mouseSensivity = value; }
        }

        public bool MouseYInverted
        {
            get { if (mouseYInverted < 0) return true; return false; }
            set { if (value) mouseYInverted = -1; else mouseYInverted = 1; }
        }
    }
}
