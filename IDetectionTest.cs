using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool
{
    /// <summary>
    /// IDetectionTest - Interface for a detection test, which inherits from the test interface
    /// </summary>
    internal interface IDetectionTest : ITest
    {
        IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineCounter);
    }
}
