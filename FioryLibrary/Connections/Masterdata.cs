using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace FioryLibrary.Connections;

public class Masterdata
{

    public List<Product> listProductDetails = new List<Product>();

    public Masterdata(JArray products) => this.setProducts(products);

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

    private void setProducts(JArray products)
    {
        foreach (var product in products)
        {
            var newProduct = new Product()
            {
                sku = product["irecurso"].ToString(),
                name = product["nrecurso"].ToString(),
                ean = product["clase2"].ToString()
            };

            this.listProductDetails.Add(newProduct);
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


