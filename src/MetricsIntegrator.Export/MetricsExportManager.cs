using MetricsIntegrator.Metrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Export
{
    public class MetricsExportManager
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private readonly string outputDir;
        private readonly string projectName;
        private readonly string delimiter;
        private Dictionary<string, List<string>> mapping;
        private Dictionary<string, MetricsContainer> dictSourceCode;
        private Dictionary<string, MetricsContainer> dictSourceTest;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsExportManager(string outputDir, string projectName, 
                                    string delimiter, 
                                    Dictionary<string, List<string>> mapping,
                                    Dictionary<string, MetricsContainer> dictSourceCode,
                                    Dictionary<string, MetricsContainer> dictSourceTest)
        {
            this.outputDir = outputDir;
            this.projectName = projectName;
            this.delimiter = delimiter;
            this.mapping = mapping;
            this.dictSourceCode = dictSourceCode;
            this.dictSourceTest = dictSourceTest;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void ExportTestPathMetrics(List<MetricsContainer> tpMetrics)
        {
            string TestPathFilePath = outputDir + @"\TP_dataset_resulting_" + projectName + ".csv";
            MetricsCSVExporter tpCSVExporter = new MetricsCSVExporter(
                TestPathFilePath,
                mapping,
                dictSourceCode,
                dictSourceTest,
                tpMetrics,
                delimiter
            );

            tpCSVExporter.Export();
        }
        
        public void ExportTestCaseMetrics(List<MetricsContainer> tcMetrics)
        {
            string TestCaseFilePath = outputDir + @"\TC_dataset_resulting_" + projectName + ".csv";
            MetricsCSVExporter tcCSVExporter = new MetricsCSVExporter(
                TestCaseFilePath,
                mapping,
                dictSourceCode,
                dictSourceTest,
                tcMetrics,
                delimiter
            );

            tcCSVExporter.Export();
        }
    }
}
