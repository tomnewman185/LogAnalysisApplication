﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Apache_Logs.Behaviour_Based_Detection_Tests
{
    internal class HTMLKeywordDetectionTest : BehaviouralDetectionTestBase
    {
        public HTMLKeywordDetectionTest(string htmlKeyword)
        {
            HTMLKeyword = htmlKeyword;
            SetRegexPattern(BuildRegexPattern(HTMLKeyword));
        }

        //Builds the regex pattern for the keyword currently being passed in from the list contained within the factory class
        private static string BuildRegexPattern(string htmlKeyword)
        {
            string regexPattern = string.Empty;
            var chars = ($"<{htmlKeyword}").ToCharArray();
            
            foreach ( char c in chars )
            {
                regexPattern += $"({Uri.HexEscape(c)}|{c})";
            }

            return regexPattern;
        }

        //Sets the regex pattern for the current keyword
        private void SetRegexPattern(string regexPattern)
        {
            regexStatement = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        //Conducts the test for the keyword on each line of the log file, returning a new malicious log file object if a detection is found
        public override IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineNumber)
        {
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            foreach (Match m in regexStatement.Matches(request))
            {
                if (m.Success)
                {
                    yield return new MaliciousLogEntryInfo(line, match, group, m.Index, lineNumber, m.Length, this);
                }
            }
        }

        public string HTMLKeyword { get; set; }
        private Regex regexStatement { get; set; }
        public override string Description => $"Check For <{HTMLKeyword} Element";
        public override string Name => $"<{HTMLKeyword} Test";
    }
}