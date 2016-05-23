using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace DataInsights.DataModel
{
    public class DBQueries
    {
        public static SqlCommand GetInsertRawDataQuery(RawDataEntry entry, SqlConnection sqlConnection)
        {
            String rawObject = (entry.RawContent == null) ? "" : JsonConvert.SerializeObject(entry.RawContent);
            String command = @"INSERT INTO RAWDATATABLE (source,author,timestamp,content,rawcontent) 
                                VALUES (@source,@author,@timestamp,@content,@rawcontent)";

            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@source", entry.Source);
            sqlCommand.Parameters.AddWithValue("@author", entry.Author);
            sqlCommand.Parameters.AddWithValue("@timestamp", entry.TimeStamp);
            sqlCommand.Parameters.AddWithValue("@content", entry.Content);
            sqlCommand.Parameters.AddWithValue("@rawcontent", rawObject);
            return sqlCommand;
        }

        public static SqlCommand GetRetrieveRawRecordsForProcessingQuery(String id, SqlConnection sqlConnection)
        {
            String command = @"SELECT TOP 90 * FROM RAWDATATABLE WHERE Id > @ID ORDER BY Id ASC";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ID", id);
            return sqlCommand;
        }

        public static SqlCommand GetAnalyzerMarkerQuery(String analyzerId, SqlConnection sqlConnection)
        {
            String command = String.Format(@"SELECT CursorValue FROM {0} WHERE AnalyzerId = @AnalyzerId", Constants.DataAnalyzersCursorTableName);
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@AnalyzerId", analyzerId);
            return sqlCommand;
        }

        public static SqlCommand GetUpdateAnalyzerMarkerQuery(String analyzerId, String newCursorValue, SqlConnection sqlConnection)
        {
            String command = String.Format(@"UPDATE {0} SET CursorValue = {1} WHERE AnalyzerId = @AnalyzerId", Constants.DataAnalyzersCursorTableName, newCursorValue);
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@AnalyzerId", analyzerId);
            return sqlCommand;
        }

        public static SqlCommand GetInsertSentimentAnalyzerDataQuery(String sentimentAnalyzerId, String id, String sentiment, SqlConnection sqlConnection)
        {
            String command = String.Format(@"INSERT INTO {0} (Id,Sentiment)
                                            VALUES (@id,@Sentiment)", sentimentAnalyzerId);
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            sqlCommand.Parameters.AddWithValue("@Sentiment", sentiment);
            return sqlCommand;
        }
    }
}
