using FioryApp.src.Entity;
using Newtonsoft.Json.Linq;

namespace FioryApp.src.Service.Sales;

public class Dispatch : Sale
{
    private List<ProductEntity>? ScannedProducts;

    private JObject? encabezado;
    private JObject? liquidacion;
    private JObject? datosprincipales;

    public override void setOrderDetails(JObject orderJson)
    {

        this.encabezado = (JObject)orderJson["encabezado"]!;
        this.encabezado["iusuarioult"] = (string)"WEBAPI";
        this.liquidacion = new JObject(
                 new JProperty("parcial", ""),
            new JProperty("descuento", ""),
            new JProperty("iva", ""),
            new JProperty("total", "")
                );
        this.datosprincipales = (JObject)orderJson["datosprincipales"]!;

        JObject order = new JObject(
            new JProperty("encabezado", orderJson["encabezado"]),
            new JProperty("liquidacion", this.liquidacion),
            new JProperty("datosprincipales", this.datosprincipales),
            new JProperty("listaproductos", JArray.FromObject(products)),
            new JProperty("qoprsok", "0")
            );

        this.orderJson = order;

        Sale.exportFile(order, "nuevo", this.orderNumber);
    }

    public void setListOfProducts(List<OrderProduct> tempList)
    {
        foreach (var x in this.ScannedProducts!)
        {
            if (x.quantity > 0)
            {
                foreach (var y in tempList)
                {
                    if (x.code == y.irecurso)
                    {

                        int nRecurso = x.quantity;
                        decimal vlr = nRecurso * y.mprecio;
                        decimal dcto = y.qporcdescuento / 100;
                        decimal vlrDescuento = dcto * vlr;

                        this.products.Add(new OrderProduct()
                        {
                            irecurso = y.irecurso,
                            itiporec = y.itiporec,
                            icc = y.icc,
                            sobserv = y.sobserv,
                            dato1 = y.dato1,
                            dato2 = y.dato2,
                            dato3 = y.dato3,
                            dato4 = y.dato4,
                            dato5 = y.dato5,
                            dato6 = y.dato6,
                            iinventario = y.iinventario,
                            qrecurso = x.quantity,
                            mprecio = y.mprecio,
                            qporcdescuento = y.qporcdescuento,
                            qporciva = y.qporciva,
                            mvrtotal = (vlr - vlrDescuento),
                            valor1 = y.valor1,
                            valor2 = y.valor2,
                            valor3 = y.valor3,
                            valor4 = y.valor4,
                            qrecurso2 = y.qrecurso2
                        });
                    }
                }
            }
        }
    }

    public void setSkuDetails(List<ProductEntity> scanned)
    {
        this.ScannedProducts = scanned;
    }
}