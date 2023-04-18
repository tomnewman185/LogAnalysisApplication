using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    // Behavioural Detection Test Factory Class
    internal class BehaviouralDetectionTestFactory
    {
        // HTML based keywords which need to be checked for signatures
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

        // SQL based keywords that need to be checked for signatures
        public static string[] GetSQLKeywords()
        {
            string[] sqlKeywords = { "select", "union", "insert", "update", "delete", "replace", "truncate" };

            return sqlKeywords;
        }

        // JavaScript based keywords that need to be checked for signatures
        public static string[] GetJSEventHandlerKeywords()
        {
            string[] jsEventHandlerKeywords =
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

            return jsEventHandlerKeywords;
        }

        // Runs all behavioural-based detection tests
        public static IEnumerable<IBehaviouralDetectionTest> RunBehaviouralDetectionTests(HashSet<string> torExitNodeIPAddresses)
        {

            // HTML keyword test, running the test for each keyword contained within GetHTMLKeywords()
            foreach (string htmlKeyword in GetHTMLKeywords())
            {
                yield return new HTMLKeywordDetectionTest(torExitNodeIPAddresses, htmlKeyword);
            }

            // SQL keyword test, running the test for each keyword contained within GetSQLKeywords()
            foreach (string sqlKeyword in GetSQLKeywords())
            {
                yield return new SQLInjectionKeywordDetectionTest(torExitNodeIPAddresses,sqlKeyword);
            }

            // JS event handler keyword test, running the test for each keyword contained within GetJSEventHandlerKeywords()
            foreach (string jsEventHandlerKeyword in GetJSEventHandlerKeywords())
            {
                yield return new JSEventHandlerDetectionTest(torExitNodeIPAddresses,jsEventHandlerKeyword);
            }

            // Insecure Direct Object Reference Test
            yield return new InsecureDirectObjectReferenceDetectionTest(torExitNodeIPAddresses);

            // Advanced Injection Test
            yield return new AdvancedInjectionDetectionTest(torExitNodeIPAddresses);

            // Malicious File Execution Test
            yield return new MaliciousFileExecutionDetectionTest(torExitNodeIPAddresses);
        }
    }
}
