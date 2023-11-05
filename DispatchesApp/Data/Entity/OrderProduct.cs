namespace DispatchesApp.Entity;
#nullable enable
public class OrderProduct
{
    public string? irecurso { get; init; }
    public string? itiporec { get; init; }
    public string? icc { get; init; }
    public string? sobserv { get; init; }
    public string? dato1 { get; init; }
    public string? dato2 { get; init; }
    public string? dato3 { get; init; }
    public string? dato4 { get; init; }
    public string? dato5 { get; init; }
    public string? dato6 { get; init; }
    public string? iinventario { get; init; }
    public Int32 qrecurso { get; set; }
    public decimal mprecio { get; init; }
    public decimal qporcdescuento { get; init; }
    public string? qporciva { get; init; }
    public decimal mvrtotal { get; set; }
    public string? valor1 { get; init; }
    public string? valor2 { get; init; }
    public string? valor3 { get; init; }
    public string? valor4 { get; init; }
    public string? qrecurso2 { get; init; }
}