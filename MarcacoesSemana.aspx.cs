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
using System.Data.SqlClient; 
using System.Configuration;
using LabMetro.REPORTS; 
using LabMetro.GERAL; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for MarcacoesSemana.
	/// </summary>
	public partial class MarcacoesSemana : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				ViewState["sortDirection"]="ASC"; 
				ViewState["sortField"] = "Date";
				fillDDfuncionarios(); 

				txtStart.Text=DateTime.Today.ToShortDateString();
				txtEnd.Text = DateTime.Today.AddDays(7).ToShortDateString();

				fillAgendaSemana(); 
			}
		}


		private DataTable dtMarcacoesPorDataFuncionario()
		{
				SqlParameter[] arrParams = new SqlParameter[4];

				arrParams[0] = new SqlParameter("@inDateStart", txtStart.Text);//DateTime.Today.ToShortDateString());
				arrParams[1] = new SqlParameter("@inDateEnd",txtEnd.Text); //DateTime.Today.AddDays(7).ToShortDateString());
				arrParams[2] = new SqlParameter("@inIdFuncionario", ddTecnicoExterior.SelectedValue);
			arrParams[3] = new SqlParameter("@inbGeral",null);

				return GERAL.clsDataAccess.SPExecuteDTParams("stpGetListMarcacoesBetweenDates",arrParams); 
 
		}
	
		//======================================================================================
		//
		//======================================================================================
		private void fillAgendaSemana()
		{

			string sHoje = DateTime.Today.ToShortDateString(); 
			//Response.Write(sHoje+"<br />"); 
            
			try
			{
				DataTable dt = dtMarcacoesPorDataFuncionario(); 
				DataView dv = new DataView(dt); 
				dv.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
				DataGrid1.DataSource = dv; 
				DataGrid1.DataBind(); 
		
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString()); 
			}
		}


		//======================================================================================
		//preenche a dropdown com os funcionarios marcados como sendo de "calibracao externa".
		//======================================================================================
		private void fillDDfuncionarios()
		{
			string strSQL = "select idFuncionario, nomeAbreviado from funcionario where activo = 1 order by nomeAbreviado";
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddTecnicoExterior.DataSource = dr;
			ddTecnicoExterior.DataBind(); 
			ddTecnicoExterior.Items.Insert(0, new ListItem("",""));
            dr.Close();
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

		//=================================================================================================
		//PAGING 
		//=================================================================================================
		public void doPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DataGrid1.CurrentPageIndex = e.NewPageIndex;
			fillAgendaSemana(); 

		}   

		//=================================================================================================
		//SORTGRID 
		//=================================================================================================
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
	
			fillAgendaSemana(); 

		}

		protected void ddTecnicoExterior_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillAgendaSemana();
		}


		


		protected string diaSemana(int iDia)
		{
			//string[] dia = new string[6] {"0", "2aFeira", "3aFeira", "4aFeira", "5aFeira", "6aFeira"}; 
			string[] dia = new string[8] {"0", "2a", "3a", "4a", "5a", "6a","Sáb","Dom" }; 
			return dia[iDia]; 
			
		}



		protected void dg_databound(object sender, DataGridItemEventArgs e)
		{

			DataRowView DRV = (DataRowView) e.Item.DataItem;

			if(e.Item.ItemType == ListItemType.Item ||e.Item.ItemType == ListItemType.AlternatingItem)

			{
				HyperLink linkBRE = (System.Web.UI.WebControls.HyperLink)e.Item.Cells[10].Controls[0]; 
				
				
				if(DRV["bdefinitivo"].ToString() =="False")
				{
					
					linkBRE.NavigateUrl="FormBreCalibExt.aspx?btn=DOC&id="+DRV["idBRE"]; 
				}
				else
				{
					linkBRE.NavigateUrl="FormBre.aspx?btn=DOC&id="+DRV["idBRE"]; 
				}
			}
		}

		protected void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			fillAgendaSemana(); 
		}



		protected string ConverteEstado(bool b)
		{
			if (b==true) return "X";
			else return ""; 
		}


		//=================================================================================================
		// DEVOLVE O CAMINHO PARA UMA REQUISICAO
		//=================================================================================================
		public string downloadpath(object filename)
		{
			if(filename!=null && filename.ToString()!="")
			{
				string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_URL"];
                
				myPath = myPath + "/" + filename.ToString(); 
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}
	
		private void BindGridEmpresasManutencao(string idMarcacao)
		{
			SqlParameter[] arrParams = new SqlParameter[1]; 
			arrParams[0] = new SqlParameter("@inIdMarcacao", idMarcacao);
			DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresaManutencaoByIdMarcacao", arrParams); 
			dgEmpresasManutencao.DataSource = dt; 
			dgEmpresasManutencao.DataBind(); 
		}

		private void DataGrid1_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{	
			if (e.Item.ItemIndex > -1)
			{
				string idMarcacao = DataGrid1.DataKeys[e.Item.ItemIndex].ToString(); 

				switch(e.CommandName.ToString())
				{
					case "verDadosEmpresaManutencao":
						BindGridEmpresasManutencao(idMarcacao); 
						break; 
					case "verOrcamentos":

						verOrcamentos(e.CommandArgument.ToString()); 
						break;
		
				}
			}
		}

		private void verOrcamentos(string idOrcamento)
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

		protected bool ConverteEstadoBotao(object o)
		{
			if(Convert.IsDBNull(o))
			{
				return false;
			}
			return true;
		}

		protected void btnReport_Click(object sender, System.EventArgs e)
		{
		
		}
	}

}
