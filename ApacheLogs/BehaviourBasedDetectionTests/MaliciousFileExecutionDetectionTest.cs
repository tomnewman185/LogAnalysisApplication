using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    // Malicious File Execution Detection Test
    class MaliciousFileExecutionDetectionTest : BehaviouralDetectionTestBase
    {
        public MaliciousFileExecutionDetectionTest(HashSet<string> torExitNodeIPAddresses) : base(torExitNodeIPAddresses)
        {
            // Definition of regular expression for test
            MaliciousFileExecutionRegularExpression = new Regex(@"(https?|ftp|php|data):");
        }

        // Carry out tests to detect signature of a Malicious File Execution attack
        public override IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineNumber)
        {
            // Defining the request group, which is the group that needs to be searched for the attack
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            // For each signature detection, return a new malicious log match
            foreach (Match m in MaliciousFileExecutionRegularExpression.Matches(request))
            {
                if (m.Success)
                {
                    yield return new MaliciousLogEntryInfo(line,
                                                           match,
                                                           group,
                                                           m.Index,
                                                           lineNumber,
                                                           m.Length,
                                                           this,
                                                           IsTorExitNodeIPAddress(match.Groups[ApacheLogComponents.RegexComponentGroups.RemoteHost].Value));
                }
            }
        }

        private Regex MaliciousFileExecutionRegularExpression { get; set; }

        // Attack Description
        public override string Description => "Malicious File Execution - A test to detect if a user has potentially manipulated a file name call to execute a system program or an external URL.";

        // Attack Name
        public override string Name => "Potential Malicious File Execution Detection";
    }
}
