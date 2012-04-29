using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Composition;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using Buffer = SlimDX.Direct3D11.Buffer;
using System.Runtime.InteropServices;

namespace Ch0nkEngine
{
    // Block Structure
    // LayoutKind.Sequential is required to ensure the public variables
    // are written to the datastream in the correct order.
    [StructLayout(LayoutKind.Sequential)]
    struct Bl0ck
    {
        public Vector3 Position;
        public static readonly InputElement[] inputElements = new[] {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
            };
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Bl0ck));
        public Bl0ck(Vector3 position)
        {
            Position = position;
        }
    }
    
    class World : Container
    {
        public Effect effect;
        public InputLayout layout;

        public Buffer vertexBuffer;
        DataBox box;
        public int vertexBufferSize; 
        
        private Bl0ck data = new Bl0ck(new Vector3(1.5f, -1.5f, 0f));
        private float z = 0f;
        private float z_increment = 0.01f;


        public override void Update(GameTime time)
        {
            if (z > 1)
                z_increment = -0.01f;
            else if (z < -1)
                z_increment = 0.01f;
            z += z_increment;
            data.Position.Z = z;
            Bl0ck[] dataArray = new []{data};
            box.Data.Position = 0;
            //Where T is my vertex structure type 
            box.Data.WriteRange<Bl0ck>(dataArray);
             
            
            UpdateComponents(time);
        }

        public override void Render(GameTime time)
        {
            Matrix worldMatrix = Matrix.Identity;
            Matrix viewMatrix = Master.I.camera.ViewMatrix;
            Matrix projectionMatrix = Master.I.camera.ProjectionMatrix;

            effect.GetVariableByName("finalMatrix").AsMatrix().SetMatrix(worldMatrix * viewMatrix * projectionMatrix);

            // Render
            effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(Master.I.device11.ImmediateContext);
            
            Master.I.device11.ImmediateContext.Draw(4, 0);
            Master.I.swapChain.Present(0, PresentFlags.None);
            RenderComponents(time);
        }

        public override void Load()
        {
            // Preparing shaders
            using (ShaderBytecode byteCode = ShaderBytecode.CompileFromFile("Effet.fx", "bidon", "fx_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None))
            {
                effect = new Effect(Master.I.device11, byteCode);
            }

            var technique = effect.GetTechniqueByIndex(0);
            var pass = technique.GetPassByIndex(0);
            layout = new InputLayout(Master.I.device11, pass.Description.Signature, Bl0ck.inputElements);

            // Creating geometry
            Bl0ck[] vertices = new[]
                                    {
                                        new Bl0ck(new Vector3(1.5f, -1.5f, 0f)),
                                        new Bl0ck(new Vector3(1.5f, 1.5f, 0f)),
                                        new Bl0ck(new Vector3(-1.5f, 1.5f, 0f)),
                                        new Bl0ck(new Vector3(-1.5f, -1.5f, 0f)),
                                    };

            vertexBufferSize = 4 * Bl0ck.SizeInBytes;
            // Creating vertex buffer
            var stream = new DataStream(vertexBufferSize, true, true);
            stream.WriteRange(vertices);
            stream.Position = 0;

            vertexBuffer = new Buffer(Master.I.device11, stream, new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage = ResourceUsage.Dynamic
            });
            stream.Dispose();

            // Uploading to the device
            Master.I.device11.ImmediateContext.InputAssembler.InputLayout = layout;
            Master.I.device11.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            Master.I.device11.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Bl0ck.SizeInBytes, 0));

            box = Master.I.device11.ImmediateContext.MapSubresource(vertexBuffer, 0, MapMode.WriteNoOverwrite, SlimDX.Direct3D11.MapFlags.None);
            // Texture
            Texture2D texture2D = Texture2D.FromFile(Master.I.device11, "yoda.jpg");
            ShaderResourceView view = new ShaderResourceView(Master.I.device11, texture2D);

            effect.GetVariableByName("yodaTexture").AsResource().SetResource(view);

        }

        public override void Unload()
        {
            Master.I.device11.ImmediateContext.UnmapSubresource(vertexBuffer, 0);
            layout.Dispose();
            effect.Dispose();
        }
    }
}
