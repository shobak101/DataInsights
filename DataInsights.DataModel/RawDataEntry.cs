using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsights.DataModel
{
    public class RawDataEntry
    {
        public String Id { get; set; }
        public String Source { get; set; }
        public String Author { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Content { get; set; }
        public object RawContent { get; set; }

        public void Commit()
        {
            DataBaseHelper.RecordRawData(this);
        }
    }
}
