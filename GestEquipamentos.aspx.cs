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
using System.Text; 

namespace LabMetro
{
	/// <summary>
	/// Summary description for GestaoEquipamentos.
	/// </summary>
	public partial class GestaoEquipamentos : System.Web.UI.Page
	{
		DataView DV;
		
		private const string ID_PAG = "GESTEQUIPAMENTOS_1";//NOME DA PAGINA
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text = ""; 
            
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
						ViewState["sortField"] = "TipoEquipamento";
						ViewState["grandeza"] = ""; //inicia vazio
						if(Request.QueryString["id"]!= null) ddEmpresa.SelectedValue=Request.QueryString["id"].ToString(); 
					}
				}
			}
		}


		
		//=========================================================================================
		//BIND LIST DOS EQUIPAMENTOS

		//=========================================================================================
		private void BindList()
		{
			
			DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 
			if(ddEmpresa.SelectedIndex>-1)
			{
				DataTable DT =  equipamento.DTEquipamento(ddEmpresa.SelectedValue.ToString(), txtTipoEquipamento.Text, txtNSerie.Text, txtNumIdent.Text); 
				
				DV = new DataView(DT);
				if(cbActivos.Checked==true)  DV.RowFilter = " Estado = 1"; 
				
				DV.Sort = ViewState["sortField"].ToString()+ " ASC"; 
           
				DLEquipamentos.DataSource =DV; 	
				DLEquipamentos.DataBind(); 

				if(DV.Table.Rows.Count == 0)
				{
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
				}
			}
			else
			{
				DLEquipamentos.Dispose();
				DLEquipamentos.Visible=false; 
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
			}

			equipamento = null; 
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
            bntShowHeader.Click += new System.EventHandler(bntShowHeader_Click);
            ddFiltro.SelectedIndexChanged += new System.EventHandler(ddFiltro_SelectedIndexChanged);
            txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
            txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
            btnPesquisaEmpresa.Click += new System.EventHandler(btnPesquisaEmpresa_Click);
            ddEmpresa.SelectedIndexChanged += new System.EventHandler(ddEmpresa_SelectedIndexChanged);
            //btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            btnSearch.Click += new System.EventHandler(btnSearch_Click);
        }
        #endregion


		//====================================================================================
		//APAGA  DA BD - não está a ser utilizado de momento
		//====================================================================================
		protected void Delete_Command(object source,System.Web.UI.WebControls.DataListCommandEventArgs e)		
		{
			string id = DLEquipamentos.DataKeys[e.Item.ItemIndex].ToString();
                
		}
		//====================================================================================
		//EDITA um item da lista
		//====================================================================================
		protected void Edit_Command(Object sender, DataListCommandEventArgs e)     
		{
			if(DLEquipamentos.ShowHeader == true)
			{
				lblMessage.Text ="P.F. feche o formulário de inserção primeiro.";
			}
			else
			{

				//aqui devia fazer disable aos validators do header, mas depois tenho de fazer outra vez antes de chamar o page.isvalid
				disableValidatorsInHeader(); 
				DLEquipamentos.EditItemIndex = e.Item.ItemIndex; 
			
				string id = DLEquipamentos.DataKeys[e.Item.ItemIndex].ToString(); 
				BindList(); 
				
				
			}

		}

		//====================================================================================
		//CANCELA um editItem
		//====================================================================================
		protected void Cancel_Command(Object sender, DataListCommandEventArgs e)
		{
			DLEquipamentos.EditItemIndex = -1; 
			ViewState["grandeza"] = ""; //limpar 
			BindList(); 
		}
		
		//=========================================================================================
		//INSERT DO EQUIPAMENTO
		//=========================================================================================
		protected void DL_ItemCommand(object sender, DataListCommandEventArgs e)    
		{     
			if(e.CommandName=="insereEquipamento")
			{
//				if(e.CommandSource is WebControl)SetControlFocus(e.CommandSource);

				//declaração dos controls===================================================
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoHeader"); 
				DropDownList ddUnidadeAlcance = (DropDownList)e.Item.FindControl("ddUnidadeAlcanceHeader"); 
				DropDownList ddClasse = (DropDownList)e.Item.FindControl("ddClasseHeader"); 
				DropDownList ddMarca = (DropDownList)e.Item.FindControl("ddMarcaHeader"); 
				DropDownList ddModelo = (DropDownList)e.Item.FindControl("ddModeloHeader"); 
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoHeader"); 
			
				TextBox txtAlcance = (TextBox)e.Item.FindControl("txtAlcanceHeader"); 
				TextBox txtAlcanceInf = (TextBox)e.Item.FindControl("txtAlcanceInfHeader"); 
				TextBox txtAlcanceSup = (TextBox)e.Item.FindControl("txtAlcanceSupHeader"); 
				TextBox txtClasse = (TextBox)e.Item.FindControl("txtClasseHeader"); 
				TextBox txtRefUltimaCalib = (TextBox)e.Item.FindControl("txtRefUltimaCalibHeader"); 
				TextBox txtDataUltimaCalib = (TextBox)e.Item.FindControl("txtDataUltimaCalibHeader"); 
				TextBox txtFabricante = (TextBox)e.Item.FindControl("txtFabricanteHeader"); 
				TextBox txtForma = (TextBox)e.Item.FindControl("txtFormaHeader"); 
			
				TextBox txtNumIdentif = (TextBox)e.Item.FindControl("txtNumIdentifHeader"); 
				TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerieHeader"); 
				TextBox txtObservacoes = (TextBox)e.Item.FindControl("txtObservacoesHeader"); 
				TextBox txtPeriodicidade = (TextBox)e.Item.FindControl("txtPeriodicidadeHeader"); 
				TextBox txtResolucao = (TextBox)e.Item.FindControl("txtResolucaoHeader"); 
				CheckBox cbCalibInt	= (CheckBox)e.Item.FindControl("cbCalibIntHeader"); 

				TextBox txtCampo1 = (TextBox)e.Item.FindControl("txtCampo1Header"); 
				TextBox txtCampo2 = (TextBox)e.Item.FindControl("txtCampo2Header");

                TextBox txtEtiqueta1 = (TextBox)e.Item.FindControl("txtEtiqueta1Header");
                TextBox txtEtiqueta2 = (TextBox)e.Item.FindControl("txtEtiqueta2Header");
                TextBox txtEtiqueta3 = (TextBox)e.Item.FindControl("txtEtiqueta3Header");
      
                CheckBox cbCertConclusivo	= (CheckBox)e.Item.FindControl("cbCertConclusivoHeader"); 

				string idmarca = ddMarca.SelectedValue;
				string	idmodelo = ddModelo.SelectedValue; 
				
				string strCalibInt = ""; 
				if(cbCalibInt.Checked == true) strCalibInt = "1"; 

				string CertificadoConclusivo = ""; 
				if(cbCertConclusivo.Checked == true) CertificadoConclusivo = "1"; 

				DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 
				
				try
				{
					if(Page.IsValid)
					{
						string x = equipamento.InsertUpdateEquipamento("",ddEmpresa.SelectedValue, ddTipoEquipamento.SelectedValue,txtNumSerie.Text,txtNumIdentif.Text,txtAlcanceInf.Text,txtAlcanceSup.Text,ddUnidadeAlcance.SelectedValue.ToString(), txtAlcance.Text, txtResolucao.Text, idmodelo, idmarca,ddClasse.SelectedValue.ToString(),txtClasse.Text, txtForma.Text, txtFabricante.Text, txtRefUltimaCalib.Text, txtDataUltimaCalib.Text, txtPeriodicidade.Text, ddEstado.SelectedValue.ToString(),txtObservacoes.Text,"insert",strCalibInt,CertificadoConclusivo, txtCampo1.Text, txtCampo2.Text, txtEtiqueta1.Text, txtEtiqueta2.Text, txtEtiqueta3.Text,null);

						lblMessage.Text = x; 
						
						if(x=="Equipamento inserido com sucesso." || x=="Equipamento alterado com sucesso.")
						{
							DLEquipamentos.ShowHeader=false;
							DLEquipamentos.EditItemIndex = -1; 
							ViewState["grandeza"] = ""; 
							BindList(); 
						}
					}
					else
					{
						//writeStatusOfValidators(); 
					}
				}
				catch
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DB; 
				}
				equipamento = null; 

				
			}
			else if(e.CommandName=="cancelInsert")
			{
				DLEquipamentos.ShowHeader = false; 
			}
		}


		private void writeStatusOfValidators()
		{
			//foreach (Item i in System.Web.UI.ValidatorCollection)
			// Get 'Validators' of the page to myCollection.
			ValidatorCollection coll = Page.Validators;

			// Print the values of Collection using 'Item' property.
			string myStr = " ";         
			for(int i = 0; i<coll.Count; i++)
			{
				myStr += coll[i].ToString() + " - " + coll[i].IsValid.ToString() +  "-" + coll[i].ErrorMessage.ToString() +"<br />";  
				switch(coll[i].GetType().ToString())
				{
					case "System.Web.UI.WebControls.CompareValidator":
						CompareValidator comp = (CompareValidator)coll[i]; 
						Response.Write(comp.ID.ToString()+"<br />"); 
						break; 
					case "System.Web.UI.WebControls.CustomValidator":
						CustomValidator cust = (CustomValidator)coll[i]; 
						Response.Write(cust.ID.ToString()+"<br />"); 
						break; 
					case "System.Web.UI.WebControls.RequiredFieldValidator":
						RequiredFieldValidator req = (RequiredFieldValidator)coll[i]; 
						Response.Write(req.ID.ToString()+"<br />"); 
						break; 
				}
			}
		}
	

		private void disableValidatorsInHeader()
		{
			Control dli = DLEquipamentos.Controls[0]; 

			RequiredFieldValidator reqTipoEquipamentoHeader = (RequiredFieldValidator)dli.FindControl("reqTipoEquipamentoHeader"); 
			CustomValidator custValidaEquipamentoHeader = (CustomValidator)dli.FindControl("custValidaEquipamentoHeader"); 
			CompareValidator compAlcanceInfHeader = (CompareValidator)dli.FindControl("compAlcanceInfHeader"); 
			CompareValidator comlcanceSupHeader = (CompareValidator)dli.FindControl("comlcanceSupHeader"); 
			CompareValidator comDataUltimaCalibHeader = (CompareValidator)dli.FindControl("comDataUltimaCalibHeader"); 

			reqTipoEquipamentoHeader.Enabled=false;
			custValidaEquipamentoHeader.Enabled = false; 
			compAlcanceInfHeader.Enabled=false; 
			comlcanceSupHeader.Enabled=false; 
			comDataUltimaCalibHeader.Enabled=false;
		}

