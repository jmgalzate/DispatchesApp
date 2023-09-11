namespace DispatchesApp.Entity;
#nullable enable
public class OrderProductStrings
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
    public string? qrecurso { get; set; }
    public string? mprecio { get; init; }
    public string? qporcdescuento { get; init; }
    public string? qporciva { get; init; }
    public string? mvrtotal { get; set; }
    public string? valor1 { get; init; }
    public string? valor2 { get; init; }
    public string? valor3 { get; init; }
    public string? valor4 { get; init; }
    public string? qrecurso2 { get; init; }
    
    public OrderProduct ToOrderProduct()
    {
        return new OrderProduct
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
            iinventario = this.iinventario,
            qrecurso = Int32.Parse(this.qrecurso!),
            mprecio = decimal.Parse(this.mprecio!),
            qporcdescuento = decimal.Parse(this.qporcdescuento!),
            qporciva = this.qporciva,
            mvrtotal = decimal.Parse(this.mvrtotal!),
            valor1 = this.valor1,
            valor2 = this.valor2,
            valor3 = this.valor3,
            valor4 = this.valor4,
            qrecurso2 = this.qrecurso2,
        };
    }
}