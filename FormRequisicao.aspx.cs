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
using System.Configuration; 
using System.IO; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for FormRequisicao.
	/// </summary>
    public partial class FormRequisicao : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.DataGrid DGLinhasRequisicao;
        protected System.Web.UI.WebControls.TextBox txtObservacoesFooter;
        protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
        protected System.Web.UI.WebControls.CompareValidator CompareValidator1;
        protected System.Web.UI.WebControls.RegularExpressionValidator Regularexpressionvalidator2;
        protected System.Web.UI.WebControls.CompareValidator Comparevalidator2; 
        private const string ID_PAG = "REQUISICOES_1";//NOME DA PAGINA
                    
//		DataTable DT; 
//		DataView DV; 


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
//            DGLinhasRequisicao.ItemCommand += new DataGridCommandEventHandler (DGLinhasRequisicao_ItemCommand); 
//            DGLinhasRequisicao.ItemDataBound +=  new DataGridItemEventHandler(DGLinhasRequisicao_ItemDataBound); 
//            DGLinhasRequisicao.DeleteCommand += new  DataGridCommandEventHandler(DGLinhasRequisicao_DeleteCommand);
            
			btnRemove.Click += new System.EventHandler(btnRemove_Click);
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            
            btnUpload.Click += new System.EventHandler(btnUpload_Click);

			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);

        }
        #endregion

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
                    int intAcesso = System.Convert.ToInt32(ht[ID_PAG]); 
                    if(intAcesso ==0) //so tem permissoes de leitura
                    {
                        btnSubmit.Enabled=false;
                        btnUpload.Enabled=false;
                        btnRemove.Enabled=false;  
                    }
                   
                    if(!Page.IsPostBack)	
                    {   
                        ViewState["idRequisicao"] =""; //para estar declarado

                        if(Request.QueryString["id"]!=null)
                        {
                            if(Request.QueryString["id"]!="")
                            {
                                if(!Page.IsPostBack)
                                {
                                    ViewState["idRequisicao"] = Request.QueryString["id"].ToString();
									fillDDEmpresa(); 
                                    fillForm(ViewState["idRequisicao"].ToString());
                                    btnSubmit.CommandArgument="update";                                    
                                    
                                    if(!Page.IsPostBack)
                                    {
                                        if(Request.QueryString["errUpd"]!=null &&Request.QueryString["errUpd"]!="")  
                                        {
                                            string retValue = Request.QueryString["errUpd"].ToString();  
                                            if (retValue == "0")
                                            {
                                                lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
                                            }
                                            else
                                            {
                                                lblMessage.Text= GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
                                            }
                                        }
                                    }
                                }   
                            }    
                        }
                        else
                        {
                            if(!Page.IsPostBack)
                            {
                                btnSubmit.CommandArgument="insert";                                 
                            }
                        }                      
                    }
                }
            }    
        }

		private void fillCompanyInfo()
		{
			LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(ddEmpresa.SelectedValue);
			if(detEmpresa!= null)
			{
				lblObsEmpresa.Text = detEmpresa.observacoes;    //meu

				if ("1".Equals(detEmpresa.pagamentoAtraso) )
				{
					lblEmpresaDevedora.Text = "*****   PAGAMENTOS EM ATRASO   *****";
				}
				else
				{ 
					lblEmpresaDevedora.Text = "";
				}
			}
		}

		private void fillDDEmpresa()
		{

			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			
			DataTable DT = empresa.DTEmpresas(txtPesquisaEmpresa.Text, txtPesquisaNif.Text,"1","","","","","",""); 
			DataView DV = new DataView(DT);
            
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind(); 

			empresa = null;

			try
			{
				if(ViewState["idEmpresa"]!= null)
				{
					if(ViewState["idEmpresa"].ToString()!= "")
					{
						ddEmpresa.SelectedValue = ViewState["idEmpresa"].ToString(); 
					}
				}
			}
			catch
			{
			}

			fillCompanyInfo(); //quando muda a ddEmpresa, muda a ddCompanyInfo
		}
    
		private void fillForm(string id)
		{
			DATA.RequisicaoBD fillreq = new LabMetro.DATA.RequisicaoBD(); 
			LabMetro.DATA.RequisicaoDetails det = fillreq.GetRequisicaoDetails(id); 

			if(det!= null)
			{
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
				fillCompanyInfo(); 
				txtPesquisaEmpresa.Enabled=false;
				txtPesquisaNif.Enabled = false;
				btnEmpresas.Enabled=false; 

				ViewState["idEmpresa"] = det.idEmpresa; //para o itemdatabound depois

				lblNumRequisicao.Text = "Ref.Requisição ISQ :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + det.refRequisicao.ToString();
				txtDataRequisicao.Text = GERAL.clsGeral.ToShortDate(det.dtRequisicao.ToString()); 
				txtDataValidade.Text = GERAL.clsGeral.ToShortDate(det.dtValidade); 
				txtNomeFicheiro.Text = det.nomeFicheiro.ToString(); 
				txtRefCliente.Text = det.referenciaCliente.ToString(); 
				txtObservacoes.Text = det.observacoes.ToString(); 
				rblCompleta.SelectedValue = det.completa; 
				chbContrato.Checked =  det.bContrato;
				chbRenovavel.Checked = det.bRenovavel;

                if (chbContrato.Checked == true)
                {
                    btnRemove.Enabled = false;
                    btnUpload.Enabled = true; //como nao pode apagar o anterior, tb nao pode trocar o ficheiro que la está (o que equivalaria a apagar o outro).
                }
			}


            fillreq = null;
            det = null;
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
        
			if(Page.IsValid)
			{
				if(btnSubmit.CommandArgument=="insert")
				{
					InsertBD(); 
				}
				else if(btnSubmit.CommandArgument=="update")
				{
					UpdateBD(); 
				}
			}
		}

		private void InsertBD()
		{
			DATA.RequisicaoBD  req = new LabMetro.DATA.RequisicaoBD(); 
			string idRequisicao = req.InsertRequisicao(ddEmpresa.SelectedValue.ToString(),txtRefCliente.Text,txtDataRequisicao.Text,txtDataValidade.Text.ToString(),rblCompleta.SelectedValue.ToString(),txtNomeFicheiro.Text,txtObservacoes.Text,User.Identity.Name.ToString(),GERAL.clsGeral.ConvertStringToBool(chbContrato.Checked.ToString()), GERAL.clsGeral.ConvertStringToBool(chbRenovavel.Checked.ToString())).ToString(); 

			if (idRequisicao == "0")
			{
				lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
			}
			else
			{
				Response.Redirect("FormRequisicao.aspx?btn=DOC&id=" + idRequisicao);
			}
		}

		private void UpdateBD() 
		{
			DATA.RequisicaoBD  req = new LabMetro.DATA.RequisicaoBD(); 

			int retValue = req.UpdateRequisicao(ViewState["idRequisicao"].ToString(),ViewState["idEmpresa"].ToString(),txtRefCliente.Text.ToString(),txtDataRequisicao.Text.ToString(), txtDataValidade.Text.ToString(),rblCompleta.SelectedValue.ToString(),txtNomeFicheiro.Text,txtObservacoes.Text,User.Identity.Name.ToString(),GERAL.clsGeral.ConvertStringToBool(chbContrato.Checked.ToString()), GERAL.clsGeral.ConvertStringToBool(chbRenovavel.Checked.ToString())); 

			if(retValue ==2) retValue = 1; //retorna mais de uma linha por causa da auditoria...
			else retValue = 0; 
			Response.Redirect("FormRequisicao.aspx?btn=DOC&errUpd="+retValue.ToString()+"&id="+ViewState["idRequisicao"].ToString(),true);   
		}
		
		private void btnUpload_Click(object sender, System.EventArgs e)
		{
			if(fileIn.PostedFile.FileName != "")
			{
				string strFileName; 
				try
				{
					strFileName = System.IO.Path.GetFileName(fileIn.PostedFile.FileName);

					string path = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_PATH_REL"];
					string myPath = Server.MapPath("~/"+path); 

					if(File.Exists(myPath + "/" + strFileName))
					{
						lblMessage.Text ="Já existe um ficheiro com o mesmo nome."; 
					}
					else
					{
						fileIn.PostedFile.SaveAs(myPath + "/" + strFileName);    
						txtNomeFicheiro.Text = strFileName; 
					}
				}
				catch(Exception ex) 
				{
					GERAL.clsWriteError.WriteLog("Erro no carregamento de ficheiros."+ex.ToString()); 
					lblMessage.Text= "Erro no carregamento do ficheiro."; 
				}
			}
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{

			string strFileName = txtNomeFicheiro.Text.ToString(); ; 
			//caminho absoluto
			//string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_PATH"];

			//caminho relativo
			string path = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_PATH_REL"];
			string myPath = Server.MapPath("~/"+path); 
			if(File.Exists(myPath + "/" + strFileName))
			{
				try
				{     
					File.Delete(myPath + "/" + strFileName);       
					txtNomeFicheiro.Text =""; 
				}
				catch(Exception ex) 
				{
					GERAL.clsWriteError.WriteLog("Erro no apagar de ficheiros."+ex.ToString()); 
					lblMessage.Text= "Erro ao tentar apagar do ficheiro."; 
				}
				try
				{
					DATA.RequisicaoBD  req = new LabMetro.DATA.RequisicaoBD(); 
					//se ele nao consegue fazer isto, nao faz mal.
					req.ApagaFicheiroRequisicao(ViewState["idRequisicao"].ToString(),User.Identity.Name.ToString()); 
				}
				catch
				{
					//nada. nao posso controlar se ele quer apagar mm da bd, ou se carrega para apagar da dir.
				}
			}
		}

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		private void btnEmpresas_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 	 
		}

		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillCompanyInfo(); 
		}   
    }
}
