using System.Security.Cryptography;
using System.Text;

namespace Point_of_Sale.Extensions
{
    public static class Encryptor
    {

        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            Random _random = new Random();

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public static string RadomVoucher()
        {
            var voucher = new StringBuilder();
            voucher.Append(RandomString(10, true));

            // 4-Digits between 1000 and 9999  
            voucher.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            voucher.Append(RandomString(2));

            return voucher.ToString().ToUpper();
        }
        public static int RandomNumber(int min, int max)
        {
            Random _random = new Random();
            return _random.Next(min, max);
        }
        public static string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));

            return passwordBuilder.ToString();
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static string auto_gen_code(int count)
        {
            string code = "";
            code = code + count.ToString();
            for (int i = code.Length; i < 3; i++)
            {
                code = "0" + code;
            }
            return code;
        }

    }
}
