namespace Ch0nkEngine.Data.Data.Materials.Types
{
    public struct GrassMaterial : IMaterial
    {
        public string MaterialName
        {
            get { return "Grass"; }
        }

        public short MaterialCode
        {
            get{ return 1; }
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
