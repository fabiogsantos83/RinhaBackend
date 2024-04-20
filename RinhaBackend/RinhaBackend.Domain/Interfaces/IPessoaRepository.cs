using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Domain.Interfaces
{
    public interface IPessoaRepository
    {
        Task Add(PessoaEntity pessoa);
        Task Add(List<PessoaEntity> pessoas);
        Task<PessoaEntity> Get(Guid id);
        Task<long> GetQuantidadePessoas();
        Task<IEnumerable<PessoaEntity>> List(string termo);
        Task<PessoaEntity> Get(string apelido);
    }
}
