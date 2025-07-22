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

namespace LabMetro
{
   
    public partial class ListaTaxasServico : System.Web.UI.Page
    {

        private const string ID_PAG = "TAXASERVICO_0";//NOME DA PAGINA


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

                    if (!Page.IsPostBack)
                    {
                        ViewState["sortField"] = "idTaxaServico";
                        ViewState["sortDirection"] = "DESC";
                        FillDDEstadoTaxaServico();
                        fillResponsaveisTecnicos();
                        //BindGrid(); 

                    }
                    if (!ht.ContainsKey("TAXASERVICO_1")) //se n tem permissoes para ver os detalhes dos funcionarios, desactivar o link
                    {
                        DG.Columns[5].Visible = false;
                    }
                }
            }
        }

        private void FillDDEstadoTaxaServico()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();

            ddEstadoTaxaServico.DataSource = lista.DVListaEstadosOrcamentos();
            ddEstadoTaxaServico.DataBind();
            ddEstadoTaxaServico.Items.Insert(0, new ListItem("", ""));


            lista = null;
        }

        private void fillResponsaveisTecnicos()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            DataTable DT = lista.DTListaFuncionarios("1,4");  //MUITO MAU, HARDCODED!!! MAU PQ FOI PEDIDO ASSIM, DEVIAMOS IR POR PERFIL, mas vamos por funcao.


            // Obter funcionários cuja função é "Responsável Técnico" (4)
            ddFuncionario.DataSource = DT;
            ddFuncionario.DataBind();
            //por default seleccionar o luis godinho, no caso do insert:
            //é feito no pageload
            ddFuncionario.Items.Insert(0, new ListItem("", ""));
        }
    

        private void BindGrid()
        {
            DATA.TaxaServicoBD TaxaServico = new LabMetro.DATA.TaxaServicoBD();

            DataTable dt = TaxaServico.DTTaxaServicos(txtEmpresa.Text, txtTipoEquipamento.Text, ddEstadoTaxaServico.SelectedValue, txtRefTaxaServico.Text,"", ddFuncionario.SelectedValue); 

            DataView DV = new DataView(dt);

            DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

            DG.DataSource = DV;
            DG.DataBind();

            TaxaServico = null;

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
            DG.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
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

        protected void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DG.CurrentPageIndex = 0;
            BindGrid();
        }
    }
}
