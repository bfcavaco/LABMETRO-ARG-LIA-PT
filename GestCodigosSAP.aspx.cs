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
	/// Summary description for GestCodigosSAP.
	/// </summary>
	public partial class GestCodigosSAP : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dgRel;
	
		private const string ID_PAG = "CODIGOS_SAP";//NOME DA PAGINA

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
					// Put user code to initialize the page here
					if(!Page.IsPostBack)
					{
						ViewState["sortField"] = "valor";
						ViewState["sortDirection"] = "ASC";

						BindDDTabelas(); 
						BindGrid();
					}
				}
			}
		}

		//==========================================================
		//bind da dropdown das tabelas
		//==========================================================
		private void BindDDTabelas()
		{
			ddTabelas.Items.Add(new ListItem("Codigos PEP3", "PEP")); 
			ddTabelas.Items.Add(new ListItem("Codigos de Serviço", "Servico")); 
			ddTabelas.Items.Add(new ListItem("Escritório de Vendas", "EscritorioVendas")); 
			ddTabelas.Items.Add(new ListItem("Região de Vendas", "RegiaoVendas")); 
			ddTabelas.Items.Add(new ListItem("Condições de Pagamento", "CondPagam")); 
			ddTabelas.Items.Add(new ListItem("Tipo de Documento", "TipoDocumento"));
			ddTabelas.Items.Add(new ListItem("Códigos de Bloqueio", "Bloqueio"));
		}

		//==========================================================
		//BINDGRID do grid dos valores dos campos das tabelas
		//==========================================================
		private void BindGrid()
		{
		
			string tableName= ddTabelas.SelectedValue; 

			string strSQL ="SELECT idCodigo"+tableName+" as id, codigo"+tableName+" as codigo, descCodigo"+tableName+" as descricao,activo FROM sap_Codigo"+tableName+ " order by 3"; 
			//Response.Write(strSQL); 
			DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			DG.DataSource = dt; 
			DG.DataBind(); 
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
           
			DG.ItemDataBound += new DataGridItemEventHandler(DG_ItemDataBound); 
			DG.ItemCommand +=new DataGridCommandEventHandler(DG_ItemCommand);
		}
		#endregion


		
		
		//==========================================================
		//selectedIndexChanged da dropdown das tabelas
		//==========================================================	
		protected void ddTabelas_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindGrid(); 
		}

		
		//==========================================================
		// INSERT do grid dos valores 
		//==========================================================
		private void DG_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Insert")
			{
				if(e.Item.ItemType == ListItemType.Footer)
				{
					TextBox txtValor = (TextBox)e.Item.FindControl("txtValorFooter"); 
					TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 
					DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoFooter");
                    
					if((txtDescricao.Text =="") ||(txtValor.Text ==""))
					{
						lblMessage.Text=GERAL.clsGeral.ErrorMessage.ERR_MISSING_FIELDS; 
					}
					else
					{
						string tableName= ddTabelas.SelectedValue; 

						string strSQL = "INSERT INTO sap_Codigo"+tableName+" (codigo"+tableName+", descCodigo"+tableName+", dtInsert, idInsertUser, dtModif, idModifUser, activo) VALUES ('"+ txtValor.Text+"','"+txtDescricao.Text + "', getDate(), "+Session["UserId"]+",getdate(), "+Session["UserId"]+", "+ddEstado.SelectedValue+")"; 
						
						//Response.Write(strSQL+"<br />"); 
						
						int i = GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
						if(i != 1)
						{
							lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
						}
						else
						{
							lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
						}

						DG.EditItemIndex = -1;
						BindGrid(); 
						DG.ShowFooter=true; 
						
					}    
				}
			}
		}

		//==========================================================
		//itemdatabound do grid dos valores
		//==========================================================
		private void DG_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
				DataRowView DRV = (DataRowView) e.Item.DataItem;
				string estado  = DRV["activo"].ToString();      
				if(estado == "True") ddEstado.SelectedValue ="1";
				else ddEstado.SelectedValue="0";
			}
		}

		//==========================================================
		//EDIT do grid dos valores
		//==========================================================
		protected void DG_Edit(Object sender, DataGridCommandEventArgs e)     
		{
			DG.ShowFooter=false;     
			DG.EditItemIndex = e.Item.ItemIndex;	
			BindGrid();
		}

		//==========================================================
		//CANCEL do grid dos valores
		//==========================================================
		protected void DG_CancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DG.ShowFooter=true;  
			DG.EditItemIndex = -1;
			BindGrid();
		}
		
		//==========================================================
		//UPDATE do grid dos valores
		//==========================================================
		protected void DG_UpdateGrid(Object sender, DataGridCommandEventArgs e)
		{
			string id= DG.DataKeys[e.Item.ItemIndex].ToString();
            
			TextBox txtValor = (TextBox)e.Item.FindControl("txtValor");
			TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
			DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
        
			if(txtDescricao.Text =="") 
			{
				lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
			}
			else
			{
				string tableName= ddTabelas.SelectedValue; 
				
				string strSQL = "UPDATE sap_Codigo"+tableName+" SET codigo"+tableName+" = '"+ txtValor.Text+"', descCodigo"+tableName+"= '"+txtDescricao.Text + "',   idModifUser="+Session["UserId"]+",dtModif = getDate(), activo="+ddEstado.SelectedValue +" WHERE idCodigo"+tableName+" = "+id; 

				//Response.Write(strSQL); 
						
				int i = GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
				if(i != 1)
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE; 
				}
				else
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
				}

				DG.EditItemIndex = -1;
				BindGrid(); 
				DG.ShowFooter=true; 
			}
		}

		//==========================================================
		//PAGING do grid dos valores
		//==========================================================
		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DG.CurrentPageIndex = e.NewPageIndex;
			BindGrid();
		}

		//==========================================================
		//SORT do grid dos valores
		//==========================================================
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


		//==========================================================
		//função que converte o estado para string
		//==========================================================
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

		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************


		//as relações deixaram de existir da forma que foram incialmente pensados.
		//agora, os codigos do sap correspondem:

		//codigoPEP3 relacionado ao laboratório
		//codigoServico estão associados à grandeza
		//regiaoVendas está: associado ao Laborotório que efectuou o serviço no caso de serviços
		//dentro do isq, e à regiao de vendas do cliente no caso de calibrações no exterior.
		//campo é actualizado por trigger na tabela serviço, quando um serviço é passado para calibrado.
		//os outros dois campos tb são associados ao serviço nessa altura, mas são obtidos através da tabela grandeza.

		//o escritorio de vendas está associado à factura... 


	}
}
