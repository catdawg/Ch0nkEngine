
using System;

namespace Ch0nkEngine.Data
{
    [Serializable]
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

        public static Vector3i operator *(Vector3i a, int scalar)
        {
            return new Vector3i(a._x * scalar, a._y * scalar, a._z * scalar);
        }

        public static Vector3i operator /(Vector3i a, int scalar)
        {
            return new Vector3i(a._x / scalar, a._y / scalar, a._z / scalar);
        }

        public static Vector3i operator %(Vector3i a, int scalar)
        {
            return new Vector3i(a._x % scalar, a._y % scalar, a._z % scalar);
        }

        public static Vector3i operator +(Vector3i a, int scalar)
        {
            return new Vector3i(a._x + scalar, a._y + scalar, a._z + scalar);
        }

        public static Vector3i operator -(Vector3i a, int scalar)
        {
            return new Vector3i(a._x - scalar, a._y - scalar, a._z - scalar);
        }

        public static Vector3i operator -(Vector3i a)
        {
            return new Vector3i(-a._x, -a._y, -a._z);
        }

        public static Vector3i operator *(Vector3i a, Vector3i b)
        {
            return new Vector3i(a._x * b._x, a._y * b._y, a._z * b._z);
        }

        public static Vector3i operator /(Vector3i a, Vector3i b)
        {
            return new Vector3i(a._x / b._x, a._y / b._y, a._z / b._z);
        }

        public static Vector3i operator +(Vector3i a, Vector3i b)
        {
            return new Vector3i(a._x + b._x, a._y + b._y, a._z + b._z);
        }

        public static Vector3i operator -(Vector3i a, Vector3i b)
        {
            return new Vector3i(a._x - b._x, a._y - b._y, a._z - b._z);
        }

        public static Vector3i operator %(Vector3i a, Vector3i b)
        {
            return new Vector3i(a._x % b._x, a._y % b._y, a._z % b._z);
        }

        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }
    }
}
