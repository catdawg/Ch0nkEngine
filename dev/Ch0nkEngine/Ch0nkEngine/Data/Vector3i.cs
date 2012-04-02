using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch0nkEngine.Data
{
    public struct Vector3i
    {
        private int _x;
        private int _y;
        private int _z;

        public Vector3i(int x, int y)
        {
            _x = x;
            _y = y;
            _z = 0;
        }

        public Vector3i(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Z
        {
            get { return _z; }
            set { _z = value; }
        }
    }
}
