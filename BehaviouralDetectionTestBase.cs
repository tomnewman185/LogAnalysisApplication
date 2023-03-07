using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    //Base class for a behaviorual detection test which has the properties contained from the behaviorual detection test interface
    internal abstract class BehaviouralDetectionTestBase : IBehaviouralDetectionTest
    {
        public abstract IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineCounter);
        public abstract string Description { get; }
        public abstract string Name { get; }

    }
}
