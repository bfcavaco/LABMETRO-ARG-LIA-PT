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


namespace LabMetro
{
    public partial class RepOutras : System.Web.UI.Page
    {


        //protected System.Web.UI.WebControls.DataGrid dgFuncionario;

        private const string ID_PAG = "EST_OUTRAS_0";//NOME DA PAGINA
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
                        rblPeriodo.Items[0].Selected = true; // Ano
                        txtSuperior.Text = "0";
                        txtVariacao.Text = "0";

                        // Fill da DropDownList dos BRE's
                        DATA.BreBD listaBRE = new LabMetro.DATA.BreBD();
                        SqlDataReader DR = listaBRE.DRGetBrePorFacturar();
                        ddBRE.DataSource = DR;
                        ddBRE.DataBind();
                        ddBRE.Items.Insert(0, new ListItem("", ""));
                        DR.Close();

                        listaBRE = null;

                        // Fill das DropDownList dos anos
                        for (int ano = 2004; ano <= DateTime.Today.Year + 1; ano++)
                        {
                            ddAnosIni.Items.Add(ano.ToString());
                            ddAnosFim.Items.Add(ano.ToString());
                        }

                        // Seleccionar valores default
                        ddAnosIni.SelectedValue = (DateTime.Today.Year - 2).ToString();
                        ddAnosFim.SelectedValue = (DateTime.Today.Year - 1).ToString();
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
            lblBRE.Visible = false;
            ddBRE.Visible = false;
            lblAnosIni.Visible = false;
            ddAnosIni.Visible = false;
            lblAnosFim.Visible = false;
            ddAnosFim.Visible = false;
            lblPeriodo.Visible = false;
            rblPeriodo.Visible = false;
            lblSuperior.Visible = false;
            txtSuperior.Visible = false;
            lblVariacao.Visible = false;
            txtVariacao.Visible = false;
            lblUltimoAno.Visible = false;
            ddGrandeza.Visible = false;
            lblGrandeza.Visible = false;
            dlTipoEquipamento.Controls.Clear();
            dlTipoEquipamento.Dispose();
            DLFuncionario.Controls.Clear();
            DLFuncionario.Visible = false;
            DLFuncionario.Controls.Clear();
            DLFuncionario.Dispose();
            ddFuncionario.Visible = false;
            lblEmpresa.Visible = false;
            txtEmpresa.Visible = false;
            btnReport.Enabled = false;
        }

        // *****************************************************************************
        // Exportação do report para PDF
        // *****************************************************************************

        protected void btnReport_Click(object sender, System.EventArgs e)
        {
            
            lblMessage.Text = "";
            
            switch (rblReports.SelectedValue)  //SELECTEDVALUE, NOT INDEX!!!
            {
                case "rptNovasEmpresas": // Listagem de Novas Empresas
                    toPdfNovasEmpresas();
                    break;
                case "rptMelhoresEmpresas": // Listagem das Melhores Empresas
                    toPdfMelhoresEmpresas();
                    break;
                case "rptTrabalhosGranhosPerdidos": // Listagem de Trabalhos Ganhos / Perdidos
                    toPdfTrabalhosGanhosPerdidos();
                    break;
                case "rptOrcamentos": // Listagem de Orçamentos
                    toPdfListaOrcamentos();
                    break;

                case "rptTaxasServico": // Listagem de Taxas de Servico
                    toPdfListaTaxasServico();
                    break;
                case "rptOrcamentosAtraso": // Listagem de Orçamentos em Atraso
                    toPdfListaOrcamentosAtraso();
                    break;
                case "rptOrcamentosMes": // Nº de Orçamentos por Mês
                    toPdfNumOrcamentosMes();
                    break;
                case "rptTempoMedioEsperaOrcamentos": // Tempo Médio de Espera dos Orçamentos
                    toPdfTempoMedioOrcamentos();
                    break;
                case "rptEquipamentosPrecoZero": // Listagem de Equipamentos Facturados com Preços a Zero
                    toPdfEquipamentoPrecosZero();
                    break;
                case "rptBresNaoFacturados": // Listagem de BRE's Não Facturados
                    toPdfBREsNaoFacturados();
                    break;
                case "rptEquipamentoNaoFacturadoAgrupadoBRE": // Listagem de Equipamentos Não Facturados
                    toPdfEquipamentoNaoFacturado();
                    break;
                case "rptEquipamentoNaoFacturadoBRE": // Listagem de Equipamentos Não Facturados por BRE
                    toPdfEquipamentoNaoFacturadoPorBRE();
                    break;
                case "rptEquipamentoNaoFacturadoAgrupadoEmpresa":  // Listagem de Equipamentos Não Facturados(agrupado por Empresa)
                    toPdfEquipamentoNaoFacturadoAgrupadoPorEmpresa();
                    break;
                case "rptNumEquipamentoNaoFacturado": // Número de Equipamentos Não Facturados
                    toPdfNumeroEquipamentosNaoFacturados();
                    break;
                case "rptNumCalibracoesFuncionario": // Número de Calibrações por Funcionário
                    toPdfNumCalibracoesPorFuncionario();
                    break;
                case "rptNumCalibracoesServicosFuncionario": // Número de Calibrações por Funcionário
                    toPdfNumCalibracoesServicosPorFuncionario();
                    break;
                case "rptNumCalibracoesTipoEquipamento": // Número de Calibrações por Conjunto de Tipos de Equipamento
                    toPdfCalibracaoConjuntoTiposEquipamento();
                    break;
                case "rptNumCalibracoesFuncionarioTreino": // Número de Calibrações por Conjunto de Tipos de Equipamento
                    toPdfCalibracoesFuncionarioTreino();
                    break;
                case "rptBresSemRequisicaoNaoFacturados": // Serviços sem Requisição
                    toPdfServicosSemRequisicao();
                    break;
                case "rptEquipamentoNaoFacturadoPesquisaEmpresa": // Serviços sem Requisição
                    toPdfEquipamentoNaoFacturadoPorEmpresa();
                    break;
                case "rptRepLisbtBRE": // Serviços sem Requisição
                    toPdfRepListBRE();
                    break;
                case "rptNovosClientesEntreDatas": // Serviços sem Requisição
                    toPdfNovosClientesEntreDatas();
                    break;
                case "rptClientesActivosEntreDatas": // Serviços sem Requisição
                    toPdfClientesActivosEntreDatas();
                    break;
                case "rptServicosSupervisionados": // Serviços sem Requisição
                    toPdfServicosSupervisionados();
                    break;
                default:
                    break;
            }
        }


