using System;

namespace Ch0nkEngine.Data.Data.Materials
{
    public class MaterialFactory
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
    }
}
