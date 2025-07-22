using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaEmpresasSAP.
	/// </summary>
	public partial class ListaEmpresasSAP : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtEmpresa;
		protected System.Web.UI.WebControls.PlaceHolder menuPlaceHolder;

		protected System.Web.UI.WebControls.DropDownList ddEstado;

		DataView DV;
		

		private const string ID_PAG = "EMPRESAS_SAP_0";//NOME DA PAGINA -- igual à pagina empresa
    
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
						ViewState["sortField"] = "nome";
						ViewState["sortDirection"] = "ASC";
						fillDDCodigoBloqueio(); 
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
			txtNumClienteSAP.TextChanged += new System.EventHandler(txtNumClienteSAP_TextChanged);
			txtNumClienteAntigo.TextChanged += new System.EventHandler(txtNumClienteAntigo_TextChanged);
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
		}

		#endregion

		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			DGEmpresas.CurrentPageIndex=0; 
			BindGrid(); 
		}
		
		private void fillDDCodigoBloqueio()
		{
			DATA.FacturaData f = new LabMetro.DATA.FacturaData(); 
			SqlDataReader dr; 
			
			dr = f.drCodigoBloqueioSap(); 
			ddCodigoBloqueioSap.DataTextField= "descricao"; 
			ddCodigoBloqueioSap.DataValueField= "id"; 
			ddCodigoBloqueioSap.DataSource = dr; 
			ddCodigoBloqueioSap.DataBind(); 
			dr.Close(); 
			dr = null; 
			ddCodigoBloqueioSap.Items.Insert(0,new ListItem("","")); //isto tem de ser tirado depois, not null

			f = null; 
		}

		private void BindGrid()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresasSAP(txtNomeEmpresa.Text,txtNumClienteSAP.Text,txtNumClienteAntigo.Text,txtNIF.Text,ddCodigoBloqueioSap.SelectedValue.ToString(),ddGrupoContas.SelectedValue.ToString()); 
            
			DV = new DataView(DT);

			string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
			DV.Sort = strSort; 
			DGEmpresas.DataSource =DV; 
			DGEmpresas.DataBind(); 
            
			if(DV.Table.Rows.Count > 0)
			{
				DGEmpresas.Visible=true;
			}
			else
			{
				DGEmpresas.Dispose();
				DGEmpresas.Visible=false; 
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
			}

			empresa = null; 
		}
       
//		protected System.Drawing.Color ConvertColor(string s)
//		{
//			System.Drawing.ColorConverter colConvert = new ColorConverter();
//
//			System.Drawing.Color colorName; 
//			switch(s)
//			{
//				case "00":
//					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Gainsboro");
//					break;
//				default: //todos os que estão de alguma forma bloqueados
//					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Medium Orange-Yellow");
//					break;
//			}
//			return colorName; 
//			
//		}


		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DGEmpresas.CurrentPageIndex = e.NewPageIndex; 
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

		

		private void txtNomeEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			resetEmpresas(); 
		}

		private void txtNIF_TextChanged(object sender, System.EventArgs e)
		{
			resetEmpresas(); 
		}

		private void resetEmpresas()
		{
			DGEmpresas.CurrentPageIndex=0; 
			BindGrid(); 
		
		}

		private void txtNumClienteSAP_TextChanged(object sender, System.EventArgs e)
		{
			resetEmpresas(); 
		}

		private void txtNumClienteAntigo_TextChanged(object sender, System.EventArgs e)
		{
			resetEmpresas(); 
		}
	}
}
