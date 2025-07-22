using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace LabMetro
{
    /// <summary>
    /// Summary description for GestPasswords.
    /// </summary>
	public partial class GestPasswords : System.Web.UI.Page
	{
        private const string ID_PAG = "PASSWORDS_1";//NOME DA PAGINA

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

            base.OnInit(e);
        }
		
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {    

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

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected void UpdateDgFuncionario(Object sender,DataGridCommandEventArgs e)
        {
            string id = dgFuncionario.DataKeys[e.Item.ItemIndex].ToString(); 
            
            TextBox txtPassword = (TextBox)e.Item.FindControl("txtPasswordFuncionario"); 
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.ToString(),"md5");

            if ((txtPassword.Text=="")) 
            {
                lblMessage.Text="Tem de introduzir uma password.";  
            }
            else
            {
                DATA.UtilizadoresBD utilizador= new LabMetro.DATA.UtilizadoresBD(); 

                lblMessage.Text = utilizador.UpdatePassword(id,User.Identity.Name.ToString(),hashedPassword);
                dgFuncionario.EditItemIndex = -1; 
                BindGridFuncionarios();

				utilizador = null; 
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

      

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			BindGridFuncionarios();        
		}
    }
}
