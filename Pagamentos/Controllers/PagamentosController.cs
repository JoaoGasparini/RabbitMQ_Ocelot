using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Vendas.Models;

namespace Pagamentos.Controllers
{
    public class PagamentosController : Controller
    {
        private readonly IBus _bus;
        private IConfiguration _configuration;

        public PagamentosController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("/processar-pagamentos")]
        public async Task<IActionResult> ProcessarPagamentosPost([FromBody] VendaBilhete VendaProcessada)
        {
            var nomeFila = _configuration.GetSection("MassTransit")["ProcessarPagamentosFila"];
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

            await endpoint.Send(VendaProcessada);

            return Ok();
        }
    }
}
