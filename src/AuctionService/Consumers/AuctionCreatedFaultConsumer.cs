using Contracts;
using MassTransit;

namespace SearchService.Consumers;

public class AuctionCreatedFaultConsumer {
    public async Task Cosume(ConsumeContext<Fault<AuctionCreated>> ctx) {
        Console.WriteLine("->> Faulty consume");
        var exception = ctx.Message.Exceptions.First();
        if (exception.ExceptionType == "System.ArgumentException") {
            ctx.Message.Message.Model = "Foobar";
            await ctx.Publish(ctx.Message.Message);
        }
        else {
            Console.WriteLine("Not an argument exception");
        }
    }
}