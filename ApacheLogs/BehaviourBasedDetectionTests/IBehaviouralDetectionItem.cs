﻿using System;
using System.Linq;

namespace LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests
{
    /// <summary>
    /// IBehaviouralDetectionItem - Implemented on types that represent a detected threat that is behavioural based.
    /// </summary>
    public interface IBehaviouralDetectionItem : IDetectionItem
    {
    }
}
