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
	/// Summary description for GestPerfis.
	/// </summary>
	public partial class GestPerfis : System.Web.UI.Page
	{
        private const string ID_PAG = "PERFIS_1";//NOME DA PAGINA

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
                        BindGridPerfil(); 
                        fillPerfis();
                        BindGridPaginas();  
                    }
                }
            }
        }

        //nao quero mostrar o perfil -1, utilizador externo "proforma"

        private void fillPerfis()  //lista para preencher a dropdown.

        {
//            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
//            ddPerfis.DataSource=lista.DRListaPerfis(); 
//            ddPerfis.DataBind(); 
            DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD();
            ddPerfis.DataSource =  perfis.ListaPerfis(); 
            ddPerfis.DataBind(); 

			perfis = null;
        }

        private void BindGridPerfil() //datareader para preenhcer o datagrid
        {
            DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD();
            SqlDataReader DR =  perfis.ListaPerfis();
            DGPerfis.DataSource = DR;
            DGPerfis.DataBind(); 
            DR.Close(); 

			perfis = null; 			
        }


        private void BindGridPaginas()
        {
            DATA.PerfisBD perfis= new LabMetro.DATA.PerfisBD(); 
            DataTable DT = perfis.DTPermissions(ddPerfis.SelectedValue); 
			//Apagar os registos sobre as páginas de certificação
			//Se se pretender fazer esta gestão na página GestCertUtilizadore
//			foreach(DataRow dr in DT.Rows)
//			{
//				if(dr["shortname"].ToString() == "CER")
//				{
//					dr.Delete();
//				}
//			}		
			DataView DV = new DataView(DT,null,null,DataViewRowState.CurrentRows);
            //DV.Sort = "MENU ASC, PAGINA ASC"; 

            DGPaginas.DataSource = DV; 
        
            DGPaginas.DataBind();

             
            SqlDataReader myDR = perfis.ListaUtilizadoresPerfil(ddPerfis.SelectedValue); 
            if(myDR.HasRows)
            {
				
                dlUtilizadores.DataSource= myDR; 
                dlUtilizadores.DataBind();
                dlUtilizadores.Visible=true;
            }
            else
            {
                dlUtilizadores.Dispose(); 
                dlUtilizadores.Visible=false;
            }

            myDR.Close(); 

			perfis = null; 			
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
           ddPerfis.SelectedIndexChanged += new System.EventHandler(ddPerfis_SelectedIndexChanged);
           
         
           DGPerfis.ItemDataBound += new DataGridItemEventHandler(DGPerfis_ItemDataBound); 
           DGPerfis.ItemCommand +=new DataGridCommandEventHandler(DGPerfis_ItemCommand);
            btnUpdateDGPaginas.Click += new EventHandler(btnUpdateDGPaginas_Click); 
            

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

        protected string ConverteNome(string str)
        {
            //return(str.IndexOf("Inserir").ToString()); 
            if(str.IndexOf("Inserir") >=0)
            {
                string strNew = str.Replace("Inserir", "Form"); 
                return strNew; 
            }
            else
            {
                return str; 
            }
           
        }

        private void ddPerfis_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DGPaginas.EditItemIndex = -1;
            BindGridPaginas(); 
       
        }

        private void btnUpdateDGPaginas_Click(object sender, System.EventArgs e)
        {   
            foreach(DataGridItem dgi in DGPaginas.Items) 
            { 
                CheckBox cbA =
                    (CheckBox)dgi.Cells[2].FindControl("checkAcesso"); 
                CheckBox cbAT =
                    (CheckBox)dgi.Cells[3].FindControl("checkAcessoTotal"); 
                string idPagina = DGPaginas.DataKeys[dgi.ItemIndex].ToString(); 

                 
                DATA.PerfisBD perfis= new LabMetro.DATA.PerfisBD(); 
                
                //dm 01-08-05 ATENCAO Q OS DADOS NO ADMIN NAO PODEM SER ALTERADOS VIA BACKOFFICE....
                
                //			//TODO: tirar valor manual
                int i = perfis.UpdatePermissions(ddPerfis.SelectedValue,idPagina,cbA.Checked.ToString(),cbAT.Checked.ToString());
                if(i==0)
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB; 
                else lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE; 

				perfis = null; 			

            }
        }
      
