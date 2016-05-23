using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInsights.DataModel;

namespace DataInsights.SentimentAnalyzer
{
    public class AnalyzedSentiment : IAnalyzedDataEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
