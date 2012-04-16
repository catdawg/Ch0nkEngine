using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ch0nkEngine.Data
{
    [Serializable]
    public class Ch0nk
    {
        //this could be changed by the user
        public const int Size = 512;
        public const int BlockGroupSize = 32;
        public static String Ch0nksFolder = @"Ch0nks\"; 

        private Vector3i _location;
        private EightFoldTree _eightFoldTree;

        public Ch0nk()
        {
            _location = new Vector3i(0,0,0);
            _eightFoldTree = new EightFoldTree(Size, MaterialType.Dirt);
        }

        public Vector3i Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public EightFoldTree EightFoldTree
        {
            get { return _eightFoldTree; }
            set { _eightFoldTree = value; }
        }

        public List<Block> GetAllBlocks()
        {
            return null;//return EightFoldTree.GetAllBlocks(_location);
        }

        public List<LocalizedEightFoldTree> GetBlockGroups()
        {
            return null; // return EightFoldTree.GetLocalizedTreesOfSize(_location, BlockGroupSize);
        }


        /// <summary>
        /// Converts (Serializes) the chunk and saves it to file.
        /// </summary>
        public void SaveCh0nk()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            File.WriteAllBytes(GetFileName(_location), ms.ToArray());
            return ;
        }


        /// <summary>
        /// Loads a chunk from a certain location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Ch0nk LoadCh0nk(Vector3i location)
        {
            byte[] arrBytes = File.ReadAllBytes(GetFileName(location));

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return (Ch0nk)binForm.Deserialize(memStream);
        }


        private static String GetFileName(Vector3i location)
        {
            return Ch0nksFolder + "Ch0nk(" + location.X + "," + location.Y + "," + location.Z + ").ch0nk";
        }
    }
}
