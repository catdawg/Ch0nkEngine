using System;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data.Materials;

namespace Ch0nkEngine.Data.Data
{
    public class Block
    {
        protected Ch0nk ch0Nk;
        protected Vector3b relativePosition;
        protected byte size;
        protected IMaterial material;

        public Block(Ch0nk ch0Nk, Vector3b relativePosition, IMaterial material, byte size)
        {
            this.ch0Nk = ch0Nk;
            this.relativePosition = relativePosition;
            this.material = material;
            this.size = size;
        }

        public Vector3i AbsolutePosition
        {
            get { return new Vector3i(ch0Nk.Position.X + relativePosition.X, ch0Nk.Position.Y + relativePosition.Y, ch0Nk.Position.Z + relativePosition.Z); }
        }

        public Vector4i AbsoluteRealmLocation
        {
            get { return new Vector4i(); }
        }

        public Vector3b RelativePosition
        {
            get { return relativePosition; }
        }

        public byte Size
        {
            get { return size; }
        }

        public Ch0nk Ch0Nk
        {
            get { return ch0Nk; }
        }

        public IMaterial Material
        {
            get { return material; }
        }
    }
}
