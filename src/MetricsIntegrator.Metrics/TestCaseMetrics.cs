using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator
{
    public class TestCaseMetrics
    {
        public string id;
        public double avgPathLength;
        public int hasLoop;
        public double avgCountLoop;
        public int countReqEcCovered;
        public double edgeCoverage;
        public int countReqPcCovered;
        public double primePathCoverage;
    }
}
