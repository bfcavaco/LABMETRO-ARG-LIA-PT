using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace LabMetro
{
    /// <summary>
    /// Summary description for GestUtilizadores.
    /// </summary>
	public partial class GestUtilizadores : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.DropDownList ddEmpresa;
        
        private const string ID_PAG = "UTILIZADORES_1";//NOME DA PAGINA
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
                        
                        ViewState["sortDirectionF"]="ASC";
                        ViewState["sortFieldF"] = "nome"; 

        				
                            BindGridFuncionarios();        
                      
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
            dgFuncionario.ItemDataBound += new DataGridItemEventHandler(dgFuncionario_ItemDataBound); 
		
			btnSearch.Click += new System.EventHandler(btnSearch_Click);
		
            
        }
		#endregion

        

        
        
        
        private void BindGridFuncionarios()

        {
            
            
            DATA.UtilizadoresBD utilizador = new LabMetro.DATA.UtilizadoresBD(); 

			DataTable DT = utilizador.DTListaFuncionarios(); 
			DataView DV = new DataView(DT);

			string strRowfilter ="nome LIKE '"+txtNome.Text+"%'";  

			DV.RowFilter =  strRowfilter;

			DV.Sort = ViewState["sortFieldF"].ToString()+ " " + ViewState["sortDirectionF"]; 
    
            if(DT.Rows.Count > 0)
            {
                dgFuncionario.DataSource=DV; 
                dgFuncionario.DataBind(); 
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_DATA; 
                dgFuncionario.Controls.Clear(); 
            }

			utilizador = null; 
        }

        
        protected void EditDgFuncionario(Object sender,DataGridCommandEventArgs e)
        {
            dgFuncionario.EditItemIndex = e.Item.ItemIndex;    
           
            BindGridFuncionarios(); 
        }
        protected void CancelDgFuncionario(Object sender,DataGridCommandEventArgs e)
        {
            dgFuncionario.EditItemIndex = -1;
           
            BindGridFuncionarios(); 
        }

        protected void UpdateDgFuncionario(Object sender,DataGridCommandEventArgs e)
        {
            string id = dgFuncionario.DataKeys[e.Item.ItemIndex].ToString(); 
            TextBox txtUsername = (TextBox)e.Item.FindControl("txtUsernameFuncionario"); 
            TextBox txtPassword = (TextBox)e.Item.FindControl("txtPasswordFuncionario"); 
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.ToString(),"md5");
            DropDownList ddPerfil = (DropDownList)e.Item.FindControl("ddPerfilFuncionario"); 
            
            if((txtUsername.Text =="") ||(ddPerfil.SelectedIndex ==0) || (txtPassword.Text=="")) 
            {
                lblMessage.Text= GERAL.clsGeral.ErrorMessage.ERR_MISSING_FIELDS; 
                
            }
            else
            {
                DATA.UtilizadoresBD utilizador= new LabMetro.DATA.UtilizadoresBD(); 

                lblMessage.Text = utilizador.ExecuteUsers("Funcionario", id,ddPerfil.SelectedValue.ToString(),User.Identity.Name.ToString(), txtUsername.Text, hashedPassword);
                dgFuncionario.EditItemIndex = -1; 
                BindGridFuncionarios();

				utilizador = null; 
            }
        
        }
    
        private void dgFuncionario_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            DataRowView DRV = (DataRowView) e.Item.DataItem;

            if((e.Item.ItemType == ListItemType.Item) ||(e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if(DRV["passwd"].ToString() !="") e.Item.Cells[4].Text = "******"; 
            }

            if(e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddPerfil = (DropDownList)e.Item.FindControl("ddPerfilFuncionario");              
                DATA.PerfisBD perfil = new LabMetro.DATA.PerfisBD(); 
                SqlDataReader DR =  perfil.ListaPerfis(); 
                ddPerfil.DataSource = DR;
                ddPerfil.DataBind(); 
                ddPerfil.Items.Insert(0, new ListItem("","")); 
                
                DR.Close(); 

				perfil = null; 
                
                string idPerfil = DRV["idPerfil"].ToString();   
                ddPerfil.SelectedValue = idPerfil.ToString(); 

                string passwd = DRV["passwd"].ToString();   
                string username = DRV["username"].ToString();   

                TextBox txtUsername = (TextBox)e.Item.FindControl("txtUsernameFuncionario"); 
                TextBox txtPassword = (TextBox)e.Item.FindControl("txtPasswordFuncionario"); 

                if(passwd!="") 
                {
                    txtPassword.Enabled=false; 
                    txtPassword.Text = "******"; 
                }
                //DM 01-08-2005 temporariamente podem mudar o username ate indicacao contrario
//                if(username!="")
//                {
//                    txtUsername.Enabled=false; 
//                }
            }
        }

        
        public void DoPagingFuncionarios(Object s,DataGridPageChangedEventArgs e)
        {
            dgFuncionario.CurrentPageIndex = e.NewPageIndex;
            BindGridFuncionarios();
        }

        public void SortGridFuncionarios(Object s, DataGridSortCommandEventArgs e)
        {
            switch (ViewState["sortDirectionF"].ToString())
            {
                case "ASC":
                    ViewState["sortDirectionF"]="DESC"; 
                    break;
                case "DESC":
                    ViewState["sortDirectionF"]="ASC";
                    break;
            }
            ViewState["sortFieldF"] = e.SortExpression;
            BindGridFuncionarios(); 
        }

		private void btnSearch_Click(object sender, System.EventArgs e)
		{

            dgFuncionario.CurrentPageIndex = 0; 
			BindGridFuncionarios();        
		
		}
	}
}
