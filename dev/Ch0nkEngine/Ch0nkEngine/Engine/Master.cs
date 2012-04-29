using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Cameras;
using Ch0nkEngine.Composition;
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
        public Stats stats;

        public Device device11;
        public SwapChain swapChain;
        public Texture2D backBuffer;
        public Texture2D depthBuffer;
        public DepthStencilView depthView;
        public DepthStencilState depthState;
        public RenderTargetView renderTargetView;
        public RasterizerState rasterState;



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
            InitializeStats();
        }

        private void InitializeStats()
        {

            stats = new Stats();
            stats.Load();
            AddComponent(stats);
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
            desc.Usage = Usage.RenderTargetOutput;
            desc.ModeDescription = new ModeDescription(0, 0, new Rational(0, 0), Format.R8G8B8A8_UNorm);
            desc.SampleDescription = new SampleDescription(1, 0);
            desc.OutputHandle = form.Handle;
            desc.IsWindowed = true;
            desc.SwapEffect = SwapEffect.Discard;

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, levels, desc, out device11, out swapChain);

            Format depthFormat = Format.D24_UNorm_S8_UInt;
            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = depthFormat,
                Height = form.Height,
                Width = form.Width,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

            depthBuffer = new Texture2D(device11, depthBufferDesc);

            DepthStencilViewDescription dsViewDesc = new DepthStencilViewDescription
            {
                Format = depthFormat,
                Dimension = DepthStencilViewDimension.Texture2D,
                MipSlice = 0,
            };

            depthView = new DepthStencilView(device11, depthBuffer, dsViewDesc);

            DepthStencilStateDescription dsStateDesc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,

                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,


                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },
                
            };

            depthState = DepthStencilState.FromDescription(device11, dsStateDesc);
           

            // Getting back buffer
            backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);

            // Defining render view
            renderTargetView = new RenderTargetView(device11, backBuffer);
            device11.ImmediateContext.OutputMerger.DepthStencilState = depthState;
            device11.ImmediateContext.OutputMerger.SetTargets(depthView, renderTargetView);


            // Setup the raster description which will determine how and what polygons will be drawn.
            RasterizerStateDescription rasterDesc = new RasterizerStateDescription(){
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f,
            };

            // Create the rasterizer state from the description we just filled out.
            rasterState = RasterizerState.FromDescription(device11, rasterDesc);
            
            // Now set the rasterizer state.
            device11.ImmediateContext.Rasterizer.State = rasterState;
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

            device11.ImmediateContext.ClearRenderTargetView(renderTargetView, new Color4(1.0f, 0, 0, 1.0f));
            device11.ImmediateContext.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
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
