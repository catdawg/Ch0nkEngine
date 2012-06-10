using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch0nkEngine.Data.Data.Materials.Types
{
    public class SandMaterial : IMaterial
    {
        public string MaterialName
        {
            get { return "Sand"; }
        }

        public short MaterialCode
        {
            get { return 2; }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is IMaterial)
            {
                return ((IMaterial)obj).MaterialName == MaterialName;
            }

            return false;
        }
    }
}
