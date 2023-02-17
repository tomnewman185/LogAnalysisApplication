using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool.Behaviour_Based_Detection_Tests
{
    //Class to create the different types of behavioural detection tests
    internal class BehaviouralDetectionTestFactory
    {
        public static IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests()
        {
            yield return _scriptElementTest ??= new ScriptTagDetectionTest();
        }

        private static ScriptTagDetectionTest _scriptElementTest;

    }
}
