
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
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;

using System.Text;


namespace LabMetro
{
	
	public partial class GestValidarCertificados : System.Web.UI.Page
	{
		private const string ID_PAG = "CERTPENDENTES_1";//NOME DA PAGINA
		
		//A assinatura do Técnico é encontrada no servico pelo funcionário que efectuou o serviço (por linha, mais la em baixo)
		//A assinatura do Responsável é encontrada pelo utilizador que está logado à aplicação
		private string caminhoAssinaturaResponsavel = (string)ConfigurationManager.AppSettings["PASTA_ASSINATURAS_CERTIFICADOS"] + System.Web.HttpContext.Current.User.Identity.Name.ToString() +".gif"; 

		private string caminhoPastaAssinaturas = (string)ConfigurationManager.AppSettings["PASTA_ASSINATURAS_CERTIFICADOS"]; 
		private string caminhoPastaFinaisConstrucao = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CONSTRUCAO"];
		private string caminhoPastaTemporaria = (string)ConfigurationManager.AppSettings["PASTA_TEMPORARIA_CERT"];
		private string caminhoPastaCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];
        private string caminhoPastaCertificadosEspanhois = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS_ES"];

		private string certificado = (string)ConfigurationManager.AppSettings["CAMINHO_CERTIFICADO"];
		private string password = (string)ConfigurationManager.AppSettings["CERT_PASSWORD"];
		private string cert_reason = (string) ConfigurationManager.AppSettings["CERT_REASON"];
		private string cert_location =(string) ConfigurationManager.AppSettings["CERT_LOCATION"];
        private static string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();

		DataTable DT; 
		DataView DV; 
			
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
			btnSearch.Click += new System.EventHandler(btnSearch_Click);
			btnLimparCampos.Click += new System.EventHandler(btnLimparCampos_Click);
			btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			btnAprovarAll.Click += new System.EventHandler(btnAprovarAll_Click);
			btnRejeitarAll.Click += new System.EventHandler(btnRejeitarAll_Click);
			btnDeselectAll.Click += new System.EventHandler(btnDeselectAll_Click);
			cbTodos.CheckedChanged += new System.EventHandler(cbTodos_CheckedChanged);
			dgDocumentos.ItemDataBound += new DataGridItemEventHandler(dgDocumentos_ItemDataBound); 
		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Response.Expires = 0; 	//nao tirar!!!

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
					setupClientScript();

					if(!Page.IsPostBack)
					{
						if (strGrau() == "0")
						{
							cbTodos.Visible = true;
						}
						else
						{
							cbTodos.Visible = false;
						}
						ViewState["sortField"] = "idServico"; //importante
						ViewState["sortDirection"] = "DESC";
						ViewState["nomeResponsavel"] = strNomeResponsavel(); 
						BindGridInicial(); 	
					}

