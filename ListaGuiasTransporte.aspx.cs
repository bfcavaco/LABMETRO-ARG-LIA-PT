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
	/// Summary description for ListaGuiasTransporte.
	/// </summary>
	public partial class ListaGuiasTransporte : System.Web.UI.Page
	{
	

		private const string ID_PAG = "GUIA_TRANSPORTE_0";//NOME DA PAGINA
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
						ViewState["sortField"] = "idGuiaTransporte"; 
						ViewState["sortDirection"] = "DESC";
                 
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
			
			DataTable DT = DTGuiasTransporte(); 
            
			DataView DV = new DataView(DT);
           
			DV.Sort =ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

			DGGT.DataSource =DV; 
			DGGT.DataBind(); 
            
			if(DV.Table.Rows.Count > 0)
			{
				DGGT.Visible=true;
			}
			else
			{
				DGGT.Dispose();
				DGGT.Visible=false; 
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
			DGGT.CurrentPageIndex = e.NewPageIndex;
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
			DGGT.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGGT.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGGT.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGGT.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGGT.CurrentPageIndex=0; 
			BindGrid();
		}		



		
		public DataTable DTGuiasTransporte()
		{
			SqlParameter[] arrParams = new SqlParameter[5];
			arrParams[0] = new SqlParameter("@idEmpresa", ddEmpresa.SelectedValue);
			arrParams[1] = new SqlParameter("@inRefGuiaTrasnporte", txtRefGuia.Text);	

			arrParams[2] = new SqlParameter("@inLocalCarregamento", txtLocalCarregamento.Text);
			arrParams[3] = new SqlParameter("@inLocalDescarregamento", txtLocalDesCarregamento.Text);	

			
			arrParams[4] = new SqlParameter("@inDestinatario", txtDestinatario.Text);	

				
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stgGetListGuiasTransporte", arrParams); 
		 
		}

	}
}

