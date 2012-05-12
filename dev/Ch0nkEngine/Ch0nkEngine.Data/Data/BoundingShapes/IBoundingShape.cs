using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch0nkEngine.Data.Data.BoundingShapes
{
    public interface IBoundingShape
    {
        bool Intersects(BoundingSphere boundingSphere);

        bool Intersects(BoundingBox boundingBox);

        bool Intersects(BoundingCylinder boundingCylinder);

        bool Encloses(BoundingShape boundingShape);
    }
}
