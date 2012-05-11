using System.Collections.Generic;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data;
using Ch0nkEngine.Data.Data.Dimensions;

namespace Ch0nkEngine.Data
{
    public class Realm
    {
        List<Dimension> _dimensions = new List<Dimension>();

        public Realm()
        {
            _dimensions.Add(new EarthDimension(this));
        }

        public List<Dimension> Dimensions
        {
            get { return _dimensions; }
        }
    }
}
