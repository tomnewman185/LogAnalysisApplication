using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    //Interface for a behavioural-based detection test, which inherits from the detection test interface
    internal interface IBehaviouralDetectionTest : ITest
    {
        IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineCounter);
    }
}
