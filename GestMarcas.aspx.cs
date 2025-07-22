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

using LabMetro.GERAL;
using System.Configuration;


namespace LabMetro
{
	/// <summary>
	/// Summary description for GestMarcas.
	/// </summary>
	public partial class GestMarcas : System.Web.UI.Page
	{
        private const string ID_PAG = "MARCAS_1";//NOME DA PAGINA

    
		
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
                        ViewState["sortField"] = "descricao";
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
			btnSearch.Click += new System.EventHandler(btnSearch_Click);
			DGMarcas.ItemDataBound += new DataGridItemEventHandler(DGMarcas_ItemDataBound); 
            DGMarcas.ItemCommand +=new DataGridCommandEventHandler(DGMarcas_ItemCommand);

        }
		#endregion

        private void BindGrid()
        {

			//nao vou mandar o campo de pesquisa como parametro, mas sim aplica-lo à datatable (para ser mais rapido e visto nao receber demasiados registos).

            DATA.MarcasModelosBD marcas = new LabMetro.DATA.MarcasModelosBD(); 

			DataTable DT = marcas.DTMarcas(); 
			DataView DV = new DataView(DT);

			string strRowfilter ="descricao LIKE '"+txtNome.Text+"%'";  

			DV.RowFilter =  strRowfilter;
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
	    
			DGMarcas.DataSource = DV; 
			DGMarcas.DataBind(); 
		

			marcas = null; 
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

        private void DGMarcas_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
				if(e.Item.ItemType == ListItemType.Footer)

                {
                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 
                    
                    DropDownList ddEmpManut = (DropDownList)e.Item.FindControl("ddEmpManutFooter");
					
					if(txtDescricao.Text =="") 
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
                    }
                    else
                    {
                        DATA.MarcasModelosBD marcas = new LabMetro.DATA.MarcasModelosBD();

                        lblMessage.Text = marcas.InsertMarcas(txtDescricao.Text, ddEmpManut.SelectedValue.ToString()) + GERAL.clsGeral.ErrorMessage.MSG_DB;

                        DGMarcas.EditItemIndex = -1;
                        BindGrid(); 
                        DGMarcas.ShowFooter=true; 

						marcas = null; 
                    }
                }
            }
            if (e.CommandName == "verModelos")
            {

                dsModelos.SelectParameters["idMarca"].DefaultValue = DGMarcas.DataKeys[e.Item.ItemIndex].ToString();
                gvGenerico.DataSourceID = "dsModelos";

                gvGenerico.DataBind();


            }

            if (e.CommandName == "verEquipamentos")
            {

                dsEquips.SelectParameters["idMarca"].DefaultValue = DGMarcas.DataKeys[e.Item.ItemIndex].ToString();
                gvGenerico.DataSourceID = "dsEquips";

                gvGenerico.DataBind();


            }
        }

        private void DGMarcas_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

			DATA.MarcasModelosBD d = new LabMetro.DATA.MarcasModelosBD();
            if(e.Item.ItemType == ListItemType.EditItem)
            {
              
                DataRowView DRV = (DataRowView) e.Item.DataItem;
               


				DropDownList ddEmpManut = (DropDownList)e.Item.FindControl("ddEmpManutEdit");

				SqlDataReader DR =  d.DREmpresasManutencao();
				ddEmpManut.DataSource = DR; 
				ddEmpManut.DataBind(); 
				DR.Close(); 
				ddEmpManut.Items.Insert(0,new ListItem("","")); 
				ddEmpManut.SelectedValue  = DRV["idEmpresaManutCTA"].ToString();      

			
            }
			if(e.Item.ItemType == ListItemType.Footer)
			{
				DropDownList ddEmpManut = (DropDownList)e.Item.FindControl("ddEmpManutFooter");
    
				
				SqlDataReader DR =  d.DREmpresasManutencao();
				ddEmpManut.DataSource = DR; 
				ddEmpManut.DataBind(); 
				DR.Close(); 

				ddEmpManut.Items.Insert(0,new ListItem("","")); 

				
               
			}

			d = null;

        }

        protected void DGMarcas_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGMarcas.ShowFooter=false;     
            DGMarcas.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DGMarcas_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGMarcas.ShowFooter=true;  
            DGMarcas.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DGMarcas_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGMarcas.DataKeys[e.Item.ItemIndex].ToString();
            
		    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
           
			DropDownList ddEmpManut = (DropDownList)e.Item.FindControl("ddEmpManutEdit");


            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
            }
            else
            {
                DATA.MarcasModelosBD marcas = new LabMetro.DATA.MarcasModelosBD();

                lblMessage.Text = marcas.UpdateMarcas(id, txtDescricao.Text, "1", ddEmpManut.SelectedValue.ToString()) + GERAL.clsGeral.ErrorMessage.MSG_DB; 

                DGMarcas.EditItemIndex = -1;
                BindGrid(); 
                DGMarcas.ShowFooter=true; 

				marcas = null; 		
        
            }
        }

       
        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGMarcas.CurrentPageIndex = e.NewPageIndex;
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

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
            DGMarcas.CurrentPageIndex = 0;
			BindGrid(); 
		}

        protected void Button1_Click(object sender, EventArgs e)
        {

            trocarMarcas();
        }

        private void trocarMarcas()
        {

            string strIdsApagar = "";
            foreach (DataGridItem dgi in DGMarcas.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("checkboxApagar");
                CheckBox myCheckBoxManter = (CheckBox)dgi.Cells[0].FindControl("checkboxManter");

                if (myCheckBox.Checked == true)
                {
                    if (myCheckBoxManter.Checked == true)
                    {
                        lblMessage.Text = "Não pode apagar e manter ao mesmo tempo.";
                        return;
                    }
                    strIdsApagar += DGMarcas.DataKeys[dgi.ItemIndex].ToString();
                    strIdsApagar += ",";
                }
            }

            strIdsApagar = strIdsApagar.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item
            if (strIdsApagar == "")
            {
                lblMessage.Text = "Tem de apagar uma marca.";
                return;
            } 

            string strIdManter = "";
            foreach (DataGridItem dgi in DGMarcas.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("checkboxManter");

                if (myCheckBox.Checked == true)
                {

                    strIdManter = DGMarcas.DataKeys[dgi.ItemIndex].ToString();
                    break;

                }
            }
            if (strIdManter == "")
            {
                lblMessage.Text = "Tem de manter uma marca.";
                return;
            } 


            lblMessage.Text = "apagar " + strIdsApagar + "- " + "manter " + strIdManter;
            TrocarMarcasBD(strIdsApagar, strIdManter);

        }


        private void TrocarMarcasBD(string apagar, string manter)
        {


            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];

            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                objCmd.Connection = objConn;

                objConn.Open();

                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;
                    objCmd.CommandType = CommandType.Text;
                    try
                    {

                        objCmd.CommandText = "update modelo set idMarca = " + manter + " where idMarca IN (" + apagar + ")";  

                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "update equipamento set idmarca =  " + manter + "  where equipamento.idMarca IN (" + apagar + ") "; 

                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "delete from marca where idMarca in (" + apagar + ")";
                        objCmd.ExecuteNonQuery();
                        objTrans.Commit();
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DB;
                        BindGrid();

                    }

                    catch (Exception ex)
                    {
                        try
                        {
                            objTrans.Rollback();
                        }
                        catch (Exception excep)
                        {
                            GERAL.clsWriteError.WriteLog(excep);
                            lblMessage.Text += excep.Message.ToString() + "<br />";

                        }
                        GERAL.clsWriteError.WriteLog(ex);
                        lblMessage.Text += ex.Message.ToString() + "<br />";

                    }
                }
            }
        }

      
	}
}
