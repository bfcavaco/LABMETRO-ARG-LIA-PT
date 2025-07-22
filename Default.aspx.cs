using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace LabMetro
{
    /// <summary>
    /// Summary description for _Default.
    /// </summary>
	public partial class _Default : System.Web.UI.Page
	{
        protected System.Web.UI.HtmlControls.HtmlTableCell TD1;
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //Page.Form.DefaultButton = btnSubmit.UniqueID;

            if(!Page.IsPostBack)
            {
                if(Request.QueryString["err"] !=null)
                {
                    if(Request.QueryString["err"].ToString() =="1") lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_USER_REDIRECT; 

                    else if(Request.QueryString["err"].ToString() =="2") lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_SESSION_TIMEOUT; 


                }


                txtUsername.Focus();

                tblLogin.Visible = true; 
                tblPwd.Visible = false;
           
            }
		}

		public string IpAddress()
		{
			string strIpAddress;
			strIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (strIpAddress == null)
			{
				strIpAddress = Request.ServerVariables["REMOTE_ADDR"];
			}
			return strIpAddress;
		}


        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string hashedPassword; 
            hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Value.ToString(),"md5");

            bool b = false; //martelada para poder fechar a classe e depois fazer o response redirect

			string visitorsIP = IpAddress(); 
			if(visitorsIP =="") visitorsIP = "---"; 


            DATA.UtilizadoresBD utilizador = new LabMetro.DATA.UtilizadoresBD(); 
            
			if (utilizador.bAuthenticateUser(txtUsername.Text, hashedPassword,visitorsIP) == true)
            {

                    FormsAuthentication.RedirectFromLoginPage(txtUsername.Text,false);
          
                    Session["HTPermissions"] = utilizador.GetPermissions(utilizador.ProfileId(txtUsername.Text)); //aqui passar a variavel do perfil

                    Session["UserId"] = utilizador.UserId(txtUsername.Text); 
                    Session["idPerfil"] = utilizador.ProfileId(txtUsername.Text); 
					
					b = true; 
                    //
					
//                }
            }
            else
            {    
				
                lblMessage.Text =  GERAL.clsGeral.ErrorMessage.ERR_LOGIN;    
            }
			
			utilizador = null; 

			string returnUrl = Request.QueryString["ReturnUrl"];
			if (returnUrl == null) returnUrl = "Home.aspx";

			if(b) Response.Redirect(returnUrl,true);

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
        protected void btnChangePwd_Click(object sender, System.EventArgs e)
        {
            string hashedPasswordOld = HashPassword(passwordold.Text.ToString());
            string hashedPasswordNew = HashPassword(passwordnew1.Text.ToString());

            DATA.UtilizadoresBD utilizador = new LabMetro.DATA.UtilizadoresBD(); 
            
            lblMessage.Text=utilizador.ChangePassword(txtUserNamePwd.Text,hashedPasswordOld,hashedPasswordNew); 
            tblLogin.Visible=true;
            tblPwd.Visible=false; 

			utilizador = null; 
        }

        protected void btnPassword_Click(object sender, System.EventArgs e)
        {
           tblLogin.Visible = false; 
           tblPwd.Visible = true; 
        }

       

    }
}
