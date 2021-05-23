namespace MetricsIntegrator.IO
{
    /// <summary>
    ///     Responsible for storing metrics files.
    /// </summary>
    public class MetricsFileManager
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MetricsFileManager()
        {
            SourceCodePath = string.Empty;
            CodeCoveragePath = string.Empty;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public string SourceCodePath { get; set; }
        public string CodeCoveragePath { get; set; }
    }
}
