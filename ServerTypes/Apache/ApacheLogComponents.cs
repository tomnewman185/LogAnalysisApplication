using System;
using System.Collections.Generic;
using System.Linq;
using LogAnalysisTool.Detections.BehaviourBasedDetectionTests;
using LogAnalysisTool.Detections.BehaviourBasedDetectionTests.Apache.Tests;

namespace LogAnalysisTool.ServerTypes.Apache
{
    // Apache Log Components - defining the different component groups of an Apache access log file
    internal class ApacheLogComponents : ILogTypeInfo
    {
        public ApacheLogComponents()
        {
        }

        #region ILogTypeInfo Implementation
        //Regular expression statement to break up each log entry into its components
        public string LogComponentsRegularExpression => @"^
            \s*                                                       # Match any whitespace chars
            (?<StartQuote>""?)                                        # Match 0 or 1 quote as group <StartQuote>
            (?<1>\S+)                                                 # Match 1 or more non-whitespace chars as group <1t>
            \s+                                                       # Match 1 or more whitespace chars
            (?<2>\S+)                                                 # Match 1 or more non-whitespace chars as group <2>
            \s+                                                       # Match 1 or more whitespace chars
            (?<3>\S+)                                                 # Match 1 or more non-whitespace chars as group <3>
            \s+                                                       # Match 1 or more whitespace chars
            \[                                                        # Match [
            (?<4>.+)                                                  # Match any char (except newline) 1 or more times as group <4>
            \]                                                        # Match ]
            \s                                                        # Match 1 whitespace char
            ""                                                        # Match quote
            (?<5>.+?)                                                 # Match any char 1 or more times
            (?=""\s\S+\s\S+\s""[^""]+""\s""[^""]+""\k<StartQuote>)    # Zero width positive lookahead assertion of rest of the main pattern
            ""                                                        # Match quote
            \s                                                        # Match 1 whitespace char
            (?<6>\S+)                                                 # Match 1 or more non-whitespace chars as group <6>
            \s                                                        # Match 1 whitespace char
            (?<7>\S+)                                                 # Match 1 or more non-whitespace chars as group <7>
            \s                                                        # Match 1 whitespace char
            ""                                                        # Match quote
            (?<8>[^""]+)                                              # Match any char not a quote 1 or more times as group <8>
            ""                                                        # Match quote
            \s                                                        # Match 1 whitespace char
            ""                                                        # Match quote
            (?<9>[^""]+)                                              # Match any char not a quote 1 or more times as group <9>
            ""                                                        # Match quote
            \k<StartQuote>                                            # matches the same text as most recently matched by the capturing group named <StartQuote>
            $";

        // Name of log type
        public string Name => "Apache Log Type";

        public IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests(HashSet<string> torExitNodeIPAddresses)
        {
            return BehaviouralDetectionTestFactory.GetBehaviouralDetectionTests(torExitNodeIPAddresses);
        }
        #endregion

        // Defines the different component groups that are present within each log entry of an Apache access log file
        internal class RegexComponentGroups
        {
            public static string AuthUser => "3";

            public static string Bytes => "7";

            public static string Date => "4";

            public static string Referer => "8";

            public static string RemoteHost => "1";

            public static string Request => "5";

            public static string RFC931 => "2";

            public static string Status => "6";

            public static string UserAgent => "9";
        }
    }
}
