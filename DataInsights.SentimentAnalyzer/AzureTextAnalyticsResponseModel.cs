using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsights.SentimentAnalyzer
{
    public class AzureTextAnalyticsResponseModel
    {
        public List<AzureTextAnalyticsEntry> Documents;
        public List<string> Errors;
    }

    public class AzureTextAnalyticsEntry
    {
        public String Score { get; set; }
        public String Id { get; set; }
    }
}
