namespace RinhaBackend.Domain.Cliente
{
    public class AddTransacaoRequest
    {
        public AddTransacaoRequest(int valor, char tipo, string descricao)
        {
            Valor = valor;
            Tipo = tipo;
            Descricao = descricao;
        }
        public int Valor { get; private set; }
        public char Tipo  { get; private set; }
        public string Descricao { get; private set; }
    }
}
