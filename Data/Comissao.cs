namespace MinhaApiHub.Data;

public class Comissao
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int NumeroApolice { get; set; }
    public string TipoSeguro { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Premio { get; set; }
    public decimal ComissaoValor { get; set; }
    public string Status { get; set; } = string.Empty;

    public Cliente? Cliente { get; set; }
} 