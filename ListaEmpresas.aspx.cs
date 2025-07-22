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

namespace LabMetro
{
    /// <summary>
    /// Summary description for Empresas.
    /// </summary>
    public partial class Empresas : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.TextBox txtEmpresa;
        protected System.Web.UI.WebControls.PlaceHolder menuPlaceHolder;
		protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;

		DataView DV;
        
		private const string ID_PAG = "EMPRESAS_0";//NOME DA PAGINA
    
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Page.Form.DefaultButton = btnPesquisa.UniqueID; 

            lblMessage.Text ="";
            gv.DataSource = null;
            gv.DataBind();
            
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
                    //no caso de ser uma pagina do tipo 0, nao ha nada a validar,
                    //a nao ser links para paginas seguintes, 
                    //ex: listaempresas tem link para formempresas e formcontactos
                    
                    if(!Page.IsPostBack)
                    {
                        ViewState["sortField"] = "nomeComprido";
                        ViewState["sortDirection"] = "ASC";
						fillDropDowns();
                     
                        ListItem item = new ListItem("...", "");
                       // ddConcelho.Items.Insert(0, item);
                       // ddConcelho.SelectedIndex = 0;

                        ddDistrito.Items.Insert(0, item);
                        ddDistrito.SelectedIndex = 0;

                        ddActividade.Items.Insert(0, item);
                        ddActividade.SelectedIndex = 0;

                        ddPais.Items.Insert(0, item);
                        ddPais.SelectedIndex = 0;


                    }

                    if(!ht.ContainsKey("EMPRESAS_1")) //se n tem permissoes para ver os detalhes da empresas, desactivar o link
                    {
                        DGEmpresas.Columns[5].Visible=false;     
                    }

                    if(!ht.ContainsKey("CONTACTOS_0")) //se n tem permissoes para ver a lista dos Contactos, desactivar o link
                    {
                        DGEmpresas.Columns[6].Visible=false; 
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
			txtNomeEmpresa.TextChanged += new System.EventHandler(txtNomeEmpresa_TextChanged);
			txtNIF.TextChanged += new System.EventHandler(txtNIF_TextChanged);
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
		}


        #endregion

        private void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DGEmpresas.CurrentPageIndex=0; 
            BindGrid(); 
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT Empresa.idEmpresa, Empresa.nome, Empresa.nif, Empresa.morada, Empresa.codigoPostal, Empresa.localidade, Concelho.descricao AS concelho, Distrito.descricao AS distrito, Pais.descricao AS pais, Empresa.telefone, Empresa.telefone2, Empresa.fax, Empresa.email, Empresa.numClienteSAP, Actividade.descricao AS actividade, EstadoEmpresa.descricao AS estado,contacto.nome, contacto.cargo, contacto.departamento, contacto.telefoneempresa, contacto.faxempresa, contacto.emailempresa FROM Empresa INNER JOIN  EstadoEmpresa ON Empresa.idEstadoEmpresa = EstadoEmpresa.idEstadoEmpresa LEFT OUTER JOIN Actividade ON Empresa.idActividade = Actividade.idActividade LEFT OUTER JOIN Concelho ON Empresa.idConcelho = Concelho.idConcelho LEFT OUTER JOIN Distrito ON Concelho.idDistrito = Distrito.idDistrito LEFT OUTER JOIN Pais ON Empresa.idPais = Pais.idPais left join contacto on contacto.idEMpresa = empresa.idEmpresa";

            DataSet ds = GERAL.clsDataAccess.ExecuteDS(strSQL);

            //criar aqui uma outra pesquisa que a pesquisa principal
            DV = new DataView(ds.Tables[0]);

            
            gv.DataSource = DV;
            gv.DataBind();

            ds = null;


