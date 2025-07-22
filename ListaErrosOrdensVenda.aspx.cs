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
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration; 



namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaErrosOrdensVenda.
	/// </summary>
	public partial class ListaErrosOrdensVenda : System.Web.UI.Page
	{
		
		private const string ID_PAG = "LOGS_SAP_0";//NOME DA PAGINA
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(!ht.ContainsKey(ID_PAG))
			{
				Server.Transfer("Default.aspx?err=1",false);
			}
			else
			{
				if(!Page.IsPostBack)
				{
					WriteSchemaIni(); 
					ViewState["sortDirection"]="ASC";
					ViewState["sortField"]="nomeFicheiro"; 
					fillDDFicheiros(); 
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
			ddFicheiros.SelectedIndexChanged += new System.EventHandler(ddFicheiros_SelectedIndexChanged);
			
		}
		#endregion


		private void fillDDFicheiros()
		{
			
				//' --- Directory Example ----
				string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_LOGS_REL"];     
			
				string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro); 
				//IMPORTANTE, NAO APAGAR!

				//The differences between the two classes is the level of information it returns and how it is used. The Directory and File classes are static classes, meaning that you don't have to create an instance of the class in order to use its methods. These classes are useful if you want to quickly perform some directory-related function. For example, to delete a file, you can use File.Delete(filePath); to determine if a directory exists, you can use Directory.Exists(directoryPath
				//As you can see, the Directory.GetFiles() method accepts one or two parameters. You must specify the path of the directory whose files you want to get, and you may optionally specify a wildcard path (like *.aspx). This method returns a String array, which contains the filenames in the directory (that match the wildcard expression, if supplied). The DirectoryInfo.GetFiles() method doesn't accept a directory path input since the directory's path is already known. Unlike the Directory.GetFiles() method, the DirectoryInfo.GetFiles() method returns an array of FileInfo objects, not Strings. 

				/*Displaying a Directory's Files in a DataGrid
				To display a directory's files in a DataGrid (or DataList or Repeater) all we need to do is assign the String array or FileInfo array to the DataGrid's DataSource property and then call the DataGrid's DataBind() method. For this example, we'll use the DirectoryInfo.GetFiles() method instead of the Directory.GetFiles() method. If we opted to use the Directory.GetFiles() method then all we'd be able to show in the DataGrid is the file's name. By using the DirectoryInfo.GetFiles() method instead, we can display other attributes of the file, such as its size, last modified date, and so on. */

				DirectoryInfo dirInfo = new DirectoryInfo(path); 
				FileInfo[] files = dirInfo.GetFiles("log_*"); 
				IComparer comp = new LabMetro.GERAL.CompareFileInfo(); 
				Array.Sort(files, comp); 
				
			if(files.Length>0)
			{
				ddFicheiros.DataSource = files;
				ddFicheiros.DataBind(); 
				
				BindGrid(); 
			}
			else
			{
				lblMessage.Text= "Não existem logs para visualizar"; 
			}

		}
		//===========================================================================
		//tem de dentro do schema.ini uma definição para CADA ficheiro de texto
		//tenho de apagar o schema.ini existente e criar um novo antes de ler os ficheiros
		//===========================================================================
		private void WriteSchemaIni()
		{
			//nome do ficheiro
			string strFileName= "Schema.ini";
			
			string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_LOGS_REL"];     

			string file = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName); 
			
			StreamWriter sw = new StreamWriter(file,false,System.Text.Encoding.GetEncoding(1252)); //false na segunda posição faz overwrite ao ficheiro já existente

			string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro); 
			
			DirectoryInfo dir = new DirectoryInfo(path); 
			FileInfo[] files = dir.GetFiles("*.txt");
			Array.Sort(files, new GERAL.CompareFileInfo()); 
			
			foreach(FileInfo f in files)
			{ 
				//isto tem de ser feito para cada ficheiro existente na directoria

				// Use the MaxScanRows option to indicate how many rows should be scanned when determining the column types. If you set MaxScanRows to 0, the entire file is scanned. The MaxScanRows setting in Schema.ini overrides the setting in the Windows Registry on a file-by-file basis.

				//The ColNameHeader indicates that Microsoft Jet should use the data in the first row of the table to determine field names and should examine the entire file to determine the data types used:


				sw.WriteLine("["+f.Name+"]"); 
				sw.WriteLine("ColNameHeader=FALSE"); 
				sw.WriteLine("Format=TabDelimited"); 
				//sw.WriteLine("MaxScanRows=0");
				sw.WriteLine("CharacterSet=ANSI");
				sw.WriteLine("Col1=tipoErro Char Width 1");
				sw.WriteLine("Col2=nomeFicheiro Char Width 128 ");
				sw.WriteLine("Col3=numSapFicheiro Char Width 10");
				sw.WriteLine("Col4=idMensagem Char Width 20");
				sw.WriteLine("Col5=numMensagem Char Width 3");
				sw.WriteLine("Col6=variavel1 Char Width 50");
				sw.WriteLine("Col7=variavel2 Char Width 50");
				sw.WriteLine("Col8=variavel3 Char Width 50");
				sw.WriteLine("Col9=variavel4 Char Width 50 ");
				sw.WriteLine("Col10=descricaoErro Char Width 273");
				
			}
			sw.Close(); 
		}

		


		
		private DataTable DTLogs()
		{
			string strSQL = "SELECT  * FROM " + ddFicheiros.SelectedValue; 
//			strSQL+= " ORDER BY 1,2"; 

			return GERAL.clsDataAccess.csvSAPLOGSExecuteDT(strSQL); 
		}

