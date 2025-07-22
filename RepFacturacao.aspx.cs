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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;


namespace LabMetro

{
    public partial class RepFacturacao : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.DataGrid dgGrandezas;
        private const string ID_PAG = "EST_FAC_0";//NOME DA PAGINA
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

                        // Preencher as datas com um valor default
                        DateTime dt = DateTime.Now;
                        string dia = dt.Day.ToString();
                        string year = dt.Year.ToString();


                        string dtInicio = "01-01-" + dt.Year.ToString(); //o dia 1 de Janeiro do ano Corrente.
                        txtDtInicio.Text = dtInicio;
                        txtDtFim.Text = dt.ToShortDateString();

                        // Fill da DropDownList do ano
                        for (int ano = 2005; ano <= DateTime.Today.Year + 1; ano++)
                        {
                            ddAno.Items.Add(ano.ToString());
                        }

                        // Seleccionar valores default
                        ddAno.SelectedValue = (DateTime.Today.Year - 1).ToString();
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

            dlTipoEquipamento.Controls.Clear();
            dlTipoEquipamento.Dispose();

            lblAno.Visible = false;
            ddAno.Visible = false;
            dgGrandeza.Controls.Clear();

            cbPrevisao.Visible = false;

            btnReport.Enabled = false;
        }


        // *****************************************************************************
        // SELECÇÃO DE UM REPORT
        // *****************************************************************************

        public void OnSelectedIndexChangedMethod(object s, EventArgs e)
        {
            // Esconder todos os controls
            hideAllSearchControls();

            // Enable botão para gerar o report
            btnReport.Enabled = true;

            // Depois mostramos só os que interessam para o report seleccionado
            switch (rblReports.SelectedIndex)
            {
                case 0: // Facturação por tipo de equipamento
                    showFacturacaoTipoEquipamento();
                    break;
                case 1: // Facturação por conjunto de tipos de equipamento
                    showFacturacaoConjuntoTiposEquipamento();
                    break;
                case 2: // Facturação por famílias de equipamentos
                    showFacturacaoFamiliasEquipamentos();
                    break;
                case 3: // Facturação por laboratório / grandeza
                    showFacturacaoGrandeza();
                    break;
                case 4: // Facturação mensal por laboratório / grandeza
                    showFacturacaoMensalGrandeza();
                    break;
                case 5: // Facturação acumulada dos últimos três anos
                    showFacturacaoAcumulada();
                    break;
                case 6: // Listagem de Valores Facturados por Equipamento
                    showEquipamentoFacturado();
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
        // Mostrar parâmetros de entrada para o report "Facturação por tipo de equipamento"
        private void showFacturacaoTipoEquipamento()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Facturação por conjunto de tipos de equipamento"
        private void showFacturacaoConjuntoTiposEquipamento()
        {
            showParamDates();

            DATA.ListasBD listas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = listas.DRListaTiposEquipamento();
            dlTipoEquipamento.DataSource = DR;
            dlTipoEquipamento.DataBind();
            DR.Close();

            listas = null;

            if (dlTipoEquipamento.Items.Count == 0)
            {
                dlTipoEquipamento.Dispose();
                dlTipoEquipamento.Controls.Clear();
            }
        }

        // Mostrar parâmetros de entrada para o report "Facturação por famílias de equipamentos"
        private void showFacturacaoFamiliasEquipamentos()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Facturação por laboratório / grandeza"
        private void showFacturacaoGrandeza()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Facturação mensal por laboratório / grandeza"
        private void showFacturacaoMensalGrandeza()
        {
            lblAno.Visible = true;
            ddAno.Visible = true;

            DATA.ListasBD listas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = listas.DRListaGrandezas();
            dgGrandeza.DataSource = DR;
            dgGrandeza.DataBind();
            DR.Close();

            listas = null;
        }

        // Mostrar parâmetros de entrada para o report "Facturação acumulada dos últimos três anos"
        private void showFacturacaoAcumulada()
        {
            lblAno.Visible = true;
            ddAno.Visible = true;
            //cbPrevisao.Visible = true;

            DATA.ListasBD listas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = listas.DRListaGrandezas();
            dgGrandeza.DataSource = DR;
            dgGrandeza.DataBind();
            DR.Close();

            listas = null;
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Valores Facturados por Equipamento"
        private void showEquipamentoFacturado()
        {
            showParamDates();
        }


        // *****************************************************************************
        // Exportação do report para PDF
        // *****************************************************************************

        protected void btnReport_Click(object sender, System.EventArgs e)
        {
            // Limpar eventuais mensagens de erro antigas
            lblMessage.Text = "";

            switch (rblReports.SelectedIndex)
            {
                case 0: // Facturação por tipo de equipamento
                    toPdfFacturacaoTipoEquipamento();
                    break;
                case 1: // Facturação por conjunto de tipos de equipamento
                    toPdfFacturacaoConjuntoTiposEquipamento();
                    break;
                case 2: // Facturação por famílias de equipamentos
                    toPdfFacturacaoFamiliasEquipamentos();
                    break;
                case 3: // Facturação por laboratório / grandeza
                    toPdfFacturacaoGrandeza();
                    break;
                case 4: // Facturação mensal por laboratório / grandeza
                    toPdfFacturacaoMensalGrandeza();
                    break;
                case 5: // Facturação acumulada dos últimos três anos
                    toPdfFacturacaoAcumulada();
                    break;
                case 6: // Listagem de Valores Facturados por Equipamento
                    toPdfEquipamentoFacturado();
                    break;
                default:
                    break;
            }
        }

        // Exportar para PDF o report "Facturação por tipo de equipamento"
        private void toPdfFacturacaoTipoEquipamento()
        {
            if (checkParamFacturacaoTipoEquipamento() == true)
            {


                 ReportClass report = null;

                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crFacturacaoTipoEquipamento();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crFacturacaoTipoEquipamento();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crFacturacaoTipoEquipamento();
                        break;


                }
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                //cr = null;
                //report = null;
                cr.exportReportToPDF(report,"Report");

                
            }
        }

        // Exportar para PDF o report "Facturação por conjunto de tipos de equipamento"
        private void toPdfFacturacaoConjuntoTiposEquipamento()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamFacturacaoConjuntoTiposEquipamento() == true)
            {
                int num = dlTipoEquipamento.Items.Count; // nº de tipos de equip.

                Array arrayIdsTipoEquipamento = Array.CreateInstance(typeof(Int32), num);

                int index = 0;

                foreach (DataListItem dli in dlTipoEquipamento.Items)
                {
                    CheckBox myCheckBox =
                        (CheckBox)dli.FindControl("chb");
                    if (myCheckBox.Checked == true)
                    {
                        // Adicionar os ID's seleccionados a um array
                        arrayIdsTipoEquipamento.SetValue(int.Parse(dlTipoEquipamento.DataKeys[dli.ItemIndex].ToString()), index);
                        index += 1;
                    }
                }

                ReportClass report = null;

                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crFacturacaoConjuntoTiposEquipamento();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crFacturacaoConjuntoTiposEquipamento();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crFacturacaoConjuntoTiposEquipamento();
                        break;


                }


               
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inIdTipoEquipamento", arrayIdsTipoEquipamento);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                cr.exportReportToPDF(report,"Report");

            }
        }

        // Exportar para PDF o report "Facturação por famílias de equipamentos"
        private void toPdfFacturacaoFamiliasEquipamentos()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamFacturacaoFamiliasEquipamentos() == true)
            {
                

                ReportClass report = null;

                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crFacturacaoFamiliasEquipamentos();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crFacturacaoFamiliasEquipamentos();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crFacturacaoFamiliasEquipamentos();
                        break;

                }


                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));

                cr.exportReportToPDF(report,"Report");

               
            }
        }

        // Exportar para PDF o report "Facturação por laboratório / grandeza"
        private void toPdfFacturacaoGrandeza()
        {
            if (checkParamFacturacaoGrandeza() == true)
            {
                ReportClass report = null;

                switch (myApp)
                {
                    case "ISQ_LABMETRO":
                        report = new LabMetro.REPORTS.crFacturacaoPorGrandeza();
                        break;
                    case "ANG_LABMETRO":
                        report = new LabMetro.REPORTS_ANG.crFacturacaoPorGrandeza();
                        break;
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crFacturacaoPorGrandeza();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crFacturacaoPorGrandeza();
                        break;
                    default:
                        break;
                }

                clsReport cr = new clsReport();

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                cr.exportReportToPDF(report,"Report");

                
            }
        }

        // Exportar para PDF o report "Facturação mensal por laboratório / grandeza"
        private void toPdfFacturacaoMensalGrandeza()
        {

            //checkParamFacturacaoMensalGrandeza();

            ReportClass report = null;

            switch (myApp)
            {
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crFacturacaoMensalPorGrandeza();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.crFacturacaoMensalPorGrandeza();
                    break;
                default:
                    report = new LabMetro.REPORTS.crFacturacaoMensalPorGrandeza();
                    break;

            }


               
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);

                string strIDs = "";

                foreach (DataGridItem dgi in dgGrandeza.Items)
                {
                    CheckBox myCheckBox =
                        (CheckBox)dgi.Cells[0].FindControl("cbGrandeza");
                    if (myCheckBox.Checked == true)
                    {
                        if (strIDs != "")
                            strIDs += ",";

                        // Concatenar os ID's separados por vírgulas
                        strIDs += dgGrandeza.DataKeys[dgi.ItemIndex].ToString();
                    }
                }

                report.SetParameterValue("@inIDs", strIDs);

                report.SetParameterValue("@inAno", ddAno.SelectedValue.ToString());

                // Exportar o report para PDF
                cr.exportReportToPDF(report,"Report");

                
        }

        // Exportar para PDF o report "Facturação acumulada dos últimos três anos"
        private void toPdfFacturacaoAcumulada()
        {
            checkParamFacturacaoAcumulada();

          
            ReportClass report = null;

            switch (myApp)
            {
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.crFacturacaoAcumulada();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.crFacturacaoAcumulada();
                    break;
                default:
                    report = new LabMetro.REPORTS.crFacturacaoAcumulada();
                    break;

            }


            clsReport cr = new clsReport();
            cr.setReportConnectionInfo(report);
            string strIDs = "";

            foreach (DataGridItem dgi in dgGrandeza.Items)
            {
                CheckBox myCheckBox =
                    (CheckBox)dgi.Cells[0].FindControl("cbGrandeza");
                if (myCheckBox.Checked == true)
                {
                    if (strIDs != "")
                        strIDs += ",";

                    // Concatenar os ID's separados por vírgulas
                    strIDs += dgGrandeza.DataKeys[dgi.ItemIndex].ToString();
                }
            }

            report.SetParameterValue("@inIDs", strIDs);
            report.SetParameterValue("@inAno", ddAno.SelectedValue.ToString());
            // Exportar o report para PDF
            cr.exportReportToPDF(report, "Report");
        }


        //======================================================================
        //NOVO:: DISCONNTECTED DATASET =========================================
        //DATASET PARA REPORT EQUIPAMENTO CALIBRADO ============================
        //USA EXACTAMENTE O MESMO DATASET QUE O ANTERIOR, MAS FAZ FILL A PARTIR
        //DE OUTRA STORED PROCEDURE ============================================
        //======================================================================
        public DataSet dsEquipamentoFacturado()
        {
            LabMetro.DATASETS.DSRepListas ds = new DATASETS.DSRepListas();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {


                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objCmd.Connection = objConn;


                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "stpRepListEquipamentoFacturado";

                objCmd.Parameters.AddWithValue("@inDtInicio", txtDtInicio.Text);
                objCmd.Parameters.AddWithValue("@inDtFim", txtDtFim.Text);


                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                try
                {

                    da.Fill(ds, "dtEquipamentoFacturado");
                }
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex);
                }
                da.Dispose();
                da = null;
            }


            return ds;
        }
        //======================================================================
        //======================================================================
        //NOVO - com dataset disconnected
        // Exportar para PDF o report "Listagem de Valores Facturados por Equipamento"
        private void toPdfEquipamentoFacturado()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamEquipamentoFacturado() == true)
            {
                try
                {

                    ReportClass report = null;

                    switch (myApp)
                    {
                        case "ES_LABMETRO":
                            report = new LabMetro.REPORTS_ES.crEquipamentoFacturado();
                            break;
                        case "DZ_LABMETRO":
                            report = new LabMetro.REPORTS_DZ.crEquipamentoFacturado();
                            break;
                        default:
                            report = new LabMetro.REPORTS.crEquipamentoFacturado();
                            break;

                    }



                 
                    clsReport cr = new clsReport();

                    //					report.SetParameterValue("@inDtInicio",txtDtInicio.Text);
                    //					report.SetParameterValue("@inDtFim", txtDtFim.Text);

                    DataSet ds = dsEquipamentoFacturado();
                    report.SetDataSource(ds);

                    ds = null;
                    // Exportar o report para PDF
                    cr.exportReportToPDF(report, "Report");
                    
                }
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex);
                }
            }
        }


        // *****************************************************************************
        // Validações dos parâmetros de entrada dos reports
        // *****************************************************************************

        // Validação das datas
        private bool checkParamDates()
        {
            try
            {
                if (DateTime.Compare(DateTime.Parse(txtDtInicio.Text), DateTime.Parse(txtDtFim.Text)) <= 0)
                {
                    return true;
                }

                else
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DATA_FIM_SUPERIOR_DATA_INICIO;
                    return false;
                }
            }
            catch
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DATAS_INCORRECTAS;
                return false;
            }
        }

        // Validação dos parâmetros do report "Facturação por tipo de equipamento"
        private bool checkParamFacturacaoTipoEquipamento()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Facturação por conjunto de tipos de equipamento"
        private bool checkParamFacturacaoConjuntoTiposEquipamento()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Facturação por famílias de equipamentos"
        private bool checkParamFacturacaoFamiliasEquipamentos()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Facturação por laboratório / grandeza"
        private bool checkParamFacturacaoGrandeza()
        {
            return checkParamDates();
        }

        //// Validação dos parâmetros do report "Facturação mensal por laboratório / grandeza"
        //private bool checkParamFacturacaoMensalGrandeza()
        //{
        //    //return checkParamDates();
        //}

        // Validação dos parâmetros do report "Facturação acumulada dos últimos três anos"
        private bool checkParamFacturacaoAcumulada()
        {
            return true;
        }

        // Validação dos parâmetros do report "Listagem de Valores Facturados por Equipamento"
        private bool checkParamEquipamentoFacturado()
        {
            return checkParamDates();
        }
    }
}
