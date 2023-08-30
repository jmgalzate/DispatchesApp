namespace FioryApp.src.Service
{
	public class SessionService
	{
        public string KeyAgent { get; set; } = "";

        public event EventHandler SessionStateChanged;

        public void UpdateSessionKeyAgent(string keyAgent)
        {
            KeyAgent = keyAgent;
            SessionStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

