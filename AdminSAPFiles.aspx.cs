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
using System.IO; 
using System.Configuration; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for AdminSAPFiles.
	/// </summary>
	public partial class AdminSAPFiles : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{


			GERAL.clsHandleFiles.WriteSchemaIni(); 
			if(!Page.IsPostBack)
			{
				ViewState["sortDirection"]="ASC";
				ViewState["sortField"]="nome1";
				BindGrid(); 

			}
			
			BindGrid(); 
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
		
		private void BindGrid()
		{	
			//FileInfo myFile = new FileInfo(filePath); 
			// myFile.Delete(); 

			//Getting a List of Files in a Directory
			//Both the Directory and DirectoryInfo classes contain a method to get all of the files in a directory (or to get all of the files in a directory matching some wildcard expression, like *.htm) - this method is called GetFiles() and is used as follows: 

			//' --- Directory Example ----
			string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_PATH_REL_DM"];     
			
			string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro); //+ "/" + strFileName); 


 

			//IMPORTANTE, NAO APAGAR!

			//The differences between the two classes is the level of information it returns and how it is used. The Directory and File classes are static classes, meaning that you don't have to create an instance of the class in order to use its methods. These classes are useful if you want to quickly perform some directory-related function. For example, to delete a file, you can use File.Delete(filePath); to determine if a directory exists, you can use Directory.Exists(directoryPath
			//As you can see, the Directory.GetFiles() method accepts one or two parameters. You must specify the path of the directory whose files you want to get, and you may optionally specify a wildcard path (like *.aspx). This method returns a String array, which contains the filenames in the directory (that match the wildcard expression, if supplied). The DirectoryInfo.GetFiles() method doesn't accept a directory path input since the directory's path is already known. Unlike the Directory.GetFiles() method, the DirectoryInfo.GetFiles() method returns an array of FileInfo objects, not Strings. 

			/*Displaying a Directory's Files in a DataGrid
			To display a directory's files in a DataGrid (or DataList or Repeater) all we need to do is assign the String array or FileInfo array to the DataGrid's DataSource property and then call the DataGrid's DataBind() method. For this example, we'll use the DirectoryInfo.GetFiles() method instead of the Directory.GetFiles() method. If we opted to use the Directory.GetFiles() method then all we'd be able to show in the DataGrid is the file's name. By using the DirectoryInfo.GetFiles() method instead, we can display other attributes of the file, such as its size, last modified date, and so on. */

			DirectoryInfo dirInfo = new DirectoryInfo(path); 
			FileInfo[] files = dirInfo.GetFiles("*.txt"); 
			IComparer comp = new LabMetro.GERAL.CompareFileInfo(); 
			Array.Sort(files, comp); 

			dgFiles.DataSource = files; 
			dgFiles.DataBind();
		}
		
		
		private void BindGridDM(string fileName)
		{
			try
			{
				string strSQL = "SELECT  * FROM " + fileName;  
				DataTable dt = GERAL.clsDataAccess.csvSAPExecuteDT(strSQL); 
				DataView dv = new DataView(dt); 
//				string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
//				dv.Sort = strSort; 
				
				DG.DataSource = dv;
				DG.DataBind(); 
			}
			catch(Exception ex)
			{
                Response.Write(ex.ToString() + "<br>");

                Response.Write(ex.Message.ToString()+"<br>");
                Response.Write(ex.Source.ToString() + "<br>");
     
          
			}
		}

		protected void dgFiles_ItemCommand(object sender, DataGridCommandEventArgs e)    
		{     
			if(e.CommandName=="fillData")
			{
			
				 BindGridDM(dgFiles.DataKeys[e.Item.ItemIndex].ToString()); 
			}		
		}

		public void sortGrid(Object s, DataGridSortCommandEventArgs e)
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

		public string downloadpath(object filename)
		{
			if(filename!=null && filename.ToString()!="")
			{
				string myPath = (string)ConfigurationManager.AppSettings["SAP_DM_URL"];   
				myPath = myPath + "/" + filename.ToString(); 
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			GERAL.clsHandleFiles handle = new LabMetro.GERAL.clsHandleFiles(); 	
			try
			{
				handle.UpdateEmpresasSAP(); 				
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString()); 
			}
			handle = null; 
		}
	}
}
