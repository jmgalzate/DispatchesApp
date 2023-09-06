namespace FioryApp.Entity;

public class OrderInvoiceSettlement
{
    public decimal parcial { get; set; }
    public decimal descuento { get; set; }
    public decimal iva { get; set; }
    public decimal total { get; set; }
}