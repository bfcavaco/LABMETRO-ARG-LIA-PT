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
using LabMetro.REPORTS;

namespace LabMetro
{
	public partial class ListaOrcamentosTecnicos : System.Web.UI.Page
	{

//        private const string ID_PAG = "ORCAMENTOSTECNICOS_0";//NOME DA PAGINA
		        private const string ID_PAG = "ORCAMENTOSSEMPRECO_0";//NOME DA PAGINA

    
		protected void Page_Load(object sender, System.EventArgs e)
		{

            Page.Form.DefaultButton = btnPesquisa.UniqueID;

            
            lblMessage.Text = "";
            
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
                        ViewState["sortField"] = "idOrcamento";
                        ViewState["sortDirection"] = "DESC";
                        
                        //BindGrid(); 

                    }
                    
                }
            }
        }
    

        private void BindGrid()
        {
            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD(); 
            
			DataTable dt = orcamento.DTOrcamentos(txtEmpresa.Text, txtTipoEquipamento.Text, null, txtRefOrcamento.Text,"",null); 

			DataView DV = new DataView(dt);
	
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
            
            DG.DataSource = DV;
            DG.DataBind(); 

			orcamento = null; 

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

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DG.CurrentPageIndex = e.NewPageIndex;
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
			DG.ItemCommand +=new DataGridCommandEventHandler(dgItemCommand);

		}
		#endregion

        protected void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DG.CurrentPageIndex=0; 
            BindGrid(); 
        }

		private void dgItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{	
			
				 
			if (e.Item.ItemIndex > -1)
			{
				
				string idOrcamento = DG.DataKeys[e.Item.ItemIndex].ToString();

				if(e.CommandName.ToString()=="verOrcamento")
				{
					verReportOrcamento(idOrcamento);
				
				}
			}
		}


		private void verReportOrcamento(string idOrcamento)
		{
			rptOrcamentoSemPreco report = new rptOrcamentoSemPreco(); 
			clsReport cr = new clsReport();
		
			string faxNumber = "---"; 
			
			

			DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD(); 
			DataSet ds  = orc.DSOrcamFax(idOrcamento); 	
			report.SetDataSource(ds);
            report.SetParameterValue("@inFaxNumber", faxNumber);
            ds = null; 
            cr.exportReportToPDF(report,"Orcamento"); 
			
		}
	}
}
