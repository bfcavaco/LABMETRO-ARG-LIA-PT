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
    /// <summary>
    /// Summary description for ListaBRE.
    /// </summary>
    public partial class ListaBRECaliExt : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.CompareValidator compv1;
        DataView DV;
        private const string ID_PAG = "BRE_CE_0";//NOME DA PAGINA

        protected void Page_Load(object sender, System.EventArgs e)
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
                        ViewState["sortField"] = "idBRE";
                        ViewState["sortDirection"] = "DESC";

						fillDDEmpresa();    
                        BindGrid();//faz logo um bindgrid para todos
                       
                    }

                    if(!ht.ContainsKey("BRE_CE_1")) //se n tem permissoes para ver os detalhes do BRE, desactivar o link
                    {
                        DGBRE.Columns[4].Visible=false; 
                    }
                }
            }
        }

		//pode ser preenchido com as mesmas empresas do bre normal
		private void fillDDEmpresa()
		{
			DATA.BRECalibExtBD empresa = new LabMetro.DATA.BRECalibExtBD(); 
			DataTable DT = empresa.DTListaEmpresasForListaBRECalibExt(txtPesquisaEmpresa.Text,txtPesquisaNif.Text); 
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 
			
			if((txtPesquisaNif.Text == "") &&(txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}



        private void BindGrid()
        {
            DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
            DataTable DT = bre.DTBRE(ddEmpresa.SelectedValue.ToString(),txtNumBre.Text,"0",null,null);  //este 0 no final indica que so vai buscar bre's definitivos e nao bres de calibs externas cujos equipamentos podem sofrer alteracoes.
            
            DV = new DataView(DT);
           
            DV.Sort =  ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

            DGBRE.DataSource =DV; 
            DGBRE.DataBind(); 

			if(DV.Table.Rows.Count > 0)
            {
                DGBRE.Visible=true;
            }
            else
            {
                DGBRE.Dispose();
                DGBRE.Visible=false; 
                lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
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

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGBRE.CurrentPageIndex = e.NewPageIndex;
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

        protected void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DGBRE.CurrentPageIndex=0; 
            BindGrid(); 
        }

		protected void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBRE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBRE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBRE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGBRE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected System.Drawing.Color ConvertColor(int i)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();
			 


			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Gold");
					break;
				case 2:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("DarkOrange");
					break;
				case 3: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Crimson");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}
			return colorName; 
		}
    }
}
