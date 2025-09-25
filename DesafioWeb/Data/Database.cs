using Microsoft.Data.Sqlite;
using DesafioWeb.Models;

namespace DesafioWeb.Data
{
    public class Database
    {
        private string connectionString = "Data Source=fundacoes.db";

        public void CriarTabela()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Fundacoes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    CNPJ TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL,
                    Telefone TEXT,
                    InstituicaoApoiada TEXT
                );
            ";
            cmd.ExecuteNonQuery();
        }

        public void Inserir(Fundacao f)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
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
    }
}
