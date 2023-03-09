using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Apache_Logs.Behaviour_Based_Detection_Tests
{
    internal class AdvancedInjectionDetectionTest : BehaviouralDetectionTestBase
    {
        public AdvancedInjectionDetectionTest() 
        {
            Regex = new Regex(@"(%3D|=)[^\n]*((%27|')|(--)|(%3B|;))", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public override IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineNumber)
        {
            var group = match.Groups[ApacheLogComponents.RegexComponentGroups.Request];
            var request = group.Value;

            foreach (Match m in Regex.Matches(request))
            {
                if (m.Success)
                {
                    yield return new MaliciousLogEntryInfo(line, match, group, m.Index, m.Length, lineNumber, this);
                }
            }
        }

        public override string Description => "Advanced Injection Test (=) (') (;)";
        public override string Name => "Advanced Injection Test";

        private Regex Regex { get; set; }


    }
}
