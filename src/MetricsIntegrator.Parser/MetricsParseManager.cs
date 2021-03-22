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
        }

        /// <summary>
        ///     Parses files contained in a metric file manager. Using this
        ///     constructor, delimiter will be ';'.
        /// </summary>
        /// 
        /// <param name="metricsFileManager">Metric files</param>
        /// 
        /// <exception cref="System.ArgumentException">
        ///     If metrics file manager is null or if delimiter is null or empty.
        /// </exception>
        public MetricsParseManager(MetricsFileManager metricsFileManager) 
            : this(metricsFileManager, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public Dictionary<string, List<string>> Mapping { get; private set; }
        public Dictionary<string, Metrics> SourceCodeMetrics { get; private set; }
        public Dictionary<string, Metrics> TestCodeMetrics { get; private set; }
        public List<Metrics> TestPathMetrics { get; private set; }
        public List<Metrics> TestCaseMetrics { get; private set; }


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
            TestPathMetricsParser tpParser = new TestPathMetricsParser(
                metricsFileManager.TestPathsPath, 
                delimiter
            );

            TestPathMetrics = tpParser.Parse();
        }

        private void DoTestCaseMetricsParsing()
        {
            TestCaseMetricsParser tcParser = new TestCaseMetricsParser(
                metricsFileManager.TestCasePath, 
                delimiter
            );

            TestCaseMetrics = tcParser.Parse();
        }

        private void DoSourceCodeMetricsParsing(Dictionary<string, List<string>> mapping)
        {
            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(
                metricsFileManager.SourceCodePath, 
                mapping, 
                delimiter
            );
            
            scmParser.Parse();

            SourceCodeMetrics = scmParser.DictSourceCode;
            TestCodeMetrics = scmParser.DictSourceTest;
        }
    }
}
