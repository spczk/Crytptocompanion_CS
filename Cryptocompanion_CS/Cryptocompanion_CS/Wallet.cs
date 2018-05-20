using System;
namespace Cryptocompanion
{
    public class Wallet
    {
        public Wallet()
        {
        }

        public String Name { get; set; }
        public String PublicKey { get; set; }
        public String PassPhrase { get; set; }
        public String PrivateKey { get; set; }
        public String WordCode { get; set; }
        public String Address { get; set; }
        public String CryptocurrencyName { get; set; }
    }
}
