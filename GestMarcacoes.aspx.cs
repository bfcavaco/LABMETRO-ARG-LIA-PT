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
using LabMetro.REPORTS;
using LabMetro.GERAL;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;



namespace LabMetro
{
    /// <summary>
    /// Summary description for MarcacoesSemana.
    /// </summary>
    public partial class GestMarcacoes : System.Web.UI.Page
    {
        private const string ID_PAG = "GESTMARCACOES_1";//NOME DA PAGINA

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
                        ViewState["sortDirection"] = "ASC";
                        ViewState["sortField"] = "Date";
                        ViewState["numFaxCliente"] = "";
                        ViewState["emailCliente"] = "";
                        fillDDfuncionarios();
                        txtDataInicio.Text = DateTime.Today.ToShortDateString();
                        txtDataFim.Text = DateTime.Today.AddDays(7).ToShortDateString();

                    }
                }
            }
        }


        private DataTable dtMarcacoesPorDataFuncionario()
        {

            string dtStart = txtDataInicio.Text;
            string dtEnd = txtDataFim.Text;

            string strSemFax = "";
            if (cbSemFax.Checked == true) strSemFax = "0";

            SqlParameter[] arrParams = new SqlParameter[7];

            arrParams[0] = new SqlParameter("@inDateStart", dtStart);//DateTime.Today.ToShortDateString());
            arrParams[1] = new SqlParameter("@inDateEnd", dtEnd); //DateTime.Today.AddDays(7).ToShortDateString());
            arrParams[2] = new SqlParameter("@inIdFuncionario", ddTecnicoExterior.SelectedValue);
            arrParams[3] = new SqlParameter("@inBFaxEnviado", strSemFax);
            arrParams[4] = new SqlParameter("@inEmpresa", txtEmpresa.Text);
            arrParams[5] = new SqlParameter("@inRefMarcacao", txtRefMarcacao.Text);
            arrParams[6] = new SqlParameter("@inbGeral", "0");


            if (cbLinhasUnicas.Checked == true)
            {
                return GERAL.clsDataAccess.SPExecuteDTParams("stpGetListMarcacoesLinhasUnicas", arrParams);
            }


            return GERAL.clsDataAccess.SPExecuteDTParams("stpGetListMarcacoesBetweenDates", arrParams);

        }

        //======================================================================================
        //
        //======================================================================================
        private void BindGridMarcacoes()
        {
            //aqui devo considerar juntar a informação do contacto ou ver como posso obter os dados do contacto para envio de email fax...

            string sHoje = DateTime.Today.ToShortDateString();

            try
            {
                DataTable dt = dtMarcacoesPorDataFuncionario();
                DataView dv = new DataView(dt);
                dv.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
                DataGrid1.DataSource = dv;
                DataGrid1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        //======================================================================================
        //preenche a dropdown com os funcionarios marcados como sendo de "calibracao externa".
        //======================================================================================
        private void fillDDfuncionarios()
        {
            string strSQL = "select idFuncionario, nomeAbreviado from funcionario where bCta = 1 and activo = 1 ";
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddTecnicoExterior.DataSource = dr;
            ddTecnicoExterior.DataBind();
            ddTecnicoExterior.Items.Insert(0, new ListItem("", ""));
            dr.Close();
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
            ddTecnicoExterior.SelectedIndexChanged += new System.EventHandler(ddTecnicoExterior_SelectedIndexChanged);
            btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
            DataGrid1.ItemCommand += new DataGridCommandEventHandler(DataGrid1_ItemCommand);
        }
        #endregion

        //=================================================================================================
        //PAGING 
        //=================================================================================================
        public void doPaging(Object s, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindGridMarcacoes();

        }

        //=================================================================================================
        //SORTGRID 
        //=================================================================================================
        public void SortGrid(Object s, DataGridSortCommandEventArgs e)
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

            BindGridMarcacoes();
        }

        private void ddTecnicoExterior_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindGridMarcacoes();
        }



        protected string diaSemana(object iDia)
        {

            if (!Convert.IsDBNull(iDia))
            {

                //string[] dia = new string[6] {"0", "2aFeira", "3aFeira", "4aFeira", "5aFeira", "6aFeira"}; 
                string[] dia = new string[8] { "0", "2aFeira", "3aFeira", "4aFeira", "5aFeira", "6aFeira", "Sábado", "Domingo" };

                int i = Convert.ToInt16(iDia);
                return dia[i];
            }
            return String.Empty;
        }

        protected void dg_databound(object sender, DataGridItemEventArgs e)
        {
            DataRowView DRV = (DataRowView)e.Item.DataItem;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btn = (System.Web.UI.WebControls.LinkButton)e.Item.Cells[21].Controls[0];
                btn.Attributes.Add("onclick", "return confirm_delete();");

                LinkButton btnfax = (System.Web.UI.WebControls.LinkButton)e.Item.Cells[18].Controls[0];
                btnfax.Attributes.Add("onclick", "return confirm_fax();");

                LinkButton btnmail = (System.Web.UI.WebControls.LinkButton)e.Item.Cells[19].Controls[0];
                btnfax.Attributes.Add("onclick", "return confirm_mail();");

                HyperLink linkBRE = (System.Web.UI.WebControls.HyperLink)e.Item.Cells[7].Controls[0];

                if (DRV["bdefinitivo"].ToString() == "False")
                {
                    linkBRE.NavigateUrl = "FormBreCalibExt.aspx?btn=DOC&id=" + DRV["idBRE"];
                }
                else
                {
                    linkBRE.NavigateUrl = "FormBre.aspx?btn=DOC&id=" + DRV["idBRE"];
                }
            }
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddFuncionario = (DropDownList)e.Item.FindControl("ddFuncionarioEdit");


                string strSQL = "select idFuncionario, nomeAbreviado as nome from funcionario where bCta = 1 and activo = 1 ";
                SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
                ddFuncionario.DataSource = dr;
                ddFuncionario.DataBind();
                dr.Close();

                string idF = DRV["idfuncionario"].ToString();
                if (idF != "") ddFuncionario.SelectedValue = idF;

                DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaEdit");

                string idg = DRV["idGrandeza"].ToString();
                if (idg != "") ddGrandeza.SelectedValue = idg;


                DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");


                //a requisicao está sempre associada à mesma empresa que os serviços, por isso tudo bem. 

                DATA.RequisicaoBD requisicao = new LabMetro.DATA.RequisicaoBD();
                SqlDataReader DR = requisicao.DRGetRequisicoesIncompletasByEmpresa(DRV["idEmpresa"].ToString());
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

        private void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DataGrid1.CurrentPageIndex = 0;
            BindGridMarcacoes();
        }


        protected void editGrid(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = e.Item.ItemIndex;
            BindGridMarcacoes();
        }

        protected void cancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = -1;
            BindGridMarcacoes();
        }


        private bool deleteMarcacao(string idMarcacao)
        {
            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                objConn.Open();
                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;

                    try
                    {

                        //aqui, podia tb fazer select ao idBRE...

                        objCmd.CommandText = "SELECT count(idMarcacao) FROM marcacao INNER JOIN BRE on marcacao.idBre = bre.idBRE WHERE marcacao.idMarcacao = " + idMarcacao + " AND MARCaCAO.idBRE = bre.idBRE AND bre.bdefinitivo = 0";

                        int i = Convert.ToInt32(objCmd.ExecuteScalar());

                        //Response.Write(objCmd.CommandText+"<br />"); 
                        if (i == 1)
                        {

                            objCmd.CommandText = "UPDATE bre SET bDefinitivo = 1  FROM marcacao m, bre bre WHERE m.idMarcacao = " + idMarcacao + " AND  m.idBRE = bre.idBRE ";

                            i = objCmd.ExecuteNonQuery();
                            //Response.Write(objCmd.CommandText+"<br />"); 

                            if (i == 1)
                            {
                                objCmd.CommandText = "UPDATE SERVICO SET idEstadoServico = -1, bdefinitivo = 1   FROM marcacao m, bre bre, servico s WHERE m.idMarcacao = " + idMarcacao + " AND  m.idBRE = bre.idBRE and bre.idBRE = s.idBRE ";

                                i = objCmd.ExecuteNonQuery();
                                //Response.Write(objCmd.CommandText+"<br />"); 
                            }

                            objCmd.CommandText = "DELETE FROM marcacao  WHERE idMarcacao = " + idMarcacao;
                            objCmd.ExecuteNonQuery();
                            lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_MARCACAO_APAGADA;

                        }
                        objTrans.Commit();

                        return true;


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
                            lblMessage.Text += excep.ToString() + "<br />";
                            return false;
                        }
                        GERAL.clsWriteError.WriteLog(ex);
                        lblMessage.Text += ex.ToString() + "<br />";
                        return false;
                    }
                }
            }
        }

        //====================================================================================
        //"APAGA" Marcação
        //====================================================================================
        protected void dg_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

            if (deleteMarcacao(id)) BindGridMarcacoes();

        }

        private void updateMarcacao(string idMarcacao, string dtStart, string dtEnd, string idFuncionario, string idRequisicao, string idGrandeza, string obs)
        {

            SqlParameter[] arrParams = new SqlParameter[9];

            arrParams[0] = new SqlParameter("@dtStart", dtStart);
            arrParams[1] = new SqlParameter("@dtEnd", dtEnd);
            arrParams[2] = new SqlParameter("@idFuncionario", idFuncionario);
            arrParams[3] = new SqlParameter("@idMarcacao", idMarcacao);
            arrParams[4] = new SqlParameter("@idRequisicao", idRequisicao);
            arrParams[5] = new SqlParameter("@idGrandeza", idGrandeza);
            arrParams[6] = new SqlParameter("@idOrcamento", null);
            arrParams[7] = new SqlParameter("@idFuncionario2", null);
            arrParams[8] = new SqlParameter("@obsInternas", obs);

            //se a data nao muda, nao faz reset do fax enviado
            //se a data muda, diz que nao ha fax enviado e obrigada a enviar um novo fax.
            GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdateDadosMarcacao", arrParams);

            //martelada: 


        }

        private void updateRequisicaoServicos(string idBRE, string idRequisicao)
        {

            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@idBRE", idBRE);

            arrParams[1] = new SqlParameter("@idRequisicao", idRequisicao);
            if (arrParams[1].Value.ToString() == "")
            {
                arrParams[1].Value = DBNull.Value;
            }

            string strSQL = "Update servico set idRequisicao = @idRequisicao where idBRE = @idBRE";
            GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL, arrParams);
        }

        protected void updateGrid(Object sender, DataGridCommandEventArgs e)
        {

            TextBox txtStart = (TextBox)e.Item.FindControl("txtStartEdit");
            TextBox txtEnd = (TextBox)e.Item.FindControl("txtEndEdit");
            DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicaoEdit");
            DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaEdit");

            string idMarcacao = DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            string dtStart = txtStart.Text.TrimEnd();
            string dtFim = txtEnd.Text;
            if (dtFim == "") dtFim = dtStart;

            string t1 = DateTime.Parse(dtStart).ToString();
            string t2 = DateTime.Now.ToString(dtFim);

            if (DateTime.Compare(DateTime.Parse(t1), DateTime.Parse(t2)) > 0)
            {
                lblMessage.Text = "A data de fim de ser superior à data de início da marcação";
                return;
            }

            DropDownList ddFuncionario = (DropDownList)e.Item.FindControl("ddFuncionarioEdit");
            TextBox txtObs = (TextBox)e.Item.FindControl("txtObsInternasEdit");

            updateMarcacao(idMarcacao, dtStart, dtFim, ddFuncionario.SelectedValue, ddRequisicao.SelectedValue, ddGrandeza.SelectedValue, txtObs.Text);
            //manual -- passar para transacção com mais tempo 
            updateRequisicaoServicos(idMarcacao, ddRequisicao.SelectedValue);

            DataGrid1.EditItemIndex = -1;
            BindGridMarcacoes();
        }

        //=================================================================================================
        //=================================================================================================
        protected string ConverteEstado(bool b)
        {
            if (b == true) return "X";
            else return "";
        }

        private void BindGridEmpresasManutencao(string idMarcacao)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdMarcacao", idMarcacao);
            DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresaManutencaoByIdMarcacao", arrParams);
            dgEmpresasManutencao.DataSource = dt;
            dgEmpresasManutencao.DataBind();
        }



        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            //for (int i = 0; i < e.Item.Cells.Count; i++)
            //{
            //    Response.Write(i + e.Item.Cells[i].Text.ToString() + "<br />"); 
            //}
            if (e.Item.ItemIndex > -1)
            {
                //n devo conseguir aceder ao valor que está dentro de uma template column
                //				DataBoundLiteralControl c  = (DataBoundLiteralControl)e.Item.Cells[12].Controls[0]; 
                //				string idGrandeza = c.Text; 
                //				//isto dá-me sempre algo como 	Text	"\r\n\t\t\t\t\t\t\t\t\t\tAGE\r\n\t\t\t\t\t\t\t\t\t"	string

                //por isso vou pôr uma hidden column 

                string tipoReport = e.Item.Cells[23].Text.ToString();  //= idGrandeza
                string idMarcacao = DataGrid1.DataKeys[e.Item.ItemIndex].ToString();



                switch (e.CommandName.ToString())
                {
                    case "verDadosEmpresaManutencao":
                        BindGridEmpresasManutencao(idMarcacao);
                        break;
                    case "verFax":
                        verFax(report(idMarcacao, tipoReport));
                        break;
                    case "enviarFax":
                        enviarFax(report(idMarcacao, tipoReport), idMarcacao);
                        break;
                    case "enviarMail":
                        enviarMail(report(idMarcacao, tipoReport), idMarcacao);
                        break;
                }
            }
        }


        private ReportClass report(string idMarcacao, string tipoReport)
        {

            ReportClass report = null;

            string strSQL = "  SELECT  dbo.udfGetNomeUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";
            string nomeFuncionario = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdMarcacao", idMarcacao);


            SqlDataReader dr = GERAL.clsDataAccess.SPExecuteDRParams("stpGetDadosMarcacaoByIdMarcacao", arrParams);

            if (!dr.HasRows)
            {
                lblMessage.Text = "Verifique dados empresa/marcação.";
                return null;
            }
            else
            {
                switch (tipoReport)
                {
                    case "CTA":
                        {
                            report = new crFaxMarcacao();
                            break;
                        }
                    case "AUT":
                        {
                            report = new crFaxMarcacaoConc();
                            break;
                        }
                    case "AGE":
                        {
                            //report = new crFaxMarcacaoAGE(); 


                            report = new faxOVM_AGE();
                            break;
                        }
                    case "OPA":
                        {
                            report = new faxOVM_AGE();   //o mesmo ??
                            break;
                        }
                    default:
                        return null;

                }

                while (dr.Read())
                {
                    ViewState["numFaxCliente"] = dr["fax"].ToString();
                    ViewState["emailCliente"] = dr["email"].ToString();
                    ViewState["NomeEmpresa"] = dr["NomeEmpresa"].ToString();


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


                    //    For Each key As ParameterField In CrystalReport.ParameterFields
                    //If key.HasCurrentValue = False Then
                    //    Debug.Print(key.Name.ToString)
                    //End If


                    DATA.BreBD bre = new LabMetro.DATA.BreBD();
                    //DataSet ds = bre.DSBRE_MarcacaoOVM(idMarcacao);
                    DataSet ds = bre.DSBRE_EquipsMarcacaoOVM(dr["idEmpresa"].ToString()); //ja nao sao equipanmentos do bre mas vou usar o mesmo dataset e preecnhher com outros dados
                                                                                          //em vez de ir buscar os equipamentos do BRE, vou buscar todos os equipamentos da grandeza AGE+OPC que tenham calibracoes em atraso e que estejam activos, e ate à data da marcacao ou ate ao fim do mes da marcacao?!



                    try
                    {
                        report.SetDataSource(ds);
                    }
                    catch
                    {

                    }

                    ///pfffffffffffffffff set sempre datasource ANTES dos parametros senão dá erro!!!!!!

                    report.SetParameterValue("@inNomeFuncionario", nomeFuncionario);
                    report.SetParameterValue("@inFaxNumber", dr["fax"].ToString());
                    report.SetParameterValue("@inNomeEmpresa", dr["nomeEmpresa"].ToString());
                    report.SetParameterValue("@inNomeContacto", dr["nomeContacto"].ToString());
                    report.SetParameterValue("@inRefFax", dr["refFax"].ToString());
                    report.SetParameterValue("@inDiaVisita", dataMarcacao);
                    report.SetParameterValue("@inMoradaEmpresa", dr["morada"].ToString());
                    report.SetParameterValue("@inLocalidadeEmpresa", dr["localidade"].ToString());




                    ds = null;
                    bre = null;

                }
            }

            dr.Close();



            return report;

        }

        //a ser repensado mas agora estou com extrema pressa
        private void verFax(ReportClass report)
        {
            if (report == null) return;
            clsReport cr = new clsReport();
            cr.exportReportToPDF(report, "Fax");
            //report = null; 
            //cr = null; 
        }




        private void enviarFax(ReportClass report, string idMarcacao)
        {
            if (report == null) return;

            clsReport cr = new clsReport();

            string strSQL = "  SELECT  dbo.udfGetNomeUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";
            string nomeFuncionario = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            string numFaxCliente = ViewState["numFaxCliente"].ToString();

            string id = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();

            strSQL = "  SELECT  dbo.udfGetEmailUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";

            string mailSender = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            if (mailSender == "")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INDICAR_MAIL_FUNCIONARIO;
                return;

            }

            //cr.faxReport(report, numFaxCliente, mailSender, "FAX", id);
            cr.sendFaxNovo(report, numFaxCliente,  "FAX", id);
            cr = null;
            report = null;

            ViewState["numFaxCliente"] = "";

            updateMarcacaoSetFaxSent(idMarcacao, "fax");


        }

        //Posterior, nao optimizado
        private void enviarMail(ReportClass report, string idMarcacao)
        {
            if (report == null) return;
            clsReport cr = new clsReport();

            string strSQL = "  SELECT  dbo.udfGetNomeUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";
            string nomeFuncionario = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));
            string emailCliente = ViewState["emailCliente"].ToString();
            string NomeCliente = ViewState["NomeEmpresa"].ToString();

            string id = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();

            strSQL = "  SELECT  dbo.udfGetEmailUtilizadorByUserName('" + HttpContext.Current.User.Identity.Name.ToString().Trim() + "')";

            string mailSender = System.Convert.ToString(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            if (mailSender == "")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INDICAR_MAIL_FUNCIONARIO;
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
            cr.mailReport(report, emailCliente, mailSender, "Marcação de Trabalhos", "MARC", "", mailBody, "","");

            cr = null;
            report = null;

            ViewState["emailCliente"] = "";
            ViewState["nomeCliente"] = "";
            ViewState["Nomeempresa"] = "";

            updateMarcacaoSetFaxSent(idMarcacao, "mail");

        }



        //string strSQL = "UPDATE marcacao set bFaxEnviado = 1 , dtUltimoFax = getdate() where idMarcacao = " +idMarcacao; 
        //int i = GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
        //BindGridMarcacoes(); 
        private void updateMarcacaoSetFaxSent(string idMarcacao, string tipo)
        {
            string strSQL = "UPDATE marcacao set bFaxEnviado = 1 , tipo = '" + tipo + "', dtUltimoFax = getdate() where idMarcacao = " + idMarcacao;
            int i = GERAL.clsDataAccess.myExecuteNonQuery(strSQL);
            BindGridMarcacoes();
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
    }
}
