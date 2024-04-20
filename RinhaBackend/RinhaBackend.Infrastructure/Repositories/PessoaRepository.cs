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

        private NpgsqlConnection _conn;

        public PessoaRepository(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        private async Task ConnectDB() 
        {
            if (_conn.State != ConnectionState.Open) 
            {
                await _conn.OpenAsync();
            }
        }
        public async Task Add(PessoaEntity pessoa)
        {
            await ConnectDB();

            var sql = new StringBuilder();

            sql.AppendLine("insert into pessoas");
            sql.AppendLine("(id, apelido, nome, nascimento, stack)");
            sql.AppendLine("values");
            sql.AppendLine("(@id, @apelido, @nome, @nascimento, @stack)");

            var comm = _conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("id", pessoa.Id));
            comm.Parameters.Add(new NpgsqlParameter("apelido", pessoa.Apelido));
            comm.Parameters.Add(new NpgsqlParameter("nome", pessoa.Nome));
            comm.Parameters.Add(new NpgsqlParameter("nascimento", pessoa.Nascimento));
            comm.Parameters.Add(new NpgsqlParameter("stack", pessoa.Stack));

            await comm.ExecuteNonQueryAsync();
        }


        public async Task Add(List<PessoaEntity> pessoas)
        {
            await ConnectDB();

            var batch = _conn.CreateBatch();
            var batchCommands = new List<NpgsqlBatchCommand>();

            foreach (var p in pessoas)
            {
                var batchCmd = new NpgsqlBatchCommand(@"
                        insert into pessoas
                        (id, apelido, nome, nascimento, stack)
                        values ($1, $2, $3, $4, $5)
                        on conflict do nothing;
                    ");
                batchCmd.Parameters.AddWithValue(p.Id);
                batchCmd.Parameters.AddWithValue(p.Apelido);
                batchCmd.Parameters.AddWithValue(p.Nome);
                batchCmd.Parameters.AddWithValue(p.Nascimento);
                batchCmd.Parameters.AddWithValue(p.Stack);
                batch.BatchCommands.Add(batchCmd);

            }

            await batch.ExecuteNonQueryAsync();
        }

        public async Task<PessoaEntity> Get(Guid id)
        {
            await ConnectDB();

            var pessoa = new PessoaEntity();
            var sql = new StringBuilder();

            sql.AppendLine("select id, apelido, nome, nascimento, stack");
            sql.AppendLine("from pessoas");
            sql.AppendLine("where id = @id");

            var comm = _conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("id", id));
            
            var result = await comm.ExecuteReaderAsync();

            while (result.Read())
            {
                pessoa.Id = (Guid)result["id"];
                pessoa.Nome = result["Nome"].ToString();
                pessoa.Nascimento = (DateTime)result["nascimento"];
                pessoa.Stack = result["stack"].ToString();
                pessoa.Apelido = result["apelido"].ToString();
            }

            result.Close();

            return pessoa;
        }

        public async Task<PessoaEntity> Get(string apelido)
        {
            await ConnectDB();

            PessoaEntity pessoa = null;
            var sql = new StringBuilder();

            sql.AppendLine("select id, apelido, nome, nascimento, stack");
            sql.AppendLine("from pessoas");
            sql.AppendLine("where apelido = @apelido");

            var comm = _conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("apelido", apelido));

            var result = await comm.ExecuteReaderAsync();

            while (result.Read())
            {
                pessoa = new PessoaEntity();

                pessoa.Id = (Guid)result["id"];
                pessoa.Nome = result["Nome"].ToString();
                pessoa.Nascimento = (DateTime)result["nascimento"];
                pessoa.Stack = result["stack"].ToString();
                pessoa.Apelido = result["apelido"].ToString();
            }

            result.Close();

            return pessoa;
        }

        public async Task<long> GetQuantidadePessoas()
        {
            await ConnectDB();

            long quantidade = 0;
            var sql = new StringBuilder();

            sql.AppendLine("select count(1) quantidade");
            sql.AppendLine("from pessoas");

            var comm = _conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;

            var result = await comm.ExecuteReaderAsync();

            while (result.Read())
            {
                quantidade = (long)result["quantidade"];
            }
            
            result.Close();

            return quantidade;
        }

        public async Task<IEnumerable<PessoaEntity>> List(string termo)
        {
            await ConnectDB();

            var pessoas = new List<PessoaEntity>();
            var sql = new StringBuilder();

            sql.AppendLine("select id, apelido, nome, nascimento, stack");
            sql.AppendLine("from pessoas");
            sql.AppendLine("where pesquisa like @termo");
            sql.AppendLine("limit 50");

            var comm = _conn.CreateCommand();

            comm.CommandText = sql.ToString();
            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new NpgsqlParameter("termo", $"%{termo}%"));

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

            result.Close();

            return pessoas;
        }


        public void Dispose()
        {
            if(_conn.State == ConnectionState.Open)
                _conn.Close();
            _conn.Dispose();
        }

    }
}
