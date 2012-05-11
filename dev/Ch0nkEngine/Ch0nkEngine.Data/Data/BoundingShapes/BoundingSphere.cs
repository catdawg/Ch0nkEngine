using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data.BoundingShapes
{
    public class BoundingSphere : BoundingShape
    {
        private Vector3i _center;
        private int _radius;

        protected BoundingSphere(Vector3i center, int radius)
        {
            _center = center;
            _radius = radius;
        }

        public override bool Intersects(BoundingShape boundingShape)
        {
            if (boundingShape is BoundingSphere)
                return IntersectsSphere((BoundingSphere)boundingShape);

            return false;
        }

        public override bool Encloses(BoundingShape boundingShape)
        {
            return false;
        }

        private bool IntersectsSphere(BoundingSphere boundingSphere)
        {
            return _center.DistanceTo(boundingSphere.Center) < (_radius + boundingSphere.Radius);
        }

        public Vector3i Center
        {
            get { return _center; }
        }

        public int Radius
        {
            get { return _radius; }
        }
    }
}
