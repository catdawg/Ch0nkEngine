using System.Collections.Generic;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data.BoundingShapes;
using Ch0nkEngine.Data.Data.Materials;

namespace Ch0nkEngine.Data.Data
{
    public abstract class Dimension
    {
        /// <summary>
        /// Realm it belongs to
        /// </summary>
        protected Realm realm;

        /// <summary>
        /// List of ch0nks contained in this dimension
        /// </summary>
        protected Dictionary<Vector3i, Ch0nk> _chonks = new Dictionary<Vector3i, Ch0nk>();

        protected Dimension(Realm realm)
        {
            this.realm = realm;
            Initialize();
        }

        /// <summary>
        /// Generates the dimension for the first time
        /// </summary>
        protected abstract void Initialize();


        /// <summary>
        /// Gets a list with all contained blocks within this dimension.
        /// </summary>
        /// <returns></returns>
        public List<Block> GetAllBlocks()
        {
            List<Block> allBlocks = new List<Block>();

            foreach (Ch0nk ch0Nk in _chonks.Values)
            {
                allBlocks.AddRange(ch0Nk.GetAllBlocks());
            }

            return allBlocks;
        }

        public void ChangeMaterial(BoundingShape boundingShape, IMaterial material)
        {
            //ONLY FOR NOW: check all, but it can be optimized
            foreach (Ch0nk ch0Nk in _chonks.Values)
            {
                //if the ch0nk bounding box intersects, then go deeper
                if (ch0Nk.BoundingCube.Intersects(boundingShape))
                    ch0Nk.ChangeMaterial(boundingShape, material);
            }
        }
    }
}
