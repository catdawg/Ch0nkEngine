using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ch0nkEngine.Data.Utils
{
    public class Serialization
    {
        /// <summary>
        /// Converts (Serializes) an object to an byte array.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">Object to be converted.</param>
        /// <returns>Byte array obtained.</returns>
        public static byte[] SerializeToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }



        /// <summary>
        /// Converts (Deserializes) a byte array to an object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="arrBytes">Byte array.</param>
        /// <returns>Object obtained.</returns>
        public static T DeserializeFromByteArray<T>(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);
            return obj;
        }

        /// <summary>
        /// Creates a new binary file, writes the specified object to the file, and then closes the file. 
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">Object to be written.</param>
        /// <param name="filePath">Path to the file.</param>
        public static void WriteToBinaryFile<T>(T obj, String filePath)
        {
            byte[] serializedBytes = SerializeToByteArray(obj);
            File.WriteAllBytes(filePath, serializedBytes);
        }



        /// <summary>
        /// Opens a binary file, converts to an object, and then closes the file.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="filePath">Path to the file.</param>
        public static T ReadFromBinaryFile<T>(String filePath)
        {
            byte[] byteFile = File.ReadAllBytes(filePath);

            T obj = DeserializeFromByteArray<T>(byteFile);

            return obj;
        }
    }
}
