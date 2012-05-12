using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data.BoundingShapes
{
    public class BoundingSphere : BoundingShape
    {
        private Vector3i _center;
        private int _radius;

        public BoundingSphere(Vector3i center, int radius)
        {
            _center = center;
            _radius = radius;
        }

        #region Intersects

        public override bool Intersects(BoundingShape boundingShape)
        {
            if (boundingShape is BoundingSphere)
                return IntersectsSphere((BoundingSphere)boundingShape);
            else if (boundingShape is BoundingCube)
                return IntersectsCube((BoundingCube)boundingShape);

            return false;
        }

        private bool IntersectsCube(BoundingCube boundingCube)
        {
            return boundingCube.Intersects(this);
        }

        private bool IntersectsSphere(BoundingSphere boundingSphere)
        {
            return _center.DistanceTo(boundingSphere.Center) < (_radius + boundingSphere.Radius);
        }

        #endregion

        #region Encloses

        public override bool Encloses(BoundingShape boundingShape)
        {
            if (boundingShape is BoundingSphere)
                return EnclosesSphere((BoundingSphere)boundingShape);
            if (boundingShape is BoundingBox)
                return EnclosesBox((BoundingBox) boundingShape);

            return false;
        }

        private bool EnclosesBox(BoundingBox boundingBox)
        {
            return boundingBox.Min.DistanceTo(_center) < _radius && boundingBox.Max.DistanceTo(_center) < _radius;
        }

        private bool EnclosesSphere(BoundingSphere boundingSphere)
        {
            float distance = _center.DistanceTo(boundingSphere.Center);

            return distance < _radius && (_radius - distance) > boundingSphere.Radius;
        }

        #endregion

        

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
