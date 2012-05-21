using System;
using System.Collections.Generic;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data.BoundingShapes;
using Ch0nkEngine.Data.Data.Materials;
using Ch0nkEngine.Data.Data.Materials.Types;

namespace Ch0nkEngine.Data.Data
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
        private IMaterial _material;
        //private MaterialType _materialType;
        private EightFoldTree[,,] _children;
        private byte _middle;  //min é 0

        /*public EightFoldTree(byte size, MaterialType materialType)
        {
            _middle = (byte)(size / 2);
            _materialType = materialType;
        }*/

        public EightFoldTree(byte size, IMaterial material)
        {
            _middle = (byte)(size / 2);
            _material = material;
        }

        #region Old, deprecated functions

        /*
        public EightFoldTree(short size, String materialType)
        {
            _middle = (short)(size / 2);
            _materialType = materialType;
        }

        public EightFoldTree(short size, short materialType)
        {
            _middle = (short)(size / 2);
            _materialType = materialType;
        }*/

        /*public void Expand()
        {
            _children = new EightFoldTree[2, 2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        _children[i, j, k] = new EightFoldTree(_middle, _materialType); //the middle of this tree is the size of a subtree
             
        }*/

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
        #endregion

        public IMaterial this[Vector3i vectorLocation]
        {
            get
            {
                if (_children == null || _middle == 0)
                    return _material;

                EightFoldTree childTree = _children[vectorLocation.X / _middle, vectorLocation.Y / _middle, vectorLocation.Z / _middle];
                if (childTree == null)
                    return _material;

                return childTree[vectorLocation.X % _middle, vectorLocation.Y % _middle, vectorLocation.Z % _middle];

                //Vector3i[] vectors = GetIndexAndLocation(vectorLocation);

                //return _children[vectors[0].X, vectors[0].Y, vectors[0].Z][vectors[1]];
            }
            set
            {
                if (Size == 1)
                    _material = value;
                else
                {
                    //if there are no children, expand now
                    if (_children == null)
                        //Expand();
                        _children = new EightFoldTree[2, 2, 2];

                    //determine the child tree index
                    Vector3i location = new Vector3i(vectorLocation.X / _middle, vectorLocation.Y / _middle, vectorLocation.Z / _middle);

                    //if that index has not yet been allocated, do it now
                    if (_children[location.X, location.Y, location.Z] == null)
                        _children[location.X, location.Y, location.Z] = new EightFoldTree(_middle, _material);

                    _children[location.X, location.Y, location.Z][vectorLocation.X % _middle, vectorLocation.Y % _middle, vectorLocation.Z % _middle] = value;

                    //Vector3i[] vectors = GetIndexAndLocation(vectorLocation);
                    //_children[vectors[0].X, vectors[0].Y, vectors[0].Z][vectors[1]] = value;
                }
            }
        }

        public IMaterial this[int x, int y, int z]
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
        /// <param name="ch0Nk"></param>
        /// <param name="startLocation"></param>
        /// <returns></returns>
        public List<Block> GetAllBlocks(Ch0nk ch0Nk, Vector3b startLocation)
        {
                //if there are no children, this block is returned
            if (_children == null)
                //if (_material is AirMaterial)
                //    return new List<Block>(1);
                //else
                    return new List<Block>(new[] { new Block(ch0Nk, startLocation, _material, Size) });

            //otherwise, iterate over all the children
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        if (_children[i, j, k] == null)
                            blocks.Add(new Block(ch0Nk, startLocation + new Vector3b(i * _middle, j * _middle, k * _middle), _material, _middle));
                        else
                            blocks.AddRange(_children[i, j, k].GetAllBlocks(ch0Nk, startLocation + new Vector3b(i * _middle, j * _middle, k * _middle)));
                        //else if (!(_material is AirMaterial))
                            
                    }
                        
                        

            return blocks;
        }

        public void ChangeMaterial(BoundingShape boundingShape, IMaterial material, Ch0nk ch0Nk, Vector3b startLocation)
        {
            Vector3i treeAbsolutePosition = ch0Nk.Position + startLocation;
            BoundingCube treeBoundingBox = new BoundingCube(treeAbsolutePosition, Size);

            //if (_children == null && boundingShape.Encloses(treeBoundingBox) && Size > 1)
                //Console.WriteLine("GOT IT");

            if ((Size == 1) || (_children == null && boundingShape.Encloses(treeBoundingBox)))
            {
                _material = material;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                        for (int k = 0; k < 2; k++)
                        {
                            Vector3i childAbsolutePosition = ch0Nk.Position + startLocation + new Vector3b(i * _middle, j * _middle, k * _middle);
                            BoundingCube childBoundingBox = new BoundingCube(childAbsolutePosition, _middle);
                            if (childBoundingBox.Intersects(boundingShape))
                            {
                                if (_children == null)
                                    _children = new EightFoldTree[2, 2, 2];

                                if (_children[i, j, k] == null)
                                    _children[i, j, k] = new EightFoldTree(_middle, _material);

                                _children[i, j, k].ChangeMaterial(boundingShape, material, ch0Nk, startLocation + new Vector3b(i * _middle, j * _middle, k * _middle));
                            }
                        }
            }
        }

        /// <summary>
        /// Gets all the blocks contained in this tree and the subtrees
        /// </summary>
        /// <param name="startLocation"></param>
        /// <returns></returns>
        /*public List<Block> GetAllBlocks(Vector3i startLocation)
        {
            //if there are no children, this block is returned
            if (_children == null)
                return new List<Block>(new[] { MaterialFactory.CreateBlock(startLocation,) });
            var asr = new DirtBlock(new Vector3i()) { Size = 2 };
            


            //otherwise, iterate over all the children
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        blocks.AddRange(_children[i, j, k].GetAllBlocks(startLocation + new Vector3i(i * _middle, j * _middle, k * _middle)));

            return blocks;
        }*/

        private void CleanUp()
        {
            //if (_children == null)
            //    return _material;

            IMaterial commonMaterial = null;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        var child = _children[i, j, k];

                        if (child != null)
                        {
                            child.CleanUp();
                            
                            if(child._children == null && child._material == _material)
                            {
                                _children[i, j, k] = null;
                            }
                            else
                            {
                                commonMaterial = child._material;
                            }
                        }
                    }
        }

        /// <summary>
        /// The cube dimensions of the tree.
        /// 1 is the smallest dimension possible.
        /// </summary>
        public byte Size
        {
            get { return (byte)(_middle > 0 ? (_middle * 2) : 1); }
        }

        
    }
}
