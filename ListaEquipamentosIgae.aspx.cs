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
    public partial class ListaEquipamentosIgae : System.Web.UI.Page
    {
        private const string ID_PAG = "EQUIPAMENTOSIGAE_0";//NOME DA PAGINA

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
                        ViewState["sortField"] = "Equipamento";
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

        private void BindGrid() //AQUI SERÁ O BIND GRID EQUIPAMENTOS
        {
            DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD();
            if (ddEmpresa.SelectedIndex > -1)
            {
                DataTable DT = equipamento.DTEquipamentoForIgae(ddEmpresa.SelectedValue.ToString());

                DataView DV = new DataView(DT);

                DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];


                DGEquipamentos.DataSource = DV;

                if (DGEquipamentos.CurrentPageIndex >= DGEquipamentos.PageCount)
                {
                    DGEquipamentos.CurrentPageIndex = 0;
                }

                DGEquipamentos.DataBind();

                if (DV.Table.Rows.Count > 0)
                {
                    DGEquipamentos.Visible = true;
                }
                else
                {
                    DGEquipamentos.Dispose();
                    DGEquipamentos.Visible = false;
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
                }
            }
            else
            {
                DGEquipamentos.Dispose();
                DGEquipamentos.Visible = false;
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;

            }

            equipamento = null; 

        }

        protected string ConverteEstado(bool b)
        {
            if (b == true)
            {
                return "activo";
            }
            else
            {
                return "inactivo";
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
            ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            
        }
        #endregion

       
        public void DoPaging(Object s, DataGridPageChangedEventArgs e)
        {
            DGEquipamentos.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DGEquipamentos.CurrentPageIndex = 0;
            BindGrid();
            //fillEtiqueta(); 
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

       
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {

            DGEquipamentos.CurrentPageIndex = 0;
            BindGrid();
            //fillEtiqueta(); 

        }
    }
}
