using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    // JavaScript Event Handler Detection Test
    internal class JSEventHandlerDetectionTest : BehaviouralDetectionTestBase
    {
        public JSEventHandlerDetectionTest(HashSet<string> torExitNodeIPAddresses, string jsEventHandlerKeyword) : base(torExitNodeIPAddresses)
        {
            // Pull current JavaScript keyword from string list defined in BehaviouralDetectionTestFactory
            JSEventHandlerKeyword = jsEventHandlerKeyword;

            // Build and set the regex pattern for the current JavaScript keyword
            SetRegexPattern(BuildRegexPattern(JSEventHandlerKeyword));
        }

        // Builds the regex pattern for the keyword currently being passed in from the list contained in BehaviouralDetectionTestFactory
        private static string BuildRegexPattern(string jsEventHandlerKeyword)
        {
            string regexPattern = string.Empty;

            // Splits the current JavaScript keyword into its individual characters
            var chars = ($"{jsEventHandlerKeyword}").ToCharArray();

            // For each character of the keyword, add the letter and its hex equivalent to the regular expression pattern
            foreach (char c in chars)
            {
                regexPattern += $"({Uri.HexEscape(c)}|{c})";
            }

            return regexPattern;
        }

        // Sets the regex pattern for the current keyword
        private void SetRegexPattern(string regexPattern)
        {
            javascriptEventHandlerKeywordRegularExpression = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        // Carry out tests to detect signature of the current JavaScript keyword
        public override IEnumerable<MaliciousLogEntryInfo> ConductTests(Match match, string line, int lineNumber)
        {
            // Defining the request group, which is the group that needs to be searched for the attack
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            // For each signature detection, return a new malicious log match
            foreach (Match m in javascriptEventHandlerKeywordRegularExpression.Matches(request))
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

        public string JSEventHandlerKeyword { get; set; }

        private Regex javascriptEventHandlerKeywordRegularExpression { get; set; }

        // Attack Description
        public override string Description => "JavaScript Event Handler Injection - A test to detect if a user has injected JavaScript event handler keywords to potentially manipulate the server to run external resources.";

        // Attack Name
        public override string Name => $"{JSEventHandlerKeyword} - Potential JavaScript Event Handler Element Detection";
    }
}
