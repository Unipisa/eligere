using RocksDbSharp;
using System;

namespace tktdump
{
    class Program
    {
        static void Main(string[] args)
        {
            var opt = new DbOptions().SetCreateIfMissing(false);
            var db = RocksDb.Open(opt, args[0]);
            var it = db.NewIterator();
            Console.WriteLine("{");
            it.SeekToFirst();
            while (it.Valid())
            {
                var k = it.StringKey();
                var v = it.StringValue();

                Console.WriteLine($"{{\"id\": \"{k}\", \"ticket\": {v} }},");
                it.Next();
            }
            Console.WriteLine("}");
        }
    }
}
