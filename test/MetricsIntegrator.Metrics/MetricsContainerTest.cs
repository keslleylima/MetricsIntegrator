using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Metrics
{
    public class MetricsContainerTest
    {
        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestAddMetric()
        {
            string metricName = "m";
            string metricValue = "v";
            MetricsContainer metricsContainer = new MetricsContainer();
            
            metricsContainer.AddMetric(metricName, metricValue);

            Assert.Equal(metricValue, metricsContainer.GetMetric(metricName));
        }
    }
}
