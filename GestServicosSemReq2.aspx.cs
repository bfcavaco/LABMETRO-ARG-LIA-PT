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
using LabMetro.GERAL;
using LabMetro.REPORTS;



namespace LabMetro
{
	/// <summary>
	/// Summary description for GestServicosSemReq.
	/// </summary>
	public partial class GestServicosSemReq2 : System.Web.UI.Page
	{
		private const string ID_PAG = "SERVICOSSEMREQ_0";//NOME DA PAGINA
														 //isto era suposto ser uma pagina de gestao (tipo 1, mas ficou-se por uma pagina de listagem tipo 0)
														 //a versao 2 da pagina gestServicosSemReq2 usa a mesma validacao que a primeira gestServicosSemReq
		protected void Page_Load(object sender, System.EventArgs e)
		{

			lblMessage.Text = "";
			lblMessage2.Text = "";
			Hashtable ht = (Hashtable)Session["HTPermissions"];
			if (ht == null) //session expired
			{
				Server.Transfer("Default.aspx?err=2", false);
			}
			else
			{
				if (!ht.ContainsKey(ID_PAG))
				{
					Server.Transfer("Default.aspx?err=1", false);
				}
				else
				{
					// Put user code to initialize the page here
					if (!Page.IsPostBack)
					{
						ViewState["sortDirection"] = "ASC";
						ViewState["sortField"] = "empresa";

						ViewState["sortDirectionReq"] = "ASC";
						ViewState["sortFieldReq"] = "refRequisicao";
						fillDDEmpresa();
						BindGrid();

					}
				}
			}
		}

		private void BindGrid()
		{

			

			DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();

			
			bool bFactura = chckFactura.Checked;
			bool bSemFactura = chckSemFactura.Checked;
			string idEmpresa = ddEmpresa.SelectedValue.ToString();
			string idBre = ddBre.SelectedValue.ToString();

			DataSet ds = data.DSServicosSemRequisicao2(idEmpresa, idBre, bFactura, bSemFactura);
			
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataView DV = new DataView(ds.Tables[0]);
				DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
				DG.DataSource = DV;
				DG.DataBind();
			}
			else
			{
				DG.DataSource = null;
				DG.DataBind();
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
			btnList.Click += new System.EventHandler(Button1_Click);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			DG.ItemDataBound += new DataGridItemEventHandler(DG_ItemDataBound);

			chckFactura.CheckedChanged += new System.EventHandler(chckFactura_CheckedChanged);
			chckSemFactura.CheckedChanged += new System.EventHandler(chckSemFactura_CheckedChanged);
			DG.SelectedIndexChanged += new System.EventHandler(DG_SelectedIndexChanged);
			DGRequisicoes.SelectedIndexChanged += new System.EventHandler(DGRequisicoes_SelectedIndexChanged);
		}

		#endregion


		private void DG_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DG.EditItemIndex = -1; //caso alguem tenha ficado aberto 
			LinkButton button = (LinkButton)DG.Items[DG.SelectedIndex].Cells[0].Controls[0];
			//button.Attributes.Add("onClick","javascript:location.href=#bottom");

			string id = DG.DataKeys[DG.SelectedIndex].ToString();
			DGRequisicoes.CurrentPageIndex = 0;
			BindGridRequisicoes(id);
			//limpar 
			dgServicos.DataSource = null;
			dgServicos.DataBind();

		}
		private void fillDDEmpresa()
        {
			DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();

			
			bool bFactura = chckFactura.Checked;
			bool bSemFactura = chckSemFactura.Checked;

			SqlDataReader dr = data.drEmpresasComServicosSemRequisicao(bFactura, bSemFactura);
			
			ddEmpresa.DataSource = dr;
			ddEmpresa.DataBind();
			ddEmpresa.Items.Insert(0, new ListItem("--", ""));

			dr.Close();

		}

		private void fillDDBre(string idEmpresa)
		{

			DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();


			bool bFactura = chckFactura.Checked;
			bool bSemFactura = chckSemFactura.Checked;

			SqlDataReader dr = data.drBREdeEmpresasComServicosSemRequisicao(bFactura, bSemFactura,idEmpresa);

			ddBre.DataSource = dr;
			ddBre.DataBind();
			ddBre.Items.Insert(0, new ListItem("--", ""));

			dr.Close();
		}

