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
	/// Summary description for GestTiposEquipamento.
	/// </summary>
	public partial class GestTiposEquipamento : System.Web.UI.Page
	{
        private const string ID_PAG = "TIPOSEQUIPAMENTO_1";//NOME DA PAGINA


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
                        ViewState["sortField"] = "familia, descricao";
                        ViewState["sortDirection"] = "ASC";
						FillGrandezas(); 
                        FillFamilias();     
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
             
            DGTipoEquip.ItemDataBound += new DataGridItemEventHandler(DGTipoEquip_ItemDataBound); 
            DGTipoEquip.ItemCommand +=new DataGridCommandEventHandler(DGTipoEquip_ItemCommand);

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

        private void FillFamilias()
        {
            DATA.FamiliasGrandezasBD lista = new LabMetro.DATA.FamiliasGrandezasBD();
            SqlDataReader DR = lista.DRFamilias(ddGrandeza.SelectedValue);  
            ddFamilia.DataSource = DR;
            ddFamilia.DataBind();  
            ddFamilia.Items.Insert(0, new ListItem("Todas","")); 
            DR.Close(); 

			lista = null; 
        }

        private void BindGrid()
        {
            DATA.TipoEquipamentoBD tipoEquipamento = new LabMetro.DATA.TipoEquipamentoBD(); 
            DataTable DT = tipoEquipamento.DTTipoEquipamento(ddFamilia.SelectedValue.ToString(),txtCodigo.Text, txtTipoEquipamento.Text, ddGrandeza.SelectedValue); 

			DataView DV = new DataView(DT);

			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
			 
            DGTipoEquip.DataSource =DV; 
            DGTipoEquip.DataBind(); 

            if(DT.Rows.Count == 0)lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;  

			tipoEquipamento = null; 
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

		protected string ConverteEstadoAC(bool b)
		{
			if (b==true) 
			{
				return "acreditado";
			}
			else
			{
				return "não acreditado"; 
			}
		}

        private void DGTipoEquip_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
                if(e.Item.ItemType == ListItemType.Footer)
                {
                    DropDownList ddFamilia = (DropDownList)e.Item.FindControl("ddFamiliaFooter");
                    TextBox txtCodigo = (TextBox)e.Item.FindControl("txtCodigoFooter"); 
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter");
					TextBox txtSimbolo = (TextBox)e.Item.FindControl("txtSimboloFooter"); 
					DropDownList ddAcreditado = (DropDownList)e.Item.FindControl("ddAcreditadoFooter");
                    DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoFooter");
                    
                    if((txtDescricao.Text =="") ||(txtCodigo.Text ==""))
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.ERR_MISSING_FIELDS; 
                    }
                    else
                    {
                        DATA.TipoEquipamentoBD tipoEquipamento = new LabMetro.DATA.TipoEquipamentoBD(); 
//Acrescentar na store de insert os novos campos
                        lblMessage.Text = tipoEquipamento.InsertTipoEquipamento(ddFamilia.SelectedValue.ToString(),txtCodigo.Text, txtDescricao.Text, ddAcreditado.SelectedValue.ToString(), txtSimbolo.Text, ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString()); 

                        DGTipoEquip.EditItemIndex = -1;
                        BindGrid(); 
                        DGTipoEquip.ShowFooter=true;

						tipoEquipamento = null; 
                    }
                }
            }
        }

        private void DGTipoEquip_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.EditItem)
            {

			
                DataRowView DRV = (DataRowView) e.Item.DataItem;
           		
                DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoEdit");
                
                string estado  = DRV["activo"].ToString();      
                if(estado == "True") ddEstado.SelectedValue ="1";
                else ddEstado.SelectedValue="0";

				DropDownList ddAcreditado = (DropDownList)e.Item.FindControl("ddAcreditadoEdit");
				
				string acreditado  = DRV["acreditado"].ToString();      
				if(acreditado == "True") ddAcreditado.SelectedValue ="1";
				else ddAcreditado.SelectedValue="0";

                DropDownList ddFamilias = (DropDownList)e.Item.FindControl("ddFamiliaEdit");
                //DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
                //SqlDataReader DR1 = lista.DRListaFamilias(); 
                //ddFamilias.DataSource = DR1; 
                //ddFamilias.DataBind(); 
                //DR1.Close(); //isto vai buscar o idfamilia as ident e eu nao quero


                DATA.FamiliasGrandezasBD lista = new LabMetro.DATA.FamiliasGrandezasBD();
                SqlDataReader DR1 = lista.DRFamilias(ddGrandeza.SelectedValue);  
                ddFamilias.DataSource = DR1;
                ddFamilias.DataBind();

                DR1.Close(); 
                lista = null; 


				string idFamilia = DRV["idFamilia"].ToString();   
                                
			
				try
				{
					ddFamilias.SelectedValue = idFamilia; 
				}
				catch
				{
					ddFamilias.Items.Insert(0,new ListItem(DRV["familia"].ToString(),idFamilia)); 

				}
			

                if(ddFamilia.SelectedValue!="")ddFamilias.SelectedValue = ddFamilia.SelectedValue; //para a segunda abrir no mesmo da primeira q está na pesquisa la em cima
            }     
            
            if(e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddFamilias = (DropDownList)e.Item.FindControl("ddFamiliaFooter");

                DATA.FamiliasGrandezasBD lista = new LabMetro.DATA.FamiliasGrandezasBD();
                SqlDataReader DR2 = lista.DRFamilias(ddGrandeza.SelectedValue);  
               
                ddFamilias.DataSource = DR2;
                ddFamilias.DataBind(); 
                DR2.Close(); 

				lista = null; 

				try
				{
					if(ddFamilia.SelectedValue!="")
					{//para a segunda abrir no mesmo da primeira q está na pesquisa la em cima
						ddFamilias.SelectedValue = ddFamilia.SelectedValue;
					}
				}
				catch
				{
				}
            }
        }

        protected void DGTipoEquip_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGTipoEquip.ShowFooter=false;     
            DGTipoEquip.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DGTipoEquip_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGTipoEquip.ShowFooter=true;  
            DGTipoEquip.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DGTipoEquip_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGTipoEquip.DataKeys[e.Item.ItemIndex].ToString();
              
            DropDownList ddFamilia = (DropDownList)e.Item.FindControl("ddFamiliaEdit");
            TextBox txtCodigo = (TextBox)e.Item.FindControl("txtCodigoEdit"); 
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoEdit"); 
			TextBox txtSimbolo = (TextBox)e.Item.FindControl("txtSimboloEdit"); 
			DropDownList ddAcreditado = (DropDownList)e.Item.FindControl("ddAcreditadoEdit");
            DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoEdit");
           
            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
            }
            else
            {
                DATA.TipoEquipamentoBD tipoEquipamento = new LabMetro.DATA.TipoEquipamentoBD(); 
//Alterar a store de update do tipo de equipamento
                lblMessage.Text = tipoEquipamento.UpdateTipoEquipamento(id,ddFamilia.SelectedValue.ToString(),txtCodigo.Text, txtDescricao.Text, ddAcreditado.SelectedValue.ToString(), txtSimbolo.Text, ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString()); 

                DGTipoEquip.EditItemIndex = -1;
                BindGrid(); 

                DGTipoEquip.ShowFooter=true; 

				tipoEquipamento = null; 
            }
        }

        protected void DGTipoEquip_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGTipoEquip.CurrentPageIndex = e.NewPageIndex;
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
            DGTipoEquip.CurrentPageIndex=0;
            BindGrid(); 
        }

		protected void ddGrandeza_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			FillFamilias(); 
		}
    }
}

