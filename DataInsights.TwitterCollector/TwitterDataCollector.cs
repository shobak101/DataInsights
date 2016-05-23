using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using System.IO;
using Tweetinvi.Core.Interfaces.Streaminvi;
using DataInsights.DataModel;
using DataInsights.DataCollector;

namespace TwitterConnector
{
    public class TwitterDataCollector : IDataCollector
    {
        private IFilteredStream _stream;
        public void SetUpListener()
        {
            // Set up your credentials (https://apps.twitter.com)
            Auth.SetUserCredentials(Environment.GetEnvironmentVariable("CONSUMER_KEY"), Environment.GetEnvironmentVariable("CONSUMER_SECRET"),
                                    Environment.GetEnvironmentVariable("ACCESS_TOKEN"), Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET"));

            // Publish the Tweet "Hello World" on your Timeline
            //Tweet.PublishTweet("Hello World!");

            _stream = Tweetinvi.Stream.CreateFilteredStream();

            var keyWordString = File.ReadAllText("KeyWords.txt");
            var keyWords = keyWordString.Split(',');

            foreach (var word in keyWords)
            {
                _stream.AddTrack(word);
            }
            if (!File.Exists("out.txt"))
            {
                File.Create("out.txt");
            }
            var outputFile = new StreamWriter("out.txt", true);
            _stream.MatchingTweetReceived += (sender, arguments) =>
            {
                string message = "A tweet containing '" + arguments.MatchingTracks[0] + "' has been found; the tweet is '" + arguments.Tweet + "' created by " + arguments.Tweet.CreatedBy;
                Console.WriteLine(message);
                RawDataEntry entry = new RawDataEntry
                {
                    Id = "",
                    Source = "Twitter",
                    Author = arguments.Tweet.CreatedBy.ToString(),
                    Content = arguments.Tweet.ToString(),
                    TimeStamp = arguments.Tweet.CreatedAt
                };
                entry.Commit();
            };
        }

        public void StartListening()
        {
            _stream.StartStreamMatchingAllConditions();
        }

        public void StopListening()
        {
            _stream.StopStream();
        }
    }
}
