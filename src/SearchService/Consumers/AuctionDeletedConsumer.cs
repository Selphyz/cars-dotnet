using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted> {
    public async Task Consume(ConsumeContext<AuctionDeleted> ctx) {
        Console.WriteLine("->> Consuming AuctionDeleted" + ctx.Message.Id);
        var result = await DB.DeleteAsync<Item>(ctx.Message.Id);
        if (!result.IsAcknowledged) throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
    }
}