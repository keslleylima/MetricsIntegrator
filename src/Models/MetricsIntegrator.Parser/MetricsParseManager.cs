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
    ///         <item>Code coverage</item>
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
            CodeCoverage = new Dictionary<string, List<Metrics>>();
            CodeCoverageFieldKeys = new List<string>();
            SourceCodeFieldKeys = new List<string>();
            SourceCodeIdentifierKey = default!;
            CodeCoverageIdentifierKey = default!;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public IDictionary<string, List<string>> Mapping { get; private set; }
        public IDictionary<string, Metrics> SourceCodeMetrics { get; private set; }
        public IDictionary<string, Metrics> TestCodeMetrics { get; private set; }
        public IDictionary<string, List<Metrics>> CodeCoverage { get; private set; }
        public List<string> CodeCoverageFieldKeys { get; private set; }
        public List<string> SourceCodeFieldKeys { get; private set; }
        public string SourceCodeIdentifierKey { get; private set; }
        public string CodeCoverageIdentifierKey { get; private set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Parse()
        {
            DoMappingParser();
            DoCodeCoverageParsing();
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

        private void DoCodeCoverageParsing()
        {
            CodeCoverageMetricsParser tpParser = new CodeCoverageMetricsParser(
                metricsFileManager.CodeCoveragePath
            );

            CodeCoverage = tpParser.Parse();
            CodeCoverageFieldKeys = tpParser.FieldKeys;
            CodeCoverageIdentifierKey = tpParser.CodeCoverageIdentifierKey;
        }

        private void DoSourceCodeMetricsParsing(IDictionary<string, List<string>> mapping)
        {
            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(
                metricsFileManager.SourceCodePath, 
                mapping
            );
            
            scmParser.Parse();

            SourceCodeMetrics = scmParser.SourceCodeMetrics;
            TestCodeMetrics = scmParser.SourceTestMetrics;
            SourceCodeFieldKeys = scmParser.FieldKeys;
            SourceCodeIdentifierKey = scmParser.SourceCodeIdentifierKey;
        }
    }
}
