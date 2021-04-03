using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace BlockChain
{
    internal static class Program
    {
        private static void Main(string[] args)
        { 
            const int difficulty = 5;

            const int numberOfBlockToBeMined = 50;

            var chain = new BlockChain();

            chain.StartMining(numberOfBlockToBeMined, difficulty);

            Console.WriteLine("Chain Validation: " + chain.ChainValidator());

            Console.ReadLine();
        }
    
    }
}