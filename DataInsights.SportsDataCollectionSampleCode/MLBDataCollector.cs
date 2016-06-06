using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebClientTest
{
    class MLBDataCollector
    {
        // constructor
        public MLBDataCollector()
        {
            url = "http://api.sportradar.us/mlb-t5/games/";
        }

        // get real-time box scores
        public string GetBoxScores(string month, string day, string year, string key)
        {
            url = url + year;
            url = url + '/';
            url = url + month;
            url = url + '/';
            url = url + day;
            url = url + "/boxscore.xml?api_key=";
            url = url + key;
            Console.WriteLine(url);
            WebClient client = new WebClient();
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string boxScores = reader.ReadToEnd();
            data.Close();
            reader.Close();
            return boxScores;
        }

        // save box scores to fiel
        //
        // TO DO:  convert to save to database
        //
        public void saveBoxScores(string scores)
        {
            File.WriteAllText(@"C:\CSS553TestData\sportsAPITest.xml", scores);
        }

        // base URL
        private string url;
    }
}
