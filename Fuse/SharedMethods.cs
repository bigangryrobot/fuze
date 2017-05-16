using System;
using System.Collections.Generic;
using System.IO;

namespace Fuze
{
	/// <summary>
	/// This class is purely because of DRY. Makes maintenance of generation methods that are shared easy. 
	/// </summary>
    public class SharedMethods
    {
		/// <summary>
		/// Ensures proper indention of methods in the yml. 
		/// </summary>
		/// <param name="indent">indent is the amount of indention. '1' gives 2 spaces, '2' gives 4 spaces and so on.</param>
		/// <param name="message">message is the string after the indention.</param>
		/// <returns></returns>
		public string Padding(int indent, string message)
		{
			string spaceIndent = "";
			for (int i = 0; i < indent; i++)
			{
				spaceIndent += "  ";
			}

			return string.Format("{0}{1}", spaceIndent, message);
		}

		/// <summary>
		/// Loops through file structure and writes out each line to a physical file. 
		/// </summary>
		/// <param name="fileStructure">List of strings, each list element represents a line in the file.</param>
		/// <param name="path">The path to save the file.</param>
		public void WriteOut(List<string> fileStructure, string path)
		{
			try {			
				Stream fileStream = File.Create(path);
				using (StreamWriter file = new StreamWriter(fileStream))
				{
					foreach (string line in fileStructure)
						file.WriteLine(line);
				}
			}
			catch  (System.IO.IOException e) {
				Console.WriteLine(string.Format("Error on path {0} and exception {1}", path,e.Message));
			}
		}

		public List<KeyValuePair<string, string>> ReadProperties(string[] properties)
		{
			List<KeyValuePair<string, string>> readProperties = new List<KeyValuePair<string, string>>();

			for (int i = 0; i < properties.Length; i++)
			{
				readProperties.Add(new KeyValuePair<string, string>(properties[i].Split(':')[0], properties[i].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[1])); 
				Console.WriteLine(readProperties[i].Key+" was given a value of "+readProperties[i].Value);
			}
			return readProperties; 
		}

	}
}
