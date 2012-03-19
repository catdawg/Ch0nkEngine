using SlimDX;
using SlimDX.Direct3D11;

namespace Ch0nkEngine.Cameras
{
    class OrthogonalCamera : Camera
    {
        public OrthogonalCamera(Viewport viewport) 
            : base(viewport)
        {
        }

        public override void Initialize()
        {
            
            //projectionMatrix = Matrix.CreateOrthographic(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, 0.1f, 10000);
            viewMatrix = Matrix.LookAtLH(position, target, UpVector);
        }

        public override void Update(GameTime gameTime)
        {
            //sets up the view in case it was changed
            //if (bUpdateView)
            viewMatrix = Matrix.LookAtLH(position, target, UpVector);
        }
    }
}
