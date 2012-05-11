using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data.Materials;
using Ch0nkEngine.Data.Data.Materials.Types;

namespace Ch0nkEngine.Data.Data.Dimensions
{
    public class EarthDimension : Dimension
    {
        public EarthDimension(Realm realm) 
            : base(realm)
        {
        }

        protected override void Initialize()
        {
            _chonks.Add(new Vector3i(), new Ch0nk(this, new Vector3i(), new GrassMaterial()));

            /*for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        IMaterial material = k <= 0 ? (IMaterial)new GrassMaterial() : new AirMaterial();

                        Vector3i position = new Vector3i(i * Ch0nk.Size, j * Ch0nk.Size, k * Ch0nk.Size);
                        _chonks.Add(position, new Ch0nk(this, position, material));
                    }
                }
            }*/
        }


    }
}
