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
using LabMetro.REPORTS;
using LabMetro.GERAL;

namespace LabMetro
{
    public partial class ListaEmpresasIgae : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.TextBox txtEmpresa;
        protected System.Web.UI.WebControls.PlaceHolder menuPlaceHolder;
        protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;

        DataView DV;

        private const string ID_PAG = "EMPRESASIGAE_0";//NOME DA PAGINA

        protected void Page_Load(object sender, System.EventArgs e)
        {
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
                    //no caso de ser uma pagina do tipo 0, nao ha nada a validar,
                    //a nao ser links para paginas seguintes, 
                    //ex: listaempresas tem link para formempresas e formcontactos

                    if (!Page.IsPostBack)
                    {
                        ViewState["sortField"] = "nome";
                        ViewState["sortDirection"] = "ASC";
                        //fillDropDowns();

                       
                    }

                    
                    if (!ht.ContainsKey("CONTACTOSIGAE_0")) //se n tem permissoes para ver a lista dos Contactos, desactivar o link
                    {
                        DGEmpresas.Columns[6].Visible = false;
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
            txtNomeEmpresa.TextChanged += new System.EventHandler(txtNomeEmpresa_TextChanged);
            txtNIF.TextChanged += new System.EventHandler(txtNIF_TextChanged);
            btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
        }


        #endregion

        private void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DGEmpresas.CurrentPageIndex = 0;
            BindGrid();
        }

        private void fillDropDowns()
        {
        }

        private DataSet DS()
        {
            DataSet ds = new DATASETS.DSEmpresa();

            
            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inNome", txtNomeEmpresa.Text);
            arrParams[1] = new SqlParameter("@inNif", txtNIF.Text);
            arrParams[2] = new SqlParameter("@idConcelho", ddConcelho.SelectedValue);
          
            ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

            try
            {
                ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListEmpresasIgae", ds, "dtEmpresa", arrParams);

                return ds;
            }
            catch
            {
                return null;
            }

        }


        private void BindGrid()
        {

            DataSet ds = DS();

            if (ds != null)
            {

                DV = new DataView(ds.Tables["dtEmpresa"]);
                DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
                DGEmpresas.DataSource = DV;
                DGEmpresas.DataBind();

                if (DV.Table.Rows.Count > 0)
                {
                    DGEmpresas.Visible = true;
                }
                else
                {
                    DGEmpresas.Dispose();
                    DGEmpresas.Visible = false;
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
                }

            }

            ds = null;

            //empresa = null; 

        }

        public void DoPaging(Object s, DataGridPageChangedEventArgs e)
        {
            DGEmpresas.CurrentPageIndex = e.NewPageIndex;
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


        private void txtNomeEmpresa_TextChanged(object sender, System.EventArgs e)
        {
            DGEmpresas.CurrentPageIndex = 0;
            BindGrid();
        }

        private void txtNIF_TextChanged(object sender, System.EventArgs e)
        {
            DGEmpresas.CurrentPageIndex = 0;
            BindGrid();
        }

        protected void ddConcelhoDataBound(object sender, EventArgs e)
        {
            
          
            ddConcelho.Items.Insert(0, new ListItem("...", ""));
        }

        protected void ddDistritoDataBound(object sender, EventArgs e)
        {

           
        }

    }
}
