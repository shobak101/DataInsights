using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DataInsights.DataAnalyzer;

namespace DataInsights.SentimentAnalyzer
{
    static class Program
    {
        static void Main()
        {
            SentimentAnalyzer sentimentAnalyzer = new SentimentAnalyzer();
            DataAnalyzerRunner sentimentAnalyzerRunner = new DataAnalyzerRunner(sentimentAnalyzer);
            sentimentAnalyzerRunner.Run();
        }        
    }
}
