using MetricsIntegrator.IO;
using MetricsIntegrator.Data;
using System;
using System.Collections.Generic;

namespace MetricsIntegrator.Parser
{
    /// <summary>
    ///     Responsible for parsing metrics files of the following files:
    ///     
    ///     <list type="bullet">
    ///         <item>Source code metrics</item>
    ///         <item>Mapping of tested methods along with the test method that tests it</item>
    ///         <item>Test path metrics</item>
    ///         <item>Test case metrics</item>
    ///     </list>
    /// </summary>
    public class MetricsParseManager
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string delimiter;
        private readonly MetricsFileManager metricsFileManager;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        /// <summary>
        ///     Parses files contained in a metric file manager.
        /// </summary>
        /// 
        /// <param name="metricsFileManager">Metric files</param>
        /// <param name="delimiter">Symbol used to separate data</param>
        /// <param name="filterMetrics">Metrics to be avoided</param>
        /// 
        /// <exception cref="System.ArgumentException">
        ///     If metrics file manager is null or if delimiter is null or empty.
        /// </exception>
        public MetricsParseManager(MetricsFileManager metricsFileManager, string delimiter)
        {
            if (metricsFileManager == null)
                throw new ArgumentException("Metrics file manager cannot be null");

            if ((delimiter == null) || delimiter.Length == 0)
                throw new ArgumentException("Delimiter cannot be null");
            
            this.metricsFileManager = metricsFileManager;
            this.delimiter = delimiter;

            Mapping = new Dictionary<string, List<string>>();
            SourceCodeMetrics = new Dictionary<string, Metrics>();
            TestCodeMetrics = new Dictionary<string, Metrics>();
            TestPathMetrics = new Dictionary<string, List<Metrics>>();
            TestCaseMetrics = new Dictionary<string, List<Metrics>>();
            TestCaseFieldKeys = new List<string>();
            SourceCodeFieldKeys = new List<string>();
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public Dictionary<string, List<string>> Mapping { get; private set; }
        public Dictionary<string, Metrics> SourceCodeMetrics { get; private set; }
        public Dictionary<string, Metrics> TestCodeMetrics { get; private set; }
        public IDictionary<string, List<Metrics>> TestPathMetrics { get; private set; }
        public IDictionary<string, List<Metrics>> TestCaseMetrics { get; private set; }
        public List<string> TestCaseFieldKeys { get; private set; }
        public List<string> SourceCodeFieldKeys { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Parse()
        {
            DoMappingParser();
            DoTestPathMetricsParsing();
            DoTestCaseMetricsParsing();
            DoSourceCodeMetricsParsing(Mapping);
        }

        private void DoMappingParser()
        {
            MappingMetricsParser mapParser = new MappingMetricsParser(
                metricsFileManager.MapPath, 
                delimiter
            );

            Mapping = mapParser.Parse();
        }

        private void DoTestPathMetricsParsing()
        {
            BaseMetricsParser tpParser = new BaseMetricsParser(
                metricsFileManager.TestPathsPath, 
                delimiter
            );

            TestPathMetrics = tpParser.Parse();
        }

        private void DoTestCaseMetricsParsing()
        {
            BaseMetricsParser tcParser = new BaseMetricsParser(
                metricsFileManager.TestCasePath, 
                delimiter
            );

            TestCaseMetrics = tcParser.Parse();
            TestCaseFieldKeys = tcParser.FieldKeys;
        }

        private void DoSourceCodeMetricsParsing(Dictionary<string, List<string>> mapping)
        {
            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(
                metricsFileManager.SourceCodePath, 
                mapping, 
                delimiter
            );
            
            scmParser.Parse();

            SourceCodeMetrics = scmParser.SourceCodeMetrics;
            TestCodeMetrics = scmParser.SourceTestMetrics;
            SourceCodeFieldKeys = scmParser.FieldKeys;
        }
    }
}
