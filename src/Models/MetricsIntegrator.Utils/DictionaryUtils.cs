using System;
using System.Collections.Generic;
using System.Text;

namespace MetricsIntegrator.Utils
{
    /// <summary>
    ///     Responsible for grouping auxiliary methods of manipulating 
    ///     dictionaries.
    /// </summary>
    public class DictionaryUtils
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private DictionaryUtils()
        {
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public static string DictionaryToString(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentException("Dictionary cannot be null");

            StringBuilder dictionaryToString = new StringBuilder();

            foreach (KeyValuePair<string, string> keyValues in dictionary)
            {
                dictionaryToString.Append(keyValues.Key + " : " + keyValues.Value + ", ");
            }

            return dictionaryToString.ToString().TrimEnd(',', ' ');
        }
    }
}
