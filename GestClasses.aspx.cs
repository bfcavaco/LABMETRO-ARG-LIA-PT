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
	/// Summary description for GestClasses.
	/// </summary>
	public partial class GestClasses : System.Web.UI.Page
	{
        private const string ID_PAG = "CLASSES_1";//NOME DA PAGINA
        
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
                        ViewState["sortDirection"] = "ASC";
                        
                        BindGrid(); 
                    }
                }
            }
        }
        private void BindGrid()
        {
            DATA.TabelasBD tabela = new LabMetro.DATA.TabelasBD(); 
            DataTable DT = tabela.DTTabela("Classe"); 
			DataView DV = new DataView(DT);

			string strRowfilter ="descricao LIKE '"+txtNome.Text+"%'";  

			DV.RowFilter =  strRowfilter;
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

            DG.DataSource =DV; 
            DG.DataBind(); 
            
			tabela = null; 
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

        private void DG_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {

                if(e.Item.ItemType == ListItemType.Footer)
                {
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 
                    DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoFooter");
                    
                    if(txtDescricao.Text =="") 
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
                    }
                    else
                    {
                        DATA.TabelasBD tabela = new LabMetro.DATA.TabelasBD(); 

                        lblMessage.Text = tabela.InsertTable("Classe",txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString()); 

                        DG.EditItemIndex = -1;
                        BindGrid(); 
                        DG.ShowFooter=true; 

						tabela = null; 
                    }    
                }
            }
        }

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
            DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
        
            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
            }
            else
            {
                DATA.TabelasBD tabela = new LabMetro.DATA.TabelasBD(); 

                lblMessage.Text = tabela.UpdateTable("Classe",id,txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString()); 

				tabela = null; 

                DG.EditItemIndex = -1;
                BindGrid(); 
                DG.ShowFooter=true; 
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


		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			BindGrid(); 
		}
    }
}