        protected void rblReports_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Esconder todos os controls
            hideAllSearchControls();

            // Enable botão para gerar o report
            btnReport.Enabled = true;

            // Depois mostramos só os que interessam para o report seleccionado
            switch (rblReports.SelectedValue)
            {

                case "rptNovasEmpresas": // Listagem de Novas Empresas
                    showNovasEmpresas();
                    break;
                case "rptMelhoresEmpresas": // Listagem das Melhores Empresas
                    showMelhoresEmpresas();
                    break;
                case "rptTrabalhosGranhosPerdidos": // Listagem de Trabalhos Ganhos / Perdidos
                    showTrabalhosGanhosPerdidos();
                    break;
                case "rptOrcamentos": // Listagem de Orçamentos
                    showListaOrcamentos();
                    break;
                case "rptTaxasServico": // Listagem de Orçamentos
                    showListaOrcamentos();//acho que pode ser igual aos orçamentos?
                    break;
                case "rptOrcamentosAtraso": // Listagem de Orçamentos em Atraso
                    showListaOrcamentosAtraso();
                    break;
                case "rptOrcamentosMes": // Nº de Orçamentos por Mês
                    showNumOrcamentosMes();
                    break;
                case "rptTempoMedioEsperaOrcamentos": // Tempo Médio de Espera dos Orçamentos
                    showTempoMedioOrcamentos();
                    break;
                case "rptEquipamentosPrecoZero": // Listagem de Equipamentos Facturados com Preços a Zero
                    showEquipamentoPrecosZero();
                    break;
                case "rptBresNaoFacturados": // Listagem de BRE's Não Facturados
                    showBREsNaoFacturados();
                    break;
                case "rptEquipamentoNaoFacturadoAgrupadoBRE": // Listagem de Equipamentos Não Facturados (agrupado por bre)
                    showEquipamentoNaoFacturado();
                    break;
                case "rptEquipamentoNaoFacturadoBRE": // Listagem de Equipamentos Não Facturados por BRE
                    showEquipamentoNaoFacturadoPorBRE();
                    break;
                case "rptEquipamentoNaoFacturadoAgrupadoEmpresa": // Listagem de Equipamentos Não Facturados (agrupado por empresa)
                    showEquipamentoNaoFacturado(); //Usa a mesma funcao do anterior
                    break;
                case "rptNumEquipamentoNaoFacturado": // Número de Equipamentos Não Facturados
                    showNumeroEquipamentosNaoFacturados();
                    break;
                case "rptNumCalibracoesFuncionario": // Número de Calibrações por Funcionário
                    showNumCalibracoesPorFuncionario();
                    break;
                case "rptNumCalibracoesServicosFuncionario": // Número de Calibrações por Funcionário
                    showNumCalibracoesServicosPorFuncionario();
                    break;
                case "rptNumCalibracoesTipoEquipamento": // Número de Calibrações por Conjunto de Tipos de Equipamento
                    showCalibracaoConjuntoTiposEquipamento();
                    break;
                case "rptNumCalibracoesFuncionarioTreino": // Número de Calibrações por Funcionário em treino (quando bem preenchido)
                    showCalibracoesFuncionarioTreino();
                    break;
                case "rptBresSemRequisicaoNaoFacturados": //Serviços sem requisição
                    break;
                case "rptEquipamentoNaoFacturadoPesquisaEmpresa": // Listagem de Equipamentos Não Facturados por Empresa
                    showEquipamentoNaoFacturadoPorEmpresa();
                    break;

                case "rptRepLisbtBRE": // Listagem de Equipamentos Não Facturados por Empresa
                    showParamDates();//usa esta 
                    break;
                case "rptNovosClientesEntreDatas": // Serviços sem Requisição
                    showParamDates();
                    break;
                case "rptClientesActivosEntreDatas": // Serviços sem Requisição
                    showParamDates();
                    break;
                case "rptServicosSupervisionados": // Serviços sem Requisição
                    showServicosSupervisionados();
                    break;
                default:
                    break;
            }

            lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_PARAMS;
        }


        private void toPdfNovosClientesEntreDatas()
        {
            if (checkParamDates() == true)
            {

                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                     report = new LabMetro.REPORTS_ES.crNovosClientesEntreDatas();
                }

              
                else
                {
                    report = new LabMetro.REPORTS.crNovosClientesEntreDAtas();
                }


                clsReport cr = new clsReport();

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de Novas Empresas"
        private void toPdfClientesActivosEntreDatas()
        {
            if (checkParamDates() == true)
            {

                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                    report = new LabMetro.REPORTS_ES.crClientesActivosEntreDatas();
                }

                
                else
                {
                    report = new LabMetro.REPORTS.crClientesActivosEntreDatas();
                }


                clsReport cr = new clsReport();

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }
     
        // Exportar para PDF o report "Listagem de Novas Empresas"
        private void toPdfNovasEmpresas()
        {
            if (checkParamNovasEmpresas() == true) //uso os params de outra funcao com os mesmos params
            {
                
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                    report = new LabMetro.REPORTS_ES.crNovasEmpresas();
                }

                
                else
                {
                    report = new LabMetro.REPORTS.crNovasEmpresas();
                }

                
                clsReport cr = new clsReport();
                
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem das Melhores Empresas"
        private void toPdfMelhoresEmpresas()
        {
            if (checkParamMelhoresEmpresas() == true)
            {
                
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                    report = new LabMetro.REPORTS_ES.crMelhoresEmpresas();
                }
             
                else
                {
                    report = new LabMetro.REPORTS.crMelhoresEmpresas();
                }
                clsReport cr = new clsReport();

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inValorMin", double.Parse(txtValorMin.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de Trabalhos Ganhos / Perdidos"
        private void toPdfTrabalhosGanhosPerdidos()
        {
            if (checkParamTrabalhosGanhosPerdidos() == true)
            {
                
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                    report = new LabMetro.REPORTS_ES.crTrabalhosGanhosPerdidos();
                }
               
                else
                {
                    report = new LabMetro.REPORTS.crTrabalhosGanhosPerdidos();
                }
                clsReport cr = new clsReport();
                
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inAnoIni", ddAnosIni.SelectedValue);
                report.SetParameterValue("@inAnoFim", ddAnosFim.SelectedValue);
                report.SetParameterValue("@inPeriodo", rblPeriodo.SelectedIndex);
                report.SetParameterValue("@inValorMin", double.Parse(txtSuperior.Text));
                report.SetParameterValue("@inPercVariacao", double.Parse(txtVariacao.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de Orçamentos"
        private void toPdfListaOrcamentos()
        {
            if (checkParamListaOrcamentos() == true)
            {
                
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                    report = new LabMetro.REPORTS_ES.crListaOrcamentos();
                }
                
                else
                {
                    report = new LabMetro.REPORTS.crListaOrcamentos();
                }
                clsReport cr = new clsReport();
                
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inValorMin", double.Parse(txtValorMin.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }


        // Exportar para PDF o report "Listagem de Orçamentos"
        private void toPdfListaTaxasServico()
        {
            if (checkParamListaOrcamentos() == true) //params iguais aos orçamentos
            {

                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

              
                    report = new LabMetro.REPORTS.crListaTaxasServico();
                
                clsReport cr = new clsReport();

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inValorMin", double.Parse(txtValorMin.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de Orçamentos em Atraso"
        private void toPdfListaOrcamentosAtraso()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamListaOrcamentosAtraso() == true)
            {
              
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                if (myApp == "ES_LABMETRO")
                {
                    report = new LabMetro.REPORTS_ES.crListaOrcamentosAtraso();
                }
                else
                {
                    report = new LabMetro.REPORTS.crListaOrcamentosAtraso();
                }
                clsReport cr = new clsReport();

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Nº de Orçamentos por Mês"
        private void toPdfNumOrcamentosMes()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamNumOrcamentosMes() == true)
            {
                crNumOrcamentosMes report = new crNumOrcamentosMes();
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inUltimoAno", ddAnosIni.SelectedValue);
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Tempo Médio de Espera dos Orçamentos"
        private void toPdfTempoMedioOrcamentos()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamTempoMedioOrcamentos() == true)
            {
                crTempoMedioOrcamentos report = new crTempoMedioOrcamentos();
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de Equipamentos Facturados com Preços a Zero"
        private void toPdfEquipamentoPrecosZero()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamEquipamentoPrecosZero() == true)
            {
                crEquipamentoPrecosZero report = new crEquipamentoPrecosZero();
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de BRE's Não Facturados"
        private void toPdfBREsNaoFacturados()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamBREsNaoFacturados() == true)
            {
               
                ReportClass report = null; //marteladas mal estruturadas, sem tempo para refazer tudo

                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crBREsNaoFacturados();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crBREsNaoFacturados();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crBREsNaoFacturados();
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

        // Exportar para PDF o report "Listagem de Equipamentos Não Facturados"
        private void toPdfEquipamentoNaoFacturado()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamEquipamentoNaoFacturado() == true)
            {
                ReportClass report = null; 
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crEquipamentoNaoFacturado();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crEquipamentoNaoFacturado();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crEquipamentoNaoFacturado();
                        break;
                }

                clsReport cr = new clsReport();
                

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@idGrandeza", ddGrandeza.SelectedItem.Value);
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }


        private void toPdfRepListBRE()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamDates() == true) 
            {
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crRepListBRE();
                        break;
                    
                }

                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
               
                cr.exportReportToPDF(report, "Report");
            }
        }
        // copiei o report equip nao facturado. agrupo por empresa.
        private void toPdfEquipamentoNaoFacturadoAgrupadoPorEmpresa()
        {
            //dentro do report é feito a escolha facturar = true (mas rpaido neste momento
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamEquipamentoNaoFacturado() == true)
            {
                 
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crEquipamNaoFacturadoAgrupEmpresa();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crEquipamNaoFacturadoAgrupEmpresa();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crEquipamentoNaoFacturado();
                        break;
                }

                clsReport cr = new clsReport();
                

                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@idGrandeza", ddGrandeza.SelectedItem.Value);
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        // Exportar para PDF o report "Listagem de Equipamentos Não Facturados por BRE"
        private void toPdfEquipamentoNaoFacturadoPorBRE()
        {
            if (checkParamEquipamentoNaoFacturadoPorBRE() == true)
            {
                
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crEquipamentoNaoFacturadoPorBRE();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crEquipamentoNaoFacturadoPorBRE();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crEquipamentoNaoFacturado();
                        break;
                }
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);
                report.SetParameterValue("@inIdBRE", ddBRE.SelectedValue);

                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }
        //======================================================================
        // DISCONNTECTED DATASET =========================================
        //======================================================================
        private DataSet dsServicosNaofacturados()
        {

            LabMetro.DATASETS.DSServicosNaoFacturados ds = new LabMetro.DATASETS.DSServicosNaoFacturados();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objCmd.Connection = objConn;

                objCmd.CommandText = "stpGetServicosNaoFacturadosPorStringEmpresa";
                objCmd.Parameters.AddWithValue("@empresa", txtEmpresa.Text);

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                try
                {
                    da.Fill(ds, "dtServicos");
                }
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex.ToString());
                }

                da.Dispose();
                da = null;
            }

            return ds;
        }


        //vou fazer uma query nova 
        private void toPdfEquipamentoNaoFacturadoPorEmpresa()
        {
            if (txtEmpresa.Text == "")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_EMPRESA;
            }
            else
            {
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.rptServicosNaoFacturados();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.rptServicosNaoFacturados();
                        break;
                    default:
                        report = new LabMetro.REPORTS.rptServicosNaoFacturados();
                        break;
                }
                
                clsReport cr = new clsReport();
                DataSet ds = dsServicosNaofacturados();

                report.SetDataSource(ds);
                ds = null;
                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");

            }

        }


        // Exportar para PDF o report "Número de Equipamentos Não Facturados"
        private void toPdfNumeroEquipamentosNaoFacturados()
        {
            // Validar se foram correctamente preenchidos todos os parâmetros de entrada do report
            if (checkParamNumeroEquipamentosNaoFacturados() == true)
            {
                
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crNumeroEquipamentosNaoFacturados();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crNumeroEquipamentosNaoFacturados();
                        break;
                    default:
                        report = new LabMetro.REPORTS.rptServicosNaoFacturados();
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

        //como fix isso:
        //crio um dataset com 2 tables
        //numa stp devolvo 2 result sets na mesma stp
        //faço fill do dataset e adiciono os tablemappings para serem preenchidas as tabelas correctas
        //faço um report + subreport com as 2 tabelas do dataset, e as ligacao entre report e subreport sao feitas automaticamnete.
        //nos dados do report principal devolvo apenas os registos que quero, mas no subreport tudo e dps é automaticamente feia a seleccao correcta no crystal.


        //======================================================================
        //DATASET PARA AS CALIBRAÇÕES POR FUNCIONARIO COM DETALHES DOS SERVIÇOS
        //os funcionarios em treino usam o mesmo dataset e o mesmo crystal report
        //======================================================================
        public DataSet DSNumCalibracoesPorFuncionario(string query)
        {
            LabMetro.DATASETS.DSNumCalibracoesFuncionario ds = new DATASETS.DSNumCalibracoesFuncionario();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                //exemplo ver fim da pagina
                //fazer fill de dataset com 2 tables com 1 stp: 
                objCmd.Connection = objConn;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                objCmd.CommandText = query;
                if (query == "stpGetNumCalibracoesFuncionarioPorTipoEquipamento")
                {
                    objCmd.Parameters.AddWithValue("@inDtInicio", txtDtInicio.Text);
                    objCmd.Parameters.AddWithValue("@inDtFim", txtDtFim.Text);
                    objCmd.Parameters.AddWithValue("@inIdGrandeza", ddGrandeza.SelectedValue);
                }

                else if (query == "stpRepCalibracoesFuncionarioTreino")
                {
                    objCmd.Parameters.AddWithValue("@idFuncionario", ddFuncionario.SelectedValue);
                }

                //por default preenche table, table1, table2 ... por isso tenho de fazer map às tabelas existentes
                //e dizer qual o nome da tabela que deve fazer fill

                da.TableMappings.Add("Table", "DTCalibracoesPorFuncionario");
                da.TableMappings.Add("Table1", "DTServicosPorFuncionarioTipo");

                try
                {
                   da.Fill(ds);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
                da.Dispose();
            }
            return ds;
        }

        
        //numero de calibrcoes por funcionario com detalhes dos serviços
        private void toPdfNumCalibracoesServicosPorFuncionario()
        {

            if (checkParamNumCalibracoesServicoPorFuncionario() == true)
            {

               string titulo = Resources.Resource.NumCalibracoesPorFuncionario;
               
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.rptCalibracoesPorFuncionario();
                       
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.rptCalibracoesPorFuncionario();
                       
                        break;
                    default:
                        report = new LabMetro.REPORTS.rptCalibracoesPorFuncionario();
                        break;
                }


                clsReport cr = new clsReport();
                DataSet ds = DSNumCalibracoesPorFuncionario("stpGetNumCalibracoesFuncionarioPorTipoEquipamento");

                report.SetDataSource(ds);
                report.SetParameterValue("@dtInicio", txtDtInicio.Text);
                report.SetParameterValue("@dtFim", txtDtFim.Text);
                report.SetParameterValue("@title",titulo );
                
                ds = null;

                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }

        //uso o mesmo dataset que o anterior, mas com outra query, que vai buscar os dados para os funcionarios em treino
        //em vez de ser para os funcionarios

        private void toPdfCalibracoesFuncionarioTreino()
        {
            //rptCalibracoesFuncionarioTreino report = new rptCalibracoesFuncionarioTreino(); deixa de existir
            string titulo = Resources.Resource.NumCalibracoesFuncionarioTreino;

            ReportClass report = null;
            switch (myApp)
            {
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.rptCalibracoesPorFuncionario();
                  
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.rptCalibracoesPorFuncionario();
                    break;
                default:
                    report = new LabMetro.REPORTS.rptCalibracoesPorFuncionario();
                    break;
            }
            clsReport cr = new clsReport();
            
            DataSet ds = DSNumCalibracoesPorFuncionario("stpRepCalibracoesFuncionarioTreino");
            

            report.SetDataSource(ds);

            report.SetParameterValue("@dtInicio", "---");
            report.SetParameterValue("@dtFim", "---");
            report.SetParameterValue("@title",titulo);

            ds = null;
            // Exportar o report para PDF
            cr.exportReportToPDF(report, "Report");

        }


        private void toPdfServicosSupervisionados()
        {
            if (checkParamDates() == true)
            {

                if (ddFuncionario.SelectedIndex > -1 && ddGrandeza.SelectedIndex > -1)
                {

                    ReportClass report = null;

                    report = new LabMetro.REPORTS.rptServicosSupervisionados();

                    clsReport cr = new clsReport();

                    cr.setReportConnectionInfo(report);

                    cr.setReportConnectionInfo(report);
                    report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                    report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                    report.SetParameterValue("@idGrandeza", ddGrandeza.SelectedItem.Value);
                    report.SetParameterValue("@idFuncionario", ddFuncionario.SelectedValue);
                    
                    cr.exportReportToPDF(report, "ServicosSupervisionados"); //dps disto, nada mais é executado
                }
            }
            else
            {
                lblMessage.Text = "Preencha as datas!"; 
            }
          



        }


        //numero de calibracoesPorFuncionario
        private void toPdfNumCalibracoesPorFuncionario()
        {

            if (checkParamNumCalibracoesPorFuncionario() == true)
            {
                 
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crCalibracoesPorFuncionario();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crCalibracoesPorFuncionario();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crCalibracoesPorFuncionario();
                        break;
                }
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);

                string strIds = "";
                foreach (DataListItem dli in DLFuncionario.Items)
                {
                    CheckBox myCheckBox = (CheckBox)dli.FindControl("checkboxFunc");

                    if (myCheckBox.Checked == true)
                    {

                        strIds += DLFuncionario.DataKeys[dli.ItemIndex].ToString();
                        strIds += ",";
                    }
                }

                string delimStr = ",";
                char[] delimiter = delimStr.ToCharArray();

                strIds = strIds.TrimEnd(delimiter);//tem de ser senao manda um vazio no ultimo item
                string[] idsFuncionarios = strIds.Split(delimiter);

                report.SetParameterValue("@inIdFuncionario", idsFuncionarios);
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));

                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }
        // Exportar para PDF o report "Número de Calibrações por Conjunto de Tipos de Equipamento"
        private void toPdfCalibracaoConjuntoTiposEquipamento()
        {
            if (checkParamCalibracaoConjuntoTiposEquipamento() == true)
            {
                
                ReportClass report = null;
                switch (myApp)
                {
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crCalibracaoConjuntoTiposEquipamento();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crCalibracaoConjuntoTiposEquipamento();
                        break;
                    default:
                        report = new LabMetro.REPORTS.crCalibracaoConjuntoTiposEquipamento();
                        break;
                }
                clsReport cr = new clsReport();
                cr.setReportConnectionInfo(report);

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

                report.SetParameterValue("@inIdTipoEquipamento", arrayIdsTipoEquipamento);

                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));

                // Exportar o report para PDF
                cr.exportReportToPDF(report, "Report");
            }
        }


        // Exportar para PDF o report "Número de Calibrações por Conjunto de Tipos de Equipamento"
        private void toPdfServicosSemRequisicao()
        {
            DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();

            string strTitulo = "Serviços Sem Requisição Não Facturados";

            //ultimo param = false, so os serviços n facturados
            DataSet ds = data.DSServicosSemRequisicao("", "", "", "", false, true);

            
            ReportClass report = null;
            switch (myApp)
            {
                case "ES_LABMETRO":
                    report = new LabMetro.REPORTS_ES.rptServicosSemRequisicao();
                    break;
                case "DZ_LABMETRO":
                    report = new LabMetro.REPORTS_DZ.rptServicosSemRequisicao();
                    break;
                default:
                    report = new LabMetro.REPORTS.rptServicosSemRequisicao();
                    break;
            }
            clsReport cr = new clsReport();
           
            report.SetDataSource(ds);
            report.SetParameterValue("@tituloReport", strTitulo);
            
            ds = null;
            // Exportar o report para PDF
            cr.exportReportToPDF(report, "Report");
        }
        


        // Mostrar as datas
        private void showParamDates()
        {
            lblDtInicio.Visible = true;
            txtDtInicio.Visible = true;
            lblDtFim.Visible = true;
            txtDtFim.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Novas Empresas"
        private void showNovasEmpresas()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Listagem das Melhores Empresas"
        private void showMelhoresEmpresas()
        {
            showParamDates();

            lblValorMin.Visible = true;
            txtValorMin.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Trabalhos Ganhos / Perdidos"
        private void showTrabalhosGanhosPerdidos()
        {
            lblAnosIni.Visible = true;
            ddAnosIni.Visible = true;
            lblAnosFim.Visible = true;
            ddAnosFim.Visible = true;
            lblPeriodo.Visible = true;
            rblPeriodo.Visible = true;
            lblSuperior.Visible = true;
            txtSuperior.Visible = true;
            lblVariacao.Visible = true;
            txtVariacao.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Orçamentos"
        private void showListaOrcamentos()
        {
            showParamDates();

            lblValorMin.Visible = true;
            txtValorMin.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Orçamentos em Atraso"
        private void showListaOrcamentosAtraso()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Nº de Orçamentos por Mês"
        private void showNumOrcamentosMes()
        {
            lblUltimoAno.Visible = true;
            ddAnosIni.Visible = true;
        }

        // Mostrar parâmetros de entrada para o report "Tempo Médio de Espera dos Orçamentos"
        private void showTempoMedioOrcamentos()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Equipamentos Facturados com Preços a Zero"
        private void showEquipamentoPrecosZero()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Listagem de BRE's Não Facturados"
        private void showBREsNaoFacturados()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Equipamentos Não Facturados"
        private void showEquipamentoNaoFacturado()
        {
            showParamDates();
            showParamsLaboratorio();
        }

        // Mostrar parâmetros de entrada para o report "Listagem de Equipamentos Não Facturados por BRE"
        private void showEquipamentoNaoFacturadoPorBRE()
        {
            lblBRE.Visible = true;
            ddBRE.Visible = true;

        }

        private void showEquipamentoNaoFacturadoPorEmpresa()
        {
            lblEmpresa.Visible = true;
            txtEmpresa.Visible = true;
        }
        // Mostrar parâmetros de entrada para o report "Número de Equipamentos Não Facturados"
        private void showNumeroEquipamentosNaoFacturados()
        {
            showParamDates();
        }

        // Mostrar parâmetros de entrada para o report "Número de Calibrações por Funcionário"
        private void showNumCalibracoesPorFuncionario()
        {
            showParamDates();

            //string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
            //SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

            //ddGrandeza.Visible = true;
            //lblGrandeza.Visible = true;

            //ddGrandeza.DataSource = dr;
            //ddGrandeza.DataBind();

            //dr.Close();

            DLFuncionario.Visible = true;

            DATA.ListasBD listas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = listas.DRListaFuncionarios();
            DLFuncionario.DataSource = DR;
            DLFuncionario.DataBind();
            DR.Close();

            listas = null;
        }

        // Mostrar parâmetros de entrada para o report "Número de Calibrações por Funcionário"
        private void showNumCalibracoesServicosPorFuncionario()
        {
            showParamDates();

            string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddGrandeza.Visible = true;
            lblGrandeza.Visible = true;

            ddGrandeza.DataSource = dr;
            ddGrandeza.DataBind();

            dr.Close();

            
        }


        private void showParamsLaboratorio()
        {
            ddGrandeza.Visible = true;
            lblGrandeza.Visible = true;

            string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddGrandeza.DataSource = dr;
            ddGrandeza.DataBind();
            ddGrandeza.Items.Insert(0, new ListItem("Todos", ""));

            dr.Close();
        }




        // Mostrar parâmetros de entrada para o report "Número de Calibrações por Conjunto de Tipos de Equipamento"
        private void showCalibracaoConjuntoTiposEquipamento()
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

        // Mostrar parâmetros de entrada para o report "Número de Calibrações por Conjunto de Tipos de Equipamento"
        private void showCalibracoesFuncionarioTreino()
        {

            ddFuncionario.Visible = true;

            //isto nao tem datas pois mostra todas em que o funcionario em treino participou
            DATA.ListasBD listas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = listas.DRListaFuncionariosTreino();

            ddFuncionario.DataSource = DR;
            ddFuncionario.DataBind();
            DR.Close();

            listas = null;
        }


        // Mostrar parâmetros de entrada para o report "Serviços Supervisionados"
        private void showServicosSupervisionados()
        {
            txtDtFim.Visible = true;
            txtDtInicio.Visible = true;
            ddFuncionario.Visible = true;
            ddGrandeza.Visible = true;

            string strSQL = "select idFuncionario, nomeAbreviado as nome from funcionario inner join utilizador on Funcionario.idUtilizador = utilizador.idUtilizador where utilizador.idPerfil in (4, 5, 6) and funcionario.activo = 1 order by  funcionario.nomeAbreviado "; //WHERE activo = 1"; 
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddFuncionario.DataSource = dr;
            ddFuncionario.DataBind();
     
         

             strSQL = "SELECT idGrandeza, descricao FROM Grandeza order by 2"; //WHERE activo = 1"; 
            dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddGrandeza.DataSource = dr;
            ddGrandeza.DataBind();
            dr.Close();

            dr = null;

           
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

        // Validação dos parâmetros do report "Listagem de Novas Empresas"
        private bool checkParamNovasEmpresas()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Listagem das Melhores Empresas"
        private bool checkParamMelhoresEmpresas()
        {
            if (checkParamDates() == true)
            {
                try
                {
                    double.Parse(txtValorMin.Text);
                    return true;
                }
                catch
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_VALORES_INCORRECTOS;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Validação dos parâmetros do report "Listagem de Trabalhos Ganhos / Perdidos"
        private bool checkParamTrabalhosGanhosPerdidos()
        {
            try
            {
                double.Parse(txtSuperior.Text);
                double.Parse(txtVariacao.Text);
                return true;
            }
            catch
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_VALORES_INCORRECTOS;
                return false;
            }
        }

        // Validação dos parâmetros do report "Listagem de Orçamentos"
        private bool checkParamListaOrcamentos()
        {
            if (checkParamDates() == true)
            {
                try
                {
                    double.Parse(txtValorMin.Text);
                    return true;
                }
                catch
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_VALORES_INCORRECTOS;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Validação dos parâmetros do report "Listagem de Orçamentos em Atraso"
        private bool checkParamListaOrcamentosAtraso()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Nº de Orçamentos por Mês"
        private bool checkParamNumOrcamentosMes()
        {
            return true;
        }
        // Validação dos parâmetros do report "Tempo Médio de Espera dos Orçamentos"
        private bool checkParamTempoMedioOrcamentos()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Listagem de Equipamentos Facturados com Preços a Zero"
        private bool checkParamEquipamentoPrecosZero()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Listagem de BRE's Não Facturados"
        private bool checkParamBREsNaoFacturados()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Listagem de Equipamentos Não Facturados"
        private bool checkParamEquipamentoNaoFacturado()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Listagem de Equipamentos Não Facturados por BRE"
        private bool checkParamEquipamentoNaoFacturadoPorBRE()
        {
            if (ddBRE.SelectedIndex > 0)
            {
                return true;
            }
            else // se não tiver sido seleccionado nenhum BRE
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_BRE;
                return false;
            }
        }

        // Validação dos parâmetros do report "Número de Equipamentos Não Facturados"
        private bool checkParamNumeroEquipamentosNaoFacturados()
        {
            return checkParamDates();
        }

        // Validação dos parâmetros do report "Número de Calibrações por Funcionário"
        private bool checkParamNumCalibracoesPorFuncionario()
        {
            return checkParamDates();
           

        }

         // Validação dos parâmetros do report "Número de Calibrações por Funcionário com detalhes serviços"
        private bool checkParamNumCalibracoesServicoPorFuncionario()
        {
            return checkParamDates();

        }
        

        // Validação dos parâmetros do report "Número de Calibrações por Conjunto de Tipos de Equipamento"
        private bool checkParamCalibracaoConjuntoTiposEquipamento()
        {
            return checkParamDates();
        }

//If you have a stored procedure with two select statements in it:

//=-=-=-=-
//....
//select *
//from tblProducts
//where strproductCode = @strProductCode

//select * from tblJctProductColors
//....
//=-=-=-=-

//and you call SqlDataAdapter.Fill(dataSet), you will find there are two
//tables (rowsets) returned upon return from the Fill() method (provided both
//select statements return data. If you want to examine the DataSet, you can
//set a breakpoint and open a watch window to verify the contents of the
//dataSet object.)

//You can then name the tables and specify the relationship between the tables
//using syntax similar to:

//=-=-=-=-=-=-
//dataSet.Table[0].TableName = "Products";
//dataSet.Table[1].TableName = "Colors";

//dataSet.Relations.Add("ProductColor",
//dsTables["Products"].Columns["ProductID"],
//dsTables["ProductColor"].Columns["ColorID"]);
//=-=-=-=-=-=-

//Hope this helps.
//--
//brians
//http://www.limbertech.com


    }
}
