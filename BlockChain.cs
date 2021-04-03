using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain
{
    internal class BlockChain
    {
        public BlockChain()
        {
            //initializing
            Chain = new List<Block> { CreateGenesisBlock() };
        }

        private List<Block> Chain { get; }

        private Block CreateGenesisBlock() //default first starting block
        {
            const int index = 0;
            var timeStamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "." + DateTime.Now.Ticks;
            var data = new BlockTransactionData() { Amount = 0, Seller = "Genesis Block", Buyer = "Genesis Block" };
            const string previousHash = "0";

            var block = new
                Block(
                    difficulty:0,
                    index: index,
                    timeStamp: timeStamp,
                    data: data,
                    previousHash: previousHash
                );

            block.PrintMinedHashInfo();
            return block;
        }

        private Block GetLatestBlock()
        {
            return Chain[^1];

        }
        private void AddNewBlock(Block newBLock)
        {
            newBLock.PreviousHash = this.GetLatestBlock().Hash;
            newBLock.MineBlock();  //find block with desired difficulty
            this.Chain.Add(newBLock);
        }

        public void StartMining(int numberOfBlockToBeMined, int difficulty)
        {
            for (var i = 1; i <= numberOfBlockToBeMined; i++)
            {
                this.AddNewBlock
                (
                    new Block
                    (
                        difficulty: difficulty,
                        index: i,
                        timeStamp: DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "." + DateTime.Now.Ticks,
                        data: new BlockTransactionData()
                        {
                            Seller = RandomString(5),
                            Buyer = RandomString(5),
                            Amount = new Random().Next(1, 50)
                        }
                    )
                );
            }
        }

        public bool ChainValidator()
        {
            for (var i = 1; i < Chain.Count; i++)
            {
                var previousBlock = Chain[i - 1];
                var currentBlock = Chain[i];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }
                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }

                if (previousBlock.Hash != previousBlock.CalculateHash())
                {
                    return false;
                }
            }

            return true;
        }

        private static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            var random = new Random();

            var offset = lowerCase ? 'a' : 'A';

            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);

                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}