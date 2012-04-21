using Ch0nkEngine.Utils;
using SlimDX;
using SlimDX.Direct3D11;
using System.Windows.Forms;

namespace Ch0nkEngine.Cameras
{
    public class TargetCamera : Camera
    {
        public TargetCamera()
        {
        }

        public override void Load()
        {
            viewMatrix = Matrix.LookAtRH(position, target, UpVector);
            projectionMatrix =
                Matrix.PerspectiveFovRH(MathExtended.Pi / 3.0f, Master.I.form.ClientSize.Width / (float)Master.I.form.ClientSize.Height, 1f, 10000f);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateComponents(gameTime);
            viewMatrix = Matrix.LookAtRH(position, target, UpVector);
        }
    }
}
