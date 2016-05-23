using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsights.DataModel
{
    public static class AnalyzerDataHelper
    {
        public static IEnumerable<RawDataEntry> GetRawDataForAnalyzer(String analyzerId)
        {
            return DataBaseHelper.GetRawDataForAnalyzer(analyzerId);
        }

        public static void InsertSentimentAnalyzerData(String analyzerId, IEnumerable<IAnalyzedDataEntry> sentiments)
        {
            List<IAnalyzedDataEntry> sentimentsList = sentiments.ToList();
            foreach (var item in sentimentsList)
            {
                DataBaseHelper.RecordSentimentAnalyzedData(analyzerId, item.Key, item.Value);
            }
        }

        public static void UpdateAnalyzerCursorValue(String analyzerId, String newCursorValue)
        {
            DataBaseHelper.UpdateAnalyzerCursor(analyzerId, newCursorValue);
        }
    }
}
