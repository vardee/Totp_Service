using System;

namespace totp_Module.Services
{
    public class EntropyService
    {
        private readonly Random _random;
        public EntropyService()
        {
            _random = new Random();
        }

        public async Task<int> CheckEntropy()
        {
            int randomValue = _random.Next(0, 10);
            return randomValue;
        }

    }
}
