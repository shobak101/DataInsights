using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using System.Configuration;
using DataInsights.DataCollector;

namespace TwitterConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitterDataCollector twitterDataCollector = new TwitterDataCollector();
            CollectorRunner collectorRunner = new CollectorRunner(twitterDataCollector);
            collectorRunner.Run();
        }
    }
}
