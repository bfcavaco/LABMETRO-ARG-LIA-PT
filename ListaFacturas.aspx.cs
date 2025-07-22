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
	/// Summary description for ListaFacturas.
	/// </summary>
	public partial class ListaFacturas : System.Web.UI.Page
	{
//NOME DA PAGINA
		
		
		private const string ID_PAG = "FACTURAS_0";
        
		
		protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            Page.Form.DefaultButton = btnPesquisa.UniqueID;

            
            lblMessage.Text = "";
            
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
                        ViewState["sortField"] = "idFactura";
                        ViewState["sortDirection"] = "DESC";
						//bindDDAno(); 
						//ddAno.SelectedValue = DateTime.Today.Year.ToString(); 
                    }
                    
                    if(!ht.ContainsKey("FACTURAS_1")) //se n tem permissoes para ver os detalhes dos equipamentos, desactivar o link
                    {
                        //Response.Write(DGFacturas.Columns[7].HeaderText.ToString()); 
                        DGFacturas.Columns[0].Visible=false; 
                    }
                }
            }
        }

        //private void bindDDAno()
        //{
        //    for (int ano = DateTime.Today.Year; ano > 2003 ; ano--)
        //    {
        //        ddAno.Items.Add(new ListItem(ano.ToString(),ano.ToString()));  		 
        //    }
        //    ddAno.Items.Insert(0, new ListItem("",""));
        //}

		private void BindGrid()
		{
			DATA.FacturaData Factura = new LabMetro.DATA.FacturaData(); 
			DataTable DT = Factura.DTFacturas(txtNomeEmpresa.Text, txtNumBRE.Text, txtNumFactura.Text, txtTipoEquipamento.Text,null, txtNomeFuncionarioAlteracao.Text,txtRefServico.Text); 
            
            DataView DV = new DataView(DT);
			

            DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

			DGFacturas.DataSource = DV; 
			DGFacturas.DataBind(); 
            
			if(DT.Rows.Count > 0)
			{
				DGFacturas.Visible=true;
			}
			else
			{
				DGFacturas.Dispose();
				DGFacturas.Visible=false; 
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
			}

			Factura = null; 

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
			DGFacturas.CurrentPageIndex = e.NewPageIndex;
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
			DGFacturas.CurrentPageIndex=0; 
			BindGrid(); 

		}

		protected string ConverteVersaoFicheiro(int i)
		{
			
			if(i==0) return "não"; 
			
			return "sim"; 
		}
	}
}
