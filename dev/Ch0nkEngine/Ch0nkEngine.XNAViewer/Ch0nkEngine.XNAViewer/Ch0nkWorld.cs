using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data;
using Ch0nkEngine.Data.Data.Materials;
using Ch0nkEngine.Data.Data.Materials.Types;
using Ch0nkEngine.Data.Utils;
using Ch0nkEngine.XNAViewer.Drawing;
using ImageTools.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SXL.Cameras;
using Simplicit.Net.Lzo;
using BoundingBox = Ch0nkEngine.Data.Data.BoundingShapes.BoundingBox;
using BoundingSphere = Ch0nkEngine.Data.Data.BoundingShapes.BoundingSphere;

namespace Ch0nkEngine.XNAViewer
{
    public class Face
    {
        public Block[,] data = new Block[64,64];

        public List<Block> GetBlocks()
        {
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    blocks.Add(data[i, j]);
                }
            }

            return blocks;
        }

    }

    public class Ch0nkWorld
    {
        /*private readonly List<InstancedSingleTextureShadedGroup> _instancedSingleTextureShadedGroups = new List<InstancedSingleTextureShadedGroup>();
        Dictionary<String, List<InstanceInfo>> _materialInstances = new Dictionary<String, List<InstanceInfo>>();
        Dictionary<Vector3i,Sector> sectors = new Dictionary<Vector3i, Sector>();
        private Ch0nk _ch0nk = new Ch0nk();*/
        private DrawableFaceBuffer _buffer;
        private Realm _realm;

        [Serializable]
        struct Test
        {
            public short p1;
            public int p2;
            public byte p3;

            public Test(short p1, int p2, byte p3)
            {
                this.p1 = p1;
                this.p2 = p2;
                this.p3 = p3;
            }
        }

        private Dimension _dimension;
        private Game _game;

        public Ch0nkWorld(Game game)
        {
            _realm = new Realm();

            _dimension = _realm.Dimensions[0];

            _dimension.GenerateAt(new Vector3i(0, 0, 0));

            _game = game;

            _realm.Dimensions[0].ChangeMaterial(new BoundingSphere(new Vector3i(32, 32, 64), 20), new AirMaterial());
            _realm.Dimensions[0].ChangeMaterial(new BoundingSphere(new Vector3i(0, 32, 64), 20), new SandMaterial());
            _realm.Dimensions[0].ChangeMaterial(new BoundingSphere(new Vector3i(0, 64, 32), 20), new StoneMaterial());

            //List<Block> blocks = new List<Block>();
            //blocks.Add(new Block(c, new Vector3b(0, 0, 0), new GrassMaterial(), 1));
            /*Stopwatch watch = new Stopwatch();
            watch.Start();
            blocks = _dimension.GetAllBlocks();*/

            List<Block> blocks = _dimension.GetRandomTestingBlocks(new Vector3i());

            Stopwatch watch = new Stopwatch();
            
            Block[,,] cubeBlocks = new Block[64,64,64];
            foreach (Block block in blocks)
            {
                cubeBlocks[block.RelativePosition.X, block.RelativePosition.Y, block.RelativePosition.Z] = block;
            }

            int size = 32;
            watch.Start();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    for (int k = 0; k < size; k++)
                    {
                        cubeBlocks[i, j, k].ViewFacesArray = new BitArray(6, false);
                        cubeBlocks[i, j, k].ViewFaces = 0x00;

                        //back and front
                        cubeBlocks[i, j, k].ViewFacesArray.Set(0, CheckValue(i, j - 1, k, cubeBlocks, size));
                        cubeBlocks[i, j, k].ViewFacesArray.Set(1, CheckValue(i, j + 1, k, cubeBlocks, size));

                        //left and right
                        cubeBlocks[i, j, k].ViewFacesArray.Set(2, CheckValue(i + 1, j, k, cubeBlocks, size));
                        cubeBlocks[i, j, k].ViewFacesArray.Set(3, CheckValue(i - 1, j, k, cubeBlocks, size));

                        //bottom and top
                        cubeBlocks[i, j, k].ViewFacesArray.Set(4, CheckValue(i, j, k - 1, cubeBlocks, size));
                        cubeBlocks[i, j, k].ViewFacesArray.Set(5, CheckValue(i, j, k + 1, cubeBlocks, size));

                        /*
                        //back and front
                        cubeBlocks[i, j, k].ViewFaces |= CheckValue(i - 1, j, k, cubeBlocks);
                        cubeBlocks[i, j, k].ViewFaces |= (byte)(CheckValue(i + 1, j, k, cubeBlocks) << 1);

                        //left and right
                        cubeBlocks[i, j, k].ViewFaces |= (byte)(CheckValue(i, j - 1, k, cubeBlocks) << 3);
                        cubeBlocks[i, j, k].ViewFaces |= (byte)(CheckValue(i, j + 1, k, cubeBlocks) << 2);

                        //bottom and top
                        cubeBlocks[i, j, k].ViewFaces |= (byte)(CheckValue(i, j, k-1, cubeBlocks) << 4);
                        cubeBlocks[i, j, k].ViewFaces |= (byte)(CheckValue(i, j, k+1, cubeBlocks) << 5);
                         * */
                    }
            Console.WriteLine(watch.ElapsedMilliseconds);

                /*List<Block>[,] verticalBlocks = new List<Block>[64,64];

                for (int i = 0; i < 64; i++ )
                {
                    for (int j = 0; j < 64; j++)
                    {
                        for (int k = 0; k < 64; k++)
                        {
                            if(verticalBlocks[i,j] == null)
                            {
                                verticalBlocks[i,j] = new List<Block>();
                                verticalBlocks[i,j].Add(cubeBlocks[i,j,k]);
                            }
                            else
                            {
                                Block lastBlock = verticalBlocks[i, j].Last();
                                if(lastBlock.Material.Equals(cubeBlocks[i,j,k].Material))
                                {
                                    lastBlock.Size += 1;
                                }
                                else
                                {
                                    verticalBlocks[i, j].Add(cubeBlocks[i, j, k]);
                                }
                            }
                        }
                    }
                }

                blocks = new List<Block>();

                foreach (List<Block> verticalBlock in verticalBlocks)
                {
                    blocks.Add(verticalBlock.First());
                }*/

                //blocks = new List<Block>();

            /*for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    for (int k = 0; k < 64; k++)
                    {
                        if((i == 0 || i == 63) ||
                            (j == 0 || j == 63) ||
                            (k == 0 || k == 63))
                        {
                            blocks.Add(cubeBlocks[i, j, k]);
                        }
                    }
                }
            }*/

            /*Face face = new Face();
            for (int j = 0; j < 64; j++)
            {
                for (int k = 0; k < 64; k++)
                {
                    if (cubeBlocks[0, j, k].Material is AirMaterial)
                    {
                        for(int i = 0; i < 64; i++)
                        {
                             if (!(cubeBlocks[i, j, k].Material is AirMaterial))
                             {
                                 face.data[j, k] = cubeBlocks[i, j, k];
                                 break;
                             }
                        }
                    }
                    else
                        face.data[j, k] = cubeBlocks[0, j, k];
                }
            }*/

            /*Face[] faces = new Face[6];
            for (int i = 0; i < 6; i++)
            {
                faces[i] = new Face();
                for (int j = 0; j < 64; j++)
                {
                    for (int k = 0; k < 64; k++)
                    {
                        faces[i].data[i,j] 
                    }
                }
                faces[i].data[]
            }*/

            //blocks.AddRange(face.GetBlocks());

            //Console.WriteLine(watch.ElapsedMilliseconds);
            //_buffer = new DrawableBuffer(game, blocks);
            _buffer = new DrawableFaceBuffer(game, blocks);
            //Console.WriteLine(watch.ElapsedMilliseconds);
            
        }

        private bool CheckValue(int x, int y, int z, Block[,,] cubeBlocks, int size)
        {
            if (x < 0 || y < 0 || z < 0 || x >= size || y >= size || z >= size || cubeBlocks[x, y, z].Material is AirMaterial)
                return true;

            return false;
        }


        public void MouseClick(Camera camera)
        {
            /*
            //Ray ray = new Ray(camera.Position, camera.Direction);
            _dimension.GenerateAt(new Vector3i((int)camera.Position.X, (int)camera.Position.Y, (int)camera.Position.Z));

            List<Block> blocks = _dimension.GetAllBlocks();

            _buffer = new DrawableBuffer(_game, blocks);
             * */
        }

        public void MouseMiddleClick(Camera camera)
        {
        }

        public void MouseRightClick(Camera camera)
        {
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, Camera camera)
        {
            _buffer.Draw(gameTime, Matrix.Identity, camera.ViewMatrix, camera.ProjectionMatrix, camera.Position, camera.Direction);
        }
    }
}
