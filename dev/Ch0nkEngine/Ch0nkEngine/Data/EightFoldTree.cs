namespace Ch0nkEngine.Data
{
    public class EightFoldTree
    {
        private MaterialType _materialType;
        public EightFoldTree[,,] _children;
        public short _middle = 256;  //min é 0

        public EightFoldTree(short middle, MaterialType materialType)
        {
            _middle = middle;
            _materialType = materialType;
        }

        public void Expand()
        {
            _children = new EightFoldTree[2, 2, 2];
            for(int i = 0; i < 2; i++)
                for(int j = 0; j < 2; j++)
                    for(int k = 0; k < 2; k++)
                        _children[i, j, k] = new EightFoldTree((short)(_middle / 2), _materialType);
        }

        private Vector3i[] GetIndexAndLocation(Vector3i vectorLocation)
        {
            Vector3i vectorIndices = new Vector3i(0, 0, 0);
            Vector3i vectorDeeperLocation = vectorLocation;

            //first for x
            if (vectorLocation.X >= _middle)
            {
                vectorDeeperLocation.X = vectorLocation.X - _middle;
                vectorIndices.X = 1;
            }

            //then for Y
            if (vectorLocation.Y >= _middle)
            {
                vectorDeeperLocation.Y = vectorLocation.Y - _middle;
                vectorIndices.Y = 1;
            }

            //lastly, for Z
            if (vectorLocation.Z >= _middle)
            {
                vectorDeeperLocation.Z = vectorLocation.Z - _middle;
                vectorIndices.Z = 1;
            }

            return new[] {vectorIndices, vectorLocation};
        }

        public MaterialType this[Vector3i vectorLocation]
        {
            get
            {
                if (_children == null || _middle == 0)
                    return _materialType;

                Vector3i[] vectors = GetIndexAndLocation(vectorLocation);

                return _children[vectors[0].X, vectors[0].Y, vectors[0].Z][vectors[1]];
            }
            set
            {
                if (_middle == 0)
                    _materialType = value;
                else
                {
                    //if there are no children, expand now
                    if (_children == null)
                        Expand();

                    Vector3i[] vectors = GetIndexAndLocation(vectorLocation);

                    _children[vectors[0].X, vectors[0].Y, vectors[0].Z][vectors[1]] = value;
                }
            }
        }

        public MaterialType this[int x, int y, int z]
        {
            get { return this[new Vector3i(x, y, z)]; }
            set { this[new Vector3i(x, y, z)] = value; }
        }

        public int Size
        {
            get { return _middle*2; }
        }
    }
}
