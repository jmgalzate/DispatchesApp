using FioryApp.src.Entity;
using Newtonsoft.Json.Linq;

namespace FioryApp.src.Service.Sales;

public class Order : Sale
{
    public decimal totalProductsToScan = 0;

    public override void setOrderDetails(JObject orderJson)
    {

        setOrderJson(orderJson);
        JArray productsArray = (JArray)orderJson["listaproductos"]!;
        this.setListOfProducts(productsArray);
        this.setSkuDetails(this.products);

        if (this.products.Count == 1)
            this.totalProductsToScan = products[0].qrecurso;
        else
            this.totalProductsToScan = productsArray.Sum(m => (decimal)m.SelectToken("qrecurso")!);

        Sale.exportFile(this.orderJson!, "actual", this.orderNumber);
    }

    public void setSkuDetails(List<OrderProduct> products)
    {

        int nProducts = products.Count;
        listOfSku = new string[nProducts];

        for (int i = 0; i < nProducts; i++)
        {
            this.listOfSku[i] = products[i].irecurso!;
        }
    }

    public bool vlookup(string sku)
    {
        bool exists = false;

        for (int i = 0; i < listOfSku!.Length; i++)
        {
            string product = this.listOfSku[i];
            if (sku == product) exists = true;
        }

        return exists;
    }

    private void setListOfProducts(JArray productsArray)
    {
        List<OrderProduct> tempList = productsArray.ToObject<List<OrderProduct>>()!;

        int total = tempList.Count;

        foreach (var tempValue in tempList)
        {
            int index = this.existProduct(tempValue.irecurso!);

            if (index > -1)
            {
                int valor = products[index].qrecurso + tempValue.qrecurso;
                products[index].qrecurso = valor;
            }
            else
            {
                this.products.Add(new OrderProduct()
                {
                    irecurso = tempValue.irecurso,
                    itiporec = tempValue.itiporec,
                    icc = tempValue.icc,
                    sobserv = tempValue.sobserv,
                    dato1 = tempValue.dato1,
                    dato2 = tempValue.dato2,
                    dato3 = tempValue.dato3,
                    dato4 = tempValue.dato4,
                    dato5 = tempValue.dato5,
                    dato6 = tempValue.dato6,
                    iinventario = tempValue.iinventario,
                    qrecurso = tempValue.qrecurso,
                    mprecio = tempValue.mprecio,
                    qporcdescuento = tempValue.qporcdescuento,
                    qporciva = tempValue.qporciva,
                    mvrtotal = tempValue.mvrtotal,
                    valor1 = tempValue.valor1,
                    valor2 = tempValue.valor2,
                    valor3 = tempValue.valor3,
                    valor4 = tempValue.valor4,
                    qrecurso2 = tempValue.qrecurso2
                });
            }
        }
    }
}

