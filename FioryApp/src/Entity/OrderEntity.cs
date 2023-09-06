namespace FioryApp.Entity;
#nullable enable

public class OrderEntity
{
    public OrderHeader? encabezado { get; init; }
    public OrderInvoiceSettlement? liquidacion { get; set; }
    public OrderMainData? datosprincipales { get; init; }
    public List<OrderProduct>? listaproductos { get; set; }
    public string? qoprsok { get; init; }
}