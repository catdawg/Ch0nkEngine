using SlimDX;
using SlimDX.Direct3D11;
using System.Windows.Forms;

namespace Ch0nkEngine.Cameras
{
    class OrthogonalCamera : Camera
    {
        public OrthogonalCamera() 
        {
        }

        public override void Load()
        {
            projectionMatrix = Matrix.OrthoRH(Master.I.form.ClientSize.Width, Master.I.form.ClientSize.Height, 1f, 10000f);
            viewMatrix = Matrix.LookAtLH(position, target, UpVector);
        }

        public override void Update(GameTime gameTime)
        {
            viewMatrix = Matrix.LookAtLH(position, target, UpVector);
        }

    }
}
