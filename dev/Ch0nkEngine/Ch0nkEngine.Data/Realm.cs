using System.Collections.Generic;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data;
using Ch0nkEngine.Data.Dimensions;

namespace Ch0nkEngine.Data
{
    public class Realm
    {
        Dictionary<Vector4i,Ch0nk> _chonks = new Dictionary<Vector4i, Ch0nk>();

        List<Dimension> _dimensions = new List<Dimension>();

        public Realm()
        {
            _dimensions.Add(new EarthDimension());
        }


    }
}
