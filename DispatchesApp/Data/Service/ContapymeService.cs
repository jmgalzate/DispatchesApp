using DispatchesApp.Entity;
using DispatchesApp.src.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DispatchesApp.Service;


public class ContapymeService
{
    private readonly Random _random = new();
    private readonly JObject[] _operationsArray = new JObject[1];
    private ConnectionStrings _connectionInformation;
    private readonly string[] _arrParams = new string[4];
    private int _cantProducts;
    private string _itdoper;

    public string agentkey { get; set; }

    private ContapymeResult _contapymeResult = new();

    public ContapymeService()
    {
    }
    
    public async Task SetContapymeAsync()
    {
        SettingsService settings = new SettingsService();
        _connectionInformation = settings.GetConnectionStrings();

        _cantProducts = _connectionInformation.cantProducts;
        _itdoper = _connectionInformation.itdoper;

        _arrParams[2] = _connectionInformation.iapp!; // IAPP
        _arrParams[3] = _random.Next(0, 9).ToString(); // Random number from 0 to 9
        _arrParams[1] = await GetAuthAsync(); // Keyagent
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

    private async Task<ContapymeResult> _requestPostAsync(Uri endpoint, Dictionary<string, Array> objSend)
    {
        HttpResponseMessage httpResponseMessage = null;
        string responseContent = null;

        try
        {
            using var httpClient = new HttpClient();
            var newPostJson = JsonConvert.SerializeObject(objSend);
            var payload = new StringContent(newPostJson, System.Text.Encoding.UTF8, "application/json");
            LoggerService.Info("Connection: sending request to " + endpoint + " with payload: " + newPostJson);

            httpResponseMessage = await httpClient.PostAsync(endpoint, payload);
            LoggerService.Info("Connection: response received from " + endpoint + " with status code: " +
                               httpResponseMessage.StatusCode);

            responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            // Check if the response is empty or not in the expected format
            if (string.IsNullOrEmpty(responseContent))
            {
                LoggerService.Error("Connection: Empty response received from " + endpoint);
                // Handle this case as needed, e.g., return an error response
            }
            else
            {
                _contapymeResult = JsonConvert.DeserializeObject<ContapymeResult>(responseContent)!;
            }

            return _contapymeResult;
        }
        catch (HttpRequestException httpException)
        {
            LoggerService.Error("Connection: " + httpException.Message);

            // Log the response content and status code in case of an exception
            if (httpResponseMessage != null)
            {
                LoggerService.Error("Connection: Status Code: " + httpResponseMessage.StatusCode);
                LoggerService.Error("Connection: Response Content: " + responseContent);
            }

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
                            mensaje = "Connection: " + httpException.Message,
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
            LoggerService.Warning("JSON Deserialization Error: " + jsonException.Message);

            // Log the response content in case of a JSON deserialization error
            LoggerService.Warning("JSON Deserialization Error: Response Content: " + responseContent);

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
                            mensaje = "Connection: " + jsonException.Message,
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

    private JObject[] _getOperations(string orderNumber)
    {
        string operations = "{\"inumoper\": \"" + (orderNumber) + "\", \"itdoper\": \"" + _itdoper + "\"}";
        _operationsArray[0] = JObject.Parse(operations);
        return _operationsArray;
    }

    /***** Authentication *****/

    private async Task<string> GetAuthAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"GetAuth\"/");

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
        
        JToken datosToken = response.result[0].respuesta.datos;
        ContapymeBodyDataAuthentication data = datosToken.ToObject<ContapymeBodyDataAuthentication>();
        
        agentkey = data.keyagente;
        return data.keyagente;
    }

    public async Task CloseAgentAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TBasicoGeneral/\"Logout\"/");
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(""));

        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );
    }

    /***** Actions *****/
    public async Task Unprocess(string orderNumber)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        ContapymeParameters objParams = new ContapymeParameters
        {
            accion = "UNPROCESS",
            operaciones = _getOperations(orderNumber),
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        JToken datosToken = response.result[0].respuesta.datos;
        ContapymeBodyData data = datosToken.ToObject<ContapymeBodyData>();
    }

    public async Task<OrderEntity> Load(string orderNumber)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");

        ContapymeParameters objParams = new ContapymeParameters
        {
            accion = "LOAD",
            operaciones = _getOperations(orderNumber),
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        JToken datosToken = response.result[0].respuesta.datos;
        OrderEntity data = datosToken.ToObject<OrderEntity>();
        return data;
    }

    public async Task Save(string orderNumber, OrderEntity order)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        ContapymeParametersSave objParams = new ContapymeParametersSave
        {
            accion = "SAVE",
            operaciones = _getOperations(orderNumber),
            oprdata = order
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        JToken datosToken = response.result[0].respuesta.datos;
        ContapymeBodyData data = datosToken.ToObject<ContapymeBodyData>();
    }

    public async Task Taxes(string orderNumber, OrderEntity order)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        ContapymeParametersSave objParams = new ContapymeParametersSave
        {
            accion = "CALCULAR IMPUESTOS",
            operaciones = _getOperations(orderNumber),
            oprdata = order
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        JToken datosToken = response.result[0].respuesta.datos;
        ContapymeBodyData data = datosToken.ToObject<ContapymeBodyData>();
    }

    public async Task Process(string orderNumber)
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatOperaciones/\"DoExecuteOprAction\"/");
        ContapymeParameters objParams = new ContapymeParameters
        {
            accion = "PROCESS",
            operaciones = _getOperations(orderNumber),
        };

        string objSend = JsonConvert.SerializeObject(objParams, Formatting.None);
        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );

        JToken datosToken = response.result[0].respuesta.datos;
        ContapymeBodyData data = datosToken.ToObject<ContapymeBodyData>();
    }

    public async Task<List<ProductEntity>> GetProductsAsync()
    {
        Uri endpoint = new Uri(_connectionInformation.server + "datasnap/rest/TCatElemInv/\"GetListaElemInv\"/");

        string objSend =
            "{\"datospagina\":{\"cantidadregistros\":\"" + _cantProducts +
            "\",\"pagina\":\"\"},\"camposderetorno\":[\"irecurso\",\"nrecurso\",\"clase2\"]}";

        ContapymeResult response = await _requestPostAsync(endpoint, _setParameters(objSend));
        if (response.result[0].encabezado.resultado == false)
            throw new Exception(
                "Connection: the authentication was not processed successfully; please check the exception message"
            );
        
        JToken datosToken = response.result[0].respuesta.datos;
        List<ProductEntity> productList = datosToken.ToObject<List<ProductEntity>>();
        return productList;
    }
}