namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;

public class ConnectionInfoResponse
{
    public string ConnectionString { get; set; }
    public string ConnectionType { get; set; }
    public int ConnectionId { get; set; }
    public string CurrentEnv { get; set; }
}
