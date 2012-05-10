using System;
using System.Collections.Generic;
using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data
{
    public class Dimension
    {
        protected String _name;
        protected Dictionary<Vector3i, Ch0nk> _chonks = new Dictionary<Vector3i, Ch0nk>();


    }
}
