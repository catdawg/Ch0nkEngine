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

        public void GenerateAt(Vector3i center)
        {
            Vector3i centerChonkLocation = GetLowestEnclosingCh0nkLocation(center);
            //Vector3i centerChonkLocation = new Vector3i(center.X / Ch0nk.Size, center.Y / Ch0nk.Size, center.Y / Ch0nk.Size);

            for(int i=-1; i <= 1;i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        Vector3i neighbourCh0Nk = new Vector3i(centerChonkLocation.X + i * Ch0nk.Size, centerChonkLocation.Y + j * Ch0nk.Size, centerChonkLocation.Z + k * Ch0nk.Size);
                        if (!_chonks.ContainsKey(neighbourCh0Nk))
                            GenerateBlock(neighbourCh0Nk);
                    }
                }
            }
                    

        }

        private Vector3i GetLowestEnclosingCh0nkLocation(Vector3i center)
        {
            int x = ((center.X / Ch0nk.Size)) * Ch0nk.Size;
            int y = ((center.Y / Ch0nk.Size)) * Ch0nk.Size;
            int z = ((center.Z / Ch0nk.Size)) * Ch0nk.Size;

            return new Vector3i(x <= center.X ? x : x - Ch0nk.Size, y <= center.Y ? y : y - Ch0nk.Size, z <= center.Z ? z : z - Ch0nk.Size);
        }

        protected abstract void GenerateBlock(Vector3i startLocation);
    }
}
