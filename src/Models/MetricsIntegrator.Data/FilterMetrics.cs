using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsIntegrator.Data
{
    public class FilterMetrics
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private ISet<string> testCaseMetricsFilter = new HashSet<string>();
        private ISet<string> sourceCodeMetricsFilter = new HashSet<string>();


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public FilterMetrics()
        {
            testCaseMetricsFilter = new HashSet<string>();
            sourceCodeMetricsFilter = new HashSet<string>();
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void AddSourceCodeFilter(string metric)
        {
            sourceCodeMetricsFilter.Add(metric);
        }

        public void AddTestCaseFilter(string metric)
        {
            testCaseMetricsFilter.Add(metric);
        }

        public bool IsFilteredByBaseMetric(string metricValue)
        {
            return testCaseMetricsFilter.Contains(metricValue);
        }

        public bool IsFilteredBySourceCodeMetric(string metricValue)
        {
            return sourceCodeMetricsFilter.Contains(metricValue);
        }
    }
}
