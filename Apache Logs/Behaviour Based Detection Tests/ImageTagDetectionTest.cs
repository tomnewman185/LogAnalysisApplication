using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Apache_Logs.Behaviour_Based_Detection_Tests
{
    internal class ImageTagDetectionTest : BehaviouralDetectionTestBase
    {
        public override string Description => "Check For Image Tag Element";
        public override string Name => "Image Element Test";
        public override MaliciousLogEntryInfo ConductTest(Match match, string line, int lineCounter)
        {
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];

            var request = group.Value;

            var searchString = "<img";

            int indexWithinGroup = request.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase);
            if (indexWithinGroup != -1)
            {
                return new MaliciousLogEntryInfo(line, match, group, indexWithinGroup, lineCounter, searchString.Length, this);
            }
            else
            {
                return null;
            }
        }
    }
}
