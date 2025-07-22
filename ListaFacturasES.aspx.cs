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
using System.Linq;


namespace LabMetro
{
    /// <summary>
    /// Summary description for ListaFacturas.
    /// </summary>
    /// 
    

    public partial class ListaFacturasES : System.Web.UI.Page
    {
        //NOME DA PAGINA


        private const string ID_PAG = "FACTURAS_0";
        private static string caminhoPastaFacturas = (string)ConfigurationManager.AppSettings["FACTURACAO_ES_PASTA"];

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            Page.Form.DefaultButton = btnPesquisa.UniqueID;


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
                        ViewState["sortField"] = "idFactura";
                        ViewState["sortDirection"] = "DESC";
                        //bindDDAno(); 
                        //ddAno.SelectedValue = DateTime.Today.Year.ToString(); 
                    }

                    if (!ht.ContainsKey("FACTURAS_1")) //se n tem permissoes para ver os detalhes dos equipamentos, desactivar o link
                    {
                        //Response.Write(DGFacturas.Columns[7].HeaderText.ToString()); 
                        DGFacturas.Columns[0].Visible = false;
                    }
                }
            }
        }

        //private void bindDDAno()
        //{
        //    for (int ano = DateTime.Today.Year; ano > 2003 ; ano--)
        //    {
        //        ddAno.Items.Add(new ListItem(ano.ToString(),ano.ToString()));  		 
        //    }
        //    ddAno.Items.Insert(0, new ListItem("",""));
        //}

        private void BindGrid()
        {
            DATA.FacturaData Factura = new LabMetro.DATA.FacturaData();
            DataTable DT = Factura.DTFacturas(txtNomeEmpresa.Text, txtNumBRE.Text, txtNumFactura.Text, txtTipoEquipamento.Text, null, txtNomeFuncionarioAlteracao.Text, txtRefServico.Text);

            DataView DV = new DataView(DT);


            DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

            DGFacturas.DataSource = DV;
            DGFacturas.DataBind();

            if (DT.Rows.Count > 0)
            {
                DGFacturas.Visible = true;
            }
            else
            {
                DGFacturas.Dispose();
                DGFacturas.Visible = false;
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
            }

            Factura = null;

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

        public void DoPaging(Object s, DataGridPageChangedEventArgs e)
        {
            DGFacturas.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

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
            BindGrid();
        }

        protected void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DGFacturas.CurrentPageIndex = 0;
            BindGrid();

        }

        protected string ConverteVersaoFicheiro(int i)
        {

            if (i == 0) return "não";

            return "sim";
        }

        protected void btnFicheiroAxapta_Click(object sender, EventArgs e)
        {
            string strIds = "";

            foreach (DataGridItem dgi in DGFacturas.Items)
            {
                CheckBox myCheckBox =
                    (CheckBox)dgi.Cells[0].FindControl("checkbox");
                if (myCheckBox.Checked == true)
                {
                    strIds += DGFacturas.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter);

            writeAxaptaFile(strIds);

        }


        //criar ficheiro AXAPTA
        private bool writeAxaptaFile(string ids)
        {
            string pathFicheiro = (string)ConfigurationManager.AppSettings["FACTURACAO_PATH_REL"];

            string nomeFicheiro = "FACTURA_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss-tt") + ".txt";
            string ficheiro = Server.MapPath("~/" + pathFicheiro + "/" + nomeFicheiro);

            //**************			

            string path = System.Web.HttpContext.Current.Server.MapPath("~/" + pathFicheiro);

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            string strSearch = "FACTURA.txt"; //idFactura + "_*";
            FileInfo[] files = dirInfo.GetFiles(strSearch);
            foreach (FileInfo f in files)
            {
                f.Delete(); //apagar todos os ficheiros que já existem com o mesmo idFactura //APAGA SO O FICHEIRO ANTERIOR
            }

            StreamWriter sw = new StreamWriter(ficheiro, false, System.Text.Encoding.GetEncoding(1252));

            try
            {
                DATA.FacturaData factura = new LabMetro.DATA.FacturaData();
                DataTable dt = factura.dtServicosParaFicheiroAXAPTA(ids);

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

                updateFacturas(ids);

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

        //=================================================================================================
        // DEVOLVE O CAMINHO PARA UM NOME DE FICHEIRO
        //=================================================================================================
        public string downloadpathFicheiroAXAPTA(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = (string)ConfigurationManager.AppSettings["FACTURACAO_PATH_REL"];

                myPath = myPath + "/" + filename.ToString();
                return myPath;
            }
            else
            {
                return "#";
            }
        }


        private void updateFacturas(string idsf)
        {

            string strSQL = "update factura set dataficheiro = getdate() where idFactura IN  (" + idsf + ")";
            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
        }

        protected void btnAbrirFicheiro_Click(object sender, EventArgs e)
        {

           
            FileInfo newestFile = GetNewestFile(new DirectoryInfo(caminhoPastaFacturas));

            Response.Write(newestFile.Name.ToString());

           
            if (newestFile.Exists)
            {

                Response.Clear();
                Response.Buffer = true;
                //' x-mxdownload forces automatic download no matter what the content type

                Response.ContentType = "Application/x-msdownload";
                Response.ContentEncoding = System.Text.Encoding.Default;
                //força o dialog "save as"; 
                Response.AddHeader("content-disposition", "attachment; filename=" + newestFile.FullName);

                Response.WriteFile(newestFile.FullName);
                HttpContext.Current.Response.End();
            }

           

        }

       public static FileInfo GetNewestFile(DirectoryInfo directory) {
   return directory.GetFiles()
       .Union(directory.GetDirectories().Select(d => GetNewestFile(d)))
       .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
       .FirstOrDefault();                        
}

    }
}
