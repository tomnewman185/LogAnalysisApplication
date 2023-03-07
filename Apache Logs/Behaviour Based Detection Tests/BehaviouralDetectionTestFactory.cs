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
        public static string[] GetHTMLKeywords()
        {
            string[] htmlKeywords =
            {
                "javascript",
                "vbscript",
                "expression",
                "applet",
                "meta",
                "xml",
                "blink",
                "link",
                "style",
                "script",
                "embed",
                "object",
                "iframe",
                "frame",
                "frameset",
                "ilayer",
                "layer",
                "bgsound",
                "title",
                "base",
                "img"
            };

            return htmlKeywords;
        }

        public static string[] GetSQLKeywords()
        {
            string[] sqlKeywords =
            {
                "or",
                "select",
                "union",
                "insert",
                "update",
                "delete",
                "replace",
                "truncate"
            };

            return sqlKeywords;
        }

        public static IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests()
        {
            foreach (string htmlKeyword in GetHTMLKeywords())
            {
                yield return new HTMLKeywordDetectionTest(htmlKeyword);
            }
        }


        //public static IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests()
        //{
            //yield return _scriptElementTest ??= new ScriptTagDetectionTest();
            //yield return _imgElementTest ??= new ImageTagDetectionTest();
            //yield return _jsElementTest ??= new JavaScriptElementTest();
            //yield return _HTMLElementTest ??= new HTMLTagDetectionTest();
        //}

        //private static ScriptTagDetectionTest _scriptElementTest;
        //private static ImageTagDetectionTest _imgElementTest;
        //private static JavaScriptElementTest _jsElementTest;
        //private static HTMLTagDetectionTest _HTMLElementTest;


    }
}
