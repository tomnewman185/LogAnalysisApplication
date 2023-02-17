using LogAnalysisTool.Behaviour_Based_Detection_Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.Apache_Logs
{
    internal class ApacheLogComponents : ILogTypeInfo
    {
        public ApacheLogComponents()
        {

        }

        //Interface properties for log type Apache
        public string Name => "Apache Log Type Components";

        //Regex breaks up each log entry into its different components
        public string RegularExpression => $@"^""(?<{Regex.Escape(RegexComponentGroups.RemoteHost)}>\S*)\s+(?<{Regex.Escape(RegexComponentGroups.RFC931)}>\S+)\s+(?<{Regex.Escape(RegexComponentGroups.AuthUser)}>\S+).*\[(?<{Regex.Escape(RegexComponentGroups.Date)}>.*)\]\s""(?<{Regex.Escape(RegexComponentGroups.Request)}>[^""]*)""\s(?<{Regex.Escape(RegexComponentGroups.Status)}>\S*)\s(?<{Regex.Escape(RegexComponentGroups.Bytes)}>\S*)\s""(?<{Regex.Escape(RegexComponentGroups.Referer)}>[^""]*)""\s""(?<{Regex.Escape(RegexComponentGroups.UserAgent)}>[^""]*)""""$";

        public IEnumerable<IBehaviouralDetectionTest> GetBehaviouralDetectionTests() => BehaviouralDetectionTestFactory.GetBehaviouralDetectionTests();


        //Defines the different component groups that are present within each log entry
        internal class RegexComponentGroups
        {
            public static string RemoteHost => "1";
            public static string RFC931 => "2";

            public static string AuthUser => "3";

            public static string Date => "4";

            public static string Request => "5";

            public static string Status => "6";

            public static string Bytes => "7";

            public static string Referer => "8";

            public static string UserAgent => "9";
        }
    }
}
