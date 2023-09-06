using FioryApp.src.Entity;

namespace FioryApp.src.Service;

public class SessionService
{
    public string keyAgent { get; private set; }
    public List<ProductEntity> productsList { get; private set; }
    public int sessionProducts { get; private set; } = 0;

    public event EventHandler SessionStateChanged;

    public void UpdateSessionKeyAgent(string agentToken)
    {
        keyAgent = agentToken;
        SessionStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateSessionProducts(List<ProductEntity> products)
    {
        if (products == null)
        {
            sessionProducts = 0;
            SessionStateChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            productsList = products;
            sessionProducts = products.Count;
            SessionStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}