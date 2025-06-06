using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.ExchangeService.Models;
using Bank.ExchangeService.Repositories;
using Bank.Http.Clients.User;

namespace Bank.ExchangeService.Database.Processors;

public class OrderRealtimeProcessor(IRedisRepository redisRepository, IUserServiceHttpClient userServiceHttpClient, IOrderRepository orderRepository) : IRealtimeProcessor
{
    private readonly IRedisRepository       m_RedisRepository       = redisRepository;
    private readonly IOrderRepository       m_OrderRepository       = orderRepository;
    private readonly IUserServiceHttpClient m_UserServiceHttpClient = userServiceHttpClient;
    private          Guid                   m_SecurityTransactionCodeId;

    public async Task OnApplicationStarted(CancellationToken cancellationToken)
    {
        var transactionCodeFilter = new TransactionCodeFilterQuery
                                    {
                                        Code = "280"
                                    };

        var transactionCodePageTask = m_UserServiceHttpClient.GetAllTransactionCodes(transactionCodeFilter, Pageable.Create(1, 1));

        await Task.WhenAll(transactionCodePageTask);

        var transactionCodePage = await transactionCodePageTask;

        m_SecurityTransactionCodeId = transactionCodePage.Items[0].Id;
    }

    public async Task ProcessStockQuotes(List<Quote> quotes)
    {
        var stockOrders = await m_RedisRepository.FindAllStockOrders();

        var orderQuoteList = quotes.GroupJoin(stockOrders, quote => quote.Security!.Ticker, order => order.Ticker, (quote, orders) => new { quote, orders })
                                   .SelectMany(group => group.orders.Select(order => new { group.quote, order }))
                                   .Where(pair => pair.order.OrderType is not OrderType.Market || pair.order.Direction is not Direction.Buy  || true)
                                   .Where(pair => pair.order.OrderType is not OrderType.Market || pair.order.Direction is not Direction.Sell || true)
                                   .Where(pair => pair.order.OrderType is not OrderType.Limit || pair.order.Direction is not Direction.Buy ||
                                                  pair.quote.BidPrice <= pair.order.LimitPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.Limit || pair.order.Direction is not Direction.Sell ||
                                                  pair.quote.AskPrice >= pair.order.LimitPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.Stop || pair.order.Direction is not Direction.Buy ||
                                                  pair.quote.BidPrice >= pair.order.StopPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.Stop || pair.order.Direction is not Direction.Sell ||
                                                  pair.quote.AskPrice <= pair.order.StopPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.StopLimit || pair.order.Direction is not Direction.Buy ||
                                                  (pair.quote.BidPrice >= pair.order.StopPrice && pair.quote.AskPrice <= pair.order.LimitPrice))
                                   .Where(pair => pair.order.OrderType is not OrderType.StopLimit || pair.order.Direction is not Direction.Sell ||
                                                  (pair.quote.AskPrice <= pair.order.StopPrice && pair.quote.BidPrice >= pair.order.LimitPrice))
                                   .GroupBy(pair => pair.quote)
                                   .Select(group => (quote: group.Key, group.Select(pair => pair.order)
                                                                            .ToList()))
                                   .ToList();

        foreach (var (quote, orders) in orderQuoteList)
        {
            RedisOrder currentOrder;

            var sellOrders = orders.Where(order => order.Direction is Direction.Sell)
                                   .ToList();

            for (int index = 0; index < sellOrders.Count;)
            {
                currentOrder = sellOrders[index];

                if (quote.BidSize >= currentOrder.RemainingPortions)
                {
                    quote.BidSize -= currentOrder.RemainingPortions;
                    index++;
                }
                else
                {
                    sellOrders.RemoveAt(index);
                }
            }

            var buyOrders = orders.Where(order => order.Direction is Direction.Buy)
                                  .ToList();

            for (int index = 0; index < buyOrders.Count; index++)
            {
                currentOrder = buyOrders[index];

                if (quote.AskSize >= currentOrder.RemainingPortions)
                {
                    quote.AskSize -= currentOrder.RemainingPortions;
                    index++;
                }
                else
                {
                    buyOrders.RemoveAt(index);
                }
            }
        }

        await ExecuteQuoteOrders(orderQuoteList);
    }

