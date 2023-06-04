using Newtonsoft.Json.Linq;

namespace FioryLibrary.Connections;

public class MasterData
{
    private readonly List<Product> _listProductDetails = new();
    public MasterData(JArray products) => setProducts(products);
    
    private void setProducts(JArray products)
    {
        foreach (var product in products)
        {
            var newProduct = new Product()
            {
                sku = product["irecurso"]!.ToString(),
                name = product["nrecurso"]!.ToString(),
                ean = product["clase2"]!.ToString()
            };
            _listProductDetails.Add(newProduct);
        }
    }

    public string vLookup(string ean)
    {
        string sku = "";
        _listProductDetails.ForEach(delegate(Product productDetails)
        {
            if (productDetails.ean == ean)
                sku = productDetails.sku!;
        });

        return sku;
    }

    private class Product
    {
        public string? sku { init; get; }
        public string? name { init; get; }
        public string? ean { init; get; }
    }
}