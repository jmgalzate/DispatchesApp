using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace FioryLibrary.Connections;

public class Masterdata
{

    public List<Product> listProductDetails = new List<Product>();

    public Masterdata() => this.getProducts();

    private void getProducts()
    {
        try
        {
            var separator = Path.DirectorySeparatorChar;
            string path = ConfigFiles.getConfigFilesPath("Products.csv");

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var values = line.Split(',');
                var newProduct = new Product()
                {
                    sku = values[0],
                    name = values[1],
                    ean = values[2]
                };

                this.listProductDetails.Add(newProduct);
            }
            Logger.info("Master Data: Products loaded");
        } catch (Exception ex)
        {
            Logger.error("Master Data: "+ex.Message);
        }
    }

    public string vlookup(string ean)
    {
        string sku = "";
        this.listProductDetails.ForEach(delegate (Product productDetails)
    {
        if (productDetails.ean == ean)
            sku = productDetails.sku!;
    });

        return sku;
    }

    public class Product
    {
        public string? sku { set; get; }
        public string? name { set; get; }
        public string? ean { set; get; }
    }

}


