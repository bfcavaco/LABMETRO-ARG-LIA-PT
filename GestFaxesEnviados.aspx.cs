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

namespace LabMetro
{
	/// <summary>
	/// Summary description for GestFaxesEnviados.
	/// </summary>
	public partial class GestFaxesEnviados : System.Web.UI.Page
	{
        private const string ID_PAG = "EQUIP_CALIB_1";//NOME DA PAGINA - tenho de mudar!!!
    
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
                    // Put user code to initialize the page here
                    if(!Page.IsPostBack)
                    {
						try
						{
							fillEmpresasComFaxesJaEnviados(); //ja inclui o fillFaxesEnviados
							//lbEmpresasComFax.SelectedIndex = 0; //marcar o primeiro pq as listboxes nao se marcam automaticamente como as dropdowns
							fillFaxesEnviados(); 
							dgFaxesEnviados.SelectedIndex =0; 
							fillDGEquipamentosAvisadosPorFax(); 
							fillContactos(lbEmpresasComFax.SelectedValue); 
						}
						catch
						{
                            lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
							btnSubmit.Enabled=false; 

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
            lbEmpresasComFax.SelectedIndexChanged += new System.EventHandler(lbEmpresasComFax_SelectedIndexChanged);
            
             dgFaxesEnviados.SelectedIndexChanged += new System.EventHandler(dgFaxesEnviados_SelectedIndexChanged);

            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            btnVer.Click += new System.EventHandler(btnVer_Click);

            rbFaxContacto.CheckedChanged += new System.EventHandler(rbFaxContacto_CheckedChanged);
            rbFaxEmpresa.CheckedChanged += new System.EventHandler(rbFaxEmpresa_CheckedChanged);
        }
		#endregion


        private void fillEmpresasComFaxesJaEnviados()
        {
            DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
            SqlDataReader dr = data.DREmpresasComFax(); 
            lbEmpresasComFax.DataSource = dr; 
            lbEmpresasComFax.DataTextField= "nome"; 
            lbEmpresasComFax.DataValueField = "idEmpresa"; 
            lbEmpresasComFax.DataBind(); 
            dr.Close(); 
			data = null; 

        }

        private void fillContactos(string idEmpresa)
        {
            // Todos os contactos activos
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaContactos(idEmpresa,"1"); 
            
            ddContacto.DataSource= DR; 
            ddContacto.DataBind(); 
            
            DR.Close(); 
			lista = null; 

            fillContactInfo(ddContacto.SelectedValue.ToString()); 
        }
        
        private void fillContactInfo(string idContacto)
        {
            //reset para vazio, no caso de nao haver dados para preencher
            txtFaxEmpresa.Text = ""; 
            txtFaxContacto.Text =""; 
            txtFaxParaEnvio.Text=""; //limpar campo!

            if(idContacto != "")
            {
                DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
                SqlDataReader dr = data.DRContactInfo(idContacto); 
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        txtFaxContacto.Text = dr["faxContacto"].ToString(); 
                        //txtEmailContacto.Text = dr["emailContacto"].ToString(); 
                        txtFaxEmpresa.Text = dr["fax"].ToString(); 
                        //txtEmailEmpresa.Text = dr["email"].ToString();
                    }
                }
                dr.Close(); 
				data = null; 
            }    
        }

        private void fillFaxesEnviados()
        {
			SqlDataReader dr; 
			
			DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
			dr = data.DRFaxesEnviados(lbEmpresasComFax.SelectedValue); 
			
			try
			{
				dgFaxesEnviados.DataSource = dr; 
				dgFaxesEnviados.DataBind(); 
				dr.Close(); 
				data = null; 

				fillDGEquipamentosAvisadosPorFax();
			}
			catch
			{
				dr.Close(); 
				data = null; 
			}
			
        }

        private void fillDGEquipamentosAvisadosPorFax()
        {
            if(dgFaxesEnviados.SelectedIndex > -1)
            {
                string idFax = dgFaxesEnviados.DataKeys[dgFaxesEnviados.SelectedIndex].ToString(); 
                if(idFax!="")
                {
                    DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
                    dgEquipamentosAvisados.DataSource = data.DTEquipamentosPorFax(idFax); 
                    dgEquipamentosAvisados.DataBind(); 
					data = null; 
                }
                else
                {
                    dgEquipamentosAvisados.Controls.Clear(); 
                }
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_FAX;
            }
        
        }

