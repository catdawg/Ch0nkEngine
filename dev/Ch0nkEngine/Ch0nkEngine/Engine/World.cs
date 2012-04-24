using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Engine.Composition;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace Ch0nkEngine
{
    class World : Container
    {
        public Effect effect;
        public InputLayout layout;

        const int vertexSize = sizeof(float) * 3;

        public override void Render(GameTime time)
        {
            Matrix worldMatrix = Matrix.Identity;
            Matrix viewMatrix = Master.I.camera.ViewMatrix;
            Matrix projectionMatrix = Master.I.camera.ProjectionMatrix;

            effect.GetVariableByName("finalMatrix").AsMatrix().SetMatrix(worldMatrix * viewMatrix * projectionMatrix);

            // Render
            Master.I.device11.ImmediateContext.ClearRenderTargetView(Master.I.renderTargetView, new Color4(1.0f, 0, 0, 1.0f));
            effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(Master.I.device11.ImmediateContext);
            Master.I.device11.ImmediateContext.Draw(4, 0);
            Master.I.swapChain.Present(0, PresentFlags.None);
            RenderComponents(time);
        }

        public override void Load()
        {
            // Preparing shaders
            PrepareShaders();

            // Creating geometry
            CreateGeometry();

            // Setting constants
            AffectConstants();
        }

        public override void  Unload()
        {
            layout.Dispose();
            effect.Dispose();
        }

        private void AffectConstants()
        {
            // Texture
            Texture2D texture2D = Texture2D.FromFile(Master.I.device11, "yoda.jpg");
            ShaderResourceView view = new ShaderResourceView(Master.I.device11, texture2D);

            effect.GetVariableByName("yodaTexture").AsResource().SetResource(view);

            RasterizerStateDescription rasterizerStateDescription = new RasterizerStateDescription { CullMode = CullMode.None, FillMode = FillMode.Solid };

            Master.I.device11.ImmediateContext.Rasterizer.State = RasterizerState.FromDescription(Master.I.device11, rasterizerStateDescription);

            // Matrices
            //Matrix worldMatrix = Matrix.RotationY(0.5f);
            //Matrix viewMatrix = Matrix.Translation(0, 0, 5.0f);
            //const float fov = 0.8f;
            //Matrix projectionMatrix = Matrix.PerspectiveFovLH(fov, ClientSize.Width / (float)ClientSize.Height, 0.1f, 1000.0f);
            Matrix worldMatrix = Matrix.Identity;
            Matrix viewMatrix = Master.I.camera.ViewMatrix;
            Matrix projectionMatrix = Master.I.camera.ProjectionMatrix;

            effect.GetVariableByName("finalMatrix").AsMatrix().SetMatrix(worldMatrix * viewMatrix * projectionMatrix);
        }

        private void PrepareShaders()
        {
            using (ShaderBytecode byteCode = ShaderBytecode.CompileFromFile("Effet.fx", "bidon", "fx_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None))
            {
                effect = new Effect(Master.I.device11, byteCode);
            }

            var technique = effect.GetTechniqueByIndex(0);
            var pass = technique.GetPassByIndex(0);
            layout = new InputLayout(Master.I.device11, pass.Description.Signature, new[] {
                                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0)
                });
        }

        void CreateGeometry()
        {
            /*
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
            */
            float[] vertices = new[]
                                    {
                                        -1.0f, -1.0f, 0f,
                                        1.0f, -1.0f, 0f,
                                        1.0f, 1.0f, 0f,
                                        -1.0f, 1.0f, 0f
                                    };
            // Creating vertex buffer
            var stream = new DataStream(4 * vertexSize, true, true);
            stream.WriteRange(vertices);
            stream.Position = 0;

            var vertexBuffer = new Buffer(Master.I.device11, stream, new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();

            /*
            // Index buffer
            stream = new DataStream(6 * 2, true, true);
            stream.WriteRange(faces);
            stream.Position = 0;

            var indices = new Buffer(Master.I.device11, stream, new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();
             */

            // Uploading to the device
            Master.I.device11.ImmediateContext.InputAssembler.InputLayout = layout;
            Master.I.device11.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            Master.I.device11.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexSize, 0));
            //Master.I.device11.ImmediateContext.InputAssembler.SetIndexBuffer(indices, Format.R16_UInt, 0);
        }
    }
}
