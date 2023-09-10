using Newtonsoft.Json;

namespace DispatchesApp.Entity;

public class ProductEntity
{
	[JsonProperty("nrecurso")]
	public string name { set; get; }
	[JsonProperty("clase2")]
	public string barcode { set; get; }
	[JsonProperty("irecurso")]
	public string code { set; get; }
	public int quantity { set; get; }
	public int requested { set; get; }
}
