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
using LabMetro.GERAL;
using LabMetro.REPORTS;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;


namespace LabMetro
{
	/// <summary>
	/// Summary description for FormBRECalibExt.
	/// </summary>
	public partial class FormBRECalibExt : System.Web.UI.Page
	{
		public const string ID_PAG = "BRE_CE_1";
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();


		DataTable DTRequisicoes;
		DataTable DT; //datatable q contem os serviços
		DataView DV;  //dataview sobre a datatable dos serviços
		DataTable DTEquipamentos;

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
			btnInserirServicos.Click += new System.EventHandler(btnInserirServicos_Click);
			btnPesquisaEquipamentoEmpresa.Click += new System.EventHandler(btnPesquisaEquipamentoEmpresa_Click);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnVerBRE.Click += new System.EventHandler(btnVerBRE_Click);
			btnVerEtiquetas.Click += new System.EventHandler(btnVerEtiquetas_Click);
			DGServicosBRE.ItemDataBound +=  new DataGridItemEventHandler(DGServicosBRE_ItemDataBound); 
			DGServicosBRE.DeleteCommand += new  DataGridCommandEventHandler(DGServicosBRE_DeleteCommand);
			DGEquipamentos.ItemDataBound +=  new DataGridItemEventHandler(DGEquipamentos_ItemDataBound); 
			DGEquipamentos.ItemCommand +=  new DataGridCommandEventHandler(DGEquipamentos_ItemCommand);     
			//DGRequisicoes.ItemCommand += new DataGridCommandEventHandler(DGRequisicoes_ItemCommand); 
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            ddEmpresaContratante.SelectedIndexChanged += new System.EventHandler(ddEmpresaContratante_SelectedIndexChanged);

            txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
			btnListaEquipamentos.Click += new System.EventHandler(btnListaEquipamentos_Click);
			btnGravar.Click += new System.EventHandler(btnSubmit_Click); //chama a mesma accao que o btnSubmit_Click
			DGRequisicoes.SelectedIndexChanged += new System.EventHandler(DGRequisicoes_SelectedIndexChanged);
		}

		#endregion
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Response.Expires = 0; 	//nao tirar!!!
			lblMessage.Text =""; 

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

					if(!Page.IsPostBack) //sempre e para todos os casos 
					{
						FillDropDownsCabecalho(); //Funcionarios, Tipos de Serviço, Local de Execução
					}

					//****************************************************************************
					//FORM PREENCHIDO*************************************************************
					//****************************************************************************
					if(Request.QueryString["id"]!=null && Request.QueryString["id"]!="")           
					{
						//*********** viewstates********************************************
						if(!Page.IsPostBack)  //independente de estar preenchido ou nao
						{
							ViewState["sortDirection"]="DESC"; 
							ViewState["sortField"]="idEquipamento"; 
							ViewState["selIndex"] =""; 
							fillForm();     //ja inclui o bindgridservices                
							fillCompanyInfo();
							fillDDTipoEquipamentoPesquisa();
							BindGridRequisicoes(); //cria datagrid das requisições
						}

						btnSubmit.CommandArgument="update"; 
						btnVerBRE.Enabled=true; 
						btnVerEtiquetas.Enabled = true; 
						                
					}   
						
