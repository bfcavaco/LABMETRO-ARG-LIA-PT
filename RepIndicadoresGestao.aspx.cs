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


namespace LabMetro
{
    public partial class RepIndicadoresGestao : System.Web.UI.Page
    {


        //protected System.Web.UI.WebControls.DataGrid dgFuncionario;

        private const string ID_PAG = "EST_INDICADORES_0";//NOME DA PAGINA
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
                       
                    }
                }
            }
        }

        // *****************************************************************************
        // Exportação do report para EXCEL
        // *****************************************************************************

        protected void btnExportExcel_Click(object sender, System.EventArgs e)
        {

            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@inDtInicio", txtDtInicio.Text);
            arrParams[1] = new SqlParameter("@inDtFim", txtDtFim.Text);

            DataTable DT = clsDataAccess.SPExecuteDTParams("stpRepIndicadoresLabmetro",arrParams);
                DataView DV = new DataView(DT);
            gvReport.DataSource = DV;
            gvReport.DataBind();
            GERAL.GridViewExportUtil.Export("Indicadores.xls", gvReport);
                

        }
            // *****************************************************************************
            // Exportação do report para PDF
            // *****************************************************************************

            protected void btnReport_Click(object sender, System.EventArgs e)
        {
            if (checkParamDates() == true)
            { 
              
            rptIndicadoresGestao report = new rptIndicadoresGestao();
            clsReport cr = new clsReport();
                try
                {
                    cr.setReportConnectionInfo(report);
                    report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                    report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));

                    cr.exportReportToPDF(report, "rptIndicadoresGestao");
                }
                catch (Exception ex)
                {

                    Response.Write(ex.InnerException.ToString());

                }
            }
            else
            {
                lblMessage.Text = "Verifique as datas.";

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

        //private void loadReport()
        //{

        //        try { 
        //    ReportClass report = new LabMetro.REPORTS.rptIndicadoresGestao();
              
        //    clsReport cr = new clsReport();
            
        //    cr.setReportConnectionInfo(report);
        //    report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
        //    report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
            
        //    //CrystalReportViewer1.ReportSource = report;
           
        //    Session.Add("report", report);
        //    }
        //    catch(Exception ex)
        //    {
        //            Response.Write(ex.ToString());
        //   }
        //}
       
    }
}
