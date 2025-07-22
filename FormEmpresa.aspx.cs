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
using System.Text.RegularExpressions;


using System.IO; 



namespace LabMetro
{
	/// <summary>
	/// Summary description for FormEmpresa.
	/// </summary>
    public partial class FormEmpresa : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.DropDownList ddCodigoPostal;
        //protected System.Web.UI.WebControls.Button btnResumo;
        protected System.Web.UI.WebControls.RadioButtonList Radiobuttonlist1;
        //protected System.Web.UI.WebControls.Button btnResumo;
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

        private const string ID_PAG = "EMPRESAS_1";//NOME DA PAGIN

        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblMessage.Text = "";

            Hashtable ht = (Hashtable)Session["HTPermissions"];
            if (ht == null) //session expired
            {
                Server.Transfer("Default.aspx?err=2", false);
            }
            else
            {
                if (!ht.ContainsKey(ID_PAG))
                {
                    Server.Transfer("Default.aspx?err=1", false);
                }
                else
                {
                    int intAcesso = System.Convert.ToInt32(ht[ID_PAG]);
                    //if(intAcesso == 1)//tem permissoes para tudo. 

                    if (intAcesso == 0)
                    {
                        btnSubmit.Enabled = false;
                    }

                    if (!Page.IsPostBack)
                    {
                        fillDropDowns();
                        setPermissionsPagamentosAtraso();

                        if (Request.QueryString["id"] != null)
                        {
                            if (Request.QueryString["id"] != "")
                            {
                                ViewState["idEmpresa"] = Request.QueryString["id"].ToString();
                                fillDDEmpresa();
                                fillForm(ViewState["idEmpresa"].ToString());
                                BindGridHistorico();
                                btnSubmit.CommandArgument = "update";
                            }
                            else
                            {

                                btnSubmit.CommandArgument = "insert";
                            }
                        }
                        else
                        {
                            btnSubmit.CommandArgument = "insert";
                        }


                    }
                }
            }


        }

        private void setPermissionsPagamentosAtraso()
        {
            bool b = validaUsersPagamentosAtraso();

            rblPagamentoAtraso.Enabled = b;
            ddNivelBloqueio.Enabled = b;
            //ddCodigoBloqueioSap.Enabled=b; //abril 2009 fica semper a disabled
            rblRequisicaoAtraso.Enabled = b;
            ddCondPagamentoFactura.Enabled = b;
            cbVerCertifsSemReq.Enabled = b;

        }
        private void fillDDEmpresa()
        {
            DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            DataTable DT = empresa.DTEmpresas(txtPesquisaEmpresa.Text, txtPesquisaNif.Text, "1", "", "", "", "", "", ""); //estado 1 = estado activo

            DataView DV = new DataView(DT);

            ddEmpresaPai.DataSource = DV; ;
            ddEmpresaPai.DataBind();
            

            empresa = null;

            ddEmpresaPai.Items.Insert(0, new ListItem("", "")); //para ter sempre um item vazio
        }

        private void fillDropDowns()
        {

            SqlDataReader dra = clsDataAccess.ExecuteDR("select idActividade, descricao from actividade order by descricao");
            ddActividade.DataSource = dra;
            ddActividade.DataBind();
            ddActividade.Items.Insert(0, new ListItem("", ""));
            dra.Close();
            dra = null;

            SqlDataReader DR2 = clsDataAccess.ExecuteDR("select idConcelho, descricao from concelho order by descricao");
            ddConcelho.DataSource = DR2;
            ddConcelho.DataBind();
            ddConcelho.Items.Insert(0, new ListItem("", ""));
            DR2.Close();
            DR2 = null;

            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR3 = lista.DRListaTiposEmpresa();
            ddTipoEmpresa.DataSource = DR3;
            ddTipoEmpresa.DataBind();
            DR3.Close();

            DR3 = null;

            SqlDataReader DR4 = lista.DRListaEstadosEmpresas();
            ddEstado.DataSource = DR4;
            ddEstado.DataBind();
            DR4.Close();

            DR4 = null;


            SqlDataReader DR5 = lista.DRListaPaises();
            ddPais.DataSource = DR5;
            ddPais.DataBind();
            //ddPais.Items.Insert(0, new ListItem("", ""));
           
            DR5.Close();
            ddPais.SelectedValue = "351";
            DR5 = null;

            lista = null;

            DATA.FacturaData f = new LabMetro.DATA.FacturaData();

            SqlDataReader dr;
            //CÓDIGOS BLOQUEIO
            dr = f.drCodigoBloqueioSap();
            ddCodigoBloqueioSap.DataTextField = "descricao";
            ddCodigoBloqueioSap.DataValueField = "id";
            ddCodigoBloqueioSap.DataSource = dr;
            ddCodigoBloqueioSap.DataBind();
            dr.Close();
            dr = null;


            //CONDIÇÕES DE PAGAMENTO - da empresa e da factura
            dr = f.drCondicoesPagamento();

            ddCondPagamentoFactura.DataTextField = "descricao";
            ddCondPagamentoFactura.DataValueField = "id";
            ddCondPagamentoFactura.DataSource = dr;
            ddCondPagamentoFactura.DataBind();
            dr.Close();
            dr = null;
            //ddCondPagamentoFactura.Items.Insert(0,new ListItem("","")); //isto tem de ser tirado depois, not null



            //REGIÃO DE VENDAS
            dr = f.drRegiaoVendas();
            ddRegiaoVendas.DataTextField = "descricao";
            ddRegiaoVendas.DataValueField = "id";
            ddRegiaoVendas.DataSource = dr;
            ddRegiaoVendas.DataBind();
            dr.Close();
            dr = null;
            ddRegiaoVendas.Items.Insert(0, new ListItem("", ""));

            SqlDataReader drFuncionario = clsDataAccess.ExecuteDR("select idFuncionario, nomeAbreviado  from funcionario where activo = 1 order by 2");
            ddFuncionario.DataSource = drFuncionario;
            ddFuncionario.DataBind();
            ddFuncionario.Items.Insert(0, new ListItem("", ""));
            drFuncionario.Close();
            drFuncionario = null;

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
        private void fillForm(string id)
        {
            LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            LabMetro.DATA.CompanyDetails detalhesEmpresa = empresa.GetCompanyDetails(id);

            if (!Convert.IsDBNull(detalhesEmpresa.idEmpresaPai) && detalhesEmpresa.idEmpresaPai.ToString() != "")
            {
                try
                {
                    ddEmpresaPai.SelectedValue = detalhesEmpresa.idEmpresaPai;
                }
                catch
                {
                    ddEmpresaPai.Items.Insert(0, new ListItem(detalhesEmpresa.nome, detalhesEmpresa.idEmpresaPai));
                    ddEmpresaPai.SelectedValue = detalhesEmpresa.idEmpresaPai;
                }
            }


            try
            {
                ddTipoEmpresa.SelectedValue = detalhesEmpresa.idTipoEmpresa;
            }
            catch
            {
                ddTipoEmpresa.Items.Insert(0, new ListItem(detalhesEmpresa.tipoEmpresa, detalhesEmpresa.idTipoEmpresa));
                ddTipoEmpresa.SelectedValue = detalhesEmpresa.idTipoEmpresa;
            }

            try
            {
                ddPais.SelectedValue = detalhesEmpresa.idPais;
            }
            catch
            {
                ddPais.Items.Insert(0, new ListItem(detalhesEmpresa.pais, detalhesEmpresa.idPais));
                ddPais.SelectedValue = detalhesEmpresa.idPais;
            }
            try
            {
                ddEstado.SelectedValue = detalhesEmpresa.idEstadoEmpresa;
            }
            catch
            {
                //falta;
            }


            try
            {
                ddActividade.SelectedValue = detalhesEmpresa.idActividade;
            }
            catch
            {

            }

            txtLocalidade.Text = detalhesEmpresa.localidade;
            txtLocalidadePostal.Text = detalhesEmpresa.localidadePostal;
            txtLatitude.Text = detalhesEmpresa.latitude;
            txtLongitude.Text = detalhesEmpresa.longitude;
            ddConcelho.SelectedValue = detalhesEmpresa.idConcelho;
            txtNomeEmpresa.Text = detalhesEmpresa.nome;
            txtNomeAbrev.Text = detalhesEmpresa.nomeAbreviado;
            txtNIF.Text = detalhesEmpresa.nif;
            txtMorada.Text = detalhesEmpresa.morada;
            txtCodigoPostal.Text = detalhesEmpresa.cp;
            txtTelefone1.Text = detalhesEmpresa.telefone;
            txtTelefone2.Text = detalhesEmpresa.telefone2;
            txtFax.Text = detalhesEmpresa.fax;
            txtEmail.Text = detalhesEmpresa.email;
            rblSede.SelectedValue = detalhesEmpresa.sede;
            rblPagamentoAtraso.SelectedValue = detalhesEmpresa.pagamentoAtraso;
            ddNivelBloqueio.SelectedValue = detalhesEmpresa.nivelBloqueioLabmetro;
            ddCodigoBloqueioSap.SelectedValue = detalhesEmpresa.codigoBloqueioSAP;
            rblRequisicaoAtraso.SelectedValue = detalhesEmpresa.requisicaoAtraso;
            rblContrato.SelectedValue = detalhesEmpresa.bContrato;
            txtNumObra.Text = detalhesEmpresa.numObra;
            setPermissionsPagamentosAtraso(); //permissoes gerais qe sao mudadas a seguir
            txtDocumentacaoEntrada.Text = detalhesEmpresa.documentacaoEntrada;
            cbRequerDocEntrada.Checked = detalhesEmpresa.bRequerDocEntrada;


            //daniela janeiro 2009 tirei isto nao sei se deve ficar ou se faz sentido. 
            //if(detalhesEmpresa.codigoBloqueioSAP =="01") //falidas
            //{

            //    ddNivelBloqueio.SelectedValue ="3"; 
            //    ddNivelBloqueio.Enabled=false; 
            //    ddCodigoBloqueioSap.Enabled=false; 

            //}
            //else if(detalhesEmpresa.codigoBloqueioSAP=="02")//contencioso
            //{
            //    ddNivelBloqueio.SelectedValue ="3"; 
            //    //ddNivelBloqueio.Enabled=false; as contenciosas podem mudar

            //}


            txtDesconto.Text = detalhesEmpresa.percDesconto;
            txtObservacoes.Text = detalhesEmpresa.observacoes;

            try
            {
                ddRegiaoVendas.SelectedValue = detalhesEmpresa.idCodigoRegiaoVendas;
            }
            catch
            {
                ddRegiaoVendas.Items.Insert(0, new ListItem(detalhesEmpresa.regiaoVendas, detalhesEmpresa.idCodigoRegiaoVendas));
                ddRegiaoVendas.SelectedValue = detalhesEmpresa.idCodigoRegiaoVendas;
            }

            try
            {
                ddCondPagamentoFactura.SelectedValue = detalhesEmpresa.idCondicoesPagamento;
            }
            catch
            {
                ddCondPagamentoFactura.Items.Insert(0, new ListItem(detalhesEmpresa.condicoesPagamento, detalhesEmpresa.idCondicoesPagamento));
                ddCondPagamentoFactura.SelectedValue = detalhesEmpresa.idCodigoRegiaoVendas;
            }

            txtNumClienteSAP.Text = detalhesEmpresa.numClienteSAP;
            cbVerCertifsSemReq.Checked = detalhesEmpresa.bCertifsSemReq;
            cbNPrecisaReqPFacturar.Checked = detalhesEmpresa.bPodeFacturarSemRequisicao;
            cbGestaoEquipamentos.Checked = detalhesEmpresa.bGestaoEquipamentos;
          

            try
            {
                ddFuncionario.SelectedValue = detalhesEmpresa.idFuncionarioGestaoEquipamentos;
            }
            catch
            {
                ddFuncionario.Items.Insert(0, new ListItem(detalhesEmpresa.FuncionarioGestaoEquipamentos, detalhesEmpresa.idFuncionarioGestaoEquipamentos));
                ddFuncionario.SelectedValue = detalhesEmpresa.idFuncionarioGestaoEquipamentos;
            }

            txtCreditoMax.Text = detalhesEmpresa.creditoMax;
            txtWebsite.Text = detalhesEmpresa.website;
            txtmodoPagamento.Text = detalhesEmpresa.modoPagamento;
            txtModoEntrega.Text = detalhesEmpresa.modoEntrega;

            txtCriteriosAceitacao.Text = detalhesEmpresa.criteriosAceitacao;//.Replace("\n", "<br />");
            txtNomeFicheiro.Text = detalhesEmpresa.ficheiroCriterios;

            //para abrir o ficheiro
            if (detalhesEmpresa.ficheiroCriterios != null && detalhesEmpresa.ficheiroCriterios != "")
            {
               

               

                linkFicheiro.NavigateUrl = downloadpathCriterios(detalhesEmpresa.ficheiroCriterios);
                
            }
            else
            {
               linkFicheiro.NavigateUrl = "#";
            }


             



            empresa = null;
            detalhesEmpresa = null;
        }

        public string downloadpathCriterios(object filename)
        {
            if (filename != null && filename.ToString() != "")
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

        private bool validaPageData()
        {
            bool b = true;

            if ((rblSede.SelectedValue == "0") && (ddEmpresaPai.SelectedValue == ""))
            {
                lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_ASSOCIAR_EMPRESA_GRUPO + "<br />";
                b = false;
            }
            else if ((rblSede.SelectedValue == "1") && (ddEmpresaPai.SelectedValue != ""))
            {
                lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_EMPRESA_SEDE_TIRAR_GRUPO + "<br />";
                b = false;
            }



            //se pagamentoAtraso = SIM OU requisicaoatraso = SIM entao nivelBloqueio nao pode estar a livre
            if (rblPagamentoAtraso.SelectedValue == "1" || rblRequisicaoAtraso.SelectedValue == "1")
            {
                if (ddNivelBloqueio.SelectedValue == "0")
                {
                    lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_AUMENTAR_BLOQ_AMARELO_LARANJA + "<br />";
                    b = false;
                }
            }
            else
            {

                if ((ddNivelBloqueio.SelectedValue == "1" || ddNivelBloqueio.SelectedValue == "2") && ddCodigoBloqueioSap.SelectedValue == "00")
                {
                    lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_BLOQ_DEVE_ESTAR_LIVRE;
                    b = false;
                }
            }

            //se pagamentoAtraso = NO E requisicaoatraso = NAO entao nivelBloqueio nao amarelo ou laranja (mas pode estar vermelho)
            if ((ddNivelBloqueio.SelectedValue == "1" || ddNivelBloqueio.SelectedValue == "2") && (rblPagamentoAtraso.SelectedValue == "0" && rblRequisicaoAtraso.SelectedValue == "0"))
            {
                lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_LIMPAR_BLOQUEIO_INTERNO + "<br />";
                b = false;
            }

            //se empresa em contencioso OU em falencia, nivel de bloqueio nao pode estar diferente de vermelho
            //se a empresa está bloqueada em SAP, não se pode emitir facturas e por isso tem de estar a vermelho!
            //o problema é que nenhuma empresa a vermelho pode ver os seus certificados via web, mesmo que esteja
            //por ex. em "plano pagamento.

            //o unico codigo de bloqueio SAP diferente aqui é o 4 = cliente inactivo. 


            if (ddCodigoBloqueioSap.SelectedValue == "01" || ddCodigoBloqueioSap.SelectedValue == "02"
                || ddCodigoBloqueioSap.SelectedValue == "04" || ddCodigoBloqueioSap.SelectedValue == "09")
            {
                if (ddNivelBloqueio.SelectedValue != "3")
                {

                    lblMessage.Text += "<br />";
                    b = false;
                }

            }
            else //contrário
            {
                if (ddNivelBloqueio.SelectedValue == "3")
                {

                    lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_NIVEL_NAO_PODE_ESTAR_VERMELHO + "<br />";
                    b = false;
                }
            }


            return b;
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                if (validaPageData() == true)
                {

                    //if (ddEstado.SelectedValue == "1") //activa 
                    //{
                    //    //if (txtNIF.Text == "")
                    //    //{
                    //    //    lblMessage.Text = "Registo não actualizado. Por favor indique o NIF da empresa";
                    //    //    return;
                    //    //}
                    //    //if (ddPais.SelectedValue == "351")
                    //    //{
                    //    //    if (!IsValidNIF(txtNIF.Text))
                    //    //    {
                    //    //        lblMessage.Text = "Registo não actualizado. O NIF não é válido."; 
                    //    //        return;
                    //    //    }
                    //    //}
                    //}
                    if (btnSubmit.CommandArgument == "insert")
                    {
                        insertBD();
                    }
                    else if (btnSubmit.CommandArgument == "update")
                    {
                        updateBD();
                    }
                }
            }
        }

        
        private bool validaNumClienteSAP(string numCliente)
        {
            if (myApp == "ISQ_LABMETRO")
            {
                if (numCliente == "") return true;

                double retNum; //proforma, nao vou precisar
                //so posso fazer double.tryparse, nao posso fazer int.try...

                if (Double.TryParse(numCliente, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == false)
                {
                    lblMessage.Text = "Tem de introduzir um valor numérico no número de Cliente.";
                    return false;
                }
                if (iNumClienteSAPExists(numCliente) <= 0)
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NUMCLIENTE_SAP_INEXISTENTE;
                    return false;
                }
                return true;
            }
            else
            {

                return true;

            }

            //martelada mas mais facil que mudar nas outras funcoes. se for espanha tem de se fazer otura coisa.
        }


        private int iNumClienteSAPExists(string numCliente)
        {
            string strSQL = "SELECT COUNT (codigoClienteSAP) FROM sap_empresas WHERE canalDistribuicao = 'RE' AND CAST(codigoClienteSAP as int) = cast('" + numCliente + "' as int)";
            return Convert.ToInt16(GERAL.clsDataAccess.myExecuteScalar(strSQL));

        }

        // Função que insere a Empresa na BD
        private void insertBD()
        {
            if (validaNumClienteSAP(txtNumClienteSAP.Text) == true)
            {
                DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
                try
                {


                    string id = empresa.InsertCompany(ddEmpresaPai.SelectedValue, txtLocalidade.Text, txtLocalidadePostal.Text, ddConcelho.SelectedValue, ddActividade.SelectedValue, txtLatitude.Text, txtLongitude.Text, ddTipoEmpresa.SelectedValue, ddPais.SelectedValue, ddEstado.SelectedValue, txtNomeEmpresa.Text, txtNomeAbrev.Text, txtNIF.Text, txtMorada.Text, txtCodigoPostal.Text, txtTelefone1.Text, txtTelefone2.Text, txtFax.Text, txtEmail.Text, rblSede.SelectedValue, rblPagamentoAtraso.SelectedValue, txtDesconto.Text, txtObservacoes.Text, User.Identity.Name.ToString(), ddNivelBloqueio.SelectedValue, ddCodigoBloqueioSap.SelectedValue, rblRequisicaoAtraso.SelectedValue, rblContrato.SelectedValue, txtNumClienteSAP.Text, ddRegiaoVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue, cbVerCertifsSemReq.Checked.ToString(), cbNPrecisaReqPFacturar.Checked.ToString(), cbGestaoEquipamentos.Checked.ToString(), ddFuncionario.SelectedValue, txtNumObra.Text, txtWebsite.Text, txtModoEntrega.Text, txtmodoPagamento.Text, txtCreditoMax.Text, txtCriteriosAceitacao.Text, txtNomeFicheiro.Text, cbRequerDocEntrada.Checked.ToString(), txtDocumentacaoEntrada.Text);

                    ViewState["idEmpresa"] = id;
                    btnSubmit.CommandArgument = "update";
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
                }

                catch (SqlException ex)
                {
                    lblMessage.Text = ex.Message.ToString(); // GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
                }

                empresa = null;
            }
        }

        private void updateBD()
        {
            if (validaNumClienteSAP(txtNumClienteSAP.Text) == true)
            {
                DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();

                string s = empresa.UpdateCompany(ViewState["idEmpresa"].ToString(), ddEmpresaPai.SelectedValue, txtLocalidade.Text, txtLocalidadePostal.Text, ddConcelho.SelectedValue, ddActividade.SelectedValue, txtLatitude.Text, txtLongitude.Text, ddTipoEmpresa.SelectedValue, ddPais.SelectedValue, ddEstado.SelectedValue, txtNomeEmpresa.Text, txtNomeAbrev.Text, txtNIF.Text, txtMorada.Text, txtCodigoPostal.Text, txtTelefone1.Text, txtTelefone2.Text, txtFax.Text, txtEmail.Text, rblSede.SelectedValue, rblPagamentoAtraso.SelectedValue, txtDesconto.Text, txtObservacoes.Text, User.Identity.Name.ToString(), ddNivelBloqueio.SelectedValue, ddCodigoBloqueioSap.SelectedValue, rblRequisicaoAtraso.SelectedValue, rblContrato.SelectedValue, txtNumClienteSAP.Text, ddRegiaoVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue, cbVerCertifsSemReq.Checked.ToString(), cbNPrecisaReqPFacturar.Checked.ToString(), cbGestaoEquipamentos.Checked.ToString(), ddFuncionario.SelectedValue, txtNumObra.Text, txtWebsite.Text, txtModoEntrega.Text, txtmodoPagamento.Text, txtCreditoMax.Text, txtCriteriosAceitacao.Text, txtNomeFicheiro.Text, cbRequerDocEntrada.Checked.ToString(), txtDocumentacaoEntrada.Text);

                string retValue;
                switch (s)
                {

                    case "0":
                        retValue = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;//correu bem
                        break;
                    case "2":
                        retValue = GERAL.clsGeral.ErrorMessage.ERR_JA_EXISTE_EMPRESA_NIF;
                        break;
                    case "3":
                        retValue = GERAL.clsGeral.ErrorMessage.ERR_NAO_TEM_MESMO_NIF;
                        break;
                    default:
                        retValue = GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
                        break;
                }

                lblMessage.Text = retValue;
                btnSubmit.CommandArgument = "update"; //just in case

                empresa = null;
            }
        }


        //ok, isto funciona lindamente!
        //nao preciso nem devo fazer o setReportConnectionInfo; 
        protected void btnDetalhes_Click(object sender, System.EventArgs e)
        {
            showDetalhesEmpresa();
        }

        private void showDetalhesEmpresa()
        {
            try
            {
                crEmpresa report = new crEmpresa();
                clsReport cr = new clsReport();

                DATA.EmpresasBD emp = new DATA.EmpresasBD();
                DataSet ds = emp.DSEmpresa(ViewState["idEmpresa"].ToString());
                report.SetDataSource(ds);
                emp = null;
                ds = null;
                cr.exportReportToPDF(report,"Empresa");
                //cr = null;
                //report = null;


            }
            catch (Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex);
            }

        }

        protected void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
        {
            fillDDEmpresa();
        }

        protected void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            fillDDEmpresa();
        }


        private void validaNivelBloqueioLabmetro()
        {
            //isso ja dever 
            if (ddCodigoBloqueioSap.SelectedValue == "01")// || ddCodigoBloqueioSap.SelectedValue=="02")
            {
                ddNivelBloqueio.SelectedValue = "3"; //vermelho
                ddNivelBloqueio.Enabled = false;
            }
            else
            {
                //ddNivelBloqueio.SelectedValue = rblPagamentoAtraso.SelectedValue; 
                //é manual, é posto à mão entro o amarelo e o laranja.
                //0:livre
                //1:amarelo
                //2:Laranja
                //3:Vermelho
                ddNivelBloqueio.Enabled = true;


            }
        }

        protected void rblPagamentoAtraso_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            setPermissionsPagamentosAtraso(); //permissoes gerais qe sao mudadas a seguir
            validaNivelBloqueioLabmetro();
        }

        protected void rblRequisicaoAtraso_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            setPermissionsPagamentosAtraso(); //vai usar a mesma função que os pagamentos em atraso
            validaNivelBloqueioLabmetro();
        }

        private void ddCodigoBloqueioSap_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            setPermissionsPagamentosAtraso(); //permissoes gerais qe sao mudadas a seguir
            validaNivelBloqueioLabmetro();
        }

        private bool validaUsersPagamentosAtraso()
        {

            string userNames = System.Configuration.ConfigurationManager.AppSettings["canUpdatePagAtraso"];

            string[] strUserNames = userNames.Split(",".ToCharArray());

            foreach (string s in strUserNames)
            {
                if (s.ToLower() == User.Identity.Name.ToString().ToLower())
                {
                    return true;
                }
            }
            return false;
        }


        private void BindGridHistorico()
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdEmpresa", ViewState["idEmpresa"].ToString());

            DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresaHistoricoEstadosById", arrParams);
            dgHistoricoEstadosEmpresa.DataSource = dt;
            dgHistoricoEstadosEmpresa.DataBind();
        }

        protected void dgHistoricoEstadosEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        private static bool IsNumeric(string inputString)
        {
            return Regex.IsMatch(inputString, "^[0-9]+$");
        }

        //Para empresas também é utilizado o acrónimo NIPC Número de Identificação de Pessoa Colectiva
        //É constituído por 9 dígitos. O primeiro dígito (o mais à esquerda) do NIF tem os seguintes interpretações:
        //1 ou 2: pessoa singular;
        //5: pessoa colectiva;
        //6: pessoa colectiva pública;
        //8: empresário em nome individual (deixou de ser utilizado);
        //9: pessoa colectiva irregular ou número provisório.

        //O nono e último dígito é o dígito de controle. É calculado utilizando o algoritmo módulo 11.

        private static bool IsValidNIF(string nif)
        {
            //Check if is numeric and if has 9 numbers
            if (IsNumeric(nif) && nif.Length == 9)
            {
                //Get the first number of NIF
                char c = nif[0];
                string firstNumber = nif.Substring(0, 1); 
                //Check firt number is (1, 2, 5, 6, 8 or 9)
                if (firstNumber.Equals("1") || firstNumber.Equals("2") || firstNumber.Equals("5") || firstNumber.Equals("6") || firstNumber.Equals("8") || firstNumber.Equals("9"))
                {
                    //Perform CheckDigit calculations
                    int checkDigit = (Convert.ToInt32(firstNumber) * 9);

                    for (int i = 2; i <= 8; i++)
                    {
                        checkDigit += Convert.ToInt32(nif[i - 1].ToString()) * (10 - i);
                    }

                    checkDigit = 11 - (checkDigit % 11);

                    //if checkDigit is higher than ten set it to zero
                    if (checkDigit >= 10) checkDigit = 0;

                    //Compare checkDigit with the last number of NIF
                    //If equal the NIF is Valid.
                    if (checkDigit.ToString() == nif[8].ToString()) return true;
                }

                return false;
            }
            return false;

        }

       
        protected void btnRemove_Click(object sender, System.EventArgs e)
        {

            string strFileName = txtNomeFicheiro.Text.ToString(); ;
            //caminho relativo
            string path = (string)ConfigurationManager.AppSettings["UPLOAD_CRITERIOS_CLIENTES"];
            string myPath = Server.MapPath("~/" + path);
            if (File.Exists(myPath + "/" + strFileName))
            {
                try
                {
                    File.Delete(myPath + "/" + strFileName);
                    txtNomeFicheiro.Text = "";
                }

                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog("Erro no apagar de ficheiros." + ex.ToString());
                    lblMessage.Text = "Erro ao tentar apagar do ficheiro.";
                }

                try
                {
                    DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();

                    //se ele nao consegue fazer isto, nao faz mal.
                    orc.ApagaFicheiroPedidoOrcamento(ViewState["idOrcamento"].ToString(), User.Identity.Name.ToString());
                }
                catch
                {
                    //nada. nao posso controlar se ele quer apagar mm da bd, ou se carrega para apagar da dir.
                }
            }
        }

        protected void btnUpload_Click(object sender, System.EventArgs e)
        {
            if (fileIn.PostedFile.FileName != "")
            {
                string strFileName;

                try
                {
                    strFileName = System.IO.Path.GetFileName(fileIn.PostedFile.FileName);
                    string path = (string)ConfigurationManager.AppSettings["UPLOAD_CRITERIOS_CLIENTES"];
                    string myPath = Server.MapPath("~/" + path);

                    if (File.Exists(myPath + "/" + strFileName))
                    {
                        lblMessage.Text = "Já existe um ficheiro com o mesmo nome.";
                    }
                    else
                    {
                        fileIn.PostedFile.SaveAs(myPath + "/" + strFileName);
                        txtNomeFicheiro.Text = strFileName;

                        //string url = (string)ConfigurationManager.AppSettings["UPLOAD_CRITERIOS_CLIENTES"];
                        ////file:////SomeServer/Some File With Spaces....

                        //url = url + "/" + strFileName;
                        //tdFicheiroCriterios.InnerHtml = "<img src='IMAGES/ic_seta_red.gif' /> <a href='" + downloadpathCriterios(strFileName) + "' target='_blank'>Criterios Cliente </a>";

                          linkFicheiro.NavigateUrl = downloadpathCriterios(strFileName);
                
            

                        
                    }
                }

                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog("Erro no carregamento de ficheiros." + ex.ToString());
                    lblMessage.Text = "Erro no carregamento do ficheiro.";
                }
            }
        }
    }
}
