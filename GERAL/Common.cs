using System;
using System.Web.Mail;

namespace LabMetro.GERAL
{
	/// <summary>
	/// Class to hold common methods.  These are static methods so you do not
	/// have to instantiate the "Common" class.  Currently has the following methods:
	///		
	///	SendMailAlert: sends an Email alert
	///		Parameter: String that is the text of the email to send
	///		returns: void
	///		
	///	VerifyName: Replaces the space with an underscore
	///		Parameter: string to do the replace on
	///		returns: string after replacement
	///		
	///	Author: Chris Harrison (HarrisonLogic.com)
	/// </summary>
	public class Common
	{
//		public static void SendMailAlert (string emailBodyText)
//		{
//			// For now all this method does is return without sending an email alert.
//			//		To use this function, simply remove the "return;" below and fill in
//			//		the appropriate settings below. - Chris Harrison
//			return;
//
//			// Instantiate a MailMessage object. This serves as a message object
//			// on which we can set properties.
//			MailMessage mailObj = new MailMessage();
//
//			// Set the from and to address on the email
//			mailObj.From = "you@yourDomain.com";
//			mailObj.To = "you@yourDomain.com";
//
//			mailObj.Subject = "Document Manager Alert";
//			mailObj.Body = emailBodyText;
//
//			// Optional: HTML format for the email
//			// mailObj.BodyFormat = MailFormat.Html;
//			System.Web.Mail.SmtpMail.SmtpServer = "mail.yourdomain.com";
//
//			// Optional: Set the priority of the message to high
//			mailObj.Priority = MailPriority.High;
//
//			// Send the email using the SmtpMail object
//			SmtpMail.Send(mailObj);
//		}

		// Method that Replaces spaces with Underscores
		public static string VerifyName(string FileName)
		{
			return FileName.Replace(" ", "_");
		}
	}
}
