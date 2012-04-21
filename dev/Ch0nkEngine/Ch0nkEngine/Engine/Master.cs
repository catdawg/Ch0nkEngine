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
using Buffer = SlimDX.Direct3D11.Buffer;
using SlimDX;
using SlimDX.D3DCompiler;

namespace Ch0nkEngine
{
    class Master : Container
    {

        public MainForm form;
        public World world;
        public Camera camera;

        SlimDX.Direct3D11.Device device11;
        SwapChain swapChain;
        Texture2D backBuffer;
        RenderTargetView renderTargetView;
        Effect effect;
        InputLayout layout;



        const int vertexSize = sizeof(float) * 5;

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


            // Preparing shaders
            PrepareShaders();

            // Creating geometry
            CreateGeometry();

            // Setting constants
            AffectConstants();
        }

        private void AffectConstants()
        {
            // Texture
            Texture2D texture2D = Texture2D.FromFile(device11, "yoda.jpg");
            ShaderResourceView view = new ShaderResourceView(device11, texture2D);

            effect.GetVariableByName("yodaTexture").AsResource().SetResource(view);

            RasterizerStateDescription rasterizerStateDescription = new RasterizerStateDescription { CullMode = CullMode.None, FillMode = FillMode.Solid };

            device11.ImmediateContext.Rasterizer.State = RasterizerState.FromDescription(device11, rasterizerStateDescription);

            // Matrices
            //Matrix worldMatrix = Matrix.RotationY(0.5f);
            //Matrix viewMatrix = Matrix.Translation(0, 0, 5.0f);
            //const float fov = 0.8f;
            //Matrix projectionMatrix = Matrix.PerspectiveFovLH(fov, ClientSize.Width / (float)ClientSize.Height, 0.1f, 1000.0f);
            Matrix worldMatrix = Matrix.Identity;
            Matrix viewMatrix = camera.ViewMatrix;
            Matrix projectionMatrix = camera.ProjectionMatrix;

            effect.GetVariableByName("finalMatrix").AsMatrix().SetMatrix(worldMatrix * viewMatrix * projectionMatrix);
        }

        private void PrepareShaders()
        {
            using (ShaderBytecode byteCode = ShaderBytecode.CompileFromFile("Effet.fx", "bidon", "fx_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None))
            {
                effect = new Effect(device11, byteCode);
            }

            var technique = effect.GetTechniqueByIndex(0);
            var pass = technique.GetPassByIndex(0);
            layout = new InputLayout(device11, pass.Description.Signature, new[] {
                                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0)
                });
        }

        void CreateGeometry()
        {
            float[] vertices = new[]
                                    {
                                        -1.0f, -1.0f, 0f, 0f, 1.0f,
                                        1.0f, -1.0f, 0f, 1.0f, 1.0f,
                                        1.0f, 1.0f, 0f, 1.0f, 0.0f,
                                        -1.0f, 1.0f, 0f, 0.0f, 0.0f,

                                    };

            short[] faces = new[]
                                {
                                        (short)0, (short)1, (short)2,
                                        (short)0, (short)2, (short)3
                                };

            // Creating vertex buffer
            var stream = new DataStream(4 * vertexSize, true, true);
            stream.WriteRange(vertices);
            stream.Position = 0;

            var vertexBuffer = new Buffer(device11, stream, new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();

            // Index buffer
            stream = new DataStream(6 * 2, true, true);
            stream.WriteRange(faces);
            stream.Position = 0;

            var indices = new Buffer(device11, stream, new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();

            // Uploading to the device
            device11.ImmediateContext.InputAssembler.InputLayout = layout;
            device11.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            device11.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexSize, 0));
            device11.ImmediateContext.InputAssembler.SetIndexBuffer(indices, Format.R16_UInt, 0);
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
            
            Matrix worldMatrix = Matrix.Identity;
            Matrix viewMatrix = camera.ViewMatrix;
            Matrix projectionMatrix = camera.ProjectionMatrix;

            effect.GetVariableByName("finalMatrix").AsMatrix().SetMatrix(worldMatrix * viewMatrix * projectionMatrix);

            // Render
            device11.ImmediateContext.ClearRenderTargetView(renderTargetView, new Color4(1.0f, 0, 0, 1.0f));
            effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(device11.ImmediateContext);
            device11.ImmediateContext.DrawIndexed(6, 0, 0);
            swapChain.Present(0, PresentFlags.None);
            RenderComponents(time);
        }

        public override void Update(GameTime time)
        {
            UpdateComponents(time);
        }

        public override void Unload()
        {
            layout.Dispose();
            effect.Dispose();
            renderTargetView.Dispose();
            backBuffer.Dispose();
            swapChain.Dispose();
            device11.Dispose();
            device11 = null;

            UnloadComponents();

        }
    }
}
