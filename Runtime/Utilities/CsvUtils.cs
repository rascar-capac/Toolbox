using System.Collections.Generic;
using UnityEngine;

namespace Rascar.Toolbox.Utilities
{
    public static class CSVUtils
    {
        public static List<Dictionary<string, string>> FetchData(TextAsset file)
        {
            string[] lines = file.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            string[] headers = lines[0].Split(new string[] { "\t" }, System.StringSplitOptions.None);

            for (int index = 0; index < headers.Length; index++)
            {
                headers[index] = headers[index].Trim('\"');
            }

            List<Dictionary<string, string>> data = new();

            for (int index = 1; index < lines.Length; index++)
            {
                string[] properties = lines[index].Split(new string[] { "\t" }, System.StringSplitOptions.None);

                for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
                {
                    properties[propertyIndex] = properties[propertyIndex].Trim('\"');
                }

                if (properties.Length != headers.Length)
                {
                    continue;
                }

                Dictionary<string, string> element = new();

                for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
                {
                    element.Add(headers[propertyIndex], properties[propertyIndex]);
                }

                data.Add(element);
            }

            return data;
        }
    }
}
