using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace MaxAdsl_PP_Net.Utility
{
    internal static class Tools
    {
        private static byte[] encryptionEntropy = { 0, 1, 1, 2, 3, 5, 8, 13, 21 };

        public static byte[] EncryptData(byte[] data)
        {
            byte[] encryptedData = ProtectedData.Protect(data, encryptionEntropy, DataProtectionScope.CurrentUser);
            return encryptedData;
        }

        public static byte[] EncryptData(object data)
        {
            return EncryptData(ObjectToByteArray(data));
        }

        public static byte[] DecryptData(byte[] encData)
        {
            byte[] data = ProtectedData.Unprotect(encData, encryptionEntropy, DataProtectionScope.CurrentUser);
            return data;
        }

        public static T DecryptData<T>(byte[] encData)
        {
            T retVal;
            byte[] data = DecryptData(encData);
            retVal = ByteArrayToObject<T>(data);
            return retVal;
        }


        private static byte[] ObjectToByteArray(Object obj)
        {
            byte[] retVal;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            retVal = ms.ToArray();
            ms.Dispose();
            return retVal;
        }

        private static T ByteArrayToObject<T>(byte[] arr)
        {
            T retVal;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(arr);
            retVal = (T)bf.Deserialize(ms);
            ms.Dispose();
            return retVal;
        }

    }
}
