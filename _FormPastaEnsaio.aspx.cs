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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using LabMetro.DATA;

namespace LabMetro
{

    public partial class FormPastaEnsaio : System.Web.UI.Page
    {
		
		private const string ID_PAG = "PASTASENSAIO_1";//NOME DA PAGINA
		
		DataView dvLinhasServico;
		DataView dvCertificados; 
		DataTable dtLinhasServico;
		DataTable dtCertificados;

        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

  
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Page.Form.DefaultButton = this.btnSubmit.UniqueID;

			Response.Expires = 0; 	//nao tirar!!!

			this.lblMessage.Text="";
			this.lblTipoEquipamento.Text = "";

            if (myApp.ToUpper() == "ES_LABMETRO")
            {
                this.ddEstadoRelacaoCalibracao.Enabled = true;
            }
            else
            {
                this.ddEstadoRelacaoCalibracao.Enabled = false;
            }

            Hashtable ht = (Hashtable)this.Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				//this.Response.Redirect("Default.aspx?err=2",true);            
				this.Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					//this.Response.Redirect("Default.aspx?err=1",true);//user n.tem permissoes                             
					this.Server.Transfer("Default.aspx?err=1",false);
				}
                else
                {
                    int intAcesso = System.Convert.ToInt32(ht[ID_PAG]); 
                    if(intAcesso ==0) //so tem permissoes de leitura
                    {
                        this.btnSubmit.Enabled=false;
                    }
                    
                    if(this.Request.QueryString["id"]!=null)
                    {
                        if(this.Request.QueryString["id"]!="")
                        {
                            this.ViewState["idServico"] = this.Request.QueryString["id"].ToString();

                            if(!this.Page.IsPostBack)
                            {                        
                                this.FillDropDowns(); 
                                this.fillForm(this.ViewState["idServico"].ToString());  
								this.fillCompanyInfo(); 
								this.BindGridRequisicoes(); 
								
								//DO DGOrcamentos ORÇAMENTO
								this.ViewState["sortField"] = "idOrcamento";
								this.ViewState["sortDirection"] = "DESC";
								this.BindGrid(); //Orçamentos
                            }  
                        }   
                    }
                    else
                    {
                        if(!this.Page.IsPostBack)
                        {	
							
                            FillDropDowns(); 
							//DO DGOrcamentos ORÇAMENTO
							this.ViewState["sortField"] = "idOrcamento";
							this.ViewState["sortDirection"] = "DESC";
                        } 
                    }
                }
            }

           
        }

		
        private void BindGridHistorico(string idServico)
        {   
            DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD(); 

            //this.dgHistorico.DataSource = servicos.DTGetServicoHistoricoEstados(this.ViewState["idServico"].ToString()); 
			this.dgHistorico.DataSource = servicos.DTGetServicoHistoricoEstados(idServico); 
            this.dgHistorico.DataBind();

			servicos = null; 
        }

        private void BindGridCertificados()
        {   

            dtCertificados = (DataTable)this.ViewState["dtCertificados"];
            dvCertificados = new DataView(dtCertificados); 
            this.dgCertificados.DataSource = dvCertificados; 
            this.dgCertificados.DataBind();
        }

        private void BindGridLinhasServico()
        {
            dtLinhasServico = (DataTable)this.ViewState["dtLinhasServico"];
            dvLinhasServico = new DataView(dtLinhasServico); 
            this.DGLinhasServico.DataSource = dvLinhasServico;
            this.DGLinhasServico.DataBind();
        }

        private void FillDropDowns()
        {
            this.fillLocalActual();
            this.fillEstados();
            this.fillFuncTreino();
            this.fillAlcanceClasse();
            fillNivelPrioridade();
            fillEstadoRelacaoCalibracao();
			//this.fillRazoes(); vai para dentro do fillform, so pode ser chamado depois da ddestado ter sido preenchida
        }

        private void fillNivelPrioridade()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader dr = lista.DRListaNivelPrioridade();
            this.ddNivelPrioridade.DataSource = dr;
            this.ddNivelPrioridade.DataBind();

            dr.Close();

            lista = null;
        }

        private void fillMarcas()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR =  lista.DRListaMarcas(); 
            this.ddMarca.DataSource = DR;
            this.ddMarca.DataBind(); 
            this.ddMarca.Items.Insert(0,new ListItem("",""));
            DR.Close(); 

			lista = null; 
        }

        
        private void fillModelos()
        {
            if(this.ddMarca.SelectedIndex >0)
            {
                DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
                SqlDataReader DR = lista.DRListaModelos(this.ddMarca.SelectedValue); 
                this.ddModelo.DataSource= DR;
                this.ddModelo.DataBind(); 
                this.ddModelo.Items.Insert(0,new ListItem("",""));
                DR.Close(); 

				lista = null;
            }
        }

        private void fillLocalActual()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaLocalCalibracao();
            this.ddLocalActual.DataSource = DR;
            this.ddLocalActual.DataBind();
            DR.Close(); 

			lista = null;
        }

        private void fillFuncTreino()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaFuncionarios(); 
            this.ddFuncTreino.DataSource = DR;
            this.ddFuncTreino.DataBind();
            this.ddFuncTreino.Items.Insert(0,new ListItem("",""));
            DR.Close(); 

			lista = null;
        }

        private void fillEstados()
        {
            DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
            SqlDataReader DR =  servico.DRGetListaEstadosServico(this.ViewState["idServico"].ToString()); 
            this.ddEstado.DataSource =DR; 
            this.ddEstado.DataBind(); 
            DR.Close(); 		
			servico = null;
			
        }

        private void fillEstadoRelacaoCalibracao()
        {
           string strSQL = "SELECT [idEstadoRelacaoCalibracao]     ,[estadoRelacaoCalibracao] as descricao FROM [dbo].[EstadoRelacaoCalibracao]"; 
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
               
            this.ddEstadoRelacaoCalibracao.DataSource = DR;
            this.ddEstadoRelacaoCalibracao.DataBind();
            DR.Close();
          

        }

		private void fillRazoes()
		{
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
		
			SqlDataReader DR =  servico.DRGetListaComentariosEstado(this.ddEstado.SelectedValue); 
			if(DR.HasRows)
			{

				this.ddComentarioEstado.DataSource = DR; 
				this.ddComentarioEstado.DataBind(); 
			}
			else
			{
				this.ddComentarioEstado.DataSource = null;
				this.ddComentarioEstado.DataBind(); 
			}

			DR.Close(); 
			this.ddComentarioEstado.Items.Insert(0,new ListItem("",""));
					
			servico = null;
		}


		private void fillEstadosSeguintes(string idEstado)
		{
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
			SqlDataReader DR =  servico.DRGetListaEstadosSeguintes(idEstado); 
			this.ddEstado.DataSource =DR; 
			this.ddEstado.DataBind(); 
			DR.Close(); 

			this.ddEstado.SelectedValue = idEstado; 

			servico = null;


            //so no fillform é que era feita a validacao destes estados.
            //mas este é chamado quando se carrega no botao de submeteter....//por isso tem de se repetir aqui a validação
            int idPerfil = (int)Session["idPerfil"]; 
            if (idPerfil != 6 && idPerfil != 4 && idPerfil != 5) //todos os que noa podem calibrar
            {
                if (idEstado != "6")
                {
                    ListItem foundItem6 = (ListItem)ddEstado.Items.FindByValue("6"); //calibrado
                    if (foundItem6 != null) ddEstado.Items.Remove(foundItem6);
                }

                if (idEstado != "25")
                {
                    ListItem foundItem25 = (ListItem)ddEstado.Items.FindByValue("25"); //calibrado no ext
                    if (foundItem25 != null) ddEstado.Items.Remove(foundItem25);
                }
            }

		}

        private DropDownList fillTiposCertificados(DropDownList ddTipoCertificado)
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();    
            //SqlDataReader DR =  lista.DRListaTiposCertificado(); 

            ddTipoCertificado.DataSource = lista.DVListaTiposCertificado(); 
            ddTipoCertificado.DataBind();
            

			lista = null;
            
			return ddTipoCertificado;
        } 

        private DropDownList fillFuncionarios(DropDownList ddFuncionarios)
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR =  lista.DRListaFuncionarios(); 
            ddFuncionarios.DataSource = DR;
            ddFuncionarios.DataBind();
            DR.Close(); 

			lista = null;

            return ddFuncionarios;
        } 
        
        private void fillAlcanceClasse()
        {
            DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD();
            SqlDataReader dr = preco.DRClasse();
            this.ddClassse.DataSource = dr;
            this.ddClassse.DataBind();
            dr.Close();

            this.ddClassse.Items.Insert(0,new ListItem("",""));

            SqlDataReader dr2= preco.DRListaUnidadeAlcance(); 
            this.ddUnidadeAlcance.DataSource=dr2; 
            this.ddUnidadeAlcance.DataBind(); 
            dr2.Close(); 

            this.ddUnidadeAlcance.Items.Insert(0,new ListItem("",""));

			preco = null; 
        }

		private void fillTipoEquipamento(string idTipoEquipamento)
		{
			
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaEquipamentosMesmaGrandeza(idTipoEquipamento); 
			this.ddTipoEquipamento.DataSource = DR2; 
			this.ddTipoEquipamento.DataBind(); 
			DR2.Close();
			
			lista = null;
		}
    
		private void BindCertificadosServico(string refServico)
		{
			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD(); 
			DataTable DT = data.DTListCertificado("","",refServico,"",""); 
			data = null; 
			
			this.dgCertificados.DataSource=DT;
			this.dgCertificados.DataBind(); 

			
			
		}

		private void fillSubTipoServico(string pai)
		{
			string strSQL = "SELECT idSubTipoServico, descricao from subTipoServico where pai = '"+pai+"' AND activo = 1 ";
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            this.ddlSubtipoServico.DataSource = dr;
			
			this.ddlSubtipoServico.DataValueField="idSubTipoServico";
			this.ddlSubtipoServico.DataTextField="descricao"; 
			this.ddlSubtipoServico.DataBind(); 
			this.ddlSubtipoServico.Items.Insert(0,new ListItem("",""));
            dr.Close();
		}


		//chama-se subtipoEquipamento mas está relacionado com ambos o equipamento e o serviço
		private void FillSubTipoEquipamento(string idTipoEquipamento)
		{
			//ha sempre novos subtipos que podem vir dos orçamentos, e que nao vao ter o activo preenchido
			//logo eu so tenho de eliminar os que por alguma razao -e  para ja manualmente - desactivei
			//11-2020
			string strSQL = "SELECT idSubTipoEquipamento, descricao from subtipoEquipamento where idTipoEquipamento  = " + idTipoEquipamento + " AND (activo is null or activo = 1)";
			SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
			this.ddSubtipoEquipamento.DataSource = dr;

			
			this.ddSubtipoEquipamento.DataBind();
			this.ddSubtipoEquipamento.Items.Insert(0, new ListItem("", ""));
			dr.Close();
		}


		private void fillForm(string id)
        {
            LabMetro.DATA.ServicoBD fillServico = new LabMetro.DATA.ServicoBD(); 
            LabMetro.DATA.ServicoDetails det = fillServico.GetServicoDetails(id); 
			if(det!= null)
			{
				////antigo, servia para calculo do preço
				////GUARDA A VARIAVEL TIPO DE PREÇO, PARA CALCULAR DEPOIS
				////ANTES DO INSERT
				////*******************************************************
				//ViewState["idTipoPreco"] = det.idTipoPreco; 
				//ViewState["formula"] = det.formula; 
				//ViewState["idTipoEquipamento"] = this.ddTipoEquipamento.SelectedValue.ToString(); //det.idTipoEquipamento;
				//ViewState["idTipoServico"] = det.idTipoServico;
				//ViewState["idFactura"] =det.idFactura; 
				//ViewState["valor"] =det.valor;
    //            ViewState["acreditado"] = det.acreditado;
    //            //this.Response.Write(ViewState["acreditado"]);

				////                this.Response.Write(det.idFactura.ToString()); 
				////                this.Response.Write(det.valor); 
				////*******************************************************

				// Preencher a informação referente ao Serviço
                this.lblRefServico.Text = det.refServico;

                 this.lblRefServico2.Text = "";

                 this.lblAcessorios.Text = det.acessorios;

                 this.lblMultiplosServicos.Text = det.bVariasGrandezas;

				if(det.refServico.Substring(0,1)=="A") this.lblRefServico2.Text += " Aditamento ao serviço <a href='FormPastaEnsaio.aspx?btn=LAB&id="+det.idServicoPai+"' target='_blank'>"+det.refServicoPai+"</a>";

                if (det.idFactura != "") this.lblFacturado.Text = Resources.Resource.ServicoFacturadoNaoAlterar;

				if(det.idRequisicao!="" && det.nomeFicheiroReq !="") this.tdReq.InnerHtml = "<img src='IMAGES/ic_seta_red.gif' /> <a href='"+this.downloadpath(det.nomeFicheiroReq)+"' target='_blank'>Ver Requ.</a>";


               


				try
				{
                    this.ddLocalActual.SelectedValue = det.idLocalActual;
				}
				catch
				{
                    this.ddLocalActual.Items.Insert(0, new ListItem(det.localActual, det.idLocalActual));
                    this.ddLocalActual.SelectedValue = det.idLocalActual; 
				}

				string subtipoPai = det.refServico.Substring(0,4);//isto ja nao faz sentido

                //posso ir pelo idsubtipoServico ou CACV
               // if (subtipoPai == "VGAS" || subtipoPai == "VACV" || subtipoPai == "CACV" || subtipoPai == "VTER" || subtipoPai == "VGPL" || subtipoPai == "VMLP" || subtipoPai == "VSCM" || subtipoPai == "VAGE" || subtipoPai == "VOPC" || subtipoPai == "VBAS")
               if(subtipoPai == "CACV" || det.idTipoServico =="V")
				
				{
					this.fillSubTipoServico(subtipoPai); 
					this.ddlSubtipoServico.Visible=true; 
					this.lblGAS.Text ="Para "+ subtipoPai +": Indicar (Sub-)Tipo de Serviço:";
					try
					{
						this.ddlSubtipoServico.SelectedValue = det.idSubTipoServico;
                        RequiredFieldValidator1.Enabled = true;
					}
					catch
					{
					}
				}
				else
				{
                    this.RequiredFieldValidator1.Enabled = false;
					this.lblGAS.Text =""; 
					this.ddlSubtipoServico.Visible=false; 
				}

				this.cbCalExterna.Checked = GERAL.clsGeral.ConvertStringToBoolean(det.calibracaoExterna);
				this.txtObservacoes.Text = det.observacoes;
				this.txtObsCliente.Text = det.obsCliente; 
                
				this.ViewState["idEstadoServicoOriginal"] = det.idEstadoServico;
				try
				{
					this.ddEstado.SelectedValue = det.idEstadoServico;
				}
				catch
				{
					this.ddEstado.Items.Insert(0,new ListItem(det.estadoServico,det.idEstadoServico));
					this.ddEstado.SelectedValue = det.idEstadoServico; 
				}

				int idPerfil = (int)Session["idPerfil"]; 
				if(idPerfil != 6  &&idPerfil != 4 && idPerfil != 5) //todos os que noa podem calibrar
				{
					if(det.idEstadoServico !="6")
					{
						ListItem foundItem6 = (ListItem)ddEstado.Items.FindByValue("6"); //calibrado
						if(foundItem6!=null) ddEstado.Items.Remove(foundItem6); 
					}

					if(det.idEstadoServico !="25")
					{
						ListItem foundItem25 = (ListItem)ddEstado.Items.FindByValue("25"); //calibrado no ext
						if(foundItem25!=null) ddEstado.Items.Remove(foundItem25); 
					}
				}

				this.fillRazoes(); //preencher as razoes associadas ao estado seleccionado
				try
				{
					this.ddComentarioEstado.SelectedValue = det.idComentarioEstado;
				}
				catch
				{
					//falta aqui isto		//se estiver inactivo, ou pôr a vazio.
					this.ddComentarioEstado.SelectedValue=""; 
				}
				try
				{
					this.ddFuncTreino.SelectedValue = det.idFuncionarioTreino;
				}
				catch
				{
					this.ddFuncTreino.Items.Insert(0,new ListItem(det.FuncionarioTreino,det.idFuncionarioTreino));
					this.ddFuncTreino.SelectedValue = det.idFuncionarioTreino; 
				}
                
				this.txtQuantidade.Text = det.quantidade; 
				this.txtUnidadeQuantidade.Text = det.unidadeQuantidade; 

				if(det.bConforme.ToString()=="False")
				{
					this.ddConforme.SelectedValue="0";
				}
				else if(det.bConforme.ToString()=="True")
				{
					this.ddConforme.SelectedValue="1";
				}
				else
				{
					ddConforme.SelectedValue=""; 
				}
				
				if(det.bDeslocacao =="True")
				{
					this.cbDeslocacao.Checked = true; 
				}
                this.ddTipoDeslocacao.SelectedValue = det.tipoDeslocacao;

                this.ddLinguaCertificado.SelectedValue = det.linguaCertificado;
                this.ddNivelPrioridade.SelectedValue = det.idNivelPrioridade;
                this.txtDataPrevisao.Text = GERAL.clsGeral.ToShortDate(det.dtPrevisao);
                this.rbRejeitado.SelectedValue =det.bRejeitado;
                this.txtNumEtiquetaIPQ.Text = det.numEtiquetaIPQ;


                // Preencher a informação referente ao BRE
                LabMetro.DATA.BreBD fillBRE = new LabMetro.DATA.BreBD(); 
				LabMetro.DATA.BreDetails detBRE = fillBRE.GetBreDetails(det.idBRE); 
				if(detBRE!= null)
				{
					this.lblRefBRE.Text = detBRE.refBRE; 
					this.lblDtBRE.Text =  GERAL.clsGeral.ToShortDate(detBRE.dtBRE);
					this.lblEmpresa.Text = detBRE.nomeCompridoEmpresa;
					
					this.lblEmpresa.Text += "<br />"+detBRE.morada; 
					this.lblEmpresa.Text += "<br />"+detBRE.cp + " " + detBRE.localidade;
                    this.lblEmpresa.Text += "<br />" + detBRE.concelho;
					this.lblEmpresa.Text += "<br />"+detBRE.obsEmpresa+"<br />"; 
					this.lblObservacoes.Text = detBRE.observacoes;
                    
					if(detBRE.idEmpresaContratante!="") this.lblEmpresa.Text+="CONTRATADO POR: " + detBRE.nomeEmpresaContratante; 

					this.ViewState["idEmpresa"] = detBRE.idEmpresa;
					this.ViewState["idEmpresaContratante"] = detBRE.idEmpresaContratante;
				}

				// Preencher a informação referente ao Equipamento
				LabMetro.DATA.EquipamentoBD fillEquipamento = new LabMetro.DATA.EquipamentoBD(); 
				LabMetro.DATA.EquipmentDetails detEquipamento = fillEquipamento.GetEquipmentDetails(det.idEquipamento); 
				if(detEquipamento!= null)
				{
					this.ViewState["idEquipamento"] = detEquipamento.idEquipamento; // necessário para o update
					this.ViewState["idTipoEquipamento"] = detEquipamento.idTipoEquipamento;



					//******************************************************************
					//******************************************************************
					
					this.fillTipoEquipamento(det.idTipoEquipamento);
					//try //estranhamente, isto funcionava mas ja nao funciona! nao dá qquando o valor nao exsite! 2020!

					//{
					
					//catch
					//{
					//	this.ddTipoEquipamento.Items.Insert(0,new ListItem(detEquipamento.tipoEquipamento,detEquipamento.idTipoEquipamento));
					//	this.ddTipoEquipamento.SelectedIndex = 0; //o primeiro
					//	this.lblTipoEquipamento.Text = "Atenção, tipo INACTIVO! Por favor actualizar o tipo!";
					//	this.lblTipoEquipamento.ForeColor = Color.Red;
					//	this.ddTipoEquipamento.Focus();

					//}
					try
					{
						//	this.ddTipoEquipamento.SelectedValue = detEquipamento.idTipoEquipamento
						//2020 esta sintaxo ja nao funciona!!!!! 

						ddTipoEquipamento.Items.FindByValue(det.idTipoEquipamento).Selected = true;
					}


					catch
                    {
                        this.ddTipoEquipamento.Items.Insert(0, new ListItem(detEquipamento.tipoEquipamento, ""));
						//ponho o id a emtpy para obrigar a preencher

                        this.ddTipoEquipamento.SelectedIndex = 0; //o primeiro
                        this.lblTipoEquipamento.Text = "Atenção, tipo INACTIVO! Por favor actualizar o tipo!";
                        this.lblTipoEquipamento.ForeColor = Color.Red;
                        this.ddTipoEquipamento.Focus();

                    }

                    

					//no fillform, o subtipo é preenchdio com base no tipo que vem do serviço
					this.FillSubTipoEquipamento(det.idTipoEquipamento);
					try
					{
						this.ddSubtipoEquipamento.SelectedValue = det.idSubTipoEquipamento;
					}
					catch
					{
						this.ddSubtipoEquipamento.Items.Insert(0, new ListItem(det.subTipoEquipamento, det.idSubTipoEquipamento));
						this.ddSubtipoEquipamento.SelectedIndex = 0; //o primeiro
					}


					this.lblIdEquipanmento.Text="(ID do Equip. na BD: "+detEquipamento.idEquipamento +")"; 
					//******************************************************************
					//******************************************************************
					//this.lblTipoEquipamento.Text = detEquipamento.tipoEquipamento;
					//label foi subsituida por uma dropdown
					this.txtNumIdentificacao.Text = detEquipamento.numIdentificacao;
					this.txtNumSerie.Text = detEquipamento.numSerie;
                    this.txtCodigoIPQ.Text = detEquipamento.codigoIPQ;

					this.txtForma.Text = detEquipamento.forma;
					try
					{
						this.ddClassse.SelectedValue = detEquipamento.idClasse; 
					}
					catch 
					{  
					}
					this.txtClasse.Text = detEquipamento.classe;
					this.txtAlcanceInf.Text = detEquipamento.alcanceInf; 
					this.txtAlcanceSup.Text = detEquipamento.alcanceSup; 
					try
					{
						this.ddUnidadeAlcance.SelectedValue = detEquipamento.idUnidadeAlcance;
					}
					catch 
					{  
					}
					this.txtAlcance.Text = detEquipamento.alcance;
					this.txtResolucao.Text = detEquipamento.resolucao;
				

					this.ViewState["grandeza"] = fillEquipamento.GetGrandeza(detEquipamento.idTipoEquipamento); 

					// Carregar combo das marcas
					this.fillMarcas();

					try
					{
						this.ddMarca.SelectedValue = detEquipamento.idmarca; //this.ddMarca.Items.FindByText(detEquipamento.marca).Value;
					}
					catch 
					{	
					}

					if (detEquipamento.idmarca != "")
						this.fillModelos(); // Carregar combo dos modelos

					try
					{
						this.ddModelo.SelectedValue = detEquipamento.idmodelo; 
					}
					catch 
					{
					}
					
					if(detEquipamento.certConclusivo =="True") 
					{
						this.cbCertConclusivo.Checked = true;
						this.ddConforme.Enabled=true;
					} 
					else
					{
						this.ddConforme.Enabled=false;
					}

					this.txtCampo1.Text = detEquipamento.campo1;
					this.txtCampo2.Text = detEquipamento.campo2;

                    this.txtEtiqueta1.Text = detEquipamento.etiqueta1;
                    this.txtEtiqueta2.Text = detEquipamento.etiqueta2;
                    this.txtEtiqueta3.Text = detEquipamento.etiqueta3;
                    this.txtObsEquipamento.Text = detEquipamento.observacoes;

                    this.txtCriteriosEquipamento.Text = detEquipamento.criterios;
                    try
                    {
                        this.ddEstadoRelacaoCalibracao.SelectedValue = detEquipamento.idEstadoRelacaoCalibracao;
                    }
                    catch
                    {
                    }
				}

				bool bDef = System.Convert.ToBoolean(det.bDefinitivo); 
				if(bDef ==false) 
				{
					this.lblMessage.Text="Serviço não definitivo."; 
					this.lblMessage.Text+="Tem de passar o BRE de calibração externa para definitivo primeiro."; 
					this.btnSubmit.Enabled=false;
				}

				fillServico = null; 
				fillEquipamento = null; //novo nov 2008

                DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
				dtLinhasServico = servico.dtLinhasServicoByIdServico(this.ViewState["idServico"].ToString());
                
				this.ViewState["dtLinhasServico"] = dtLinhasServico; 
                this.BindGridLinhasServico();

                servico = null;

                DATA.PastaEnsaioBD pasta = new LabMetro.DATA.PastaEnsaioBD(); 
                dtCertificados = pasta.dtCertificadosByIdServico(this.ViewState["idServico"].ToString()); 
                
                this.ViewState["dtCertificados"] = dtCertificados; 


                this.BindCertificadosServico(det.refServico); 

				this.BindGridHistorico(det.idServico); 
				this.BindGridServicosAnteriores(det.idEquipamento, det.idServico); 
				//pasta = null; 
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
            this.DGLinhasServico.EditCommand += new DataGridCommandEventHandler (this.DGLinhasServico_EditCommand);  
            this.DGLinhasServico.ItemCommand += new DataGridCommandEventHandler (this.DGLinhasServico_ItemCommand); 
            this.DGLinhasServico.CancelCommand += new DataGridCommandEventHandler (this.DGLinhasServico_CancelGrid); 
            this.DGLinhasServico.UpdateCommand += new DataGridCommandEventHandler (this.DGLinhasServico_UpdateGrid); 
            this.DGLinhasServico.DeleteCommand += new  DataGridCommandEventHandler(this.DGLinhasServico_DeleteCommand);

			this.ddMarca.SelectedIndexChanged += new System.EventHandler(this.ddMarca_SelectedIndexChanged);
			this.ddTipoEquipamento.SelectedIndexChanged += new System.EventHandler(this.ddTipoEquipamento_SelectedIndexChanged);
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			this.btnEtiqueta.Click += new System.EventHandler(this.btnEtiqueta_Click);
			this.btnEtiquetaCal.Click += new System.EventHandler(this.btnEtiquetaCal_Click);
			this.btnRequisicoes.Click += new System.EventHandler(this.btnRequisicoes_Click);
			this.btnOrcamentos.Click += new System.EventHandler(this.btnOrcamentos_Click);
			this.DGOrcamentos.SelectedIndexChanged += new System.EventHandler(this.DGOrcamentos_SelectedIndexChanged);
			this.ddEstado.SelectedIndexChanged += new System.EventHandler(this.ddEstado_SelectedIndexChanged);
			
		}
        #endregion
        // *****************************************************************************
        // DGLinhasServico - DATAGRID COMMANDS
        // *****************************************************************************
        private void DGLinhasServico_EditCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {	 
            DGLinhasServico.EditItemIndex = e.Item.ItemIndex; 
            BindGridLinhasServico(); 
            DGLinhasServico.ShowFooter = false;
        }

        private void DGLinhasServico_CancelGrid(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {	
            DGLinhasServico.EditItemIndex = -1; 
            BindGridLinhasServico(); 
            DGLinhasServico.ShowFooter = true; 
        }

        private void DGLinhasServico_UpdateGrid(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {	
            // Obter os novos valores
            TextBox txtNumPecas = (TextBox)e.Item.FindControl("txtNumPecasEdit"); 
            TextBox txtNumPontos = (TextBox)e.Item.FindControl("txtNumPontosEdit"); 
            TextBox txtPontosCalib = (TextBox)e.Item.FindControl("txtPontosCalibEdit"); 

            // Obter o índice da linha que foi alterada
            int index = this.DGLinhasServico.EditItemIndex;
          
            if(index >= 0)
            {    
                
                dtLinhasServico = (DataTable)this.ViewState["dtLinhasServico"]; 
                dvLinhasServico = new DataView(dtLinhasServico); 
                dvLinhasServico[index]["numPecas"] = txtNumPecas.Text; 
                dvLinhasServico[index]["numPontos"] = txtNumPontos.Text; 
                dvLinhasServico[index]["pontosCalib"] = txtPontosCalib.Text; 
                this.ViewState["dtLinhasServico"] = dtLinhasServico; 
            }

            this.DGLinhasServico.EditItemIndex = -1; 
            this.BindGridLinhasServico();
            this.DGLinhasServico.ShowFooter = true;
        }

        private void DGLinhasServico_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {  
                if(e.Item.ItemType == ListItemType.Footer)
                {
                    // Ir buscar os valores inseridos no Footer
                    TextBox txtNumPecas = (TextBox)e.Item.FindControl("txtNumPecas"); 
                    TextBox txtNumPontos = (TextBox)e.Item.FindControl("txtNumPontos"); 
                    TextBox txtPontosCalib = (TextBox)e.Item.FindControl("txtPontosCalib"); 
                   
                    // Verificar se os campos obrigatórios foram preenchidos
                    if (txtNumPecas.Text=="") 
                        this.lblMessage.Text ="Tem de preencher o campo 'Nº de Peças' antes de adicionar."; 
                    else if (txtNumPontos.Text=="") 
                        this.lblMessage.Text ="Tem de preencher o campo 'Nº de Pontos' antes de adicionar."; 
                    else if (txtPontosCalib.Text=="") 
                        this.lblMessage.Text ="Tem de preencher o campo 'Pontos de Calibração' antes de adicionar."; 
                    else
                    {
                        dtLinhasServico = (DataTable)this.ViewState["dtLinhasServico"]; 
                        dvLinhasServico = new DataView(dtLinhasServico); 
                        // Adicionar a nova linha ao DataView
                        DataRowView drv = dvLinhasServico.AddNew();
                        drv["numPecas"] = txtNumPecas.Text; 
                        drv["numPontos"] = txtNumPontos.Text; 
                        drv["pontosCalib"] = txtPontosCalib.Text; 
                        drv.EndEdit();
                        this.ViewState["dtLinhasServico"] = dtLinhasServico; 
                    }
                }
                DGLinhasServico.EditItemIndex = -1; 
                this.BindGridLinhasServico(); 
            }
        }

        private void DGLinhasServico_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dtLinhasServico = (DataTable)this.ViewState["dtLinhasServico"]; 
            dvLinhasServico = new DataView(dtLinhasServico); 

            dvLinhasServico.Delete(e.Item.ItemIndex);

            this.ViewState["dtLinhasServico"] = dtLinhasServico;
            this.BindGridLinhasServico();
            DGLinhasServico.ShowFooter = true;
        }
		
		//============================================================================	
		//ITEM DATABOUND DO GRID CERTIFICADOS.
		//============================================================================	
		public void dgCertificados_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			
			
		}

		//==================================================================================
		// Função que permite visualizar o documento pretendido pelo utilizador
		//==================================================================================
		public void visualisarDocumento(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName.ToString() == "Select")
			{
				string doc = e.Item.Cells[4].Text;
				string nome = downloadpathCert(doc);
				this.Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");
			}
		}

		public string downloadpathCert(object filename)
		{
			if(filename!=null && filename.ToString()!="") 
			{

//				this.Response.Write(this.Request.Url.Segments[0].ToString());
//				this.Response.Write(this.Request.Url.Segments[1].ToString());
//				this.Response.Write(this.Request.Url.Segments[2].ToString());
				string myPath = (string)ConfigurationManager.AppSettings["PATHABS_CERT_FINAIS_CERTIFICADOS"];
				myPath =myPath + "/" + filename.ToString();
				
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}


        
        //// *****************************************************************************
        //// CALCULAR PREÇO DO SERVIÇO ANTES DE MANDAR 
        //// TENHO DE FAZER UPDATE AO CAMPO VALOR DA TABELA SERVIÇO
        //// *****************************************************************************
        
        //private double calculateServicePrice()
        //{
        //    //passar para variavel para ser mais rapido.
        //    string idTipoPreco = ViewState["idTipoPreco"].ToString(); 

        //    string formula = ViewState["formula"].ToString(); 
        //    string idTipoEquipamento = this.ddTipoEquipamento.SelectedValue.ToString(); //ViewState["idTipoEquipamento"].ToString(); 
        //    string idTipoServico = ViewState["idTipoServico"].ToString(); 

        //    //a unidade da quantidade nao interessa pois a cada tipo de equipamento so
        //    //está atribuido um tipo de quantidade.    
        //    string quantidade= this.txtQuantidade.Text;
        //    //retorna algo a dizer se é calibração interna ou externa, para móvel, não ha nada

        //    //movel, nao sei onde se vê; por isso so vou distinguir entre externa e interna
        //    string tipoPr = "preco";  

        //    if(this.cbCalExterna.Checked == true)
        //    {
        //        tipoPr = "precoExterior"; 
        //    }

        //    double precoServico = 0;  //se conseguir calcular alguma preco, muda, senao, fica a 0

        //    //PÔR AQUI UM TRY CATCH NO FINAL!!!

        //    DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 

        //    switch(idTipoPreco)
        //    {
        //        case "1": //preço directo por tipo de equipamento.
                        
        //            precoServico = preco.getPriceByTipoEquipamento(idTipoEquipamento, idTipoServico,tipoPr); 
        //        break; 

        //        case "2":  //tipo de equipamento e quantidade

        //            //se nao tiver quantidade, nao calcula o preço
        //            if(quantidade != "")
        //            {
        //                precoServico = preco.getPriceByTipoEquipamentoQuantidade(idTipoEquipamento, idTipoServico,quantidade,tipoPr); 

        //            }

        //        break;                 

        //        case "3": //tipo de equipamento e alcance
                
        //            precoServico = preco.getPriceByTipoEquipamentoAlcanceSimples(idTipoEquipamento,idTipoServico,this.ddUnidadeAlcance.SelectedValue,this.txtAlcanceInf.Text,this.txtAlcanceSup.Text, tipoPr); 
        //            break; 

                    
        //        case "4": //tipo de equipamento + alcance + numPontos/pares
                
        //            //aqui fazer um loop pelas possiveis linhas do datagrid linhas servico
        //            dtLinhasServico = (DataTable)this.ViewState["dtLinhasServico"]; 
        //            DataView dv = new DataView(dtLinhasServico); 

        //            foreach(DataRowView drv in dv)
        //            {
        //                string strAlcance=""; 
        //                string strPontos =""; 
        //                string numPecas = "1"; 
        //                try
        //                {
        //                    //aqui recebe os pontos do datagrid. //e pode receber varias linhas
        //                    string p = drv["pontosCalib"].ToString(); 
        //                    numPecas = drv["numPecas"].ToString() ; 
        //                    int strHum = p.IndexOf("%"); 
        //                    int strTemp = p.IndexOf("ºC"); 
        //                    int strMisto = p.IndexOf("%(ºC)"); 
                       
        //                    int indexOf = 0;  

        //                    if(strMisto >=0) //alcance Misto
        //                    {
        //                        strAlcance="M"; 
        //                        indexOf = strMisto; 
        //                    }
        //                    else if(strHum >=0) //alcance Humidade
        //                    {
        //                        strAlcance = "H"; 
        //                        indexOf = strHum; 
        //                    }
        //                    else if(strTemp >=0) //alcance Temperatura
        //                    {
        //                        strAlcance = "T"; 
        //                        indexOf = strTemp; 
        //                    }
        //                    else
        //                    {
        //                        this.lblMessage.Text="Por favor, verifique as linhas do Serviço.<br />Com base nos critérios introduzidos, não é possível calcular o preço do serviço.<br />"; 
        //                        return 0; 

        //                    }

        //                    //remover o alcance para receber a string so com pontos/pares
        //                   strPontos = p.Substring(0,indexOf);
        //                }
                    
        //                catch(Exception ex)
        //                {
        //                    GERAL.clsWriteError.WriteLog(ex.ToString()); 
        //                }

        //                if(strPontos!="")
        //                {
        //                    precoServico +=  preco.getPriceByTipoEquipamentoAlcancesMistos(idTipoEquipamento,idTipoServico,strPontos,strAlcance,tipoPr,numPecas,formula);
                           
        //                }
        //            }   

        //            break; 

        //        case "5": //tipo equipa + alcance + classe

        //            precoServico = preco.getPriceByTipoEquipamentoAlcanceSimplesClasse(idTipoEquipamento,idTipoServico,this.ddUnidadeAlcance.SelectedValue,this.txtAlcanceInf.Text,this.txtAlcanceSup.Text,this.ddClassse.SelectedValue,tipoPr); 
        //        break; 
                 
        //        case "6"://marca-modelo 
        //            if(this.ViewState["grandeza"].ToString() == "ELE") //preços por marca modelo
        //                //so para electricos E CTA???? PRIMEIRO SO PARA ELECTRICOS... 
        //                //CTA VIERAM DPS MAS POR OUTRAS RAZOES (EMPRESAS MANUTENCAO ASSOCIADAS À MARCA). 
        //            {
        //                precoServico = preco.getPriceByIdModelo(idTipoEquipamento,this.ddModelo.SelectedValue,idTipoServico,tipoPr); 

        //            }
        //        break;                           
        //    }

        //    preco = null; 

        //    return precoServico; 
        //}
        

        // *****************************************************************************
        // Gravar as alterações da Pasta de Ensaio - SUBMIT
        // *****************************************************************************
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
			// Guardar os valores das combos nas textboxes porque é o valor destas que usamos
			

            dtCertificados = (DataTable)this.ViewState["dtCertificados"]; 
            dvCertificados = new DataView(dtCertificados); 

            dtLinhasServico = (DataTable)this.ViewState["dtLinhasServico"]; 
            dvLinhasServico = new DataView(dtLinhasServico); 

           
			
			DATA.PastaEnsaioBD pastaEnsaio = new LabMetro.DATA.PastaEnsaioBD(); 
                  
			if(this.Page.IsValid)
			{
				if(this.ddEstado.SelectedValue =="7" || this.ddEstado.SelectedValue=="9")
				{
					if(this.ddComentarioEstado.SelectedValue =="") 
					{
						this.lblMessage.Text="Indique a razão da Anulação ou Suspensão."; 
						return; 
					}
				}

                if (this.cbDeslocacao.Checked == true)
                {
                    if (this.ddTipoDeslocacao.SelectedValue == "0")
                    {
                        this.lblMessage.Text = "Indique qual a deslocação aplicável.";
                        return;
                    }
                }
				
				string	idmarca = this.ddMarca.SelectedValue; 
				string idmodelo = this.ddModelo.SelectedValue; 
				
				string bDeslocacao = "0";
				if (this.cbDeslocacao.Checked==true) bDeslocacao = "1";

                //if (myApp.ToUpper() == "ES_LABMETRO")
                //{

                //    if (this.ddEstado.SelectedValue == "6" || ddEstado.SelectedValue == "25")
                //    {

                //        if (this.txtNumIdentificacao.Text == "" || this.txtNumSerie.Text == "" || this.ddMarca.SelectedItem.Text == "" || this.ddModelo.SelectedItem.Text == "" || txtAlcanceInf.Text == "" || this.txtAlcanceSup.Text == "" || this.ddUnidadeAlcance.SelectedItem.Text == "" || this.txtResolucao.Text == "")
                //        {
                //            this.lblMessage.Text = "Por favor rellenar todos los campos: num. Serie, num. ID y los demas datos del equipo .";
                //            return;
                //        }
                //    }
 
                //}
				string  strError  = pastaEnsaio.UpdatePastaEnsaio(this.ViewState["idServico"].ToString(), this.ddLocalActual.SelectedValue, this.cbCalExterna.Checked.ToString(), this.txtObservacoes.Text, this.ddEstado.SelectedValue.ToString(), this.ViewState["idEquipamento"].ToString(), this.txtNumIdentificacao.Text, this.txtNumSerie.Text, this.txtForma.Text,this.ddClassse.SelectedValue, this.txtClasse.Text,this.txtAlcanceInf.Text,this.txtAlcanceSup.Text,this.ddUnidadeAlcance.SelectedValue, this.txtAlcance.Text, this.txtResolucao.Text, idmarca, idmodelo, this.ddFuncTreino.SelectedValue.ToString(), dvLinhasServico, dvCertificados, User.Identity.Name.ToString(),this.txtQuantidade.Text, this.txtUnidadeQuantidade.Text, null, this.ViewState["idEmpresa"].ToString(), this.ddTipoEquipamento.SelectedValue.ToString(),this.txtObsCliente.Text, this.ddComentarioEstado.SelectedValue.ToString(),this.ddlSubtipoServico.SelectedValue.ToString(),this.ViewState["idEstadoServicoOriginal"].ToString(),this.txtCampo1.Text,this.txtCampo2.Text,this.ddConforme.SelectedValue, bDeslocacao, this.ddNivelPrioridade.SelectedValue, this.txtDataPrevisao.Text, this.txtEtiqueta1.Text, this.txtEtiqueta2.Text, this.txtEtiqueta3.Text, this.ddLinguaCertificado.SelectedValue, this.txtObsEquipamento.Text, this.ddTipoDeslocacao.SelectedValue, this.txtCodigoIPQ.Text, this.ddEstadoRelacaoCalibracao.SelectedValue, rbRejeitado.SelectedValue, txtNumEtiquetaIPQ.Text, this.ddSubtipoEquipamento.SelectedValue);

				this.lblMessage.Text += strError; 
				if(strError == 	GERAL.clsGeral.ErrorMessage.MSG_DB)
				{
					this.ViewState["idEstado"] = this.ddEstado.SelectedValue.ToString(); 
					this.ViewState["idEstadoServicoOriginal"] = this.ddEstado.SelectedValue.ToString();
				
					//para nao ver as linhas da dataview sempre como unchanged fazer o commit das alterações após o update.
					//MUITO IMPORTANTE! (para nao repetir linhas).
					this.dtLinhasServico.AcceptChanges(); 
					//this.dtCertificados.AcceptChanges(); 

					//para mostrar os estados proximos, caso se tenha mudado o estado.
					this.fillEstadosSeguintes(this.ViewState["idEstado"].ToString()); 
					this.BindGridHistorico(this.ViewState["idServico"].ToString()); 
				}
			}

			pastaEnsaio = null; 
        }

        private void ddMarca_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.fillModelos(); 
        }

		
			private void ddTipoEquipamento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.FillSubTipoEquipamento(ddTipoEquipamento.SelectedValue);
		}
		private void btnEtiqueta_Click(object sender, System.EventArgs e)
		{

            ReportClass report = null;

            switch (myApp)
            {
                case "ISQ_LABMETRO":
                    report = new LabMetro.REPORTS.crEtiquetaCal();
                    break;
                case "ANG_LABMETRO":
                    report = new LabMetro.REPORTS_ANG.crEtiquetaCal();
                    break;
                case "SON_LABMETRO":
                    report = new LabMetro.REPORTS_SON.crEtiquetaCal();
                    break;
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crEtiquetaCal();
                    break;
                case "CV_LABMETRO":
                    report = new LabMetro.REPORTS_CV.crEtiquetaCal();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.crEtiquetaCal();
                    break;
            }

            clsReport cr = new clsReport();

			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
			DataSet ds  = bre.DSEtiquetaServico(this.ViewState["idServico"].ToString()); 	
				
			report.SetDataSource(ds); 	
            ds = null; 
            bre = null; 
			cr.exportReportToPDF(report,"Etiqueta");
				
		
            //cr = null; 
            //report = null;
			
		}

		private void btnEtiquetaCal_Click(object sender, System.EventArgs e)
		{
//			rptEtiquetaCalibracao report = new rptEtiquetaCalibracao();

            ReportClass report = null;

            switch (myApp)
            {
                case "ISQ_LABMETRO":
                    report = new LabMetro.REPORTS.rptEtiquetaCalTodos();
                    break;
                case "ANG_LABMETRO":
                    report = new LabMetro.REPORTS_ANG.rptEtiquetaCalTodos();
                    break;
                case "BR_LABMETRO":
                    report = new LabMetro.REPORTS_BR.rptEtiquetaCalTodos();
                    break;
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crEtiquetaMista();
                    break;
                    //if (this.ViewState["acreditado"].ToString() == "False")
                    //{
                    //    report = new LabMetro.REPORTS_ES.rptEtiquetaCalTodos();
                    //}
                    //else
                    //{
                    //    report = new LabMetro.REPORTS_ES.rptEtiquetaCalENAC();
                    //}
                    //break;
                case "CV_LABMETRO":
                    report = new LabMetro.REPORTS_CV.rptEtiquetaCalTodos();
                    break;

                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.rptEtiquetaCalTodos();
                    break;
            }

            clsReport cr = new clsReport();

			DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
			DataSet ds  = bre.DSEtiquetaCalibracao(this.ViewState["idServico"].ToString());

            //this.Response.Write(   ds.Tables["dtEtiquetaPosCalib"].Rows[0]["dtProximaCalibracao"].ToString());


            report.SetDataSource(ds);
            ds = null;

            bre = null;
            cr.exportReportToPDF(report, "Etiqueta");


        }

		protected void validaEquipamento(object source, ServerValidateEventArgs args)
		{

			//num Serie- num Ident. nao pode estar ambos vazios
			//nao podem conter ambos ---
			//se um contem --- o outro nao pode estar vazio.
			if((this.txtNumIdentificacao.Text.Length == 0 && this.txtNumSerie.Text.Length ==0) ||(this.txtNumIdentificacao.Text.ToString()== "---" && this.txtNumSerie.Text.ToString() =="---") ||(this.txtNumIdentificacao.Text.ToString() =="---" && this.txtNumSerie.Text.Length==0) || (this.txtNumSerie.Text.ToString() == "---" && this.txtNumIdentificacao.Text.Length==0))
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true; 
			}
		}

		//=============================================================================
		//BINDGRID DATAGRID REQUISICOES
		//=============================================================================

		protected void BindGridRequisicoes()
		{
			DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();         
			DataTable dt  = req.DTListaRequisicoes(this.ViewState["idEmpresa"].ToString(),"","","",false,false); 
    
			this.DGRequisicoes.DataSource = dt;
			this.DGRequisicoes.DataBind();               

			req = null; 
		}

		//=============================================================================
		//PAGING DATAGRID REQUISIÇOES
		//=============================================================================
		protected void DoPagingRequisicoes(Object s,DataGridPageChangedEventArgs e)
		{
			this.DGRequisicoes.CurrentPageIndex=e.NewPageIndex;
			this.BindGridRequisicoes(); 
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

        public string downloadpathCriterios(object filename)
		{
			if(filename!=null && filename.ToString()!="")
			{


                string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_CRITERIOS_URL"];
                
				myPath = myPath + "/" + filename.ToString(); 
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}

        //=================================================================================================
        // DEVOLVE O CAMINHO PARA UM NOME DE FICHEIRO
        //=================================================================================================
        public string downloadpathOrcamento(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_URL"];

                myPath = myPath + "/" + filename.ToString();
                return myPath;
            }
            else
            {
                return "#";
            }
        }

		protected string ConverteEstado(bool b)
		{
			if (b==true) 
			{
				return "sim";
			}
			else
			{
				return "não"; 
			}

		}

		private void btnRequisicoes_Click(object sender, System.EventArgs e)
		{
			if(this.DGRequisicoes.Visible == false)
			{
				this.DGRequisicoes.Visible= true;
			}
			else
			{
				this.DGRequisicoes.Visible= false; 
			}
		}

		//BINDGRID DOS ORÇAMENTOS
		private void BindGrid()
		{
			DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD(); 
			DataTable dt = orcamento.DTOrcamentos("","","","",this.ViewState["idEmpresa"].ToString(),null); 

			DataView DV = new DataView(dt);
	
			DV.Sort = this.ViewState["sortField"].ToString()+ " " + this.ViewState["sortDirection"]; 
            
			this.DGOrcamentos.DataSource = DV;
			this.DGOrcamentos.DataBind(); 

			orcamento = null; 

		}

		//SORT DOS ORÇAMENTOS
		public void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{
			switch (this.ViewState["sortDirection"].ToString())
			{
				case "ASC":
					this.ViewState["sortDirection"]="DESC"; 
					break;
				case "DESC":
					this.ViewState["sortDirection"]="ASC";
					break;
			}
			this.ViewState["sortField"] = e.SortExpression;

			this.BindGrid(); 

		}

		//PAGING DOS ORÇAMENTOS
		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			this.DGOrcamentos.CurrentPageIndex = e.NewPageIndex;
			this.BindGrid(); 
		}

		private void btnOrcamentos_Click(object sender, System.EventArgs e)
		{
			if(this.DGOrcamentos.Visible == false)
			{
				this.DGOrcamentos.Visible= true;
				this.dgLinhasOrcamento.Visible = true; 
				this.dgComentariosOrcamento.Visible=true;
			}
			else
			{
				this.DGOrcamentos.Visible= false; 
				this.dgLinhasOrcamento.Visible = false; 
				this.dgComentariosOrcamento.Visible=false;
			}
		}


		//preencher a label empresa devedora com os respectivos dados, caso aplicável
		private void fillCompanyInfo()
		{

			string idEmpresa = this.ViewState["idEmpresa"].ToString();
			if(this.ViewState["idEmpresaContratante"].ToString() !="") 
			{
				idEmpresa = this.ViewState["idEmpresaContratante"].ToString();
				

			}
			
			this.lblEmpresaDevedora.Text =""; //limpar a label

			LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(idEmpresa); 
			if(detEmpresa != null)
			{


                ///colocar aqui o nome do ficheiro dos criterios de aceitacao
                if (detEmpresa.ficheiroCriterios != "" && detEmpresa.ficheiroCriterios != "") this.tdFicheiroCriterios.InnerHtml = "<img src='IMAGES/ic_seta_red.gif' /> <a href='" + this.downloadpathCriterios(detEmpresa.ficheiroCriterios) + "' target='_blank'>Ver Criterios do Cliente</a>";

                this.txtCriteriosAceitacaoEmpresa.Text = detEmpresa.criteriosAceitacao;

				if (detEmpresa.pagamentoAtraso =="1")
				{
					this.lblEmpresaDevedora.Text = "** PAGAMENTOS EM ATRASO **<br />";
				}
				else
				{
					this.lblEmpresaDevedora.Text = ""; 
				}
				

				switch(detEmpresa.nivelBloqueioLabmetro)	
				{ 
					case "0": 
						//this.lblEmpresa.BackColor =System.Drawing.ColorTranslator.FromHtml("#cccccc");
						break;
					case "1":
						this.lblEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("Gold");
;  
						break; 
					case "2": 
						this.lblEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("DarkOrange");
						this.lblEmpresaDevedora.Text +="Venda à dinheiro ou Pagamento do Atrasado.<br />"; 
						break; 
					case "3": 
						this.lblEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("Crimson");
						this.lblEmpresaDevedora.Text +="Venda à dinheiro.<br />"; 
						break; 
				}

				switch(detEmpresa.codigoBloqueioSAP)
				{
					case "00":  //nada mas tem de estar tratado
						break; 
					case "01": 
						this.lblEmpresaDevedora.Text += "** EMPRESA FALIDA **";
						break;
					case "02":
						this.lblEmpresaDevedora.Text += "** EMPRESA EM CONTENCIOSO **"; 
						break; 
					case "03":
						this.lblEmpresaDevedora.Text += "** Empresa com nºCliente SAP inactivo **"; 
						break; 
					default: //todos os outros bloqueados
						this.lblEmpresaDevedora.Text += "** EMPRESA COM BLOQUEIO EM SAP **"; 
						break; 
				}					
				
			}
			empresa = null; 
		}

		
		private void DGOrcamentos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string idOrcamento = this.DGOrcamentos.DataKeys[this.DGOrcamentos.SelectedIndex].ToString();
				
			DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD(); 
			
			DataTable dt = orc.DTLinhasOrcamentoByIdOrcamento(idOrcamento); 
			DataView dv = new DataView(dt); 
			this.dgLinhasOrcamento.DataSource = dv; 
			this.dgLinhasOrcamento.DataBind();

			dt = orc.DTComentariosOrcamentoByIdOrcamento(idOrcamento); 
			dv = new DataView(dt); 
			this.dgComentariosOrcamento.DataSource = dv;
			this.dgComentariosOrcamento.DataBind();

			orc = null; 
			
		}

		private void ddEstado_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.fillRazoes();

            if (ddEstado.SelectedItem.Value.ToString() == "25" || ddEstado.SelectedItem.Value.ToString() == "26")
            {
                this.cbCalExterna.Checked = true;
            }
		}

		private void BindGridServicosAnteriores(string idEquipamento, string idServico)
		{
				
			SqlParameter[] arrParams = new SqlParameter[2];
			arrParams[0] = new SqlParameter("@inIdEquipamento",idEquipamento);
			arrParams[1] = new SqlParameter("@inIdServicoActual",idServico);
				
			DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicosCertificadosByIdEquipamento",arrParams); 
				
					
				this.dgServicosAnteriores.DataSource =dt;
				this.dgServicosAnteriores.DataBind();
			
				
		}

       
	
	
    }
}