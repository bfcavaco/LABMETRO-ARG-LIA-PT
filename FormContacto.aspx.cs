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
using System.Text.RegularExpressions;



namespace LabMetro
{
	/// <summary>
	/// Summary description for FormContacto.
	/// </summary>
	public partial class FormContacto : System.Web.UI.Page
	{
      
        private const string ID_PAG = "CONTACTOS_1";//NOME DA PAGINA
    
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
					else


					{
						
						if(validaUsersContactInfo())//se user logado pode altarar os valores das checkboxes
						{
							chbFacturacao.Enabled=true; 
							chbOrcamento.Enabled =true;
							chbRequisicoes.Enabled = true;
							chbManutencao.Enabled = true;
							chbQualidade.Enabled = true;
							chbCertificados.Enabled=true;
                            //chbGestaoEquipamentos.Enabled = true; //esta nunca está enables porque de momento nao está a ser usada
							//este ainda é dúvidoso se são estes users que vao controlar, mas por enquanto fica assim

                            chbAlertasPlanosCalibracao.Enabled = true;

                            //as duas checkboxes de receber alertas de levantamento e alertas de novos certificados são marcados pelos proprios clientes no labmetro online
						}
						else
						{
							chbFacturacao.Enabled=false; 
							chbOrcamento.Enabled =false;
							chbRequisicoes.Enabled = false;
							chbManutencao.Enabled = false;
							chbQualidade.Enabled = false;
							chbCertificados.Enabled=false;
                            chbAlertasPlanosCalibracao.Enabled = false;
						}
					}

