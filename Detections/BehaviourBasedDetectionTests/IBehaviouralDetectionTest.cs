﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool.Detections.BehaviourBasedDetectionTests
{
    /// <summary>
    /// IBehaviouralDetectionTest - Interface for a behavioural-based detection test, which inherits from the test
    /// interface
    /// </summary>
    internal interface IBehaviouralDetectionTest : ITest
    {
        IEnumerable<MaliciousLogEntryInfo> ConductTest(Match match, string line, int lineCounter);
    }
}
