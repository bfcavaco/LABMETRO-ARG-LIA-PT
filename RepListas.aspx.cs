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
using LabMetro.GERAL;
using LabMetro.REPORTS;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace LabMetro
{
    public partial class RepListas : System.Web.UI.Page
        
    {
        private const string ID_PAG = "EST_LIST_0";//NOME DA PAGINA
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();


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
                    // Carregar a página pela primeira vez

                    if (!Page.IsPostBack)
                    {
                        unCheckAll();
                        hideAllSearchControls();

                        // Preencher valores default
                        DateTime dt = DateTime.Now;
                        string dia = dt.Day.ToString();
                        string year = dt.Year.ToString();


                        string dtInicio = "01-01-" + dt.Year.ToString(); //o dia 1 de Janeiro do ano Corrente.
                        txtDtInicio.Text = dtInicio;
                        txtDtFim.Text = dt.ToShortDateString();

                        txtValorMin.Text = "0";


                        fillDDEmpresa();

                        // Fill da DropDownList dos Estados
                        DATA.ListasBD listaEstados = new LabMetro.DATA.ListasBD();
                        SqlDataReader DR2 = listaEstados.DRListaEstadosServico();
                        ddEstado.DataSource = DR2;
                        ddEstado.DataBind();
                        ddEstado.Items.Insert(0, new ListItem("", ""));
                        DR2.Close();

                        listaEstados = null;

                        // Fill da DropDownList das Grandezas
                        DATA.ListasBD listaGrandezas = new LabMetro.DATA.ListasBD();
                        SqlDataReader DR3 = listaGrandezas.DRListaGrandezas();
                        ddGrandeza.DataSource = DR3;
                        ddGrandeza.DataBind();
                        ddGrandeza.Items.Insert(0, new ListItem("", ""));
                        DR3.Close();

                        listaGrandezas = null;
                    }
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

        }
        #endregion

        // *****************************************************************************
        // SHOW/HIDE
        // *****************************************************************************
        private void unCheckAll()
        {
            foreach (ListItem li in rblReports.Items)
            {
                li.Selected = false;
            }
        }
        private void hideAllSearchControls()
        {
            lblDtInicio.Visible = false;
            txtDtInicio.Visible = false;
            lblDtFim.Visible = false;
            txtDtFim.Visible = false;

            lblValorMin.Visible = false;
            txtValorMin.Visible = false;

            //dgTipoEquipamento.Controls.Clear();
            dlTipoEquipamento.Controls.Clear();
            dlTipoEquipamento.DataSource = null;
            dlTipoEquipamento.DataBind();


            lblEmpresa.Visible = false;
            lblPesquisaEmpresa.Visible = false;
            ddEmpresa.Visible = false;
            txtEmpresa.Visible = false;

            lblEstado.Visible = false;
            ddEstado.Visible = false;
            lblGrandeza.Visible = false;
            ddGrandeza.Visible = false;

            btnReport.Enabled = false;
            btnExcel.Visible = false;
        }


        // *****************************************************************************
        // SELECÇÃO DE UM REPORT
        // *****************************************************************************

        public void OnSelectedIndexChangedMethod(object sender, System.EventArgs e)
        {
            // Esconder todos os controls
            hideAllSearchControls();

            // Enable botão para gerar o report
            btnReport.Enabled = true;

            // Depois mostramos só os que interessam para o report seleccionado
            switch (rblReports.SelectedIndex)
            {

                case 0: // Nº de Dias de Espera
                    showNumDiasEspera();
                    break;
                case 1: // Listagem de Nº de Entradas e Tempo Médio de Espera
                    showNumEntradasTempoMedio();
                    break;
                case 2: // Listagem de Equipamento Calibrado
                    showEquipamentoCalibrado();
                    break;
                case 3: // Listagem de Equipamentos Não Calibrados
                    showEquipamentoNaoCalibrado();
                    break;
                case 4: // Listagem de Estados dos Equipamentos por Empresa
                    showEstadosEquipamentosPorEmpresa();
                    break;
                case 5: // Listagem de Equipamentos Por Estado
                    showEquipamentosPorEstado();
                    break;
                case 6: // Plano de Calibração -- NOVO OK!
                    showPlanoCalibracao();
                    break;
                case 7: // Equipamentos a Calibrar (Plano de Calibração)
                    showEquipamentosACalibrar();
                    break;
                case 8: // Calibrações em Atraso (Plano de Calibração)
                    showCalibracoesEmAtraso();
                    break;
                case 9: // Calibrações em Atraso (Plano de Calibração)
                    lblMessage.Text = "";
                    break;
                case 10: //???
                    lblMessage.Text = "";
                    break;
                case 11: //criterios como o numero 5 acima, com query difernte, para mostrar serviços a facturar em angola
                    showParamDates();
                    break;


                default:
                    break;
            }

            lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_PARAMS;
        }

        // Mostrar as datas
        private void showParamDates()
        {
            lblDtInicio.Visible = true;
            txtDtInicio.Visible = true;
            lblDtFim.Visible = true;
            txtDtFim.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Nº de Dias de Espera"
        private void showNumDiasEspera()
        {
            showParamDates();

            lblGrandeza.Visible = true;
            ddGrandeza.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Nº de Entradas e Tempo Médio de Espera"
        private void showNumEntradasTempoMedio()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Equipamento Calibrado"
        private void showEquipamentoCalibrado()
        {
            showParamDates();

            DATA.ListasBD listas = new LabMetro.DATA.ListasBD();

            SqlDataReader dr = listas.DRListaTiposEquipamento();
            dlTipoEquipamento.DataSource = dr;
            dlTipoEquipamento.DataBind();
            dr.Close();

            listas = null;

            if (dlTipoEquipamento.Items.Count == 0)
            {
                dlTipoEquipamento.DataSource = null;
                dlTipoEquipamento.DataBind();
                dlTipoEquipamento.Dispose();
            }

        }


        // Mostrar parâmetros de entrada para o report "Listagem de Equipamentos Não Calibrados"
        private void showEquipamentoNaoCalibrado()
        {
            showParamDates();
        }

        private void showEstadosEquipamentosPorEmpresa()
        {
            showParamDates();

            lblEmpresa.Visible = true;
            lblPesquisaEmpresa.Visible = true;

            ddEmpresa.Visible = true;
            txtEmpresa.Visible = true;
        }


        private void showEquipamentosPorEstado()
        {
            showParamDates();

            lblEstado.Visible = true;
            ddEstado.Visible = true;
            lblGrandeza.Visible = true;
            ddGrandeza.Visible = true;
        }


        private void showPlanoCalibracao()
        {
            lblEmpresa.Visible = true;
            lblPesquisaEmpresa.Visible = true;
            ddEmpresa.Visible = true;
            txtEmpresa.Visible = true;
            lblGrandeza.Visible = true;
            ddGrandeza.Visible = true;
            btnExcel.Visible = true;
        }


        private void showEquipamentosACalibrar()
        {
            showParamDates();
            
            lblEmpresa.Visible = true;
            lblPesquisaEmpresa.Visible = true;
            ddEmpresa.Visible = true;
            txtEmpresa.Visible = true;
            lblGrandeza.Visible = true;
            ddGrandeza.Visible = true;
        }


        private void showCalibracoesEmAtraso()
        {
            lblEmpresa.Visible = true;
            lblPesquisaEmpresa.Visible = true;
            ddEmpresa.Visible = true;
            txtEmpresa.Visible = true;
            lblGrandeza.Visible = true;
            ddGrandeza.Visible = true;
        }


        // *****************************************************************************
        // Exportação do report para PDF
        // *****************************************************************************

        protected void btnReport_Click(object sender, System.EventArgs e)
        {

            lblMessage.Text = "";

            switch (rblReports.SelectedIndex) //SELECTEDINDEX Not value....
            {

                case 0: // Nº de Dias de Espera
                    toPdfNumDiasEspera();
                    break;
                case 1: // Listagem de Nº de Entradas e Tempo Médio de Espera
                    toPdfNumEntradasTempoMedio();
                    break;
                case 2: // Listagem de Equipamento Calibrado
                    toPdfEquipamentoCalibrado();
                    break;
                case 3: // Listagem de Equipamentos Não Calibrados
                    toPdfEquipamentoNaoCalibrado();
                    break;
                case 4: // Listagem de Estados dos Equipamentos por Empresa
                    toPdfEstadosEquipamentosPorEmpresa();
                    break;
                case 5: // Listagem de Equipamentos Por Estado
                    toPdfEquipamentosPorEstado();
                    break;
                case 6: // Plano de Calibração -- NOVO
                    toPdfPlanoCalibracao();
                    break;
                case 7: // Equipamentos a Calibrar (Plano de Calibração)
                    toPdfEquipamentosACalibrar();
                    break;
                case 8: // Calibrações em Atraso (Plano de Calibração)
                    toPdfCalibracoesEmAtraso();
                    break;
                case 9: // Calibrações em Atraso (Plano de Calibração)
                    toPdfEmpresasBloqueadas();
                    break;
                case 10: // códigos correspondentes aos Laboratórios
                    toPdfCodigosLaboratorios();
                    break;
                case 11:
                    toPdfFacturarAngola();
                    break;


                default:
                    break;
            }
        }








        //======================================================================
        // REPORT  "Nº de Dias de Espera"
        //======================================================================
        private void toPdfNumDiasEspera()
        {

            if (checkParamNumDiasEspera() == true)
            {

                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crNumDiasEspera();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crNumDiasEspera();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crNumDiasEspera();
                        break;
                }


                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inIdGrandeza", ddGrandeza.SelectedValue);
                cr.exportReportToPDF(report,"Report");

                //cr = null;
                //report = null;
            }
        }

        //======================================================================
        // REPORT "Listagem de Nº de Entradas e Tempo Médio de Espera"
        //======================================================================
        private void toPdfNumEntradasTempoMedio()
        {
            if (checkParamNumEntradasTempoMedio() == true)
            {
                

                ReportClass report = null;

                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crNumEntradasTempoMedio();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crNumEntradasTempoMedio();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crNumEntradasTempoMedio();
                        break;
                }


                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));

                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");

                
            }
        }

        //======================================================================
        // REPORT "Listagem de Equipamento Calibrado"
        //======================================================================
        private void toPdfEquipamentoCalibrado()
        {

            if (checkParamEquipamentoCalibrado() == true)
            {

                string strIds = "";
                bool ok = false;

                foreach (DataListItem dli in dlTipoEquipamento.Items)
                {
                    CheckBox myCheckBox = (CheckBox)dli.FindControl("chb");
                    if (myCheckBox.Checked == true)
                    {
                        strIds += dlTipoEquipamento.DataKeys[dli.ItemIndex].ToString() + ",";
                        ok = true;
                    }
                }

                if (ok == true)
                {
                    

                     ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                   
                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.crEquipamentoCalibrado();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.crEquipamentoCalibrado();
                            break;
                        default:
                            report = new LabMetro.REPORTS.crEquipamentoCalibrado();
                            break;
                    }


                    clsReport cr = new clsReport();
                    cr.setReportConnectionInfo(report);

                    string delimStr = ",";
                    char[] delimiter = delimStr.ToCharArray();
                    strIds = strIds.TrimEnd(delimiter);

                    string[] ids = strIds.Split(delimiter);

                    report.SetParameterValue("@inIdTipoEquipamento", ids);
                    report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                    report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));

                    // Exportar o report para PDF
                    cr.exportReportToPDF(report, "Report");
                }
                else
                {
                    lblMessage.Text = "Tem de seleccionar no mínimo um tipo de equipamento.";
                }
            }
        }

        //======================================================================
        // Exportar para PDF o report "Listagem de Equipamentos Não Calibrados"
        private void toPdfEquipamentoNaoCalibrado()
        //======================================================================
        {
            if (checkParamEquipamentoNaoCalibrado() == true)
            {
                


                 ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.crEquipamentoNaoCalibrado();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.crEquipamentoNaoCalibrado();
                            break;
                        default:
                            report = new LabMetro.REPORTS.crEquipamentoNaoCalibrado();
                            break;
                    }

                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        //======================================================================
        // Exportar para PDF o report "Listagem de Estados dos Equipamentos por Empresa"
        //======================================================================
        private void toPdfEstadosEquipamentosPorEmpresa()
        {
            if (checkParamEstadosEquipamentosPorEmpresa() == true)
            {
                


                 ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                    
                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.crEstadosEquipamentosPorEmpresa();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.crEstadosEquipamentosPorEmpresa();
                            break;
                        default:
                            report = new LabMetro.REPORTS.crEstadosEquipamentosPorEmpresa();
                            break;
                    }

                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inIdEmpresa", ddEmpresa.SelectedValue);
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        //======================================================================
        // Exportar para PDF o report "Listagem de Equipamentos por Estado"
        //======================================================================
        private void toPdfEquipamentosPorEstado()
        {

            if (checkParamEquipamentosPorEstado() == true)
            {
                 ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                 
                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.crRepEquipPorEstado();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.crRepEquipPorEstado();
                            break;
                        default:
                            report = new LabMetro.REPORTS.crRepEquipPorEstado();
                            break;
                    }

                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);

                if (ddGrandeza.SelectedIndex > 0) // só uma grandeza
                    report.SetParameterValue("@inIdGrandeza", ddGrandeza.SelectedValue);
                else // todas as grandezas
                {
                    int num = ddGrandeza.Items.Count; // nº de grandezas

                    Array arrayIdsGrandezas = Array.CreateInstance(typeof(string), num);

                    int index = 0;

                    foreach (ListItem ddi in ddGrandeza.Items)
                    {
                        // Adicionar os ID's das grandezas a um array
                        arrayIdsGrandezas.SetValue(ddi.Value.ToString(), index);
                        index += 1;
                    }

                    report.SetParameterValue("@inIdGrandeza", arrayIdsGrandezas);
                }

                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inIdEstado", ddEstado.SelectedValue);

                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        //======================================================================
        // Exportar para PDF o report "Listagem de Equipamentos por Estado" 
        //para poder facturar em angola: inlc indicacao se é calib. ext e se é subcontrato
        //======================================================================
        private void toPdfFacturarAngola()
        {

            if (checkParamDates() == true)
            {

                crRepFacturarAngola report = new crRepFacturarAngola();
                
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);

                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        //======================================================================
        //NOVO:: DISCONNTECTED DATASET =========================================
        //DATASET PARA REPORT PLANOS DE CALIBRAÇÃO==============================
        //======================================================================
        public DataSet dsPlanoCalibracao()
        {
            LabMetro.DATASETS.DSPlanoCalibracao ds = new DATASETS.DSPlanoCalibracao();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                objCmd.Connection = objConn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                objCmd.CommandText = "stpRepPlanoCalibracao";
                objCmd.Parameters.AddWithValue("@inIdEmpresa", ddEmpresa.SelectedValue);
                objCmd.Parameters.AddWithValue("@inIdGrandeza", ddGrandeza.SelectedValue);


                try
                {
                    da.Fill(ds, "RepPlanoCalibracao");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }

                da.Dispose();
            }
            return ds;
        }
        //======================================================================
        //======================================================================


        //======================================================================
        //NOVO:: DISCONNTECTED DATASET =========================================
        //DATASET PARA REPORT EQUIPAMENTOS A CALIBRAR==============================
        //======================================================================
        public DataSet dsEquipamentosACalibrar()
        {
            LabMetro.DATASETS.DSEquipamentoACalibrar ds = new DATASETS.DSEquipamentoACalibrar();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                objCmd.CommandText = "stpRepEquipamentosACalibrar";

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                objCmd.Parameters.AddWithValue("@inIdEmpresa", ddEmpresa.SelectedValue);
                objCmd.Parameters.AddWithValue("@inDtInicio", txtDtInicio.Text);
                objCmd.Parameters.AddWithValue("@inDtFim", txtDtFim.Text);
                objCmd.Parameters.AddWithValue("@inIdGrandeza", ddGrandeza.SelectedValue);


                try
                {
                    da.Fill(ds, "dtEquipamentoACalibrar");
                }
                catch (Exception)
                {
                    //Response.Write(ex.ToString());
                }
                da.Dispose();
            }
            return ds;
        }
        //		//======================================================================
        //		//======================================================================

        //======================================================================
        //NOVO:: DISCONNTECTED DATASET =========================================
        //DATASET PARA REPORT CALIBRAÇÕES EM ATRASO ============================
        //USA EXACTAMENTE O MESMO DATASET QUE O ANTERIOR, MAS FAZ FILL A PARTIR
        //DE OUTRA STORED PROCEDURE ============================================
        //======================================================================
        public DataSet dsCalibracoesEmAtraso()
        {
            LabMetro.DATASETS.DSEquipamentoACalibrar ds = new DATASETS.DSEquipamentoACalibrar();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "stpRepCalibracoesEmAtraso";

                objCmd.Parameters.AddWithValue("@inIdEmpresa", ddEmpresa.SelectedValue);

                objCmd.Parameters.AddWithValue("@inIdGrandeza", ddGrandeza.SelectedValue);

                try
                {
                    //este nome está mal, mas quando criei, 
                    //n sabia que ia usar o ds para muitas	//situações

                    da.Fill(ds, "dtEquipamentoACalibrar");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }

                da.Dispose();
                da = null;
            }
            return ds;
        }
        //======================================================================
        //======================================================================

        //======================================================================
        // Exportar para PDF o report "Plano de Calibração"
        //======================================================================
        private void toPdfPlanoCalibracao()
        {
            if (checkParamPlanoCalibracao() == true)
            {
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

              
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.rptPlanoCalibracao();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.rptPlanoCalibracao();
                        break;
                    default:
                        report = new LabMetro.REPORTS.rptPlanoCalibracao();
                        break;
                }

                clsReport cr = new clsReport();
                DataSet ds = dsPlanoCalibracao();
                report.SetDataSource(ds); 
                ds = null;
                cr.exportReportToPDF(report,"Report");
               
            }
        }

        //======================================================================
        // Exportar para EXCEL O PLANO DE CALIBRACAO ACIMA
        //======================================================================
        protected void Button1_Click(object sender, EventArgs e)
        {

            if (checkParamPlanoCalibracao() == true)
            {
                DataSet ds = dsPlanoCalibracao();

                gv.DataSource = ds;
                gv.DataBind();
                string nomeexcel = ddEmpresa.SelectedValue + ".xls";
                GERAL.GridViewExportUtil.Export(nomeexcel, gv);
            }
        }


        //======================================================================
        // Exportar para PDF o report "Equipamentos a Calibrar (Plano de Calibração)"
        //======================================================================
        private void toPdfEquipamentosACalibrar()
        {
            if (checkParamEquipamentosACalibrar() == true)
            {
                try
                {
                    ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                    
                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.rptEquipamentoACalibrar();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.rptEquipamentoACalibrar();
                            break;
                        default:
                            report = new LabMetro.REPORTS.rptEquipamentoACalibrar();
                            break;
                    }


                 
                    clsReport cr = new clsReport();
                    DataSet ds = dsEquipamentosACalibrar();
                    report.SetDataSource(ds);
                    report.SetParameterValue("@dataInicio", DateTime.Parse(txtDtInicio.Text));
                    report.SetParameterValue("@dataFim", DateTime.Parse(txtDtFim.Text));
                    // Exportar o report para PDF
                    ds = null;
                    cr.exportReportToPDF(report, "Report");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

      


        //======================================================================
        // Exportar para PDF o report "Calibrações em Atraso (Plano de Calibração)"
        //======================================================================
        private void toPdfCalibracoesEmAtraso()
        {

            if (checkParamCalibracoesEmAtraso() == true)
            {
                try
                {
                    ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                    
                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.rptCalibracoesEmAtraso();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.rptCalibracoesEmAtraso();
                            break;
                        default:
                            report = new LabMetro.REPORTS.rptCalibracoesEmAtraso();
                            break;
                    }

                    clsReport cr = new clsReport();

                    //fiquei aqui
                   

                    DataSet ds = dsCalibracoesEmAtraso();
                    report.SetDataSource(ds);
                    
                    ds = null;
                    // Exportar o report para PDF
                    cr.exportReportToPDF(report, "Report");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        //======================================================================
        // REPORT  CÓDIGOS DOS LABORATÓRIOS (PEP, REGIAO VENDAS, CENTRO CUSTO)...
        //======================================================================
        private void toPdfCodigosLaboratorios()
        {
            crPEP report = new crPEP();
            clsReport cr = new clsReport();
            cr.setReportConnectionInfo(report);
            cr.exportReportToPDF(report,"Report");

            

        }

        //==============================================================================
        //==============================================================================
        // Validações dos parâmetros de entrada dos reports
        //==============================================================================
        //==============================================================================


        private void toPdfEmpresasBloqueadas()
        {
            //copiado da pagina listaempresas
            try
            {

                DataSet ds = new DATASETS.DSEmpresa();

                if (ds != null)
                {

                    ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!
                    ds = GERAL.clsDataAccess.DSFillDS_SP("stpGetListEmpresasPagAtrasoFalCont", ds, "dtEmpresa");


                    ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo
                    //nao ha para espanha. sao coisas do SAP.
                    report = new LabMetro.REPORTS.rptEmpresasPagamentoAtraso();
                    


                    
                    clsReport cr = new clsReport();

                    report.SetDataSource(ds);
                    ds = null;
                    cr.exportReportToPDF(report,"Report");

                }

            }
            catch (Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex.ToString());
            }
        }

        private bool checkParamDates()
        {
            try
            {
                if (DateTime.Compare(DateTime.Parse(txtDtInicio.Text), DateTime.Parse(txtDtFim.Text)) <= 0)
                    return true;
                else
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DATA_FIM_SUPERIOR_DATA_INICIO;
                return false;
            }
            catch
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DATAS_INCORRECTAS;
                return false;
            }
        }



        private bool checkParamNumDiasEspera()
        {
            if (checkParamDates() == true)
            {

                if (ddGrandeza.SelectedIndex > 0)
                {
                    return true;
                }
                else // se não tiver sido seleccionada nenhuma grandeza
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_GRANDEZA;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool checkParamNumEntradasTempoMedio()
        {
            return checkParamDates();
        }


        private bool checkParamEquipamentoCalibrado()
        {
            return checkParamDates();
        }


        private bool checkParamEquipamentoNaoCalibrado()
        {
            return checkParamDates();
        }


        private bool checkParamEstadosEquipamentosPorEmpresa()
        {
            if (checkParamDates() == true)
            {
                if (ddEmpresa.SelectedIndex > 0)
                {
                    return true;
                }
                else
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_GRANDEZA;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool checkParamEquipamentosPorEstado()
        {
            if (checkParamDates() == true)
            {

                if (ddEstado.SelectedIndex > 0)
                {
                    return true;
                }
                else
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_ESTADO;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool checkParamPlanoCalibracao()
        {
            if (ddEmpresa.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_EMPRESA;
                return false;
            }
        }


        private bool checkParamEquipamentosACalibrar()
        {
            if (checkParamDates() == true)
            {
                if (ddEmpresa.SelectedIndex > 0)
                {
                    return true;
                }
                else
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_EMPRESA;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool checkParamCalibracoesEmAtraso()
        {
            if (ddEmpresa.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_EMPRESA;
                return false;
            }
        }

        protected void txtEmpresa_TextChanged(object sender, System.EventArgs e)
        {
            fillDDEmpresa();
        }

        private void fillDDEmpresa()
        {
            DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();

            DataTable DT = empresa.DTEmpresas(txtEmpresa.Text, "", "1", "", "", "", "", "", "");
            DataView DV = new DataView(DT);

            empresa = null;

            ddEmpresa.DataSource = DV; ;
            ddEmpresa.DataBind();
            ddEmpresa.Items.Insert(0, new ListItem("", ""));

        }

        
       

    }
}
