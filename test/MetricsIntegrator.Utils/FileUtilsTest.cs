using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsIntegrator.Utils
{
    public class FileUtilsTest
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private string workingDirectory;
        private List<string> createdFiles;
        private string[] searchedFiles;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public FileUtilsTest()
        {
            workingDirectory = null;
            createdFiles = new List<string>();
            searchedFiles = null;
        }


        //---------------------------------------------------------------------
        //		Tests
        //---------------------------------------------------------------------
        [Fact]
        public void TestGetAllFilesFromDirectoryEndingWith()
        {
            WithWorkingDirectory(Path.GetTempPath());
            CreateFile("test-file.txt");
            CreateFile("test-file2.txt");

            SearchAllFilesFromDirectoryEndingWith("txt");

            AssertObtainedFilesContainsCreatedFiles();
        }

        [Fact]
        public void TestGetAllFilesFromNullDirectoryEndingWith()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileUtils.GetAllFilesFromDirectoryEndingWith(null, "some-extension");
            });
        }

        [Fact]
        public void TestGetAllFilesFromDirectoryEndingWithNullExtension()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileUtils.GetAllFilesFromDirectoryEndingWith(".", null);
            });
        }

        [Fact]
        public void TestGetAllFilesFromDirectoryEndingWithEmptyExtension()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileUtils.GetAllFilesFromDirectoryEndingWith(".", "");
            });
        }

        [Fact]
        public void TestGetAllFilesFromDirectoryEndingWithUsingFilePath()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileUtils.GetAllFilesFromDirectoryEndingWith("./FileUtilsTest.cs", ".cs");
            });
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void WithWorkingDirectory(string path)
        {
            workingDirectory = path;
        }

        private void CreateFile(string filename)
        {
            string filepath = workingDirectory + filename;

            File.Create(filepath);
            createdFiles.Add(filepath);
        }

        private void SearchAllFilesFromDirectoryEndingWith(string extension)
        {
            searchedFiles = FileUtils.GetAllFilesFromDirectoryEndingWith(
                workingDirectory,
                extension
            );
        }

        private void AssertObtainedFilesContainsCreatedFiles()
        {
            List<string> obtainedFiles = new List<string>(searchedFiles);

            foreach (string expectedFile in createdFiles)
            {
                Assert.Contains(expectedFile, obtainedFiles);
            }
        }
    }
}
