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
using LabMetro.REPORTS;
using LabMetro.GERAL; 
using System.Collections.Specialized;



namespace LabMetro
{
	/// <summary>
	/// Summary description for Home.
	/// </summary>
	public partial class Home : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.TextBox txtLegendaAmarela;
		protected System.Web.UI.WebControls.TextBox txtLegendaLaranja;
		protected System.Web.UI.WebControls.TextBox Textbox1;
		
		DataTable DT;
		DataView DV; 

		protected void Page_Load(object sender, System.EventArgs e)
		{	
			//serverVariables(); 

			if(!Page.IsPostBack)
			{
				ViewState["sortFieldCalib"] = "Dias";
				ViewState["sortDirectionCalib"] = "DESC";
				ViewState["sortFieldActual"] = "Dias";
				ViewState["sortDirectionActual"] = "DESC";
				ViewState["sortFieldFinal"] = "Dias";
				ViewState["sortDirectionFinal"] = "DESC";
				ViewState["sortFieldEmpresa"] = "dtCriacao";
				ViewState["sortDirectionEmpresa"] = "DESC";
				//ViewState["idLab"] = idGrandeza().ToString();
				BindDDLaboratorio(); 
				BindGridEquipsActualizar();
				BindGridEquipsFinalizar();
				BindGridEmpresas(); 
			}
		}


        private void DoSomethingWrong()//para testar o errorhandling
        {
            // raises exception
            throw new Exception("New exception was thrown!");

        }
		
		private string idGrandeza()
		{
			
			string strSQL = "SELECT Laboratorio.idGrandeza FROM Utilizador INNER JOIN Funcionario ON Utilizador.idUtilizador = Funcionario.idUtilizador INNER JOIN Laboratorio ON Funcionario.idLaboratorio = Laboratorio.idLaboratorio INNER JOIN Grandeza ON Laboratorio.idGrandeza = Grandeza.idGrandeza WHERE Utilizador.idUtilizador ="+Session["UserId"];
			return GERAL.clsDataAccess.myExecuteScalar(strSQL).ToString(); //id
			
		}
       
		private void BindDDLaboratorio()
		{
			string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
			SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			
			ddGrandeza.DataSource = dr;
			ddGrandeza.DataBind();
            dr.Close();
			
			try
			{
				ddGrandeza.SelectedValue = idGrandeza().ToString(); 
			}
			catch
			{
			}
			
		}


        private string strSQLCalibrar()
        {

            return "SELECT idServico,refServico,EstadoServico.descricao as EstadoServico, BRE.dtBre ,dtEstado,servico.idBRE, Datediff(day, servico.dtEstado, getDate())as Dias, servico.bdefinitivo , servico.observacoes FROM servico INNER JOIN Grandeza on Servico.idGrandeza = Grandeza.idGrandeza INNER JOIN BRE on Servico.idBRE = BRE.idBre INNER JOIN EstadoServico on servico.idEstadoServico = EstadoServico.idEstadoServico WHERE servico.idEstadoServico IN (1,2,3,4,5)AND servico.idGrandeza = '" + ddGrandeza.SelectedValue + "'  AND Datediff(day, servico.dtEstado, getDate()) > 30   ORDER BY DIAS DESC";
        
        }
		private string strSQLActual()
		{
			
			//estou num estado intermédio.... (foram calibrados, mas nao teem certificado... 
			//entram tb o 25: calib. no ext e o 15: entregue calib (tem de ser actualizado com o certificado correspondente). 
			return "SELECT idServico,refServico,EstadoServico.descricao as EstadoServico, BRE.dtBre ,dtEstado, Datediff(day, servico.dtEstado, getDate())as Dias, servico.observacoes FROM servico INNER JOIN Grandeza on Servico.idGrandeza = Grandeza.idGrandeza INNER JOIN BRE on Servico.idBRE = BRE.idBre INNER JOIN EstadoServico on servico.idEstadoServico = EstadoServico.idEstadoServico WHERE servico.idEstadoServico IN (6,7,9,10,25,15,27)AND servico.idGrandeza = '"+ddGrandeza.SelectedValue+"' AND Datediff(day, servico.dtEstado, getDate()) > 30   ORDER BY DIAS DESC"; 

		}