        protected void dgFaxesEnviadosa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillDGEquipamentosAvisadosPorFax(); 
        }

        private void lbEmpresasComFax_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            rbFaxContacto.Checked = false;
            rbFaxEmpresa.Checked = false; 
            rblLocalLavantamento.SelectedIndex =-1; 
            ////dgFaxesEnviados.SelectedIndex = 0; //marcar logo o primeiro
            fillFaxesEnviados(); 
            fillDGEquipamentosAvisadosPorFax(); 
            fillContactos(lbEmpresasComFax.SelectedValue.ToString());
        }

      
        protected void dgFaxesEnviados_SelectedIndexChanged(object sender, System.EventArgs e)
        {
                fillDGEquipamentosAvisadosPorFax(); 
        }


        private void rbFaxContacto_CheckedChanged(object sender, System.EventArgs e)
        {
            if(rbFaxContacto.Checked == true)
            {
                rbFaxEmpresa.Checked=false; 
                txtFaxParaEnvio.Text = txtFaxContacto.Text; 
            }
        }

        private void rbFaxEmpresa_CheckedChanged(object sender, System.EventArgs e)
        {
            if(rbFaxEmpresa.Checked == true)
            {
                rbFaxContacto.Checked=false; 
                txtFaxParaEnvio.Text = txtFaxEmpresa.Text; 
            }
        }

        //abre fax em pdf
        private void btnVer_Click(object sender, System.EventArgs e)
        {
            if(ddContacto.Items.Count ==0)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NAO_EXISTEM_CONTACTOS; 
                return; 
            }
            if(ddContacto.SelectedValue =="")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_CONTACTO; 
                return; 
            }
            if(txtFaxParaEnvio.Text=="")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_FAX; 

                return; 
            }

            crFaxAviso report = new crFaxAviso(); 

			clsReport cr = new clsReport();
            
			DATA.FaxAvisoBD data = new LabMetro.DATA.FaxAvisoBD(); 

            report.SetParameterValue("@inFuncionario", data.nomeFuncionario(User.Identity.Name.ToString()));
            report.SetParameterValue("@inFaxNumber",txtFaxParaEnvio.Text); 
			report.SetParameterValue("@inLocalLevantamento",rblLocalLavantamento.SelectedValue.ToString()); 

			DataSet ds  = data.DSFaxAvisoEnviados(lbEmpresasComFax.SelectedValue,ddContacto.SelectedValue, dgFaxesEnviados.DataKeys[dgFaxesEnviados.SelectedIndex].ToString()); 

			report.SetDataSource(ds);

            ds = null;
            data = null; 
			cr.exportReportToPDF(report,"Fax");

            //cr = null; 
            //report = null; 
		

        }

        //Manda fax
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {

            if(ddContacto.Items.Count ==0)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NAO_EXISTEM_CONTACTOS; 
                return; 
            }
            if(ddContacto.SelectedValue =="")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_CONTACTO; 
                return; 
            }
            if(txtFaxParaEnvio.Text=="")
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INDICAR_FAX; 
                return; 
            }

            crFaxAviso report = new crFaxAviso(); 
            clsReport cr = new clsReport();

            DATA.FaxAvisoBD data = new LabMetro.DATA.FaxAvisoBD(); 
         
			string faxNumber = txtFaxParaEnvio.Text.Replace(" ",""); 
			report.SetParameterValue("@inFuncionario", data.nomeFuncionario(User.Identity.Name.ToString()));
            report.SetParameterValue("@inFaxNumber",faxNumber); 
			report.SetParameterValue("@inLocalLevantamento",rblLocalLavantamento.SelectedValue.ToString()); 
            

            string id = DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString();
            string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_FAXAVISO"];
			DataSet ds = data.DSFaxAvisoEnviados(lbEmpresasComFax.SelectedValue,ddContacto.SelectedValue, dgFaxesEnviados.DataKeys[dgFaxesEnviados.SelectedIndex].ToString()); 

				report.SetDataSource(ds); 

				//cr.faxReport(report,faxNumber,mailSender,"FAX",id);
                cr.sendFaxNovo(report, faxNumber,  "FAX", id);
            
				//insere na tabela faxes enviados e faz update à tabela serviço
				//com o id do fax inserido.

				//aqui faz um update à data de envio do fax
				int i = data.updateFaxData(dgFaxesEnviados.DataKeys[dgFaxesEnviados.SelectedIndex].ToString(),Session["UserId"].ToString(),rblLocalLavantamento.SelectedValue.ToString()); 
				if(i == 1) //correu bem
				{
                
					fillFaxesEnviados(); 
                
				}
			
			data = null; 
			cr = null; 
			report = null; 
			ds = null;
            
        }

	}
}
