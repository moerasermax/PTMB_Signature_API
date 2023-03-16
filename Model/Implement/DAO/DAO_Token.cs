using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PTMB_Signature_API.Model.Implement.DAO
{
    public class DAO_Token
    {

        public string EncryptionProcess(string Acccount, string Password)
        {
            string Token = null;

            Token = EncryptionMD5(EncryptionSHA256(Acccount, Password));
            
            return Token;
        }


        public string EncryptionSHA256(string Account, string Password)
        {
            string hash_data = string.Format("{0},{1}", Account, Password);

            SHA256 sha256 = SHA256.Create();
            Byte[] hash_result = sha256.ComputeHash(Encoding.UTF8.GetBytes(hash_data));
            string hash_result_string = BitConverter.ToString(hash_result).Replace("-","").ToUpper();
            
            return hash_result_string;
        } 

        public string EncryptionMD5(string Sha256Token)
        {
            MD5 md5 = MD5.Create();
            Byte[] hash_result = md5.ComputeHash(Encoding.UTF8.GetBytes(Sha256Token));
            string hash_result_string = BitConverter.ToString(hash_result).Replace("-", "").ToUpper();
            return hash_result_string;
        }




        public static DAO_Token getInstance()
        {
            return instance;
        }

        public static DAO_Token instance = new DAO_Token();

        private DAO_Token()
        {

        }


    }
}
