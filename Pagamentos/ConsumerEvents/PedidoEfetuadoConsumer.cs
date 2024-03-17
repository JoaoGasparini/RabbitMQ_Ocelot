using Classes;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;
using Vendas.Models;

namespace Pagamentos.ConsumerEvents
{
    public class PedidoEfetuadoConsumer : IConsumer<VendaBilhete>
    {
        public Task Consume(ConsumeContext<VendaBilhete> context)
        {
            Console.WriteLine("Pagamento Recebido - Processando");

            Random random = new Random();
            var chance = random.NextDouble();

            if (chance < 0.2)
                context.Message.PagamentosStatus = EPaymentStatus.Pago;
            else if (chance >= 0.2 && chance < 0.4)
                context.Message.PagamentosStatus = EPaymentStatus.Falha;
            else if (chance >= 0.4 && chance < 0.6)
                context.Message.PagamentosStatus = EPaymentStatus.SenhaIncorreta;
            else
                context.Message.PagamentosStatus = EPaymentStatus.CartaoRecusado;

            using (var httpClient = new HttpClient())
                try
                {
                    var httpContent = new StringContent(JsonSerializer.Serialize(context),
                        System.Text.Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync("https://localhost:7138/processar-pagamentos", httpContent);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro: {ex.Message}");
                }

            return Task.CompletedTask;
        }
    }
}
