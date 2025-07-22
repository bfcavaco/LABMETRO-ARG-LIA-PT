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

namespace LabMetro
{
	/// <summary>
	/// Summary description for DadosSap.
	/// </summary>
	public partial class DadosSap : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				ViewState["sortDirection"]="ASC";
				ViewState["sortField"]="nome1";
				BindGrid(); 

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

		private void BindGrid()

		{

			try
			{

				string strSQL = "SELECT * from sap_Empresas"; 
				DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
				DataView dv = new DataView(dt); 
				string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
				dv.Sort = strSort; 
				
				Response.Write(dt.Rows.Count.ToString()); 
				DG.DataSource = dv;
				DG.DataBind(); 

//				string strSQL = "SELECT  * FROM DMCLIENTE_000001_199999_METR.TXT order by 1,2"; 
//				DataTable dt = GERAL.clsDataAccess.csvSAPExecuteDT(strSQL); 
//				DataView dv = new DataView(dt); 
//				string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
//				dv.Sort = strSort; 
//				
//				Response.Write(dt.Rows.Count.ToString()); 
//				DG.DataSource = dv;
//				DG.DataBind(); 
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString());
			}

		}

		public void sortGrid(Object s, DataGridSortCommandEventArgs e)
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

	}
}
