using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace DataInsights.DataModel
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
                SqlCommand sqlCommand = DBQueries.GetInsertRawDataQuery(entry, SqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Committing to SQL failed for RecordRawData with exception: {0}", e);
            }
        }

        public static IEnumerable<RawDataEntry> GetRawDataForAnalyzer(String AnalyzerId)
        {
            List<RawDataEntry> result = new List<RawDataEntry>();
            try
            {
                if (SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    SqlConnection.Open();
                }
                SqlCommand sqlCommand = DBQueries.GetAnalyzerMarkerQuery(AnalyzerId, SqlConnection);
                String id = sqlCommand.ExecuteScalar().ToString();

                sqlCommand = DBQueries.GetRetrieveRawRecordsForProcessingQuery(id, SqlConnection);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            RawDataEntry entry = new RawDataEntry
                            {
                                Id = reader.GetInt32(0).ToString(),
                                Source = reader.GetString(1),
                                Author = reader.GetString(2),
                                TimeStamp = reader.GetDateTime(3),
                                Content = reader.GetString(4)
                            };
                            result.Add(entry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Committing to SQL failed for GetRawDataForAnalyzer with exception: {0}", e);
            }
            return result;
        }

        public static void RecordSentimentAnalyzedData(String analyzerId, String id, String sentiment)
        {
            try
            {
                if (SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    SqlConnection.Open();
                }
                SqlCommand sqlCommand = DBQueries.GetInsertSentimentAnalyzerDataQuery(analyzerId, id, sentiment, SqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Committing to SQL failed for RecordSentimentAnalyzedData with exception: {0}", e);
            }
        }

        public static void UpdateAnalyzerCursor(String analyzerId, String newCursorValue)
        {
            try
            {
                if (SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    SqlConnection.Open();
                }
                SqlCommand sqlCommand = DBQueries.GetUpdateAnalyzerMarkerQuery(analyzerId, newCursorValue, SqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Committing to SQL failed for UpdateAnalyzerCursor with exception: {0}", e);
            }
        }
    }
}