						//****************************************************************************
						//FORM VAZIO*************************************************************
						//****************************************************************************
					else 
					{
						if(!Page.IsPostBack)
						{
							ViewState["idBRE"] = ""; 
						}
						btnSubmit.CommandArgument="insert"; 
						btnInserirServicos.Enabled=false;
						btnVerBRE.Enabled=false; 
						btnVerEtiquetas.Enabled = false; 
						btnPesquisaEquipamentoEmpresa.Enabled=false;
					} 
				}
			}
		}
		private void fillDDEmpresa()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,txtPesquisaNif.Text,"1","","","","","",""); //activas
			DataView DV = new DataView(DT);		
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();
			empresa = null; 
			
			try //... marteladas posteriores
			{
				if(ViewState["idEmpresa"]!= null)
				{
					if(ViewState["idEmpresa"].ToString()!= "")
					{
						ddEmpresa.SelectedValue = ViewState["idEmpresa"].ToString(); 
					}
				}
			}
			catch
			{	
			}

			if(Page.IsPostBack)//Para limpar tudo que possa estar associado à uma empresa seleccionada anteriormente
			{
				clearCompanyRelatedData(); 
			}

		}

		private void clearCompanyRelatedData() //pq a empresa pode mudar agora embora nao mude o selectedIndex da dropdown.
		{
		
			// Preencher os dados relativos à empresa (se é devedora ou não)
			fillCompanyInfo();
			//se houver mais coisas a limpar, é aqui.
            
			
			
		}

		private void fillDDEmpresaContratante()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtEmpresaContratante.Text,txtNifEmpresaContratante.Text,"1","","","","","",""); //activas
			DataView DV = new DataView(DT);		
			ddEmpresaContratante.DataSource = DV; ; 
			ddEmpresaContratante.DataBind();
			empresa = null;
            rbEmpbrepodevercertificados.SelectedValue = "0"; //por default nao pode ver, teem de marcar explicitamente.

        }


        //=============================================================================
        //PREENCHE AS DROPDOWNS DA PÁGINA
        //=============================================================================
        private void FillDropDownsCabecalho()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaFuncionarios(); 
			ddFuncionarioRecepcao.DataSource = DR2; 
			ddFuncionarioRecepcao.DataBind(); 
			DR2.Close(); 

			//DATA.GeralBD geral = new LabMetro.DATA.GeralBD();
		
			if(!Page.IsPostBack) ddFuncionarioRecepcao.SelectedValue= System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));   

			SqlDataReader DR4 = lista.DRListaTipoServico(); 
			ddTipoServico.DataSource = DR4; 
			ddTipoServico.DataBind(); 
			DR4.Close(); 

			ddTipoServico.Items.RemoveAt(0); //remove o aditamento


			SqlDataReader DR5 = lista.DRListaLocalCalibracao(); 
			ddLocalDestino.DataSource = DR5; 
			ddLocalDestino.DataBind(); 
			DR5.Close();
            ddLocalDestino.Items.Insert(0, new ListItem("----", "")); 

			lista = null; 
			//geral = null; 
		}

		// Preencher os dados relativos à empresa (se é devedora ou não)
		private void fillCompanyInfo()
		{
			string idEmpresa = ddEmpresa.SelectedValue; 
			if(ddEmpresaContratante.SelectedValue !="") 
			{
				idEmpresa = ddEmpresaContratante.SelectedValue;
				lblEmpresaDevedora.Text ="Empresa contratante: "; //limpar a label

			}
			lblEmpresaDevedora.Text =""; //limpar a label

			LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(idEmpresa); 
			if(detEmpresa != null)
			{
				lblObsEmpresa.Text = detEmpresa.observacoes;    //meu
				lblCondPagamEmpresa.Text = detEmpresa.condicoesPagamento.ToUpper(); 

				if (detEmpresa.pagamentoAtraso =="1")
				{
					lblEmpresaDevedora.Text += "** PAGAMENTOS EM ATRASO **<br />";
					
					//isto ou similar acho q é um bug na framwork e não dá para fazer
					//ddEmpresa.SelectedItem.Attributes.Add("style", "color:" + ds.Tables[0].Rows[i]["CategoryColor"].ToString () );
					//trEmpresa.BgColor = System.Drawing.ColorTranslator.FromHtml("#FF0033").ToString();
				}
				else
				{
					lblEmpresaDevedora.Text = ""; 
				}
				
				//martelada

				if(ddEmpresaContratante.SelectedValue !="") 
				{

					trEmpresa.BgColor=""; 
					switch(detEmpresa.nivelBloqueioLabmetro)	
					{ 
						case "0": 
							trEmpresaContratante.BgColor=""; 
							break;
						case "1":
							trEmpresaContratante.BgColor ="Gold"; 
							break; 
						case "2": 
							trEmpresaContratante.BgColor ="DarkOrange";
							lblEmpresaDevedora.Text +="Venda à dinheiro ou Pagamento do Atrasado.<br />"; 
							break; 
						case "3": 
							trEmpresaContratante.BgColor ="Crimson";
							lblEmpresaDevedora.Text +="Venda à dinheiro.<br />"; 
							break; 
					}
				}
				else
				{
					trEmpresaContratante.BgColor=""; 
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

		//=============================================================================
		//PREENCHE A PÁGINA QUANDO O BRE JÁ EXISTE
		//=============================================================================
		private void fillForm()
		{

			string id= Request.QueryString["id"].ToString(); 
			ViewState["idBRE"]=id;  //guardar em viewstate

			LabMetro.DATA.BreBD fillbre = new LabMetro.DATA.BreBD(); 
			LabMetro.DATA.BreDetails det = fillbre.GetBreDetails(id); 
			
			if(det!= null)
			{
				txtRefBRE.Text = det.refBRE; 
				txtRefBRE.Enabled=false; 

				txtDataBRE.Text = GERAL.clsGeral.ToShortDate(det.dtBRE.ToString());
				txtEntreguePor.Text = det.entreguePor; 
				txtObservacoes.Text = det.observacoes; 
                
				try
				{
					ddFuncionarioRecepcao.SelectedValue = det.idFuncionarioRecepcao; 
				}
				catch
				{
					ddFuncionarioRecepcao.Items.Insert(0,new ListItem(det.funcionarioRecepcao,det.idFuncionarioRecepcao));
					ddFuncionarioRecepcao.SelectedValue = det.idFuncionarioRecepcao; 
				}

				ViewState["idEmpresa"] = det.idEmpresa; 
				ddEmpresa.Items.Insert(0, new ListItem(det.nomeEmpresa,det.idEmpresa)); 
				ddEmpresa.SelectedValue = det.idEmpresa; 
				ddEmpresa.DataBind(); 

				ddEmpresaContratante.Items.Insert(0, new ListItem(det.nomeEmpresaContratante,det.idEmpresaContratante)); 
				ddEmpresaContratante.SelectedValue = det.idEmpresaContratante; 
				ddEmpresaContratante.DataBind(); //aqui nao faço fill, so adiciono o seleccionado

				ddEmpresa.Enabled=false; 
				ddEmpresaContratante.Enabled=false; 
				pesquisaEmpresaContratante.Enabled=false;
				txtPesquisaEmpresa.Enabled=false;
				txtPesquisaNif.Enabled = false;
				btnEmpresas.Enabled=false; 



				rbReqCompleta.SelectedValue = det.requisicaoCompleta;
               
                
                //dps de emitido nao se pode alterar a empresa contratante a nao ser no BO


                txtEmpresaContratante.Enabled = false;
                pesquisaEmpresaContratante.Enabled = false;


                rbEmpbrepodevercertificados.SelectedValue = det.bEmpBrePodeverCertificados;

                rbTaxaUrgencia.SelectedValue = det.bTaxaUrgencia;


                if (det.idEmpresaContratante != "")
                {
                    rbEmpbrepodevercertificados.Enabled = true;

                }
                else
                {
                    rbEmpbrepodevercertificados.Enabled = false;

                }
                try
				{
					chbDadosDefinitivos.Checked = GERAL.clsGeral.ConvertBStringToBoolean(det.bDefinitivo); 
				}
				catch
				{}
			}

			fillbre = null; 

			BindGridServices(); 
		}


		//=============================================================================
		//BINDGRID DOS SERVIÇOS ASSOCIADOS AO BRE
		//=============================================================================
		private void BindGridServices()
		{
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
			DT = servico.DTGetServicoByBRE(ViewState["idBRE"].ToString()); 
			DGServicosBRE.DataSource=DT; 
			DGServicosBRE.DataBind();  
		}

		//=============================================================================
		//APAGA SERVIÇO DO DATAGRID SERVIÇOS //passa o estado para anulado
		//=============================================================================
		private void DGServicosBRE_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
		{
			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
			
//			DataRowView DRV = (DataRowView) e.Item.DataItem; isto so deve funcionar com o edit
			//string idServico = DRV["idServico"].ToString(); 
			//string idEstadoServico = DRV["idEstadoServico"].ToString(); 

			string idServico = DGServicosBRE.DataKeys[e.Item.ItemIndex].ToString();
			string idEstadoServico = DGServicosBRE.Items[e.Item.ItemIndex].Cells[10].Text.ToString(); //so consigo ir buscar o text de boundcolumns

			bre.disableServiceInBRE(idServico,idEstadoServico); 
			bre = null; 
			string idBRE = ViewState["idBRE"].ToString(); 
			Response.Redirect("FormBRECalibExt.aspx?btn=DOC&id="+idBRE,true); 
			
//			for(int i = 0; i< DGServicosBRE.Columns.Count; i++)
//			{
//				Response.Write(i + "-" + DGServicosBRE.Items[e.Item.ItemIndex].Cells[i].Text.ToString()+"<br />"); 
//			}

			
		}

		//=============================================================================
		//EDITA LINHA DE SERVIÇO DO DATAGRID SERVIÇOS
		//=============================================================================
		protected void DGServicosBRE_Edit(Object sender, DataGridCommandEventArgs e)     
		{
			DGServicosBRE.ShowFooter=false; 
			DGServicosBRE.EditItemIndex = e.Item.ItemIndex;	
			BindGridServices(); 
		}

		//=============================================================================
		//CANCELA LINHA DE SERVIÇO DO DATAGRID SERVIÇOS
		//=============================================================================
		protected void DGServicosBRE_CancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DGServicosBRE.ShowFooter=true;  
			DGServicosBRE.EditItemIndex = -1;
			BindGridServices(); 
		}
		
		//=============================================================================
		//UPDATE DATAGRID SERVIÇOS
		//=============================================================================
		protected void DGServicosBRE_UpdateGrid(Object sender, DataGridCommandEventArgs e)
		{
			string idServico = DGServicosBRE.DataKeys[e.Item.ItemIndex].ToString(); 

			DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");
            DropDownList ddLocalDestino = (DropDownList)e.Item.FindControl("ddLocalDestinoEdit");
			DropDownList ddEstadoServico = (DropDownList)e.Item.FindControl("ddEstadoServicoEdit");

			DropDownList ddCodTipoEquipamento = (DropDownList)e.Item.FindControl("ddCodTipoEquipamentoEdit");
			DropDownList ddNumIdent = (DropDownList)e.Item.FindControl("ddNumIdentEdit");
			DropDownList ddNumSerie = (DropDownList)e.Item.FindControl("ddNumSerieEdit");
			DropDownList ddRefUltimaCalibracao = (DropDownList)e.Item.FindControl("ddRefUltimaCalibracaoEdit");
			DropDownList ddMarteladaIdTipoEquipamento = (DropDownList)e.Item.FindControl("ddMarteladaIdTipoEquipamento");//o selectedValue é o idEquipamento

			TextBox txtObs = (TextBox) e.Item.FindControl("txtObservacoesEdit"); 

			DATA.BreBD bre = new LabMetro.DATA.BreBD();
			bre.updateServicoInBRECalibExt(idServico, ddRequisicao.SelectedValue, ddLocalDestino.SelectedValue,txtObs.Text,ddMarteladaIdTipoEquipamento.SelectedValue,ddEstadoServico.SelectedValue,"0");  //bdefintivo aqui vai a zero, pq nao pode ser alterado a este nivel
			bre = null; 

			DGServicosBRE.EditItemIndex=-1;
			BindGridServices(); 
			

		}        

		//=============================================================================
		//ITEMDATABOUND DO DATAGRID SERVIÇOS
		//=============================================================================
		private void DGServicosBRE_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item ||e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;
                
				if((DRV["canEdit"].ToString()== "0") ||(DRV["idEstadoServico"].ToString()== "7") || (DRV["idEstadoServico"].ToString()== "20") || (DRV["idEstadoServico"].ToString()== "-1")) //ANULADO
				{
					LinkButton button =(LinkButton)e.Item.Cells[14].Controls[0];
					button.Enabled =false; 
					button.CausesValidation=false; 

					LinkButton button2 =(LinkButton)e.Item.Cells[15].Controls[0];
					button2.Enabled =false; 
					button2.CausesValidation=false; 
				}
			}
			else if(e.Item.ItemType == ListItemType.EditItem)
			{

				DataRowView DRV = (DataRowView) e.Item.DataItem;


				DATA.RequisicaoBD requisicao = new LabMetro.DATA.RequisicaoBD(); 
                
				//ver se isto funciona
				if(ViewState["idEmpresa"].ToString()=="") ViewState["idEmpresa"] = ddEmpresa.SelectedValue.ToString(); 

				//*************************************************************************
				//REQUISICAO===============================================================
				//*************************************************************************

				string refServico = DRV["refServico"].ToString(); 
				DataTable DT = requisicao.DTGetRequisicoesIncompletasByEmpresaServico(ViewState["idEmpresa"].ToString(), refServico); 

				DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");
				ddRequisicao.DataSource= DT; 
				ddRequisicao.DataBind(); 
				ddRequisicao.Items.Insert(0, new ListItem("","")); 
				//DR.Close(); 

				requisicao = null; 

				

				string idRequisicao = DRV["idRequisicao"].ToString();      
				if(idRequisicao !="") ddRequisicao.SelectedValue = idRequisicao; 
                
				//*************************************************************************
				//ESTADO SERVIÇO===========================================================
				//*************************************************************************
				DropDownList ddEstadoServico = (DropDownList)e.Item.FindControl("ddEstadoServicoEdit");                  
				DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
				SqlDataReader dr = servico.DRGetListaEstadosServico(""); 
				ddEstadoServico.DataSource = dr; 
				ddEstadoServico.DataBind(); 
				dr.Close();     

				servico = null; 
				try
				{
					ddEstadoServico.SelectedValue = DRV["idEstadoServico"].ToString(); 
				}
				catch
				{
					ddEstadoServico.Items.Clear(); //tirar os outros todos
					ddEstadoServico.Items.Insert(0,new ListItem(DRV["estadoServico"].ToString(),DRV["idEstadoServico"].ToString())); 
				}

				//*************************************************************************
				//LOCAL CALIBRAÇÃO=========================================================
				//*************************************************************************
                DropDownList ddLocalDestino = (DropDownList)e.Item.FindControl("ddLocalDestinoEdit");
                
				DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
				SqlDataReader DR2 = lista.DRListaLocalCalibracao();
                ddLocalDestino.DataSource = DR2;
                ddLocalDestino.DataBind(); 
				DR2.Close();
                ddLocalDestino.Items.Insert(0, new ListItem("----", "")); 

				lista = null;

                ddLocalDestino.SelectedValue = DRV["idLocalDestino"].ToString(); 

                
				//*************************************************************************
				//DATATABLE PARA DATASOURCE DAS DROPDOWNS DE EQUIPAMENTOS==================
				//*************************************************************************
				DATA.BRECalibExtBD cls = new LabMetro.DATA.BRECalibExtBD(); 
                
				DataTable dt = cls.DTGetEquipamentosActivosByEmpresaPorGrandeza(ViewState["idEmpresa"].ToString(),DRV["idTipoEquipamento"].ToString(),DRV["refServico"].ToString());
				

				cls= null; 

				//                //criar primary key para o find....
				//                DataColumn[] dcPk = new DataColumn[1];
				//                dcPk[0] = dt.Columns["idEquipamento"];
				//                dt.PrimaryKey = dcPk;
				//
				DataView dv = new DataView(dt); 
				//                //dv.Sort = "idEquipamento"; 
				//
				//                ViewState["dt"] = dt; 

				if(dv.Table.Rows.Count > 0)
				{
                    
					//para abrir as dropdowns no sitio seleccionado
					//tenho de me reger por uma q esteja definida! ou pelo index da datatable
					//unicas sao: numId JA NAO!!!!, refUltimaCalibracao - obrigatorio é o numIdentificacao
					//tenho de igualar todos ao selectedIndex do numIdent
					//RESOLVER ISSO, NUM iDENTIFICACAO DEIXOU DE SER OBRIGATORIO

					//NÚMERO DE IDENTIFICAÇÃO
					DropDownList ddNumIdent = (DropDownList)e.Item.FindControl("ddNumIdentEdit");
					ddNumIdent.DataSource= dv; 
					ddNumIdent.DataBind(); 

					//if(DRV["numIdentificacao"].ToString() !="") //em principio nao pode estar a null, so se tiver vindo mal da migracao
					//agora ja pode estar a null --- DM 04-01-2006

					try //just in case...
					{
						ddNumIdent.SelectedValue = DRV["idEquipamento"].ToString();  
					}
					catch
					{}

					//CODIGO TIPO EQUIPAMENTO
					DropDownList ddCodTipoEquipamentoEdit = (DropDownList)e.Item.FindControl("ddCodTipoEquipamentoEdit");

					//so podem mudar tipo de equipamento depois de terem submetido o bre
					//pq eu preciso da ref de calibracao para saber a grandeza
					//                    if(ViewState["idBRE"].ToString() != "")
					//                    {    
					ddCodTipoEquipamentoEdit.DataSource= dv; 
					ddCodTipoEquipamentoEdit.DataBind(); 
					ddCodTipoEquipamentoEdit.SelectedIndex = ddNumIdent.SelectedIndex; 
					//                    }
					//                    else
					//                    {
					//                        //a dropwdown passa a estar preenchida com o idEquipamento
					//                        //no campo datavaluefield
					//                        ddCodTipoEquipamentoEdit.Items.Add(new ListItem(DRV["codTipoEquipamento"].ToString(),DRV["idEquipamento"].ToString())); 
					//
					//                    }

					//NÚMERO DE SÉRIE
					DropDownList ddNumSerie = (DropDownList)e.Item.FindControl("ddNumSerieEdit");
					ddNumSerie.DataSource= dv; 
					ddNumSerie.DataBind(); 
                
					ddNumSerie.SelectedIndex = ddNumIdent.SelectedIndex;     

					//REFERENCIA DA ULTIMA CALIBRAÇÃO
					DropDownList ddRefUltimaCalibracao = (DropDownList)e.Item.FindControl("ddRefUltimaCalibracaoEdit");
                        
					ddRefUltimaCalibracao.DataSource= dv; 
					ddRefUltimaCalibracao.DataBind(); 
                
					ddRefUltimaCalibracao.SelectedIndex = ddNumIdent.SelectedIndex;   
  
					//DROPDOWNLIST INVISIVEL Q SERVE PARA ALBERGAR NO SEU TEXT O IDTIPOEQUIPAMENTO
					DropDownList ddMarteladaIdTipoEquipamento =  (DropDownList)e.Item.FindControl("ddMarteladaIdTipoEquipamento");
					ddMarteladaIdTipoEquipamento.DataSource = dv; 
					ddMarteladaIdTipoEquipamento.DataBind(); 

					ddMarteladaIdTipoEquipamento.SelectedIndex = ddNumIdent.SelectedIndex;   
                        
				}
                
			}
		} 
        
		//isto nao está pronto, tenho de captar o evento no footer do datagrid...
		protected void dd_SelectedIndexChanged(object sender, EventArgs e)
		{
			//encontrar a dropdown
			DropDownList ddSender = (DropDownList)sender;
                
			//encontrar o seu selected indezx
			int selIndex = ddSender.SelectedIndex; 

			//encontrar o seu id
			string id = ddSender.ID.ToString(); 
                 
			int dgIndex = DGServicosBRE.EditItemIndex; 

			//esta serve apenas para poder guardar nalgum sitio o meu idTipoEquipamento e muda-lo no onselectedIndexChanged das outras
			DropDownList ddNumSerieEdit = (DropDownList)DGServicosBRE.Items[dgIndex].FindControl("ddNumSerieEdit");
			DropDownList ddNumIdentEdit = (DropDownList)DGServicosBRE.Items[dgIndex].FindControl("ddNumIdentEdit");
			DropDownList ddRefUltimaCalibracaoEdit = (DropDownList)DGServicosBRE.Items[dgIndex].FindControl("ddRefUltimaCalibracaoEdit");
			DropDownList ddCodTipoEquipamentoEdit = (DropDownList)DGServicosBRE.Items[dgIndex].FindControl("ddCodTipoEquipamentoEdit");

			DropDownList ddMarteladaIdTipoEquipamento =  (DropDownList)DGServicosBRE.Items[dgIndex].FindControl("ddMarteladaIdTipoEquipamento");

			switch (id)
			{

				case "ddCodTipoEquipamentoEdit":
					//mudar as outras 3 para o mesmo index:
					ddRefUltimaCalibracaoEdit.SelectedIndex =selIndex; 
					ddNumSerieEdit.SelectedIndex = selIndex; 
					ddNumIdentEdit.SelectedIndex = selIndex; 
					ddMarteladaIdTipoEquipamento.SelectedIndex = selIndex;
					break; 


				case "ddNumSerieEdit":
					if(ViewState["idBRE"].ToString() != "")
					{ 
						ddCodTipoEquipamentoEdit.SelectedIndex = selIndex; 
					}
					ddRefUltimaCalibracaoEdit.SelectedIndex = selIndex; 
					ddNumIdentEdit.SelectedIndex = selIndex; 
					ddMarteladaIdTipoEquipamento.SelectedIndex = selIndex;
					break;

				case "ddNumIdentEdit":
					if(ViewState["idBRE"].ToString() != "")
					{ 
						ddCodTipoEquipamentoEdit.SelectedIndex = selIndex; 
					}
					ddRefUltimaCalibracaoEdit.SelectedIndex = selIndex; 
					ddNumSerieEdit.SelectedIndex = selIndex; 
					ddMarteladaIdTipoEquipamento.SelectedIndex = selIndex;
					break; 


				case "ddRefUltimaCalibracaoEdit":
					if(ViewState["idBRE"].ToString() != "")
					{ 
						ddCodTipoEquipamentoEdit.SelectedIndex = selIndex; 
					}
					ddNumIdentEdit.SelectedIndex = selIndex; 
					ddNumSerieEdit.SelectedIndex = selIndex; 
					ddMarteladaIdTipoEquipamento.SelectedIndex = selIndex;
					break; 
			}

		}



		//=============================================================================
		//+++++++++++++++++++++++++++FIM DATAGRID SERVIÇOS+++++++++++++++++++++++++++++
		//=============================================================================

		//=============================================================================
		//+++++++++++++++++++++++++++DATAGRID EQUIPAMENTOS+++++++++++++++++++++++++++++
		//=============================================================================

		private void fillDDTipoEquipamentoPesquisa()
		{
			DATA.BreBD bre = new DATA.BreBD();
			ddTipoEquipamentoPesquisa.DataSource = bre.DRTiposEquipamentoByEmpresa(ddEmpresa.SelectedValue); 
			ddTipoEquipamentoPesquisa.DataBind(); 
			ddTipoEquipamentoPesquisa.Items.Insert(0, new ListItem("","")); 
			bre = null;

		}

		//=============================================================================
		//BINDGRID DATAGRID EQUIPAMENTOS
		//=============================================================================
		private void BindGridEquipamentos()
		{
			DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 
			
			DTEquipamentos = equipamento.DTGetEquipamentosActivosByEmpresa(ddEmpresa.SelectedValue,"",txtPesquisaRefCalibracao.Text,txtPesquisaNumIdentificacao.Text,txtPesquisaNumSerie.Text, ddTipoEquipamentoPesquisa.SelectedValue); 

			DataView DVEquipamentos = new DataView(DTEquipamentos); 
 
			DVEquipamentos.Sort= ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

			if(DVEquipamentos.Table.Rows.Count ==0) lblMessage.Text+= "<br />Não existem equipamentos correspondentes os critérios de pesquisa introduzidos.";

			DGEquipamentos.DataSource = DVEquipamentos; 
			DGEquipamentos.DataBind(); 

			equipamento = null; 

			
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
			SqlDataReader dr = servico.DRGetListaEstadosServico(""); 
			ddEstado.DataSource = dr; 
			ddEstado.DataBind(); 
			dr.Close();    
          
			ddEstado.SelectedValue="2"; //fica automaticamente marcado como "aguarda calibração externa"
		}
       
		//=============================================================================
		//SORT DATAGRID EQUIPAMENTOS
		//=============================================================================D
		protected void SortGridEquipamento(Object s, DataGridSortCommandEventArgs e)
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
			BindGridEquipamentos(); 
		}

		//=============================================================================
		//ITEMCOMMAND DO DATAGRID EQUIPAMENTOS
		//INSERE UM EQUIPAMENTO NOVO NOS EQUIPAMENTOS DA EMPRESA
		//=============================================================================
		protected void DGEquipamentos_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName=="Insert")
			{               
				TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerieFoter"); 
				TextBox txtNumIdentificacao = (TextBox)e.Item.FindControl("txtNumIdentificacaoFooter"); 
                    
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoFooter");
                
				if(txtNumIdentificacao.Text =="")
				{
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_NUM_IDENTIFICACAO;
				}
				else
				{
					DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 
                    
					//passar valores para vars para igualar dps ao critérios de pesquisa para bindgrid dos equipamentos
					string idTipoEquip = ddTipoEquipamento.SelectedValue.ToString(); 
					string txtNumSer = txtNumSerie.Text.ToString(); 
					string txtNumId = txtNumIdentificacao.Text.ToString(); 
					try
					{
						lblMessage.Text = equipamento.InsertEquipmentinBre(ddEmpresa.SelectedValue.ToString(),idTipoEquip,txtNumSer,txtNumId,"","");
					}
					catch(Exception ex)
					{
						Response.Write(ex.ToString());
					}
					
					equipamento = null; 
					//limpar campos
					
					//fazer um set dos critérios de pesquisa para o equipamento recent-introduzido, 
					//para aparecer logo disponível.
					fillDDTipoEquipamentoPesquisa(); //ja com o novo  valor

					ddTipoEquipamentoPesquisa.SelectedValue=idTipoEquip;
					txtPesquisaNumIdentificacao.Text=txtNumId; 
					txtPesquisaNumSerie.Text=txtNumSer;

					BindGridEquipamentos();
				}
			}
		}

		//=============================================================================
		//ITEMDATABOUND DO DATAGRID EQUIPAMENTOS
		//PREENCHE A DROPDOWN TIPO EQUIPAMENTO NO FOOTER
		//=============================================================================
		private void DGEquipamentos_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoFooter");
				DATA.ListasBD lista = new LabMetro.DATA.ListasBD();  
				SqlDataReader DR = lista.DRListaTiposEquipamento(); 
				ddTipoEquipamento.DataSource = DR; 
				ddTipoEquipamento.DataBind(); 
				DR.Close(); 

				lista = null;
			}
		}

		//=============================================================================
		//+++++++++++++++++++++++++++FIM DATAGRID EQUIPAMENTO++++++++++++++++++++++++++
		//=============================================================================

		//=============================================================================
		//+++++++++++++++++++++++++++DATAGRID REQUISICOES++++++++++++++++++++++++++++++
		//=============================================================================

		//=============================================================================
		//BINDGRID DATAGRID REQUISICOES
		//=============================================================================

		protected void BindGridRequisicoes()
		{
  
			DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();         
			DTRequisicoes = req.DTGetRequisicoesIncompletasByEmpresa(ddEmpresa.SelectedValue); 
    
			DGRequisicoes.DataSource = DTRequisicoes; 
			DGRequisicoes.DataBind();               

			req = null; 
		}
      
		//=============================================================================
		//PAGING DATAGRID REQUISIÇOES
		//=============================================================================
		protected void DoPagingRequisicoes(Object s,DataGridPageChangedEventArgs e)
		{
			DGRequisicoes.CurrentPageIndex=e.NewPageIndex;
			BindGridRequisicoes(); 
		}

