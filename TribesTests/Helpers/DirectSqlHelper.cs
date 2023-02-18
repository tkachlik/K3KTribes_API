using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace TribesTests.Helpers
{
    public class DirectSqlHelper
    {
        private readonly IConfiguration _config;

        public DirectSqlHelper()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string restOfAddress = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) restOfAddress += @"/appsettings.json";
            else restOfAddress += @"\appsettings.json";

            _config = new ConfigurationBuilder()
                .AddJsonFile($"{currentDirectory}{restOfAddress}")
                .Build();
        }

        public void DeleteCreatedPlayer()
        {
            string queryString = "DELETE FROM Players WHERE Id>2;";
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("Test")))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader());
            }
        }

        public void DeleteCreatedWorld()
        {
            string queryString = "DELETE FROM World WHERE Id>2;";
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("Test")))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader());
            }
        }
    }
}