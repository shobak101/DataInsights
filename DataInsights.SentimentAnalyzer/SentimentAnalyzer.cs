using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInsights.DataAnalyzer;
using DataInsights.DataModel;
using System.Net.Http;
using System.Net.Http.Headers;
using DataInsights.Common;
using Newtonsoft.Json;

namespace DataInsights.SentimentAnalyzer
{
    public class SentimentAnalyzer : IDataAnalyzer
    {
        /// <summary>
        /// Azure portal URL.
        /// </summary>
        private const string BaseUrl = "https://westus.api.cognitive.microsoft.com/";

        /// <summary>
        /// Maximum number of languages to return in language detection API.
        /// </summary>
        private const int NumLanguages = 1;

        private String _lastProcessedId = "0";

        public IEnumerable<IAnalyzedDataEntry> AnalyzeData(IEnumerable<RawDataEntry> RawData)
        {
            List<AnalyzedSentiment> result = new List<AnalyzedSentiment>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("AzureAnalyticsAPIKey"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                String BatchItems = "";
                foreach (var item in RawData)
                {
                    BatchItems += "{\"id\":\""+ item.Id+ "\",\"text\":\"" + item.Content.Replace(":","")
                                    .Replace(Environment.NewLine,"").Replace("\"","").Replace("\n","")
                                    .Replace("\r","").Replace("}","").Replace("{","").Replace(".","")
                                    .Replace("…","")+ "\"},";
                }

                BatchItems = "{\"documents\":[" + BatchItems + "]}";
                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes(BatchItems);

                // Detect key phrases:
                //var uri = "text/analytics/v2.0/keyPhrases";
                //var response = CallEndpoint(client, uri, byteData);
                //Console.WriteLine("\nDetect key phrases response:\n" + response);

                // Detect language:
                //var queryString = HttpUtility.ParseQueryString(string.Empty);
                //queryString["numberOfLanguagesToDetect"] = NumLanguages.ToString(CultureInfo.InvariantCulture);
                //uri = "text/analytics/v2.0/languages?" + queryString;
                //response = await CallEndpoint(client, uri, byteData);
                //Console.WriteLine("\nDetect language response:\n" + response);

                // Detect sentiment:
                var uri = "text/analytics/v2.0/sentiment";
                var response = CallEndpoint(client, uri, byteData);
                var analysisResponse = JsonConvert.DeserializeObject<AzureTextAnalyticsResponseModel>(response);
                foreach (var item in analysisResponse.Documents)
                {
                    AnalyzedSentiment aSentiment = new AnalyzedSentiment
                    {
                        Key = item.Id,
                        Value = item.Score
                    };
                    result.Add(aSentiment);
                    if (Int32.Parse(item.Id) > Int32.Parse(_lastProcessedId))
                    {
                        _lastProcessedId = item.Id;
                    }
                }
            }
            return result;
        }

        static String CallEndpoint(HttpClient client, string uri, byte[] byteData)
        {
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(uri, content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public IEnumerable<RawDataEntry> GetRawDataBatch()
        {
            return AnalyzerDataHelper.GetRawDataForAnalyzer(DataAnalyzers.SentimentAnalyzer);
        }

        public void SaveAnalyzedData(IEnumerable<IAnalyzedDataEntry> analyzedData)
        {
            AnalyzerDataHelper.InsertSentimentAnalyzerData(DataAnalyzers.SentimentAnalyzer, analyzedData);
        }

        public void UpdateCursor()
        {
            AnalyzerDataHelper.UpdateAnalyzerCursorValue(DataAnalyzers.SentimentAnalyzer, _lastProcessedId);
        }
    }
}
