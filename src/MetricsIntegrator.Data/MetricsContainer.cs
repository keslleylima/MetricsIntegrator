using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Data
{
    public class MetricsContainer
    {
        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public Dictionary<string, List<string>> Mapping { get; set; }
        public Dictionary<string, Metrics> SourceCodeMetrics { get; set; }
        public Dictionary<string, Metrics> TestCodeMetrics { get; set; }
        public List<Metrics> TestPathMetrics { get; set; }
        public List<Metrics> TestCaseMetrics { get; set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public bool HasTestCaseMetrics()
        {
            return (TestCaseMetrics != null);
        }

        public bool HasTestPathMetrics()
        {
            return (TestPathMetrics != null);
        }
    }
}
