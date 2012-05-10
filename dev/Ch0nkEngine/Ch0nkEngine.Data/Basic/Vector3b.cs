
using System;

namespace Ch0nkEngine.Data.Basic
{
    [Serializable]
    public struct Vector3b
    {
        private byte _x;
        private byte _y;
        private byte _z;

        public Vector3b(byte x, byte y)
        {
            _x = x;
            _y = y;
            _z = 0;
        }

        public Vector3b(byte x, byte y, byte z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3b(int x, int y)
        {
            _x = (byte)x;
            _y = (byte)y;
            _z = 0;
        }

        public Vector3b(int x, int y, int z)
        {
            _x = (byte)x;
            _y = (byte)y;
            _z = (byte)z;
        }

        public byte X
        {
            get { return _x; }
            set { _x = value; }
        }

        public byte Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public byte Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public static Vector3b operator *(Vector3b a, int scalar)
        {
            return new Vector3b(a._x * scalar, a._y * scalar, a._z * scalar);
        }

        public static Vector3b operator /(Vector3b a, int scalar)
        {
            return new Vector3b(a._x / scalar, a._y / scalar, a._z / scalar);
        }

        public static Vector3b operator %(Vector3b a, int scalar)
        {
            return new Vector3b(a._x % scalar, a._y % scalar, a._z % scalar);
        }

        public static Vector3b operator +(Vector3b a, int scalar)
        {
            return new Vector3b(a._x + scalar, a._y + scalar, a._z + scalar);
        }

        public static Vector3b operator -(Vector3b a, int scalar)
        {
            return new Vector3b(a._x - scalar, a._y - scalar, a._z - scalar);
        }

        public static Vector3b operator -(Vector3b a)
        {
            return new Vector3b(-a._x, -a._y, -a._z);
        }

        public static Vector3b operator *(Vector3b a, Vector3b b)
        {
            return new Vector3b(a._x * b._x, a._y * b._y, a._z * b._z);
        }

        public static Vector3b operator /(Vector3b a, Vector3b b)
        {
            return new Vector3b(a._x / b._x, a._y / b._y, a._z / b._z);
        }

        public static Vector3b operator +(Vector3b a, Vector3b b)
        {
            return new Vector3b(a._x + b._x, a._y + b._y, a._z + b._z);
        }

        public static Vector3b operator -(Vector3b a, Vector3b b)
        {
            return new Vector3b(a._x - b._x, a._y - b._y, a._z - b._z);
        }

        public static Vector3b operator %(Vector3b a, Vector3b b)
        {
            return new Vector3b(a._x % b._x, a._y % b._y, a._z % b._z);
        }

        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }
    }
}
