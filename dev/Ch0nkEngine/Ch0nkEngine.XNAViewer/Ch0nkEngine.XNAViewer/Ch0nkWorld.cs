using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data;
using Ch0nkEngine.Data.Materials;
using Ch0nkEngine.Data.Utils;
using Ch0nkEngine.XNAViewer.Drawing;
using Microsoft.Xna.Framework;
using SXL.Cameras;
using Simplicit.Net.Lzo;

namespace Ch0nkEngine.XNAViewer
{
    public class Ch0nkWorld
    {
        /*private readonly List<InstancedSingleTextureShadedGroup> _instancedSingleTextureShadedGroups = new List<InstancedSingleTextureShadedGroup>();
        Dictionary<String, List<InstanceInfo>> _materialInstances = new Dictionary<String, List<InstanceInfo>>();
        Dictionary<Vector3i,Sector> sectors = new Dictionary<Vector3i, Sector>();
        private Ch0nk _ch0nk = new Ch0nk();*/
        private DrawableBuffer _buffer;

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

        public Ch0nkWorld(Game game)
        {

            /*for (int i = 0; i < 256; i++)
                for (int j = 0; j < 256; j++)
                    for (int k = 0; k < 256; k++)
                        _ch0nk.EightFoldTree[i, j, k] = MaterialType.Stone;*/
            //Console.WriteLine(Serialization.SerializeToByteArray(new EightFoldTree2()).Length);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            EightFoldTree tree = new EightFoldTree(64,MaterialType.Dirt);
            //EightFoldTree tree2 = new EightFoldTree(64, "Air");
            /* for(int i = 0; i < 64; i += 2)
            {
                for (int j = 0; j < 64; j+=2)
                {
                    for (int k = 0; k < 64; k += 2)
                    {
                        tree[i,j,k] = MaterialType.Sand;
                    }
                }
            }*/
            int count = 1000000000;
            byte[] stuff = new byte[count];
            for (int i = 0; i < count; i++)
            {
                stuff[i] = (byte)(i % 256);
            }


            Test[] t = new Test[count];
            for (int i = 0; i < count; i++)
            {
                t[i] = new Test((short)i,i,128);
            }
            

            Console.WriteLine("Time To Set Up: " + watch.ElapsedMilliseconds + "ms");
            watch.Restart();

            byte[] shortArray = Serialization.SerializeToByteArray(t);
            Console.WriteLine("Size: " + shortArray.Length/1024 + "Kbs");
            Console.WriteLine("Size of Unit: " + shortArray.Length/count + "bytes");
            Console.WriteLine("Marshall says: " + Marshal.SizeOf(typeof(byte[])));

            LZOCompressor compressor = new LZOCompressor();

            /*EightFoldTree tree = new EightFoldTree(128,MaterialType.Dirt);
            for (int i = 0; i < 128; i++)
                for (int j = 0; j < 128; j++)
                    for (int k = 0; k < 128; k++)
                        tree[i, j, k] = MaterialType.Stone;
            Console.WriteLine("Time to Create: " + watch.ElapsedMilliseconds + "ms");
            watch.Restart();
            

            byte[] treeArray = Serialization.SerializeToByteArray(tree);
            byte[] treeArrayCompressed = compressor.Compress(treeArray);
            Console.WriteLine("Time To Compress: " + watch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Uncompressed: " + treeArray.Length/1024 + "KB");
            Console.WriteLine("Compressed: " + treeArrayCompressed.Length / 1024 + "KB");*/


            /*
            Random random = new Random(1);
            short[] materials = new short[] { (short)MaterialType.Air, (short)MaterialType.Stone, (short)MaterialType.Dirt, (short)MaterialType.Sand };


            short[, ,] values = new short[128, 128, 256];
            for (int i = 0; i < 128; i++)
                for (int j = 0; j < 128; j++)
                    for (int k = 0; k < 256; k++)
                    {
                        int index = random.Next(materials.Length);
                        values[i, j, k] = materials[index];
                    }
                        


            Console.WriteLine("Time to Create: " + watch.ElapsedMilliseconds + "ms");
            watch.Restart();
            byte[] shortArray = Serialization.SerializeToByteArray(values);
            byte[] shortArrayCompressed = compressor.Compress(shortArray);
            Console.WriteLine("Time To Compress: " + watch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Uncompressed: " + shortArray.Length / 1024 + "KB");
            Console.WriteLine("Compressed: " + shortArrayCompressed.Length / 1024 + "KB");
            watch.Restart();
            byte[] shortArrayUnCompressed = compressor.Decompress(shortArrayCompressed);
            Console.WriteLine("Time To Decompress: " + watch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Uncompressed: " + shortArrayUnCompressed.Length / 1024 + "KB");*/

            /*Dictionary<Vector3i, MaterialType> dic = new Dictionary<Vector3i, MaterialType>();
            

            for (int i = 0; i < 128; i++)
                for (int j = 0; j < 128; j++)
                    for (int k = 0; k < 128; k++)
                        dic.Add(new Vector3i(i, j, k), MaterialType.Dirt);
            Console.WriteLine("Time to Create: " + watch.ElapsedMilliseconds + "ms");*/

            //PrepareCubes();



            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 10; i++)

