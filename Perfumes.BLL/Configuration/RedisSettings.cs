namespace Perfumes.BLL.Configuration
{
    public class RedisSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string InstanceName { get; set; } = "Perfumes_";
        public int DefaultDatabase { get; set; } = 0;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;
        public bool AbortConnect { get; set; } = false;
        public int ConnectRetry { get; set; } = 3;
        public bool KeepAlive { get; set; } = true;
    }
} 