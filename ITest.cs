using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool
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
