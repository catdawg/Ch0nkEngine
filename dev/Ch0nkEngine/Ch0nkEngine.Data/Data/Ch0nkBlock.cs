using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ch0nkEngine.Data.Basic;

namespace Ch0nkEngine.Data.Data
{
    [Serializable()]
    public class Ch0nkBlock
    {
        public const int BlockGroupSize = 32;
        public static String Ch0nksFolder = @"Ch0nks\"; 

        private Vector4i _location;
        short[,,] _blocks;

        public Ch0nkBlock(Vector4i location)
        {
            _location = location;
            _blocks = new short[BlockGroupSize, BlockGroupSize, BlockGroupSize];
            /*for (int i = 0; i < BlockGroupSize; i++)
            {
                for (int j = 0; j < BlockGroupSize; j++)
                {
                    for (int k = 0; k < BlockGroupSize; k++)
                    {

                    }
                }
            }*/
        }

        public void SaveCh0nk()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);

            File.WriteAllBytes(GetFileName(_location), ms.ToArray());
            return;
        }


        /// <summary>
        /// Loads a chunk from a certain location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Ch0nk LoadCh0nk(Vector4i location)
        {
            byte[] arrBytes = File.ReadAllBytes(GetFileName(location));

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return (Ch0nk)binForm.Deserialize(memStream);
        }


        private static String GetFileName(Vector4i location)
        {
            return Ch0nksFolder + "Ch0nk(" + location.X + "," + location.Y + "," + location.Z + "," + location.W + ").ch0nk";
        }
    }
}
