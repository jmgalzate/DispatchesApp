namespace FioryApp.src.Service;

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

    public static string OperativeSystem() {

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
}