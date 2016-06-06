using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MLBDataCollector mlbCollector = new MLBDataCollector();
            string scores = mlbCollector.GetBoxScores("06", "05", "2016", "g4eryffabm8ap4v3f5zw7jem");
            mlbCollector.saveBoxScores(scores);
        }
    }
}
