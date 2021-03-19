using MetricsIntegratorTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace MetricsIntegrator.IO
{
    public class MetricsFileManagerTest
    {
        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestSetFilesFromCLI()
        {
            string scPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "SC_test.csv";
            string mapPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "MAP_test.csv";
            string tpPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "TP_test.csv";
            string tcPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "TC_test.csv";

            string[] args =
            {
                "-sc",
                scPath,
                "-map",
                mapPath,
                "-tp",
                tpPath,
                "-tc",
                tcPath
            };

            MetricsFileManager metricsFileManager = new MetricsFileManager();
            metricsFileManager.SetFilesFromCLI(args);

            Assert.Equal(scPath, metricsFileManager.SourceCodePath);
            Assert.Equal(mapPath, metricsFileManager.MapPath);
            Assert.Equal(tpPath, metricsFileManager.TestPathsPath);
            Assert.Equal(tcPath, metricsFileManager.TestCasePath);
        }

        [Fact]
        public void TestSetFilesFromCLIWithNullArgs()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                MetricsFileManager metricsFileManager = new MetricsFileManager();
                metricsFileManager.SetFilesFromCLI(null);
            });
        }

        [Fact]
        public void TestSetFilesFromCLIWithEmptyArgs()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                MetricsFileManager metricsFileManager = new MetricsFileManager();
                metricsFileManager.SetFilesFromCLI(new string[] { });
            });
        }

        [Fact]
        public void TestFindAllFromDirectory()
        {
            string scPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "SC_test.csv";
            string mapPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "MAP_test.csv";
            string tpPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "TP_test.csv";
            string tcPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "TC_test.csv";

            MetricsFileManager metricsFileManager = new MetricsFileManager();
            metricsFileManager.FindAllFromDirectory(PathManager.GetResourcesPath());

            Assert.Equal(scPath, metricsFileManager.SourceCodePath);
            Assert.Equal(mapPath, metricsFileManager.MapPath);
            Assert.Equal(tpPath, metricsFileManager.TestPathsPath);
            Assert.Equal(tcPath, metricsFileManager.TestCasePath);
        }

        [Fact]
        public void TestFindAllFromNullDirectory()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                MetricsFileManager metricsFileManager = new MetricsFileManager();
                metricsFileManager.FindAllFromDirectory(null);
            });
        }

        [Fact]
        public void TestFindAllFromEmptyDirectory()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                MetricsFileManager metricsFileManager = new MetricsFileManager();
                metricsFileManager.FindAllFromDirectory("");
            });
        }
    }
}
