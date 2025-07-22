using System;
using System.Collections.Generic;
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
    public partial class ListaBSE: System.Web.UI.Page
    {
        DataView DV; 

        private const string ID_PAG = "BSE_0";//NOME DA PAGINA
    
        private void Page_Load(object sender, System.EventArgs e)
        {
            Page.Form.DefaultButton = btnPesquisa.UniqueID;

            
           lblMessage.Text =""; 
            
            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					Server.Transfer("Default.aspx?err=1",false);
				}
                else
                {
                    if(!Page.IsPostBack)
                    {
						ViewState["sortField"] = "idBSE";
						ViewState["sortDirection"] = "DESC";
		                if(Request.QueryString["id"]!= null) ddEmpresa.SelectedValue=Request.QueryString["id"].ToString(); 
                       
                        //lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SEARCH; 
                 
                    }
                    if(!ht.ContainsKey("BSE_1")) //se n tem permissoes para ver os detalhes do BSE, desactivar o link
                    {
                        DGBSE.Columns[3].Visible=false; 
                    }
                }
            }
        }

		private void fillDDEmpresa()
		{
			DATA.BseBD empresa = new LabMetro.DATA.BseBD(); 
			DataTable DT = empresa.DTEmpresasForListaBSE(txtPesquisaEmpresa.Text, txtPesquisaNif.Text); 	
	        
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaNif.Text == "") &&(txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));

		}

        private void BindGrid()
        {
            DATA.BseBD BSE = new LabMetro.DATA.BseBD(); 
            DataTable DT = BSE.DTBse(ddEmpresa.SelectedValue.ToString(),txtNumBse.Text); 
            
            DV = new DataView(DT);
           
            DV.Sort =ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

            DGBSE.DataSource =DV; 
            DGBSE.DataBind(); 
            
            if(DV.Table.Rows.Count > 0)
            {
                DGBSE.Visible=true;
            }
            else
            {
                DGBSE.Dispose();
                DGBSE.Visible=false; 
                //lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
            }

			BSE = null; 
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
			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnPesquisaEmpresa.Click += new System.EventHandler(btnPesquisaEmpresa_Click);
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
			Load += new System.EventHandler(Page_Load);

		}
        #endregion

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGBSE.CurrentPageIndex = e.NewPageIndex;
            BindGrid(); 
        }

        public void SortGrid(Object s, DataGridSortCommandEventArgs e)
        {
            switch (ViewState["sortDirection"].ToString())
            {
                case "ASC":
                    ViewState["sortDirection"]="DESC"; 
                    break;
                case "DESC":
                    ViewState["sortDirection"]="ASC";
                    break;
            }

            ViewState["sortField"] = e.SortExpression;
            BindGrid(); 

        }

        private void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DGBSE.CurrentPageIndex=0; 
            BindGrid(); 
        }

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBSE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBSE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBSE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGBSE.CurrentPageIndex=0; 
			BindGrid();
		}		
    
    }
}
