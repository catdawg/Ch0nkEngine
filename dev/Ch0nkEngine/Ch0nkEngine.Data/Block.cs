namespace Ch0nkEngine.Data
{
    public struct Block
    {
        public Vector3i Location;
        public short MaterialType;
        public short Size;

        public Block(Vector3i location, short materialType, short size)
        {
            Location = location;
            MaterialType = materialType;
            Size = size;
        }
    }
}