            string nomeexcel =  "ListaEmpresas.xls";
            GERAL.GridViewExportUtil.Export(nomeexcel, gv);
        }

		private void fillDropDowns()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 

			SqlDataReader dr = lista.DRListaEstadosEmpresas(); 
			ddEstado.DataSource = dr; 
			ddEstado.DataBind(); 
			dr.Close(); 
			ddEstado.Items.Insert(0,new ListItem("...","")); 

			SqlDataReader DR = lista.DRListaTiposEmpresa(); 
			ddTipoEmpresa.DataSource = DR; 
			ddTipoEmpresa.DataBind(); 
			DR.Close(); 
			ddTipoEmpresa.Items.Insert(0,new ListItem("...",""));

			lista = null; 

			DATA.SapBD sap  = new LabMetro.DATA.SapBD(); 
			SqlDataReader dr1 = sap.DRCodigoBloqueioSAP(); 
			ddCodigoBloqueioSap.DataSource = dr1; 
			ddCodigoBloqueioSap.DataBind(); 
			dr1.Close(); 
			ddCodigoBloqueioSap.Items.Insert(0,new ListItem("...",""));
			
			sap = null; 

			DATA.FacturaData f = new LabMetro.DATA.FacturaData(); 

			
			dr = f.drCondicoesPagamento();

			ddCondPagamento.DataTextField= "descricao"; 
			ddCondPagamento.DataValueField= "id"; 
			ddCondPagamento.DataSource = dr; 
			ddCondPagamento.DataBind(); 
			dr.Close(); 
			dr = null; 
			f = null; 

			ddCondPagamento.Items.Insert(0,new ListItem("...","")); //isto tem de ser tirado depois, not null
			ddCondPagamento.Items.Add(new ListItem("sem condições","0")); //mando  o zero em vez do null
																			//mando um numero qq sem ser ZERO
		}



		private DataSet DS()
		{
			DataSet ds = new DATASETS.DSEmpresa(); 

			string strPagamentoAtraso = ""; 
			if(cbPagamentoAtraso.Checked == true) strPagamentoAtraso = "1"; 


			string strRequisicaoAtraso = ""; 
			if(chRequisicaoAtraso.Checked == true) strRequisicaoAtraso = "1"; 

			string strSede = ""; 
			if(cbSede.Checked == true) strSede = "1"; 


			string strCertifsSemReq = ""; 
			if(cbCertifsSemReq.Checked == true) strCertifsSemReq = "1"; 

			string strPodeFacturarSemRequisicao = ""; 
			if(cbPodeFacturarSemRequisicao.Checked == true) strPodeFacturarSemRequisicao = "1";

            string strGestaoEquipamentos = "";
            if (cbGestaoEquipamentos.Checked == true) strGestaoEquipamentos = "1"; 
		

			SqlParameter[] arrParams = new SqlParameter[23];
			arrParams[0] = new SqlParameter("@inNome", txtNomeEmpresa.Text);
			arrParams[1] = new SqlParameter("@inNif", txtNIF.Text);
			
			arrParams[2] = new SqlParameter("@inIdEstadoEmpresa", ddEstado.SelectedValue);
			arrParams[3] = new SqlParameter("@inIdTipoEmpresa", ddTipoEmpresa.SelectedValue);

			arrParams[4] = new SqlParameter("@inCodigoBloqueioSAP", ddCodigoBloqueioSap.SelectedValue);
			arrParams[5] = new SqlParameter("@inPagamentoAtraso", strPagamentoAtraso);
			arrParams[6] = new SqlParameter("@inNivelBloqueioLabmetro", ddNivelBloqueio.SelectedValue);
			arrParams[7] = new SqlParameter("@inRequisicaoAtraso",strRequisicaoAtraso);
			arrParams[8] = new SqlParameter("@inNumCliente",txtNumClienteSAP.Text);

			if(ddCondPagamento.SelectedValue !="")  
			{
				arrParams[9] = new SqlParameter("@inIdCondicoesPagamento",System.Convert.ToInt16(ddCondPagamento.SelectedValue));
			}
			else
			{
				arrParams[9] = new SqlParameter("@inIdCondicoesPagamento",System.DBNull.Value);
			}
			arrParams[10] = new SqlParameter("@inSede",strSede);
			arrParams[11] = new SqlParameter("@inCertifsSemReq",strCertifsSemReq);
			arrParams[12] = new SqlParameter("@inConcessionario","");
			arrParams[13] = new SqlParameter("@inCentroB","");
			arrParams[14] = new SqlParameter("@inCTA","");
			arrParams[15] = new SqlParameter("@inAGE", "");
			arrParams[16] = new SqlParameter("@inSistemaPesagem","");
			arrParams[17] = new SqlParameter("@bPodeFacturarSemRequisicao",strPodeFacturarSemRequisicao);
            arrParams[18] = new SqlParameter("@idActividade", ddActividade.SelectedValue);
            arrParams[19] = new SqlParameter("@idConcelho", ddConcelho.SelectedValue);
            arrParams[20] = new SqlParameter("@idDistrito", ddDistrito.SelectedValue);
            arrParams[21] = new SqlParameter("@idPais", ddPais.SelectedValue);
            arrParams[22] = new SqlParameter("@bGestaoEquipamentos", strGestaoEquipamentos);



		
			ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

			try
			{
				ds = GERAL.clsDataAccess.DSFillDS_SP_Params("stpGetListEmpresas",ds,"dtEmpresa",arrParams);

				return ds; 
			}
			catch
			{
				return null; 
			}

		}


        private void BindGrid()
		{

			DataSet ds = DS(); 
		
			if(ds != null)
			{
		
				DV = new DataView(ds.Tables["dtEmpresa"]);
				DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
				DGEmpresas.DataSource =DV; 
				DGEmpresas.DataBind(); 
            
				if(DV.Table.Rows.Count > 0)
				{
					DGEmpresas.Visible=true;
				}
				else
				{
					DGEmpresas.Dispose();
					DGEmpresas.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
				}

			}

			ds = null; 

			//empresa = null; 

        }
       
        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGEmpresas.CurrentPageIndex = e.NewPageIndex; 
            BindGrid(); 
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


		private void txtNomeEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			DGEmpresas.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void txtNIF_TextChanged(object sender, System.EventArgs e)
		{
			DGEmpresas.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected string ConvertePagamentoAtraso(bool b)
		{
			if (b==true) 
			{
				return "sim";
			}
			else
			{
				return "não"; 
			}

		}

		private void btnReport_Click(object sender, System.EventArgs e)
		{
			//copiado tb para a pagina formreplistas
			try
			{
				DataSet ds = new DATASETS.DSEmpresa(); 

				if(ds != null)
				{
					ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!
					ds =  GERAL.clsDataAccess.DSFillDS_SP("stpGetListEmpresasPagAtrasoFalCont",ds,"dtEmpresa");

					rptEmpresasPagamentoAtraso report = new rptEmpresasPagamentoAtraso();
					clsReport cr = new clsReport();
				
					report.SetDataSource(ds); 		
                    ds = null;
					cr.exportReportToPDF(report,"Report");
	
                    //report = null;
                    //cr = null;
					 
				}
			}
			catch(Exception ex)
			{
				GERAL.clsWriteError.WriteLog("Lista Empresas, linha 321 - " + ex.ToString()); 
			}
		}


		protected System.Drawing.Color ConvertColor(int i, string codigoBloqueioSAP)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();
			 


			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Gold");
					break;
				case 2:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("DarkOrange");
					break;
				case 3: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Crimson");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}

			if(codigoBloqueioSAP =="03") //se a empresa está inactiva em SAP (n tem a ver com o nivelBloqueiolabmetro, martelada...)
			{
				colorName = (System.Drawing.Color)colConvert.ConvertFromString("PowderBlue");
			}
			return colorName; 
		}

        protected void ddConcelhoDataBound(object sender, EventArgs e)
        {

            if (ddDistrito.SelectedValue == "") ddConcelho.Items.Clear();
            ddConcelho.Items.Insert(0, new ListItem("...", ""));
        }

        protected void ddDistritoDataBound(object sender, EventArgs e)
        {


        }
        protected string strX(bool b)
        {
            if (b == true) return "x";
            return "";
        }
    }
}
