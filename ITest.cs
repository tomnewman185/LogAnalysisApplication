using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    public interface ITest
    {
        string Description { get; }
        string Name { get; }
    }
}
