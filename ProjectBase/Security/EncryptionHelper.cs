using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Security;

namespace ProjectBase.Security
{
    public class EncryptionHelper
    {
        public EncryptionHelper() { }

        #region RSA Encryption helper


        public static string EncryptWithPublicKey(
            string valueToEncrypt,
            string publicKeyFile,
            bool oaepPadding = false
        )
        {
            string filePath = "";
            using (
                RSACryptoServiceProvider key = (RSACryptoServiceProvider)
                    GetCertificate(publicKeyFile, out filePath).PublicKey.Key
            )
            {
                byte[] bytes = Encoding.UTF8.GetBytes(valueToEncrypt);
                var returnString = Convert.ToBase64String(key.Encrypt(bytes, oaepPadding));
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return returnString;
            }
        }

        public static X509Certificate2 GetCertificate(string publicKeyFile, out string filePath)
        {
            filePath = "";
            try
            {
                filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    $"{Guid.NewGuid().ToString()}.pfx"
                );
                File.WriteAllText(filePath, publicKeyFile);
                //var goodString = publicKeyFile.Trim().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Trim();
                //byte[] bytes = Convert.FromBase64String(goodString);
                var cert = new X509Certificate2(filePath);
                return new X509Certificate2(cert);
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                var stringValue = ex.ToString();
                throw ex;
            }
        }

