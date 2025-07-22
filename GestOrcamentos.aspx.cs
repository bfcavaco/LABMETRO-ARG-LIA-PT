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
using LabMetro.REPORTS; 
using LabMetro.GERAL; 
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine; 
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaOrcamentos.
	/// </summary>
	public partial class GestOrcamentos : System.Web.UI.Page
	{

		private const string ID_PAG = "GESTORCAMENTOS_1";//NOME DA PAGINA

    
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

					if(!Page.IsPostBack)
					{
						ViewState["sortField"] = "idOrcamento";
						ViewState["sortDirection"] = "DESC";
						FillDDEstadoOrcamento();
                        fillResponsaveisTecnicos();
                        FillDDRazaoCliente();
						//BindGrid(); 

					}
					if(!ht.ContainsKey("ORCAMENTOS_1")) //se n tem permissoes para ver os detalhes dos funcionarios, desactivar o link
					{
						DG.Columns[5].Visible=false; 
					}
				}
			}
		}
    
		private void FillDDEstadoOrcamento()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            
			ddEstadoOrcamento.DataSource = lista.DVListaEstadosOrcamentos();
			ddEstadoOrcamento.DataBind(); 
			ddEstadoOrcamento.Items.Insert(0, new ListItem("","")); 


			lista = null; 
		}


        private void FillDDRazaoCliente()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();

            ddRazaoCliente.DataSource = lista.DVRazaoClienteOcamentos();
            ddRazaoCliente.DataBind();
            ddRazaoCliente.Items.Insert(0, new ListItem("", ""));


            lista = null;
        }


        private void fillResponsaveisTecnicos()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            DataTable DT = lista.DTListaFuncionarios("1,4");  //MUITO MAU, HARDCODED!!! MAU PQ FOI PEDIDO ASSIM, DEVIAMOS IR POR PERFIL, mas vamos por funcao.


            // Obter funcionários cuja função é "Responsável Técnico" (4)
            ddFuncionario.DataSource = DT;
            ddFuncionario.DataBind();
            //por default seleccionar o luis godinho, no caso do insert:
            //é feito no pageload
            ddFuncionario.Items.Insert(0, new ListItem("", ""));
        }


		private void BindGrid()
		{

            string strCalibracaoExterna = "";
            if (cbCalibracaoExterna.Checked == true) strCalibracaoExterna = "1";

            string strFollowup = "";
            if (cbFollowup.Checked == true) strFollowup = "1";

			DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            DataTable dt = orcamento.DTOrcamentos(txtEmpresa.Text, txtTipoEquipamento.Text, ddEstadoOrcamento.SelectedValue, txtRefOrcamento.Text, "", txtDtInicio.Text, txtDtFim.Text, txtValorMinimo.Text, strCalibracaoExterna, ddFuncionario.SelectedValue, ddRazaoCliente.SelectedValue, strFollowup); 

			DataView DV = new DataView(dt);
	
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
            
			DG.DataSource = DV;
			DG.DataBind(); 

			orcamento = null;
        
		}



        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            //DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
            string strCalibracaoExterna = "";
            if (cbCalibracaoExterna.Checked == true) strCalibracaoExterna = "1";

            string strFollowup = "";
            if (cbFollowup.Checked == true) strFollowup = "1";

            DATA.OrcamentoBD orcamento = new LabMetro.DATA.OrcamentoBD();

            DataTable dt = orcamento.DTOrcamentos(txtEmpresa.Text, txtTipoEquipamento.Text, ddEstadoOrcamento.SelectedValue, txtRefOrcamento.Text, "", txtDtInicio.Text, txtDtFim.Text, txtValorMinimo.Text, strCalibracaoExterna, ddFuncionario.SelectedValue, ddRazaoCliente.SelectedValue, strFollowup);

            DataView DV = new DataView(dt);

            DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

            DG.DataSource = DV;
            DG.DataBind();

            orcamento = null; 
            gv.DataSource = DV;
            gv.DataBind();
            GERAL.GridViewExportUtil.Export("orcamentos.xls", gv);


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

		}
		#endregion

		protected void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			DG.CurrentPageIndex=0; 
			BindGrid(); 
		}

		//=================================================================================================
		// DEVOLVE O CAMINHO PARA UMA REQUISICAO
		//=================================================================================================
		
		private void verOrcamento(string idOrcamento)
		{
			rptOrcamentoNovo report = new rptOrcamentoNovo(); 
			clsReport cr = new clsReport();
			
			report.SetParameterValue("@inFaxNumber","==por email=="); 
			DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD(); 
			DataSet ds  = orc.DSOrcamFax(idOrcamento); 	
			report.SetDataSource(ds);
            ds = null;
			cr.exportReportToPDF(report,"Orcamento"); 
		
		}

		protected void dg_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{	
			
			
				if(e.CommandName.ToString()=="verOrcamentos")
				{
					verOrcamento(e.CommandArgument.ToString()); 
				}
				if(e.CommandName.ToString()=="verServicos")
				{
					string idOrcamento = DG.DataKeys[e.Item.ItemIndex].ToString();
					verServicos(idOrcamento); 
				}
			if(e.CommandName.ToString()=="verRequisicoes")
			{
				string idOrcamento = DG.DataKeys[e.Item.ItemIndex].ToString();
				verRequisicoes(idOrcamento); 
			}
			
		}


		//============================================================================	
		//EDITA OS DADOS DE UM ORÇAMENTO
		//============================================================================	
		protected void editGrid(Object sender, DataGridCommandEventArgs e)     
		{
			DG.ShowFooter=false;     
			DG.EditItemIndex = e.Item.ItemIndex;	
			BindGrid(); 
		}

		//============================================================================	
		//CANCELA A EDIÇÃO DOS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void cancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DG.ShowFooter=true;  
			DG.EditItemIndex = -1;
			BindGrid();
		}

		//============================================================================	
		//ALTERA OS DADOS DE UM EQUIPAMENTO
		//============================================================================	
		protected void updateGrid(Object sender, DataGridCommandEventArgs e)
		{
			string idOrcamento = DG.DataKeys[e.Item.ItemIndex].ToString();
		

			DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado");
            DropDownList ddRazaoCliente = (DropDownList)e.Item.FindControl("ddRazaoCliente");
			
			TextBox  txtObs = (TextBox) e.Item.FindControl("txtObs");
            TextBox txtObsFollowup = (TextBox)e.Item.FindControl("txtObsFollowup");
            CheckBox checkFollowup = (CheckBox)e.Item.FindControl("checkFollowup");
			
			
			DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD(); 


			//mais uma vez, p ser mais rapido:
            string idRazaoCliente = "";
            if (ddRazaoCliente.SelectedIndex > 0)
            {
                idRazaoCliente = ddRazaoCliente.SelectedValue.ToString();
            }
            string strSQL = "UPDATE Orcamento SET idEstadoOrcamento = " + ddEstado.SelectedValue.ToString() + ", idRazaoCliente = '" + idRazaoCliente + "', obsFollowup = '" + txtObsFollowup.Text.ToString() + "',observacoes = '" + txtObs.Text.ToString() + "',bFollowup = " + GERAL.clsGeral.ConvertStringToBool(checkFollowup.Checked.ToString()) + ", idUtilAlteracao =" + Session["UserId"].ToString() + ", dtAlteracao = CONVERT(DATETIME, GETDATE(), 102) WHERE idOrcamento = " + idOrcamento;

            if(GERAL.clsDataAccess.myExecuteNonQuery(strSQL)!= 1)

            {
                Response.Write(strSQL + "<br>");
            }

            insereBDPSI(idOrcamento);
             

			DG.EditItemIndex = -1;
			DG.ShowFooter=true;   
			BindGrid(); 
            
			
		}


        private void insereBDPSI(string idOrcamento)
        {

            try
            {
                //   User: ISQ\psiexuser
                //·         PW: 67?%bkTw
                WebServicePSI.PSIIntegrationProposal proposal = new WebServicePSI.PSIIntegrationProposal();

                string strSQL = "select * from vOrcamentosLabmetro_PSI v inner join orcamento o on v.proposalTitle = o.refOrcamento where o.idOrcamento =  " + idOrcamento;  

                using (Impersonation imp = new Impersonation("psiexuser", "67?%bkTw", "ISQ"))
                {
                    if (imp.impersonateValidUser())
                    {
                        using (WebServicePSI.ProposalServiceClient pc = new WebServicePSI.ProposalServiceClient())
                        {
                            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    proposal.ProposalNumber = dr["proposalNumber"].ToString();
                                    proposal.ProposalOrigin = System.Convert.ToInt16(dr["ProposalOrigin"].ToString());//INT
                                    proposal.ProposalTitle = dr["proposalTitle"].ToString();
                                    proposal.ProposalManagement = dr["ProposalManagement"].ToString();
                                    proposal.ProposalClient = dr["ProposalClient"].ToString();
                                    proposal.ProposalClientName = dr["ProposalClientName"].ToString();
                                    proposal.ProposalClientNIF = dr["ProposalClientFiscalCode"].ToString();
                                    proposal.ProposalStatus = Guid.Parse(dr["ProposalStatus"].ToString());//GUID
                                    proposal.ProposalValue = Decimal.Parse(dr["ProposalValue"].ToString()); //decimal
                                    proposal.ProposalDate = DateTime.Parse(dr["ProposalDate"].ToString()); //date
                                    proposal.ProposalManager = dr["proposalManager"].ToString();
                                    proposal.ProposalDepartment = dr["proposalDepartment"].ToString();
                                    proposal.ProposalCostCenter = dr["ProposalCostCenter"].ToString();
                                    proposal.ProposalCountry = dr["ProposalCountry"].ToString();
                                }

                                dr.Close();
                                pc.IntegrateProposals(new WebServicePSI.PSIIntegrationProposal[] { proposal });


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GERAL.clsWriteError.WriteLog("userID: " + HttpContext.Current.Session["UserId"].ToString() + " " + ex.ToString() + "Erro no Orçamento: " + idOrcamento.ToString());

            }
        }


        public class Impersonation : IDisposable
        {
            private bool disposed = false;

            private string _username, _password, _domain;

            public const int LOGON32_LOGON_INTERACTIVE = 2;
            public const int LOGON32_PROVIDER_DEFAULT = 0;

            WindowsImpersonationContext impersonationContext;

            internal Impersonation(string username, string password, string domain)
            {
                _username = username;
                _password = password;
                _domain = domain;
            }

            [DllImport("advapi32.dll")]
            internal static extern int LogonUserA(String lpszUserName,
                String lpszDomain,
                String lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                ref IntPtr phToken);

            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int DuplicateToken(IntPtr hToken,
                int impersonationLevel,
                ref IntPtr hNewToken);

            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool RevertToSelf();

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            internal static extern bool CloseHandle(IntPtr handle);


            internal bool impersonateValidUser()
            {
                WindowsIdentity tempWindowsIdentity;
                IntPtr token = IntPtr.Zero;
                IntPtr tokenDuplicate = IntPtr.Zero;

                if (RevertToSelf())
                {
                    if (LogonUserA(_username, _domain, _password, LOGON32_LOGON_INTERACTIVE,
                        LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = tempWindowsIdentity.Impersonate();
                            if (impersonationContext != null)
                            {
                                CloseHandle(token);
                                CloseHandle(tokenDuplicate);
                                return true;
                            }
                        }
                    }
                }
                if (token != IntPtr.Zero)
                    CloseHandle(token);
                if (tokenDuplicate != IntPtr.Zero)
                    CloseHandle(tokenDuplicate);
                return false;
            }

            public void undoImpersonation()
            {
                if (impersonationContext != null)
                    impersonationContext.Undo();
            }


            // Implement IDisposable.
            // Do not make this method virtual.
            // A derived class should not be able to override this method.
            public void Dispose()
            {
                Dispose(true);
                // This object will be cleaned up by the Dispose method.
                // Therefore, you should call GC.SupressFinalize to
                // take this object off the finalization queue
                // and prevent finalization code for this object
                // from executing a second time.
                GC.SuppressFinalize(this);
            }

            // Dispose(bool disposing) executes in two distinct scenarios.
            // If disposing equals true, the method has been called directly
            // or indirectly by a user's code. Managed and unmanaged resources
            // can be disposed.
            // If disposing equals false, the method has been called by the
            // runtime from inside the finalizer and you should not reference
            // other objects. Only unmanaged resources can be disposed.
            protected virtual void Dispose(bool disposing)
            {
                // Check to see if Dispose has already been called.
                if (!disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.
                        undoImpersonation();
                    }

                    // Note disposing has been done.
                    disposed = true;

                }
            }

            Impersonation()
            {
                // Do not re-create Dispose clean-up code here.
                // Calling Dispose(false) is optimal in terms of
                // readability and maintainability.
                Dispose(false);
            }


        }

		//====================================================================================
		//ITEM DATABOUND DO DATAGRID EQUIPAMENTOS
		//====================================================================================
		
		protected void DG_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;
				

				TextBox  txtObs = (TextBox) e.Item.FindControl("txtObs");
				txtObs.Text = DRV["observacoes"].ToString();


                TextBox txtObsFollowup = (TextBox)e.Item.FindControl("txtObsFollowup");
                txtObs.Text = DRV["obsFollowup"].ToString(); 

				
				DropDownList ddEstadoOrcamento = (DropDownList)e.Item.FindControl("ddEstado");
				

				DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            
				ddEstadoOrcamento.DataSource = lista.DVListaEstadosOrcamentos();
				ddEstadoOrcamento.DataBind(); 
				
   		
				ddEstadoOrcamento.SelectedValue = DRV["idEstadoOrcamento"].ToString();


                DropDownList ddRazaoCliente = (DropDownList)e.Item.FindControl("ddRazaoCliente");

                ddRazaoCliente.DataSource = lista.DVRazaoClienteOcamentos();
                ddRazaoCliente.DataBind();
                ddRazaoCliente.Items.Insert(0,new ListItem("", ""));


                ddRazaoCliente.SelectedValue = DRV["idRazaoCliente"].ToString();

                CheckBox checkFollowup = (CheckBox)e.Item.FindControl("checkFollowup");
                checkFollowup.Checked = System.Convert.ToBoolean(DRV["bFollowup"]); 
                lista = null; 
				
			}
		}

		private void verServicos(string idOrcamento)
		{

			DGRequisicoes.DataSource = null;
			DGRequisicoes.DataBind();
			DGRequisicoes.Visible=false;

			//reset
			DGServicos.DataSource = null;
			DGServicos.DataBind();
			DGServicos.Visible=true;

			SqlParameter[] arrParams = new SqlParameter[1]; 
			arrParams[0] = new SqlParameter("@idOrcamento",idOrcamento); 
			

			DGServicos.DataSource = GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosEmpresaPosterioresAOrcamento", arrParams);
			DGServicos.DataBind(); 

			
		}

		private void verRequisicoes(string idOrcamento)
		{
			DGServicos.DataSource = null;
			DGServicos.DataBind();
			DGServicos.Visible=false;

			//reset
			DGRequisicoes.DataSource = null;
			DGRequisicoes.DataBind();
			DGRequisicoes.Visible=true;
			
			SqlParameter[] arrParams = new SqlParameter[1]; 
			arrParams[0] = new SqlParameter("@idOrcamento",idOrcamento); 
			

			DGRequisicoes.DataSource = GERAL.clsDataAccess.SPExecuteDTParams("stpGetRequisicoesEmpresaPosterioresAOrcamento", arrParams);
			DGRequisicoes.DataBind(); 

			
		}
		public string downloadpath(object filename)
		{
			if(filename!=null && filename.ToString()!="")
			{
				string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_URL"];
                
				myPath = myPath + "/" + filename.ToString(); 
				return myPath;
			}
			return "#"; 
            
		}

      

        protected void btnNumOrcamentosPorEstadoEMes_Click(object sender, EventArgs e)
        {
            string strSQL = " SELECT  COUNT(Orcamento.idOrcamento) AS num, DATEPART(mm, Orcamento.dtCriacao) AS mes, DATEPART(yy, Orcamento.dtCriacao) AS ano, EstadoOrcamento.descricao FROM   Orcamento INNER JOIN   EstadoOrcamento ON Orcamento.idEstadoOrcamento = EstadoOrcamento.idEstadoOrcamento GROUP BY DATEPART(yy, Orcamento.dtCriacao), DATEPART(mm, Orcamento.dtCriacao), EstadoOrcamento.descricao order by 3,2 desc";


            DataTable DT = GERAL.clsDataAccess.ExecuteDT(strSQL);

            DataView DV = new DataView(DT);


            GridView2.DataSource = DV;
            GridView2.DataBind();
            GERAL.GridViewExportUtil.Export("orcamentos_estado.xls", GridView2);
        }
      
	}
}
