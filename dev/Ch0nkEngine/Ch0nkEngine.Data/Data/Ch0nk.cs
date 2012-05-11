using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ch0nkEngine.Data.Basic;
using Ch0nkEngine.Data.Data.BoundingShapes;
using Ch0nkEngine.Data.Data.Materials;
using Ch0nkEngine.Data.Data.Materials.Types;

namespace Ch0nkEngine.Data.Data
{
    [Serializable]
    public class Ch0nk
    {
        //this could be changed by the user
        public const byte Size = 64;
        public static String Ch0nksFolder = @"Ch0nks\";

        private Vector3i _position;
        private EightFoldTree _eightFoldTree;
        private Dimension _dimension;
        
        public Ch0nk(Dimension dimension, Vector3i position, IMaterial material)
        {
            _dimension = dimension;
            _position = position;
            _eightFoldTree = new EightFoldTree(Size, material);
            _eightFoldTree[0, 0, 0] = new SandMaterial();
        }

        public Vector3i Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public EightFoldTree EightFoldTree
        {
            get { return _eightFoldTree; }
            set { _eightFoldTree = value; }
        }

        public List<Block> GetAllBlocks()
        {
            return EightFoldTree.GetAllBlocks(this,new Vector3b());
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
            File.WriteAllBytes(GetFileName(_position), ms.ToArray());
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

        public BoundingBox BoundingBox
        {
            get{return new BoundingBox(_position,Size);}
        }

        public void ChangeMaterial(BoundingShape boundingShape, IMaterial material)
        {
            _eightFoldTree.ChangeMaterial(boundingShape, material,this,new Vector3b());
        }
    }
}