					DT = (DataTable)ViewState["DT"]; 
				}
			}
		}

		//futuramente, fazer aqui um mecanismo automatico para alimentar uma tabela com os dados dos certificados 
		//nas pastas

		//faz fill da datatable DT, contem apenas os serviços que estejam CALIBRADOS COM VALIDAÇÃO,idEstadoCertificado = 3
		//=============================================================================================================
		private void BindGridInicial()
		{
			Hashtable htIdCert = LabMetro.GestDocs.HTIdCertificadoBySigla();  //para ir buscar idCertificado...
			Hashtable HTDescCert = LabMetro.GestDocs.HTDescCertificadoBySigla(); //contem sigla /tipoCertificado(descricao)

			bool admin = false; 
			if(cbTodos.Checked) 
			{
				admin = true;
			}
			
			DATA.EstadoCertificadoBD certificadoBD = new LabMetro.DATA.EstadoCertificadoBD(); 

			DT = certificadoBD.DTCertificadosPorAprovar(txtSearchEmpresa.Text, txtSearchNServico.Text, HttpContext.Current.User.Identity.Name.ToString(),admin);
			
			certificadoBD = null; 				  
	
			DT.Columns.Add(new DataColumn("cbAprovar",typeof(bool))); 
			DT.Columns.Add(new DataColumn("cbRejeitar",typeof(bool)));
			DT.Columns.Add(new DataColumn("nomeFicheiro",typeof(string)));
			DT.Columns.Add(new DataColumn("tipoCertificado",typeof(string)));
            DT.Columns.Add(new DataColumn("siglaCertificado", typeof(string))); //para construir o ficheiro de espanha
			
			DirectoryInfo dirInfoFinais = new DirectoryInfo(caminhoPastaFinaisConstrucao);
       
			foreach(DataRow dr in DT.Rows)
			{
				FileInfo[] fileInfosfinais = dirInfoFinais.GetFiles("*" + dr["nomeDocumentoIn"].ToString() + "*");
				if(fileInfosfinais.Length > 0)
				{
					dr["nomeFicheiro"] = fileInfosfinais[0].ToString(); 
			
					string sigla = strExtraiSigla(fileInfosfinais[0].ToString());  
					//é o value da hashtable/a key é int idTipoCertificado
                    dr["siglaCertificado"] = sigla; 
    
					object x = htIdCert[sigla];
					int idTipoCert = System.Convert.ToInt16(x); 
					dr["idTipoCertificadoEmValidacao"] = idTipoCert.ToString(); 

					object y = HTDescCert[sigla];

					string descTipoCert = System.Convert.ToString(y); 
					dr["tipoCertificado"] = descTipoCert;

				}

				dr["cbAprovar"] = false; 
				dr["cbRejeitar"] = false;
			}


			foreach(DataRow dr in DT.Rows)
			{
				if(dr["nomeFicheiro"].ToString() == "")
				{
					dr.Delete();
				}
			}

			dgDocumentos.DataSource = DT;
			dgDocumentos.DataBind();

			DT.AcceptChanges(); 
			ViewState["DT"] = DT; 
		}
		
        private void BindGrid()
		{
			DataTable DT = (DataTable) ViewState["DT"]; 
			DV = new DataView(DT); 
			DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];

			dgDocumentos.DataSource = DV;
			dgDocumentos.DataBind();
			
		}

		//*******************************************************************************
		//*******************************************************************************
		public void dgDocumentos_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
			{		
				LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];
				
				CheckBox cbAprovar = (CheckBox)e.Item.FindControl("cbAprovar");
				CheckBox cbRejeitar = (CheckBox)e.Item.FindControl("cbRejeitar");
				TextBox txtObs =(TextBox)e.Item.FindControl("txtObservacoes");
				txtObs.ToolTip = txtObs.Text.ToString();

				for (int i = 1; i < e.Item.Cells.Count; i++)
				{
					if(!e.Item.Cells[i].HasControls())
					{
						e.Item.Cells[i].ToolTip = "Click para visualisar o documento " + e.Item.Cells[5].Text;
						//e.Item.Cells[i].Attributes.Add("onclick", Page.GetPostBackClientHyperlink(button, ""));
                        e.Item.Cells[i].Attributes.Add("onclick", ClientScript.GetPostBackEventReference(button, ""));
                    }
				}

				///Definição da cor das linhas dos caminhoPastaCertificados não pertencentes ao user
				Response.Write(e.Item.Cells[9].Text.ToString()); 
				if(e.Item.Cells[10].Text == "False")
				{
					e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE5BF");
					//Se for necessário limitar as opções do utilizador
					if(strGrau()!= "0")
					{
						cbAprovar.Enabled = false;
						cbRejeitar.Enabled = false;
					}
				}
			}
		}
		//===============================================================================
		//===============================================================================
		public void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{

			SaveCheckBoxValuesToDT(); 
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

	
		//===============================================================================
		//===============================================================================
		private void btnAprovarAll_Click(object sender, System.EventArgs e)
		{   
			foreach(DataGridItem dgi in dgDocumentos.Items) 
			{ 
				CheckBox cbAprovar =(CheckBox)dgi.Cells[0].FindControl("cbAprovar"); 
				cbAprovar.Checked = true; 
				CheckBox cbRejeitar =(CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
				cbRejeitar.Checked = false; 				
			}  
		}

		//===============================================================================
		//===============================================================================
		private void btnRejeitarAll_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in dgDocumentos.Items) 
			{ 				
				CheckBox cbAprovar =(CheckBox)dgi.Cells[0].FindControl("cbAprovar"); 
				cbAprovar.Checked = false; 
				CheckBox cbRejeitar =(CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
				cbRejeitar.Checked = true; 				
			}  
		}
		//===============================================================================
		//===============================================================================
		private void btnDeselectAll_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem dgi in dgDocumentos.Items) 
			{ 
				CheckBox cbAprovar =(CheckBox)dgi.Cells[0].FindControl("cbAprovar"); 
				cbAprovar.Checked = false; 
				CheckBox cbRejeitar =(CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
				cbRejeitar.Checked = false; 				
			}
		}

		//===============================================================================
		//===============================================================================
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			dgDocumentos.CurrentPageIndex = 0;
			BindGridInicial();
		}

		//===============================================================================
		//===============================================================================
		private void btnLimparCampos_Click(object sender, System.EventArgs e)
		{
			txtSearchNServico.Text=""; 
			txtSearchEmpresa.Text=""; 
		}

		//DM**************************************************************************************
		//valida se ha dentro da mesma linha caixas marcadas para aprovar e rejeitar em simultaneo	
		private bool VerificaCheckBoxes()
		{
			foreach(DataGridItem dgi in dgDocumentos.Items)
			{
				CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
				CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
				
				if(cbAprovar.Checked && cbRejeitar.Checked) 
				{
					return false; 
				}
			}
			return true; 
		}

		private void SaveCheckBoxValuesToDT()
		{
			DV = new DataView(DT);
			DV.RowStateFilter = DV.RowStateFilter= DataViewRowState.CurrentRows;

			//para o find que vem a seguir. 
			//posso fazer o sort pela dataview mas o update à datatable
			DV.Sort = "idServico"; 

			//vai buscar as checkboxes
			foreach(DataGridItem dgi in dgDocumentos.Items)
			{	
				//significaria que nao encontra na dataview algum documentos que encontrou no datagrid?!?
				int index = DV.Find(dgDocumentos.DataKeys[dgi.ItemIndex]); 
 		    
				if(index >-1)
				{
					CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
					CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");

					TextBox txtObs =(TextBox)dgi.FindControl("txtObservacoes");
					DT.Rows[index]["obsWorkflowCertificado"] = txtObs.Text.ToString();
				
					if(cbAprovar.Checked && cbRejeitar.Checked) 
					{
						cbAprovar.Checked=false;
						cbRejeitar.Checked=false;
						DT.Rows[index]["cbAprovar"] = "False"; 
						DT.Rows[index]["cbRejeitar"] = "False"; 
					}
					else
					{
						DT.Rows[index]["cbAprovar"] = cbAprovar.Checked.ToString(); 
						DT.Rows[index]["cbRejeitar"] = cbRejeitar.Checked.ToString(); 
					}
				}
			}

			DT.AcceptChanges(); //suponho que quando chamo o acceptchanges já tudo o que foi feito antes (rowstates) 
			//ficam limpos, verificar

			ViewState["DT"] = DT; //so preciso guardar dt em viewstate quando faço alterações
		}


		//===============================================================================
		//ACCAO DA CHECKBOX CHECK TODOS CHAMA OUTRA VEZ A FUNCAO BINDGRIDINICIAL
		//===============================================================================
		private void cbTodos_CheckedChanged(object sender, System.EventArgs e)
		{
			BindGridInicial();
		}
		
		//==============================================================================
		//BOTAO SUBMETER DA PAGINA
		//==============================================================================
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(VerificaCheckBoxes()==false)
			{
				lblMessage.Text ="Verifique as checkboxes, não pode aprovar e rejeitar um certificado ao mesmo tempo."; 
				return; 
			}
			else
			{
				SaveCheckBoxValuesToDT();
				
				DV = new DataView(DT);

				string strIds = "";
				string strIdsPais = "";
				string strEstActual="";
				string strEstNovo = "";
				string strtCert = "";
				
				string sSearch = "cbRejeitar = True"; //ele nao apanhar isto aqui... converter para algo!
				DataView DVR = new DataView(DT);
				DVR.RowFilter=sSearch;


				sSearch = "cbAprovar = True"; //ele nao apanhar isto aqui... converter para algo!
				DataView DVA = new DataView(DT);
				DVA.RowFilter=sSearch;

				//preciso de fazer aqui os search pq preciso de saber o tamanho de ambos para o array
				string[] obsWorkflow = new string[DVR.Count+DVA.Count]; //tem de ter o tamanho dos rejeitados e aprovados
				int n = 0;

				//::::::::::::::::::::::::::::::::::::::::::::::::
				//certificados a aprovar
				//::::::::::::::::::::::::::::::::::::::::::::::::
				foreach(DataRowView drv in DVA)
				{ 	
					string userNameTecLab; 
					string nomeTecLab; 							
					
					string idServico = drv["idServico"].ToString();
					string idServicoPai = drv["idServicoPai"].ToString();
					string idEstadoActual = drv["idEstadoCertificado"].ToString();
					
					string tCert = drv["idTipoCertificadoEmValidacao"].ToString();
					string nomeDocumentoIn = drv["nomeFicheiro"].ToString();
					string nomeDocumentoOut = drv["nomeDocumentoOut"] + "-"+strExtraiSigla(nomeDocumentoIn); 
					string refServicoPai = drv["refServicoPai"].ToString(); 
					
					switch(drv["idTipoCertificadoEmValidacao"].ToString())
					{
						//1º certificado ou qq revisão, aparece sempre o nome de quem CALIBROU (=EFECTUOU SERVIÇO)
						case "1": //1C
						case "3": //1R
						case "7": //2R
						case "8": //3R
							userNameTecLab = drv["userNameTecCal"].ToString(); //usernamem de quem CALIBROU
							nomeTecLab = drv["funcionarioCalibrou"].ToString(); //NOME DE QUEM CALIBROU SERVIÇO
							break; 

						default: //todos os outros
							userNameTecLab = drv["userNameTecVal"].ToString(); //usernamem de quem VALIDOU
							nomeTecLab = drv["funcionarioValidou"].ToString(); //NOME DE QUEM VALIDOU SERVIÇO
							break; 
					}
					
					string letterRefServico = drv["refServico"].ToString().Substring(0,1); 
					string sTipoServico = drv["refServico"].ToString().Substring(0,4); 
					string sEquipamento = drv["equipamento"].ToString();
                    string idSubtipoServico = "";
                    string refServicoCertificado = drv["refServicoCertificado"].ToString(); 
                    string sigla = drv["siglaCertificado"].ToString();
                    string linguaCertificado = drv["linguaCertificado"].ToString();

                    if(!Convert.IsDBNull(drv["idSubtipoServico"])) idSubtipoServico = drv["idSubtipoServico"].ToString(); 
		

					//::::::::::::::::::::::::::
					//:: move o ficheiro :::::::
					//::::::::::::::::::::::::::
					if(assinarEmoverDocumentos(idServico, nomeDocumentoIn,userNameTecLab,nomeTecLab,letterRefServico,sTipoServico, sEquipamento,refServicoPai, nomeDocumentoOut,idSubtipoServico,refServicoCertificado,sigla,linguaCertificado)==true)
					{
						strIds+= idServico +",";
						strIdsPais += idServicoPai + " ,";
						strEstActual +=idEstadoActual +","; 
						strEstNovo+= 4 + ",";
						obsWorkflow[n] = string.Empty;//quando sao aprovados, as observacoes n interessam
						n+=1; 	
						strtCert+= tCert+",";
					}	
					
				}
				
				
				//::::::::::::::::::::::::::::::::::::::::::::::::
				//certificados a rejeitar
				//::::::::::::::::::::::::::::::::::::::::::::::::
			
				//este aqui serviria no caso de ser preciso fazer um update separado idservico-observaoces, para nao sobrecarregar a outra string - e isto so é preciso no rejeitar pq so os rejeitados guardam as observacoes
				string[,] sObserv = new string[DV.Count,2]; //tantos pares de 2 quanto linhas na DV
				int i1 = 0; 
				int i2 = 0; 
				
				foreach(DataRowView drv in DVR)
				{ 			
					string idServico = drv.Row["idServico"].ToString();
					strIds+= idServico +",";

					string idServicoPai = drv["idServicoPai"].ToString();
                    strIdsPais += idServicoPai + " ,"; //isto nos maus nao vai ser assinarCertificadoDigitalmente, mas just incase...

					string idEstadoActual = drv["idEstadoCertificado"].ToString();
					strEstActual +=idEstadoActual +","; 

					string tCert = drv.Row["idTipoCertificadoEmValidacao"].ToString();
					strtCert+= tCert+",";	

					strEstNovo+= 6 + ",";	//REIJEITADOS - NAO MUDA O ESTADO DO SERVIÇO

					obsWorkflow[n]= drv.Row["obsWorkflowCertificado"].ToString();
					n+=1;


					//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
					//isto squi é uma coisa aparte que preenche um array de 2 dimensoes com idServico+observacoes
					//para permitir fazer dps um update separado das observacoes (por causa do tamanho das strings). 
					//nao estou a usar este campo, mas poderia usa-lo para fazer update idServico + observacoes;
					//dps disso faria update
					
					sObserv[i1,i2] = idServico; //[0,0]; [1,0]; [2,0] etc
					i2+=1; 
					
					sObserv[i1,i2] = drv.Row["obsWorkflowCertificado"].ToString(); //[0,1] ; [1,1]; [2,1] etc

					i1 +=1 ; //as linhas aumentam
					i2 = 0;  //as colunas sao sempre 0 ou 1 
					//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
				}

				char [] delimiter = ",".ToCharArray();
				

				if(strIds!="")
				{
					strIds = strIds.TrimEnd(delimiter);
					string [] idsServicos = strIds.Split(delimiter); 

					strEstNovo = strEstNovo.TrimEnd(delimiter);
					string [] idsEstadosNovosCertificado = strEstNovo.Split(delimiter); 

					strEstActual = strEstActual.TrimEnd(delimiter); 
					string [] idsEstadosActuaisCertificado = strEstActual.Split(delimiter); 

					strIdsPais = strIdsPais.TrimEnd(delimiter); 
					string [] idsServicosPais = strIdsPais.Split(delimiter); 
					
					strtCert = strtCert.TrimEnd(delimiter); 
					string [] idsTipoCertificado = strtCert.Split(delimiter);

					//observacoe já estão em array					
					
						if(idsServicos.Length > 0)
					{

						//aqui ha uma fonte de problemas quando a string é demasiado grande, mas nao é tratada aqui porque de qualquer maneira ja nao dá para fazer undo de tudo isto, futuramente, pensar numa maneira de solucionar isto! um loop um por um, ou algo assim.... 
						//updateEstadoCertificados(idsServicos, idsEstadosActuaisCertificado,idsEstadosNovosCertificado, observacoes, idsTipoCertificado,idsServicosPais);

						updateEstadoCertificados(idsServicos, idsEstadosActuaisCertificado,idsEstadosNovosCertificado, obsWorkflow, idsTipoCertificado,idsServicosPais);
					}


                    deleteTempFiles(); //apagar temporarios - penso que este código é obsoleto. dm. out 2007
					BindGridInicial(); 
				}
				
			}
		}


		private void updateEstadoCertificados(string[] idsServicos,string[] idsEstadosActuais,  string[] idsEstadosNovos, string[] observacoes, string[] idsTipoCertificado, string[] idsServicoPai)
		{
			DATA.EstadoCertificadoBD certificadoBD = new LabMetro.DATA.EstadoCertificadoBD(); 

			certificadoBD.UpdateEstadosCertificados(idsServicos,idsEstadosActuais, idsEstadosNovos,User.Identity.Name.ToString(), observacoes, idsTipoCertificado, idsServicoPai);

			certificadoBD = null; 

		}

		//*******************************************************************************************
		//*******************************************************************************************
		//*********************CÓDIGO RELACIONADO COM A INSERÇÃO DA ASSINATURA, ECT *****************
		//*******************************************************************************************
		//*******************************************************************************************


		private string sEfectuado (string s)
		{
			string textoEfectuado =""; 
			switch(s)
			{
				case "C":
                    textoEfectuado = Resources.Resource.CalibradoPor; ; //"Calibrado por" ; 
					break; 
				case "E":
					textoEfectuado = Resources.Resource.EnsaiadoPor; //"Ensaiado por" ;  
					break; 
				case "R":
					textoEfectuado = Resources.Resource.ReparadoPor;  //"Reparado por" ;
					break; 
				case "V":
					textoEfectuado = Resources.Resource.VerificadoPor; //"Verificado por" ;  
					break; 
				default:
                    textoEfectuado = Resources.Resource.EfectuadoPor; // "Efectuado por"; 
					break;
			}
		
			return textoEfectuado;
		}

        private string sEfectuadoEN(string s)
        {
            string textoEfectuado = "";
            switch (s)
            {
                case "C":
                    textoEfectuado = Resources.Resource.CalibradoPorEN; ; //"Calibrado por" ; 
                    break;
                case "E":
                    textoEfectuado = Resources.Resource.EnsaiadoPorEN; //"Ensaiado por" ;  
                    break;
                case "R":
                    textoEfectuado = Resources.Resource.ReparadoPorEN;  //"Reparado por" ;
                    break;
                case "V":
                    textoEfectuado = Resources.Resource.VerificadoPorEN; //"Verificado por" ;  
                    break;
                default:
                    textoEfectuado = Resources.Resource.EfectuadoPorEN; // "Efectuado por"; 
                    break;
            }

            return textoEfectuado;
        }

        private string sEfectuadoES(string s)
        {
            string textoEfectuado = "";
            switch (s)
            {
                case "C":
                    textoEfectuado = Resources.Resource.CalibradoPorES; ; //"Calibrado por" ; 
                    break;
                case "E":
                    textoEfectuado = Resources.Resource.EnsaiadoPorES; //"Ensaiado por" ;  
                    break;
                case "R":
                    textoEfectuado = Resources.Resource.ReparadoPorES;  //"Reparado por" ;
                    break;
                case "V":
                    textoEfectuado = Resources.Resource.VerificadoPorES; //"Verificado por" ;  
                    break;
                default:
                    textoEfectuado = Resources.Resource.EfectuadoPorES; // "Efectuado por"; 
                    break;
            }

            return textoEfectuado;
        }

        private string sEfectuadoFR(string s)
        {
            string textoEfectuado = "";
            switch (s)
            {
                case "C":
                    textoEfectuado = Resources.Resource.CalibradoPorFR; ; //"Calibrado por" ; 
                    break;
                case "E":
                    textoEfectuado = Resources.Resource.EnsaiadoPorFR; //"Ensaiado por" ;  
                    break;
                case "R":
                    textoEfectuado = Resources.Resource.ReparadoPorFR;  //"Reparado por" ;
                    break;
                case "V":
                    textoEfectuado = Resources.Resource.VerificadoPorFR; //"Verificado por" ;  
                    break;
                default:
                    textoEfectuado = Resources.Resource.EfectuadoPorFR; // "Efectuado por"; 
                    break;
            }

            return textoEfectuado;
        }

		//===============================================================================
		// Insere as rúbricas do Técnico de Laboratório e do Responsável de Laboratório
		// retorna false se nao conseguiu.... (se falta alguma assinatura!)
         
		//		CACV DOSÍMETRO SEM SUBTIPO (novo maio 2010)
        //                    ============
		//		- AS ULTIMAS 2 FOLHAS NAO LEVAM SIMBOLO E LEVAM TEMPLATE SEM ACREDITACAO. gestdocs.aspx

		//		- A ASSINATURA VAI EM TODAS MENOS NA ULTIMA, MAS NA PENULTIMA SO VAI A DO SUPERIOR.


		//		VACV: 
		//		AS ASSINATURAS SO VAO NA 1A E NA 3A E NA 3A SO VAI A DO SUPERIOR?

		//	VAGE, VOPC E REPARAÇÕES SO LEVAM UMA ASSINATURA _ JA NAO
		//===============================================================================
		// Insere as rúbricas do Técnico de Laboratório e do Responsável de Laboratório
		// retorna false se nao conseguiu.... (se falta alguma assinatura!)
		//===============================================================================
		private bool inserirAssinaturas(string caminhoDocumentoEntrada, string caminhoDocumentoFinal, string userNameTecnico, string nomeDocumento, string nomeTecnico, string tipoServicoEfectuado, string sTipoServico, string sEquipamento, string sRefServicoPai,string idSubtipoServico, string linguaCertificado)
		{
			//em vez de usar diferente de "", uso maior a 4...sem razao especial.
			//se o servico tem 1 pai, para verificar qual a grandeza, temos de ir ao pai. 
			if(sRefServicoPai.Length>4) sTipoServico = sRefServicoPai.Substring(0,4);

            //default PT
            string strServico = sEfectuado(tipoServicoEfectuado);
            string nomeTec= nomeTecnico;
            string strValidado = Resources.Resource.ResponsavelPelaValidacao;// "Responsável pela Validação";

            switch (linguaCertificado)
            { 
                case "PT":
                    strServico = sEfectuado(tipoServicoEfectuado);
                    break;
                case "EN":
                    strServico = sEfectuadoEN(tipoServicoEfectuado);
                    strValidado = Resources.Resource.ResponsavelPelaValidacaoEN;
                    break;
                case "FR":
                    strServico = sEfectuadoFR(tipoServicoEfectuado);
                    strValidado = Resources.Resource.ResponsavelPelaValidacaoFR;
                    break;
                case "ES":
                    strServico = sEfectuadoES(tipoServicoEfectuado);
                    strValidado = Resources.Resource.ResponsavelPelaValidacaoES;
                    break;
            }
            
			
            //so podem aprovar 4 + 5 - rt's e 5 te's
            //ponho por default 5 (te) e quando sao do perfil 4 escrevo responsavel técnico
            //a unica exepcao é de momento o luis gonçalves que mudou o seu perfil para admin.... sendo assim ele aparece sempre como tecnico
            
            //por default PT
            string strTecnico = " (" + Resources.Resource.Tecnico + ")";

            if (Session["idPerfil"].ToString() == "4") strTecnico = " (" + Resources.Resource.ResponsavelTecnico + ")";
            
            
            
            switch (linguaCertificado)
                
            {
                case "PT":
                      strTecnico = " (" + Resources.Resource.Tecnico + ")";
                        if (Session["idPerfil"].ToString() == "4") strTecnico = " (" + Resources.Resource.ResponsavelTecnico + ")";
            
                    break;
                case "EN":  
                    strTecnico = " (" + Resources.Resource.TecnicoEN + ")";
                    if (Session["idPerfil"].ToString() == "4") strTecnico = " (" + Resources.Resource.ResponsavelTecnicoEN + ")";
                    break;
                case "FR":
                    strTecnico = " (" + Resources.Resource.TecnicoFR + ")";
                    if (Session["idPerfil"].ToString() == "4") strTecnico = " (" + Resources.Resource.ResponsavelTecnicoFR + ")";
                    break;
                case "ES":
                    strTecnico = " (" + Resources.Resource.TecnicoES + ")";
                    if (Session["idPerfil"].ToString() == "4") strTecnico = " (" + Resources.Resource.ResponsavelTecnicoES + ")";
                    break;
            }
 

            string nomeRespTec = ViewState["nomeResponsavel"].ToString() + strTecnico;
                 
            //*******************************************************
			//assinatura do responsável******************************
			iTextSharp.text.Image assinaturaResponsavel; 
			try
			{
				assinaturaResponsavel = iTextSharp.text.Image.GetInstance(caminhoAssinaturaResponsavel);
			}
			catch
			{
				lblMessage.Text+="Falta a assinatura do utilizador " +System.Web.HttpContext.Current.User.Identity.Name.ToString()  + ".<br />"; 		
				return false;
			}

            
			//*******************************************************	
			//assinatura do técnico**********************************	
			string assinaturaTecnicoLaboratorio = caminhoPastaAssinaturas + userNameTecnico +".gif";


			iTextSharp.text.Image assinaturaTecnico; 
			try
			{
				assinaturaTecnico = iTextSharp.text.Image.GetInstance(assinaturaTecnicoLaboratorio);
			}
			catch
			{
				lblMessage.Text+="Falta a assinatura do utilizador " +userNameTecnico  + " para validar o certificado "+ nomeDocumento + ".<br />"; 
				return false;

			}


			//*******************************************************	
			//posicionamento das assinaturas
			//*******************************************************	

			
			//*******************************************************	
			// para ficheiros "normais"
			if(sTipoServico != "VACV")  //codigo repetido, marteladas, qq dia, melhorar isto!
			{
			 
					if(tipoServicoEfectuado !="R")
					{//todos os que nao sao VACV, VAGE e VOPC e os que nao são serviços de reparação R 
                    //sairam: sTipoServico !="VAGE" && sTipoServico !="VOPC" && 
						assinaturaResponsavel.ScalePercent(18);
						assinaturaResponsavel.SetAbsolutePosition(360,60);;//360 px =9cm, 96px: 2,54cm

						assinaturaTecnico.ScalePercent(18);
						assinaturaTecnico.SetAbsolutePosition(120,60); 
					}
					else
					{
						assinaturaTecnico.ScalePercent(18);
						assinaturaTecnico.SetAbsolutePosition(360,60);;//360 px =9cm, 96px: 2,54cm

					}
			}
				//*******************************************************	
				//para VACV's
			else 
			{		
				assinaturaTecnico.ScalePercent(20); //aqui está com um scaling de 20, mas assim está bom por isso fica
				assinaturaTecnico.SetAbsolutePosition(270,105); 
				
				assinaturaResponsavel.ScalePercent(20); //aqui está com um scaling de 20, mas assim está bom por isso fica
				assinaturaResponsavel.SetAbsolutePosition(440,105);
			}

			//*******************************************************	
			//código ************************************************
			//*******************************************************	

			PdfReader pdfRdrRelatorio = new PdfReader(caminhoDocumentoEntrada); //o pdfreader é que de vez em quando
			//estoira... deve ser um bug, pq mesmo assim actualiza o ficheiro. 

			Document DocumentoFinal = new Document(PageSize.A4);

			FileStream fileST = new FileStream(caminhoDocumentoFinal, FileMode.Create); 
			PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);
			
			DocumentoFinal.Open();

			PdfImportedPage pageRelatorio;
			PdfContentByte cb = writer.DirectContent;

			int j = 0;
			int nPages = pdfRdrRelatorio.NumberOfPages;
			//*******************************************************
			//Assinar páginas
			while (j < nPages)
			{
				j++; //incrementa logo o contador
	
				DocumentoFinal.SetPageSize(PageSize.A4);
				DocumentoFinal.NewPage();
				pageRelatorio = writer.GetImportedPage(pdfRdrRelatorio, j);	
				
				//adicionar template e assinaturas
				cb.AddTemplate(pageRelatorio,0,0);

				//mas uma martelada, se é vacv e pagina nao é 1 e nao é 3, entao nao se insere as assinaturas
				if(sTipoServico != "VACV")
				{

					//MAIS EXCEPCOES....
					// se cacv e dosimetro som e pagina ultima ou a penultima...
					//ASSINATURA EM TODAS MENOS A ULTIMA NA PENULTIMA DO DO RESPONSAVEL
					if((sTipoServico =="CACV" && sEquipamento =="DOSÍMETRO SOM" && (idSubtipoServico== "" || idSubtipoServico=="32" )) && ((j==nPages) || (j==nPages-1)))
					{
						//na ultima nao faz nada logo nao ha codigo

						//nao penultima escrecve a assinatura do superior.
						if(j== nPages-1)
						{
							cb.AddImage(assinaturaResponsavel);
							
							cb.MoveTo(360,73) ;
							cb.LineTo(460,73);
							cb.Stroke();
						}
					}
					else //todos os que nao sao VACV
					{	
						//assinaturas e linhas
					
						if(tipoServicoEfectuado !="R")
							//todos os que nao sao VACV, VAGE e VOPC e não são reparação-- sairam: sTipoServico !="VAGE" && sTipoServico !="VOPC" &&  
						    //os outros levam todos as 2 assinaturas nos mesmos sitios
						{
							cb.AddImage(assinaturaTecnico);
							cb.AddImage(assinaturaResponsavel);

							cb.MoveTo(120,73) ;
							cb.LineTo(220,73);
							
							cb.MoveTo(360,73) ;
							cb.LineTo(460,73);
						}
						else //so assinatura tecnico no local do responsavel (VAGE + VOPC) //esses 2 n levam a assinatura do responsavel
						{
							cb.AddImage(assinaturaTecnico);
							cb.MoveTo(360,73) ;
							cb.LineTo(460,73);
						
						}
						//lineTo(float x, float y) 
						//Appends a	straight line segment from the current point (x, y). 		

						cb.Stroke();
						//stroke()= Strokes the path.
					}
				}
				else //vacv teem outro posicionamento - estes sao os VACV
				{
					
					if(j==1 || j==3) //adicionar estas coisas so na pagina 1 e 3
					{

						//n se pode cruzar imagens com linhas, por isso imagens teem de vir primeiro
						if(j==1)cb.AddImage(assinaturaTecnico); //so na 1a ass do tecn
						cb.AddImage(assinaturaResponsavel);
						
						if(j==1) //so na primeira
						{
							cb.MoveTo(285,127) ;
							cb.LineTo(385,127);
						}

						//igual para as duas: ass do superior.
						cb.MoveTo(440,127) ;
						cb.LineTo(540,127);

						cb.Stroke();
					}
				}			
			
				// we tell the ContentByte we're ready to draw text
				cb.BeginText();
				
				//nome tecncico
				BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				cb.SetFontAndSize(bf, 10);
				
				if(sTipoServico != "VACV") //todos menos os VACV
				{
                    if ((sTipoServico == "CACV" && sEquipamento == "DOSÍMETRO SOM" && idSubtipoServico == "32") && ((j == nPages) || (j == nPages - 1)))
					//if(sTipoServico =="CACV" && sEquipamento =="DOSÍMETRO SOM" && ((j==nPages) || (j==nPages-1)))
					{
						//na ultima nao faz nada logo nao ha codigo

						//nao penultima escrecve a assinatura do superior.
						if(j== nPages-1)
						{
							
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strValidado,410,116.5f,0);
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, nomeRespTec ,410,63,0); 
						}
					}
					else //todos os normais (menos os VACVS)
					{	
						if(tipoServicoEfectuado !="R")  //todos os que nao sao VACV, VAGE e VOPC e reparções
							//sairam:sTipoServico !="VAGE" && sTipoServico !="VOPC" && 		
							//os outros levam todos as 2 assinaturas nos mesmos sitios
						{

							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strServico,170,116.5f,0);
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, nomeTec ,170,63,0); 

							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strValidado,410,116.5f,0);
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, nomeRespTec ,410,63,0); 
						}
						else //os vage e os vopc levam a assinatura do técnico no local do responsável (com as coordenadas do responsavel)
						{
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strServico,410,116.5f,0);
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, nomeTec ,410,63,0); 
						}
					}
				}
				else  // os VACV....
				{
					//VACV
					if(j==1 || j==3)
					{
						if(j==1) //so na primeira
						{
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strServico,330,157,0);
							cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, nomeTec ,330,112,0);
						}
						cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, strValidado,490,157,0);
						cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, nomeRespTec ,490,112,0); 
						
					}
				}
				cb.EndText();
				
			}

			pdfRdrRelatorio.Close();
			DocumentoFinal.Close(); //nao vai para o finally pq supostamente se estoira, estoira ANTES disto

			
			return true; 

		}

		#region inserirAssinaturaCertificado

		//==============================================================================
		// Assina digitalmente o certificado
		// recebe como parâmetros o caminhoDocumentoEntrada a ser assinado
		// e o caminho do caminhoDocumentoEntrada final
		//==============================================================================
		private void assinarCertificadoDigitalmente(string caminhoDocumentoEntrada, string caminhoDocumentoFinal)
		{
			//string base64 = Labmetro.MultiCertSign.DigitalSignMulticert(caminhoDocumentoEntrada, Path.GetFileName(caminhoDocumentoEntrada)).Result;
			//if ((base64 ?? "").Length > 0 && !(base64 ?? "").Contains("ERRO"))
			//{
			//	if (File.Exists(caminhoDocumentoFinal)) File.Delete(caminhoDocumentoFinal);
			//	Labmetro.MultiCertSign.Base64ToPdf(base64, caminhoDocumentoFinal);
			//	if (File.Exists(caminhoDocumentoFinal)) File.Delete(caminhoDocumentoEntrada);
			//	return true;
			//}
			//else
			//         {
			//	Response.Write("<script>alert('NÃO FOI POSSÍVEL ASSINAR CERTIFICADO!\nCONFIRME SE TEM ACESSO AO SERVIÇO DE ASSINATURA DIGITAL!')</script>");
			//	return false;
			//}

			string alias = null;

			//First we'll read the certificate file
			Org.BouncyCastle.Pkcs.Pkcs12Store pk12 = new Pkcs12Store(new FileStream(certificado, FileMode.Open, FileAccess.Read), password.ToCharArray());

			//IEnumerator i = pk12.Aliases(); //AQUI TINHA ()
			IEnumerator i = pk12.Aliases.GetEnumerator();
			while (i.MoveNext())
			{
				alias = ((string)i.Current);
				if (pk12.IsKeyEntry(alias))
					break;
			}
			AsymmetricKeyParameter akp = pk12.GetKey(alias).Key; //.GetKey();
			X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
			Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[ce.Length];
			for (int k = 0; k < ce.Length; ++k)
			{
				chain[k] = ce[k].Certificate;//.GetCertificate();
			}

			PdfReader reader = new PdfReader(caminhoDocumentoEntrada);//o pdfreader é que de vez em quando
																		//estoira... deve ser um bug, pq mesmo assim actualiza o ficheiro. 

			PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(caminhoDocumentoFinal, FileMode.Create, FileAccess.Write), '\0');
			PdfSignatureAppearance sap = st.SignatureAppearance;

			sap.SetCrypto(akp, chain, null, PdfSignatureAppearance.WINCER_SIGNED);//quando removi isto é que me apereceu válido...wincer: windows certificate
			sap.Reason = cert_reason;

			sap.SetVisibleSignature(new iTextSharp.text.Rectangle(150, 710, 250, 810), 1, null);

			//dm08-01-2008
			reader.Close(); //ver se é isto que falta para isto não estoirar. em muitos loops, o reader nunca fecha... pode ser isso... 
			st.Close();
		}
		#endregion

		//==============================================================================
		// Copia os documentos para a pasta de caminhoPastaCertificados final
		// Após ter incluído a rúbrica dos técnicos e 
		// assinado digitalmente o documento
		//==============================================================================
		
        private bool assinarEmoverDocumentos(string idServico, string nomeDocumentoIn, string userNameTecLab,string nomeTecLab, string refServico, string sTipoServico, string sEquipamento, string sRefServicoPai,string nomeDocumentoOut,string idSubtipoServico, string refServicoCertificado, string sigla, string linguaCertificado) 
		{
			DirectoryInfo dirInfoConstrucao = new DirectoryInfo(caminhoPastaFinaisConstrucao);
			FileInfo[] fileInfoConstrucao = dirInfoConstrucao.GetFiles("*" + nomeDocumentoIn + "*");

			if (fileInfoConstrucao.Length > 0)
			{
				string caminhoDocumento = caminhoPastaFinaisConstrucao + "\\" + fileInfoConstrucao[0].ToString();
				string caminhoDocumentoTemp = caminhoPastaTemporaria + "\\" + nomeDocumentoOut + ".pdf";
				string caminhoDocumentoFinal = caminhoPastaCertificados + "\\" + nomeDocumentoOut + ".pdf";
				try
				{
					if (inserirAssinaturas(caminhoDocumento, caminhoDocumentoTemp, userNameTecLab, nomeDocumentoIn, nomeTecLab, refServico, sTipoServico, sEquipamento, sRefServicoPai, idSubtipoServico, linguaCertificado) == true)
					{
						try
						{
							assinarCertificadoDigitalmente(caminhoDocumentoTemp, caminhoDocumentoFinal);
							if (File.Exists(fileInfoConstrucao[0].FullName)) fileInfoConstrucao[0].Delete();
							return true;
						}
						catch (Exception ex)
						{
							lblMessage.Text += ex.ToString();
							return false;
						}
					}
					else
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					Response.Write(ex.ToString());
					return false;
				}
			}
			else
			{
				return false;
			}
		}


		private void deleteTempFiles() //nao sei..... se calhar usa isto para ir buscar os ficheiros de volta caso 
			// tenha havia algum problema, mas isso perdeu-se algures....
		{
			//Apagar ficheiros temporários
			DirectoryInfo dirInfoTemporary = new DirectoryInfo(caminhoPastaTemporaria);
			FileInfo[] fileInfoTemporary = dirInfoTemporary.GetFiles();
			for(int i=0;i<fileInfoTemporary.Length;i++)
			{
				if(fileInfoTemporary[i].Exists) //pode já ter sido apagado entretanto por outro user
				{
					fileInfoTemporary[i].Delete();
				}
			}
		}
		#region FUNÇÕES AUXILIARES

		//*******************************************************************************************
		//*******************************************************************************************
		//****************************FUNÇÕES AUXILIARES ********************************************
		//*******************************************************************************************
		//*******************************************************************************************


		//==============================================================================
		//recebe nome de ficheiro e extrai a sigla (1C, 1R ...)
		//==============================================================================
		public string strExtraiSigla(string nomeDocumento)
		{
			try
			{
				string delimStr = "-";
				char[] delimiter = delimStr.ToCharArray();
				string[] campos = nomeDocumento.Split(delimiter);

				delimStr = ".";
				delimiter = delimStr.ToCharArray();
				string[] TC = campos[campos.Length - 1].Split(delimiter);
				string sigla = TC[0];
			
				return sigla; 
			}
			catch
			{
				return ""; 
			}

		}
		//===================================================================================
		//===================================================================================
		public void visualisarDocumento(object sender, DataGridCommandEventArgs e)
		{
			if(e.CommandName.ToString() == "Select")
			{
				string doc = e.Item.Cells[4].Text;
				string nome = downloadpath(doc);
				//Response.Write("<script language=javascript>window.open('"+nome+"','new_Win','toolbar=0,menubar=0,resizable=1');</script>");

                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language=JavaScript>");
                strScript.Append("window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');");
                strScript.Append("</script>");
                //RegisterClientScriptBlock("imprimefactura", strScript.ToString());
                ClientScript.RegisterClientScriptBlock(GetType(), "mostraCertif", strScript.ToString());
            
			}
		}
	
		//===================================================================================
		//===================================================================================
		private void setupClientScript()
		{
			string js = @"
			<script language=JavaScript> 
			function ConfirmarEscolha(btnWaiter) 
			{
				if (confirm('Tem a certeza dos registos seleccionados?'))
				{
					document.body.style.cursor=""wait"";
					return true;
				} 
				return false; 
			}
			</script>";					
			//btnWaiter.setAttribute(""value"", ""Please Wait..."");
			//Register the script
			if (!ClientScript.IsClientScriptBlockRegistered("ConfirmarEscolha"))
			{
				//RegisterClientScriptBlock("ConfirmarEscolha", js);
                ClientScript.RegisterClientScriptBlock(typeof(Page), "ConfirmarEscolha", js, true);

            }
        }
		

		//===================================================================================
		// Retorna o grau de hierarquia do utilizador
		//===================================================================================
		public string strGrau()
		{
			DATA.EstadoCertificadoBD gr = new LabMetro.DATA.EstadoCertificadoBD(); 
			string grau = gr.strGrauUtilizador(); 
			gr = null; 
				
			return grau; 
		}

		//===================================================================================
		//===================================================================================
		public string downloadpath(object filename)
		{
			if(filename!=null && filename.ToString()!="")
			{
				string myPath = (string)ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CONSTRUCAO"];
				myPath = myPath + "/" + filename.ToString();
				return myPath;
			}
			else
			{
				return "#"; 
			}
		}	
		
		#endregion

	
		//===================================================================================
		//vai buscar o NOME de quem valida (quem está logado à aplicação, e guarda em viewstate,
		//para não ter de ir repetidamente à BD.
		//===================================================================================
		
		private string strNomeResponsavel()
		{
			
			string strSQL = "SELECT dbo.udfGetNomeUtilizadorByUserName('" +System.Web.HttpContext.Current.User.Identity.Name.ToString()+"')"; 
			return (string)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
							
		}

        protected void btnDeselectAll_Click1(object sender, EventArgs e)
        {
           //martelada
           //supostamente nao se devia rejeitar nada que nao devesse ser substituido mais tarde - requisito inicial
           //agora ha certificados rejeitados que ja nao sao para fazer, devia se eliminar o ficheiro da pasta construção e voltar a colocar o estado no estado anterior.
           //se fosse uma revisao, o estadoCertificado passaria a 4 e se fosse um primeiro certificado,entao para 1
           //agora vou ignorar aqui os serviços pai....


        }
    }

}