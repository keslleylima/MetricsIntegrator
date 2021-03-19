using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Metrics
{
    public class MetricsContainerTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string metricName;
        private readonly string metricValue;
        private MetricsContainer metricsContainer;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsContainerTest()
        {
            metricName = "m";
            metricValue = "v";
            metricsContainer = new MetricsContainer();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestAddMetric()
        {
            metricsContainer.AddMetric(metricName, metricValue);

            Assert.Equal(metricValue, metricsContainer.GetMetric(metricName));
        }

        [Fact]
        public void TestAddMetricWithNullName()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metricsContainer.AddMetric(null, metricValue);
            });
        }

        [Fact]
        public void TestAddMetricWithEmptyName()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metricsContainer.AddMetric("", metricValue);
            });
        }

        [Fact]
        public void TestAddMetricWithNullValue()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metricsContainer.AddMetric(metricName, null);
            });
        }

        [Fact]
        public void TestAddMetricWithEmptyValue()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metricsContainer.AddMetric(metricName, "");
            });
        }

        [Fact]
        public void TestGetAllMetricValues()
        {
            metricsContainer.AddMetric(metricName, metricValue);
            metricsContainer.AddMetric(metricName + "2", metricValue + "2");

            Assert.Equal(
                new string[] { metricValue, metricValue + "2" }, 
                metricsContainer.GetAllMetricValues()
            );
        }

        [Fact]
        public void TestGetAllMetrics()
        {
            metricsContainer.AddMetric(metricName, metricValue);
            metricsContainer.AddMetric(metricName + "2", metricValue + "2");

            Assert.Equal(
                new string[] { metricName, metricName + "2" },
                metricsContainer.GetAllMetrics()
            );
        }
    }
}
