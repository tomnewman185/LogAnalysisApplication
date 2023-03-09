using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Apache_Logs.Behaviour_Based_Detection_Tests
{
    internal class InsecureDirectObjectReferenceDetectionTest : BehaviouralDetectionTestBase
    {
        public InsecureDirectObjectReferenceDetectionTest() 
        {
            Regex = new Regex(@"(\.|(%|%25)2E)(\.|(%|%25)2E)(\/|(%|%25)2F|\\|(%|%25)5C)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
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
        private Regex Regex { get; set; }
        public override string Description => $"Check For Path Traversal Attack";
        public override string Name => $"Test For Path Traversal Attack";
    }
}
