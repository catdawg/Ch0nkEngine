using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data
{
    public struct Block
    {
        public Ch0nk Ch0nk;
        public Vector3i Location;
        public short MaterialType;
        public short Size;

        public Block(Vector3i location, short materialType, short size)
        {
            Location = location;
            MaterialType = materialType;
            Size = size;
        }

        public Vector4i
    }
}
