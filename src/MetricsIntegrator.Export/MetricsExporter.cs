using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetricsIntegrator.Export
{
    abstract class MetricsExporter
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        protected List<string> fields;
        private string delimiter;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        protected MetricsExporter(string templateFilepath)
        {
            ParseTemplate(templateFilepath);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void ParseTemplate(string templateFilepath)
        {
            string[] lines = File.ReadAllLines(templateFilepath);

            delimiter = ExtractDelimiterFrom(lines[0]);
            fields = ExtractFieldsFrom(lines);
        }

        private string ExtractDelimiterFrom(string line)
        {
            return line.Split('=')[1];
        }

        private List<string> ExtractFieldsFrom(string[] lines)
        {
            if (delimiter.Equals("\\n"))
                return ExtractFieldsUsingLineBreak(lines);
           
           return ExtractFieldsUsingDelimiter(lines);
        }

        private List<string> ExtractFieldsUsingLineBreak(string[] lines)
        {
            List<string> fields = new List<string>();

            for (int i = 1; i < lines.Length; i++)
            {
                fields.Add(lines[i]);
            }

            return fields;
        }

        private List<string> ExtractFieldsUsingDelimiter(string[] lines)
        {
            List<string> fields = new List<string>();

            foreach (string line in lines[1].Split(delimiter))
            {
                fields.Add(line);
            }

            return fields;
        }

        public abstract void Export();
    }
}