        public static RSAParameters GetRSAParameters(string keyValue, bool publicKey)
        {
            keyValue = keyValue.Trim();
            try
            {
                if (publicKey)
                {
                    //var sampleString = "<RSAKeyValue><Modulus>x8TpVk1dklE+EHa2j2RNywqmmA+s1YQIyggzYML+2sQawIH/ivYjAVtgmjBG1oVZiyNiGdsG7H8AWGt/X5jYoIlLQq56FLBv5zN5DwHkq8Uep7dbJzXPYXva9qE309ZlDNRI5anEPphYuuK0yel1lg/OrGr8ihiwqCOKO14P/qhN1PaA4RBTuDowR4cjmpGaP6xaWEtzRFm8cIwIiHD7lk2e9wIZ6PqNZgRPD/RAIWOHh4YrTt31z7T9q6XqidRtj3sP8ifJda3/jx8JstaIVS2yiVSDCy6JQbY25j9q6aFm/y8ImevbjPx2N9Flv7M9ACU/gq6OQmzTx9iyNO16DQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                    using (var rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(keyValue);
                        var parameters = rsa.ExportParameters(false);
                        return parameters;
                    }

                    //using (TextReader publicKeyTextReader = new StringReader(keyValue))
                    //{
                    //    RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                    //    RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);

                    //    return rsaParams;
                    //}
                }
                else
                {
                    var pr = new PemReader(new StringReader(keyValue));
                    AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
                    return DotNetUtilities.ToRSAParameters(
                        (RsaPrivateCrtKeyParameters)KeyPair.Private
                    );
                }
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                throw ex;
            }
        }

        public static string RSAEncryptString(
            string dataToEncrypt,
            string publickKeyString,
            Encoding encoding,
            bool DoOAEPPadding = false
        )
        {
            //var encrypted = EncryptWithPublicKey(dataToEncrypt, publickKeyString, DoOAEPPadding);
            //return encrypted;
            byte[] bytesDataToEncrypt = encoding.GetBytes(dataToEncrypt);

            var rsaParam = GetRSAParameters(publickKeyString, true);

            var encryptedData = RSAEncrypt(bytesDataToEncrypt, rsaParam, DoOAEPPadding);

            return Convert.ToBase64String(encryptedData);
        }

        public static string RSADecryptString(
            string dataToEncrypt,
            string privateKeyString,
            Encoding encoding,
            bool DoOAEPPadding = false
        )
        {
            byte[] bytesDataToDecrypt = Convert.FromBase64String(dataToEncrypt);

            var rsaParam = GetRSAParameters(privateKeyString, false);

            var decryptedData = RSADecrypt(bytesDataToDecrypt, rsaParam, DoOAEPPadding);

            return encoding.GetString(decryptedData);
        }

        public static byte[] RSAEncrypt(
            byte[] DataToEncrypt,
            RSAParameters RSAKeyInfo,
            bool DoOAEPPadding
        )
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static byte[] RSADecrypt(
            byte[] DataToDecrypt,
            RSAParameters RSAKeyInfo,
            bool DoOAEPPadding
        )
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        #endregion
        public static string GetBase64String(string sourceString, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(sourceString));
        }

        public static string FromBase64String(string sourceString, Encoding encoding)
        {
            var bytesFromString = Convert.FromBase64String(sourceString);
            var finalString = encoding.GetString(bytesFromString);
            return finalString;
        }

        /// <summary>
        /// Returns a base64 encoded string of the SHA256
        /// </summary>
        /// <param name="hashValue"></param>
        /// <returns></returns>
        public static string SHA256Base64Hash(string hashValue)
        {
            return Convert.ToBase64String(SHA256HashBytes(hashValue));
        }

        /// <summary>
        /// Return a Hex equivalent of a sha256
        /// </summary>
        /// <param name="hashValue"></param>
        /// <param name="hexToUpperCase"></param>
        /// <returns></returns>
        public static string SHA256HexHash(string hashValue, bool hexToUpperCase = true)
        {
            return BytesToHex(SHA256HashBytes(hashValue), hexToUpperCase);
        }

        public static byte[] SHA256HashBytes(string hashValue)
        {
            return SHA256HashBytes(hashValue, ASCIIEncoding.ASCII);
        }

        public static byte[] SHA256HashBytes(string hashValue, Encoding encoding)
        {
            return new SHA256CryptoServiceProvider().ComputeHash(encoding.GetBytes(hashValue));
        }

        public static string BytesToHex(byte[] bytes, bool upperCase = false)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public static string decrypt(string message, string passphrase)
        {
            byte[] numArray;
            string str;
            using (
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider()
            )
            {
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                byte[] numArray1 = mD5CryptoServiceProvider.ComputeHash(
                    uTF8Encoding.GetBytes(passphrase)
                );
                using (
                    TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider =
                        new TripleDESCryptoServiceProvider()
                )
                {
                    tripleDESCryptoServiceProvider.Key = numArray1;
                    tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                    tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                    byte[] numArray2 = Convert.FromBase64String(message);
                    using (
                        ICryptoTransform cryptoTransform =
                            tripleDESCryptoServiceProvider.CreateDecryptor()
                    )
                    {
                        numArray = cryptoTransform.TransformFinalBlock(
                            numArray2,
                            0,
                            (int)numArray2.Length
                        );
                    }
                }
                str = uTF8Encoding.GetString(numArray);
            }
            return str;
        }

        public static string encrypt(string message, string passphrase)
        {
            byte[] numArray;
            string base64String;
            using (
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider()
            )
            {
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                byte[] numArray1 = mD5CryptoServiceProvider.ComputeHash(
                    uTF8Encoding.GetBytes(passphrase)
                );
                using (
                    TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider =
                        new TripleDESCryptoServiceProvider()
                )
                {
                    tripleDESCryptoServiceProvider.Key = numArray1;
                    tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                    tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                    byte[] bytes = uTF8Encoding.GetBytes(message);
                    using (
                        ICryptoTransform cryptoTransform =
                            tripleDESCryptoServiceProvider.CreateEncryptor()
                    )
                    {
                        numArray = cryptoTransform.TransformFinalBlock(bytes, 0, (int)bytes.Length);
                    }
                    base64String = Convert.ToBase64String(numArray);
                }
            }
            return base64String;
        }

        public static int GetRandomID(long? seed = null, int minValue = 10000, int maxValue = 99999)
        {
            Random random;
            random = (!seed.HasValue ? new Random() : new Random((int)seed.Value));
            return random.Next(minValue, maxValue);
        }

        public static bool IsPassWordStrenghtOk(string PassWord)
        {
            return true;
        }

        public static string CreateRandomPasswordPassword(int length = 8)
        {
            var randomString = CreateRandomPasswordLetters((int)(0.5 * length));
            var randomSpecial = CreateRandomPasswordSpecialCharacters((int)(0.25 * length));
            var randomNumbers = CreateRandomPasswordNumber((int)(0.25 * length));

            return $"{randomSpecial}{randomString}{randomNumbers}";
        }

        private static string CreateRandomPasswordLetters(int length = 4)
        {
            // Create a string of characters, numbers, special characters that allowed in the password
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
                if (i % 2 == 0)
                    chars[i] = char.ToUpper(chars[i]);
                else
                    chars[i] = char.ToLower(chars[i]);
            }
            return new string(chars);
        }

        private static string CreateRandomPasswordNumber(int length = 2)
        {
            // Create a string of characters, numbers, special characters that allowed in the password
            string validChars = "0123456789";
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
                if (i % 2 == 0)
                    chars[i] = char.ToUpper(chars[i]);
            }
            return new string(chars);
        }

        private static string CreateRandomPasswordSpecialCharacters(int length = 2)
        {
            // Create a string of characters, numbers, special characters that allowed in the password
            string validChars = "!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
                if (i % 2 == 0)
                    chars[i] = char.ToUpper(chars[i]);
            }
            return new string(chars);
        }
    }
}
