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
	/// Summary description for ListaServicosRequisicao.
	/// </summary>
	public partial class ListaServicosRequisicao : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			//acessível únicamente a partir da factura
			if(!Page.IsPostBack)
			{
				string idRequisicao = Request.QueryString["id"]; 
				BindGridServicos(idRequisicao); 
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

		private void BindGridServicos(string idRequisicao)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inIdRequisicao",idRequisicao);
				
			DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosByIdRequisicao",arrParams); 
			if(dt.Rows.Count > 0)
			{
				dgServicos.DataSource=dt; 
				dgServicos.DataBind(); 
			}
			else
			{
				dgServicos.DataSource=null; 
				dgServicos.DataBind(); 
				lblMessage.Text ="Não existem serviços associados na Base de Dados.<br />Para associar serviços, utilize o item de menu 'Serviços Sem Requisição' dentro da secção 'Facturação'."; 

			}

		}
	}
}
