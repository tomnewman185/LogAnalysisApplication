using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    /// <summary>
    /// IBehaviouralDetectionTest - Interface for a behavioural-based detection test, which inherits from the test
    /// interface
    /// </summary>
    internal interface IBehaviouralDetectionTest : ITest
    {
        IEnumerable<MaliciousLogEntryInfo> ConductTests(Match match, string line, int lineCounter);
    }
}
