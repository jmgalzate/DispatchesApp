namespace FioryApp.Entity;
#nullable enable

/*** Objects to deserialize the generic API response ***/

public class ContapymeEntity
{
    public ContapymeHeader? encabezado { init; get; }
    public ContapymeBody? respuesta { init; get; }
}

/**** Objects to deserialize each kind of API body response ****/