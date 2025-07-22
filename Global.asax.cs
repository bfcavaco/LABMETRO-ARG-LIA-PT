using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics; 
using System.Data.SqlClient; 
using System.Security.Principal; 
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Net.Mail;

namespace LabMetro 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		//private System.ComponentModel.IContainer components = null;
       

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)

		{
//			DateTime dt = DateTime.Now; 
//
//			 LabMetro.GERAL.clsWriteError.WriteLog("ApplicationStart: "+ dt.ToLongTimeString());
//
//			
//			GERAL.clsHandleFiles handle = new LabMetro.GERAL.clsHandleFiles(); 	
//
//			//automatismo para actualizar as empresas sap sem correr nenhum serviço. 
//			//pelo menos de manha, a aplicação arranca, e normalmente também depois da hora de almoço. 
//			try
//			{
//				handle.UpdateEmpresasSAP(); 				
//			}
//			catch(Exception ex)
//			{
//				LabMetro.GERAL.clsWriteError.WriteLog("Erro no handlefiles. Se o erro for - System.IndexOutOfRangeException: Cannot find table 0 - é pq não há ficheiro a alterar e não é erro."); 
//				LabMetro.GERAL.clsWriteError.WriteLog(ex.ToString()); 
//			}
//			handle = null; 

		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
            
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

//			When you call Server.Transfer, ASP.NET internally calls the Server.Execute method to transfer the control, and calls the Response.End method to end the processing of the current page. Response.End ends the page execution and calls the Thread.Abort method. The Thread.Abort method causes the ThreadAbortException error message to appear.
           
            //get reference to the source of the exception chain
            Exception ex = Server.GetLastError().GetBaseException();
			if(ex.GetType().ToString() == "System.Threading.ThreadAbortException")
			{
				LabMetro.GERAL.clsWriteError.WriteLog("threadabort"); 
			}
			else
			{
				LabMetro.GERAL.clsWriteError.WriteLog("User:"+User.Identity.Name.ToString()+"--"+ ex.ToString()); 
			}

            //MailMessage msg = new MailMessage();

            //msg.To.Add("AppsLog@isq.pt"); //passar para webconfig
            //msg.From = new MailAddress("dmelamed@isq.pt");//new MailAddress("isq@unceta.es"); tentei colocar o mesmo mail que a conta de outloo
            //string myAPP = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();
            //msg.Subject = "Erro na Aplicação: "+ myAPP;

            
            //    msg.Body = ex.ToString();


            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = (ConfigurationManager.AppSettings["IPSERVIDORMAIL"].ToString());

           
            //    try
            //    {
            //        smtp.Send(msg);
                  
            //    }
            //    catch (Exception exep)
            //    {
            //        GERAL.clsWriteError.WriteLog("Erro na função de envio do email parte 2: " + exep.ToString());
            //    }
          

            //msg.Dispose();

//            //Retrieve the last exception that occured 
//            ex = Server.GetLastError(); 
//            Server.ClearError(); 
//            string strError ="correu bem.";
//
//            //Check if the exception is a Crystal Reports 
//            //EngineException 
//            if (ex is CrystalDecisions.CrystalReports.Engine.EngineException) 
//            { 
//                //Cast the exception into an EngineException object 
//                EngineException exEngine = (EngineException)ex; 
//
//                //Check the type of error and handle accordingly 
//                switch (exEngine.ErrorID) 
//                { 
//                    case EngineExceptionErrorID.DataSourceError: 
//                        strError = "An error has occurred while connecting to the database."; 
//                                                           break; 
//                    case EngineExceptionErrorID.ExportingFailed: 
//                        strError = "An error occured while exporting the report."; 
//                                           break; 
//                    case 
//                    EngineExceptionErrorID.MissingParameterFieldCurrentValue: 
//                        strError = "At least one of the parameter  fields is missing a current value."; 
//                                                                  break; 
//                    case EngineExceptionErrorID.LogOnFailed: 
//                        strError = "Incorrect Logon Parameters. Check your user name and password."; 
//                                                            break; 
//                    case EngineExceptionErrorID.OutOfLicense: 
//                        strError = "There are no more licenses available. Contact your network administrator."; 
//                                                                              break; 
//                    default: 
//                        strError = "An undocumented error occured!"; 
//                        break; 
//                }
//            } 
//            else //Display the error message 
//            { 
//                GERAL.clsWriteError.WriteLog(strError); 
//            } 
            
		}

		protected void Session_End(Object sender, EventArgs e)
		{
            //Server.Transfer("Default.aspx"); 
		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}

       
	
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

        }
		#endregion
	}
}

