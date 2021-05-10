﻿using MetricsIntegrator.Utils;
using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Data
{
    /// <summary>
    ///     Responsible for storing metrics.
    /// </summary>
    public class Metrics
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly Dictionary<string, string> metrics;
        private readonly string identifier;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public Metrics(string identifier)
        {
            metrics = new Dictionary<string, string>();
            this.identifier = identifier;
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
        ///     If metrics is null or empty.
        /// </exception>
        public void AddMetric(string metric, string value)
        {
            if ((metric == null) || metric.Length == 0)
                throw new ArgumentException("Metric cannot be empty");

            if (value == null)
                value = "";

            metrics.Add(metric, value);
        }

        public override string ToString()
        {
            return $"MetricsContainer [ {DictionaryUtils.DictionaryToString(metrics)} ]";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            
            if (other == this)
                return true;

            return (other.GetHashCode() == GetHashCode());
        }


        //---------------------------------------------------------------------
        //		Getters
        //---------------------------------------------------------------------
        public string[] GetAllMetrics()
        {
            string[] metricKeys = new string[metrics.Count];

            metrics.Keys.CopyTo(metricKeys, 0);

            return metricKeys;
        }

        public string[] GetAllMetricValues()
        {
            string[] metricValues = new string[metrics.Count];

            metrics.Values.CopyTo(metricValues, 0);

            return metricValues;
        }

        public string GetMetric(string metric)
        {
            string value;

            metrics.TryGetValue(metric, out value);

            return (value == null) ? "" : value;
        }

        /// <summary>
        ///     Gets metrics identifier.
        /// </summary>
        /// 
        /// <returns>Value associated with the identifier</returns>
        public string GetID()
        {
            return GetMetric(identifier);
        }
    }
}
