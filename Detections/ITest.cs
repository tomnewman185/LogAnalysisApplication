using System;
using System.Linq;

namespace LogAnalysisTool.Detections
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
