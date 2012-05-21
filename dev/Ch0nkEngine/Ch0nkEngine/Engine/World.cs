using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Composition;
using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Data;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using Buffer = SlimDX.Direct3D11.Buffer;
using System.Runtime.InteropServices;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data.Materials.Types;

namespace Ch0nkEngine
{
    // Block Structure
    // LayoutKind.Sequential is required to ensure the public variables
    // are written to the datastream in the correct order.
    [StructLayout(LayoutKind.Sequential)]
    struct Bl0ck
    {
        public Vector3 Position;
        public byte Size;
        public byte Attr1;
        public static readonly InputElement[] inputElements = new[] {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("ATTRIBUTES", 0, Format.R8G8_UInt, InputElement.AppendAligned, 0),
            };
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Bl0ck));
        public Bl0ck(Vector3 position, byte size)
        {
            Size = 0;
            Attr1 = size;
            Position = position;
        }
    }
    
    class World : Container
    {
        private Effect _effect;
        private InputLayout _layout;

        private Buffer _vertexBuffer;
        private DataBox _box;
        private int _vertexBufferSize;
        private int _numOfVertices;
        


        public override void Update(GameTime time)
        {
            
            UpdateComponents(time);
        }

        public override void Render(GameTime time)
        {
            var technique = _effect.GetTechniqueByIndex(0);
            var pass = technique.GetPassByIndex(0);
            _layout = new InputLayout(Master.I.device11, pass.Description.Signature, Bl0ck.inputElements);

            // Uploading to the device
            Master.I.device11.ImmediateContext.InputAssembler.InputLayout = _layout;
            Master.I.device11.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            Master.I.device11.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Bl0ck.SizeInBytes, 0));

            Matrix worldMatrix = Matrix.Identity;
            Matrix viewMatrix = Master.I.camera.ViewMatrix;
            Matrix projectionMatrix = Master.I.camera.ProjectionMatrix;

            _effect.GetVariableByName("finalMatrix").AsMatrix().SetMatrix(worldMatrix * viewMatrix * projectionMatrix);

            // Render
            _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(Master.I.device11.ImmediateContext);

            Master.I.device11.ImmediateContext.Draw(_numOfVertices, 0);
            Master.I.swapChain.Present(0, PresentFlags.None);
            RenderComponents(time);
        }

        public override void Load()
        {
            // Preparing shaders
            using (ShaderBytecode byteCode = ShaderBytecode.CompileFromFile("Effet.fx", "bidon", "fx_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None))
            {
                _effect = new Effect(Master.I.device11, byteCode);
            }

            // Creating geometry
            List<Bl0ck> verticesList = new List<Bl0ck>();
            Realm realm = new Realm();
            var dimension = realm.Dimensions[0];
            {
                dimension.ChangeMaterial(new Ch0nkEngine.Data.Data.BoundingShapes.BoundingSphere(new Vector3i(32, 32, 64), 20), new AirMaterial());
                //var blocks = dimension.GetRandomTestingBlocks(new Vector3i());
                var blocks = dimension.GetAllBlocks();
                foreach(var block in blocks)
                {
                    if (block.Material.MaterialCode == 0)
                        continue;
                    verticesList.Add(new Bl0ck(new Vector3(block.AbsolutePosition.X, block.AbsolutePosition.Y, block.AbsolutePosition.Z),
                        block.Size));
                }
            }
            
            _numOfVertices = verticesList.Count;
            _vertexBufferSize = verticesList.Count * Bl0ck.SizeInBytes;
            // Creating vertex buffer
            var stream = new DataStream(_vertexBufferSize, true, true);
            stream.WriteRange(verticesList.ToArray());
            stream.Position = 0;

            _vertexBuffer = new Buffer(Master.I.device11, stream, new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage = ResourceUsage.Dynamic
            });
            stream.Dispose();


            _box = Master.I.device11.ImmediateContext.MapSubresource(_vertexBuffer, 0, MapMode.WriteNoOverwrite, SlimDX.Direct3D11.MapFlags.None);
            // Texture
            Texture2D texture2D = Texture2D.FromFile(Master.I.device11, "yoda.jpg");
            ShaderResourceView view = new ShaderResourceView(Master.I.device11, texture2D);

            _effect.GetVariableByName("yodaTexture").AsResource().SetResource(view);

        }

        public override void Unload()
        {
            Master.I.device11.ImmediateContext.UnmapSubresource(_vertexBuffer, 0);
            _layout.Dispose();
            _effect.Dispose();
        }
    }
}
