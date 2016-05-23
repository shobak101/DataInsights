using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInsights.DataModel;

namespace DataInsights.DataAnalyzer
{
    public interface IDataAnalyzer
    {
        IEnumerable<RawDataEntry> GetRawDataBatch();

        void SaveAnalyzedData(IEnumerable<IAnalyzedDataEntry> analyzedData);

        IEnumerable<IAnalyzedDataEntry> AnalyzeData(IEnumerable<RawDataEntry> RawData);

        void UpdateCursor();
    }
}
