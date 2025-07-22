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
using System.Text.RegularExpressions; 

namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_Equipamentos.
	/// </summary>
	public partial class BO_Equipamentos : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid DQEquipamentos;
	




		#region Web Form Designer generated code


		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			InitializeComponent2();
			base.OnInit(e);
		}
	

		private void InitializeComponent()
		{

		}

		private void InitializeComponent2()
		{    
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
			
			btnPesquisaEmpresa.Click += new System.EventHandler(btnPesquisaEmpresa_Click);				
			Linkbutton1.Click += new System.EventHandler(Linkbutton1_Click);
			Linkbutton2.Click += new System.EventHandler(Linkbutton2_Click);
			Linkbutton3.Click += new System.EventHandler(Linkbutton3_Click);
			DGEquipamentos.SelectedIndexChanged += new System.EventHandler(DGEquipamentos_SelectedIndexChanged);
			Linkbutton4.Click += new System.EventHandler(Linkbutton4_Click);
			DGEquipamentos.ItemDataBound += new DataGridItemEventHandler(DGEquipamentos_ItemDataBound); 
			
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
			rbCriteriosPesquisa.SelectedIndexChanged += new System.EventHandler(rbCriteriosPesquisa_SelectedIndexChanged);
			btnTrocar.Click += new System.EventHandler(btnTrocar_Click);
		
		}

		#endregion
		

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text =""; 
			lblMessage2.Text =""; 

			btnTrocar.Attributes.Add("onClick","javascript:if(confirm('Confirma troca de equipamentos?')== false) return false;");

			if(!Page.IsPostBack)
			{
				ViewState["sortField"] = "tipoEquipamento";
				ViewState["sortDirection"] = "ASC";
				
			}
		}


		//*********************************************************************************
		//*********************************************************************************
		//********** ACÇÕES SOBRE O DATAGRID EQUIPAMENTOS *********************************
		//*********************************************************************************
		//*********************************************************************************


		
		//============================================================================
		//BIND DO DATAGRID (EQUIPAMENTOS)
		//============================================================================
		private void BindGrid()
		{  
			if(ddEmpresa.SelectedValue=="") return; 

			DATA.EquipamentoBD equip = new LabMetro.DATA.EquipamentoBD();
			try
			{
				
				string filter = "0"; 
				if (cbFilterResultados.Checked == true) filter = "1"; 

				DataTable DT = equip.DTEquipamentoBO(ddEmpresa.SelectedValue,txtTipoEquipamento.Text,txtNumSerie.Text,txtNumIdentificacao.Text,rbCriteriosPesquisa.SelectedValue,filter, txtGrandeza.Text, txtFamilia.Text,txtRefUltEntrada.Text, txtIdEquipamento.Text ); 


				DataView DV = new DataView(DT); //,"",strSort,DataViewRowState.CurrentRows); 
				DV.Sort= ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"];  
				
	        
				DGEquipamentos.DataSource = DV; 
				DGEquipamentos.DataBind(); 
				
				
				if(DV.Table.Rows.Count > 0)
				{
					DGEquipamentos.Visible=true;
				}
				else
				{
					DGEquipamentos.Dispose();
					DGEquipamentos.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
					
				}
			}
			catch(Exception ex)
			{

				DGEquipamentos.Dispose();
				DGEquipamentos.Visible=false; 
				lblMessage.Text= ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
			}
			finally
			{

				equip = null; 
			}
		}

		//============================================================================
		//BOTAO QUE PESQUISA POR EQUIPAMENTOS E FAZ BIND DO GRID DE EQUIPAMENTOS
		//============================================================================
		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			DGEquipamentos.CurrentPageIndex=0; 
			BindGrid(); 
			//BindDDEquipamentos(); 
		}
		
		//============================================================================
		//PAGINAÇÃO DO DATAGRID EQUIPAMENTOS
		//============================================================================
		public void doPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DGEquipamentos.CurrentPageIndex = e.NewPageIndex; 
			BindGrid(); 
		}

		//============================================================================
		//ORDENAÇÃO DO DATAGRID EQUIPAMENTOS
		//============================================================================
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

		//============================================================================	
		//REBIND DO GRID EQUIPAMENTOS, LIMPA O RESTO.
		//============================================================================	
		private void DGEquipamentos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			dgGenerico.DataSource=null; 
			dgGenerico.DataBind(); 
			
			dgCertificados.Visible=false; 
			dgGenerico.Visible=true; 

			BindDDEquipamentos(); 
		}

		//============================================================================	
		//EDITA OS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void editEquipamento(Object sender, DataGridCommandEventArgs e)     
		{
			DGEquipamentos.ShowFooter=false;     
			DGEquipamentos.EditItemIndex = e.Item.ItemIndex;	
			BindGrid();
		}

		//============================================================================	
		//CANCELA A EDIÇÃO DOS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void cancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DGEquipamentos.ShowFooter=true;  
			DGEquipamentos.EditItemIndex = -1;
			BindGrid();
		}

		//============================================================================	
		//ALTERA OS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void updateEquipamento(Object sender, DataGridCommandEventArgs e)
		{
			string idEquipamento = DGEquipamentos.DataKeys[e.Item.ItemIndex].ToString();
			//Response.Write("vai alterar equipamento com o id " + idEquipamento); 

			DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamento");
			
			TextBox  numSerie = (TextBox) e.Item.FindControl("txtNS");
			TextBox  numIdentificacao = (TextBox) e.Item.FindControl("txtNID");
			TextBox  periodCalib = (TextBox) e.Item.FindControl("txtPC");
			TextBox  refUltCalib = (TextBox) e.Item.FindControl("txtRUC");
			TextBox  dtUltCalib = (TextBox) e.Item.FindControl("txtDUC");
			
			if(Session["UserID"] == null) Response.Redirect("../Default.aspx",true); 
			DATA.EquipamentoBD eq = new LabMetro.DATA.EquipamentoBD(); 

			int i = eq.updateEquipamentoInBO(ddTipoEquipamento.SelectedValue.ToString(), numSerie.Text,numIdentificacao.Text, periodCalib.Text,refUltCalib.Text,dtUltCalib.Text,Session["UserId"].ToString(),idEquipamento); //pode devolver mais que 1 por causa do trigger na bd. 
			if(i>0)	lblMessage2.Text = "Equipamento alterado";
			else  lblMessage2.Text = "Erro na actualização.";
			eq = null; 
			DGEquipamentos.EditItemIndex = -1;
			DGEquipamentos.ShowFooter=true;   
			BindGrid(); 
            
			
		}


		//====================================================================================
		//ACÇÃO DE APAGAR EQUIPAMENTO
		//====================================================================================
		protected void deleteEquipamento(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
		{
			if(Session["UserID"] == null) Response.Redirect("../Default.aspx",true); 
			string id = DGEquipamentos.DataKeys[e.Item.ItemIndex].ToString();
			//Response.Write("vai apagar equipamento com o id " + id); 

			int nServicos = System.Convert.ToInt16(e.Item.Cells[15].Text); 
			if(nServicos ==0)
			{
				//Response.Write("vai apagar equipamento com o id " + id); 
				apagaEquipamento(id); 
			}
			else 
			{
				Response.Write("não vai apagar equipamento com o id " + id + " porque tem serviços associados"); 
			}
			
			BindGrid(); 

		}

		//====================================================================================
		//ITEM DATABOUND DO DATAGRID EQUIPAMENTOS
		//====================================================================================
		
		private void DGEquipamentos_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;
				

				TextBox  numSerie = (TextBox) e.Item.FindControl("txtNS");
				numSerie.Text = DRV["numSerie"].ToString(); 

				TextBox  numIdentificacao = (TextBox) e.Item.FindControl("txtNID");
				numIdentificacao.Text = DRV["numIdentificacao"].ToString(); 

				TextBox  periodCalib = (TextBox) e.Item.FindControl("txtPC");
				periodCalib.Text = DRV["periodicidadeCalibracao"].ToString(); 

				TextBox  refUltCalib = (TextBox) e.Item.FindControl("txtRUC");
				refUltCalib.Text = DRV["refUltimaCalibracao"].ToString(); 

				//				TextBox  dtUltCalib = (TextBox) e.Item.FindControl("txtDUC");
				//				dtUltCalib.Text = DRV["dtUltimaCalibracao"].ToString(); 

				DropDownList dd = (DropDownList)e.Item.FindControl("ddTipoEquipamento");
				string idTipoEquipamento = DRV["idTipoEquipamento"].ToString();

				//DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
				
				string idGrandeza= DRV["idGrandeza"].ToString(); 
				string strSQL = "SELECT  TipoEquipamento.idTipoEquipamento, TipoEquipamento.descricao FROM         TipoEquipamento INNER JOIN Familia ON TipoEquipamento.idFamilia = Familia.idFamilia INNER JOIN                 Grandeza ON Familia.idGrandeza = Grandeza.idGrandeza WHERE (Grandeza.idGrandeza = '"+idGrandeza+"') AND (TipoEquipamento.activo = 1) OR idTipoEquipamento = "+idTipoEquipamento + " ORDER BY TipoEquipamento.descricao"; 

				//Response.Write ("n sei : " + strSQL + "<br />"); 

				SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
				dd.DataSource = dr; 
				dd.DataBind(); 
				dr.Close();

   		
				dd.SelectedValue = idTipoEquipamento; 
				
			}
		}



		//*********************************************************************************
		//*********************************************************************************
		//********** FIM ACÇÕES SOBRE O DATAGRID EQUIPAMENTOS *****************************
		//*********************************************************************************
		//*********************************************************************************

		//============================================================================
		//BIND DROPDOWN EQUIPAMENTOS
		//============================================================================
		private void BindDDEquipamentos()
		{
			if(DGEquipamentos.SelectedIndex <0) 
			{
				lblMessage2.Text="Tem de seleccionar um equipamento primeiro."; 
				return; 
			}

			string strGrandeza = DGEquipamentos.Items[DGEquipamentos.SelectedIndex].Cells[3].Text; 
            string numIdent = DGEquipamentos.Items[DGEquipamentos.SelectedIndex].Cells[18].Text;
            string numSerie = DGEquipamentos.Items[DGEquipamentos.SelectedIndex].Cells[19].Text;


			if(strGrandeza =="")
			{
				lblMessage2.Text = "Seleccione o Equipamento."; 
				return; 
			}

			if(ddEmpresa.SelectedValue=="")
			{
				lblMessage2.Text ="Seleccione a Empresa antes de proceder."; 
				return; 
			}




			string strSQL = "SELECT Equipamento.idEquipamento, CAST(Equipamento.idEquipamento AS varchar) + ' - ' + CAST(TipoEquipamento.descricao AS varchar) + ' NID :' + CAST(ISNULL(Equipamento.numIdentificacao,'') AS varchar) + ' -  NS:' + CAST(ISNULL(Equipamento.numSerie,'') AS varchar) AS descricao FROM Equipamento INNER JOIN TipoEquipamento ON Equipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento  INNER JOIN Familia ON TipoEquipamento.idFamilia = Familia.idFamilia INNER JOIN Grandeza ON Familia.idGrandeza = Grandeza.idGrandeza WHERE (Grandeza.idGrandeza = '"+strGrandeza+"' "; 


			if(strGrandeza =="AUT") strSQL += " OR Grandeza.idGrandeza = 'CTA' " ; 
			if(strGrandeza =="CTA") strSQL += " OR Grandeza.idGrandeza = 'AUT' "; 


			strSQL += " ) AND idEmpresa  = " +ddEmpresa.SelectedValue;
            strSQL += " AND ( equipamento.numSerie   LIKE '" + numSerie + "%' or equipamento.numIdentificacao  LIKE '" + numIdent + "%') ";

            strSQL += " order by equipamento.idEquipamento "; 

			//Response.Write ("ddequipamentos : " + strSQL + "<br />"); 

			SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			ddEquipamentoAManter.DataTextField = "descricao";
			ddEquipamentoAManter.DataValueField="idEquipamento"; 
			ddEquipamentoAManter.DataSource = dr; 
			ddEquipamentoAManter.DataBind();
            dr.Close();

			ddEquipamentoAManter.Items.Insert(0,new ListItem("","")); 

		}



		//============================================================================
		//FILL DROPDOWN DE EMPRESAS
		//============================================================================
		private void fillDDEmpresa()
		{

			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,"","1","","","","","",""); 
            
			DataColumn dc = new DataColumn(); 
			
			dc.ColumnName="Nome_"; 
			
			DT.Columns.Add(dc); 
			foreach (DataRow dr in DT.Rows)
			{
				dr["Nome_"] = dr["nome"].ToString(); 
			}
			 

			DataView DV = new DataView(DT);
			DV.Sort = "nome ASC"; 
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}

		//============================================================================
		//FILL DROPDOWN DE EMPRESAS
		//============================================================================
		//REMOVE ESPAÇOS DE UMA STRING
		private string removeSpaces(string s)
		{
			return s.Replace(" ",""); 
		}
		
		//============================================================================
		//REMOVE CARACTERES DE UMA STRING
		//============================================================================
		private string removeCaracters(string s) //remove todos os caracters que nao sejam letra ou numero. 
		{
			string pattern = "[^a-zA-Z0-9]";
			return Regex.Replace(s, pattern,String.Empty);
		}

		//============================================================================
		//SELECCIONA A FORMATAÇÃO A SER APLICADA NA STRING
		//============================================================================
		protected string formataString(string s) //usado no datagrid
		{
			switch(rbCriteriosPesquisa.SelectedValue)
			{
				case "ignorarEspacos":
					return removeSpaces(s); 
					//break; 
				case "ignorarCaracteres" :
					return removeCaracters(s); 
					//break; 
				case "ignorarAmbos" :
					return(removeCaracters(removeSpaces(s))); 	
					//break; 
				case "nada" :
					return s; 
					//break; 
				 default:
					return s; 
					//break; 
			}
			
		}
		

		//============================================================================
		//PESQUISA DE EMPRESA (FILL DA DROPDOWN DE EMPRESAS)
		//============================================================================
		private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			
			DGEquipamentos.DataSource = null; 
			DGEquipamentos.DataBind(); 

			txtFamilia.Text ="";
			txtGrandeza.Text =""; 
			txtNumIdentificacao.Text=""; 
			txtNumSerie.Text=""; 
			
		}
		
		//============================================================================
		//TROCA EQUIPAMENTOS
		//============================================================================
		private void btnTrocar_Click(object sender, System.EventArgs e)
		{
			
			string idASubstituir = DGEquipamentos.DataKeys[DGEquipamentos.SelectedIndex].ToString(); 
			if(idASubstituir =="") 
			{
				lblMessage2.Text ="Tem de seleccionar o equipamento a substituir."; 
				return;
			}

			if(ddEquipamentoAManter.SelectedValue =="")
			{
				lblMessage2.Text ="Tem de seleccionar o equipamento a manter."; 
				return;
			}

			if(trocarEquipamentos(ddEquipamentoAManter.SelectedValue,idASubstituir)) 
			{
				BindGrid();  
			}
			
		}

		//============================================================================
		//EFECTUA A ACÇÃO NA BD QUE TROCA OS EQUIPAMENTOS
		//============================================================================	
		private bool trocarEquipamentos(string idEquipamentoAManter, string idAApagar)
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
					objCmd.CommandType = CommandType.Text; 

					try
					{
						if(idEquipamentoAManter == idAApagar) return false; //nao eliminar

						objCmd.CommandText="UPDATE SERVICO SET idEquipamento = "+idEquipamentoAManter +" WHERE idEquipamento ="+idAApagar; 
						objCmd.ExecuteNonQuery(); 

						//guarda em log. 
						GERAL.clsWriteError.WriteLog(User.Identity.Name.ToString() + " --" + objCmd.CommandText.ToString()); 

						objCmd.CommandText =" INSERT INTO EquipamentosApagados SELECT * FROM Equipamento WHERE idEquipamento = "+ idAApagar;
						objCmd.ExecuteNonQuery(); 

						objCmd.CommandText =" UPDATE EquipamentosApagados SET idUtilAlteracao = "+Session["UserID"] +" WHERE idEquipamento = "+ idAApagar;
						objCmd.ExecuteNonQuery(); 

						objCmd.CommandText =" DELETE FROM Equipamento WHERE idEquipamento = "+ idAApagar;
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
		
		
		//============================================================================	
		//COR DO ACTIVO (BRANCO/VERMELHO)
		//============================================================================	
		protected System.Drawing.Color ConvertColor(int i)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();

			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Red");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}

			return colorName; 
		}

		//============================================================================	
		//MOSTRA OS DETALHES DO EQUIPAMENTO (BIND GRID GENERICO)
		//============================================================================	
		private void Linkbutton1_Click(object sender, System.EventArgs e)
		{//detalhes do equipamento
			if(DGEquipamentos.SelectedIndex <0) 
			{
				lblMessage2.Text="Tem de seleccionar um equipamento primeiro."; 
				return; 
			}

			dgGenerico.AutoGenerateColumns=true; 
			
			DATA.EquipamentoBD equip = new LabMetro.DATA.EquipamentoBD();
			
			DataTable DT = equip.DTEquipamentobyIDBO(DGEquipamentos.DataKeys[DGEquipamentos.SelectedIndex].ToString()); 
			
			equip = null; 

			dgGenerico.DataSource =DT;
			dgGenerico.DataBind();

			dgCertificados.Visible=false; 
			dgGenerico.Visible=true; 
		}

		//============================================================================	
		//MOSTRA OS HISTÓRICO DO EQUIPAMENTO (BIND GRID GENERICO)
		//============================================================================	
		private void Linkbutton2_Click(object sender, System.EventArgs e)
		{//Histórico
			if(DGEquipamentos.SelectedIndex <0) 
			{
				lblMessage2.Text="Tem de seleccionar um equipamento primeiro."; 
				return; 
			}

			dgGenerico.AutoGenerateColumns=true; 
			string strSQL = "SELECT     TipoEquipamento.descricao AS tipoEquipamento, HistoricoEquipamento.numIdentificacao, HistoricoEquipamento.numSerie, HistoricoEquipamento.refUltimaCalibracao, HistoricoEquipamento.dtAlteracao, Funcionario.nomeAbreviado FROM HistoricoEquipamento INNER JOIN Funcionario ON HistoricoEquipamento.idUtilAlteracao = Funcionario.idUtilizador INNER JOIN TipoEquipamento ON HistoricoEquipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento WHERE idEquipamento = "+ DGEquipamentos.DataKeys[DGEquipamentos.SelectedIndex].ToString() + " ORDER BY HistoricoEquipamento.dtAlteracao desc "; 

			//Response.Write(strSQL); 
			
			DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			dgGenerico.DataSource =dt;
			dgGenerico.DataBind();

			dgCertificados.Visible=false; 
			dgGenerico.Visible=true; 
		}
		
		//============================================================================	
		//MOSTRA OS SERVIÇOS EFECTUADOS SOBRE O EQUIPAMENTO (BIND GRID GENERICO)
		//============================================================================	
		private void Linkbutton3_Click(object sender, System.EventArgs e)
		{//serviços

			if(DGEquipamentos.SelectedIndex <0) 
			{
				lblMessage2.Text="Tem de seleccionar um equipamento primeiro."; 
				return; 
			}

			dgGenerico.AutoGenerateColumns=true; 
			string strSQL = "SELECT S.idEquipamento, S.refServico, ESTADOsERVICO.DESCRICAO AS ESTADO, s.dtCriacao as dtEntrada FROM SERVICO  S INNER JOIN ESTADOSERVICO ON S.IDESTADOSERVICO = ESTADOSERVICO.IDESTADOSERVICO 	WHERE S.idEquipamento = "+ DGEquipamentos.DataKeys[DGEquipamentos.SelectedIndex].ToString() + " ORDER BY s.dtCriacao"; 
			//Response.Write(strSQL); 
			
			DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			dgGenerico.DataSource =dt;
			dgGenerico.DataBind();
			
			dgCertificados.Visible=false; 
			dgGenerico.Visible=true; 
		}

		
		//============================================================================	
		//MOSTRA OS CERTIFICADOS DOS SERVIÇO SOBRE O EQUIPAMENTO (BIND GRID CERTIFICADOS)
		//============================================================================	
		private void Linkbutton4_Click(object sender, System.EventArgs e)
		{

			if(DGEquipamentos.SelectedIndex <0) 
			{
				lblMessage2.Text="Tem de seleccionar um equipamento primeiro."; 
				return; 
			}
			//certificados
			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD(); 
			DataTable DT = data.DTListCertificadoByEquipamento(DGEquipamentos.DataKeys[DGEquipamentos.SelectedIndex].ToString()); 
			data = null; 
			
			dgCertificados.DataSource=DT;
			dgCertificados.DataBind(); 

			dgCertificados.Visible=true; 
			dgGenerico.Visible=false; 
			
		}	

		//============================================================================	
		//ITEM DATABOUND DO GRID CERTIFICADOS.
		//============================================================================	
		public void dgCertificados_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
			{
				LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];

				int numCells = e.Item.Cells.Count; 
				for (int i = 1; i < numCells; i++)
				{

					//Response.Write("cell" + i + "-"+e.Item.Cells[i].Text+"<br />"); 
					if(!e.Item.Cells[i].HasControls()) //para nao pôr o link nas cells que conteem checkboxes
					{
						e.Item.Cells[i].ToolTip = "Click para visualisar o documento " + e.Item.Cells[3].Text;
						e.Item.Cells[i].Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(button, ""));
					}
				}
			}
		}

		//==================================================================================
		// Função que permite visualizar o documento pretendido pelo utilizador
		//==================================================================================
		public void visualisarDocumento(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName.ToString() == "Select")
			{
				string doc = e.Item.Cells[4].Text;
				string nome = downloadpath(doc);
				Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");
			}
		}

		public string downloadpath(object filename)
		{
			if(filename!=null && filename.ToString()!="") 
			{
				string myPath = (string)ConfigurationManager.AppSettings["PATHABS_CERT_FINAIS_CERTIFICADOS"];
				myPath =myPath + "/" + filename.ToString();
				
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}

		
		//============================================================================	
		//APAAGA UM EQUIPAMENTO DA BD(QUASE...) (COPIA O EQUIPAMENTO PARA OUTRA TABELA, PARA HISTORICO)...	
		//============================================================================	
		private void apagaEquipamento(string idEquipamento)
		{
			
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				
				objCmd.Connection = objConn; 

				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					objCmd.CommandType = CommandType.Text; 
					try
					{
						objCmd.CommandText =" INSERT INTO EquipamentosApagados SELECT * FROM Equipamento WHERE idEquipamento = "+ idEquipamento;
						objCmd.ExecuteNonQuery(); 

						objCmd.CommandText =" UPDATE EquipamentosApagados SET idUtilAlteracao = "+Session["UserID"] +" WHERE idEquipamento = "+ idEquipamento;
						objCmd.ExecuteNonQuery(); 

						objCmd.CommandText =" DELETE FROM Equipamento WHERE idEquipamento = "+ idEquipamento;
						objCmd.ExecuteNonQuery(); 

						objTrans.Commit(); 
						
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
							
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text +=ex.Message.ToString()+"<br />";
						
					}
				}	
			}
		}
		

		//====================================================================================
		//SELECTEDINDEXCHANGED DA DROPDOWN EMPRESAS
		//====================================================================================
		
		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGEquipamentos.DataSource =null; 
			DGEquipamentos.DataBind(); 
			ddEquipamentoAManter.Items.Clear(); 
			DGEquipamentos.SelectedIndex = -1; 
		}

		private void rbCriteriosPesquisa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void Linkbutton5_Click(object sender, System.EventArgs e)
		{
		
			BindDDEquipamentos(); 

		}

			
	}
}
