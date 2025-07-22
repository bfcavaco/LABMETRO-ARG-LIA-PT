using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; 
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LabMetro.REPORTS;
using LabMetro.GERAL;
using System.Configuration;


namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_servico.
	/// </summary>
	public partial class BO_servico : System.Web.UI.Page
	{
	
		

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

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text =""; 
			
			btnTrocar.Attributes.Add("onClick","javascript:if(confirm('Confirma troca?')== false) return false;");

			if(!Page.IsPostBack)
			{
				ViewState["sortField"] = "refServico";
				ViewState["sortDirection"] = "ASC";
				
			}
		}

	

		private void BindDDEquipamentos()
		{
		
			string idServico = DGServicos.DataKeys[DGServicos.SelectedIndex].ToString(); 
			//string refServico = DGServicos.Items[DGServicos.SelectedIndex].Cells[4].Text.ToString(); 

			//string idGrandeza = refServico.Substring(1,3); 
			
//
//			if(idGrandeza == "THU") idGrandeza = "'TEM', 'HUM'"; 
//			else if(idGrandeza == "PRF") idGrandeza = "'PRE','FOR'"; 
//
//			Response.Write(idGrandeza); 


			//eu quero é TROCAR DENTRO DA MESMA GRANDEZA DO EQUIPAMENTO. UM EQUIPAMENTO THU PASSOU POR EXEMPLO A TEM, E EU QUERO 
			//TROCAR DENTRO DA GRANDEZA DO EQUIPAMENTO!!!

//		string strSQL ="SELECT Equipamento.idEquipamento, CAST(Equipamento.idEquipamento AS varchar) + ' - ' + CAST(TipoEquipamento.descricao AS varchar) + ' NID :' + CAST(ISNULL(Equipamento.numIdentificacao,'') AS varchar) + ' -  NS:' + CAST(ISNULL(Equipamento.numSerie,'') AS varchar) AS equipamento FROM equipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento inner join familia on tipoEquipamento.idFamilia = familia.idFamilia inner join grandeza on familia.idGrandeza = grandeza.idGrandeza and grandeza.idgrandeza IN ("+idGrandeza+") WHERE idEmpresa = (select idEmpresa from equipamento where idEquipamento = (select idEquipamento from servico where idServico ="+idServico+")) order by TipoEquipamento.descricao, Equipamento.numIdentificacao, Equipamento.numSerie"; 
			//Response.Write(strSQL); 


			string idEquipamento = DGServicos.Items[DGServicos.SelectedIndex].Cells[3].Text.ToString(); 
			string strSQL = "SELECT Equipamento.idEquipamento, CAST(Equipamento.idEquipamento AS varchar) + ' - ' + CAST(TipoEquipamento.descricao AS varchar) + ' NID :' + CAST(ISNULL(Equipamento.numIdentificacao,'') AS varchar) + ' -  NS:' + CAST(ISNULL(Equipamento.numSerie,'') AS varchar) AS equipamento FROM equipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento inner join familia on tipoEquipamento.idFamilia = familia.idFamilia and familia.idGrandeza = dbo.udfGetIdGrandezaByIdEquipamento("+idEquipamento+") where idEmpresa = (select idEmpresa from equipamento where idEquipamento = (select idEquipamento from servico where idServico ="+idServico+")) order by TipoEquipamento.descricao, Equipamento.numIdentificacao, Equipamento.numSerie"; 
			
			//Response.Write(strSQL); 
			
			SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			ddEquipamentoAManter.DataTextField = "equipamento";
			ddEquipamentoAManter.DataValueField="idEquipamento"; 
			ddEquipamentoAManter.DataSource = dr; 
			ddEquipamentoAManter.DataBind(); 		
			ddEquipamentoAManter.Items.Insert(0,new ListItem("",""));
            dr.Close();
			//equip = null;
		}
		
		private void BindGrid()
		{  
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
			try
			{
           
				//se quero receber todos, tenho de meter o ZERO como param
				DataTable DT = servico.DTGetServicos(txtEmpresa.Text, txtRefBRE.Text, txtRefServico.Text,"","","","","","","","","",""); 
				DataView DV = new DataView(DT); 
				 
	        
				DGServicos.DataSource = DV; 
				DGServicos.DataBind(); 
				
				
				if(DV.Table.Rows.Count > 0)
				{
					DGServicos.Visible=true;
				}
				else
				{
					DGServicos.Dispose();
					DGServicos.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
					
				}
			}
			catch(Exception ex)
			{

				DGServicos.Dispose();
				DGServicos.Visible=false; 
				lblMessage.Text= ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
			}
			
			servico  = null; 
		}
		

		


		protected void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			DGServicos.CurrentPageIndex=0; 
			BindGrid(); 
			ddEquipamentoAManter.DataSource= null;
			ddEquipamentoAManter.DataBind(); 
			
		}
		
		public void doPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DGServicos.CurrentPageIndex = e.NewPageIndex; 
			BindGrid(); 
		}

		public void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirection"].ToString())
			{
				case "ASC":
					ViewState["sortDirection"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirection"]="ASC";
					break;
			}
			ViewState["sortField"] = e.SortExpression;
			BindGrid(); 
		}


		
		
		protected void btnTrocar_Click(object sender, System.EventArgs e)
		{
			
			
			if(ddEquipamentoAManter.SelectedValue =="" || DGServicos.SelectedIndex < 0)
			{
				lblMessage.Text ="Tem de seleccionar um equipamento primeiro."; 
				return;
			}

			
			if(trocarEquipamentos(ddEquipamentoAManter.SelectedValue,DGServicos.DataKeys[DGServicos.SelectedIndex].ToString())) 
			{
				BindGrid();  	
				
			}
		}


		private bool trocarEquipamentos(string idEquipamentoManter, string idservico)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 
				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try


					{
						objCmd.CommandText = "UPDATE SERVICO SET idEquipamento = "+idEquipamentoManter+" WHERE idServico = "+idservico; 
						GERAL.clsWriteError.WriteLog(User.Identity.Name.ToString() + " --" + objCmd.CommandText.ToString()); 
						objCmd.ExecuteNonQuery();
	
						objTrans.Commit();

                        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_TROCA_EFECTUADA; 
						return true;


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
							lblMessage.Text +=excep.Message.ToString()+"<br />";
							return false;
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text +=ex.Message.ToString()+"<br />";
						return false;
					}
				}
			}
		}

		protected void DGServicos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindDDEquipamentos(); 
		}
	}
}
