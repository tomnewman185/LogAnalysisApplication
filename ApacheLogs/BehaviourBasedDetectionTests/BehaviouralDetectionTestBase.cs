using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    /// <summary>
    /// BehaviouralDetectionTestBase() - Base class for a behavioural detection test which has the properties contained
    /// from the behaviorual detection test interface
    /// </summary>
    internal abstract class BehaviouralDetectionTestBase : IBehaviouralDetectionTest
    {
        protected BehaviouralDetectionTestBase(HashSet<string> torExitNodeIPAddresses)
        {
            TorExitNodeIPAddresses = torExitNodeIPAddresses;
        }

        protected HashSet<string> TorExitNodeIPAddresses { get; set; }

        public abstract IEnumerable<MaliciousLogEntryInfo> ConductTests(Match match, string line, int lineCounter);

        public abstract string Description { get; }

        public abstract string Name { get; }

        // Flag to see whether an IP address is a Tor exit node
        protected bool IsTorExitNodeIPAddress(string ipAddress)
        {
            var flag = TorExitNodeIPAddresses.Contains(ipAddress);

            return flag;
        }
    }
}
