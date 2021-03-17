﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator
{
    public class TestCaseMetrics
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        Dictionary<string, string> metrics;
        /*
        public string id;
        public double avgPathLength;
        public int hasLoop;
        public double avgCountLoop;
        public int countReqEcCovered;
        public double edgeCoverage;
        public int countReqPcCovered;
        public double primePathCoverage;
        */

        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public TestCaseMetrics()
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
        public string GetValueFromMetric(string metric)
        {
            string value;
            
            metrics.TryGetValue(metric, out value);

            return (value == null) ? "" : value;
        }

        public string[] GetMetrics()
        {
            string[] metricKeys = new string[metrics.Count];

            metrics.Keys.CopyTo(metricKeys, 0);

            return metricKeys;
        }
    }
}
