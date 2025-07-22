using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration; 
using System.IO; 


namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_Estado.
	/// </summary>
	public partial class BO_Estado : System.Web.UI.Page
	{
		
		string pathFicheiro = (string)ConfigurationManager.AppSettings["ROOT_CERTIFICADOS_PATH"];
		
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Session["UserID"] == null) Response.Redirect("../Default.aspx",true); 
			lblMessage.Text = ""; 
			DirectoryInfo dirInfo = new DirectoryInfo(pathFicheiro); 
			//getDirsFiles(dirInfo); 
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			anularServico(txtRefServico.Text); 
		}

		private void anularServico(string refServico)
		{
		
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				string strSQL; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try
					{
                
						objCmd.CommandType = CommandType.Text; 
						
						strSQL ="SELECT idServico FROM SERVICO WHERE refServico = '"+refServico+"'"; 
						objCmd.CommandText = strSQL; 
						object idServico = objCmd.ExecuteScalar();

						if(Convert.IsDBNull(idServico)) //se serviço nao existe
						{
							objTrans.Rollback(); 
							lblMessage.Text ="A referência indicada não corresponde a nenhum serviço."; 
						}
						else
						{
							//==================================================================================
							//anular Serviço, remover do bse e da requisicao, alterar o estado do certificado	
							//==================================================================================
							strSQL = "UPDATE SERVICO SET idEstadoServico   = -1, idBSE = null, idRequisicao = null, idEstadoCertificado = 1, observacoes = isnull(observacoes,'') + ' Serviço anulado por '+ '" +System.Web.HttpContext.Current.User.Identity.Name.ToString()+"', idUtilAlteracao = "+  Session["UserId"].ToString() +" , dtAlteracao = CONVERT(DATETIME, GETDATE(), 102) WHERE refServico = '"+refServico+"'"; 
						
							GERAL.clsWriteError.WriteLog(strSQL);

							objCmd.CommandText = strSQL; 
							objCmd.ExecuteNonQuery(); 

							
							//==================================================================================
							//apagar da tabela certificados 
							//==================================================================================
							strSQL = "DELETE FROM Certificado WHERE idServico = "+ Convert.ToString(idServico); 
							objCmd.CommandText = strSQL; 
							objCmd.ExecuteNonQuery(); 

						
							//==================================================================================
							//apagar da tabela DocsCertificados no caso de ja ter sido adicionado
							//==================================================================================
							strSQL = "DELETE FROM DocsCertificados WHERE refServico = '"+refServico+"'"; 
							objCmd.CommandText = strSQL; 
							objCmd.ExecuteNonQuery(); 

						
							//==================================================================================
							//apagar o certificado dentro de qq uma das directorias dos certificados
							//origem, construcao, backup ou finais. 
							//==================================================================================
							DirectoryInfo dirInfo = new DirectoryInfo(pathFicheiro); 
								
							string nomeDoc = refServico.Replace("/","-"); 
							

							strSQL ="SELECT idFactura FROM SERVICO WHERE refServico = '"+refServico+"'"; 
							objCmd.CommandText = strSQL; 
							object idFactura = objCmd.ExecuteScalar();

							if(!Convert.IsDBNull(idFactura)) //se existe
							{
								lblMessage.Text +=("Atenção, muito importante. O serviço já foi facturado e deverá informar	a facturação da anulação do serviço por ser provável haver devolução de factura."); 

							}

							objTrans.Commit(); 
							lblMessage.Text +=	GERAL.clsGeral.ErrorMessage.MSG_DB;

                            //por motivos de performance, vou deixer de apagar certificados de serviços anulados.
                            //apagaCertificados(nomeDoc, dirInfo);  //isto passa aqui para o fim pq penso que era isto que gerava um timeout na aplicação.

						}
					}
	
					catch(Exception ex)
					{ 	
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); 
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text+= GERAL.clsGeral.ErrorMessage.ERR_DB; 
					}
				}
			}
		}


		//apaga recursivamente todos os certificados dentro de todas as directorias 
		private void apagaCertificados(string nomeFicheiro, DirectoryInfo dirInfo)
		{			
			//create an array of files using FileInfo object
			FileInfo [] files;
			//get all files for the current directory
			files = dirInfo.GetFiles(nomeFicheiro+"*");

			//iterate through the directory and print the files
			foreach (FileInfo file in files)
			{
					file.Delete(); 	
			}
        
			//get sub-folders for the current directory
			DirectoryInfo [] dirs = dirInfo.GetDirectories("*.*");
        
			//This is the code that calls 
			//the getDirsFiles (calls itself recursively)
			//This is also the stopping point 
			//(End Condition) for this recursion function 
			//as it loops through until 
			//reaches the child folder and then stops.
			foreach (DirectoryInfo dir in dirs)
			{
				apagaCertificados(nomeFicheiro, dir);
			}
		}


		//lista recursivamento todo o conteudo da directoria CERTIFICADOS! código bom e importante. 
		private void getDirsFiles(DirectoryInfo dirInfo)
		{
			
			//create an array of files using FileInfo object
			FileInfo [] files;
			//get all files for the current directory
			files = dirInfo.GetFiles("*.*");

			//iterate through the directory and print the files
			foreach (FileInfo file in files)
			{
				//get details of each file using file object
				String fileName = file.FullName;
				String fileSize = file.Length.ToString();
				String fileExtension =file.Extension;
				String fileCreated = file.LastWriteTime.ToString();

				Response.Write(fileName + " " + fileSize + 
					" " + fileExtension + " " + fileCreated+"<br />");
			}
        
			//get sub-folders for the current directory
			DirectoryInfo [] dirs = dirInfo.GetDirectories("*.*");
        
			//This is the code that calls 
			//the getDirsFiles (calls itself recursively)
			//This is also the stopping point 
			//(End Condition) for this recursion function 
			//as it loops through until 
			//reaches the child folder and then stops.
			foreach (DirectoryInfo dir in dirs)
			{
				Response.Write("--------->> {0} "+ dir.Name+"<br />");
				getDirsFiles(dir);
			}

		}

		private void txtRefServico_TextChanged(object sender, System.EventArgs e)
		{
		
		}
	
	}
}
