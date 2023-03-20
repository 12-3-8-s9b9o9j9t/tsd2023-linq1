using GoldSaving.Lib;
using GoldSaving.Lib.Model;

// See https://aka.ms/new-console-template for more information

GoldClient goldClient = new GoldClient();

Console.WriteLine("What are the TOP 3 highest and TOP 3 lowest prices of gold within the last year?");

List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31))
    .GetAwaiter()
    .GetResult();

Console.WriteLine("Query Syntax:");

var top3 = (from gold in lastYearPrices
            orderby gold.Price descending
            select gold).Take(3).Union(
    (from gold in lastYearPrices
     orderby gold.Price descending
     select gold).TakeLast(3));

foreach (GoldPrice gold in top3)
{
    Console.WriteLine(gold.Price);
}

Console.WriteLine("Method Syntax:");

top3 = lastYearPrices.OrderByDescending(gold => gold.Price)
    .Take(3)
    .Union(lastYearPrices.OrderByDescending(gold => gold.Price)
    .TakeLast(3));

foreach (GoldPrice gold in top3)
{
    Console.WriteLine(gold.Price);
}

Console.WriteLine("\nIf one would have bought gold in January 2020, is it possible that they earned more then 5%?" +
    "\nOn which days?");

List<GoldPrice> jan2020 = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 01, 31))
    .GetAwaiter()
    .GetResult();

Console.WriteLine("Query Syntax:");

var dates = from buy in jan2020
            from sell in jan2020
            where sell.Date == new DateTime(2020, 01, 31) && buy.Date < sell.Date && sell.Price > 1.05 * buy.Price
            select new { BuyDate = buy.Date, Profit = (sell.Price / buy.Price * 100 - 100) };


foreach (var date in dates)
{
    Console.WriteLine($"Buy: {date.BuyDate.ToShortDateString()}, Profit: {Math.Round(date.Profit, 2)}%");
}

Console.WriteLine("Method Syntax:");

dates = jan2020.SelectMany(buy => jan2020.Where(sell => sell.Date == new DateTime(2020, 01, 31) && buy.Date < sell.Date && sell.Price > 1.05 * buy.Price),
       (buy, sell) => new { BuyDate = buy.Date, Profit = (sell.Price / buy.Price * 100 - 100) });

foreach (var date in dates)
{
    Console.WriteLine($"Buy: {date.BuyDate.ToShortDateString()}, Profit: {Math.Round(date.Profit, 2)}%");
}

Console.WriteLine("\nWhich 3 dates of 2022-2019 opens the second ten of the prices ranking?");

List<GoldPrice> prices = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31))
    .GetAwaiter()
    .GetResult()
    .Concat(goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31))
        .GetAwaiter()
        .GetResult())
    .Concat(goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31))
        .GetAwaiter()
        .GetResult())
    .Concat(goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31))
        .GetAwaiter()
        .GetResult())
    .ToList();

Console.WriteLine("Query Syntax:");

var open = (from price in prices
            orderby price.Price descending
            select price.Date)
           .Skip(10)
           .Take(3);

foreach (var date in open)
{
    Console.WriteLine(date.ToShortDateString());
}

Console.WriteLine("Method Syntax:");

open = prices.OrderByDescending(price => price.Price)
    .Select(price => price.Date)
    .Skip(10)
    .Take(3);

foreach (var date in open)
{
    Console.WriteLine(date.ToShortDateString());
}
