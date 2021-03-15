using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator
{
    public class TestPathMetrics
    {
        public string id;
        public string testPath;
        public int pathLength;
        public int hasLoop;
        public int countLoop;
        public int countnewReqNcCovered;
        public int countReqNcCovered;
        public double nodeCoverage;
        public int countnewReqPpcCovered;
        public int countReqPcCovered;
        public double primePathCoverage;
    }
}
