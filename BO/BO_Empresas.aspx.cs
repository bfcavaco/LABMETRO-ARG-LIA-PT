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


namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_Empresas.
	/// </summary>
	public partial class BO_Empresas : System.Web.UI.Page
	{
			protected System.Web.UI.WebControls.TextBox txtEmpresa;
			protected System.Web.UI.WebControls.PlaceHolder menuPlaceHolder;
			protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;
			protected System.Web.UI.WebControls.Button btnReport;	
			protected System.Web.UI.WebControls.TextBox Textbox1;
			protected System.Web.UI.WebControls.Button btnContinuar;
			DataView DV; 
		
		//*******************************************************
		//tabelas relacionadas com a empresa pelo campo IDEMPRESA
		//		contacto
		//		bre
		//		bse
		//		subcontratobse
		//		subcontratobre
		//		equipamento
		//		guiatransporte
		//		requisicao
		//		factura
		//		orcamento
        //      taxa de serviço
		//*******************************************************
			protected void Page_Load(object sender, System.EventArgs e)
			{
				lblMessage.Text =""; 
				dgGenerico.DataSource = null;
				dgGenerico.DataBind(); 

				btnTrocar.Attributes.Add("onClick","javascript:if(confirm('Confirma troca de empresas?')== false) return false;");

				if(!Page.IsPostBack)
				{
					ViewState["sortField"] = "nomeComprido";
					ViewState["sortDirection"] = "ASC";
					fillDropDowns();   
                    BindDDEmpresas();    
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
				btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
				btnTrocar.Click += new System.EventHandler(btnTrocar_Click);
				Linkbutton1.Click += new System.EventHandler(Linkbutton1_Click);
				Linkbutton2.Click += new System.EventHandler(Linkbutton2_Click);
				Linkbutton3.Click += new System.EventHandler(Linkbutton3_Click);
				Linkbutton4.Click += new System.EventHandler(Linkbutton4_Click);
				Linkbutton5.Click += new System.EventHandler(Linkbutton5_Click);
				
			}


			#endregion

		
			private void fillDropDowns()
			{
				DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 

				SqlDataReader dr = lista.DRListaEstadosEmpresas(); 
				ddEstado.DataSource = dr; 
				ddEstado.DataBind(); 
				dr.Close(); 
				ddEstado.Items.Insert(0,new ListItem("","")); 

				SqlDataReader DR = lista.DRListaTiposEmpresa(); 
				ddTipoEmpresa.DataSource = DR; 
				ddTipoEmpresa.DataBind(); 
				DR.Close(); 
				ddTipoEmpresa.Items.Insert(0,new ListItem("",""));

				lista = null; 	
			}


			private void btnPesquisa_Click(object sender, System.EventArgs e)
			{
				DGEmpresas.CurrentPageIndex=0;
				BindGrid(); 
				BindDDEmpresas(); 
			}

	
			private DataSet DS()
			{
				DataSet ds = new DATASETS.DSEmpresa(); 

				SqlParameter[] arrParams = new SqlParameter[10];
				arrParams[0] = new SqlParameter("@inNome", txtNomeEmpresa.Text);
				arrParams[1] = new SqlParameter("@inNif", txtNIF.Text);
		
				arrParams[2] = new SqlParameter("@inIdEstadoEmpresa", ddEstado.SelectedValue);
				arrParams[3] = new SqlParameter("@inIdTipoEmpresa", ddTipoEmpresa.SelectedValue);

				arrParams[4] = new SqlParameter("@inCodigoBloqueioSAP", null);
				arrParams[5] = new SqlParameter("@inPagamentoAtraso", null);
				arrParams[6] = new SqlParameter("@inNivelBloqueioLabmetro", null);
				arrParams[7] = new SqlParameter("@inRequisicaoAtraso",null);
				arrParams[8] = new SqlParameter("@inNumCliente",txtNumClienteSAP.Text);
				arrParams[9] = new SqlParameter("@inIdCondicoesPagamento",null);
	
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
				
			}
    

			private void BindDDEmpresas()
			{
		
				string strSQL = "SELECT idEmpresa, (CAST(idEmpresa as varchar) +' - ' + Cast(nome as varchar)) as descricao FROM EMPRESA  WHERE nome like '"+txtNomeEmpresa.Text+"%' COLLATE SQL_Latin1_General_Cp850_CI_AI OR nomeAbreviado like  '"+txtNomeEmpresa.Text+"%' COLLATE SQL_Latin1_General_Cp850_CI_AI ORDER BY NOME COLLATE SQL_Latin1_General_Cp850_CI_AI";  
				SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 

				ddEmpresaManter.DataTextField = "descricao";
				ddEmpresaManter.DataValueField="idEmpresa"; 
				ddEmpresaManter.DataSource = dr; 
				ddEmpresaManter.DataBind(); 		
				ddEmpresaManter.Items.Insert(0,new ListItem("",""));

                dr.Close();

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

		private void DGEmpresas_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}

		private void btnTrocar_Click(object sender, System.EventArgs e)
		{
			string strIds =""; 
			foreach(DataGridItem dgi in DGEmpresas.Items) 
			{ 
				CheckBox myCheckBox =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				
				if(myCheckBox.Checked == true)
				{
					
					strIds+= DGEmpresas.DataKeys[dgi.ItemIndex].ToString();
					strIds+=",";
				}   
			}
			strIds = strIds.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item

			if(ddEmpresaManter.SelectedValue =="" || strIds.Length ==0)
			{
				lblMessage.Text =Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return;
			}
			if(trocarEmpresas(ddEmpresaManter.SelectedValue,strIds)) 
			{
				BindGrid();  
				BindDDEmpresas();
			}
			
		}


		private bool trocarEmpresas(string idEmpresaManter, string idsEmpresasApagar)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 
				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try
					{
						//atenção que so so pode trocar empresas do mesmo tipo, por isso isto começa com uma validação do tipo
							//o contacto tem de ser trocado primeiro.
						
						objCmd.CommandText = "Select count(distinct idTipoEmpresa) FROM EMPRESA WHERE idEmpresa in ("+idsEmpresasApagar+","+idEmpresaManter +")"; 

						//Response.Write(objCmd.CommandText); 
						objCmd.ExecuteScalar();

						objCmd.CommandText = "UPDATE CONTACTO SET idEmpresa = " + idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 
						objCmd.ExecuteNonQuery();

						
						objCmd.CommandText = "UPDATE BRE SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 
						objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "UPDATE BRE SET idEmpresaContratante = " + idEmpresaManter + " WHERE idEmpresaContratante in (" + idsEmpresasApagar + ")";
                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "UPDATE EMPRESA SET idEmpresaPai = "+idEmpresaManter + " WHERE idEmpresaPai in ("+idsEmpresasApagar+")"; 
						objCmd.ExecuteNonQuery();

						objCmd.CommandText = "UPDATE BSE SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE EQUIPAMENTO SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE GUIATRANSPORTE SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE REQUISICAO SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE ORCAMENTO SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")";



                        objCmd.ExecuteNonQuery();
                        objCmd.CommandText = "UPDATE TaxaServico SET idEmpresa = " + idEmpresaManter + " WHERE idEmpresa in (" + idsEmpresasApagar + ")"; 


						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE FACTURA SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE subcontratobse SET idEmpresaSubcontratada = "+idEmpresaManter + " WHERE idEmpresaSubcontratada in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE subcontratobre SET idEmpresaSubcontratada = "+idEmpresaManter + " WHERE idEmpresaSubcontratada in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();
						objCmd.CommandText = "UPDATE marcacao SET idEmpresa = "+idEmpresaManter + " WHERE idEmpresa in ("+idsEmpresasApagar+")"; 

						objCmd.ExecuteNonQuery();


						int numIds = idsEmpresasApagar.Split(",".ToCharArray()).Length; 

						objCmd.CommandText = "DELETE FROM EMPRESA WHERE idEmpresa in ("+idsEmpresasApagar+")"; 
						if(objCmd.ExecuteNonQuery()!=numIds) throw new Exception("Erro em apagar empresas."); 

						
						objTrans.Commit();

                        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_TROCA_EFECTUADA;
						return true;


					}
					catch(Exception ex)
					{

                        GERAL.clsWriteError.WriteLog(ex.ToString());
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); 
							lblMessage.Text +=excep.ToString()+"<br />";
							return false;
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text +=ex.ToString()+"<br />";
						return false;
					}
				}
			}
		}

		private void Linkbutton1_Click(object sender, System.EventArgs e)
		{
			//contactos
			if(DGEmpresas.SelectedIndex <0) 
			{
				lblMessage.Text=Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}
			dgGenerico.AutoGenerateColumns=false;
			
			BoundColumn bc1 = new BoundColumn(); 
			bc1.DataField = "idContacto"; 
			bc1.HeaderText="idContacto";
			dgGenerico.Columns.Add(bc1); 
			
			BoundColumn bc2 = new BoundColumn(); 
			bc2.DataField = "nome"; 
			bc2.HeaderText="nome";
			dgGenerico.Columns.Add(bc2); 

			BoundColumn bc3 = new BoundColumn(); 
			bc3.DataField = "cargo"; 
			bc3.HeaderText="cargo";
			dgGenerico.Columns.Add(bc3); 

			BoundColumn bc4 = new BoundColumn(); 
			bc4.DataField = "departamento"; 
			bc4.HeaderText="departamento";
			dgGenerico.Columns.Add(bc4); 

			BoundColumn bc5 = new BoundColumn(); 
			bc5.DataField = "telefoneEmpresa"; 
			bc5.HeaderText="telefone";
			dgGenerico.Columns.Add(bc5); 

			BoundColumn bc6 = new BoundColumn(); 
			bc6.DataField = "extensaoEmpresa"; 
			bc6.HeaderText="extensao";
			dgGenerico.Columns.Add(bc6);
 
			BoundColumn bc7 = new BoundColumn(); 
			bc7.DataField = "emailEmpresa"; 
			bc7.HeaderText="emailEmpresa";
			dgGenerico.Columns.Add(bc7);

			BoundColumn bc8 = new BoundColumn(); 
			bc8.DataField = "faxEmpresa"; 
			bc8.HeaderText="fax";
			dgGenerico.Columns.Add(bc8);

			DATA.ContactosBD contactos = new LabMetro.DATA.ContactosBD();
			dgGenerico.DataSource = contactos.DTFillContacts(DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString(),null, null,null); 
			dgGenerico.DataBind();
			contactos = null; 
		}

		private void Linkbutton2_Click(object sender, System.EventArgs e)
		{
			//equipamentos

			if(DGEmpresas.SelectedIndex <0) 
			{
				lblMessage.Text=Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}

			dgGenerico.AutoGenerateColumns=true; 
			DATA.EquipamentoBD equip = new LabMetro.DATA.EquipamentoBD();
			dgGenerico.DataSource = equip.DTGetEquipamentosActivosByEmpresa(DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString(),null,null,null,null,null); 
			dgGenerico.DataBind();
			equip = null; 
		}

		private void Linkbutton3_Click(object sender, System.EventArgs e)
		{
			if(DGEmpresas.SelectedIndex <0) 
			{
				lblMessage.Text=Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}
			//serviços
			dgGenerico.AutoGenerateColumns=true; 
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
			dgGenerico.DataSource = servico.DTServicosByIdEmpresa(DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString()); 
			dgGenerico.DataBind();
			servico = null; 
		}

		private void Linkbutton4_Click(object sender, System.EventArgs e)
		{
			if(DGEmpresas.SelectedIndex <0) 
			{
				lblMessage.Text=Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}
			//orçamentos
			dgGenerico.AutoGenerateColumns=true; 
			DATA.OrcamentoBD orc = new LabMetro.DATA.OrcamentoBD();
			dgGenerico.DataSource = orc.DTOrcamentos(null, null, null,"",DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString(),null); 
			dgGenerico.DataBind();
			orc = null; 
		}

		private void Linkbutton5_Click(object sender, System.EventArgs e)
		{
			if(DGEmpresas.SelectedIndex <0) 
			{
				lblMessage.Text=Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}
			//requisicoes
			
			BoundColumn bc1 = new BoundColumn(); 
			bc1.DataField = "idRequisicao"; 
			bc1.HeaderText="idRequisicao";
			dgGenerico.Columns.Add(bc1); 
			
			BoundColumn bc2 = new BoundColumn(); 
			bc2.DataField = "dtRequisicao"; 
			bc2.HeaderText="dtRequisicao";
			dgGenerico.Columns.Add(bc2); 

			BoundColumn bc3 = new BoundColumn(); 
			bc3.DataField = "referenciaCliente"; 
			bc3.HeaderText="referenciaCliente";
			dgGenerico.Columns.Add(bc3); 

			BoundColumn bc4 = new BoundColumn(); 
			bc4.DataField = "dtCriacao"; 
			bc4.HeaderText="dtCriacao";
			dgGenerico.Columns.Add(bc4); 

			BoundColumn bc5 = new BoundColumn(); 
			bc5.DataField = "refRequisicao"; 
			bc5.HeaderText="refRequisicao";
			dgGenerico.Columns.Add(bc5); 


			dgGenerico.AutoGenerateColumns=false; 
			DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD();
			dgGenerico.DataSource = req.DTListaRequisicoesByIdEmpresa(DGEmpresas.DataKeys[DGEmpresas.SelectedIndex].ToString());

			dgGenerico.DataBind();
			req = null; 
		}
	}
}
