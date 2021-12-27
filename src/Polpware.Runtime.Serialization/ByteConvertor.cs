using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Polpware.Runtime.Serialization
{
    public static class ByteConvertor
    {
        /// <summary>
        /// Convert an object to a byte array
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Bytes</returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Convert a byte array to an Object
        /// </summary>
        /// <param name="arrBytes">Bytes</param>
        /// <returns>Object</returns>
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);

                BinaryFormatter binForm = new BinaryFormatter();
                Object obj = (Object)binForm.Deserialize(memStream);

                return obj;
            }
        }
    }
}
