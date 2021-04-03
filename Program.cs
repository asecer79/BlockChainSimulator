using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace BlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            BlockChain chain = new BlockChain();

            for (int i = 1; i <= 100; i++)
            {
                chain.AddNewBlock
                (
                    new Block
                    (
                        i,
                        DateTime.Now.ToShortDateString(),
                        new BlockTransactionData()
                        {
                            Seller = RandomString(5),
                            Buyer = RandomString(5),
                            Amount = new Random().Next(1, 50)
                        }
                    )
                );
            }
            


         


            foreach (var block in chain.chain)
            {
                Console.WriteLine($"Index        : {block._index}");
                Console.WriteLine($"Data         : {block._data}");
                Console.WriteLine($"Previous Hash: {block._previousHash}");
                Console.WriteLine($"Hash         : {block._hash}");
                Console.WriteLine("\n----------------\n");
            }

            Console.WriteLine("Chain Validation: " +chain.ChainValidator());


            //test for attack
            //chain.chain[0]._index = 3;
            //chain.chain[1]._index = 44;
            //chain.chain[2]._index = 22;
            //chain.chain[0]._data = "3";
            //chain.chain[1]._data = "44";
            //chain.chain[2]._data = "22";
            //Console.WriteLine("Chain Validation: " + chain.ChainValidator());

            Console.ReadLine();
        }

        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            Random _random = new Random();
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }


    class BlockTransactionData
    {
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public int Amount { get; set; }

        //etc
    }
    class Block
    {
        public long _index;
        public string _timeStamp;
        public object _data;
        public string _previousHash;
        public string _hash;

        public Block(long index, string timeStamp, BlockTransactionData data, string previousHash = "")
        {
            this._index = index;
            this._timeStamp = timeStamp;
            this._data = JsonConvert.SerializeObject(data);
            this._previousHash = previousHash;

            if (index == 0) //if genesis
                this._hash = CalculateHash();
            else
                this._hash = ""; //if not must be calculated later.


        }

        public string CalculateHash()
        {
            var rawData = $"{this._index}{this._previousHash}{this._timeStamp}{this._data}";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder hashedData = new StringBuilder();
                foreach (var b in bytes)
                {
                    hashedData.Append(b.ToString("x2"));
                }

                return hashedData.ToString();
            }


        }


    }

    class BlockChain
    {
        public BlockChain()
        {
            //initializing
            chain = new List<Block> { CreateGenesisBlock() };
        }

        public List<Block> chain;

        Block CreateGenesisBlock() //default first starting block
        {
            return new Block(index: 0, timeStamp: DateTime.Now.ToLongDateString(), data: new BlockTransactionData() { Amount = 0, Seller = "Genesis Block", Buyer = "Genesis Block" },
                previousHash: "0");
        }

        Block GetLatestBlock()
        {
            return chain[chain.Count - 1];

        }

        public void AddNewBlock(Block newBLock)
        {
            newBLock._previousHash = this.GetLatestBlock()._hash;
            newBLock._hash = newBLock.CalculateHash();
            this.chain.Add(newBLock);
        }

        public bool ChainValidator()
        {
            for (int i = 1; i < chain.Count; i++)
            {
                var previousBlock = chain[i - 1];
                var currentBlock = chain[i];

                if (currentBlock._hash!=currentBlock.CalculateHash())
                {
                    return false;
                }
                if (currentBlock._previousHash != previousBlock._hash)
                {
                    return false;
                }

                if (previousBlock._hash != previousBlock.CalculateHash())
                {
                    return false;
                }
            }

            return true;
        }
    }

}