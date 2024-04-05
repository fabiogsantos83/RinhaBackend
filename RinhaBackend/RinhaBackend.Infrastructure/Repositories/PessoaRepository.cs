using Microsoft.Extensions.Configuration;
using Npgsql;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Interfaces;
using System.Data;
using System.Text;

namespace RinhaBackend.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository, IDisposable
    {

        private NpgsqlConnection conn;

        public PessoaRepository(IConfiguration config)
        {
            conn = new NpgsqlConnection(config.GetConnectionString("rinha"));
        }

        public Task Add(PessoaEntity pessoa)
        {
            var sql = new StringBuilder();

            sql.AppendLine("insert into pessoas");
            sql.AppendLine("(id, apelido, nome, nascimento, stack)");
            sql.AppendLine("values");
            sql.AppendLine("(@id, @apelido, @nome, @nascimento, @stack)");

            var comm = conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("id", pessoa.Id));
            comm.Parameters.Add(new NpgsqlParameter("apelido", pessoa.Apelido));
            comm.Parameters.Add(new NpgsqlParameter("nome", pessoa.Nome));
            comm.Parameters.Add(new NpgsqlParameter("nascimento", pessoa.Nascimento));
            comm.Parameters.Add(new NpgsqlParameter("stack", pessoa.Stack));

            conn.Open();

            return comm.ExecuteNonQueryAsync();
        }

        public async Task<PessoaEntity> Get(Guid id)
        {
            var pessoa = new PessoaEntity();
            var sql = new StringBuilder();

            sql.AppendLine("select id, apelido, nome, nascimento, stack");
            sql.AppendLine("from pessoas");
            sql.AppendLine("where id = @id");

            var comm = conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("id", id));

            conn.Open();

            var result = await comm.ExecuteReaderAsync();

            while (result.Read())
            {
                pessoa.Id = (Guid)result["id"];
                pessoa.Nome = result["Nome"].ToString();
                pessoa.Nascimento = (DateTime)result["nascimento"];
                pessoa.Stack = result["stack"].ToString();
                pessoa.Apelido = result["apelido"].ToString();
            }

            return pessoa;
        }

        public async Task<long> GetQuantidadePessoas()
        {
            long quantidade = 0;
            var sql = new StringBuilder();

            sql.AppendLine("select count(1) quantidade");
            sql.AppendLine("from pessoas");

            var comm = conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;

            conn.Open();

            var result = await comm.ExecuteReaderAsync();

            while (result.Read())
            {
                quantidade = (long)result["quantidade"];
            }

            return quantidade;
        }

        public async Task<IEnumerable<PessoaEntity>> List(string termo)
        {
            var pessoas = new List<PessoaEntity>();
            var sql = new StringBuilder();

            sql.AppendLine("select id, apelido, nome, nascimento, stack");
            sql.AppendLine("from pessoas");
            sql.AppendLine("where apelido like @termo");
            sql.AppendLine("or nome like @termo");
            sql.AppendLine("or stack like @termo");
            sql.AppendLine("limit 50");

            var comm = conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("termo", $"%{termo}%"));

            conn.Open();

            var result = await comm.ExecuteReaderAsync();

            while (result.Read())
            {
                pessoas.Add(new PessoaEntity()
                {
                    Id = (Guid)result["id"],
                    Nome = result["Nome"].ToString(),
                    Nascimento = (DateTime)result["nascimento"],
                    Stack = result["stack"].ToString(),
                    Apelido = result["apelido"].ToString()
                });
            }

            return pessoas;
        }


        public void Dispose()
        {
            conn.Dispose();
        }

    }
}
