using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace FioryLibrary.Connections;

public class Settings
{
    private readonly string? _path;
    private readonly string? _appsettings;
    private readonly ConnectionStrings? _connection;

    public Settings()
    {
        try
        {
            _path = ConfigFiles.getConfigFilesPath("appsettings.json");
            _appsettings = File.ReadAllText(_path);
            _connection = JsonConvert.DeserializeObject<ConnectionStrings>(_appsettings);
            Logger.info("AppSettings: file loaded");
        }
        catch (Exception ex)
        {
            Logger.error("AppSettings: " + ex.Message);
        }
    }

    public ConnectionStrings GetConnectionStrings() => _connection!;

    public static string CalculateMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

public class ConnectionStrings
{
    public string? server { set; get; }
    public string? username { set; get; }
    public string? password { set; get; }
    public string? machineId { set; get; }
    public string? iapp { set; get; }
    public string? itdoper { set; get; }
}

