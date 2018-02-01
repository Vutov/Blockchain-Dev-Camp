namespace BitcoinWallet
{
    using System;
    using System.Globalization;
    using System.Linq;
    using HBitcoin.KeyManagement;
    using NBitcoin;
    using QBitNinja.Client;
    using QBitNinja.Client.Models;

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
            Safe safe = Safe.Recover(new Mnemonic(mnemonic), password, $"{WalletPath}RecoveredWalletNum{rand.Next()}.json", _network, DateTimeOffset.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            Console.WriteLine("Wallet successfully recovered");
        }

        public void Recieve(string walletName, string password)
        {
            var loadedSafe = Safe.Load(password, $"{WalletPath}{walletName}.json");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(loadedSafe.GetAddress(i));
            }
        }

        public void ShowBalance(string address)
        {
            var client = new QBitNinjaClient(_network);
            var balance = client.GetBalanceSummary(BitcoinAddress.Create(address)).Result;
            Console.WriteLine($"Confirmed Balance of wallet: {balance.Confirmed.Amount}");
            Console.WriteLine($"UnConfirmed Balance of wallet: {balance.UnConfirmed.Amount}");
        }

        public void ShowHistory(string address)
        {
            var client = new QBitNinjaClient(_network);
            var wallet = client.GetBalance(BitcoinAddress.Create(address), true).Result;
            var recievedMessages = wallet.Operations
                .Select(o => o.ReceivedCoins.Select(c => $"TransactionID: {c.Outpoint}; Recieved Coins:{((Money)c.Amount).ToDecimal(MoneyUnit.BTC)}"))
                .SelectMany(i => i)
                .ToList();
            var spendMessages = wallet.Operations
                .Select(o => o.SpentCoins.Select(c => $"TransactionID: {c.Outpoint}; Spend Coins:{((Money)c.Amount).ToDecimal(MoneyUnit.BTC)}"))
                .SelectMany(i => i)
                .ToList();
            recievedMessages.AddRange(spendMessages);
            foreach (var message in recievedMessages)
            {
                Console.WriteLine(message);
            }
        }

        public void Send(string password, string walletName, string address, string outPoint)
        {
            BitcoinExtKey privateKey = null;
            try
            {
                var loadedSafe = Safe.Load(password, $"{WalletPath}{walletName}.json");
                for (int i = 0; i < 10; i++)
                {
                    if (loadedSafe.GetAddress(i).ToString() == address)
                    {
                        Console.Write("Enter private key: ");
                        privateKey = new BitcoinExtKey(Console.ReadLine());
                        if (!privateKey.Equals(loadedSafe.FindPrivateKey(loadedSafe.GetAddress(i))))
                        {
                            Console.WriteLine("Wrong private key!");
                            return;
                        }
                        break;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Wrong wallet or password!");
                return;
            }

            QBitNinjaClient client = new QBitNinjaClient(Network.TestNet);
            var balance = client.GetBalance(BitcoinAddress.Create(address), false).Result;
            OutPoint outPointToSpend = null;
            foreach (var entry in balance.Operations)
            {
                foreach (var coin in entry.ReceivedCoins)
                {
                    if (coin.Outpoint.ToString().Substring(0, coin.Outpoint.ToString().Length - 2) == outPoint)
                    {
                        outPointToSpend = coin.Outpoint;
                        break;
                    }
                }
            }

            var transaction = new Transaction();
            transaction.Inputs.Add(new TxIn()
            {
                PrevOut = outPointToSpend
            });
            Console.Write("Enter address to send to: ");
            string addressToSentTo = Console.ReadLine();
            var hallOfTheMakersAddress = BitcoinAddress.Create(addressToSentTo);
            Console.Write("Enter amount to send: ");
            decimal amountToSend = decimal.Parse(Console.ReadLine());
            TxOut hallOfTheMakersTxOut = new TxOut()
            {
                Value = new Money(amountToSend, MoneyUnit.BTC),
                ScriptPubKey = hallOfTheMakersAddress.ScriptPubKey
            };
            Console.Write("Enter amount to get back: ");
            decimal amountToGetBack = decimal.Parse(Console.ReadLine());
            TxOut changeBackTxOut = new TxOut()
            {
                Value = new Money(amountToGetBack, MoneyUnit.BTC),
                ScriptPubKey = privateKey.ScriptPubKey
            };
            transaction.Outputs.Add(hallOfTheMakersTxOut);
            transaction.Outputs.Add(changeBackTxOut);

            transaction.Inputs[0].ScriptSig = privateKey.ScriptPubKey;
            transaction.Sign(privateKey, false);
            BroadcastResponse broadcastResponse = client.Broadcast(transaction).Result;
            Console.WriteLine(broadcastResponse.Error);
        }
    }
}
