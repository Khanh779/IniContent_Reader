using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IniContent_Lib
{
    public class IniContentReader
    {
        private Dictionary<string, Dictionary<string, string>> data;

        

        string filePath;

        public IniContentReader()
        {
            data = new Dictionary<string, Dictionary<string, string>>();
          
        }


        /// <summary>
        /// Get the content of the file
        /// </summary>
        /// <param name="content"></param>

        public void LoadContent(string content)
        {
            string currentSection = null;
            bool hasSection = false;
            data = new Dictionary<string, Dictionary<string, string>>();
            foreach (var line in content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";"))
                    continue;

                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    if (!data.ContainsKey(currentSection))
                        data[currentSection] = new Dictionary<string, string>();
                }
                else if (trimmedLine.Contains("="))
                {
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    if (currentSection != null)
                        data[currentSection][key] = value;
                }
            }
        }

        public string[] GetSections()
        {
            return data.Keys.ToArray();
        }

        public string[] GetKeys(string section)
        {
            if (data.TryGetValue(section, out var sectionData))
                return sectionData.Keys.ToArray();

            return new string[0];
        }

        public bool CheckSection(string section)
        {
            return data.ContainsKey(section);
        }

        public bool CheckKey(string section, string key)
        {
            return data.ContainsKey(section) && data[section].ContainsKey(key);
        }

        public string GetValue(string section, string key)
        {
            if (data.TryGetValue(section, out var sectionData) && sectionData.TryGetValue(key, out var value))
                return value;

            return null;
        }

        public void AddOrUpdateValue(string section, string key, string value)
        {
            if (!data.ContainsKey(section))
            {
                data[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            data[section][key] = value;
        }

        public void Save()
        {
            Save(filePath);
        }

        public void Save(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var section in data)
                {
                    writer.WriteLine($"[{section.Key}]");
                    foreach (var kvp in section.Value)
                    {
                        writer.WriteLine($"{kvp.Key}={kvp.Value}");
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
