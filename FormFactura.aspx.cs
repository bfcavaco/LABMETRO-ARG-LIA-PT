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
using System.Text;
using System.IO;
using System.Data.SqlTypes;
//using LabMetro.WebServiceSAP;
//using LabMetro.wsSAP;
using LabMetro.wsInsereFacturaSAP;
using LabMetro.wsFatintSAP;
using System.Collections.Generic;

namespace LabMetro
{
    public partial class FormFactura : System.Web.UI.Page
    {
        private double dbValorSubTotal = 0;
        private double dbValorDescontoTotal = 0;

        private const string ID_PAG = "FACTURAS_1";//NOME DA PAGINA
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

        DataView DVOrigem;
        DataTable DTOrigem;
        DataTable DTDestino;
        DataView DVDestino;

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

            btnUpdateRegiaoVendasEmpresa.Click += new System.EventHandler(btnUpdateRegiaoVendasEmpresa_Click);
            btnFicheiro.Click += new System.EventHandler(btnFicheiro_Click);
            ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            ddBRE.SelectedIndexChanged += new System.EventHandler(ddBRE_SelectedIndexChanged);
            ddbSE.SelectedIndexChanged += new System.EventHandler(ddbSE_SelectedIndexChanged);
            cbBRE.CheckedChanged += new System.EventHandler(cbBRE_CheckedChanged);
            btnSubmitGrid.Click += new System.EventHandler(btnSubmitGrid_Click);
            dgDestino.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(dgDestino_DeleteCommand);
            txtValorAjudasCustoDeslocacoes.TextChanged += new System.EventHandler(txtValorAjudasCustoDeslocacoes_TextChanged);
            txtValorDespesasEnvio.TextChanged += new System.EventHandler(txtValorDespesasEnvio_TextChanged);
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            btnReset.Click += new System.EventHandler(btnReset_Click);
            btnRequisicoes.Click += new System.EventHandler(btnRequisicoes_Click);
            txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
            txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
            btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
            btnVerFactura.Click += new System.EventHandler(btnVerFactura_Click);

        }
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            lblMessage.Text = "";
            lblErroRequisicoes.Text = "";

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

                    if (intAcesso == 0)
                    {
                        btnSubmit.Enabled = false;
                        btnReset.Enabled = false;
                    }

                    if (!Page.IsPostBack)
                    {

                        fillDDsSAP(); //preenche das dd's com parametros do SAP

                        //form preenchido
                        if (Request.QueryString["id"] != null)
                        {
                            btnSubmit.CommandArgument = "update";

                            if (Request.QueryString["id"] != "")
                            {
                                ViewState["idFactura"] = Request.QueryString["id"].ToString();


                                fillForm(ViewState["idFactura"].ToString());

                                fillEmpresaContratanteDoBRE(); //isto ja foi chamado no filldata... nao sei pq está aqui de novo.

                                //btnFicheiro.Enabled = true; //NUNCA PODE FICAR ENABLEDate indicacao contrária...
                                fillEmpresaData_SAP(); //preenche os dados sap da empresa ou empresa contratante




                                fillEmpresaDevedora(); //prenche os dados devedores da empresa ou empresa contratante
                                fillDadosBRE();
                            }
                        }
                        //form vazio
                        else
                        {
                            ViewState["idFactura"] = null;

                            btnSubmit.CommandArgument = "insert";

                            fillEmpresas(); //preenche empresas
                            fillBREs(); //preenche bre's
                            fillDadosBRE();
                            fillBSEs();
                            fillEmpresaContratanteDoBRE(); //preenche empresas contratantes

                            fillEmpresaData_SAP(); //preenche os dados SAP da empresa ou empresa contratante


                            fillEmpresaDevedora(); //preenche se empresa ou emp.cont. é devedora

                            setFacturaDataToZero(); //para inicializar alguns campos a 0
                            btnFicheiro.Enabled = false; //factura n está inserida, n se pode criar ficheiro
                            btnVerFactura.Enabled = false;
                        }

                        //para a ordenacao do datagrid de origem
                        ViewState["sortField"] = "codTipoEquipamento";
                        ViewState["sortDirection"] = "DESC";

                        ViewState["sortFieldDestino"] = "codTipoEquipamento";
                        ViewState["sortDirectionDestino"] = "DESC";

                        ViewState["sortFieldReq"] = "idRequisicao";
                        ViewState["sortDirectionReq"] = "DESC";
                        //==============================================

                        CreateDataSource();
                        BindGridSource();
                        try
                        {
                            CreateDataSourceDTDestino();
                            BindGridDestino();
                        }
                        catch (Exception ex)
                        {
                            //Response.Write(ex.ToString()); 
                            GERAL.clsWriteError.WriteLog(ex);
                        }
                    }
                    //==================================================


