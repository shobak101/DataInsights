using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;

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
                String rawObject = (entry.RawContent == null) ? null : JsonConvert.SerializeObject(entry.RawContent);
                if (SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    SqlConnection.Open();
                }
                String command = @"INSERT INTO RAWDATATABLE (source,author,timestamp,content,rawcontent) 
                                VALUES (@source,@author,@timestamp,@content,@rawcontent)";
                
                SqlCommand sqlCommand = new SqlCommand(command, SqlConnection);
                sqlCommand.Parameters.AddWithValue("@source", entry.Source);
                sqlCommand.Parameters.AddWithValue("@author", entry.Author);
                sqlCommand.Parameters.AddWithValue("@timestamp", entry.TimeStamp);
                sqlCommand.Parameters.AddWithValue("@content", entry.Content);
                sqlCommand.Parameters.AddWithValue("@rawcontent", rawObject);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Committing to SQL failed with exception: {0}", e);
            }
        }
    }
}
