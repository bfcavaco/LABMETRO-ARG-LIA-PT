using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Configuration;

namespace LabMetro
{
	/// <summary>
	/// Summary description for FormEquipamento.
	/// </summary>
	public partial class FormEquipamento : System.Web.UI.Page
	{
		private const string ID_PAG = "EQUIPAMENTOS_1";//NOME DA PAGINA
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

		protected void Page_Load(object sender, System.EventArgs e)
		{
		
			//esta página vai ter de ser revista!!!!
			//dm. 25-06-2007
            //<ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="ddMarca" Category="Marca" PromptText="Seleccione a marca" ServicePath="~/Webservices/wsMarcaModeloByGrandeza.asmx"ServiceMethod="GetMarcas"  --> 
//        http://stackoverflow.com/questions/730555/dropdownlist-appenddatabounditems-first-item-to-be-blank

    		lblMessage.Text ="";
			lblTipoEquipamento.Text = "";


            switch (myApp)
            {

                case "DZ_LABMETRO":
                    txtCriteriosAceitacao.Enabled = true;
                    break;
                default:
                    txtCriteriosAceitacao.Enabled = false;
                    break;

            }


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
                        ViewState["idModelo"] = "";
						if(Request.QueryString["id"]!=null)
						{
							if(Request.QueryString["id"].ToString()!="")
							{
								ViewState["idEquipamento"] = Request.QueryString["id"];
							}
						}

						fillDropDowns(); 
						//fillMarcas();      
						//para preencher a marca, preciso de saber a grandeza primeiro                  

                        //ddMarcaForm.DataSourceID = "OBJDS_Marca_WS";
                        //ddMarcaForm.DataBind();
					
						if(ViewState["idEquipamento"] != null) //UPDATE
						{
							//o fillEquipamentosUpdate é chamado no fillForm.
							///fillDDEmpresa();//2021 tirei isto, no update, nao quero preencher as empresas todas
							//dentro do fillform vou chamar apenas aquela empresa

                            //tive um probelma com a ddModelo cujo databound era sempre chamado 2 x
                            //por isso tenho de fazer aqui o bind manual da ddmarca - maio 2010
                            //ddMarcaForm.DataSourceID = "OBJDS_Marca_WS";
                            //ddMarcaForm.DataBind();
                            //

                            fillForm(ViewState["idEquipamento"].ToString()); 
							txtPesquisaEmpresa.Enabled=false; 
							txtPesquisaNif.Enabled= false; 
							btnEmpresas.Enabled=false; 
							ddEmpresa.Enabled=false; 
							btnSubmit.CommandArgument = "update"; 
						}
						else //INSERT 
						{
							btnSubmit.CommandArgument ="insert"; 
							fillEquipamentosInsert();
                            
						}   
					}
					else //é postback mas entretanto pode ter ja a variavel do id Preenchida
					{
						if(ViewState["idEquipamento"] != null) //UPDATE
						{
							btnSubmit.CommandArgument = "update"; 
							//aqui ainda pode alterar as outras coisas todas, pq
							//supostamente ainda nao saiu desta página. 
						}
					}
				} 
			}
		}

        protected void MyListDataBound(object sender, EventArgs e)
        {

            //entra sempre 2 vezes aqui e nao funciona o set da dropdown, pq parece sempre que faz um databound novamente no final... ja nao sei nada!
            //agora ja nao entra 2x pq removi a datasrouce da pagina aspx da ddMarcaFrom e coloquei aqui no pageload... grande SECA!!!!

            //OnDatabound: Occurs after the server control binds to a data source.
            //mesmo assim, dps disto tudo, é chamado o databind NOVAMENTE... quando faço dps, vou la parar depois disto... nao percebo...


            //http://stackoverflow.com/questions/730555/dropdownlist-appenddatabounditems-first-item-to-be-blank

            ddModeloForm.Items.Insert(0, new ListItem("---", ""));
           // Response.Write("<-->" + ViewState["idModelo"].ToString() + "<--><br />");

            try
            {
                ddModeloForm.SelectedValue = ViewState["idModelo"].ToString(); //nao 
            }
            catch 
            {
                //Response.Write(ex.ToString()); 
            }
           

        }

		private void fillEquipamentosInsert()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaTiposEquipamento();
			ddTipoEquipamento.DataSource = DR2; 
			ddTipoEquipamento.DataBind(); 
			ddTipoEquipamento.Items.Insert(0,new ListItem("","")); //para o primeiro nunca ser ELE e isto funcionar com menos codigo.
			DR2.Close(); 

			lista = null; 
		}


		private void fillEquipamentosUpdate(string idTipoEquipamento)
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR2 = lista.DRListaEquipamentosMesmaGrandeza(idTipoEquipamento); 
			ddTipoEquipamento.DataSource = DR2; 
			ddTipoEquipamento.DataBind(); 
			ddTipoEquipamento.Items.Insert(0,new ListItem("","")); //para o primeiro nunca ser ELE e isto funcionar com menos codigo.
			DR2.Close();

			lista = null; 
		}


		private void fillDDEmpresaSoActual(string idEmpresa)
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
			DataTable DT = empresa.DTEmpresas(idEmpresa); //so a actual

			DataView DV = new DataView(DT);

			ddEmpresa.DataSource = DV; ;
			ddEmpresa.DataBind();

			empresa = null;
		}

		private void fillDDEmpresa()
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,txtPesquisaNif.Text,"1","","","","","",""); //activas
            
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 
		}
		    
		private void fillDropDowns()
		{
			//DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
			          

			//vou buscar ao precario pq sei q nao precario isto já está definido.
			//DM
			DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 
			SqlDataReader dr= preco.DRClasse(); 
			ddClassse.DataSource=dr; 
			ddClassse.DataBind();  
			dr.Close(); 
			
			ddClassse.Items.Insert(0,new ListItem("",""));

			SqlDataReader dr2= preco.DRListaUnidadeAlcance(); 
			ddUnidadeAlcance.DataSource=dr2; 
			ddUnidadeAlcance.DataBind(); 
			dr2.Close(); 

			ddUnidadeAlcance.Items.Insert(0,new ListItem("",""));

            preco = null; 
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

		
			txtPesquisaEmpresa.TextChanged += new System.EventHandler(txtPesquisaEmpresa_TextChanged);
			btnEmpresas.Click += new System.EventHandler(btnEmpresas_Click);
			
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			txtPesquisaNif.TextChanged += new System.EventHandler(txtPesquisaNif_TextChanged);
		}
		#endregion


		private void btnSubmit_Click(object sender, System.EventArgs e)
		{   
			if(Page.IsValid)
			{
				executeDB(); 
			}
		}

		private void executeDB()
		{
			string	idmarca = hfidMarca.Value; //ddMarcaForm.SelectedValue;
			string	idmodelo = ddModeloForm.SelectedValue; 
			
			string strCalibInt = ""; 
			if(cbCalibInt.Checked == true) strCalibInt = "1"; 

			string CertificadoConclusivo = ""; 
			if(cbCertConclusivo.Checked == true) CertificadoConclusivo = "1"; 

			DATA.EquipamentoBD equipamento = new LabMetro.DATA.EquipamentoBD(); 

			try

			{
				string idEquipamento = ""; 
				if(ViewState["idEquipamento"] != null && ViewState["idEquipamento"].ToString()!="")
				{
					idEquipamento = ViewState["idEquipamento"].ToString(); 
				}
				
				string operation = btnSubmit.CommandArgument.ToString(); 
				
				lblMessage.Text = equipamento.InsertUpdateEquipamento(idEquipamento,ddEmpresa.SelectedValue, ddTipoEquipamento.SelectedValue,txtNumSerie.Text,txtNumIdentif.Text,txtAlcanceInf.Text,txtAlcanceSup.Text,ddUnidadeAlcance.SelectedValue.ToString(), txtAlcance.Text, txtResolucao.Text, idmodelo, idmarca,ddClassse.SelectedValue.ToString(),"", txtForma.Text, txtFabricante.Text, txtRefUltimaCalib.Text, txtDataUltimaCalib.Text, txtPeriodicidade.Text, ddEstado.SelectedValue.ToString(),txtObservacoes.Text,operation, strCalibInt, CertificadoConclusivo, txtCampo1.Text, txtCampo2.Text, txtEtiqueta1.Text, txtEtiqueta2.Text, txtEtiqueta3.Text, txtCriteriosAceitacao.Text).ToString(); 
				if(operation =="insert") btnSubmit.Enabled=false; 
				
			}
			catch (Exception ex)
			{
                lblMessage.Text = ex.ToString();
            }
			equipamento = null; 
		}
	       
		private void fillForm(string id)
		{
			LabMetro.DATA.EquipamentoBD fillEquipment= new LabMetro.DATA.EquipamentoBD(); 
			LabMetro.DATA.EquipmentDetails dt = fillEquipment.GetEquipmentDetails(id);  
			if(dt!= null)
			{

				fillDDEmpresaSoActual(dt.idEmpresa); 
				try
				{
					ddEmpresa.SelectedValue = dt.idEmpresa; 
				}
				catch 
				{
					ddEmpresa.Items.Insert(0,new ListItem(dt.nomeEmpresa,dt.idEmpresa));
					ddEmpresa.SelectedValue = dt.idEmpresa;
				}
				ddEstado.SelectedValue = dt.activo;
	            lblIdEquipamento.Text = dt.idEquipamento; 
				fillEquipamentosUpdate(dt.idTipoEquipamento); 

				try
				{
					ddTipoEquipamento.Items.FindByValue(dt.idTipoEquipamento).Selected = true;
					//ddTipoEquipamento.SelectedValue = dt.idTipoEquipamento; 
				}
				catch 
				{
					ddTipoEquipamento.Items.Insert(0, new ListItem(dt.tipoEquipamento, ""));
					//ponho o id a emtpy para obrigar a preencher

					ddTipoEquipamento.SelectedIndex = 0; //o primeiro
					lblTipoEquipamento.Text = "Atenção, tipo INACTIVO! Por favor actualizar o tipo!";
					lblTipoEquipamento.ForeColor = Color.Red;
					ddTipoEquipamento.Focus();
				}


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
					ddClassse.SelectedValue = dt.idClasse; 
				}
				catch 
				{	                
				}
				
                //txtClasse.Text = dt.classe; -- campo eliminado
				txtRefUltimaCalib.Text = dt.refUltimaCalibracao; 
				txtDataUltimaCalib.Text = GERAL.clsGeral.ToShortDate(dt.dtUltimaCalibracao); 
				txtFabricante.Text = dt.fabricante; 
				txtForma.Text = dt.forma; 
				string grandeza = dt.idGrandeza; 

                if (dt.idmarca != "")
                {
                    try
                    {
                        //ddMarcaForm.SelectedValue = dt.idmarca; 
                        txtSearchMarca.Text = dt.marca;
                        hfidMarca.Value = dt.idmarca;
                    }
                    catch
                    {

                    }
                }
				

                ViewState["idModelo"] = dt.idmodelo;


                txtNumIdentif.Text = dt.numIdentificacao;
				txtNumSerie.Text = dt.numSerie;
				txtObservacoes.Text = dt.observacoes; 
				txtPeriodicidade.Text = dt.periodicidadeCalibracao;
				txtResolucao.Text = dt.resolucao; 

				if( dt.calibInt =="True") cbCalibInt.Checked = true; 
				if( dt.certConclusivo =="True") cbCertConclusivo.Checked = true; 

				txtCampo1.Text = dt.campo1;
				txtCampo2.Text = dt.campo2;

                txtEtiqueta1.Text = dt.etiqueta1;
                txtEtiqueta2.Text = dt.etiqueta2;
                txtEtiqueta3.Text = dt.etiqueta3;
                txtCriteriosAceitacao.Text = dt.criterios;

			}
			else
			{
				lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_NO_DATA; 
			}

			BindGridHistorico(dt.idEquipamento); 
			fillEquipment = null; 
		}

       

		private void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		private void btnEmpresas_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}

		protected void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		
		}

		//outro validaoEquipamento que a funcao geral que verifica o equip contra a BD
		protected void validaEquipamento(object source, ServerValidateEventArgs args) 
		{

			//num Serie- num Ident. nao pode estar ambos vazios
			//nao podem conter ambos ---
			//se um contem --- o outro nao pode estar vazio.
			if((txtNumIdentif.Text.Length == 0 && txtNumSerie.Text.Length ==0) ||(txtNumIdentif.Text.ToString()== "---" && txtNumSerie.Text.ToString() =="---") ||(txtNumIdentif.Text.ToString() =="---" && txtNumSerie.Text.Length==0) || (txtNumSerie.Text.ToString() == "---" && txtNumIdentif.Text.Length==0))
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true; 
			}
		}

		
		private void BindGridHistorico(string idEquipamento)
		{
			

			string strSQL = "SELECT TipoEquipamento.descricao AS tipoEquipamento, HistoricoEquipamento.numIdentificacao, HistoricoEquipamento.numSerie, HistoricoEquipamento.refUltimaCalibracao, HistoricoEquipamento.dtAlteracao, Funcionario.nomeAbreviado, HistoricoEquipamento.observacoes FROM HistoricoEquipamento INNER JOIN Funcionario ON HistoricoEquipamento.idUtilAlteracao = Funcionario.idUtilizador INNER JOIN TipoEquipamento ON HistoricoEquipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento WHERE idEquipamento = "+idEquipamento+ " ORDER BY HistoricoEquipamento.dtAlteracao desc "; 

			//Response.Write(strSQL); 
					
			DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			dgHistorico.DataSource =dt;
			dgHistorico.DataBind();
		}
	}
}
