using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator
{
    public class SourceCodeMetrics
    {
        public int countInput;
        public int countLineCode;
        public int countLineCodeDecl;
        public int countLineCodeExe;
        public int countOutput;
        public int countPath;
        public int countPathLog;
        public int countStmt;
        public int countStmtDecl;
        public int countStmtExe;
        public int cyclomatic;
        public int cyclomaticModified;
        public int cyclomaticStrict;
        public int essential;
        public int knots;
        public int maxEssentialKnots;
        public int maxNesting;
        public int minEssentialKnots;
    }
}
