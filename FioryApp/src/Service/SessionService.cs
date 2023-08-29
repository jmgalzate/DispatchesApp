using FioryLibrary.Connections;
using Newtonsoft.Json.Linq;

namespace FioryApp.src.Service
{
	public class SessionService
	{
        public string KeyAgent { get; set; } = ""; // Initialize as empty string

        public event EventHandler SessionStateChanged;

        public void UpdateSessionKeyAgent(string keyAgent)
        {
            KeyAgent = keyAgent;
            SessionStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