                //for (int j = 0; j < 64; j++)
                    //for (int k = 0; k < 64; k++)
            {
                blocks.Add(new Block(new Vector3i(-5,5,0) + new Vector3i(i*10,0, 0),1,10));
            }

            _buffer = new DrawableBuffer(game, blocks);
        }

        /*private void PrepareCubes2(short[, ,] values)
        {
            FaceSet allFaces = new FaceSet();

             for (int i = 0; i < 128; i++)
                for (int j = 0; j < 128; j++)
                    for (int k = 0; k < 256; k++)
                    {
                        if(values[i,j,k] == (short)MaterialType.Air)
                            continue;

                        FaceSet cubeFaces = new Cube(1).Faces;
                        cubeFaces.UVMap(1, 1);
                        cubeFaces.Translate(new Vector3D(0.5f + i, 0.5f + j, -0.5f - k));

                        switch ((MaterialType)values[i,j,k])
                        {
                            case MaterialType.Dirt:
                                cubeFaces.SetMaterial(_assetManager.Materials["Grass"]);
                                break;
                            case MaterialType.Stone:
                                cubeFaces.SetMaterial(_assetManager.Materials["Stone"]);
                                break;
                            case MaterialType.Sand:
                                cubeFaces.SetMaterial(_assetManager.Materials["Sand"]);
                                break;
                        }

                        allFaces.Add(cubeFaces);
                    }

             _drawableGeometry = new DrawableGeometry(_renderManager, allFaces);
        }

        private void PrepareCubes()
        {
            List<Block> blocks = _ch0nk.GetAllBlocks();
            
            FaceSet allFaces = new FaceSet();

            foreach (Block block in blocks)
            {
                Console.WriteLine(block.Location + "-" + block.Size);
                if(block.MaterialType == MaterialType.Air)
                    continue;
                
                FaceSet cubeFaces = new Cube(block.Size).Faces;
                cubeFaces.UVMap(2, 2);
                cubeFaces.Translate(new Vector3D(block.Size / 2f + block.Location.X, block.Size/2f + block.Location.Y, -block.Size / 2f - block.Location.Z));
                //cubeFaces.Translate(new Vector3D(block.Size / 2f, block.Size / 2f, block.Size / 2f));

                switch (block.MaterialType)
                {
                    case MaterialType.Dirt:
                        cubeFaces.SetMaterial(_assetManager.Materials["Grass"]);
                        break;
                    case MaterialType.Stone:
                        cubeFaces.SetMaterial(_assetManager.Materials["Stone"]);
                        break;
                    case MaterialType.Sand:
                        cubeFaces.SetMaterial(_assetManager.Materials["Sand"]);
                        break;
                }

                allFaces.Add(cubeFaces);
            }

            _drawableGeometry = new DrawableGeometry(_renderManager,allFaces);
        }*/


        public void MouseClick(Camera camera)
        {
            Ray ray = new Ray(camera.Position, camera.Direction);
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
            _buffer.Draw(gameTime, Matrix.Identity, camera.ViewMatrix, camera.ProjectionMatrix);
        }
    }
}
