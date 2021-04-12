using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace AUA.AiS_FruiT
{
	public class Ini
	{
		[DllImport("kernel32.dll")]
		private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder returnValue, int size, String filePath);

		[DllImport("kernel32.dll")]
		private static extern long WritePrivateProfileString(String section, String key, String val, String filePath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        private string _pathFile;

		public Ini(string pathFile)
		{
			this._pathFile = pathFile;
		}

		public string GetValue(string section, string key)
		{
			StringBuilder returnValue = new StringBuilder(255);

			if (GetPrivateProfileString(section, key, "", returnValue, 255, _pathFile) > 0)
				return returnValue.ToString();
			else
				return "";
		}

		public void SetValue(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, _pathFile);
		}

        public List<string> GetKeys(string category)
        {

            byte[] buffer = new byte[2048];

            GetPrivateProfileSection(category, buffer, 2048, _pathFile);
            String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');

            List<string> result = new List<string>();

            foreach (String entry in tmp)
            {
                result.Add(entry.Substring(0, entry.IndexOf("=")));
            }

            return result;
        }
        public string FileDataDump()
        {
            string text = File.ReadAllText(_pathFile);
            return text;
        }

    }
}
