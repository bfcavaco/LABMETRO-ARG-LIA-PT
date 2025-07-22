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
using System.Configuration;
using System.Data.SqlClient;

namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_UtilizadorCalibrou.
	/// </summary>
	public partial class BO_UtilizadorCalibrou : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text = ""; 
			// Put user code to initialize the page here
			if(!Page.IsPostBack) 
			{
				bindDDTecnicos(); 
			
			}
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

		private void bindDDTecnicos()
		{
			string strSQL = "SELECT Funcionario.idFuncionario, Funcionario.nomeAbreviado FROM         Funcionario INNER JOIN Utilizador ON Funcionario.idUtilizador = Utilizador.idUtilizador WHERE     (Funcionario.activo = 1) AND (Utilizador.idPerfil IN (4, 5, 6)) ORDER BY Funcionario.nome";

            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddTecnicoLaboratorio.DataSource = dr;
			ddTecnicoLaboratorio.DataBind();
            dr.Close();

			ddTecnicoLaboratorio.Items.Insert(0,new ListItem("","")); 
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(ddTecnicoLaboratorio.SelectedValue!="" && txtRefServico.Text.Length > 7)
			{
				updateServico(txtRefServico.Text); 
			}
			else
			{
				lblMessage.Text="Por favor indique um técnico e um serviço."; 
			}
		}

		private void updateServico(string refServico)
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
							//alterar serviço
							//==================================================================================
							strSQL = "UPDATE SERVICO SET idFuncionarioEfectuouServico   = "+ddTecnicoLaboratorio.SelectedValue+", idUtilAlteracao = "+  Session["UserId"].ToString() +" , dtAlteracao = CONVERT(DATETIME, GETDATE(), 102) WHERE refServico = '"+refServico+"'"; 
						
							objCmd.CommandText = strSQL; 
							objCmd.ExecuteNonQuery(); 

							//==================================================================================
							//buscar idUtilizador
							//==================================================================================
							strSQL = "SELECT idUtilizador From funcionario where idFuncionario =" + ddTecnicoLaboratorio.SelectedValue; 
							objCmd.CommandText = strSQL; 
							object idUtilizarCalibrou = objCmd.ExecuteScalar();

							if(Convert.IsDBNull(idUtilizarCalibrou)) //se serviço nao existe
							{
								objTrans.Rollback(); 
								lblMessage.Text ="Não existe nenhum utilizador com este nome."; //... ma mensagem
							}
							else
							{
							
								//==================================================================================
								//alterar histórico
								//==================================================================================
								strSQL = "UPDATE SERVICOHISTORICOESTADOS SET idUtilCriacao = "+Convert.ToString(idUtilizarCalibrou)+"  WHERE idServico = "+ Convert.ToString(idServico) + " AND idEstadoServico IN (6,25)" ; 
								objCmd.CommandText = strSQL; 
								objCmd.ExecuteNonQuery(); 

								objTrans.Commit(); 
								lblMessage.Text +=	GERAL.clsGeral.ErrorMessage.MSG_DB; 
							}	
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
	}
}
