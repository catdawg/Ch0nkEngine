using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Data.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ch0nkEngine.XNAViewer.Drawing
{
    

    public class DrawableFaceCollection
    {
        private readonly Game _game;
        private Effect _effect;
        private Texture2D _singleTexture;

        protected VertexBuffer MyVertexBuffer;
        protected IndexBuffer MyIndexBuffer;
        protected int NumVertices;
        protected int NumTriangles;

        public DrawableFaceCollection(Game game, String materialTexture, List<Block> blocks)
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

            //new Block(new Vector3i(0,0,0),1), 
            

            foreach (Block block in blocks)
            {
                CreateCube(block, ref customVertexList, ref indicesList, ref indexerValue);

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
        }


        private void CreateCube(Block block, ref List<VertexNormalTexture> customVertexList, ref List<int> indicesList, ref int indexerValue)
        {
            Vector3 location = block.AbsolutePosition.ToVector3();
            float size = 1;

            //Attention! Texture coordinates are different from OpenGL: http://msdn.microsoft.com/en-us/library/windows/desktop/bb206245(v=vs.85).aspx
            //top face
            if(block.HasTop)
            {
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, size), Vector3.UnitZ, new Vector2(0, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, size, size), Vector3.UnitZ, new Vector2(0, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, size, size), Vector3.UnitZ, new Vector2(1, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, size), Vector3.UnitZ, new Vector2(1, 1)));
                AddIndices(ref indicesList, ref indexerValue);
            }


            //bottom face
            if (block.HasBottom)
            {
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, 0), -Vector3.UnitZ,new Vector2(0, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, 0), -Vector3.UnitZ,new Vector2(1, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, size, 0), -Vector3.UnitZ,new Vector2(1, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, size, 0), -Vector3.UnitZ,new Vector2(0, 0)));
                AddIndices(ref indicesList, ref indexerValue);
            }

            //back face
            if (block.HasBack)
            {
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, 0), -Vector3.UnitY, new Vector2(0, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, size), -Vector3.UnitY, new Vector2(0, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, size), -Vector3.UnitY, new Vector2(1, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, 0), -Vector3.UnitY, new Vector2(1, 1)));
                AddIndices(ref indicesList, ref indexerValue);
            }

            //front face
            if (block.HasFront)
            {
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, size, 0), Vector3.UnitY, new Vector2(0, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, size, 0), Vector3.UnitY, new Vector2(1, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, size, size), Vector3.UnitY, new Vector2(1, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, size, size), Vector3.UnitY, new Vector2(0, 0)));
                AddIndices(ref indicesList, ref indexerValue);
            }

            //right face
            if (block.HasRight)
            {
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, 0), Vector3.UnitX, new Vector2(0, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, 0, size), Vector3.UnitX, new Vector2(0, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, size, size), Vector3.UnitX, new Vector2(1, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(size, size, 0), Vector3.UnitX, new Vector2(1, 1)));
                AddIndices(ref indicesList, ref indexerValue);
            }

            //left face
            if (block.HasLeft)
            {
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, 0), -Vector3.UnitX, new Vector2(0, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, size, 0), -Vector3.UnitX, new Vector2(1, 1)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, size, size), -Vector3.UnitX, new Vector2(1, 0)));
                customVertexList.Add(new VertexNormalTexture(location + new Vector3(0, 0, size), -Vector3.UnitX, new Vector2(0, 0)));
                AddIndices(ref indicesList, ref indexerValue);
            }
        }

        private void AddIndices(ref List<int> indicesList, ref int indexerValue)
        {
            indicesList.AddRange(new[] { indexerValue, indexerValue + 1, indexerValue + 2, indexerValue, indexerValue + 2, indexerValue + 3 });
            indexerValue += 4;
        }


        public void Draw(GameTime gameTime, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Vector3 cameraPosition, Vector3 cameraDirection)
        {
            Matrix viewProjectionMatrix = viewMatrix * projectionMatrix;

            _effect.CurrentTechnique = _effect.Techniques["SingleTextured"];
            _effect.Parameters["World"].SetValue(worldMatrix);
            _effect.Parameters["View"].SetValue(viewMatrix); //camera.ViewMatrix
            _effect.Parameters["Projection"].SetValue(projectionMatrix); //camera.ViewMatrix
            _effect.Parameters["ViewProjectionMatrix"].SetValue(viewProjectionMatrix); //camera.ViewMatrix
            _effect.Parameters["Texture0"].SetValue(_singleTexture);
            _effect.Parameters["CameraPosition"].SetValue(cameraPosition);
            _effect.Parameters["CameraDirection"].SetValue(cameraDirection);

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                //apply each pass
                pass.Apply();

                Device.Indices = MyIndexBuffer;
                Device.SetVertexBuffer(MyVertexBuffer);
                Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumVertices, 0, NumTriangles);
            }
        }

        public GraphicsDevice Device
        {
            get { return _game.GraphicsDevice; }
        }
    }
}
