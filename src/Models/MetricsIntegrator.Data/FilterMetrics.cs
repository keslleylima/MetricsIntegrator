using System.Collections.Generic;

namespace MetricsIntegrator.Data
{
    /// <summary>
    ///     Responsible for storing metrics to be ignored when exporting.
    /// </summary>
    public class FilterMetrics
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public FilterMetrics()
        {
            SourceCodeMetricsFilter = new HashSet<string>();
            CodeCoverageFilter = new HashSet<string>();
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public ISet<string> SourceCodeMetricsFilter { get; private set; }
        public ISet<string> CodeCoverageFilter { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void AddSourceCodeFilter(string metric)
        {
            SourceCodeMetricsFilter.Add(metric);
        }

        public void AddCodeCoverageFilter(string metric)
        {
            CodeCoverageFilter.Add(metric);
        }

        public bool IsFilteredBySourceCodeMetric(string metricValue)
        {
            return SourceCodeMetricsFilter.Contains(metricValue);
        }

        public bool IsFilteredByCodeCoverage(string metricValue)
        {
            return CodeCoverageFilter.Contains(metricValue);
        }
    }
}