//Inacabado, queria fazer um merge entre as facturas e os logs
		private DataTable DTFacturas()
		{
			string strSQL = "SELECT idFactura, dbo.udfGetReferenciaFactura(idFactura) AS refFactura, CASE WHEN intVersaoFicheiro = 0 THEN '---' ELSE (cast(idFactura as varchar)+'_'+ cast(intVersaoFicheiro as varchar)+'.txt') END as nomeFicheiro FROM Factura WHERE dtFactura > '01-01-2007'"; 

			//Response.Write(strSQL); 
			return GERAL.clsDataAccess.ExecuteDT(strSQL); 

		}

//nao dá para fazer merge pois pode haver varias vezes o nome do ficheiro num log, logo nao seria unique, para fazer de chave no merge
//		private DataSet  DSmergeTables()
//		{
//			try
//			{
//				DataSet ds = new DataSet("myDataSet");
//				DataTable dtFacturas = DTFacturas(); 
//
//				ds.Tables.Add(dtFacturas);
//
//				DataColumn[] dcPk_ = new DataColumn[1];
//				dcPk_[0] = dtFacturas.Columns[2];//NomeFicheiro da tabela factura
//				dtFacturas.PrimaryKey = dcPk_;
//				dtFacturas.DefaultView.Sort = "idFactura";
//
//				ds.AcceptChanges(); 
//
//
//				DataTable dtLogs= DTLogs(); 
//
//				DataColumn[] dcPk = new DataColumn[1];
//				dcPk[0] = dtLogs.Columns[1]; //nomeficheiro do ficheiro texto log de erros
//				dtLogs.PrimaryKey = dcPk;
//				dtLogs.DefaultView.Sort = "nomeFicheiro";
//
//				ds.Merge(dtLogs,false,MissingSchemaAction.Add);
//
//				return ds; 
//			}
//			catch(Exception ex)
//			{
//				Response.Write(ex.ToString()); 
//				return null; 
//			
//			}
//	
//
//		}

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

		private void ddFicheiros_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindGrid();
		}		

		protected string ConverteID(string nomeFicheiro)
		{
			
			string idFactura;
			int i = nomeFicheiro.IndexOf("_"); 
			int fLen = nomeFicheiro.Length; 
			
			if(i<=0) return "" ; 
			idFactura = nomeFicheiro.Substring(0,i); 
			string link ="FormFactura.aspx?btn=EMP&id="+idFactura; 
			return link; 

		}

		private void BindGrid()
		{
			try
			{
				//string strSQL = "SELECT  nomeFicheiro, descricaoErro, left(nomeficheiro,CHARINDEX('_', nomeficheiro)-1) as idFactura FROM " + ddFicheiros.SelectedValue; isto so funciona em sql
				
				string strSQL = "SELECT  nomeFicheiro, descricaoErro FROM " + ddFicheiros.SelectedValue; 
				DataTable dt = GERAL.clsDataAccess.csvSAPLOGSExecuteDT(strSQL); 
				DataView dv = new DataView(dt); 
				dv.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
				
				DG.DataSource = dv;
				DG.DataBind(); 
			}
			catch(Exception ex)
			{
				lblMessage.Text="Erro na visualização do ficheiro.";
				GERAL.clsWriteError.WriteLog(ex); 
			}

		}


//		private void DemonstrateMergeTableAddSchema()
//		{
//			// Create a DataSet with one table, two columns, and ten rows.
//			DataSet ds = new DataSet("myDataSet");
//			DataTable t = new DataTable("Items");
//
//			// Add table to the DataSet
//			ds.Tables.Add(t);
//
//			// Create and add two columns to the DataTable
//			DataColumn c1 = new DataColumn("id", Type.GetType("System.Int32"),"");
//			c1.AutoIncrement=true;
//			DataColumn c2 = new DataColumn("Item", Type.GetType("System.Int32"),"");
//			t.Columns.Add(c1);
//			t.Columns.Add(c2);
//
//			// Set the primary key to the first column.
//			t.PrimaryKey = new DataColumn[1]{ c1 };
//
//			// Add RowChanged event handler for the table.
//			t.RowChanged+= new DataRowChangeEventHandler(Row_Changed);
//
//			// Add ten rows.
//			for(int i = 0; i <10;i++)
//			{
//				DataRow r=t.NewRow();
//				r["Item"]= i;
//				t.Rows.Add(r);
//			}
//
//			// Accept changes.
//			ds.AcceptChanges();
//			PrintValues(ds, "Original values");
//
//			// Create a second DataTable identical to the first, with
//			// one extra column using the Clone method.
//			DataTable t2 = t.Clone();
//			t2.Columns.Add("extra", typeof(string));
//
//			// Add two rows. Note that the id column can't be the 
//			// same as existing rows in the DataSet table.
//			DataRow newRow;
//			newRow=t2.NewRow();
//			newRow["id"]= 12;
//			newRow["Item"]=555;
//			newRow["extra"]= "extra Column 1";
//			t2.Rows.Add(newRow);
//
//			newRow=t2.NewRow();
//			newRow["id"]= 13;
//			newRow["Item"]=665;
//			newRow["extra"]= "extra Column 2";
//			t2.Rows.Add(newRow);
//
//			// Merge the table into the DataSet.
//			Console.WriteLine("Merging");
//			ds.Merge(t2,false,MissingSchemaAction.Add);
//			PrintValues(ds, "Merged With Table, Schema Added");
//
//
//		}
	}
}
