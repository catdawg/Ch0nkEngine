using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Basic;
using Microsoft.Xna.Framework;

namespace Ch0nkEngine.XNAViewer.Drawing
{
    public static class Utils
    {
        public static Vector3 ToVector3(this Vector3i vector3i)
        {
            return new Vector3(vector3i.X, vector3i.Y, vector3i.Z);
        }
    }
}
