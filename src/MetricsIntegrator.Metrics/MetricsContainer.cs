using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Metric
{
    public class MetricsContainer
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private Dictionary<string, string> metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsContainer()
        {
            metrics = new Dictionary<string, string>();
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void AddMetric(string metric, string value)
        {
            metrics.Add(metric, value);
        }


        //---------------------------------------------------------------------
        //		Getters
        //---------------------------------------------------------------------
        public string[] GetMetrics()
        {
            string[] metricKeys = new string[metrics.Count];

            metrics.Keys.CopyTo(metricKeys, 0);

            return metricKeys;
        }

        public string GetValueFromMetric(string metric)
        {
            string value;

            metrics.TryGetValue(metric, out value);

            return (value == null) ? "" : value;
        }

        public string[] GetAllMetricValues()
        {
            string[] metricValues = new string[metrics.Count];

            metrics.Values.CopyTo(metricValues, 0);

            return metricValues;
        }

        public string GetID()
        {
            var iterator = metrics.GetEnumerator();
            iterator.MoveNext();

            return iterator.Current.Value;
        }
    }
}
