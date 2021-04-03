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
            var chain = new BlockChain
            {
                Difficulty = 4
            };

            const int numberOfBlockToBeMine = 5;

            for (var i = 1;i<= numberOfBlockToBeMine; i++)
            {
                chain.AddNewBlock
                (
                    new Block
                    (
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


            Console.WriteLine("Chain Validation: " + chain.ChainValidator());

            Console.ReadLine();
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