//        protected void DGPaginas_Edit(Object sender, DataGridCommandEventArgs e)     
//        {
//
//            DGPaginas.EditItemIndex = e.Item.ItemIndex;	
//            BindGridPaginas();
//        }

//        protected void DGPaginas_CancelGrid(Object sender, DataGridCommandEventArgs e)
//        {
//
//            DGPaginas.EditItemIndex = -1;
//            BindGridPaginas();
//        }
		
//        protected void DGPaginas_UpdateGrid(Object sender, DataGridCommandEventArgs e)
//        {
//            string idPagina = DGPaginas.DataKeys[e.Item.ItemIndex].ToString();
//            //            
//            CheckBox acesso = (System.Web.UI.WebControls.CheckBox)e.Item.FindControl("checkAcessoEdit");
//            CheckBox acessoTotal = (System.Web.UI.WebControls.CheckBox)e.Item.FindControl("checkAcessoTotalEdit");
// 
//            DATA.PerfisBD perfis= new LabMetro.DATA.PerfisBD(); 
//
//			//TODO: tirar valor manual
//            perfis.UpdatePermissions(ddPerfis.SelectedValue,idPagina,acesso.Checked.ToString(),acessoTotal.Checked.ToString(), User.Identity.Name.ToString());
//            
//            DGPaginas.EditItemIndex = -1;
//            BindGridPaginas(); 
//        }
//
        private void DGPerfis_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
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
                        DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD(); 

                        lblMessage.Text = perfis.InsertPerfil(txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString()); 

                        DGPerfis.EditItemIndex = -1;
                        BindGridPerfil(); 
                        DGPerfis.ShowFooter=true; 
                        //limpa a dd e preenche com novos valores
                        ddPerfis.Dispose(); 
                        fillPerfis();
						
						perfis = null; 			
                    }            
                }
            }
        }

        private void DGPerfis_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
                //como a datasource é um reader e nao um dataset, tem de ser feito assim....
                System.Data.Common.DbDataRecord xxx = (System.Data.Common.DbDataRecord)e.Item.DataItem;
                string  estado = xxx.GetValue(2).ToString(); 
//              
                if(estado == "True") ddEstado.SelectedValue ="1";
                else ddEstado.SelectedValue="0";
            }
        }

        protected void DGPerfis_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGPerfis.ShowFooter=false;     
            DGPerfis.EditItemIndex = e.Item.ItemIndex;	
            BindGridPerfil();
        }

        protected void DGPerfis_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGPerfis.ShowFooter=true;  
            DGPerfis.EditItemIndex = -1;
            BindGridPerfil();
        }
		
        protected void DGPerfis_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGPerfis.DataKeys[e.Item.ItemIndex].ToString();
            
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
        

            //************************
            //validacao: se existem utilizadores associados ao perfil, 
            //nao pode alterar o estado do perfil para inactivo
            DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD(); 
            SqlDataReader myDR = perfis.ListaUtilizadoresPerfil(id); 

            if((ddEstado.SelectedValue=="0")&&(myDR.HasRows))
            {
             
                lblMessage.Text="Não pode desactivar um perfil que tenha utilizadores associados.";  
            }
                
            else
            { 

                //************************
                if(txtDescricao.Text =="") 
                {
                    lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
                }
                else
                {
                    
                    lblMessage.Text = perfis.UpdatePerfis(id,txtDescricao.Text,ddEstado.SelectedValue.ToString(), User.Identity.Name.ToString()); //TODO: tirar hard coded values

                    DGPerfis.EditItemIndex = -1;
                    BindGridPerfil(); 
                    DGPerfis.ShowFooter=true; 
                    ddPerfis.Dispose(); 
                    fillPerfis();
                }
            }

			myDR.Close(); 
			perfis = null; 			
        }

		
	}
}
