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
namespace LabMetro
{
    public partial class RelatoriosIgae : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.DataGrid dgGrandezas;

        private const string ID_PAG = "RELATORIOSIGAE_0";//NOME DA PAGINA

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
                        
                        // Preencher as datas com um valor default
                        DateTime dt = DateTime.Now;
                        string dia = dt.Day.ToString();
                        string year = dt.Year.ToString();


                        string dtInicio = "01-01-" + dt.Year.ToString(); //o dia 1 de Janeiro do ano Corrente.
                        txtDtInicio.Text = dtInicio;
                        txtDtFim.Text = dt.ToShortDateString();

                       
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


        //lista de verificacoes com tipoequipamento, municipio, data, resultado, empresa
        protected void button1_Click(object sender, EventArgs e)
        {
            if (checkParamDates())
            {
                DataSet ds = DSListaVerificacoes();
                if (ds != null)
                {
                   
                    LabMetro.REPORTS_CV.crVerificacoes report = new LabMetro.REPORTS_CV.crVerificacoes();

                    clsReport cr = new clsReport();
                    report.SetDataSource(ds);
                    report.SetParameterValue("entre", txtDtInicio.Text);
                    report.SetParameterValue("e", txtDtFim.Text);
                    string municipio = "todos";
                    if (ddConcelho.SelectedValue != "") municipio = ddConcelho.SelectedItem.Text;
                    report.SetParameterValue("municipio", municipio);
                    ds = null;

                    cr.exportReportToPDF(report,"IGAE");

                    //cr = null;
                    //report = null;
                   
                }
                else
                {
                    lblMessage.Text = "Sem resultados";
                }
            }
        }

        //lista de verificacoes agrupada por municipio tipo 
         protected void button2_Click(object sender, EventArgs e)
         {
             if (checkParamDates())
             {
                 DataSet ds = DSVerificacoesAgrupadasMunicipioTipo();
                 if (ds != null)
                 {

                     LabMetro.REPORTS_CV.crVerificacoesAgrupadas report = new LabMetro.REPORTS_CV.crVerificacoesAgrupadas();

                     clsReport cr = new clsReport();

                     report.SetDataSource(ds);
                     report.SetParameterValue("entre", txtDtInicio.Text);
                     report.SetParameterValue("e", txtDtFim.Text);
                     string municipio = "todos";
                     if (ddConcelho.SelectedValue != "") municipio = ddConcelho.SelectedItem.Text;
                     report.SetParameterValue("municipio", municipio);
                     ds = null;
                     cr.exportReportToPDF(report,"Report");

                     //cr = null;
                     //report = null;
             
                 }
                 else
                 {
                     lblMessage.Text = "Sem resultados";
                 }
             }
         }

         //lista de verificacoes agrupada por municipio e aprovado, reprovado
         protected void button3_Click(object sender, EventArgs e)
         {
             if (checkParamDates())
             {
                 DataSet ds = DSVerificacoesAgrupadasMunicipioTotal();
                 if (ds != null)
                 {

                     LabMetro.REPORTS_CV.crVerificacoesTotais report = new LabMetro.REPORTS_CV.crVerificacoesTotais();

                     clsReport cr = new clsReport();

                     report.SetDataSource(ds);
                     report.SetParameterValue("entre", txtDtInicio.Text);
                     report.SetParameterValue("e", txtDtFim.Text);
                     string municipio = "todos";
                     if (ddConcelho.SelectedValue != "") municipio = ddConcelho.SelectedItem.Text;
                     report.SetParameterValue("municipio", municipio);
                     ds = null;
                     cr.exportReportToPDF(report,"Report");

                     //cr = null;
                     //report = null;
                     
                 }
                 else
                 {
                     lblMessage.Text = "Sem resultados";
                 }
             }
         }

         protected void button4_Click(object sender, EventArgs e)
         {
             if (checkParamDates())
             {
                 DataSet ds = DSPlanoVerificacoes();
                 if (ds != null)
                 {

                     LabMetro.REPORTS_CV.crPlanoVerificacoes report = new LabMetro.REPORTS_CV.crPlanoVerificacoes();

                     clsReport cr = new clsReport();

                     report.SetDataSource(ds);
                     report.SetParameterValue("entre", txtDtInicio.Text);
                     report.SetParameterValue("e", txtDtFim.Text);
                     string municipio = "todos";
                     if (ddConcelho.SelectedValue != "") municipio = ddConcelho.SelectedItem.Text;
                     report.SetParameterValue("municipio", municipio);
                     ds = null;
                     cr.exportReportToPDF(report,"Report");

                     //cr = null;
                     //report = null;
                     
                 }
                 else
                 {
                     lblMessage.Text = "Sem resultados";
                 }
             }
         }

         private DataSet DSPlanoVerificacoes() 
         {
             DataSet ds = new DATASETS.DSVerificacoes();

             SqlParameter[] arrParams = new SqlParameter[3];
             arrParams[0] = new SqlParameter("@inDtInicio", txtDtInicio.Text);
             arrParams[1] = new SqlParameter("@inDtFim", txtDtFim.Text);
             arrParams[2] = new SqlParameter("@inIdConcelho", ddConcelho.SelectedValue);

             ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

             try
             {
                 ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListPlanoVerificacoes", ds, "PlanoVerificacoes", arrParams);
                 return ds;

             }
             catch
             {
                 //Response.Write(ex.ToString()); 
                 return null;
             }
         }


        private DataSet DSListaVerificacoes()  
        {
            DataSet ds = new DATASETS.DSVerificacoes();

            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inDtInicio", txtDtInicio.Text);
            arrParams[1] = new SqlParameter("@inDtFim", txtDtFim.Text);
            arrParams[2] = new SqlParameter("@inIdConcelho", ddConcelho.SelectedValue);
          
            ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

            try                                            
            {
                ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListRepVerificacoes", ds, "Verificacoes",arrParams);
                return ds;

            }
            catch(Exception)
            {
                //Response.Write(ex.ToString()); 
                return null;
            }
        }


        private DataSet DSVerificacoesAgrupadasMunicipioTipo()  
        {
            DataSet ds = new DATASETS.DSVerificacoes();

            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inDtInicio", txtDtInicio.Text);
            arrParams[1] = new SqlParameter("@inDtFim", txtDtFim.Text);
            arrParams[2] = new SqlParameter("@inIdConcelho", ddConcelho.SelectedValue);

            ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

            try
            {
                ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListRepVerificacoesAgrupadasMunicipioTipo", ds, "VerificacoesAgrupadas", arrParams);
                return ds;

            }
            catch (Exception)
            {
                //Response.Write(ex.ToString());
                return null;
            }
        }


        private DataSet DSVerificacoesAgrupadasMunicipioTotal()  
        {
            DataSet ds = new DATASETS.DSVerificacoes();

            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inDtInicio", txtDtInicio.Text);
            arrParams[1] = new SqlParameter("@inDtFim", txtDtFim.Text);
            arrParams[2] = new SqlParameter("@inIdConcelho", ddConcelho.SelectedValue);

            ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

            try
            {
                ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListRepVerificacoesAgrupadasMunicipioTotal", ds, "VerificacoesAgrupadasTotal", arrParams);
                return ds;

            }
            catch (Exception)
            {
                //Response.Write(ex.ToString());
                return null;
            }
        }


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
     }
}
