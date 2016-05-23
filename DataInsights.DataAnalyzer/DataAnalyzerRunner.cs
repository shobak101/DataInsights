using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DataInsights.DataAnalyzer
{
    public class DataAnalyzerRunner
    {
        private IDataAnalyzer _analyzer;
        private bool isRunning = false;
        private TimeSpan _pollintInterval = TimeSpan.FromMinutes(2);
        public DataAnalyzerRunner(IDataAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public void Run()
        {
            isRunning = true;
            while (isRunning)
            {
                var rawData = _analyzer.GetRawDataBatch();
                var analyzedData = _analyzer.AnalyzeData(rawData);
                _analyzer.SaveAnalyzedData(analyzedData);
                _analyzer.UpdateCursor();
                Thread.Sleep(_pollintInterval);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
