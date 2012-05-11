using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data;
using Ch0nkEngine.Data.Data.Materials.Types;
using Ch0nkEngine.Data.Utils;
using Ch0nkEngine.XNAViewer.Drawing;
using Microsoft.Xna.Framework;
using SXL.Cameras;
using Simplicit.Net.Lzo;
using BoundingBox = Ch0nkEngine.Data.Data.BoundingShapes.BoundingBox;

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

        public Ch0nkWorld(Game game)
        {
            _realm = new Realm();

            _realm.Dimensions[0].ChangeMaterial(new BoundingBox(new Vector3i(0,0,10),32), new SandMaterial());

            List<Block> blocks = _realm.Dimensions[0].GetAllBlocks();

            
            _buffer = new DrawableBuffer(game, blocks);
        }


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
