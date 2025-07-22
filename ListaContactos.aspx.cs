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
using System.Text.RegularExpressions;
using System.Configuration;

namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaContactos.
	/// </summary>
	public partial class ListaContactos : System.Web.UI.Page
	{
        private const string ID_PAG = "CONTACTOS_0";//NOME DA PAGINA

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
                    if(!Page.IsPostBack)
                    {
						ViewState["sortField"] = "nome";
						ViewState["sortDirection"] = "ASC";
                        if(Request.QueryString["id"]!= null && Request.QueryString["id"].ToString()!="")        
                        {
							fillDDEmpresa(); 

                            try
                            {
								ddEmpresa.SelectedValue=Request.QueryString["id"].ToString(); 
                                BindGrid(); 
								//fillEtiqueta(); 
                            }
                            catch
                            {
                            }
                        }  
                    }
                    if(!ht.ContainsKey("CONTACTOS_1")) 
                    //se n tem permissoes para ver os detalhes dos contactos, desactivar o link
                    {
                        DGContactos.Columns[3].Visible=false; 
                    }		
                }
            }
        }

		private void fillDDEmpresa()
		{

			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,txtPesquisaNif.Text,"","","","","","",""); 
		 	DataView DV = new DataView(DT);
			
			if(cbSede.Checked) DV.RowFilter ="sede = 1";
			else DV.RowFilter = String.Empty;

            DV.Sort = "nome ASC"; 
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaNif.Text == "") &&(txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}

        private void BindGrid()
        {  
			DATA.ContactosBD contactos = new LabMetro.DATA.ContactosBD();
			try
			{
           
			int activo = 1; 
			if(chbActivo.Checked==false) activo = 0; 
			DataTable DT = contactos.DTFillContacts(ddEmpresa.SelectedValue,txtNomeContacto.Text,activo.ToString(),txtEmail.Text); 
            DataView DV = new DataView(DT); 
            DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
	        
				DGContactos.DataSource = DV; 
				DGContactos.DataBind(); 
				
				
				if(DV.Table.Rows.Count > 0)
				{
					DGContactos.Visible=true;
				}
				else
				{
					DGContactos.Dispose();
					DGContactos.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
					
				}
			}
			catch(Exception ex)
			{

				DGContactos.Dispose();
				DGContactos.Visible=false; 
				lblMessage.Text= ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
			}
			Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			int intAcesso = System.Convert.ToInt32(ht["CONTACTOS_1"]); 
			//if(intAcesso == 1)//tem permissoes para tudo. 
			if(intAcesso ==0) 
			{
				//se n tem permissoes para ALTERAR CONTACTOS, NAO APARECE A COLUNA DE EDITAR
			
				DGContactos.Columns[14].Visible=false; 
                        
			}
			contactos = null; 
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
			
			
			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
			btnPesquisaEmpresa.Click += new System.EventHandler(btnPesquisaEmpresa_Click);
			ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
//			btnListaUtilizadores.Click += new System.EventHandler(btnLista_Click);
			DGContactos.ItemDataBound += new DataGridItemEventHandler(dgContactos_ItemDataBound); 
			DGContactos.ItemCommand +=new DataGridCommandEventHandler(dgContactos_ItemCommand);
			
		}
		#endregion

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


        

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGContactos.CurrentPageIndex = e.NewPageIndex;
			BindGrid(); 
         }

        private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DGContactos.CurrentPageIndex=0; 
            BindGrid(); 
			//fillEtiqueta(); 
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

        private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
        {
			fillDDEmpresa(); 
			//txtEtiqueta.Text="";
        }

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			DGContactos.DataSource = null;
			DGContactos.DataBind(); 
			fillDDEmpresa(); 
			//txtEtiqueta.Text="";
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			DGContactos.DataSource = null;
			DGContactos.DataBind(); 
			fillDDEmpresa(); 
			//txtEtiqueta.Text="";
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			    
			DGContactos.CurrentPageIndex =0; 
			BindGrid(); 
			//fillEtiqueta(); 
			
		}


        //private void btnLista_Click(object sender, System.EventArgs e)
        //{
        //    REPORTS.rptContactosUtilizadores report = new LabMetro.REPORTS.rptContactosUtilizadores(); 
        //    GERAL.clsReport  cr = new LabMetro.GERAL.clsReport(); 

        //    LabMetro.DATASETS.DsContactosUtilizadores ds = new LabMetro.DATASETS.DsContactosUtilizadores(); 

        //    string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
        //    using (SqlConnection objConn = new SqlConnection(connectionString)) 
        //    using (SqlCommand objCmd = new SqlCommand())
        //    {
        //        objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
        //        objCmd.Connection = objConn; 
				

        //        objCmd.CommandText="select distinct empresa.nome as empresa, contacto.nome  as contacto, empresa.localidade  from contacto inner join empresa on contacto.idEmpresa = Empresa.idEmpresa  where bCertificados = 1 and userId is null order by 1,2"; ; 
        //        SqlDataAdapter DA = new SqlDataAdapter(objCmd);

        //        DA.Fill(ds, "dtContactosUtilizadores");
				
        //        report.SetDataSource(ds); 
        //        cr.exportReportToPDF(report);
				
        //        DA.Dispose();
        //        DA = null; 
        //    }
        //    ds = null; 
        //    cr = null; 
        //    report = null;
			

        //}


		protected System.Drawing.Color ConvertColor(int i)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();

			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Red");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}

			return colorName; 
		}

		

		protected string strX(bool b)
		{
			if(b==true) return "x"; 
			return "";  
		}

		protected void dgContactos_edit(Object sender, DataGridCommandEventArgs e)     
		{
			DGContactos.ShowFooter=false;     
			DGContactos.EditItemIndex = e.Item.ItemIndex;	
			BindGrid();
		}

		protected void dgContactos_cancel(Object sender, DataGridCommandEventArgs e)
		{
			DGContactos.ShowFooter=true;  
			DGContactos.EditItemIndex = -1;
			BindGrid();
		}
		
		protected void dgContactos_update(Object sender, DataGridCommandEventArgs e)
		{
			string idContacto = DGContactos.DataKeys[e.Item.ItemIndex].ToString();
			
			TextBox txtNome = (TextBox) e.Item.Cells[1].Controls[0];
			TextBox txtDepartamento = (TextBox) e.Item.Cells[4].Controls[0];
			TextBox txtCargo = (TextBox) e.Item.Cells[5].Controls[0];
			TextBox txtTelefone = (TextBox) e.Item.Cells[6].Controls[0];
			TextBox txtFax = (TextBox) e.Item.Cells[7].Controls[0];
			TextBox txtEmail = (TextBox) e.Item.Cells[8].Controls[0];

			CheckBox chbFacturacao = (CheckBox)e.Item.FindControl("chbFacturacao");
			CheckBox chbOrcamento = (CheckBox)e.Item.FindControl("chbOrcamento");
			CheckBox chbRequisicoes = (CheckBox)e.Item.FindControl("chbRequisicoes");
			CheckBox chbManutencao = (CheckBox)e.Item.FindControl("chbManutencao");
			CheckBox chbQualidade = (CheckBox)e.Item.FindControl("chbQualidade");
			CheckBox chbCertificados = (CheckBox)e.Item.FindControl("chbCertificados");
			CheckBox chbActivo = (CheckBox)e.Item.FindControl("cbEstado");
            CheckBox chbGestaoEquipamentos = (CheckBox)e.Item.FindControl("chbGestaoEquipamentos");

			
			DATA.ContactosBD contacto = new LabMetro.DATA.ContactosBD(); 
			if(txtNome.Text=="")
			{
				lblMessage.Text ="Tem de inserir um nome";
				return;
			}

			if(chbCertificados.Checked==true)
			{
				if(txtEmail.Text=="")
				{
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERIR_EMAIL; 
					return; 
				}
			}

            if (chbGestaoEquipamentos.Checked == true)
            {
              if(chbCertificados.Checked==false)
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DAR_ACESSO_LABMETRO_ONLINE; 
                    return;
                }
            }

			
			if(txtEmail.Text!="" && IsValidEmail(txtEmail.Text) ==false)
			{
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_EMAIL_FORMATO_INVALIDO;
				return; 
			}

            contacto.UpdateContactCurto(idContacto, txtNome.Text, txtCargo.Text, txtTelefone.Text, txtFax.Text, txtEmail.Text, chbFacturacao.Checked.ToString(), chbOrcamento.Checked.ToString(), chbQualidade.Checked.ToString(), chbManutencao.Checked.ToString(), chbCertificados.Checked.ToString(), chbRequisicoes.Checked.ToString(), chbActivo.Checked.ToString(), HttpContext.Current.User.Identity.Name.ToString(), txtDepartamento.Text,  chbGestaoEquipamentos.Checked.ToString()); 
				  
			contacto = null;
			DGContactos.EditItemIndex = -1; 
			BindGrid();
		
		}
    
		private void dgContactos_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView DRV = (DataRowView) e.Item.DataItem;

				TextBox txtNome = (TextBox) e.Item.Cells[1].Controls[0];
				TextBox txtDepartamento = (TextBox) e.Item.Cells[4].Controls[0];
				TextBox txtCargo = (TextBox) e.Item.Cells[5].Controls[0];
				TextBox txtTelefone = (TextBox) e.Item.Cells[6].Controls[0];
				TextBox txtFax = (TextBox) e.Item.Cells[7].Controls[0];
				TextBox txtEmail = (TextBox) e.Item.Cells[8].Controls[0];
				
				txtDepartamento.Width=80; 
				txtTelefone.Width=80;
				txtFax.Width=80;
				txtEmail.Width = 100; 
				txtNome.Width=100;
				txtCargo.Width=100;

				CheckBox chbFacturacao = (CheckBox)e.Item.FindControl("chbFacturacao");
				CheckBox chbOrcamento = (CheckBox)e.Item.FindControl("chbOrcamento");
				CheckBox chbRequisicoes = (CheckBox)e.Item.FindControl("chbRequisicoes");
				CheckBox chbManutencao = (CheckBox)e.Item.FindControl("chbManutencao");
				CheckBox chbQualidade = (CheckBox)e.Item.FindControl("chbQualidade");
				CheckBox chbCertificados = (CheckBox)e.Item.FindControl("chbCertificados");
                CheckBox chbActivo = (CheckBox)e.Item.FindControl("cbEstado");
                CheckBox chbGestaoEquipamentos = (CheckBox)e.Item.FindControl("chbGestaoEquipamentos");

				chbFacturacao.Checked = System.Convert.ToBoolean(DRV["bFacturacao"].ToString());
				chbOrcamento.Checked = System.Convert.ToBoolean(DRV["bOrcamento"].ToString());

				chbRequisicoes.Checked = System.Convert.ToBoolean(DRV["bRequisicoes"].ToString());
				chbManutencao.Checked =System.Convert.ToBoolean(DRV["bManutencao"].ToString());

				chbQualidade.Checked = System.Convert.ToBoolean(DRV["bQualidade"].ToString());
				chbCertificados.Checked = System.Convert.ToBoolean(DRV["bCertificados"].ToString());
				chbActivo.Checked = System.Convert.ToBoolean(DRV["activo"].ToString());
                chbGestaoEquipamentos.Checked = System.Convert.ToBoolean(DRV["bGestaoEquipamentos"].ToString()); 

				if(validaUsersContactInfo())//se user logado pode altarar os valores das checkboxes
				{
                    chbFacturacao.Enabled=true; 
					chbOrcamento.Enabled =true;
					chbRequisicoes.Enabled = true;
					chbManutencao.Enabled = true;
					chbQualidade.Enabled = true;
		            chbCertificados.Enabled=true;
                    chbGestaoEquipamentos.Enabled = true;
					//este ainda é dúvidoso se são estes users que vao controlar, mas por enquanto fica assim
					//não são os users que controlam, fica marcado se é associados um userId ao contacto 	
				}
				else
				{
					chbFacturacao.Enabled=false; 
					chbOrcamento.Enabled =false;
					chbRequisicoes.Enabled = false;
					chbManutencao.Enabled = false;
					chbQualidade.Enabled = false;
					chbCertificados.Enabled=false;
                    chbGestaoEquipamentos.Enabled = false;
				}
			}
		}

		private void dgContactos_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
//			
		}
		bool IsValidEmail(string strIn)
		{
			// Return true if strIn is in valid e-mail format.
			return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); 
		}

	}
}
