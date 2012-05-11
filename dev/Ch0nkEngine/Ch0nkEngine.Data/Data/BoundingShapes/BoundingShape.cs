using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data.BoundingShapes
{
    public abstract class BoundingShape
    {
        public abstract bool Intersects(BoundingShape boundingShape);

        public abstract bool Encloses(BoundingShape boundingShape);
    }
}
