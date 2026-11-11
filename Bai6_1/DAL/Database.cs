using Microsoft.Data.SqlClient;

namespace Bai6_1.DAL
{
    public class Database
    {
        private static readonly string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=SalesDB;
              Integrated Security=True;
              TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}