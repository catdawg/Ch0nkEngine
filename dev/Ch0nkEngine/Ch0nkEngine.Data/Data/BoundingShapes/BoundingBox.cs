using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data.BoundingShapes
{
    public class BoundingBox : BoundingShape
    {
        private readonly Vector3i _min;
        private readonly Vector3i _max;

        public BoundingBox(Vector3i min, Vector3i max)
        {
            _min = min;
            _max = max;
        }

        public BoundingBox(Vector3i min, int size)
        {
            _min = min;
            _max = min + size;
        }

        public override bool Encloses(BoundingShape boundingShape)
        {
            if (boundingShape is BoundingBox)
                return EnclosesBox((BoundingBox)boundingShape);

            return false;
        }

        private bool EnclosesBox(BoundingBox boundingBox)
        {
            return (_min.X <= boundingBox._min.X) && (_max.X >= boundingBox._max.X) &&
                    (_min.Y <= boundingBox._min.Y) && (_max.Y >= boundingBox._max.Y) &&
                    (_min.Z <= boundingBox._min.Z) && (_max.Z >= boundingBox._max.Z);
        }

        public override bool Intersects(BoundingShape boundingShape)
        {
            if (boundingShape is BoundingSphere)
                return IntersectsSphere((BoundingSphere) boundingShape);
            else if (boundingShape is BoundingBox)
                return IntersectsBox((BoundingBox)boundingShape);

            return false;
        }

        private bool IntersectsBox(BoundingBox boundingBox)
        {
            return (_min.X < boundingBox._max.X) && (_max.X > boundingBox._min.X) &&
                    (_min.Y < boundingBox._max.Y) && (_max.Y > boundingBox._min.Y) &&
                    (_min.Z < boundingBox._max.Z) && (_max.Z > boundingBox._min.Z);
        }

        private bool IntersectsSphere(BoundingSphere boundingSphere)
        {
            //if (Center.DistanceTo(boundingSphere.Center) < (_size / 2 + boundingSphere.Radius))
            //    return true;

            return false;
        }

        /*public Vector3i Center
        {
            get { return _min + Size/2; }
        }*/

        public Vector3i Min
        {
            get { return _min; }
        }

        public Vector3i Max
        {
            get { return _max; }
        }
    }
}
