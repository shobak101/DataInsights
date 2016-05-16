using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    public class CollectorRunner
    {
        private IDataCollector _collector;
        public CollectorRunner(IDataCollector collector)
        {
            _collector = collector;
        }

        public void Run()
        {
            _collector.SetUpListener();
            _collector.StartListening();
        }

        public void Stop()
        {
            _collector.StopListening();
        }
    }
}
