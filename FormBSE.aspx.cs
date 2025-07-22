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
using LabMetro.GERAL;
using LabMetro.REPORTS;
using System.Data.SqlClient; 
using System.Configuration;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;



namespace LabMetro
{
	public partial class FormBSE : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
	
		DataView DVOrigem; 
		DataTable DTOrigem; 

		DataView DVDestino; 
		DataTable DTDestino;
		
		private const string ID_PAG = "BSE_1";
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text = ""; 
            
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
					int intAcesso = System.Convert.ToInt32(ht[ID_PAG]); 
				
					if(intAcesso ==0) 
					{
						btnSubmit.Enabled=false;

					}
					if(!Page.IsPostBack)
					{ 
						FillDropDowns(); 
				      
						if(Request.QueryString["id"]!=null)
						{
							if(Request.QueryString["id"]!="")
							{
								ViewState["idBSE"] =  Request.QueryString["id"].ToString();								
								fillForm(ViewState["idBSE"].ToString());

                                createDTServicos();      	
							}
                            
							if(Request.QueryString["errUpd"]!=null &&Request.QueryString["errUpd"]!="")  
							{
								string retValue = Request.QueryString["errUpd"].ToString();

                                if (retValue.ToString() == "0")	//atenção q este é diferente, retorna 0 se correu mal.
								{
									lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
								}
								else
								{
									lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
								}
							}
							btnSubmit.CommandArgument="update"; 
							btnVerBSE.Enabled=true;
							//btnZip.Enabled=true;   
						}
						else
						{
							btnSubmit.CommandArgument="insert"; 
							btnVerBSE.Enabled=false; 
							//btnZip.Enabled=false;    
                            
						}	
					}  
					fillCompanyInfo();
				}
			}    
		}

        private void fillddBRE()
        {
            DATA.BreBD bre = new LabMetro.DATA.BreBD();
            SqlDataReader dr = bre.DRGetBreByIdEmpresa(ddEmpresa.SelectedValue);
            ddBRE.DataSource = dr;
            ddBRE.DataBind();

            bre = null;

            ddBRE.Items.Insert(0, new System.Web.UI.WebControls.ListItem("",""));

        }

        private void createDTServicos()
        { 
        
            //cria já a datasource destino e guarda em viewstate
			DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD();
			DTDestino = servicos.DTGetServicoByBSE(Request.QueryString["id"].ToString()); 
			DataColumn[] dc_Pk = new DataColumn[1];
			dc_Pk[0] = DTDestino.Columns["idServico"];
			DTDestino.PrimaryKey = dc_Pk;
                                
			ViewState["DTDestino"] = DTDestino;
			dgDestino.DataSource= DTDestino; 
			dgDestino.DataBind(); 

			servicos = null; 
        }

		private void btnSelectAll_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in dgOrigem.Items) 
			{ 
				CheckBox chb = (CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				chb.Checked=true; 
			}
		}

		private void btnDeselectAll_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in dgOrigem.Items) 
			{ 
				CheckBox chb =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				chb.Checked=false; 
			}
		}


		private void CreateDataSources()
		{
            
			//carrega da primeira vez os servicos disponiveis por empresa para BSE
			DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD();
			DataTable DTOrigem = servicos.DTServicosForBSE(ddEmpresa.SelectedValue.ToString(),ddBRE.SelectedItem.Text,ddFuncionarioSaida.SelectedValue);

			DataColumn[] dcPk = new DataColumn[1];
			dcPk[0] = DTOrigem.Columns["idServico"];
			DTOrigem.PrimaryKey = dcPk;

			//preciso para ficar coerente
			DTOrigem.Columns.Add(new DataColumn("nomeDocumento",typeof(string))); 
			DTOrigem.Columns["nomeDocumento"].DefaultValue = ""; 
			
			DVOrigem = new DataView(DTOrigem); 
			DVOrigem.Sort ="refServico ASC"; 

			dgOrigem.DataSource = DVOrigem; 
			dgOrigem.DataBind(); 
           
			if(ViewState["DTDestino"] == null)
			{
				DTDestino = new DataTable(); 
				DTDestino = DTOrigem.Clone(); 
				
				ViewState["DTDestino"] = DTDestino;
			}

			dgOrigem.DataSource = DTOrigem; 
			dgDestino.DataSource = DTDestino; 
            
		}
		private void BindGridSource()
		{
			DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD();
			DataTable DTOrigem = servicos.DTServicosForBSE(ddEmpresa.SelectedValue.ToString(),ddBRE.SelectedItem.Text,ddFuncionarioSaida.SelectedValue); 
			DVOrigem = new DataView(DTOrigem); 
			string strSort = "refServico ASC"; 
			DVOrigem.Sort = strSort; 
			dgOrigem.DataSource = DVOrigem; 
			dgOrigem.DataBind(); 
        
			servicos = null; 
		}

		private void BindGridDestino()
		{

			DTDestino= (DataTable)ViewState["DTDestino"]; 
			DVDestino = new DataView(DTDestino); 
			string strSort = "refServico ASC"; 
			DVDestino.Sort = strSort; 
			dgDestino.DataSource= DVDestino; 
			dgDestino.DataBind();
		}

		private void dgDestino_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
		{
			DTDestino= (DataTable)ViewState["DTDestino"]; 
			DVDestino = new DataView(DTDestino); 
			DataRowView drv = DVDestino[e.Item.ItemIndex]; 
 
			//APAGA DO DESTINO 
			string id = drv["idServico"].ToString(); 
			DataRow dr = DTDestino.Rows.Find(id); 
			dr.Delete(); 

			ViewState["DTDestino"] = DTDestino; 
            
			BindGridDestino(); 
		}

		

		private void AddLinesToDestino()
		{
            
			DTDestino= (DataTable)ViewState["DTDestino"]; 

			//volto a carregar da bd========================
			DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD();
			DTOrigem = servicos.DTServicosForBSE(ddEmpresa.SelectedValue.ToString(),ddBRE.SelectedItem.Text,ddFuncionarioSaida.SelectedValue);

			DataColumn[] dcPk = new DataColumn[1];
			dcPk[0] = DTOrigem.Columns["idServico"];
			DTOrigem.PrimaryKey = dcPk;
			//=============================================

			string strIds =""; 
			foreach(DataGridItem dgi in dgOrigem.Items) 
			{ 
				CheckBox myCheckBox =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				if(myCheckBox.Checked == true)
				{
					strIds+= dgOrigem.DataKeys[dgi.ItemIndex].ToString();
					strIds+=",";
				}   
			}

			strIds = strIds.TrimEnd(",".ToCharArray()); 

			if(strIds != "")
			{
				string strSearch = "idServico IN (" + strIds + ")"; 
			
				DataRow[] foundRows = DTOrigem.Select(strSearch); 
                
				foreach(DataRow dr in foundRows)
				{        
					if(!DTDestino.Rows.Contains(dr["idServico"]))
					{
                        
						DataRow DR; 
						DR = DTDestino.NewRow(); 
						DR["idServico"] = dr["idServico"].ToString(); 
						DR["idBRE"] = dr["idBRE"].ToString(); 
						DR["refBRE"] = dr["refBRE"].ToString(); 
						DR["idRequisicao"] = dr["idRequisicao"].ToString(); 
						DR["refRequisicao"] = dr["refRequisicao"].ToString(); 
						DR["codTipoEquipamento"] = dr["codTipoEquipamento"].ToString(); 
						string idEstadoServico = dr["idEstadoServico"].ToString(); 


						switch(idEstadoServico)
						{
							case "6": //Calibrado
								DR["idEstadoServico"] = "15"; 
								DR["estadoServico"] = "Entregue Calibrado"; 
								break; 
							case "10": //subcontratação calibrada 
								DR["idEstadoServico"] = "24"; 
								DR["estadoServico"] = "Entregue Sub.Calibrada"; 
								break; 
							case "18": //subcontratação com certificado
								DR["idEstadoServico"] = "14"; 
								DR["estadoServico"] = "Entregue c/ Certif."; 
								break; 
							case "19": //aguarda entrega
								DR["idEstadoServico"] = "14"; 
								DR["estadoServico"] = "Entregue c/ Certif."; 
								break; 
							case "21"://equipamento anulado na recepcao
								DR["idEstadoServico"] = "22"; 
								DR["estadoServico"] = "Entregue Anulado"; 
								break;
                            case "23"://Calib. c/Cert. e Equip.no Cliente --so para angola, nos outros paises este estado nao deve aparecer para dar saida pois nao consta da tabela dos equipamentos para dar saída.
                                DR["idEstadoServico"] = "16";
                                DR["estadoServico"] = "Certificado entregue em mão";
                                break; 
						}
						

						DR["idLocalCalibracao"] = dr["idLocalCalibracao"].ToString(); 
						DR["localCalibracao"] = dr["localCalibracao"].ToString();  
						DR["refServico"] = dr["refServico"].ToString(); 
						DR["observacoes"] = dr["observacoes"].ToString(); 
						DR["nomeDocumento"] = ""; //dr["nomeDocumento"].ToString(); --aqui nao existe
						DR["expedicao"] = dr["expedicao"];
                        DR["acessorios"] = dr["acessorios"];
                        DR["bVariasGrandezas"] = dr["bVariasGrandezas"];

						DTDestino.Rows.Add(DR); 
 
					}
				}
			}

            servicos = null; 
			ViewState["DTDestino"] = DTDestino; 	  
		}
        
		private void DGDestino_ItemDataBound(object sender, DataGridItemEventArgs e)
		{

			DataRowView drv = (DataRowView) e.Item	.DataItem;
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoServicoEdit");
				DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
				SqlDataReader DR = servico.DRGetListaEstadosServico(dgDestino.DataKeys[e.Item.ItemIndex].ToString()); 
				ddEstado.DataSource = DR; 
				ddEstado.DataBind(); 
				DR.Close(); 

				servico = null; 
				ddEstado.SelectedValue = drv["idEstadoServico"].ToString(); 
			}  
		}

		private void fillDDEmpresa()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT = empresa.DTGetEmpresasForBSE(txtPesquisaEmpresa.Text, txtPesquisaNif.Text); 

			DataView DV = new DataView(DT);
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind(); 
			
			empresa = null;
		
			fillCompanyInfo();
            if (Request.QueryString["id"] == null)
            {
                fillddBRE();
            }

		}

		private void FillDropDowns()
		{

			DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
			SqlDataReader DR2 = lista.DRListaFuncionarios(); 
			ddFuncionarioSaida.DataSource = DR2; 
			ddFuncionarioSaida.DataBind(); 
			DR2.Close(); 

			lista = null; 
             
			//DATA.GeralBD geral = new LabMetro.DATA.GeralBD();
			if(!Page.IsPostBack)

				try
				{
                    ddFuncionarioSaida.SelectedValue = System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));        
				}
				catch
				{
					//nada
				}

			//geral = null; 

		}

		private void fillForm(string id)
		{
			LabMetro.DATA.BseBD fillBSE = new LabMetro.DATA.BseBD(); 
			LabMetro.DATA.BseDetails det = fillBSE.GetBseDetails(id); 
			if(det!= null)
			{  
				txtDtBSE.Text =  GERAL.clsGeral.ToShortDate(det.dtBSE);
				txtObservacoes.Text = det.observacoes;
				txtRecebidoPor.Text = det.recebidoPor;
				txtRefBSE.Text = det.refBSE;


                txtRefOrcamento.Text = det.refOrcamento;
                txtRefRequisicao.Text = det.refRequisicao;


				txtRefBSE.Enabled=false; 
				try
				{
                //    ddEmpresa.SelectedValue = det.idEmpresa;
                //}
                //catch
				//{
					ddEmpresa.Items.Insert(0,new System.Web.UI.WebControls.ListItem(det.nomeEmpresa,det.idEmpresa)); 
					ddEmpresa.SelectedValue=det.idEmpresa;

                    fillddBRE();
           
				}
                    
                catch{}; 
                
				ddEmpresa.Enabled=false; 
				txtPesquisaEmpresa.Enabled=false;
				txtPesquisaNif.Enabled = false;
				btnEmpresas.Enabled=false; 

				lblFuncionarioSaida.Text = det.nomeFuncionarioSaida;
			}

			fillBSE = null; 
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
		private void InitializeComponent2()
		{    
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
			btnSubmitGrid.Click += new System.EventHandler(btnSubmitGrid_Click);
			dgDestino.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(dgDestino_DeleteCommand);
			dgDestino.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(DGDestino_ItemDataBound);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnVerBSE.Click += new System.EventHandler(btnVerBSE_Click);
			Load += new System.EventHandler(Page_Load);
			btnSelectAll.Click +=new  System.EventHandler(btnSelectAll_Click); 
			btnDeselectAll.Click +=new  System.EventHandler(btnDeselectAll_Click); 
			pesquisaPorBRE.Click += new System.EventHandler(pesquisaPorBRE_Click);
			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
			//btnZip.Click += new System.EventHandler(btnZip_Click);
		}

		private void InitializeComponent()
		{  

		} 

		#endregion
        
       
		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ViewState["DTDestino"] = null; //limpar a viewstate
			dgOrigem.Controls.Clear(); dgOrigem = null; 
			dgDestino.Controls.Clear(); 
			dgDestino = null;

            if (Request.QueryString["id"] == null)
            {
                fillddBRE();
            }
		}

		protected void editGrid(Object sender,DataGridCommandEventArgs e)     
		{
			dgDestino.EditItemIndex = e.Item.ItemIndex;    
			BindGridDestino();
		}

		protected void cancelGrid(Object sender,   DataGridCommandEventArgs e)
		{
			dgDestino.EditItemIndex = -1;
			BindGridDestino();  
		}
		
		protected void dg_itemCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Select")
			{

				LinkButton button = (LinkButton)e.Item.Cells[e.Item.Cells.Count-1].Controls[0];
				string fName = button.Text;
				//é um campo invisivel pq nao consigo aceder ao valor q está dentro de uma templatecolumn

				string fPath = (string) ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CERTIFICADOS"].ToString();
				string filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + fPath + "/" + fName);
				//clsWriteError.WriteLog(filePath);
				try
				{
					if (File.Exists(filePath))
					{
						Response.Clear();
						Response.Buffer = true;
						//' x-mxdownload forces automatic download no matter what the content type

						Response.ContentType = "Application/x-msdownload";
						Response.ContentEncoding = System.Text.Encoding.Default;
						//força o dialog "save as"; 
						Response.AddHeader("content-disposition", "attachment; filename=" + fName);

						Response.WriteFile(filePath);
						HttpContext.Current.Response.End();

					}
				}
				catch 
				{

				}
			}
		
		}
       
		protected void updateGrid(Object sender,DataGridCommandEventArgs e)
		{
			string id =	dgDestino.DataKeys[e.Item.ItemIndex].ToString();
			
			TextBox txtObs = (TextBox)e.Item.FindControl("txtObservacoesEdit"); 
			DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoServicoEdit");
            
			DTDestino= (DataTable)ViewState["DTDestino"]; 
			DVDestino = new DataView(DTDestino); 

			DVDestino.RowFilter = "idServico=" + id ;
            
			if(DVDestino.Count > 0) 
			{    
				DVDestino[0]["observacoes"] = txtObs.Text; 
				DVDestino[0]["idEstadoServico"] = ddEstado.SelectedValue.ToString(); 
				DVDestino[0]["estadoServico"] = ddEstado.SelectedItem.Text.ToString(); 
			}

			DVDestino.RowFilter=""; 
			
			ViewState["DTDestino"] = DTDestino; 
            
			dgDestino.EditItemIndex = -1; 
			BindGridDestino(); 
       
		}   
        
		private void btnSubmitGrid_Click(object sender, System.EventArgs e)
		{   
			AddLinesToDestino(); 
			BindGridDestino();    
			BindGridSource(); 
		}

		private void InsertBD()
		{   
			DTDestino= (DataTable)ViewState["DTDestino"]; 
			DVDestino = new DataView(DTDestino); 

			DATA.BseBD bse = new LabMetro.DATA.BseBD(); 
            
			if(DTDestino == null)
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_MIN_1EQUIPAMENTO;
                                                                                                   
			}
			else if(DTDestino.Rows.Count == 0)
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_MIN_1EQUIPAMENTO;
			}
			else
			{
				//aqui tem de ser um idBse diferente da variavel global
				string idBse = bse.InsertBseWithServices(ddEmpresa.SelectedValue.ToString(),ddFuncionarioSaida.SelectedValue,txtRecebidoPor.Text,txtObservacoes.Text,User.Identity.Name.ToString(),DTDestino, txtRefOrcamento.Text, txtRefRequisicao.Text).ToString();
				
				bse = null; 

				if (idBse == "0")
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
				}
				else
				{
					Response.Redirect("FormBSE.aspx?btn=DOC&id=" + idBse,true);
				}
			}
		}

		private void UpdateBD()
		{   
			DTDestino= (DataTable)ViewState["DTDestino"]; 
			DTOrigem = (DataTable)ViewState["DTOrigem"]; 

			DATA.BseBD bse = new LabMetro.DATA.BseBD(); 
            
			int retValue = bse.UpdateBseWithServices(ViewState["idBSE"].ToString(),ddEmpresa.SelectedValue.ToString(),ddFuncionarioSaida.SelectedValue,txtRecebidoPor.Text,txtObservacoes.Text,User.Identity.Name.ToString(),DTDestino, txtRefOrcamento.Text,txtRefRequisicao.Text); 

			bse = null; 

			Response.Redirect("FormBSE.aspx?btn=DOC&errUpd="+retValue.ToString()+"&id="+ViewState["idBSE"].ToString(),true); 
    
		}


		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				if(btnSubmit.CommandArgument=="insert")
				{
					InsertBD();
				}
				else if(btnSubmit.CommandArgument =="update")
				{
					UpdateBD(); 
				}
			}
		}

		private void btnVerBSE_Click(object sender, System.EventArgs e)
		{

            ReportClass report = null;
            switch (myApp)
            {

                case "ANG_LABMETRO":
                    report = new LabMetro.REPORTS_ANG.crBSE();
                    break;
                case "SON_LABMETRO":
                    report = new LabMetro.REPORTS_SON.crBSE();
                    break;
                case "ISQ_LABMETRO":
                    report = new LabMetro.REPORTS.crBSE();
                    break;
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crBSE();
                    break;
                case "CV_LABMETRO":
                    report = new LabMetro.REPORTS_CV.crBSE();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.crBSE();
                    break;
                case "BR_LABMETRO":
                    report = new LabMetro.REPORTS_BR.crBSE();
                    break;


            }

           
            DATA.BseBD bse = new LabMetro.DATA.BseBD();
            DataSet ds = bse.DSBSE(ViewState["idBSE"].ToString());
            
            report.SetDataSource(ds);
            ds = null; 		
            bse = null; 
            clsReport cr = new clsReport();
			
            cr.exportReportToPDF(report,"BSE");
	
			
            //cr = null; 
            //report = null; 
	
		}

       
		private void pesquisaPorBRE_Click(object sender, System.EventArgs e)
		{
			CreateDataSources(); 
		}

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			
			ViewState["DTDestino"] = null; //limpar a viewstate
			dgOrigem.DataSource = null; 
			dgOrigem.DataBind(); 

			dgDestino.DataSource = null; 
			dgDestino.DataBind();
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa();

			ViewState["DTDestino"] = null; //limpar a viewstate
			dgOrigem.DataSource = null; 
			dgOrigem.DataBind(); 

			dgDestino.DataSource = null; 
			dgDestino.DataBind();
		}

		private void btnEmpresas_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 

			ViewState["DTDestino"] = null; //limpar a viewstate
			
			dgOrigem.DataSource = null; 
			dgOrigem.DataBind(); 

			dgDestino.DataSource = null; 
			dgDestino.DataBind(); 

			
		}
		
		// Preencher os dados relativos à empresa (se é devedora ou não)
		private void fillCompanyInfo()
		{

			lblEmpresaDevedora.Text =""; //limpar a label

			LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(ddEmpresa.SelectedValue); 
			if(detEmpresa != null)
			{
				lblObsEmpresa.Text = detEmpresa.observacoes;    //meu

				if (detEmpresa.pagamentoAtraso =="1")
				{
					lblEmpresaDevedora.Text = "** PAGAMENTOS EM ATRASO **<br />";
					
					//isto ou similar acho q é um bug na framwork e não dá para fazer
					//ddEmpresa.SelectedItem.Attributes.Add("style", "color:" + ds.Tables[0].Rows[i]["CategoryColor"].ToString () );
					//trEmpresa.BgColor = System.Drawing.ColorTranslator.FromHtml("#FF0033").ToString();
				}
				else
				{
					lblEmpresaDevedora.Text = ""; 
				}
				lblCondPagamEmpresa.Text = detEmpresa.condicoesPagamento.ToUpper(); 

				switch(detEmpresa.nivelBloqueioLabmetro)	
				{ 
					case "0": 
						trEmpresa.BgColor=""; 
						break;
					case "1":
						trEmpresa.BgColor ="Gold"; 
						break; 
					case "2": 
						trEmpresa.BgColor ="DarkOrange";
						lblEmpresaDevedora.Text +="Venda à dinheiro ou Pagamento do Atrasado.<br />"; 
						break; 
					case "3": 
						trEmpresa.BgColor ="Crimson";
						lblEmpresaDevedora.Text +="Venda à dinheiro.<br />"; 
						break; 
				}

				switch(detEmpresa.codigoBloqueioSAP)
				{
					case "00":  //nada mas tem de estar tratado
						break; 
					case "01": 
						lblEmpresaDevedora.Text += "** EMPRESA FALIDA **";
						break;
					case "02":
						lblEmpresaDevedora.Text += "** EMPRESA EM CONTENCIOSO **"; 
						break; 
					case "03":
						lblEmpresaDevedora.Text += "** Empresa com nºCliente SAP inactivo **"; 
						break; 
					default: //todos os outros bloqueados
						lblEmpresaDevedora.Text += "** EMPRESA COM BLOQUEIO EM SAP **"; 
						break; 
				}				
				
			}


			empresa = null; 
		}

        ////codigo para zipar etc //substituir pelo código que está no labmetro online!!!!
        //private void createZip(string nomeZip)
        //{
        //    ICSharpCode.SharpZipLib.Zip.FastZip z = new ICSharpCode.SharpZipLib.Zip.FastZip();
        //    z.CreateEmptyDirectories = true;
			
        //    //o caminho e nome do ficheiro zip a ser criado; ex:"C:\\test.zip"
        //    string pathzip = pastaZips +   nomeZip + ".zip";  
			
        //    //o caminho da directoria a ser zippada ex; "C:\\test\\"
        //    string pathZipDir = pastaZips+   nomeZip;  
			
        //    //primeiro parametro é outout, segundo o input, o resto nao sei...
        //    z.CreateZip(pathzip,pathZipDir,false,""); 

        //    //			if (File.Exists(pathzip))
        //    //				Response.Write("criado com sucesso");
        //    //			else
        //    //				Response.Write("zip nao criado"); 
        //    // 
        //}



        ////criar uma directoria e copiar la para dentro todos os certificados do bse. 		
        //private void CreateDir(string refBSE, string idBSE) 
        //{
        //    deleteOldFilesAndFolders(); //apagar os anteriores
        //                                        //verificar, isto nem sempre fucniona

        //    string path = pastaZips; // a pasta dos zips (todos)
        //    string subpath = @path+refBSE; // a pasta dos documentos a serem zipados

        //    try 
        //    {
				
        //        if (!Directory.Exists(subpath)) 
        //        {
        //            Directory.CreateDirectory(subpath);
        //        }
        //        else
        //        {
        //            Directory.Delete(subpath,true); //apaga 
        //            Directory.CreateDirectory(subpath); //cria de novo
        //        }
				
        //        //copiar os ficheiros para dentro deste subdirectory, 
        //        DATA.ServicoBD certifs = new LabMetro.DATA.ServicoBD();
        //        DataTable dt = certifs.DTGetCertificadosByBSE(idBSE); //nomeDirNovo = idBSE
				
        //        foreach(DataRow dr in dt.Rows)
        //        {
        //            //ir buscar o ficheiro ao sitio e COPIA_lo para a pasta criada
        //            FileInfo[] myFiles = dirInfopastaCertificados.GetFiles("" + dr[0].ToString() + "");			
					
        //            FileInfo myFile = myFiles[0]; 
        //            string myTarget = subpath+"/"+myFile.Name;
        //            myFile.CopyTo(myTarget,true); 
        //        }
        //        certifs = null; 

        //        createZip(refBSE);
        //        string pathzip = pastaZips +   refBSE + ".zip";  
        //        openZipForDownload(pathzip, refBSE); 
        //    } 

        //    catch// (Exception ex) 
        //    {
        //        //	Response.Write(ex.ToString());
        //    } 
        //}

	

        //private void openZipForDownload(string fPathtoZip,string fileName)
        //{
        //    try
        //    {
        //        if (File.Exists(fPathtoZip))
        //        {
        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.ContentType = "Application/x-msdownload";
        //            //Response.ContentEncoding = System.Text.Encoding.Default;
					
        //            Response.AddHeader("content-disposition", "attachment; filename="+fileName+".zip;");
        //            Response.WriteFile(fPathtoZip);
        //            HttpContext.Current.Response.End();	
        //        }
        //    }
        //    catch 
        //    {	
        //    }
        //}


        //private void deleteOldFilesAndFolders() //dos zips, apaga os antigos
        //{
        //    //todos os ficheiros na pasta zips (hao de ser os próprios zips)
        //    FileInfo[] files = dirInfopastaZips.GetFiles(); 
        //    foreach(FileInfo f in files)
        //    {
        //        if(DateTime.Parse(f.CreationTime.ToString()) > (DateTime.Parse(DateTime.Now.AddDays(-1).ToString())))
        //        {
        //            f.Delete(); 
        //        }
        //    }
        //    //todas as subpastas na pasta zips (hao de ser os próprios zips)
        //    DirectoryInfo[] dirInfo = dirInfopastaZips.GetDirectories(); 
        //    foreach(DirectoryInfo d in dirInfo)
        //    {
        //        if(DateTime.Parse(d.CreationTime.ToString()) > (DateTime.Parse(DateTime.Now.AddDays(-1).ToString())))
        //        {
						
        //            d.Delete(true);
        //        }
        //    }
        //}

        //private void btnZip_Click(object sender, System.EventArgs e)
        //{
        //    string refBSE = txtRefBSE.Text.Replace("/","_"); 
        //    string idBSE = ViewState["idBSE"].ToString(); 
        //    CreateDir(refBSE, idBSE); //vai criar a dir e depois vai chamar o create zip
        //}

        protected void ddBRE_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddBRE.SelectedValue != "")
            {
                string strSQL = "Select o.refOrcamento, bre.referenciaRequisicao from bre left join orcamento o on bre.idOrcamento = o.idOrcamento where bre.idBRE = " + ddBRE.SelectedValue;

                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        txtRefOrcamento.Text = dr["refOrcamento"].ToString();
                        txtRefRequisicao.Text = dr["referenciaRequisicao"].ToString();
                    }
                }
                dr.Close();
            }
        }
	}
}
