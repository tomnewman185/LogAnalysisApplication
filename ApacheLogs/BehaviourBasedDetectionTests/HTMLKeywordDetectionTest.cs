using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    // HTML Keyword Detection Test
    internal class HTMLKeywordDetectionTest : BehaviouralDetectionTestBase
    {
        public HTMLKeywordDetectionTest(HashSet<string> torExitNodeIPAddresses, string htmlKeyword) : base(torExitNodeIPAddresses)
        {
            // Pull current HTML keyword from string list defined in BehaviouralDetectionTestFactory
            HTMLKeyword = htmlKeyword;

            // Build and set the regex pattern for the current HTML keyword
            SetRegexPattern(BuildRegexPattern(HTMLKeyword));
        }

        // Builds the regex pattern for the keyword currently being passed in from the list contained in BehaviouralDetectionTestFactory
        private static string BuildRegexPattern(string htmlKeyword)
        {
            string regexPattern = string.Empty;

            // Splits the current HTML keyword into its individual characters
            var chars = ($"<{htmlKeyword}").ToCharArray();

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
            htmlKeywordRegularExpression = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        // Carry out tests to detect signature of the current HTML keyword
        public override IEnumerable<MaliciousLogEntryInfo> ConductTests(Match match, string line, int lineNumber)
        {
            // Defining the request group, which is the group that needs to be searched for the attack
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            // For each signature detection, return a new malicious log match
            foreach (Match m in htmlKeywordRegularExpression.Matches(request))
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

        private Regex htmlKeywordRegularExpression { get; set; }

        // Attack Description
        public override string Description => "Malicious HTML Element - A test to highlight potentially malicious HTML element injection which can be used to stage an attack, such as a XSS attack.";

        public string HTMLKeyword { get; set; }

        // Attack Name
        public override string Name => $"{HTMLKeyword} - Potential Malicious HTML Element Detection";
    }
}
