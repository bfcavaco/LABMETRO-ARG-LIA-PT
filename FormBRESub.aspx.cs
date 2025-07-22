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
	/// Summary description for FormBRESub.
	/// </summary>
	public partial class FormBRESub : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblFuncionarioRecepcao;
		
        DataView DVOrigem; 
        DataTable DTOrigem; 

        DataView DVDestino; 
        DataTable DTDestino;


        private const string ID_PAG = "BRESUB_1";//NOME DA PAGINA
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();


		protected void Page_Load(object sender, System.EventArgs e)
		{
            lblMessage.Text = ""; 

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
                {
                    int intAcesso = System.Convert.ToInt32(ht[ID_PAG]); 
                    //if(intAcesso == 1)//tem permissoes para tudo. 
                    //so tem permissoes de leitura, desactivar todos os submits
                    if(intAcesso ==0) 
                    {
                        btnSubmit.Enabled=false;
                      

                    }
                    //codigo normal do page_load ======================  
			        if(!Page.IsPostBack)
			        {	
                      
						FillDropDowns();
                        if(Request.QueryString["id"]!=null)
                        {
                            if(Request.QueryString["id"]!="")
                            {
                                
                                ViewState["idSubcontratoBRE"] = Request.QueryString["id"].ToString();
                                fillForm(ViewState["idSubcontratoBRE"].ToString());     
                            }
                            btnSubmit.CommandArgument="update";                              
                            btnVerBRESub.Enabled=true; 
                            
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
                            btnVerBRESub.Enabled=false; 
                          
                        }

				        //para a ordenacao do datagrid de origem
				        ViewState["sortField"] = "codTipoEquipamento";
				        ViewState["sortDirection"] = "DESC";
				        //==============================================
        				
				        //limpaControls();
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
            DTOrigem = servicos.DTGetServicosForSubcontratoBRE(ddEmpresa.SelectedValue, ddEmpresaSub.SelectedValue);

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
                    DTDestino = servicos.DTGetServicoBySubcontratoBRE(ViewState["idSubcontratoBRE"].ToString()); 
                    
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

        protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillEmpresasSub();
            //?? novo
            CreateDataSource(); 
            BindGridSource(); 
            CreateDataSourceDTDestino(); 
            BindGridSelected();
        }
        
        protected void ddEmpresaSub_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CreateDataSource(); 
            BindGridSource(); 
            CreateDataSourceDTDestino(); 
            BindGridSelected(); 
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
            //DR["idSubcontratoBSE"] = drv["idSubcontratoBSE"].ToString(); 
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
                        //=================================================================
                        //quando passam para baixo, o estado é mudado para 
                        //subcontratação calibrada
                        //HARDCODED
                        DR["idEstadoServico"] = "10"; 
                        DR["estadoServico"] = "Subcontratação Calibrada"; 
                        //DR["idEstadoServico"] = dr["idEstadoServico"].ToString(); 
                        //DR["estadoServico"] = dr["estadoServico"].ToString(); 
                        //=================================================================
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

        //preenche o footer do datagrid de serviços associados ao BRE (Sub.)
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

                DataRowView DRV = (DataRowView) e.Item.DataItem;
                string idEstado = DRV["idEstadoServico"].ToString();      
                if(idEstado !="") ddEstado.SelectedValue = idEstado; 

				servico = null; 
            }     
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

			ddFuncionarioRecepcao.DataSource = DR; 
			ddFuncionarioRecepcao.DataBind();	
            
            DR.Close(); 
            
            if(!Page.IsPostBack) 
			{
				try
				{
					ddFuncionarioRecepcao.SelectedValue= System.Convert.ToString(DATA.GeralBD.GetIdFuncionarioByUsername(User.Identity.Name.ToString()));     
				}
				catch
				{
				}
			}
			lista= null; 
		} 

		private void fillEmpresas()
		{
			DATA.EmpresasBD lista = new LabMetro.DATA.EmpresasBD(); 

            SqlDataReader DR = lista.DRGetEmpresasForBRESub(); 

			ddEmpresa.DataSource = DR;  
			ddEmpresa.DataBind(); 

            DR.Close(); 
			lista = null; 
		}

		private void fillEmpresasSub()
		{
			if(ddEmpresa.SelectedIndex >=0)
			{
				DATA.EmpresasBD lista = new LabMetro.DATA.EmpresasBD();  

                SqlDataReader DR =lista.DRGetEmpresasSubcontratadasByIdEmpresa(ddEmpresa.SelectedValue); 
				ddEmpresaSub.DataSource = DR; 
				ddEmpresaSub.DataBind(); 

                DR.Close(); 
				lista = null; 
			}
		}

		private void fillForm(string id)
		{
			LabMetro.DATA.SubcontratoBreBD fillBRESub = new LabMetro.DATA.SubcontratoBreBD(); 
			LabMetro.DATA.SubcontratoBreDetails det = fillBRESub.GetSubcontratoBreDetails(id); 
			if(det!= null)
			{
				// Preencher o BRE (sub.)
				txtDtBRESub.Text = GERAL.clsGeral.ToShortDate(det.dtSubcontratoBRE);
				txtObservacoes.Text = det.observacoes;
				txtEntreguePor.Text = det.entreguePor;
				txtRefBRESub.Text = det.refSubcontratoBRE;
				//lblEmpresa.Text = det.nomeEmpresa;
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
                    ddFuncionarioRecepcao.SelectedValue = det.idFuncionarioRecepcao; 
                }
                catch
                {
                    ddFuncionarioRecepcao.Items.Insert(0,new ListItem(det.idFuncionarioRecepcao,det.idFuncionarioRecepcao)); 
                    ddFuncionarioRecepcao.SelectedValue = det.idFuncionarioRecepcao; 
                }
			}

			fillBRESub = null; 
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
            BindGridSource(); //NOVO!!!!
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

        private void UpdateBD()
        {
            DTDestino= (DataTable)ViewState["DTDestino"]; 
            DTOrigem = (DataTable)ViewState["DTOrigem"]; 

            if(DTDestino.Rows.Count == 0)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_MIN_1EQUIPAMENTO;
            }
            else
            {
				DATA.SubcontratoBreBD subBRE = new LabMetro.DATA.SubcontratoBreBD(); 
                int retValue = subBRE.UpdateSubBREWithLinhas(ViewState["idSubcontratoBRE"].ToString(),ddFuncionarioRecepcao.SelectedValue.ToString(),txtEntreguePor.Text,txtObservacoes.Text,User.Identity.Name.ToString(),DTOrigem,DTDestino); 
				subBRE = null; 

				Response.Redirect("FormBRESub.aspx?btn=DOC&errUpd="+retValue.ToString()+"&id="+ViewState["idSubcontratoBRE"].ToString(),true);   
            }
        }

		private void InsertBD()
		{

            DTDestino= (DataTable)ViewState["DTDestino"]; 
   
			if(DTDestino.Rows.Count == 0)
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_MIN_1EQUIPAMENTO;

			}
			else
			{
				DATA.SubcontratoBreBD subBRE = new LabMetro.DATA.SubcontratoBreBD(); 
				string id = subBRE.InsertSubBREWithLinhas(ddEmpresaSub.SelectedValue.ToString(), ddFuncionarioRecepcao.SelectedValue.ToString(), txtEntreguePor.Text, txtObservacoes.Text, User.Identity.Name.ToString(), DTDestino).ToString();
				subBRE = null; 

				if (id == "0")
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
				}
				else
				{
					Response.Redirect("FormBRESub.aspx?btn=DOC&id=" + id, true);
					//Server.Transfer("FormBRESub.aspx?btn=DOC&id=" + id);
				}
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

        protected void btnVerBRESub_Click(object sender, System.EventArgs e)
        {
                
                ReportClass report = null;

                switch (myApp)
                {
                    case "ISQ_LABMETRO":
                        report = new LabMetro.REPORTS.crSubcontratoBRE();
                        break;
                    case "ANG_LABMETRO":
                        report = new LabMetro.REPORTS_ANG.crSubcontratoBRE();
                        break;
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crSubContratoBRE();
                        break;
                    case "CV_LABMETRO":
                        break;
                    case "SON_LABMETRO":
                        report = new LabMetro.REPORTS_SON.crSubcontratoBRE();
                        break;
                    case "BR_LABMETRO":
                        report = new LabMetro.REPORTS_BR.crSubcontratoBRE();
                        break;

                }

                DATA.SubcontratoBreBD bresub = new LabMetro.DATA.SubcontratoBreBD();
                DataSet ds = bresub.DSBRESub(ViewState["idSubcontratoBRE"].ToString());
            
                report.SetDataSource(ds);
            
                clsReport cr = new clsReport();
                ds = null;
                bresub = null; 
                cr.exportReportToPDF(report,"BSE_SUB");
				
				
                //cr = null; 
				
                //report = null; 
        }
	}
}

