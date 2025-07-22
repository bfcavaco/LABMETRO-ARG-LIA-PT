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

using System.IO;
using System.Text;


namespace LabMetro
{
    /// <summary>
    /// Summary description for FormOrcam.
    /// isto já está tudo uma grande confusao.... qq dia tem de se refazer isto tudo
    /// </summary>
    public partial class FormOrcamentoES : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.TextBox Textbox1;
        protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator1;
        protected System.Web.UI.WebControls.DataGrid dgEmpresas;
        protected System.Web.UI.WebControls.DropDownList dd;

        private double dbValorSubTotal = 0;
        private string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();
        private const string ID_PAG = "ORCAMENTOS_1";//NOME DA PAGINA

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
                    if (intAcesso == 0) //so tem permissoes de leitura
                    {
                        btnSubmit.Enabled = false;
                        btnNovaVersao.Enabled = false;
                        btnReplica.Enabled = false;
                        btnFax.Enabled = false;
                        btnEmail.Enabled = false;
                        btnVerFax.Enabled = false;
                        btnVerMail.Enabled = false;
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
                            ViewState["idOrcamento"] = Request.QueryString["id"].ToString();
                        }
                    }
                    //validar se o id existe (tb pode ter sido preenchido apos o insert)
                    if (ViewState["idOrcamento"] != null)
                    {
                        if (ViewState["idOrcamento"].ToString() != "")
                        {
                            if (!Page.IsPostBack)
                            {
                                fillForm(ViewState["idOrcamento"].ToString());
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
            //*************
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
            if (ViewState["idOrcamento"] == null)
            {
                resetTotais();
                LabMetro.DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();
                BindGridLinhasOrcamento("0");
                BindGridComentariosOrcamento("0");

                // Preencher as datas 
                txtDtPedido.Text = System.DateTime.Today.ToShortDateString();
                txtDtOrcamento.Text = System.DateTime.Today.ToShortDateString();
                int mesesValidade = System.Convert.ToInt32(orcamento.mesesValidadeOrcamento());

                txtMesesValidade.Text = mesesValidade.ToString();

                System.DateTime today_ = System.DateTime.Today;
                System.DateTime dtValidadeOrcamento = today_.AddMonths(mesesValidade);
                txtDtValidade.Text = dtValidadeOrcamento.ToShortDateString();

                orcamento = null;
            }
        }

        private void txtMesesValidade_TextChanged(object sender, System.EventArgs e)
        {
            int mesesValidade = System.Convert.ToInt32(txtMesesValidade.Text);
            System.DateTime today_ = System.DateTime.Today;
            System.DateTime dtValidadeOrcamento = today_.AddMonths(mesesValidade);
            txtDtValidade.Text = dtValidadeOrcamento.ToShortDateString();
        }

        private void BindGridHistorico()
        {
            DATA.OrcamentoBD orcamentos = new LabMetro.DATA.OrcamentoBD();
            SqlDataReader DR = orcamentos.DRGetOrcamentoHistoricoEstados(ViewState["idOrcamento"].ToString());
            dgHistorico.DataSource = DR;
            dgHistorico.DataBind();
            DR.Close();
            orcamentos = null;
        }

        private void BindGridComentariosOrcamento(string idOrcamento)
        {
            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataTable dt = orc.DTComentariosOrcamentoByIdOrcamento(idOrcamento);
            DataView dv = new DataView(dt);
            dgComentariosOrcamento.DataSource = dv;
            dgComentariosOrcamento.DataBind();
            orc = null;
        }

        private void BindGridLinhasOrcamento(string idOrcamento)
        {
            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataTable dt = orc.DTLinhasOrcamentoByIdOrcamento(idOrcamento);
            DataView dv = new DataView(dt);
            dgLinhasOrcamento.DataSource = dv;
            dgLinhasOrcamento.DataBind();
            orc = null;
        }


        private void fillForm(string id)
        {
            LabMetro.DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();
            DataTable OrcamentoDT = orcamento.DTOrcamentoDetails(id);

            if (OrcamentoDT.Rows.Count > 0)
            {
                lblRefOrcamento.Text = OrcamentoDT.Rows[0]["refOrcamento"].ToString();
                ViewState["refOrcamento"] = OrcamentoDT.Rows[0]["refOrcamento"].ToString();

                lblVersao.Text = OrcamentoDT.Rows[0]["versao"].ToString();
                string idEmpresa = OrcamentoDT.Rows[0]["idEmpresa"].ToString();
                string nomeEmpresa = OrcamentoDT.Rows[0]["nomeEmpresa"].ToString();

                ddEmpresa.Items.Insert(0, new ListItem(nomeEmpresa, idEmpresa));
                ddEmpresa.SelectedValue = idEmpresa;

                string idEstadoOrcamento = OrcamentoDT.Rows[0]["idEstadoOrcamento"].ToString();
                string estadoOrcamento = OrcamentoDT.Rows[0]["estadoOrcamento"].ToString();

                try
                {
                    ddEstado.SelectedValue = idEstadoOrcamento;
                }
                catch
                {
                    ddEstado.Items.Insert(0, new ListItem(estadoOrcamento, idEstadoOrcamento));
                    ddEstado.SelectedValue = idEstadoOrcamento;
                }

                FillEmpresaInfoContactos();
                // actualizar a lista de contactos porque seleccionámos uma empresa
                //.. tem de ser chamado antes da próxima linha...

                string idContacto = OrcamentoDT.Rows[0]["idContacto"].ToString();
                string nomeContacto = OrcamentoDT.Rows[0]["nomeContacto"].ToString();
                try
                {
                    ddContacto.SelectedValue = idContacto;
                }
                catch
                {
                    ddContacto.Items.Insert(0, new ListItem(nomeContacto, idContacto));
                    ddContacto.SelectedValue = idContacto;
                }

                string idFuncionarioRespTecnico = OrcamentoDT.Rows[0]["idFuncionarioRespTecnico"].ToString();
                string nomeFuncionarioRespTecnico = OrcamentoDT.Rows[0]["nomeFuncionario"].ToString();
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

                string idObsLocalExecucao = OrcamentoDT.Rows[0]["idObsLocalExecucao"].ToString();
                string obsLocalExecucao = OrcamentoDT.Rows[0]["obsLocalExecucao"].ToString();
                try
                {
                    ddLocalExecucao.SelectedValue = idObsLocalExecucao;
                }
                catch
                {
                    ddLocalExecucao.Items.Insert(0, new ListItem(obsLocalExecucao, idObsLocalExecucao));
                    ddLocalExecucao.SelectedValue = idObsLocalExecucao;
                }

                cbCalExterna.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(OrcamentoDT.Rows[0]["calibracaoExterna"].ToString()).ToString());
                txtValorAjudasCustoDeslocacoes.Text = GERAL.clsGeral.ConvertDBMoneyToString(OrcamentoDT.Rows[0]["valorAjudasCustoDeslocacoes"].ToString());

                txtDtPedido.Text = clsGeral.ToShortDate(OrcamentoDT.Rows[0]["dtPedido"].ToString());

                if (idEstadoOrcamento.ToString() == "1") //pedido
                {

                    txtDtOrcamento.Text = System.DateTime.Today.ToShortDateString();
                    int mesesValidade;
                    try
                    {
                        mesesValidade = System.Convert.ToInt32(OrcamentoDT.Rows[0]["validadeMeses"].ToString());
                    }
                    catch
                    {
                        Response.Write(OrcamentoDT.Rows[0]["validadeMeses"].ToString());
                        mesesValidade = 1;
                    }

                    System.DateTime today_ = System.DateTime.Today;
                    System.DateTime dtValidadeOrcamento = today_.AddMonths(mesesValidade);
                    txtDtValidade.Text = dtValidadeOrcamento.ToShortDateString();
                }
                else
                {
                    txtDtOrcamento.Text = clsGeral.ToShortDate(OrcamentoDT.Rows[0]["dtOrcamento"].ToString());
                    txtDtValidade.Text = clsGeral.ToShortDate(OrcamentoDT.Rows[0]["dtValidade"].ToString());

                }

                txtMesesValidade.Text = OrcamentoDT.Rows[0]["validadeMeses"].ToString();
                txtRefCliente.Text = OrcamentoDT.Rows[0]["referenciaCliente"].ToString();
                txtTempoExecucao.Text = OrcamentoDT.Rows[0]["tempoExecucao"].ToString();
                txtLocalidadeCalib.Text = OrcamentoDT.Rows[0]["localidadeCalibracao"].ToString();
                txtObservacoes.Text = OrcamentoDT.Rows[0]["observacoes"].ToString();
                chbComTotal.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(OrcamentoDT.Rows[0]["bTotal"].ToString()).ToString());
                chbDesconto.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(OrcamentoDT.Rows[0]["bDesconto"].ToString()).ToString());
                chbDeslocacoes.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(OrcamentoDT.Rows[0]["bDeslocacoes"].ToString()).ToString());
                cbFollowup.Checked = clsGeral.ConvertStringToBoolean(clsGeral.ConvertStringToBool(OrcamentoDT.Rows[0]["bFollowup"].ToString()).ToString());
                txtNomeFicheiro.Text = OrcamentoDT.Rows[0]["nomeFicheiro"].ToString();

                //para abrir o ficheiro
                if (OrcamentoDT.Rows[0]["nomeFicheiro"].ToString() != null && OrcamentoDT.Rows[0]["nomeFicheiro"].ToString().ToString() != "")
                {
                    string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_URL"];

                    myPath = myPath + "/" + OrcamentoDT.Rows[0]["nomeFicheiro"].ToString();
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
                    ddCondicoesPagamento.SelectedValue = OrcamentoDT.Rows[0]["idCondicoesPagamento"].ToString();
                }
                catch
                { }
                // Preencher as Linhas do Orçamento

                BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());

                // Preencher os Comentários (preencho o DataView com o que vem da BD)

                BindGridComentariosOrcamento(ViewState["idOrcamento"].ToString());

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
            dgLinhasOrcamento.EditCommand += new DataGridCommandEventHandler(dgLinhasOrcamento_EditCommand);
            dgLinhasOrcamento.ItemCommand += new DataGridCommandEventHandler(dgLinhasOrcamento_ItemCommand);
            dgLinhasOrcamento.CancelCommand += new DataGridCommandEventHandler(dgLinhasOrcamento_CancelGrid);
            dgLinhasOrcamento.UpdateCommand += new DataGridCommandEventHandler(dgLinhasOrcamento_UpdateGrid);
            dgLinhasOrcamento.DeleteCommand += new DataGridCommandEventHandler(dgLinhasOrcamento_DeleteCommand);
            dgLinhasOrcamento.ItemDataBound += new DataGridItemEventHandler(dgLinhasOrcamento_ItemDataBound);

            dgComentariosOrcamento.EditCommand += new DataGridCommandEventHandler(dgComentariosOrcamento_EditCommand);
            dgComentariosOrcamento.ItemCommand += new DataGridCommandEventHandler(dgComentariosOrcamento_ItemCommand);
            dgComentariosOrcamento.CancelCommand += new DataGridCommandEventHandler(dgComentariosOrcamento_CancelGrid);
            dgComentariosOrcamento.UpdateCommand += new DataGridCommandEventHandler(dgComentariosOrcamento_UpdateGrid);
            dgComentariosOrcamento.DeleteCommand += new DataGridCommandEventHandler(dgComentariosOrcamento_DeleteCommand);
            dgComentariosOrcamento.ItemDataBound += new DataGridItemEventHandler(dgComentariosOrcamento_ItemDataBound);

            ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            ddContacto.SelectedIndexChanged += new System.EventHandler(ddContacto_SelectedIndexChanged);
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
            btnVerMail.Click += new System.EventHandler(btnVerMail_Click);
            btnRemove.Click += new System.EventHandler(btnRemove_Click);
            btnUpload.Click += new System.EventHandler(btnUpload_Click);

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
        // dgLinhasOrcamento - DATAGRID COMMANDS
        // *****************************************************************************
        private void dgLinhasOrcamento_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgLinhasOrcamento.EditItemIndex = e.Item.ItemIndex;
            BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());
            dgLinhasOrcamento.ShowFooter = false;
        }

        private void dgLinhasOrcamento_CancelGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgLinhasOrcamento.EditItemIndex = -1;
            BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());
            dgLinhasOrcamento.ShowFooter = true;
        }

        private void dgLinhasOrcamento_UpdateGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idOrcamento = ViewState["idOrcamento"].ToString();

            if (idOrcamento == "")
            {
                lblMessage.Text = "Tem de submeter o orçamento.";
            }
            else
            {
                // Obter os novos valores
                DropDownList ddTipoServico = (DropDownList)e.Item.FindControl("ddTipoServicoEdit");
                DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoEdit");
                DropDownList ddEstadoOrcamentoLinha = (DropDownList)e.Item.FindControl("ddEstadoOrcamentoLinhaEdit");
                DropDownList ddRazaoOrcamentoLinha = (DropDownList)e.Item.FindControl("ddRazaoOrcamentoLinhaEdit");

                TextBox txtQuantidade = (TextBox)e.Item.FindControl("txtQuantidadeEdit");
                TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoEdit");
                TextBox txtAsteriscoLinha = (TextBox)e.Item.FindControl("txtAsteriscoLinhaEdit");
                //TextBox txtMarca = (TextBox)e.Item.FindControl("txtMarcaEdit"); 
                //TextBox txtModelo = (TextBox)e.Item.FindControl("txtModeloEdit"); 

                //TextBox txtAlcance = (TextBox)e.Item.FindControl("txtAlcanceEdit"); 
                //TextBox txtClasse = (TextBox)e.Item.FindControl("txtClasseEdit"); 

                //TextBox txtNumPontos =(TextBox)e.Item.FindControl("txtNumPontosEdit"); 
                //TextBox txtDescPontos = (TextBox)e.Item.FindControl("txtDescPontosEdit"); 

                TextBox txtValorUnitario = (TextBox)e.Item.FindControl("txtValorUnitarioEdit");
                TextBox txtValorLinha = (TextBox)e.Item.FindControl("txtValorLinhaEdit");
                TextBox txtPercDescontoLinha = (TextBox)e.Item.FindControl("txtPercDescontoEdit");


                // Preencher valores default caso os campos estejam vazios
                if (txtQuantidade.Text == "") txtQuantidade.Text = "1";
                if (txtValorUnitario.Text == "") txtValorUnitario.Text = "0";
                if (txtValorLinha.Text == "") txtValorLinha.Text = "0";
                if (txtPercDescontoLinha.Text == "") txtPercDescontoLinha.Text = "0";

                 //EM ESPANHA NAO PRECISAM DE TER DESCRICAO.
                //if (txtDescricao.Text == "")
                //    lblMessage.Text = "Tem de preencher o campo 'Descrição'.";
                //else
                //{
                    // Actualizar directamente a Base de dados. 
                    string idOrcamentoLinha = dgLinhasOrcamento.DataKeys[e.Item.ItemIndex].ToString();
                    string idTipoServico = ddTipoServico.SelectedValue.ToString();
                    //string tipoServico = ddTipoServico.SelectedItem.Text;
                    string idTipoEquipamento = ddTipoEquipamento.SelectedValue.ToString();
                    string tipoEquipamento = ddTipoEquipamento.SelectedItem.Text;
                    string quantidade = txtQuantidade.Text;
                    string descricaoEquipamento = txtDescricao.Text;
                    string asterisco = txtAsteriscoLinha.Text;
                    //string marca = "";//txtMarca.Text;
                    //string modelo = "";// txtModelo.Text;
                    //string alcance = "";//txtAlcance.Text;
                    //string classe = "";//txtClasse.Text;
                    //string numPontos = "";//txtNumPontos.Text;
                    //string descPontos = "";//txtDescPontos.Text;
                    string percDescontoLinha = txtPercDescontoLinha.Text;
                    string valorUnitario = txtValorUnitario.Text;
                    double valorUnitarioComDesconto = (Double.Parse(valorUnitario) * (100 - Double.Parse(percDescontoLinha)) * 0.01);
                    double valorLinha = System.Math.Round(Double.Parse(quantidade) * valorUnitarioComDesconto, 2);

                    DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
                    //retorna o num de linhas afectadas mas eu agora nao utilizo
                    orc.UpdateOrcamentoLinha(idOrcamentoLinha, idOrcamento, idTipoServico, idTipoEquipamento, quantidade, descricaoEquipamento, asterisco,  valorUnitario, valorLinha.ToString(), User.Identity.Name.ToString(), percDescontoLinha, ddEstadoOrcamentoLinha.SelectedValue.ToString(), ddRazaoOrcamentoLinha.SelectedValue.ToString());
                    orc = null;

                //}

                dgLinhasOrcamento.EditItemIndex = -1;
                BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());
                dgLinhasOrcamento.ShowFooter = true;
            }


        }

        private void dgLinhasOrcamento_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            if ((ViewState["idOrcamento"] == null) || (ViewState["idOrcamento"].ToString() == ""))
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
                    //TextBox txtMarca = (TextBox)e.Item.FindControl("txtMarca"); 
                    //TextBox txtModelo = (TextBox)e.Item.FindControl("txtModelo"); 

                    //TextBox txtAlcance = (TextBox)e.Item.FindControl("txtAlcance"); 

                    //TextBox txtClasse = (TextBox)e.Item.FindControl("txtClasse"); 

                    //TextBox txtNumPontos = (TextBox)e.Item.FindControl("txtNumPontos"); 
                    //TextBox txtDescPontos = (TextBox)e.Item.FindControl("txtDescPontos"); 
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

                        string idOrcamento = ViewState["idOrcamento"].ToString();
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

                        DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();

                        orc.InsertOrcamentoLinha(idOrcamento, idTipoServico, idTipoEquipamento, quantidade, descricaoEquipamento, asterisco, valorUnitario, valorLinha.ToString(), User.Identity.Name.ToString(), percDescontoLinha);

                        orc = null;

                    }
                }
                dgLinhasOrcamento.EditItemIndex = -1;
                BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());
            }
        }

        private void dgLinhasOrcamento_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                DropDownList ddEstadoOrcamentoLinha = fillEstadoOrcamentoLinha((DropDownList)e.Item.FindControl("ddEstadoOrcamentoLinhaEdit"));
                DropDownList ddRazaoOrcamentoLinha = fillRazaoOrcamentoLinha((DropDownList)e.Item.FindControl("ddRazaoOrcamentoLinhaEdit"));

                //Para as combos ficarem seleccionadas no item correcto
                string idTipoServico = drv["idTipoServico"].ToString();
                if (idTipoServico != "") ddTipoServico.SelectedValue = idTipoServico;

                string idTipoEquipamento = drv["idTipoEquipamento"].ToString();
                if (idTipoEquipamento != "") ddTipoEquipamento.SelectedValue = idTipoEquipamento;

                string idEstadoOrcamentoLinha = drv["idEstadoOrcamentoLinha"].ToString();
                ddEstadoOrcamentoLinha.SelectedValue = idEstadoOrcamentoLinha;

                string idRazaoOrcamentoLinha = drv["idRazaoOrcamentoLinha"].ToString();
                ddRazaoOrcamentoLinha.SelectedValue = idRazaoOrcamentoLinha;
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

        private void dgLinhasOrcamento_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idOrcamentoLinha = dgLinhasOrcamento.DataKeys[e.Item.ItemIndex].ToString();

            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();

            orc.DeleteOrcamentoLinha(idOrcamentoLinha, User.Identity.Name.ToString());

            orc = null;

            BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());
            dgLinhasOrcamento.ShowFooter = true;

            calculaValorTotal();

            if (dgLinhasOrcamento.DataKeys.Count == 0) //nao ha items - (nao encontrei outra maneira...)
            {
                resetTotais();
            }
        }

        // *****************************************************************************
        // dgComentariosOrcamento - DATAGRID COMMANDS
        // *****************************************************************************
        private void dgComentariosOrcamento_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgComentariosOrcamento.EditItemIndex = e.Item.ItemIndex;
            BindGridComentariosOrcamento(ViewState["idOrcamento"].ToString());
            dgComentariosOrcamento.ShowFooter = false;
        }

        private void dgComentariosOrcamento_CancelGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgComentariosOrcamento.EditItemIndex = -1;
            BindGridComentariosOrcamento(ViewState["idOrcamento"].ToString());
            dgComentariosOrcamento.ShowFooter = true;
        }

        private void dgComentariosOrcamento_UpdateGrid(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            DropDownList ddComentarioTipo = (DropDownList)e.Item.FindControl("ddComentarioTipoEdit");
            TextBox txtComentario = (TextBox)e.Item.FindControl("txtComentarioEdit");
            TextBox txtAsterisco = (TextBox)e.Item.FindControl("txtAsteriscoEdit");

            string idOrcamento = ViewState["idOrcamento"].ToString();
            string idOrcamentoComentario = dgComentariosOrcamento.DataKeys[e.Item.ItemIndex].ToString();

            //string ident = ddComentarioTipo.SelectedValue.ToString(); //???
            //string orcamentoComentarioTipo = ddComentarioTipo.SelectedItem.Text;
            string descricao = txtComentario.Text;
            string asterisco = txtAsterisco.Text;

            //alterar directamente na bd
            DATA.OrcamentoBD orc = new DATA.OrcamentoBD();
            orc.UpdateOrcamentoComentario(idOrcamentoComentario, idOrcamento, descricao, asterisco, User.Identity.Name.ToString());
            orc = null;

            dgComentariosOrcamento.EditItemIndex = -1;
            BindGridComentariosOrcamento(ViewState["idOrcamento"].ToString());
            dgComentariosOrcamento.ShowFooter = true;
        }


        private void dgComentariosOrcamento_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if ((ViewState["idOrcamento"] == null) || (ViewState["idOrcamento"].ToString() == ""))
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
                        string idOrcamento = ViewState["idOrcamento"].ToString();
                        //??//string ident = ddComentarioTipo.SelectedValue.ToString();
                        string orcamentoComentarioTipo = ddComentarioTipo.SelectedItem.Text;
                        string descricao = txtComentario.Text;
                        string asterisco = txtAsterisco.Text;

                        DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
                        orc.InsertOrcamentoComentario(idOrcamento, descricao, asterisco, User.Identity.Name.ToString());
                        orc = null;
                    }
                }
                dgComentariosOrcamento.EditItemIndex = -1;
                BindGridComentariosOrcamento(ViewState["idOrcamento"].ToString());
            }
        }

        private void dgComentariosOrcamento_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idOrcamentoComentario = dgComentariosOrcamento.DataKeys[e.Item.ItemIndex].ToString();
            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            orc.DeleteOrcamentoComentario(idOrcamentoComentario, User.Identity.Name.ToString());
            orc = null;

            BindGridComentariosOrcamento(ViewState["idOrcamento"].ToString());
            dgComentariosOrcamento.ShowFooter = true;
        }

        private void dgComentariosOrcamento_ItemDataBound(object sender, DataGridItemEventArgs e)
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

              string strFollowup = "";
            if (cbFollowup.Checked == true) strFollowup = "1";

            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();
            int idOrcamento = orcamento.InsertOrcamento(ddEmpresa.SelectedValue, ddContacto.SelectedValue, ddFuncionario.SelectedValue, ddLocalExecucao.SelectedValue, ddEstado.SelectedValue, txtRefCliente.Text, txtDtPedido.Text, txtTempoExecucao.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotal.Text, txtDtOrcamento.Text, txtDtValidade.Text, cbCalExterna.Checked.ToString(), txtLocalidadeCalib.Text, txtObservacoes.Text, User.Identity.Name.ToString(), chbComTotal.Checked.ToString(), txtMesesValidade.Text, chbDesconto.Checked.ToString(), chbDeslocacoes.Checked.ToString(), ddCondicoesPagamento.SelectedValue, txtNomeFicheiro.Text,null,null,strFollowup);

            orcamento = null;
            return idOrcamento;
        }

        private void UpdateBD()
        {

            string strFollowup = "";
            if (cbFollowup.Checked == true) strFollowup = "1";
            DATA.OrcamentoBD orc = new DATA.OrcamentoBD();
            int x = orc.UpdateOrcamento(ViewState["idOrcamento"].ToString(), ddEmpresa.SelectedValue, ddContacto.SelectedValue, ddFuncionario.SelectedValue, ddLocalExecucao.SelectedValue, ddEstado.SelectedValue, txtRefCliente.Text, txtDtPedido.Text, txtTempoExecucao.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotal.Text, txtDtOrcamento.Text, txtDtValidade.Text, cbCalExterna.Checked.ToString(), txtLocalidadeCalib.Text, txtObservacoes.Text, User.Identity.Name.ToString(), chbComTotal.Checked.ToString(), txtMesesValidade.Text, chbDesconto.Checked.ToString(), chbDeslocacoes.Checked.ToString(), ddCondicoesPagamento.SelectedValue, txtNomeFicheiro.Text, null, null, strFollowup);

            orc = null;
            BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString()); //bind grid por causa da actualizacao das linhas com o estadoOrcamentoLinha no caso de aceite ou reprovado;
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                calculaValorTotal(); // só para garantir que o total está bem actualizado

                DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

                if (btnSubmit.CommandArgument == "insert")
                {
                    try
                    {
                        string idOrcamento = InsertBD().ToString();
                        ViewState["idOrcamento"] = idOrcamento;
                        btnSubmit.CommandArgument = "update";
                        enableButtons();

                        //ir buscar a ref para mostrar e guardar em viewstate para 
                        //criar o nome do ficheiro no envio do fax
                        string refOrcamento = orcamento.refOrcamento(idOrcamento);
                        lblRefOrcamento.Text = refOrcamento;
                        ViewState["refOrcamento"] = refOrcamento;
                    }
                    catch
                    {
                        lblMessage.Text = "Erro na inserção do Orçamento";
                    }
                }
                else if (btnSubmit.CommandArgument == "update")
                {
                    UpdateBD();
                    btnSubmit.CommandArgument = "update";
                }

                orcamento = null;
            }
        }

        // *****************************************************************************
        // button EVENTS
        // *****************************************************************************

        private void btnNovaVersao_Click(object sender, System.EventArgs e)
        {
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            string idOrcamento = orcamento.InsertOrcamentoNovaVersao(ViewState["idOrcamento"].ToString(), User.Identity.Name.ToString()).ToString();

            orcamento = null;

            if (idOrcamento == "0")
            {
                lblMessage.Text = clsGeral.ErrorMessage.ERR_UPDATE;
            }
            else
            {
                Response.Redirect("FormOrcamentoES.aspx?btn=ORC&id=" + idOrcamento, true);
            }
        }

        private void btnReplica_Click(object sender, System.EventArgs e)
        {
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            string idOrcamento = orcamento.InsertOrcamentoReplica(ViewState["idOrcamento"].ToString(), User.Identity.Name.ToString()).ToString();

            orcamento = null;

            if (idOrcamento == "0")
                lblMessage.Text = clsGeral.ErrorMessage.ERR_UPDATE;
            else
                Response.Redirect("FormOrcamentoES.aspx?btn=ORC&id=" + idOrcamento, true);
        }

        private string sReparacao() //nao posso passar bool porque o crystal reconhece como "Verdadeiro" e "Falso", ja tive isso mas n me lembro onde.
        {
            foreach (DataGridItem dgi in dgLinhasOrcamento.Items)
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

            string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_ORCAMENTO"];

            ReportClass myreport = null; //marteladas mal estruturadas, sem tempo para refazer tudo

            switch (myApp)
            {

                case "ANG_LABMETRO":
                    myreport = new LabMetro.REPORTS_ANG.rptOrcamentoNovo();
                    break;
                case "SON_LABMETRO":
                    myreport = new LabMetro.REPORTS_ANG.rptOrcamentoNovo();
                    break;
                case "ISQ_LABMETRO":
                    myreport = new LabMetro.REPORTS.rptOrcamentoNovo();
                    break;
                case "ES_LABMETRO":
                    myreport = new LabMetro.REPORTS_ES.rptOrcamentoNovo();
                    break;
                case "CV_LABMETRO":
                    myreport = new LabMetro.REPORTS_CV.rptOrcamentoNovo();
                    break;
            }

            clsReport cr = new clsReport();
            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataSet ds = orc.myDsOrcamentoFax(System.Convert.ToInt32(ViewState["idOrcamento"]));

            myreport.SetDataSource(ds);

            DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter adapter = new LabMetro.DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter();

            DataAccessLayer.dlOrcamento.dtOrcamentoComentarioFaxDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idOrcamento"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            //fillCountryRelatedData(); 

            myreport.SetParameterValue("@inFaxNumber", faxNumber);  //de quem recebe!!!!
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

            if (myApp == "ISQ_LABMETRO")
            {
                myreport.SetParameterValue("@inReparacao", sReparacao());
            }


            //cr.faxReport(myreport, faxNumber, mailSender, "ORC", ViewState["refOrcamento"].ToString());
            cr.sendFaxNovo(myreport, faxNumber, "ORC", ViewState["refOrcamento"].ToString());
            myreport.Close();
            myreport.Dispose();
            myreport = null;
            cr = null;
            ds = null;
            orc = null;

            UpdateSetOrcamentoAsEnviado(ViewState["idOrcamento"].ToString());
            Response.Redirect("FormOrcamentoES.aspx?btn=ORC&id=" + ViewState["idOrcamento"].ToString(), true);

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


            string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_ORCAMENTO"];


            ReportClass myreport = null; //marteladas mal estruturadas, sem tempo para refazer tudo

            switch (myApp)
            {
                
                case "ES_LABMETRO":
                    myreport = new LabMetro.REPORTS_ES.rptOrcamentoNovo();
                    break;
               

            }

            clsReport cr = new clsReport();

            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();

            DataSet ds = orc.myDsOrcamentoFax(System.Convert.ToInt32(ViewState["idOrcamento"]));

            myreport.SetDataSource(ds);

            DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter adapter = new LabMetro.DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter();

            DataAccessLayer.dlOrcamento.dtOrcamentoComentarioFaxDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idOrcamento"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", "==por email==");
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

          

            string mailBody = "Buenos días:" + "\r\n" + "\r\n" +"De acuerdo con su solicitud, adjunto le enviamos la oferta para la calibración de los equipos con  referencia " + lblRefOrcamento.Text + "." + "\r\n" + "\r\n" + "Estaremos disponibles para cualquier aclaración que necesiten."+ "\r\n" + "\r\n" + "Un Saludo" + "\r\n" + "\r\n" + "Labmetro, SL";

            cr.mailReport(myreport, emailAddress, mailSender, "Envio de Oferta", "ORC", ViewState["refOrcamento"].ToString(), mailBody,"","");


            myreport.Close();
            myreport.Dispose();
            myreport = null;
            cr = null;
            ds = null;
            orc = null;

            UpdateSetOrcamentoAsEnviado(ViewState["idOrcamento"].ToString());

            Response.Redirect("FormOrcamentoES.aspx?btn=ORC&id=" + ViewState["idOrcamento"].ToString(), true);

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

            if (ViewState["idOrcamento"] == null)
            {
                resetTotais();
            }
        }

        private void ddContacto_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillContactInfo(ddContacto.SelectedValue);
        }

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
            ddCondicoesPagamento.Items.Insert(0, new ListItem("", ""));
        }


        private void FillDropDowns()
        {
            fillResponsaveisTecnicos();
            fillEstados();
            fillLocaisExecucao();
            fillCondicoesPagamento();
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


        private DropDownList fillEstadoOrcamentoLinha(DropDownList ddEstadoOrcamentoLinha)
        {
            string strSQL = "SELECT idEstadoOrcamentoLinha, descricao from EstadoOrcamentoLinha";
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddEstadoOrcamentoLinha.DataSource = DR;
            ddEstadoOrcamentoLinha.DataBind();
            DR.Close();

            return ddEstadoOrcamentoLinha;
        }

        private DropDownList fillRazaoOrcamentoLinha(DropDownList ddRazaoOrcamentoLinha)
        {
            string strSQL = "SELECT idRazaoOrcamentoLinha, descricao from RazaoOrcamentoLinha";
            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddRazaoOrcamentoLinha.DataSource = DR;
            ddRazaoOrcamentoLinha.DataBind();
            DR.Close();
            return ddRazaoOrcamentoLinha;
        }

        private DropDownList fillComentariosTipo(DropDownList ddComentarioTipo)
        {
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();
            //DATATABLE
            ddComentarioTipo.DataSource = orcamento.FillComentariosTipo("", "");
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
            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataTable dt = orc.DTEquipamentosDeOrcamento(txtPesquisaEquipamento.Text, txtPesquisaEmpresa.Text);
            DGEq.DataSource = dt;
            DGEq.DataBind();
        }

        private void UpdateSetOrcamentoAsEnviado(string idOrcamento)
        {
            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            int i = orc.setOrcamentoAsEnviado(idOrcamento, User.Identity.Name.ToString());
        }


        private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
        {
            FillEmpresasContactos();

            if (ViewState["idOrcamento"] == null)
            {
                resetTotais();
            }
        }

        private void txtPesqEmpresa_TextChanged(object sender, System.EventArgs e)
        {
            FillEmpresasContactos();

            if (ViewState["idOrcamento"] == null)
            {
                resetTotais();
            }
        }

        private void btnEmpresas_Click(object sender, System.EventArgs e)
        {
            FillEmpresasContactos();

            if (ViewState["idOrcamento"] == null)
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

            ReportClass myreport = null; //marteladas mal estruturadas, sem tempo para refazer tudo

            switch (myApp)
            {
                case "ANG_LABMETRO":
                    myreport = new LabMetro.REPORTS_ANG.rptOrcamentoNovo();
                    break;
                case "ISQ_LABMETRO":
                    myreport = new LabMetro.REPORTS.rptOrcamentoNovo();
                    break;
                case "ES_LABMETRO":
                    myreport = new LabMetro.REPORTS_ES.rptOrcamentoNovo();
                    break;
                case "CV_LABMETRO":
                    myreport = new LabMetro.REPORTS_CV.rptOrcamentoNovo();
                    break;
                case "BR_LABMETRO":
                    //myreport = new LabMetro.REPORTS_BR.rptOrcamentoNovo(); descomentar
                    break;
            }

            clsReport cr = new clsReport();

            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataSet ds = orc.myDsOrcamentoFax(System.Convert.ToInt32(ViewState["idOrcamento"]));
            myreport.SetDataSource(ds);

            DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter adapter = new LabMetro.DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter();

            DataAccessLayer.dlOrcamento.dtOrcamentoComentarioFaxDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idOrcamento"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", faxNumber);  //de quem recebe!!!!
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

            


            ds = null;
            orc = null;
            cr.exportReportToPDF(myreport, ViewState["refOrcamento"].ToString());
           
        }

        private void btnVerMail_Click(object sender, System.EventArgs e)
        {
            ReportClass myreport = null; //marteladas mal estruturadas, sem tempo para refazer tudo

            switch (myApp)
            {
                case "ANG_LABMETRO":
                    myreport = new LabMetro.REPORTS_ANG.rptOrcamentoNovo();
                    break;
                case "ISQ_LABMETRO":
                    myreport = new LabMetro.REPORTS.rptOrcamentoNovo();
                    break;
                case "ES_LABMETRO":
                    myreport = new LabMetro.REPORTS_ES.rptOrcamentoNovo();
                    break;
                case "CV_LABMETRO":
                    myreport = new LabMetro.REPORTS_CV.rptOrcamentoNovo();
                    break;

                case "BR_LABMETRO":
                   // myreport = new LabMetro.REPORTS_BR.rptOrcamentoNovo();
                    break;
            }

            clsReport cr = new clsReport();

            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataSet ds = orc.myDsOrcamentoFax(System.Convert.ToInt32(ViewState["idOrcamento"]));
            myreport.SetDataSource(ds);

            DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter adapter = new LabMetro.DataAccessLayer.dlOrcamentoTableAdapters.dtOrcamentoComentarioFaxTableAdapter();

            DataAccessLayer.dlOrcamento.dtOrcamentoComentarioFaxDataTable dt = adapter.GetData(System.Convert.ToInt32(ViewState["idOrcamento"]));

            DataTable DT = dt; //pq passar directamente diz-me que o nome é dubio....

            myreport.Subreports[0].SetDataSource(DT); //muito importante!!!!

            myreport.SetParameterValue("@inFaxNumber", "==por email==");
            myreport.SetParameterValue("@inFaxOrigem", (string)ConfigurationManager.AppSettings["Fax_Orcamento"]);

            if (myApp == "ISQ_LABMETRO")
            {
                myreport.SetParameterValue("@inReparacao", sReparacao());
            }


            ds = null;
            orc = null;

            cr.exportReportToPDF(myreport, ViewState["refOrcamento"].ToString());
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

                if (Request.QueryString["id"] == null)
                {
                    ddCondicoesPagamento.SelectedValue = detEmpresa.idCondicoesPagamento;
                }
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
                        lblMessage.Text = "Ya existe un fichero con el mismo nombre.";
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

            foreach (DataGridItem dgi in dgLinhasOrcamento.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("chkSelected");
                if (myCheckBox.Checked == true)
                {
                    strIds += dgLinhasOrcamento.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter);

            string strSQL = "UPDATE OrcamentoLinha SET idEstadoOrcamentoLinha = 2 WHERE idOrcamentoLinha IN (" + strIds + ")";
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());

        }



        //mesmo codigo que acima, podia fazer uma funcao dps para ficar mais bonito
        protected void btnRejeitarTodos_Click(object sender, EventArgs e)
        {
            string strIds = "";

            foreach (DataGridItem dgi in dgLinhasOrcamento.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("chkSelected");
                if (myCheckBox.Checked == true)
                {
                    strIds += dgLinhasOrcamento.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter);

            string strSQL = "UPDATE OrcamentoLinha SET idEstadoOrcamentoLinha = 3 WHERE idOrcamentoLinha IN (" + strIds + ")";
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            BindGridLinhasOrcamento(ViewState["idOrcamento"].ToString());
        }

        protected void fillPrice(object sender, EventArgs e)
        {

            DropDownList list = (DropDownList)sender;
            //Response.Write(list.SelectedValue);

            double preco = 0;
            if (list.SelectedValue != "")
            {
                string strSQL = "select isnull(preco,0) from tbPrecosDirectos where idTipoEquipamento = " + list.SelectedValue;

                preco = System.Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL));
            }
            


            //TableCell cell = list.Parent as TableCell;
            //DataGridItem item = cell.Parent as DataGridItem;

            ////int i = item.ItemIndex;

            //int dgIndex = dgLinhasOrcamento.EditItemIndex; 

            //FOOTER IS NOT IN THE ITEMS COLLECTION!!! access the footer through controls[0] - tb nao é bem assim


            
            //TextBox txtPreco = (TextBox) dgLinhasOrcamento.Controls[0].FindControl("txtValorUnitario");

            ////Footer
            //DataGridItem footer =  (DataGridItem)dgLinhasOrcamento.Controls[0].Controls[Controls[0].Controls.Count - 1];


            //TextBox txtPreco = (TextBox) footer.FindControl("txtValorUnitario");
            //txtPreco.Text =   preco.ToString();


            int footerIndex = dgLinhasOrcamento.Controls[0].Controls.Count - 1;
           TextBox t = (TextBox)dgLinhasOrcamento.Controls[0].Controls[footerIndex].FindControl("txtValorUnitario");
          
               t.Text = preco.ToString();
            //   if (t != null)
            //{
            //    Response.Write("Found footer t in SelectedIndexChanged event<br>");
            //    Response.Write(FindUtil.DumpParents(t));
            //}


            
            
        }

        protected void fillPriceEdit(object sender, EventArgs e)
        {

            DropDownList list = (DropDownList)sender;
            //Response.Write(list.SelectedValue);

            double preco = 0;
            if (list.SelectedValue != "")
            {
                string strSQL = "select isnull(preco,0) from tbPrecosDirectos where idTipoEquipamento = " + list.SelectedValue;

                preco = System.Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL));
            }
            

                int dgIndex = dgLinhasOrcamento.EditItemIndex;
                DataGridItem dgi = (DataGridItem)list.Parent.Parent;

                TextBox t = (TextBox)dgi.Cells[0].FindControl("txtValorUnitarioEdit");

                t.Text = preco.ToString();
                //   if (t != null)
                //{
                //    Response.Write("Found footer t in SelectedIndexChanged event<br>");
                //    Response.Write(FindUtil.DumpParents(t));
                //}

            



        }

        public class FindUtil
        {
            public static string DumpParents(Control c)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(c.ID + " (" + c.GetType().ToString() + ")");

                while (c.Parent != null)
                {
                    c = c.Parent;
                    sb.Append(" -><br>");
                    sb.Append(c.ID + " (" + c.GetType().ToString() + ")");
                }

                return sb.ToString();
            }
        }
    }
}

