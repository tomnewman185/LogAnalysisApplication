using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    //Interface for a detection test
    internal interface IDetectionTest
    {
        string Name { get; }

        string Description { get; }
    }
}
