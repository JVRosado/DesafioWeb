namespace DesafioWeb.Models
{
    public class Fundacao
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string CNPJ { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public string? InstituicaoApoiada { get; set; }
    }
}
