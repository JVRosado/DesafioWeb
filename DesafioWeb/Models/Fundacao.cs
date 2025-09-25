// Models/Fundacao.cs
namespace DesafioWeb.Models
{
    // Representa a entidade Fundação (uma linha na tabela Fundacoes)
    public class Fundacao
    {
        public int Id { get; set; }                         // PK, gerado pelo SQLite
        public required string Nome { get; set; }           // Obrigatório
        public required string CNPJ { get; set; }           // Obrigatório e único
        public required string Email { get; set; }          // Obrigatório
        public required string Telefone { get; set; }       // Obrigatório
        public required string InstituicaoApoiada { get; set; } // Obrigatório
    }
}