		private void fillDDRequisicao(string idEmpresa)
		{
			DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();

			SqlDataReader dr = data.DRGetRequisicoesIncompletasByEmpresa( idEmpresa);

			ddRequisao.DataSource = dr;
			ddRequisao.DataBind();
			//ddRequisao.Items.Insert(0, new ListItem("--", ""));

			dr.Close();

		}


		protected void ddEmpresa_SelectedIndexChanged(object sender, EventArgs e)
		{
			fillDDBre(ddEmpresa.SelectedValue);
			DG.CurrentPageIndex = 0;

			BindGrid();//para nao aparecem as requisicoes preenchidas mas as grids com serviços de outras empresas
			fillDDRequisicao(ddEmpresa.SelectedValue);
			DGRequisicoes.DataSource = null;
			DGRequisicoes.DataBind();
		}

		private void DG_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.EditItem)
			{
				//				DATA.RequisicaoBD requisicao = new LabMetro.DATA.RequisicaoBD(); 
				//				//ver se isto funciona
				//				string idEmpresa = DG.DataKeys[e.Item.ItemIndex].ToString(); 
				//				SqlDataReader DR = requisicao.DRGetRequisicoesIncompletasByEmpresa(idEmpresa); 
				//				DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicao");
				//				ddRequisicao.DataSource= DR; 
				//				ddRequisicao.DataBind(); 
				//				ddRequisicao.Items.Insert(0, new ListItem("","")); 
				//				DR.Close(); 
				//
				//				requisicao = null; 

				string idEmpresa = DG.DataKeys[e.Item.ItemIndex].ToString();
				DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
				DataTable DT = req.DTListaRequisicoes(idEmpresa, "", "1", "0", false, false);
				DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicao");

				ddRequisicao.DataSource = DT;
				ddRequisicao.DataBind();
				ddRequisicao.Items.Insert(0, new ListItem("", ""));

				req = null;



			}
		}

		protected void DG_EditGrid(Object sender, DataGridCommandEventArgs e)
		{
			DG.EditItemIndex = e.Item.ItemIndex;
			BindGrid();
		}

		protected void DG_CancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DG.EditItemIndex = -1;
			BindGrid();
		}

		protected void DG_UpdateGrid(Object sender, DataGridCommandEventArgs e)
		{
			string id = DG.DataKeys[e.Item.ItemIndex].ToString();

			DropDownList ddRequisicao = (DropDownList)e.Item.FindControl("ddRequisicao");

			try
			{
				string idServico = DG.Items[DG.EditItemIndex].Cells[4].Text.ToString().Trim();
				if (ddRequisicao.SelectedValue != "")
				{
					string strSQL = "UPDATE SERVICO set idRequisicao = " + ddRequisicao.SelectedValue.ToString() + " WHERE idServico = " + idServico;

					//Response.Write(strSQL); 

					if (GERAL.clsDataAccess.myExecuteNonQuery(strSQL) == 1)
					{
						lblMessage.Text = "Actualizado com sucesso.";
						strSQL = "INSERT INTO [dbo].[HistoricoRequisicoes]   ([idServico], [idRequisicao]  ,[accao],  [idUtilizador] ,[data])  VALUES (" + idServico + ", " + ddRequisicao.SelectedValue.ToString() + ",'A'," + Session["UserID"] + ", getDate()) ";
						try
						{
							GERAL.clsDataAccess.myExecuteNonQuery(strSQL);

						}
						catch { }
					}
					else
					{
						lblMessage.Text = "Erro na actualização do registo.";
					}
				}
			}
			catch
			{
				lblMessage.Text = "Erro na actualização do registo.";
			}
			DG.CurrentPageIndex = 0;
			DG.EditItemIndex = -1;
			BindGrid();
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			DG.CurrentPageIndex = 0;
			BindGrid();
			
		}

		protected void DoPaging(Object s, DataGridPageChangedEventArgs e)
		{
			DG.CurrentPageIndex = e.NewPageIndex;
			BindGrid();

			//em qq evento do primeiro datagrid, tenho de repôr o segundo datagrid na sua posição original
			DGRequisicoes.CurrentPageIndex = 0;
			DGRequisicoes.DataSource = null;
			DGRequisicoes.DataBind();

		}

		protected void DoPagingReq(Object s, DataGridPageChangedEventArgs e)
		{
			DGRequisicoes.CurrentPageIndex = e.NewPageIndex;
			BindGridRequisicoes(DG.DataKeys[DG.SelectedIndex].ToString());

		}

		protected void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirection"].ToString())
			{
				case "ASC":
					ViewState["sortDirection"] = "DESC";
					break;
				case "DESC":
					ViewState["sortDirection"] = "ASC";
					break;
			}

			ViewState["sortField"] = e.SortExpression;

			BindGrid();

			//em qq evento do primeiro datagrid, tenho de repôr o segundo datagrid na sua posição original
			DGRequisicoes.CurrentPageIndex = 0;
			DGRequisicoes.DataSource = null;
			DGRequisicoes.DataBind();

		}

		protected void SortGridReq(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionReq"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionReq"] = "DESC";
					break;
				case "DESC":
					ViewState["sortDirectionReq"] = "ASC";
					break;
			}

			ViewState["sortFieldReq"] = e.SortExpression;

			BindGridRequisicoes(DG.DataKeys[DG.SelectedIndex].ToString());


		}


		private void chckFactura_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chckFactura.Checked == true) chckSemFactura.Checked = false;
			fillDDEmpresa(); //os valores mudam se mudam os parametros da pesquisa
			fillDDBre(ddEmpresa.SelectedValue);
			fillDDRequisicao(ddEmpresa.SelectedValue);
			DG.CurrentPageIndex = 0;

			BindGrid();

		}

		private void chckSemFactura_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chckSemFactura.Checked == true) chckFactura.Checked = false;
			fillDDEmpresa(); //os valores mudam se mudam os parametros da pesquisa
			fillDDBre(ddEmpresa.SelectedValue);
			fillDDRequisicao(ddEmpresa.SelectedValue);
			DG.CurrentPageIndex = 0;

			BindGrid();

		}

		private void BindGridRequisicoes(string idEmpresa)
		{


			DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
			//DataTable DT = req.DTListaRequisicoes(idEmpresa, "", "1", "0", false, false);
			DataTable DT = req.DTGetRequisicoesIncompletasByEmpresa(idEmpresa); //11/2020
			//ver quais os parametros, teem de aparecer apenas as reqs validas e incompletas

			DataView DV = new DataView(DT);

		//	DV.Sort = ViewState["sortFieldReq"].ToString() + " " + ViewState["sortDirectionReq"];

			DGRequisicoes.DataSource = DV;
			DGRequisicoes.DataBind();

			if (DV.Table.Rows.Count > 0)
			{
				DGRequisicoes.Visible = true;
			}
			else
			{
				DGRequisicoes.Dispose();
				DGRequisicoes.Visible = false;
				lblMessage.Text = "Não existem requisições para a empresa seleccionada.";
			}

			req = null;
		}

		protected string ConverteEstado(bool b)
		{
			if (b == true)
			{
				return "sim";
			}
			else
			{
				return "não";
			}
		}

		public string downloadpath(object filename)
		{
			if (filename != null && filename.ToString() != "")
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

		private void Button1_Click(object sender, System.EventArgs e)
		{

			DATA.RequisicaoBD data = new LabMetro.DATA.RequisicaoBD();

			//string strNumBRE = txtBRE.Text;
			//string strAno = txtAno.Text;
			//string strEmpresa = txtEmpresa.Text;
			//string strRefServico = txtRefServico.Text;


			string strTitulo = "Serviços Sem Requisição Não Facturados";

			//ultimo param = false, so os serviços n facturados
			DataSet ds = data.DSServicosSemRequisicao("", "", "", "", false, true);


			REPORTS.rptServicosSemRequisicao report = new rptServicosSemRequisicao();

			clsReport cr = new clsReport();

			report.SetDataSource(ds);
			report.SetParameterValue("@tituloReport", strTitulo);
			ds = null;
			cr.exportReportToPDF(report, "Report");

			//cr = null;
			//report = null;

		}


		private void BindGridServicos()
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inIdRequisicao", DGRequisicoes.DataKeys[DGRequisicoes.SelectedIndex].ToString());

			DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosByIdRequisicao", arrParams);
			if (dt.Rows.Count > 0)
			{
				dgServicos.DataSource = dt;
				dgServicos.DataBind();
			}
			else
			{
				dgServicos.DataSource = null;
				dgServicos.DataBind();
				lblMessage2.Text = "Sem resultados.";
			}
		}

		private void DGRequisicoes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindGridServicos();
		}






		protected void btnExcel_Click(object sender, EventArgs e)
		{
			//string strNumBRE = txtBRE.Text;
			//string strAno = txtAno.Text;
			//string strEmpresa = txtEmpresa.Text;
			//string strRefServico = txtRefServico.Text;
			bool bFactura = chckFactura.Checked;
			bool bSemFactura = chckSemFactura.Checked;


			string strSQL = "SELECT dbo.udfGetReferenciaBRE(BRE.idBRE) AS refBRE, bre.dtBre, s.refServico ,tipoEquipamento.descricao as equipamento, es.descricao AS estado, e.nomeAbreviado AS empresa, e.observacoes as obsEmpresa, e.bPodefacturarSemRequisicao as EmpFacturaSemReq,EC.nomeAbreviado as EmpContrat, ec.observacoes as obsEC,  ec.bPodefacturarSemRequisicao as ECFacturaSemReq FROM servico s INNER JOIN bre ON s.idBre = bre.idBre INNER JOIN EstadoServico ES ON s.idEstadoServico = es.idEstadoServico INNER JOIN empresa e ON bre.idEmpresa = e.idEmpresa  LEFT JOIN Empresa EC ON BRE.idEmpresaContratante = EC.idEmpresa INNER JOIN equipamento on s.idEquipamento = equipamento.idEquipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento  WHERE s.idRequisicao IS NULL AND es.idEstadoServico NOT IN (-1,20,21,22,7) AND (s.ano > 2006 or s.idFactura is null)";

			//SqlParameter[] arrParams = new SqlParameter[4];       

			if (bFactura == true)
			{
				strSQL += " AND s.idfactura IS NOT NULL";
			}

			if (bSemFactura == true)
			{
				strSQL += " AND s.idfactura IS NULL and  ISNULL(EC.bPodeFacturarSemRequisicao,E.bPodeFacturarSemRequisicao) = 0";  //nao esquecer que pode haver aqui uma empresa contratante....
			}

			//if (strNumBRE != "") strSQL += " AND  dbo.udfGetReferenciaBRE(BRE.idBRE) LIKE '" + strNumBRE + "%'";
			//if (strAno != "") strSQL += " AND s.ano = '" + strAno + "'";
			//if (strEmpresa != "") strSQL += " AND e.nome LIKE '" + strEmpresa + "%'";
			//if (strRefServico != "") strSQL += " AND s.refServico LIKE '" + strRefServico + "%'";

			strSQL += " ORDER BY BRE.dtBRE ";
			//HttpContext.Current.Response.Write(strSQL); 



			DataTable DT = GERAL.clsDataAccess.ExecuteDT(strSQL);

			DataView DV = new DataView(DT);


			gv.DataSource = DV;
			gv.DataBind();
			GERAL.GridViewExportUtil.Export("requisicoes.xls", gv);

		}

        protected void btnUpdate_Click(object sender, EventArgs e)
		{
			string strIds = "";
			foreach (DataGridItem dgi in DG.Items)
			{
				CheckBox myCheckBox =
					(CheckBox)dgi.Cells[0].FindControl("checkbox");
				if (myCheckBox.Checked == true)
				{
					string idServico = DG.Items[dgi.ItemIndex].Cells[4].Text.ToString().Trim();
					strIds += idServico;
					strIds += ",";
				}
			}

			string delimStr = ",";
			char[] delimiter = delimStr.ToCharArray();
			strIds = strIds.TrimEnd(delimiter);
            //Response.Write(strIds);

            string idRequisicao = ddRequisao.SelectedValue.ToString();
			if (idRequisicao !="")
			{ 
			string strSQL = "Update servico set idRequisicao = "+idRequisicao + ", dtAlteracao = getdate(), idUtilAlteracao = " + HttpContext.Current.Session["UserId"].ToString() + "  where idServico in (" + strIds+")";
				lblMessage.Text = GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);

				BindGrid();
				
			}
		}

        protected void ddBre_SelectedIndexChanged(object sender, EventArgs e)
        {
			DG.CurrentPageIndex = 0;

			BindGrid();
        }
    }
}

