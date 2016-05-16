using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataModel
{
    public static class DataBaseHelper
    {
        private static String ConnectionString = Environment.GetEnvironmentVariable("DataBaseConnectionString");
        private static SqlConnection SqlConnection = new SqlConnection(ConnectionString);
        public static void RecordRawData(RawDataEntry entry)
        {
            try
            {
                if (SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    SqlConnection.Open();
                }
                String command = String.Format("INSERT INTO RAWDATATABLE (source,author,timestamp,content) VALUES ('{0}','{1}','{2}','{3}')", entry.Source, entry.Author, entry.TimeStamp, entry.Content);
                SqlCommand sqlCommand = new SqlCommand(command, SqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Committing to SQL failed with exception: {0}", e);
            }
        }
    }
}
