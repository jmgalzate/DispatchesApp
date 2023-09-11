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
    
    public OrderProductStrings ToOrderProductStrings()
    {
        return new OrderProductStrings
        {
            irecurso = this.irecurso,
            itiporec = this.itiporec,
            icc = this.icc,
            sobserv = this.sobserv,
            dato1 = this.dato1,
            dato2 = this.dato2,
            dato3 = this.dato3,
            dato4 = this.dato4,
            dato5 = this.dato5,
            dato6 = this.dato6,
            iinventario = this.iinventario!.ToString(), // Convert Int32 to string
            qrecurso = this.qrecurso.ToString(), // Convert Int32 to string
            mprecio = this.mprecio.ToString(), // Convert decimal to string
            qporcdescuento = this.qporcdescuento.ToString(), // Convert decimal to string
            qporciva = this.qporciva,
            mvrtotal = this.mvrtotal.ToString(), // Convert decimal to string
            valor1 = this.valor1,
            valor2 = this.valor2,
            valor3 = this.valor3,
            valor4 = this.valor4,
            qrecurso2 = this.qrecurso2,
        };
    }
}