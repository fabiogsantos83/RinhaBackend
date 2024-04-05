using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Domain.Interfaces
{
    public interface IPessoaRepository
    {
        Task Add(PessoaEntity pessoa);
        Task<PessoaEntity> Get(Guid id);
        Task<long> GetQuantidadePessoas();
        Task<IEnumerable<PessoaEntity>> List(string termo);
    }
}
