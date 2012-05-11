
using System;

namespace Ch0nkEngine.Data.Basic
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

        public static Vector3i operator +(Vector3i a, Vector3b b)
        {
            return new Vector3i(a._x + b.X, a._y + b.Y, a._z + b.Z);
        }

        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }

        public float Length
        {
            get { return (float)Math.Sqrt(_x * _x + _y * _y + _z * _z); }
        }

        public float DistanceTo(Vector3i b)
        {
            float dx = _x - b._x;
            float dy = _y - b._y;
            float dz = _z - b._z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static float Distance(Vector3i a, Vector3i b)
        {
            float dx = a._x - b._x;
            float dy = a._y - b._y;
            float dz = a._z - b._z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
    }
}
