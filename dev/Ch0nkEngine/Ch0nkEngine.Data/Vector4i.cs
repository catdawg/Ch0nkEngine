
using System;

namespace Ch0nkEngine.Data
{
    [Serializable]
    public struct Vector4i
    {
        private int _x;
        private int _y;
        private int _z;
        private int _w;

        public Vector4i(int x, int y)
        {
            _x = x;
            _y = y;
            _z = 0;
            _w = 0;
        }

        public Vector4i(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = 0;
        }

        public Vector4i(int x, int y, int z, int w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = 0;
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

        public int W
        {
            get { return _w; }
            set { _w = value; }
        }

        public static Vector4i operator *(Vector4i a, int scalar)
        {
            return new Vector4i(a._x * scalar, a._y * scalar, a._z * scalar);
        }

        public static Vector4i operator /(Vector4i a, int scalar)
        {
            return new Vector4i(a._x / scalar, a._y / scalar, a._z / scalar);
        }

        public static Vector4i operator %(Vector4i a, int scalar)
        {
            return new Vector4i(a._x % scalar, a._y % scalar, a._z % scalar);
        }

        public static Vector4i operator +(Vector4i a, int scalar)
        {
            return new Vector4i(a._x + scalar, a._y + scalar, a._z + scalar);
        }

        public static Vector4i operator -(Vector4i a, int scalar)
        {
            return new Vector4i(a._x - scalar, a._y - scalar, a._z - scalar);
        }

        public static Vector4i operator -(Vector4i a)
        {
            return new Vector4i(-a._x, -a._y, -a._z);
        }

        public static Vector4i operator *(Vector4i a, Vector4i b)
        {
            return new Vector4i(a._x * b._x, a._y * b._y, a._z * b._z);
        }

        public static Vector4i operator /(Vector4i a, Vector4i b)
        {
            return new Vector4i(a._x / b._x, a._y / b._y, a._z / b._z);
        }

        public static Vector4i operator +(Vector4i a, Vector4i b)
        {
            return new Vector4i(a._x + b._x, a._y + b._y, a._z + b._z);
        }

        public static Vector4i operator -(Vector4i a, Vector4i b)
        {
            return new Vector4i(a._x - b._x, a._y - b._y, a._z - b._z);
        }

        public static Vector4i operator %(Vector4i a, Vector4i b)
        {
            return new Vector4i(a._x % b._x, a._y % b._y, a._z % b._z);
        }

        public override string ToString()
        {
            return X + "," + Y + "," + Z + "," + W;
        }
    }
}
