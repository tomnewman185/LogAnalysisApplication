using LogAnalysisTool.Apache_Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Behaviour_Based_Detection_Tests
{
    internal class ScriptTagDetectionTest : BehaviouralDetectionTestBase
    {

        public override string Description => "Check For Script Tag Element";
        public override string Name => "script element test";
        public override MaliciousLogEntryInfo ConductTest(Match match, string line, int lineCounter)
        {
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];

            var request = group.Value;

            var searchString = "<script";

            int indexWithinGroup = request.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase);
            if (indexWithinGroup != -1)
            {
                return new MaliciousLogEntryInfo(line, match, group, indexWithinGroup, searchString.Length, lineCounter, this);
            }
            else
            {
                return null;
            }
        }
    }
}
