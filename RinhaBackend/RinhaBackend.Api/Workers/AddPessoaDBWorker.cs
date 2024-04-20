using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Interfaces;
using System.Collections.Concurrent;

namespace RinhaBackend.Api.Workers
{
    public class AddPessoaDBWorker : BackgroundService
    {

        private readonly ILogger<AddPessoaDBWorker> _logger;
        private readonly ConcurrentQueue<PessoaEntity> _queuePessoas;
        private readonly IPessoaRepository _repository;
        private const int DELAY_INSERT = 5;

        public AddPessoaDBWorker(ConcurrentQueue<PessoaEntity> queuePessoas, IPessoaRepository repository, ILogger<AddPessoaDBWorker> logger)
        {
            _queuePessoas = queuePessoas;
            _repository = repository;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                await Task.Delay(TimeSpan.FromSeconds(DELAY_INSERT));

                var pessoas = new List<PessoaEntity>();
                
                PessoaEntity pessoa;
                while (_queuePessoas.TryDequeue(out pessoa))
                    pessoas.Add(pessoa);

                if (pessoas == null || pessoas.Count == 0)
                    continue;

                try
                {
                    await _repository.Add(pessoas);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }
    }
}
