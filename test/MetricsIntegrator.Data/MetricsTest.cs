using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MetricsIntegrator.Data
{
    public class MetricsTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string metricName;
        private readonly string metricValue;
        private Metrics metrics;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsTest()
        {
            metricName = "m";
            metricValue = "v";
            metrics = new Metrics();
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestAddMetric()
        {
            metrics.AddMetric(metricName, metricValue);

            Assert.Equal(metricValue, metrics.GetMetric(metricName));
        }

        [Fact]
        public void TestAddMetricWithNullName()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metrics.AddMetric(null, metricValue);
            });
        }

        [Fact]
        public void TestAddMetricWithEmptyName()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metrics.AddMetric("", metricValue);
            });
        }

        [Fact]
        public void TestAddMetricWithNullValue()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metrics.AddMetric(metricName, null);
            });
        }

        [Fact]
        public void TestAddMetricWithEmptyValue()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metrics.AddMetric(metricName, "");
            });
        }

        [Fact]
        public void TestGetAllMetricValues()
        {
            metrics.AddMetric(metricName, metricValue);
            metrics.AddMetric(metricName + "2", metricValue + "2");

            Assert.Equal(
                new string[] { metricValue, metricValue + "2" }, 
                metrics.GetAllMetricValues()
            );
        }

        [Fact]
        public void TestGetAllMetrics()
        {
            metrics.AddMetric(metricName, metricValue);
            metrics.AddMetric(metricName + "2", metricValue + "2");

            Assert.Equal(
                new string[] { metricName, metricName + "2" },
                metrics.GetAllMetrics()
            );
        }
    }
}
