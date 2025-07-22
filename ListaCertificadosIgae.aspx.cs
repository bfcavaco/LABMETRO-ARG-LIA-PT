using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;


namespace LabMetro
{
    /// <summary>
    /// Summary description for ListaCertificados.
    /// </summary>
    public partial class ListaCertificadosIgae : System.Web.UI.Page
    {
        private const string ID_PAG = "CERTIFICADOSIGAE_0";//NOME DA PAGINA

        DataTable DT;
        DataView DV;

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

            btnSearch.Click += new System.EventHandler(btnSearch_Click);
            
        }
        #endregion


        protected void Page_Load(object sender, System.EventArgs e)
        {
            Page.Form.DefaultButton = btnSearch.UniqueID;


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
                        ViewState["sortField"] = "dtCertificado";
                        ViewState["sortDirection"] = "DESC";
                        fillDDEmpresa();
                        fillddAno();
                    }
                }
            }
        }

        private void fillDDEmpresa()
        {
            DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            SqlDataReader dr = empresa.DREmpresasForIgae();
            ddEmpresa.DataSource = dr;
            ddEmpresa.DataBind();
            dr.Close();
            empresa = null;
            ddEmpresa.Items.Insert(0,new ListItem("---",""));
        }


        private void fillddAno()
        {   
            for (int year = 2010; year <= DateTime.Today.Year; year++)
            {
                ddAno.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }


        //==================================================================================
        // Função que permite visualizar o documento pretendido pelo utilizador
        //==================================================================================
        public void visualisarDocumento(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Select")
            {
                string doc = e.Item.Cells[6].Text;
                string nome = downloadpath(doc);
                Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");
            }
        }

        private void BindGrid()
        {
            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();

            DT = data.DTListCertificadoIgae(ddEmpresa.SelectedValue, ddAno.SelectedValue, txtSearchEquipamento.Text);

            data = null;

            DV = new DataView(DT);

            if (Page.IsPostBack)
            {
                DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
            }

            dgCertificados.DataSource = DV;
            dgCertificados.DataBind();

            foreach (DataGridItem dgi in dgCertificados.Items)
            {
                string docName = dgi.Cells[6].Text;
                dgi.ToolTip = "Click para visualisar o documento " + docName;
            }

            if (DV.Table.Rows.Count > 0)
            {
                dgCertificados.Visible = true;
            }
            else
            {
                dgCertificados.Dispose();
                dgCertificados.Visible = false;
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
            }

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

        public void DoPaging(Object s, DataGridPageChangedEventArgs e)
        {
            dgCertificados.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            dgCertificados.CurrentPageIndex = 0;
            BindGrid();
        }

        public string downloadpath(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = (string)ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CERTIFICADOS"];
                myPath = myPath + "/" + filename.ToString();
                return myPath;
            }
            else
            {
                return "#";
            }
        }

        public void dgCertificados_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];

                int numCells = e.Item.Cells.Count;
                for (int i = 1; i < numCells; i++)
                {

                    //Response.Write("cell" + i + "-"+e.Item.Cells[i].Text+"<br />"); 
                    if (!e.Item.Cells[i].HasControls()) //para nao pôr o link nas cells que conteem checkboxes
                    {
                        e.Item.Cells[i].ToolTip = "Click para visualisar o documento " + e.Item.Cells[6].Text;
                        e.Item.Cells[i].Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(button, ""));
                    }
                }
            }
        }
    }
}
