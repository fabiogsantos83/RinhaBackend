namespace RinhaBackend.Domain.Cliente
{
    public class GetExtratoResponse
    {
        public GetExtratoSaldoResponse Saldo { get; set; }
        public List<GetExtratoTransacoesResponse> UltimasTransacoes { get; set; }
    }

    public class GetExtratoSaldoResponse
    {
        public int Total  { get; set; }
        public DateTime DataExtrato { get; set; }
        public int Limite { get; set;}
    }

    public class GetExtratoTransacoesResponse
    { 
        public int Valor { get; set; }
        public char Tipo { get; set; }
        public string Descricao { get; set; }
        public string RealizadaEm { get; set; }

    }
}
