using Microsoft.Data.Sqlite;         
using DesafioWeb.Models;

namespace DesafioWeb.Data
{
    public interface IDatabase
    {
        void CriarTabela();
        void Inserir(Fundacao f);
        Fundacao? BuscarPorCNPJ(string cnpj);
        void Atualizar(Fundacao f);
        void Excluir(string cnpj);
        List<Fundacao> ListarTodas();
    }

    /// <summary>
    /// Classe responsável por toda a comunicação com o banco SQLite.
    /// O arquivo fundacoes.db será criado automaticamente no diretório de execução.
    /// </summary>
    public class Database : IDatabase
    {
        // Conexão: usa o diretório atual da aplicação para criar/abrir fundacoes.db
        // Isso normalmente coloca o arquivo na raiz do projeto quando rodado com "dotnet run" a partir da raiz.
        private string ConnectionString => $"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "fundacoes.db")}";

        public Database()
        {
            // Garante que a tabela exista quando a instância é criada (útil para DI singleton)
            try { CriarTabela(); } catch { /* ignorar erros de criação em ambiente de teste */ }
        }

        /// <summary>
        /// Cria a tabela Fundacoes se ela não existir.
        /// Chame este método na inicialização da aplicação para garantir que a estrutura exista.
        /// </summary>
        public void CriarTabela()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Fundacoes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    CNPJ TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL,
                    Telefone TEXT NOT NULL,
                    InstituicaoApoiada TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Insere uma nova fundação. Não permite duplicar CNPJ (constraint UNIQUE).
        /// </summary>
        public void Inserir(Fundacao f)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Fundacoes (Nome, CNPJ, Email, Telefone, InstituicaoApoiada)
                VALUES ($nome, $cnpj, $email, $telefone, $instituicao);
            ";

            cmd.Parameters.AddWithValue("$nome", f.Nome);
            cmd.Parameters.AddWithValue("$cnpj", f.CNPJ);
            cmd.Parameters.AddWithValue("$email", f.Email);
            cmd.Parameters.AddWithValue("$telefone", f.Telefone);
            cmd.Parameters.AddWithValue("$instituicao", f.InstituicaoApoiada);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Busca uma fundação pelo CNPJ. Retorna null se não encontrar.
        /// </summary>
        public Fundacao? BuscarPorCNPJ(string cnpj)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, CNPJ, Email, Telefone, InstituicaoApoiada FROM Fundacoes WHERE CNPJ = $cnpj;";
            cmd.Parameters.AddWithValue("$cnpj", cnpj);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Fundacao
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    CNPJ = reader.GetString(2),
                    Email = reader.GetString(3),
                    Telefone = reader.GetString(4),
                    InstituicaoApoiada = reader.GetString(5)
                };
            }

            return null;
        }

        /// <summary>
        /// Atualiza os campos (exceto o CNPJ, que usamos como identificador aqui).
        /// </summary>
        public void Atualizar(Fundacao f)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Fundacoes
                SET Nome = $nome,
                    Email = $email,
                    Telefone = $telefone,
                    InstituicaoApoiada = $instituicao
                WHERE CNPJ = $cnpj;
            ";

            cmd.Parameters.AddWithValue("$nome", f.Nome);
            cmd.Parameters.AddWithValue("$email", f.Email);
            cmd.Parameters.AddWithValue("$telefone", f.Telefone);
            cmd.Parameters.AddWithValue("$instituicao", f.InstituicaoApoiada);
            cmd.Parameters.AddWithValue("$cnpj", f.CNPJ);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Exclui a fundação com o CNPJ informado.
        /// </summary>
        public void Excluir(string cnpj)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Fundacoes WHERE CNPJ = $cnpj;";
            cmd.Parameters.AddWithValue("$cnpj", cnpj);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Lista todas as fundações (uso útil para uma página de listagem).
        /// </summary>
        public List<Fundacao> ListarTodas()
        {
            var lista = new List<Fundacao>();

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, CNPJ, Email, Telefone, InstituicaoApoiada FROM Fundacoes;";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Fundacao
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    CNPJ = reader.GetString(2),
                    Email = reader.GetString(3),
                    Telefone = reader.GetString(4),
                    InstituicaoApoiada = reader.GetString(5)
                });
            }

            return lista;
        }
    }
}
