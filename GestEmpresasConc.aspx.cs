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


namespace LabMetro
{
	/// <summary>
	/// Summary description for GestEmpresasConc.
	/// </summary>
	public partial class GestEmpresasConc : System.Web.UI.Page
	{
		
		protected System.Web.UI.WebControls.TextBox txtEmpresa;
		protected System.Web.UI.WebControls.PlaceHolder menuPlaceHolder;
		protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa; 
		protected System.Web.UI.WebControls.TextBox Textbox1;
		protected System.Web.UI.WebControls.Button btnContinuar;
		protected System.Web.UI.WebControls.LinkButton Linkbutton4;
		protected System.Web.UI.WebControls.LinkButton Linkbutton5;
		protected System.Web.UI.WebControls.LinkButton btnBREs;
		protected System.Web.UI.WebControls.DropDownList DropDownList2;
		protected System.Web.UI.WebControls.DropDownList ddFiltro;
        //NOME DA PAGINA
        private const string ID_PAG = "GESTEMPCONC_1";

		DataView DV; 
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text =""; 
			lblAlerta.Text=""; 
			Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			
			if(ht == null) //session expired
			{
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					Server.Transfer("Default.aspx?err=1",false);
				}
				else
				{

					if(!Page.IsPostBack)
					{

                       
						//defaultSort das empresas
						ViewState["sortFieldEmpresa"] = "nome";
						ViewState["sortFieldContacto"] = "nome";
						ViewState["sortFieldEqu"] = "tipoEquipamento,numSerie";
						ViewState["sortDirection"] = "ASC";			
						ViewState["sortDirectionEqu"] = "ASC";	
						ViewState["sortDirectionReq"]="DESC";
						ViewState["sortFieldReq"] = "idRequisicao";
						lblLegenda.Visible=false; 

						fillDDs(); 
				
						ViewState["alerta"]="sim"; 

						lblAlerta.Text = "Empresas com visitas em atraso ou equipamentos com calibrações em atraso";

						BindGridEmpresasAlertas(); 
						fillDDFamilias();
				
					}
					else
					{
						if(ViewState["alerta"].ToString()=="sim")
						{
							lblAlerta.Text = "Empresas com visitas em atraso ou equipamentos com calibrações em atraso";
							btnReport.Text="Imp. Lista empresas atraso";
						}
						else
						{
							lblAlerta.Text = "";
							btnReport.Text="Imp. Calibs. período"; 
						}
					}
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			InitializeComponent2();
			base.OnInit(e);
		}
	
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		private void InitializeComponent2()
		{    
			btnContactos.Click += new System.EventHandler(btnContactos_Click);
			btnEquipamentos.Click += new System.EventHandler(btnEquipamentos_Click);
			btnMarcacoes.Click += new System.EventHandler(btnMarcacoes_Click);
			
			btnRequisicao.Click += new System.EventHandler(btnRequisicao_Click);
			
			dgContactos.SelectedIndexChanged += new System.EventHandler(dgContactos_SelectedIndexChanged);
			dgGenerico.SelectedIndexChanged += new System.EventHandler(dgGenerico_SelectedIndexChanged);
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
			btnActualizarDadosEmpresaEquip.Click += new System.EventHandler(btnActualizarDadosEmpresaEquip_Click);

			DGEmpresas.SelectedIndexChanged += new System.EventHandler(dgEmpresas_SelectedIndexChanged);
			btnFazerMarcacao.Click += new System.EventHandler(btnFazerMarcacao_Click);
			dgContactos.SelectedIndexChanged += new System.EventHandler(dgContactos_SelectedIndexChanged);
			dgGenerico.SelectedIndexChanged += new System.EventHandler(dgGenerico_SelectedIndexChanged);
			btnReport.Click += new System.EventHandler(btnReport_Click);

			btnA.Click += new System.EventHandler(btnA_Click);
			bntC.Click += new System.EventHandler(bntC_Click);
			bntE.Click += new System.EventHandler(bntE_Click);
			
		}

		#endregion

		
		private void fillDDFamilias()
		{
			string strSQL = "select idfamilia, descricao from familia where idGrandeza = 'cta'";
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddFamilias.DataSource = DR;
			ddFamilias.DataBind(); 
			ddFamilias.Items.Insert(0, new ListItem("Sem filtro",""));
            DR.Close();

		}
	
		//******************************************************************************
		//********** FUNÇÕSE SOBRE O DATAGRID EMPRESAS ********************************
		//******************************************************************************

		//===================================================================
		//EDITA DATAGRID EMPRESAS
		//===================================================================
		protected void editEmpresa(Object sender, DataGridCommandEventArgs e)     
		{
			DGEmpresas.ShowFooter=false;     
			DGEmpresas.EditItemIndex = e.Item.ItemIndex;	
			BindGridEmpresas();
		}

		//===================================================================
		//CANCELA EDIÇÃO DE DATAGRID EMPRESAS
		//===================================================================
		protected void cancelEmpresa(Object sender, DataGridCommandEventArgs e)
		{
			DGEmpresas.ShowFooter=true;  
			DGEmpresas.EditItemIndex = -1;
			BindGridEmpresas();
		}
		
		//===================================================================
		//ALTERA DADOS DE EMPRESA
		//===================================================================
		protected void updateEmpresa(Object sender, DataGridCommandEventArgs e)
		{
			string idEmpresa = DGEmpresas.DataKeys[e.Item.ItemIndex].ToString();
		
			TextBox  tipoContrato = (TextBox) e.Item.FindControl("txtEditTipoContrato");
			TextBox  dtUltimaVisita = (TextBox) e.Item.FindControl("txtEditDtUltimaVisita");
			CheckBox cbEditConcessionario = (CheckBox) e.Item.FindControl("cbEditConcessionario");
			CheckBox cbEditCTA = (CheckBox) e.Item.FindControl("cbEditCTA");
			CheckBox cbEditCentroB = (CheckBox) e.Item.FindControl("cbEditCentroB");
			CheckBox cbEditSistemaPesagem = (CheckBox) e.Item.FindControl("cbEditSistemaPesagem");
			CheckBox cbAGE = (CheckBox) e.Item.FindControl("cbEditAGE");

			TextBox  telefone = (TextBox) e.Item.FindControl("txtEditTelefone");
			TextBox  fax = (TextBox) e.Item.FindControl("txtEditFax");
			TextBox  email = (TextBox) e.Item.FindControl("txtEditEmail");


			DATA.EmpresasBD emp = new LabMetro.DATA.EmpresasBD(); 
			
			int i = emp.UpdateEmpresaConc(idEmpresa, tipoContrato.Text,dtUltimaVisita.Text,cbEditCTA.Checked.ToString(),cbEditConcessionario.Checked.ToString(), cbEditSistemaPesagem.Checked.ToString(),cbEditCentroB.Checked.ToString(),telefone.Text, email.Text,fax.Text,cbAGE.Checked.ToString()); 

			
			emp = null; 

			if(i>0) 
			{
				lblMessage.Text="Alteração efectuada"; 
				DGEmpresas.EditItemIndex = -1;
				DGEmpresas.ShowFooter=true;   
				BindGridEmpresas(); 
			}	
			else 
			{
				lblMessage.Text ="Alteração não efectuada"; 
			}
		}

