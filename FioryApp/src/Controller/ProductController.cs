using FioryApp.src.Entity;
using Newtonsoft.Json.Linq;

namespace FioryApp.src.Controller;

public class ProductController
{
    public ProductController()
    {
    }

    //TODO: add a method that search in the Products Entity if a barcode or product code exists.

    private readonly List<ProductEntity> ListProductDetails = new();

    public async Task SetProductsAsync(JArray products)
    {
        foreach (JObject product in products)
        {
            var newProduct = new ProductEntity()
            {
                code = product["irecurso"].ToString(),
                name = product["nrecurso"].ToString(),
                barcode = product["clase2"].ToString()
            };
            ListProductDetails.Add(newProduct);
        }
    }


    public string vLookup(string ean)
    {
        string sku = "";
        ListProductDetails.ForEach(delegate(ProductEntity productDetails)
        {
            if (productDetails.barcode == ean)
                sku = productDetails.code!;
        });

        return sku;
    }
}