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
	/// Summary description for GestAvisoEmpresas.
	/// </summary>
	public partial class GestAvisoEmpresas : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.ListBox lbEmpresasComFax;
        protected System.Web.UI.WebControls.ListBox lbFaxesEnviados_;
        protected System.Web.UI.WebControls.DataGrid dgEquipamentosAvisados_;
        private const string ID_PAG = "EQUIP_CALIB_1";//NOME DA PAGINA
    
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
					Response.Redirect("Default.aspx?err=1",true);
				}
                else
                {
			        // Put user code to initialize the page here
                    if(!Page.IsPostBack)
                    {
                        fillEmpresas();
                        lbEmpresa.SelectedIndex = 0; //Seleccionar o primeiro
						fillBRE(lbEmpresa.SelectedValue); 
                        fillEquipamentos(ddBRE.SelectedValue);
                        fillContactos(lbEmpresa.SelectedValue); 
                        fillFaxesEnviados(); 

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

        protected void lbEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            rbFaxContacto.Checked = false;
            rbFaxEmpresa.Checked = false; 
			rblLocalLavantamento.SelectedIndex =-1; 
			fillBRE(lbEmpresa.SelectedValue); 
            fillEquipamentos(ddBRE.SelectedValue);
            fillContactos(lbEmpresa.SelectedValue);
            
            fillFaxesEnviados(); 
            
        }

        private void fillEmpresas()
        {
        
            DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
            SqlDataReader dr = data.DREmpresas(); 
            lbEmpresa.DataSource = dr; 
            lbEmpresa.DataTextField= "nome"; 
            lbEmpresa.DataValueField = "idEmpresa"; 
            lbEmpresa.DataBind(); 
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

		private void fillBRE(string idEmpresa)
		{
			DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
			SqlDataReader dr = data.DRBre(idEmpresa); 
			if(dr.HasRows)
			{
				ddBRE.DataSource=dr; 
				ddBRE.DataBind(); 
			}
			dr.Close(); 
			data= null; 
		}

        private void fillEquipamentos(string idBRE)
        {
            DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
            DataTable DT = data.DTEquipamentos(idBRE); 
            dgEquipamento.DataSource = DT; 
            dgEquipamento.DataBind(); 
            ViewState["DT"] = DT; 
            data=null; 


        }

        
        protected void rbFaxContacto_CheckedChanged(object sender, System.EventArgs e)
        {
            if(rbFaxContacto.Checked == true)
            {
                rbFaxEmpresa.Checked=false; 
                txtFaxParaEnvio.Text = txtFaxContacto.Text; 
            }
        }

        protected void rbFaxEmpresa_CheckedChanged(object sender, System.EventArgs e)
        {
            if(rbFaxEmpresa.Checked == true)
            {
                rbFaxContacto.Checked=false; 
                txtFaxParaEnvio.Text = txtFaxEmpresa.Text; 
            }
        }

        //abre fax em pdf
        protected void btnVer_Click(object sender, System.EventArgs e)
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

			

			DataSet ds  = data.DSFaxAviso(lbEmpresa.SelectedValue,ddContacto.SelectedValue, ddBRE.SelectedValue); 
			
		
			report.SetDataSource(ds);
            
            ds = null;
            data = null; 

            string faxNumber = txtFaxParaEnvio.Text.Replace(" ", "");
            report.SetParameterValue("@inFuncionario", data.nomeFuncionario(User.Identity.Name.ToString()));
            report.SetParameterValue("@inFaxNumber", faxNumber);
            report.SetParameterValue("@inLocalLevantamento", rblLocalLavantamento.SelectedValue); 


            cr.exportReportToPDF(report,"FaxAviso");

            //cr = null; 
            //report = null; 
			
        
        }

        //Manda fax
        protected void btnSubmit_Click(object sender, System.EventArgs e)
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

			DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
			
			DataSet ds  = data.DSFaxAviso(lbEmpresa.SelectedValue,ddContacto.SelectedValue, ddBRE.SelectedValue);

            report.SetDataSource(ds); 


            string faxNumber = txtFaxParaEnvio.Text.Replace(" ", "");

            report.SetParameterValue("@inFuncionario", data.nomeFuncionario(User.Identity.Name.ToString()));
            report.SetParameterValue("@inFaxNumber", faxNumber);
            report.SetParameterValue("@inLocalLevantamento", rblLocalLavantamento.SelectedValue); 

			
			
            string id = DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString();
            string mailSender = (string)ConfigurationManager.AppSettings["MAIL_CONFIRMATION_FAXAVISO"];
            
			//cr.faxReport(report,txtFaxParaEnvio.Text.Trim(),mailSender,"FAX",id);
            cr.sendFaxNovo(report, txtFaxParaEnvio.Text.Trim(),  "FAX", id);
            //insere na tabela faxes enviados e faz update à tabela serviço
            //com o id do fax inserido.
            DataTable DT = (DataTable)ViewState["DT"]; 
            DataView DV = new DataView(DT); 
            
			int i = data.insertFaxData(lbEmpresa.SelectedValue,ddContacto.SelectedValue,faxNumber,Session["UserId"].ToString(),rblLocalLavantamento.SelectedValue.ToString(),DV); 
			if(i == 1) //correu bem
			{
				//fazer reload aos dados, para elimnar os equipamentos que ja foram avisados.
				
				fillEmpresas();
				lbEmpresa.SelectedIndex = 0; //Seleccionar o primeiro
				fillBRE(lbEmpresa.SelectedValue); 
				fillEquipamentos(ddBRE.SelectedValue);
				fillContactos(lbEmpresa.SelectedValue); 
				fillFaxesEnviados(); 
			}
			
            report = null; 
			cr = null; 
			data = null; 
        }

        private void fillFaxesEnviados()
        {
            DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
            
            SqlDataReader dr = data.DRFaxesEnviados(lbEmpresa.SelectedValue); 
           
            lbFaxesEnviados.DataSource = dr; 
            lbFaxesEnviados.DataBind(); 
           
           // lbFaxesEnviados.SelectedIndex =0; //Marcar o primeiro

            fillDGEquipamentosAvisadosPorFax(); //preencher os equipamentos
           
            dr.Close(); 
			data = null; 
        }

        private void fillDGEquipamentosAvisadosPorFax()
        {
            if(lbFaxesEnviados.SelectedValue!="")
            {
                DATA.FaxAvisoBD data = new DATA.FaxAvisoBD(); 
                dgEquipamentosAvisados.DataSource = data.DTEquipamentosPorFax(lbFaxesEnviados.SelectedValue); 
                dgEquipamentosAvisados.DataBind(); 
				data = null; 
            }
            else
            {
                dgEquipamentosAvisados.Controls.Clear(); 
            }
        }

        protected void lbFaxesEnviados_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            fillDGEquipamentosAvisadosPorFax(); 
        }

		protected void ddBRE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillEquipamentos(ddBRE.SelectedValue); 
		}

	}
}
