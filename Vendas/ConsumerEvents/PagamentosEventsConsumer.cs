using MassTransit;
using Vendas.Models;

namespace Vendas.ConsumerEvents
{
    public class PagamentosEventsConsumer : IConsumer<VendaBilhete>
    {
        public Task Consume(ConsumeContext<VendaBilhete> context)
        {
            Console.WriteLine(context.Message.ToString());

            return Task.CompletedTask;
        }
    }
}
