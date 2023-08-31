using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FioryLibrary.Connections;

namespace FioryApp.src.Service;

public class ContapymeService
{
    private readonly Random _random = new();
    private readonly JObject[] _operationsArray = new JObject[1];
    private ConnectionStrings _connectionInformation = new();
    private readonly string[] _arrParams = new string[4];
    public string agente { set; get; }

    public ContapymeService()
    {
        
    }

    private async Task<JObject> _requestPostAsync(Uri endpoint, Dictionary<string, Array> objSend)
    {
        try
        {
            using var webclient = new HttpClient();
            var newPostJson = JsonConvert.SerializeObject(objSend);
            var payload = new StringContent(newPostJson, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await webclient.PostAsync(endpoint, payload);
            Logger.info("Connection: response received from " + endpoint + " with status code: " + httpResponseMessage.StatusCode);

            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseContent)!;
        }
        catch (HttpRequestException httpException)
        {
            Logger.error("Connection: " + httpException.Message);
            return new JObject();
        }
        catch (JsonException jsonException)
        {
            Logger.error("JSON Deserialization Error: " + jsonException.Message);
            return new JObject();
        }
    }

    public async Task SetContapymeAsync()
    {
        Settings settings = new Settings();
        _connectionInformation = settings.GetConnectionStrings();

        _arrParams[2] = _connectionInformation.iapp!; // IAPP
        _arrParams[3] = _random.Next(0, 9).ToString(); // Random number from 0 to 9
        _arrParams[1] = await GetAuthAsync(); // Keyagent
    }

    private JObject[] _getOperations(string orderNumber)
    {
        string operations = "{\"inumoper\": \"" + (orderNumber) + "\", \"itdoper\": \"ORD1\"}";
        _operationsArray[0] = JObject.Parse(operations);
        return _operationsArray;
    }

    private async Task<string> GetAuthAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"GetAuth\"/");
        Logger.info("Connection: sending request to " + endpoint);

        Dictionary<string, string> objParams = new Dictionary<string, string>
    {
        {"email", _connectionInformation.username!},
        {"password", _connectionInformation.password!},
        {"id_maquina", _connectionInformation.machineId!}
    };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = await _requestPostAsync(endpoint, _setParameters(objSend));

        (JObject header, JObject body) = ProcessResponse(response);
        JObject objTemp = (JObject)body["datos"]!;

        string keyAgente = objTemp.TryGetValue("keyagente", out JToken? tokTemp)
            ? (string)tokTemp!
            : string.Empty;

        this.agente = keyAgente;

        return keyAgente;
    }

    public async Task CloseAgentAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"Logout\"/");
        Logger.info("Connection: sending request to " + endpoint);
        JObject response = await _requestPostAsync(endpoint, _setParameters(""));
    }

    public async Task<JObject> Action(string action, string orderNumber, JObject orderBody = null)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);

        Dictionary<string, dynamic> objParams;

        if (action == "SAVE" || action == "CALCULAR IMPUESTOS")
        {
            objParams = new Dictionary<string, dynamic>
            {
                {"accion", "CALCULAR IMPUESTOS" },
                {"operaciones", _getOperations(orderNumber)},
                {"oprdata", orderBody }
            };

        }
        else
        {
            objParams = new Dictionary<string, dynamic>
            {
                {"accion", action },
                {"operaciones", _getOperations(orderNumber) }
            };
        }

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = await _requestPostAsync(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = ProcessResponse(response);
        JObject objTemp = (JObject)body["datos"]!;

        return objTemp;
    }

    public async Task<JArray> GetProductsAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatElemInv/\"GetListaElemInv\"/");
        Logger.info("Connection: sending request to " + endpoint);

        string objSend = "{\"datospagina\":{\"cantidadregistros\":\"50000\",\"pagina\":\"\"},\"camposderetorno\":[\"irecurso\",\"nrecurso\",\"clase2\"]}";

        JObject response = await _requestPostAsync(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = ProcessResponse(response);
        JArray objTemp = (JArray)body["datos"];

        return objTemp;
    }

    private Dictionary<string, Array> _setParameters(string dataJson)
    {
        _arrParams[0] = dataJson;

        Dictionary<string, Array> objSend = new Dictionary<string, Array>
    {
        {"_parameters", _arrParams }
    };

        return objSend;
    }

    private (JObject responseHeader, JObject responseAnswer) ProcessResponse(JObject response)
    {
        JObject responseHeader = new JObject();
        JObject responseAnswer = new JObject();

        if (!response.TryGetValue("result", out JToken resultToken) || !(resultToken is JArray resultArray) || resultArray.Count == 0)
        {
            Logger.error("Contapyme response: the request was not processed successfully; please check the server connection is working");
            return (responseHeader, responseAnswer);
        }

        responseHeader = (JObject)resultArray[0]["encabezado"];

        if (responseHeader["resultado"]?.ToString() == "false")
        {
            Logger.error("Contapyme response: " + responseHeader["mensaje"]);
            return (responseHeader, responseAnswer);
        }

        Logger.info("Contapyme response: the request was processed successfully");

        responseAnswer = (JObject)resultArray[0]["respuesta"];
        return (responseHeader, responseAnswer);
    }

}