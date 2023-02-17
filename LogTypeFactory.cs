using LogAnalysisTool.Apache_Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    //Class to create the different types of log
    internal class LogTypeFactory
    {
        public static ILogTypeInfo GetLogType(LogType logType) 
        {
            //Creates new LogType of the logs that exist if called, else throws exception stating the LogType is not supported.
            switch (logType)
            {
                case LogType.Apache:
                    return new ApacheLogComponents();
                default:
                    throw new ArgumentException("The type of log is not recognised or supported.", nameof(logType));
            }
        }
    }
}
