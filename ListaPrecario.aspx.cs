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
using System.Configuration;
using System.Data.SqlClient; 
using System.Text; 
using System.IO; 
using System.Threading;
using System.Globalization; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaPrecario.
	/// </summary>
	public partial class ListaPrecario : System.Web.UI.Page
	{
        //isto pode ficar pq isto é sempre igual para todos.
        private static DataTable dt; 
        private static DataView dv; 
        private const string ID_PAG = "PRECARIO_0";//NOME DA PAGINA
        
        
        string space;

	
        protected void Page_Load(object sender, System.EventArgs e)
        {

            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				//Response.Redirect("Default.aspx?err=2",true);            
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					//Response.Redirect("Default.aspx?err=1",true);//user n.tem permissoes                             
					Server.Transfer("Default.aspx?err=1",false);
				}
                else
                {
                    //=====================================================================
                    //PAGELOAD=============================================================
                    if(!Page.IsPostBack)
                    
                    {    
                        dt = new DataTable(); 
                          

                        DataColumn c1 = new DataColumn("ID", Type.GetType("System.String"),"");
                        DataColumn c2 = new DataColumn("IDPAI", Type.GetType("System.String"),"");
                        DataColumn c3 = new DataColumn("DESC", Type.GetType("System.String"),"");

                        DataColumn c4 = new DataColumn("IDEQUIPAMENTO", Type.GetType("System.String"),"");
                        DataColumn c5 = new DataColumn("IDSERVICO", Type.GetType("System.String"),"");
                        DataColumn c6 = new DataColumn("PRECO", Type.GetType("System.String"),"");
                        DataColumn c7 = new DataColumn("PRECOMOVEL", Type.GetType("System.String"),"");
                        DataColumn c8 = new DataColumn("PRECOEXTERIOR", Type.GetType("System.String"),"");
                        DataColumn c9 = new DataColumn("TIPOEQUIPAMENTO", Type.GetType("System.String"),"");
        	         
                        dt.Columns.Add(c1);
                        dt.Columns.Add(c2);
                        dt.Columns.Add(c3);
                        dt.Columns.Add(c4);
                        dt.Columns.Add(c5);
                        dt.Columns.Add(c6);
                        dt.Columns.Add(c7);
                        dt.Columns.Add(c8);
                        dt.Columns.Add(c9);
                        
                        CreateDataSource(0);
                                       
                        DataGrid1.DataSource = dt;
                        DataGrid1.DataBind();
                            
                        dv = new DataView(dt); 

                        //na primeira vez q se entra, esconde-se tudo menos os itens do nivel superior
                        foreach(DataGridItem x in DataGrid1.Items)
                        {
                            Button btn = (Button)(x.Cells[0].Controls[0]);
                            btn.CssClass="button"; 

                            if(x.Cells[4].Text =="0") 
                            {
                                x.Visible=true;     
                                
                                x.Cells[2].Font.Bold=true; 
                            }
                            else 
                            {
                                x.Visible=false; 
                            }
                        }
                    }
                    //=====================================================================
                    //PAGELOAD=============================================================
                }
            }
        }

        int IDPai = 0;

        private void CreateDataSource(int intID)
        {
            
            string strSQL; 
            strSQL = "SELECT dbo.TipoEquipamento.descricao AS tipoEquipamento, dbo.TipoServico.descricao AS tipoServico, dbo.Precario.idPrecario, dbo.Precario.idPrecarioPai, dbo.Precario.idTipoEquipamento, dbo.Precario.idTipoServico, dbo.Precario.descricao, dbo.Precario.preco, dbo.Precario.precoMovel, dbo.Precario.precoExterior FROM  dbo.Precario INNER JOIN dbo.TipoEquipamento ON dbo.Precario.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.TipoServico ON dbo.Precario.idTipoServico = dbo.TipoServico.idTipoServico"; 
            DataTable DTPrecario = GERAL.clsDataAccess.ExecuteDT(strSQL); 

            DataView DVPrecario = new DataView(DTPrecario); 

            if(intID ==0)
            {
                DVPrecario.RowFilter = "idPrecarioPai is null"; 
            }
            else if(intID != 0)
            {
                DVPrecario.RowFilter = "idPrecarioPai = " + intID; 
            }
            
            foreach(DataRowView drv in DVPrecario)
            {
          
                DataRow newRow;
                newRow=dt.NewRow();

                newRow["ID"]= drv["idPrecario"].ToString(); 
                newRow["DESC"]=space + drv["descricao"].ToString();

                if(drv["idPrecarioPai"].ToString() == "") IDPai = 0; 
                else IDPai = Convert.ToInt32(drv["idPrecarioPai"]);
                newRow["IDPAI"]= IDPai.ToString();  

                newRow["IDEQUIPAMENTO"]= drv["idTipoEquipamento"].ToString();
                newRow["IDSERVICO"]=drv["idTipoServico"].ToString();                
                newRow["PRECO"]= GERAL.clsGeral.ConvertDBMoneyToCurrencyString(drv["preco"]); 
                newRow["PRECOMOVEL"]= GERAL.clsGeral.ConvertDBMoneyToCurrencyString(drv["precoMovel"]);
                newRow["PRECOEXTERIOR"]=  GERAL.clsGeral.ConvertDBMoneyToCurrencyString(drv["precoExterior"]); 

                newRow["TIPOEQUIPAMENTO"]= drv["tipoEquipamento"].ToString(); 

                dt.Rows.Add(newRow);

                space = space + "&nbsp;&nbsp;&nbsp;&nbsp;";

                CreateDataSource(Convert.ToInt32(drv["idPrecario"]));

                if (space.Length - 24 >= 24) 
                    space = space.Remove(space.Length - 24, 24);
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
            DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(DataGrid1_ItemCommand);

        }
        #endregion

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
		
            if( ((Button)(e.Item.Cells[0].Controls[0])).Text == "+")
            {
                // condition for node opening
                //int Root = Convert.ToInt32(DataGrid1.Items[e.Item.ItemIndex].Cells[1].Text);
                int Root = Convert.ToInt32(DataGrid1.DataKeys[e.Item.ItemIndex].ToString());
                for(int i = 1 ;i< DataGrid1.Items.Count ;i++)
                {
                 
                    if (DataGrid1.Items[i].Cells[4].Text == Root.ToString()) 
                        DataGrid1.Items[i].Visible = true;
                }

                // changing the text of Push Button to looks like opened tree node
                ((Button)(e.Item.Cells[0].Controls[0])).Text = "-";
            }
            else
            {
                // condition for tree closing
                int Root = Convert.ToInt32(DataGrid1.DataKeys[e.Item.ItemIndex].ToString());

                // all the chid of the root is stored in this arraylist object
                ArrayList ArrC = new ArrayList();
                ArrC.Add(Root);

                // this loop will exit when the iteration through all the child of the selected node is over
                do
                {
                    for(int i = 1 ;i< DataGrid1.Items.Count;i++)
                    {
                       

                        if (DataGrid1.Items[i].Cells[4].Text == ArrC[0].ToString())
                        {
                            ArrC.Add(DataGrid1.Items[i].Cells[1].Text);
                            DataGrid1.Items[i].Visible = false;
                            ((Button)(DataGrid1.Items[i].Cells[0].Controls[0])).Text = "+";
                        }
                    }

                    // removing the executed node form the arraylist
                    ArrC.Remove(ArrC[0]);
                }while (ArrC.Count > 0);

                //after closing all the child node putting back the text back to orginal value
                ((Button)(e.Item.Cells[0].Controls[0])).Text = "+";
            }
        }


        protected void btnExport_Click(object sender, System.EventArgs e)
        {           
            DataGrid objdatagrid = new DataGrid(); 
        
            objdatagrid.DataSource = dt; 
            objdatagrid.DataBind(); 
        
            DataGridToExcel(objdatagrid, Response); 
           
        }

        private void DataGridToExcel(DataGrid dgExport,HttpResponse Response)
        {
         
        //clean up the Response.object
        Response.Clear(); 
        Response.Charset = ""; 
        //set the Response mime type for excel
        Response.ContentType = "application/vnd.ms-excel"; 
        //create a string writer
        System.IO.StringWriter stringWrite = new StringWriter(); 
        //create an htmltextwriter which uses the stringwriter
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);  

        //instantiate a datagrid
        DataGrid dg = new DataGrid(); 
        // just set the input datagrid = to the new dg grid
        dg = dgExport; 

        // I want to make sure there are no annoying gridlines
        dg.GridLines = GridLines.Both; 
        // Make the header text bold
        dg.HeaderStyle.Font.Bold = true; 
        dg.HeaderStyle.BorderColor = System.Drawing.Color.Gray; 
        dg.HeaderStyle.BackColor = System.Drawing.Color.Beige; 

        // If needed, here//s how to change colors/formatting at the component level
        dg.HeaderStyle.ForeColor = System.Drawing.Color.Black; 
        dg.ItemStyle.ForeColor = System.Drawing.Color.Black; 

        //bind the modified datagrid
        dg.DataBind(); 
        
        //tell the datagrid to render itself to our htmltextwriter
        dg.RenderControl(htmlWrite); 
        //output the html
        
        Response.AddHeader("content-disposition", "attachment; filename=Precario.xls");
        Response.Write(stringWrite.ToString()); 
        Response.End(); 


            
    
        }
      
        
     }
}
