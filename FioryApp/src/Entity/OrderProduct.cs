namespace FioryApp.Entity;
#nullable enable
public class OrderProduct
{
    public string? irecurso { get; set; }
    public string? itiporec { get; set; }
    public string? icc { get; set; }
    public string? sobserv { get; set; }
    public string? dato1 { get; set; }
    public string? dato2 { get; set; }
    public string? dato3 { get; set; }
    public string? dato4 { get; set; }
    public string? dato5 { get; set; }
    public string? dato6 { get; set; }
    public Int32 iinventario { get; set; } = 0;
    public Int32 qrecurso { get; set; } = 0;
    public decimal mprecio { get; set; }
    public decimal qporcdescuento { get; set; }
    public decimal qporciva { get; set; }
    public decimal mvrtotal { get; set; }
    public decimal valor1 { get; set; }
    public decimal valor2 { get; set; }
    public decimal valor3 { get; set; }
    public decimal valor4 { get; set; }
    public string? qrecurso2 { get; set; }
}