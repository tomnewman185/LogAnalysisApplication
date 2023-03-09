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
        //List of HTML based keywords which need to be checked against each log entry 
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

        //List of SQL keywords that need to be checked against each log entry
        public static string[] GetSQLKeywords()
        {
            string[] sqlKeywords =
            {
                //"or",
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

        public static string[] GetJSEventHandlerKeywords()
        {
            string[] JSEventHandlerKeywords =
            {
                "onAbort",
                "onBlur",
                "onChange",
                "onClick",
                "onDblClick",
                "onDragDrop",
                "onError",
                "onFocus",
                "onKeyDown",
                "onKeyPress",
                "onKeyUp",
                "onLoad",
                "onMouseDown",
                "onMouseMove",
                "onMouseOut",
                "onMouseOver",
                "onMouseUp",
                "onMove",
                "onReset",
                "onResize",
                "onSelect",
                "onSubmit",
                "onUnload"
            };

            return JSEventHandlerKeywords;
        }

        public static IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests()
        {
            //HTML keyword test, running the test for each keyword contained within GetHTMLKeywords()
            foreach (string htmlKeyword in GetHTMLKeywords())
            {
                yield return new HTMLKeywordDetectionTest(htmlKeyword);
            }

            //SQL keyword test, running the test for each keyword contained within GetSQLKeywords()
            foreach (string sqlKeyword in GetSQLKeywords())
            {
                yield return new SQLInjectionKeywordDetectionTest(sqlKeyword);
            }

            //JS event handler keyword test, running the test for each keyword contained within GetJSEventHandlerKeywords()
            foreach (string jsEventHandlerKeyword in GetJSEventHandlerKeywords())
            {
                yield return new JSEventHandlerDetectionTest(jsEventHandlerKeyword);
            }

            yield return new InsecureDirectObjectReferenceDetectionTest();

            yield return new AdvancedInjectionDetectionTest();

            yield return new MaliciousFileExecutionDetectionTest();
        }

    }
}
