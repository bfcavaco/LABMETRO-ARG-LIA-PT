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
using LabMetro.GERAL;
using LabMetro.REPORTS;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;


namespace LabMetro
{
    /// <summary>
    /// Summary description for FormOrcam.
    /// isto já está tudo uma grande confusao.... e agora copiado para a taxa de serviço
    /// 
    /// 
    /// </summary>
    /// 


    public partial class FormTaxaServico : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.TextBox Textbox1;
        protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator1;
        protected System.Web.UI.WebControls.DataGrid dgEmpresas;
        protected System.Web.UI.WebControls.DropDownList dd;

        private double dbValorSubTotal = 0;

     
        private const string ID_PAG = "TAXASERVICO_1";//NOME DA PAGINA na tabela pagina



        protected void Page_Load(object sender, System.EventArgs e)
        {
            lblMessage.Text = "";
            lblMessagePTComercial.Text = "";


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
                    if (intAcesso == 0) //so tem permissoes de leitura
                    {
                        btnSubmit.Enabled = false;
                        btnNovaVersao.Enabled = false;
                        btnReplica.Enabled = false;
                        btnFax.Enabled = false;
                        btnEmail.Enabled = false;
                        btnVerFax.Enabled = false;
                        btnVerMail.Enabled = false;
                        btnPTComercial.Enabled = false;
                    }

                    if (!Page.IsPostBack)
                    {
                        initEmptyForm();
                    }

                    //se entrou pela querystring e tem id, guardar esse id em viewstate
                    if (Request.QueryString["id"] != null)
                    {
                        if (Request.QueryString["id"] != "")
                        {
                            ViewState["idTaxaServico"] = Request.QueryString["id"].ToString();
                        }
                    }


                    //validar se o id existe (tb pode ter sido preenchido apos o insert)
                    if (ViewState["idTaxaServico"] != null)
                    {
                        if (ViewState["idTaxaServico"].ToString() != "")
                        {
                            if (!Page.IsPostBack)
                            {
                                fillForm(ViewState["idTaxaServico"].ToString());
                                btnEmpresas.Enabled = false; //no update n se pode mudar a empresa
                                fillCompanyInfo(ddEmpresa.SelectedValue);
                            }
                            btnSubmit.CommandArgument = "update";
                            enableButtons();
                        }
                    }
                    else //viewstate nao está preenchido
                    {
                        btnSubmit.CommandArgument = "insert";
                        disableButtons();

                    }
                }
            }


            //para o uplaod
            if (txtNomeFicheiro.Text != "") btnRemove.Enabled = true;
            else btnRemove.Enabled = false;
          
        }

        private void disableButtons()
        {
            btnHistorico.Enabled = false;
            btnNovaVersao.Enabled = false;
            btnReplica.Enabled = false;
            btnFax.Enabled = false;
            btnEmail.Enabled = false;
            btnVerFax.Enabled = false;
            btnVerMail.Enabled = false;
            btnPTComercial.Enabled = false;
        }

        private void enableButtons()
        {
            btnHistorico.Enabled = true;
            btnNovaVersao.Enabled = true;
            btnReplica.Enabled = true;
            btnFax.Enabled = true;
            btnEmail.Enabled = true;
            btnVerFax.Enabled = true;
            btnVerMail.Enabled = true;
            btnPTComercial.Enabled = true;
        }

        private void initEmptyForm()
        {
            FillDropDowns();
            try
            {
                ddFuncionario.SelectedValue = System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));
            }
            catch
            {
                //aqui seleccionar o luis godinho para aparecer como selected na dropdown do responsavel técnico.
                ddFuncionario.SelectedValue = (string)ConfigurationManager.AppSettings["ID_RESP_TEC_DEFAULT_ORCAM"];
            }


            //inicializar os valores para não dar erro...
            if (ViewState["idTaxaServico"] == null)
            {
                resetTotais();
                LabMetro.DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();
                BindGridLinhasTaxaServico("0");
                BindGridComentariosTaxaServico("0");

                // Preencher as datas 
                txtDtPedido.Text = System.DateTime.Today.ToShortDateString();
                txtDtTaxaServico.Text = System.DateTime.Today.ToShortDateString();
                
                
                //mess de validade desaparecem pq é sempre ate o final do ano


                TaxaServico = null;
            }
        }

       

        private void BindGridHistorico()
        {
            DATA.TaxaServicoBD TaxaServicos = new LabMetro.DATA.TaxaServicoBD();
            SqlDataReader DR = TaxaServicos.DRGetTaxaServicoHistoricoEstados(ViewState["idTaxaServico"].ToString());
            dgHistorico.DataSource = DR;
            dgHistorico.DataBind();
            DR.Close();
            TaxaServicos = null;
        }

        private void BindGridComentariosTaxaServico(string idTaxaServico)
        {
            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            DataTable dt = orc.DTComentariosTaxaServicoByIdTaxaServico(idTaxaServico);
            DataView dv = new DataView(dt);
            dgComentariosTaxaServico.DataSource = dv;
            dgComentariosTaxaServico.DataBind();
            orc = null;
        }

        private void BindGridLinhasTaxaServico(string idTaxaServico)
        {
            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            DataTable dt = orc.DTLinhasTaxaServicoByIdTaxaServico(idTaxaServico);
            DataView dv = new DataView(dt);
            dgLinhasTaxaServico.DataSource = dv;
            dgLinhasTaxaServico.DataBind();
            orc = null;
        }


        private void fillForm(string id)
        {
            LabMetro.DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();
            DataTable TaxaServicoDT = TaxaServico.DTTaxaServicoDetails(id);

            if (TaxaServicoDT.Rows.Count > 0)
            {
                lblRefTaxaServico.Text = TaxaServicoDT.Rows[0]["refTaxaServico"].ToString();
                ViewState["refTaxaServico"] = TaxaServicoDT.Rows[0]["refTaxaServico"].ToString();

                lblVersao.Text = TaxaServicoDT.Rows[0]["versao"].ToString();
                string idEmpresa = TaxaServicoDT.Rows[0]["idEmpresa"].ToString();
                string nomeEmpresa = TaxaServicoDT.Rows[0]["nomeEmpresa"].ToString();

                ddEmpresa.Items.Insert(0, new ListItem(nomeEmpresa, idEmpresa));
                ddEmpresa.SelectedValue = idEmpresa;

                string idEstadoTaxaServico = TaxaServicoDT.Rows[0]["idEstadoTaxaServico"].ToString();
                string estadoTaxaServico = TaxaServicoDT.Rows[0]["estadoTaxaServico"].ToString();

                try
                {
                    ddEstado.SelectedValue = idEstadoTaxaServico;
                }
                catch
                {
                    ddEstado.Items.Insert(0, new ListItem(estadoTaxaServico, idEstadoTaxaServico));
                    ddEstado.SelectedValue = idEstadoTaxaServico;
                }

                FillEmpresaInfoContactos();
             
                string idContacto = TaxaServicoDT.Rows[0]["idContacto"].ToString();

                string nomeContacto = TaxaServicoDT.Rows[0]["nomeContacto"].ToString();
                try
                {
                    ddContacto.SelectedValue = idContacto;
                }
                catch
                {
                    ddContacto.Items.Insert(0, new ListItem(nomeContacto, idContacto));
                    ddContacto.SelectedValue = idContacto;
                }

                string idFuncionarioRespTecnico = TaxaServicoDT.Rows[0]["idFuncionarioRespTecnico"].ToString();
                string nomeFuncionarioRespTecnico = TaxaServicoDT.Rows[0]["nomeFuncionario"].ToString();
                try
                {
                    ddFuncionario.SelectedValue = idFuncionarioRespTecnico;
                }
                catch
                {
                    ddFuncionario.Items.Insert(0, new ListItem(nomeFuncionarioRespTecnico, idFuncionarioRespTecnico));
                    ddFuncionario.SelectedValue = idFuncionarioRespTecnico;
                }

                if (nomeFuncionarioRespTecnico == "")
                {
                    ddFuncionario.SelectedValue = (string)ConfigurationManager.AppSettings["ID_RESP_TEC_DEFAULT_ORCAM"];
                }

                string idObsLocalExecucao = TaxaServicoDT.Rows[0]["idObsLocalExecucao"].ToString();
                string obsLocalExecucao = TaxaServicoDT.Rows[0]["obsLocalExecucao"].ToString();
                try
                {
                    ddLocalExecucao.SelectedValue = idObsLocalExecucao;
                }
                catch
                {
                    ddLocalExecucao.Items.Insert(0, new ListItem(obsLocalExecucao, idObsLocalExecucao));
                    ddLocalExecucao.SelectedValue = idObsLocalExecucao;
                }

                cbCalExterna.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(TaxaServicoDT.Rows[0]["calibracaoExterna"].ToString()).ToString());
                txtValorAjudasCustoDeslocacoes.Text = GERAL.clsGeral.ConvertDBMoneyToString(TaxaServicoDT.Rows[0]["valorAjudasCustoDeslocacoes"].ToString());

                txtDtPedido.Text = clsGeral.ToShortDate(TaxaServicoDT.Rows[0]["dtPedido"].ToString());
                txtCCMail.Text = TaxaServicoDT.Rows[0]["ccMail"].ToString();

                //data de validade desaparece pq é sempre até ao final do corrente ano.


                //if (idEstadoTaxaServico.ToString() == "1") //pedido
                //{

                //    txtDtTaxaServico.Text = System.DateTime.Today.ToShortDateString();
                //    int mesesValidade;
                //    try
                //    {
                //        mesesValidade = System.Convert.ToInt32(TaxaServicoDT.Rows[0]["validadeMeses"].ToString());
                //    }
                //    catch
                //    {
                //        Response.Write(TaxaServicoDT.Rows[0]["validadeMeses"].ToString());
                //        mesesValidade = 1;
                //    }

                //    System.DateTime today_ = System.DateTime.Today;
                //    System.DateTime dtValidadeTaxaServico = today_.AddMonths(mesesValidade);
                //    txtDtValidade.Text = dtValidadeTaxaServico.ToShortDateString();
                //}
                //else
                //{
                //    txtDtTaxaServico.Text = clsGeral.ToShortDate(TaxaServicoDT.Rows[0]["dtTaxaServico"].ToString());
                //    txtDtValidade.Text = clsGeral.ToShortDate(TaxaServicoDT.Rows[0]["dtValidade"].ToString());
                //    ViewState["idLinhaPtComercial"] = TaxaServicoDT.Rows[0]["idLinhaPtComercial"].ToString();

                //}

                //txtMesesValidade.Text = TaxaServicoDT.Rows[0]["validadeMeses"].ToString();

                txtRefCliente.Text = TaxaServicoDT.Rows[0]["referenciaCliente"].ToString();
                txtTempoExecucao.Text = TaxaServicoDT.Rows[0]["tempoExecucao"].ToString();
                txtLocalidadeCalib.Text = TaxaServicoDT.Rows[0]["localidadeCalibracao"].ToString();
                txtObservacoes.Text = TaxaServicoDT.Rows[0]["observacoes"].ToString();
                chbComTotal.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(TaxaServicoDT.Rows[0]["bTotal"].ToString()).ToString());
                chbDesconto.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(TaxaServicoDT.Rows[0]["bDesconto"].ToString()).ToString());
                chbDeslocacoes.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(TaxaServicoDT.Rows[0]["bDeslocacoes"].ToString()).ToString());

                txtNomeFicheiro.Text = TaxaServicoDT.Rows[0]["nomeFicheiro"].ToString();
                txtIdPtComercial.Text = TaxaServicoDT.Rows[0]["idPTComercial"].ToString();
                txtDtPTComercial.Text = (TaxaServicoDT.Rows[0]["dtPTComercial"].ToString());
                txtRefPSI.Text = (TaxaServicoDT.Rows[0]["refPSI"].ToString());
             
                ddIva.SelectedValue = TaxaServicoDT.Rows[0]["ValorIva"].ToString();
                
                //para abrir o ficheiro
                if (TaxaServicoDT.Rows[0]["nomeFicheiro"].ToString() != null && TaxaServicoDT.Rows[0]["nomeFicheiro"].ToString().ToString() != "")
                {
                    string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_URL"];

                    myPath = myPath + "/" + TaxaServicoDT.Rows[0]["nomeFicheiro"].ToString();
                    linkFicheiro.NavigateUrl = myPath;
                }
                else
                {
                    linkFicheiro.NavigateUrl = "#";
                }

                // Preencher a informação referente à Empresa
                fillCompanyInfo(idEmpresa);

                // Preencher a informação referente ao Contacto
                fillContactInfo(idContacto);


                

                //tem de ficar necessariamente depois do fillcompanyInfo
                try
                {
                    ddCondicoesPagamento.SelectedValue = TaxaServicoDT.Rows[0]["idCondicoesPagamento"].ToString();
                }
                catch
                { }
                // Preencher as Linhas do Orçamento

                BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());

                // Preencher os Comentários (preencho o DataView com o que vem da BD)

                BindGridComentariosTaxaServico(ViewState["idTaxaServico"].ToString());

                // Desabilitar os botões se o Orçamento se encontrar no estado "Anulado"
                // para que o utilizador não possa efectuar alterações
                if ("Anulado".Equals(ddEstado.SelectedItem.Text))
                {
                    btnSubmit.Enabled = false;
                    //btnReset.Enabled = false;
                    btnNovaVersao.Enabled = false;
                    btnReplica.Enabled = false;
                    btnFax.Enabled = false;
                    btnEmail.Enabled = false;
                    btnPTComercial.Enabled = false;
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


            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            btnVerFax.Click += new System.EventHandler(btnVerFax_Click);
            btnVerMail.Click += new System.EventHandler(btnVerMail_Click);
            btnRemove.Click += new System.EventHandler(btnRemove_Click);
            btnUpload.Click += new System.EventHandler(btnUpload_Click);

            btnPesquisaEquip.Click += new System.EventHandler(btnPesquisaEquip_Click);

            dgLinhasTaxaServico.EditCommand += new DataGridCommandEventHandler(dgLinhasTaxaServico_EditCommand);
            dgLinhasTaxaServico.ItemCommand += new DataGridCommandEventHandler(dgLinhasTaxaServico_ItemCommand);
            dgLinhasTaxaServico.CancelCommand += new DataGridCommandEventHandler(dgLinhasTaxaServico_CancelGrid);
            dgLinhasTaxaServico.UpdateCommand += new DataGridCommandEventHandler(dgLinhasTaxaServico_UpdateGrid);
            dgLinhasTaxaServico.DeleteCommand += new DataGridCommandEventHandler(dgLinhasTaxaServico_DeleteCommand);
            dgLinhasTaxaServico.ItemDataBound += new DataGridItemEventHandler(dgLinhasTaxaServico_ItemDataBound);

            dgComentariosTaxaServico.EditCommand += new DataGridCommandEventHandler(dgComentariosTaxaServico_EditCommand);
            dgComentariosTaxaServico.ItemCommand += new DataGridCommandEventHandler(dgComentariosTaxaServico_ItemCommand);
            dgComentariosTaxaServico.CancelCommand += new DataGridCommandEventHandler(dgComentariosTaxaServico_CancelGrid);
            dgComentariosTaxaServico.UpdateCommand += new DataGridCommandEventHandler(dgComentariosTaxaServico_UpdateGrid);
            dgComentariosTaxaServico.DeleteCommand += new DataGridCommandEventHandler(dgComentariosTaxaServico_DeleteCommand);
            dgComentariosTaxaServico.ItemDataBound += new DataGridItemEventHandler(dgComentariosTaxaServico_ItemDataBound);

            ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            ddContacto.SelectedIndexChanged += new System.EventHandler(ddContacto_SelectedIndexChanged);
            //ddIva.SelectedIndexChanged += new System.EventHandler(ddIva_SelectedIndexChanged);
            btnHistorico.Click += new System.EventHandler(btnHistorico_Click);

            btnPesquisaEquip.Click += new System.EventHandler(btnPesquisaEquip_Click);

            txtValorAjudasCustoDeslocacoes.TextChanged += new System.EventHandler(txtValorAjudasCustoDeslocacoes_TextChanged);

            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            //btnReset.Click += new System.EventHandler(btnReset_Click);

            btnNovaVersao.Click += new System.EventHandler(btnNovaVersao_Click);
            btnReplica.Click += new System.EventHandler(btnReplica_Click);

            btnFax.Click += new System.EventHandler(btnFax_Click);
            txtMailAlternativo.TextChanged += new System.EventHandler(txtMailAlternativo_TextChanged);
            btnEmail.Click += new System.EventHandler(btnEmail_Click);
            txtPesqEmpresa.TextChanged += new System.EventHandler(txtPesqEmpresa_TextChanged);
            txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
            btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
            btnResetPesquisaEquips.Click += new System.EventHandler(btnResetPesquisaEquips_Click);
            txtValorAjudasCustoDeslocacoes.TextChanged += new System.EventHandler(txtValorAjudasCustoDeslocacoes_TextChanged);


        }
        #endregion
        // *****************************************************************************
        // CÁLCULOS
        // *****************************************************************************
        private void calculaValorTotal()
        {
            txtValorTotal.Text = (Double.Parse(txtValorSubTotal.Text) + Double.Parse(txtValorAjudasCustoDeslocacoes.Text)).ToString();
        }

        private void resetTotais()
        {
            txtValorAjudasCustoDeslocacoes.Text = "0";
            txtValorSubTotal.Text = "0";
            txtValorTotal.Text = "0";
        }

        // *****************************************************************************
        // dgLinhasTaxaServico - DATAGRID COMMANDS
        // *****************************************************************************
        private void dgLinhasTaxaServico_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgLinhasTaxaServico.EditItemIndex = e.Item.ItemIndex;
            BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());
            dgLinhasTaxaServico.ShowFooter = false;
        }

        private void dgLinhasTaxaServico_CancelGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgLinhasTaxaServico.EditItemIndex = -1;
            BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());
            dgLinhasTaxaServico.ShowFooter = true;
        }

        private void dgLinhasTaxaServico_UpdateGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idTaxaServico = ViewState["idTaxaServico"].ToString();

            if (idTaxaServico == "")
            {
                lblMessage.Text = "Tem de submeter primeiro para criar a referência, dps adicionar as linhas.";
            }
            else
            {
                // Obter os novos valores
                DropDownList ddTipoServico = (DropDownList)e.Item.FindControl("ddTipoServicoEdit");
                DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoEdit");
                DropDownList ddEstadoTaxaServicoLinha = (DropDownList)e.Item.FindControl("ddEstadoTaxaServicoLinhaEdit");
                DropDownList ddRazaoTaxaServicoLinha = (DropDownList)e.Item.FindControl("ddRazaoTaxaServicoLinhaEdit");

                TextBox txtQuantidade = (TextBox)e.Item.FindControl("txtQuantidadeEdit");
                TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoEdit");
                TextBox txtAsteriscoLinha = (TextBox)e.Item.FindControl("txtAsteriscoLinhaEdit");
                TextBox txtValorUnitario = (TextBox)e.Item.FindControl("txtValorUnitarioEdit");
                TextBox txtValorLinha = (TextBox)e.Item.FindControl("txtValorLinhaEdit");
                TextBox txtPercDescontoLinha = (TextBox)e.Item.FindControl("txtPercDescontoEdit");


                // Preencher valores default caso os campos estejam vazios
                if (txtQuantidade.Text == "") txtQuantidade.Text = "1";
                if (txtValorUnitario.Text == "") txtValorUnitario.Text = "0";
                if (txtValorLinha.Text == "") txtValorLinha.Text = "0";
                if (txtPercDescontoLinha.Text == "") txtPercDescontoLinha.Text = "0";

                // Verificar se os campos obrigatórios foram preenchidos
                if (txtDescricao.Text == "")
                    lblMessage.Text = "Tem de preencher o campo 'Descrição'.";
                else
                {
                    // Actualizar directamente a Base de dados. 
                    string idTaxaServicoLinha = dgLinhasTaxaServico.DataKeys[e.Item.ItemIndex].ToString();
                    string idTipoServico = ddTipoServico.SelectedValue.ToString();
                    string idTipoEquipamento = ddTipoEquipamento.SelectedValue.ToString();
                    string tipoEquipamento = ddTipoEquipamento.SelectedItem.Text;
                    string quantidade = txtQuantidade.Text;
                    string descricaoEquipamento = txtDescricao.Text;
                    string asterisco = txtAsteriscoLinha.Text;
                   
                    string percDescontoLinha = txtPercDescontoLinha.Text;
                    string valorUnitario = txtValorUnitario.Text;
                    double valorUnitarioComDesconto = (Double.Parse(valorUnitario) * (100 - Double.Parse(percDescontoLinha)) * 0.01);
                    double valorLinha = System.Math.Round(Double.Parse(quantidade) * valorUnitarioComDesconto, 2);

                    DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
                    //retorna o num de linhas afectadas mas eu agora nao utilizo
                    orc.UpdateTaxaServicoLinha(idTaxaServicoLinha, idTaxaServico, idTipoServico, idTipoEquipamento, quantidade, descricaoEquipamento, asterisco, valorUnitario, valorLinha.ToString(), User.Identity.Name.ToString(), percDescontoLinha, ddEstadoTaxaServicoLinha.SelectedValue.ToString(), ddRazaoTaxaServicoLinha.SelectedValue.ToString());
                    orc = null;

                }

                dgLinhasTaxaServico.EditItemIndex = -1;
                BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());
                dgLinhasTaxaServico.ShowFooter = true;
            }


        }

        private void dgLinhasTaxaServico_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            if ((ViewState["idTaxaServico"] == null) || (ViewState["idTaxaServico"].ToString() == ""))
            {
                lblMessage.Text = "Tem de submeter o orçamento antes de adicionar linhas.";
                return;
            }

            if (e.CommandName == "Insert")
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    DropDownList ddTipoServico = (DropDownList)e.Item.FindControl("ddTipoServico");
                    DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamento");
                    TextBox txtQuantidade = (TextBox)e.Item.FindControl("txtQuantidade");
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
                    TextBox txtAsteriscoLinha = (TextBox)e.Item.FindControl("txtAsteriscoLinha");
                    
                    TextBox txtValorUnitario = (TextBox)e.Item.FindControl("txtValorUnitario");
                    TextBox txtValorLinha = (TextBox)e.Item.FindControl("txtValorLinha");
                    TextBox txtPercDescontoLinha = (TextBox)e.Item.FindControl("txtPercDesconto");


                    // Preencher valores default caso os campos estejam vazios
                    if (txtQuantidade.Text == "") txtQuantidade.Text = "1";
                    if (txtValorUnitario.Text == "") txtValorUnitario.Text = "0";
                    if (txtValorLinha.Text == "") txtValorLinha.Text = "0";
                    if (txtPercDescontoLinha.Text == "")
                    {
                        txtPercDescontoLinha.Text = "0";

                    }
                    else
                    {

                        chbDesconto.Checked = true;
                    }

                    // Verificar se os campos obrigatórios foram preenchidos
                    if (txtDescricao.Text == "")
                        lblMessage.Text = "Tem de preencher o campo 'Descrição' antes de adicionar.";
                    else
                    {


                        // INSERE directamente a Base de dados. 

                        string idTaxaServico = ViewState["idTaxaServico"].ToString();
                        string idTipoServico = ddTipoServico.SelectedValue.ToString();
                        //string tipoServico = ddTipoServico.SelectedItem.Text;
                        string idTipoEquipamento = ddTipoEquipamento.SelectedValue.ToString();
                        string tipoEquipamento = ddTipoEquipamento.SelectedItem.Text;
                        string quantidade = txtQuantidade.Text;
                        string descricaoEquipamento = txtDescricao.Text;
                        string asterisco = txtAsteriscoLinha.Text;
                    
                        string percDescontoLinha = txtPercDescontoLinha.Text;
                        string valorUnitario = txtValorUnitario.Text;
                        double valorUnitarioComDesconto = System.Math.Round((Double.Parse(valorUnitario)) * (100 - Double.Parse(percDescontoLinha)) * 0.01, 2);
                        double valorLinha = Double.Parse(quantidade) * valorUnitarioComDesconto;
                        valorLinha = System.Math.Round(valorLinha, 2);

                        DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();

                        orc.InsertTaxaServicoLinha(idTaxaServico, idTipoServico, idTipoEquipamento, quantidade, descricaoEquipamento, asterisco,  valorUnitario, valorLinha.ToString(), User.Identity.Name.ToString(), percDescontoLinha);

                        orc = null;

                    }
                }
                dgLinhasTaxaServico.EditItemIndex = -1;
                BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());
            }
        }

        private void dgLinhasTaxaServico_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item.ItemType == ListItemType.Footer) // INSERIR
            {
                DropDownList ddTipoServico = fillTiposServico((DropDownList)e.Item.FindControl("ddTipoServico"));
                DropDownList ddTipoEquipamento = fillTiposEquipamento((DropDownList)e.Item.FindControl("ddTipoEquipamento"));
                //aqui nao vou Pôr nem o Estado Nem a Razao, vou mandar valores default.
            }
            else if (e.Item.ItemType == ListItemType.EditItem) // EDITAR
            {
                DropDownList ddTipoServico = fillTiposServico((DropDownList)e.Item.FindControl("ddTipoServicoEdit"));
                DropDownList ddTipoEquipamento = fillTiposEquipamento((DropDownList)e.Item.FindControl("ddTipoEquipamentoEdit"));
                DropDownList ddEstadoTaxaServicoLinha = fillEstadoTaxaServicoLinha((DropDownList)e.Item.FindControl("ddEstadoTaxaServicoLinhaEdit"));
                DropDownList ddRazaoTaxaServicoLinha = fillRazaoTaxaServicoLinha((DropDownList)e.Item.FindControl("ddRazaoTaxaServicoLinhaEdit"));

                //Para as combos ficarem seleccionadas no item correcto
                string idTipoServico = drv["idTipoServico"].ToString();
                if (idTipoServico != "") ddTipoServico.SelectedValue = idTipoServico;

                string idTipoEquipamento = drv["idTipoEquipamento"].ToString();
                if (idTipoEquipamento != "") ddTipoEquipamento.SelectedValue = idTipoEquipamento;

                string idEstadoTaxaServicoLinha = drv["idEstadoTaxaServicoLinha"].ToString();
                ddEstadoTaxaServicoLinha.SelectedValue = idEstadoTaxaServicoLinha;

                string idRazaoTaxaServicoLinha = drv["idRazaoTaxaServicoLinha"].ToString();
                ddRazaoTaxaServicoLinha.SelectedValue = idRazaoTaxaServicoLinha;
            }

            // ****************************************
            // CÁLCULOS
            // ****************************************
            string strValorLinha;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                strValorLinha = drv["valorLinha"].ToString();

                if (strValorLinha != "")
                {
                    try
                    {
                        dbValorSubTotal += Double.Parse(strValorLinha);
                    }
                    catch
                    {
                        //A value was null
                    }
                }
                else //nao vale a pena pq quando nao ha items, ele nao entra aqui
                {
                    dbValorSubTotal = 0;
                }

                // actualizar o Sub-Total
                txtValorSubTotal.Text = dbValorSubTotal.ToString();

                // actualizar o total
                calculaValorTotal();
            }
        }

        private void dgLinhasTaxaServico_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idTaxaServicoLinha = dgLinhasTaxaServico.DataKeys[e.Item.ItemIndex].ToString();

            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();

            orc.DeleteTaxaServicoLinha(idTaxaServicoLinha, User.Identity.Name.ToString());

            orc = null;

            BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());
            dgLinhasTaxaServico.ShowFooter = true;

            calculaValorTotal();

            if (dgLinhasTaxaServico.DataKeys.Count == 0) //nao ha items - (nao encontrei outra maneira...)
            {
                resetTotais();
            }
        }

        // *****************************************************************************
        // dgComentariosTaxaServico - DATAGRID COMMANDS
        // *****************************************************************************
        private void dgComentariosTaxaServico_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgComentariosTaxaServico.EditItemIndex = e.Item.ItemIndex;
            BindGridComentariosTaxaServico(ViewState["idTaxaServico"].ToString());
            dgComentariosTaxaServico.ShowFooter = false;
        }

        private void dgComentariosTaxaServico_CancelGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgComentariosTaxaServico.EditItemIndex = -1;
            BindGridComentariosTaxaServico(ViewState["idTaxaServico"].ToString());
            dgComentariosTaxaServico.ShowFooter = true;
        }

        private void dgComentariosTaxaServico_UpdateGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            DropDownList ddComentarioTipo = (DropDownList)e.Item.FindControl("ddComentarioTipoEdit");
            TextBox txtComentario = (TextBox)e.Item.FindControl("txtComentarioEdit");
            TextBox txtAsterisco = (TextBox)e.Item.FindControl("txtAsteriscoEdit");

            string idTaxaServico = ViewState["idTaxaServico"].ToString();
            string idTaxaServicoComentario = dgComentariosTaxaServico.DataKeys[e.Item.ItemIndex].ToString();

          
            string descricao = txtComentario.Text;
            string asterisco = txtAsterisco.Text;

            //alterar directamente na bd
            DATA.TaxaServicoBD orc = new DATA.TaxaServicoBD();
            orc.UpdateTaxaServicoComentario(idTaxaServicoComentario, idTaxaServico, descricao, asterisco, User.Identity.Name.ToString());
            orc = null;

            dgComentariosTaxaServico.EditItemIndex = -1;
            BindGridComentariosTaxaServico(ViewState["idTaxaServico"].ToString());
            dgComentariosTaxaServico.ShowFooter = true;
        }


        private void dgComentariosTaxaServico_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if ((ViewState["idTaxaServico"] == null) || (ViewState["idTaxaServico"].ToString() == ""))
            {
                lblMessage.Text = "Tem de submeter o orçamento antes de adicionar comentários.";
                return;
            }

            if (e.CommandName == "Insert")
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    DropDownList ddComentarioTipo = (DropDownList)e.Item.FindControl("ddComentarioTipo");
                    TextBox txtComentario = (TextBox)e.Item.FindControl("txtComentario");
                    TextBox txtAsterisco = (TextBox)e.Item.FindControl("txtAsterisco");
                    if (txtComentario.Text == "")
                    {
                        lblMessage.Text = "Tem de preencher o campo 'Comentário' antes de adicionar.";
                    }
                    else
                    {
                        string idTaxaServico = ViewState["idTaxaServico"].ToString();
                     
                        string TaxaServicoComentarioTipo = ddComentarioTipo.SelectedItem.Text;
                        string descricao = txtComentario.Text;
                        string asterisco = txtAsterisco.Text;

                        DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
                        orc.InsertTaxaServicoComentario(idTaxaServico, descricao, asterisco, User.Identity.Name.ToString());
                        orc = null;
                    }
                }
                dgComentariosTaxaServico.EditItemIndex = -1;
                BindGridComentariosTaxaServico(ViewState["idTaxaServico"].ToString());
            }
        }

        private void dgComentariosTaxaServico_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idTaxaServicoComentario = dgComentariosTaxaServico.DataKeys[e.Item.ItemIndex].ToString();
            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            orc.DeleteTaxaServicoComentario(idTaxaServicoComentario, User.Identity.Name.ToString());
            orc = null;

            BindGridComentariosTaxaServico(ViewState["idTaxaServico"].ToString());
            dgComentariosTaxaServico.ShowFooter = true;
        }

        private void dgComentariosTaxaServico_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer) // INSERIR
            {
                DropDownList ddComentarioTipo = fillComentariosTipo((DropDownList)e.Item.FindControl("ddComentarioTipo"));
            }
            else if (e.Item.ItemType == ListItemType.EditItem) // EDITAR
            {
                DropDownList ddComentarioTipo = fillComentariosTipo((DropDownList)e.Item.FindControl("ddComentarioTipoEdit"));

                //Para as combos ficarem seleccionadas no item correcto
                DataRowView drv = (DataRowView)e.Item.DataItem;

                string idComentarioTipo = drv["ident"].ToString();
                if (idComentarioTipo != "") ddComentarioTipo.SelectedValue = idComentarioTipo;
            }
        }

        // *****************************************************************************
        // Histórico de Estados - SHOW/HIDE
        // *****************************************************************************
        private void btnHistorico_Click(object sender, System.EventArgs e)
        {
            if (dgHistorico.Items.Count != 0)
            {
                dgHistorico.DataSource = null;
                dgHistorico.DataBind();
                dgHistorico.Controls.Clear();
                dgHistorico.Visible = false;
                btnHistorico.Text = "Ver Histórico de Estados";
            }
            else
            {
                BindGridHistorico();
                if (dgHistorico.Items.Count == 0)
                    dgHistorico.Visible = false;
                else
                {
                    dgHistorico.Visible = true;
                    btnHistorico.Text = "Ocultar Histórico de Estados";
                }
            }
        }

        // *****************************************************************************
        // Gravar as alterações do Orçamento - SUBMIT
        // *****************************************************************************

        private int InsertBD()
        {

            DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();
            
            //retirei os campos dtvalidade, mesesvalidades e condicoes de pagamento

            //datavalidade fica para o caso de ser necessário para alguma coisa na BD
            string dataValidade = "31-12-"+System.DateTime.Today.Year.ToString();


            //mess de validade desaparecem 

            txtDtTaxaServico.Text = System.DateTime.Today.ToShortDateString();


            //mess de validade desaparecem pq é sempre ate o final do ano

            int idTaxaServico = TaxaServico.InsertTaxaServico(ddEmpresa.SelectedValue, ddContacto.SelectedValue, ddFuncionario.SelectedValue, ddLocalExecucao.SelectedValue, ddEstado.SelectedValue, txtRefCliente.Text, txtDtPedido.Text, txtTempoExecucao.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotal.Text, txtDtTaxaServico.Text, dataValidade, cbCalExterna.Checked.ToString(), txtLocalidadeCalib.Text, txtObservacoes.Text, User.Identity.Name.ToString(), chbComTotal.Checked.ToString(),  chbDesconto.Checked.ToString(), chbDeslocacoes.Checked.ToString(), ddCondicoesPagamento.SelectedValue, txtNomeFicheiro.Text, txtIdPtComercial.Text, txtRefPSI.Text, txtCCMail.Text, ddIva.SelectedValue.ToString());

            TaxaServico = null;

            if (txtRefPSI.Text != "")
            {
                insereBDPSI();
            }
            return idTaxaServico;
        }

        private void UpdateBD()
        {

            //datavalidade fica para o caso de ser necessário para alguma coisa na BD
            string dataValidade = "31-12-" + System.DateTime.Today.Year.ToString();


            DATA.TaxaServicoBD orc = new DATA.TaxaServicoBD();
            int x = orc.UpdateTaxaServico(ViewState["idTaxaServico"].ToString(), ddEmpresa.SelectedValue, ddContacto.SelectedValue, ddFuncionario.SelectedValue, ddLocalExecucao.SelectedValue, ddEstado.SelectedValue, txtRefCliente.Text, txtDtPedido.Text, txtTempoExecucao.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotal.Text, txtDtTaxaServico.Text, dataValidade, cbCalExterna.Checked.ToString(), txtLocalidadeCalib.Text, txtObservacoes.Text, User.Identity.Name.ToString(), chbComTotal.Checked.ToString(), chbDesconto.Checked.ToString(), chbDeslocacoes.Checked.ToString(), ddCondicoesPagamento.SelectedValue, txtNomeFicheiro.Text, txtIdPtComercial.Text, txtRefPSI.Text,txtCCMail.Text, ddIva.SelectedValue);

            orc = null;
          

            BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString()); //bind grid por causa da actualizacao das linhas com o estadoTaxaServicoLinha no caso de aceite ou reprovado;

            if (txtRefPSI.Text != "")
            {
                insereBDPSI();
            }


        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                calculaValorTotal(); // só para garantir que o total está bem actualizado

                DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();

                if (btnSubmit.CommandArgument == "insert")
                {
                    try
                    {
                        string idTaxaServico = InsertBD().ToString();
                        ViewState["idTaxaServico"] = idTaxaServico;
                        btnSubmit.CommandArgument = "update";
                        enableButtons();

                        //ir buscar a ref para mostrar e guardar em viewstate para 
                        //criar o nome do ficheiro no envio do fax
                        string refTaxaServico = TaxaServico.refTaxaServico(idTaxaServico);
                        lblRefTaxaServico.Text = refTaxaServico;
                        ViewState["refTaxaServico"] = refTaxaServico;
                    }
                    catch (Exception ex)
                    {
                        GERAL.clsWriteError.WriteLog(ex.ToString());
                        lblMessage.Text = "Erro na inserção";
                    }
                }
                else if (btnSubmit.CommandArgument == "update")
                {
                    UpdateBD();
                    btnSubmit.CommandArgument = "update";
                }

                TaxaServico = null;
            }
        }

        // as taxas de serviço so integram com o PSI quando teem a refPSI preenchida
        private void insereBDPSI()
        {

            try
            {
                //   User: ISQ\psiexuser
                //·         PW: 67?%bkTw
                WebServicePSI.PSIIntegrationProposal proposal = new WebServicePSI.PSIIntegrationProposal();

                // testar se o viewstate vem sempre preenchido.

                string strSQL = "select * from vOrcamentosLabmetro_PSI where proposalTitle = '" + ViewState["refTaxaServico"].ToString() + "'";

                using (Impersonation imp = new Impersonation("psiexuser", "67?%bkTw", "ISQ"))
                {
                    if (imp.impersonateValidUser())
                    {
                        using (WebServicePSI.ProposalServiceClient pc = new WebServicePSI.ProposalServiceClient())
                        {
                            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    proposal.ProposalNumber = dr["proposalNumber"].ToString();
                                    proposal.ProposalOrigin = System.Convert.ToInt16(dr["ProposalOrigin"].ToString());//INT
                                    proposal.ProposalTitle = dr["proposalTitle"].ToString();
                                    proposal.ProposalManagement = dr["ProposalManagement"].ToString();
                                    proposal.ProposalClient = dr["ProposalClient"].ToString();
                                    proposal.ProposalClientName = dr["ProposalClientName"].ToString();
                                    proposal.ProposalClientNIF = dr["ProposalClientFiscalCode"].ToString();
                                    proposal.ProposalStatus = Guid.Parse(dr["ProposalStatus"].ToString());//GUID
                                    proposal.ProposalValue = Decimal.Parse(dr["ProposalValue"].ToString()); //decimal
                                    proposal.ProposalDate = DateTime.Parse(dr["ProposalDate"].ToString()); //date
                                    proposal.ProposalManager = dr["proposalManager"].ToString();
                                    proposal.ProposalDepartment = dr["proposalDepartment"].ToString();
                                    proposal.ProposalCostCenter = dr["ProposalCostCenter"].ToString();
                                    proposal.ProposalCountry = dr["ProposalCountry"].ToString();
                                }

                                dr.Close();
                                pc.IntegrateProposals(new WebServicePSI.PSIIntegrationProposal[] { proposal });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GERAL.clsWriteError.WriteLog("userID: " + HttpContext.Current.Session["UserId"].ToString() + " " + ex.ToString() + "Erro na Taxa de Serviço: " + ViewState["refTaxaServico"].ToString());
            }
        }


        public class Impersonation : IDisposable
        {
            private bool disposed = false;

            private string _username, _password, _domain;

            public const int LOGON32_LOGON_INTERACTIVE = 2;
            public const int LOGON32_PROVIDER_DEFAULT = 0;

            WindowsImpersonationContext impersonationContext;

            internal Impersonation(string username, string password, string domain)
            {
                _username = username;
                _password = password;
                _domain = domain;
            }

            [DllImport("advapi32.dll")]
            internal static extern int LogonUserA(String lpszUserName,
                String lpszDomain,
                String lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                ref IntPtr phToken);

            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int DuplicateToken(IntPtr hToken,
                int impersonationLevel,
                ref IntPtr hNewToken);

            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool RevertToSelf();

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            internal static extern bool CloseHandle(IntPtr handle);


            internal bool impersonateValidUser()
            {
                WindowsIdentity tempWindowsIdentity;
                IntPtr token = IntPtr.Zero;
                IntPtr tokenDuplicate = IntPtr.Zero;

                if (RevertToSelf())
                {
                    if (LogonUserA(_username, _domain, _password, LOGON32_LOGON_INTERACTIVE,
                        LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = tempWindowsIdentity.Impersonate();
                            if (impersonationContext != null)
                            {
                                CloseHandle(token);
                                CloseHandle(tokenDuplicate);
                                return true;
                            }
                        }
                    }
                }
                if (token != IntPtr.Zero)
                    CloseHandle(token);
                if (tokenDuplicate != IntPtr.Zero)
                    CloseHandle(tokenDuplicate);
                return false;
            }

            public void undoImpersonation()
            {
                if (impersonationContext != null)
                    impersonationContext.Undo();
            }


            // Implement IDisposable.
            // Do not make this method virtual.
            // A derived class should not be able to override this method.
            public void Dispose()
            {
                Dispose(true);
                // This object will be cleaned up by the Dispose method.
                // Therefore, you should call GC.SupressFinalize to
                // take this object off the finalization queue
                // and prevent finalization code for this object
                // from executing a second time.
                GC.SuppressFinalize(this);
            }

            // Dispose(bool disposing) executes in two distinct scenarios.
            // If disposing equals true, the method has been called directly
            // or indirectly by a user's code. Managed and unmanaged resources
            // can be disposed.
            // If disposing equals false, the method has been called by the
            // runtime from inside the finalizer and you should not reference
            // other objects. Only unmanaged resources can be disposed.
            protected virtual void Dispose(bool disposing)
            {
                // Check to see if Dispose has already been called.
                if (!disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.
                        undoImpersonation();
                    }

                    // Note disposing has been done.
                    disposed = true;

                }
            }

            Impersonation()
            {
                // Do not re-create Dispose clean-up code here.
                // Calling Dispose(false) is optimal in terms of
                // readability and maintainability.
                Dispose(false);
            }


        }

 





        // *****************************************************************************
        // button EVENTS
        // *****************************************************************************

        private void btnNovaVersao_Click(object sender, System.EventArgs e)
        {
            DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();

            string idTaxaServico = TaxaServico.InsertTaxaServicoNovaVersao(ViewState["idTaxaServico"].ToString(), User.Identity.Name.ToString()).ToString();

            TaxaServico = null;

            if (idTaxaServico == "0")
            {
                lblMessage.Text = clsGeral.ErrorMessage.ERR_UPDATE;
            }
            else
            {
                Response.Redirect("FormTaxaServico.aspx?btn=ORC&id=" + idTaxaServico, true);
            }
        }

        private void btnReplica_Click(object sender, System.EventArgs e)
        {
            DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();

            string idTaxaServico = TaxaServico.InsertTaxaServicoReplica(ViewState["idTaxaServico"].ToString(), User.Identity.Name.ToString()).ToString();

            TaxaServico = null;

            if (idTaxaServico == "0")
                lblMessage.Text = clsGeral.ErrorMessage.ERR_UPDATE;
            else
                Response.Redirect("FormTaxaServico.aspx?btn=ORC&id=" + idTaxaServico, true);
        }

        private string sReparacao() //nao posso passar bool porque o crystal reconhece como "Verdadeiro" e "Falso", ja tive isso mas n me lembro onde.
        {
            foreach (DataGridItem dgi in dgLinhasTaxaServico.Items)
            {
                //for (int i = 1; i < dgi.Cells.Count; i++)
                //{
                //    Response.Write(dgi.Cells[i].Text.ToString() + "  " + i.ToString());
                //}

                //for (int i = 1; i < dgi.Cells.Count; i++)
                //{
                //    Response.Write(dgi.Cells[i].ToString() + "  " + i.ToString());
                //}


                //Response.End();

                Label lblTipoServico = (Label)dgi.Cells[0].FindControl("lblTipoServico");
                if (lblTipoServico.Text.StartsWith("Rep") == true)
                {
                    return "sim";
                }

            }
            return "nao";
        }


        private void btnFax_Click(object sender, System.EventArgs e)
        {
            string faxNumber;

            if (txtFaxAlternativo.Text != "")
            {
                faxNumber = txtFaxAlternativo.Text;
            }
            else if (txtFax.Text != "")
            {
                faxNumber = txtFax.Text;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_FAX;
                return;
            }

            //string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_ORCAMENTO"];
            string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_TAXASERVICO"];

            

            ReportClass myreport = new LabMetro.REPORTS.rptTaxaServico();
                   

            clsReport cr = new clsReport();
            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            DataSet ds = orc.myDsTaxaServicoFax(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            myreport.SetDataSource(ds);

            DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter adapter = new LabMetro.DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter();

            DataAccessLayer.TaxaServicoFax.TaxaServicoComentarioDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            //fillCountryRelatedData(); 

            myreport.SetParameterValue("@inFaxNumber", faxNumber);  //de quem recebe!!!!
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

           

            //cr.faxReport(myreport, faxNumber, mailSender, "ORC", ViewState["refTaxaServico"].ToString());
            cr.sendFaxNovo(myreport, faxNumber, "ORC", ViewState["refOrcamento"].ToString());
            myreport.Close();
            myreport.Dispose();
            myreport = null;
            cr = null;
            ds = null;
            orc = null;

            UpdateSetTaxaServicoAsEnviado(ViewState["idTaxaServico"].ToString());

            if (txtRefPSI.Text != "")
            {
                insereBDPSI();
            }


            Response.Redirect("FormTaxaServico.aspx?btn=ORC&id=" + ViewState["idTaxaServico"].ToString(), true);

        }

        private void btnEmail_Click(object sender, System.EventArgs e)
        {
            string emailAddress;
            if (txtMailAlternativo.Text != "")
            {
                emailAddress = txtMailAlternativo.Text;
            }
            else if (txtEmail.Text != "")
            {
           
                emailAddress = txtEmail.Text;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_EMAIL;
                return;
            }

            string cc = txtCCMail.Text;
            if(cc.Contains("@"))
            {
                if(IsValidEmail(cc) == false)
                {
                    lblMessage.Text="Formato do email CC inválido";
                    return;
                }
            }

            
          
           // string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_ORCAMENTO"];
            string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_TAXASERVICO"];


            ReportClass myreport = null; //marteladas mal estruturadas, sem tempo para refazer tudo

            myreport = new LabMetro.REPORTS.rptTaxaServico();

            clsReport cr = new clsReport();

            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();

            DataSet ds = orc.myDsTaxaServicoFax(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            myreport.SetDataSource(ds);

            DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter adapter = new LabMetro.DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter();

            DataAccessLayer.TaxaServicoFax.TaxaServicoComentarioDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", "==por email==");
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);



            string mailBody = "Junto enviamos as Taxas Metrológicas em vigor para a Verificação dos vossos instrumentos metrológicos. " + lblRefTaxaServico.Text + "." + "\r\n" + "\r\n" + "Sem outro assunto," + "\r\n" + "\r\n" + "A equipa Labmetro";

            string title = "Envio de Taxas de Serviço";

           

            cr.mailReport(myreport, emailAddress, mailSender, title, "ORC", ViewState["refTaxaServico"].ToString(), mailBody,cc,"");
            myreport.Close();
            myreport.Dispose();
            myreport = null;
            cr = null;
            ds = null;
            orc = null;

            UpdateSetTaxaServicoAsEnviado(ViewState["idTaxaServico"].ToString());

            if (txtRefPSI.Text != "")
            {
                insereBDPSI();
            }


            Response.Redirect("FormTaxaServico.aspx?btn=ORC&id=" + ViewState["idTaxaServico"].ToString(), true);

        }

        // *****************************************************************************
        // other EVENTS
        // *****************************************************************************
        private void txtValorAjudasCustoDeslocacoes_TextChanged(object sender, System.EventArgs e)
        {
            if (txtValorAjudasCustoDeslocacoes.Text != "")
            {
                if (Double.Parse(txtValorAjudasCustoDeslocacoes.Text.ToString()) > 0)
                {
                    chbDeslocacoes.Checked = true;
                }
                else
                {
                    chbDeslocacoes.Checked = false;
                }
            }
            else
            {
                chbDeslocacoes.Checked = false;
            }
            calculaValorTotal();

        }

        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            fillCompanyInfo(ddEmpresa.SelectedValue);
            fillContactos();
            fillContactInfo(ddContacto.SelectedValue);

            if (ViewState["idTaxaServico"] == null)
            {
                resetTotais();
            }
        }

        private void ddContacto_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillContactInfo(ddContacto.SelectedValue);
        }

        //private void ddIva_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    calculaValorTotal();
        //    TextBoxIva.Text = ddIva.Text + "%";
        //}

        protected void ddComentarioTipoEdit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DropDownList ddComentarioTipo = (DropDownList)sender;
            TextBox txtComentario = (TextBox)(ddComentarioTipo.NamingContainer).FindControl("txtComentarioEdit");

            if (ddComentarioTipo.SelectedIndex > 0)
            {
                txtComentario.Text = ddComentarioTipo.SelectedValue;
            }
        }

        protected void ddComentarioTipo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DropDownList ddComentarioTipo = (DropDownList)sender;

            TextBox txtComentario = (TextBox)(ddComentarioTipo.NamingContainer).FindControl("txtComentario");

            if (ddComentarioTipo.SelectedIndex > 0)
            {
                txtComentario.Text = ddComentarioTipo.SelectedValue;
            }
        }

        private void FillEmpresasContactos()
        {
            fillEmpresas();
            fillCompanyInfo(ddEmpresa.SelectedValue);
            fillContactos();
            fillContactInfo(ddContacto.SelectedValue);
        }

        private void FillEmpresaInfoContactos() //nao preenche a dd empresa
        {
            fillCompanyInfo(ddEmpresa.SelectedValue);
            fillContactos();
            fillContactInfo(ddContacto.SelectedValue);
        }

        private void fillCondicoesPagamento()
        {
            DATA.FacturaData f = new LabMetro.DATA.FacturaData();

            SqlDataReader dr;

            //CONDIÇÕES DE PAGAMENTO - da empresa e da factura
            dr = f.drCondicoesPagamento();

            ddCondicoesPagamento.DataTextField = "descricao";
            ddCondicoesPagamento.DataValueField = "id";
            ddCondicoesPagamento.DataSource = dr;
            ddCondicoesPagamento.DataBind();
            dr.Close();
            dr = null;
          //  ddCondicoesPagamento.Items.Insert(0, new ListItem("", ""));
        }

        private void fillIva()
        {
            DATA.FacturaData f = new LabMetro.DATA.FacturaData();

            SqlDataReader dr;

            //CONDIÇÕES DE PAGAMENTO - da empresa e da factura
            dr = f.drIva();

            ddIva.DataTextField = "Local";
            ddIva.DataValueField = "Valor";
            ddIva.DataSource = dr;
            ddIva.DataBind();
            dr.Close();
            dr = null;
        }

        private void FillDropDowns()
        {
            fillResponsaveisTecnicos();
            fillEstados();
            fillLocaisExecucao();
            fillCondicoesPagamento();
            fillIva();
            //ddIva.Text = ViewState.
        }

        private string codEstadoEmpresa(string idEstadoEmpresa)
        {
            string s = "";
            switch (idEstadoEmpresa)
            {
                case "1":
                    s = "Activa";
                    break;
                case "2":
                    s = "Inactiva";
                    break;
                case "3":
                    s = "Prospecto";
                    break;
            }
            return s;

        }
        private void fillEmpresas()
        {
            DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            DataTable DT = empresa.DTEmpresas(txtPesqEmpresa.Text, txtPesquisaNif.Text, "", "", "", "", "", "", "");

            DataColumn dc = new DataColumn();

            dc.ColumnName = "Nome_";

            DT.Columns.Add(dc);
            foreach (DataRow dr in DT.Rows)
            {
                dr["Nome_"] = dr["nome"].ToString() + "**" + codEstadoEmpresa(dr["idEstadoEmpresa"].ToString()) + "**";
            }

            DataView DV = new DataView(DT);
            ddEmpresa.DataSource = DV; ;
            ddEmpresa.DataBind();

            empresa = null;
        }

        private void fillContactos()
        {
            // Todos os contactos independentemente do estado
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaContactos(ddEmpresa.SelectedValue, "1");
            ddContacto.DataSource = DR;
            ddContacto.DataBind();
            DR.Close();

            lista = null;
        }

        private void fillResponsaveisTecnicos()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            DataTable DT = lista.DTListaFuncionarios("1,4");  //MUITO MAU, HARDCODED!!! MAU PQ FOI PEDIDO ASSIM, DEVIAMOS IR POR PERFIL, mas vamos por funcao.


            // Obter funcionários cuja função é "Responsável Técnico" (4)
            ddFuncionario.DataSource = DT;
            ddFuncionario.DataBind();
            //por default seleccionar o luis godinho, no caso do insert:
            //é feito no pageload
        }

        private void fillEstados()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();

            ddEstado.DataSource = lista.DVListaEstadosOrcamentos();
            ddEstado.DataBind();
        }

        private void fillLocaisExecucao()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaObsLocalExecucao();
            ddLocalExecucao.DataSource = DR;
            ddLocalExecucao.DataBind();
            DR.Close();
        }

        private DropDownList fillTiposServico(DropDownList ddTipoServico)
        {

            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaTipoServico();
            ddTipoServico.DataSource = DR;
            ddTipoServico.DataBind();
            DR.Close();
            ddTipoServico.Items.RemoveAt(0);
            
            ddTipoServico.SelectedValue = "V";
            return ddTipoServico;
        }

        private DropDownList fillTiposEquipamento(DropDownList ddTipoEquipamento)
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaTiposEquipamento();
            ddTipoEquipamento.DataSource = DR;
            ddTipoEquipamento.DataBind();
            ddTipoEquipamento.Items.Insert(0, new ListItem("", ""));
            DR.Close();

            foreach (ListItem i in ddTipoEquipamento.Items)
            {
                i.Text = i.Text.ToLower();
            }
            return ddTipoEquipamento;
        }


        private DropDownList fillEstadoTaxaServicoLinha(DropDownList ddEstadoTaxaServicoLinha)
        {
            string strSQL = "SELECT idEstadoOrcamentoLinha as idEstadoLinha, descricao from EstadoOrcamentoLinha"; //igual para a taxa de serviço
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddEstadoTaxaServicoLinha.DataSource = DR;
            ddEstadoTaxaServicoLinha.DataBind();
            DR.Close();

            return ddEstadoTaxaServicoLinha;
        }

        private DropDownList fillRazaoTaxaServicoLinha(DropDownList ddRazaoTaxaServicoLinha)
        {
            string strSQL = "SELECT idRazaoOrcamentoLinha as idRazaoLinha, descricao from RazaoOrcamentoLinha";//igual para a taxa de servico
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddRazaoTaxaServicoLinha.DataSource = DR;
            ddRazaoTaxaServicoLinha.DataBind();
            DR.Close();
            return ddRazaoTaxaServicoLinha;
        }

        private DropDownList fillComentariosTipo(DropDownList ddComentarioTipo)
        {
            DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();
            //DATATABLE
            ddComentarioTipo.DataSource = TaxaServico.FillComentariosTipo("", "");
            ddComentarioTipo.DataBind();
            ddComentarioTipo.Items.Insert(0, new ListItem("", ""));

            foreach (ListItem i in ddComentarioTipo.Items)
            {
                i.Attributes.Add("Title", i.Text.ToString());

                int l = i.Text.Length;
                if (l > 100)
                {
                    i.Text = i.Text.Substring(0, 100) + ".";
                }
            }
            return ddComentarioTipo;
        }


        private void fillContactInfo(string idContacto)
        {
            LabMetro.DATA.ContactosBD contacto = new LabMetro.DATA.ContactosBD();
            LabMetro.DATA.ContactDetails detContacto = contacto.GetContactDetails(idContacto);
            if (detContacto != null)
            {
                txtDepartamento.Text = detContacto.departamento;
                txtEmail.Text = detContacto.emailEmpresa;
                txtFax.Text = detContacto.faxEmpresa;
            }
        }


        private void txtMailAlternativo_TextChanged(object sender, System.EventArgs e)
        {
        }

        private void btnPesquisaEquip_Click(object sender, System.EventArgs e)
        {
            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            DataTable dt = orc.DTEquipamentosDeTaxaServico(txtPesquisaEquipamento.Text, txtPesquisaEmpresa.Text);
            DGEq.DataSource = dt;
            DGEq.DataBind();
        }

        private void UpdateSetTaxaServicoAsEnviado(string idTaxaServico)
        {
            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            int i = orc.setTaxaServicoAsEnviado(idTaxaServico, User.Identity.Name.ToString());
        }


        private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
        {
            FillEmpresasContactos();

            if (ViewState["idTaxaServico"] == null)
            {
                resetTotais();
            }
        }

        private void txtPesqEmpresa_TextChanged(object sender, System.EventArgs e)
        {
            FillEmpresasContactos();

            if (ViewState["idTaxaServico"] == null)
            {
                resetTotais();
            }
        }

        private void btnEmpresas_Click(object sender, System.EventArgs e)
        {
            FillEmpresasContactos();

            if (ViewState["idTaxaServico"] == null)
            {
                resetTotais();
            }
        }

        private void btnResetPesquisaEquips_Click(object sender, System.EventArgs e)
        {
            DGEq.DataSource = null;
            DGEq.DataBind();
        }

        private void btnVerFax_Click(object sender, System.EventArgs e)
        {
            string faxNumber = " ";
            if (txtFaxAlternativo.Text != "")
            {
                faxNumber = txtFaxAlternativo.Text;
            }
            else if (txtFax.Text != "")
            {
                faxNumber = txtFax.Text;
            }

            ReportClass myreport = new LabMetro.REPORTS.rptTaxaServico();
                  

            clsReport cr = new clsReport();

            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();

            DataSet ds = orc.myDsTaxaServicoFax(System.Convert.ToInt32(ViewState["idTaxaServico"]));
            myreport.SetDataSource(ds);

            DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter adapter = new   DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter();

            DataAccessLayer.TaxaServicoFax.TaxaServicoComentarioDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", faxNumber);  //de quem recebe!!!!
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);



            ds = null;
            orc = null;
            cr.exportReportToPDF(myreport, "TaxaServico");
        }

        private void btnVerMail_Click(object sender, System.EventArgs e)
        {
            

            ReportClass myreport = new LabMetro.REPORTS.rptTaxaServico();

            clsReport cr = new clsReport();

            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            DataSet ds = orc.myDsTaxaServicoFax(System.Convert.ToInt32(ViewState["idTaxaServico"]));
            myreport.SetDataSource(ds);


            DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter adapter = new LabMetro.DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter();

            DataAccessLayer.TaxaServicoFax.TaxaServicoComentarioDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", "==por email==");
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

            

            ds = null;
            orc = null;

            cr.exportReportToPDF(myreport, "TaxaServico");
        }

        // Preencher os dados relativos à empresa (se é devedora ou não)
        private void fillCompanyInfo(string idEmpresa)
        {
            if (idEmpresa != "")
            {
                lblEmpresaDevedora.Text = ""; //limpar a label

                LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
                LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(idEmpresa);

                if (detEmpresa != null)
                {
                    lblObsEmpresa.Text = detEmpresa.observacoes;    //meu

                    txtDesconto.Text = detEmpresa.percDesconto;

                    lblCondPagamEmpresa.Text = detEmpresa.condicoesPagamento.ToUpper();

                    if (detEmpresa.pagamentoAtraso == "1")
                    {
                        lblEmpresaDevedora.Text += "** PAGAMENTOS EM ATRASO **<br />";
                    }
                    else
                    {
                        lblEmpresaDevedora.Text = "";
                    }
                    //martelada
                    switch (detEmpresa.nivelBloqueioLabmetro)
                    {
                        case "0":
                            trEmpresa.BgColor = "";
                            break;
                        case "1":
                            trEmpresa.BgColor = "Gold";
                            break;
                        case "2":
                            trEmpresa.BgColor = "DarkOrange";
                            lblEmpresaDevedora.Text += "Venda à dinheiro ou Pagamento do Atrasado.<br />";
                            break;
                        case "3":
                            trEmpresa.BgColor = "Crimson";
                            lblEmpresaDevedora.Text += "Venda à dinheiro.<br />";
                            break;
                    }

                    switch (detEmpresa.codigoBloqueioSAP)
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

                //if (Request.QueryString["id"] == null)
                //{
                //    ddCondicoesPagamento.SelectedValue = detEmpresa.idCondicoesPagamento;
                //}
            }
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {

            string strFileName = txtNomeFicheiro.Text.ToString(); ;
            //caminho relativo
            string path = (string)ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_PATH_REL"];
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
                    DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();

                    //se ele nao consegue fazer isto, nao faz mal.
                    orc.ApagaFicheiroPedidoTaxaServico(ViewState["idTaxaServico"].ToString(), User.Identity.Name.ToString());
                }
                catch
                {
                    //nada. nao posso controlar se ele quer apagar mm da bd, ou se carrega para apagar da dir.
                }
            }
        }

        private void btnUpload_Click(object sender, System.EventArgs e)
        {
            if (fileIn.PostedFile.FileName != "")
            {
                string strFileName;

                try
                {
                    strFileName = System.IO.Path.GetFileName(fileIn.PostedFile.FileName);
                    string path = (string)ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_PATH_REL"];
                    string myPath = Server.MapPath("~/" + path);

                    if (File.Exists(myPath + "/" + strFileName))
                    {
                        lblMessage.Text = "Já existe um ficheiro com o mesmo nome.";
                    }
                    else
                    {
                        fileIn.PostedFile.SaveAs(myPath + "/" + strFileName);
                        txtNomeFicheiro.Text = strFileName;

                        string url = (string)ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_URL"];
                        //file:////SomeServer/Some File With Spaces....

                        url = url + "/" + strFileName;
                        linkFicheiro.NavigateUrl = url;
                    }
                }

                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog("Erro no carregamento de ficheiros." + ex.ToString());
                    lblMessage.Text = "Erro no carregamento do ficheiro.";
                }
            }
        }

        protected void btnAprovarTodos_Click(object sender, EventArgs e)
        {
            string strIds = "";

            foreach (DataGridItem dgi in dgLinhasTaxaServico.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("chkSelected");
                if (myCheckBox.Checked == true)
                {
                    strIds += dgLinhasTaxaServico.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter);

            string strSQL = "UPDATE TaxaServicoLinha SET idEstadoTaxaServicoLinha = 2 WHERE idTaxaServicoLinha IN (" + strIds + ")";
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());

        }



        //mesmo codigo que acima, podia fazer uma funcao dps para ficar mais bonito
        protected void btnRejeitarTodos_Click(object sender, EventArgs e)
        {
            string strIds = "";

            foreach (DataGridItem dgi in dgLinhasTaxaServico.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("chkSelected");
                if (myCheckBox.Checked == true)
                {
                    strIds += dgLinhasTaxaServico.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter);

            string strSQL = "UPDATE TaxaServicoLinha SET idEstadoTaxaServicoLinha = 3 WHERE idTaxaServicoLinha IN (" + strIds + ")";
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            BindGridLinhasTaxaServico(ViewState["idTaxaServico"].ToString());
        }


        private string savetodisk()
        {
            ReportClass myreport = new LabMetro.REPORTS.rptTaxaServico();

            clsReport cr = new clsReport();

            DATA.TaxaServicoBD orc = new LabMetro.DATA.TaxaServicoBD();
            DataSet ds = orc.myDsTaxaServicoFax(System.Convert.ToInt32(ViewState["idTaxaServico"]));
            myreport.SetDataSource(ds);

            DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter adapter = new DataAccessLayer.TaxaServicoFaxTableAdapters.TaxaServicoComentarioTableAdapter();

            DataAccessLayer.TaxaServicoFax.TaxaServicoComentarioDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idTaxaServico"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", "==por email==");
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

          

            ds = null;
            orc = null;

            return cr.saveReportToDisk(myreport, "LAB", txtIdPtComercial.Text);


        }

        protected void btnPTComercial_Click(object sender, EventArgs e)
        {

            if (txtIdPtComercial.Text == "")
            {
                lblMessagePTComercial.Text = "Insira a referência PT Comercial";
                return;
            }

            // GRAVAR O REPORT PARA O DISCO:
            string fName = savetodisk();





            //string myPath = (string)ConfigurationManager.AppSettings["TEMP_PATH_PTCOMERCIAL"];
            //myPath = myPath + "/" + "teste.pdf";//filename.ToString();

            //converter o ficheiro para byte
            byte[] file;
            using (var stream = new FileStream(fName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            //SqlParameter[] arrParams = new SqlParameter[7];

           
            BDCOMERCIAL.WebService2 bdComercial = new BDCOMERCIAL.WebService2();
         
            int i = bdComercial.PROPOSTASLABPT(lblRefTaxaServico.Text, txtDtTaxaServico.Text, ddFuncionario.SelectedItem.Text, Decimal.Parse(txtValorTotal.Text), "nomeficheiro", Int64.Parse(txtIdPtComercial.Text), file);
            string id = i.ToString();

            if (id != "-1")
            {
                ViewState["idLinhaPtComercial"] = id;

                //apagar ficheiro da pasta temporario
                FileInfo myfileinf = new FileInfo(fName);
                if (myfileinf.Exists)
                {
                    try
                    {
                        myfileinf.Delete();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }
                }

                //actualizar o orçamento com a informação
                string strSQL = "UPDATE TaxaServico set idPtComercial = " + txtIdPtComercial.Text + ", idEstadoTaxaServico =  3,   dtPTComercial = getDate(), idLinhaPTComercial = " + id + " where idTaxaServico = " + ViewState["idTaxaServico"].ToString();
                GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
                fillForm(ViewState["idTaxaServico"].ToString()); //reload form (???)
                lblMessagePTComercial.Text = "Inserido com sucesso";


            }

            else
            {
                ViewState["idLinhaPtComercial"] = "";
                lblMessagePTComercial.Text = "ID do Pedido não existe ou ocorreu outro erro de BD.";
            }

        }

        private void OpenFilePtComercial(string id)
        {

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionStringPTCOMERCIAL"];
            string commandText = "SELECT f from ComercialPropostas where id = " + id;

            SqlConnection objConn = new SqlConnection(connectionString);
            SqlCommand objCmd = new SqlCommand(commandText, objConn);


            objCmd.CommandType = CommandType.Text;

            try
            {
                objConn.Open();

                using (SqlDataReader myReader = objCmd.ExecuteReader())
                {
                    if (myReader.Read())
                    {

                        byte[] file = (byte[])myReader["f"];
                        Response.Clear();

                        MemoryStream ms = new MemoryStream(file);
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;");// filename=myFile.pdf");
                        HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                        HttpContext.Current.Response.End();

                        ms.Close();
                    }
                }
            }
            catch
            {
                objConn.Close();
                throw;
            }


            
        }

        bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }


    }
}

