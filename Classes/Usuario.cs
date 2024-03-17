namespace Vendas.Models
{
    public class Usuario
    {
        public Usuario()
        {
            
        }
        public Usuario(string nome)
        {
            Nome = nome;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Token { get; set; }
        public string? Nome { get; set; } 
    }
}