		//===================================================================
		//PAGINAÇÃO DO DATAGRID EMPRESAS
		//===================================================================
		public void DoPagingEmpresas(Object s,DataGridPageChangedEventArgs e)
		{
			DGEmpresas.CurrentPageIndex = e.NewPageIndex; 
			if(ViewState["alerta"].ToString()=="sim")
			{
				BindGridEmpresasAlertas(); 
			}
			else
			{
				BindGridEmpresas(); 
			}
		}

		//===================================================================
		//ORDENAÇÃO DO DATAGRID EMPREAS
		//===================================================================
		public void SortGridEmpresas(Object s, DataGridSortCommandEventArgs e)
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
			ViewState["sortFieldEmpresa"] = e.SortExpression;

			if(ViewState["alerta"].ToString()=="sim")
			{
				BindGridEmpresasAlertas(); 
			}
			else
			{
				BindGridEmpresas(); 
			}

			
		}

		//===================================================================
		//DATASET COM DADOS DAS EMPREASAS
		//===================================================================
		private DataSet DSEmpresas()  //aqui uso este pq isto vem do copypaste da pagina empresas
										//mas criei um novo com mais campos na funcao a seguir
										//sem tempo para testar implicacoes, por isso criei um novo em vez de
										//alterar o ja existente.
		{
			//DataSet ds = new DATASETS.DSEmpresaNew(); 

            DataSet ds = new DATASETS.DSEmpresa();
			string strConcessionario = ""; 
			if(cbConcessionario.Checked == true) strConcessionario = "1"; 

			string strCTA = ""; 
			if(cbCTA.Checked == true) strCTA = "1"; 

			string strCentroB = ""; 
			if(cbCentroB.Checked == true) strCentroB = "1"; 

			string strSistemaPesagem = ""; 
			if(cbSistemaPesagem.Checked == true) strSistemaPesagem = "1"; 

			string strAGE = ""; 
			if(cbAGE.Checked == true) strAGE = "1"; 

			SqlParameter[] arrParams = new SqlParameter[13];
			arrParams[0] = new SqlParameter("@inNome", txtNomeEmpresa.Text);
			arrParams[1] = new SqlParameter("@inNif", txtNIF.Text);
			arrParams[2] = new SqlParameter("@inNumClienteSAP",txtNumClienteSAP.Text);
			arrParams[3] = new SqlParameter("@inConcessionario",strConcessionario);
			arrParams[4] = new SqlParameter("@inCentroB",strCentroB);
			arrParams[5] = new SqlParameter("@inCTA",strCTA);
			arrParams[6] = new SqlParameter("@inSistemaPesagem",strSistemaPesagem);
			arrParams[7] = new SqlParameter("@inDtUltCalibStart",txtDtInicioCalib.Text);
			arrParams[8] = new SqlParameter("@inDtUltCalibEnd", txtDtFimCalib.Text);
			arrParams[9] = new SqlParameter("@inDtUltVisitaStart",txtDtInicioVisita.Text);
			arrParams[10] = new SqlParameter("@inDtUltVisitaEnd", txtDtFimVisita.Text);
			arrParams[11] = new SqlParameter("@inNumSerie", txtNumSerie.Text);
			arrParams[12] = new SqlParameter("@inAGE", strAGE);
	
			ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

			try
			{
				ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListEmpresasConcessionarios",ds,"Empresa",arrParams);
				return ds; 
			}
			catch
			{
				return null; 
			}
		}

		//===================================================================
		//BIND GRID EMPRESAS
		//===================================================================
		private void BindGridEmpresas()
		{
			DataSet ds = DSEmpresas();
	
			if(ds != null)
			{
	
				DV = new DataView(ds.Tables["Empresa"]);
				//DV.Sort = ViewState["sortFieldEmpresa"].ToString()+ " " + ViewState["sortDirection"]; 
				DGEmpresas.DataSource =DV; 
				DGEmpresas.DataBind(); 
        
				if(DV.Table.Rows.Count > 0)
				{
					DGEmpresas.Visible=true;
				}
				else
				{
					DGEmpresas.Dispose();
					DGEmpresas.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
				}
			}		
		}

			//===================================================================
			//BIND GRID EMPRESAS COM ALERTA, ISTO É COM CALIBRAÇÕES EM ATRASO. 
			//===================================================================
			private void BindGridEmpresasAlertas()
			{
				//DataSet ds = new DATASETS.DSEmpresaNew(); 
                DataSet ds = new DATASETS.DSEmpresa();
				ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

				try
				{
					ds = GERAL.clsDataAccess.DSFillDS_SP("stpGetListEmpresasConcessionariosAlertas",ds,"Empresa");
				}
				catch
				{
				}
				


				if(ds.Tables[0]!= null)
				{
	
					DV = new DataView(ds.Tables[0]);
					DV.Sort = ViewState["sortFieldEmpresa"].ToString()+ " " + ViewState["sortDirection"]; 
					DGEmpresas.DataSource =DV; 
					DGEmpresas.DataBind(); 
        
					if(DV.Table.Rows.Count > 0)
					{
						DGEmpresas.Visible=true;
					}
					else
					{
						DGEmpresas.Dispose();
						DGEmpresas.Visible=false; 
						lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
					}
				}
				
			}

			
		//******************************************************************************
		//********** relatórios a imprimir - alertas, e calibrações do período *********
		//******************************************************************************
		
