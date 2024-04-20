using FluentValidation;
using RinhaBackend.Domain.Commands;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Globalization;

namespace RinhaBackend.Application
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _repository;
        private readonly IValidator<AddPessoaRequest> _validator;
        private readonly ConcurrentQueue<PessoaEntity> _queuePessoa;
        public PessoaService(IPessoaRepository repository, IValidator<AddPessoaRequest> validator, ConcurrentQueue<PessoaEntity> queuePessoa)
        {
            _repository = repository;
            _validator = validator;
            _queuePessoa = queuePessoa;
        }
        public async Task<Guid> AddPessoa(AddPessoaRequest request)
        {
            var id = Guid.NewGuid();

            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var pessoa = await _repository.Get(request.Apelido);

            if (pessoa != null) 
            {
                throw new ValidationException("Pessoa já cadastrada");
            }

            var pessoaEntity = new PessoaEntity()
            {
                Apelido = request.Apelido,
                Nascimento = Convert.ToDateTime(request.Nascimento),
                Nome = request.Nome,
                Stack = String.Join(",", request.Stack),
                Id = id
            };

            //_queuePessoa.aEnqueue(pessoaEntity);
            await _repository.Add(pessoaEntity);

            return id;
        }

        public async Task<GetPessoasResponse> GetPessoa(Guid id)
        {
            var pessoaEntity = await _repository.Get(id);

            var stackSplit = pessoaEntity.Stack.Split(',');
            var stack = new List<string>();

            stackSplit.ToList().ForEach(x =>
            {
                stack.Add(x);
            });

            return new GetPessoasResponse()
            {
                Apelido = pessoaEntity.Apelido,
                Id = id,
                Nascimento = pessoaEntity.Nascimento,
                Nome = pessoaEntity.Nome,
                Stack = stack
            };
        }

        public async Task<long> GetQuantidadePessoas()
        {
            var quantidade = await _repository.GetQuantidadePessoas();
            return quantidade;
        }

        public async Task<IEnumerable<GetPessoasResponse>> ListPessoas(string termo)
        {
            var response = new List<GetPessoasResponse>();
            var pessoasEntity = await _repository.List(termo);


            pessoasEntity.ToList().ForEach(x =>
            {

                var stackSplit = x.Stack.Split(',');
                var stack = new List<string>();

                stackSplit.ToList().ForEach(x =>
                {
                    stack.Add(x);
                });

                response.Add(new GetPessoasResponse()
                {
                    Apelido = x.Apelido,
                    Id = x.Id,
                    Nascimento = x.Nascimento,
                    Nome = x.Nome,
                    Stack = stack
                });
            });

            return response;

        }

    }
}