using MetricsIntegratorTest;
using System;
using System.IO;
using Xunit;

namespace MetricsIntegrator.IO
{
    public class MetricsFileManagerTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private MetricsFileManager metricsFileManager;
        private readonly string scPath;
        private readonly string mapPath;
        private readonly string tpPath;
        private readonly string tcPath;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsFileManagerTest()
        {
            metricsFileManager = new MetricsFileManager();
            scPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "SC_test.csv";
            mapPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "MAP_test.csv";
            tpPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "TP_test.csv";
            tcPath = PathManager.GetResourcesPath() + Path.DirectorySeparatorChar + "TC_test.csv";
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestSetFilesFromCLI()
        {
            string[] args =
            {
                "-sc", scPath,
                "-map", mapPath,
                "-tp", tpPath,
                "-tc", tcPath
            };

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
                metricsFileManager.SetFilesFromCLI(null);
            });
        }

        [Fact]
        public void TestSetFilesFromCLIWithEmptyArgs()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metricsFileManager.SetFilesFromCLI(new string[] { });
            });
        }

        [Fact]
        public void TestFindAllFromDirectory()
        {
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
                metricsFileManager.FindAllFromDirectory(null);
            });
        }

        [Fact]
        public void TestFindAllFromEmptyDirectory()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                metricsFileManager.FindAllFromDirectory("");
            });
        }
    }
}
