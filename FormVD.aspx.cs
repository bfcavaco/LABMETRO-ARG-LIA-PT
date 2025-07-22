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
using System.Text.RegularExpressions; 

namespace LabMetro
{
    public partial class FormVD : System.Web.UI.Page
    {

        private double dbValorSubTotal = 0;
        private double dbValorDescontoTotal = 0;

        private const string ID_PAG = "VD_1";//NOME DA PAGINA
        
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
            
            ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            ddBRE.SelectedIndexChanged += new System.EventHandler(ddBRE_SelectedIndexChanged);
            btnSubmitGrid.Click += new System.EventHandler(btnSubmitGrid_Click);
            dgDestino.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(dgDestino_DeleteCommand);
            txtValorAjudasCustoDeslocacoes.TextChanged += new System.EventHandler(txtValorAjudasCustoDeslocacoes_TextChanged);
            txtValorDespesasEnvio.TextChanged += new System.EventHandler(txtValorDespesasEnvio_TextChanged);
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            //btnReset.Click += new System.EventHandler(btnReset_Click);
            btnRequisicoes.Click += new System.EventHandler(btnRequisicoes_Click);
            txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
            txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
            btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
            //btnVerFactura.Click += new System.EventHandler(btnVerFactura_Click);

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
                        //btnReset.Enabled = false;
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
                                fillEmpresaData_SAP(); //preenche os dados sap da empresa ou empresa contratante
                                fillEmpresaDevedora(); //prenche os dados devedores da empresa ou empresa contratante
                            }
                        }
                        //form vazio
                        else
                        {
                            ViewState["idFactura"] = null;

                            btnSubmit.CommandArgument = "insert";

                            fillEmpresas(); //preenche empresas
                            fillBREs(); //preenche bre's
                            fillEmpresaContratanteDoBRE(); //preenche empresas contratantes
                            fillEmpresaData_SAP(); //preenche os dados SAP da empresa ou empresa contratante
                            fillEmpresaDevedora(); //preenche se empresa ou emp.cont. é devedora

                            setFacturaData(); //para inicializar alguns campos a 0
                            
                            //btnVerFactura.Enabled = false;
                            btnVD.Enabled = false;
                            btnImprimir.Enabled = false;
                            
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
            DTOrigem = f.DTGetServicosForVD(ddBRE.SelectedValue);
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
            string id;

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

                    idFactura = factura.InsertFacturaWithLinhas(ddBRE.SelectedValue.ToString(), ddEmpresa.SelectedValue.ToString(), txtValorDespesasEnvio.Text, txtValorAjudasCustoDeslocacoes.Text, txtValorTotalFactura.Text, txtObservacoes.Text, User.Identity.Name.ToString(), DTDestino, ddTipoDocumento.SelectedValue.ToString(), ddCanalDistribuicao.SelectedValue, ddEscritorioVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue,null, null,null,null).ToString(); //Não juntar estas linhas
                    //senao entra 2 vezes na mesma função. 
                    id = idFactura;

                    if (id == "0")
                    {
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
                    }
                    else
                    {
                        //Server.Transfer("FormFacturaSAP.aspx?btn=FAC&id=" + id,false);  //Deixar a false senao entra - nao sei porquê - novamente na função insertfacturawithlinha e dps dá erro.
                        Response.Redirect("FormVD.aspx?btn=FAC&id=" + idFactura, true);
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
                    DATA.FacturaData factura = new LabMetro.DATA.FacturaData();

                    string idFactura = ViewState["idFactura"].ToString();

                    int retvalue = factura.UpdateFacturaWithLinhas(ViewState["idFactura"].ToString(), txtValorDespesasEnvio.Text.ToString(), txtValorAjudasCustoDeslocacoes.Text.ToString(), txtValorTotalFactura.Text.ToString(), txtObservacoes.Text.ToString(), User.Identity.Name.ToString(), DTOrigem, DTDestino, ddTipoDocumento.SelectedValue, ddCanalDistribuicao.SelectedValue, ddEscritorioVendas.SelectedValue, ddCondPagamentoFactura.SelectedValue, ViewState["idModif"].ToString(),"",txtDtFactura.Text,null,null);

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
                            string id = Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

                            ViewState["idModif"] = id;
                        }
                        catch (Exception ex)
                        {
                            GERAL.clsWriteError.WriteLog(ex);
                        }
                    }

                    factura = null;
                }
            }
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
            DataTable DT = f.DTListaEmpresasForVD(txtPesquisaEmpresa.Text, txtPesquisaNif.Text, txtPesquisaNumClienteSAP.Text);
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

                strSQL = "SELECT E.observacoes, E.numClienteSAP, E.codigoBloqueioSap, E.idCondicoesPagamento, E.bPodeFacturarSemRequisicao, E.NIF FROM EMPRESA E  WHERE E.idEmpresa = " + idEmpresa;

                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lblObsEmpresa.Text = dr["observacoes"].ToString();
                        txtNumClienteSAP.Text = dr["numClienteSAP"].ToString();
                        txtNif.Text = dr["NIF"].ToString();

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
                    dr.Close();
                }
            }

            //para a regiao de vendas. 
            idEmpresa = ddEmpresa.SelectedValue;
            if (idEmpresa != "")
            {
                strSQL = "SELECT E.idCodigoRegiaoVendas, L.descricao AS localidade FROM EMPRESA E LEFT JOIN LOCALIDADE L on E.idLocalidade = L.idLocalidade WHERE idEmpresa = " + idEmpresa;

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

            //ddCondPagamentoFactura.DataTextField = "descricao";
            //ddCondPagamentoFactura.DataValueField = "id";
            //ddCondPagamentoFactura.DataSource = dr;
            //ddCondPagamentoFactura.DataBind();

            //dr.Close();
            //dr = null;
            ddCondPagamentoFactura.Items.Insert(0,new ListItem("Pronto Pagamento","1")); //isto tem de ser tirado depois, not null

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
                DataTable DT = lista.DTGetBreParaVDByIdEmpresa(ddEmpresa.SelectedValue);
                ddBRE.DataSource = DT;
                ddBRE.DataBind();

                lista = null;
            }
            else
            {
                ddBRE.Items.Clear();
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

                txtNumVD.Text = FacturaDT.Rows[0]["numVD"].ToString();
                txtNif.Text = FacturaDT.Rows[0]["NIF"].ToString();

                //se existe vd nao se pode gerar outra
                //nao se pode alterar os valores da factura, apenas remover os items.
                //mas isso nao consigo controloar por codigo.
        
                if (txtNumVD.Text != "") btnVD.Enabled = false;
                
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

                ViewState["idModif"] = FacturaDT.Rows[0]["idUtilAlteracao"];

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
            }
        }

        //=================================================================================================
        //PREENCHE ALGUNS VALORES DA FACTURA
        //=================================================================================================
        private void setFacturaData()
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
            fillEmpresaContratanteDoBRE();
            fillEmpresaData_SAP();
            fillEmpresaDevedora();

            setFacturaData();
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

       
            if(txtValorTotalFactura.Text == "0")
            {
                lblMessage.Text = "O valor da VD tem de ser maior que zero.";
                return; 
            }
            
            if (Page.IsValid)
            {
                if (txtNif.Text == "")
                {
                    lblMessage.Text = " Por favor indique o NIF da empresa";
                    return;
                }

                if (!IsValidNIF(txtNif.Text))
                    {
                        lblMessage.Text = "O NIF não é válido.";
                        return;
                    }
               
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

        ////=================================================================================================
        ////BUTTON RESET, REDIRECCIONAR PARA UM PAGINA NOVA
        ////=================================================================================================
        //private void btnReset_Click(object sender, System.EventArgs e)
        //{
        //    //Server.Transfer("FormFacturaSAP.aspx?btn=FAC",false);            
        //    Response.Redirect("FormFactura.aspx?btn=FAC", true);
        //}

        
        protected void btnVD_Click(object sender, EventArgs e)
        {
            if (ddCondPagamentoFactura.SelectedValue != "1") //pronto pagamento
            {
                lblMessage.Text = "Só pode gerar vendas à dinheiro para facturas com as condições de pagamento 'Pronto Pagamento'.";
            }
            else
            {
                CriaVD(ViewState["idFactura"].ToString());

                ////retorna true ou false e faz o response redirect depois aqui, senao causa um erro:
                ////14:31:44.9049179- System.Threading.ThreadAbortException: Thread was being aborted.
                //if(writeSapFile(ViewState["idFactura"].ToString())) 
                //{
                //    Response.Redirect("FormFacturaSAP.aspx?btn=FAC&id=" + ViewState["idFactura"].ToString(),true);
                //}

            }

        }

        private void CriaVD(string idFactura)
        {
            //tem de haver aqui uma validação que so certos funcionarios podem criar vds, se tem um numero (e uma serie) associados
            //inserir tantas linhas quantas temos na dtFactura

            DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
            DataTable dt = factura.dtServicosParaVD(idFactura);

            string numVD = factura.insertVD(dt).ToString();

            string strSQL = "UPDATE factura set numVD = '" + numVD + "' WHERE idFactura = " + idFactura;
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            fillForm(idFactura); //para actualizar agora tb o numero de vd.
        }


        //=================================================================================================
        //QUANDO A EMPRESA MUDA, TODA A INFORMAÇÃO TEM DE SER RE-CARREGADA
        //=================================================================================================
        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            refillAllData();
            fillEmpresaDevedora();
        }

        //=================================================================================================
        //QUANDO O BRE MUDA, PARTE DA INFORMAÇÃO TEM DE SER RECARREGADA====================================
        //=================================================================================================
        private void ddBRE_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            setFacturaData();
            CreateDataSource();
            BindGridSource();
            CreateDataSourceDTDestino();
            BindGridDestino();
            fillEmpresaContratanteDoBRE();
            fillEmpresaDevedora();
            fillEmpresaData_SAP();
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

        protected void btnUpdateNif_Click(object sender, EventArgs e)
        {
            if (!IsValidNIF(txtNif.Text))
               {
                   lblMessage.Text = "Registo não actualizado. O NIF não é válido."; 
                   return;
                }

            string idEmpresa = ddEmpresa.SelectedValue;
            if(ddEmpresaContratante.SelectedValue !="")  idEmpresa = ddEmpresaContratante.SelectedValue; 

            string strSQL = "UPDATE Empresa SET NIF = " + txtNif.Text + " WHERE idEmpresa = " + idEmpresa;

            lblMessage.Text = GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
        }


        //=================================================================================================
        //VALIDA SE EXISTEM CALIBRAÇÕES EXTERNAS NAS LINHAS DA FACTURA, SE SIM, A REGIAO DE VENDAS TEM DE SER PREENCHIDA.
        //isto nao funciona bem bem, pq podem ter seleccionado a dropdown e terem-se esquecido de submeter....
        //=================================================================================================
        private bool validaRegiaoVendas(DataTable dt)
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


            //idPlaneamento é o identificador e equivale ao numero da ORDEM

            //DESTCONTINUADO:operacao (para já vamos colocar 2, mas servirá para definir original ou duplicado)- DESCONTINUADO

            //folha (1 – para formato A4 e 2 – para formato Inspector Mobile)



        protected void btnImprimir_Click(object sender, EventArgs e)
        {

            if (txtNumVD.Text == "")
            {
                return; 
            }
            //da primeira vez que invoca esta página é impresso um original e da segunda vez, um duplicado

             
            //string url = "http://bdedifica.isq.pt/ISQ_Vendas/Reports/VendaADinheiro.aspx?idPlaneamento="+txtNumVD.Text+"&operacao="+rblOperacao.SelectedValue+"&folha="+rblFormato.SelectedValue; 

            string url = "http://sgao.isq.pt/Reports/VendaADinheiro.aspx?idPlaneamento= " + txtNumVD.Text + "&folha=" + rblFormato.SelectedValue; 
            //string url = "http://bdedifica.isq.pt/ISQ_Vendas/Reports/VendaADinheiro.aspx?idPlaneamento=" + txtNumVD.Text + "&folha=" + rblFormato.SelectedValue; 
            //Response.Redirect(url);
        	//Response.Write("<script language=javascript>window.open('" + url + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");

            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=JavaScript>");
            strScript.Append("window.open('" + url + "','new_Win','toolbar=0,menubar=0,resizable=1');");
            strScript.Append("</script>");
            //RegisterClientScriptBlock("imprimefactura", strScript.ToString());
            ClientScript.RegisterClientScriptBlock(GetType(), "imprimefactura", strScript.ToString());
        }


            //Então para as anulações teremos o seguinte storedprocedure AnulaVenda:
            //Temos dois parametros de entrada:
            //Ordem
            //Estado (2 – Inutilizada, 3 – Substituida)
            //NOTA: O storedprocedure pode devolver 3 valores:
            //-1 – Venda a dinheiro não pode ser anulada (data limite de anulação ultrapassada)
            //0 – Anulado por inutilização com sucesso
            //Nº da Ordem Seguinte – Caso se Anule por Substituição com sucesso. A Daniela aqui deverá fazer um update ao seu numero de ordem para ficar actualizado.


        //anular por inutilização - não se cria nada de novo
        protected void btnAnular_Click(object sender, EventArgs e)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@Ordem", txtNumVD.Text);
            arrParams[1] = new SqlParameter("@Estado", 2); //2: inutilizada

            string ret= GERAL.clsDataAccess.ExecuteNonQuerySPOutput_BD_VD("AnulaVendaMET", arrParams);
            switch (ret)
			{ 
                case "0":
                    lblMessage.Text = "Anulada por inutilização com sucesso.";
                     //aqui pensar se vale a pena apagar o numero da vd ou se deixamos estar.
                    string strSQL = "UPDATE factura set numVD = null WHERE idFactura = " + ViewState["idFactura"].ToString();
                    GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
                    fillForm(ViewState["idFactura"].ToString());
                    btnGerarNovoOriginal.Enabled = false; 
					break; 
				case "-1": 
					lblMessage.Text ="Venda a dinheiro não pode ser anulada (data limite de anulação ultrapassada.";
					break;					
			}
        }

        //anular para criar um novo original
        protected void btnGerarNovoOriginal_Click(object sender, EventArgs e)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@Ordem", txtNumVD.Text);
            arrParams[1] = new SqlParameter("@Estado", 3); //3=substituida

            string ret = GERAL.clsDataAccess.ExecuteNonQuerySPOutput_BD_VD("AnulaVendaMET", arrParams);
       
            switch (ret)
                
			{ 
                case "0":
					lblMessage.Text ="Anulada por inutilização com sucesso.";
					break; 
				case "-1":
                    lblMessage.Text = "Venda a dinheiro não pode ser anulada (data limite de anulação ultrapassada)";
					break;					
				default: //o numero da nova vd 
					string strSQL = "UPDATE factura set numVD = '" + ret + "' WHERE idFactura = " + ViewState["idFactura"].ToString();
                    GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
                    fillForm(ViewState["idFactura"].ToString()); //para actualizar agora tb o numero de vd. 
                break;
			}
        }
  


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

        private static bool IsNumeric(string inputString)
        {
            return Regex.IsMatch(inputString, "^[0-9]+$");
        }

       
    }
}
