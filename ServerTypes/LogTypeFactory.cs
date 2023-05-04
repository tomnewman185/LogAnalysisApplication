using System;
using System.Linq;
using LogAnalysisTool.ServerTypes.Apache;

namespace LogAnalysisTool.ServerTypes
{
    /// <summary>
    /// LogTypeFactory - Factory class to create the different types of log
    /// </summary>
    internal class LogTypeFactory
    {
        /// <summary>
        /// GetLogType() - Creates new LogType of the logs that exist if called, else throws exception stating the
        /// LogType is not supported.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ILogTypeInfo GetLogType(LogType logType)
        {
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
