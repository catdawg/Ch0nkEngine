namespace Ch0nkEngine.Cameras.Components
{
    public abstract class CameraComponent
    {
        protected internal Camera Camera { get; set; }

        public virtual void Initialize()
        {
            //does nothing here
        }

        public virtual void Update(GameTime gameTime)
        {
            //does nothing here   
        }

        public virtual void Draw(GameTime gameTime)
        {
            //does nothing here   
        }
    }
}
