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

namespace LabMetro
{
	/// <summary>
	/// Summary description for FormFuncionario.
	/// </summary>
	public partial class FormFuncionario : System.Web.UI.Page
	{
        private const string ID_PAG = "FUNCIONARIOS_1";//NOME DA PAGINA

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
                    //if(intAcesso == 1)//tem permissoes para tudo. 
                 
                    if(intAcesso ==0) 
                    {
                        btnSubmit.Enabled=false;
                    }
                 
			        if(!Page.IsPostBack)
			        {
				        fillDropDowns(); 

				        if(Request.QueryString["id"]!=null)
				        {
					        if(Request.QueryString["id"]!="")
					        {
								if(Request.QueryString["ok"]!=null)
								{
									if(Request.QueryString["ok"]=="ok")
										lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DB;
								}


						        ViewState["idFuncionario"] = Request.QueryString["id"].ToString();
						        fillForm(ViewState["idFuncionario"].ToString()); 
						        btnSubmit.CommandArgument = "update"; 

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
					        else
					        {
						        btnSubmit.CommandArgument ="insert"; 
					        }  
				        }
				        else
				        {
					        btnSubmit.CommandArgument ="insert"; 
				        }
			        }
                }
            }    
        }

        private void fillDropDowns()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
          
            SqlDataReader DR4 = lista.DRListaFuncoes();
            ddFuncao.DataSource = DR4; 
            ddFuncao.DataBind(); 
            ddFuncao.Items.Insert(0,new ListItem("",""));
            DR4.Close(); 
        
            SqlDataReader DR5 = lista.DRListaLocalCalibracao(); 
            ddLocalCalibracao.DataSource = DR5; 
            ddLocalCalibracao.DataBind(); 
            ddLocalCalibracao.Items.Insert(0,new ListItem("",""));
            DR5.Close(); 
            
            SqlDataReader DR6 =  lista.DRListaLaboratorios(); 
            ddLaboratorio.DataSource = DR6;  
            ddLaboratorio.DataBind();
            ddLaboratorio.Items.Insert(0,new ListItem("",""));
            
            DR6.Close(); 

			lista = null; 
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

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{   
			if(Page.IsValid)
			{
				if(btnSubmit.CommandArgument=="insert")
				{
					insertBD();
				}
				else if(btnSubmit.CommandArgument=="update")
				{
					updateBD(); 
				}
			}
		}

		// Função que insere o Funcionário na BD
		// TODO: eliminar valores hard coded
        private void insertBD()
        {
            LabMetro.DATA.FuncionariosBD funcionario = new LabMetro.DATA.FuncionariosBD(); 
            
            try
            {

                string id = funcionario.InsertFuncionario(ddLaboratorio.SelectedValue, ddFuncao.SelectedValue, ddLocalCalibracao.SelectedValue, txtNome.Text, txtNomeAbreviado.Text,  txtExtensao.Text, txtTelefone.Text, txtDataAdmissao.Text, ddEstado.SelectedValue, txtObservacoes.Text,User.Identity.Name.ToString(),cbCTA.Checked.ToString(),txtEmail.Text.ToString(),txtNumFuncionario.Text);

				funcionario = null; 
				Response.Redirect("FormFuncionario.aspx?btn=GES&id="+id+"&ok=ok"); 
            }
            catch(Exception ex)
            {
				funcionario = null; 
				GERAL.clsWriteError.WriteLog(ex.ToString());
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
            }
           
        }
      
		// Função que actualiza o Funcionário na BD
		// TODO: eliminar valores hard coded
        private void updateBD()
        {
            LabMetro.DATA.FuncionariosBD funcionario = new LabMetro.DATA.FuncionariosBD(); 

           lblMessage.Text  = funcionario.UpdateFuncionario(ViewState["idFuncionario"].ToString(),ddLaboratorio.SelectedValue, ddFuncao.SelectedValue, ddLocalCalibracao.SelectedValue, txtNome.Text, txtNomeAbreviado.Text,  txtExtensao.Text, txtTelefone.Text, txtDataAdmissao.Text, ddEstado.SelectedValue, txtObservacoes.Text,User.Identity.Name.ToString(), cbCTA.Checked.ToString(),txtEmail.Text.ToString(), txtNumFuncionario.Text);

			funcionario = null;
            btnSubmit.Enabled = false;           
		}

        private void fillForm(string id)
        {
            LabMetro.DATA.FuncionariosBD fillFuncionarios = new LabMetro.DATA.FuncionariosBD(); 

            LabMetro.DATA.FuncionarioDetails myDetails = fillFuncionarios.GetFuncionarioDetails(id); 

            if(myDetails!= null)
            {
                
                try
                {
                    ddLaboratorio.SelectedValue = myDetails.idLaboratorio; 
                }
                catch
                {
                    ddLaboratorio.Items.Insert(0,new ListItem(myDetails.laboratorio,myDetails.idLaboratorio));
                    ddLaboratorio.SelectedValue = myDetails.idLaboratorio; 
                }
                
                try
                {
                    ddFuncao.SelectedValue = myDetails.idFuncao;
                }
                catch
                {
                    ddFuncao.Items.Insert(0,new ListItem(myDetails.funcao,myDetails.idFuncao));
                    ddFuncao.SelectedValue = myDetails.idFuncao; 
                }
 
                try
                {
                    ddLocalCalibracao.SelectedValue = myDetails.idLocalCalibracao; 
                }
                catch
                {
                    ddLocalCalibracao.Items.Insert(0,new ListItem(myDetails.localCalibracao,myDetails.idLocalCalibracao));

                    ddLocalCalibracao.SelectedValue = myDetails.idLocalCalibracao; 
                }
				
                txtNome.Text = myDetails.nome; 
				txtNomeAbreviado.Text = myDetails.nomeAbreviado; 
				txtExtensao.Text = myDetails.extensao; 
				txtTelefone.Text = myDetails.telefoneDirecto; 
				txtDataAdmissao.Text = GERAL.clsGeral.ToShortDate(myDetails.dataAdmissao); 
				txtEmail.Text = myDetails.email;
                txtNumFuncionario.Text = myDetails.numFuncionario;

				
				
                try
                {
                    ddEstado.SelectedValue = myDetails.estado; 
                }
                catch
                {
                }
				try
				{
					cbCTA.Checked = System.Convert.ToBoolean(myDetails.bCta);
				}
				catch
				{
				}
                txtObservacoes.Text = myDetails.observacoes;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_DATA; 
            }

			fillFuncionarios = null; 
        }

	}
}
