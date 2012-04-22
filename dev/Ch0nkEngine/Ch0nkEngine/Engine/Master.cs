using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Cameras;
using Ch0nkEngine.Engine.Composition;
using Ch0nkEngine.Cameras.Components;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
using SlimDX;
using SlimDX.D3DCompiler;

namespace Ch0nkEngine
{
    class Master : Container
    {

        public MainForm form;
        public World world;
        public Camera camera;

        public Device device11;
        public SwapChain swapChain;
        public Texture2D backBuffer;
        public RenderTargetView renderTargetView;



        #region Singleton

        private static volatile Master instance;
        private static object syncRoot = new Object();


        public static Master I
        {
            get
            {
                if (instance == null)
                {
                    throw new Exception("Master called without being created. Do 'new Master(form)' somewhere first.");
                }

                return instance;
            }
        }

        #endregion

        public Master(MainForm form)
        {
            if (instance != null)
            {
                throw new Exception("The Master, there can be only one.");
            }
            instance = this;
            this.form = form;
        }

        public override void Load()
        {
            InitializeCamera();
            InitializeGraphics();
            InitializeWorld();
        }

        private void InitializeGraphics()
        {
            // Creating device (we accept dx10 cards or greater)
            FeatureLevel[] levels = {
                                        FeatureLevel.Level_11_0,
                                        FeatureLevel.Level_10_1,
                                        FeatureLevel.Level_10_0
                                    };

            // Defining our swap chain
            SwapChainDescription desc = new SwapChainDescription();
            desc.BufferCount = 1;
            desc.Usage = Usage.BackBuffer | Usage.RenderTargetOutput;
            desc.ModeDescription = new ModeDescription(0, 0, new Rational(0, 0), Format.R8G8B8A8_UNorm);
            desc.SampleDescription = new SampleDescription(1, 0);
            desc.OutputHandle = form.Handle;
            desc.IsWindowed = true;
            desc.SwapEffect = SwapEffect.Discard;

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, levels, desc, out device11, out swapChain);

            // Getting back buffer
            backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);

            // Defining render view
            renderTargetView = new RenderTargetView(device11, backBuffer);
            device11.ImmediateContext.OutputMerger.SetTargets(renderTargetView);
            device11.ImmediateContext.Rasterizer.SetViewports(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));


        }



        private void InitializeCamera()
        {
            camera = new TargetCamera();
            camera.Position = new Vector3(0, -5, 5);
            camera.Target = Vector3.Zero;
            camera.UpVector = Vector3.UnitZ;
            camera.Load();
            camera.AddComponent(new KeyboardComponent());
            camera.AddComponent(new MouseComponent());
            camera.LoadComponents();
            AddComponent(camera);
        }

        private void InitializeWorld()
        {
            world = new World();
            world.Load();
            AddComponent(world);
        }


        public override void Render(GameTime time)
        {
            if (device11 == null)
            {
                return;
            }
            RenderComponents(time);
        }

        public override void Update(GameTime time)
        {
            UpdateComponents(time);
        }

        public override void Unload()
        {

            UnloadComponents();
            renderTargetView.Dispose();
            backBuffer.Dispose();
            swapChain.Dispose();
            device11.Dispose();
            device11 = null;

        }
    }
}
