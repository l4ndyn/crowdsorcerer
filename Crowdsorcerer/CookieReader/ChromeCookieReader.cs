using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace ChromeBrowserCookieReader
{
    public class ChromeCookieReader
    {
        public IEnumerable<(string name, string value)> ReadCookies(string hostName)
        {
            if (hostName == null) throw new ArgumentNullException(nameof(hostName));

            using var context = new ChromeCookieDbContext();

            var cookies = context
                .Cookies
                .Where(c => c.HostKey.Equals(hostName))
                .AsNoTracking();

            string encKey = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Google\Chrome\User Data\Local State"));
            encKey = JObject.Parse(encKey)["os_crypt"]?["encrypted_key"]?.ToString();
            var decodedKey = ProtectedData.Unprotect(Convert.FromBase64String(encKey).Skip(5).ToArray(), null, DataProtectionScope.LocalMachine);

            foreach (var cookie in cookies)
            {
                var data = cookie.EncryptedValue;
                var decodedValue = DecryptWithKey(data, decodedKey, 3);

                yield return (cookie.Name, decodedValue);
            }
        }

        private string DecryptWithKey(byte[] message, byte[] key, int nonSecretPayloadLength)
        {
            const int KEY_BIT_SIZE = 256;
            const int MAC_BIT_SIZE = 128;
            const int NONCE_BIT_SIZE = 96;

            if (key == null || key.Length != KEY_BIT_SIZE / 8)
                throw new ArgumentException($"Key needs to be {KEY_BIT_SIZE} bit!", nameof(key));
            if (message == null || message.Length == 0)
                throw new ArgumentException("Message required!", nameof(message));

            using (var cipherStream = new MemoryStream(message))
            using (var cipherReader = new BinaryReader(cipherStream))
            {
                cipherReader.ReadBytes(nonSecretPayloadLength);

                var nonce = cipherReader.ReadBytes(NONCE_BIT_SIZE / 8);
                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(key), MAC_BIT_SIZE, nonce);
                cipher.Init(false, parameters);

                var cipherText = cipherReader.ReadBytes(message.Length);
                var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];
                try
                {
                    var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                    cipher.DoFinal(plainText, len);
                }
                catch (InvalidCipherTextException)
                {
                    return null;
                }
                return Encoding.Default.GetString(plainText);
            }
        }
    }
}
