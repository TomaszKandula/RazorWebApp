using Microsoft.Extensions.Configuration;

namespace SecureWebApp.Extensions.ConnectionService
{

    public class ConnectionService : IConnectionService
    {

        private IConfiguration FConfiguration { get; }

        public ConnectionService(IConfiguration AConfiguration)
        {
            FConfiguration = AConfiguration;
        }

        public string GetMainDatabase()
        {
            return FConfiguration.GetConnectionString("DbConnect");
        }

        // Add more databases as required here...

    }

}
