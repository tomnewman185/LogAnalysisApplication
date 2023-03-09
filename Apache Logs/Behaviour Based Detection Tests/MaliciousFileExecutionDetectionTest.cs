using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Apache_Logs.Behaviour_Based_Detection_Tests
{
    class MaliciousFileExecutionDetectionTest : BehaviouralDetectionTestBase
    {
        public MaliciousFileExecutionDetectionTest() 
        {
            Regex = new Regex(@"(https?|ftp|php|data):");
        }

        public override IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineNumber)
        {
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            foreach (Match m in Regex.Matches(request))
            {
                if (m.Success)
                {
                    yield return new MaliciousLogEntryInfo(line, match, group, m.Index, lineNumber, m.Length, this);
                }
            }
        }

        private Regex Regex { get; set; }
        public override string Description => "Malicious File Execution Test";
        public override string Name => "Malicious File Execution Test";

    }
}
