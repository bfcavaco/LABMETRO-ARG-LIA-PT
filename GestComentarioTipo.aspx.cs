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
	/// Summary description for GestComentarioTipo.
	/// </summary>
	public partial class GestComentarioTipo : System.Web.UI.Page
	{
        private const string ID_PAG = "COMENTARIOSTIPO_1";//NOME DA PAGINA

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
                        ViewState["sortField"] = "descricao";
                        ViewState["sortDirection"] = "DESC";
                        BindGrid(); 
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
            DG.DeleteCommand += new  DataGridCommandEventHandler(DG_DeleteCommand);
            //DG.ItemDataBound += new DataGridItemEventHandler(DG_ItemDataBound); 
            DG.ItemCommand +=new DataGridCommandEventHandler(DG_ItemCommand);
        }

		#endregion

        private void BindGrid()
        {
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            DataTable DT = orcamento.FillComentariosTipo(ViewState["sortField"].ToString(),ViewState["sortDirection"].ToString()); 

            DG.DataSource =DT; 
            DG.DataBind(); 
            
            orcamento = null; 
        }

        private void DG_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
                if(e.Item.ItemType == ListItemType.Footer)
                {
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 

                    
                    if(txtDescricao.Text =="") 
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
                    }
                    else
                    {
                        DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

                        lblMessage.Text = orcamento.InsertOrcamentoComentarioTipo(txtDescricao.Text,User.Identity.Name.ToString()); 

                        DG.EditItemIndex = -1;
                        BindGrid(); 
                        DG.ShowFooter=true; 

						orcamento = null; 
                    }    
                }
            }
        }

        protected void DG_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DG.ShowFooter=false;     
            DG.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DG_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DG.ShowFooter=true;  
            DG.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DG_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DG.DataKeys[e.Item.ItemIndex].ToString();
            
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
           
        
            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
            }
            else
            {
                DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

                lblMessage.Text = orcamento.UpdateOrcamentoComentarioTipo(id,txtDescricao.Text,User.Identity.Name.ToString()); 

                DG.EditItemIndex = -1;
                BindGrid(); 
                DG.ShowFooter=true; 

				orcamento = null; 
            }
        }

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DG.CurrentPageIndex = e.NewPageIndex;
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
        
        private void DG_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
        {
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            string id = DG.DataKeys[e.Item.ItemIndex].ToString(); 
            
			lblMessage.Text = orcamento.DeleteOrcamentoComentarioTipo(id,User.Identity.Name.ToString()); 
            
			BindGrid(); 

			orcamento = null; 
                
        }

	}
}
