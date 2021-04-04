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
        //		Constructor
        //---------------------------------------------------------------------
        public FilterMetrics()
        {
            SourceCodeMetricsFilter = new HashSet<string>();
            TestPathMetricsFilter = new HashSet<string>();
            TestCaseMetricsFilter = new HashSet<string>();
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public ISet<string> SourceCodeMetricsFilter { get; private set; }
        public ISet<string> TestPathMetricsFilter { get; private set; }
        public ISet<string> TestCaseMetricsFilter { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void AddSourceCodeFilter(string metric)
        {
            SourceCodeMetricsFilter.Add(metric);
        }

        public void AddTestPathFilter(string metric)
        {
            TestPathMetricsFilter.Add(metric);
        }

        public void AddTestCaseFilter(string metric)
        {
            TestCaseMetricsFilter.Add(metric);
        }

        public bool IsFilteredBySourceCodeMetric(string metricValue)
        {
            return SourceCodeMetricsFilter.Contains(metricValue);
        }

        public bool IsFilteredByTestPathMetric(string metricValue)
        {
            return TestPathMetricsFilter.Contains(metricValue);
        }

        public bool IsFilteredByTestCaseMetric(string metricValue)
        {
            return TestCaseMetricsFilter.Contains(metricValue);
        }
    }
}
