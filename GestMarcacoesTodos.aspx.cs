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
    public partial class GestMarcacoesTodos : System.Web.UI.Page
    {

        protected System.Web.UI.WebControls.TextBox txtEmpresa;
        protected System.Web.UI.WebControls.PlaceHolder menuPlaceHolder;
        protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;
        protected System.Web.UI.WebControls.TextBox Textbox1;
        protected System.Web.UI.WebControls.Button btnContinuar;
        protected System.Web.UI.WebControls.LinkButton Linkbutton4;
        protected System.Web.UI.WebControls.LinkButton Linkbutton5;
        protected System.Web.UI.WebControls.CheckBox cbItemConcessionario;
        protected System.Web.UI.WebControls.CheckBox cbItemCTA;
        protected System.Web.UI.WebControls.CheckBox cbItemCentroB;
        protected System.Web.UI.WebControls.LinkButton btnBREs;




        DataView DV;
        protected System.Web.UI.WebControls.DropDownList Dropdownlist1;

        private const string ID_PAG = "MARCACOES_1";//NOME DA PAGINA
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

                    if (!Page.IsPostBack)
                    {
                        //defaultSort das empresas
                        ViewState["sortFieldEmpresa"] = "nome";
                        ViewState["sortFieldContacto"] = "nome";
                        ViewState["sortFieldEqu"] = "tipoEquipamento,numSerie";
                        ViewState["sortDirection"] = "ASC";
                        ViewState["sortDirectionEqu"] = "ASC";
                        ViewState["sortDirectionReq"] = "DESC";
                        ViewState["sortFieldReq"] = "idRequisicao";
                        ViewState["sortDirectionOrc"] = "DESC";
                        ViewState["sortFieldOrc"] = "idOrcamento";
                        lblLegenda.Visible = false;
                        fillDDs();
                        fillDDGrandezas(); //para filtrar Equipamentos

                    }
                }
            }
        }


        private void fillDDGrandezas()
        {
            // Fill da DropDownList das Grandezas
            DATA.ListasBD listaGrandezas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = listaGrandezas.DRListaGrandezas();
            ddGrandeza.DataSource = DR;
            ddGrandeza.DataBind();
            ddGrandeza.Items.Insert(0, new ListItem("", ""));
            DR.Close();
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
            btnFiltrarEquips.Click += new System.EventHandler(btnFiltrarEquips_Click);
            dgOrcamentos.ItemCommand += new DataGridCommandEventHandler(dgOrcItemCommand);
            btnOrcamentos.Click += new System.EventHandler(btnOrcamentos_Click);


            btnContactos.Click += new System.EventHandler(btnContactos_Click);
            btnEquipamentos.Click += new System.EventHandler(btnEquipamentos_Click);
            btnMarcacoes.Click += new System.EventHandler(btnMarcacoes_Click);

            btnRequisicao.Click += new System.EventHandler(btnRequisicao_Click);


            btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);


            DGEmpresas.SelectedIndexChanged += new System.EventHandler(dgEmpresas_SelectedIndexChanged);
            btnFazerMarcacao.Click += new System.EventHandler(btnFazerMarcacao_Click);




            cbEnviarFax.CheckedChanged += new System.EventHandler(cbEnviarFax_CheckedChanged);
            cbEnviarMail.CheckedChanged += new System.EventHandler(cbEnviarMail_CheckedChanged);


            bntC.Click += new System.EventHandler(bntC_Click);
            bntE.Click += new System.EventHandler(bntE_Click);
            btnV.Click += new System.EventHandler(btnV_Click);
        }


        #endregion



        //******************************************************************************
        //********** FUNÇÕSE SOBRE O DATAGRID EMPRESAS ********************************
        //******************************************************************************

        //===================================================================
        //EDITA DATAGRID EMPRESAS
        //===================================================================
        protected void editEmpresa(Object sender, DataGridCommandEventArgs e)
        {
            DGEmpresas.ShowFooter = false;
            DGEmpresas.EditItemIndex = e.Item.ItemIndex;
            BindGridEmpresas();
        }

        //===================================================================
        //CANCELA EDIÇÃO DE DATAGRID EMPRESAS
        //===================================================================
        protected void cancelEmpresa(Object sender, DataGridCommandEventArgs e)
        {
            DGEmpresas.ShowFooter = true;
            DGEmpresas.EditItemIndex = -1;
            BindGridEmpresas();
        }

        //===================================================================
        //ALTERA DADOS DE EMPRESA
        //===================================================================
        protected void updateEmpresa(Object sender, DataGridCommandEventArgs e)
        {
            string idEmpresa = DGEmpresas.DataKeys[e.Item.ItemIndex].ToString();



            TextBox tipoContrato = (TextBox)e.Item.FindControl("txtEditTipoContrato");
            TextBox dtUltimaVisita = (TextBox)e.Item.FindControl("txtEditDtUltimaVisita");
            CheckBox cbEditConcessionario = (CheckBox)e.Item.FindControl("cbEditConcessionario");
            CheckBox cbEditCTA = (CheckBox)e.Item.FindControl("cbEditCTA");
            CheckBox cbEditCentroB = (CheckBox)e.Item.FindControl("cbEditCentroB");
            CheckBox cbEditSistemaPesagem = (CheckBox)e.Item.FindControl("cbEditSistemaPesagem");
            CheckBox cbAGE = (CheckBox)e.Item.FindControl("cbEditAGE");

            TextBox telefone = (TextBox)e.Item.FindControl("txtEditTelefone");
            TextBox fax = (TextBox)e.Item.FindControl("txtEditFax");
            TextBox email = (TextBox)e.Item.FindControl("txtEditEmail");


            DATA.EmpresasBD emp = new LabMetro.DATA.EmpresasBD();

            int i = emp.UpdateEmpresaConc(idEmpresa, tipoContrato.Text, dtUltimaVisita.Text, cbEditCTA.Checked.ToString(), cbEditConcessionario.Checked.ToString(), cbEditSistemaPesagem.Checked.ToString(), cbEditCentroB.Checked.ToString(), telefone.Text, email.Text, fax.Text, cbAGE.Checked.ToString());


            emp = null;

            if (i > 0)
            {
                lblMessage.Text = "Alteração efectuada";
                DGEmpresas.EditItemIndex = -1;
                DGEmpresas.ShowFooter = true;
                BindGridEmpresas();
            }
            else
            {
                lblMessage.Text = "Alteração não efectuada";
            }



        }

        //===================================================================
        //PAGINAÇÃO DO DATAGRID EMPRESAS
        //===================================================================
        public void DoPagingEmpresas(Object s, DataGridPageChangedEventArgs e)
        {
            DGEmpresas.CurrentPageIndex = e.NewPageIndex;
            BindGridEmpresas();

        }

        //===================================================================
        //ORDENAÇÃO DO DATAGRID EMPREAS
        //===================================================================
        public void SortGridEmpresas(Object s, DataGridSortCommandEventArgs e)
        {
            switch (ViewState["sortDirection"].ToString())
            {
                case "ASC":
                    ViewState["sortDirection"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirection"] = "ASC";
                    break;
            }
            ViewState["sortFieldEmpresa"] = e.SortExpression;
            BindGridEmpresas();
        }

        //===================================================================
        //BIND GRID EMPRESAS
        //===================================================================
        private void BindGridEmpresas()
        {

            DataSet ds = DSEmpresas();

            if (ds != null)
            {
                DV = new DataView(ds.Tables["Empresa"]);
                DV.Sort = ViewState["sortFieldEmpresa"].ToString() + " " + ViewState["sortDirection"];
                DGEmpresas.DataSource = DV;
                DGEmpresas.DataBind();

                if (DV.Table.Rows.Count > 0)
                {
                    DGEmpresas.Visible = true;
                }
                else
                {
                    DGEmpresas.Dispose();
                    DGEmpresas.Visible = false;
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
                }
            }
        }


        //===================================================================
        //DATASET COM DADOS DAS EMPREASAS
        //===================================================================
        private DataSet DSEmpresas()
        {
            //DataSet ds = new DATASETS.DSEmpresaNew(); 
            DataSet ds = new DATASETS.DSEmpresa();

            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inNome", txtNomeEmpresa.Text);
            arrParams[1] = new SqlParameter("@inNif", txtNIF.Text);
            arrParams[2] = new SqlParameter("@inNumCliente", txtNumClienteSAP.Text);

            ds.EnforceConstraints = false;  //muito importante, senão dá me um erro no fill!!!!

            try
            {
                ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListEmpresasMarcacoesTodos", ds, "Empresa", arrParams);
                return ds;
            }
            catch
            {
                return null;
            }
        }



        //******************************************************************************
        //********** FUNÇÕSE SOBRE O DATAGRID CONTACTOS ********************************
        //******************************************************************************

        //===================================================================
        //EDITAR GRID CONTACTOS
        //===================================================================
        protected void editContacto(Object sender, DataGridCommandEventArgs e)
        {
            dgContactos.ShowFooter = false;
            dgContactos.EditItemIndex = e.Item.ItemIndex;
            BindGridContactos();
        }

        //===================================================================
        //CANCELAR EDIÇÃO DE GRID CONTACTOS
        //===================================================================
        protected void cancelContacto(Object sender, DataGridCommandEventArgs e)
        {
            dgContactos.ShowFooter = true;
            dgContactos.EditItemIndex = -1;
            BindGridContactos();
        }

        //===================================================================
        //ALTERAR DADOS DE CONTACTO
        //===================================================================
        protected void updateContacto(Object sender, DataGridCommandEventArgs e)
        {
            //string idContacto = dgContactos.DataKeys[e.Item.ItemIndex].ToString();

        }

        //===================================================================
        //BIND GRID CONTACTOS
        //===================================================================
        private void BindGridContactos()
        {
            if (DGEmpresas.SelectedIndex < 0)
            {
                lblMessage.Text = "Seleccione a empresa.";
                return;
            }

            dgContactos.SelectedIndex = -1;
            DATA.ContactosBD contactos = new LabMetro.DATA.ContactosBD();
            dgContactos.DataSource = contactos.DTFillContacts(DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString(), null, "1", null);
            dgContactos.DataBind();
            contactos = null;
        }

        //===================================================================
        //PAGINAÇÃO GRID CONTACTOS
        //===================================================================
        public void DoPagingContactos(Object s, DataGridPageChangedEventArgs e)
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
                    ViewState["sortDirection"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirection"] = "ASC";
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
            if (DGEmpresas.SelectedIndex < 0)
            {
                lblMessage.Text = "Seleccione a empresa.";
                return;
            }
            string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();

            DATA.EquipamentoBD equip = new LabMetro.DATA.EquipamentoBD();
            try
            {

                DataTable DT = equip.DTEquipamento(idEmpresa, null, null, null);
                DataView DV = new DataView(DT); //,"",strSort,DataViewRowState.CurrentRows); 
                string rowFilter = "Estado = 1 ";
                if (ddGrandeza.SelectedValue != "") rowFilter += " and idgrandeza = '" + ddGrandeza.SelectedValue + "'";
                DV.RowFilter = rowFilter;
                DV.Sort = ViewState["sortFieldEqu"].ToString() + " " + ViewState["sortDirectionEqu"];


                DGEquipamentos.DataSource = DV;
                DGEquipamentos.DataBind();


                if (DV.Table.Rows.Count > 0)
                {
                    DGEquipamentos.Visible = true;
                    lblLegenda.Visible = true;
                }
                else
                {
                    DGEquipamentos.Dispose();
                    DGEquipamentos.Visible = false;
                    lblLegenda.Visible = false;
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;

                }
            }
            catch (Exception ex)
            {

                DGEquipamentos.Dispose();
                DGEquipamentos.Visible = false;
                lblLegenda.Visible = false;
                lblMessage.Text = ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
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
                    ViewState["sortDirectionEqu"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirectionEqu"] = "ASC";
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
            DGEquipamentos.ShowFooter = false;
            DGEquipamentos.EditItemIndex = e.Item.ItemIndex;
            BindGridEquipamentos();
        }

        //============================================================================	
        //CANCELA A EDIÇÃO DOS DADOS DE UM EQUIPAMENTO
        //============================================================================	
        protected void cancelGridEquipamento(Object sender, DataGridCommandEventArgs e)
        {
            DGEquipamentos.ShowFooter = true;
            DGEquipamentos.EditItemIndex = -1;
            BindGridEquipamentos();
        }

        //============================================================================	
        //ALTERA OS DADOS DE UM EQUIPAMENTO
        //============================================================================	
        protected void updateEquipamento(Object sender, DataGridCommandEventArgs e)
        {
            string idEquipamento = DGEquipamentos.DataKeys[e.Item.ItemIndex].ToString();

            TextBox periodCalib = (TextBox)e.Item.FindControl("txtPC");
            TextBox refUltCalib = (TextBox)e.Item.FindControl("txtRUC");
            TextBox dtUltCalib = (TextBox)e.Item.FindControl("txtDUC");
            TextBox obs = (TextBox)e.Item.FindControl("txtObs");

            if (Session["UserId"] == null) Session["UserId"] = "0";//Response.Redirect("../Default.aspx",true); 

            try
            {
                DATA.EquipamentoBD eq = new LabMetro.DATA.EquipamentoBD();

                int i = eq.updateEquipamentoInBOConc(periodCalib.Text, refUltCalib.Text, dtUltCalib.Text, Session["UserId"].ToString(), idEquipamento, obs.Text);  //pode devolver mais que 1 por causa do trigger na bd. 
                if (i > 0) lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
                else lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
                eq = null;
                DGEquipamentos.EditItemIndex = -1;
                DGEquipamentos.ShowFooter = true;
                BindGridEquipamentos();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void DGEmpresas_DataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                LinkButton button = (LinkButton)e.Item.Cells[15].Controls[0];
                button.CausesValidation = false;
            }
        }


        //====================================================================================
        //ITEM DATABOUND DO DATAGRID EQUIPAMENTOS
        //====================================================================================

        protected void DGEquipamentos_DataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                LinkButton b = (LinkButton)e.Item.Cells[12].Controls[0];
                b.CausesValidation = false;

                DataRowView DRV = (DataRowView)e.Item.DataItem;

                TextBox periodCalib = (TextBox)e.Item.FindControl("txtPC");
                periodCalib.Text = DRV["periodicidadeCalibracao"].ToString();

                TextBox refUltCalib = (TextBox)e.Item.FindControl("txtRUC");
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


            DGEmpresas.CurrentPageIndex = 0;
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
                lblLegenda.Visible = false;
                btnEquipamentos.Text = "Ver Equip.";
            }
            else
            {
                BindGridEquipamentos();
                if (DGEquipamentos.Items.Count == 0)
                {
                    DGEquipamentos.Visible = false;
                    lblLegenda.Visible = false;
                }
                else
                {
                    DGEquipamentos.Visible = true;
                    lblLegenda.Visible = true;
                    btnEquipamentos.Text = "Ocultar Equip.";
                }
            }
        }


        //=========================================================================================
        //==========actualizar equipamentos com data ultima visita à empresa
        //=========================================================================================


        private void dgEmpresas_SelectedIndexChanged(object sender, System.EventArgs e)

        {
            btnContactos.Text = "Contactos";
            btnEquipamentos.Text = "Equipamentos";
            btnMarcacoes.Text = "Marcações";
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

            dgOrcamentos.SelectedIndex = 0;
            dgOrcamentos.DataSource = null;
            dgOrcamentos.DataBind();

            bindDDRequisicao();
            bindDDOrcamento();

            txtMorada.Text = DGEmpresas.Items[DGEmpresas.SelectedIndex].Cells[5].Text.ToString();
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
            if (DGEmpresas.SelectedIndex < 0)
            {
                lblMessage.Text = "Seleccione a empresa.";
                return;
            }
            string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();

            string strSQL = "SELECT Empresa.nomeAbreviado AS empresa, Funcionario.nomeAbreviado AS funcionario, BRE.dtBRE AS dtBre, cast(BRE.numBRE as varchar) + '/' +  substring (  CAST(BRE.ano as varchar),3,2) AS refBRE, bre.idBRE, Marcacao.refMarcacao AS refmarcacao, Marcacao.obsInternas AS obs, Marcacao.dtMarcacao, Marcacao.idMarcacao FROM Marcacao INNER JOIN BRE ON Marcacao.idBre = BRE.idBRE INNER JOIN   Funcionario ON Marcacao.idFuncionarioTecnico = Funcionario.idFuncionario INNER JOIN Empresa ON Marcacao.idEmpresa = Empresa.idEmpresa and Empresa.idEmpresa = " + idEmpresa;

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

            ddRequisicao.DataSource = requisicao.DRGetRequisicoesIncompletasByEmpresa(idEmpresa);

            ddRequisicao.DataBind();
            ddRequisicao.Items.Insert(0, new ListItem("", ""));

            requisicao = null;
        }

        //=================================================================================================
        //FILL DA DD orçamentos PARA ASSOCIAR À MARCAÇÃO
        //=================================================================================================
        private void bindDDOrcamento()
        {

            ddOrcamento.Items.Clear();

            string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            ddOrcamento.DataSource = orcamento.DRGetOrcamentosByEmpresa(idEmpresa);

            ddOrcamento.DataBind();
            ddOrcamento.Items.Insert(0, new ListItem("", ""));

            orcamento = null;
        }
        //=================================================================================================
        //ACCÇÃO QUE INSERE UMA MARCAÇÃO E UM RESPECTIVO "BRE DE CALIBRAÇÃO EXTERNA"
        //=================================================================================================
        private void fazerMarcacao()
        {

            string dtStart = txtDataProximaVisita.Text.TrimEnd();
            string dtFim = txtDataUltDiaMarcacao.Text;
            if (dtFim == "") dtFim = txtDataProximaVisita.Text;

            string t1 = DateTime.Parse(dtStart).ToString();

            string t2 = DateTime.Now.ToString(dtFim);

            if (dgContactos.SelectedIndex < 0)
            {
                lblMessage.Text = "Tem de esolher um contacto (com fax ou email, conforme necessário.)";
                return;
            }

            if (DateTime.Compare(DateTime.Parse(t1), DateTime.Parse(t2)) > 0)
            {
                lblMessage.Text = "A data de fim de ser superior à data de início da marcação";
                return;
            }

            string idEmpresa;
            string idsEquipsCalibracao = null;
            string idsEquipsEnsaio = null;
            string idsEquipsVerificacao = null;

            string idFuncionario = "";

            //DATA.GeralBD geral = new LabMetro.DATA.GeralBD();
            try
            {
                idFuncionario = System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));
            }
            catch
            { }

            //geral = null; 

            string idRequisicao;
            int idBRE;

            idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();
            if (idEmpresa == "")
            {
                lblMessage.Text = "Seleccione a empresa.";
                return;
            }

            idRequisicao = ddRequisicao.SelectedValue;

            foreach (DataGridItem dgi in DGEquipamentos.Items)
            {
                CheckBox cbc = (CheckBox)dgi.Cells[0].FindControl("cbCalibracao");
                CheckBox cbe = (CheckBox)dgi.Cells[0].FindControl("cbEnsaio");
                CheckBox cbv = (CheckBox)dgi.Cells[0].FindControl("cbVerificacao");

                if (cbc.Checked == true)
                {

                    idsEquipsCalibracao += DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
                    idsEquipsCalibracao += ",";
                }
                if (cbe.Checked == true)
                {

                    idsEquipsEnsaio += DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
                    idsEquipsEnsaio += ",";
                }
                if (cbv.Checked == true)
                {

                    idsEquipsVerificacao += DGEquipamentos.DataKeys[dgi.ItemIndex].ToString();
                    idsEquipsVerificacao += ",";
                }
            }

            if (idsEquipsCalibracao != null) idsEquipsCalibracao = idsEquipsCalibracao.TrimEnd(",".ToCharArray());
            if (idsEquipsEnsaio != null) idsEquipsEnsaio = idsEquipsEnsaio.TrimEnd(",".ToCharArray());
            if (idsEquipsVerificacao != null) idsEquipsVerificacao = idsEquipsVerificacao.TrimEnd(",".ToCharArray());

            //*****************************************************************
            //=================================================================
            //passar queries para stored procedures depois!
            //=================================================================
            //*****************************************************************

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                int idMarcacao = 0; //para depois

                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                objConn.Open();
                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;

                    try
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        DATA.BreBD bre = new DATA.BreBD();

                        idBRE = bre.InsertBreConnn(objConn, objCmd, idEmpresa, idFuncionario, "0", ddTecnicoExterior.SelectedItem.Text, txtObservacoes.Text, HttpContext.Current.User.Identity.Name.ToString(), "0", "", ddEmpresaContratante.SelectedValue, "", "", "", rbEmpbrepodevercertificados.SelectedValue.ToString(),"");


                        objCmd.CommandType = CommandType.Text;

                        if (objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();

                        SqlParameter param = new SqlParameter("@dtFim", dtFim);
                        SqlParameter param2 = new SqlParameter("@dtStart", dtStart);
                        SqlParameter param3 = new SqlParameter("@idRequisicao", idRequisicao);
                        SqlParameter param4 = new SqlParameter("@idOrcamento", ddOrcamento.SelectedValue);
                        SqlParameter param5 = new SqlParameter("@idTecnico2", ddTecnicoExterior2.SelectedValue);

                        SqlParameter param6 = new SqlParameter("@idContacto", dgContactos.DataKeys[dgContactos.SelectedIndex].ToString());



                        if (param3.Value.ToString() == "") param3.Value = DBNull.Value;
                        if (param4.Value.ToString() == "") param4.Value = DBNull.Value;
                        if (param5.Value.ToString() == "") param5.Value = DBNull.Value;
                        if (param6.Value.ToString() == "") param6.Value = DBNull.Value;


                        objCmd.CommandText = "INSERT INTO Marcacao([idEmpresa],[dtMarcacao],[idBre],[refMarcacao],[idUtilCriacao],[dtCriacao],[idUtilAlteracao],[dtAlteracao],[obsInternas],[idFuncionarioTecnico], dtFimMarcacao, idRequisicao, idGrandeza,idFuncionarioTecnico2,idOrcamento,bGeral,idContacto,periodoMarcacao, obsCliente, moradaMarcacao)VALUES (" + idEmpresa + ",@dtStart," + idBRE.ToString() + ",null," + Session["UserId"].ToString() + ",getDate()," + Session["UserId"].ToString() + ", getDate(),'" + txtObservacoes.Text + "'," + ddTecnicoExterior.SelectedValue.ToString() + ",@dtFim,@idRequisicao,null,@idTecnico2, @idOrcamento,1,@idContacto,'" + txtPeriodoMarcacao.Text + "', '" + txtObsCliente.Text + "','" + txtMorada.Text + "')";

                        objCmd.Parameters.Add(param);
                        objCmd.Parameters.Add(param2);
                        objCmd.Parameters.Add(param3);
                        objCmd.Parameters.Add(param4);
                        objCmd.Parameters.Add(param5);
                        objCmd.Parameters.Add(param6); //nao pode ser null ....

                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "SELECT @@IDENTITY"; //retorna um decimal (ou null)
                        idMarcacao = Decimal.ToInt32((decimal)objCmd.ExecuteScalar());


                        objCmd.CommandType = CommandType.StoredProcedure;

                        //******************************************************************
                        //passar para funcao DEPOIS, para classe propria (ConcessionariosBD)
                        //com mais tempo.
                        //******************************************************************

                        char[] delimiter = ",".ToCharArray();

                        if (idsEquipsCalibracao != null)
                        {

                            idsEquipsCalibracao = idsEquipsCalibracao.TrimEnd(delimiter);
                            string[] idsC = idsEquipsCalibracao.Split(delimiter);

                            foreach (string idEquipamento in idsC)
                            {
                                if (objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();
                                SqlParameter[] arrParams = new SqlParameter[12];

                                arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
                                arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao); //passar idRequisicao, se existe
                                arrParams[2] = new SqlParameter("@inIdEquipamento", idEquipamento);
                                arrParams[3] = new SqlParameter("@inIdEstadoServico", "2");//aguarda calib.ext.
                                arrParams[4] = new SqlParameter("@inIdLocalCalibracao", ddLocalCalibracao.SelectedValue);
                                arrParams[5] = new SqlParameter("@inIdTipoServico", "C");//aqui é Calibracao
                                arrParams[6] = new SqlParameter("@inValor", "0");
                                arrParams[7] = new SqlParameter("@inPercDesconto", "0");
                                arrParams[8] = new SqlParameter("@inValorFinal", "0");
                                arrParams[9] = new SqlParameter("@inCalibracaoExterna", "1");//sempre
                                arrParams[10] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
                                arrParams[11] = new SqlParameter("@inBDefinitivo", "0"); //sempre


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


                        if (idsEquipsVerificacao != null)
                        {

                            idsEquipsVerificacao = idsEquipsVerificacao.TrimEnd(delimiter);
                            string[] idsV = idsEquipsVerificacao.Split(delimiter);

                            foreach (string idEquipamento in idsV)
                            {
                                if (objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();
                                SqlParameter[] arrParams = new SqlParameter[12];

                                arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
                                arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao); //passar idRequisicao, se existe
                                arrParams[2] = new SqlParameter("@inIdEquipamento", idEquipamento);
                                arrParams[3] = new SqlParameter("@inIdEstadoServico", "2");//aguarda calib.ext.
                                arrParams[4] = new SqlParameter("@inIdLocalCalibracao", ddLocalCalibracao.SelectedValue);
                                arrParams[5] = new SqlParameter("@inIdTipoServico", "V");//aqui é Calibracao
                                arrParams[6] = new SqlParameter("@inValor", "0");
                                arrParams[7] = new SqlParameter("@inPercDesconto", "0");
                                arrParams[8] = new SqlParameter("@inValorFinal", "0");
                                arrParams[9] = new SqlParameter("@inCalibracaoExterna", "1");//sempre

                                arrParams[10] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
                                arrParams[11] = new SqlParameter("@inBDefinitivo", "0"); //sempre


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

                        if (idsEquipsEnsaio != null)
                        {
                            idsEquipsEnsaio = idsEquipsEnsaio.TrimEnd(delimiter);
                            string[] idsE = idsEquipsEnsaio.Split(delimiter);

                            foreach (string idEquipamento in idsE)
                            {
                                if (objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();
                                SqlParameter[] arrParams = new SqlParameter[12];

                                arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
                                arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao);
                                arrParams[2] = new SqlParameter("@inIdEquipamento", idEquipamento);
                                arrParams[3] = new SqlParameter("@inIdEstadoServico", "2");//aguarda calib.ext.
                                arrParams[4] = new SqlParameter("@inIdLocalCalibracao", ddLocalCalibracao.SelectedValue);
                                arrParams[5] = new SqlParameter("@inIdTipoServico", "E");//aqui é ENSAIO
                                arrParams[6] = new SqlParameter("@inValor", "0");
                                arrParams[7] = new SqlParameter("@inPercDesconto", "0");
                                arrParams[8] = new SqlParameter("@inValorFinal", "0");
                                arrParams[9] = new SqlParameter("@inCalibracaoExterna", "1");//sempre

                                arrParams[10] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
                                arrParams[11] = new SqlParameter("@inBDefinitivo", "0"); //sempre

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

                        //objCmd.CommandText ="UPDATE EMPRESA	set dtUltimaVisita = '"+txtDataProximaVisita.Text+"' WHERE idEmpresa =" +idEmpresa; 

                        //objCmd.ExecuteNonQuery();
                        //int i = objCmd.ExecuteNonQuery();
                        //Response.Write(i.ToString()); 

                        objTrans.Commit();


                        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_CONFIRMACAO_MARCACAO;
                        BindGridEmpresas();//para actualizar data da ultima marcacao



                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            objTrans.Rollback();
                        }
                        catch (Exception excep)
                        {
                            GERAL.clsWriteError.WriteLog(excep);
                            lblMessage.Text += excep.Message.ToString() + "<br />";
                        }
                        GERAL.clsWriteError.WriteLog(ex);
                        lblMessage.Text += ex.Message.ToString() + "<br />";
                    }

                    try
                    {
                        //novo, aqui!
                        if (cbEnviarFax.Checked)
                        {
                            if (idMarcacao != 0)
                            {
                                enviarMarcacao(idMarcacao.ToString(), "fax");
                            }
                        }
                        //novo, aqui!
                        if (cbEnviarMail.Checked)
                        {
                            if (idMarcacao != 0)
                            {
                                enviarMarcacao(idMarcacao.ToString(), "mail");
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        lblMessage.Text = e.ToString();
                    }
                }
            }
        }

        private void btnFazerMarcacao_Click(object sender, System.EventArgs e)
        {
            fazerMarcacao();
        }



        //======================================================================================
        //preenche a dropdown com os funcionarios marcados como sendo de "calibracao externa".
        //======================================================================================
        private void fillDDfuncionarios()

        {
            string strSQL = "select idFuncionario, nomeAbreviado from funcionario where activo = 1 order by 2";

            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddTecnicoExterior.DataSource = DR;
            ddTecnicoExterior.DataBind();
            DR.Close();
            //o 1º é obrigatorio

            DR = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddTecnicoExterior2.DataSource = DR;
            ddTecnicoExterior2.DataBind();
            ddTecnicoExterior2.Items.Insert(0, new ListItem("", ""));
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
            if (DGEmpresas.SelectedIndex < 0)
            {
                lblMessage.Text = "Seleccione a empresa.";
                return;
            }
            string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();

            DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
            DataTable DT = req.DTListaRequisicoes(idEmpresa, "", "", "", false, false);
            DataView DV = new DataView(DT);
            DV.Sort = ViewState["sortFieldReq"].ToString() + " " + ViewState["sortDirectionReq"];

            dgRequisicoes.DataSource = DV;
            dgRequisicoes.DataBind();

            req = null;
        }

        //=================================================================================================
        //PAGING DAS REQUISICOES
        //=================================================================================================
        public void DoPaging(Object s, DataGridPageChangedEventArgs e)
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
                    ViewState["sortDirectionReq"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirectionReq"] = "ASC";
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
            DataGridItem dgi = (DataGridItem)cbSender.Parent.Parent;

            string idRequisicao = dgRequisicoes.DataKeys[dgi.ItemIndex].ToString();
            //Response.Write(idRequisicao); 
            DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
            req.UpdateRequisicaoCompleta(idRequisicao, cbSender.Checked.ToString(), User.Identity.Name.ToString());
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
            switch (i)
            {
                case 0:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
                case 1:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("Gold");
                    break;
                case 2:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("DarkOrange");
                    break;
                case 3:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("Crimson");
                    break;
                default:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
            }

            if (codigoBloqueioSAP == "03") //se a empresa está inactiva em SAP (n tem a ver com o nivelBloqueiolabmetro, martelada...)
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
            switch (i)
            {
                case 0:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("Red");
                    break;
                case 1:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
                default:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
            }

            return colorName;
        }


        //=================================================================================================
        //=================================================================================================
        protected string ConverteEstado(bool b)
        {
            if (b == true) return "sim";
            else return "não";
        }

        //=================================================================================================
        // DEVOLVE O CAMINHO PARA UMA REQUISICAO
        //=================================================================================================
        public string downloadpath(object filename)
        {
            if (filename != null && filename.ToString() != "")
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
            foreach (DataGridItem dgi in DGEquipamentos.Items)
            {
                CheckBox cbA = (CheckBox)dgi.Cells[0].FindControl("checkbox");

                bool b = cbA.Checked;
                cbA.Checked = !b;
            }
        }

        private void bntC_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridItem dgi in DGEquipamentos.Items)
            {
                CheckBox cbA = (CheckBox)dgi.Cells[0].FindControl("cbCalibracao");

                bool b = cbA.Checked;
                cbA.Checked = !b;
            }

        }

        private void bntE_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridItem dgi in DGEquipamentos.Items)
            {
                CheckBox cbA = (CheckBox)dgi.Cells[0].FindControl("cbEnsaio");

                bool b = cbA.Checked;
                cbA.Checked = !b;
            }

        }

        private void btnFiltrarEquips_Click(object sender, System.EventArgs e)
        {
            BindGridEquipamentos();
            DGEquipamentos.Visible = true;
        }

        private void btnV_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridItem dgi in DGEquipamentos.Items)
            {
                CheckBox cbV = (CheckBox)dgi.Cells[0].FindControl("cbVerificacao");

                bool b = cbV.Checked;
                cbV.Checked = !b;
            }

        }

        private void Checkbox1_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        private void Linkbutton1_Click(object sender, System.EventArgs e)
        {

        }


        //*************************************************************************************************
        //INÍCIO ORÇAMENTOS
        //*************************************************************************************************


        //=================================================================================================
        //BOTAO QUE VAI MOSTRAR OS ORÇAMENTOS
        //=================================================================================================
        private void btnOrcamentos_Click(object sender, System.EventArgs e)
        {

            dgOrcamentos.CurrentPageIndex = 0;
            if (dgOrcamentos.Items.Count != 0)
            {
                dgOrcamentos.DataSource = null;
                dgOrcamentos.DataBind();
                dgOrcamentos.Controls.Clear();
                dgOrcamentos.Visible = false;
                btnOrcamentos.Text = "Ver Orçamentos";
            }
            else
            {
                BindGridOrcamentos();
                if (dgOrcamentos.Items.Count == 0)
                    dgOrcamentos.Visible = false;
                else
                {
                    dgOrcamentos.Visible = true;
                    btnOrcamentos.Text = "Ocultar Orçamentos";
                }
            }
        }

        //=================================================================================================
        //BIND GRID ORÇAMENTOS
        //=================================================================================================
        private void BindGridOrcamentos()
        {
            if (DGEmpresas.SelectedIndex < 0)
            {
                lblMessage.Text = "Seleccione a empresa.";
                return;
            }
            string idEmpresa = DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString();

            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();

            DataTable DT = orc.DTOrcamentos(null, null, null, null, idEmpresa,null);

            DataView DV = new DataView(DT);
            DV.Sort = ViewState["sortFieldOrc"].ToString() + " " + ViewState["sortDirectionOrc"];

            dgOrcamentos.DataSource = DV;
            dgOrcamentos.DataBind();

            orc = null;
        }

        //=================================================================================================
        //PAGING DAS REQUISICOES
        //=================================================================================================
        public void DoPagingOrc(Object s, DataGridPageChangedEventArgs e)
        {
            dgOrcamentos.CurrentPageIndex = e.NewPageIndex;
            BindGridOrcamentos();

        }

        //=================================================================================================
        //SORTGRID DAS REQUISICOES
        //=================================================================================================
        public void SortGridOrc(Object s, DataGridSortCommandEventArgs e)
        {
            switch (ViewState["sortDirectionOrc"].ToString())
            {
                case "ASC":
                    ViewState["sortDirectionOrc"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirectionOrc"] = "ASC";
                    break;
            }

            ViewState["sortFieldOrc"] = e.SortExpression;

            BindGridOrcamentos();

        }

        private void dgOrcItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string idOrcamento = dgOrcamentos.DataKeys[e.Item.ItemIndex].ToString();

            if (e.Item.ItemIndex > -1)
            {

                if (e.CommandName.ToString() == "verOrcamento")
                {
                    verReportOrcamento(idOrcamento);

                }
            }
        }
        private void verReportOrcamento(string idOrcamento)
        {
            rptOrcamentoSemPreco report = new rptOrcamentoSemPreco();
            clsReport cr = new clsReport();

            string faxNumber = "---";

            report.SetParameterValue("@inFaxNumber", faxNumber);

            DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
            DataSet ds = orc.DSOrcamFax(idOrcamento);
            report.SetDataSource(ds);

            ds = null;
            // Exportar o report para PDF
            cr.exportReportToPDF(report, "Report");
        }


        private void enviarMarcacao(string idMarcacao, string tipo) //COPY PASTE DE DIVERSAS ORIGENS SEM INSPIRACAO...+ alteracoes posteriores
        {
            clsReport cr = new clsReport();
            rptFaxMarcacaoGeral report = new rptFaxMarcacaoGeral();

            //nomeFuncionario ISQ
            string strSQL = "  SELECT  dbo.udfGetNomeUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";
            string nomeFuncionario = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            string numFaxCliente = "";
            string emailCliente = "";
            string NomeCliente = "";
            string refFax = "";

            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdMarcacao", idMarcacao);
            SqlDataReader dr = GERAL.clsDataAccess.SPExecuteDRParams("stpGetDadosMarcacaoGeralByIdMarcacao", arrParams);
            if (!dr.HasRows)
            {
                lblMessage.Text = "Verifique dados empresa/marcação.";
                return;
            }
            else
            {

                while (dr.Read())
                {
                    numFaxCliente = dr["faxEmpresa"].ToString();
                    emailCliente = dr["emailEmpresa"].ToString();
                    NomeCliente = dr["NomeEmpresa"].ToString();
                    refFax = dr["refFax"].ToString();

                    report.SetParameterValue("@inNomeFuncionario", nomeFuncionario);
                    report.SetParameterValue("@inFaxNumber", numFaxCliente);
                    report.SetParameterValue("@inNomeEmpresa", dr["nomeEmpresa"].ToString());
                    report.SetParameterValue("@inNomeContacto", dr["nomeContacto"].ToString());
                    report.SetParameterValue("@inRefFax", refFax);

                    string dtMarcacao = dr["dtMarcacao"].ToString();
                    string dtFimMarcacao = dr["dtFimMarcacao"].ToString();
                    string dataMarcacao = "";

                    if (dtMarcacao == dtFimMarcacao)
                    {
                        dataMarcacao = dtMarcacao;
                    }
                    else
                    {
                        dataMarcacao = dtMarcacao + " a " + dtFimMarcacao + " ";
                    }

                    report.SetParameterValue("@inDiaVisita", dataMarcacao);
                    report.SetParameterValue("@inMoradaEmpresa", txtMorada.Text);
                    report.SetParameterValue("@inPeriodo", txtPeriodoMarcacao.Text);
                    report.SetParameterValue("@inObsCliente", txtObsCliente.Text);
                    report.SetParameterValue("@inRequisicao", ddRequisicao.SelectedItem.Text);
                    string empresaContratante = "";
                    if (ddEmpresaContratante.SelectedIndex > -1) empresaContratante = ddEmpresaContratante.SelectedItem.Text;
                    report.SetParameterValue("@inEmpresaContratante", empresaContratante);
                }
            }

            dr.Close();

            string id = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();

            strSQL = "  SELECT  dbo.udfGetEmailUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";

            string mailSender = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            if (mailSender == "")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INDICAR_MAIL_FUNCIONARIO;
                return;

            }


            if (tipo == "fax")
            {
                if (numFaxCliente == "")
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_CLIENTE_SEM_NUMFAX;
                    return;

                }
                //cr.faxReport(report, numFaxCliente, mailSender, "FAX", id);
                cr.sendFaxNovo(report, numFaxCliente,  "FAX", id);

            }
            else
            {
                if (emailCliente == "")
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_CLIENTE_SEM_EMAIL;
                    return;

                }

                string str_Cumprimentos = "";
                if (DateTime.Compare(DateTime.Now, Convert.ToDateTime("13:00:00")) < 0)
                {
                    str_Cumprimentos = "bom dia";
                }
                else
                {
                    str_Cumprimentos = "boa tarde";
                }

                string mailBody = "Muito " + str_Cumprimentos + "," + "\r\n" + "\r\n" +
                    "Exmo(s). Senhor(es)," + "\r\n" + NomeCliente + "\r\n" + "\r\n" +
                    "Gratos pela vossa disponibilidade, anexamos a confirmação de agendamento dos serviços a prestar nas vossas instalações." + "\r\n" + "\r\n" +
                    "Estamos ao dispor." + "\r\n" + "\r\n" +
                    "Cumprimentos," + "\r\n" +
                    "Equipa LabMetro.";
                //emailCliente = "bfcavaco@isq.pt"; //DEBUG MODE ONLY
                cr.mailReport(report, emailCliente, mailSender, "Marcação de Trabalhos", "MARC", refFax, mailBody, "","");
            }

            cr = null;
            report = null;

            updateMarcacaoSetFaxSent(idMarcacao, tipo);
        }


        private void updateMarcacaoSetFaxSent(string idMarcacao, string tipo)
        {
            string strSQL = "UPDATE marcacao set bFaxEnviado = 1 , tipo = '" + tipo + "', dtUltimoFax = getdate() where idMarcacao = " + idMarcacao;
            int i = GERAL.clsDataAccess.myExecuteNonQuery(strSQL);
            BindGridMarcacoes();
        }

        private void cbEnviarFax_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbEnviarMail.Checked) cbEnviarMail.Checked = false;
        }

        private void cbEnviarMail_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbEnviarFax.Checked) cbEnviarFax.Checked = false;
        }

        protected void pesquisaEmpresaContratante_Click(object sender, System.EventArgs e)
        {
            fillDDEmpresaContratante();

        }
        private void fillDDEmpresaContratante()
        {
            DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            DataTable DT = empresa.DTEmpresas(txtEmpresaContratante.Text, txtNifEmpresaContratante.Text, "1", "", "", "", "", "", ""); //activas
            DataView DV = new DataView(DT);
            ddEmpresaContratante.DataSource = DV; ;
            ddEmpresaContratante.DataBind();
            empresa = null;
            rbEmpbrepodevercertificados.Enabled = true;
            rbEmpbrepodevercertificados.SelectedValue = "0"; //dps pode mudar
        }

        protected void txtEmpresaContratante_TextChanged(object sender, System.EventArgs e)
        {
            fillDDEmpresaContratante();
        }

    }
}
