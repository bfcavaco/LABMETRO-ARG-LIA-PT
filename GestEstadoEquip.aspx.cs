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
using LabMetro.REPORTS;
using LabMetro.GERAL;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;


namespace LabMetro
{
	/// <summary>
	/// Summary description for GestEstadoEquip.
	/// </summary>
    public partial class GestEstadoEquip : System.Web.UI.Page
    {

		protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
		protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator4;
		protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;
                private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();
        
        private const string ID_PAG = "ESTADOS_EQUIP_1";//NOME DA PAGINA

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //if (myApp.ToUpper() == "ES_LABMETRO")
            //{
            //    btnImprimirEtiquetasCalibracao.Visible = false;
            //}

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

	                btnSubmit.Attributes.Add("onClick","javascript:if(confirm('OK?')== false) return false;");
                    if(!Page.IsPostBack)
                    {

                        ViewState["sortField"] = "Equipamento";
                        ViewState["sortDirection"] = "ASC";
			
                        fillDDEstadoOriginal(); 
                        fillDDEstadoDestino(); 

                       
                      
                    }
                }
            }
        }


		//==============================================================================================
		//==============================================================================================
		private void fillDDEmpresa()
		{
			if(txtPesquisaEmpresa.Text !="")
			{
				DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
				DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,"","1","","","","","",""); //activas
            
				DataView DV = new DataView(DT);
				ddEmpresa.DataSource=DV; 
				ddEmpresa.DataBind(); 
				empresa = null; 
			}
			else
			{
				ddEmpresa.Items.Clear(); 
			}
		}

		//preencher a label empresa devedora com os respectivos dados, caso aplicável
		private void fillCompanyInfo()
		{

			lblEmpresaDevedora.Text =""; //limpar a label

			LabMetro.DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			LabMetro.DATA.CompanyDetails detEmpresa = empresa.GetCompanyDetails(ddEmpresa.SelectedValue); 
			if(detEmpresa != null)
			{
			
				if (detEmpresa.pagamentoAtraso =="1")
				{
					lblEmpresaDevedora.Text = "** PAGAMENTOS EM ATRASO **<br />";
					
					//isto ou similar acho q é um bug na framwork e não dá para fazer
					//ddEmpresa.SelectedItem.Attributes.Add("style", "color:" + ds.Tables[0].Rows[i]["CategoryColor"].ToString () );
					//trEmpresa.BgColor = System.Drawing.ColorTranslator.FromHtml("#FF0033").ToString();
				}
				else
				{
					lblEmpresaDevedora.Text = ""; 
				}
				

				switch(detEmpresa.nivelBloqueioLabmetro)	
				{ 
					case "0": 
						trEmpresa.BgColor=""; 
						break;
					case "1":
						trEmpresa.BgColor ="Gold"; 
						break; 
					case "2": 
						trEmpresa.BgColor ="DarkOrange";
						lblEmpresaDevedora.Text +="Venda à dinheiro ou Pagamento do Atrasado.<br />"; 
						break; 
					case "3": 
						trEmpresa.BgColor ="Crimson";
						lblEmpresaDevedora.Text +="Venda à dinheiro.<br />"; 
						break; 
				}

				switch(detEmpresa.codigoBloqueioSAP)
				{
					case "00":  //nada mas tem de estar tratado
						break; 
					case "01": 
						lblEmpresaDevedora.Text += "** EMPRESA FALIDA **";
						break;
					case "02":
						lblEmpresaDevedora.Text += "** EMPRESA EM CONTENCIOSO **"; 
						break; 
					case "03":
						lblEmpresaDevedora.Text += "** Empresa com nºCliente SAP inactivo **"; 
						break; 
					default: //todos os outros bloqueados
						lblEmpresaDevedora.Text += "** EMPRESA COM BLOQUEIO EM SAP **"; 
						break; 
				}					
				
			}
			empresa = null; 
		}

		//==============================================================================================
		//==============================================================================================
        private void fillDDEstadoOriginal()
        {

            DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD(); 
            SqlDataReader dr = estado.DRGetListaEstadosServico(); 
            ddEstadoOrigem.DataTextField = "descricao"; 
            ddEstadoOrigem.DataValueField = "ident"; 
            ddEstadoOrigem.DataSource=dr; 
            ddEstadoOrigem.DataBind(); 

			ddEstadoOrigem.Items.Insert(0,new ListItem("","")); 
            dr.Close(); 

			estado = null; 

        }

		//==============================================================================================
		//==============================================================================================
        private void fillDDEstadoDestino()
        {
			if(ddEstadoOrigem.SelectedValue!="")
			{
				DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD(); 
				SqlDataReader dr = estado.DRGetEstadosServicosEsubsequentes(ddEstadoOrigem.SelectedValue); 
				ddEstadoDestino.DataTextField = "descricao"; 
				ddEstadoDestino.DataValueField = "idEstadoServico"; 
				ddEstadoDestino.DataSource=dr; 
				ddEstadoDestino.DataBind(); 

				//adicionado à mao pq nao ha maneira de chegar a este estado, e caso haja um
				//engano no bre, pode mudar-se aqui de recepcionado para "aguarda calib.ext"
				//que é um estado paralelo ao recepcionado.

//				if(ddEstadoOrigem.SelectedValue.ToString() == "1") 
//				{
//					ddEstadoDestino.Items.Insert(0,new ListItem("Aguarda Calibração Ext.","2")); 
//				}
//				if(ddEstadoOrigem.SelectedValue.ToString() == "2") 
//				{
//					ddEstadoDestino.Items.Insert(0,new ListItem("Recepcionado","1")); 
//				}
				dr.Close(); 

				estado = null; 
				int idPerfil = (int)Session["idPerfil"]; 
				if(idPerfil != 6  && idPerfil != 4 && idPerfil != 5) //todos os que noa podem calibrar
				{
					ListItem foundItem6 = (ListItem)ddEstadoDestino.Items.FindByValue("6"); //calibrado
					ListItem foundItem25 = (ListItem)ddEstadoDestino.Items.FindByValue("25"); //calibrado no ext
					if(foundItem6!=null) ddEstadoDestino.Items.Remove(foundItem6); 
					if(foundItem25!=null) ddEstadoDestino.Items.Remove(foundItem25); 
				}
			}
			else
			{
				ddEstadoDestino.Items.Clear();  
			}
        }

		//==============================================================================================
		//==============================================================================================
        private void BindGrid()
        {
            DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD(); 
            DataTable DT = estado.DTEquipamentoByEstado(ddEstadoOrigem.SelectedValue,ddEmpresa.SelectedValue,txtSearchEquipamento.Text, txtSearchRefBRE.Text); 
            
            DataView DV = new DataView(DT);
          
            if(Page.IsPostBack)
            {
                string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
                DV.Sort = strSort; 
            }

            dgEstadosServico.DataSource =DV; 
            dgEstadosServico.DataBind(); 

            
            //sao boundcolumns e quero esconder a ultima q tem o campo idServico.

            //ele aqui so vê uma, a primeiro, q nao vem da datasource
            //int n = dgEstadosServico.Columns.Count; 


//            dgEstadosServico.Columns[n-2].Visible=false; //idServico
//            dgEstadosServico.Columns[n-1].Visible=false; //cb
            //===================================================================

            if(DV.Table.Rows.Count > 0)
            {
                dgEstadosServico.Visible=true;
            }
            else
            {
                dgEstadosServico.Dispose();
                dgEstadosServico.Visible=false; 
                lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
            }
			
			estado = null; 
        }

        
		//==============================================================================================
		//==============================================================================================
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

		
		//==============================================================================================
		//==============================================================================================
		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {   
            dgEstadosServico.CurrentPageIndex = e.NewPageIndex;
            BindGrid(); 
        
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
			ddEstadoOrigem.SelectedIndexChanged += new System.EventHandler(ddEstadoOrigem_SelectedIndexChanged);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnSelectAll.Click += new System.EventHandler(btnSelectAll_Click);
			btnDeselectAll.Click += new System.EventHandler(btnDeselectAll_Click);
			btnLimparCampos.Click += new System.EventHandler(btnLimparCampos_Click); 
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
            }
		#endregion

		
		//==============================================================================================
		//==============================================================================================
        private void ddEstadoOrigem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
			if(ddEstadoOrigem.SelectedValue!="")
			{
				fillDDEstadoDestino(); 
				BindGrid(); 
			}
			else
			{
				ddEstadoDestino.Items.Clear();
			}
        }

		//==============================================================================================
		//==============================================================================================
        private void btnSelectAll_Click(object sender, System.EventArgs e)
        {
            foreach(DataGridItem dgi in dgEstadosServico.Items) 
            { 
                CheckBox chb =  (CheckBox)dgi.Cells[0].FindControl("checkbox"); 
                chb.Checked=true; 
            }
        }

		//==============================================================================================
		//==============================================================================================
        private void btnDeselectAll_Click(object sender, System.EventArgs e)
        {
        
            foreach(DataGridItem dgi in dgEstadosServico.Items) 
            { 
                CheckBox chb =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
                chb.Checked=false; 
            }
        }

		//==============================================================================================
		//==============================================================================================
        private void btnSubmit_Click(object sender, System.EventArgs e)
        {


			if( ddEstadoOrigem.SelectedValue=="" || ddEstadoDestino.SelectedValue=="")
			{
				lblMessage.Text ="Seleccione estados antes de submeter."; 
				return; 
			}
			else
			{
				if(ddEstadoDestino.SelectedValue=="7" || ddEstadoDestino.SelectedValue=="9")
				{
					lblMessage.Text="Só pode anular ou suspender equipamento na pasta de Ensaio, indicado a razão."; 
					return;
				}	
				else
				{
					//isto aqui funciona quando nao é feito o paging
					//se for feito o pagina, o checked nao se lê pelo datagrid, 
					//mas pela datasource (datatable,ou dataview)
					string strIds =""; 
					foreach(DataGridItem dgi in dgEstadosServico.Items) 
					{ 
						CheckBox myCheckBox =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
						if(myCheckBox.Checked == true)
						{
							strIds+= dgEstadosServico.DataKeys[dgi.ItemIndex].ToString();
							strIds+=",";
						}   
					}

					string delimStr = ",";
					char [] delimiter = delimStr.ToCharArray();
            
					strIds = strIds.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item
					string [] idsServicos = strIds.Split(delimiter); 
            

					//            Response.Write(idsServicos.Length.ToString()); 
					//            Response.Write(idsServicos[0].ToString()); 
					if(idsServicos.Length ==0) //tb tem lenght um quando noa tem nada, agora n tenho tempo de ver, deve ter algum character, aspas ou outra coisa....
					{
                
						btnSubmit.Attributes.Remove("onClick"); 
					}
					else
					{
						if(strIds!="")
						{
							DATA.EstadoEquipBD data = new LabMetro.DATA.EstadoEquipBD(); 
							lblMessage.Text= data.UpdateEstadosServicos(ddEstadoOrigem.SelectedValue,ddEstadoDestino.SelectedValue,Session["UserId"].ToString(),strIds,idsServicos.Length).ToString(); 
							data = null; 
							resetData(); 
						}
					}
				}
			}
        }

		//==============================================================================================
		//==============================================================================================
		private void btnLimparCampos_Click(object sender, System.EventArgs e)
        {
		
			resetData(); 
        }

		private void resetData()
		{			
			txtSearchRefBRE.Text=""; 
			txtSearchEquipamento.Text =""; 

			fillDDEstadoOriginal(); 
			fillDDEstadoDestino(); 
			dgEstadosServico.DataSource = null; 
			dgEstadosServico.DataBind(); 
		}
		
		//==============================================================================================
		//pesquisa para retornar os serviços de acordo com os critérios indicados
		//==============================================================================================
		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			if((ddEmpresa.SelectedValue=="") && (txtSearchRefBRE.Text==""))
			{
				lblMessage.Text ="Tem de preencher indicar a empresa ou o BRE"; 
			}
			else
			{
				BindGrid();	
			}
		}
		
		//==============================================================================================
		//pesquisa das empresas pelas condicoes inseridas na textbox empresa
		//==============================================================================================
		private void btnEmpresas_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			fillCompanyInfo(); 
			
		}

		//==============================================================================================
		//pesquisa das empresas pelas condicoes inseridas na textbox empresa
		//==============================================================================================
		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			fillCompanyInfo();
			
		}

		protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillCompanyInfo(); 
		}

        

        protected void btnImprimirEtiquetasCalibracao_Click(object sender, EventArgs e)
        {

            //isto aqui funciona quando nao é feito o paging
            //se for feito o pagina, o checked nao se lê pelo datagrid, 
            //mas pela datasource (datatable,ou dataview)
            string strIds = "";
            foreach (DataGridItem dgi in dgEstadosServico.Items)
            {
                CheckBox myCheckBox = (CheckBox)dgi.Cells[0].FindControl("checkbox");
                if (myCheckBox.Checked == true)
                {
                    strIds += dgEstadosServico.DataKeys[dgi.ItemIndex].ToString();
                    strIds += ",";
                }
            }

            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();

            strIds = strIds.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item

            if (strIds != "")
            {

                //Response.Write(strIds);

                //DATA.EstadoEquipBD data = new LabMetro.DATA.EstadoEquipBD();
                //lblMessage.Text = //data.UpdateEstadosServicos(ddEstadoOrigem.SelectedValue, ddEstadoDestino.SelectedValue, Session["UserId"].ToString(), strIds, idsServicos.Length).ToString();
                //data = null;
                //resetData();


                ReportClass report = null;

                switch (myApp)
                {
                    case "ISQ_LABMETRO":
                        report = new LabMetro.REPORTS.rptEtiquetaCalTodos();
                        break;
                    case "ANG_LABMETRO":
                        report = new LabMetro.REPORTS_ANG.rptEtiquetaCalTodos();
                        break;
                    case "ES_LABMETRO":
                        report = new LabMetro.REPORTS_ES.crEtiquetaMista();
                        break;
                    case "CV_LABMETRO":
                        report = new LabMetro.REPORTS_CV.rptEtiquetaCalTodos();
                        break;
                }

                clsReport cr = new clsReport();




                DATA.BreBD bre = new LabMetro.DATA.BreBD();
                DataSet ds = bre.DSEtiquetasCalibracao(strIds);

                report.SetDataSource(ds);
                ds = null;
                bre = null;
                cr.exportReportToPDF(report,"Report");

                
                //cr = null;
                //report = null;
               

            }
        }     
	}
}
