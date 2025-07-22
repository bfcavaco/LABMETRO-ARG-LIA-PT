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
	/// Summary description for GestGrandezas.
	/// </summary>
	public partial class GestGrandezas : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.DataGrid DGPais;
        private const string ID_PAG = "GRANDEZAS_1";//NOME DA PAGINA

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
           
              DGGrandeza.ItemCommand +=new DataGridCommandEventHandler(DGGrandeza_ItemCommand);
          }
        #endregion

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

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGGrandeza.CurrentPageIndex = e.NewPageIndex;
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

        private void BindGrid()
        {
            DATA.FamiliasGrandezasBD data = new LabMetro.DATA.FamiliasGrandezasBD(); 
			DataTable DT = data.DTListaGrandezas(); 

			DataView DV = new DataView(DT); 
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
	        
            DGGrandeza.DataSource =  DV; 
            DGGrandeza.DataBind(); 
			data = null; 

        }

       
        protected void DGGrandeza_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGGrandeza.ShowFooter=false;     
            DGGrandeza.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DGGrandeza_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGGrandeza.ShowFooter=true;  
            DGGrandeza.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DGGrandeza_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGGrandeza.DataKeys[e.Item.ItemIndex].ToString();
            
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
			DropDownList ddCodServico = (DropDownList)e.Item.FindControl("ddCodServico");
        
            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.ERR_MISSING_FIELDS; 
            }
            else
            {
                DATA.FamiliasGrandezasBD data = new LabMetro.DATA.FamiliasGrandezasBD(); 
                
                lblMessage.Text = data.UpdateGrandezas(id,txtDescricao.Text,ddEstado.SelectedValue.ToString(),User.Identity.Name.ToString(),ddCodServico.SelectedValue);

                DGGrandeza.EditItemIndex = -1;
                DGGrandeza.ShowFooter=true; 
				BindGrid(); 

				data = null; 
            }
        }
      
        private void DGGrandeza_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
                if(e.Item.ItemType == ListItemType.Footer)

                {
                    TextBox txtIdGrandeza = (TextBox)e.Item.FindControl("txtIdGrandezaFooter");
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 
                    DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoFooter");
					DropDownList ddCodServico = (DropDownList)e.Item.FindControl("ddCodServicoFooter");


                    if((txtDescricao.Text =="") ||(txtIdGrandeza.Text==""))
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.ERR_MISSING_FIELDS; 
                    }
                    else
                    {
                         DATA.FamiliasGrandezasBD data = new LabMetro.DATA.FamiliasGrandezasBD(); 

                        lblMessage.Text = data.InsertGrandeza(txtIdGrandeza.Text,txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString(), ddCodServico.SelectedValue); 

                        DGGrandeza.EditItemIndex = -1;
                        DGGrandeza.ShowFooter=true; 
						BindGrid(); 

						data = null; 
                    }          
				}
            }
        }     
   
		protected void DGGrandeza_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;

				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
				string estado  = DRV["activo"].ToString();      
				if(estado == "True") ddEstado.SelectedValue ="1";
				else ddEstado.SelectedValue="0";
				
				//dropdown codigo SERVIÇO
				DropDownList ddCodServico = (DropDownList)e.Item.FindControl("ddCodServico");
				ddCodServico.DataValueField="id";
				ddCodServico.DataTextField="descricao"; 
				ddCodServico.DataSource= drCodigosServico();
				ddCodServico.DataBind(); 
				ddCodServico.Items.Insert(0, new ListItem("","")); 
				
				string codServico  = DRV["idCodigoServico"].ToString();
				if(codServico.Trim() != "")	ddCodServico.SelectedValue= codServico; 
			}     

			if(e.Item.ItemType == ListItemType.Footer)
			{	
				//footer codigo SERVIÇO
				DropDownList ddCodServicoFooter = (DropDownList)e.Item.FindControl("ddCodServicoFooter");
				ddCodServicoFooter.DataValueField="id";
				ddCodServicoFooter.DataTextField="descricao"; 
				ddCodServicoFooter.DataSource= drCodigosServico(); 
				ddCodServicoFooter.DataBind(); 		
				ddCodServicoFooter.Items.Insert(0, new ListItem("","")); 
			}
		}

		
		private SqlDataReader drCodigosServico()
		{
			string strSQL ="SELECT idCodigoServico as id, codigoServico, descCodigoServico as descricao FROM sap_CodigoServico where activo = 1 ORDER BY 3"; 
		
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}
    }
}
