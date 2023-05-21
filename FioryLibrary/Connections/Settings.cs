using System;
using Newtonsoft.Json;
using System.Security.Principal;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FioryLibrary.Connections;

public class Settings
{
    private string? _path;
    private string? _appsettings;
    private ConnectionStrings? _connection;

    public Settings()
    {
        try
        {
            this._path = ConfigFiles.getConfigFilesPath("appsettings.json");
            this._appsettings = File.ReadAllText(this._path);
            this._connection = JsonConvert.DeserializeObject<ConnectionStrings>(this._appsettings);
            Logger.info("Config: las configuraciones se cargaron correctamente");
        } catch (Exception ex)
        {
            Logger.error("Config: " + ex.Message);
        }
        
    }

    public ConnectionStrings getConnectionStrings() => this._connection!;
}

public class ConnectionStrings
{
    public string? Server { set; get; }
    public string? Username { set; get; }
    public string? Password { set; get; }
    public string? MachineID { set; get; }
    public string? iapp { set; get; }
    public string? itdoper { set; get; }
}

