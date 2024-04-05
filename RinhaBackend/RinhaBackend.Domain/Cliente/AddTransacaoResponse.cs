namespace RinhaBackend.Domain.Cliente
{
    public class AddTransacaoResponse
    {
        public AddTransacaoResponse(int limite, int saldo)
        {
            Limite = limite;
            Saldo = saldo;
        }

        public int Limite { get; private set; }
        public int Saldo { get; private set; }
    }
}
