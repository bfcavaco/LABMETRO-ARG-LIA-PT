

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.IO; 
using System.Configuration; 
using System.Threading;
using System.Xml; 
using System.Text;
using System.Data.SqlClient; 



namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for clsWriteError.
	/// </summary>
	public class clsWriteError
	{
		public clsWriteError()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //===========================================================================
        //recebe uma excepção e escreve para o ficheiro de log
        //===========================================================================
        public static void WriteLog(Exception ex)
        {
            //nome do ficheiro
            string s = DateTime.Now.ToString("dd-MM-yyyy");


            string strFileName= "ErrorLog_" + s + ".txt";
            
            //usando caminhos relativos
            string pathFicheiro = (string)ConfigurationManager.AppSettings["LOG_PATH_REL"];     
            string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName); 
            StreamWriter sw = new StreamWriter(path,true,System.Text.Encoding.GetEncoding(1252));

            // usando caminhos absolutos....
            // string myPath = (string)ConfigurationManager.AppSettings["LOG_PATH"];
            // StreamWriter sw = new StreamWriter(myPath + "/" + strFileName,true,System.Text.Encoding.GetEncoding(1252));
            
            sw.WriteLine(DateTime.Now.TimeOfDay + "- " +  ex.ToString() );
            sw.WriteLine("================="); 

            sw.Close(); 
        }

        
        //===========================================================================
        //recebe uma string de erro manual e escreve para o ficheiro de log
        //===========================================================================    
        public static void WriteLog(string strError)
        {

            //nome do ficheiro
            string s = DateTime.Now.ToString("dd-MM-yyyy");



            //            string strFileName= "ErrorLog_" + DateTime.Now.ToShortDateString() + ".txt";
            //            string myPath = (string)ConfigurationManager.AppSettings["LOG_PATH"];
            //            StreamWriter sw = new StreamWriter(myPath + "/" + strFileName,true,System.Text.Encoding.GetEncoding(1252));

            //nome do ficheiro
            string strFileName = "ErrorLog_" + s + ".txt";
            string pathFicheiro = (string)ConfigurationManager.AppSettings["LOG_PATH_REL"];     
            string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName); 
            StreamWriter sw = new StreamWriter(path,true,System.Text.Encoding.GetEncoding(1252));


            sw.WriteLine(DateTime.Now.TimeOfDay + "- " +  strError );
            sw.WriteLine("================="); 

           
            sw.Close(); 

        }

		//===========================================================================
		//SAP recebe uma excepção e escreve para o ficheiro de log
		//===========================================================================
		public static void WriteSapLog(Exception ex)
		{
            //nome do ficheiro
            string s = DateTime.Now.ToString("dd-MM-yyyy");

            //nome do ficheiro
            string strFileName = "ErrorLog_" + s + ".txt";
            
			//usando caminhos relativos
			string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_LIDOS_PATH_REL"];     
			string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName); 
			StreamWriter sw = new StreamWriter(path,true,System.Text.Encoding.GetEncoding(1252));
    
			sw.WriteLine(DateTime.Now.TimeOfDay + "- " +  ex.ToString() );
			sw.WriteLine("================="); 

			sw.Close(); 
		}

		//===========================================================================
		//SAP recebe uma string de erro manual e escreve para o ficheiro de log
		//===========================================================================    
		public static void WriteSapLog(string strError)
		{


            //nome do ficheiro
            string s = DateTime.Now.ToString("dd-MM-yyyy");

            //nome do ficheiro
            string strFileName = "ErrorLog_" + s+ ".txt";
			string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_LIDOS_PATH_REL"];     
			string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName); 
			StreamWriter sw = new StreamWriter(path,true,System.Text.Encoding.GetEncoding(1252));


			sw.WriteLine(DateTime.Now.TimeOfDay + "- " +  strError );
			sw.WriteLine("================="); 

           
			sw.Close(); 

		}

//        //nao usado======================================================
//        public static void WriteXMLLog(Exception ex) 
//        {
//
//            string strFileName= "ErrorLogXML_" + DateTime.Now.ToShortDateString() + ".XML";
//            
//            string myPath = (string)ConfigurationManager.AppSettings["LOG_PATH"];
//
//            XmlTextWriter writer = new XmlTextWriter((myPath + "/" + strFileName),Encoding.UTF8);
//            writer.WriteStartElement("ERROR");
//            writer.WriteStartElement("INFO");
//            writer.WriteString(ex.ToString()); 
//            writer.WriteEndElement();	
//            writer.WriteStartElement("DATETIME");
//            writer.WriteString(DateTime.Now.ToShortDateString()); 
//            writer.WriteEndElement();	
//            writer.WriteEndElement();	
//            writer.Close();  
//            
//        }

		public static void insertErrorTable(string tabela, string mensagem, string stringSQL, string obs)
		{
			
			string strSQL; 
			try
			{

				 strSQL = "INSERT INTO tblErros (tabela, mensagem, strSQL, obs, data) VALUES ('"+tabela+"','"+mensagem+"','','"+obs+"',getDate()) "; //isto tem de ser completado / melhorado.
				GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
			}
			catch(Exception excep)
			{
				
				GERAL.clsWriteError.WriteLog(excep); 
				
			}

		
		
		}
        
	}
}
