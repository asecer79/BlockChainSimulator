using System;
using System.Collections.Generic;

namespace BlockChain
{
    internal class BlockChain
    {
        public BlockChain()
        {
            //initializing
            Chain = new List<Block> { CreateGenesisBlock() };
        }

        public int Difficulty { get; set; } = 0;

        private List<Block> Chain { get; }

        private Block CreateGenesisBlock() //default first starting block
        {
            const int index = 0;
            var timeStamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "." + DateTime.Now.Ticks;
            var data = new BlockTransactionData() { Amount = 0, Seller = "Genesis Block", Buyer = "Genesis Block" };
            const string previousHash = "0";

            var block = new
                Block(
                    index: index,
                    timeStamp: timeStamp,
                    data: data,
                    previousHash: previousHash
                );

            block.PrintMinedHashInfo(Difficulty);
            return block;
        }

        private Block GetLatestBlock()
        {
            return Chain[^1];

        }

        public void AddNewBlock(Block newBLock)
        {
            newBLock.PreviousHash = this.GetLatestBlock().Hash;
            newBLock.MineBlock(Difficulty);  //find block with desired difficulty
            this.Chain.Add(newBLock);
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
    }
}