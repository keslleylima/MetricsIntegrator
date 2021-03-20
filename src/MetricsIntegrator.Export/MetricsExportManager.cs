using MetricsIntegrator.Data;
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
        private Dictionary<string, Metrics> dictSourceCode;
        private Dictionary<string, Metrics> dictSourceTest;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsExportManager(string outputDir, string projectName, 
                                    string delimiter, 
                                    Dictionary<string, List<string>> mapping,
                                    Dictionary<string, Metrics> dictSourceCode,
                                    Dictionary<string, Metrics> dictSourceTest)
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
        public void ExportTestPathMetrics(List<Metrics> tpMetrics)
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
        
        public void ExportTestCaseMetrics(List<Metrics> tcMetrics)
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
