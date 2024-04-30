using OtpNet;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using totp_Module.Data.DTO.TOTPService;
using totp_Module.Services.IServices;

namespace totp_Module.Services
{
    public class OTPService : IOTPService
    {
        private readonly EntropyService _entropyService;
        private readonly RandomService _randomService;

        public OTPService(EntropyService entropyService, RandomService randomService)
        {
            _entropyService = entropyService;
            _randomService = randomService;
        }
        public async Task<string> GenerateSecretKey()
        {
            var entropyValue = await _entropyService.CheckEntropy();
            if (entropyValue >= 2)
            {
                byte[] secretKeyBytes = new byte[32];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(secretKeyBytes);
                }
                string secretKey = Base32Encoding.ToString(secretKeyBytes);
                return secretKey;
            }
            else if(entropyValue == 0) {
                var value = await _randomService.GetRandomData();
                byte[] entropyBytes = Encoding.UTF8.GetBytes(value);

                byte[] secretKeyBytes = new byte[32];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(secretKeyBytes);
                }

                for (int i = 0; i < secretKeyBytes.Length; i++)
                {
                    secretKeyBytes[i] ^= entropyBytes[i % entropyBytes.Length];
                }

                string secretKey = Base32Encoding.ToString(secretKeyBytes);
                return secretKey;
            }
            else if (entropyValue < 2 && entropyValue > 0) {
                throw new BadHttpRequestException("BABABA BEBEBE NON REGISTER AHAHAHAHHAHAAHHA");
            }
            return null;
        }

        public async Task<string> GenerateTOTP(string secretKey)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            byte[] secretKeyBytes = Base32Encoding.ToBytes(secretKey);

            byte[] timestampBytes = BitConverter.GetBytes(timestamp / 60); 
            byte[] hmacResult;
            using (var hmac = new HMACSHA1(secretKeyBytes))
            {
                hmacResult = hmac.ComputeHash(timestampBytes);
            }

            int offset = hmacResult[hmacResult.Length - 1] & 0x0F;
            int binaryCode = ((hmacResult[offset] & 0x7F) << 24) |
                             ((hmacResult[offset + 1] & 0xFF) << 16) |
                             ((hmacResult[offset + 2] & 0xFF) << 8) |
                             (hmacResult[offset + 3] & 0xFF);

            string otp = (binaryCode % 1000000).ToString("D6");

            return otp;
        }

        public Task<bool> VerifyTOTP(string expectedOTP, string totpKey)
        {
            return Task.FromResult(expectedOTP == totpKey);
        }

    }
}
