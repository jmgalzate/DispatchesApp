using FioryApp.src.Entity;

namespace FioryApp.src.Controller;

public class ScanController
{
    public List<ProductEntity> ScannedProducts = new List<ProductEntity>();
    public int IndexScanned;

    public ScanController()
    {
        IndexScanned = 0;
    }

    public void SetListScannedProducts(List<OrderProduct> requestedProducts)
    {
        foreach (OrderProduct product in requestedProducts)
        {
            ScannedProducts.Add(new ProductEntity()
                {
                    code = product.irecurso,
                    requested = product.qrecurso
                }
            );
            ;
        }
    }

    private int ExistProduct(string sku)
    {
        int index = -1;

        if (ScannedProducts.Count == 0)
        {
            index = -1;
        }
        else
        {
            for (int i = 0; i < ScannedProducts.Count; i++)
            {
                if (ScannedProducts[i].code!.Equals(sku))
                {
                    index = i;
                    break;
                }
            }
        }

        return index;
    }

    public void AddProduct(string sku, string ean)
    {
        int index = ExistProduct(sku);

        if (index > -1)
        {
            ScannedProducts[index].quantity = ScannedProducts[index].quantity + 1;
            ScannedProducts[index].barcode = ean;
        }
        else
        {
            ScannedProducts.Add(new ProductEntity()
                {
                    code = sku,
                    barcode = ean,
                    quantity = 1
                }
            );
        }
    }
}