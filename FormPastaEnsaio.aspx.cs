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
            Page.Form.DefaultButton = btnSubmit.UniqueID;

			Response.Expires = 0; 	//nao tirar!!!

			lblMessage.Text="";
			lblTipoEquipamento.Text = "";

            if (myApp.ToUpper() == "ES_LABMETRO")
            {
                ddEstadoRelacaoCalibracao.Enabled = true;
            }
            else
            {
                ddEstadoRelacaoCalibracao.Enabled = false;
            }

            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				//Response.Redirect("Default.aspx?err=2",true);            
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					//Response.Redirect("Default.aspx?err=1",true);//user n.tem permissoes                             
					Server.Transfer("Default.aspx?err=1",false);
				}
                else
                {
                    int intAcesso = System.Convert.ToInt32(ht[ID_PAG]); 
                    if(intAcesso ==0) //so tem permissoes de leitura
                    {
                        btnSubmit.Enabled=false;
                    }
                    
                    if(Request.QueryString["id"]!=null)
                    {
                        if(Request.QueryString["id"]!="")
                        {
                            ViewState["idServico"] = Request.QueryString["id"].ToString();

                            if(!Page.IsPostBack)
                            {                        
                                FillDropDowns(); 
                                fillForm(ViewState["idServico"].ToString());  
								fillCompanyInfo(); 
								BindGridRequisicoes(); 
								
								//DO DGOrcamentos ORÇAMENTO
								ViewState["sortField"] = "idOrcamento";
								ViewState["sortDirection"] = "DESC";
								BindGrid(); //Orçamentos
                            }  
                        }   
                    }
                    else
                    {
                        if(!Page.IsPostBack)
                        {	
							
                            FillDropDowns(); 
							//DO DGOrcamentos ORÇAMENTO
							ViewState["sortField"] = "idOrcamento";
							ViewState["sortDirection"] = "DESC";
                        } 
                    }
                }
            }

           
        }

		
        private void BindGridHistorico(string idServico)
        {   
            DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD(); 

            //dgHistorico.DataSource = servicos.DTGetServicoHistoricoEstados(ViewState["idServico"].ToString()); 
			dgHistorico.DataSource = servicos.DTGetServicoHistoricoEstados(idServico); 
            dgHistorico.DataBind();

			servicos = null; 
        }

        private void BindGridCertificados()
        {   

            dtCertificados = (DataTable)ViewState["dtCertificados"];
            dvCertificados = new DataView(dtCertificados); 
            dgCertificados.DataSource = dvCertificados; 
            dgCertificados.DataBind();
        }

        private void BindGridLinhasServico()
        {
            dtLinhasServico = (DataTable)ViewState["dtLinhasServico"];
            dvLinhasServico = new DataView(dtLinhasServico); 
            DGLinhasServico.DataSource = dvLinhasServico;
            DGLinhasServico.DataBind();
        }

        private void FillDropDowns()
        {
            fillLocalActual();
            fillEstados();
            fillFuncTreino();
            fillAlcanceClasse();
            fillNivelPrioridade();
            fillEstadoRelacaoCalibracao();
			//fillRazoes(); vai para dentro do fillform, so pode ser chamado depois da ddestado ter sido preenchida
        }

        private void fillNivelPrioridade()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader dr = lista.DRListaNivelPrioridade();
            ddNivelPrioridade.DataSource = dr;
            ddNivelPrioridade.DataBind();

            dr.Close();

            lista = null;
        }

        private void fillMarcas()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR =  lista.DRListaMarcas(); 
            ddMarca.DataSource = DR;
            ddMarca.DataBind(); 
            ddMarca.Items.Insert(0,new ListItem("",""));
            DR.Close(); 

			lista = null; 
        }

        
        private void fillModelos()
        {
            if(ddMarca.SelectedIndex >0)
            {
                DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
                SqlDataReader DR = lista.DRListaModelos(ddMarca.SelectedValue); 
                ddModelo.DataSource= DR;
                ddModelo.DataBind(); 
                ddModelo.Items.Insert(0,new ListItem("",""));
                DR.Close(); 

				lista = null;
            }
        }

        private void fillLocalActual()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaLocalCalibracao();
            ddLocalActual.DataSource = DR;
            ddLocalActual.DataBind();
            DR.Close(); 

			lista = null;
        }

        private void fillFuncTreino()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaFuncionarios(); 
            ddFuncTreino.DataSource = DR;
            ddFuncTreino.DataBind();
            ddFuncTreino.Items.Insert(0,new ListItem("",""));
            DR.Close(); 

			lista = null;
        }

        private void fillEstados()
        {
            DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
            SqlDataReader DR =  servico.DRGetListaEstadosServico(ViewState["idServico"].ToString()); 
            ddEstado.DataSource =DR; 
            ddEstado.DataBind(); 
            DR.Close(); 		
			servico = null;
			
        }

        private void fillEstadoRelacaoCalibracao()
        {
           string strSQL = "SELECT [idEstadoRelacaoCalibracao]     ,[estadoRelacaoCalibracao] as descricao FROM [dbo].[EstadoRelacaoCalibracao]"; 
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
               
            ddEstadoRelacaoCalibracao.DataSource = DR;
            ddEstadoRelacaoCalibracao.DataBind();
            DR.Close();
          

        }

		private void fillRazoes()
		{
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
		
			SqlDataReader DR =  servico.DRGetListaComentariosEstado(ddEstado.SelectedValue); 
			if(DR.HasRows)
			{

				ddComentarioEstado.DataSource = DR; 
				ddComentarioEstado.DataBind(); 
			}
			else
			{
				ddComentarioEstado.DataSource = null;
				ddComentarioEstado.DataBind(); 
			}

			DR.Close(); 
			ddComentarioEstado.Items.Insert(0,new ListItem("",""));
					
			servico = null;
		}


		private void fillEstadosSeguintes(string idEstado)
		{
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
			SqlDataReader DR =  servico.DRGetListaEstadosSeguintes(idEstado); 
			ddEstado.DataSource =DR; 
			ddEstado.DataBind(); 
			DR.Close(); 

			ddEstado.SelectedValue = idEstado; 

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
            ddClassse.DataSource = dr;
            ddClassse.DataBind();
            dr.Close();

            ddClassse.Items.Insert(0,new ListItem("",""));

            SqlDataReader dr2= preco.DRListaUnidadeAlcance(); 
            ddUnidadeAlcance.DataSource=dr2; 
            ddUnidadeAlcance.DataBind(); 
            dr2.Close(); 

            ddUnidadeAlcance.Items.Insert(0,new ListItem("",""));

			preco = null; 
        }

		private void fillTipoEquipamento(string idTipoEquipamento)
		{
			
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaEquipamentosMesmaGrandeza(idTipoEquipamento); 
			ddTipoEquipamento.DataSource = DR2; 
			ddTipoEquipamento.DataBind(); 
			DR2.Close();
			
			lista = null;
		}
    
		private void BindCertificadosServico(string refServico)
		{
			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD(); 
			DataTable DT = data.DTListCertificado("","",refServico,"",""); 
			data = null; 
			
			dgCertificados.DataSource=DT;
			dgCertificados.DataBind(); 

			
			
		}

		private void fillSubTipoServico(string pai)
		{
			string strSQL = "SELECT idSubTipoServico, descricao from subTipoServico where pai = '"+pai+"' AND activo = 1 ";
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddlSubtipoServico.DataSource = dr;
			
			ddlSubtipoServico.DataValueField="idSubTipoServico";
			ddlSubtipoServico.DataTextField="descricao"; 
			ddlSubtipoServico.DataBind(); 
			ddlSubtipoServico.Items.Insert(0,new ListItem("",""));
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
			ddSubtipoEquipamento.DataSource = dr;

			
			ddSubtipoEquipamento.DataBind();
			ddSubtipoEquipamento.Items.Insert(0, new ListItem("", ""));
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
				//ViewState["idTipoEquipamento"] = ddTipoEquipamento.SelectedValue.ToString(); //det.idTipoEquipamento;
				//ViewState["idTipoServico"] = det.idTipoServico;
				//ViewState["idFactura"] =det.idFactura; 
				//ViewState["valor"] =det.valor;
    //            ViewState["acreditado"] = det.acreditado;
    //            //Response.Write(ViewState["acreditado"]);

				////                Response.Write(det.idFactura.ToString()); 
				////                Response.Write(det.valor); 
				////*******************************************************

				// Preencher a informação referente ao Serviço
                lblRefServico.Text = det.refServico;

                 lblRefServico2.Text = "";

                 lblAcessorios.Text = det.acessorios;

                 lblMultiplosServicos.Text = det.bVariasGrandezas;

				if(det.refServico.Substring(0,1)=="A") lblRefServico2.Text += " Aditamento ao serviço <a href='FormPastaEnsaio.aspx?btn=LAB&id="+det.idServicoPai+"' target='_blank'>"+det.refServicoPai+"</a>";

                if (det.idFactura != "") lblFacturado.Text = Resources.Resource.ServicoFacturadoNaoAlterar;

				if(det.idRequisicao!="" && det.nomeFicheiroReq !="") tdReq.InnerHtml = "<img src='IMAGES/ic_seta_red.gif' /> <a href='"+downloadpath(det.nomeFicheiroReq)+"' target='_blank'>Ver Requ.</a>";


               


				try
				{
                    ddLocalActual.SelectedValue = det.idLocalActual;
				}
				catch
				{
                    ddLocalActual.Items.Insert(0, new ListItem(det.localActual, det.idLocalActual));
                    ddLocalActual.SelectedValue = det.idLocalActual; 
				}

				string subtipoPai = det.refServico.Substring(0,4);//isto ja nao faz sentido

                //posso ir pelo idsubtipoServico ou CACV
               // if (subtipoPai == "VGAS" || subtipoPai == "VACV" || subtipoPai == "CACV" || subtipoPai == "VTER" || subtipoPai == "VGPL" || subtipoPai == "VMLP" || subtipoPai == "VSCM" || subtipoPai == "VAGE" || subtipoPai == "VOPC" || subtipoPai == "VBAS")
               if(subtipoPai == "CACV" || det.idTipoServico =="V")
				
				{
					fillSubTipoServico(subtipoPai); 
					ddlSubtipoServico.Visible=true; 
					lblGAS.Text ="Para "+ subtipoPai +": Indicar (Sub-)Tipo de Serviço:";
					try
					{
						ddlSubtipoServico.SelectedValue = det.idSubTipoServico;
                        RequiredFieldValidator1.Enabled = true;
					}
					catch
					{
					}
				}
				else
				{
                    RequiredFieldValidator1.Enabled = false;
					lblGAS.Text =""; 
					ddlSubtipoServico.Visible=false; 
				}

				cbCalExterna.Checked = GERAL.clsGeral.ConvertStringToBoolean(det.calibracaoExterna);
				txtObservacoes.Text = det.observacoes;
				txtObsCliente.Text = det.obsCliente; 
                
				ViewState["idEstadoServicoOriginal"] = det.idEstadoServico;
				try
				{
					ddEstado.SelectedValue = det.idEstadoServico;
				}
				catch
				{
					ddEstado.Items.Insert(0,new ListItem(det.estadoServico,det.idEstadoServico));
					ddEstado.SelectedValue = det.idEstadoServico; 
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

				fillRazoes(); //preencher as razoes associadas ao estado seleccionado
				try
				{
					ddComentarioEstado.SelectedValue = det.idComentarioEstado;
				}
				catch
				{
					//falta aqui isto		//se estiver inactivo, ou pôr a vazio.
					ddComentarioEstado.SelectedValue=""; 
				}
				try
				{
					ddFuncTreino.SelectedValue = det.idFuncionarioTreino;
				}
				catch
				{
					ddFuncTreino.Items.Insert(0,new ListItem(det.FuncionarioTreino,det.idFuncionarioTreino));
					ddFuncTreino.SelectedValue = det.idFuncionarioTreino; 
				}
                
				txtQuantidade.Text = det.quantidade; 
				txtUnidadeQuantidade.Text = det.unidadeQuantidade; 

				if(det.bConforme.ToString()=="False")
				{
					ddConforme.SelectedValue="0";
				}
				else if(det.bConforme.ToString()=="True")
				{
					ddConforme.SelectedValue="1";
				}
				else
				{
					ddConforme.SelectedValue=""; 
				}
				
				if(det.bDeslocacao =="True")
				{
					cbDeslocacao.Checked = true; 
				}
                ddTipoDeslocacao.SelectedValue = det.tipoDeslocacao;

                ddLinguaCertificado.SelectedValue = det.linguaCertificado;
                ddNivelPrioridade.SelectedValue = det.idNivelPrioridade;
                txtDataPrevisao.Text = GERAL.clsGeral.ToShortDate(det.dtPrevisao);
                rbRejeitado.SelectedValue =det.bRejeitado;
                txtNumEtiquetaIPQ.Text = det.numEtiquetaIPQ;


                // Preencher a informação referente ao BRE
                LabMetro.DATA.BreBD fillBRE = new LabMetro.DATA.BreBD(); 
				LabMetro.DATA.BreDetails detBRE = fillBRE.GetBreDetails(det.idBRE); 
				if(detBRE!= null)
				{
					lblRefBRE.Text = detBRE.refBRE; 
					lblDtBRE.Text =  GERAL.clsGeral.ToShortDate(detBRE.dtBRE);
					lblEmpresa.Text = detBRE.nomeCompridoEmpresa;
					
					lblEmpresa.Text += "<br />"+detBRE.morada; 
					lblEmpresa.Text += "<br />"+detBRE.cp + " " + detBRE.localidade;
                    lblEmpresa.Text += "<br />" + detBRE.concelho;
					lblEmpresa.Text += "<br />"+detBRE.obsEmpresa+"<br />"; 
					lblObservacoes.Text = detBRE.observacoes;
                    
					if(detBRE.idEmpresaContratante!="") lblEmpresa.Text+="CONTRATADO POR: " + detBRE.nomeEmpresaContratante; 

					ViewState["idEmpresa"] = detBRE.idEmpresa;
					ViewState["idEmpresaContratante"] = detBRE.idEmpresaContratante;
				}

				// Preencher a informação referente ao Equipamento
				LabMetro.DATA.EquipamentoBD fillEquipamento = new LabMetro.DATA.EquipamentoBD(); 
				LabMetro.DATA.EquipmentDetails detEquipamento = fillEquipamento.GetEquipmentDetails(det.idEquipamento); 
				if(detEquipamento!= null)
				{
					ViewState["idEquipamento"] = detEquipamento.idEquipamento; // necessário para o update
					ViewState["idTipoEquipamento"] = detEquipamento.idTipoEquipamento;



					//******************************************************************
					//******************************************************************
					
					fillTipoEquipamento(det.idTipoEquipamento);
					//try //estranhamente, isto funcionava mas ja nao funciona! nao dá qquando o valor nao exsite! 2020!

					//{
					
					//catch
					//{
					//	ddTipoEquipamento.Items.Insert(0,new ListItem(detEquipamento.tipoEquipamento,detEquipamento.idTipoEquipamento));
					//	ddTipoEquipamento.SelectedIndex = 0; //o primeiro
					//	lblTipoEquipamento.Text = "Atenção, tipo INACTIVO! Por favor actualizar o tipo!";
					//	lblTipoEquipamento.ForeColor = Color.Red;
					//	ddTipoEquipamento.Focus();

					//}
					try
					{
						//	ddTipoEquipamento.SelectedValue = detEquipamento.idTipoEquipamento
						//2020 esta sintaxo ja nao funciona!!!!! 

						ddTipoEquipamento.Items.FindByValue(det.idTipoEquipamento).Selected = true;
					}


					catch
                    {
                        ddTipoEquipamento.Items.Insert(0, new ListItem(detEquipamento.tipoEquipamento, ""));
						//ponho o id a emtpy para obrigar a preencher

                        ddTipoEquipamento.SelectedIndex = 0; //o primeiro
                        lblTipoEquipamento.Text = "Atenção, tipo INACTIVO! Por favor actualizar o tipo!";
                        lblTipoEquipamento.ForeColor = Color.Red;
                        ddTipoEquipamento.Focus();

                    }

                    

					//no fillform, o subtipo é preenchdio com base no tipo que vem do serviço
					FillSubTipoEquipamento(det.idTipoEquipamento);
					try
					{
						ddSubtipoEquipamento.SelectedValue = det.idSubTipoEquipamento;
					}
					catch
					{
						ddSubtipoEquipamento.Items.Insert(0, new ListItem(det.subTipoEquipamento, det.idSubTipoEquipamento));
						ddSubtipoEquipamento.SelectedIndex = 0; //o primeiro
					}


					lblIdEquipanmento.Text="(ID do Equip. na BD: "+detEquipamento.idEquipamento +")"; 
					//******************************************************************
					//******************************************************************
					//lblTipoEquipamento.Text = detEquipamento.tipoEquipamento;
					//label foi subsituida por uma dropdown
					txtNumIdentificacao.Text = detEquipamento.numIdentificacao;
					txtNumSerie.Text = detEquipamento.numSerie;
                    txtCodigoIPQ.Text = detEquipamento.codigoIPQ;

					txtForma.Text = detEquipamento.forma;
					try
					{
						ddClassse.SelectedValue = detEquipamento.idClasse; 
					}
					catch 
					{  
					}
					txtClasse.Text = detEquipamento.classe;
					txtAlcanceInf.Text = detEquipamento.alcanceInf; 
					txtAlcanceSup.Text = detEquipamento.alcanceSup; 
					try
					{
						ddUnidadeAlcance.SelectedValue = detEquipamento.idUnidadeAlcance;
					}
					catch 
					{  
					}
					txtAlcance.Text = detEquipamento.alcance;
					txtResolucao.Text = detEquipamento.resolucao;
				

					ViewState["grandeza"] = fillEquipamento.GetGrandeza(detEquipamento.idTipoEquipamento); 

					// Carregar combo das marcas
					fillMarcas();

					try
					{
						ddMarca.SelectedValue = detEquipamento.idmarca; //ddMarca.Items.FindByText(detEquipamento.marca).Value;
					}
					catch 
					{	
					}

					if (detEquipamento.idmarca != "")
						fillModelos(); // Carregar combo dos modelos

					try
					{
						ddModelo.SelectedValue = detEquipamento.idmodelo; 
					}
					catch 
					{
					}
					
					if(detEquipamento.certConclusivo =="True") 
					{
						cbCertConclusivo.Checked = true;
						ddConforme.Enabled=true;
					} 
					else
					{
						ddConforme.Enabled=false;
					}

					txtCampo1.Text = detEquipamento.campo1;
					txtCampo2.Text = detEquipamento.campo2;

                    txtEtiqueta1.Text = detEquipamento.etiqueta1;
                    txtEtiqueta2.Text = detEquipamento.etiqueta2;
                    txtEtiqueta3.Text = detEquipamento.etiqueta3;
                    txtObsEquipamento.Text = detEquipamento.observacoes;

                    txtCriteriosEquipamento.Text = detEquipamento.criterios;
                    try
                    {
                        ddEstadoRelacaoCalibracao.SelectedValue = detEquipamento.idEstadoRelacaoCalibracao;
                    }
                    catch
                    {
                    }
				}

				bool bDef = System.Convert.ToBoolean(det.bDefinitivo); 
				if(bDef ==false) 
				{
					lblMessage.Text="Serviço não definitivo."; 
					lblMessage.Text+="Tem de passar o BRE de calibração externa para definitivo primeiro."; 
					btnSubmit.Enabled=false;
				}

				fillServico = null; 
				fillEquipamento = null; //novo nov 2008

                DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
				dtLinhasServico = servico.dtLinhasServicoByIdServico(ViewState["idServico"].ToString());
                
				ViewState["dtLinhasServico"] = dtLinhasServico; 
                BindGridLinhasServico();

                servico = null;

                DATA.PastaEnsaioBD pasta = new LabMetro.DATA.PastaEnsaioBD(); 
                dtCertificados = pasta.dtCertificadosByIdServico(ViewState["idServico"].ToString()); 
                
                ViewState["dtCertificados"] = dtCertificados; 


                BindCertificadosServico(det.refServico); 

				BindGridHistorico(det.idServico); 
				BindGridServicosAnteriores(det.idEquipamento, det.idServico); 
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
            DGLinhasServico.EditCommand += new DataGridCommandEventHandler (DGLinhasServico_EditCommand);  
            DGLinhasServico.ItemCommand += new DataGridCommandEventHandler (DGLinhasServico_ItemCommand); 
            DGLinhasServico.CancelCommand += new DataGridCommandEventHandler (DGLinhasServico_CancelGrid); 
            DGLinhasServico.UpdateCommand += new DataGridCommandEventHandler (DGLinhasServico_UpdateGrid); 
            DGLinhasServico.DeleteCommand += new  DataGridCommandEventHandler(DGLinhasServico_DeleteCommand);

			ddMarca.SelectedIndexChanged += new System.EventHandler(ddMarca_SelectedIndexChanged);
			ddTipoEquipamento.SelectedIndexChanged += new System.EventHandler(ddTipoEquipamento_SelectedIndexChanged);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnEtiqueta.Click += new System.EventHandler(btnEtiqueta_Click);
			btnEtiquetaCal.Click += new System.EventHandler(btnEtiquetaCal_Click);
			btnRequisicoes.Click += new System.EventHandler(btnRequisicoes_Click);
			btnOrcamentos.Click += new System.EventHandler(btnOrcamentos_Click);
			DGOrcamentos.SelectedIndexChanged += new System.EventHandler(DGOrcamentos_SelectedIndexChanged);
			ddEstado.SelectedIndexChanged += new System.EventHandler(ddEstado_SelectedIndexChanged);
			
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
            int index = DGLinhasServico.EditItemIndex;
          
            if(index >= 0)
            {    
                
                dtLinhasServico = (DataTable)ViewState["dtLinhasServico"]; 
                dvLinhasServico = new DataView(dtLinhasServico); 
                dvLinhasServico[index]["numPecas"] = txtNumPecas.Text; 
                dvLinhasServico[index]["numPontos"] = txtNumPontos.Text; 
                dvLinhasServico[index]["pontosCalib"] = txtPontosCalib.Text; 
                ViewState["dtLinhasServico"] = dtLinhasServico; 
            }

            DGLinhasServico.EditItemIndex = -1; 
            BindGridLinhasServico();
            DGLinhasServico.ShowFooter = true;
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
                        lblMessage.Text ="Tem de preencher o campo 'Nº de Peças' antes de adicionar."; 
                    else if (txtNumPontos.Text=="") 
                        lblMessage.Text ="Tem de preencher o campo 'Nº de Pontos' antes de adicionar."; 
                    else if (txtPontosCalib.Text=="") 
                        lblMessage.Text ="Tem de preencher o campo 'Pontos de Calibração' antes de adicionar."; 
                    else
                    {
                        dtLinhasServico = (DataTable)ViewState["dtLinhasServico"]; 
                        dvLinhasServico = new DataView(dtLinhasServico); 
                        // Adicionar a nova linha ao DataView
                        DataRowView drv = dvLinhasServico.AddNew();
                        drv["numPecas"] = txtNumPecas.Text; 
                        drv["numPontos"] = txtNumPontos.Text; 
                        drv["pontosCalib"] = txtPontosCalib.Text; 
                        drv.EndEdit();
                        ViewState["dtLinhasServico"] = dtLinhasServico; 
                    }
                }
                DGLinhasServico.EditItemIndex = -1; 
                BindGridLinhasServico(); 
            }
        }

        private void DGLinhasServico_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dtLinhasServico = (DataTable)ViewState["dtLinhasServico"]; 
            dvLinhasServico = new DataView(dtLinhasServico); 

            dvLinhasServico.Delete(e.Item.ItemIndex);

            ViewState["dtLinhasServico"] = dtLinhasServico;
            BindGridLinhasServico();
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
				Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");
			}
		}

		public string downloadpathCert(object filename)
		{
			if(filename!=null && filename.ToString()!="") 
			{

//				Response.Write(Request.Url.Segments[0].ToString());
//				Response.Write(Request.Url.Segments[1].ToString());
//				Response.Write(Request.Url.Segments[2].ToString());
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
        //    string idTipoEquipamento = ddTipoEquipamento.SelectedValue.ToString(); //ViewState["idTipoEquipamento"].ToString(); 
        //    string idTipoServico = ViewState["idTipoServico"].ToString(); 

        //    //a unidade da quantidade nao interessa pois a cada tipo de equipamento so
        //    //está atribuido um tipo de quantidade.    
        //    string quantidade= txtQuantidade.Text;
        //    //retorna algo a dizer se é calibração interna ou externa, para móvel, não ha nada

        //    //movel, nao sei onde se vê; por isso so vou distinguir entre externa e interna
        //    string tipoPr = "preco";  

        //    if(cbCalExterna.Checked == true)
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
                
        //            precoServico = preco.getPriceByTipoEquipamentoAlcanceSimples(idTipoEquipamento,idTipoServico,ddUnidadeAlcance.SelectedValue,txtAlcanceInf.Text,txtAlcanceSup.Text, tipoPr); 
        //            break; 

                    
        //        case "4": //tipo de equipamento + alcance + numPontos/pares
                
        //            //aqui fazer um loop pelas possiveis linhas do datagrid linhas servico
        //            dtLinhasServico = (DataTable)ViewState["dtLinhasServico"]; 
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
        //                        lblMessage.Text="Por favor, verifique as linhas do Serviço.<br />Com base nos critérios introduzidos, não é possível calcular o preço do serviço.<br />"; 
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

        //            precoServico = preco.getPriceByTipoEquipamentoAlcanceSimplesClasse(idTipoEquipamento,idTipoServico,ddUnidadeAlcance.SelectedValue,txtAlcanceInf.Text,txtAlcanceSup.Text,ddClassse.SelectedValue,tipoPr); 
        //        break; 
                 
        //        case "6"://marca-modelo 
        //            if(ViewState["grandeza"].ToString() == "ELE") //preços por marca modelo
        //                //so para electricos E CTA???? PRIMEIRO SO PARA ELECTRICOS... 
        //                //CTA VIERAM DPS MAS POR OUTRAS RAZOES (EMPRESAS MANUTENCAO ASSOCIADAS À MARCA). 
        //            {
        //                precoServico = preco.getPriceByIdModelo(idTipoEquipamento,ddModelo.SelectedValue,idTipoServico,tipoPr); 

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
			

            dtCertificados = (DataTable)ViewState["dtCertificados"]; 
            dvCertificados = new DataView(dtCertificados); 

            dtLinhasServico = (DataTable)ViewState["dtLinhasServico"]; 
            dvLinhasServico = new DataView(dtLinhasServico); 

           
			
			DATA.PastaEnsaioBD pastaEnsaio = new LabMetro.DATA.PastaEnsaioBD(); 
                  
			if(Page.IsValid)
			{
				if(ddEstado.SelectedValue =="7" || ddEstado.SelectedValue=="9")
				{
					if(ddComentarioEstado.SelectedValue =="") 
					{
						lblMessage.Text="Indique a razão da Anulação ou Suspensão."; 
						return; 
					}
				}

                if (cbDeslocacao.Checked == true)
                {
                    if (ddTipoDeslocacao.SelectedValue == "0")
                    {
                        lblMessage.Text = "Indique qual a deslocação aplicável.";
                        return;
                    }
                }
				
				string	idmarca = ddMarca.SelectedValue; 
				string idmodelo = ddModelo.SelectedValue; 
				
				string bDeslocacao = "0";
				if (cbDeslocacao.Checked==true) bDeslocacao = "1";

                //if (myApp.ToUpper() == "ES_LABMETRO")
                //{

                //    if (ddEstado.SelectedValue == "6" || ddEstado.SelectedValue == "25")
                //    {

                //        if (txtNumIdentificacao.Text == "" || txtNumSerie.Text == "" || ddMarca.SelectedItem.Text == "" || ddModelo.SelectedItem.Text == "" || txtAlcanceInf.Text == "" || txtAlcanceSup.Text == "" || ddUnidadeAlcance.SelectedItem.Text == "" || txtResolucao.Text == "")
                //        {
                //            lblMessage.Text = "Por favor rellenar todos los campos: num. Serie, num. ID y los demas datos del equipo .";
                //            return;
                //        }
                //    }
 
                //}
				string  strError  = pastaEnsaio.UpdatePastaEnsaio(ViewState["idServico"].ToString(), ddLocalActual.SelectedValue, cbCalExterna.Checked.ToString(), txtObservacoes.Text, ddEstado.SelectedValue.ToString(), ViewState["idEquipamento"].ToString(), txtNumIdentificacao.Text, txtNumSerie.Text, txtForma.Text,ddClassse.SelectedValue, txtClasse.Text,txtAlcanceInf.Text,txtAlcanceSup.Text,ddUnidadeAlcance.SelectedValue, txtAlcance.Text, txtResolucao.Text, idmarca, idmodelo, ddFuncTreino.SelectedValue.ToString(), dvLinhasServico, dvCertificados, User.Identity.Name.ToString(),txtQuantidade.Text, txtUnidadeQuantidade.Text, null, ViewState["idEmpresa"].ToString(), ddTipoEquipamento.SelectedValue.ToString(),txtObsCliente.Text, ddComentarioEstado.SelectedValue.ToString(),ddlSubtipoServico.SelectedValue.ToString(),ViewState["idEstadoServicoOriginal"].ToString(),txtCampo1.Text,txtCampo2.Text,ddConforme.SelectedValue, bDeslocacao, ddNivelPrioridade.SelectedValue, txtDataPrevisao.Text, txtEtiqueta1.Text, txtEtiqueta2.Text, txtEtiqueta3.Text, ddLinguaCertificado.SelectedValue, txtObsEquipamento.Text, ddTipoDeslocacao.SelectedValue, txtCodigoIPQ.Text, ddEstadoRelacaoCalibracao.SelectedValue, rbRejeitado.SelectedValue, txtNumEtiquetaIPQ.Text, ddSubtipoEquipamento.SelectedValue);

				lblMessage.Text += strError; 
				if(strError == 	GERAL.clsGeral.ErrorMessage.MSG_DB)
				{
					ViewState["idEstado"] = ddEstado.SelectedValue.ToString(); 
					ViewState["idEstadoServicoOriginal"] = ddEstado.SelectedValue.ToString();
				
					//para nao ver as linhas da dataview sempre como unchanged fazer o commit das alterações após o update.
					//MUITO IMPORTANTE! (para nao repetir linhas).
					dtLinhasServico.AcceptChanges(); 
					//dtCertificados.AcceptChanges(); 

					//para mostrar os estados proximos, caso se tenha mudado o estado.
					fillEstadosSeguintes(ViewState["idEstado"].ToString()); 
					BindGridHistorico(ViewState["idServico"].ToString()); 
				}
			}

			pastaEnsaio = null; 
        }

        private void ddMarca_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillModelos(); 
        }

		
		private void ddTipoEquipamento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			FillSubTipoEquipamento(ddTipoEquipamento.SelectedValue);
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
			DataSet ds  = bre.DSEtiquetaServico(ViewState["idServico"].ToString()); 	
				
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
                    //if (ViewState["acreditado"].ToString() == "False")
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
			DataSet ds  = bre.DSEtiquetaCalibracao(ViewState["idServico"].ToString());

            //Response.Write(   ds.Tables["dtEtiquetaPosCalib"].Rows[0]["dtProximaCalibracao"].ToString());


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
			if((txtNumIdentificacao.Text.Length == 0 && txtNumSerie.Text.Length ==0) ||(txtNumIdentificacao.Text.ToString()== "---" && txtNumSerie.Text.ToString() =="---") ||(txtNumIdentificacao.Text.ToString() =="---" && txtNumSerie.Text.Length==0) || (txtNumSerie.Text.ToString() == "---" && txtNumIdentificacao.Text.Length==0))
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
			DataTable dt  = req.DTListaRequisicoes(ViewState["idEmpresa"].ToString(),"","","",false,false); 
    
			DGRequisicoes.DataSource = dt;
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
			if(DGRequisicoes.Visible == false)
			{
				DGRequisicoes.Visible= true;
			}
			else
			{
				DGRequisicoes.Visible= false; 
			}
		}

		//BINDGRID DOS ORÇAMENTOS
		private void BindGrid()
		{
			DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD(); 
			DataTable dt = orcamento.DTOrcamentos("","","","",ViewState["idEmpresa"].ToString(),null); 

			DataView DV = new DataView(dt);
	
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
            
			DGOrcamentos.DataSource = DV;
			DGOrcamentos.DataBind(); 

			orcamento = null; 

		}

		//SORT DOS ORÇAMENTOS
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

		//PAGING DOS ORÇAMENTOS
		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DGOrcamentos.CurrentPageIndex = e.NewPageIndex;
			BindGrid(); 
		}

		private void btnOrcamentos_Click(object sender, System.EventArgs e)
		{
			if(DGOrcamentos.Visible == false)
			{
				DGOrcamentos.Visible= true;
				dgLinhasOrcamento.Visible = true; 
				dgComentariosOrcamento.Visible=true;
			}
			else
			{
				DGOrcamentos.Visible= false; 
				dgLinhasOrcamento.Visible = false; 
				dgComentariosOrcamento.Visible=false;
			}
		}


		//preencher a label empresa devedora com os respectivos dados, caso aplicável
		private void fillCompanyInfo()
		{

			string idEmpresa = ViewState["idEmpresa"].ToString();
			if(ViewState["idEmpresaContratante"].ToString() !="") 
			{
				idEmpresa = ViewState["idEmpresaContratante"].ToString();
				

			}
			
			lblEmpresaDevedora.Text =""; //limpar a label

			LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(idEmpresa); 
			if(detEmpresa != null)
			{


                ///colocar aqui o nome do ficheiro dos criterios de aceitacao
                if (detEmpresa.ficheiroCriterios != "" && detEmpresa.ficheiroCriterios != "") tdFicheiroCriterios.InnerHtml = "<img src='IMAGES/ic_seta_red.gif' /> <a href='" + downloadpathCriterios(detEmpresa.ficheiroCriterios) + "' target='_blank'>Ver Criterios do Cliente</a>";

                txtCriteriosAceitacaoEmpresa.Text = detEmpresa.criteriosAceitacao;

				if (detEmpresa.pagamentoAtraso =="1")
				{
					lblEmpresaDevedora.Text = "** PAGAMENTOS EM ATRASO **<br />";
				}
				else
				{
					lblEmpresaDevedora.Text = ""; 
				}
				

				switch(detEmpresa.nivelBloqueioLabmetro)	
				{ 
					case "0": 
						//lblEmpresa.BackColor =System.Drawing.ColorTranslator.FromHtml("#cccccc");
						break;
					case "1":
						lblEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("Gold");
;  
						break; 
					case "2": 
						lblEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("DarkOrange");
						lblEmpresaDevedora.Text +="Venda à dinheiro ou Pagamento do Atrasado.<br />"; 
						break; 
					case "3": 
						lblEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("Crimson");
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

		
		private void DGOrcamentos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string idOrcamento = DGOrcamentos.DataKeys[DGOrcamentos.SelectedIndex].ToString();
				
			DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD(); 
			
			DataTable dt = orc.DTLinhasOrcamentoByIdOrcamento(idOrcamento); 
			DataView dv = new DataView(dt); 
			dgLinhasOrcamento.DataSource = dv; 
			dgLinhasOrcamento.DataBind();

			dt = orc.DTComentariosOrcamentoByIdOrcamento(idOrcamento); 
			dv = new DataView(dt); 
			dgComentariosOrcamento.DataSource = dv;
			dgComentariosOrcamento.DataBind();

			orc = null; 
			
		}

		private void ddEstado_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillRazoes();

            if (ddEstado.SelectedItem.Value.ToString() == "25" || ddEstado.SelectedItem.Value.ToString() == "26")
            {
                cbCalExterna.Checked = true;
            }
		}

		private void BindGridServicosAnteriores(string idEquipamento, string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
			arrParams[0] = new SqlParameter("@inIdEquipamento",idEquipamento);
			arrParams[1] = new SqlParameter("@inIdServicoActual",idServico);
				
			DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicosCertificadosByIdEquipamento",arrParams); 
					
			dgServicosAnteriores.DataSource =dt;
			dgServicosAnteriores.DataBind();
		}
    }
}