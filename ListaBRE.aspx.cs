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
	public partial class ListaBRE : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.CompareValidator compv1;
        DataView DV;
        private const string ID_PAG = "BRE_0";//NOME DA PAGINA

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
                        
						//as empresas sao preenchidas após pesquisa
						//fillDDEmpresa(); 
						ddEmpresa.Items.Insert(0,new ListItem("Todas","")); 
						
                        if(Request.QueryString["id"]!= null) ddEmpresa.SelectedValue=Request.QueryString["id"].ToString(); 
                       
                        
                       
                    }
                    if(!ht.ContainsKey("BRE_1")) //se n tem permissoes para ver os detalhes do BRE, desactivar o link
                    {
                        DGBRE.Columns[4].Visible=false; 
                    }
                }
            }
        }

		private void fillDDEmpresa()
		{
			DATA.BreBD empresa = new LabMetro.DATA.BreBD(); 
			DataTable DT = empresa.DTListaEmpresasForListaBRE(txtPesquisaEmpresa.Text, txtPesquisaNif.Text);  
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaNif.Text == "") &&(txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}

        private void BindGrid()
        {
            DATA.BreBD bre = new LabMetro.DATA.BreBD(); 
            DataTable DT = bre.DTBRE(ddEmpresa.SelectedValue.ToString(),txtNumBre.Text,"1",txtRefReq.Text, cbCompletos.Checked.ToString());  //este 1 no final indica que so vai buscar bre's definitivos e nao bres de calibs externas cujos equipamentos podem sofrer alteracoes.
            
            DV = new DataView(DT);
            

            if(DV.Table.Rows.Count > 0)
            {
                DGBRE.Visible=true;
                DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
                DGBRE.DataSource = DV;
                DGBRE.DataBind(); 
            }
            else
            {
                DGBRE.DataSource = null;
                DGBRE.Dispose();
                DGBRE.Visible=false; 
                lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
            }

			bre = null; 
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
			DGBRE.DataSource = null;
			DGBRE.DataBind(); 
		}

		protected void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa();
			DGBRE.DataSource = null;
			DGBRE.DataBind(); 
		}

		protected void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGBRE.DataSource = null;
			DGBRE.DataBind(); 
		}

		protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGBRE.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected System.Drawing.Color ConvertColor(int i, string codigoBloqueioSAP)
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

			if(codigoBloqueioSAP =="03") //se a empresa está inactiva em SAP (n tem a ver com o nivelBloqueiolabmetro, martelada...)
			{
				colorName = (System.Drawing.Color)colConvert.ConvertFromString("PowderBlue");
			}
			return colorName; 
		}
        protected string strX(bool b)
        {
            if (b == true) return "x";
            return "";
        }
    }
}
