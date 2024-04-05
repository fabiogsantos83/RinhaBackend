using RinhaBackend.Domain.Commands;

namespace RinhaBackend.Domain.Interfaces
{
    public interface IPessoaService
    {
        Task<Guid> AddPessoa(AddPessoaRequest request);
        Task<GetPessoasResponse> GetPessoa(Guid id);
        Task<long> GetQuantidadePessoas();
        Task<IEnumerable<GetPessoasResponse>> ListPessoas(string termo);
    }
}