//		private void disableValidatorsInEditItem(string i)
//		{
//			Response.Write(i.ToString()); 
//			//Control dli = DLEquipamentos.Items[i]; 
//
//			Control dli = DLEquipamentos.Controls[0]; 
//			RequiredFieldValidator reqTipoEquipamentoHeader = (RequiredFieldValidator)dli.FindControl("reqTipoEquipamentoHeader"); 
//			CustomValidator custValidaEquipamentoHeader = (CustomValidator)dli.FindControl("custValidaEquipamentoHeader"); 
//			CompareValidator compAlcanceInfHeader = (CompareValidator)dli.FindControl("compAlcanceInfHeader"); 
//			CompareValidator comlcanceSupHeader = (CompareValidator)dli.FindControl("comlcanceSupHeader"); 
//			CompareValidator comDataUltimaCalibHeader = (CompareValidator)dli.FindControl("comDataUltimaCalibHeader"); 
//
//			reqTipoEquipamentoHeader.Enabled=true;
//			custValidaEquipamentoHeader.Enabled = true; 
//			compAlcanceInfHeader.Enabled=true; 
//			comlcanceSupHeader.Enabled=true; 
//			comDataUltimaCalibHeader.Enabled=true;
//		}
		//====================================================================================
		//UPDATE DO EQUIPAMENTO
		//====================================================================================
		protected void Update_Command(Object sender, DataListCommandEventArgs e)
		{
			
			//no update, nao quero que disparem os validators do insert, vou tentar disable them.
			disableValidatorsInHeader(); 
			if(Page.IsValid) //para chamar explicitamente o customvalidator Page.IsValid is what triggers server side validation
			{
				string idEquipamento = DLEquipamentos.DataKeys[e.Item.ItemIndex].ToString();
				              
				//declaração dos controls===================================================				
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado"); 
				DropDownList ddUnidadeAlcance = (DropDownList)e.Item.FindControl("ddUnidadeAlcance"); 
				DropDownList ddClasse = (DropDownList)e.Item.FindControl("ddClasse"); 
				DropDownList ddMarca = (DropDownList)e.Item.FindControl("ddMarca"); 
				DropDownList ddModelo = (DropDownList)e.Item.FindControl("ddModelo"); 
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamento"); 
			
				TextBox txtAlcance = (TextBox)e.Item.FindControl("txtAlcance"); 
				TextBox txtAlcanceInf = (TextBox)e.Item.FindControl("txtAlcanceInf"); 
				TextBox txtAlcanceSup = (TextBox)e.Item.FindControl("txtAlcanceSup"); 
				TextBox txtClasse = (TextBox)e.Item.FindControl("txtClasse"); 
				TextBox txtRefUltimaCalib = (TextBox)e.Item.FindControl("txtRefUltimaCalib"); 
				TextBox txtDataUltimaCalib = (TextBox)e.Item.FindControl("txtDataUltimaCalib"); 
				TextBox txtFabricante = (TextBox)e.Item.FindControl("txtFabricante"); 
				TextBox txtForma = (TextBox)e.Item.FindControl("txtForma"); 
				TextBox txtNumIdentif = (TextBox)e.Item.FindControl("txtNumIdentif"); 
				TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerie"); 
				TextBox txtObservacoes = (TextBox)e.Item.FindControl("txtObservacoes"); 
				TextBox txtPeriodicidade = (TextBox)e.Item.FindControl("txtPeriodicidade"); 
				TextBox txtResolucao = (TextBox)e.Item.FindControl("txtResolucao"); 

				CheckBox cbCalibInt	= (CheckBox)e.Item.FindControl("cbCalibInt"); 

				TextBox txtCampo1 = (TextBox)e.Item.FindControl("txtCampo1"); 
				TextBox txtCampo2 = (TextBox)e.Item.FindControl("txtCampo2"); 
				CheckBox cbCertConclusivo	= (CheckBox)e.Item.FindControl("cbCertConclusivo");

                TextBox txtEtiqueta1 = (TextBox)e.Item.FindControl("txtEtiqueta1");
                TextBox txtEtiqueta2 = (TextBox)e.Item.FindControl("txtEtiqueta2");
                TextBox txtEtiqueta3 = (TextBox)e.Item.FindControl("txtEtiqueta3"); 

				string strCalibInt = ""; 
				if(cbCalibInt.Checked == true) strCalibInt = "1"; 

				string CertificadoConclusivo = ""; 
				if(cbCertConclusivo.Checked == true) CertificadoConclusivo = "1"; 

				string	idmarca = ddMarca.SelectedValue;
				string	idmodelo = ddModelo.SelectedValue; 				
	
				DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 

				try
				{
					if(Page.IsValid)
					{
                        string x = equipamento.InsertUpdateEquipamento(idEquipamento, ddEmpresa.SelectedValue, ddTipoEquipamento.SelectedValue, txtNumSerie.Text, txtNumIdentif.Text, txtAlcanceInf.Text, txtAlcanceSup.Text, ddUnidadeAlcance.SelectedValue.ToString(), txtAlcance.Text, txtResolucao.Text, idmodelo, idmarca, ddClasse.SelectedValue.ToString(), txtClasse.Text, txtForma.Text, txtFabricante.Text, txtRefUltimaCalib.Text, txtDataUltimaCalib.Text, txtPeriodicidade.Text, ddEstado.SelectedValue.ToString(), txtObservacoes.Text, "update", strCalibInt, CertificadoConclusivo, txtCampo1.Text, txtCampo2.Text, txtEtiqueta1.Text, txtEtiqueta2.Text, txtEtiqueta3.Text,null);
						lblMessage.Text = x; 
						if(x=="Equipamento inserido com sucesso." || x=="Equipamento alterado com sucesso.")
						{
							DLEquipamentos.ShowHeader=false;
							DLEquipamentos.EditItemIndex = -1; 
							ViewState["grandeza"] = ""; 
							BindList(); 
						}
					}
				}

				catch
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DB; 
				}
				equipamento = null; 
			}
			else
			{
				//lblMessage.Text +="Page not valid";				
			}
		}

		
		//=========================================================================================
		//Datalista ITEM DATABOUND  (fillform) (do item insert e update)
		//=========================================================================================
		protected void DLEquipamentos_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			
			//==============================================================================
			if(e.Item.ItemType == ListItemType.EditItem)
			{
				//declaração dos controls===================================================

				
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstado"); 
				DropDownList ddUnidadeAlcance = (DropDownList)e.Item.FindControl("ddUnidadeAlcance"); 
				DropDownList ddClasse = (DropDownList)e.Item.FindControl("ddClasse"); 
				DropDownList ddMarca = (DropDownList)e.Item.FindControl("ddMarca"); 
				DropDownList ddModelo = (DropDownList)e.Item.FindControl("ddModelo"); 
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamento"); 
			
				TextBox txtAlcance = (TextBox)e.Item.FindControl("txtAlcance"); 
				TextBox txtAlcanceInf = (TextBox)e.Item.FindControl("txtAlcanceInf"); 
				TextBox txtAlcanceSup = (TextBox)e.Item.FindControl("txtAlcanceSup"); 
				TextBox txtClasse = (TextBox)e.Item.FindControl("txtClasse"); 
				TextBox txtRefUltimaCalib = (TextBox)e.Item.FindControl("txtRefUltimaCalib"); 
				TextBox txtDataUltimaCalib = (TextBox)e.Item.FindControl("txtDataUltimaCalib"); 
				TextBox txtFabricante = (TextBox)e.Item.FindControl("txtFabricante"); 
				TextBox txtForma = (TextBox)e.Item.FindControl("txtForma"); 
				TextBox txtNumIdentif = (TextBox)e.Item.FindControl("txtNumIdentif"); 
				TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerie"); 
				TextBox txtObservacoes = (TextBox)e.Item.FindControl("txtObservacoes"); 
				TextBox txtPeriodicidade = (TextBox)e.Item.FindControl("txtPeriodicidade"); 
				TextBox txtResolucao = (TextBox)e.Item.FindControl("txtResolucao"); 
				CheckBox cbCalibInt	= (CheckBox)e.Item.FindControl("cbCalibInt"); 
			
				TextBox txtCampo1 = (TextBox)e.Item.FindControl("txtCampo1"); 
				TextBox txtCampo2 = (TextBox)e.Item.FindControl("txtCampo2");

                TextBox txtEtiqueta1 = (TextBox)e.Item.FindControl("txtEtiqueta1");
                TextBox txtEtiqueta2 = (TextBox)e.Item.FindControl("txtEtiqueta2");
                TextBox txtEtiqueta3 = (TextBox)e.Item.FindControl("txtEtiqueta3"); 

				CheckBox cbCertConclusivo	= (CheckBox)e.Item.FindControl("cbCertConclusivo"); 

				//fill dos controls===========================================================

				string id = DLEquipamentos.DataKeys[e.Item.ItemIndex].ToString(); 

				LabMetro.DATA.EquipamentoBD fillEquipment= new LabMetro.DATA.EquipamentoBD(); 
				LabMetro.DATA.EquipmentDetails dt = fillEquipment.GetEquipmentDetails(id);  


				if(dt!= null)
				{

					ddEstado.SelectedValue = dt.activo;
			        
					fillEquipamentosUpdate(ddTipoEquipamento, dt.idTipoEquipamento); //no editar = alterar

					try
					{
						ddTipoEquipamento.SelectedValue = dt.idTipoEquipamento; 
					}
					catch 
					{
						ddTipoEquipamento.Items.Insert(0,new ListItem(dt.tipoEquipamento,dt.idTipoEquipamento));
						ddTipoEquipamento.SelectedValue = dt.idTipoEquipamento;
					}

					
				//	ViewState["grandeza"]  = fillEquipment.GetGrandeza(dt.idTipoEquipamento); 
					

					//fill das outras dropdowns==========================================================
					//fillMarcas(ddMarca);
					//fillModelos(ddModelo, ddMarca.SelectedValue); 
					fillDropDowns(ddClasse, ddUnidadeAlcance); 
					//============================================================================


					txtAlcance.Text = dt.alcance; 
					txtAlcanceInf.Text = dt.alcanceInf; 
					txtAlcanceSup.Text = dt.alcanceSup; 
					try
					{
						ddUnidadeAlcance.SelectedValue = dt.idUnidadeAlcance;
					}
					catch 
					{    
					}

					try
					{
						ddClasse.SelectedValue = dt.idClasse; 
					}
					catch 
					{   
					}

					txtClasse.Text = dt.classe; 
					txtRefUltimaCalib.Text = dt.refUltimaCalibracao; 
					txtDataUltimaCalib.Text = GERAL.clsGeral.ToShortDate(dt.dtUltimaCalibracao); 
					txtFabricante.Text = dt.fabricante; 
					txtForma.Text = dt.forma; 
					
					string grandeza = dt.idGrandeza;
                    if (dt.idmarca != "")
                    {
                        try
                        {
                            ddMarca.SelectedValue = dt.idmarca; //ddMarca.Items.FindByText(dt.marca).Value;
                        }
                        catch
                        {

                        }
                    }

                    ddModelo.Items.Insert(0,new ListItem("---","")); //nao funciona
                    if (dt.idmodelo != "")
                    {
                        try
                        {
                            ddModelo.SelectedValue = dt.idmodelo;
                        }
                        catch
                        {


                        }
                    }
					
					txtNumIdentif.Text = dt.numIdentificacao;
					txtNumSerie.Text = dt.numSerie;
					txtObservacoes.Text = dt.observacoes; 
					txtPeriodicidade.Text = dt.periodicidadeCalibracao;
					txtResolucao.Text = dt.resolucao; 
					if( dt.calibInt =="True") cbCalibInt.Checked = true; 
					if( dt.certConclusivo =="True") cbCertConclusivo.Checked = true; 

					txtCampo1.Text= dt.campo1;
					txtCampo2.Text= dt.campo2;
                    txtEtiqueta1.Text = dt.etiqueta1;
                    txtEtiqueta2.Text = dt.etiqueta2;
                    txtEtiqueta3.Text = dt.etiqueta3;
				}
				else
				{
					lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_DATA; 
				}
				//============================================================================	

				
			}
			else if (e.Item.ItemType == ListItemType.Header)

				//se calhar devia fazer isto sempre aqui dentro
				//disableValidatorsInEditItem(""); 

			{	
				//declaração dos controls===================================================
				DropDownList ddEstado = (DropDownList)e.Item.FindControl("ddEstadoHeader"); 
				DropDownList ddUnidadeAlcance = (DropDownList)e.Item.FindControl("ddUnidadeAlcanceHeader"); 
				DropDownList ddClasse = (DropDownList)e.Item.FindControl("ddClasseHeader"); 
				DropDownList ddMarca = (DropDownList)e.Item.FindControl("ddMarcaHeader"); 
				DropDownList ddModelo = (DropDownList)e.Item.FindControl("ddModeloHeader"); 
				DropDownList ddTipoEquipamento = (DropDownList)e.Item.FindControl("ddTipoEquipamentoHeader"); 
			
				TextBox txtAlcance = (TextBox)e.Item.FindControl("txtAlcanceHeader"); 
				TextBox txtAlcanceInf = (TextBox)e.Item.FindControl("txtAlcanceInfHeader"); 
				TextBox txtAlcanceSup = (TextBox)e.Item.FindControl("txtAlcanceSupHeader"); 
				TextBox txtClasse = (TextBox)e.Item.FindControl("txtClasseHeader"); 
				TextBox txtRefUltimaCalib = (TextBox)e.Item.FindControl("txtRefUltimaCalibHeader"); 
				TextBox txtDataUltimaCalib = (TextBox)e.Item.FindControl("txtDataUltimaCalibHeader"); 
				TextBox txtFabricante = (TextBox)e.Item.FindControl("txtFabricanteHeader"); 
				TextBox txtForma = (TextBox)e.Item.FindControl("txtFormaHeader"); 
				
				TextBox txtNumIdentif = (TextBox)e.Item.FindControl("txtNumIdentifHeader"); 
				TextBox txtNumSerie = (TextBox)e.Item.FindControl("txtNumSerieHeader"); 
				TextBox txtObservacoes = (TextBox)e.Item.FindControl("txtObservacoesHeader"); 
				TextBox txtPeriodicidade = (TextBox)e.Item.FindControl("txtPeriodicidadeHeader"); 
				TextBox txtResolucao = (TextBox)e.Item.FindControl("txtResolucaoHeader"); 
				CheckBox cbCalibInt	= (CheckBox)e.Item.FindControl("cbCalibInt");

				TextBox txtCampo1 = (TextBox)e.Item.FindControl("txtCampo1Header"); 
				TextBox txtCampo2 = (TextBox)e.Item.FindControl("txtCampo2Header");

                TextBox txtEtiqueta1 = (TextBox)e.Item.FindControl("txtEtiqueta1Header");
                TextBox txtEtiqueta2 = (TextBox)e.Item.FindControl("txtEtiqueta2Header");
                TextBox txtEtiqueta3 = (TextBox)e.Item.FindControl("txtEtiqueta3Header");


				CheckBox cbCertConclusivo	= (CheckBox)e.Item.FindControl("cbCertConclusivoHeader"); 

				RequiredFieldValidator reqTipoEquipamentoHeader = (RequiredFieldValidator)e.Item.FindControl("reqTipoEquipamentoHeader"); 
				CustomValidator custValidaEquipamentoHeader = (CustomValidator)e.Item.FindControl("custValidaEquipamentoHeader"); 
				CompareValidator compAlcanceInfHeader = (CompareValidator)e.Item.FindControl("compAlcanceInfHeader"); 
				CompareValidator comlcanceSupHeader = (CompareValidator)e.Item.FindControl("comlcanceSupHeader"); 
				CompareValidator comDataUltimaCalibHeader = (CompareValidator)e.Item.FindControl("comDataUltimaCalibHeader"); 
	
				reqTipoEquipamentoHeader.Enabled=true;
				custValidaEquipamentoHeader.Enabled = true; 
				compAlcanceInfHeader.Enabled=true; 
				comlcanceSupHeader.Enabled=true; 
				comDataUltimaCalibHeader.Enabled=true;
	

				//fill das dropdowns==========================================================
				fillEquipamentosInsert(ddTipoEquipamento); 
				//fillMarcas(ddMarca);
				//fillModelos(ddModelo, ddMarca.SelectedValue); 
				fillDropDowns(ddClasse, ddUnidadeAlcance); 

				
			}
