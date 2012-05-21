using System;
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
using SXL.Cameras;
using Simplicit.Net.Lzo;
using BoundingBox = Ch0nkEngine.Data.Data.BoundingShapes.BoundingBox;
using BoundingSphere = Ch0nkEngine.Data.Data.BoundingShapes.BoundingSphere;

namespace Ch0nkEngine.XNAViewer
{
    public class Ch0nkWorld
    {
        /*private readonly List<InstancedSingleTextureShadedGroup> _instancedSingleTextureShadedGroups = new List<InstancedSingleTextureShadedGroup>();
        Dictionary<String, List<InstanceInfo>> _materialInstances = new Dictionary<String, List<InstanceInfo>>();
        Dictionary<Vector3i,Sector> sectors = new Dictionary<Vector3i, Sector>();
        private Ch0nk _ch0nk = new Ch0nk();*/
        private DrawableBuffer _buffer;
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

            //_dimension.GenerateAt(new Vector3i(0, 0, 0));

            _game = game;

            List<Block> blocks = _dimension.GetRandomTestingBlocks(new Vector3i());

            //new BoundingBox(new Vector3i(0,0,10),32)
            //_realm.Dimensions[0].ChangeMaterial(new BoundingSphere(new Vector3i(32,32,64), 20), new AirMaterial());
            //_realm.Dimensions[0].ChangeMaterial(new BoundingSphere(new Vector3i(0, 32, 64), 20), new SandMaterial());
            //_realm.Dimensions[0].ChangeMaterial(new BoundingSphere(new Vector3i(0, 64, 32), 20), new StoneMaterial());

            //blocks = new List<Block>();
            //blocks.Add(new Block(c, new Vector3b(0, 0, 0), new GrassMaterial(), 1));
            //blocks = _dimension.GetAllBlocks();
            
            _buffer = new DrawableBuffer(game, blocks);
            
        }


        public void MouseClick(Camera camera)
        {
            //Ray ray = new Ray(camera.Position, camera.Direction);
            _dimension.GenerateAt(new Vector3i((int)camera.Position.X, (int)camera.Position.Y, (int)camera.Position.Z));

            List<Block> blocks = _dimension.GetAllBlocks();

            _buffer = new DrawableBuffer(_game, blocks);
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
