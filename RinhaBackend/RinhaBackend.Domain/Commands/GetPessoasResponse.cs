namespace RinhaBackend.Domain.Commands
{
    public class GetPessoasResponse
    {
        public Guid Id { get; set; }
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public IEnumerable<string> Stack { get; set; }
    }
}
