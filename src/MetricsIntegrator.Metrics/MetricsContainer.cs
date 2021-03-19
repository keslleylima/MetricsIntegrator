﻿using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Metrics
{
    /// <summary>
    ///     Responsible for storing metrics.
    /// </summary>
    public class MetricsContainer
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly Dictionary<string, string> metrics;


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
        /// <summary>
        ///     Stores a metric with its value.
        /// </summary>
        /// 
        /// <param name="metric">Metric name</param>
        /// <param name="value">Metric value</param>
        /// 
        /// <exception cref="System.ArgumentException">
        ///     If metrics or value is null or empty.
        /// </exception>
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

        public string GetMetric(string metric)
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

        /// <summary>
        ///     Gets metrics identifier.
        /// </summary>
        /// 
        /// <returns>Value associated with the first metric found</returns>
        public string GetID()
        {
            var iterator = metrics.GetEnumerator();
            iterator.MoveNext();

            return iterator.Current.Value;
        }
    }
}
