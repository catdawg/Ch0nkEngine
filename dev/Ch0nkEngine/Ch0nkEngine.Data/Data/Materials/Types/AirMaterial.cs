namespace Ch0nkEngine.Data.Data.Materials.Types
{
    public struct AirMaterial : IMaterial
    {
        public string MaterialName
        {
            get { return "Air"; }
        }

        public short MaterialCode
        {
            get { return 0; }
        }

        public override bool Equals(object obj)
        {
            if(obj != null && obj is IMaterial)
            {
                return ((IMaterial) obj).MaterialName == MaterialName;
            }

            return false;
        }
    }
}
