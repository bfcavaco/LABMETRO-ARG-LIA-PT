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
	/// Summary description for GestaoFamilias.
	/// </summary>
	public partial class GestaoFamilias : System.Web.UI.Page
	{
        private const string ID_PAG = "FAMILIAS_1";//NOME DA PAGINA

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
                        ViewState["sortField"] = "GRANDEZA, DESCRICAO";
                        ViewState["sortDirection"] = "ASC";
                            
                        FillGrandezas();     
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
             
            DGFamilias.ItemDataBound += new DataGridItemEventHandler(DGFamilias_ItemDataBound); 
            DGFamilias.ItemCommand +=new DataGridCommandEventHandler(DGFamilias_ItemCommand);

        }
		#endregion

        private void FillGrandezas()
        {
            
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaGrandezas(); 
            ddGrandeza.DataSource = DR;
            ddGrandeza.DataBind();  
            ddGrandeza.Items.Insert(0, new ListItem("Todas","")); 
            DR.Close(); 

			lista = null; 
        }


        private void BindGrid()
        {
            DATA.FamiliasGrandezasBD familia = new LabMetro.DATA.FamiliasGrandezasBD(); 
            DataTable DT = familia.DTFamilias(ddGrandeza.SelectedValue.ToString(),txtFamilia.Text); 

			DataView DV = new DataView(DT);

			string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
			DV.Sort = strSort; 
			
            DGFamilias.DataSource =DV; 
            DGFamilias.DataBind(); 

            if(DT.Rows.Count == 0)lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;

			familia = null; 

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

        private void DGFamilias_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
                if(e.Item.ItemType == ListItemType.Footer)

                {
                    DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaFooter");
                   
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 
                    DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoFooter");
                    
                    if(txtDescricao.Text =="") 
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
                    }
                    else
                    {
                         DATA.FamiliasGrandezasBD familia = new LabMetro.DATA.FamiliasGrandezasBD(); 

                         lblMessage.Text = familia.InsertFamilia(ddGrandeza.SelectedValue.ToString(), txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString());

						familia = null; 

                        DGFamilias.EditItemIndex = -1;
                        BindGrid(); 
                        DGFamilias.ShowFooter=true;
                    }
                }
            }
        }

        private void DGFamilias_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.EditItem)
            {
				DataRowView DRV = (DataRowView) e.Item.DataItem;

                DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
                
                string estado  = DRV["activo"].ToString();      
                if(estado == "True") ddEstado.SelectedValue ="1";
                else ddEstado.SelectedValue="0";

                DropDownList ddGrandezas = (DropDownList)e.Item.FindControl("ddGrandezaEdit");
                DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
                SqlDataReader DR2 =  lista.DRListaGrandezas(); 
                ddGrandezas.DataSource =DR2;
                ddGrandezas.DataBind(); 
                DR2.Close(); 

				string idGrandeza  = DRV["idGrandeza"].ToString();      
				try
				{
					ddGrandezas.SelectedValue = idGrandeza; 
				}
				catch
				{
					ddGrandezas.Items.Insert(0,new ListItem( DRV["grandeza"].ToString(),idGrandeza)); 

				}

				lista = null; 

                if(ddGrandeza.SelectedValue!="")ddGrandezas.SelectedValue = ddGrandeza.SelectedValue; //para a segunda abrir no mesmo da primeira q está na pesquisa la em cima

            }     
            
            if(e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddGrandezas = (DropDownList)e.Item.FindControl("ddGrandezaFooter");
                
                DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
                SqlDataReader DR3 =  lista.DRListaGrandezas(); 
                ddGrandezas.DataSource = DR3;
                ddGrandezas.DataBind(); 
                DR3.Close(); 

				lista = null; 

                if(ddGrandeza.SelectedValue!="")ddGrandezas.SelectedValue = ddGrandeza.SelectedValue; //para a segunda abrir no mesmo da primeira q está na pesquisa la em cima
               
               
            }
        }

        protected void DGFamilias_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGFamilias.ShowFooter=false;     
            DGFamilias.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DGFamilias_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGFamilias.ShowFooter=true;  
            DGFamilias.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DGFamilias_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGFamilias.DataKeys[e.Item.ItemIndex].ToString();
              
            DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaEdit");
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
           
            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
            }
            else
            {
                 DATA.FamiliasGrandezasBD familia = new LabMetro.DATA.FamiliasGrandezasBD(); 

                 lblMessage.Text = familia.UpdateFamilia(id,ddGrandeza.SelectedValue.ToString(),txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString());

				familia = null; 

                DGFamilias.EditItemIndex = -1;
                BindGrid(); 
                DGFamilias.ShowFooter=true; 
            }
        }

        protected void DGFamilias_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        
        }

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGFamilias.CurrentPageIndex = e.NewPageIndex;
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

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            DGFamilias.CurrentPageIndex=0; 
            BindGrid(); 
        }
    }
}

