using Microsoft.Data.SqlClient;

namespace Bai6_2.DAL
{
    public class Database
    {
        private static readonly string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=HRMDB;
              Integrated Security=True;
              TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}