//			else if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
//			{
//				//==============================================================================
//				LinkButton btn = (LinkButton)e.Item.FindControl("edit"); 
//				//Response.Write(btn.GetType().ToString()); 
//				//id="DLEquipamentos__ctl22_ddTipoEquipamento"
//				//GERAL.clsWriteError.WriteLog(e.Item.ItemIndex.ToString()); 
//				int i = e.Item.ItemIndex + 1; 
//				string myID = "DLEquipamentos__ctl"+i+"_ddTipoEquipamento"; 
//				//GERAL.clsWriteError.WriteLog("i: " + i + " id:" + myID); 
//				//faz conflito com o javascript dopostback
//				btn.Attributes.Add("onClick","javascript:alert(myID); document.getElementById['"+myID+"'].focus();");
//
//			}
		}

        protected void ddMarcaEditDatabound(Object sender, EventArgs e)
        {
            //encontrar a dropdown
            DropDownList ddSender = (DropDownList)sender;
            ddSender.Items.Insert(0, new ListItem("---", ""));
        }

        protected void ddModeloEditDatabound(Object sender, EventArgs e)
        {
            //encontrar a dropdown
            DropDownList ddSender = (DropDownList)sender;
            ddSender.Items.Insert(0, new ListItem("---", "")); 
        }

		private void ddFiltro_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ViewState["sortField"] = ddFiltro.SelectedValue; 
			BindList(); 
		}

		protected void bntShowHeader_Click(object sender, System.EventArgs e)
		{
			//aqui tenho de desactivar os outros validators(so que nao sei dentro de que index estao
			
			DLEquipamentos.EditItemIndex = -1; 
			ViewState["grandeza"] = ""; //limpar 
			DLEquipamentos.ShowHeader=true; 
			
			//BindList(); 

		}

		//*************************************************************************************************
		//*************** FUNÇOES EMPRESA   ***************************************************************
		//*************************************************************************************************

		//=========================================================================================
		//FILL DROPDOWN EMPRESA
		//=========================================================================================
		private void fillDDEmpresa()
		{
			if(txtPesquisaEmpresa.Text !="")
			{
				DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
				DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,txtPesquisaNif.Text,"1","","","","","","");  //activas
	            
				DataView DV = new DataView(DT);
				
				ddEmpresa.DataSource = DV; ; 
				ddEmpresa.DataBind();

				empresa = null; 	
			}
			else
			{
				ddEmpresa.Items.Clear(); 
			}

		}
        
		//=========================================================================================
		//TEXTCHANGED DA PESQUISA POR NOME DE EMPRESA
		//=========================================================================================
		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DLEquipamentos.DataSource = null;
			DLEquipamentos.DataBind(); 
			
		}

		//=========================================================================================
		//TEXTCHANGED DA PESQUISA POR NIF EMPRESA
		//=========================================================================================
		private void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DLEquipamentos.DataSource = null;
			DLEquipamentos.DataBind(); 
			
		}

		//=========================================================================================
		//Botão submeter para pesquisar por detalhes da empresa
		//=========================================================================================
		private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DLEquipamentos.DataSource = null;
			DLEquipamentos.DataBind(); 
			
		}

		//=========================================================================================
		//selectedindexchanged da dropdown empresa, provoca bindlist dos equipamentos
		//=========================================================================================
		private void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DLEquipamentos.EditItemIndex = -1; //para fechar se estiver aberto
			BindList(); 
		}

		//=========================================================================================
		//botao submeter provoca bindlist dos equipamentos
		//=========================================================================================
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			DLEquipamentos.EditItemIndex = -1;
			BindList(); 
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{


			BindList(); 
		}


		//*************************************************************************************************
		//*************** DROPDOWNS ETC     ***************************************************************
		//*************************************************************************************************

		//=========================================================================================
		//=========================================================================================
		private void fillDropDowns(DropDownList ddClasse, DropDownList ddUnidadeAlcance)
		{
			

			//vou buscar ao precario pq sei q nao precario isto já está definido.
			DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 
			SqlDataReader dr= preco.DRClasse(); 
			ddClasse.DataSource=dr; 
			ddClasse.DataBind();  
			dr.Close(); 
			ddClasse.Items.Insert(0,new ListItem("",""));

			SqlDataReader dr2= preco.DRListaUnidadeAlcance(); 
			ddUnidadeAlcance.DataSource=dr2; 
			ddUnidadeAlcance.DataBind(); 
			dr2.Close(); 
			ddUnidadeAlcance.Items.Insert(0,new ListItem("",""));


			preco = null;
		}
		
		//=========================================================================================
		//=========================================================================================
		private void fillEquipamentosInsert(DropDownList ddTipoEquipamento)
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaTiposEquipamento();
			ddTipoEquipamento.DataSource = DR2; 
			ddTipoEquipamento.DataBind(); 
			ddTipoEquipamento.Items.Insert(0,new ListItem("","")); //para o primeiro nunca ser ELE e isto funcionar com menos codigo.
			DR2.Close(); 

			lista = null; 
		}


		//=========================================================================================
		//=========================================================================================
		private void fillEquipamentosUpdate(DropDownList ddTipoEquipamento, string idTipoEquipamento)
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaEquipamentosMesmaGrandeza(idTipoEquipamento); 
			ddTipoEquipamento.DataSource = DR2; 
			ddTipoEquipamento.DataBind(); 
			ddTipoEquipamento.Items.Insert(0,new ListItem("","")); //para o primeiro nunca ser ELE e isto funcionar com menos codigo.
			DR2.Close();

			lista = null; 
		}

        ////=========================================================================================
        ////no update, nao existe este tipo de validacao
        ////=========================================================================================
        //protected void ddTipoEquipamento_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList list = (DropDownList)sender;
			
        //    DataListItem dli = (DataListItem) list.Parent; //n sei se preciso disto

        //    //declaração dos controls===================================================
        //    DropDownList ddMarca = (DropDownList)dli.FindControl("ddMarcaHeader"); 
        //    DropDownList ddModelo = (DropDownList)dli.FindControl("ddModeloHeader"); 
        //    //============================================================================

        //    //ViewState["grandeza"] = Grandeza(list).ToString(); 
        //    fillMarcas(ddMarca); 
			
        //}


        ////=========================================================================================
        ////=========================================================================================
        //private void fillMarcas(DropDownList ddMarca)
        //{
        //    DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
        //    SqlDataReader DR =  lista.DRListaMarcas(); 
        //    ddMarca.DataSource = DR; 
        //    ddMarca.DataBind(); 
        //    ddMarca.Items.Insert(0,new ListItem("---",""));
        //    DR.Close(); 
        //    lista = null; 


			
        //}
		
		//=========================================================================================
		//=========================================================================================		
		protected void ddMarca_SelectedIndexChanged(object sender, EventArgs e)
		{
            DropDownList dd = (DropDownList)sender;
            DataListItem dli = DLEquipamentos.Items[DLEquipamentos.EditItemIndex]; 
            //DataListItem dli = (DataListItem)dd.Parent;
            ObjectDataSource dsModelo = (ObjectDataSource)dli.FindControl("OBJDS_ModeloEdit");
            
            dsModelo.SelectParameters["idMarca"].DefaultValue = dd.SelectedValue;
            
			
            //com o updatepanel, parece que o parent já é outro.....
            //DataListItem dli = (DataListItem) dd.Parent;
            //if(dd.ID.ToString()=="ddMarcaHeader")
            //{
            //    DropDownList ddMarcaHeader =(DropDownList)dli.FindControl("ddMarcaHeader"); 
            //    DropDownList ddModeloHeader =(DropDownList)dli.FindControl("ddModeloHeader"); 
            //    fillModelos(ddModeloHeader,ddMarcaHeader.SelectedValue.ToString()); 

            //}
            //else if(dd.ID.ToString()=="ddMarca")
            //{
            //    DropDownList ddMarca =(DropDownList)dli.FindControl("ddMarca"); 
            //    DropDownList ddModelo =(DropDownList)dli.FindControl("ddModelo"); 
            //    fillModelos(ddModelo,ddMarca.SelectedValue.ToString()); 
            //}
		}


        //=========================================================================================
        //=========================================================================================		
        protected void ddMarcaHeader_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = (DropDownList)sender;
            DataListItem dli = DLEquipamentos.Items[DLEquipamentos.EditItemIndex];
            //DataListItem dli = (DataListItem)dd.Parent;
            ObjectDataSource dsModelo = (ObjectDataSource)dli.FindControl("OBJDS_ModeloInsert");

            dsModelo.SelectParameters["idMarca"].DefaultValue = dd.SelectedValue;


            //com o updatepanel, parece que o parent já é outro.....
            //DataListItem dli = (DataListItem) dd.Parent;
            //if(dd.ID.ToString()=="ddMarcaHeader")
            //{
            //    DropDownList ddMarcaHeader =(DropDownList)dli.FindControl("ddMarcaHeader"); 
            //    DropDownList ddModeloHeader =(DropDownList)dli.FindControl("ddModeloHeader"); 
            //    fillModelos(ddModeloHeader,ddMarcaHeader.SelectedValue.ToString()); 

            //}
            //else if(dd.ID.ToString()=="ddMarca")
            //{
            //    DropDownList ddMarca =(DropDownList)dli.FindControl("ddMarca"); 
            //    DropDownList ddModelo =(DropDownList)dli.FindControl("ddModelo"); 
            //    fillModelos(ddModelo,ddMarca.SelectedValue.ToString()); 
            //}
        }

		//=========================================================================================
		//=========================================================================================
		private void fillModelos(DropDownList ddModelo, string idmarca)
		{
            //if(idmarca!="")
            //{
            //    DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            //    SqlDataReader DR = lista.DRListaModelos(idmarca); 
            //    ddModelo.DataSource= DR; 
            //    ddModelo.DataBind(); 
            //    ddModelo.Items.Insert(0,new ListItem("",""));
            //    DR.Close(); 

            //    lista = null; 
            //}
		}
		//*************************************************************************************************
		//*************** FUNÇOES ACESSÓRIAS***************************************************************
		//*************************************************************************************************


		//=========================================================================================
		//validaoEquipamento DENTRO DO ITEM UPDATE 
		//num Serie- num Ident. nao pode estar ambos vazios
		//nao podem conter ambos ---
		//se um contem --- o outro nao pode estar vazio.
		//=========================================================================================
		protected void validaEquipamento(object source, ServerValidateEventArgs args) 
		{

			
			System.Web.UI.WebControls.CustomValidator val = (CustomValidator)source; 
			DataListItem dli = (DataListItem) val.Parent;

			TextBox txtNumIdentif =(TextBox)dli.FindControl("txtNumIdentif"); 
			TextBox txtNumSerie =(TextBox)dli.FindControl("txtNumSerie"); 
 
			if((txtNumIdentif.Text.Length == 0 && txtNumSerie.Text.Length ==0) ||(txtNumIdentif.Text.ToString()== "---" && txtNumSerie.Text.ToString() =="---") ||(txtNumIdentif.Text.ToString() =="---" && txtNumSerie.Text.Length==0) || (txtNumSerie.Text.ToString() == "---" && txtNumIdentif.Text.Length==0))
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true; 
			}
		}

		//=========================================================================================
		//validaoEquipamento DENTRO DO ITEM INSERT 
		//num Serie- num Ident. nao pode estar ambos vazios
		//nao podem conter ambos ---
		//se um contem --- o outro nao pode estar vazio.
		//=========================================================================================
		protected void validaEquipamentoHeader(object source, ServerValidateEventArgs args) 
		{
			
			System.Web.UI.WebControls.CustomValidator val = (CustomValidator)source; 
			
			DataListItem dli = (DataListItem) val.Parent;
			
				TextBox txtNumIdentif =(TextBox)dli.FindControl("txtNumIdentifHeader"); 
				TextBox txtNumSerie =(TextBox)dli.FindControl("txtNumSerieHeader"); 

				if((txtNumIdentif.Text.Length == 0 && txtNumSerie.Text.Length ==0) ||(txtNumIdentif.Text.ToString()== "---" && txtNumSerie.Text.ToString() =="---") ||(txtNumIdentif.Text.ToString() =="---" && txtNumSerie.Text.Length==0) || (txtNumSerie.Text.ToString() == "---" && txtNumIdentif.Text.Length==0))
				{
					args.IsValid = false;
				}
				else
				{
					args.IsValid = true; 
				}
			
		}

		//=========================================================================================
		//DEVOLVE A GRANDEZA DO TIPO DE EQUIPAMENTO
		//=========================================================================================
		private string Grandeza(DropDownList ddTipoEquipamento)
		{
			if(ddTipoEquipamento.SelectedValue!="")
			{
				DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 
				string s =  equipamento.GetGrandeza(ddTipoEquipamento.SelectedValue); 
				equipamento = null; 
				return s; 
			}
			else
			{
				return "";
			}
			
		}

		//=========================================================================================
		//=========================================================================================
		protected string ConverteEstado(bool b)
		{
			if (b==true) 
			{
				return "activo";
			}
			else
			{
				return "inactivo"; 
			}
		}

		private void DLEquipamentos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void btnLimparCampos_Click(object sender, System.EventArgs e)
		{
			
			txtTipoEquipamento.Text=""; 
			txtNumIdent.Text="";
			txtNSerie.Text=""; 
		}

		
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


	}
}


