using Classes;

namespace Vendas.Models
{
    public class VendaBilhete
    {
        public VendaBilhete()
        {
            
        }
        public VendaBilhete(string _Nome, decimal _Valor, Usuario usuario)
        {
            Nome = _Nome;
            Valor = _Valor;
            Usuario = usuario;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Nome { get; set; } = null;
        public decimal Valor { get; set; }
        public Usuario Usuario { get; set; }
        public EPaymentStatus PagamentosStatus { get; set; } = EPaymentStatus.AguardandoPagamento;

        public override string ToString()
        {
            return $"Nome Venda: {Nome} - Nome Usuario: {Usuario.Nome} - Valor: {Valor} - PagamentoStatus: {PagamentosStatus}";
        }
    }
}
