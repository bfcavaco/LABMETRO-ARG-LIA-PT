using System;
using System.Collections.Specialized;
using System.IO;

namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for DocMgmt.
	/// </summary>

	public class DocMgmt
	{
		// Private Internal Members
		string docRoot = "";

		public DocMgmt(string DocRoot)
		{
			// Set the Document Root to the private member
			docRoot = DocRoot;

			// We are going to a check in the Constructor of the object to verify the
			// validity of the Document Root, rather than each public method
			DirectoryInfo DocRootDir = new DirectoryInfo(docRoot);
			if (!DocRootDir.Exists)
			{
				// Throw an exception that the Root folder was not Found
				throw new System.IO.DirectoryNotFoundException("The Document Root Directory passed to the object, '" + docRoot + "', does not appear to be valid.");
			}
		}

		public StringDictionary GetDirectoryNames()
		{
			// Chose a String Dictionary to hold the actual folder name
			// as well as a Presentable folder name.  You could modify to hold
			// more useful information, such as size, date created.. etc
			StringDictionary Directories = new StringDictionary();

			// List the Directories
			DirectoryInfo DocRoot = new DirectoryInfo(docRoot);

			// Add each folders in Document Root to a StringDictionary
			foreach(DirectoryInfo Folder in DocRoot.GetDirectories())
			{
				// Do a check to exclude Front Page Extension Folders
				if (!Folder.Name.StartsWith("_"))
				{
					Directories.Add(Folder.FullName, GetPracticalName(Folder.Name));
					// returnStr.Append(Library.Common.WriteSpaces(5) + "<img src='" + ApplicationRoot + "common/images/navigate_folder.gif'>" + Library.Common.UnderscoreToSpace(nextFolder.Name) + "<br />");
				}
			}
			return Directories;
		}

		public StringDictionary GetFileNames()
		{
			// Chose a String Dictionary to hold the actual file name
			// as well as a Presentable folder name.  You could modify to hold
			// more useful information, such as size, date created.. etc
			StringDictionary Files = new StringDictionary();

			// List the Directories
			DirectoryInfo DocRoot = new DirectoryInfo(docRoot);

			// Add each folders in Document Root to a StringDictionary
			foreach(FileInfo File in DocRoot.GetFiles())
			{
				// Do a check to exclude Front Page Extension Folders
				Files.Add(File.FullName, GetPracticalName(File.Name));				
			}
			return Files;
		}

		private string GetPracticalName(string FileOrFolderName)
		{
			// Returns a string with a presentable file name
			string returnString = FileOrFolderName.Trim();

			// Modify these to the application and your preference
			// Change Underscore to space
			returnString = returnString.Replace("_", " ");

			return returnString.Trim();
		}
	}
}
