using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LogAnalysisTool.ServerTypes.Apache;

namespace LogAnalysisTool.Detections.BehaviourBasedDetectionTests.Apache.Tests
{
    // Advanced Injection Detection Test
    internal class AdvancedInjectionDetectionTest : BehaviouralDetectionTestBase
    {
        public AdvancedInjectionDetectionTest(HashSet<string> torExitNodeIPAddresses) : base(torExitNodeIPAddresses)
        {
            // Definition of regular expression for test
            AdvancedInjectionRegularExpression = new Regex(@"(%3D|=)[^\n]*((%27|')|(--)|(%3B|;))", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        // Carry out tests to detect signature of an Advanced Injection attack
        public override IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineNumber)
        {
            // Defining the request group, which is the group that needs to be searched for the attack
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            // For each signature detection, return a new malicious log match
            foreach (Match m in AdvancedInjectionRegularExpression.Matches(request))
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

        private Regex AdvancedInjectionRegularExpression { get; set; }

        // Attack Description
        public override string Description => "Advanced Injection - A test to detect a potential advanced injection attack based on the use of specific symbols - (=) (') (;).";

        // Attack Name
        public override string Name => "Potential Advanced Injection Attack Detection (=) (') (;)";
    }
}
