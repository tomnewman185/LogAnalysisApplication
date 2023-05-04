using System;
using System.Collections.Generic;
using System.Linq;
using LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests.Apache;

namespace LogAnalysisTool.ServerTypes
{
    /// <summary>
    /// ILogTypeInfo - Interface for a log type
    /// </summary>
    internal interface ILogTypeInfo
    {
        IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests(HashSet<string> torExitNodeIPAddresses);

        string LogComponentsRegularExpression { get; }

        string Name { get; }
    }
}
