using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.ViewModels;
using MiPaladar.Entities;

using System.IO;
using System.Security.Cryptography;

namespace MiPaladar.Services
{
    public interface IEncrypter 
    {
        //bool CheckUserExists(string username);
        bool CheckUserPassword(Employee emp, string password);
        void ChangeUserPassword(Employee emp, string newPassowrd);
        //bool CheckAdminPassword(string adminPassword);
        //void ChangeAdminPassword(string newPassowrd);
    }

    public class Encrypter : IEncrypter
    {
        //RestaurantDBEntities context;

        //public Encrypter(RestaurantDBEntities context) 
        //{
        //    this.context = context;
        //}

        byte[] key = new byte[32] { 16, 81, 143, 171, 113, 237, 125, 98, 
            171, 27, 201, 77, 201, 35, 144, 63, 
            194, 44, 86, 172, 221, 29, 62, 176, 
            5, 17, 138, 233, 178, 27, 181, 146 };
        byte[] IV = new byte[16] { 44, 195, 203, 163, 29, 146, 130, 234, 
            204, 101, 23, 118, 142, 220, 14, 33 };

        //public bool CheckUserExists(string username) 
        //{
        //    var query = from user in context.Employees
        //                where user.Name == username
        //                select user;

        //    return query.Count() > 0;
        //}

        public bool CheckUserPassword(Employee emp, string password) 
        {
            //if anyone is null or ws, both have to be
            if (string.IsNullOrWhiteSpace(emp.Password) || string.IsNullOrWhiteSpace(password))
            {
                return string.IsNullOrWhiteSpace(emp.Password) && string.IsNullOrWhiteSpace(password);
            }
            else
            {
                //read byte array from database
                string[] blocks = emp.Password.Split('-');

                byte[] byteBlocks = new byte[blocks.Length];
                for (int i = 0; i < blocks.Length; i++)
                {
                    byteBlocks[i] = byte.Parse(blocks[i]);
                }

                //encrypt given password
                byte[] passInBytes = encryptStringToBytes_AES(password, key, IV);

                if (byteBlocks.Length != passInBytes.Length) return false;

                //compare
                for (int i = 0; i < passInBytes.Length; i++)
                {
                    if (byteBlocks[i] != passInBytes[i]) return false;
                }

                return true;
            }
        }

        public void ChangeUserPassword(Employee emp, string newPassowrd)
        {
            if (string.IsNullOrWhiteSpace(newPassowrd))
            {
                emp.Password = null;
            }
            else
            {
                //encrypt given password
                byte[] passInBytes = encryptStringToBytes_AES(newPassowrd, key, IV);

                StringBuilder sb = new StringBuilder();
                int i = 0;
                for (; i < passInBytes.Length - 1; i++)
                {
                    sb.Append(passInBytes[i].ToString() + '-');
                }
                if (passInBytes.Length > 0) sb.Append(passInBytes[i]);

                emp.Password = sb.ToString();
            }           
        }

        //public bool CheckAdminPassword(string adminPassword)
        //{
        //    var query = from user in context.UserPasswords
        //                where user.UserName == "admin"
        //                select user;

        //    if (query.Count() > 0)
        //    {
        //        //admin password
        //        UserPassword up = query.First();

        //        if (string.IsNullOrWhiteSpace(adminPassword)) 
        //        {
        //            return string.IsNullOrWhiteSpace(up.Password);
        //        }
        //        else if (string.IsNullOrWhiteSpace(up.Password))
        //        {
        //            return string.IsNullOrWhiteSpace(adminPassword);
        //        }
        //        else 
        //        {
        //            //read byte array from database
        //            string[] blocks = up.Password.Split('-');

        //            byte[] byteBlocks = new byte[blocks.Length];
        //            for (int i = 0; i < blocks.Length; i++)
        //            {
        //                byteBlocks[i] = byte.Parse(blocks[i]);
        //            }

        //            //encrypt given password
        //            byte[] passInBytes = encryptStringToBytes_AES(adminPassword, key, IV);

        //            if (byteBlocks.Length != passInBytes.Length) return false;

        //            //compare
        //            for (int i = 0; i < passInBytes.Length; i++)
        //            {
        //                if (byteBlocks[i] != passInBytes[i]) return false;
        //            }

        //            return true;
        //        }                
        //    }

        //    return false;
        //}

        //public void ChangeAdminPassword(string newPassowrd)
        //{
        //    var query = from user in context.UserPasswords
        //                where user.UserName == "admin"
        //                select user;

        //    if (query.Count() > 0)
        //    {
        //        UserPassword up = query.First();

        //        if (string.IsNullOrWhiteSpace(newPassowrd))
        //        {
        //            up.Password = newPassowrd;
        //        }
        //        else 
        //        {
        //            //encrypt given password
        //            byte[] passInBytes = encryptStringToBytes_AES(newPassowrd, key, IV);

        //            StringBuilder sb = new StringBuilder();
        //            int i = 0;
        //            for (; i < passInBytes.Length - 1; i++)
        //            {
        //                sb.Append(passInBytes[i].ToString() + '-');
        //            }
        //            if (passInBytes.Length > 0) sb.Append(passInBytes[i]);

        //            up.Password = sb.ToString();
        //        }               

        //        context.SaveChanges();
        //    }
        //}

        byte[] encryptStringToBytes_AES(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the stream used to encrypt to an in memory

            // array of bytes.

            MemoryStream msEncrypt = null;

            // Declare the RijndaelManaged object

            // used to encrypt the data.

            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object

                // with the specified key and IV.

                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.

                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.

                        swEncrypt.Write(plainText);
                    }
                }

            }
            finally
            {

                // Clear the RijndaelManaged object.

                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.

            return msEncrypt.ToArray();

        }

        string decryptStringFromBytes_AES(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the RijndaelManaged object

            // used to decrypt the data.

            RijndaelManaged aesAlg = null;

            // Declare the string used to hold

            // the decrypted text.

            string plaintext = null;

            try
            {
                // Create a RijndaelManaged object

                // with the specified key and IV.

                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream

                            // and place them in a string.

                            plaintext = srDecrypt.ReadToEnd();
                    }
                }

            }
            finally
            {

                // Clear the RijndaelManaged object.

                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;

        }
             
    }
}
