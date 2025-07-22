namespace LabMetro.INCLUDES
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    
    

	/// <summary>
	///		Summary description for __Header.
	/// </summary>
	public class HeaderInclude : System.Web.UI.UserControl
	{
        protected System.Web.UI.WebControls.Label lblHeader;
        

		private void Page_Load(object sender, System.EventArgs e)
		{
	        
            //lblHeader.Text = GERAL.clsGeral.strHeaderMessage(); 
            string user = HttpContext.Current.User.Identity.Name.ToString(); 
            
            user= "Utilizador registado: "+  user; 
            lblHeader.Text = createDate() + " <br>" + user +"  "; 
            
		}

        private string createDate()
        {
            DateTime dt = DateTime.Now; 
			
//            string dia =  dt.Day.ToString(); 
//            string year = dt.Year.ToString();
//            
//            
//			
//            //0 sunday 6 saturday; 
//            string[] month = new string[13] {"0", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"}; 
//            
//            //string[] day = new string[7] {"Domingo ", "2a Feira", "3a Feira", "4a Feira", "5a Feira", "6a Feira", "Sábado"}; 
//    
//            string myDate =   dia + " de " + month[dt.Month] + " de "+ year; 
//            return myDate; 
        
			return dt.ToLongDateString();
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Load += new System.EventHandler(Page_Load);

        }
		#endregion
	}
}
