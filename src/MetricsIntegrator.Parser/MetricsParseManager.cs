using MetricsIntegrator.IO;
using MetricsIntegrator.Metrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Parser
{
    class MetricsParseManager
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string delimiter;
        private readonly MetricsFileManager metricsFileManager;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsParseManager(MetricsFileManager metricsFileManager, string delimiter)
        {
            this.metricsFileManager = metricsFileManager;
            this.delimiter = delimiter;
        }

        public MetricsParseManager(MetricsFileManager metricsFileManager) 
            : this(metricsFileManager, ";")
        {
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public Dictionary<string, MetricsContainer> DictSourceCode { get; private set; }
        public Dictionary<string, MetricsContainer> DictSourceTest { get; private set; }
        public Dictionary<string, List<string>> Mapping { get; private set; }
        public List<MetricsContainer> ListTestPath { get; private set; }
        public List<MetricsContainer> ListTestCase { get; private set; }


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

            ListTestPath = tpParser.Parse();
        }

        private void DoTestCaseMetricsParsing()
        {
            TestCaseMetricsParser tcParser = new TestCaseMetricsParser(
                metricsFileManager.TestCasePath, 
                delimiter
            );

            ListTestCase = tcParser.Parse();
        }

        private void DoSourceCodeMetricsParsing(Dictionary<string, List<string>> mapping)
        {
            SourceCodeMetricsParser scmParser = new SourceCodeMetricsParser(
                metricsFileManager.SourceCodePath, 
                mapping, 
                delimiter
            );
            
            scmParser.Parse();

            DictSourceCode = scmParser.DictSourceCode;
            DictSourceTest = scmParser.DictSourceTest;
        }
    }
}
