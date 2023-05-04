using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests;
using LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests.Apache;
using LogAnalysisTool.ServerTypes.Apache;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests.Apache.Tests
{
    // Insecure Direct Object Reference Detection Test
    internal class InsecureDirectObjectReferenceDetectionTest : BehaviouralDetectionTestBase
    {
        public InsecureDirectObjectReferenceDetectionTest(HashSet<string> torExitNodeIPAddresses) : base(torExitNodeIPAddresses)
        {
            // Definition of regular expression for test
            InsecureDirectObjectReferenceRegularExpression = new Regex(@"(\.|(%|%25)2E)(\.|(%|%25)2E)(\/|(%|%25)2F|\\|(%|%25)5C)",
                                                                       RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        // Carry out tests to detect signature of an Insecure Direct Object Reference attack
        public override IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineNumber)
        {
            // Defining the request group, which is the group that needs to be searched for the attack
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            // For each signature detection, return a new malicious log match
            foreach (Match m in InsecureDirectObjectReferenceRegularExpression.Matches(request))
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

        private Regex InsecureDirectObjectReferenceRegularExpression { get; set; }

        // Attack Description
        public override string Description => "Insecure Direct Object Reference - A test to highlight a potential exposure of internal server objects or files which a user can manipulate to traverse internal files and directories.";

        // Attack Name
        public override string Name => "Potential Insecure Direct Object Reference Attack Detected";
    }
}
