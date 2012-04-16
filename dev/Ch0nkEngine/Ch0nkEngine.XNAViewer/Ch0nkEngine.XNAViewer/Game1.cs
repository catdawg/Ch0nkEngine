using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SXL.Cameras;
using SXL.Cameras.Components;
using SXL.Debug;

namespace Ch0nkEngine.XNAViewer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        //graphics and renderstates
        private readonly GraphicsDeviceManager _graphics;
        private readonly RasterizerState _rs = new RasterizerState();

        //the first-person camera
        private Camera _camera;

        //to draw 2D sprites
        private SpriteBatch _spriteBatch;
        
        //for debugging
        private FrameRateDisplay _frameRateDisplay;

        //flags to indicate if certain rendering modes should be enabled
        private bool _wireframeEnabled;

        //the world
        private Ch0nkWorld _ch0NkWorld;

        //In order to track input changes
        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private MouseState _oldMouseState = Mouse.GetState();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferMultiSampling = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _camera = new TargetCamera(_graphics) { Position = new Vector3(0, -25, 25), Target = new Vector3(0, 0, 0), UpVector = Vector3.UnitZ, FarPlaneDistance = 10000, NearPlaneDistance = 1 };
            _camera.Initialize();
            _camera.AddComponent(new CrosshairComponent("Crosshairs/Crosshair01"));
            _camera.AddComponent(new KeyboardComponent { Speed = 1 });
            _camera.AddComponent(new MouseComponent());

            _ch0NkWorld = new Ch0nkWorld(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _camera.LoadContent(Content);

            _frameRateDisplay = new FrameRateDisplay(this, "Fonts/Arial14");

            _rs.FillMode = FillMode.WireFrame;

            _oldKeyboardState = Keyboard.GetState();
            _oldMouseState = Mouse.GetState();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyUp(Keys.F1) && _oldKeyboardState.IsKeyDown(Keys.F1))
                _wireframeEnabled = !_wireframeEnabled;
            
            if (Mouse.GetState().LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed)
                _ch0NkWorld.MouseClick(_camera);

            if (Mouse.GetState().MiddleButton == ButtonState.Released && _oldMouseState.MiddleButton == ButtonState.Pressed)
                _ch0NkWorld.MouseMiddleClick(_camera);

            if (Mouse.GetState().RightButton == ButtonState.Released && _oldMouseState.RightButton == ButtonState.Pressed)
                _ch0NkWorld.MouseRightClick(_camera);

            //update all camera modifications
            _camera.Update(gameTime);

            //update the framerate
            _frameRateDisplay.Update(gameTime);

            //update any world modifications
            _ch0NkWorld.Update(gameTime);

            //update the states of the keyboard and mouse
            _oldKeyboardState = Keyboard.GetState();
            _oldMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //to compensate the nasty effects of the spritebatch
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //enable wireframe if desired
            if (_wireframeEnabled)
                GraphicsDevice.RasterizerState = _rs;

            //draw the world
            _ch0NkWorld.Draw(gameTime, _camera);

            //draw the crosshair and the framerate
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _camera.Draw(gameTime, _spriteBatch);
            _frameRateDisplay.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
