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
	/// Summary description for GestLocalEquipamento.
	/// </summary>
	public partial class GestLocalEquipamento : System.Web.UI.Page
	{
		private const string ID_PAG = "ESTADOS_EQUIP_1";//NOME DA PAGINA //neste momento a mesma permissao que o mudar estados equipamento. 

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
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
			btnSelectAll.Click += new System.EventHandler(btnSelectAll_Click);
			btnDeselectAll.Click += new System.EventHandler(btnDeselectAll_Click);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			

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

					btnSubmit.Attributes.Add("onClick","javascript:if(confirm('Confirma mudança de estado?')== false) return false;");
					if(!Page.IsPostBack)
					{

						ViewState["sortField"] = "idGuiaTransporte"; 
						ViewState["sortDirection"] = "DESC";
			
						fillDDLocal(); 
                      
					}
				}
			}
		}



		//===============================================================
		//DataTable que retorna todos os servicos associados a uma guira de transporte
		//===============================================================
		public DataTable DTServicos()
		{		
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@idEmpresa", ddEmpresa.SelectedValue);
			arrParams[1] = new SqlParameter("@refGuia", txtRefGuia.Text);
			arrParams[2] = new SqlParameter("@refBre", txtBRE.Text);
			arrParams[3] = new SqlParameter("@destinatario", txtDestinatario.Text);
			arrParams[4] = new SqlParameter("@localCarregamento", txtLocalCarregamento.Text);
			arrParams[5] = new SqlParameter("@localDescarregamento", txtLocalDesCarregamento.Text);
			arrParams[6] = new SqlParameter("@localActual", ddLocal.SelectedValue);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicosComGuiaTransporte", arrParams); 
		}


		
		//==============================================================================================
		//==============================================================================================
		private void BindGrid()
		{
			
			DataTable DT = DTServicos(); 
            
			DataView DV = new DataView(DT);
          
			if(Page.IsPostBack)
			{
				string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
				DV.Sort = strSort; 
			}

			dgServicos.DataSource =DV; 
			dgServicos.DataBind(); 

            
			//sao boundcolumns e quero esconder a ultima q tem o campo idServico.

			//ele aqui so vê uma, a primeiro, q nao vem da datasource
			//int n = dgEstadosServico.Columns.Count; 


			//            dgEstadosServico.Columns[n-2].Visible=false; //idServico
			//            dgEstadosServico.Columns[n-1].Visible=false; //cb
			//===================================================================

//			if(DV.Table.Rows.Count > 0)
//			{
//				dgEstadosServico.Visible=true;
//			}
//			else
//			{
//				dgEstadosServico.Dispose();
//				dgEstadosServico.Visible=false; 
//				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
//			}
//			
//			estado = null; 
		}


		//==============================================================================================
		//==============================================================================================
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{


			if(ddLocalNovo.SelectedIndex == 0)
			{
				lblMessage.Text ="Seleccione a localização por favor."; 
				return; 
			}
			else

			{
				//isto aqui funciona quando nao é feito o paging
				//se for feito o pagina, o checked nao se lê pelo datagrid, 
				//mas pela datasource (datatable,ou dataview)
				string strIds =""; 
				
				foreach(DataGridItem dgi in dgServicos.Items) 
				{ 
					CheckBox myCheckBox =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
					if(myCheckBox.Checked == true)
					{
				
						strIds+= dgServicos.DataKeys[dgi.ItemIndex].ToString();
						strIds+=",";
					}   
				}

				
				string delimStr = ",";
				char [] delimiter = delimStr.ToCharArray();
            
				strIds = strIds.TrimEnd(delimiter);//tem de ser senao manda um vazio no ultimo item
				string [] idsServicos = strIds.Split(delimiter); 
            

				//            Response.Write(idsServicos.Length.ToString()); 
				//            Response.Write(idsServicos[0].ToString()); 
				if(idsServicos.Length ==0)
				{
                
					btnSubmit.Attributes.Remove("onClick"); 
				}
				else
				{
					DATA.GuiaTransporteBD data = new LabMetro.DATA.GuiaTransporteBD(); 
					lblMessage.Text= data.UpdateLocalizacaoServicos(ddLocalNovo.SelectedValue,Session["UserId"].ToString(),strIds).ToString(); 
					data = null; 
            
					//pôr tudo no estado inicial
					resetData(); 


				}
			}

		}
		//==============================================================================================
		//==============================================================================================
		private void btnSelectAll_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in dgServicos.Items) 
			{ 
				CheckBox chb =  (CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				chb.Checked=true; 
			}
		}

		//==============================================================================================
		//==============================================================================================
		private void btnDeselectAll_Click(object sender, System.EventArgs e)
		{
        
			foreach(DataGridItem dgi in dgServicos.Items) 
			{ 
				CheckBox chb =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				chb.Checked=false; 
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

		private void fillDDLocal()
		{

			//local mudar para
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
			SqlDataReader DR = lista.DRListaLocalCalibracao(); 
			ddLocalNovo.DataSource=DR;
			ddLocalNovo.DataBind(); 
			ddLocalNovo.Items.Insert(0,new ListItem("",""));  
			DR.Close(); 

			//local da pesquisa

			SqlDataReader dr = lista.DRListaLocalCalibracao(); 
			ddLocal.DataSource=dr;
			ddLocal.DataBind(); 
			ddLocal.Items.Insert(0,new ListItem("",""));  
			dr.Close(); 


			lista = null; 

		}

		
		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			dgServicos.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			dgServicos.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			dgServicos.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			dgServicos.CurrentPageIndex=0; 
			BindGrid();
		}		



		
		//==============================================================================================
		//==============================================================================================
		private void btnLimparCampos_Click(object sender, System.EventArgs e)
		{
		
			resetData(); 
		}

		private void resetData()
		{
		
			txtPesquisaEmpresa.Text =""; 
			txtBRE.Text=""; 
			txtDestinatario.Text =""; 
			txtLocalCarregamento.Text=""; 
			txtLocalDesCarregamento.Text =""; 

			fillDDEmpresa(); 

			dgServicos.DataSource = null; 
			dgServicos.DataBind(); 
		}
		
		

		//==============================================================================================
		//pesquisa para retornar os serviços de acordo com os critérios indicados
		//==============================================================================================
		
		
		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
//			if((ddEmpresa.SelectedValue=="") && (txtRefGuia.Text==""))
//			{
//				lblMessage.Text ="Tem de preencher indicar a empresa ou o Guia Trasporte"; 
//			}
//			else
//			{	
				//dgServicos.CurrentPageIndex=0; 
				BindGrid();	
			//}
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


	}
}
