using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ch0nkEngine.Data
{
    public class LocalizedEightFoldTree
    {
        public Vector3i Location;
        public EightFoldTree Tree;

        public LocalizedEightFoldTree(Vector3i location, EightFoldTree tree)
        {
            Location = location;
            Tree = tree;
        }

        public List<Block> GetAllBlocks()
        {
            //return Tree.GetAllBlocks(Location);
            return null;
        }
    }


    [Serializable]
    public class EightFoldTree
    {
        private MaterialType _materialType;
        public EightFoldTree[,,] _children;
        public short _middle = 256;  //min é 0

        public EightFoldTree(short size, MaterialType materialType)
        {
            _middle = (short)(size / 2);
            _materialType = materialType;
        }

        public void Expand()
        {
            _children = new EightFoldTree[2, 2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        _children[i, j, k] = new EightFoldTree(_middle, _materialType); //the middle of this tree is the size of a subtree
             
        }

        /*private Vector3i[] GetIndexAndLocation(Vector3i vectorLocation)
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

            return new[] { vectorIndices, vectorDeeperLocation };
        }*/

        public MaterialType this[Vector3i vectorLocation]
        {
            get
            {
                if (_children == null || _middle == 0)
                    return _materialType;

                EightFoldTree childTree = _children[vectorLocation.X / _middle, vectorLocation.Y / _middle, vectorLocation.Z / _middle];
                if (childTree == null)
                    return _materialType;

                return childTree[vectorLocation.X % _middle, vectorLocation.Y % _middle, vectorLocation.Z % _middle];

                //Vector3i[] vectors = GetIndexAndLocation(vectorLocation);

                //return _children[vectors[0].X, vectors[0].Y, vectors[0].Z][vectors[1]];
            }
            set
            {
                if (_middle == 0)
                    _materialType = value;
                else
                {
                    //if there are no children, expand now
                    if (_children == null)
                        //Expand();
                        _children = new EightFoldTree[2, 2, 2];

                    Vector3i location = new Vector3i(vectorLocation.X / _middle, vectorLocation.Y / _middle, vectorLocation.Z / _middle);

                    if (_children[location.X, location.Y, location.Z] == null)
                        _children[location.X, location.Y, location.Z] = new EightFoldTree(_middle, _materialType);

                    _children[location.X, location.Y, location.Z][vectorLocation.X % _middle, vectorLocation.Y % _middle, vectorLocation.Z % _middle] = value;

                    //Vector3i[] vectors = GetIndexAndLocation(vectorLocation);
                    //_children[vectors[0].X, vectors[0].Y, vectors[0].Z][vectors[1]] = value;
                }
            }
        }

        public MaterialType this[int x, int y, int z]
        {
            get { return this[new Vector3i(x, y, z)]; }
            set { this[new Vector3i(x, y, z)] = value; }
        }




        /// <summary>
        /// Returns all trees with a certain size
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<LocalizedEightFoldTree> GetLocalizedTreesOfSize(Vector3i startLocation, short size)
        {
            if (Size == size || _children == null)
                return new List<LocalizedEightFoldTree>(new[] { new LocalizedEightFoldTree(startLocation, this) });

            //otherwise, iterate over all the children
            List<LocalizedEightFoldTree> eightFoldTrees = new List<LocalizedEightFoldTree>();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        eightFoldTrees.AddRange(_children[i, j, k].GetLocalizedTreesOfSize(startLocation + new Vector3i(i * _middle, j * _middle, k * _middle), size));

            return eightFoldTrees;
        }

        /// <summary>
        /// Gets all the blocks contained in this tree and the subtrees
        /// </summary>
        /// <param name="startLocation"></param>
        /// <returns></returns>
        public List<Block> GetAllBlocks(Vector3i startLocation)
        {
            //if there are no children, this block is returned
            if (_children == null)
                return new List<Block>(new[] { new Block(startLocation, MaterialTranslator.GetMaterialShortCode(_materialType), Size) });

            //otherwise, iterate over all the children
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        blocks.AddRange(_children[i, j, k].GetAllBlocks(startLocation + new Vector3i(i * _middle, j * _middle, k * _middle)));

            return blocks;
        }

        /// <summary>
        /// The cube dimensions of the tree.
        /// 1 is the smallest dimension possible.
        /// </summary>
        public short Size
        {
            get { return (short)(_middle > 0 ? (_middle * 2) : 1); }
        }
    }
}
