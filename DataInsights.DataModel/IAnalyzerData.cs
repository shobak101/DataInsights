using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsights.DataModel
{
    public interface IAnalyzedDataEntry
    {
        String Key { get; set; }
        String Value { get; set; }   
    }
}
