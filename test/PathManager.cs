using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegratorTest
{
    public class PathManager
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private static string projectPath;
        private static string resourcesPath;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private PathManager()
        {
        }


        //---------------------------------------------------------------------
        //		Getters
        //---------------------------------------------------------------------
        public static string GetProjectPath()
        {
            if (projectPath == null)
                InitializeProjectPath();

            return projectPath;
        }

        private static void InitializeProjectPath()
        {
            projectPath = Directory
                            .GetParent(Directory.GetCurrentDirectory())
                            .Parent.Parent.Parent
                            .ToString();
        }

        public static string GetResourcesPath()
        {
            if (resourcesPath == null)
                InitializeResourcesPath();

            return resourcesPath;
        }

        private static void InitializeResourcesPath()
        {
            resourcesPath = GetProjectPath() 
                + Path.DirectorySeparatorChar 
                + "test" 
                + Path.DirectorySeparatorChar 
                + "resources";
        }
    }
}
