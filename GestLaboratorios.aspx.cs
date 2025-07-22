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
	/// Summary description for GestLaboratorios.
	/// </summary>
	public partial class GestLaboratorios : System.Web.UI.Page
	{
       
        protected System.Web.UI.WebControls.Button btnSubmit;
        private const string ID_PAG = "LABORATORIOS_1";//NOME DA PAGINA

    
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
                        ViewState["sortField"] = "grandeza";
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
            //InitializeComponent2(); 
            base.OnInit(e);
        }
		
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {   

        }          
    
    //{    
    //          DGLaboratorios.SelectedIndexChanged += new System.EventHandler(DGLaboratorios_SelectedIndexChanged);
             
    //          DGLaboratorios.ItemDataBound += new DataGridItemEventHandler(DGLaboratorios_ItemDataBound); 
    //          DGLaboratorios.ItemCommand +=new DataGridCommandEventHandler(DGLaboratorios_ItemCommand);

    //      }
        #endregion

        
        private void BindGrid()
        {
            DATA.LaboratoriosBD laboratorio = new LabMetro.DATA.LaboratoriosBD(); 
            DataTable DT = laboratorio.DTLaboratorios(); 
            
			DataView DV = new DataView(DT);          
			DV.Sort =ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
			DGLaboratorios.DataSource =DV; 
            DGLaboratorios.DataBind(); 
            
			if(DT.Rows.Count == 0) lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;           

			laboratorio = null; 
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

        protected void DGLaboratorios_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
                if(e.Item.ItemType == ListItemType.Footer)

                {
                    DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaFooter");

                    DropDownList ddLocalLaboratorio = (DropDownList)e.Item.FindControl("ddLocalLaboratorioFooter");

					DropDownList ddCodPEP = (DropDownList)e.Item.FindControl("ddCodPEPFooter");
					DropDownList ddRegiaoVendas = (DropDownList)e.Item.FindControl("ddRegiaoVendasFooter");

                    //TextBox txtCentroCusto = (TextBox)e.Item.FindControl("txtCodigoCentroCustoFooter"); 
                    DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoFooter");
                    
                    //if(txtCentroCusto.Text =="") 
                    //{
                    //    lblMessage.Text=GERAL.clsGeral.ErrorMessage.ERR_MISSING_FIELDS;
                    //}
                    //else
                    //{
                        DATA.LaboratoriosBD laboratorio = new LabMetro.DATA.LaboratoriosBD(); 

                        lblMessage.Text = laboratorio.InsertLaboratorio(ddGrandeza.SelectedValue.ToString(),ddLocalLaboratorio.SelectedValue.ToString(),null,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString(), ddCodPEP.SelectedValue.ToString(), ddRegiaoVendas.SelectedValue.ToString()); 
						

                        DGLaboratorios.EditItemIndex = -1;
                        BindGrid(); 
                        DGLaboratorios.ShowFooter=true;

						laboratorio = null; 
                    //}
                }
            }
        }

        protected void DGLaboratorios_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;

				//dropdown codigo PEP
				DropDownList ddCodPEP = (DropDownList)e.Item.FindControl("ddCodPEP");
				ddCodPEP.DataValueField="id"; 
				ddCodPEP.DataTextField="descricao"; 
				ddCodPEP.DataSource= drCodigosPEP(); 
				ddCodPEP.DataBind(); 
				ddCodPEP.Items.Insert(0, new ListItem("","")); 

				string idPEP  = DRV["idCodigoPEP"].ToString();      
				if(idPEP.Trim() != "")	
					try
					{
						ddCodPEP.SelectedValue= idPEP;  
					}
					catch
					{
						Response.Write("erro na dd codigo pep - " + idPEP); 
					}

                //dropdown codigo regiao de vendas associada ao laboratório
                DropDownList ddRegiaoVendas = (DropDownList)e.Item.FindControl("ddRegiaoVendas");
                ddRegiaoVendas.DataValueField = "id";
                ddRegiaoVendas.DataTextField = "descricao";
                ddRegiaoVendas.DataSource = drCodigosRegiaoVendas();
                ddRegiaoVendas.DataBind();
                ddRegiaoVendas.Items.Insert(0, new ListItem("", ""));

                string regiaoVendas = DRV["idCodigoRegiaoVendas"].ToString();
                if (regiaoVendas.Trim() != "") ddRegiaoVendas.SelectedValue = regiaoVendas;
            }


            else if(e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaFooter");
                DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
                SqlDataReader DR =  lista.DRListaGrandezas(); 
                ddGrandeza.DataSource = DR;
                ddGrandeza.DataBind(); 
                DR.Close(); 

                DropDownList ddLocalLaboratorio = (DropDownList)e.Item.FindControl("ddLocalLaboratorioFooter");
                SqlDataReader DR2 =  lista.DRListaLocalCalibracao(); 
                ddLocalLaboratorio.DataSource =DR2;
                ddLocalLaboratorio.DataBind(); 
                DR2.Close(); 

				lista = null; 

				//footer codigo PEP
				DropDownList ddCodPEPFooter = (DropDownList)e.Item.FindControl("ddCodPEPFooter");
				ddCodPEPFooter.DataValueField="id"; 
				ddCodPEPFooter.DataTextField="descricao"; 
				ddCodPEPFooter.DataSource= drCodigosPEP(); 
				ddCodPEPFooter.DataBind(); 
				ddCodPEPFooter.Items.Insert(0, new ListItem("",""));

                DropDownList ddRegiaoVendas = (DropDownList)e.Item.FindControl("ddRegiaoVendasFooter");

                ddRegiaoVendas.DataValueField = "id";
                ddRegiaoVendas.DataTextField = "descricao";
                ddRegiaoVendas.DataSource = drCodigosRegiaoVendas();
                ddRegiaoVendas.DataBind();
                ddRegiaoVendas.Items.Insert(0, new ListItem("", ""));
            }
        }

        protected void DGLaboratorios_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGLaboratorios.ShowFooter=false;     
            DGLaboratorios.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DGLaboratorios_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGLaboratorios.ShowFooter=true;  
            DGLaboratorios.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DGLaboratorios_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGLaboratorios.DataKeys[e.Item.ItemIndex].ToString();
    
            DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
            DATA.LaboratoriosBD laboratorio = new LabMetro.DATA.LaboratoriosBD(); 

			DropDownList ddCodPEP = (DropDownList)e.Item.FindControl("ddCodPEP");
			DropDownList ddRegiaoVendas = (DropDownList)e.Item.FindControl("ddRegiaoVendas");

            lblMessage.Text = laboratorio.UpdateLaboratorio(id, ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString(), ddCodPEP.SelectedValue.ToString(),ddRegiaoVendas.SelectedValue.ToString()); 
			laboratorio = null; 
            
			DGLaboratorios.EditItemIndex = -1;
			DGLaboratorios.ShowFooter=true;   
            BindGrid(); 
            
			
        }

        private void DGLaboratorios_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        
        }

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGLaboratorios.CurrentPageIndex = e.NewPageIndex;
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

		private SqlDataReader drCodigosPEP()
		{
			string strSQL ="SELECT idCodigoPEP as id, codigoPEP, descCodigoPEP as descricao FROM sap_CodigoPEP where activo = 1 ORDER BY 3"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}
		

		private SqlDataReader drCodigosRegiaoVendas()
		{
			string strSQL ="SELECT idCodigoRegiaoVendas as id, codigoRegiaoVendas, descCodigoRegiaoVendas as descricao FROM sap_CodigoRegiaoVendas where activo = 1 ORDER BY 3"; 

			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}
		

    }
}