                    if(!Page.IsPostBack)
                    {
                        fillDropDowns(); 

                        if(Request.QueryString["id"]!=null)
                        {
                            if(Request.QueryString["id"]!="")
                            {
						        ViewState["idContacto"] = Request.QueryString["id"].ToString();
								
						        fillForm(ViewState["idContacto"].ToString()); 

								txtPesquisaEmpresa.Enabled=false; 
								txtPesquisaNif.Enabled= false; 
								btnEmpresas.Enabled=false; 
								ddEmpresa.Enabled=false; 

                                btnSubmit.CommandArgument = "update"; 
                            }   
                            else
                            {
                                fillDDEmpresa(); 
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
		private void fillDDEmpresa()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,txtPesquisaNif.Text,"","","","","","",""); 
            
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 
		}

        private void fillDropDowns()
        {   

            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();  
  
            SqlDataReader DR4 = lista.DRListaTitulos(); 
            ddtitulo.DataSource = DR4; 
            ddtitulo.DataBind(); 
            ddtitulo.Items.Insert(0,new ListItem("",""));
            DR4.Close();    

			lista = null; 
            
        }   

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{   
			if(Page.IsValid)

			{

				if(chbCertificados.Checked==true)
				{
					if(txtEmailDirecto.Text=="")
					{
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_EMAIL; 
						return; 
					}
				}

                if (chbGestaoEquipamentos.Checked == true)
                {
                    if (chbCertificados.Checked == false)
                    {
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DAR_ACESSO_LABMETRO_ONLINE;
                        return;
                    }
                }


				if(txtEmailDirecto.Text!="" && IsValidEmail(txtEmailDirecto.Text) ==false)
				{
					lblMessage.Text ="Email em formato inválido."; 
					return; 
				}
				
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

		// Função que insere o Contacto na BD
		// TODO: eliminar valores hard coded
		private void insertBD()
		{
            DATA.ContactosBD contacto = new LabMetro.DATA.ContactosBD(); 

            try
            {
                string id =  contacto.InsertContact(ddEmpresa.SelectedValue,  ddtitulo.SelectedValue.ToString(),  txtNome.Text, rblContactoPrincipal.SelectedValue, txtCargo.Text, txtDepartamento.Text, txtExtensao.Text, txtTelefoneDirecto.Text,txtFaxDirecto.Text,txtEmailDirecto.Text, txtObservacoes.Text.ToString(),txtTelemovel.Text,chbFacturacao.Checked.ToString(), chbOrcamento.Checked.ToString(), chbQualidade.Checked.ToString(), chbManutencao.Checked.ToString(),chbCertificados.Checked.ToString(),chbRequisicoes.Checked.ToString(),ddEstado.SelectedValue, User.Identity.Name.ToString(), chbGestaoEquipamentos.Checked.ToString(), chbAlertasPlanosCalibracao.Checked.ToString()); 
            
				ViewState["idContacto"] = id; 
				btnSubmit.CommandArgument ="update";
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB; 
			}
            
			catch(Exception ex)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
				GERAL.clsWriteError.WriteLog(ex.ToString()); 
            }

			contacto = null; 
        }
       
		// Função que actualiza o Contacto na BD
		// TODO: eliminar valores hard coded
        private void updateBD()
        {
			DATA.ContactosBD contacto = new LabMetro.DATA.ContactosBD(); 

			string retValue =  contacto.UpdateContact(ViewState["idContacto"].ToString(), ddEmpresa.SelectedValue, ddtitulo.SelectedValue.ToString(),  txtNome.Text, rblContactoPrincipal.SelectedValue, txtCargo.Text, txtDepartamento.Text, txtExtensao.Text, txtTelefoneDirecto.Text,txtFaxDirecto.Text,txtEmailDirecto.Text, txtObservacoes.Text.ToString(),txtTelemovel.Text,chbFacturacao.Checked.ToString(), chbOrcamento.Checked.ToString(), chbQualidade.Checked.ToString(), chbManutencao.Checked.ToString(),chbCertificados.Checked.ToString(),chbRequisicoes.Checked.ToString(), ddEstado.SelectedValue, User.Identity.Name.ToString(),chbGestaoEquipamentos.Checked.ToString(), chbAlertasPlanosCalibracao.Checked.ToString()); 

			if (retValue == "0")
			{
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			}
			else
			{
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
			}

			btnSubmit.CommandArgument ="update"; //just in case....
			
			contacto = null; 
			
        }

        private void fillForm(string id)
        {
            LabMetro.DATA.ContactosBD fillContacts = new LabMetro.DATA.ContactosBD(); 
            LabMetro.DATA.ContactDetails myDetails = fillContacts.GetContactDetails(id); 
            if(myDetails!= null)
            {
                //try
                //{
                //    ddEmpresa.SelectedValue = myDetails.idEmpresa; 
                //}
                //catch
                //{
                    ddEmpresa.Items.Insert(0,new ListItem(myDetails.nomeEmpresa,myDetails.idEmpresa));
                    ddEmpresa.SelectedValue = myDetails.idEmpresa; 
                //}
                try
                {
                    ddtitulo.SelectedValue= myDetails.idTitulo;
                }
                catch{}
                //n ha catch, n é importante
              
               	txtNome.Text = myDetails.nome;
				rblContactoPrincipal.SelectedValue = myDetails.contactoPrincipal; 
                txtCargo.Text = myDetails.cargo; 
                txtDepartamento.Text = myDetails.departamento;
				txtExtensao.Text = myDetails.extensaoEmpresa;
				txtTelefoneDirecto.Text = myDetails.telefoneEmpresa;
				txtFaxDirecto.Text = myDetails.faxEmpresa;
				txtEmailDirecto.Text = myDetails.emailEmpresa;
			    try
                {
                    ddEstado.SelectedValue = myDetails.activo; 
                }
                catch
                {
                    //nada
                }
                txtObservacoes.Text=myDetails.observacoes;
				txtTelemovel.Text = myDetails.telemovel;
				chbCertificados.Checked =  myDetails.bCertificados;
				chbFacturacao.Checked = myDetails.bFacturacao;
				chbManutencao.Checked = myDetails.bManutencao;
				chbOrcamento.Checked = myDetails.bOrcamento;
				chbQualidade.Checked =  myDetails.bQualidade;
				chbRequisicoes.Checked =  myDetails.bRequisicoes;
                chbGestaoEquipamentos.Checked = myDetails.bGestaoEquipamentos;
                chbAlertasLevantamentos.Checked = myDetails.bAlertasLevantamentos;
                chbAlertasNovosCertificados.Checked = myDetails.bAlertasCertificados;
                chbAlertasPlanosCalibracao.Checked = myDetails.bPlanosCalibracao;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_DATA; 
            }
			
			fillContacts = null; 
        }

		
		protected void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		protected void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		protected void btnEmpresas_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		bool IsValidEmail(string strIn)
		{
			// Return true if strIn is in valid e-mail format.
			//return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
           // return Regex.IsMatch(strIn, @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            //return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            }
            catch 
            {
                return false;
            }
		}

		private bool validaUsersContactInfo()
		{
			
			string userNames = System.Configuration.ConfigurationManager.AppSettings["canUpdateContactoInfo"]; 
			
			string[] strUserNames = userNames.Split(",".ToCharArray()); 

			foreach(string s in strUserNames)
			{
				if(s.ToLower() == User.Identity.Name.ToString().ToLower())
				{
					return true;
				}
			}
			return false; 
		}




	}
}
