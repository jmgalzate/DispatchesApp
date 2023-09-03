using FioryLibrary.Connections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FioryApp.src.Entity;

namespace FioryApp.src.Service;

public class ContapymeService
{
    private readonly Random _random = new();
    private readonly JObject[] _operationsArray = new JObject[1];
    private ConnectionStrings _connectionInformation = new();
    private readonly string[] _arrParams = new string[4];

    private ContapymeResult _contapymeResult = new();

    public ContapymeService()
    {
    }

    private async Task<ContapymeResult> _requestPostAsync(Uri endpoint, Dictionary<string, Array> objSend)
    {
        try
        {
            using var httpClient = new HttpClient();
            var newPostJson = JsonConvert.SerializeObject(objSend);
            var payload = new StringContent(newPostJson, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(endpoint, payload);
            Logger.info("Connection: response received from " + endpoint + " with status code: " +
                        httpResponseMessage.StatusCode);

            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            _contapymeResult = JsonConvert.DeserializeObject<ContapymeResult>(responseContent)!;

            return _contapymeResult;
        }
        catch (HttpRequestException httpException)
        {
            Logger.error("Connection: " + httpException.Message);

            // Create a custom error response with the desired structure
            return new ContapymeResult
            {
                result = new List<ContapymeEntity>
                {
                    new ContapymeEntity
                    {
                        encabezado = new ContapymeHeader
                        {
                            resultado = false,
                            imensaje = "Connection: " + httpException.Message,
                            // Other properties here
                        },
                        respuesta = new ContapymeBody
                        {
                            datos = new JObject()
                        }
                    }
                }
            };
        }
        catch (JsonException jsonException)
        {
            Logger.error("JSON Deserialization Error: " + jsonException.Message);

            // Create a custom error response with the desired structure
            return new ContapymeResult
            {
                result = new List<ContapymeEntity>
                {
                    new ContapymeEntity
                    {
                        encabezado = new ContapymeHeader
                        {
                            resultado = false,
                            imensaje = "Connection: " + jsonException.Message,
                            // Other properties here
                        },
                        respuesta = new ContapymeBody
                        {
                            datos = new JObject()
                        }
                    }
                }
            };
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

    /***** Authentication *****/

    private async Task<string> GetAuthAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"GetAuth\"/");
        Logger.info("Connection: sending request to " + endpoint);

        Dictionary<string, string> objParams = new Dictionary<string, string>
        {
            { "email", _connectionInformation.username! },
            { "password", _connectionInformation.password! },
            { "id_maquina", _connectionInformation.machineId! }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));

        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        string dataJson = response.result[0].respuesta.datos.ToString(); // Convert JObject to JSON string
        ContapymeBodyDataAuthentication
            data = JsonConvert.DeserializeObject<ContapymeBodyDataAuthentication>(dataJson)!;

        return data.keyagente;
    }

    public async Task CloseAgentAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"Logout\"/");
        Logger.info("Connection: sending request to " + endpoint);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(""));

        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );
    }

    /***** Actions *****/
    public async Task<ContapymeBodyData> Unprocess(string orderNumber)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);

        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            { "accion", "UNPROCESS" },
            { "operaciones", _getOperations(orderNumber) }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        string dataJson = response.result[0].respuesta.datos.ToString(); // Convert JObject to JSON string
        ContapymeBodyData data = JsonConvert.DeserializeObject<ContapymeBodyData>(dataJson)!;

        return data;
    }

    public async Task<OrderEntity> Load(string orderNumber)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);

        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            { "accion", "LOAD" },
            { "operaciones", _getOperations(orderNumber) }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        string dataJson = response.result[0].respuesta.datos.ToString(); // Convert JObject to JSON string
        OrderEntity data = JsonConvert.DeserializeObject<OrderEntity>(dataJson)!;

        return data;
    }

    public async Task Save(string orderNumber, OrderEntity order)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);

        string orderJson = JsonConvert.SerializeObject(order);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            { "accion", "SAVE" },
            { "operaciones", _getOperations(orderNumber) },
            { "oprdata", orderJson }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        string dataJson = response.result[0].respuesta.datos.ToString(); // Convert JObject to JSON string
        ContapymeBodyData data = JsonConvert.DeserializeObject<ContapymeBodyData>(dataJson)!;
    }

    public async Task Taxes(string orderNumber, OrderEntity order)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        Logger.info("Connection: sending request to " + endpoint);

        string orderJson = JsonConvert.SerializeObject(order);
        Dictionary<string, dynamic> objParams = new Dictionary<string, dynamic>
        {
            { "accion", "CALCULAR IMPUESTOS" },
            { "operaciones", _getOperations(orderNumber) },
            { "oprdata", orderJson }
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        string dataJson = response.result[0].respuesta.datos.ToString(); // Convert JObject to JSON string
        ContapymeBodyData data = JsonConvert.DeserializeObject<ContapymeBodyData>(dataJson)!;
    }

    public async Task<ProductEntity> GetProductsAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatElemInv/\"GetListaElemInv\"/");
        Logger.info("Connection: sending request to " + endpoint);

        string objSend =
            "{\"datospagina\":{\"cantidadregistros\":\"50000\",\"pagina\":\"\"},\"camposderetorno\":[\"irecurso\",\"nrecurso\",\"clase2\"]}";

        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        string dataJson = response.result[0].respuesta.datos.ToString(); // Convert JObject to JSON string
        ProductEntity data = JsonConvert.DeserializeObject<ProductEntity>(dataJson)!;

        return data;
    }

    private Dictionary<string, Array> _setParameters(string dataJson)
    {
        _arrParams[0] = dataJson;

        Dictionary<string, Array> objSend = new Dictionary<string, Array>
        {
            { "_parameters", _arrParams }
        };

        return objSend;
    }
}