using System.Text;
using FioryApp.src.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace FioryApp.src.Service.Sales;

public abstract class Sale
{
    public int orderNumber;
    public JObject? orderJson;
    public List<OrderProduct> products = new List<OrderProduct>();
    public String[]? listOfSku;

    public void setOrderNumber(int orderNumber)
    {
        this.orderNumber = orderNumber;
    }

    protected void setOrderJson(JObject orderJson)
    {
        this.orderJson = orderJson;
    }

    protected int existProduct(String sku)
    {
        int index = -1;

        if (this.products.Count == 0)
        {
            index = -1;
        }
        else
        {
            for (int i = 0; i < this.products.Count; i++)
            {
                if (this.products[i].irecurso!.Equals(sku))
                {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }

    public abstract void setOrderDetails(JObject orderJson);

    public static void exportFile(JObject order, string name, int orderNumber)
    {
        char s = Path.DirectorySeparatorChar;
        string folder = ConfigFilesService.GetReportFilesPath();

        string path = $"{folder}{name}_{orderNumber}.json";
        string orderToExport = JsonConvert.SerializeObject(order, Formatting.Indented);

        File.WriteAllText(path, orderToExport);
    }

    public static void exportReport(int orderNumber, JObject sale, List<OrderProduct> order, List<OrderProduct> dispatch)
    {
        bool control = false;
        string cliente = JsonConvert.SerializeObject(sale["datosprincipales"]!["init"]);
        string fcreacion = JsonConvert.SerializeObject(sale["encabezado"]!["fcreacion"]);
        string orderLiquidacion = JsonConvert.SerializeObject(sale["liquidacion"]!);

        char s = Path.DirectorySeparatorChar;
        string folder = ConfigFilesService.GetReportFilesPath();

        string pathFile = $"{folder}reporte_{orderNumber}.txt";

        StringBuilder file = new StringBuilder();

        file.AppendLine($"********* {orderNumber} *********");
        file.AppendLine($"\n");
        file.AppendLine($"NIT Cliente: \t{cliente}");
        file.AppendLine($"Fecha Orden: \t{fcreacion}");
        file.AppendLine($"\n");
        file.AppendLine("Producto\t\tRequeridos\tEscaneados");

        foreach (var requested in order)
        {
            control = false;

            foreach (var scanned in dispatch)
            {
                if (requested.irecurso == scanned.irecurso)
                {
                    control = true;
                    file.AppendLine($"{requested.irecurso} \t\t {requested.qrecurso} \t\t {scanned.qrecurso}");
                    break;
                }
                else
                    continue;
            }

            if (control == false)
                file.AppendLine($"{requested.irecurso} \t\t {requested.qrecurso} \t\t 0");
        }

        File.WriteAllText(pathFile, file.ToString());
    }
}