using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests;

namespace LogAnalysisTool
{
    /// <summary>
    /// ILogTypeInfo - Interface for a log type
    /// </summary>
    internal interface ILogTypeInfo
    {
        string Name { get; }

        string LogComponentsRegularExpression { get; }

        IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests(HashSet<string> torExitNodeIPAddresses);
    }
}
