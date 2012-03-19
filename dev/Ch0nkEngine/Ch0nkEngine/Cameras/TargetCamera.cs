using Ch0nkEngine.Utils;
using SlimDX;
using SlimDX.Direct3D11;

namespace Ch0nkEngine.Cameras
{
    public class TargetCamera : Camera
    {
        public TargetCamera(Viewport viewport)
            : base(viewport)
        {
        }

        public override void Initialize()
        {
            
            viewMatrix = Matrix.LookAtLH(position, target, UpVector);
            //projectionMatrix = Matrix.PerspectiveFovLH(MathExtended.DegreesToRadians(60), Viewport.Width / (float)Viewport.Height, 0, 1000000);
            projectionMatrix = Matrix.OrthoLH(Viewport.Width, Viewport.Height, 0, 1000000);
        
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //sets up the view in case it was changed
            //if (bUpdateView)
                viewMatrix = Matrix.LookAtLH(position, target, UpVector);
        }
    }
}