		private void imprimirAlertas()
		{
			//DataSet ds = new DATASETS.DSEmpresaNew(); 
            DataSet ds = new DATASETS.DSEmpresa();
			ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

			try
			{
				if(ViewState["alerta"].ToString()=="sim")
				{
					ds = GERAL.clsDataAccess.DSFillDS_SP("stpGetListEmpresasConcessionariosAlertas",ds,"Empresa");
				}
				else
				{
					ds = DSEmpresas(); 
				}

				REPORTS.rptEmpresasConc report = new rptEmpresasConc(); 
				clsReport cr = new clsReport();
				report.SetDataSource(ds);
                ds = null; 
                
                cr.exportReportToPDF(report,"Report");
				
			
                //cr = null; 
                //report = null;	
				
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString()); 
			}		
		}

		private void btnReport_Click(object sender, System.EventArgs e)
		{

			if(ViewState["alerta"].ToString()=="sim")
			{
				imprimirAlertas(); 

			}
			else
			{
				if(txtDtInicioCalib.Text!="" && txtDtFimCalib.Text!="") 
				{			
					imprimirCalibracoesPeriodo(); 
				}
			
			}
		}


		

		//===================================================================
		//DATASET COM DADOS DAS EMPREASAS
		//===================================================================
		private DataSet DSCalibracoesDoPeriodo()  //aqui uso este pq isto vem do copypaste da pagina empresas
			//mas criei um novo com mais campos na funcao a seguir
			//sem tempo para testar implicacoes, por isso criei um novo em vez de
			//alterar o ja existente.
		{
			DataSet ds = new DATASETS.DSCalibracoesDoPeriodo(); 

			string strConcessionario = ""; 
			if(cbConcessionario.Checked == true) strConcessionario = "1"; 

			string strCTA = ""; 
			if(cbCTA.Checked == true) strCTA = "1"; 

			string strAGE = ""; 
			if(cbAGE.Checked == true) strAGE = "1"; 

			string strCentroB = ""; 
			if(cbCentroB.Checked == true) strCentroB = "1"; 

			string strSistemaPesagem = ""; 
			if(cbSistemaPesagem.Checked == true) strSistemaPesagem = "1"; 
		
			string sGrandezas =null;
			if(cbGrandeza.SelectedIndex >-1)
			{
				foreach(ListItem li in cbGrandeza.Items)
				{
					if(li.Selected == true)
					{
						sGrandezas += "'"+ li.Value +"',";
					}
				}
				sGrandezas = sGrandezas.TrimEnd(",".ToCharArray()); 
			}
			

			SqlParameter[] arrParams = new SqlParameter[14];
			arrParams[0] = new SqlParameter("@inNome", txtNomeEmpresa.Text);
			arrParams[1] = new SqlParameter("@inNif", txtNIF.Text);
			arrParams[2] = new SqlParameter("@inNumCliente",txtNumClienteSAP.Text);
			arrParams[3] = new SqlParameter("@inConcessionario",strConcessionario);
			arrParams[4] = new SqlParameter("@inCentroB",strCentroB);
			arrParams[5] = new SqlParameter("@inCTA",strCTA);
			arrParams[6] = new SqlParameter("@inSistemaPesagem",strSistemaPesagem);
			arrParams[7] = new SqlParameter("@inDtUltCalibStart",txtDtInicioCalib.Text);
			arrParams[8] = new SqlParameter("@inDtUltCalibEnd", txtDtFimCalib.Text);
			arrParams[9] = new SqlParameter("@inDtUltVisitaStart",txtDtInicioVisita.Text);
			arrParams[10] = new SqlParameter("@inDtUltVisitaEnd", txtDtFimVisita.Text);
			arrParams[11] = new SqlParameter("@inAGE", strAGE);
			arrParams[12] = new SqlParameter("@inOrder", null);
			arrParams[13] = new SqlParameter("@inFiltroGrandeza", sGrandezas);
	
			ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

			try
			{
				
				
				ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListEmpresasConcessionariosReport",ds,"stpGetListCalibracoesDoPeriodo",arrParams);
				return ds;
				
			 
			}
			catch
			{
				return null; 
			}
		}


		private void imprimirCalibracoesPeriodo()
		{
		
			DataSet ds = DSCalibracoesDoPeriodo(); 
				
			REPORTS.rptCalibracoesPeriodo report = new rptCalibracoesPeriodo(); 
			clsReport cr = new clsReport();
			
			
			report.SetDataSource(ds); 
            ds = null;
            // Exportar o report para PDF
            cr.exportReportToPDF(report, "Report");
				
			
		}

		//******************************************************************************
		//********** FUNÇÕSE SOBRE O DATAGRID CONTACTOS ********************************
		//******************************************************************************
		
		//===================================================================
		//EDITAR GRID CONTACTOS
		//===================================================================
		protected void editContacto(Object sender, DataGridCommandEventArgs e)     
		{
			dgContactos.ShowFooter=false;     
			dgContactos.EditItemIndex = e.Item.ItemIndex;	
			BindGridContactos();
		}

		//===================================================================
		//CANCELAR EDIÇÃO DE GRID CONTACTOS
		//===================================================================
		protected void cancelContacto(Object sender, DataGridCommandEventArgs e)
		{
			dgContactos.ShowFooter=true;  
			dgContactos.EditItemIndex = -1;
			BindGridContactos();
		}
		
		//===================================================================
		//ALTERAR DADOS DE CONTACTO
		//===================================================================
		protected void updateContacto(Object sender, DataGridCommandEventArgs e)
		{
//			string idContacto = dgContactos.DataKeys[e.Item.ItemIndex].ToString();
//		
//			//            
			
		}

		//===================================================================
		//BIND GRID CONTACTOS
		//===================================================================
		private void BindGridContactos()
		{
			if(DGEmpresas.SelectedIndex <0) 
			{
				lblMessage.Text="Seleccione a empresa."; 
				return; 
			}
			

			DATA.ContactosBD contactos = new LabMetro.DATA.ContactosBD();
			dgContactos.DataSource = contactos.DTFillContacts(DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString(),null, null,null); 
			dgContactos.DataBind();
			contactos = null; 
		}

		//===================================================================
		//PAGINAÇÃO GRID CONTACTOS
		//===================================================================
		public void DoPagingContactos(Object s,DataGridPageChangedEventArgs e)
		{
			dgContactos.CurrentPageIndex = e.NewPageIndex; 
			BindGridContactos(); 
		}

		//===================================================================
		//ORDENAÇÃO GRID CONTACTOS
		//===================================================================
		public void SortGridContactos(Object s, DataGridSortCommandEventArgs e)
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
			ViewState["sortFieldContacto"] = e.SortExpression;
			BindGridContactos(); 
		}


		
		//*********************************************************************************
		//*********************************************************************************
		//********** ACÇÕES SOBRE O DATAGRID EQUIPAMENTOS *********************************
		//*********************************************************************************
		//*********************************************************************************


		
		//============================================================================
		//BIND DO DATAGRID (EQUIPAMENTOS)
		//============================================================================
		private void BindGridEquipamentos()
		{  
			if(DGEmpresas.SelectedIndex <0)
			{
				lblMessage.Text ="Seleccione a empresa."; 
				return; 
			}
			string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
			
			
			DATA.EquipamentoBD equip = new LabMetro.DATA.EquipamentoBD();
			try
			{
				
				DataTable DT = equip.DTEquipamentoBOCONC(idEmpresa,"","","","","", ""); 
				DataView DV = new DataView(DT); //,"",strSort,DataViewRowState.CurrentRows); 
				string rowFilter = "Estado = 1 "; 
				if(ddGrandezas.SelectedValue!="") rowFilter+= " and idgrandeza = '" + ddGrandezas.SelectedValue +"'";
				if(ddFamilias.SelectedValue!="") rowFilter+= " and familia = '" + ddFamilias.SelectedItem.Text +"'";
				DV.RowFilter =rowFilter; 

//				Response.Write(rowFilter); 

				DV.Sort = ViewState["sortFieldEqu"].ToString()+ " " + ViewState["sortDirectionEqu"]; 

				
				DGEquipamentos.DataSource = DV; 
				DGEquipamentos.DataBind(); 
				
				
				if(DV.Table.Rows.Count > 0)
				{
					DGEquipamentos.Visible=true;
					lblLegenda.Visible= true; 
				}
				else
				{
					DGEquipamentos.Dispose();
					DGEquipamentos.Visible=false; 
					lblLegenda.Visible= false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
					
				}
			}
			catch(Exception ex)
			{

				DGEquipamentos.Dispose();
				DGEquipamentos.Visible=false;
				lblLegenda.Visible= false; 
				lblMessage.Text= ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
			}
			finally
			{

				equip = null; 
			}
		}

		
		

		//============================================================================
		//ORDENAÇÃO DO DATAGRID EQUIPAMENTOS
		//============================================================================
		public void SortGridEquipamentos(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionEqu"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionEqu"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionEqu"]="ASC";
					break;
			}
			ViewState["sortFieldEqu"] = e.SortExpression;
			BindGridEquipamentos(); 
		}

		

		//============================================================================	
		//EDITA OS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void editEquipamento(Object sender, DataGridCommandEventArgs e)     
		{
			DGEquipamentos.ShowFooter=false;     
			DGEquipamentos.EditItemIndex = e.Item.ItemIndex;	
			BindGridEquipamentos();
		}

		//============================================================================	
		//CANCELA A EDIÇÃO DOS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void cancelGridEquipamento(Object sender, DataGridCommandEventArgs e)
		{
			DGEquipamentos.ShowFooter=true;  
			DGEquipamentos.EditItemIndex = -1;
			BindGridEquipamentos();
		}

		//============================================================================	
		//ALTERA OS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void updateEquipamento(Object sender, DataGridCommandEventArgs e)
		{
			string idEquipamento = DGEquipamentos.DataKeys[e.Item.ItemIndex].ToString();
			
			//			TextBox  numSerie = (TextBox) e.Item.FindControl("txtNS");
			//			TextBox  numIdentificacao = (TextBox) e.Item.FindControl("txtNID");
			TextBox  periodCalib = (TextBox) e.Item.FindControl("txtPC");
			TextBox  refUltCalib = (TextBox) e.Item.FindControl("txtRUC");
			TextBox  dtUltCalib = (TextBox) e.Item.FindControl("txtDUC");
			TextBox  obs = (TextBox) e.Item.FindControl("txtObs");

			
			//descomentar para produção
			if(Session["UserId"] == null) Session["UserId"] ="0";//Response.Redirect("../Default.aspx",true); 

			try
			{
				DATA.EquipamentoBD eq = new LabMetro.DATA.EquipamentoBD(); 
				
			
				int i = eq.updateEquipamentoInBOConc(periodCalib.Text, refUltCalib.Text,dtUltCalib.Text,Session["UserId"].ToString(),idEquipamento,obs.Text);  //pode devolver mais que 1 por causa do trigger na bd. 
                if (i > 0) lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
                else lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE; 
				eq = null; 
				DGEquipamentos.EditItemIndex = -1;
				DGEquipamentos.ShowFooter=true;   
				BindGridEquipamentos(); 
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString()); 
			}			
		}

		protected void DGEmpresas_DataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				LinkButton button =(LinkButton)e.Item.Cells[15].Controls[0];
				button.CausesValidation=false; 
			}
		}


		//====================================================================================
		//ITEM DATABOUND DO DATAGRID EQUIPAMENTOS
		//====================================================================================
		
		protected void DGEquipamentos_DataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{

				LinkButton b =(LinkButton)e.Item.Cells[13].Controls[0];
				b.CausesValidation=false; 

				DataRowView DRV = (DataRowView) e.Item.DataItem;

				TextBox  periodCalib = (TextBox) e.Item.FindControl("txtPC");
				periodCalib.Text = DRV["periodicidadeCalibracao"].ToString(); 

				TextBox  refUltCalib = (TextBox) e.Item.FindControl("txtRUC");
				refUltCalib.Text = DRV["refUltimaCalibracao"].ToString(); 
							
			}
		}



		//*********************************************************************************
		//*********************************************************************************
		//********** FIM ACÇÕES SOBRE O DATAGRID EQUIPAMENTOS *****************************
		//*********************************************************************************
		//*********************************************************************************

		
		//*******************************************************************************
		//*************** ACÇÕES DE BOTÕES  *********************************************
		//*******************************************************************************


		//===================================================================
		//PESQUISA POR CRITÉRIOS DE EMPRESA - MOSTRA DATAGRID EMPRESAS
		//===================================================================
		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			ViewState["alerta"] = "n"; 
			lblAlerta.Text = "";
			btnReport.Text="Imp. Calibs. período"; 
			DGEmpresas.CurrentPageIndex=0;
			BindGridEmpresas(); 	
			DGEquipamentos.DataSource = null;
			DGEquipamentos.DataBind();
			dgContactos.DataSource = null; 
			dgContactos.DataBind(); 
		}

		//===================================================================
		//MOSTRA OS CONTACTOS DA EMPRESA SELECCIONADA
		//===================================================================
		private void btnContactos_Click(object sender, System.EventArgs e)
		{
			//copy/paste de nao sei onde, esquisito, mas funciona...
			dgContactos.CurrentPageIndex = 0;
			if (dgContactos.Items.Count != 0)
			{
				dgContactos.DataSource = null;
				dgContactos.DataBind();
				dgContactos.Controls.Clear();
				dgContactos.Visible = false;
				btnContactos.Text = "Ver Contactos";
			}
			else
			{
				BindGridContactos();
				if (dgContactos.Items.Count == 0)
					dgContactos.Visible = false;
				else
				{
					dgContactos.Visible = true;
					btnContactos.Text = "Ocultar Contactos";
				}
			}	
		}

		//===================================================================
		//===================================================================
		private void btnEquipamentos_Click(object sender, System.EventArgs e)
		{
			//copy/paste de nao sei onde, esquisito, mas funciona...
			DGEquipamentos.CurrentPageIndex = 0;
			if (DGEquipamentos.Items.Count != 0)
			{
				DGEquipamentos.DataSource = null;
				DGEquipamentos.DataBind();
				DGEquipamentos.Controls.Clear();
				DGEquipamentos.Visible = false;
				lblLegenda.Visible= false; 
				btnEquipamentos.Text = "Ver Equip.";
			}
			else
			{
				BindGridEquipamentos();
				if (DGEquipamentos.Items.Count == 0)
				{
					DGEquipamentos.Visible = false;
					lblLegenda.Visible= false; 
				}
				else
				{
					DGEquipamentos.Visible = true;
					lblLegenda.Visible= true; 
					btnEquipamentos.Text = "Ocultar Equip.";
				}
			}	
		}

		
		//=========================================================================================
		//==========actualizar equipamentos com data ultima visita à empresa
		//=========================================================================================

		private void alterarDatasEquipamentos()
		{
			string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
			
			string strIds =""; //dos equipamentos
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
			{ 
				CheckBox myCheckBox =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				
				if(myCheckBox.Checked == true)
				{
					
					strIds+= DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
					strIds+=",";
				}   
			}
			strIds = strIds.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item

			string strSQL ="UPDATE Equipamento  SET dtUltimaCalibracao  = e.dtUltimaVisita, periodicidadeCalibracao = e.intTipoContrato FROM equipamento eq, empresa e    WHERE eq.idEmpresa = e.idEmpresa AND eq.idEquipamento in ("+strIds+")";
 
			int i = GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
			
			lblMessage.Text = i + " Equipamentos actualizados"; 
			BindGridEquipamentos(); 
		}


		private void dgEmpresas_SelectedIndexChanged(object sender, System.EventArgs e)

		{
			btnContactos.Text="Contactos"; 
			btnEquipamentos.Text="Equipamentos"; 
			btnMarcacoes.Text ="Marcações"; 
			btnRequisicao.Text = "Requisições"; 

			
			dgContactos.SelectedIndex = 0; 
			dgContactos.DataSource = null; 
			dgContactos.DataBind(); 

			//DGEquipamentos.SelectedIndex = 0; 
			DGEquipamentos.DataSource = null;
			DGEquipamentos.DataBind();

			dgMarcacoes.SelectedIndex = 0; 
			dgMarcacoes.DataSource = null; 
			dgMarcacoes.DataBind(); 

			dgRequisicoes.SelectedIndex = 0; 
			dgRequisicoes.DataSource = null;
			dgRequisicoes.DataBind();

			bindDDRequisicao(); 
		}

	

		///*************************************************************************************************
		//INÍCIO MARCAÇÕES
		//*************************************************************************************************


		//=================================================================================================
		//BOTAO QUE VAI MOSTRAR AS MARCAÇÕES
		//=================================================================================================

		private void btnMarcacoes_Click(object sender, System.EventArgs e)
		{
			//copy/paste de nao sei onde, esquisito, mas funciona...
			dgMarcacoes.CurrentPageIndex = 0;
			if (dgMarcacoes.Items.Count != 0)
			{
				dgMarcacoes.DataSource = null;
				dgMarcacoes.DataBind();
				dgMarcacoes.Controls.Clear();
				dgMarcacoes.Visible = false;
				btnMarcacoes.Text = "Ver Marcações";
			}
			else
			{
				BindGridMarcacoes();
				if (dgMarcacoes.Items.Count == 0)
					dgMarcacoes.Visible = false;
				else
				{
					dgMarcacoes.Visible = true;
					btnMarcacoes.Text = "Ocultar Marcações";
				}
			}
		}


		//=================================================================================================
		//BIND GRID MARCAÇÕES
		//=================================================================================================
		private void BindGridMarcacoes()
		{
			if(DGEmpresas.SelectedIndex <0)
			{
				lblMessage.Text ="Seleccione a empresa."; 
				return; 
			}
			string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();


		
			string strSQL = "SELECT     Empresa.nomeAbreviado AS empresa, Funcionario.nomeAbreviado AS funcionario, BRE.dtBRE AS dtBre, cast(BRE.numBRE as varchar) + '/' +  substring (  CAST(BRE.ano as varchar),3,2) AS refBRE,  bre.idBRE,                     Marcacao.refMarcacao AS refmarcacao, Marcacao.obsInternas AS obs, Marcacao.dtMarcacao, Marcacao.idMarcacao FROM Marcacao INNER JOIN BRE ON Marcacao.idBre = BRE.idBRE  INNER JOIN   Funcionario ON Marcacao.idFuncionarioTecnico = Funcionario.idFuncionario INNER JOIN Empresa ON Marcacao.idEmpresa = Empresa.idEmpresa and Empresa.idEmpresa = " + idEmpresa; 

			dgMarcacoes.DataSource = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			dgMarcacoes.DataBind();
			
		}

		//=================================================================================================
		//FILL DA DD REQUISICOES PARA ASSOCIAR À MARCAÇÃO, CASO JÁ EXISTA, EVITANTO ASSIM POSTERIOR NECESSIDADE
		//ASSOCIA LOGO UMA REQUISICAO A TODOS OS SERVIÇOS DA MARCAÇÃO. 
		//=================================================================================================
		private void bindDDRequisicao()
		{

			
			ddRequisicao.Items.Clear(); 

			string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
			DATA.RequisicaoBD requisicao = new LabMetro.DATA.RequisicaoBD(); 
			
			ddRequisicao.DataSource= requisicao.DRGetRequisicoesIncompletasByEmpresa(idEmpresa); 
			 
			ddRequisicao.DataBind(); 
			ddRequisicao.Items.Insert(0, new ListItem("","")); 
			
			requisicao = null; 
		}

		//=================================================================================================
		//ACCÇÃO QUE INSERE UMA MARCAÇÃO E UM RESPECTIVO "BRE DE CALIBRAÇÃO EXTERNA"
		//=================================================================================================
		private void fazerMarcacao()
		{

			string dtStart = txtDataProximaVisita.Text.TrimEnd();   
			string dtFim = txtDataUltDiaMarcacao.Text;
			if(dtFim=="") dtFim =  txtDataProximaVisita.Text;  

			string t1 = DateTime.Parse(dtStart).ToString(); 
 
			string t2 = DateTime.Now.ToString(dtFim); 
 
			if (DateTime.Compare(DateTime.Parse (t1), DateTime.Parse (t2)) > 0 ) 
 
			{
				lblMessage.Text ="A data de fim de ser superior à data de início da marcação"; 
				return; 
 			}

            if (dgContactos.SelectedIndex < 0)
            {
                lblMessage.Text = "Tem de esolher um contacto (com fax ou email, conforme necessário.)";
                return;
            }  

			string idEmpresa; 
			string idsEquipsCalibracao = null; 
			string idsEquipsEnsaio = null; 
			string idsEquipsVerificacao = null; 
			string idFuncionario =""; 

			//DATA.GeralBD geral = new LabMetro.DATA.GeralBD();
			try
			{
                idFuncionario = System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));   
			}
			catch
			{}

			//geral = null; 
			
			string idRequisicao; 
			int idBRE; 

			idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
			if (idEmpresa=="") 
			{
				lblMessage.Text ="Seleccione a empresa."; 
				return; 
			}

			idRequisicao = ddRequisicao.SelectedValue; 
			
			//int nServicos = 0; 
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
			{ 
				CheckBox cbc =(CheckBox)dgi.Cells[0].FindControl("cbCalibracao"); 
				CheckBox cbe =(CheckBox)dgi.Cells[0].FindControl("cbEnsaio"); 
				CheckBox cbv =(CheckBox)dgi.Cells[0].FindControl("cbVerificacao"); 

				
				if(cbc.Checked == true)
				{
					//nServicos = 1 ; 
					idsEquipsCalibracao+= DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
					idsEquipsCalibracao+=",";
				}   
				if(cbe.Checked == true)
				{
					//nServicos = 1 ; 
					idsEquipsEnsaio+= DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
					idsEquipsEnsaio+=",";
				}   
				if(cbv.Checked == true)
				{
					//nServicos = 1 ; 
					idsEquipsVerificacao+= DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
					idsEquipsVerificacao+=",";
				}   
			}
	
