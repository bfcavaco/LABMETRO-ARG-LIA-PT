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
using System.Text.RegularExpressions;
using System.Configuration;

namespace LabMetro
{
    /// <summary>
    /// Summary description for ListaContactos.
    /// </summary>
    public partial class ListaContactosIgae : System.Web.UI.Page
    {
        private const string ID_PAG = "CONTACTOSIGAE_0";//NOME DA PAGINA

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
                        ViewState["sortField"] = "nome";
                        ViewState["sortDirection"] = "ASC";
                        fillDDEmpresa();
                        
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

          
        }

        private void BindGrid()
        {
            DATA.ContactosBD contactos = new LabMetro.DATA.ContactosBD();
            try
            {

               
                DataTable DT = contactos.DTFillContacts(ddEmpresa.SelectedValue, "","","");
                DataView DV = new DataView(DT);
                DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

                DGContactos.DataSource = DV;
                DGContactos.DataBind();


                if (DV.Table.Rows.Count > 0)
                {
                    DGContactos.Visible = true;
                }
                else
                {
                    DGContactos.Dispose();
                    DGContactos.Visible = false;
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;

                }
            }
            catch (Exception ex)
            {

                DGContactos.Dispose();
                DGContactos.Visible = false;
                lblMessage.Text = ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
            }
            
            contactos = null;
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
             ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
           
           
        }
        #endregion

        
        public void DoPaging(Object s, DataGridPageChangedEventArgs e)
        {
            DGContactos.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DGContactos.CurrentPageIndex = 0;
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

        private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
        {
            fillDDEmpresa();   
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            DGContactos.CurrentPageIndex = 0;
            BindGrid();
        }

        protected System.Drawing.Color ConvertColor(int i)
        {
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color colorName;
            switch (i)
            {
                case 0:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("Red");
                    break;
                case 1:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
                default:
                    colorName = (System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
            }
            return colorName;
        }

    }
}
