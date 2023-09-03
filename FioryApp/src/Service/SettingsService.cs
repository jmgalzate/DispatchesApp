using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace FioryApp.src.Service;

public class SettingsService
{
    private readonly ConnectionStrings? _connection;

    public SettingsService()
    {
        try
        {
            var path = ConfigFilesService.GetConfigFilesPath("appsettings.json");
            var appsettings = File.ReadAllText(path);
            _connection = JsonConvert.DeserializeObject<ConnectionStrings>(appsettings);
            LoggerService.info("AppSettings: file loaded");
        }
        catch (Exception ex)
        {
            LoggerService.error("AppSettings: " + ex.Message);
        }
    }

    public ConnectionStrings GetConnectionStrings() => _connection!;

    public string CalculateMd5Hash(string input)
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