namespace DispatchesApp.Entity;
#nullable enable

public class OrderEntity
{
    public OrderHeader? encabezado { get; init; }
    public OrderInvoiceSettlement? liquidacion { get; set; }
    public OrderMainData? datosprincipales { get; init; }
    public List<OrderProductStrings>? listaproductos { get; set; }
    public string? qoprsok { get; init; }
}