//		//=============================================================================
//		//ITEMCOMMAND DO DATAGRID REQUISICOES
//		//NAO SEI SE VOU CONTINUAR A USAR ISTO
//		//=============================================================================
//		protected void DGRequisicoes_ItemCommand(object sender, DataGridCommandEventArgs e)
//		{
//			if(e.CommandName=="SelectReq")
//			{
//				string id = DGRequisicoes.DataKeys[e.Item.ItemIndex].ToString();
//                
//				ViewState["idRequisicao"] = id; 
//				ViewState["refRequisicao"] = e.Item.Cells[2].Text.ToString();
//
//				updateRequisicaoinGridDestino(); 
//			}
//		}
        
		//=============================================================================
		//+++++++++++++++++++++++++++FIM DATAGRID REQUISICOES++++++++++++++++++++++++++
		//=============================================================================

		//=============================================================================
		//INSERE UM BRE NA BD
		//INSERE APENAS DOS DADOS REFERENTES AO CABEÇALHO DO BRE (OS SERVIÇOS SÃO ACRESCENTADOS DEPOIS)
		//FAZ REDIRECT PARA O BRE COM OS DADOS PREENCHIDOS. 
		//=============================================================================


		private void InsertBD()
		{
			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
   
			int idBRE = bre.InsertBre(ddEmpresa.SelectedValue.ToString(), ddFuncionarioRecepcao.SelectedValue.ToString(), rbReqCompleta.SelectedValue.ToString(),txtEntreguePor.Text.ToString(), txtObservacoes.Text.ToString(),User.Identity.Name.ToString(),"0","", ddEmpresaContratante.SelectedValue.ToString(),"", rbEmpbrepodevercertificados.SelectedValue.ToString(), rbTaxaUrgencia.SelectedValue.ToString()); //ultimo param bdefinitivo = 0
			
			bre = null; 
			if(idBRE != 0) 
			{
				Response.Redirect("FormBRECalibExt.aspx?btn=DOC&id="+idBRE,true); 
			}
			else
			{
				lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
			}
			
		}

		//=============================================================================
		//ALTERA UM BRE NA BD
		//=============================================================================
		private void UpdateBD()
		{
			DATA.BreBD bre = new LabMetro.DATA.BreBD();
            
			string bDefinitivo = "0";  //está a 0, false
			if(chbDadosDefinitivos.Checked==true) bDefinitivo = "1"; 

			string retValue = bre.UpdateBreWithServicesComEquipamento(ViewState["idBRE"].ToString(),ViewState["idEmpresa"].ToString(), ddFuncionarioRecepcao.SelectedValue.ToString(), rbReqCompleta.SelectedValue.ToString(),txtEntreguePor.Text.ToString(), txtObservacoes.Text.ToString(),User.Identity.Name.ToString(),null,bDefinitivo,"","", rbEmpbrepodevercertificados.SelectedValue.ToString(),rbTaxaUrgencia.SelectedValue.ToString());  //chama a mesma funcao que o BRE Normal mas a dataview vai a NULL
			if(retValue == GERAL.clsGeral.ErrorMessage.MSG_DB)
			{
				bre = null; 
				if(bDefinitivo =="1") 
				{
					btnGravar.Enabled=false;
					btnSubmit.Enabled=false;
					btnInserirServicos.Enabled=false;
					btnEmpresas.Enabled=false; 
					btnPesquisaEquipamentoEmpresa.Enabled=false;


                    DGServicosBRE.Enabled = false; 
					//Response.Redirect("FormBRE.aspx?btn=DOC&errUpd="+0+"&id="+ViewState["idBRE"].ToString()); //o zero é esperado do outro lado se correu bem
				}
				else //é transferido para a mesma pagina
				{
					Response.Redirect("FormBRECalibExt.aspx?btn=DOC&errUpd="+retValue+"&id="+ViewState["idBRE"].ToString(),true); 
				}
			}
			else
			{
				bre = null;
			}
              
		}
        

		//=============================================================================
		//ACÇÃO DO BOTAO SUBMIT DA PÁGINA, INSERE OU ALTERA DADOS DO BRE
		//=============================================================================
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				if(DGServicosBRE.EditItemIndex > -1)
				{
					lblMessage.Text="Acabe de editar as linhas do serviço, antes de submeter a página."; 
					return; 
				}
				if(btnSubmit.CommandArgument=="insert")
				{
					InsertBD();
				}
				else if(btnSubmit.CommandArgument=="update")
				{
					UpdateBD(); 
				}
			}

		}


		//=============================================================================
		//SELECTEDINDEXCHANGED DA DROPDOWN EMPRESA
		//TEM DE MUDAR A DATASOURCE DO DATAGRID EQUIPAMENTOS
		//E NO DATAGRID REQUISICOES
		//=============================================================================

		//RESET DOS DADOS DA PAGINA

		
		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillCompanyInfo(); 
		}

		private void btnPesquisaEquipamentoEmpresa_Click(object sender, System.EventArgs e)
		{
			if(ddEmpresa.SelectedValue=="")
			{
				lblMessage.Text ="Seleccione uma empresa antes de pesquisar."; 
			}
			else
			{
				DGEquipamentos.CurrentPageIndex =0; 
				BindGridEquipamentos(); 
			}
		}

        private void btnVerBRE_Click(object sender, System.EventArgs e)
        {
            

            ReportClass report = null;

            switch (myApp)
            {

                case "ISQ_LABMETRO":
                    //o crystal dps faz um replace do <br> por char(13)
                    
                    
                    report = new LabMetro.REPORTS.crBRE();
                    break;
                case "ANG_LABMETRO":
                    //o crystal dps faz um replace do <br> por char(13)
                    report = new LabMetro.REPORTS_ANG.crBRE();
                    break;

                case "ES_LABMETRO":
                    //o crystal dps faz um replace do <br> por char(13)
                    //o crystal dps faz um replace do <br> por char(13)
                    
                    
                    report = new LabMetro.REPORTS_ES.crBRE();
                    break;
                case "CV_LABMETRO":
                    //o crystal dps faz um replace do <br> por char(13)
                    report = new LabMetro.REPORTS_CV.crBRE();
                    break;
                case "DZ_LABMETRO":
                    //o crystal dps faz um replace do <br> por char(13)
                    report = new LabMetro.REPORTS_DZ.crBRE();
                    break;
                case "SON_LABMETRO":
                    report = new LabMetro.REPORTS_SON.crBRE();
                    break;
                case "BR_LABMETRO":
                    //o crystal dps faz um replace do <br> por char(13)
                    report = new LabMetro.REPORTS_DZ.crBRE();
                    break;
            }
            

            clsReport cr = new clsReport();

            DATA.BreBD bre = new LabMetro.DATA.BreBD();
            DataSet ds = bre.DSBRE(ViewState["idBRE"].ToString());
            report.SetDataSource(ds);

            ds = null;
            bre = null;
            
            cr.exportReportToPDF(report,"BRE");

            
            //cr = null;
            //report = null;
            

        }


		private void btnVerEtiquetas_Click(object sender, System.EventArgs e)
		{
			crEtiquetaCal report = new crEtiquetaCal();
			clsReport cr = new clsReport();

			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
			DataSet ds  = bre.DSEtiqueta(ViewState["idBRE"].ToString()); 	
				
			report.SetDataSource(ds);
            ds = null;
            bre = null;
			cr.exportReportToPDF(report,"Etiqueta");
				
			 
		}


		//botao que adiciona equipamentos escolhidos ao datagrid serviços (isto é: 
		//à datasource do datagrid

		private void updateRequisicaoinGridDestino()
		{
			DataTable DT = (DataTable)ViewState["DT"];
			DV = new DataView(DT); 

			foreach(DataRowView drv in DV)
			{
				drv["idRequisicao"] = ViewState["idRequisicao"].ToString(); 
				drv["refRequisicao"] = ViewState["refRequisicao"].ToString();
				drv.EndEdit(); 
			}
		
			DT.AcceptChanges(); 
			ViewState["DT"] = DT; 
			BindGridServices(); 
		}


		private void btnInserirServicos_Click(object sender, System.EventArgs e)
		{	
			
			
			int iNumVezes = 1; 
			bool bExiste = false; 
			string strIdsEquipamentos =""; 
			foreach(DataGridItem dgi in DGEquipamentos.Items) 
			{ 
				CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				if(myCheckBox.Checked == true) 
				{
			
					strIdsEquipamentos+= DGEquipamentos.DataKeys[dgi.ItemIndex].ToString() + ","; 
					bExiste = true;
				}
			}
			if(bExiste)
			{
				string idBRE = ViewState["idBRE"].ToString(); 
				string idRequisicao = ""; 
				if(DGRequisicoes.SelectedIndex > -1) idRequisicao = DGRequisicoes.DataKeys[DGRequisicoes.SelectedIndex].ToString(); 
				string bDefinitivo = "0";  //está a 0, false
				if(chbDadosDefinitivos.Checked==true) bDefinitivo = "1"; 

				
				iNumVezes = System.Convert.ToInt16(txtNumVezes.Text); 
			
				DATA.BreBD bre = new LabMetro.DATA.BreBD();
				if(bre.InsertServicesInBRECalibExt(iNumVezes,idBRE,idRequisicao, strIdsEquipamentos,bDefinitivo, ddEstado.SelectedValue, ddTipoServico.SelectedValue,ddLocalDestino.SelectedValue,rbReqCompleta.SelectedValue, txtEntreguePor.Text,txtObservacoes.Text,"")==true)
				{
					bre=null;
					Response.Redirect("FormBRECalibExt.aspx?btn=DOC&id="+idBRE,true);
				}
				else
				{		 					
					bre = null; 	
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DB;
				}																												
			}		
		}

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			fillCompanyInfo(); 
		}

		private void btnEmpresas_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			fillCompanyInfo(); 
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			fillCompanyInfo(); 
		}

	
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

		private void btnListaEquipamentos_Click(object sender, System.EventArgs e)
		{
			crListaEquipsPorBRE report = new crListaEquipsPorBRE(); 
			clsReport cr = new clsReport();

			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
			DataSet myDs  = bre.DSEquipamentos(ViewState["idBRE"].ToString()); 	

			report.SetDataSource(myDs);
            myDs = null;
			bre = null;
            // Exportar o report para PDF
            cr.exportReportToPDF(report, "Report");
        }



		private void DGRequisicoes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//Response.Write("entrou aqui");
		}

		protected void pesquisaEmpresaContratante_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresaContratante();
			fillCompanyInfo();
		}


		private void txtEmpresaContratante_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresaContratante();
			fillCompanyInfo();
		}

        protected void ddEmpresaContratante_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillCompanyInfo();
            rbEmpbrepodevercertificados.SelectedValue = "0"; //por default nao pode ver, teem de marcar explicitamente.

        }

        protected void txtEmpresaContratante_TextChanged1(object sender, EventArgs e)
        {
            fillDDEmpresaContratante();
            fillCompanyInfo();
        }
    }
}