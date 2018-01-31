using System;
using System.Collections.Generic;

namespace BitcoinAddressGenerator
{
    using System.Globalization;
    using System.Numerics;
    using System.Security.Cryptography;

    class Program
    {
        const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        static void Main(string[] args)
        {
            string HexHash = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            byte[] PubKey = HexToByte(HexHash);
            Console.WriteLine("Public Key:" + ByteToHex(PubKey));

            byte[] PubKeySha = Sha256(PubKey);
            Console.WriteLine("Sha Public Key:" + ByteToHex(PubKeySha));

            byte[] PubKeyShaRIPE = RipeMD160(PubKeySha);
            Console.WriteLine("Ripe Sha Public Key:" + ByteToHex(PubKeyShaRIPE));

            byte[] PreHashWNetwork = AppendBitcoinNetwork(PubKeyShaRIPE, 0);
            byte[] PublicHash = Sha256(PreHashWNetwork);
            Console.WriteLine("Public Hash:" + ByteToHex(PublicHash));

            byte[] PublicHashHash = Sha256(PublicHash);
            Console.WriteLine("Public HashHash:" + ByteToHex(PublicHashHash));

            Console.WriteLine("Checksum:" + ByteToHex(PublicHashHash).Substring(0, 4));

            byte[] Address = ConcatAddress(PreHashWNetwork, PublicHashHash);
            Console.WriteLine("Address:" + ByteToHex(Address));

            Console.WriteLine("Human Address:" + Base58Encode(Address));
        }

        private static string Base58Encode(byte[] array)
        {
            string retString = string.Empty;
            BigInteger encodeSize = Alphabet.Length;
            BigInteger arrayToInt = 0;
            foreach (var item in array)
            {
                arrayToInt = arrayToInt * 256 + item;
            }

            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % encodeSize);
                arrayToInt /= encodeSize;
                retString = Alphabet[rem] + retString;
            }

            for (int i = 0; i < array.Length && array[i] == 0; i++)
            {
                retString = Alphabet[0] + retString;
            }

            return retString;
        }

        private static byte[] ConcatAddress(byte[] ripeHash, byte[] checkSum)
        {
            byte[] ret = new byte[ripeHash.Length + 4];
            Array.Copy(ripeHash, ret, ripeHash.Length);
            Array.Copy(checkSum, 0, ret, ripeHash.Length, 4);
            return ret;
        }

        private static byte[] AppendBitcoinNetwork(byte[] ripeHash, byte network)
        {
            var data = new List<byte>() { network };
            data.AddRange(ripeHash);
            return data.ToArray();
        }

        private static byte[] RipeMD160(byte[] pubKey)
        {
            using (var hasher = new RIPEMD160Managed())
            {
                return hasher.ComputeHash(pubKey);
            }
        }

        private static byte[] Sha256(byte[] pubKey)
        {
            using (var hasher = new SHA256Managed())
            {
                return hasher.ComputeHash(pubKey);
            }
        }

        private static byte[] HexToByte(string hexHash)
        {
            if (hexHash.Length % 2 != 0)
            {
                throw new ArgumentException(nameof(hexHash));
            }

            byte[] data = new byte[hexHash.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = byte.Parse(hexHash.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        private static string ByteToHex(byte[] dataBytes)
        {
            return BitConverter.ToString(dataBytes);
        }
    }
}
