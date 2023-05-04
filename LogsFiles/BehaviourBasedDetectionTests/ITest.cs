﻿using System;
using System.Linq;

namespace LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests
{
    /// <summary>
    /// ITest - Interface for a test
    /// </summary>
    public interface ITest
    {
        string Description { get; }

        string Name { get; }
    }
}