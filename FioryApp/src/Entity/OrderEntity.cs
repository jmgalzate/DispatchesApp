namespace FioryApp.src.Entity;

public class OrderEntity
{
    #nullable enable
    public OrderHeader? encabezado { get; set; }
    #nullable enable
    public OrderInvoiceSettlement? liquidacion { get; set; }
    #nullable enable
    public OrderMainData? datosprincipales { get; set; }
    #nullable enable
    public List<OrderProduct>? listaproductos { get; set; }
    #nullable enable
    public string? qoprsok { get; set; }
}

public class OrderHeader
{
    public required string tdetalle { get; set; }
    public required string itdoper { get; set; }
    public required string snumsop { get; set; }
    public required string fsoport { get; set; }
    public required string iccbase { get; set; }
    public required string imoneda { get; set; }
    public required string banulada { get; set; }
    public required string blocal { get; set; }
    public required string bniif { get; set; }
    public required string svaloradic1 { get; set; }
    public required string svaloradic2 { get; set; }
    public required string svaloradic3 { get; set; }
    public required string svaloradic4 { get; set; }
    public required string svaloradic5 { get; set; }
    public required string svaloradic6 { get; set; }
    public required string svaloradic7 { get; set; }
    public required string svaloradic8 { get; set; }
    public required string svaloradic9 { get; set; }
    public required string svaloradic10 { get; set; }
    public required string svaloradic11 { get; set; }
    public required string svaloradic12 { get; set; }
    public required string fecha1adic { get; set; }
    public required string fecha2adic { get; set; }
    public required string fecha3adic { get; set; }
    public required string datosaddin { get; set; }
    public required string fcreacion { get; set; }
    public required string fultima { get; set; }
    public required string fprocesam { get; set; }
    public required string iusuario { get; set; }
    public required string iusuarioult { get; set; }
    public required string isucursal { get; set; }
    public required string inumoperultimp { get; set; }
    public required string accionesalgrabar { get; set; }
    public required string iemp { get; set; }
    public required string inumoper { get; set; }
    public required string itdsop { get; set; }
    public required string inumsop { get; set; }
    public required string iclasifop { get; set; }
    public required string iprocess { get; set; }
    public required string mtotaloperacion { get; set; }
}

public class OrderInvoiceSettlement
{
    public required decimal Parcial { get; set; }
    public required decimal Descuento { get; set; }
    public required decimal Iva { get; set; }
    public required decimal Total { get; set; }
}

public class OrderMainData
{
    public required string init { get; set; }
    public required string initvendedor { get; set; }
    public required string finicio { get; set; }
    public required string sobserv { get; set; }
    public required string bregvrunit { get; set; }
    public required string bregvrtotal { get; set; }
    public required string condicion1 { get; set; }
    public required string icuenta { get; set; }
    public required string blistaconiva { get; set; }
    public required string icccxp { get; set; }
    public required string busarotramoneda { get; set; }
    public required string imonedaimpresion { get; set; }
    public required string ireferencia { get; set; }
    public required string bcerrarref { get; set; }
    public required string qdias { get; set; }
    public required string iinventario { get; set; }
    public required string ilistaprecios { get; set; }
    public required string qporcdescuento { get; set; }
    public required string frmenvio { get; set; }
    public required string frmpago { get; set; }
    public required string mtasacambio { get; set; }
    public required string qregfcobro { get; set; }
    public required string isucursalcliente { get; set; }
}

public class OrderProduct
{
    public required string irecurso { get; set; }
    public required string itiporec { get; set; }
    public required string icc { get; set; }
    public required string sobserv { get; set; }
    public required string dato1 { get; set; }
    public required string dato2 { get; set; }
    public required string dato3 { get; set; }
    public required string dato4 { get; set; }
    public required string dato5 { get; set; }
    public required string dato6 { get; set; }
    public required Int32 iinventario { get; set; }
    public required Int32 qrecurso { get; set; }
    public required decimal mprecio { get; set; }
    public required decimal qporcdescuento { get; set; }
    public required decimal qporciva { get; set; }
    public required decimal mvrtotal { get; set; }
    public required decimal valor1 { get; set; }
    public required decimal valor2 { get; set; }
    public required decimal valor3 { get; set; }
    public required decimal valor4 { get; set; }
    public required string qrecurso2 { get; set; }
}