using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data.BoundingShapes
{
    public class BoundingCube : BoundingBox
    {
        public BoundingCube(Vector3i min, int size) 
            : base(min, size)
        {
        }

        protected override bool IntersectsSphere(BoundingSphere boundingSphere)
        {
            return DoesCubeIntersectSphere(_min, _max, boundingSphere.Center, boundingSphere.Radius);
        }

        private static float Squared(float v) { return v * v; }


        private static bool DoesCubeIntersectSphere(Vector3i min, Vector3i max, Vector3i center, float radius)
        {
            float dist_squared = radius * radius;
            /* assume C1 and C2 are element-wise sorted, if not, do that now */
            if (center.X < min.X) dist_squared -= Squared(center.X - min.X);
            else if (center.X > max.X) dist_squared -= Squared(center.X - max.X);
            if (center.Y < min.Y) dist_squared -= Squared(center.Y - min.Y);
            else if (center.Y > max.Y) dist_squared -= Squared(center.Y - max.Y);
            if (center.Z < min.Z) dist_squared -= Squared(center.Z - min.Z);
            else if (center.Z > max.Z) dist_squared -= Squared(center.Z - max.Z);
            return dist_squared > 0;
        }
    }
}
