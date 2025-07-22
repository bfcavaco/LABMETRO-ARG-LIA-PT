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
using LabMetro.REPORTS;
using LabMetro.GERAL;
using System.Configuration;
using System.Text.RegularExpressions; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for GestModelos.
	/// </summary>
	public partial class GestModelos : System.Web.UI.Page
	{
        private const string ID_PAG = "MODELOS_1";//NOME DA PAGINA
		
        protected void Page_Load(object sender, System.EventArgs e)

            
        {
           // Page.Form.DefaultFocus = txtSearchMarca.UniqueID;
            Page.Form.DefaultButton = btnSubmit.UniqueID;  

            lblMessage.Text =""; 
            
            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
                Session.Clear();
				Response.Redirect("Default.aspx?err=2",true); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
                    Session.Clear();
                    Response.Redirect("Default.aspx?err=1", true);
				}
                else
                {
                    if(!Page.IsPostBack)
                    {
                        ViewState["sortField"] = "marca, descricao";  //descricao = modelo
                        ViewState["sortDirection"] = "ASC";

                        fillMarcas();     
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
           // InitializeComponent2(); 
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent()
        {
      
             
              DGModelos.ItemDataBound += new DataGridItemEventHandler(DGModelos_ItemDataBound); 
              DGModelos.ItemCommand +=new DataGridCommandEventHandler(DGModelos_ItemCommand);
              btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		

          }
        #endregion


        private void fillMarcas()
        {
            string strSQL = "SELECT [idMarca], [descricao] FROM [Marca] where activo = 1 ORDER BY [descricao]";
            ddMarca.DataSource = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddMarca.DataBind();

        }

        private void BindGrid()
        {
            DATA.MarcasModelosBD modelos = new LabMetro.DATA.MarcasModelosBD();
            string idMarca = ddMarca.SelectedItem.Value;// Request.Form[hfidMarca.UniqueID];
            DataTable DT = modelos.DTModelos(idMarca,txtModelo.Text); 
            
			
			DataView DV = new DataView(DT);

			string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
			DV.Sort = strSort; 
			
			DGModelos.DataSource =DV; 
            DGModelos.DataBind(); 

            if(DT.Rows.Count == 0) lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 

			modelos = null;

            

        }


      
        private void DGModelos_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if(e.CommandName == "Insert")
            {
                if(e.Item.ItemType == ListItemType.Footer)

                {
                   DropDownList ddMarca = (DropDownList)e.Item.FindControl("ddMarcaFooter");
                  

                  //  TextBox txtMarcaFooter = (TextBox)e.Item.FindControl("txtMarcaFooter");
                   // txtMarcaFooter.Text = txtSearchMarca.Text;

                    TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricaoFooter"); 
                  
                    
                    if(txtDescricao.Text =="") 
                    {
                        lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
                    }
                    else
                    {
                         DATA.MarcasModelosBD modelos = new LabMetro.DATA.MarcasModelosBD();
                         string idMarca = //Request.Form[hfidMarca.UniqueID];
                         lblMessage.Text = modelos.InsertModelos(ddMarca.SelectedValue, txtDescricao.Text, "1", User.Identity.Name.ToString()); 

                        DGModelos.EditItemIndex = -1;
                        BindGrid(); 
                        DGModelos.ShowFooter=true;

						modelos = null;
                    }
                }
            }
        }

        private void DGModelos_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.EditItem)
            {
                //DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
                //DataRowView DRV = (DataRowView) e.Item.DataItem;
                //string estado  = DRV["activo"].ToString();      
                //if(estado == "True") ddEstado.SelectedValue ="1";
                //else ddEstado.SelectedValue="0";
            }     
            
            if(e.Item.ItemType == ListItemType.Footer)
            {

                //TextBox txtMarcaFooter = (TextBox)e.Item.FindControl("txtMarcaFooter");
                //txtMarcaFooter.Text = txtSearchMarca.Text;

                DropDownList ddMarca = (DropDownList)e.Item.FindControl("ddMarcaFooter");

                DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
                SqlDataReader DR = lista.DRListaMarcas();

                ddMarca.DataSource = DR;
                ddMarca.DataBind();
                DR.Close();

                //if(ddMarca.SelectedValue!="")ddMarca.SelectedValue = ddMarca.SelectedValue; //para a segunda abrir no mesmo da primeira q está na pesquisa la em cima

                lista = null;
               
               
            }
        }

        protected void DGModelos_Edit(Object sender, DataGridCommandEventArgs e)     
        {
            DGModelos.ShowFooter=false;     
            DGModelos.EditItemIndex = e.Item.ItemIndex;	
            BindGrid();
        }

        protected void DGModelos_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            DGModelos.ShowFooter=true;  
            DGModelos.EditItemIndex = -1;
            BindGrid();
        }
		
        protected void DGModelos_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = DGModelos.DataKeys[e.Item.ItemIndex].ToString();
            
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
          
           
            if(txtDescricao.Text =="") 
            {
                lblMessage.Text=GERAL.clsGeral.ErrorMessage.MSG_INDICAR_DESCRICAO; 
            }
            else
            {
                 DATA.MarcasModelosBD modelos = new LabMetro.DATA.MarcasModelosBD(); 

                 lblMessage.Text = modelos.UpdateModelos(id, txtDescricao.Text, "1", User.Identity.Name.ToString()); 

                DGModelos.EditItemIndex = -1;
                BindGrid(); 
                DGModelos.ShowFooter=true; 

				modelos = null;
            }
        }

        

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGModelos.CurrentPageIndex = e.NewPageIndex;
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

       
        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            DGModelos.CurrentPageIndex=0;
            BindGrid(); 
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            trocarModelos(); 
        }

        private void trocarModelos()
        {

            string strIdsApagar = "";
            foreach (DataGridItem dgi in DGModelos.Items)
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

                    strIdsApagar += DGModelos.DataKeys[dgi.ItemIndex].ToString();
                    strIdsApagar += ",";
                }
            }

            strIdsApagar = strIdsApagar.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item

            if (strIdsApagar == "")
            {
                lblMessage.Text = "Tem de apagar um modelo.";
                return;
            } 


            string strIdManter = "";
            foreach (DataGridItem dgi in DGModelos.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("checkboxManter");

                if (myCheckBox.Checked == true)
                {

                    strIdManter = DGModelos.DataKeys[dgi.ItemIndex].ToString();
                    break;
                   
                }
            }

            if (strIdManter == "")
            {
                lblMessage.Text = "Tem de manter um modelo.";
                return;
            } 
            
            lblMessage.Text = "apagar " + strIdsApagar + "- " + "manter " + strIdManter;
            TrocarModelosBD(strIdsApagar, strIdManter); 
        
        }

        
        private void TrocarModelosBD(string apagar, string manter)
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


                       /// string idMarca = Request.Form[hfidMarca.UniqueID];
                        objCmd.CommandText = "update equipamento set idModelo = '" + manter + "' where equipamento.idModelo IN (" + apagar + ") and equipamento.idmarca =  " + ddMarca.SelectedValue; //idMarca; 
                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "delete from modelo where idModelo in ("+apagar+")"; 
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
        protected void ddMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGModelos.CurrentPageIndex = 0; 
            BindGrid();
        }

		
    }
}
