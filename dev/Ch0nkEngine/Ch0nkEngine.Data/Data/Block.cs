using System;
using System.Collections;
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
            set { size = value; }  //TEMPORARY
        }

        public Ch0nk Ch0Nk
        {
            get { return ch0Nk; }
        }

        public IMaterial Material
        {
            get { return material; }
        }

        public BitArray ViewFacesArray = new BitArray(6,false);
        public byte ViewFaces = 0;


        public bool HasTop
        {
            get { return ViewFacesArray.Get(5); }
        }

        public bool HasBottom
        {
            get { return ViewFacesArray.Get(4); }
        }

        public bool HasLeft
        {
            get { return ViewFacesArray.Get(3); }
        }

        public bool HasRight
        {
            get { return ViewFacesArray.Get(2); }
        }

        public bool HasFront
        {
            get { return ViewFacesArray.Get(1); }
        }

        public bool HasBack
        {
            get { return ViewFacesArray.Get(0); }
        }

        /*public bool HasTop
        {
            get { return (ViewFaces & 0x20) > 0; }
        }

        public bool HasBottom
        {
            get { return (ViewFaces & 0x10) > 0; }
        }

        public bool HasLeft
        {
            get { return (ViewFaces & 0x08) > 0; }
        }

        public bool HasRight
        {
            get { return (ViewFaces & 0x04) > 0; }
        }

        public bool HasFront
        {
            get { return (ViewFaces & 0x02) > 0; }
        }

        public bool HasBack
        {
            get { return (ViewFaces & 0x01) > 0; }
        }*/
    }
}
