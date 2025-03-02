namespace OrderMgmtRevision.Config
{
    public class FedExSettings
    {
        public string ConnectionString { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public string ApiSecret { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string LtlShipperAccountNumber { get; set; } = "";
        public string BaseUrl { get; set; } = "";
    }
}
