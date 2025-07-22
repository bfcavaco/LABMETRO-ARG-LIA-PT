
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
	/// Summary description for FormBRE.
	/// </summary>
	public partial class FormBRE : System.Web.UI.Page
	{
	
	
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox txtBRE;
        
		private const string ID_PAG = "BRE_1";//NOME DA PAGINA
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

		DataTable DT; //datatable q contem os serviços
		DataView DV; // dataviiew sobre a datatable dos serviços
		DataTable DTEquipamentos;
		DataTable DTRequisicoes;
        DataView DVRequisicoes;
		
		//nota daniela: o campo "todos com requisicao não serve de NADA"....

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
		{    DGServicosBRE.ItemCommand += new DataGridCommandEventHandler (DGServicosBRE_ItemCommand); 
			DGServicosBRE.ItemDataBound +=  new DataGridItemEventHandler(DGServicosBRE_ItemDataBound); 
			DGServicosBRE.DeleteCommand += new  DataGridCommandEventHandler(DGServicosBRE_DeleteCommand);
			DGEquipamentos.ItemDataBound +=  new DataGridItemEventHandler(DGEquipamentos_ItemDataBound); 
			//declaracao do itemcommand passou para dentro do grid         
			DGRequisicoes.ItemCommand += new DataGridCommandEventHandler(DGRequisicoes_ItemCommand); 
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged); 
			btnPesquisaEquipamentoEmpresa.Click += new System.EventHandler(btnPesquisaEquipamentoEmpresa_Click);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnVerBRE.Click += new System.EventHandler(btnVerBRE_Click);
			btnVerEtiquetas.Click += new System.EventHandler(btnVerEtiquetas_Click);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
			btnListaEquipamentos.Click += new System.EventHandler(btnListaEquipamentos_Click);
			pesquisaEmpresaContratante.Click += new System.EventHandler(pesquisaEmpresaContratante_Click);

		}

		
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Response.Expires = 0; 	//nao tirar!!!

			lblMessage.Text ="";
            //lblMessageServicos.Text = "";

			Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))//sem permissões
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

					if(Request.QueryString["id"]!=null && Request.QueryString["id"]!="")
					{
						if(!Page.IsPostBack)
						{
							ViewState["idBRE"] = Request.QueryString["id"].ToString();
              
							LabMetro.DATA.BreBD fillbre = new LabMetro.DATA.BreBD();
							LabMetro.DATA.BreDetails det = fillbre.GetBreDetails(ViewState["idBRE"].ToString());
							
							ViewState["idEmpresa"] = det.idEmpresa.ToString();
							
							btnSubmit.CommandArgument="update"; 
					
							if(Request.QueryString["errUpd"]!=null &&Request.QueryString["errUpd"]!="")  
							{
								
								if (Request.QueryString["errUpd"].ToString() == "0")
								{
									lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
								}
								else
								{
									lblMessage.Text= GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
								}
							}
							fillbre = null; 
						}
					}   
					else
					{
						if(!Page.IsPostBack)
						{
							ViewState["idBRE"] = ""; 
							//so no primeira vez, pq entretanto posso ter preenchido o bre
							ViewState["idEmpresa"] = ""; 
						}

						btnSubmit.CommandArgument="insert"; 					
						btnVerBRE.Enabled=false; 
						btnVerEtiquetas.Enabled = false; 
					}
                    
					if(!Page.IsPostBack) 
					{   
						ClearVariables();
                        FillDDFuncionario();
                        fillDDComentarios();


						if(ViewState["idBRE"].ToString()!="")

						{
                           
							fillForm(ViewState["idBRE"].ToString()); 
						}

						fillCompanyInfo();
                        
						createDGServicesDataSource();  //1º cria-se a datasource
						
                        BindGridServices();            //2º faz-se databind do dg ao datasource      
					} 
					
				}
			}    
		}

		private void createDGServicesDataSource()
		{
			DT = new DataTable();    
        
			if(ViewState["idBRE"].ToString()=="")
			{
				DT.Columns.Add(new DataColumn("idServico",typeof(string)));
				DT.Columns.Add(new DataColumn("idRequisicao",typeof(string)));
				DT.Columns.Add(new DataColumn("refRequisicao",typeof(string)));
				DT.Columns.Add(new DataColumn("idEquipamento",typeof(string))); //nao está na outra
				DT.Columns.Add(new DataColumn("numIdentificacao",typeof(string)));
				DT.Columns.Add(new DataColumn("codTipoEquipamento",typeof(string)));
				DT.Columns.Add(new DataColumn("idEstadoServico",typeof(string)));
				DT.Columns.Add(new DataColumn("estadoServico",typeof(string)));
                DT.Columns.Add(new DataColumn("idLocalDestino", typeof(string)));
				DT.Columns.Add(new DataColumn("localDestino",typeof(string)));
				DT.Columns.Add(new DataColumn("idtipoServico",typeof(string)));
				DT.Columns.Add(new DataColumn("tipoServico",typeof(string)));
				DT.Columns.Add(new DataColumn("observacoes",typeof(string)));
				DT.Columns.Add(new DataColumn("refServico",typeof(string)));
				DT.Columns.Add(new DataColumn("canEdit",typeof(string)));
				DT.Columns.Add(new DataColumn("calibExt",typeof(string))); 
				DT.Columns.Add(new DataColumn("numSerie",typeof(string))); 
				DT.Columns.Add(new DataColumn("idServicoPai",typeof(string))); 
				DT.Columns.Add(new DataColumn("refServicoPai",typeof(string)));
                DT.Columns.Add(new DataColumn("acessorios", typeof(string)));
                DT.Columns.Add(new DataColumn("bVariasGrandezas", typeof(Boolean)));
                DT.Columns.Add(new DataColumn("refServicoCertificado", typeof(string))); 


				DT.Columns["idServico"].DefaultValue = ""; //nao tenho id ate inserir na BD
				DT.Columns["canEdit"].DefaultValue = "1"; //1 se posso, 0 se nao posso
				DT.Columns["refServico"].DefaultValue =""; 
				DT.Columns["calibExt"].DefaultValue = ""; //foi adicionado ao BRECalibExt com o valor de 1, por isso aqui tem de ser adicionado tb, mas vai a vazi0.
				//preciso desta coluna pq ela vem da bd, e o datagrid espera o campo
				//PELOS VISTOS NAO FUNCIONA PARA OS MEUS OBJECTIVOS.... :(((

                DT.Columns["idServicoPai"].DefaultValue = DBNull.Value;
                DT.Columns["refServicoPai"].DefaultValue = DBNull.Value;

                DT.Columns["idLocalDestino"].DefaultValue = DBNull.Value;
                DT.Columns["localDestino"].DefaultValue = DBNull.Value;  

			}       
			else
			{
				DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
				DT = servico.DTGetServicoByBRE(ViewState["idBRE"].ToString()); 

				//neste caso, vem uma ultima coluna canedit, q me diz se posso editar uma linha ou nao.
			}
            
			ViewState["DT"] = DT; 
		}

		private void ClearVariables()
		{
			ViewState["idRequisicao"] ="";  
			ViewState["refRequisicao"] ="";  
			ViewState["idEquipamento"] =""; 
			ViewState["codTipoEquipamento"] =""; 
			ViewState["numIdentificacao"] =""; 
			ViewState["numSerie"] =""; 
			txtPesquisaNumSerie.Text =""; 
			txtPesquisaNumIdentificacao.Text =""; 
			
		}

		#region FillDropDowns 

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

		private void fillDDEmpresaContratante()
		{
            if (txtEmpresaContratante.Text.Length < 3)
            {
                lblMessage.Text = "Indique no mínimo as 3 primeiras letras da empresa contratante.";
                return; 
            }

			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtEmpresaContratante.Text,txtNifEmpresaContratante.Text,"1","","","","","",""); //activas
			DataView DV = new DataView(DT);		
			ddEmpresaContratante.DataSource = DV; ; 
			ddEmpresaContratante.DataBind();
			empresa = null;
            //ddEmpresaContratante.Items.Add(new ListItem("", "nenhuma"));  
            //tirar isso pq me dá um erro converting nvaarchar to int no ajax e agora n tenho tempo para resolver

            rbEmpbrepodevercertificados.SelectedValue = "0"; //por default nao pode ver, teem de marcar explicitamente.

            rbEmpbrepodevercertificados.Enabled = true;
        }

        private void FillDDFuncionario()
		{
           
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaFuncionarios(); 
			ddFuncionarioRecepcao.DataSource = DR2; 
			ddFuncionarioRecepcao.DataBind(); 
			
			DR2.Close(); 
			lista = null; 
             
			//DATA.GeralBD geral = new LabMetro.DATA.GeralBD();
			try
			{
				if(!Page.IsPostBack) ddFuncionarioRecepcao.SelectedValue= System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));   
			}
			catch
			{}

			//geral = null; 
			
  
		}


        private void fillDDComentarios()
        {
         //programcao à pressa
                string strSQL = "Select idComentarioBRE, descricao from comentarioBRE";
               
                ddNota.DataSource = GERAL.clsDataAccess.ExecuteDT(strSQL);
                ddNota.DataBind();
                ddNota.Items.Insert(0, new ListItem("",""));

        }
         
        

		// Preencher os dados relativos à empresa (se é devedora ou não)
        //no fim preenche tb a dd com os orçamentos da empresa E da emprsa contratante
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
            if (ViewState["idBRE"].ToString() == "") //so podem inserir da primeira vez! no fill, isto desaparece
            {
                fillDDOrcamento();
				//fillDDRequisicao();
            }
		}

		//private void fillDDRequisicao()
  //      {

		//		DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();

		//		SqlDataReader dr = data.DRGetRequisicoesIncompletasByEmpresa(ddEmpresa.SelectedValue);

		//		ddRequisicao.DataSource = dr;
		//		ddRequisicao.DataBind();
		//		//ddRequisao.Items.Insert(0, new ListItem("--", ""));

		//		dr.Close();

			

		//}

		//feito sem stp para ser mais rapido
		private void fillDDOrcamento()
        {
            string idEmpresa = ddEmpresa.SelectedValue;

            if (ddEmpresaContratante.SelectedValue != "")
            {
                idEmpresa += "," + ddEmpresaContratante.SelectedValue;
            }
            if (ddEmpresa.SelectedValue != "")
            {
                string strSQL = "Select idOrcamento, refOrcamento from orcamento where idEmpresa in (" + idEmpresa + ") and idEstadoOrcamento not in (1,5,6) order by 2 desc"; //pedido, anulado, rejeitado
                
                SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
                ddOrcamento.DataSource =DR;
                ddOrcamento.DataBind();
                DR.Close();
                
            }
            else
            {
                ddOrcamento.DataSource = null;
                ddOrcamento.DataBind();

            }
            ddOrcamento.Items.Insert(0, new ListItem("",""));
        }



		#endregion

		private void fillForm(string id)
		{
			//fillDDEmpresaContratante(); 
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

				ddEmpresa.Items.Insert(0, new ListItem(det.nomeEmpresa,det.idEmpresa)); 
				ddEmpresa.SelectedValue = det.idEmpresa; 
				ddEmpresa.DataBind(); 

				ddEmpresaContratante.Items.Insert(0, new ListItem(det.nomeEmpresaContratante,det.idEmpresaContratante)); 
				ddEmpresaContratante.SelectedValue = det.idEmpresaContratante; 
				ddEmpresaContratante.DataBind(); //aqui nao faço fill, so adiciono o seleccionado


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
            

                ddEmpresa.Enabled=false; 
				ddEmpresaContratante.Enabled=false; 
				pesquisaEmpresaContratante.Enabled=false;
				txtPesquisaEmpresa.Enabled=false;
				txtPesquisaNif.Enabled = false;
				btnEmpresas.Enabled=false;

              


                try
                {
                    ddNota.SelectedValue= det.nota;
                }
                catch
                {
                    ddNota.SelectedItem.Value = "";
                }


				txtExpedicao.Text = det.expedicao;
                
				rbReqCompleta.SelectedValue = det.requisicaoCompleta;
                txtReferenciaRequisicao.Text = det.refRequisicao;
                rbTaxaUrgencia.SelectedValue = det.bTaxaUrgencia;
                fillDDOrcamento();
                try
                {
                    ddOrcamento.SelectedValue = det.idOrcamento;
                }
                catch { }
                
				ViewState["idEmpresa"] = det.idEmpresa; 
			}

			fillbre = null; 
		}
        
    
		//bind grid dos serviços associados ao BRE
		private void BindGridServices()
		{
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT); 
			DGServicosBRE.DataSource= DV;  
			DGServicosBRE.DataBind();    
		}
    	
		#region hideGridReq - hideGridEquip
		
        private void hideGridReq()
		{
			lblRequisicao.Text =""; 
			DGRequisicoes.Dispose(); 
			DGRequisicoes.Visible=false; 
		}
        
        //private void hideGridEquip()
        //{
        //    //ViewState["idTipoEquipamento"]=""; 
        //    tblPesquisaEquipamento.Visible=false; 
        //    DGEquipamentos.Dispose(); 
        //    DGEquipamentos.Visible=false; 
        //}
		#endregion

            
		private void DGServicosBRE_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
		{
			//hideGridEquip();
			hideGridReq();
		
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT);          
            
			DV.Delete(e.Item.ItemIndex); //delete da view
			ViewState["DT"] = DT; 
			BindGridServices(); 
			DGServicosBRE.ShowFooter=true; 

		}

		private void DGServicosBRE_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
		{
			if(e.CommandName == "Insert")
			{
               
				if(e.Item.ItemType == ListItemType.Footer)
				{
					TextBox txtIdReq = (TextBox)e.Item.FindControl("txtIdRequisicaoFooter"); 
					TextBox txtRefReq = (TextBox) e.Item.FindControl("txtrefRequisicaoFooter"); 
					TextBox txtIdEquip = (TextBox)e.Item.FindControl("txtIdEquipamentoFooter"); 
					TextBox txtCodEquip = (TextBox)e.Item.FindControl("txtCodigoEquipamentoFooter"); 
					TextBox txtNumIdentificacao = (TextBox)e.Item.FindControl("txtNumIdFooter"); 
					TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerieFooter"); 

					DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoServicoFooter");
				
					DropDownList ddTipoServico = (DropDownList)e.Item.FindControl("ddTipoServicoFooter");
                    DropDownList ddLocalDestino = (DropDownList)e.Item.FindControl("ddLocalDestinoFooter");              
					DropDownList ddServicopai = (DropDownList)e.Item.FindControl("ddServicopai");
                   
           
					TextBox txtObservacoesFooter = (TextBox)e.Item.FindControl("txtObservacoesFooter");
                    TextBox txtAcessoriosFooter = (TextBox)e.Item.FindControl("txtAcessoriosFooter");
                    
                    
                    //TextBox txtRefServicoCertificadoFooter = (TextBox)e.Item.FindControl("txtRefServicoCertificadoFooter");
                    //para os certificados de espanha feitos em portugal
                    DropDownList ddServicoCertificado = (DropDownList)e.Item.FindControl("ddServicoCertificado"); 


                    CheckBox cbVariasGrandezasFooter = (CheckBox)e.Item.FindControl("cbVariasGrandezasFooter"); 
					
					if (txtIdEquip.Text=="") 
					{
                        lblMessageServicos.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_EQUIPAMENTO; 
						return;
					}

                    if (ddTipoServico.SelectedValue == "")
                    {
                        lblMessageServicos.Text += "Indicar tipo de Serviço";
                        return;
                    }

					else if(ddTipoServico.SelectedValue =="A")
					{
						if(ddServicopai.SelectedValue=="")
						{
                            lblMessageServicos.Text += GERAL.clsGeral.ErrorMessage.MSG_INDICAR_PAI_ADITAMENTO;
							return; 
						}
					}
					else
					{
						if(ddServicopai.SelectedValue!="")
						{
                            lblMessageServicos.Text += GERAL.clsGeral.ErrorMessage.MSG_REMOVER_PAI; 
							return; 
						}
					}
				
					DT = (DataTable)ViewState["DT"]; 
					DV = new DataView(DT); 

					DataRowView drv;
					drv = DV.AddNew(); 
					drv["idRequisicao"] = txtIdReq.Text.ToString();
					drv["refRequisicao"] = txtRefReq.Text.ToString();
					drv["idEquipamento"] = txtIdEquip.Text.ToString(); 
					drv["codTipoEquipamento"] = txtCodEquip.Text.ToString(); 
					drv["numIdentificacao"] = txtNumIdentificacao.Text.ToString(); 
					drv["idEstadoServico"] = ddEstado.SelectedValue.ToString(); 
					drv["estadoServico"] = ddEstado.SelectedItem.Text.ToString(); 
					drv["idtipoServico"] = ddTipoServico.SelectedValue.ToString(); 

					drv["tipoServico"] = ddTipoServico.SelectedItem.Text.ToString();


                    if (ddLocalDestino.SelectedValue != "")
                    {
                        drv["idLocalDestino"] = ddLocalDestino.SelectedValue.ToString();
                        drv["localDestino"] = ddLocalDestino.SelectedItem.Text.ToString();
                    }
                    else
                    {
                        drv["idLocalDestino"] = DBNull.Value;
                        drv["localDestino"] = DBNull.Value;
                    }

					 
					drv["observacoes"] = txtObservacoesFooter.Text.ToString();     
					drv["numSerie"] = txtNumSerie.Text.ToString();
                    drv["acessorios"] = txtAcessoriosFooter.Text.ToString();
                    drv["refServicoCertificado"] = ddServicoCertificado.SelectedItem.Text;//txtRefServicoCertificadoFooter.Text.ToString();

                    if (ddServicopai.SelectedValue != "")
                    {
                        drv["idServicoPai"] = ddServicopai.SelectedValue.ToString();
                        drv["refServicoPai"] = ddServicopai.SelectedItem.Text.ToString();
                    }
                    else
                    {
                        drv["idServicoPai"] = DBNull.Value;
                        drv["refServicoPai"] = DBNull.Value;
                    }

                    //*******************
                    string calibExt = "0";
                    if (ddEstado.SelectedValue == "2") calibExt = "1"; 
                    drv["calibExt"] = calibExt;
                    //*******************

                    //*******************

                    drv["bVariasGrandezas"] = cbVariasGrandezasFooter.Checked;
                    //*******************

        			drv.EndEdit(); 
                    
					ViewState["DT"] = DT; 

					//remover to viewstate os ids do idequi e id ....
					ClearVariables(); 
				
				}

				DGServicosBRE.EditItemIndex = -1; 
				BindGridServices(); 
			}
		}



		//selecciona um id de equipamento e passa para uma variavel hidden
		protected void DGEquipamentos_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName=="Select")
			{    
				ViewState["idEquipamento"]  = DGEquipamentos.DataKeys[e.Item.ItemIndex].ToString();   
				ViewState["codTipoEquipamento"]  = e.Item.Cells[2].Text.ToString(); 
				ViewState["numIdentificacao"]  = e.Item.Cells[6].Text.ToString(); 
				ViewState["numSerie"]  = e.Item.Cells[4].Text.ToString(); 
				//é um campo invisivel pq nao consigo aceder ao valor q está dentro de uma templatecolumn
                
				
				BindGridServices(); 
			}
			if(e.CommandName=="Insert")
			{
               
				TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerieFoter"); 
				TextBox txtNumIdentificacao = (TextBox)e.Item.FindControl("txtNumIdentificacaoFooter"); 
                    
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoFooter");
                
				if((txtNumIdentificacao.Text.Length == 0 && txtNumSerie.Text.Length ==0) ||(txtNumIdentificacao.Text.ToString()== "---" && txtNumSerie.Text.ToString() =="---") ||(txtNumIdentificacao.Text.ToString() =="---" && txtNumSerie.Text.Length==0) || (txtNumSerie.Text.ToString() == "---" && txtNumIdentificacao.Text.Length==0))
				{
                    lblMessageServicos.Text = GERAL.clsGeral.ErrorMessage.MSG_PREENCHER_NUMSERIE_OU_NUMIDENT; 
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

					DGEquipamentos.CurrentPageIndex = 0; 
					BindGridEquipamentos();
                    
				}
			}
		}

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
				ddTipoEquipamento.Items.Insert(0,new ListItem("","")); 
					
				//ddTipoEquipamento.Items.FindByValue(ViewState["idTipoEquipamento"].ToString()).Selected = true; 	
			}
		}


		//selecciona um id de requisição e passa para uma variavel hidden
		protected void DGRequisicoes_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName=="SelectReq")
			{	
				ViewState["idRequisicao"]  = DGRequisicoes.DataKeys[e.Item.ItemIndex].ToString();   
				ViewState["refRequisicao"] = e.Item.Cells[2].Text.ToString(); 
				BindGridServices(); 
			}
		}

        private void UpdateEstadoOrcamento() //so para ESPANHA!!!! para portugal nao dá por causa do PSI
        {
          


                string idOrcamento = ddOrcamento.SelectedValue;
                string strSQL = "Update ORCAMENTO set idEstadoOrcamento = " + ddEstadoOrcamento.SelectedValue + " where idOrcamento = " + idOrcamento;
                GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            
        }


		private void InsertBD()
		{
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT); 

			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
            
			string bDefinitivo = "1"; //hardcoded, aqui é sempre definitivo
			//no BRE para calibrações externas é q supostamente os equipamentos nao sao definitivos, 
			//podendo ser trocados.

			int idBRE = bre.InsertBreWithServices(ddEmpresa.SelectedValue.ToString(), ddFuncionarioRecepcao.SelectedValue.ToString(), rbReqCompleta.SelectedValue.ToString(),txtEntreguePor.Text.ToString(), txtObservacoes.Text.ToString(),User.Identity.Name.ToString(),DV,bDefinitivo,txtExpedicao.Text, ddEmpresaContratante.SelectedValue,ddOrcamento.SelectedValue, txtReferenciaRequisicao.Text, ddNota.SelectedItem.Value, rbEmpbrepodevercertificados.SelectedValue.ToString(), rbTaxaUrgencia.SelectedValue.ToString()); 
			bre = null;

            if (ddEstadoOrcamento.SelectedIndex > 0)
            {
                if (myApp == "ES_LABMETRO")
                {
                    UpdateEstadoOrcamento(); //so para ESPANHA!!!!
                }
            }
			
			if(idBRE != 0) 
			{
				Response.Redirect("FormBRE.aspx?btn=DOC&id="+idBRE,true);
			}
			else
			{
				lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
			}
		}
       
		private void UpdateBD()
		{
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT); 

			DATA.BreBD bre = new LabMetro.DATA.BreBD();
            
			string bDefinitivo = "1"; //hardcoded, aqui é sempre definitivo
			
			//a mensagem desaparece pq é chamado o redirect a seguir.
			
			string retValue = bre.UpdateBreWithServices(ViewState["idBRE"].ToString(),ViewState["idEmpresa"].ToString(), ddFuncionarioRecepcao.SelectedValue.ToString(), rbReqCompleta.SelectedValue.ToString(),txtEntreguePor.Text.ToString(), txtObservacoes.Text.ToString(),User.Identity.Name.ToString(),DV,bDefinitivo,txtExpedicao.Text,ddOrcamento.SelectedValue, txtReferenciaRequisicao.Text, ddNota.SelectedItem.ToString(), rbEmpbrepodevercertificados.SelectedValue.ToString(), rbTaxaUrgencia.SelectedValue.ToString());
            bre = null; 

            if (ddEstadoOrcamento.SelectedIndex > 0)
            {
                if (myApp == "ES_LABMETRO")
                {
                    UpdateEstadoOrcamento(); //so para ESPANHA!!!!
                }
            }

			
        
			Response.Redirect("FormBRE.aspx?btn=DOC&errUpd="+retValue+"&id="+ViewState["idBRE"].ToString(),true); 
		}


		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			//pq a pagina no submit mostra os 3 datagrids:
			//hideGridEquip(); 
			hideGridReq();



            //if (myApp.ToUpper() == "ISQ_LABMETRO" && (ddEmpresaContratante.SelectedValue == "28966" || ddEmpresaContratante.SelectedValue == "40955"))
            //{
            //    DT = (DataTable)ViewState["DT"];
            //    DV = new DataView(DT);
            //    foreach (DataRowView drv in DV)
            //    {

            //        string refServicoCertificado = drv["refServicoCertificado"].ToString();

            //        if (refServicoCertificado.Length < 8)//outra maneira de ver se isto está bem preenchido
            //        {
            //            lblMessageServicos.Text = "Atenção, preencher todas as referências dos serviços de Espanha!"; //isto está no loop, mas basta escrever 1 vez
            //            return;
            //        }
            //    }
            //}
                

			if(btnSubmit.CommandArgument=="insert")
			{
				if(Page.IsValid)
				{
                   
					InsertBD();
				}
			}
			else if(btnSubmit.CommandArgument=="update")
			{
				if(Page.IsValid)
				{
					UpdateBD(); 
				}
			}
		}


	

		private void clearCompanyRelatedData() //pq a empresa pode mudar agora embora nao mude o selectedIndex da dropdown.
		{
		
			// Preencher os dados relativos à empresa (se é devedora ou não)
			fillCompanyInfo();
			//hideGridEquip(); 
			hideGridReq(); 
            
			ClearVariables();
            
			createDGServicesDataSource(); 
			BindGridServices();
            ddTipoEquipamentoPesquisa.DataSource = null;
            ddTipoEquipamentoPesquisa.DataBind(); 
            DGEquipamentos.DataSource = null;
            DGEquipamentos.DataBind(); 
		}

		protected void DoPagingEquipamentos(Object s,DataGridPageChangedEventArgs e)
		{            
			DGEquipamentos.CurrentPageIndex = e.NewPageIndex;
			BindGridEquipamentos(); 
		}
        
		protected void DoPagingRequisicoes(Object s,DataGridPageChangedEventArgs e)
		{
			DGRequisicoes.CurrentPageIndex=e.NewPageIndex;
			BindGridRequisicoes(); 
            
		}

		//preenche e mostra datagrid requisições
		//esta funcao é chamada quando o grid das requisicoes é databound pela primeira vez:
		protected void BindGridRequisicoes(object sender, System.EventArgs e)
		{
			DGRequisicoes.CurrentPageIndex=0; 
		
			DATA.RequisicaoBD requisicoes = new LabMetro.DATA.RequisicaoBD(); 
            
			DTRequisicoes = requisicoes.DTGetRequisicoesIncompletasByEmpresa(ddEmpresa.SelectedValue); 
			DVRequisicoes = new DataView(DTRequisicoes) ; 
            
			if(DVRequisicoes.Table.Rows.Count == 0) 
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NAO_EXISTEM_REQUISICOES;
			}
			else
			{
				DGRequisicoes.DataSource = DVRequisicoes; 
				DGRequisicoes.DataBind(); 
				DGRequisicoes.Visible=true; 
			}

			ViewState["DTRequisicoes"] =DTRequisicoes;  

			//hideGridEquip(); 

			requisicoes = null; 
		}
        
        
		//tem de haver 2 por causa dos eventhandlers ou seja, parametros
		//este é chamado cada vez q faz paging ou sorting sobre os dados q ja la estao
		protected void BindGridRequisicoes() 
		{
			DTRequisicoes=(DataTable)ViewState["DTRequisicoes"]; 
			DVRequisicoes=new DataView(DTRequisicoes); 
			lblRequisicao.Text ="Requisições da empresa:"; 
             
			if(DVRequisicoes.Table.Rows.Count == 0) 
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NAO_EXISTEM_REQUISICOES;
			}
			else
			{
				DGRequisicoes.DataSource = DVRequisicoes; 
				DGRequisicoes.DataBind(); 
				DGRequisicoes.Visible=true; 
			}

			//hideGridEquip(); 
		}

		//preenche e mostra datagrid equipamentos
		//preenchido na primeira vez.
		protected void BindGridEquipamentos(object sender, System.EventArgs e)
		{
			

			//esta funcao é chamada quando o grid das requisicoes é databound pela primeira vez:
			ViewState["sortField"] = "tipoEquipamento";
			ViewState["sortDirection"] = "ASC";
			//==============================================
			DGEquipamentos.CurrentPageIndex=0; 

            //tblPesquisaEquipamento.Visible = true; 
            //txtPesquisaRefCalibracao.Visible = true; 
            //txtPesquisaNumIdentificacao.Visible=true; 
            //txtPesquisaNumSerie.Visible=true; 
            //ddTipoEquipamentoPesquisa.Visible =true; 

			fillDDTipoEquipamentoPesquisa();

            btnPesquisaEquipamentoEmpresa.Enabled = true;

            hideGridReq();
			

        
		}

		//tem de haver 2 por causa dos eventhandlers ou seja, parametros
		//este é chamado cada vez q faz paging ou sorting sobre os dados q ja la estao
		private void BindGridEquipamentos() //alteração última hora
		{
			DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 

			DTEquipamentos = equipamento.DTGetEquipamentosActivosByEmpresa(ddEmpresa.SelectedValue,"", txtPesquisaRefCalibracao.Text,txtPesquisaNumIdentificacao.Text,txtPesquisaNumSerie.Text, ddTipoEquipamentoPesquisa.SelectedValue); //ViewState["idTipoEquipamento"].ToString()); 
          
			if(DTEquipamentos.Rows.Count ==0) lblMessage.Text= "Não existem equipamentos para esta empresa, tem de adicionar um equipamento primeiro."; 
            
			DataView DVEquipamentos = new DataView(DTEquipamentos); 
			DVEquipamentos.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

			if(DVEquipamentos.Table.Rows.Count ==0) lblMessage.Text= "Não existem equipamentos correspondentes aos critérios de pesquisa introduzidos.";

			DGEquipamentos.DataSource = DVEquipamentos; 
			DGEquipamentos.DataBind(); 
			//DGEquipamentos.Visible=true; 
			hideGridReq(); 

			equipamento = null; 
		}
       
		protected void DGServicosBRE_Edit(Object sender, DataGridCommandEventArgs e)     
		{
			DGServicosBRE.ShowFooter=false; 
			DGServicosBRE.EditItemIndex = e.Item.ItemIndex;	
			BindGridServices(); 
		}

		protected void DGServicosBRE_CancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DGServicosBRE.ShowFooter=true;  
			DGServicosBRE.EditItemIndex = -1;
			BindGridServices();
		}
		
		protected void DGServicosBRE_UpdateGrid(Object sender, DataGridCommandEventArgs e)
		{
			DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");
            DropDownList ddLocalDestino = (DropDownList)e.Item.FindControl("ddLocalDestinoEdit");
            DropDownList ddTipoServico = (DropDownList)e.Item.FindControl("ddTipoServicoEdit");
            DropDownList ddServicopai = (DropDownList)e.Item.FindControl("ddServicopaiEdit");

			TextBox txtObs = (TextBox) e.Item.FindControl("txtObservacoesEdit");
            TextBox txtAcessorios = (TextBox)e.Item.FindControl("txtAcessoriosEdit");
           // TextBox txtRefServicoCertificado = (TextBox)e.Item.FindControl("txtRefServicoCertificadoEdit");

            DropDownList ddServicoCertificado = (DropDownList)e.Item.FindControl("ddServicoCertificadoEdit");

            CheckBox cbVariasGrandezas = (CheckBox)e.Item.FindControl("cbVariasGrandezasEdit");


            if(ddTipoServico.SelectedValue =="A")
			{
				if(ddServicopai.SelectedValue=="")
				{
                    lblMessageServicos.Text += GERAL.clsGeral.ErrorMessage.MSG_INDICAR_PAI_ADITAMENTO;
					return; 
				}
			}
			else
			{
				if(ddServicopai.SelectedValue!="")
				{
                    lblMessageServicos.Text += GERAL.clsGeral.ErrorMessage.MSG_REMOVER_PAI; 
					return; 
				}
			}
        
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT); 

			int index = e.Item.ItemIndex; 
			DV[index]["observacoes"] = txtObs.Text; 
			DV[index]["idRequisicao"] = ddRequisicao.SelectedValue.ToString(); 
			DV[index]["refRequisicao"] = ddRequisicao.SelectedItem.Text.ToString();

            if (ddLocalDestino.SelectedValue != "")
            {

                DV[index]["idLocalDestino"] = ddLocalDestino.SelectedValue.ToString();
                DV[index]["localDestino"] = ddLocalDestino.SelectedItem.Text.ToString();
            }
            else//apesar de dizer que é string, a dataview espera sempre um numero
            {
                DV[index]["idLocalDestino"] = DBNull.Value;
                DV[index]["localDestino"] = DBNull.Value;
            }

            DV[index]["idTipoServico"] = ddTipoServico.SelectedValue.ToString();
            DV[index]["tipoServico"] = ddTipoServico.SelectedItem.Text.ToString();
            DV[index]["acessorios"] = txtAcessorios.Text;
            DV[index]["refServicoCertificado"] = ddServicoCertificado.SelectedValue.ToString();//txtRefServicoCertificado.Text;
            
            //verificar
            DV[index]["bVariasGrandezas"] = cbVariasGrandezas.Checked;

            if (ddServicopai.SelectedValue != "")
            {
                DV[index]["idServicoPai"] = ddServicopai.SelectedValue.ToString();
                DV[index]["refServicoPai"] = ddServicopai.SelectedItem.Text.ToString();
            }
            else
            {
                DV[index]["idServicoPai"] = DBNull.Value;
                DV[index]["refServicoPai"] = DBNull.Value;
            
            }
			DV.EndInit(); 
			ViewState["DT"] = DT; 
        
			DGServicosBRE.EditItemIndex = -1; 
			BindGridServices(); 
			DGServicosBRE.ShowFooter=true; 
       
		}        
       
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

		private void btnPesquisaEquipamentoEmpresa_Click(object sender, System.EventArgs e)
		{
			DGEquipamentos.CurrentPageIndex =0;
            DGEquipamentos.SelectedIndex = -1; 
			BindGridEquipamentos(); 
 
		}

		//        //************** COISAS NOVAS ********************************************//

		private void DGServicosBRE_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item ||e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;
				if(DRV["canEdit"].ToString()== "0")
				{
					LinkButton button =(LinkButton)e.Item.Cells[15].Controls[0];
					button.Enabled =false; 

					LinkButton button2 =(LinkButton)e.Item.Cells[16].Controls[0];
					button2.Enabled =false; 
				}
			}

			else if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;

				DATA.RequisicaoBD requisicao = new LabMetro.DATA.RequisicaoBD(); 
				//ver se isto funciona
				if(ViewState["idEmpresa"].ToString()=="") ViewState["idEmpresa"] = ddEmpresa.SelectedValue.ToString(); 

				SqlDataReader DR = requisicao.DRGetRequisicoesIncompletasByEmpresa(ViewState["idEmpresa"].ToString()); 
				DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");
				ddRequisicao.DataSource= DR; 
				ddRequisicao.DataBind(); 
				ddRequisicao.Items.Insert(0, new ListItem("","")); 
				DR.Close(); 

				requisicao = null; 

				
				string idRequisicao = DRV["idRequisicao"].ToString();      
				if(idRequisicao !="") ddRequisicao.SelectedValue = idRequisicao;

                DropDownList ddLocalDestino = (DropDownList)e.Item.FindControl("ddLocalDestinoEdit");
                
				DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
				SqlDataReader DR2 = lista.DRListaLocalCalibracao(); //isto fica igual pq os locais possiveis sao sempre os mesmos.
                ddLocalDestino.DataSource = DR2;
                ddLocalDestino.DataBind(); 
				DR2.Close();
                ddLocalDestino.Items.Insert(0,new ListItem("----", "")); 

                foreach (ListItem i in ddLocalDestino.Items)
				{                
					i.Text= i.Text.Substring(0,3)+"."; 
				} 

                //novo jan 2011
                DropDownList ddTipoServico= (DropDownList)e.Item.FindControl("ddTipoServicoEdit");
                SqlDataReader DR3 = lista.DRListaTipoServico();
                ddTipoServico.DataSource = DR3;
                ddTipoServico.DataBind();
                DR3.Close();
                lista = null;

                ddTipoServico.SelectedValue = DRV["idTipoServico"].ToString();

               
                DropDownList ddServicoPai = (DropDownList)e.Item.FindControl("ddServicopaiEdit");

                string idEquipamento = DRV["idEquipamento"].ToString();
                string strSQL = "SELECT idServico, refServico from servico where idEquipamento = " + idEquipamento + " AND idTipoServico <>'A' ";

                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
                ddServicoPai.DataSource = dr;
                ddServicoPai.DataBind();
                ddServicoPai.Items.Insert(0, new ListItem("", ""));

                dr.Close();

                ddServicoPai.SelectedValue = DRV["idServicoPai"].ToString();
            
                 if (ViewState["idBRE"].ToString() != "")
                {
                    ddTipoServico.Enabled = false;
                    ddServicoPai.Enabled = false; 
                }


                //end de novo 

                //julho 2020 
                // 28966 nova - 32379 antiga
                // set 2020 - 28966,                 40955

                DropDownList ddServicoCertificado = (DropDownList)e.Item.FindControl("ddServicoCertificadoEdit");

                 //if (myApp.ToUpper() == "ISQ_LABMETRO" && (ddEmpresaContratante.SelectedValue == "28966" || ddEmpresaContratante.SelectedValue == "40955" ))
                 //{
                 //    strSQL = "SELECT refServico from servico where idestadoServico in (4,5)"; // em subcontratacao
                 //    //isso com a connectionstring de espanha

                 //    dr = GERAL.clsDataAccess.ExecuteDREspanha(strSQL);
                 //    ddServicoCertificado.DataSource = dr;
                 //    ddServicoCertificado.DataBind();
                 //    ddServicoCertificado.Items.Insert(0, new ListItem("", ""));

                 //    dr.Close();

                 //    try
                 //    {
                 //        ddServicoCertificado.SelectedValue = DRV["refServicoCertificado"].ToString();
                 //    }
                 //    catch
                 //    {
                 //        ddServicoCertificado.Items.Add(new ListItem(DRV["refServicoCertificado"].ToString(), DRV["refServicoCertificado"].ToString()));
                 //    }
                 //}
                 //else
                 //{
                     ddServicoCertificado.Items.Add(new ListItem(DRV["refServicoCertificado"].ToString(), DRV["refServicoCertificado"].ToString()));

                // }

                 string idLocalDestino = DRV["idLocalDestino"].ToString();
                 if (idLocalDestino != "") ddLocalDestino.SelectedValue = idLocalDestino; 
                
			} 
			if(e.Item.ItemType == ListItemType.Footer) 
			{

				DropDownList ddServicoPai = (DropDownList) e.Item.FindControl("ddServicopai");

				
				string idEquipamento = ViewState["idEquipamento"].ToString(); 
				if(idEquipamento !="")
				{
					string strSQL = "SELECT idServico, refServico from servico where idEquipamento = " + idEquipamento  + " AND idTipoServico <>'A' "; 

					SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
					ddServicoPai.DataSource = dr;
					ddServicoPai.DataBind(); 
					ddServicoPai.Items.Insert(0,new ListItem("",""));
                    dr.Close(); 
				}

				TextBox txtIdReq = (TextBox) e.Item.FindControl("txtIdRequisicaoFooter");                                           
				txtIdReq.Text= ViewState["idRequisicao"].ToString(); 
                
				TextBox txtRefReq = (TextBox) e.Item.FindControl("txtrefRequisicaoFooter");                                           
				txtRefReq.Text= ViewState["refRequisicao"].ToString();  
                 
				hideGridReq(); 
                
				TextBox txtIdEquip = (TextBox) e.Item.FindControl("txtIdEquipamentoFooter");                                           
				txtIdEquip.Text= ViewState["idEquipamento"].ToString();  
               
				TextBox txtCodigoEquip = (TextBox) e.Item.FindControl("txtCodigoEquipamentoFooter");                                           
				txtCodigoEquip.Text =ViewState["codTipoEquipamento"].ToString();  

				TextBox txtNumIdentificacao = (TextBox) e.Item.FindControl("txtNumIdFooter"); 
				txtNumIdentificacao.Text=ViewState["numIdentificacao"].ToString(); 

				TextBox txtNumSerie = (TextBox) e.Item.FindControl("txtNumSerieFooter"); 
				txtNumSerie.Text=ViewState["numSerie"].ToString(); 
                
				//hideGridEquip(); 

				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddestadoServicoFooter");
                
				DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
                
				SqlDataReader DR3 = servico.DRGetListaEstadosServico(""); 
				ddEstado.DataSource = DR3; 
				ddEstado.DataBind(); 
                
				DR3.Close(); 
				servico = null; 

				DropDownList ddTipoServico = (DropDownList)e.Item.FindControl("ddTipoServicoFooter");
				
				DATA.ListasBD lista = new LabMetro.DATA.ListasBD();    
				
				SqlDataReader DR4 = lista.DRListaTipoServico(); 
				ddTipoServico.DataSource = DR4; 
				ddTipoServico.DataBind(); 
				DR4.Close(); 

                
                ddTipoServico.Items.Insert(0, new ListItem("-----", ""));
                
				foreach(ListItem i in ddTipoServico.Items)
				{
					i.Text= i.Text.Substring(0,5)+"."; 
				}
                
				//ddTipoServico.SelectedValue="C";

                DropDownList ddLocalDestino = (DropDownList)e.Item.FindControl("ddLocalDestinoFooter");

				SqlDataReader DR5 = lista.DRListaLocalCalibracao(); 
				ddLocalDestino.DataSource = DR5; 
				ddLocalDestino.DataBind(); 
				DR5.Close();
                ddLocalDestino.Items.Insert(0, new ListItem("----", "")); 


				lista = null;

                foreach (ListItem i in ddLocalDestino.Items)
				{                
					i.Text= i.Text.Substring(0,3)+"."; 
				}


                DropDownList ddRefServicoCertificado = (DropDownList)e.Item.FindControl("ddServicoCertificado");

               //esta dd fica preenchida caso o pais é portugal e caso o idEmpresaContratante é do labmetro espanha.

                //if (myApp.ToUpper() == "ISQ_LABMETRO" && (ddEmpresaContratante.SelectedValue == "28966" || ddEmpresaContratante.SelectedValue == "40955" ))
                //{

                //    string strSQL = "SELECT refServico from servico where idestadoServico in (4,5)"; // aguarda subcontratacao 
                //    //isso com a connectionstring de espanha
                //    SqlDataReader drCertifEspanha = GERAL.clsDataAccess.ExecuteDREspanha(strSQL);

                //    ddRefServicoCertificado.DataSource = drCertifEspanha;
                //    ddRefServicoCertificado.DataBind();
                    
                //    drCertifEspanha.Close();

                //}
                ddRefServicoCertificado.Items.Insert(0, new ListItem("", ""));
			}
		}
      
		private void btnVerBRE_Click(object sender, System.EventArgs e)
		{
           
            ReportClass report = null;
            

            switch (myApp)
            {
                case "ISQ_LABMETRO":
                    report = new LabMetro.REPORTS.crBRE();
                    break;
                case "ANG_LABMETRO":
                    report = new LabMetro.REPORTS_ANG.crBRE();
                    break;
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crBRE();
                    break;
                case "CV_LABMETRO":
                    report = new LabMetro.REPORTS_CV.crBRE();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.crBRE();
                    break;
                case "SON_LABMETRO":
                    report = new LabMetro.REPORTS_SON.crBRE();
                    break;
                    ;
                     case "BR_LABMETRO":
                    report = new LabMetro.REPORTS_BR.crBRE();
                    break;
                    ;
            }
            
            clsReport cr = new clsReport();

            DATA.BreBD bre = new LabMetro.DATA.BreBD();
            DataSet ds = bre.DSBRE(ViewState["idBRE"].ToString());
            report.SetDataSource(ds);
            ds = null;
            bre = null;
           
			cr.exportReportToPDF_NEW(report,"BRE"); //dps disto, nada mais é executado
		}


		//falta passar este para desconnectado
		private void btnVerEtiquetas_Click(object sender, System.EventArgs e)
		{
            ReportClass report = null;

            switch (myApp)
            {
                case "ISQ_LABMETRO":
                    report = new LabMetro.REPORTS.crEtiquetaCal();
                    break;
                case "ANG_LABMETRO":
                    report = new LabMetro.REPORTS.crEtiquetaCal();
                    break;
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crEtiquetaCal();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.crEtiquetaCal();
                    break;
                case "CV_LABMETRO":
                    report = new LabMetro.REPORTS_CV.crEtiquetaCal();
                    break;
                case "SON_LABMETRO":
                    report = new LabMetro.REPORTS_SON.crEtiquetaCal();
                    break;
                     case "BR_LABMETRO":
                    report = new LabMetro.REPORTS_SON.crEtiquetaCal();
                    break;
            }

            clsReport cr = new clsReport();

			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
			DataSet ds  = bre.DSEtiqueta(ViewState["idBRE"].ToString()); 	
				
			report.SetDataSource(ds);
            ds = null;
            bre = null;
			cr.exportReportToPDF(report,"Etiqueta");
				
			
            //cr = null; 
            //report = null;
			 
		}

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

        private void changeCompanyRelatedAjaxStuff()//new
        {
            fillDDEmpresa();
            DGEquipamentos.DataSource = null;
            DGEquipamentos.DataBind();
            fillDDTipoEquipamentoPesquisa(); 
        }

		private void btnEmpresas_Click(object sender, System.EventArgs e)
		{
            changeCompanyRelatedAjaxStuff();

		}

        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            clearCompanyRelatedData();
            //changeCompanyRelatedAjaxStuff();

        }


        protected void txtEmpresaTextChanged(object sender, System.EventArgs e)
        {
        
            changeCompanyRelatedAjaxStuff();
        }

        protected void txtNifTextChanged(object sender, System.EventArgs e)
        {

            changeCompanyRelatedAjaxStuff();
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

		private void fillDDTipoEquipamentoPesquisa()
		{
			DATA.BreBD bre = new DATA.BreBD();
			ddTipoEquipamentoPesquisa.DataSource = bre.DRTiposEquipamentoByEmpresa(ddEmpresa.SelectedValue); 
			ddTipoEquipamentoPesquisa.DataBind(); 
			ddTipoEquipamentoPesquisa.Items.Insert(0, new ListItem("","")); 
			bre = null;
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

		private void pesquisaEmpresaContratante_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresaContratante();
			fillCompanyInfo();
		}

		protected void ddEmpresaContratante_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillCompanyInfo();
            rbEmpbrepodevercertificados.SelectedValue = "0"; //por default nao pode ver, teem de marcar explicitamente.

        }

		protected void txtEmpresaContratante_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresaContratante();
			fillCompanyInfo();
		}

        



        //comentado por motivos de performance
        //para isto funcionar, tenho de substituir o codigo existente 
        //da dropdownlist no footer do datagrid pelo seguinte
        //<asp:DropDownList ID="ddTipoEquipamentoFooter" Runat="server" DataTextField="descricao" DataValueField="ident"	AutoPostBack="True" OnSelectedIndexChanged="ddTipoEquipamento_SelectedIndexChanged"></asp:DropDownList>
        //		protected void ddTipoEquipamento_SelectedIndexChanged(object sender, EventArgs e)
        //		{
        //			DropDownList ddList = (DropDownList)sender;
        //			
        //			ViewState["idTipoEquipamento"] = ddList.SelectedValue.ToString();  
        //		
        //			DGEquipamentos.CurrentPageIndex=0; 
        //			BindGridEquipamentos(); 
        //		}
    }
}
