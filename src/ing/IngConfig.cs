namespace ing
{
    public class IngConfig
    {
        public string ClientId { get; set; }
        public string TokenEndpoint { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string SingingCertificate { get; set; }
        public string ClientCertificate { get; set; }
    }
}