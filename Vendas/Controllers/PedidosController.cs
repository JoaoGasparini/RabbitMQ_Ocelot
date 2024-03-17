using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendas.Models;

namespace Vendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly IBus _bus;
        private IConfiguration _configuration;
        public PedidosController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/vendas")]
        public async Task<IActionResult> VendaPost()
        {
            var nomeFila = _configuration.GetSection("MassTransit")["VendaEfetuadaFila"];

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

            List<string> nomes = new() { "João", "Alberto", "Fernando", "Jessica" };
            List<string> shows = new() { "Filme", "Musica", "Teatro", "Show" };
          
            var rnd = new Random();

            var venda = new VendaBilhete(shows[rnd.Next(shows.Count)], 2 * rnd.Next(), new Usuario(nomes[rnd.Next(nomes.Count)]));

            await endpoint.Send(venda);

            return Ok(venda);
        }
    }
}
