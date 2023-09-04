using FioryApp.src.Entity;
using FioryApp.src.Service.Sales;

namespace FioryApp.src.Service;

public class SessionService
{
    public string keyAgent { get; private set; }
    public OrderEntity order { get; set; }
    public OrderEntity dispatch { get; set; }
    public List<ProductEntity> stock { get; private set; }
    
    public SessionService()
    {
        order = new OrderEntity();
        dispatch = new OrderEntity();
        stock = new List<ProductEntity>();
    }

    public event EventHandler SessionStateChanged;

    public void UpdateSessionKeyAgent(string agentToken)
    {
        keyAgent = agentToken;
        SessionStateChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void UpdateSessionProducts(List<ProductEntity> products)
    {
        stock = products;
        SessionStateChanged?.Invoke(this, EventArgs.Empty);
    }
}