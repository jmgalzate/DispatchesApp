
namespace FioryApp.src.Entity;

public class ContapymeEntity
{
    public ContapymeEncabezado encabezado { set; get; }
    public ContapymeRespuestaDatos datos { set; get; }
}

public class ContapymeEncabezado
{
    public string resultado { set; get; }
    public string imensaje { set; get; }
    public string mensaje { set; get; }
    public string tiempo { set; get; }
    public string version { set; get; }
}

/**
* This class is used to store the data from the response of the Contapyme API. The API body is respuesta > datos {}, so I will not record the respuesta part.
*/

public class ContapymeRespuestaDatos
{
    /*
        TODO: As I can have more than one kind of data on here, so I need to create a Pholymorphism system to handle it. 
    */
}
