namespace MinhaApiHub.Data;

public class Cotacao
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string TipoSeguro { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = string.Empty;

    public Cliente? Cliente { get; set; }
} 