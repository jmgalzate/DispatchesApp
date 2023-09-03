using Newtonsoft.Json.Linq;

namespace FioryApp.src.Entity;

/*** Objects to deserialize the generic API response ***/
public class ContapymeResult
{
    public List<ContapymeEntity> result { set; get; }
}

public class ContapymeEntity
{
    public ContapymeHeader encabezado { set; get; }
    public ContapymeBody respuesta { set; get; }
}


public class ContapymeHeader
{
    public bool resultado { set; get; }
    public string imensaje { set; get; }
    public string mensaje { set; get; }
    public string tiempo { set; get; }
    public string version { set; get; }
}

public class ContapymeBody
{
    public JObject datos { set; get; }
}

/**** Objects to deserialize each kind of API body response ****/

public class ContapymeBodyData
{
    //This class handle the Unprocess, Save and Calcular Impuestos actions. 
    #nullable enable
    public string? resultado { set; get; }
    #nullable enable
    public string? qoprsok { set; get; }
    /*
        TODO: As I can have more than one kind of data on here, so I need to create a Pholymorphism system to handle it. These are some data:
        - [ ] List of products
        - [ ] Authentication
        - [x] Process
        - [x] Unprocess 
        - [x] Save
        - [x] Calcular impuestos
        - [ ] Close Session
    */
}

public class ContapymeBodyDataAuthentication
{
    /**
    Authentication:
    "datos": {
        "keyagente": "17E3AB03B9",
        "version": "4",
        "release": "8",
        "actualizacion": "20",
        "fase": "",
        "compilacion": "5",
        "versionapiagente": "1"
    }
    **/
    public string keyagente {set; get; }
    public string version {set; get; }
    public string release {set; get; }
    public string actualizacion {set; get; }
    public string fase {set; get; }
    public string compilacion {set; get; }
    public string versionapiagente {set; get; }
}

public class ContapymeBodyDataCloseSession
{
    /**
     * Close Session:
     * Close Session: 
        "datos": {
            "cerro": "true"
        }
     */
    public string cerro {set; get; }
}

public class ContapymeBodyDataProcess : ContapymeBodyData
{
    //
    /**
    Process:
    "datos": {
        "resultado": "T",
        "errores": "0",
        "advertencias": "0",
        "bitacora": "<body bgcolor=\"#CCCCCC\"><br><font face=\"Tahoma\" color=\"black\"><span style=\"font-size:16px\">Verificación . . .</span></font><br><br><font face=\"Tahoma\" color=\"black\"><span style=\"font-size:16px\"></span></font><br><br><font face=\"Tahoma\" color=\"black\"><span style=\"font-size:16px\">Proceso . . .</span></font><br><br><br><font face=\"Tahoma\" color=\"black\"><span style=\"font-size:16px\">Proceso Creación de una solicitud de pedido de un cliente. . .</span></font><br><br><br></body>",
        "qoprsok": "1"
    }
    **/
    public string errores {set; get; }
    public string advertencias {set; get; }
    public string bitacora {set; get; }
}


/********** API RESPONSES **********/

/*
load: OrderEntity.

Get List of Products: 
"datos": [
    {
        "irecurso": "1407010002",
        "nrecurso": "RUANA FIORY FUCSIA",
        "clase2": "0000000002059"
    },
    {
        "irecurso": "1407070000",
        "nrecurso": "BUSO CUELLO BANDEJA CLARISSA-1266",
        "clase2": "0000000008938"
    },
    {
        "irecurso": "1407070001",
        "nrecurso": "BUSO CUELLO V ONIX-1271",
        "clase2": "0000000008945"
    },
    {
        "irecurso": "1407070002",
        "nrecurso": "CAMISILLA VIOLETA-1227",
        "clase2": "0000000008952"
    },
    {
        "irecurso": "1407070003",
        "nrecurso": "CAMISILLA TIRAS CELINA-1265",
        "clase2": "0000000008969"
    }
]
*/