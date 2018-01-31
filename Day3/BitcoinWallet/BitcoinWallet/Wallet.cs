namespace BitcoinWallet
{
    using System;
    using System.Globalization;
    using HBitcoin.KeyManagement;
    using NBitcoin;

    public class Wallet
    {
        private const string WalletPath = "../Wallets/";
        private readonly Network _network = Network.TestNet;

        public bool CreateWallet(string walletName, string password)
        {
            try
            {
                var safe = Safe.Create(out var mnemonic, password, $"{WalletPath}{walletName}.json", _network);

                Console.WriteLine("Wallet created successfully");
                Console.WriteLine("Write down the following mnemonic words.");
                Console.WriteLine("With the mnemonic words AND the password you can recover this wallet.");
                Console.WriteLine(mnemonic);
                Console.WriteLine("Write down and keep in SECURE place your private keys. Only through them you can access your coins!");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Address: {safe.GetAddress(i)} -> Private key: {safe.FindPrivateKey(safe.GetAddress(i))}");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Recover(string password, string mnemonic, string date)
        {
            Random rand = new Random();
            Safe safe = Safe.Recover(new Mnemonic(mnemonic), password, $"{WalletPath}RecoveredWalletNum{rand.Next()}.json", _network, DateTimeOffset.ParseExact(date,"yyyy-MM-dd", CultureInfo.InvariantCulture));
            Console.WriteLine("Wallet successfully recovered");
        }
    }
}
