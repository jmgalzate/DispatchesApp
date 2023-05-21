using System.Collections.Generic;
using FioryLibrary.Sales;

namespace FioryLibrary.Operations;

public class Scanned
{
    public List<ScannedProduct> scannedProducts = new List<ScannedProduct>();
    public int indexScanned = 0;

    public Scanned()
    {
        this.indexScanned = 0;
    }

    public void setListScannedProducts(List<Sales.Product> requestedProducts)
    {
        foreach (Sales.Product product in requestedProducts)
        {
            this.scannedProducts.Add(new ScannedProduct
            {
                sku = product.irecurso,
                requested = int.Parse(product.qrecurso!)
            }
            ); ;
        }
    }

    private int existProduct(string sku)
    {
        int index = -1;

        if (this.scannedProducts.Count == 0)
        {
            index = -1;
        }
        else
        {
            for (int i = 0; i < this.scannedProducts.Count; i++)
            {
                if (this.scannedProducts[i].sku!.Equals(sku))
                {
                    index = i;
                    break;
                }   
            }
        }

        return index;
    }

    public void addProduct(string sku, string ean)
    {
        int index = this.existProduct(sku);

        if (index > -1)
        {
            this.scannedProducts[index].quantity = this.scannedProducts[index].quantity + 1;
            this.scannedProducts[index].ean = ean;
        }
        else
        {
            this.scannedProducts.Add(new ScannedProduct
            {
                sku = sku,
                ean = ean,
                quantity = 1
            }
            );
        }
    }


}

public class ScannedProduct
{
    public string? sku { set; get; }
    public string? ean { set; get; }
    public int quantity { set; get; }
    public int requested { set; get; }
}
