using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    // SQL Injection Keyword Detection Test
    internal class SQLInjectionKeywordDetectionTest : BehaviouralDetectionTestBase
    {
        public SQLInjectionKeywordDetectionTest(HashSet<string> torExitNodeIPAddresses, string sqlKeywords) : base(torExitNodeIPAddresses)
        {
            // Pull current SQL keyword from string list defined in BehaviouralDetectionTestFactory
            SQLKeywords = sqlKeywords;

            // Build and set the regex pattern for the current SQL keyword
            SetRegexPattern(BuildRegexPattern(sqlKeywords));
        }

        // Builds the regex pattern for the keyword currently being passed in from the list contained in BehaviouralDetectionTestFactory
        private static string BuildRegexPattern(string sqlKeywords)
        {
            string regexPattern = string.Empty;

            // Splits the current SQL keyword into its individual characters
            var chars = ($"{sqlKeywords}").ToCharArray();

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
            sqlInjectionKeywordRegularExpression = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        // Carry out tests to detect signature of the current SQL keyword
        public override IEnumerable<MaliciousLogEntryInfo> ConductTests(Match match, string line, int lineNumber)
        {
            // Defining the request group, which is the group that needs to be searched for the attack
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            // For each signature detection, return a new malicious log match
            foreach (Match m in sqlInjectionKeywordRegularExpression.Matches(request))
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

        private Regex sqlInjectionKeywordRegularExpression { get; set; }

        string SQLKeywords { get; set; }

        // Attack Description
        public override string Description => "SQL Injection - A test to detect if a user has injected SQL keywords into the URL to try and manipulate an internal database.";

        // Attack Name
        public override string Name => $"{SQLKeywords} - Potential SQL Injection Detection";
    }
}
