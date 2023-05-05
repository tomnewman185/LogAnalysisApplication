using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool.Detections.BehaviourBasedDetectionTests
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

        // Flag to see whether an IP address is a Tor exit node
        protected bool IsTorExitNodeIPAddress(string ipAddress)
        {
            var flag = TorExitNodeIPAddresses.Contains(ipAddress);

            return flag;
        }

        protected HashSet<string> TorExitNodeIPAddresses { get; set; }

        #region IBehaviouralDetectionTest Implementation
        public abstract IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineCounter);
        #endregion

        #region ITest Implementation
        public abstract string Description { get; }

        public abstract string Name { get; }
        #endregion
    }
}
