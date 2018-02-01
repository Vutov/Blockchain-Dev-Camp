using System;

namespace BitcoinWallet
{
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var operations = new List<string>() { "create", "recover", "balance", "history", "recieve", "send", "exit" };
            var line = string.Empty;

            var waller = new Wallet();
            while (line.ToLower() != "exit")
            {
                do
                {
                    Console.WriteLine("Enter command");
                    line = Console.ReadLine();
                } while (operations.Any(o => line.ToLower() == o) == false);

                string walletName;
                string password;
                string address;
                switch (line.ToLower())
                {
                    case "create":
                        string confirmPassword;
                        do
                        {
                            Console.WriteLine("Enter password:");
                            password = Console.ReadLine();
                            Console.WriteLine("Confirm password:");
                            confirmPassword = Console.ReadLine();
                        } while (password != confirmPassword);

                        do
                        {
                            Console.WriteLine("Enter waller name:");
                            walletName = Console.ReadLine();
                        } while (waller.CreateWallet(walletName, password) == false);
                        break;
                    case "recover":
                        Console.WriteLine(
                            "Please note the wallet cannot check if your password is correct or not. " +
                            "If you provide a wrong password a wallet will be recovered with your " +
                            "provided mnemonic AND password pair: ");
                        Console.WriteLine("Enter password: ");
                        string passowrd = Console.ReadLine();
                        Console.Write("Enter mnemonic phrase: ");
                        string mnemonic = Console.ReadLine();
                        Console.Write("Enter date (yyyy-MM-dd): ");
                        string date = Console.ReadLine();
                        waller.Recover(passowrd, mnemonic, date);
                        break;
                    case "recieve":
                        Console.WriteLine("Enter wallet's name: ");
                        walletName = Console.ReadLine();
                        Console.WriteLine("Enter password: ");
                        password = Console.ReadLine();
                        waller.Recieve(walletName, password);
                        break;
                    case "balance":
                        Console.WriteLine("Enter balance address: ");
                        address = Console.ReadLine();
                        waller.ShowBalance(address);
                        break;
                    case "history":
                        Console.WriteLine("Enter balance address: ");
                        address = Console.ReadLine();
                        waller.ShowHistory(address);
                        break;
                    case "send":
                        Console.WriteLine("Enter wallet's name: ");
                        walletName = Console.ReadLine();
                        Console.WriteLine("Enter password: ");
                        password = Console.ReadLine();
                        Console.WriteLine("Enter balance address: ");
                        address = Console.ReadLine();
                        Console.WriteLine("TransactionID: ");
                        var outPoint = Console.ReadLine();
                        waller.Send(password, walletName, address, outPoint);
                        break;
                }
            }
        }
    }
}
