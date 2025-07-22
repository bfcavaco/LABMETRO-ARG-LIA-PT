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
	/// Summary description for ListaEquipamentos.
	/// </summary>
	public partial class ListaEquipamentos : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa; 

		DataView DV;

		private const string ID_PAG = "EQUIPAMENTOS_0";//NOME DA PAGINA
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Page.Form.DefaultButton = btnSearch.UniqueID;

            
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
						ViewState["sortField"] = "TipoEquipamento";
						ViewState["sortDirection"] = "ASC";

						if(Request.QueryString["id"]!= null) ddEmpresa.SelectedValue=Request.QueryString["id"].ToString(); 
             
					}

					if(!ht.ContainsKey("EQUIPAMENTOS_1")) //se n tem permissoes para ver os detalhes dos equipamentos, desactivar o link
					{
						DGEquipamentos.Columns[5].Visible=false; 
					}
				}
			}
		}


		private void fillDDEmpresa()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,txtPesquisaNif.Text,"1","","","","","","");  //activas
            
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV;
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaNif.Text == "") &&(txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}
        

		private void BindGrid()
		{
			DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 
			if(ddEmpresa.SelectedIndex>-1)
			{
				object Estado = ComboAtivoInativo.SelectedValue == "ACTIVE" ? "1" : ComboAtivoInativo.SelectedValue == "INACTIVE" ? "0" : "";
                DataTable DT =  equipamento.DTEquipamento(ddEmpresa.SelectedValue.ToString(), txtTipoEquipamento.Text, txtNumSerie.Text, txtNumIdent.Text, Estado);
				
				DV = new DataView(DT);

				DV.Sort =  ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

				
				DGEquipamentos.DataSource =DV; 
				
				if(DGEquipamentos.CurrentPageIndex >= DGEquipamentos.PageCount)
				{
					DGEquipamentos.CurrentPageIndex=0; 
				}

				DGEquipamentos.DataBind(); 

				if(DV.Table.Rows.Count > 0)
				{
					DGEquipamentos.Visible=true;
				}
				else
				{
					DGEquipamentos.Dispose();
					DGEquipamentos.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
				}
			}
			else
			{
				DGEquipamentos.Dispose();
				DGEquipamentos.Visible=false; 
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
			
			}

			equipamento = null; 
		}


		protected string ConverteEstado(bool b)
		{
			if (b==true) 
			{
				return "activo";
			}
			else
			{
				return "inactivo"; 
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
			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnPesquisaEmpresa.Click += new System.EventHandler(btnPesquisaEmpresa_Click);
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnSearch.Click += new System.EventHandler(btnSearch_Click);
			btnLimparCampos.Click += new System.EventHandler(btnLimparCampos_Click);
		}
		#endregion

			public void DoPaging(Object s,DataGridPageChangedEventArgs e)
			{
				DGEquipamentos.CurrentPageIndex = e.NewPageIndex;
				BindGrid(); 
			}

			protected void SortGrid(Object s, DataGridSortCommandEventArgs e)
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

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGEquipamentos.DataSource = null;
			DGEquipamentos.DataBind(); 
			
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGEquipamentos.DataSource = null;
			DGEquipamentos.DataBind(); 
			
		}

		private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGEquipamentos.DataSource = null;
			DGEquipamentos.DataBind(); 
		}

		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGEquipamentos.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			DGEquipamentos.CurrentPageIndex=0;
			BindGrid(); 
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			
			DGEquipamentos.CurrentPageIndex=0;
			BindGrid();
		}

		private void btnLimparCampos_Click(object sender, System.EventArgs e)
		{
			LimpaCamposPesquisa(); 
		}

		private void LimpaCamposPesquisa()
		{
			txtTipoEquipamento.Text=""; 
			txtNumIdent.Text="";
			txtNumSerie.Text=""; 
		}

    }
}