		private void BindGridEquipsActualizar()
		{
			
			DT = GERAL.clsDataAccess.ExecuteDT(strSQLActual()); 
			try
			{
				DV = new DataView(DT);

				DV.Sort =  ViewState["sortFieldActual"].ToString()+ " " + ViewState["sortDirectionActual"]; 

				dgEquipsActualizar.DataSource = DV; 
				dgEquipsActualizar.DataBind(); 		
			}
			catch
			{
				//Response.Write(ex.ToString()); 
			}
		}



		protected void SortEquipsActualizar(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionActual"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionActual"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionActual"]="ASC";
					break;
			}

			ViewState["sortFieldActual"] = e.SortExpression;
			BindGridEquipsActualizar(); 
		}





		private string strSQLFinal()
		{
			//o 8 mantem-se por algum estado antigo que nao esteja actualizado
			//entram tb o 25: calib. no ext e o 15: entregue calib (tem de ser actualizado com o certificado correspondente). 
			return "SELECT idServico,refServico,EstadoServico.descricao as EstadoServico, BRE.dtBre ,dtEstado, Datediff(day, servico.dtEstado, getDate())as Dias , servico.observacoes FROM servico INNER JOIN Grandeza on Servico.idGrandeza = Grandeza.idGrandeza INNER JOIN BRE on Servico.idBRE = BRE.idBre INNER JOIN EstadoServico on servico.idEstadoServico = EstadoServico.idEstadoServico WHERE servico.idEstadoServico in (13,19, 21,12,18) AND Datediff(day, servico.dtEstado, getDate()) > 30   ORDER BY dtBRE DESC";//todos os estados que nao sao finais e nao sao nenhuns dos primeiros (podia fazer ao contrario, mas não da) 

		}


		private void BindGridEquipsFinalizar()
		{
			
			DT = GERAL.clsDataAccess.ExecuteDT(strSQLFinal()); 
			try
			{
				DV = new DataView(DT);

				DV.Sort =  ViewState["sortFieldFinal"].ToString()+ " " + ViewState["sortDirectionFinal"]; 

				dgFinalizar.DataSource = DV; 
				dgFinalizar.DataBind(); 		
			}
			catch
			{
				
			}
		}



		protected void SortEquipsFinalizar(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionFinal"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionFinal"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionFinal"]="ASC";
					break;
			}

			ViewState["sortFieldFinal"] = e.SortExpression;
			BindGridEquipsFinalizar(); 
		}


		private void ddLaboratorio_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindGridEquipsActualizar(); 
		}


		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSEstadosAlerta(string strSQL)
		{
			LabMetro.DSEstadosAlerta ds = new DSEstadosAlerta(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())

			{
				SqlDataAdapter da = new SqlDataAdapter(objCmd);
			
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.CommandText= strSQL;
				objCmd.Connection = objConn; 
				
				try
				{
					da.Fill(ds,"Servico"); 
				}
				catch
				{	
				}

				da.Dispose();
				da = null; 
			}
			return ds; 
		}


		private void openreport(string strSQL, string title)
		{
			rptEstadosActualizar report =new rptEstadosActualizar(); 
 
			clsReport cr = new clsReport();
           
           
			//missing parameter value error
            //solução: set parameter AFTER SETTING DATASOURCE!
            //setDataSource before SetParameterValue and it will work
            //pode ser mais que isso, mas o primeiro passo é sempre colocar o set parameter dos do datasource
			DataSet ds  = DSEstadosAlerta(strSQL); 
			
			report.SetDataSource(ds);
            report.SetParameterValue("@titulo", title);

            ds = null; 
            cr.exportReportToPDF(report,"Report"); 

			
            //cr = null; 
            //report = null; 
		}

		protected void PageEmpresas(Object s,DataGridPageChangedEventArgs e)
		{
			dgEmpresas.CurrentPageIndex = e.NewPageIndex;
			BindGridEmpresas(); 
		}

		protected void PageEquipsActualizar(Object s,DataGridPageChangedEventArgs e)
		{
			dgEquipsActualizar.CurrentPageIndex = e.NewPageIndex;
			BindGridEquipsActualizar(); 
		}


		public void PageEquipsFinalizar(Object s,DataGridPageChangedEventArgs e)
		{
			dgFinalizar.CurrentPageIndex = e.NewPageIndex;
			BindGridEquipsFinalizar(); 
		}

		protected void SortEmpresas(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionEmpresa"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionEmpresa"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionEmpresa"]="ASC";
					break;
			}

			ViewState["sortFieldEmpresa"] = e.SortExpression;
			BindGridEmpresas(); 
		}


		//bind do datagrid das empresas sem número de cliente
		private void BindGridEmpresas()
		{

			if(!Page.IsPostBack)
			{
				DataSet DS = DSEmpresasSemNumCliente(); 
				DT = DS.Tables[0];
				ViewState["DS"] = DS;
			}
			else
			{
				DataSet DS = (DataSet)ViewState["DS"]; 
				DT=DS.Tables[0];
			}
			
			
			try
			{
				DV = new DataView(DT);
				DV.Sort = ViewState["sortFieldEmpresa"].ToString()+ " " + ViewState["sortDirectionEmpresa"]; 
			
				dgEmpresas.DataSource = DV; 
				dgEmpresas.DataBind(); 		
			}
			catch
			{
				//Response.Write(ex.ToString()); 
			}
		}

		public DataSet DSEmpresasSemNumCliente()
		{
			LabMetro.DSEmpresasSemNumCliente ds = new DSEmpresasSemNumCliente(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				SqlDataAdapter da = new SqlDataAdapter(objCmd);

				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				//isto em stored procedura encrava nao sei porque!!! e demora 3 minutos a executar
				//no query analyzer 2 segundos e inline aqui tb rapido
				//o union é necessário para que seja mais rapido.
				objCmd.CommandText= "SELECT DISTINCT dbo.Empresa.idEmpresa, dbo.Empresa.nome, dbo.Empresa.nomeAbreviado,cp.descCodigoCondPagam, dbo.Empresa.nif, dbo.Empresa.numClienteSAP, dbo.Empresa.dtCriacao,dbo.Funcionario.nomeAbreviado AS UtilCriacao, Empresa.codigoBloqueioSAP FROM servico INNER JOIN equipamento ON servico.idEquipamento = equipamento.idEquipamento INNER JOIN empresa ON equipamento.idEmpresa = Empresa.idEmpresa INNER JOIN Funcionario on empresa.idUtilCriacao = Funcionario.idUtilizador left join sap_codigoCondPagam cp on empresa.idCondicoesPagamento = cp.idCodigoCondPagam LEFT JOIN Sap_Empresas ON CAST(Empresa.numClienteSAP as int) = cast( sap_Empresas.codigoClienteSAP as int) WHERE  empresa.idCondicoesPagamento <> 1 and  servico.idFactura IS NULL AND servico.idEstadoServico not in (-1,21,22,20) AND ( Empresa.numClienteSAP is null OR Empresa.numClienteSAP = '' OR Empresa.codigoBloqueioSAP = '03' ) UNION select dbo.Empresa.idEmpresa, dbo.Empresa.nome, cp.descCodigoCondPagam,dbo.Empresa.nomeAbreviado, dbo.Empresa.nif, dbo.Empresa.numClienteSAP, dbo.Empresa.dtCriacao,dbo.Funcionario.nome AS UtilCriacao, Empresa.codigoBloqueioSAP    from empresa inner join equipamento ON Equipamento.idEmpresa = Empresa.idEmpresa inner join servico ON servico.idFactura IS NULL AND servico.idEstadoServico not in (-1,21,22,20) AND servico.idEquipamento = equipamento.idEquipamento INNER JOIN Funcionario on empresa.idUtilCriacao = Funcionario.idUtilizador left join sap_codigoCondPagam cp on empresa.idCondicoesPagamento = cp.idCodigoCondPagam where empresa.idCondicoesPagamento <> 1 and CAST(numClienteSAP as int) not in  (select cast(codigoClienteSAP as int) from sap_Empresas) ";
				
				//Response.Write(objCmd.CommandText); 
				
				
				objCmd.CommandType = CommandType.Text;
				
				objCmd.Connection = objConn; 
				
				try
				{
					da.Fill(ds,"Empresa"); 
				}
				catch
				{	
				}

				da.Dispose();
				da = null; 
			}
			return ds; 
		}

		protected void btnEmpresas_Click(object sender, System.EventArgs e)
		{

			rptEmpresasSemNumCliente report =new rptEmpresasSemNumCliente(); 
			clsReport cr = new clsReport();
			DataSet DS = null; 

			if(!Page.IsPostBack)
			{
				DS = DSEmpresasSemNumCliente(); 
				DT = DS.Tables[0];
				ViewState["DS"] = DS;
			}
			else
			{
				DS = (DataSet)ViewState["DS"]; 
				DT=DS.Tables[0];
			}
			
			report.SetDataSource(DS); 
            DS = null; 
			cr.exportReportToPDF(report,"Report"); 

			
		}

		protected void ddGrandeza_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            dgEmpresas.CurrentPageIndex = 0;
            dgEquipsActualizar.CurrentPageIndex = 0;
            dgFinalizar.CurrentPageIndex = 0; 
			BindGridEquipsActualizar(); 
			BindGridEquipsFinalizar(); 
		}

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string m_SortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    m_SortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    m_SortDirection = "DESC";
                    break;
            }
            return m_SortDirection;
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable m_DataTable = gvEquipsCalibrar.DataSource as DataTable;

            if (m_DataTable != null)
            {
                DataView m_DataView = new DataView(m_DataTable);
                m_DataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

                gvEquipsCalibrar.DataSource = m_DataView;
                gvEquipsCalibrar.DataBind();
            }
        } 


		protected System.Drawing.Color ConvertColor(string codigoBloqueioSAP)
		{

			//nao trata os codigos Internos.
			//nao trata ainda os outros codigos de bloqueio
			System.Drawing.ColorConverter colConvert = new ColorConverter();

			System.Drawing.Color colorName; 
			switch(codigoBloqueioSAP)
			{
				case "00":
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				case "01":
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");
					break;
				case "02":
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");
					break;
				case "03":
					colorName = (System.Drawing.Color)colConvert.ConvertFromString("PowderBlue");
					break;
				case "04":
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");
					break;
				case "05":
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");
					break;
				case "06":
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");
					break;
				case "08":
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");
					break;
				
				default:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
					break;
			}

            colConvert = null; 

			return colorName; 
		}

        protected void btnPrintCalib_Click1(object sender, EventArgs e)
        {
            openreport(strSQLCalibrar(), "Equipamento por calibrar há mais de 30 dias - " + ddGrandeza.SelectedItem.Text.ToString()); 
        }

        protected void btnPrintActual_Click1(object sender, EventArgs e)
        {
            openreport(strSQLActual(), "Equipamento por actualizar há mais de 30 dias - " + ddGrandeza.SelectedItem.Text.ToString());
        }

        protected void btnPrintFinal_Click1(object sender, EventArgs e)
        {
            openreport(strSQLFinal(), "Equipamento por finalizar há mais de 30 dias - " + ddGrandeza.SelectedItem.Text.ToString()); 
        }



	}
}
