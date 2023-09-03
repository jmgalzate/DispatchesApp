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
    public string tdetalle { get; set; }
    public string itdoper { get; set; }
    public string snumsop { get; set; }
    public string fsoport { get; set; }
    public string iccbase { get; set; }
    public string imoneda { get; set; }
    public string banulada { get; set; }
    public string blocal { get; set; }
    public string bniif { get; set; }
    public string svaloradic1 { get; set; }
    public string svaloradic2 { get; set; }
    public string svaloradic3 { get; set; }
    public string svaloradic4 { get; set; }
    public string svaloradic5 { get; set; }
    public string svaloradic6 { get; set; }
    public string svaloradic7 { get; set; }
    public string svaloradic8 { get; set; }
    public string svaloradic9 { get; set; }
    public string svaloradic10 { get; set; }
    public string svaloradic11 { get; set; }
    public string svaloradic12 { get; set; }
    public string fecha1adic { get; set; }
    public string fecha2adic { get; set; }
    public string fecha3adic { get; set; }
    public string datosaddin { get; set; }
    public string fcreacion { get; set; }
    public string fultima { get; set; }
    public string fprocesam { get; set; }
    public string iusuario { get; set; }
    public string iusuarioult { get; set; }
    public string isucursal { get; set; }
    public string inumoperultimp { get; set; }
    public string accionesalgrabar { get; set; }
    public string iemp { get; set; }
    public string inumoper { get; set; }
    public string itdsop { get; set; }
    public string inumsop { get; set; }
    public string iclasifop { get; set; }
    public string iprocess { get; set; }
    public string mtotaloperacion { get; set; }
}

public class OrderInvoiceSettlement
{
    public decimal Parcial { get; set; }
    public decimal Descuento { get; set; }
    public decimal Iva { get; set; }
    public decimal Total { get; set; }
}

public class OrderMainData
{
    public string init { get; set; }
    public string initvendedor { get; set; }
    public string finicio { get; set; }
    public string sobserv { get; set; }
    public string bregvrunit { get; set; }
    public string bregvrtotal { get; set; }
    public string condicion1 { get; set; }
    public string icuenta { get; set; }
    public string blistaconiva { get; set; }
    public string icccxp { get; set; }
    public string busarotramoneda { get; set; }
    public string imonedaimpresion { get; set; }
    public string ireferencia { get; set; }
    public string bcerrarref { get; set; }
    public string qdias { get; set; }
    public string iinventario { get; set; }
    public string ilistaprecios { get; set; }
    public string qporcdescuento { get; set; }
    public string frmenvio { get; set; }
    public string frmpago { get; set; }
    public string mtasacambio { get; set; }
    public string qregfcobro { get; set; }
    public string isucursalcliente { get; set; }
}

public class OrderProduct
{
    public string irecurso { get; set; }
    public string itiporec { get; set; }
    public string icc { get; set; }
    public string sobserv { get; set; }
    public string dato1 { get; set; }
    public string dato2 { get; set; }
    public string dato3 { get; set; }
    public string dato4 { get; set; }
    public string dato5 { get; set; }
    public string dato6 { get; set; }
    public Int32 iinventario { get; set; }
    public Int32 qrecurso { get; set; }
    public decimal mprecio { get; set; }
    public decimal qporcdescuento { get; set; }
    public decimal qporciva { get; set; }
    public decimal mvrtotal { get; set; }
    public decimal valor1 { get; set; }
    public decimal valor2 { get; set; }
    public decimal valor3 { get; set; }
    public decimal valor4 { get; set; }
    public string qrecurso2 { get; set; }
}