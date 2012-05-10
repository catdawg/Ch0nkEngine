using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ch0nkEngine.XNAViewer.Drawing
{
    public struct VertexNormalTexture
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Texture;

        public VertexNormalTexture(Vector3 position)
            : this()
        {
            Position = position;
        }

        public VertexNormalTexture(Vector3 position, Vector3 normal, Vector2 texture) : this()
        {
            Position = position;
            Normal = normal;
            Texture = texture;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );
    }

    public struct CubeInstanceInfo
    {
        public Vector4 PositionSize;

        public CubeInstanceInfo(Vector4 positionSize)
            : this()
        {
            PositionSize = positionSize;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1)
        );

        public CubeInstanceInfo(Vector3 position, short size)
        {
            PositionSize = new Vector4(position.X, position.Y, position.Z, size);
        }
    }

    public class DrawableCubeCollection
    {
        private readonly Game _game;
        private Effect _effect;
        private Texture2D _singleTexture;

        protected VertexBuffer MyVertexBuffer;
        protected IndexBuffer MyIndexBuffer;
        protected int NumVertices;
        protected int NumTriangles;

        private VertexBuffer _instanceBuffer;
        private VertexBufferBinding[] _bindings;
        private int _instanceCount;
        
        public DrawableCubeCollection(Game game, String materialTexture, List<Block> blocks)
        {
            _game = game;
            _singleTexture = game.Content.Load<Texture2D>("Materials/" + materialTexture);
            _effect = game.Content.Load<Effect>("Shaders/SingleTextured");

            InitializeBuffers(blocks);
        }

        private void InitializeBuffers(List<Block> blocks)
        {
            List<VertexNormalTexture> customVertexList = new List<VertexNormalTexture>();
            List<int> indicesList = new List<int>();
            int indexerValue = 0;

            CreateCube(new Block(new Vector3i(0,0,0),0,1), ref customVertexList, ref indicesList, ref indexerValue);

            foreach (Block block in blocks)
            {
                //top face
                
                /*for (int i = 0; i < 4; i++)
                {
                    indicesList.Add(indexerValue++);
                }*/
            }

            MyVertexBuffer = new VertexBuffer(Device, VertexNormalTexture.VertexDeclaration, customVertexList.Count, BufferUsage.WriteOnly);
            MyVertexBuffer.SetData(customVertexList.ToArray());

            MyIndexBuffer = new IndexBuffer(Device, typeof(int), indicesList.Count, BufferUsage.WriteOnly);
            MyIndexBuffer.SetData(indicesList.ToArray());

            NumVertices = customVertexList.Count;
            NumTriangles = indicesList.Count / 3;

            InitializeInstances(blocks);

            _bindings = new VertexBufferBinding[2];
            _bindings[0] = new VertexBufferBinding(MyVertexBuffer, 0);
            _bindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);
        }

        private void InitializeInstances(List<Block> blocks)
        {
            List<CubeInstanceInfo> instanceInfos = new List<CubeInstanceInfo>();

            foreach (Block block in blocks)
            {
                instanceInfos.Add(new CubeInstanceInfo(block.Location.ToVector3(), block.Size));
            }

            _instanceBuffer = new VertexBuffer(Device, CubeInstanceInfo.VertexDeclaration, blocks.Count, BufferUsage.WriteOnly);
            _instanceBuffer.SetData(instanceInfos.ToArray());
            _instanceCount = blocks.Count;
        }

        private void CreateCube(Block block, ref List<VertexNormalTexture> customVertexList, ref List<int> indicesList, ref int indexerValue)
        {
            Vector3 location = block.Location.ToVector3();
            float size = block.Size;
            //top face
            customVertexList.Add(new VertexNormalTexture(location, Vector3.UnitZ,new Vector2(0,-1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size,0,0), Vector3.UnitZ,new Vector2(1,-1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size,-size,0), Vector3.UnitZ,new Vector2(1,0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0,-size,0), Vector3.UnitZ,new Vector2(0,0)));
            AddIndices(ref indicesList, ref indexerValue);

            //bottom face
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, -size), -Vector3.UnitZ, new Vector2(0, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, -size, -size), -Vector3.UnitZ, new Vector2(0, 0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, -size, -size), -Vector3.UnitZ, new Vector2(1, 0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, -size), -Vector3.UnitZ, new Vector2(1, -1)));
            AddIndices(ref indicesList, ref indexerValue);

            //back face
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, -size, 0), -Vector3.UnitY, new Vector2(0, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, -size, 0), -Vector3.UnitY, new Vector2(1, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, -size, -size), -Vector3.UnitY, new Vector2(1, 0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, -size, -size), -Vector3.UnitY, new Vector2(0, 0)));
            AddIndices(ref indicesList, ref indexerValue);

            //front face
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, 0), Vector3.UnitY, new Vector2(0, -1)));
            customVertexList.Add(new VertexNormalTexture(location, Vector3.UnitY, new Vector2(1, -1))); 
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, -size), Vector3.UnitY, new Vector2(1, 0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, -size), Vector3.UnitY, new Vector2(0, 0)));
            AddIndices(ref indicesList, ref indexerValue);

            //right face
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, -size, 0), Vector3.UnitX, new Vector2(0, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, 0), Vector3.UnitX, new Vector2(1, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, -size), Vector3.UnitX, new Vector2(1, 0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, -size, -size), Vector3.UnitX, new Vector2(0, 0)));
            AddIndices(ref indicesList, ref indexerValue);

            //left face
            customVertexList.Add(new VertexNormalTexture(location, -Vector3.UnitX, new Vector2(1, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, -size, 0), -Vector3.UnitX, new Vector2(0, -1)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, -size, -size), -Vector3.UnitX, new Vector2(0, 0)));
            customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, -size), -Vector3.UnitX, new Vector2(1, 0)));
            AddIndices(ref indicesList, ref indexerValue);
        }

        private void AddIndices(ref List<int> indicesList, ref int indexerValue)
        {
            indicesList.AddRange(new[] { indexerValue, indexerValue + 1, indexerValue + 2, indexerValue, indexerValue + 2, indexerValue + 3 });
            indexerValue += 4;
        }


        public void Draw(GameTime gameTime, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix viewProjectionMatrix = viewMatrix*projectionMatrix;

            _effect.CurrentTechnique = _effect.Techniques["SingleTexturedInstancing"];
            _effect.Parameters["World"].SetValue(worldMatrix);
            _effect.Parameters["View"].SetValue(viewMatrix); //camera.ViewMatrix
            _effect.Parameters["Projection"].SetValue(projectionMatrix); //camera.ViewMatrix
            _effect.Parameters["ViewProjectionMatrix"].SetValue(viewProjectionMatrix); //camera.ViewMatrix
            _effect.Parameters["Texture0"].SetValue(_singleTexture);

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                //apply each pass
                pass.Apply();

                Device.Indices = MyIndexBuffer;
                Device.SetVertexBuffers(_bindings);
                //Device.SetVertexBuffer(MyVertexBuffer);
                Device.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, NumVertices, 0, NumTriangles, _instanceCount);
            }
        }

        public GraphicsDevice Device
        {
            get { return _game.GraphicsDevice; }
        }
    }
}
