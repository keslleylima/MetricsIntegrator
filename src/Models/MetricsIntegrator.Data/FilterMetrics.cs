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
        private ISet<string> sourceCodeMetricsFilter = new HashSet<string>();
        private ISet<string> testPathMetricsFilter = new HashSet<string>();
        private ISet<string> testCaseMetricsFilter = new HashSet<string>();


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public FilterMetrics()
        {
            sourceCodeMetricsFilter = new HashSet<string>();
            testPathMetricsFilter = new HashSet<string>();
            testCaseMetricsFilter = new HashSet<string>();
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void AddSourceCodeFilter(string metric)
        {
            sourceCodeMetricsFilter.Add(metric);
        }

        public void AddTestPathFilter(string metric)
        {
            testPathMetricsFilter.Add(metric);
        }

        public void AddTestCaseFilter(string metric)
        {
            testCaseMetricsFilter.Add(metric);
        }

        public bool IsFilteredBySourceCodeMetric(string metricValue)
        {
            return sourceCodeMetricsFilter.Contains(metricValue);
        }

        public bool IsFilteredByTestPathMetric(string metricValue)
        {
            return testPathMetricsFilter.Contains(metricValue);
        }

        public bool IsFilteredByTestCaseMetric(string metricValue)
        {
            return testCaseMetricsFilter.Contains(metricValue);
        }
    }
}
