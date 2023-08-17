using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FioryLibrary.Connections;

public class Contapyme
{
    public int OrderNumber;
    private readonly Random _random = new();
    private readonly JObject[] _operationsArray = new JObject[1];
    private ConnectionStrings _connectionInformation = new();

    private readonly string[] _arrParams = new string[4];

    public Contapyme()
    {
        OrderNumber = 0;
    }

    public void setContapyme()
    {
        Settings settings = new Settings();
        _connectionInformation = settings.getConnectionStrings();
        _arrParams[2] = _connectionInformation.iapp!; //IAPP
        _arrParams[3] = (_random.Next(0, 9)).ToString(); //Random number from 0 to 9 
        _arrParams[1] = getAuth(); //Keyagent
    }

    private string getAuth()
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
        JObject response = _requestPost(endpoint, _setParameters(objSend));

        
        (JObject header, JObject body) = _processRequest(response);
        JObject objTemp = (JObject)body["datos"]!;
        
        string keyAgente = "";
        if (objTemp.TryGetValue("keyagente", out JToken? tokTemp))
            keyAgente = (string)tokTemp!;

        return keyAgente;
    }

    public JObject process()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);

        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "PROCESS" },
            {"operaciones", _getOperations() },
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = _requestPost(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = _processRequest(response);
        JObject objTemp = (JObject)body["datos"]!;
        
        return objTemp;
    }

    public JObject load()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "LOAD" },
            {"operaciones", _getOperations()},
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = _requestPost(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = _processRequest(response);
        JObject objTemp = (JObject)body["datos"]!;
        
        return objTemp;
    }

    public JObject taxes(JObject newOrder)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "CALCULAR IMPUESTOS" },
            {"operaciones", _getOperations() },
            {"oprdata", newOrder }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = _requestPost(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = _processRequest(response);
        JObject objTemp = (JObject)body["datos"]!;

        return objTemp;
    }

    public JObject save(JObject newOrder)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "SAVE" },
            {"operaciones", _getOperations() },
            {"oprdata", newOrder }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = _requestPost(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = _processRequest(response);
        JObject objTemp = (JObject)body["datos"]!;
        
        return objTemp;
    }

    public JObject unprocess()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "UNPROCESS" },
            {"operaciones", _getOperations() },
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = _requestPost(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = _processRequest(response);
        JObject objTemp = (JObject)body["datos"]!;
        
        return objTemp;
    }
    
    public JArray getProducts()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatElemInv/\"GetListaElemInv\"/");
        Logger.info("Connection: sending request to " + endpoint);
        
        string objSend = "{\"datospagina\":{\"cantidadregistros\":\"50000\",\"pagina\":\"\"},\"camposderetorno\":[\"irecurso\",\"nrecurso\",\"clase2\"]}";
        
        JObject response = _requestPost(endpoint, _setParameters(objSend));
        (JObject header, JObject body) = _processRequest(response);
        JArray objTemp = (JArray)body["datos"]!;
        
        return objTemp;
    }

    public void closeAgent()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"Logout\"/");
        Logger.info("Connection: sending request to " + endpoint);
        JObject response = _requestPost(endpoint, _setParameters(""));
    }

    private JObject[] _getOperations()
    {
        string operations = "{\"inumoper\": \"" + (OrderNumber) + "\", \"itdoper\": \"ORD1\"}";
        _operationsArray[0] = JObject.Parse(operations);
        return _operationsArray;
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

    private JObject _requestPost(Uri endpoint, Dictionary<string, Array> objSend)
    {
        try
        {
            using var webclient = new HttpClient();
            var newPostJson = JsonConvert.SerializeObject(objSend);
            var payload = new StringContent(newPostJson, System.Text.Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> httpResponse = webclient.PostAsync(endpoint, payload);
            Logger.info("Connection: request payload = " + newPostJson);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            Logger.info("Connection: response received from " + endpoint + " with status code: " + httpResponseMessage.StatusCode);
            return (JObject)JsonConvert.DeserializeObject(httpResponseMessage.Content.ReadAsStringAsync().Result)!;
        }
        catch (Exception e)
        {
            Logger.error("Connection: " + e.Message);
            return (JObject)JsonConvert.DeserializeObject("{ }")!;
        }

    }

    private (JObject, JObject) _processRequest(JObject response)
    {
        JObject responseHeader = new JObject();
        JObject responseAnswer = new JObject();

        if (!response.TryGetValue("result", out _))
        {
            Logger.error("Contapyme response: the request was not processed successfully; please check the server connection is working");
            return (responseHeader, responseAnswer);
        }

        responseHeader = (JObject)response["result"]![0]!["encabezado"]!;
        
        if (responseHeader["resultado"]!.ToString() == "false")
        {
            Logger.error("Contapyme response: " + responseHeader["mensaje"]);
            return (responseHeader, responseAnswer);
        }
        
        Logger.info("Contapyme response: the request was processed successfully");
            
        responseAnswer = (JObject)response["result"]![0]!["respuesta"]!;
        return (responseHeader, responseAnswer);
    }
}