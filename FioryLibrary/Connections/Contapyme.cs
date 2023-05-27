using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FioryLibrary.Connections;

public class Contapyme
{
    public int OrderNumber;
    private Random _random = new Random();
    private JObject[] _operationsArray = new JObject[1];
    private ConnectionStrings _connectionInformation = new ConnectionStrings();

    private string[] _arrParams = new string[4];

    public Contapyme()
    {
        this.OrderNumber = 0;
    }

    public void setContapyme()
    {
        Settings settings = new Settings();
        this._connectionInformation = settings.getConnectionStrings();
        this._arrParams[2] = this._connectionInformation.iapp!; //IAPP
        this._arrParams[3] = (_random.Next(0, 9)).ToString(); //Random number from 0 to 9 
        this._arrParams[1] = this.getAuth(); //Keyagent
    }

    public string getAuth()
    {
        Uri endpoint = new Uri(this._connectionInformation.Server + "datasnap/rest/TBasicoGeneral/\"GetAuth\"/");
        Logger.info("Connection: request sent to" + endpoint);

        Dictionary<string, string> objParams = new Dictionary<string, string>
        {
            {"email", this._connectionInformation.Username!},
            {"password", this._connectionInformation.Password!},
            {"id_maquina", this._connectionInformation.MachineID!}
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = this._requestPost(endpoint, _setParameters(objSend));

        JToken? tokTemp = null;
        JObject objTemp = this._processRequest(response);

        string keyAgente = "";
        if (objTemp.TryGetValue("keyagente", out tokTemp) && (tokTemp != null))
            keyAgente = (string)tokTemp!;

        return keyAgente;
    }

    public JObject process()
    {
        Uri endpoint = new Uri(this._connectionInformation.Server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: request sent to" + endpoint);

        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "PROCESS" },
            {"operaciones", _getOperations() },
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = this._requestPost(endpoint, _setParameters(objSend));
        return this._processRequest(response);
    }

    public JObject load()
    {
        Uri endpoint = new Uri(this._connectionInformation.Server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: request sent to" + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "LOAD" },
            {"operaciones", _getOperations()},
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = this._requestPost(endpoint, _setParameters(objSend));
        return this._processRequest(response);
    }

    public JObject save(JObject newOrder)
    {
        Uri endpoint = new Uri(this._connectionInformation.Server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: request sent to" + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "SAVE" },
            {"operaciones", _getOperations() },
            {"oprdata", newOrder }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = this._requestPost(endpoint, _setParameters(objSend));
        return this._processRequest(response);
    }

    public JObject unprocess()
    {
        Uri endpoint = new Uri(this._connectionInformation.Server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: request sent to" + endpoint);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            {"accion", "UNPROCESS" },
            {"operaciones", _getOperations() },
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        JObject response = this._requestPost(endpoint, _setParameters(objSend));
        return this._processRequest(response);
    }

    public void closeAgent()
    {
        Uri endpoint = new Uri(this._connectionInformation.Server + "datasnap/rest/TBasicoGeneral/\"Logout\"/");
        Logger.info("Connection: request sent to" + endpoint);
        JObject response = this._requestPost(endpoint, _setParameters(""));
    }

    private JObject[] _getOperations()
    {
        string operations = "{\"inumoper\": \"" + (this.OrderNumber) + "\", \"itdoper\": \"ORD1\"}";
        this._operationsArray[0] = JObject.Parse(operations);
        return this._operationsArray;
    }

    private Dictionary<string, Array> _setParameters(string dataJson)
    {
        this._arrParams[0] = dataJson;
        Logger.info("Payload: " + dataJson);

        Dictionary<string, Array> objSend = new Dictionary<string, Array>
        {
            {"_parameters", this._arrParams }
        };

        return objSend;
    }

    private JObject _requestPost(System.Uri endpoint, Dictionary<string, Array> objSend)
    {
        try
        {
            using (var webclient = new HttpClient())
            {
                var newPostJson = JsonConvert.SerializeObject(objSend);
                var payload = new StringContent(newPostJson, System.Text.Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> httpResponse = webclient.PostAsync(endpoint, payload);
                Logger.info("Connection: request sent to" + endpoint + " with payload: " + newPostJson);
                HttpResponseMessage httpResponseMessage = httpResponse.Result;

                Logger.info("Connection: response received from" + endpoint + " with status code: " + httpResponseMessage.StatusCode + " and content: " + httpResponseMessage.Content.ReadAsStringAsync().Result);
                return (JObject)JsonConvert.DeserializeObject(httpResponseMessage.Content.ReadAsStringAsync().Result)!;
            }
        }
        catch (Exception e)
        {
            Logger.error("Connection: " + e.Message);
            return (JObject)JsonConvert.DeserializeObject("{ }")!;
        }

    }

    private JObject _processRequest(JObject response)
    {
        JArray? arrTemp;
        JObject? objTemp = null;
        JToken? tokTemp;
        JObject? responseHeader = null;

        if (response.TryGetValue("result", out tokTemp) && (tokTemp != null))
        {
            arrTemp = (JArray)tokTemp;
            if ((arrTemp != null) && (arrTemp.Count > 0))
            {
                objTemp = (JObject)arrTemp[0];

                responseHeader = (JObject)objTemp["encabezado"]!;

                if (responseHeader != null)
                {
                    if (responseHeader["resultado"]!.ToString() == "false")
                        Logger.error("Contapyme response: " + responseHeader["mensaje"]);
                    else
                        Logger.info("Contapyme response: the request was processed successfully");
                }

                if (objTemp.TryGetValue("respuesta", out tokTemp) && (tokTemp != null))
                {
                    objTemp = (JObject)tokTemp;
                    if (objTemp.TryGetValue("datos", out tokTemp) && (tokTemp != null))
                    {
                        objTemp = (JObject)tokTemp;
                    }
                }
            }
        }

        return objTemp!;
    }
}