using System;

namespace Ch0nkEngine.Data
{
    public class MaterialTranslator
    {
        public static String GetMaterialName(short material)
        {
            switch (material)
            {
                case 1:
                    return "Window";
                case 2:
                    return "Water";
                case 3:
                    return "Sand";
                case 4:
                    return "Glass";
                case 5:
                    return "Gravel";
                case 6:
                    return "Brick";
                default:
                    return "Grass";
            }
        }

        public static short GetMaterialShortCode(MaterialType material)
        {
            switch (material)
            {
                case MaterialType.Air:
                    return 0;
                case MaterialType.Dirt:
                    return 1;
                case MaterialType.Stone:
                    return 2;
                case MaterialType.Sand:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException("material");
            }
        }
    }
}
