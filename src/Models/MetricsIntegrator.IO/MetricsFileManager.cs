namespace MetricsIntegrator.IO
{
    /// <summary>
    ///     Responsible for finding metrics files.
    /// </summary>
    public class MetricsFileManager
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsFileManager()
        {
            SourceCodePath = string.Empty;
            MapPath = string.Empty;
            TestPathsPath = string.Empty;
            TestCasePath = string.Empty;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string SourceCodePath { get; set; }
        public string MapPath { get; set; }
        public string TestPathsPath { get; set; }
        public string TestCasePath { get; set; }
    }
}