                    txtDtFactura.Enabled = true; //tirar depois


                }
            }


        }

        //*************************************************************************************************
        //FUNÇÕES ASSOCIADAS AOS DOIS DATAGRIDS PRINCIPAIS, ORIGEM E DESTINO.
        //*************************************************************************************************


        //=================================================================================================
        //CRIA DATASOURCE DOS SERVIÇOS POR FACTURAR (DATAGRID ORIGEM) =====================================
        //A DATATABLE DTOrigem é guardada em viewstate====================================================
        //=================================================================================================
        private void CreateDataSource()
        {
            DTOrigem = new DataTable();
            DATA.FacturaData f = new LabMetro.DATA.FacturaData();
            DTOrigem = f.DTGetServicosForFactura(ddBRE.SelectedValue);
            //====================================================================
            DataColumn[] dcPk = new DataColumn[1];
            dcPk[0] = DTOrigem.Columns["idServico"];
            DTOrigem.PrimaryKey = dcPk;
            //====================================================================
            ViewState["DTOrigem"] = DTOrigem;

            f = null;
        }

        //=================================================================================================
        //BIND GRID DO DATAGRID ORIGEM - SERVIÇOS POR FACTURAR ============================================
        //=================================================================================================
        private void BindGridSource()
        {
            DTOrigem = (DataTable)ViewState["DTOrigem"];

            DVOrigem = new DataView(DTOrigem);
            DVOrigem.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

            dgOrigem.DataSource = DVOrigem;
            dgOrigem.DataBind();
        }

        //=================================================================================================
        //SORT GRID ORIGEM ================================================================================
        //=================================================================================================
        protected void SortGridOrigem(Object s, DataGridSortCommandEventArgs e)
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
            ViewState["sortField"] = e.SortExpression;
            BindGridSource();
        }

        //========================================================================================
        //itemdatabound do dgorigem (novo-17-01-2007. para poder trocar requisicoes)
        //========================================================================================
        public void dgOrigem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DataRowView DRV = (DataRowView)e.Item.DataItem;

                DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");


                //a requisicao está sempre associada à mesma empresa que os serviços, por isso tudo bem. 

                DATA.RequisicaoBD requisicao = new LabMetro.DATA.RequisicaoBD();
                SqlDataReader DR = requisicao.DRGetRequisicoesIncompletasByEmpresa(ddEmpresa.SelectedValue);
                ddRequisicao.DataSource = DR;
                ddRequisicao.DataBind();
                ddRequisicao.Items.Insert(0, new ListItem("", ""));
                DR.Close();

                requisicao = null;


                string idRequisicao = DRV["idRequisicao"].ToString();
                try
                {
                    if (idRequisicao != "") ddRequisicao.SelectedValue = idRequisicao;
                }
                catch
                { }

            }
        }

        //=================================================================================================
        //EDITA O GRID DO ORIGEM
        //=================================================================================================
        protected void editGridOrigem(Object sender, DataGridCommandEventArgs e)
        {
            dgOrigem.EditItemIndex = e.Item.ItemIndex;
            BindGridSource();
        }

        //=================================================================================================
        //CANCELA A EDIÇÃO DO GRID DE ORIGEM
        //=================================================================================================
        protected void cancelGridOrigem(Object sender, DataGridCommandEventArgs e)
        {
            dgOrigem.EditItemIndex = -1;
            BindGridSource();
        }

        //=================================================================================================
        //UPDATE AO GRID DE ORIGEM - permite alterar a ref.Req.(Unicamente)
        //=================================================================================================
        protected void updateGridOrigem(Object sender, DataGridCommandEventArgs e)
        {
            string idServico = dgOrigem.DataKeys[e.Item.ItemIndex].ToString();

            DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");

            if (ddRequisicao.SelectedValue != "") //nao deve estar vazio
            {

                string strSQL = "UPDATE servico SET idRequisicao = " + ddRequisicao.SelectedValue + " WHERE idServico = " + idServico;
                lblMessage.Text = GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);

                dgOrigem.EditItemIndex = -1;
                CreateDataSource();
                BindGridSource();
                CreateDataSourceDTDestino();
                BindGridDestino();

            }
        }

        //=================================================================================================
        //CRIA DATASOURCE DO DATAGRID DESTINO -- SERVIÇOS QUE DE FACTO SÃO FACTURADOS======================
        //OU VEM DA BD, NO CASO DE JÁ ESTAR PREENCHIDO, OU É UMA REPLICA DO DATAGRID ORIGEM (EM ESTRUTURA)
        //A DATATABLE DTDestino é guardada em viewstate====================================================
        //=================================================================================================
        private void CreateDataSourceDTDestino()
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    DATA.FacturaData f = new LabMetro.DATA.FacturaData();
                    DTDestino = f.DTGetServicoByFactura(ViewState["idFactura"].ToString());
                    DataColumn[] dcPk = new DataColumn[1];
                    dcPk[0] = DTDestino.Columns["idServico"];
                    DTDestino.PrimaryKey = dcPk;

                    f = null;
                }
                else
                {
                    DTDestino = new DataTable();
                    DTOrigem = (DataTable)ViewState["DTOrigem"];
                    DTDestino = DTOrigem.Clone();
                }
            }
            else
            {
                DTDestino = new DataTable();
                DTOrigem = (DataTable)ViewState["DTOrigem"];
                DTDestino = DTOrigem.Clone();
            }
            ViewState["DTDestino"] = DTDestino;

        }

        //=================================================================================================
        //BIND GRID DO DATAGRID DESTINO - SERVIÇOS FACTURADOS= ============================================
        //=================================================================================================
        private void BindGridDestino()
        {
            DTDestino = (DataTable)ViewState["DTDestino"];

            DVDestino = new DataView(DTDestino);
            DVDestino.Sort = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];
            dgDestino.DataSource = DVDestino;
            dgDestino.DataBind();
        }

        //=================================================================================================
        //SORT GRID DESTINO
        //=================================================================================================
        protected void SortGridDestino(Object s, DataGridSortCommandEventArgs e)
        {
            switch (ViewState["sortDirectionDestino"].ToString())
            {
                case "ASC":
                    ViewState["sortDirectionDestino"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirectionDestino"] = "ASC";
                    break;
            }

            ViewState["sortFieldDestino"] = e.SortExpression;
            BindGridDestino();
        }

        //*************************************************************************************************
        //FIM GRIDS PRINCIPAIS
        //*************************************************************************************************


        //*************************************************************************************************
        //INÍCIO REQUISIÇÕES
        //*************************************************************************************************


        //=================================================================================================
        //BIND GRID DAS REQUISIÇÕES
        //=================================================================================================
        private void BindGridRequisicoes()
        {
            DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
            DataTable DT = req.DTListaRequisicoes(ddEmpresa.SelectedValue, "", "", "", false, false);
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
            int i = dgi.ItemIndex;
            string idRequisicao = dgRequisicoes.DataKeys[i].ToString();
            //Response.Write(idRequisicao); 
            DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
            req.UpdateRequisicaoCompleta(idRequisicao, cbSender.Checked.ToString(), User.Identity.Name.ToString());
        }

        //=================================================================================================
        // Requisições - SHOW/HIDE
        //=================================================================================================
        private void btnRequisicoes_Click(object sender, System.EventArgs e)
        {
            dgRequisicoes.CurrentPageIndex = 0;
            if (dgRequisicoes.Items.Count != 0)
            {
                dgRequisicoes.DataSource = null;
                dgRequisicoes.DataBind();
                dgRequisicoes.Controls.Clear();
                dgRequisicoes.Visible = false;
                btnRequisicoes.Text = "Ver Requisições";
            }
            else
            {
                BindGridRequisicoes();
                if (dgRequisicoes.Items.Count == 0)
                    dgRequisicoes.Visible = false;
                else
                {
                    dgRequisicoes.Visible = true;
                    btnRequisicoes.Text = "Ocultar Requisições";
                }
            }
        }

        //=================================================================================================
        // DEVOLVE O CAMINHO PARA UM NOME DE FICHEIRO
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

        //*************************************************************************************************
        //FIM REQUISIÇÕES
        //*************************************************************************************************

        //*************************************************************************************************
        //ACÇÕES DE BD
        //*************************************************************************************************

        //=================================================================================================
        //INSERE OS DADOS DA FACTURA NA BD
        //No insert da factura, a data da factura assume o valor da data actual (getdate()); 
        //a data depois é actualizada apenas na criação das ordens de venda. 
        //assume a data do proprio dia ou do dia seguinte, conforme a hora do dia em que a operação é				efectuada
        //com a hipotese de poder forçar a data.... 
        //=================================================================================================
        private void InsertBD()
        {

            string idFactura;
            

            DTDestino = (DataTable)ViewState["DTDestino"];

            if (DTDestino.Rows.Count == 0)
            {
                lblMessage.Text = "Tem de inserir pelo menos uma linha na factura.";
            }
            else
            {
                if (!validaRegiaoVendas(DTDestino))
                {
                    lblMessage.Text = "Existem calibrações externa. Actualize a região de vendas da empresa.<br>e carregue no botão 'Actualizar Região' para gravar as alterações.";
                }
                else
                {
                    calculaValorTotalFactura(); // só para garantir que o total está bem actualizado

                    DATA.FacturaData factura = new LabMetro.DATA.FacturaData();


                    //if (myApp.ToUpper() == "ES_LABMETRO")
                    //    //so em Espanha podem editar a data da factura -- em pt podem forçar a data o que serve apenas para os ficheiros SAP
                    //{
                    idFactura = factura.InsertFacturaWithLinhas(ddBRE.SelectedValue.ToString(), ddEmpresa.SelectedValue.ToString(), txtValorDespesasEnvio.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotalFactura.Text, txtObservacoes.Text, User.Identity.Name.ToString(), DTDestino, ddTipoDocumento.SelectedValue.ToString(), ddCanalDistribuicao.SelectedValue, ddEscritorioVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue, ddbSE.SelectedItem.Text, txtDtFactura.Text.ToString(), txtRefPedidoCliente.Text, txtDtPedidoCliente.Text).ToString(); //Não juntar estas linhas
                    //}
                    //else
                    //{
                    //idFactura = factura.InsertFacturaWithLinhas(ddBRE.SelectedValue.ToString(), ddEmpresa.SelectedValue.ToString(), txtValorDespesasEnvio.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotalFactura.Text, txtObservacoes.Text, User.Identity.Name.ToString(), DTDestino, ddTipoDocumento.SelectedValue.ToString(), ddCanalDistribuicao.SelectedValue, ddEscritorioVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue,ddbSE.SelectedValue,null).ToString(); //Não juntar estas linhas
                    //}
                    //senao entra 2 vezes na mesma função. 
                   

                    if (idFactura == "0")
                    {
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
                    }
                    else
                    {
                        //Server.Transfer("FormFacturaSAP.aspx?btn=FAC&id=" + id,false);  //Deixar a false senao entra - nao sei porquê - novamente na função insertfacturawithlinha e dps dá erro.
                        Response.Redirect("FormFactura.aspx?btn=FAC&id=" + idFactura, true);
                        //deixar assim, sem server transfer
                    }
                    factura = null;
                }
            }
        }

        //=================================================================================================
        //ACTUALIZA OS DADOS DA FACTURA NA BD
        //=================================================================================================
        private void UpdateBD()
        {
            DTDestino = (DataTable)ViewState["DTDestino"];
            DTOrigem = (DataTable)ViewState["DTOrigem"];

            //if (DTDestino.Rows.Count == 0)
            //{
            //    lblMessage.Text = "Tem de inserir pelo menos uma linha na factura.";
            //}
            //else
            //{

            if (!validaRegiaoVendas(DTDestino))
            {
                lblMessage.Text = "Existem calibrações externa. Actualize a região de vendas da empresa.<br>e carregue no botão 'Actualizar Região' para gravar as alterações.";
            }
            else
            {
                DATA.FacturaData factura = new LabMetro.DATA.FacturaData();

                string idFactura = ViewState["idFactura"].ToString();

                int retvalue = factura.UpdateFacturaWithLinhas(ViewState["idFactura"].ToString(), txtValorDespesasEnvio.Text.ToString(), txtValorAjudasCustoDeslocacoes.Text.ToString(), txtValorTotalFactura.Text.ToString(), txtObservacoes.Text.ToString(), User.Identity.Name.ToString(), DTOrigem, DTDestino, ddTipoDocumento.SelectedValue, ddCanalDistribuicao.SelectedValue, ddEscritorioVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue, ViewState["idModifUser"].ToString(), ddbSE.SelectedItem.Text, txtDtFactura.Text, txtRefPedidoCliente.Text, txtDtPedidoCliente.Text);

                if (retvalue == 0)
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
                }
                else
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
                    //colocar a nova dtAlteração em viewstate
                    try
                    {
                        string strSQL = "SELECT idUtilAlteracao FROM FACTURA where idFactura = " + ViewState["idFactura"].ToString();
                        string idUtilizadorModif = Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

                        ViewState["idModifUser"] = idUtilizadorModif;
                    }
                    catch (Exception ex)
                    {
                        GERAL.clsWriteError.WriteLog(ex);
                    }
                }

                factura = null;
            }
            //}
        }



        //*************************************************************************************************
        //FIM ACCÇÕES DE BD
        //*************************************************************************************************

        //*************************************************************************************************
        //ACÇÕES SOBRE OS DATAGRIDS
        //*************************************************************************************************

        //=================================================================================================
        //APAGA DO DESTINO E VOLTA A ADICIONAR À ORIGEM
        //=================================================================================================


        private void dgDestino_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DTDestino = (DataTable)ViewState["DTDestino"];

            DVDestino = new DataView(DTDestino);
            string strSort = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];

            DVDestino.Sort = strSort;

            DTOrigem = (DataTable)ViewState["DTOrigem"];

            DataRowView drv = DVDestino[e.Item.ItemIndex];
            //Response.Write(e.Item.ItemIndex.ToString()); 

            //ADICIONA À ORIGEM (aqui usa-se o add em vez do importrow)
            DataRow DR;
            DR = DTOrigem.NewRow();

            DR["idServico"] = drv["idServico"].ToString();
            DR["idBRE"] = drv["idBRE"].ToString();
            DR["refBRE"] = drv["refBRE"].ToString();
            DR["idRequisicao"] = drv["idRequisicao"].ToString();
            DR["refRequisicao"] = drv["refRequisicao"].ToString();
            DR["numIdentificacao"] = drv["numIdentificacao"].ToString();
            DR["codTipoEquipamento"] = drv["codTipoEquipamento"].ToString();
            DR["idEstadoServico"] = drv["idEstadoServico"].ToString();
            DR["estadoServico"] = drv["estadoServico"].ToString();
            DR["idLocalCalibracao"] = drv["idLocalCalibracao"].ToString();
            DR["localCalibracao"] = drv["localCalibracao"].ToString();
            DR["idTipoServico"] = drv["idTipoServico"].ToString();
            DR["tipoServico"] = drv["tipoServico"].ToString();
            DR["refServico"] = drv["refServico"].ToString();
            if (drv["valor"].ToString() != "")
            {
                DR["valor"] = Decimal.Parse(drv["valor"].ToString()); //da bd vem um decimal   
            }
            else
            {
                DR["valor"] = "0"; //da bd vem um decimal   
            }
            if (drv["percDesconto"].ToString() != "")
            {
                DR["percDesconto"] = Decimal.Parse(drv["percDesconto"].ToString()); //da bd vem um decimal   
            }
            else
            {
                DR["percDesconto"] = "0"; //da bd vem um decimal   
            }
            if (drv["valorFinal"].ToString() != "")
            {
                DR["valorFinal"] = Decimal.Parse(drv["valorFinal"].ToString()); //da bd vem um decimal   
            }
            else
            {
                DR["valorFinal"] = "0"; //da bd vem um decimal   
            }

            DR["observacoes"] = drv["observacoes"].ToString();
            DR["referenciaCliente"] = drv["referenciaCliente"].ToString();
            DR["calibracaoExterna"] = drv["calibracaoExterna"].ToString();


            DTOrigem.Rows.Add(DR);

            //APAGA DO DESTINO 
            string id = drv["idServico"].ToString();
            DataRow dr = DTDestino.Rows.Find(id);
            dr.Delete();
            DTDestino.AcceptChanges();


            ViewState["DTOrigem"] = DTOrigem;
            ViewState["DTDestino"] = DTDestino;

            BindGridDestino();
            BindGridSource();

            //os valores totais sao calculados no item databound, se eu nao tiver nada no datagrid
            //ele nao entra nessa funcao.
            //acontece quando eu removo o ultimo item da lista, o preço devia ficar a 0, mas ele nao 
            //chama a funcao, por isso:

            if (DTDestino.Rows.Count == 0)
            {
                txtValorAjudasCustoDeslocacoes.Text = "0";
                txtValorDespesasEnvio.Text = "0";
                txtValorSubTotal.Text = "0";
                txtDescontoTotal.Text = "0";
                txtValorTotalFactura.Text = "0";
            }
        }

        //=================================================================================================
        //ADICIONA LINHAS SELECCIONADAS DA ORIGEM AO DESTINO
        //=================================================================================================
        private void AddLinesToDestino()
        {
            //vai buscar a percentagem de desconto associada à empresa e passa para o datagrid de baixo quando os serviços passam para facturacao
            DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
            string idEmpresa = ddEmpresa.SelectedValue;
            if (ddEmpresaContratante.SelectedValue != "") idEmpresa = ddEmpresaContratante.SelectedValue;

            string strPercDesconto = factura.strPercentagemDesconto(idEmpresa);

            DTDestino = (DataTable)ViewState["DTDestino"];
            DTOrigem = (DataTable)ViewState["DTOrigem"];

            string strIds = "";

            foreach (DataGridItem dgi in dgOrigem.Items)
            {
                CheckBox myCheckBox =
                    (CheckBox)dgi.Cells[0].FindControl("checkbox");
                if (myCheckBox.Checked == true)
                {
                    strIds += dgOrigem.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter);

            if (strIds != "")
            {
                string strSearch = "idServico IN (" + strIds + ")";

                DataRow[] foundRows = DTOrigem.Select(strSearch);

                foreach (DataRow dr in foundRows)
                {
                    if (!DTDestino.Rows.Contains(dr["idServico"]))
                    {
                        DataRow DR;
                        DR = DTDestino.NewRow();

                        DR["idServico"] = dr["idServico"].ToString();
                        DR["idBRE"] = dr["idBRE"].ToString();
                        DR["refBRE"] = dr["refBRE"].ToString();
                        DR["idRequisicao"] = dr["idRequisicao"].ToString();
                        DR["refRequisicao"] = dr["refRequisicao"].ToString();
                        DR["numIdentificacao"] = dr["numIdentificacao"].ToString();
                        DR["codTipoEquipamento"] = dr["codTipoEquipamento"].ToString();
                        DR["idEstadoServico"] = dr["idEstadoServico"].ToString();
                        DR["estadoServico"] = dr["estadoServico"].ToString();
                        DR["idLocalCalibracao"] = dr["idLocalCalibracao"].ToString();
                        DR["localCalibracao"] = dr["localCalibracao"].ToString();
                        DR["idTipoServico"] = dr["idTipoServico"].ToString();
                        DR["tipoServico"] = dr["tipoServico"].ToString();
                        DR["refServico"] = dr["refServico"].ToString();
                        DR["tipoEquipamento"] = dr["tipoEquipamento"].ToString();
                        DR["referenciaCliente"] = dr["referenciaCliente"].ToString();
                        DR["refRequisicao"] = dr["refRequisicao"].ToString();

                        if (dr["valor"].ToString() != "") //foi calculado na pasta ensaio e vem da BD
                        {
                            DR["valor"] = Decimal.Parse(dr["valor"].ToString());

                        }
                        else //nao foi calculado na pasta de ensaio e tem de ser calculado aqui
                        {
                            double valor = 0;
                            try
                            {
                                DATA.CalculaPrecos calc = new LabMetro.DATA.CalculaPrecos();
                                valor = calc.calculaPreco(dr["idServico"].ToString());
                                DR["valor"] = valor;
                            }
                            catch
                            {
                                //GERAL.clsWriteError.WriteLog("erro ao calcular preço, valor calculado = " + valor + " idServico = " + dr["idServico"].ToString() + "<br>"); 
                            }
                        }

                        //a perc acima vem da tabela serviço e nao sei como vai para la parar em primeiro lugar. esta percentagem é a q está associada à empresa na tabela Empresa (nota DM); 
                        DR["percDesconto"] = strPercDesconto;

                        if (dr["valorFinal"].ToString() != "")
                        {
                            DR["valorFinal"] = Decimal.Parse(dr["valorFinal"].ToString());
                        }
                        else if (dr["valor"].ToString() != "")  //se o valor existe,calcula-se o valor final
                        {
                            DR["valorFinal"] = System.Math.Round(Double.Parse(dr["valor"].ToString()) * (100 - Double.Parse(strPercDesconto)) * 0.01, 2);
                        }


                        DR["observacoes"] = dr["observacoes"].ToString();
                        DR["calibracaoExterna"] = dr["calibracaoExterna"].ToString();

                        DTDestino.Rows.Add(DR);

                        dr.Delete(); //apaga da origem

                    }
                }
            }
            ViewState["DTOrigem"] = DTOrigem;
            ViewState["DTDestino"] = DTDestino;

            validaRequisicoes(DTDestino); //a validacao é feita aqui, é apenas informativa. 

            factura = null;
        }

        //=================================================================================================
        //EDITA O GRID DO DESTINO
        //=================================================================================================
        protected void editGrid(Object sender, DataGridCommandEventArgs e)
        {
            dgDestino.EditItemIndex = e.Item.ItemIndex;
            BindGridDestino();
        }

        //=================================================================================================
        //CANCELA A EDIÇÃO DO GRID DE DESTINO
        //=================================================================================================
        protected void cancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            dgDestino.EditItemIndex = -1;
            BindGridDestino();
        }

        //=================================================================================================
        //UPDATE AO GRID DE DESTINO
        protected void updateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = dgDestino.DataKeys[e.Item.ItemIndex].ToString();

            TextBox txtValor = (TextBox)e.Item.FindControl("txtValorEdit");
            TextBox txtPercDesconto = (TextBox)e.Item.FindControl("txtPercDescontoEdit");
            TextBox txtValorFinal = (TextBox)e.Item.FindControl("txtValorFinalEdit");

            DTDestino = (DataTable)ViewState["DTDestino"];
            DVDestino = new DataView(DTDestino);

            DVDestino.RowFilter = "idServico=" + id;

            if (DVDestino.Count > 0)
            {
                if (txtValor.Text == "") txtValor.Text = "0";
                DVDestino[0]["valor"] = Decimal.Parse(txtValor.Text);

                if (txtPercDesconto.Text == "") txtPercDesconto.Text = "0";
                DVDestino[0]["percDesconto"] = Decimal.Parse(txtPercDesconto.Text);

                DVDestino[0]["valorFinal"] = System.Math.Round(Double.Parse(txtValor.Text) * (100 - Double.Parse(txtPercDesconto.Text)) * 0.01, 2);
            }

            DVDestino.RowFilter = "";
            ViewState["DTDestino"] = DTDestino;

            dgDestino.EditItemIndex = -1;
            BindGridDestino();
        }



        //=================================================================================================
        //ADICIONA LINHAS DA ORGIGEM AO DESTINO, BOTÃO "FACTURAR EQUIPAMENTOS"
        //=================================================================================================
        private void btnSubmitGrid_Click(object sender, System.EventArgs e)
        {
            AddLinesToDestino();
            BindGridDestino();
            BindGridSource(); //NOVO!!!!
        }

        //*************************************************************************************************
        //FIM ACÇÕES SOBRE OS DATAGRIDS
        //*************************************************************************************************


        //*************************************************************************************************
        //ACÇÕES SOBRE OS VALORES DA PAGINA
        //*************************************************************************************************

        //=================================================================================================
        //PREENCHIMENTO DA DROPDOWN EMPRESA ===============================================================
        //=================================================================================================
        private void fillEmpresas()
        {
            DATA.FacturaData f = new LabMetro.DATA.FacturaData();
            DataTable DT = f.DTListaEmpresasForFactura(cbBRE.Checked.ToString(), txtPesquisaEmpresa.Text, txtPesquisaNif.Text, txtPesquisaNumClienteSAP.Text);
            DataView DV = new DataView(DT);

            ddEmpresa.DataSource = DV;
            ddEmpresa.DataBind();

            f = null;

            fillEmpresaDevedora(); //da primeira vez

        }

        //===========================================================================================
        //PREENCHE OS DADOS RELATIVOS À EMPRESA e que tenham a ver com os DADOS DO SAP
        //se a factura é para ser emitida em nome de outra empresa, vai buscar os dados SAP da outra empresa
        //===========================================================================================
        private void fillEmpresaData_SAP()
        {
            string strSQL;

            string idEmpresa = ddEmpresa.SelectedValue;
            if (ddEmpresaContratante.SelectedValue != "") idEmpresa = ddEmpresaContratante.SelectedValue;


            if (idEmpresa != "")
            {

                //A REGIAO DE VENDAS É SEMPRE DA EMPRESA DO BRE, POIS SO SE APLICA EM CALIBRAÇÕES EXTERNAS E DIZ RESPEITO
                //À REGIÃO DA PRESTAÇÃO DO SERVIÇO

                //strSQL = "SELECT E.observacoes, E.numClienteSAP, E.codigoBloqueioSap, E.idCondicoesPagamento, E.bPodeFacturarSemRequisicao FROM EMPRESA E  WHERE E.idEmpresa = " + idEmpresa;
                strSQL = "    SELECT     E.observacoes, E.numClienteSAP + '- ' + isnull(SAP.NIF,'--') as numClienteSAP     , E.codigoBloqueioSAP, E.idCondicoesPagamento, E.bPodeFacturarSemRequisicao FROM         Empresa AS E left JOIN                       sap_Empresas AS SAP ON CAST(e.numClienteSAP as int) = cast(sap.codigoClienteSAP as int) WHERE E.idEmpresa = " + idEmpresa; ;

                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lblObsEmpresa.Text = dr["observacoes"].ToString();
                        txtNumClienteSAP.Text = dr["numClienteSAP"].ToString();


                        try
                        {
                            string idCondicoesPagamento = dr["idCondicoesPagamento"].ToString();
                            ddCondPagamentoEmpresa.SelectedValue = idCondicoesPagamento;
                            if (ViewState["idFactura"] == null)
                            {
                                if (idCondicoesPagamento != "")
                                {
                                    ddCondPagamentoFactura.SelectedValue = ddCondPagamentoEmpresa.SelectedValue;
                                }
                                else
                                {
                                    ddCondPagamentoFactura.SelectedValue = "2"; //30 dias, default value; 
                                }
                            }
                        }
                        catch
                        { }


                        try
                        {
                            ddCodigoBloqueioSAP.SelectedValue = dr["codigoBloqueioSap"].ToString();
                        }
                        catch
                        { }


                        if (GERAL.clsGeral.ConvertBStringToBoolean(dr["bPodeFacturarSemRequisicao"].ToString()) == true)
                        {
                            cbNPrecisaReqPFacturar.Checked = true;

                        }
                        else
                        {
                            cbNPrecisaReqPFacturar.Checked = false;
                        }

                    }
                }
                dr.Close();
            }


            //para a regiao de vendas. 

            idEmpresa = ddEmpresa.SelectedValue;
            if (idEmpresa != "")
            {
                strSQL = "SELECT E.idCodigoRegiaoVendas, E.localidade FROM EMPRESA E WHERE idEmpresa = " + idEmpresa;

                SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL);

                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        txtLocalidade.Text = DR["localidade"].ToString();
                        try
                        {
                            ddRegiaoVendasEmpresa.SelectedValue = DR["idCodigoRegiaoVendas"].ToString();
                        }
                        catch
                        {
                        }
                    }
                }

                DR.Close();
            }
        }

        //=================================================================================================
        // Preencher os dados relativos à empresa (se é devedora ou não)
        // isto futuramente pode ser merged com a forma como os dados da empresas são preenhcidos nas 
        // funções anteriores, mas por enquanto, never change a running system. 
        //=================================================================================================
        private void fillEmpresaDevedora()
        {
            //DM 05-12-2007

            lblEmpresaDevedora.Text = ""; //limpar a label

            string idEmpresa = ddEmpresa.SelectedValue;
            if (ddEmpresaContratante.SelectedValue != "")
            {
                idEmpresa = ddEmpresaContratante.SelectedValue;
                lblEmpresaDevedora.Text = "Empresa contratante: "; //limpar a label

            }
            lblEmpresaDevedora.Text = ""; //limpar a label

            LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(idEmpresa);
            if (detEmpresa != null)
            {
                lblObsEmpresa.Text = detEmpresa.observacoes;    //meu

                if (detEmpresa.pagamentoAtraso == "1")
                {
                    lblEmpresaDevedora.Text += "** PAGAMENTOS EM ATRASO **<br>";
                }
                if (ddEmpresaContratante.SelectedValue != "")
                {

                    trEmpresa.BgColor = "";
                    switch (detEmpresa.nivelBloqueioLabmetro)
                    {
                        case "0":
                            trEmpresaContratante.BgColor = "";
                            break;
                        case "1":
                            trEmpresaContratante.BgColor = "Gold";
                            break;
                        case "2":
                            trEmpresaContratante.BgColor = "DarkOrange";
                            lblEmpresaDevedora.Text += "Venda à dinheiro ou Pagamento do Atrasado.<br>";
                            break;
                        case "3":
                            trEmpresaContratante.BgColor = "Crimson";
                            lblEmpresaDevedora.Text += "Venda à dinheiro.<br>";
                            break;
                    }
                }
                else
                {
                    trEmpresaContratante.BgColor = "";

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
                            lblEmpresaDevedora.Text += "Venda à dinheiro ou Pagamento do Atrasado.<br>";
                            break;
                        case "3":
                            trEmpresa.BgColor = "Crimson";
                            lblEmpresaDevedora.Text += "Venda à dinheiro.<br>";
                            break;
                    }
                }

                if (detEmpresa.codigoBloqueioSAP != "00")
                {
                    btnFicheiro.Enabled = false; //se empresa bloquada, nao pode criar ordens de venda
                    lblEmpresaDevedora.Text += "Criação de ordens de venda impossível.";
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
            else//nada - limpar
            {
                lblEmpresaDevedora.Text = "";
                trEmpresa.BgColor = "";
                lblObsEmpresa.Text = "";

            }
            empresa = null;
        }

        //=========================================================================================
        // fill dropdowns códigos sap
        //fica tudo numa funcao para nao chamar tantas instâncias da classe
        //=========================================================================================
        private void fillDDsSAP()
        {
            DATA.FacturaData f = new LabMetro.DATA.FacturaData();
            SqlDataReader dr;
            //			//CÓDIGOS BLOQUEIO
            dr = f.drCodigoBloqueioSap();
            ddCodigoBloqueioSAP.DataTextField = "descricao";
            ddCodigoBloqueioSAP.DataValueField = "id";
            ddCodigoBloqueioSAP.DataSource = dr;
            ddCodigoBloqueioSAP.DataBind();
            dr.Close();
            dr = null;
            ddCodigoBloqueioSAP.Items.Insert(0, new ListItem("", "")); //isto tem de ser tirado depois, not null

            //ESCRITÓRIO DE VENDAS
            dr = f.drEscritorioVendas();
            ddEscritorioVendas.DataTextField = "descricao";
            ddEscritorioVendas.DataValueField = "id";
            ddEscritorioVendas.DataSource = dr;
            ddEscritorioVendas.DataBind();
            dr.Close();
            dr = null;
            //ddEscritorioVendas.Items.Insert(0,new ListItem("","")); //isto tem de ser tirado depois, not null


            //CONDIÇÕES DE PAGAMENTO - da empresa e da factura
            dr = f.drCondicoesPagamento();

            ddCondPagamentoFactura.DataTextField = "descricao";
            ddCondPagamentoFactura.DataValueField = "id";
            ddCondPagamentoFactura.DataSource = dr;
            ddCondPagamentoFactura.DataBind();

            dr.Close();
            dr = null;
            //ddCondPagamentoFactura.Items.Insert(0,new ListItem("","")); //isto tem de ser tirado depois, not null



            dr = f.drCondicoesPagamento();
            ddCondPagamentoEmpresa.DataTextField = "descricao";
            ddCondPagamentoEmpresa.DataValueField = "id";
            ddCondPagamentoEmpresa.DataSource = dr;
            ddCondPagamentoEmpresa.DataBind();

            dr.Close();
            dr = null;
            ddCondPagamentoEmpresa.Items.Insert(0, new ListItem("", "")); //isto tem de ser tirado depois, not null


            //TIPO DE DOCUMENTO
            dr = f.drTipoDocumento();
            ddTipoDocumento.DataTextField = "descricao";
            ddTipoDocumento.DataValueField = "id";
            ddTipoDocumento.DataSource = dr;
            ddTipoDocumento.DataBind();
            dr.Close();
            dr = null;
            //ddTipoDocumento.Items.Insert(0,new ListItem("","")); //isto tem de ser tirado depois, not null


            ddRegiaoVendasEmpresa.DataTextField = "descricao";
            ddRegiaoVendasEmpresa.DataValueField = "id";
            try
            {
                //REGIÃO DE VENDAS
                dr = f.drRegiaoVendas();
                ddRegiaoVendasEmpresa.DataSource = dr;
                ddRegiaoVendasEmpresa.DataBind();
                dr.Close();
                dr = null;

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            ddRegiaoVendasEmpresa.Items.Insert(0, new ListItem("", ""));
        }


        //=================================================================================================
        //PREENCHIMENTO DA DROPDOWN BRES ==================================================================
        //=================================================================================================
        private void fillBREs()
        {


            if (ddEmpresa.SelectedIndex >= 0)
            {
                DATA.BreBD lista = new LabMetro.DATA.BreBD();
                DataTable DT = lista.DTGetBrePorFacturarByIdEmpresa(ddEmpresa.SelectedValue, cbBRE.Checked.ToString());
                ddBRE.DataSource = DT;
                ddBRE.DataBind();

                lista = null;

            }
            else
            {
                ddBRE.Items.Clear();
            }
        }

        private void fillBSEs()
        {
            if (ddEmpresa.SelectedIndex >= 0)
            {
                string strSQL = "select idBSE, dbo.udfgetreferenciaBSE(idBSE) as refBSE from bse where idEmpresa = " + ddEmpresa.SelectedValue + " Order by 1 desc";
                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
                ddbSE.DataSource = dr;
                ddbSE.DataBind();
                dr.Close();
                ddbSE.Items.Insert(0, new ListItem("", ""));
            }
            else
            {
                ddbSE.Items.Clear();
            }
        }

        private void fillDadosBRE()
        {

            if (ddBRE.SelectedValue != "")
            {
                string strSQL = "SELECT BRE.idOrcamento, BRE.referenciaRequisicao, BRE.observacoes as observacoesBRE, Orcamento.refOrcamento FROM BRE LEFT JOIN ORCAMENTO ON BRE.idOrcamento = Orcamento.idOrcamento where bre.idBRE = " + ddBRE.SelectedValue;

                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string idOrcamento = dr["idOrcamento"].ToString();
                        string refOrcamento = dr["refOrcamento"].ToString();
                        txtRefReq.Text = dr["referenciaRequisicao"].ToString();
                        txtObsBRE.Text = dr["observacoesBRE"].ToString();
                        if (idOrcamento != "")
                        {
                            linkOrcamento.NavigateUrl = "FormOrcam.aspx?id=" + idOrcamento;
                            linkOrcamento.Text = refOrcamento;
                        }
                        else
                        {
                            linkOrcamento.NavigateUrl = "";
                            linkOrcamento.Text = "sem orçamento associado";
                        }
                    }
                }
                dr.Close();
            }
        }

        private void fillObservacoesBSE()
        {

            if (ddBRE.SelectedValue != "")
            {
                string strSQL = "SELECT BSE.observacoes as observacoesBSE FROM BSE where bse.idBSE = " + ddbSE.SelectedValue;

                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txtObsBSE.Text = dr["observacoesBSE"].ToString();
                    }
                }
                dr.Close();
            }
        }

        //=================================================================================================
        //PREENCHIMENTO DA DROPDOWN BRES ==================================================================
        //=================================================================================================
        private void fillEmpresaContratanteDoBRE()
        {

            DATA.BreBD bre = new LabMetro.DATA.BreBD();

            SqlDataReader DR = bre.DRGetIdEmpresaContratantePorBRE(ddBRE.SelectedValue);
            ddEmpresaContratante.DataSource = DR;
            ddEmpresaContratante.DataBind();

            DR.Close();
            bre = null;

        }

        //=========================================================================================
        //Fillform - recebe o id da factura e vai buscar os dados da factura
        //=========================================================================================
        private void fillForm(string id)
        {

            LabMetro.DATA.FacturaData factura = new LabMetro.DATA.FacturaData(); //new LabMetro.DATA.FacturaData(); 

            DataTable FacturaDT = factura.DTFacturaDetails(id);
            if (FacturaDT.Rows.Count > 0)
            {
                string datatFactura = FacturaDT.Rows[0]["dtFactura"].ToString();
                if (datatFactura != "") datatFactura = GERAL.clsGeral.ToShortDate(datatFactura);

                txtDtFactura.Text = datatFactura;

                txtObservacoes.Text = FacturaDT.Rows[0]["observacoes"].ToString();
                txtRefFactura.Text = FacturaDT.Rows[0]["refFactura"].ToString();
                txtFicheiro.Text = FacturaDT.Rows[0]["idFactura"].ToString();
                txtValorAjudasCustoDeslocacoes.Text = FacturaDT.Rows[0]["valorAjudasCustoDeslocacoes"].ToString();
                txtValorDespesasEnvio.Text = FacturaDT.Rows[0]["valorDespesasEnvio"].ToString();

                string idEmpresa = FacturaDT.Rows[0]["idEmpresa"].ToString();
                string nomeEmpresa = FacturaDT.Rows[0]["nomeEmpresa"].ToString();

                //try
                //{
                //    ddEmpresa.SelectedValue = idEmpresa; 
                //}
                //catch
                //{
                ddEmpresa.Items.Insert(0, new ListItem(nomeEmpresa, idEmpresa));
                ddEmpresa.SelectedValue = idEmpresa;
                //}


                //isto é preenhcido dps do fillform e dps de preenchida a empresa contratante!!!!
                // fillEmpresaData_SAP(); //preenche os dados SAP da empresa ou empresa contratante
                // fillEmpresaDevedora(); //preenche se empresa ou emp.cont. é devedora



                ddEmpresa.Enabled = false;

                string idBRE = FacturaDT.Rows[0]["idBRE"].ToString();
                string refBRE = FacturaDT.Rows[0]["refBRE"].ToString();
                //try
                //{
                //    ddBRE.SelectedValue = idBRE;
                //}
                //catch
                //{
                ddBRE.Items.Insert(0, new ListItem(refBRE, idBRE));
                ddBRE.SelectedValue = idBRE;
                //}

                ddBRE.Enabled = false;

                fillBSEs();
                try
                {
                    ddbSE.SelectedValue = FacturaDT.Rows[0]["refBSE"].ToString();
                }
                catch
                { }
                string idTipoDocumento = FacturaDT.Rows[0]["idTipoDocumento"].ToString();
                string chCanalDistribuicao = FacturaDT.Rows[0]["chCanalDistribuicao"].ToString();
                string idEscritorioVendas = FacturaDT.Rows[0]["idEscritorioVendas"].ToString();
                string idCondicoesPagamento = FacturaDT.Rows[0]["idCondicoesPagamento"].ToString();

                string descCodigoTipoDocumento = FacturaDT.Rows[0]["descCodigoTipoDocumento"].ToString();
                string descCodigoCondPagam = FacturaDT.Rows[0]["descCodigoCondPagam"].ToString();
                string descCodigoEscritVendas = FacturaDT.Rows[0]["descCodigoEscritVendas"].ToString();

                try
                {
                    ddTipoDocumento.SelectedValue = idTipoDocumento;
                }
                catch
                {
                    ddTipoDocumento.Items.Insert(0, new ListItem(descCodigoTipoDocumento, idTipoDocumento));
                    ddTipoDocumento.SelectedValue = idTipoDocumento;
                }

                ddCanalDistribuicao.SelectedValue = chCanalDistribuicao;

                try
                {
                    ddEscritorioVendas.SelectedValue = idEscritorioVendas;
                }
                catch
                {
                    ddEscritorioVendas.Items.Insert(0, new ListItem(descCodigoEscritVendas, idEscritorioVendas));
                    ddEscritorioVendas.SelectedValue = idEscritorioVendas;
                }

                try
                {
                    ddCondPagamentoFactura.SelectedValue = idCondicoesPagamento;
                }
                catch
                {
                    ddCondPagamentoFactura.Items.Insert(0, new ListItem(descCodigoCondPagam, idCondicoesPagamento));
                    ddCondPagamentoFactura.SelectedValue = idCondicoesPagamento;
                }

                ViewState["idModifUser"] = FacturaDT.Rows[0]["idUtilAlteracao"];

                string intVersaoFicheiro = FacturaDT.Rows[0]["intVersaoFicheiro"].ToString();

                txtVersao.Text = intVersaoFicheiro.ToString();

                if (intVersaoFicheiro.ToString() == "0")
                {
                    txtFicheiro.Text = "";
                  
                }
                else
                {
                    string nomeFicheiro = id + "_" + intVersaoFicheiro.ToString() + ".txt";
                    txtFicheiro.Text = nomeFicheiro;
                }

                string numFacturaSAP = FacturaDT.Rows[0]["numFacturaSAP"].ToString();
                if (numFacturaSAP != "") 
                {
                    txtFicheiro.Text = "Num.SAP: " + numFacturaSAP;
                    btn_WS.Enabled = false; //se já tem factura sap nao se pode criar outra
                }

                txtRefPedidoCliente.Text = FacturaDT.Rows[0]["refPedidoCliente"].ToString();
                txtDtPedidoCliente.Text = GERAL.clsGeral.ToShortDate(FacturaDT.Rows[0]["dtPedidoCliente"].ToString());

            }
        }

        //=================================================================================================
        //PREENCHE ALGUNS VALORES DA FACTURA
        //=================================================================================================
        private void setFacturaDataToZero()
        {
            txtValorAjudasCustoDeslocacoes.Text = "0";
            txtValorDespesasEnvio.Text = "0";
            txtValorSubTotal.Text = "0";
            txtDescontoTotal.Text = "0";
            txtValorTotalFactura.Text = "0";
        }

        //=================================================================================================
        //RECARREGA TODOS OS CAMPOS COM VALORES INICIAIS
        //=================================================================================================
        private void refillAllData()
        {
            fillDDsSAP(); //isto talvez nao seja preciso
            fillBREs();
            fillBSEs();

            fillDadosBRE();
            fillEmpresaContratanteDoBRE();
            fillEmpresaData_SAP();
            fillEmpresaDevedora();

            setFacturaDataToZero();
            CreateDataSource();
            BindGridSource();
            CreateDataSourceDTDestino();
            BindGridDestino();

            dgRequisicoes.DataSource = null;
            dgRequisicoes.DataBind();
            dgRequisicoes.Controls.Clear();
            dgRequisicoes.Visible = false;
            btnRequisicoes.Text = "Ver Requisições";
        }


        //*************************************************************************************************
        //FIM ACÇÕES SOBRE OS VALORES DA PAGINA
        //*************************************************************************************************

        //*************************************************************************************************
        //ACÇÕES DE BOTÕES, SELECTEDINDEXCHANGED ETC.
        //*************************************************************************************************

        //=================================================================================================
        //BOTÃO SUBMIT . INSERE OU ACTUALIZA OS DADOS DA FACTURA NA BD
        //=================================================================================================
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {

            Page.Validate(); //N tirar DM, esta pagina por alguma razao precisa disto!!!

            if (Page.IsValid)
            {
                if (btnSubmit.CommandArgument == "insert")
                {
                    InsertBD();
                }
                else if (btnSubmit.CommandArgument == "update")
                {
                    UpdateBD();
                }
            }
        }

        //=================================================================================================
        //BUTTON RESET, REDIRECCIONAR PARA UM PAGINA NOVA
        //=================================================================================================
        private void btnReset_Click(object sender, System.EventArgs e)
        {
            //Server.Transfer("FormFacturaSAP.aspx?btn=FAC",false);            
            Response.Redirect("FormFactura.aspx?btn=FAC", true);
        }

        //=================================================================================================
        //BOTÃO PARA CRIAÇÃO DOS FICHEIROS PARA O SAP -- NOVO
        //=================================================================================================
        private void btnFicheiro_Click(object sender, System.EventArgs e)


        {

            //insereSAP();
            //return;


            //if ((txtNumClienteSAP.Text == "" || txtNumClienteSAP.Text == "500000") && ddCondPagamentoFactura.SelectedValue != "1")
            if ((txtNumClienteSAP.Text == "" || txtNumClienteSAP.Text.StartsWith("500000") == true) && ddCondPagamentoFactura.SelectedValue != "1")
            //a rigor, para pronto pagamento n precisa 
            //de ter numero de cliente sap.
            {
                lblMessage.Text = "Empresa sem Núm. Cliente. Num 500000 apenas para VD. Actualize a empresa para gerar ordens gerar ordens de venda.";
            }
            else if (ddCondPagamentoFactura.SelectedValue == "1") //pronto pagamento
            {
                lblMessage.Text = "Não pode gerar ordens de venda para facturas com pronto pagamento.";
            }

            else
            {

                if (myApp == "ISQ_LABMETRO")
                {
                    //retorna true ou false e faz o response redirect depois aqui, senao causa um erro:
                    //14:31:44.9049179- System.Threading.ThreadAbortException: Thread was being aborted.
                    if (writeSapFile(ViewState["idFactura"].ToString()))
                    {
                        Response.Redirect("FormFactura.aspx?btn=FAC&id=" + ViewState["idFactura"].ToString(), true);
                    }

                }
                else if (myApp == "ES_LABMETRO")
                {
                    if (writeAxaptaFile(ViewState["idFactura"].ToString()))
                    {
                        Response.Redirect("FormFactura.aspx?btn=FAC&id=" + ViewState["idFactura"].ToString(), true);
                    }

                }

            }
        }

        //=================================================================================================
        //QUANDO A EMPRESA MUDA, TODA A INFORMAÇÃO TEM DE SER RE-CARREGADA
        //=================================================================================================
        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            refillAllData();
            fillEmpresaDevedora();
            fillDadosBRE();
        }

        //=================================================================================================
        //QUANDO O BRE MUDA, PARTE DA INFORMAÇÃO TEM DE SER RECARREGADA====================================
        //=================================================================================================
        private void ddBRE_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            setFacturaDataToZero();
            CreateDataSource();
            BindGridSource();
            CreateDataSourceDTDestino();
            BindGridDestino();
            fillEmpresaContratanteDoBRE();
            fillEmpresaDevedora();
            fillEmpresaData_SAP();
            fillDadosBRE();//vao buscar referencia do orçamento e da requisicao do cliente que estão inseridos no bre - para angola 2012
        }

        private void ddbSE_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillObservacoesBSE();//vai buscar as observações do BSE
        }

        //=================================================================================================
        //=================================================================================================
        private void txtValorAjudasCustoDeslocacoes_TextChanged(object sender, System.EventArgs e)
        {

            Page.Validate(); //N tirar DM, esta pagina por alguma razao precisa disto!!!
            if (Page.IsValid) calculaValorTotalFactura();
        }

        //=================================================================================================
        //=================================================================================================
        private void txtValorDespesasEnvio_TextChanged(object sender, System.EventArgs e)
        {
            Page.Validate(); //N tirar DM, esta pagina por alguma razao precisa disto!!!	
            if (Page.IsValid) calculaValorTotalFactura();
        }

        //=================================================================================================
        //=================================================================================================
        private void cbBRE_CheckedChanged(object sender, System.EventArgs e)
        {
            fillEmpresas();
            refillAllData(); //faz fill dos bres e limpa outras coisas
        }


        //=================================================================================================
        //=================================================================================================
        private void btnVerFactura_Click(object sender, System.EventArgs e)
        {
            if (myApp == "ISQ_LABMETRO")
            {
                rptFactura report = new rptFactura();
                clsReport cr = new clsReport();

                string idFactura = ViewState["idFactura"].ToString();
                string orderby = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];

                DATA.FacturaData f = new LabMetro.DATA.FacturaData();
                int i = orderby.IndexOf("codTipoEquipamento");
                if (i > -1)
                {
                    orderby = orderby.Replace("codTipoEquipamento", "codigo");  //nome da coluna na bd
                }

                //aqui vou passar os viewstate do datagrid destino
                //esses viewstates
                DataSet ds = f.DSFactura(idFactura, orderby, myApp);

                report.SetDataSource(ds);
                ds = null;
                f = null;

                cr.exportReportToPDF(report, "Factura");


            }
            else if (myApp == "ANG_LABMETRO" || myApp == "SON_LABMETRO") //codigo nao optimizado, à pressa
            {
                REPORTS_ANG.rptFacturaANG report = new LabMetro.REPORTS_ANG.rptFacturaANG();
                clsReport cr = new clsReport();

                string idFactura = ViewState["idFactura"].ToString();
                string orderby = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];

                DATA.FacturaData f = new LabMetro.DATA.FacturaData();
                int i = orderby.IndexOf("codTipoEquipamento");
                if (i > -1)
                {
                    orderby = orderby.Replace("codTipoEquipamento", "codigo");  //nome da coluna na bd
                }

                //aqui vou passar os viewstate do datagrid destino
                //esses viewstates
                DataSet ds = f.DSFactura(idFactura, orderby, myApp);

                report.SetDataSource(ds);
                ds = null;
                f = null;

                cr.exportReportToPDF(report, "Factura");


            }

            else if (myApp == "ES_LABMETRO") //codigo nao optimizado, à pressa
            {
                REPORTS_ES.rptFactura report = new LabMetro.REPORTS_ES.rptFactura();
                clsReport cr = new clsReport();

                string idFactura = ViewState["idFactura"].ToString();
                string orderby = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];

                DATA.FacturaData f = new LabMetro.DATA.FacturaData();
                int i = orderby.IndexOf("codTipoEquipamento");
                if (i > -1)
                {
                    orderby = orderby.Replace("codTipoEquipamento", "codigo");  //nome da coluna na bd
                }

                //aqui vou passar os viewstate do datagrid destino
                //esses viewstates
                DataSet ds = f.DSFactura(idFactura, orderby, myApp);

                report.SetDataSource(ds);
                ds = null;
                f = null;

                cr.exportReportToPDF(report, "Factura");


            }
        }

        //=================================================================================================
        //=================================================================================================
        private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
        {
            fillEmpresas();
            refillAllData();
        }

        //=================================================================================================
        //=================================================================================================
        private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
        {
            fillEmpresas();
            refillAllData();
        }

        //=================================================================================================
        //=================================================================================================
        private void btnEmpresas_Click(object sender, System.EventArgs e)
        {
            fillEmpresas();
            refillAllData();
        }

        //*************************************************************************************************
        //FIM DE ACÇÕES DE BOTÕES, SELECTEDINDEXCHANGED ETC.
        //*************************************************************************************************


        //*************************************************************************************************
        //******************************FUNÇÕES ACESSÓRIAS ************************************************
        //*************************************************************************************************

        //=================================================================================================
        //=================================================================================================
        private string replaceByZero(string str)
        {
            if (str == "") str = "0";
            return str;
        }



        //=================================================================================================
        //=================================================================================================
        protected string ConverteEstado(bool b)
        {
            if (b == true) return "sim";
            else return "não";
        }

        //=================================================================================================
        //=================================================================================================
        private string strNumIdent(string s)
        {
            return " NºID: " + s;
        }

        //=================================================================================================
        //=================================================================================================
        private string strNumSerie(string s)
        {
            return " NS: " + s;
        }




        public void dgDestino_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            string strValor;
            string strValorFinal;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //                DTDestino= (DataTable)ViewState["DTDestino"]; 
                //                DVDestino = new DataView(DTDestino); 
                DataRowView DRV = (DataRowView)e.Item.DataItem;

                strValor = DRV["valor"].ToString();
                strValorFinal = DRV["valorFinal"].ToString();

                if (strValor != "")
                {
                    try
                    {
                        dbValorSubTotal += Double.Parse(strValor);
                        dbValorDescontoTotal += Double.Parse(strValor) - Double.Parse(strValorFinal);

                    }
                    catch
                    {
                        //A value was null
                    }
                }

                // actualizar as várias textbox
                txtValorSubTotal.Text = dbValorSubTotal.ToString();

                txtDescontoTotal.Text = GERAL.clsGeral.RoundString2(dbValorDescontoTotal.ToString());

                // actualizar o total da factura

                Page.Validate(); //N tirar DM, esta pagina por alguma razao precisa disto!!!
                if (Page.IsValid) calculaValorTotalFactura();
            }
        }


        //************************************************************
        //*********** COISAS DO SAP **********************************
        //************************************************************

        private SqlDataReader drTipoDocumento()
        {
            string strSQL = "SELECT idCodigoTipoDocumento, codigoTipoDocumento,descCodigoTipoDocumento FROM sap_CodigoTipoDocumento WHERE activo = 1";
            return GERAL.clsDataAccess.ExecuteDR(strSQL);
        }


        //===================================================================================================
        //funcao muito útil que compara horas!
        //===================================================================================================
        private string dtFactura()
        {
            //<add key="HORA_LIMITE_FACTURACAO_MANHA" value="13:28 PM" />
            //<add key="HORA_LIMITE_FACTURACAO_NOITE" value="21:58 PM " />
            //00:00 --> 13:30 = data hoje //13:30 --> 22:00 --> data hoje+ 1dia / 22:00 --> 00:00 = data de hoje + 1dia

            string dtFactura = "";

            string hraLimiteManha = (string)ConfigurationManager.AppSettings["HORA_LIMITE_FACTURACAO_MANHA"];
            string hraLimiteNoite = (string)ConfigurationManager.AppSettings["HORA_LIMITE_FACTURACAO_NOITE"];

            string hraManha = DateTime.Parse(hraLimiteManha).ToString("t");
            string hraNoite = DateTime.Parse(hraLimiteNoite).ToString("t");

            string horaActual = DateTime.Now.ToString("t");

            if (DateTime.Compare(DateTime.Parse(horaActual), DateTime.Parse(hraManha)) < 0)
            {
                //se antes das 13:00 data de hoje
                dtFactura = System.DateTime.Now.ToShortDateString();
            }
            else
            {
                //se depois das 13:00 data de amanha
                dtFactura = System.DateTime.Now.AddDays(1).ToShortDateString();
            }
            return dtFactura;
        }

        //criar ficheiro AXAPTA
        private bool writeAxaptaFile(string idFactura)
        {
            //data da factura
            //tem de ser actualizada antes do ficheiro ser criado
            //uma transaccao aqui nao faz sentido, pois o ficheiro é lido e nao escrito...
            //string dtFactura = dtFactura();
            //if (cbForcarData.Checked)
            //{
            //    dtFactura = txtDataFacturaForcada.Text;
            //}
            //updateDataFacturainBD(idFactura, dtFactura);
            //txtDtFactura.Text = txtDataFacturaForcada.Text; //como na ha redirect tenho de actualizar o campo à mao.

            //versao do ficheiro
            //actualizado depois da factura ter sido criada, mas declarado aqui porque é usado na criacao do ficheiro
            int iVersao = 0;
            try
            {
                iVersao = System.Convert.ToInt16(txtVersao.Text);
            }
            catch
            { }

            string pathFicheiro = (string)ConfigurationManager.AppSettings["FACTURACAO_PATH_REL"];
            string nomeFicheiro = idFactura + "_" + (iVersao + 1).ToString() + ".txt";
            string ficheiro = Server.MapPath("~/" + pathFicheiro + "/" + nomeFicheiro);

            //**************			
            string path = System.Web.HttpContext.Current.Server.MapPath("~/" + pathFicheiro);

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            string strSearch = idFactura + "_*";
            FileInfo[] files = dirInfo.GetFiles(strSearch);
            foreach (FileInfo f in files)
            {
                f.Delete(); //apagar todos os ficheiros que já existem com o mesmo idFactura
            }

            StreamWriter sw = new StreamWriter(ficheiro, false, System.Text.Encoding.GetEncoding(1252));

            try
            {
                DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
                DataTable dt = factura.dtServicosParaFicheiroAXAPTA(idFactura);

                //string orderby = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];

                //int ind = orderby.IndexOf("codTipoEquipamento");
                //if (ind > -1)
                //{
                //    orderby = orderby.Replace("codTipoEquipamento", "descServico"); // nome da coluna mais parecida na bd.	
                //}

                DataView dv = new DataView(dt);
                //dv.Sort = orderby;

                //tentar aplicar esta ordenação à datatable
                //****************************************************************

                //as ultimas 3 colunas são:
                //descServico, identificacaoEquipamento (num ID + num Serie) , refServico, e vão para tres linhas T0
                //por isso são excluídas daqui

                //para juntar linhas:
                //tenho de pôr um contador fora do loop,
                //cada vez que escrevo uma linha normal, incremento, e mando como ultimo campo (ou onde eles o quiserem receber)
                //nas deslocacoes so incremento da primeira vez e nao incremento mas e mando tb como ultimo campo. 

                int iColCount = dt.Columns.Count;
                int iRowNumber = 1;

                foreach (DataRow dr in dt.Rows)
                {

                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i]);
                        }

                        sw.Write(";");  //todos levam tab, porque no fim vou adicionar agora mais um item, o NUMITEM na posicao 22


                    }
                    sw.Write(sw.NewLine);//escreve tudo numa linha



                    iRowNumber += 1; //incrementar o contador 
                }

                factura = null;
                sw.Close();

                updateVersaoFicheiroinBD(idFactura, iVersao);

                lblMessage.Text = "Ficheiro creado.";
                return true;
            }
            catch (Exception ex)
            {

                sw.Close();
                lblMessage.Text = "Error en la creacion del fichero.";

                GERAL.clsWriteError.WriteLog(ex);
                return false;
            }
        }

        //**********************************************************************************************
        //***********************CRIA NOVO FICHEIRO DE FACTURACAO PARA O SAP****************************
        //**********************************************************************************************
        private bool writeSapFile(string idFactura)
        {

            //data da factura
            //tem de ser actualizada antes do ficheiro ser criado
            //uma transaccao aqui nao faz sentido, pois o ficheiro é lido e nao escrito...

            //data da factura
            //tem de ser actualizada antes da factura ser enviada
            string dtFactura = "";

            if (cbForcarData.Checked)
            {
                if (txtDataFacturaForcada.Text != "")
                {
                    dtFactura = txtDataFacturaForcada.Text;
                    updateDataFacturainBD(idFactura, dtFactura); //o webservice dps vai buscar este campo
                    txtDtFactura.Text = txtDataFacturaForcada.Text; //como na ha redirect tenho de actualizar o campo à mao.

                }

                else
                {
                    lblMessage.Text = "Não pode forçar a data sem indicar a data a ser assumida.";
                    return false;

                }
            }

            //versao do ficheiro
            //actualizado depois da factura ter sido criada, mas declarado aqui porque é usado na criacao do ficheiro
            int iVersao = 0;
            try
            {
                iVersao = System.Convert.ToInt16(txtVersao.Text);
            }
            catch
            { }

            string pathFicheiro = (string)ConfigurationManager.AppSettings["FACTURACAO_PATH_REL"];
            string nomeFicheiro = idFactura + "_" + (iVersao + 1).ToString() + ".txt";
            string ficheiro = Server.MapPath("~/" + pathFicheiro + "/" + nomeFicheiro);

            //**************			
            string path = System.Web.HttpContext.Current.Server.MapPath("~/" + pathFicheiro);

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            string strSearch = idFactura + "_*";
            FileInfo[] files = dirInfo.GetFiles(strSearch);
            foreach (FileInfo f in files)
            {
                f.Delete(); //apagar todos os ficheiros que já existem com o mesmo idFactura
            }

            StreamWriter sw = new StreamWriter(ficheiro, false, System.Text.Encoding.GetEncoding(1252));

            try
            {
                DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
                DataTable dt = factura.dtServicosParaFicheiroSAP(idFactura);

                string orderby = ViewState["sortFieldDestino"].ToString() + " " + ViewState["sortDirectionDestino"];

                int ind = orderby.IndexOf("codTipoEquipamento");
                if (ind > -1)
                {
                    orderby = orderby.Replace("codTipoEquipamento", "descServico"); // nome da coluna mais parecida na bd.	
                }

                DataView dv = new DataView(dt);
                dv.Sort = orderby;

                //tentar aplicar esta ordenação à datatable
                //****************************************************************

                //as ultimas 3 colunas são:
                //descServico, identificacaoEquipamento (num ID + num Serie) , refServico, e vão para tres linhas T0
                //por isso são excluídas daqui

                //para juntar linhas:
                //tenho de pôr um contador fora do loop,
                //cada vez que escrevo uma linha normal, incremento, e mando como ultimo campo (ou onde eles o quiserem receber)
                //nas deslocacoes so incremento da primeira vez e nao incremento mas e mando tb como ultimo campo. 

                int iColCount = dt.Columns.Count;  //RECEBO 24 COLUNAS - quero usar 21 para a linha, as ultimas 3 para os comentarios
                                                   //03-2011: recebo 25 colunos, 22 para a linha e as ultimas 3 para os comentarios
                int iRowNumber = 1;

                foreach (DataRow dr in dt.Rows)
                {

                    for (int i = 0; i < iColCount - 3; i++) //n escrever as ultimas 3 colunas na primeira linha e mais a ultima (tem de se dar sempre - 1) 
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i]);
                        }

                        sw.Write("\t");  //todos levam tab, porque no fim vou adicionar agora mais um item, o NUMITEM na posicao 22


                    }
                    sw.Write(sw.NewLine);//escreve tudo numa linha

                    //n aqui é igual ao i la de cima
                    for (int n = iColCount - 3; n < iColCount; n++)
                    {
                        sw.Write("T0");
                        sw.Write("\t");

                        if (!Convert.IsDBNull(dr[n]))
                        {
                            sw.Write(dr[n].ToString());
                        }
                        sw.Write(sw.NewLine); //escreve cada campo numa linha
                    }

                    iRowNumber += 1; //incrementar o contador 
                }


                //DESPESAS DE ENVIO
                //so a partir da segunda linha das deslocacoes é que escrevo o campo agregacao. 

                int iCountLinhas = 0;
                int lastRowNumber = iRowNumber;  //guardar em var


                string deslocacoes = factura.strDeslocacoes(idFactura);

                if ((deslocacoes != "") && (deslocacoes != "0") && (deslocacoes != "000"))
                {


                    DataTable DT = factura.dtDeslocacoes(idFactura);
                    int count = DT.Columns.Count; //as ultimas coluna é:

                    //descServico
                    foreach (DataRow DR in DT.Rows)
                    {
                        for (int i = 0; i < count - 1; i++) //n escrever A ULTIMA row na primeira linha e mais a ultima (tem de se dar sempre - 1)
                        {
                            if (!Convert.IsDBNull(DR[i])) //so os que teem algo são escritos
                            {
                                sw.Write(DR[i]);
                            }
                            sw.Write("\t"); //mas todos levam tab

                        }
                        if (iCountLinhas > 0)
                        {
                            sw.Write(lastRowNumber); //numero a ser agregado, so é escrito a partir da 2a linha das deslocaces
                        }

                        sw.Write(sw.NewLine);

                        //n aqui é igual ao i la de cima
                        for (int n = count - 1; n < count; n++)
                        {
                            sw.Write("T0");
                            sw.Write("\t");

                            if (!Convert.IsDBNull(DR[n]))
                            {
                                sw.Write(DR[n].ToString());
                            }
                            sw.Write(sw.NewLine); //escreve cada campo numa linha
                        }
                        iCountLinhas += 1;
                        iRowNumber += 1; //aumentar a Linha
                    }
                }


                //DESPESAS DE ENVIO + TRANSPORTE
                //so a partir da segunda linha das deslocacoes é que escrevo o campo agregacao. 


                //associar novos valores
                iCountLinhas = 0; //reset	
                lastRowNumber = iRowNumber;  //novo valor ultima linha

                string envio = factura.strDespesasEnvio(idFactura);

                if ((envio != "") && (envio != "0") && (envio != "000"))
                {

                    DataTable DT = factura.dtDespesasEnvio(idFactura);
                    int count = DT.Columns.Count; //as ultimas coluna é:
                    foreach (DataRow DR in DT.Rows)
                    {
                        for (int i = 0; i < count - 1; i++) //n escrever A ULTIMA row na primeira linha e mais a ultima (tem de se dar sempre - 1)
                        {
                            if (!Convert.IsDBNull(DR[i])) //so os que teem algo são escritos
                            {
                                sw.Write(DR[i]);
                            }
                            sw.Write("\t"); //mas todos levam tab

                        }
                        if (iCountLinhas > 0)
                        {
                            sw.Write(lastRowNumber); //numero a ser agregado, so é escrito a partir da 2a linha das deslocaces
                        }

                        sw.Write(sw.NewLine);

                        //n aqui é igual ao i la de cima
                        for (int n = count - 1; n < count; n++)
                        {
                            sw.Write("T0");
                            sw.Write("\t");

                            if (!Convert.IsDBNull(DR[n]))
                            {
                                sw.Write(DR[n].ToString());
                            }
                            sw.Write(sw.NewLine); //escreve cada campo numa linha
                        }

                        iCountLinhas += 1;
                        iRowNumber += 1; //aumentar a Linha
                    }
                }
                //fim envio+tranporte


                factura = null;
                sw.Close();


                updateVersaoFicheiroinBD(idFactura, iVersao);


                lblMessage.Text = "Ficheiro criado com sucesso";
                return true;
            }
            catch (Exception ex)
            {

                sw.Close();
                lblMessage.Text = "Erro na criação do ficheiro";

                GERAL.clsWriteError.WriteLog(ex);
                return false;
            }
        }


        //=================================================================================================
        //faz um update ao campo data factura na factura
        //isto tem de ser feito ANTES do ficheiro ser criado....

        //=================================================================================================
        private void updateDataFacturainBD(string idFactura, string dtFactura)
        {
            string strSQL = "UPDATE FACTURA SET dtFactura = CONVERT(DATETIME, '" + dtFactura + "' , 105)  WHERE idFactura = " + idFactura;
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
        }


        //=================================================================================================
        //incrementa a versao do ficheiro craido em 1 (UM)
        //tb faz um update à data da factura, com a data correspondente ao momento em que o ficheiro é gravado.
        //isto tem de ser feito depois do ficheiro ser criado.
        //=================================================================================================
        private void updateVersaoFicheiroinBD(string idFactura, int iVersao)
        {
            string strSQL = "UPDATE FACTURA SET intVersaoFicheiro = " + (iVersao + 1) + " WHERE idFactura = " + idFactura;
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
        }

        private void saveSAPFacturaNumber(string idFactura, string numFacturaSAP)
        {
            string strSQL = "UPDATE FACTURA SET numFacturaSAP = " + numFacturaSAP + " WHERE idFactura = " + idFactura;
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
        }

        //=================================================================================================
        //CALCULA O VALOR TOTAL DA FACTURA
        //=================================================================================================
        private void calculaValorTotalFactura()
        {
            txtValorTotalFactura.Text = (Double.Parse(replaceByZero(txtValorSubTotal.Text)) - Double.Parse(replaceByZero(txtDescontoTotal.Text)) + Double.Parse(replaceByZero(txtValorAjudasCustoDeslocacoes.Text)) + Double.Parse(replaceByZero(txtValorDespesasEnvio.Text))).ToString();
        }

        //actualiza o campo idRegiao de vendas da empresa (UNICAMENTE)
        private void btnUpdateRegiaoVendasEmpresa_Click(object sender, System.EventArgs e)
        {
            string idEmpresa = ddEmpresa.SelectedValue;
            //if(ddEmpresaContratante.SelectedValue !="")  idEmpresa = ddEmpresaContratante.SelectedValue; 


            string strSQL = "UPDATE Empresa SET idCodigoRegiaoVendas = " + ddRegiaoVendasEmpresa.SelectedValue.ToString() + " WHERE idEmpresa = " + idEmpresa;

            lblMessage.Text = GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);

        }


        //=================================================================================================
        //VALIDA SE EXISTEM CALIBRAÇÕES EXTERNAS NAS LINHAS DA FACTURA, SE SIM, A REGIAO DE VENDAS TEM DE SER PREENCHIDA.
        //isto nao funciona bem bem, pq podem ter seleccionado a dropdown e terem-se esquecido de submeter....
        //=================================================================================================
        private bool validaRegiaoVendas(DataTable dt)
        {

            if (myApp == "ISQ_LABMETRO")
            {
                DataRow[] dRow = dt.Select(null, null, DataViewRowState.CurrentRows);
                foreach (DataRow dr in dRow)
                {

                    if (dr["calibracaoExterna"].ToString() == "True") //basta encontrar UM
                    {
                        if (ddRegiaoVendasEmpresa.SelectedValue == "")
                        {
                            return false;
                        }
                        else
                        {
                            if (validaRegiaoVendasBD()) return true;
                            else return false;
                        }
                    }
                    return true;
                }
                return true;
            }
            else
            {
                return true;
            }
        }


        private bool validaRegiaoVendasBD()
        {
            string strSQL = "SELECT idCodigoRegiaoVendas FROM Empresa WHERE idEmpresa = " + ddEmpresa.SelectedValue.ToString();
            object o = GERAL.clsDataAccess.myExecuteScalar(strSQL);
            if ((!Convert.IsDBNull(o)) && Convert.ToInt16(o) > 0) return true;
            return false;
        }
        //=================================================================================================
        //VALIDA quantas requisições existem associadas aos serviços a serem facturados.
        //=================================================================================================
        private void validaRequisicoes(DataTable dt)
        {
            Hashtable ht = new Hashtable();
            DataView dv = new DataView(dt);
            dv.RowStateFilter = DataViewRowState.CurrentRows;
            foreach (DataRowView drv in dv)
            {
                if (drv["idRequisicao"].ToString() != "")
                {
                    try
                    {
                        ht.Add(drv["idRequisicao"], drv["referenciaCliente"]);
                    }
                    catch
                    {
                        //nada, ele aqui nao adiciona os items repetidos, que é o q se pretende.
                    }
                }
            }

            int i = ht.Count;
            if (i == 0) //nao ha nenhuma requisicao
            {
                lblErroRequisicoes.Text = "Há linhas de Serviço sem Requisição.";
                //return false; 
            }
            else if (i > 1)
            {
                lblErroRequisicoes.Text = "Há mais de uma Requisição nas linhas de Serviço.";
                //return false; 
            }
            //return true; 
        }

        private bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }


        private DataTable SelectDistinct(DataTable SourceTable, string FieldName)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;

            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    DT.Rows.Add(new object[] { LastValue });
                }
            }
            //			if (ds != null) 
            //				ds.Tables.Add(dt);
            return DT;
        }

        //private void insere_WS_SAPTeste()
        //{

        //    //confirmar os campos
        //    //fazer uma stored procedure para ir buscar o cabeçalho à parte (dados da factura)
        //    //uma para ir buscar as linhas da factura 
        //    ZisqCreateCustOrder order = new ZisqCreateCustOrder();

        //    //cabecalho da factura - 1 vez // dado
        //    var hdrSalesOrder = new ZisqSHdrSalesOrder()
        //    {
        //        DocType = "ZRET", //FIXO
        //        SalesOrg = "ISQ1", // FIXO
        //        Division = "55", //FIXO
        //        SalesOff = "1001",  //VAR
        //        DistrChan = "RE", //FIXO - NO CASO DA METROLOGIA, SEMPRE RETALHO
        //        ZpurchDate = "08-06-2020", //DATA DO PEDIDO - USAMOS A DATA DO BRE QUE ESTÁ A SER FACTURADO
        //        PartnNumb = "104289", //numcliente
        //        Pmnttrms = "0002", //payment terms - CONDIÇÕES DE PAGAMENTO
        //        PurchNoC = "pedido-ontem", //numero do pedido REQUISICAO
        //        Bname = "mccampos", //USERNAME DO FUNCIONARIO QUE EMITIU
        //    };

        //    order.IsHdrSalesOrder = hdrSalesOrder; //header

        //    //linhas da factura - multiplas veze
        //    var itmSalesOrder = new ZisqSItmSalesOrder();

        //    itmSalesOrder.PartnNumb = "104289"; //num cliente outra vez
        //    itmSalesOrder.ItmNumber = null; //Item number null (ou vazio)
        //    itmSalesOrder.Material = "1100000413"; //codigo de serviço. vem da grandeza mas há de vir da familia
        //    itmSalesOrder.WbsElem = "20000515655"; //PEP
        //    itmSalesOrder.SalesDist = "100011";//regiao de vendas
        //    itmSalesOrder.ReqQty = "1.00"; //sempre mandamos 1 
        //    itmSalesOrder.Posex = null; //mandamos null agora
        //    itmSalesOrder.ShortText = null;//breve descircao mandamos a null
        //    itmSalesOrder.Zfkdat = "09-06-2020"; //data da factura
        //    itmSalesOrder.Dtpserv = "Maio-2020"; //data prestacao serviço, nao sei se é o mesmo formato que nós usamos agora
        //    //MES-ANO COMO CALCULADO DA DATA DA FACTURA
        //    itmSalesOrder.CondValue = "0.00";//  Isto deve ser o valor tb, Montante de condição, joao diz que é o valor total - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL FALAR COM ALGUEM DA FACTURACAO
        //    itmSalesOrder.Fakwr = "150.50"; // Valor a faturar / calcular na data do prog./ plano facturamento - o campo deverá ser //preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.
        //    itmSalesOrder.Fproz = "100.00";// Percentagem do valor a faturar - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL


        //    //LINHAS DE DESCRICAO, SAO SEMPRE 3
        //    List<ZisqSIntordtx> linhas = new List<ZisqSIntordtx>();
        //    for (int i = 0; i < 3; i++) //3 linhas
        //    {
        //        string s1 = "T0";
        //        string s2 = "descrição do serviço";
        //        linhas.Add(new ZisqSIntordtx() { Tplin = s1, Textl = s2 });

        //    }

        //    //aqui ainda faltam as linhas das deslocacoes.
        //    //////////////////////////////////////////////////

        //    itmSalesOrder.TextTab = linhas.ToArray();

        //    ZisqSItmSalesOrder[] salesItems = new[] { itmSalesOrder };

        //    order.ItItmSalesOrder = salesItems;

        //    //ESTES 2 ESTAO FORA DO RESTO
        //    order.IvTest = ""; //tipo de documento nao faço ideia, pode ir a null ou vazio ZRET?
        //    order.IvFaksp = ""; //organizacao de vendas ISQ1

        //    ZISQ_WS_CUST_ORDER_PROVIDERClient client = new ZISQ_WS_CUST_ORDER_PROVIDERClient("ConfigSL");
        //    client.ClientCredentials.UserName.UserName = "WEBSERVICE";
        //    client.ClientCredentials.UserName.Password = "WEBSERVICE";

        //    ZisqCreateCustOrderResponse response = client.ZisqCreateCustOrder(order);
        //    //ZisqSMessage[] responses = response.EtMessages;

        //    Response.Write(response.EtMessages[0].Message.ToString());
        //    Response.Write(response.EtMessages[1].Message.ToString());
        //    Response.Write(response.EtMessages[2].Message.ToString());
        //}



        private void InsereSAPWS(string idFactura)
        {

            //data da factura
            //tem de ser actualizada antes da factura ser enviada
            string dtFactura = "";

            if (cbForcarData.Checked)
            {
                if (txtDataFacturaForcada.Text != "")
                {
                    dtFactura = txtDataFacturaForcada.Text;
                    updateDataFacturainBD(idFactura, dtFactura); //o webservice dps vai buscar este campo
                    txtDtFactura.Text = txtDataFacturaForcada.Text; //como na ha redirect tenho de actualizar o campo à mao.
                }
                else
                {
                    lblMessage.Text = "Não pode forçar a data sem indicar a data a ser assumida.";
                    return;
                }
            }

            //O CONTADOR DO NUMERO DE LINHAS L0 QUE A FACTURA VAI TER PARA AGRUPAMENTO QUANDO NECESSÁRIO (DESLOCACOES E DESPESAS ENVIO)
            int iRowNumber = 1;

            try
            {
                DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
                SqlDataReader dr = factura.drCabecalhoFacturaParWebServiceSAP(idFactura);

                //criar a ORDER
                ZisqCreateCustOrder order = new ZisqCreateCustOrder();

                while (dr.Read())
                {
                    //ESCREVER O HEADER
                    var hdrSalesOrder = new ZisqSHdrSalesOrder()
                    {
                        DocType = dr["tipoDocumento"].ToString(), //"ZRET" // FIXO
                        SalesOrg = dr["OrganizacaoVendas"].ToString(),//"ISQ1", // FIXO
                        Division = dr["sectorActividade"].ToString(),//"56", //FIXO
                        SalesOff = dr["escritorioVendas"].ToString(),//"1001",  //VAR
                        DistrChan = dr["canalDistribuicao"].ToString(),//"RE", // SEMPRE RETALHO
                        ZpurchDate = dr["dataPedido"].ToString(),//"01-04-2020", //DATA DO PEDIDO 
                        PartnNumb = dr["numCliente"].ToString(),//"300631", //numcliente
                        Pmnttrms = dr["condicoesPagamento"].ToString(),//"0002", //payment terms --> NULLABLE
                        PurchNoC = dr["numPedido"].ToString(),//"9000049465", //numero do pedido REQUISICAO
                        Bname = dr["username"].ToString(), //"mccampos", //USERNAME DO FUNCIONARIO QUE EMITIU

                    };

                    //ADICIONAR O HEADER À ORDER
                    order.IsHdrSalesOrder = hdrSalesOrder; //header
                }

                //linhas, esta lista será preenchida primeiro pelas linhas da factura e dps linhas de deslocações e linhas de despesas de envio.

                List<ZisqSItmSalesOrder> orderLinhas = new List<ZisqSItmSalesOrder>();

                DataTable dtLinhasServicos = factura.dtLinhasFacturaParWebServiceSAP(idFactura);
                DataView dv = new DataView(dtLinhasServicos);
                dv.Sort = "descServico"; //nao sei para que servia mas so existe nas linhas serviço //nas outras chama-se descricaoServico

                foreach (DataRow drow in dtLinhasServicos.Rows)
                {

                    var itmSalesOrder = new ZisqSItmSalesOrder();
                    {
                        itmSalesOrder.PartnNumb = drow["numCliente"].ToString(); // "300631"; //num cliente outra vez
                        itmSalesOrder.ItmNumber = ""; // drow["numItem"].ToString(); //null; //Item number  (ou vazio) --> NULLABLE
                        itmSalesOrder.Material = drow["codigoServico"].ToString(); //"1100000413"; //codigo de serviço. 
                        itmSalesOrder.WbsElem = drow["elementoPEP"].ToString(); //"20000515655"; //PEP
                        itmSalesOrder.SalesDist = drow["regiaoVendas"].ToString(); //100011";//regiao de vendas
                        itmSalesOrder.ReqQty = "1.00"; // drow["quantidade"].ToString(); //"1"; //sempre mandamos 1 
                        itmSalesOrder.ShortText = "";  //breve descricao mandamos a null --> NULLABLE
                        itmSalesOrder.Zfkdat = drow["dataFactura"].ToString();  //data da factura
                        itmSalesOrder.Dtpserv = drow["dataPrestacaoServico"].ToString();  //data prestacao serviço, nao sei se é o mesmo formato que nós usamos agora   //MES-ANO COMO CALCULADO DA DATA DA FACTURA    
                        itmSalesOrder.CondValue = drow["cond_value"].ToString(); // "0.00";//  Isto deve ser o valor tb, Montante de condição, joao diz que é o valor total - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL FALAR COM ALGUEM DA FACTURACAO
                        itmSalesOrder.Fakwr = drow["valorFacturar"].ToString(); //"150.50"; // Valor a faturar //preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto
                        itmSalesOrder.Fproz = "100.00"; //drow["percValorFacturar"].ToString(); //"100.00";// Percentagem do valor a faturar - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL
                        itmSalesOrder.Posex = ""; //POSICAO, UTILIZADO PARA AGRUPAR LINHAS
                    }

                    // ======================================
                    //LINHAS DE DESCRICAO, SAO SEMPRE 3 nos serviços, e 1 nas deslocacoes e despesesas de envio
                    List<ZisqSIntordtx> linhas = new List<ZisqSIntordtx>();
                    //n aqui é igual ao i la de cima
                    int iColCount = 16; //numero de campos que recebo nas linhas de serviço
                                        //os 3 ultimos vao para as linhas TO
                    for (int n = iColCount - 3; n < iColCount; n++)
                    {

                        string s1 = "T0";
                        string s2 = drow[n].ToString();
                        linhas.Add(new ZisqSIntordtx() { Tplin = s1, Textl = s2 });
                    }

                    iRowNumber += 1; //incrementar o contador DAS LINHAS L0 
                    itmSalesOrder.TextTab = linhas.ToArray();

                    //adicionar ao Array das Linhas
                    orderLinhas.Add(itmSalesOrder);

                }

                int groupRowNumber = 0; //nao sei //so serve para agrupamento la em baixo mas so deve ser incremetnado se existem linhas de deslocao ou ou despesas envio
                //DESLOCACOES

                DataTable dtLinhasDeslocacoes = factura.dtDeslocacoesParWebServiceSAP(idFactura);
                dv = new DataView(dtLinhasDeslocacoes);

                //as deslocacoes vao em várias linhas divididas por PEP mas teem de ser agrupadas na factura que será enviada ao cliente, 
                //suponho que pelo campo POSEX? confirmar
                int i = 1; //so na primeira vez é que nao escreve nada na posicao, na 2a vez tem de escrever sempre a posicao da linha anterior
                if (dv.Count > 0)
                {
                    groupRowNumber = iRowNumber; // é igual para todas as linhas das deslocacao, fica na posicao da primeira linha de deslocacao
                }
                foreach (DataRow drow in dtLinhasDeslocacoes.Rows) // nao executa se nao houver linhas
                {
                    //linhas da factura - multiplas veze
                    var itmSalesOrder = new ZisqSItmSalesOrder();
                    {
                        itmSalesOrder.PartnNumb = drow["numCliente"].ToString(); // "300631"; //num cliente outra vez
                        itmSalesOrder.ItmNumber = ""; // drow["numItem"].ToString(); //null; //Item number  (ou vazio) --> NULLABLE
                        itmSalesOrder.Material = drow["codigoServico"].ToString(); //"1100000413"; //codigo de serviço. 
                        itmSalesOrder.WbsElem = drow["elementoPEP"].ToString(); //"20000515655"; //PEP
                        itmSalesOrder.SalesDist = drow["regiaoVendas"].ToString(); //100011";//regiao de vendas
                        itmSalesOrder.ReqQty = "1.00"; // drow["quantidade"].ToString(); //"1"; //sempre mandamos 1 
                        itmSalesOrder.ShortText = "";  //breve descricao mandamos a null --> NULLABLE
                        itmSalesOrder.Zfkdat = drow["dataFactura"].ToString();  //data da factura
                        itmSalesOrder.Dtpserv = drow["dataPrestacaoServico"].ToString();  //data prestacao serviço, nao sei se é o mesmo formato que nós usamos agora   //MES-ANO COMO CALCULADO DA DATA DA FACTURA    
                        itmSalesOrder.CondValue = drow["cond_value"].ToString(); // "0.00";//  Isto deve ser o valor tb, Montante de condição, joao diz que é o valor total - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL FALAR COM ALGUEM DA FACTURACAO
                        itmSalesOrder.Fakwr = drow["valorFacturar"].ToString(); //"150.50"; // Valor a faturar //preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto
                        itmSalesOrder.Fproz = "100.00"; //drow["percValorFacturar"].ToString(); //"100.00";// Percentagem do valor a faturar - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL
                        if (i == 1) //dps da primeira row junta sempre este campo
                        {
                            itmSalesOrder.Posex = "";

                        }
                        else
                        {
                            itmSalesOrder.Posex = (groupRowNumber).ToString();
                        }

                        i = i + 1;
                        // null; ========ATENÇÃO:NAO SEI SE O CAMPO POSEX É USADO PARA ISTO ====================

                    }

                    // ======================================
                    //LINHAS DE DESCRICAO, SAO SEMPRE 3 nos serviços, e 1 nas deslocacoes e despesesas de envio
                    List<ZisqSIntordtx> linhas = new List<ZisqSIntordtx>();
                    //n aqui é igual ao i la de cima
                    int iColCount = 13; //numero de campos que recebo nas deslocacoes
                                        //so um campo para a linha T0
                    for (int n = iColCount - 1; n < iColCount; n++)
                    {

                        string s1 = "T0";
                        string s2 = drow[n].ToString();
                        linhas.Add(new ZisqSIntordtx() { Tplin = s1, Textl = s2 });
                    }

                    iRowNumber += 1; //incrementar o contador DAS LINHAS L0 
                    itmSalesOrder.TextTab = linhas.ToArray();
                    //adicionar ao Array das Linhas
                    orderLinhas.Add(itmSalesOrder);


                }

                //DESPESAS DE ENVIO

                DataTable dtLinhasDepesasEnvio = factura.dtDespesasEnvioParWebServiceSAP(idFactura);
                dv = new DataView(dtLinhasDepesasEnvio);

                //as deslocacoes vao em várias linhas divididas por PEP mas teem de ser agrupadas, 

                i = 1; //so na primeira vez é que nao escreve nada na posicao, na 2a vez tem de escrever sempre a posicao da linha anterior
                if (dv.Count > 0)
                {
                    groupRowNumber = iRowNumber; // é igual para todas as linhas das deslocacao, fica na posicao da primeira linha de despeas
                }
                foreach (DataRow drow in dtLinhasDepesasEnvio.Rows) //nao executa se nao houver linhas
                {
                    //linhas da factura - multiplas veze
                    var itmSalesOrder = new ZisqSItmSalesOrder();
                    {
                        itmSalesOrder.PartnNumb = drow["numCliente"].ToString(); // "300631"; //num cliente outra vez
                        itmSalesOrder.ItmNumber = ""; // drow["numItem"].ToString(); //null; //Item number  (ou vazio) --> NULLABLE
                        itmSalesOrder.Material = drow["codigoServico"].ToString(); //"1100000413"; //codigo de serviço. 
                        itmSalesOrder.WbsElem = drow["elementoPEP"].ToString(); //"20000515655"; //PEP
                        itmSalesOrder.SalesDist = drow["regiaoVendas"].ToString(); //100011";//regiao de vendas
                        itmSalesOrder.ReqQty = "1.00"; // drow["quantidade"].ToString(); //"1"; //sempre mandamos 1 
                        itmSalesOrder.ShortText = "";  //breve descricao mandamos a null --> NULLABLE
                        itmSalesOrder.Zfkdat = drow["dataFactura"].ToString();  //data da factura
                        itmSalesOrder.Dtpserv = drow["dataPrestacaoServico"].ToString();  //data prestacao serviço, nao sei se é o mesmo formato que nós usamos agora   //MES-ANO COMO CALCULADO DA DATA DA FACTURA    
                        itmSalesOrder.CondValue = drow["cond_value"].ToString(); // "0.00";//  Isto deve ser o valor tb, Montante de condição, joao diz que é o valor total - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL FALAR COM ALGUEM DA FACTURACAO
                        itmSalesOrder.Fakwr = drow["valorFacturar"].ToString(); //"150.50"; // Valor a faturar //preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto
                        itmSalesOrder.Fproz = "100.00"; //drow["percValorFacturar"].ToString(); //"100.00";// Percentagem do valor a faturar - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL
                        if (i == 1) //dps da primeira row junta sempre este campo
                        {
                            itmSalesOrder.Posex = "";


                        }
                        else
                        {
                            itmSalesOrder.Posex = groupRowNumber.ToString(); //está bem e testado, o i serve para duas coisas, p
                        }

                        i = i + 1; //acima de 1 fica sempre junto, so seria preciso incrementar o contador uma vez
                        // null; ========ATENÇÃO:NAO SEI SE O CAMPO POSEX É USADO PARA ISTO ====================
                        // drow["itemPed"].ToString(); ???? 
                        // ou   é a posicao dentro das linhas SE CALHAR É O CAMPO PARA JUNTAR AS LINHAS?
                    }

                    // ======================================
                    //LINHAS DE DESCRICAO, SAO SEMPRE 3 nos serviços, e 1 nas deslocacoes e despesesas de envio
                    List<ZisqSIntordtx> linhas = new List<ZisqSIntordtx>();
                    //n aqui é igual ao i la de cima
                    int iColCount = 13; //numero de campos que recebo nas deslocacoes
                                        //so um campo para a linha T0
                    for (int n = iColCount - 1; n < iColCount; n++)
                    {

                        string s1 = "T0";
                        string s2 = drow[n].ToString();
                        linhas.Add(new ZisqSIntordtx() { Tplin = s1, Textl = s2 });
                    }

                    iRowNumber += 1; //incrementar o contador DAS LINHAS L0 
                    itmSalesOrder.TextTab = linhas.ToArray();
                    //adicionar ao Array das Linhas
                    orderLinhas.Add(itmSalesOrder);


                }

                //ADICIONAR TODAS AS LINHAS À ORDER
                order.ItItmSalesOrder = orderLinhas.ToArray();
                //nao sei o que  é isso que está aqui fora
                order.IvTest = ""; //tipo de documento nao faço ideia, pode ir a null ou vazio TEM DE IR A NULL SENAO DÁ ERRO 
                order.IvFaksp = ""; //organizacao de vendas TEM DE IR À NULL SENAO DÁ ERRO



                ZISQ_WS_CUST_ORDER_PROVIDERClient client = new ZISQ_WS_CUST_ORDER_PROVIDERClient("ConfigSL");
                client.ClientCredentials.UserName.UserName = "WEBSERVICE";
                client.ClientCredentials.UserName.Password = "WEBSERVICE";

                ZisqCreateCustOrderResponse response = client.ZisqCreateCustOrder(order);
                ZisqSMessage[] responses = response.EtMessages;
                string invoiceNumber = response.EvOrder.ToString();

                lblMessage.Text = response.EtMessages[0].Message.ToString() + response.EtMessages[1].Message.ToString();

                factura = null;

                if (invoiceNumber != "")
                {
                    saveSAPFacturaNumber(idFactura, invoiceNumber);
                    lblMessage.Text = "Ordem criada no SAP com o número: " + invoiceNumber;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = "Erro na integração da Factura por WS." + ex.ToString();

                GERAL.clsWriteError.WriteLog(ex);

            }
        }

        protected void btn_WS_Click(object sender, EventArgs e)
        {

            if ((txtNumClienteSAP.Text == "" || txtNumClienteSAP.Text.StartsWith("500000") == true) && ddCondPagamentoFactura.SelectedValue != "1")
            //a rigor, para pronto pagamento n precisa 
            //de ter numero de cliente sap.
            {
                lblMessage.Text = "Empresa sem Núm. Cliente. Num 500000 apenas para VD. Actualize a empresa para gerar ordens gerar ordens de venda.";
                return;
            }


            //insere_WS_SAPTeste();// (ViewState["idFactura"].ToString());
            InsereSAPWS(ViewState["idFactura"].ToString());
          

        }


        private void InsereSAPWSInterno(string idFactura)
        {
            string dtFactura = "";
            if (cbForcarData.Checked)
            {
                if (txtDataFacturaForcada.Text != "")
                {
                    dtFactura = txtDataFacturaForcada.Text;
                    updateDataFacturainBD(idFactura, dtFactura); //o webservice dps vai buscar este campo
                    txtDtFactura.Text = txtDataFacturaForcada.Text; //como na ha redirect tenho de actualizar o campo à mao.
                }
                else
                {
                    lblMessage.Text = "Não pode forçar a data sem indicar a data a ser assumida.";
                    return;
                }
            }

            //O CONTADOR DO NUMERO DE LINHAS L0 QUE A FACTURA VAI TER PARA AGRUPAMENTO QUANDO NECESSÁRIO (DESLOCACOES E DESPESAS ENVIO)
            int iRowNumber = 1;

            try
            {
                DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
                SqlDataReader dr = factura.drCabecalhoFacturaParWebServiceSAP(idFactura);

                //criar a ORDER
                ZISQ_CREATE_CUST_ORDER_FATINT order = new ZISQ_CREATE_CUST_ORDER_FATINT();

                while (dr.Read())
                {
                    //ESCREVER O HEADER
                    var hdrSalesOrder = new ZISQ_S_HDR_SALES_ORDER()
                    {
                        DOC_TYPE = dr["tipoDocumento"].ToString(), //"ZRET" // FIXO
                        SALES_ORG = dr["OrganizacaoVendas"].ToString(),//"ISQ1", // FIXO
                        DIVISION = dr["sectorActividade"].ToString(),//"56", //FIXO
                        SALES_OFF = dr["escritorioVendas"].ToString(),//"1001",  //VAR
                        DISTR_CHAN = dr["canalDistribuicao"].ToString(),//"RE", // SEMPRE RETALHO
                        ZPURCH_DATE = dr["dataPedido"].ToString(),//"01-04-2020", //DATA DO PEDIDO 
                        PARTN_NUMB = dr["numCliente"].ToString(),//"300631", //numcliente --> tem de vir um numCliente ZRET 2
                        PMNTTRMS = dr["condicoesPagamento"].ToString(),//"0002", //payment terms --> NULLABLE
                        PURCH_NO_C = dr["numPedido"].ToString(),//"9000049465", //numero do pedido REQUISICAO
                        BNAME = dr["username"].ToString(), //"mccampos", //USERNAME DO FUNCIONARIO QUE EMITIU

                    };
                    //ADICIONAR O HEADER À ORDER
                    order.IS_HDR_SALES_ORDER = hdrSalesOrder; //header
                }

                //linhas
                List<ZISQ_S_ITM_SALES_ORDER_FATINT> orderLinhas = new List<ZISQ_S_ITM_SALES_ORDER_FATINT>();

                DataTable dtLinhasServicos = factura.dtLinhasFacturaParWebServiceSAP(idFactura);
                DataView dv = new DataView(dtLinhasServicos);
                dv.Sort = "descServico"; //nao sei para que servia mas so existe nas linhas serviço //nas outras chama-se descricaoServico

                foreach (DataRow drow in dtLinhasServicos.Rows)
                {
                    var itmSalesOrder = new ZISQ_S_ITM_SALES_ORDER_FATINT();
                    {
                        itmSalesOrder.PARTN_NUMB = drow["numCliente"].ToString(); // "300631"; //num cliente outra vez
                        itmSalesOrder.ITM_NUMBER = ""; // drow["numItem"].ToString(); //null; //Item number  (ou vazio) --> NULLABLE
                        itmSalesOrder.MATERIAL = drow["materialSAP"].ToString();//material tem de vir da tabela SUBTIPO, mas o serviço continua associado ao codigoservico normal, isso nao foi alterdado

                        itmSalesOrder.WBS_ELEM = drow["elementoPEP"].ToString(); //"20000515655"; //PEP
                        itmSalesOrder.SALES_DIST = drow["regiaoVendas"].ToString(); //100011";//regiao de vendas
                        itmSalesOrder.REQ_QTY = "1.00"; // drow["quantidade"].ToString(); //"1"; //sempre mandamos 1 
                        itmSalesOrder.SHORT_TEXT = "";  //breve descricao mandamos a null --> NULLABLE
                        itmSalesOrder.ZFKDAT = drow["dataFactura"].ToString();  //data da factura
                        itmSalesOrder.DTPSERV = drow["dataPrestacaoServico"].ToString();  //data prestacao serviço, nao sei se é o mesmo formato que nós usamos agora   //MES-ANO COMO CALCULADO DA DATA DA FACTURA    
                        itmSalesOrder.COND_VALUE = drow["cond_value"].ToString(); // "0.00";//  Isto deve ser o valor tb, Montante de condição, joao diz que é o valor total - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL FALAR COM ALGUEM DA FACTURACAO
                        itmSalesOrder.FAKWR = drow["valorFacturar"].ToString(); //"150.50"; // Valor a faturar //preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto
                        itmSalesOrder.FPROZ = "100.00"; //drow["percValorFacturar"].ToString(); //"100.00";// Percentagem do valor a faturar - o campo deverá ser preenchido com duas casas decimais, em que o separador decimal deverá ser o ponto.NOT NULL
                        itmSalesOrder.POSEX = ""; //POSICAO, UTILIZADO PARA AGRUPAR LINHAS

                        itmSalesOrder.ZZCOD = drow["ZZCOD"].ToString(); 
                        itmSalesOrder.ZZDEP = "LAB"; //fixo
                    }

                    // ======================================
                    //LINHAS DE DESCRICAO, SAO SEMPRE 3 nos serviços, e 1 nas deslocacoes e despesesas de envio
                    List<ZISQ_S_INTORDTX> linhas = new List<ZISQ_S_INTORDTX>();
                    //n aqui é igual ao i la de cima
                    int iColCount = 16; //numero de campos que recebo nas linhas de serviço
                                        //os 3 ultimos vao para as linhas TO
                    for (int n = iColCount - 3; n < iColCount; n++)
                    {
                        string s1 = "T0";
                        string s2 = drow[n].ToString();
                        linhas.Add(new ZISQ_S_INTORDTX() { TPLIN = s1, TEXTL = s2 });
                    }

                    iRowNumber += 1; //incrementar o contador DAS LINHAS L0 
                    itmSalesOrder.TEXT_TAB = linhas.ToArray();

                    //adicionar ao Array das Linhas
                    orderLinhas.Add(itmSalesOrder);

                }

                //int groupRowNumber = 0; //nao sei //so serve para agrupamento la em baixo mas so deve ser incremetnado se existem linhas de deslocao ou ou despesas envio
                //NAO HA NEM DESCLOCACAO NEM DESPESAS DE ENVIO

                //ADICIONAR TODAS AS LINHAS À ORDER
                order.IT_ITM_SALES_ORDER = orderLinhas.ToArray();
                //nao sei o que  é isso que está aqui fora
                order.IV_TEST = ""; //tipo de documento nao faço ideia, pode ir a null ou vazio TEM DE IR A NULL SENAO DÁ ERRO 
                order.IV_FAKSP = ""; //organizacao de vendas TEM DE IR À NULL SENAO DÁ ERRO

                ZISQ_WS_CUST_ORDER_PROV_FATINTClient client = new ZISQ_WS_CUST_ORDER_PROV_FATINTClient("ZWS_SALESORDER_PROV_FATINT");
                client.ClientCredentials.UserName.UserName = "WEBSERVICE";
                client.ClientCredentials.UserName.Password = "WEBSERVICE";

                //AQUI
                ZISQ_CREATE_CUST_ORDER_FATINTResponse response = client.ZISQ_CREATE_CUST_ORDER_FATINT(order);
                ZISQ_S_MESSAGE[] responses = response.ET_MESSAGES;
                string invoiceNumber = response.EV_ORDER.ToString();

                lblMessage.Text = response.ET_MESSAGES[0].MESSAGE.ToString() + response.ET_MESSAGES[0].MESSAGE_CODE.ToString()+ response.ET_MESSAGES[0].MESSAGE_TYPE.ToString();

                factura = null;

                if (invoiceNumber != "")
                {
                    saveSAPFacturaNumber(idFactura, invoiceNumber);
                    lblMessage.Text = "Ordem de venda interna criada no SAP com o número: " + invoiceNumber;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Erro na integração da Ordem Interna por Webservice " + ex.ToString();
                GERAL.clsWriteError.WriteLog(ex);
            }
        }
        protected void btn_WS_Interno_Click(object sender, EventArgs e)
        {
            //so pode inserir aqui se o numero de cliente é grupo de contas v012
            InsereSAPWSInterno(ViewState["idFactura"].ToString());
            ///aasdasd
            ////////ooooooooooo
        }
    }
}
