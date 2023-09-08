
using FioryApp.Entity;

namespace FioryApp.src.Entity;

public class ContapymeParameters
{
	public string accion { get; set; }
	public Array operaciones { get; set; }
}

public class ContapymeParametersSave : ContapymeParameters
{
	public OrderEntity oprdata { get; set; }
}
