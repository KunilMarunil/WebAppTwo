using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApp.Models
{
    public class CRUD
    {
        private static string conString = "";

        #region GetConfiguration
        public static void GetConfiguration(IConfiguration configuration)
        {
            conString = configuration["ConnectionStrings:DefaultConnection"];
        }

        #endregion

        #region ExecuteQuery
        public static DataTable ExecuteQuery(string query, SqlParameter[] sqlParams = null)
        {
            DataTable result = new DataTable();

            // begin connection
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                #region query process to database
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (sqlParams != null) cmd.Parameters.AddRange(sqlParams);
                    // mengisi dengan SqlDataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(result);
                }
                #endregion

                // close connection
                con.Close();
            }

            return result;
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string query, SqlParameter[] sqlParams = null)
        {
            int result = 0;

            // begin connection
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                #region query process to database
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (sqlParams != null) cmd.Parameters.AddRange(sqlParams);
                    result = cmd.ExecuteNonQuery(); // ExecuteNonQuery untuk query yang tidak return apa-apa
                }
                #endregion

                // close connection
                con.Close();
            }

            return result;
        }
        #endregion

        public static void LogAuditTrail(int userId, string username, string menuAccessed, string activity, string dataChanges = null)
        {
            string query = @"
        INSERT INTO AuditTrail (UserId, Username, MenuAccessed, Activity, ActivityTimestamp, DataChanges)
        VALUES (@UserId, @Username, @MenuAccessed, @Activity, GETDATE(), @DataChanges)";

            SqlParameter[] parameters = {
        new SqlParameter("@UserId", userId),
        new SqlParameter("@Username", username),
        new SqlParameter("@MenuAccessed", menuAccessed),
        new SqlParameter("@Activity", activity),
        new SqlParameter("@DataChanges", (object)dataChanges ?? DBNull.Value) // Set to null if no data changes
    };

            ExecuteNonQuery(query, parameters);
        }

    }
}
