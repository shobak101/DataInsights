using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace TwitterConnector.WebAPI
{
    public class Constants
    {
        public static String JobLocation = @"D:\home\site\wwwroot\app_data\jobs\continuous\deployedjob";
        public static String KeyWordsFileName = "KeyWords.txt";
        public static String KeyWordsFileLocation = Path.Combine(JobLocation, KeyWordsFileName);
    }
}