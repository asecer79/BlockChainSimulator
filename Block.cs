using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlockChain
{
    internal class Block
    {
        private int Difficulty { get; }
        private long Index { get; }
        private string TimeStamp { get; }
        private object Data { get; }
        public string PreviousHash { get; set; }
        private int Nonce { get; set; } //for desired hash is less than difficulty
        public string Hash { get; private set; }

        public Block(int difficulty, long index, string timeStamp, BlockTransactionData data, string previousHash = "")
        {
            Difficulty = difficulty;

            this.Index = index;
            this.TimeStamp = timeStamp;
            this.Data = JsonConvert.SerializeObject(data);
            this.PreviousHash = previousHash;
            this.Hash = CalculateHash(); //initial hash, will be changed according to difficulty


        }

        public string CalculateHash()
        {
            var rawData = $"{this.Index}{this.PreviousHash}{this.TimeStamp}{this.Data}{Nonce}{Difficulty}";

            using var sha256Hash = SHA256.Create();

            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var hashedData = new StringBuilder();
            foreach (var b in bytes)
            {
                hashedData.Append(b.ToString("x2"));
            }

            return hashedData.ToString();
        }

        public void MineBlock()
        {
            var target = "";

            for (var i = 0; i < Difficulty; i++) target += "0";
            this.Hash = this.CalculateHash();//initialize hash

            Console.WriteLine($"Mining new block......");
            while (Hash.Substring(0, Difficulty) != target)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }

            PrintMinedHashInfo();
        }

        public void PrintMinedHashInfo()
        {
            Console.WriteLine($"Index        : {this.Index}");
            Console.WriteLine($"Difficulty   : {Difficulty}");
            Console.WriteLine($"Nonce        : {this.Nonce}");
            Console.WriteLine($"TimeStamp    : {this.TimeStamp}");
            Console.WriteLine($"Data         : {this.Data}");
            Console.WriteLine($"Previous Hash: {this.PreviousHash}");
            Console.WriteLine($"Hash         : {this.Hash}");
            Console.WriteLine($"Block Mined  : ({Index})\n");
            Console.WriteLine($"----------------------------------------------------------------");
        }

    }
}