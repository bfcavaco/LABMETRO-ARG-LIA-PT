using System;
using System.IO;

namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for DocMgmtImages.
	/// </summary>
	public class DocMgmtImages
	{
		// Constants holding all images
		public const string HtmlDoc = "<IMG border='0' Src='/LabMetro/images/Html.gif'>";
		public const string TextFile = "<IMG border='0' Src='/LabMetro/images/Text.gif'>";
		public const string WordDoc = "<IMG border='0' Src='/LabMetro/images/Word.gif'>";
		public const string Folder = "<IMG border='0' Src='/LabMetro/images/Folder.gif'>";
		public const string OpenFolder = "<IMG border='0' Src='/LabMetro/images/OpenFolder.gif'>";
		public const string FullFolder = "<IMG border='0' Src='/LabMetro/images/FullFolder.gif'>";
		public const string ExcellDoc = "<IMG border='0' Src='/LabMetro/images/Excell.gif'>";
		public const string Image = "<IMG border='0' Src='/LabMetro/images/Pic.gif'>";
		public const string SoundFile = "<IMG border='0' Src='/LabMetro/images/Sound.gif'>";
		public const string Add = "<IMG border='0' Src='/LabMetro/images/Add.gif'>";
		public const string Delete = "<IMG border='0' Src='/LabMetro/images/Delete.gif'>";
		public const string Upload = "<IMG border='0' Src='/LabMetro/images/Upload.gif'>";
		public const string ZipFile = "<IMG border='0' Src='/LabMetro/images/zip.gif'>";

		public static string GetImageFromExt(string Extension)
		{
			// Accepts the File extension as a string and returns the
			//		appropriate image from the above constants
			switch (Extension.ToLower())
			{
				case ".doc":
					return DocMgmtImages.WordDoc;
				case ".txt":
					return DocMgmtImages.TextFile;
				case ".zip":
					return DocMgmtImages.ZipFile;
				case ".mp3":
				case ".wma":
				case ".wav":
					return DocMgmtImages.SoundFile;
				case ".xls":
					return DocMgmtImages.ExcellDoc;
				case ".gif":
				case ".jpg":
				case ".psd":
					return DocMgmtImages.Image;
				default:
					return DocMgmtImages.HtmlDoc;
			}
		}
	}
}
