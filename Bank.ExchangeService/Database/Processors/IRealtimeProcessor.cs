using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Database.Processors;

public interface IRealtimeProcessor
{
    Task ProcessStockQuotes(List<Quote> quotes) => Task.CompletedTask;

    Task ProcessForexQuotes(List<Quote> quotes) => Task.CompletedTask;

    Task ProcessOptionQuotes(List<Quote> quotes) => Task.CompletedTask;

    Task ProcessFutureQuotes(List<Quote> quotes) => Task.CompletedTask;
}
