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
	/// Summary description for ListaBRESub.
	/// </summary>
	public partial class ListaBRESub : System.Web.UI.Page
	{
        DataView DV;
        private const string ID_PAG = "BRESUB_0";//NOME DA PAGINA
	
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
				        ViewState["sortField"] = "dtSubcontratoBRE";
				        ViewState["sortDirection"] = "DESC";
                        BindGrid();
				        txtNumBRE.Text = "";
			        }
                    if(!ht.ContainsKey("BRESUB_1")) //se n tem permissoes para ver os detalhes do BRESUB, desactivar o link
                    {
                        DGBRESub.Columns[6].Visible=false; 
                    }
                }
            }
        }

		private void BindGrid()
		{
			DATA.SubcontratoBreBD subBRE = new LabMetro.DATA.SubcontratoBreBD(); 
			DataTable DT = subBRE.FillListaSubBRE(txtNomeEmpresa.Text, txtNumBRE.Text, txtNumBRESub.Text); 
        
            DV = new DataView(DT);
         
            DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

			DGBRESub.DataSource = DV; 
			DGBRESub.DataBind(); 
        
			if(DV.Table.Rows.Count > 0)
			{
				DGBRESub.Visible=true;
			}
			else
			{
				DGBRESub.Dispose();
				DGBRESub.Visible=false; 
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
			}

			subBRE = null; 
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
			DGBRESub.CurrentPageIndex = e.NewPageIndex;
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
			DGBRESub.CurrentPageIndex=0; 
			BindGrid(); 
		}
	}
}

