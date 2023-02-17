using LogAnalysisTool.Apache_Logs.Behaviour_Based_Detection_Tests;
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
            yield return _imgElementTest ??= new ImageTagDetectionTest();
            yield return _jsElementTest ??= new JavaScriptElementTest();
        }

        private static ScriptTagDetectionTest _scriptElementTest;
        private static ImageTagDetectionTest _imgElementTest;
        private static JavaScriptElementTest _jsElementTest;


    }
}
