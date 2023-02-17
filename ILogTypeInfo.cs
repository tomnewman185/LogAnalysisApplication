﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    //Interface for a log type
    internal interface ILogTypeInfo
    {
        string Name { get; }
        string RegularExpression { get; }

        IEnumerable<IBehaviouralDetectionTest>GetBehaviouralDetectionTests();
    }
}
