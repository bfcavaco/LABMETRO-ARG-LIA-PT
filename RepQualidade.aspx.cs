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
using System.Data.SqlClient;
using System.Configuration;
using LabMetro.REPORTS;
using LabMetro.GERAL;

namespace LabMetro
{
    public partial class RepQualidade : System.Web.UI.Page
    {
        private const string ID_PAG = "REP_QUALIDADE_0";//NOME DA PAGINA

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
                        ViewState["sortField"] = "descGrandeza";
                        ViewState["sortDirection"] = "ASC";

                        DateTime dt = DateTime.Now;
                        string dia = dt.Day.ToString();
                        string year = dt.Year.ToString();

                        string dtInicio = "01-01-" + dt.Year.ToString(); //o dia 1 de Janeiro do ano Corrente.
                        txtDtInicio.Text = dtInicio;
                        txtDtFim.Text = dt.ToShortDateString();
                        txNumDias.Text = "7"; //da primeira vez

                        BindGrid();
                        filddDias();
                        fillddGrandeza();

                    }
                    // Put user code to initialize the page here


                    //fazer um agrupamento por dias como já estava feito originalmente.
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
            btnRecalcular.Click += new System.EventHandler(btnRecalcular_Click);
            ddDias.SelectedIndexChanged += new System.EventHandler(ddDias_SelectedIndexChanged);
            btnReport.Click += new System.EventHandler(btnReport_Click);
            btnServicos.Click += new System.EventHandler(btnServicos_Click);
            btnDias.Click += new System.EventHandler(btnDias_Click);
        }
        #endregion

        //========================================================================
        //DATASET QUE SERVE COMO DATASOURCE DOS RESULTADOS PRINCIPAIS.
        //========================================================================
        private DataSet DSCalibs()
        {
            if (checkParamDates() == true)
            {
                string dtInicio = txtDtInicio.Text.TrimEnd() + " 00:00:00.000";
                string dtFim = txtDtFim.Text.TrimEnd() + " 00:00:00.000";

                LabMetro.DATASETS.DSDiasCalibracaoEstatistica myDS = new LabMetro.DATASETS.DSDiasCalibracaoEstatistica();

                string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
                using (SqlConnection objConn = new SqlConnection(connectionString))
                using (SqlCommand objCmd = new SqlCommand())
                {
                    SqlDataAdapter myDA = new SqlDataAdapter(objCmd);
                    objCmd.Connection = objConn;
                    objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "stpRepTotaisDias";

                    objCmd.Parameters.AddWithValue("@inDtInicio", txtDtInicio.Text);
                    objCmd.Parameters.AddWithValue("@inDtFim", txtDtFim.Text);
                    objCmd.Parameters.AddWithValue("@inNumDias", txNumDias.Text);
                    //mandar os params das datas.

                    try
                    {
                        myDA.Fill(myDS, "dtTempTotaisDias");
                    }
                    catch (Exception ex)
                    {
                        GERAL.clsWriteError.WriteLog(ex.ToString());
                    }

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "stpRepTotaisDiasIntExt";

                    //os params ja estao adicionados ao command, por isso nao devo adiciona-los de  novo
                    //mandar os params das datas.

                    try
                    {
                        myDA.Fill(myDS, "dtTempTotaisDiasIntExt");
                    }
                    catch (Exception ex)
                    {
                        GERAL.clsWriteError.WriteLog(ex.ToString());
                    }

                    myDA.Dispose();
                }

                return myDS;
            }
            else
            {
                return null;
            }
        }
        //========================================================================
        //BINDGRID PRINCIPAL. RESULTADOS POR GRANDEZA, ATÉ 7 DIAS, COM A RESPECTIVA PERCENTAGEM
        //========================================================================
        private void BindGrid()
        {
            DataSet DS = DSCalibs();
            if (DS != null)
            {
                DataView DV = new DataView(DS.Tables["dtTempTotaisDiasIntExt"]);
                DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

                dgTotais.DataSource = DV;
                dgTotais.DataBind();

                DV = null;

                DS = null;
            }

            DS = null; 	//just in case
        }

        //========================================================================
        // BOTÃO VER RELATÓRIO
        // MOSTRA CRYSTAL REPORT
        //========================================================================
        private void btnReport_Click(object sender, System.EventArgs e)
        {
            if (checkParamDates() == true)
            {
                rptSeteDias report = new rptSeteDias(); //n mudei nome do report

                clsReport cr = new clsReport();

                DataSet ds = DSCalibs();

               

                report.SetDataSource(ds);
                //params do report!
                report.SetParameterValue("@inDtInicio", DateTime.Parse(txtDtInicio.Text));
                report.SetParameterValue("@inDtFim", DateTime.Parse(txtDtFim.Text));
                report.SetParameterValue("@inNumDias", txNumDias.Text);
                ds = null;
                cr.exportReportToPDF(report,"Report");

                
                //cr = null;
                //report = null;
            }

        }



        protected void dgTotais_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.Cells[1].Text = "Cal.Int.";
                e.Item.Cells[2].Text = "Cal.Int<br />até " + txNumDias.Text.ToString() + "dias<br />(incl.)";
                e.Item.Cells[3].Text = "%Cal.Int<br />até " + txNumDias.Text.ToString() + "dias<br />(incl.)";

                e.Item.Cells[4].Text = "Cal.Ext.";
                e.Item.Cells[5].Text = "Cal.Ext<br />até " + txNumDias.Text.ToString() + "dias<br />(incl.)";
                e.Item.Cells[6].Text = "%Cal.Ext<br />até " + txNumDias.Text.ToString() + "dias<br />(incl.)";

                e.Item.Cells[7].Text = "Serv.Tot.";
                e.Item.Cells[8].Text = "Serv.Tot.<br />até " + txNumDias.Text.ToString() + "dias<br />(incl.)";
                e.Item.Cells[9].Text = "%Serv.Tot.<br />até " + txNumDias.Text.ToString() + "dias<br />(incl.)";
            }
        }

        //========================================================================
        //SORTGRID - NÃO UTILIZADO PORQUE CADA SORT OBRIGADA A RECARREGAR A INFO TODA
        //========================================================================
        private void SortGrid(Object s, DataGridSortCommandEventArgs e)
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
            BindGrid();
        }

        //========================================================================
        // Validação das datas
        //========================================================================
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


        //========================================================================
        //BIND DATAGRID DOS DIAS
        //========================================================================
        private void BindGridDias()
        {
            if (checkParamDates() == true)
            {
                string dtInicio = txtDtInicio.Text.TrimEnd() + " 00:00:00.000";
                string dtFim = txtDtFim.Text.TrimEnd() + " 00:00:00.000";

                //	string strSQL = "SELECT DISTINCT  vTempoCalibracoesMinusSuspensos.idGrandeza, count(numdiasUteis) as numServicos,numDiasUteis as numDiasUteis, descricao as Grandeza FROM vTempoCalibracoesCertificado INNER JOIN Grandeza ON vTempoCalibracoesMinusSuspensos.idGrandeza = Grandeza.idGrandeza  WHERE dtBre BETWEEN (CONVERT(DATETIME, '"+dtInicio+"', 105))  AND CONVERT(DATETIME,'"+dtFim+"', 105) "; 

                //string strSQL = "SELECT DISTINCT  v.idGrandeza, count(numdiasUteis) as numServicos,numDiasUteis as numDiasUteis, descricao as Grandeza FROM vTempoCalibracoesMinusSuspensos v INNER JOIN Grandeza ON v.idGrandeza = Grandeza.idGrandeza  WHERE dtBre BETWEEN (CONVERT(DATETIME, '"+dtInicio+"', 105))  AND CONVERT(DATETIME,'"+dtFim+"', 105) "; 

                //VERIFICAR ISTO 
                //ESPECIALMENTE O COUNT(NUMDIASUTEIS)
                //VER SE DEVO USAR O DATAREADER OU DATATABLE E TESTAR ISTO BEM COM TODOS OS ANOS!!!! A PARTI DE 01-01-2006...
                //POIS NAO ME DÁ RESULTADOS...

                //				//SELECT DISTINCT  v.idGrandeza, count(numdiasUteis) as numServicos,descricao as Grandeza,
                //case when(numDiasUteis-numDiasSuspenso) = 0 then 1  else (numDiasUteis-numDiasSuspenso) 
                //end as numDiasUteisV
                //FROM vTempoCalibracoesMinusSuspensos v 
                //INNER JOIN Grandeza ON v.idGrandeza = Grandeza.idGrandeza  
                //WHERE dtBre BETWEEN (CONVERT(DATETIME, '01-01-2007', 105))  AND CONVERT(DATETIME,'01-12-2007', 105)
                //AND numDiasUteis <= 7 AND v.idGrandeza = 'dim'
                //group by v.idGrandeza, numdiasUteis,Grandeza.descricao
                //having 
                //ORDER BY 4 --v.idGrandeza ,grandeza.descricao, v.numDiasUteis, v.numDiasSuspenso

                string strSQL = "SELECT DISTINCT  v.idGrandeza, count(numdiasUteis) as numServicos,case when(numDiasUteis-numDiasSuspenso) = 0 then 1  else (numDiasUteis-numDiasSuspenso) end as numDiasUteis, descricao as Grandeza FROM vTempoCalibracoesMinusSuspensos v INNER JOIN Grandeza ON v.idGrandeza = Grandeza.idGrandeza  WHERE dtBre BETWEEN (CONVERT(DATETIME, '" + dtInicio + "', 105))  AND CONVERT(DATETIME,'" + dtFim + "', 105) ";
                if (ddDias.SelectedValue != "")
                {
                    strSQL += " AND numDiasUteis <= " + ddDias.SelectedValue.ToString();
                }

                if (ddGrandeza.SelectedValue != "")
                {
                    strSQL += " AND v.idGrandeza = '" + ddGrandeza.SelectedValue.ToString() + "' ";
                }

                strSQL += " group by v.idGrandeza, v.numdiasUteis,Grandeza.descricao ORDER BY v.idGrandeza , v.numDiasUteis, grandeza.descricao";

                //Response.Write ("strDias<br />" + strSQL + "<br />"); 

                //DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
                dgDias.DataSource = dr; //dt; 
                dgDias.DataBind();
                dr.Close();

            }
        }


        //========================================================================
        //BIND DATAGRID DOS SERVIÇOS
        //========================================================================
        private void BindGridServicos()
        {
            if (checkParamDates() == true)
            {

                string dtInicio = txtDtInicio.Text.TrimEnd() + " 00:00:00.000";
                string dtFim = txtDtFim.Text.TrimEnd() + " 00:00:00.000";

                //string strSQL = "SELECT * FROM vTempoCalibracoesCertificado WHERE dtBre between (CONVERT(DATETIME, '"+dtInicio+"', 105))  AND CONVERT(DATETIME,'"+dtFim+"', 105) "; 
                //string strSQL = "SELECT idServico,refServico,dtBRE,dtCalibracao,calibracaoExterna,(numDiasUteis-numDiasSuspenso) as numDiasUteis FROM vTempoCalibracoesMinusSuspensos WHERE dtBre between (CONVERT(DATETIME, '"+dtInicio+"', 105))  AND CONVERT(DATETIME,'"+dtFim+"', 105) "; 
                string strSQL = "SELECT idServico,refServico,dtBRE,dtCalibracao, dtInicio, calibracaoExterna,case when(numDiasUteis-numDiasSuspenso) = 0 then 1  else (numDiasUteis-numDiasSuspenso) end as numDiasUteis  FROM vTempoCalibracoesMinusSuspensos WHERE dtBre between (CONVERT(DATETIME, '" + dtInicio + "', 105))  AND CONVERT(DATETIME,'" + dtFim + "', 105) ";
                if (ddDias.SelectedValue != "")
                {
                    strSQL += " AND (numDiasUteis-numDiasSuspenso) <= " + ddDias.SelectedValue;
                }
                if (ddGrandeza.SelectedValue != "")
                {
                    strSQL += " AND idGrandeza = '" + ddGrandeza.SelectedValue.ToString() + "' ";
                }
                strSQL += " ORDER BY numDiasUteis DESC";

                //Response.Write ("strServicos<br />" + strSQL + "<br />"); 

                //DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
                dgServicos.DataSource = dr;
                dgServicos.DataBind();
                dr.Close();
            }
        }


        //========================================================================
        //FILL DA DROPDOWN DIAS
        //========================================================================
        private void filddDias()
        {
            if (checkParamDates() == true)
            {
                string dtInicio = txtDtInicio.Text.TrimEnd() + " 00:00:00.000";
                string dtFim = txtDtFim.Text.TrimEnd() + " 00:00:00.000";

                string strSQL = "SELECT DISTINCT (numDiasUteis-numDiasSuspenso) as numDiasUteis FROM vTempoCalibracoesMinusSuspensos WHERE dtBre between (CONVERT(DATETIME, '" + dtInicio + "', 105))  AND CONVERT(DATETIME,'" + dtFim + "', 105) ";
                if (ddDias.SelectedValue != "")
                {
                    strSQL += " AND (numDiasUteis-numDiasSuspenso) <= " + ddDias.SelectedValue;
                }
                if (ddGrandeza.SelectedValue != "")
                {
                    strSQL += " AND idGrandeza = '" + ddGrandeza.SelectedValue.ToString() + "' ";
                }

                strSQL += " ORDER BY numDiasUteis DESC";


                //Response.Write ("strddDias<br />" + strSQL + "<br />"); 
                DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL);
                ddDias.DataSource = dt;
                ddDias.DataBind();
                ddDias.Items.Insert(0, new ListItem("Todos", ""));
            }

        }

        private void fillddGrandeza()
        {
            string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddGrandeza.DataSource = dr;
            ddGrandeza.DataBind();

            dr.Close();

            ddGrandeza.Items.Insert(0, new ListItem("Todas", ""));
        }


        //========================================================================
        //BOTÃO VER DIAS
        //Preenche o datagrid dos dias
        //========================================================================
        private void btnDias_Click(object sender, System.EventArgs e)
        {
            if (ddGrandeza.SelectedValue == "")
            {
                lblMessage.Text = "Seleccione uma grandeza.";
            }
            else
            {
                BindGridDias();
            }
        }


        //========================================================================
        //BOTÃO VER SERVIÇOS
        //Preenche o datagrid dos serviços
        //========================================================================
        private void btnServicos_Click(object sender, System.EventArgs e)
        {
            if (ddGrandeza.SelectedValue == "")
            {
                lblMessage.Text = "Seleccione uma grandeza.";
            }
            else
            {
                BindGridServicos();
            }
        }

        private void ddDias_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        protected void doPagingServicos(Object s, DataGridPageChangedEventArgs e)
        {
            dgServicos.CurrentPageIndex = e.NewPageIndex;
            BindGridServicos();
        }

        private void btnRecalcular_Click(object sender, System.EventArgs e)
        {
            BindGrid();
            filddDias();

            //LIMPAR OS OUTROS
            dgDias.DataSource = null;
            dgDias.DataBind();
            dgServicos.DataSource = null;
            dgServicos.DataBind();
        }

        protected string ConverteEstado(bool b)
        {
            if (b == true) return "sim";
            else return "não";
        }

        protected string Calculapercentagem(double total, double parcial)
        {

            double f = Math.Round((100.00 * parcial) / total, 2);

            return f.ToString();

        }



        protected void dgServicos_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
    }
}

