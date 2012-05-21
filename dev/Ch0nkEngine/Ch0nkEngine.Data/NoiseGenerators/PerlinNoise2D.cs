using System;

namespace Ch0nkEngine.Data.NoiseGenerators
{
    public class PerlinNoise2D
    {
        
          private double Noise(int x, int y)
          {
              int n = x + y*57;
                n = (n<<13) ^ n;
                return ( 1.0 - ( (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);    
          }
        /*
            private double SmoothNoise(float x, float y)
            {
                double corners = (Noise(x - 1, y - 1) + Noise(x + 1, y - 1) + Noise(x - 1, y + 1) + Noise(x + 1, y + 1))/16;
                double sides = (Noise(x - 1, y) + Noise(x + 1, y) + Noise(x, y - 1) + Noise(x, y + 1))/8;
                double center = Noise(x, y)/4;

                return corners + sides + center;
            }

        private double InterpolatedNoise(float x, float y)
        {
            int integer_X = (int) x;
            float fractional_X = x - integer_X;

          integer_Y    = int(y)
          fractional_Y = y - integer_Y

          v1 = SmoothedNoise1(integer_X,     integer_Y)
          v2 = SmoothedNoise1(integer_X + 1, integer_Y)
          v3 = SmoothedNoise1(integer_X,     integer_Y + 1)
          v4 = SmoothedNoise1(integer_X + 1, integer_Y + 1)

          i1 = Interpolate(v1 , v2 , fractional_X)
          i2 = Interpolate(v3 , v4 , fractional_X)

          return Math.(i1 , i2 , fractional_Y)
        }

        float 

        private int Number_Of_Octaves = 8;
        private int persistence = 8;
      
      private float PerlinNoise_2D(float x, float y)
      {
          int total = 0
          int p = persistence;
          int n = Number_Of_Octaves - 1;

        for(int i = 0; i < Number_Of_Octaves; i++)
        {

              frequency = 2i
              amplitude = pi

          total = total + InterpolatedNoisei(x*frequency, y*frequency)*amplitude;
        }

          return total;
      }

      

  end function*/
    }
}
