using System.Text;
using DispatchesApp.Entity;
using Newtonsoft.Json;

namespace DispatchesApp.Service;

public class ConfigFilesService
{
    public static string GetConfigFilesPath(string file)
    {
        char s = Path.DirectorySeparatorChar;
        string path = "";
        var os = OperativeSystem();

        switch (os) {
            case "MacOS":
                path = $"{s}Users{s}{Environment.UserName}{s}projects{s}masterdata{s}Fiory{s}{file}";
                break;
            case "Windows":
                path = $"C:{s}programdata{s}FioryApp{s}Masterfiles{s}{file}";
                break;
        }
        return path;
    }

    public static string GetReportFilesPath()
    {
        string path = "";
        char s = Path.DirectorySeparatorChar;

        path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        path += $"{s}FioryApp{s}Exports{s}";
        bool exists = System.IO.Directory.Exists(path);
        if (!exists)
            System.IO.Directory.CreateDirectory(path);

        return path;
    }

    public static List<string> GetFilesInFolder(string rootPath)
    {

        List<string> filesinRoot = new List<string>(
            Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly)
        );

        return filesinRoot;
    }

    private static string OperativeSystem() {

        string os = "";
        if (OperatingSystem.IsWindows())
            os = "Windows";
        else if (OperatingSystem.IsLinux())
            os = "Linux";
        else if (OperatingSystem.IsMacOS())
            os = "MacOS";
        else if (OperatingSystem.IsMacCatalyst())
            os = "MacOS";
        else if (OperatingSystem.IsAndroid())
            os = "Android";

        return os;
    }
    
    public static void ExportFile(OrderEntity order, string name, int orderNumber)
    {
        char s = Path.DirectorySeparatorChar;
        string folder = GetReportFilesPath();

        string path = $"{folder}{name}_{orderNumber}.json";
        string orderToExport = JsonConvert.SerializeObject(order, Formatting.Indented);

        File.WriteAllText(path, orderToExport);
    }
    
    public static void ExportReport(string cliente, string fcreacion, int orderNumber, decimal efficiency, List<ProductEntity> 
            products)
    {

        char s = Path.DirectorySeparatorChar;
        string folder = GetReportFilesPath();

        string pathFile = $"{folder}reporte_{orderNumber}.txt";

        StringBuilder file = new StringBuilder();

        file.AppendLine($"********* {orderNumber} *********");
        file.AppendLine($"\n");
        file.AppendLine($"NIT Cliente: \t{cliente}");
        file.AppendLine($"Fecha Orden: \t{fcreacion}");
        file.AppendLine($"\n");
        file.AppendLine("Producto\t\tRequeridos\tEscaneados");

        foreach (var product in products)
        {
            file.AppendLine($"{product.code} \t\t {product.requested} \t\t {product.quantity}");
        }

        file.AppendLine($"\n");
        file.AppendLine(
            $"Totales: \t\t {products.Sum(product => product.requested)} \t\t {products.Sum(product => product.quantity)}");
        file.AppendLine($"Eficiencia: \t{efficiency}%");
        
        File.WriteAllText(pathFile, file.ToString());
    }
}