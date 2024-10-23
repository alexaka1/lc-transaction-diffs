// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Globalization;
using CsvHelper;

using FileStream fileA = File.Open("Data/Attachments-TechTest/Transactions-A.csv", FileMode.Open);
using FileStream fileB = File.Open("Data/Attachments-TechTest/Transactions-B.csv", FileMode.Open);
using var reader = new StreamReader(fileA);
using var readerB = new StreamReader(fileB);
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
using var csvB = new CsvReader(readerB, CultureInfo.InvariantCulture);
var recordsA = csv.GetRecords<Transaction>().ToList();
var recordsB = csvB.GetRecords<Transaction>().ToList();
Console.WriteLine($"Transaction A Count: {recordsA.Count}");
Console.WriteLine($"Transaction B Count: {recordsB.Count}");

var task1 = recordsA.Where(x => recordsB.All(y => y.TradeId != x.TradeId)).ToList();
var dictionaryB = recordsB.ToDictionary(a => a.TradeId);
foreach (Transaction transaction in recordsA)
{
    if (!dictionaryB.ContainsKey(transaction.TradeId))
    {
        Console.WriteLine(transaction);
    }
}

Console.WriteLine($"Result1 count: {task1.Count}");
foreach (Transaction transaction in task1)
{
    Console.WriteLine(transaction);
}
// var task2 = recordsB.Where(x => recordsA.All(y => y.TradeId != x.TradeId)).ToList();
// Console.WriteLine($"Result2 count: {task2.Count}");
// foreach (Transaction transaction2 in task2)
// {
//     Console.WriteLine(transaction2);
// }

// var task3 = recordsB.Where(b => recordsA.Any(a => a.TradeId == b.TradeId && a != b)).ToList();
// // var task3 = recordsB.Intersect(recordsA, new TransactionTradeIdComparer()).ToList();
// // var task3_a = recordsA.Where(x => task3.Select(y => y.TradeId).Any(y => y == x.TradeId));
// Console.WriteLine($"Result3 count: {task3.Count}");
// foreach (Transaction transaction3 in task3)
// {
//     Console.WriteLine(transaction3);
// }

var task4 = recordsB
    .Join(recordsA, a => a.TradeId, b => b.TradeId, (a, b) => new { a, b })
    .Where(join => join.a != join.b).ToList();
Console.WriteLine($"Result4 count: {task4.Count}");
foreach (var transaction4 in task4)
{
    Console.WriteLine(transaction4);
}

public record Transaction
{
    public string Ticker { get; set; }
    public int TradeId { get; set; }
    public string Counterparty { get; set; }
    public decimal Quantity { get; set; }
    public long CalcEstimate { get; set; }
    public string TradeType { get; set; }
}