//			if(nServicos == 0)
//			{
//				lblMessage.Text ="Tem de seleccionar pelo menos um equipamento.";
//				return; 
//			}
			
			if(idsEquipsCalibracao != null) idsEquipsCalibracao = idsEquipsCalibracao.TrimEnd(",".ToCharArray());
			if(idsEquipsEnsaio != null) idsEquipsEnsaio = idsEquipsEnsaio.TrimEnd(",".ToCharArray());
			if(idsEquipsVerificacao != null) idsEquipsVerificacao = idsEquipsVerificacao.TrimEnd(",".ToCharArray());
			
			//*****************************************************************
			//=================================================================
			//passar queries para stored procedures depois!
			//=================================================================
			//*****************************************************************

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
						objCmd.CommandType = CommandType.StoredProcedure;
						DATA.BreBD bre = new DATA.BreBD(); 

						idBRE = bre.InsertBreConnn(objConn,objCmd,idEmpresa,idFuncionario,"0",ddTecnicoExterior.SelectedItem.Text,txtObservacoes.Text,HttpContext.Current.User.Identity.Name.ToString(),"0","","","","","","1",""); //tirar o ADMIN vai passar a haver um campo com idFuncionario no BRE que vai ser usado em vez do entregue por			IDfuncionario fica faxassociado à marcacao. 

			
						objCmd.CommandType = CommandType.Text;
						
						//NÃO VALE A PENA IR POR HORAS, PQ OS CALENDÁRIOS DPS USAM AS HORAS DA NOITE TB....
						//SEM HORA, A DATA DO INICIO SO CONTA ATÉ O PRINCIPIO DO ULTIMO DIA (E LOGO NAO INCLUI ESSE							DIA).
						//TODO ESSE TRATAMENTE VAI SER FEITO AGORA MANUALMENTA NA PAGINA QUE MOSTRA A AGENDA DA								SEMANA....
						
						
						SqlParameter param = new SqlParameter("@dtFim", dtFim); 
						SqlParameter param2 = new SqlParameter("@dtStart", dtStart); 
						SqlParameter param3 = new SqlParameter("@idRequisicao", idRequisicao); 
						SqlParameter param4 = new SqlParameter("@idGrandeza", ddGrandezaFax.SelectedValue);
                        SqlParameter param5 = new SqlParameter();
                        param5.ParameterName = "@idContacto"; 

                       
                        
                        try //se tem contacto escolhido
                        {   

                            param5.Value= dgContactos.DataKeys[dgContactos.SelectedIndex].ToString();
                        }
                        catch
                        {
                            param5.Value = DBNull.Value;

                        }
                        
                        

						if(param3.Value.ToString()=="") param3.Value = DBNull.Value;
						if(param4.Value.ToString()=="") param4.Value = DBNull.Value;

						objCmd.CommandText = "INSERT INTO Marcacao([idEmpresa],[dtMarcacao],[idBre],[refMarcacao],[idUtilCriacao],[dtCriacao],[idUtilAlteracao],[dtAlteracao],[obsInternas],[idFuncionarioTecnico], dtFimMarcacao, idRequisicao, idGrandeza,bGeral, idContacto)VALUES ("+idEmpresa+",@dtStart,"+idBRE.ToString()+",null,"+Session["UserId"].ToString()+",getDate(),"+Session["UserId"].ToString() +", getDate(),'"+txtObservacoes.Text+"',"+ddTecnicoExterior.SelectedValue.ToString()+",@dtFim,@idRequisicao,@idGrandeza,0,@idContacto)";  

						objCmd.Parameters.Add(param); 
						objCmd.Parameters.Add(param2); 
						objCmd.Parameters.Add(param3);
						objCmd.Parameters.Add(param4);
                        objCmd.Parameters.Add(param5);

           				objCmd.ExecuteNonQuery();

						objCmd.CommandType = CommandType.StoredProcedure;
						
						//******************************************************************
						//passar para funcao DEPOIS, para classe propria (ConcessionariosBD)
						//com mais tempo.
						//******************************************************************
						
						char[] delimiter = ",".ToCharArray();

						if(idsEquipsCalibracao!=null)
						{
							
							idsEquipsCalibracao = idsEquipsCalibracao.TrimEnd(delimiter);
							string[] idsC = idsEquipsCalibracao.Split(delimiter); 

							foreach(string idEquipamento in idsC)
							{
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
								SqlParameter[] arrParams = new SqlParameter[11];

								arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
								arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao); //passar idRequisicao, se existe
								arrParams[2] = new SqlParameter("@inIdEquipamento", idEquipamento);
								arrParams[3] = new SqlParameter("@inIdEstadoServico", "2");//aguarda calib.ext.
								//arrParams[4] = new SqlParameter("@inIdLocalCalibracao", ddLocalCalibracao.SelectedValue);
								arrParams[4] = new SqlParameter("@inIdTipoServico","C" );//aqui é Calibracao
								arrParams[5] = new SqlParameter("@inValor", "0");
								arrParams[6] = new SqlParameter("@inPercDesconto","0");
								arrParams[7] = new SqlParameter("@inValorFinal","0");
								arrParams[8] = new SqlParameter("@inCalibracaoExterna", "1");//sempre
								
								arrParams[9] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
								arrParams[10] = new SqlParameter("@inBDefinitivo","0");


								objCmd.CommandText = "stpInsServico"; 
                        
								foreach (SqlParameter p in arrParams)
								{
									//check for derived output value with no value assigned
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
				
									objCmd.Parameters.Add(p);
								}

								objCmd.ExecuteNonQuery();
							}
						}


						if(idsEquipsVerificacao!=null)
						{
							
							idsEquipsVerificacao = idsEquipsVerificacao.TrimEnd(delimiter);
							string[] idsV = idsEquipsVerificacao.Split(delimiter); 

							foreach(string idEquipamento in idsV)
							{
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();
                                SqlParameter[] arrParams = new SqlParameter[11];

                                arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
                                arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao); //passar idRequisicao, se existe
                                arrParams[2] = new SqlParameter("@inIdEquipamento", idEquipamento);
                                arrParams[3] = new SqlParameter("@inIdEstadoServico", "2");//aguarda calib.ext.
                                //arrParams[4] = new SqlParameter("@inIdLocalCalibracao", ddLocalCalibracao.SelectedValue);
                                arrParams[4] = new SqlParameter("@inIdTipoServico", "V");//aqui é Verificacao
                                arrParams[5] = new SqlParameter("@inValor", "0");
                                arrParams[6] = new SqlParameter("@inPercDesconto", "0");
                                arrParams[7] = new SqlParameter("@inValorFinal", "0");
                                arrParams[8] = new SqlParameter("@inCalibracaoExterna", "1");//sempre

                                arrParams[9] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
                                arrParams[10] = new SqlParameter("@inBDefinitivo", "0");

								objCmd.CommandText = "stpInsServico"; 
                        
								foreach (SqlParameter p in arrParams)
								{
									//check for derived output value with no value assigned
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
				
									objCmd.Parameters.Add(p);
								}

								objCmd.ExecuteNonQuery();
							}
						}

						if(idsEquipsEnsaio!=null)
						{
							idsEquipsEnsaio = idsEquipsEnsaio.TrimEnd(delimiter);
							string[] idsE = idsEquipsEnsaio.Split(delimiter); 

							foreach(string idEquipamento in idsE)
							{
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();
                                SqlParameter[] arrParams = new SqlParameter[11];

                                arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
                                arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao); //passar idRequisicao, se existe
                                arrParams[2] = new SqlParameter("@inIdEquipamento", idEquipamento);
                                arrParams[3] = new SqlParameter("@inIdEstadoServico", "2");//aguarda calib.ext.
                                //arrParams[4] = new SqlParameter("@inIdLocalCalibracao", ddLocalCalibracao.SelectedValue);
                                arrParams[4] = new SqlParameter("@inIdTipoServico", "E");//aqui é Ensaio
                                arrParams[5] = new SqlParameter("@inValor", "0");
                                arrParams[6] = new SqlParameter("@inPercDesconto", "0");
                                arrParams[7] = new SqlParameter("@inValorFinal", "0");
                                arrParams[8] = new SqlParameter("@inCalibracaoExterna", "1");//sempre

                                arrParams[9] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
                                arrParams[10] = new SqlParameter("@inBDefinitivo", "0");

								objCmd.CommandText = "stpInsServico"; 
                        
								foreach (SqlParameter p in arrParams)
								{
									//check for derived output value with no value assigned
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
									objCmd.Parameters.Add(p);
								}	

								objCmd.ExecuteNonQuery();
							}
						}
						objCmd.CommandType = CommandType.Text;

						objCmd.CommandText ="UPDATE EMPRESA	set dtUltimaVisita = '"+txtDataProximaVisita.Text+"' WHERE idEmpresa =" +idEmpresa; 

						objCmd.ExecuteNonQuery();
						//int i = objCmd.ExecuteNonQuery();
						//Response.Write(i.ToString()); 

						objTrans.Commit();

                        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_CONFIRMACAO_MARCACAO;
						BindGridEmpresas();//para actualizar data da ultima marcacao

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

		private void btnFazerMarcacao_Click(object sender, System.EventArgs e)
		{
			fazerMarcacao(); 
		}

		private void dgGenerico_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void dgContactos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		//======================================================================================
		//preenche a dropdown com os funcionarios marcados como sendo de "calibracao externa".
		//======================================================================================
		private void fillDDfuncionarios()

		{
			string strSQL = "select idFuncionario, nomeAbreviado from funcionario where bCta = 1 and activo = 1 ";
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddTecnicoExterior.DataSource = DR;
			ddTecnicoExterior.DataBind();
            DR.Close();
		}

		private void fillDDLocalCalibracao()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
			
			ddLocalCalibracao.DataSource = lista.DRListaLocalCalibracao();
			ddLocalCalibracao.DataBind(); 
			
			lista = null; 
		}

		private void fillDDs()
		{
			fillDDfuncionarios();
			fillDDLocalCalibracao(); 

		}

		private void btnActualizarDadosEmpresaEquip_Click(object sender, System.EventArgs e)
		{
			alterarDatasEquipamentos(); 
		
		}


		//*************************************************************************************************
		//INÍCIO REQUISIÇÕES
		//*************************************************************************************************


		//=================================================================================================
		//BOTAO QUE VAI MOSTRAR AS REQUISICOES
		//=================================================================================================
		private void btnRequisicao_Click(object sender, System.EventArgs e)
		{
			
			dgRequisicoes.CurrentPageIndex = 0;
			if (dgRequisicoes.Items.Count != 0)
			{
				dgRequisicoes.DataSource = null;
				dgRequisicoes.DataBind();
				dgRequisicoes.Controls.Clear();
				dgRequisicoes.Visible = false;
				btnRequisicao.Text = "Ver Requisições";
			}
			else
			{
				BindGridRequisicoes();
				if (dgRequisicoes.Items.Count == 0)
					dgRequisicoes.Visible = false;
				else
				{
					dgRequisicoes.Visible = true;
					btnRequisicao.Text = "Ocultar Requisições";
				}
			}
		}

		//=================================================================================================
		//BIND GRID DAS REQUISIÇÕES
		//=================================================================================================
		private void BindGridRequisicoes()
		{   
			if(DGEmpresas.SelectedIndex <0)
			{
				lblMessage.Text ="Seleccione a empresa."; 
				return; 
			}
			string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
			
			DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD(); 
			DataTable DT =  req.DTListaRequisicoes(idEmpresa, "","","",false,false); 
			DataView DV = new DataView(DT);
			DV.Sort = ViewState["sortFieldReq"].ToString()+ " " + ViewState["sortDirectionReq"]; 

			dgRequisicoes.DataSource = DV; 
			dgRequisicoes.DataBind();

			req = null; 
		}

		//=================================================================================================
		//PAGING DAS REQUISICOES
		//=================================================================================================
		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			dgRequisicoes.CurrentPageIndex = e.NewPageIndex;
			BindGridRequisicoes(); 

		}   

		//=================================================================================================
		//SORTGRID DAS REQUISICOES
		//=================================================================================================
		public void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionReq"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionReq"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionReq"]="ASC";
					break;
			}

			ViewState["sortFieldReq"] = e.SortExpression;
	
			BindGridRequisicoes(); 

		}

		//marcar requisicoes como completas
		//=================================================================================================
		//=================================================================================================
		protected void cb_SetComplete(object sender, EventArgs e)
		{
			//encontrar a dropdown
			CheckBox cbSender = (CheckBox)sender;
			DataGridItem dgi = (DataGridItem) cbSender.Parent.Parent;
			
			string idRequisicao = dgRequisicoes.DataKeys[dgi.ItemIndex].ToString();  
			//Response.Write(idRequisicao); 
			DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD(); 
			req.UpdateRequisicaoCompleta(idRequisicao, cbSender.Checked.ToString(),User.Identity.Name.ToString());
		}

		//*******************************************************************************
		//*************** FUNÇÕES AUXILIARES ********************************************
		//*******************************************************************************

		//===================================================================
		//PARA DESENHAR A CÔR DO ESTADO DA EMPRESA(BRANCO, AMARELO, LARANJA, VERMLHO)
		//===================================================================
		protected System.Drawing.Color ConvertColor(int i, string codigoBloqueioSAP)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();
		
			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Gold");
					break;
				case 2:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("DarkOrange");
					break;
				case 3: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Crimson");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}

			if(codigoBloqueioSAP =="03") //se a empresa está inactiva em SAP (n tem a ver com o nivelBloqueiolabmetro, martelada...)
			{
				colorName = (System.Drawing.Color)colConvert.ConvertFromString("PowderBlue");
			}
			return colorName; 
		}


		//===================================================================
		//COR DO ACTIVO (BRANCO/VERMELHO)
		//===================================================================
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


		//=================================================================================================
		//=================================================================================================
		protected string ConverteEstado(bool b)
		{
			if (b==true) return "sim";
			else return "não"; 
		}

		//=================================================================================================
		// DEVOLVE O CAMINHO PARA UMA REQUISICAO
		//=================================================================================================
		public string downloadpath(object filename)
		{
			if(filename!=null && filename.ToString()!="")
			{
				string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_URL"];
                
				myPath = myPath + "/" + filename.ToString(); 
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}

		


		private void btnA_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
			{ 				
				CheckBox cbA =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				
				bool b = cbA.Checked; 
				cbA.Checked = !b; 
			}  
		}

		private void bntC_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
			{ 				
				CheckBox cbA =(CheckBox)dgi.Cells[0].FindControl("cbCalibracao"); 
				
				bool b = cbA.Checked; 
				cbA.Checked = !b; 
			}  
		
		}

		private void bntE_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
			{ 				
				CheckBox cbA =(CheckBox)dgi.Cells[0].FindControl("cbEnsaio"); 
				
				bool b = cbA.Checked; 
				cbA.Checked = !b; 
			}  
		
		}

		protected void btnFiltrarEquips_Click(object sender, System.EventArgs e)
		{
			BindGridEquipamentos(); 
			DGEquipamentos.Visible = true;
		}

		protected void btnV_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
		 { 				
			 CheckBox cbV =(CheckBox)dgi.Cells[0].FindControl("cbVerificacao"); 
				
			 bool b = cbV.Checked; 
			 cbV.Checked = !b; 
		 }  
		
		}

		private void Checkbox1_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

	
	}
}
