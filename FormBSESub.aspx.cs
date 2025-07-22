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
using LabMetro.GERAL;
using LabMetro.REPORTS;
using System.Data.SqlClient; 
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;


namespace LabMetro
{
	/// <summary>
	/// Summary description for FormBSESub.
	/// </summary>
	public partial class FormBSESub : System.Web.UI.Page
	{
		DataView DVOrigem; 
		DataTable DTOrigem; 
		DataView DVDestino; 
		DataTable DTDestino;
        
        private const string ID_PAG = "BSESUB_1";//NOME DA PAGINA
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

        protected void Page_Load(object sender, System.EventArgs e)
        {
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
                    int intAcesso = System.Convert.ToInt32(ht[ID_PAG]); 
                    //if(intAcesso == 1)//tem permissoes para tudo. 
                    if(intAcesso ==0) 
                    {
                        btnSubmit.Enabled=false;
                    }
                    
                    if(!Page.IsPostBack)
                    {	
                        FillDropDowns(); 
                        
                        if(Request.QueryString["id"]!=null)
                        {
                            if(Request.QueryString["id"]!="")
                            {
                                
                                ViewState["idSubcontratoBSE"] = Request.QueryString["id"].ToString();
                                fillForm(ViewState["idSubcontratoBSE"].ToString());     
                            }
                            btnSubmit.CommandArgument="update";  
                            btnVerBSESub.Enabled=true; 
                        

                            if(!Page.IsPostBack)
                            {
                                if(Request.QueryString["errUpd"]!=null &&Request.QueryString["errUpd"]!="")  

                                {
                                    string retValue = Request.QueryString["errUpd"].ToString();  
                                    //atenção q este é diferente, retorna 0 se correu mal.
                                    if (retValue.ToString() == "0")
                                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
                                    else
                                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
                                }
                            }
                        }
                        else
                        {
                            btnSubmit.CommandArgument="insert";      
                            btnVerBSESub.Enabled=false; 
                        
                        }

                        //para a ordenacao do datagrid de origem
                        ViewState["sortField"] = "codTipoEquipamento";
                        ViewState["sortDirection"] = "DESC";
                        
                        CreateDataSource(); 
                        BindGridSource();
                        CreateDataSourceDTDestino();
                        BindGridSelected(); 
                    }   
                    
                }
            }    
        }
        

        private void CreateDataSource()
        {
            DTOrigem = new DataTable(); 
            DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD(); 
            DTOrigem = servicos.DTGetServicosForSubcontratoBSE(ddEmpresa.SelectedValue);	
            //====================================================================
            DataColumn[] dcPk = new DataColumn[1];
            dcPk[0] = DTOrigem.Columns["idServico"];
            DTOrigem.PrimaryKey = dcPk;
            //DTOrigem.DefaultView.Sort = "idServico";
            //====================================================================

             ViewState["DTOrigem"] = DTOrigem; 

			servicos = null; 
        }
        
        private void BindGridSource()
        {   
            DTOrigem = (DataTable)ViewState["DTOrigem"]; 
            DVOrigem = new DataView(DTOrigem); 

            string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
            DVOrigem.Sort = strSort; 
            dgOrigem.DataSource = DVOrigem; 
            dgOrigem.DataBind(); 
        }

        
        private void CreateDataSourceDTDestino()
        {
            if(!Page.IsPostBack)
            {
                if(Request.QueryString["id"]!= null && Request.QueryString["id"]!="")
                {
                    DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD(); 
                    //qual o select que devo usar aqui????
                    DTDestino = servicos.DTGetServicoBySubcontratoBSE(ViewState["idSubcontratoBSE"].ToString()); 
                    
                    DataColumn[] dcPk = new DataColumn[1];
                    dcPk[0] = DTDestino.Columns["idServico"];
                    DTDestino.PrimaryKey = dcPk;

					servicos = null; 
                }

                else
                {
                    DTDestino = new DataTable(); 
                    DTOrigem = (DataTable)ViewState["DTOrigem"]; 
                    DTDestino = DTOrigem.Clone(); 
                }   
            }
            else
            {
                DTDestino = new DataTable(); 
                DTOrigem = (DataTable)ViewState["DTOrigem"]; 
                DTDestino = DTOrigem.Clone(); 
            }

           ViewState["DTDestino"] = DTDestino;

        }

        private void BindGridSelected()
        {
            DTDestino= (DataTable)ViewState["DTDestino"]; 
            DVDestino = new DataView(DTDestino); 
            dgDestino.DataSource= DVDestino; 
            dgDestino.DataBind();
        }


        private void dgDestino_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
        {

            DTDestino= (DataTable)ViewState["DTDestino"]; 
            DVDestino = new DataView(DTDestino); 
            DTOrigem= (DataTable)ViewState["DTOrigem"];

            DataRowView drv = DVDestino[e.Item.ItemIndex]; 
 
            //ADICIONA À ORIGEM (aqui usa-se o add em vez do importrow)
            DataRow DR; 
            DR = DTOrigem.NewRow(); 
            //DR = DTOrigem.c
            
            DR["idServico"] = drv["idServico"].ToString(); 
            DR["idBRE"] = drv["idBRE"].ToString(); 
            DR["refBRE"] = drv["refBRE"].ToString(); 
            DR["idRequisicao"] = drv["idRequisicao"].ToString(); 
            DR["refRequisicao"] = drv["refRequisicao"].ToString(); 
            DR["numIdentificacao"] = drv["numIdentificacao"].ToString(); 
            DR["codTipoEquipamento"] = drv["codTipoEquipamento"].ToString(); 
            DR["idEstadoServico"] = drv["idEstadoServico"].ToString(); 
            DR["estadoServico"] = drv["estadoServico"].ToString(); 
            DR["idLocalCalibracao"] = drv["idLocalCalibracao"].ToString(); 
            DR["localCalibracao"] = drv["localCalibracao"].ToString(); 
            DR["idTipoServico"] = drv["idTipoServico"].ToString(); 
            DR["tipoServico"] = drv["tipoServico"].ToString(); 
            DR["refServico"] = drv["refServico"].ToString(); 
            DR["observacoes"] = drv["observacoes"].ToString(); 

            
            DTOrigem.Rows.Add(DR); 
            
            //APAGA DO DESTINO 
            string id = drv["idServico"].ToString(); 
            DataRow dr = DTDestino.Rows.Find(id); 
            dr.Delete(); 
            
            ViewState["DTOrigem"] = DTOrigem; 
            ViewState["DTDestino"] = DTDestino; 

            BindGridSelected(); 
            BindGridSource(); 

        }
        private void AddLinesToDestino()
        {

            DTDestino= (DataTable)ViewState["DTDestino"]; 
            DTOrigem = (DataTable)ViewState["DTOrigem"];

            string strIds =""; 

            foreach(DataGridItem dgi in dgOrigem.Items) 
            { 
                CheckBox myCheckBox =
                    (CheckBox)dgi.Cells[0].FindControl("checkbox"); 
                if(myCheckBox.Checked == true)
                {
                    strIds+= dgOrigem.DataKeys[dgi.ItemIndex].ToString();
                    strIds+=",";
                }   
            }

            string delimStr = ",";
            char [] delimiter = delimStr.ToCharArray();
            strIds = strIds.TrimEnd(delimiter); 

            if(strIds != "")
            {
                string strSearch = "idServico IN (" + strIds + ")"; 
    	
                DataRow[] foundRows = DTOrigem.Select(strSearch); 
            
                foreach(DataRow dr in foundRows)
                {        
                    if(!DTDestino.Rows.Contains(dr["idServico"]))
                    {
                        DataRow DR; 
                        DR = DTDestino.NewRow(); 
        
                        DR["idServico"] = dr["idServico"].ToString(); 
                        DR["idBRE"] = dr["idBRE"].ToString(); 
                        DR["refBRE"] = dr["refBRE"].ToString();
                        //DR["idSubcontratoBSE"] = dr["idSubcontratoBSE"].ToString(); 
                        DR["idRequisicao"] = dr["idRequisicao"].ToString(); 
                        DR["refRequisicao"] = dr["refRequisicao"].ToString(); 
                        DR["numIdentificacao"] = dr["numIdentificacao"].ToString(); 
                        DR["codTipoEquipamento"] = dr["codTipoEquipamento"].ToString(); 

                        //hardcoded: 5-Subcontratação em Calibração
                        DR["idEstadoServico"] = "5";  
                        DR["estadoServico"] = "Subcontratação em Calibração"; 
//                        DR["idEstadoServico"] = dr["idEstadoServico"].ToString(); 
//                        DR["estadoServico"] = dr["estadoServico"].ToString(); 
                        DR["idLocalCalibracao"] = dr["idLocalCalibracao"].ToString(); 
                        DR["localCalibracao"] = dr["localCalibracao"].ToString(); 
                        DR["idTipoServico"] = dr["idTipoServico"].ToString(); 
                        DR["tipoServico"] = dr["tipoServico"].ToString(); 
                        DR["refServico"] = dr["refServico"].ToString(); 
                        DR["observacoes"] = dr["observacoes"].ToString(); 

                        DTDestino.Rows.Add(DR); 

                        dr.Delete(); //apaga da origem
                    
                    }
                }
            }
            ViewState["DTOrigem"] = DTOrigem; 
            ViewState["DTDestino"] = DTDestino; 
        }

		private void FillDropDowns()
		{
			fillEmpresas();
			fillEmpresasSub();
			fillFuncionarios();
		}

		private void fillFuncionarios()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaFuncionarios();
			ddFuncionarioSaida.DataSource =  DR; 
			ddFuncionarioSaida.DataBind();
            DR.Close(); 

            //DATA.GeralBD geral = new LabMetro.DATA.GeralBD();
            if(!Page.IsPostBack) 
                try
                {
                    ddFuncionarioSaida.SelectedValue = System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));   
                }
                catch
                {
                    //nada
                }

			lista = null; 
			
		} 

		private void fillEmpresas()
		{
			DATA.EmpresasBD lista = new LabMetro.DATA.EmpresasBD(); 
            SqlDataReader DR = lista.DRGetEmpresasForBSESub();
			ddEmpresa.DataSource =  DR; 
			ddEmpresa.DataBind(); 
            DR.Close();

			lista = null; 
		}

		private void fillEmpresasSub()
		{
			DATA.EmpresasBD lista = new LabMetro.DATA.EmpresasBD(); 
            SqlDataReader DR = lista.DRGetEmpresasSubcontratadas(); 
			ddEmpresaSub.DataSource = DR; 
			ddEmpresaSub.DataBind(); 

            DR.Close(); 

			lista = null; 
		}

		private void fillForm(string id)
		{
			LabMetro.DATA.SubcontratoBseBD fillBSESub = new LabMetro.DATA.SubcontratoBseBD(); 
			LabMetro.DATA.SubcontratoBseDetails det = fillBSESub.GetSubcontratoBseDetails(id); 
			if(det!= null)
			{
				// Preencher o BSE (sub.)
				txtDtBSESub.Text = GERAL.clsGeral.ToShortDate(det.dtSubcontratoBSE);
				txtObservacoes.Text = det.observacoes;
				txtRecebidoPor.Text = det.recebidoPor;
				txtRefBSESub.Text = det.refSubcontratoBSE;
                cbCertNomeCliente.Checked = det.bCertNomeCliente;

                try
                {
                    ddEmpresa.SelectedValue = det.idEmpresa; 
                }
                catch
                {
                    ddEmpresa.Items.Insert(0,new ListItem(det.nomeEmpresa,det.idEmpresa)); 
                    ddEmpresa.SelectedValue = det.idEmpresa; 
                }
                
                ddEmpresa.Enabled=false; 

                try
                {
                    ddEmpresaSub.SelectedValue = det.idEmpresaSubcontratada; 
                }
                catch
                {
                    ddEmpresaSub.Items.Insert(0,new ListItem(det.nomeEmpresaSubcont,det.idEmpresaSubcontratada)); 
                    ddEmpresaSub.SelectedValue = det.idEmpresaSubcontratada; 

                }

                ddEmpresaSub.Enabled=false; 
                
                try
                {
                    ddFuncionarioSaida.SelectedValue = det.idFuncionarioSaida; 
                }
                catch
                {
                    ddFuncionarioSaida.Items.Insert(0,new ListItem(det.nomeFuncionarioSaida,det.idFuncionarioSaida)); 
                    ddFuncionarioSaida.SelectedValue = det.idFuncionarioSaida; 
                }

				fillBSESub = null; 
//				// Preencher as Linhas do BSE (sub.)
//				DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD(); 
//				DTDestino = servicos.DTGetServicoBySubcontratoBSE(det.idSubcontratoBSE);
//				DVDestino = new DataView(DTDestino); 
//				BindGridSelected(); 

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
			dgDestino.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(dgDestino_DeleteCommand);
			dgDestino.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(dgDestino_ItemDataBound);

		}
		#endregion
        
		
		protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{

            fillEmpresasSub();
            CreateDataSource(); 
            BindGridSource(); 
            CreateDataSourceDTDestino(); 
            BindGridSelected();

		}

        //a empresa sub nao muda nada
        protected void ddEmpresaSub_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

		protected void editGrid(Object sender,DataGridCommandEventArgs e)     
		{
			dgDestino.EditItemIndex = e.Item.ItemIndex;    
			BindGridSelected();
		}

		protected void cancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			dgDestino.EditItemIndex = -1;
			BindGridSelected();  
		}

		protected void updateGrid(Object sender,DataGridCommandEventArgs e)
		{
			string id =	dgDestino.DataKeys[e.Item.ItemIndex].ToString();
			
			TextBox txtObs = (TextBox)e.Item.FindControl("txtObservacoesEdit"); 
			DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoServicoEdit");
            
            DTDestino= (DataTable)ViewState["DTDestino"]; 
            DVDestino = new DataView(DTDestino); 

			DVDestino.RowFilter = "idServico=" + id ;
            
			if(DVDestino.Count > 0) 
			{    
				DVDestino[0]["observacoes"] = txtObs.Text; 
				DVDestino[0]["idEstadoServico"] = ddEstado.SelectedValue.ToString(); 
				DVDestino[0]["estadoServico"] = ddEstado.SelectedItem.Text;
			}

			DVDestino.RowFilter=""; 
            
            ViewState["DTDestino"] = DTDestino; 
			
            dgDestino.EditItemIndex = -1; 
			BindGridSelected();
		}
  
		protected void btnSubmitGrid_Click(object sender, System.EventArgs e)
		{   
            AddLinesToDestino(); 
            BindGridSelected();
            BindGridSource(); //NOVO!!!  
		}

		protected void SortGridOrigem(Object s, DataGridSortCommandEventArgs e)
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
			BindGridSource(); 
		}

		private void InsertBD()
		{

            DTDestino= (DataTable)ViewState["DTDestino"]; 

			DATA.SubcontratoBseBD subBSE = new LabMetro.DATA.SubcontratoBseBD(); 
   
			if(DTDestino.Rows.Count == 0)
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_MIN_1EQUIPAMENTO;
			}
			else
			{
				string idSubcontratoBSE = subBSE.InsertSubBSEWithLinhas(ddEmpresaSub.SelectedValue.ToString(), ddFuncionarioSaida.SelectedValue.ToString(), txtRecebidoPor.Text, txtObservacoes.Text, User.Identity.Name.ToString(), DTDestino, cbCertNomeCliente.Checked.ToString()).ToString();

				subBSE = null; 

				if (idSubcontratoBSE == "0")
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
				}
				else
				{
					Response.Redirect("FormBSESub.aspx?btn=DOC&id=" + idSubcontratoBSE,true); 
				}
			}
		}
       
        private void UpdateBD()
        {

            DTDestino= (DataTable)ViewState["DTDestino"]; 
            DTOrigem = (DataTable)ViewState["DTOrigem"]; 

            DATA.SubcontratoBseBD subBSE = new LabMetro.DATA.SubcontratoBseBD(); 
            if(DTDestino.Rows.Count == 0)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_MIN_1EQUIPAMENTO;
            }
            else
            {
                int retValue = subBSE.UpdateSubBSEWithLinhas(ViewState["idSubcontratoBSE"].ToString(), ddFuncionarioSaida.SelectedValue.ToString(), txtRecebidoPor.Text, txtObservacoes.Text, User.Identity.Name.ToString(), DTOrigem, DTDestino, cbCertNomeCliente.Checked.ToString()); 
                
				subBSE = null; 

				Response.Redirect("FormBSESub.aspx?btn=DOC&errUpd="+retValue.ToString()+"&id="+ViewState["idSubcontratoBSE"].ToString(),true);
                
            }
        }

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				if(btnSubmit.CommandArgument=="insert")
				{
					InsertBD();	
				}
				else if(btnSubmit.CommandArgument =="update")
				{
					UpdateBD(); 
					
				}
			}
		}

		//preenche o footer do datagrid de serviços associados ao BSE (Sub.)
		private void dgDestino_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoServicoEdit");

                DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();     
                SqlDataReader DR = servico.DRGetListaEstadosServico(dgDestino.DataKeys[e.Item.ItemIndex].ToString());
				ddEstado.DataSource = DR; 
				ddEstado.DataBind(); 
                DR.Close(); 
				
				servico = null; 

				DataRowView DRV = (DataRowView) e.Item.DataItem;
				string idEstado = DRV["idEstadoServico"].ToString();      
				if(idEstado !="") ddEstado.SelectedValue = idEstado; 
			}     
		}

        protected void btnVerBSESub_Click(object sender, System.EventArgs e)
        {
			try
			{
                ReportClass report = null;

                switch (myApp)
                {
                    case "ANG_LABMETRO":
                        report = new LabMetro.REPORTS_ANG.crSubcontratoBSE();
                        break;
                    case "SON_LABMETRO":
                        report = new LabMetro.REPORTS_SON.crSubcontratoBSE();
                        break;
                    case "ISQ_LABMETRO":
                        report = new LabMetro.REPORTS.crSubcontratoBSE();
                        break;
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crSubContratoBSE();
                        break;
                    case "DZ_LABMETRO":
                        report = new LabMetro.REPORTS_DZ.crSubcontratoBSE();
                        break;
                    case "CV_LABMETRO":
                        //report = new LabMetro.REPORTS_CV.crSubcontratoBSE();
                        break;
                    case "BR_LABMETRO":
                        report = new LabMetro.REPORTS_BR.crSubcontratoBSE();
                        break;
                }
			   
                DATA.SubcontratoBseBD bse = new LabMetro.DATA.SubcontratoBseBD();
                DataSet ds = bse.DSBSE(ViewState["idSubcontratoBSE"].ToString());

                report.SetDataSource(ds);
                ds = null;
                bse = null; 
                
                clsReport cr = new clsReport();
                cr.exportReportToPDF(report,"Report");


                //cr = null;
                //report = null; 
				
			}
			catch(Exception ex)
			{
				GERAL.clsWriteError.WriteLog(ex.ToString()); 
			}

        }
	}
}