    public async Task ProcessForexQuotes(List<Quote> quotes) //TODO: check for values
    {
        var stockOrders = await m_RedisRepository.FindAllStockOrders();

        var orderQuoteList = quotes.GroupJoin(stockOrders, quote => quote.Security!.Ticker, order => order.Ticker, (quote, orders) => new { quote, orders })
                                   .SelectMany(group => group.orders.Select(order => new { group.quote, order }))
                                   .Where(pair => pair.order.OrderType is not OrderType.Market || pair.order.Direction is not Direction.Buy  || true)
                                   .Where(pair => pair.order.OrderType is not OrderType.Market || pair.order.Direction is not Direction.Sell || true)
                                   .Where(pair => pair.order.OrderType is not OrderType.Limit || pair.order.Direction is not Direction.Buy ||
                                                  pair.quote.BidPrice <= pair.order.LimitPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.Limit || pair.order.Direction is not Direction.Sell ||
                                                  pair.quote.AskPrice >= pair.order.LimitPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.Stop || pair.order.Direction is not Direction.Buy ||
                                                  pair.quote.BidPrice >= pair.order.StopPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.Stop || pair.order.Direction is not Direction.Sell ||
                                                  pair.quote.AskPrice <= pair.order.StopPrice)
                                   .Where(pair => pair.order.OrderType is not OrderType.StopLimit || pair.order.Direction is not Direction.Buy ||
                                                  (pair.quote.BidPrice >= pair.order.StopPrice && pair.quote.AskPrice <= pair.order.LimitPrice))
                                   .Where(pair => pair.order.OrderType is not OrderType.StopLimit || pair.order.Direction is not Direction.Sell ||
                                                  (pair.quote.AskPrice <= pair.order.StopPrice && pair.quote.BidPrice >= pair.order.LimitPrice))
                                   .GroupBy(pair => pair.quote)
                                   .Select(group => (quote: group.Key, group.Select(pair => pair.order)
                                                                            .ToList()))
                                   .ToList();

        foreach (var (quote, orders) in orderQuoteList)
        {
            RedisOrder currentOrder;

            var sellOrders = orders.Where(order => order.Direction is Direction.Sell)
                                   .ToList();

            for (int index = 0; index < sellOrders.Count;)
            {
                currentOrder = sellOrders[index];

                if (quote.BidSize >= currentOrder.RemainingPortions)
                {
                    quote.BidSize -= currentOrder.RemainingPortions;
                    index++;
                }
                else
                {
                    sellOrders.RemoveAt(index);
                }
            }

            var buyOrders = orders.Where(order => order.Direction is Direction.Buy)
                                  .ToList();

            for (int index = 0; index < buyOrders.Count; index++)
            {
                currentOrder = buyOrders[index];

                if (quote.AskSize >= currentOrder.RemainingPortions)
                {
                    quote.AskSize -= currentOrder.RemainingPortions;
                    index++;
                }
                else
                {
                    buyOrders.RemoveAt(index);
                }
            }
        }

        await ExecuteQuoteOrders(orderQuoteList);
    }

    private async Task ExecuteQuoteOrders(List<(Quote quote, List<RedisOrder> orders)> orderQuoteList)
    {
        var accountIds = orderQuoteList.SelectMany(pair => pair.orders)
                                       .Select(order => order.AccountId)
                                       .Distinct()
                                       .ToList();

        var accountFilter = new AccountFilterQuery
                            {
                                Ids = accountIds
                            };

        var accountResponsePage = await m_UserServiceHttpClient.GetAllAccounts(accountFilter, Pageable.Create(1, accountIds.Count));

        // @formatter:off
        await Task.WhenAll(orderQuoteList.SelectMany(pair => pair.orders.Select(order => (pair.quote, order)).ToList())
                                         .GroupJoin(accountResponsePage.Items, orderQuote => orderQuote.order.AccountId, accountResponse => accountResponse.Id,
                                                    (orderQuote, accountResponses) => new { orderQuote.quote, orderQuote.order, accountResponses })
                                         .SelectMany(group => group.accountResponses.Select(accountResponse => new { group.quote, group.order, accountResponse }))
                                         .Select(triple => m_UserServiceHttpClient.CreateTransaction(new TransactionCreateRequest
                                                                                                     {
                                                                                                         FromAccountNumber = triple.order.Direction == Direction.Buy ? triple.accountResponse.AccountNumber : null,
                                                                                                         FromCurrencyId    = triple.order.Direction == Direction.Buy ? triple.accountResponse.Currency.Id : triple.quote.Security!.StockExchange!.CurrencyId,
                                                                                                         ToAccountNumber   = triple.order.Direction == Direction.Sell ? triple.accountResponse.AccountNumber : null,
                                                                                                         ToCurrencyId      = triple.order.Direction == Direction.Sell ? triple.accountResponse.Currency.Id : triple.quote.Security!.StockExchange!.CurrencyId,
                                                                                                         Amount            = triple.order.Direction == Direction.Buy ? triple.order.RemainingPortions * triple.quote.AskPrice
                                                                                                                             : triple.order.RemainingPortions                                         * triple.quote.BidPrice,
                                                                                                         CodeId  = m_SecurityTransactionCodeId,
                                                                                                         Purpose = "Execute Order"
                                                                                                     })));
        // @formatter:on

        await m_OrderRepository.UpdateStatus(orderQuoteList.SelectMany(pair => pair.orders)
                                                           .Select(order => order.Id)
                                                           .ToList(), OrderStatus.Completed);
    }
}
