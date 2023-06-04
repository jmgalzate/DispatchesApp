using Newtonsoft.Json.Linq;

namespace FioryLibrary.Connections;

public static class MasterData
{
    private static readonly List<Product> ListProductDetails = new();

    public static void setProducts(JArray products)
    {
        foreach (var product in products)
        {
            var newProduct = new Product()
            {
                sku = product["irecurso"]!.ToString(),
                name = product["nrecurso"]!.ToString(),
                ean = product["clase2"]!.ToString()
            };
            ListProductDetails.Add(newProduct);
        }
    }

    public static string vLookup(string ean)
    {
        string sku = "";
        ListProductDetails.ForEach(delegate(Product productDetails)
        {
            if (productDetails.ean == ean)
                sku = productDetails.sku!;
        });

        return sku;
    }
}

public class Product
{
    public string? sku { init; get; }
    public string? name { init; get; }
    public string? ean { init; get; }
}