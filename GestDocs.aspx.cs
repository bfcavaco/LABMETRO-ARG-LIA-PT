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
using System.Configuration;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LabMetro.REPORTS;
using LabMetro.GERAL;
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
using System.Text;


// nota: dm, junho 2009: vou retirar as permissoes para o user labmetrousers poder read files na pasta origem!


namespace LabMetro
{
	/// <summary>
	/// Summary description for GestDocumentos.
	/// Página para a primeira fase de validação dos certificados.
	/// Os documentos deverão ser validados pelos técnicos que executaram os ensaios.
	/// </summary>
	public partial class GestDocs : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btnHelp;
		protected System.Web.UI.WebControls.Label lblErro;

		private DataTable DT; 
		private DataView DV;
        DataTable dt;

		//NOME DA PAGINA
		private const string ID_PAG = "CERTDOCUMENTOS_1";	
		private  string MSG_PDF_INEXISTENTE = "Criar PDF urgente.";
		private  string MSG_PDF_CRIARNOVO = "Falta criar novo PDF.";
        private string  MSG_PDF_CORROMPIDO = "Pdf corrompido. Criar novo."; 
		
		//PASTAS
		private static string pastaTemplates = (string)ConfigurationManager.AppSettings["PASTA_TEMPLATES"];
		private static string pastaSimbolos = (string)ConfigurationManager.AppSettings["PASTA_SIMBOLOS"];
		private static string pastaCertificadosOriginais = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGINAIS"];
		private static string pastaCertificadosOriginaisBackup = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGINAIS_BACKUP"];
		private static string pastaCertificadosFinaisConstrucao = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CONSTRUCAO"];
		private string caminhoPastaCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];

		//TEMPLATES
		private static string templatePagina = (string)ConfigurationManager.AppSettings["TEMPLATE_PAGINA"];
		private static string templatePaginaIPAC = (string)ConfigurationManager.AppSettings["TEMPLATE_PAGINA_IPAC"];
		private static string templatePaginaREP = (string)ConfigurationManager.AppSettings["TEMPLATE_PAGINA_REP"];

		private static string caminhoTemplate = pastaTemplates + "\\" + templatePagina;
		private static string caminhoTemplateIPAC = pastaTemplates + "\\" + templatePaginaIPAC;
		private static string caminhoTemplateREP = pastaTemplates + "\\" + templatePaginaREP;
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			InitializeComponent2();
			base.OnInit(e);
		}
		
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
			//btnUpload.Click += new System.EventHandler(btnUpload_Click);
			cbTodos.CheckedChanged += new System.EventHandler(cbTodos_CheckedChanged);
            cbCertifsUser.CheckedChanged += new System.EventHandler(cbCertifsUser_CheckedChanged);
			dgDocumentos.ItemDataBound += new DataGridItemEventHandler(dgDocumentos_ItemDataBound); 
		}
		#endregion
		

		//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//coisas a fazer futuramente neste ficheiro:
		//é possivel carregar por exemplo um ficheiro 1R e dps, ainda antes de aprovar, colocar um ficheiro 1C
		//o ficheiro 1R fica retido para sempre... 
		//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		
		protected void Page_Load(object sender, System.EventArgs e)
		{

            Response.Expires = 0; 	//nao tirar!!!

			lblMessage.Text = "";
			
			Hashtable ht = (Hashtable)Session["HTPermissions"];
			if (ht == null)
			{
				Server.Transfer("Default.aspx?err=2", false);
			}
			else
			{
				if (!ht.ContainsKey(ID_PAG))
				{
					Server.Transfer("Default.aspx?err=1", false);
				}
				else
				{
					if (!Page.IsPostBack)
					{
						//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
						//SE UM DOC JA EXISTE NA PASTA FINAIS CERTIFICADOS NAO SE PODE CARREGAR NENHUM  DOCUMENTO COM O MESMO NOME 
						//PRIMEIRA COISA A FAZER, ASSIM NEM SEQUER VAI BUSCAR ESSES SERVIÇOS!!!
						//o unico problema a rever aqui, mas isso nao ha nada a fazer, sao os aditamentos...
						//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
						apagaDocsJaExistentesOuErrados(); 

						setupClientScript();
						btnSubmit.Attributes.Add("onClick","return ConfirmarEscolha(this);");

						ViewState["sortField"] = "dtEstadoServico";
						ViewState["sortDirection"] = "DESC";

						getGrandezaUtilizadorLogado(); //guarda a grandeza do user logado em sessao. 
						
						cbTodos.Visible = false;
						if (strGrau() == "0")
						{
							cbTodos.Visible = true;	
						}
						
						fillDDGrandeza(); 

						//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
						//1. IR BUSCAR TODOS OS SERVIÇOS QUE CORRSPONDEM A CERTOS REQUISITOS
						//idEstadoCertificado=(1,2,5,6) n.existente, calibra.s/val,rejeit.tecn, rejeit.resp. 
                        //podem tb vir docs em estado 3    por causa dos OR's na query
						//idEstadoServico = (6, 7, 8, 9, 10,15,25,21) (calibrado, anulado, avariado, suspenso,sub.cal)	
						//carrega a datatable incial, nada mais. 
						//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

						createDTServicos();
                        

                        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
                        //2: MOVER OS DOCS COLOCADOS NA PASTA ORIGEM p/ A PASTA FINAIS CONSTRUÇÃO
                        //PRECISA DA DT CRIADA POR ISSO TEM DE VIR A SEGUIR à createDTServicos
                        //depois faz update à alguns etados de serviço, pelo q a DT nao continua actualizada
                        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
                        string err = MergePDFInicial();
						
						//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::	
						//apaga os ficheiros mal formados da pasta origem, que vem da msg de erro do mergefile
						//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
						if (err !="" && err!= MSG_PDF_CORROMPIDO)
						{
							apagaDocsIncorrectosPastaOrigem(err); 
						}
						
						//aqui devia entrar uma funcao que apaga os incorrecto da pasta origem e actualiza a DT; 
						//por exemplo para situações de um estado calibrado com validacao e colocacao de novo ficheiro por engano na origem, que deve ser apagado. 

						bVerificacaoPastaFinaisConst(); //ja nao devolvo true ou false, simiplesmente executo

						BindGrid();
						
						DT.AcceptChanges(); //já é chamado antes mas ju
						
						ViewState["DT"] = DT; 
						//GERAL.clsWriteError.WriteLog(ViewState.Values.ToString());
					}
					//					else//toda a acção do POSTBACK
					//					{
					//						//nao há nada aqui..... 
					//					}
				}
			}
		}
	

		//=================================================================================================
		//=================================================================================================
		//criacao da datasource ESTA FUNÇÃO SÓ É CHAMADA UMA UNICA VEZ, DA PRIMEIRA VEZ Q A PÁGINA É CARREGADA!!!!!!
		//vai buscar os serviços adiciona as outras colunas, mas ainda nao trata os documentos encontrados
		//=================================================================================================
		#region CREATEDATASOURCE
		private void createDTServicos()
		{
			
			//novembro 2008
			//pagina muito pesada

			//os perfis 6 podem ver todos os serviços por eles calibrados e as revisões das suas grandeszas
			//os perfis 4+5  podem ver todos os serviços das suas grandezas
			//o perfil 0 admin pode ver tudo. 

			string idPerfil = Session["idPerfil"].ToString();
			string userName = User.Identity.Name.ToString(); 
			string idUtilizador = Session["UserId"].ToString(); 

			if(!Page.IsPostBack)
			{
				//da primeira vez que entra ... (aqui entra só da primeira vez pq nunca mais vou chamar esta função)
				
				userName = ""; 
			}

			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
			
			DT = data.DTDocumentsForValidation(stringDocs()); //stringDocs = refServiço de todos os <> de 1C na pasta ORIGEM															
			//adiciona linhas à DataTable
			DT.Columns.Add(new DataColumn("cbAprovar", typeof(bool)));
			DT.Columns.Add(new DataColumn("cbRejeitar", typeof(bool)));
			DT.Columns.Add(new DataColumn("cbTemplate", typeof(bool))); //isto estoira em testes, vou dar-lhe um default value DM
			DT.Columns.Add(new DataColumn("nomeFichCalibracao", typeof(string))); 
			DT.Columns.Add(new DataColumn("belongsToUser", typeof(bool))); //diz se doc pertence ao user 
          
			Hashtable HTDescCertByID = HTDescCertificadoIdTipo();  	//contem idTipo(string) /tipoCertificado(descricao)
			
			//preenhce os valores dos campos acima criados, nomeadamente se o serviço pertence ao user logado, se o equip. é acreditado
			foreach (DataRow dRow in DT.Rows)
			{

				dRow["cbTemplate"] = dRow["acreditado"]; //novo out 2008 //nao consigo muito bem reconstruir toda a logica desta pagina
				if(Convert.IsDBNull(dRow["cbTemplate"])) dRow["cbTemplate"] = false; //enfim...analisar. no join das revisoes os pais vêm a null
				if(dRow["refServico"].ToString().IndexOf("R",0,1) > -1) //reparacao
				{
					dRow["simbolo"] = null; 
					dRow["cbTemplate"] = false; //SEMPRE
				}
				dRow["cbAprovar"] = false;  //inicializado a false
				dRow["cbRejeitar"] = false; //inicializado a false
				
				//estes 2 conceitos foram criados pelo rui e é um bocado complicado de resolver todas as implicacoes disto....
				//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::		
				dRow["nomeFichCalibracao"] = dRow["nomeDocumento"]; //inicializa com o nome do documento (menos extensao); 
				//serve para poder apagar os que estão a mais mais adiante
				//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::

				if(dRow["idEstadoCertificado"].ToString() =="1") //idestadoCertificado = 1 - ficheiro nao carregado. a nao ser que seja alterado dps
				{
					dRow["nomeFichCalibracao"] = MSG_PDF_INEXISTENTE; //indicacao que falta criar pdf
				}

				dRow["tipoCertificado"] = ""; 
				dRow["belongsToUser"] = false;

				if (dRow["username"].ToString().ToUpper() == User.Identity.Name.ToString().ToUpper())//username que EFECTUOU O SERVIÇO
				{
					dRow["belongsToUser"] = true;
				}

				if(dRow["idEstadoCertificado"].ToString()=="5"||dRow["idEstadoCertificado"].ToString() =="6") //REJEITADOS
				{
					
					dRow["nomeFichCalibracao"] = MSG_PDF_CRIARNOVO; //indicacao que falta criar NOVO pdf

					string idTipoCertificado =  dRow["idTipoCertificadoEmValidacao"].ToString(); //substituir o campo tipo certificado pelo tipocertificado em validacao		

					if(idTipoCertificado !="")
					{
						dRow["idTipoCertificado"] = idTipoCertificado;
						object y = HTDescCertByID[idTipoCertificado];
						string descTipoCert = System.Convert.ToString(y); 
						dRow["tipoCertificado"] = descTipoCert;
					}

					//substituir o campo userefectuouServiço pelo user que validou o serviço em último
					if(idTipoCertificado != "1") //isto agora so é valido para tudo menos as primeiras calibracoes!!!dm 28-08-2007	
					{
						dRow["username"] = dRow["userNameValidou"]; 
						dRow["tecnicoLaboratorio"] = dRow["tecnicoLaboratorioValidou"]; 
					}
				}
			}

			//HTAcreditado = null;
			HTDescCertByID = null;
			data = null;

			if(Page.IsPostBack) ViewState["DT"] = DT;
            //necessário para depois do update, quando quero recarregar o datagrid, 
           
		}
		
		#endregion
		
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//VAI LER OS DOCS DA PASTA ORIGINAIS E PASSA OS DOCUMENTOS CORRECTOS PARA A PASTA FINAIS, 
		//COM TODOS OS TEMPLATES CORRECTOS e ACTUALIZA O ESTADO PARA 2 (CALIBRADO SEM VALIDACAO)
		//USA A	DATATABLE ANTES DO POSTBACK 

		//ENCAMINHA PARA AS FUNCOES DE MERGE
		//DEVOLVE UMA STRING QUE É UMA MENSAGEM DE ERRO
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		#region MERGEPDFINICIAL

		private string MergePDFInicial()
		{
			string errMessage=""; 

			DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOriginais);
			DirectoryInfo dirInfoPastaBackupOrigem = new DirectoryInfo(pastaCertificadosOriginaisBackup);
			DirectoryInfo dirInfopastaCertificadosConstrucao = new DirectoryInfo(pastaCertificadosFinaisConstrucao);

			DV = new DataView(DT); //DT ESTÁ CARREGADA EM MEMÓRIA, AINDA NÃO HOUVE POSTBACK. 

			string idServico=""; 

			foreach (DataRowView drv in DV)//AQUI POSSO RECEBER MUITA COISA...E POSSO RECEBER UM IDSERVICO VARIAS VEZES
				//CONFORME EXISTEM TIPOS DE CERTIFICADO ASSOCIADOS AO SERVIÇO

				//E POSSO VALIDAR CADA FICHEIRO DEMASIADAS VEZES...
				//por isso tenho de pensar numa maneira de comparar cada idServico com 
				//todos os seus certificados associados apenas uma vez
			{
				
				if(idServico==drv.Row["idServico"].ToString())//já e a segunda vez que entra aqui 
				{
					idServico = drv.Row["idServico"].ToString(); //alterar o "contador" //tem de vir depois do IF
					//sair pq o ficheiro já foi avaliado contra todas as outras linhas da DT COM O MESMO IDSERVICO
				}
				else
				{
					idServico = drv.Row["idServico"].ToString(); 

					if(drv.Row["nomeDocumento"].ToString() !="")  // A NAO CONFUNDIR COM nomeFichCalibracao. jan 2019 ESTÀ SEMPRE DIFERENTE DE vazio
					{
						
						FileInfo[] fileInfosPastaOrigem = dirInfoPastaOrigem.GetFiles("*" + drv.Row["nomeDocumento"].ToString() + "*");
					
						for (int i = 0; i < fileInfosPastaOrigem.GetLength(0); i++) //pode encontrar varios ficheiros, i guess...
						{
							if (!isValidFileName(fileInfosPastaOrigem[i].ToString()))//Validação da nomenclatura do ficheiro encontrado
							{
								
								
								try
								{
									fileInfosPastaOrigem[i].Delete(); //apaga da pasta de origem!				
								}
								catch{}
							}
							if (drv.Row["idEstadoCertificado"].ToString() =="3")//já tem certificado nao posso carregar outro, mas tarde vou ter de apagar a datarow
							{
								try
								{
									fileInfosPastaOrigem[i].Delete(); 		
								}
								catch{}
							}
							
								//agora aqui, se o documento já foi criado nao pode ser criado mais vez nenhuma
								//a não ser que esteja no estado 5 ou 6 e para o tipo de certificado 1C tb no estado 1 e isto pode ser validado logo antes de mandar os ficheiros para o outro lado. 

							else
							{
								string strSimboloAcreditacao = drv.Row["simbolo"].ToString().TrimEnd();	
								string caminhoDocumentoOrigem = pastaCertificadosOriginais + "\\" + fileInfosPastaOrigem[i].ToString().ToUpper();
								string caminhoDocumentoDestino = pastaCertificadosFinaisConstrucao + "\\" + fileInfosPastaOrigem[i].ToString().ToUpper();
								string caminhoDocumentoBackup = pastaCertificadosOriginaisBackup + "\\" + fileInfosPastaOrigem[i].ToString().ToUpper();

                               //martelada de ultima hora (março 2010!!!!), nao ha neste momento tempo para fazer isto com BO etc
                                //basicamente, o que se pretende é que conforme os simboloso de acredtiacao, apareça
                                //um nome de laboratorio diferente no cabeçalo do documento. nao existe bo para isso
                                //e nao vai existir tao depressa, dada a urgencia, vai ser hardcoded. 
                                 /** Arial Normal Font of size 11. */
                                iTextSharp.text.Font ARIAL_NORMAL_FONT = FontFactory.GetFont("Arial", 11);
                                
                                iTextSharp.text.Chunk nomeLaboratorioHeader = new Chunk();
                                nomeLaboratorioHeader.Font = ARIAL_NORMAL_FONT;

                               ////////////////////////////////////////////////////////////////////////////////////////////////////////////
     
                                ///>>>>>> FAZER ALTERAÇão PEDRO GOMES
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                //pensar numa maneira quando a grandeza é RAD escrever nomeLaboratório " Unidade Técnica de Protecção Radiológica";
                                //Na folha de fundo nao acreditada
                                switch (strSimboloAcreditacao)
                                {
                                    case "M0046":
                                        nomeLaboratorioHeader.Append("Laboratório de Calibração em Metrologia Física");
                                        break;
                                    case "L0268":
                                        nomeLaboratorioHeader.Append("Laboratório de Ensaios Físicos");
                                        break;
                                    case "M0059":
                                        nomeLaboratorioHeader.Append("Laboratório de Calibração em Metrologia Electro-Física");
                                        break;
                                    case "L0342":
                                        nomeLaboratorioHeader.Append("Laboratório de Ensaios de Controlo Dimensional");
                                        break;
                                    case "M0009":
                                        nomeLaboratorioHeader.Append("Laboratório de Metrologia Dimensional");
                                        break;
                                    case "L0610":
                                        nomeLaboratorioHeader.Append("Labmetro Saúde \n");
                                        nomeLaboratorioHeader.Append("Laboratório de Metrologia Equipamento Clínico Hospitalar");
                                        break;
                                    default:
                                        //nomeLaboratorioHeader.Append("Laboratório de Metrologia"); 
                                        nomeLaboratorioHeader.Append("");
                                        break;
                                }

								//nova excecpa
			//					vacv
   //so nas 2 primeiras paginas o simbolo e o fundo acreditado. -- atenção que simbolo vem hardoded da uery


								//NOVO - PROGRAMAÇÃO DE UMA EXCEPÇÃO.
								//SE O TIPO DE SERVIÇO FOR UM CACV E O TIPO DE EQUIPAMENTO UM DOSIMETRO, 
								//AS ULTIMAS DUAS PAGINAS LEVAM O TEMPLATE NORMAL, SEM SIMBOLO NEM LINHA DE LADO. 

								//muita atençºão que em testes o subtipo é diferente!
								string caminhoSimboloAcreditacao = pastaSimbolos + "\\" + strSimboloAcreditacao+".gif";
								
								if(drv["refServico"].ToString().IndexOf("R",0,1) > -1) //se é uma reparação, vai buscar a template da reparação. 
								{
                                    errMessage += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplateREP, null, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
								}
								else	//se não é uma reparação
								{
                                    //so faz isso para dosímetros com subtipo == NULL (confirmar)
                                    //se for um dosimetro com subtipo segue o fluxo normal dos certificados todos.
									if((drv["refServico"].ToString().IndexOf("CACV",0) > -1) && (drv["equipamento"].ToString()=="DOSÍMETRO SOM") && ((Convert.IsDBNull(drv["idSubtipoServico"]) || drv["idSubtipoServico"].ToString() =="32" )) )		
									{
                                        errMessage += mergeFilesDosimetro(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoSimboloAcreditacao, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
									}

									//else if (drv["refServico"].ToString().IndexOf("VMLF", 0) > -1) //novo -- sempre acreditado
									//{
									//    errMessage += mergeFilesVMLF(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoSimboloAcreditacao, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
									//}

									// out 2020: VACV 
									// so nas 2 primeiras paginas o simbolo e o fundo acreditado.
									//ATENÇÃO QUE VACV TRAZ UM SIMBOLO DIFERNTE HARDCODED NA QUERY QUE VAI BUSCAR OS SERVIÇOS
									//talvez na query vá pelo tipo de equipamento, que é o que foi pedido, pq nao ha outroa tipos em vacv....


									else if (drv["refServico"].ToString().IndexOf("VACV", 0) > -1) 
									{
									    errMessage += mergeFilesVACV(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoSimboloAcreditacao, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
									}



										//Novembro 2011
										//novo ECH pode ser nao acreditado ou acreditado
										//se acreditado, nao leva o simbolo nem a template acreditada na ultima pagina




									else if (drv["refServico"].ToString().IndexOf("ECH", 1) > -1) //PODE SER ACREDITADO OU NAO
                                        //MAS SE É ACREDITADO, NAO LEVA NEM SIMBOLO NEM TEMPLATE NA ULTIMA PAGINA
                                    {

                                        if (strSimboloAcreditacao != "")
                                        {
                                            errMessage += mergeFilesECH(caminhoDocumentoDestino, caminhoDocumentoOrigem,  caminhoSimboloAcreditacao, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
                                        }

                                        else  //manda um null no simbolo e vai buscar template sem IPAC
                                        {
                                            errMessage += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplate, null, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
                                        }
                                    }

                                    else
                                    {
                                        //se existe simbolo de acreditacao, junta o simbolo+vai escolher template do IPAC
                                        if (strSimboloAcreditacao != "")
                                        {
                                            errMessage += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplateIPAC, caminhoSimboloAcreditacao, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
                                        }

                                        else  //manda um null no simbolo e vai buscar template sem IPAC
                                        {
                                            errMessage += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplate, null, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
                                        }
                                    }
								}
                                if (errMessage.IndexOf(MSG_PDF_CORROMPIDO) >-1)//Janeiro 2009
                                {
                                    drv["nomeFichCalibracao"] = MSG_PDF_CORROMPIDO;
                                    drv["idEstadoCertificado"] = "1"; //é como se fosse não existente, e fica disabled no loop que faz enable/disable das checkboxes
                                        
                                        //no caso de a mensagem conter(e nao percebo ainda pq contem e não é igual à... mas agora funciona e fica assim, portante se contem a msg pdf corrompido, essa mensagem +e mostrada no campo correspondente para o ficheiro ser substituido. 
                                }
							}
						}
					}
				}
			}

            errMessage = errMessage.Replace(MSG_PDF_CORROMPIDO, "");
			lblMessage.Text+=errMessage; //.... tenho de remover daqui agora a msg do pdfCorrompido.

			return errMessage;
						
		}
		#endregion

		//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//muito simples: SE UM DOC JA EXISTE NA PASTA FINAIS CERTIFICADOS NAO SE PODE CARREGAR 
		//NENHUM OUTRO DOCUMENTO COM O MESMO NOME. -- nao da
        //se um doc já existe na tabela certificados (onde NAO é gravado pelo seu nome) o doc é apagado.

		//tb apaga todos os outros docs com nomes incorrectos - os que consegue apanhar.
		//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		private void apagaDocsJaExistentesOuErrados() 
		{
            //grande martelada  - junho 2010
			
			string sErr="";

            //DirectoryInfo dirInfoPastaCertificados = new DirectoryInfo(caminhoPastaCertificados);
			DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOriginais);
			FileInfo[] filesOrigem = dirInfoPastaOrigem.GetFiles();
			
			if (filesOrigem.Length > 0)
			{
				for (int i = 0; i < filesOrigem.Length; i++)
				{
                    if (!isValidFileName(filesOrigem[i].ToString()))
                    {
                        try
                        {
                            filesOrigem[i].Delete(); //apaga da pasta de origem!.
                        }
                        catch (Exception e)
                        {
                            GERAL.clsWriteError.WriteLog("linha 507" + e.ToString());

                            if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
                            {
                                sErr += filesOrigem[i].ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
                            }

                        }
                        continue;
                    }
                    

                    string refServico = filesOrigem[i].ToString().Replace("-", "/");
                    refServico = refServico.Substring(0, refServico.Length - 7);

                    string[] documento = filesOrigem[i].ToString().Split("-,.".ToCharArray());
                    string strSQL ="";

                    //DataTable dt;

                    if (documento.Length != 4) //tem um nome mal formado
                    {
                        try
                        {
                            filesOrigem[i].Delete(); //apaga da pasta de origem!.[
                        }
                        catch
                        {
                            
                        }
                    }
                    else
                    {
                        string sigla = documento[2].ToUpper();
                        //os aditamentos teem de ser tratados a parte
                        switch (sigla)
                        {
                            case "1C":
                                strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 1";
                                break;
                            case "1R":
                                strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 3";
                                break;
                            case "2R":
                                strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 7";
                                break;
                            case "3R":
                                strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 8";
                                break;
                            case "4R":
                                strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 11";
                                break;
                        }
                        if (strSQL != "")
                        {
                            dt = GERAL.clsDataAccess.ExecuteDT(strSQL);
                        }
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                                try
                                {
                                    filesOrigem[i].Delete(); //apaga da pasta de origem!.[
                                }
                                catch
                                {
                                }
                        }
                    }
			        
				}
			}

            dirInfoPastaOrigem = null;   
			//dirInfoPastaCertificados = null;
			//dirInfoPastaBackupOrigem = null;
			lblMessage.Text +=sErr;
		}

		//martelada de ultima hora!!! validacao dos ficheiros erra
		//recebe uma string com ficheiros onde foi encontrado algum erro, nome ficheiro + mensagem
		//tudo partia do principio que users podiam apagar ficheiros da pasta, mas afinal ja nao podem. 

		private void apagaDocsIncorrectosPastaOrigem(string err) //dps vai comparar esta string ERR cm od ficheiros encontrados
		{
			string sErr=""; //=erro. n vou mostar aqui o erro, depois poderá ser redundante... esperar
			DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOriginais);
			FileInfo[] filesExistentes = dirInfoPastaOrigem.GetFiles();
					
			if (filesExistentes.Length > 0)
			{
				for (int i = 0; i < filesExistentes.Length; i++)
				{
					if(err.IndexOf(filesExistentes[i].ToString())==-1)
						
						//se o ficheiro nao corresponde a algum dos ficheiros que vem na mensagem de erro.
						//NAO POSSO APGAR TODOS PORQUE PODEM TER SIDO GRAVADOS ANTES DO ESTADO TER SIDO MUDADO.
					{
						//martelada pois introduzi uma nova validacao: se o erro for causado por um ficheiro aberto,
						//nao faz mais nada, portanto, se recebe esta mensagem junto com o ficheiro, nao escreve nenhuma
						//outra mensagem de erro adicional 
						if(err.IndexOf(filesExistentes[i]+" - Alguém tem este ficheiro em pdf aberto na Pasta Certificados" )==-1)
						{	
							if(filesExistentes[i].ToString().ToUpper().EndsWith(".PDF") == false) 
							{
								filesExistentes[i].Delete(); 
							}
						}
					}
				}
			}
			
			dirInfoPastaOrigem = null;   
		
			lblMessage.Text +=sErr;
		}

		
				
		//FileMode: Specifies how the operating system should open a file.
		//Usados aqui: (existem mais)

		//Create: Specifies that the operating system should create a new file. 
		//If the file already exists, it will be overwritten. 

		//CreateNew: Specifies that the operating system should create a new file. 
		//This requires FileIOPermissionAccess.Write. 
		//If the file already exists, an IOException is thrown. 

		#region MERGEFILES
		//==================================================================================
		//faz merge do ficheiro recebido com os templates e o simbolo e grava na pasta de destino
		//neste caso, certificados construcao
		//pelo meio ainda actualizado o estado do certificado.... para 2 (calibrado sem validacao)
		//==================================================================================


		/// <summary>
		/// INICIO MARTELADA dosimetros
		/// USA AS DUAS TEMPLATES, 
        /// EM TODAS AS PAGNAS MENOS AS ULTIMA DUAS com SIMBOLO E TEMPLATE ACREDITADOS
		/// NAS DUAS ULTIMAS SEM SIMBOLO E TEMPLATE NAO ACREDITADO
		/// copiado do mergefiles, mas foi removido o template dos parametros recebidos, pois aqui usa os dois.
		/// </summary>
        private string mergeFilesDosimetro(string caminhoDocumentoDestino, string caminhoDocumentoOrigem, string caminhoSimboloAcreditacao, string caminhoDocumentoBackup, FileInfo fileOriginal, bool bAlteracaoTemplate, Chunk nomeLaboratorioHeader)
		{

			//ultimas 2 folhas nao levam simbolo e levam template nao acreditado
			//as outras levam simbolo e template IPAQ
			string errMessage =""; 

			Document DocumentoFinal = new Document(PageSize.A4); 
			
			try
			{
					
				PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem); 
				PdfReader readerTemplate = new PdfReader(caminhoTemplate);
				//criado mais um reader, para ler o template do ipac juntamente com o outro. 
				PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateIPAC);
					
				iTextSharp.text.Image imgSimboloAcreditacao ; 

				if(caminhoSimboloAcreditacao !=null)
				{
					try
					{
						imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
					}
					catch
					{
                        errMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
						return errMessage; 
					
					}
				}

				FileMode fm = new FileMode();

				if (bAlteracaoTemplate) 
				{
					fm = FileMode.Create; //vai criar um novo
				}
				else
				{	
					string nomeDocumento = fileOriginal.ToString(); 

					string refServico = nomeDocumento.Replace("-","/"); 
					refServico = refServico.Substring(0, refServico.Length-7); 
			
					string[] docIn = nomeDocumento.Split("-,.".ToCharArray()); 
					string siglaIn = docIn[2];  

					//***** novo, tem a ver com os aditamentos **********************fev.2008
					string letraSigla = siglaIn.Substring(1,1); 
					if(letraSigla.ToUpper() =="A")
					{
						//so se pode carregar aditamentos para serviços aditamentos.
						string s = refServico.Substring(0,1); 
						if(s.ToUpper()!="A")
						{
							throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Por favor elimine o ficheiro.");  //o certificado já existe, 
						}


						//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
						//strSearch = "refServico = 
						string sSearch = "refServico = '" + refServico +"'";
						DataRow[] fRows = DT.Select(sSearch,null, DataViewRowState.CurrentRows );
						string refPai = fRows[0]["refServicoPai"].ToString(); 

						sSearch = "refServico = '" + refPai +"' AND tipoCertificadoSigla ='"+siglaIn +"'";
						DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows );
						if(fR.Length >0) //o certificado já foi criado 
						{
							throw new Exception(" - O certificado já existe. ("+refPai+"-"+siglaIn+")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
							//nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
							//isto é: antes da migração foi criado à mao a referência de um certificado. 
							//normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem
						 
						}	
					}
					//++++ fim novo aditamentos +++


					//procurar se um certificado desse tipo já foi criado
					string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' "; 
					
					DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows );
					if(foundRows.Length >0) //o certificado já foi criado 
					{
						throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
						//nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
						//isto é: antes da migração foi criado à mao a referência de um certificado. 
						//normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.
						 
					}

					strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
					foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows );
					if(foundRows.Length >0) //o certificado já foi criado 
					{
						throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

					}

					strSearch =  "refServico = '" + refServico + "' AND idEstadoCertificado <> 3"; 
					foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows );
					if(foundRows.Length	> 0)
					{
						foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
						{
							string idServico = dRow["idServico"].ToString();
							string observacoes = dRow["obsWorkflowCertificado"].ToString();
							string idEstadoActual = dRow["idEstadoCertificado"].ToString();

							DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
							int iRes = data.UpdateEstadoCertificado(idServico,idEstadoActual, "2", observacoes, null,null);
							data = null; 

							//aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
							//aqui e nos b sitios todos onde essa funcao é chamada
	
							dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
							dRow["estadoCertificado"] = "Calibrado sem Validação";
							DT.AcceptChanges();
					
							fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
							//filemode é sempre create, pelos vistos
							break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
						}
					}
				}
				FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);
				
				PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

				DocumentoFinal.Open();

				PdfContentByte cb = writer.DirectContent;

				PdfImportedPage pageRelatorio;
				PdfImportedPage pageTemplatePagina;

				//ultimas 2 folhas nao levam simbolo e levam template nao acreditado
				//as outras levam simbolo e template IPAQ
				int cont = 1;
				int nPages = readerRelatorio.NumberOfPages;
				while (cont < nPages+1) //dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1
				{
					
					DocumentoFinal.SetPageSize(PageSize.A4);
					DocumentoFinal.NewPage();
					pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);

					//este template é adcionado em todas as paginas menos as 2 ultimas
					if(cont < (nPages-1)) //
					{
						pageTemplatePagina = writer.GetImportedPage(readerTemplateIPAC, 1);
					}
					else
					{
						pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);
					}

					cb.AddTemplate(pageTemplatePagina, 0, 0); //na posicao 0-0
						
					//inserir simbolo em todas as paginas menos as duas ultimas
					if(cont < (nPages-1))
					{
						if(caminhoSimboloAcreditacao !=null)
						{
							imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                            //cb.AddImage(imgSimboloAcreditacao,65.31f,0,0,62.37f,470,760); //
                            cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
                        }

                        //*************************************************************************************************
                        //************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
                        //em todas tambem menos nas 2 ultimas
                        //*************************************************************************************************
                        iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

                        iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
                        //60, 300, 100, 500
                        //2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
                        //ct.SetSimpleColumn(myPhrase, 325, 640, 495, 770, 15, Element.ALIGN_LEFT);
                        //ct.SetSimpleColumn(myPhrase, 310, 630, 495, 760, 15, Element.ALIGN_LEFT);
                        ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
                        ct.Go();

                        //*************************************************************************************************
                        //*************************************************************************************************
                        //*************************************************************************************************
					}
					cb.AddTemplate(pageRelatorio,0, 0); 		
					cont++;
				}
				DocumentoFinal.Close();

				//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
				if (!bAlteracaoTemplate)
				{
					fileOriginal.CopyTo(caminhoDocumentoBackup, true);
					fileOriginal.Delete();
				}
			}
			catch (Exception e)
			{
				DocumentoFinal.Close();
				if(e.Message.ToString().IndexOf("The process cannot access the file") >=0)
				{
					errMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
				}
				else
				{
					errMessage += fileOriginal.ToString() + " " + e.Message.ToString()+"<br />";
				}
			}	

			return errMessage; 
		}
		
		//FIM MARTELADA DOSIMETROS



       

        //INICIO MARTELADA VMLF - COPIADO DOS DOSIMETROS. SEMPRE ACREDITADO COM SIMBOLO E TEMPLATE MENOS NA ULTIMA PAGINA. 
        // USA AS DUAS TEMPLATES, EM TODAS AS PAGNAS MENOS na ultima, SIMBOLO E TEMPLATE ACREDITADOS
		/// NA ULTIMA SEM SIMBOLO E TEMPLATE NAO ACREDITADO
		/// copiado do mergefilesdosimetros
        /// 
        /// 
        /// 2018: agora é ao contrario, é na primeira sem simbolo e template nao acreditado
        /// </summary>
        private string mergeFilesVMLF(string caminhoDocumentoDestino, string caminhoDocumentoOrigem, string caminhoSimboloAcreditacao, string caminhoDocumentoBackup, FileInfo fileOriginal, bool bAlteracaoTemplate, Chunk nomeLaboratorioHeader)
        {

            //a ultima pagina nao leva simbolo e leva template nao acreditada
            //as outras levam simbolo e template IPAQ (sempre)
            //Ha sempre mais de uma pagina, pq é sempre feito o anexo de um documento ao certificado. 

            //2018 agora é ao contrario, a primeira é que nao leva nem simbolo nem template acreditada
            string errMessage = "";

            Document DocumentoFinal = new Document(PageSize.A4);

            try
            {

                PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);

                PdfReader readerTemplate = new PdfReader(caminhoTemplate);
                
                PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateIPAC);

                iTextSharp.text.Image imgSimboloAcreditacao;

                if (caminhoSimboloAcreditacao != null)
                {
                    try
                    {
                        imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                    }
                    catch
                    {
                        errMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
                        return errMessage;

                    }
                }

                FileMode fm = new FileMode();

                if (bAlteracaoTemplate)
                {
                    fm = FileMode.Create; //vai criar um novo
                }
                else
                {
                    string nomeDocumento = fileOriginal.ToString();

                    string refServico = nomeDocumento.Replace("-", "/");
                    refServico = refServico.Substring(0, refServico.Length - 7);

                    string[] docIn = nomeDocumento.Split("-,.".ToCharArray());
                    string siglaIn = docIn[2];

                    //***** novo, tem a ver com os aditamentos **********************fev.2008
                    string letraSigla = siglaIn.Substring(1, 1);
                    if (letraSigla.ToUpper() == "A")
                    {
                        //so se pode carregar aditamentos para serviços aditamentos.
                        string s = refServico.Substring(0, 1);
                        if (s.ToUpper() != "A")
                        {
                            throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Elimine ficheiro.");  //o certificado já existe, 
                        }


                        //se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
                        //strSearch = "refServico = 
                        string sSearch = "refServico = '" + refServico + "'";
                        DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
                        string refPai = fRows[0]["refServicoPai"].ToString();

                        sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
                        DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
                        if (fR.Length > 0) //o certificado já foi criado 
                        {
                            throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
                            //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
                            //isto é: antes da migração foi criado à mao a referência de um certificado. 
                            //normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem

                        }
                    }
                    //++++ fim novo aditamentos +++


                    //procurar se um certificado desse tipo já foi criado
                    string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

                    DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                    if (foundRows.Length > 0) //o certificado já foi criado 
                    {
                        throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
                        //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
                        //isto é: antes da migração foi criado à mao a referência de um certificado. 
                        //normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.

                    }

                    strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
                    foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                    if (foundRows.Length > 0) //o certificado já foi criado 
                    {
                        throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

                    }

                    strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
                    foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
                        {
                            string idServico = dRow["idServico"].ToString();
                            string observacoes = dRow["obsWorkflowCertificado"].ToString();
                            string idEstadoActual = dRow["idEstadoCertificado"].ToString();

                            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
                            int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
                            data = null;

                            //aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
                            //aqui e nos b sitios todos onde essa funcao é chamada

                            dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
                            dRow["estadoCertificado"] = "Calibrado sem Validação";
                            DT.AcceptChanges();

                            fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
                            //filemode é sempre create, pelos vistos
                            break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
                        }
                    }
                }


                FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);

                PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

                DocumentoFinal.Open();

                PdfContentByte cb = writer.DirectContent;

                PdfImportedPage pageRelatorio;
                PdfImportedPage pageTemplatePagina;


                //ultima folha nao leva simbolo e leva template nao acreditado
                //as outras levam simbolo e template IPAQ
                //2018: agora é ao contrario: a primeira é que nao leva nada disso, o resto leva.


                int cont = 1;
                int nPages = readerRelatorio.NumberOfPages;
                while (cont < nPages + 1) 
                    
                    //dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1

                    //nao está programado, mas é subentendido que o documento tem sempre no minimo 2 paginas.
                {

                    DocumentoFinal.SetPageSize(PageSize.A4);
                    DocumentoFinal.NewPage();
                    pageRelatorio = writer.GetImportedPage(readerRelatorio,cont);

                    ////este template é adcionado em todas as paginas na 2 ultimas ??
                    //if (cont < (nPages )) //
                    //{
                    //    pageTemplatePagina = writer.GetImportedPage(readerTemplateIPAC, 1);
                    //}
                    //else
                    //{
                    //    pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);
                    //}

                    if (cont == 1 ) //
                    {
                        pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);
                      
                    }
                    else
                    {
                          pageTemplatePagina = writer.GetImportedPage(readerTemplateIPAC, 1);
                    }

                    cb.AddTemplate(pageTemplatePagina, 0, 0); //na posicao 0-0

                    ////inserir simbolo em todas as paginas menos a ultima
                    //if (cont < (nPages ))
                    //{
                    //    if (caminhoSimboloAcreditacao != null)
                    //    {
                    //        imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                    //        cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
                    //    }


                    //    //************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
                    //    // em todas menos nas duas ultimas
                    //    //*************************************************************************************************
                    //    iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

                    //    iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
                    //    //60, 300, 100, 500
                    //    //2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
                    //     ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
                    //    ct.Go();



                    //}


                    //inserir simbolo em todas as paginas menos a primeira
                    if (cont > 1)
                    {
                        if (caminhoSimboloAcreditacao != null)
                        {
                            imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                            //cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
                            cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
                        }

                        //*************************************************************************************************
                        //************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
                        // em todas menos nas duas ultimas
                        //*************************************************************************************************
                        iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

                        iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
                        //60, 300, 100, 500
                        //2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
                        // ct.SetSimpleColumn(myPhrase, 325, 640, 495, 770, 15, Element.ALIGN_LEFT);
                        // ct.SetSimpleColumn(myPhrase, 310, 630, 495, 760, 15, Element.ALIGN_LEFT);
                        ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);

                        ct.Go();

                        //*************************************************************************************************
                        //*************************************************************************************************
                        //*************************************************************************************************

                    }

                    cb.AddTemplate(pageRelatorio, 0, 0);

                    cont++;
                }

                DocumentoFinal.Close();

                //3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
                if (!bAlteracaoTemplate)
                {
                    fileOriginal.CopyTo(caminhoDocumentoBackup, true);
                    fileOriginal.Delete();
                }
            }
            catch (Exception e)
            {
                DocumentoFinal.Close();
                if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
                {
                    errMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
                }
                else
                {
                    errMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
                }
            }

            return errMessage; 
        }
		//FIM MARTELADA VMLF


		//INICIO VACV

		private string mergeFilesVACV(string caminhoDocumentoDestino, string caminhoDocumentoOrigem, string caminhoSimboloAcreditacao, string caminhoDocumentoBackup, FileInfo fileOriginal, bool bAlteracaoTemplate, Chunk nomeLaboratorioHeader)
		{

			// so nas 2 primeiras paginas o simbolo e o fundo acreditado.

			string errMessage = "";

			Document DocumentoFinal = new Document(PageSize.A4);

			try
			{

				PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);

				PdfReader readerTemplate = new PdfReader(caminhoTemplate);

				PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateIPAC);

				iTextSharp.text.Image imgSimboloAcreditacao;

				if (caminhoSimboloAcreditacao != null)
				{
					try
					{
						imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
					}
					catch
					{
						errMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
						return errMessage;

					}
				}

				FileMode fm = new FileMode();

				if (bAlteracaoTemplate)
				{
					fm = FileMode.Create; //vai criar um novo
				}
				else
				{
					string nomeDocumento = fileOriginal.ToString();

					string refServico = nomeDocumento.Replace("-", "/");
					refServico = refServico.Substring(0, refServico.Length - 7);

					string[] docIn = nomeDocumento.Split("-,.".ToCharArray());
					string siglaIn = docIn[2];

					//***** novo, tem a ver com os aditamentos **********************fev.2008
					string letraSigla = siglaIn.Substring(1, 1);
					if (letraSigla.ToUpper() == "A")
					{
						//so se pode carregar aditamentos para serviços aditamentos.
						string s = refServico.Substring(0, 1);
						if (s.ToUpper() != "A")
						{
							throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Elimine ficheiro.");  //o certificado já existe, 
						}


						//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
						//strSearch = "refServico = 
						string sSearch = "refServico = '" + refServico + "'";
						DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
						string refPai = fRows[0]["refServicoPai"].ToString();

						sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
						DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
						if (fR.Length > 0) //o certificado já foi criado 
						{
							throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
																												  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
																												  //isto é: antes da migração foi criado à mao a referência de um certificado. 
																												  //normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem

						}
					}
					//++++ fim novo aditamentos +++


					//procurar se um certificado desse tipo já foi criado
					string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

					DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
					if (foundRows.Length > 0) //o certificado já foi criado 
					{
						throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
																			  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
																			  //isto é: antes da migração foi criado à mao a referência de um certificado. 
																			  //normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.

					}

					strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
					foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
					if (foundRows.Length > 0) //o certificado já foi criado 
					{
						throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

					}

					strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
					foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
					if (foundRows.Length > 0)
					{
						foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
						{
							string idServico = dRow["idServico"].ToString();
							string observacoes = dRow["obsWorkflowCertificado"].ToString();
							string idEstadoActual = dRow["idEstadoCertificado"].ToString();

							DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
							int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
							data = null;

							//aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
							//aqui e nos b sitios todos onde essa funcao é chamada

							dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
							dRow["estadoCertificado"] = "Calibrado sem Validação";
							DT.AcceptChanges();

							fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
												  //filemode é sempre create, pelos vistos
							break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
						}
					}
				}


				FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);

				PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

				DocumentoFinal.Open();

				PdfContentByte cb = writer.DirectContent;

				PdfImportedPage pageRelatorio;
				PdfImportedPage pageTemplatePagina;


				//ultima folha nao leva simbolo e leva template nao acreditado
				//as outras levam simbolo e template IPAQ



				int cont = 1;
				int nPages = readerRelatorio.NumberOfPages;
				while (cont < nPages + 1)

				//dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1

				//nao está programado, mas é subentendido que o documento tem sempre no minimo 2 paginas.
				{

					DocumentoFinal.SetPageSize(PageSize.A4);
					DocumentoFinal.NewPage();
					pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);

					//este template é SO NAS PRIMERIAS DUAS
					if (cont < 3) //
					{
						pageTemplatePagina = writer.GetImportedPage(readerTemplateIPAC, 1);
					}
					else
					{
						pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);
					}

				

					cb.AddTemplate(pageTemplatePagina, 0, 0); //na posicao 0-0

					//O SIMBOLO é SO NAS PRIMERIAS DUAS

					if (cont < 3)
					{
						if (caminhoSimboloAcreditacao != null)
						{
							imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
							//    cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
							cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
						}


						
						//*************************************************************************************************
						iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

						iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
						//60, 300, 100, 500
						//2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
						//ct.SetSimpleColumn(myPhrase, 325, 640, 495, 770, 15, Element.ALIGN_LEFT);
						//ct.SetSimpleColumn(myPhrase, 310, 630, 495, 760, 15, Element.ALIGN_LEFT);
						ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
						ct.Go();



					}




					cb.AddTemplate(pageRelatorio, 0, 0);

					cont++;
				}

				DocumentoFinal.Close();

				//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
				if (!bAlteracaoTemplate)
				{
					fileOriginal.CopyTo(caminhoDocumentoBackup, true);
					fileOriginal.Delete();
				}
			}
			catch (Exception e)
			{
				DocumentoFinal.Close();
				if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
				{
					errMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
				}
				else
				{
					errMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
				}
			}

			return errMessage;
		}
		//FIM MARTELADA ECH

		//FIM VACV


		//INICIO MARTELADA ECH - COPIADO DOS DOSIMETROS e do vmlf (QUE ERAM AO CONTRARIO PRIMEIRO). SEMPRE ACREDITADO COM SIMBOLO E TEMPLATE MENOS NA ULTIMA PAGINA. 
		// USA AS DUAS TEMPLATES, EM TODAS AS PAGNAS MENOS na ultima, SIMBOLO E TEMPLATE ACREDITADOS
		/// NA ULTIMA SEM SIMBOLO E TEMPLATE NAO ACREDITADO
		/// 
		/// 

		/// </summary>
		private string mergeFilesECH(string caminhoDocumentoDestino, string caminhoDocumentoOrigem, string caminhoSimboloAcreditacao, string caminhoDocumentoBackup, FileInfo fileOriginal, bool bAlteracaoTemplate, Chunk nomeLaboratorioHeader)
        {

            //a ultima pagina nao leva simbolo e leva template nao acreditada
            //as outras levam simbolo e template IPAQ (sempre)
            //Ha sempre mais de uma pagina, pq é sempre feito o anexo de um documento ao certificado. 

          
            string errMessage = "";

            Document DocumentoFinal = new Document(PageSize.A4);

            try
            {

                PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);

                PdfReader readerTemplate = new PdfReader(caminhoTemplate);

                PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateIPAC);

                iTextSharp.text.Image imgSimboloAcreditacao;

                if (caminhoSimboloAcreditacao != null)
                {
                    try
                    {
                        imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                    }
                    catch
                    {
                        errMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
                        return errMessage;

                    }
                }

                FileMode fm = new FileMode();

                if (bAlteracaoTemplate)
                {
                    fm = FileMode.Create; //vai criar um novo
                }
                else
                {
                    string nomeDocumento = fileOriginal.ToString();

                    string refServico = nomeDocumento.Replace("-", "/");
                    refServico = refServico.Substring(0, refServico.Length - 7);

                    string[] docIn = nomeDocumento.Split("-,.".ToCharArray());
                    string siglaIn = docIn[2];

                    //***** novo, tem a ver com os aditamentos **********************fev.2008
                    string letraSigla = siglaIn.Substring(1, 1);
                    if (letraSigla.ToUpper() == "A")
                    {
                        //so se pode carregar aditamentos para serviços aditamentos.
                        string s = refServico.Substring(0, 1);
                        if (s.ToUpper() != "A")
                        {
                            throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Elimine ficheiro.");  //o certificado já existe, 
                        }


                        //se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
                        //strSearch = "refServico = 
                        string sSearch = "refServico = '" + refServico + "'";
                        DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
                        string refPai = fRows[0]["refServicoPai"].ToString();

                        sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
                        DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
                        if (fR.Length > 0) //o certificado já foi criado 
                        {
                            throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
                            //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
                            //isto é: antes da migração foi criado à mao a referência de um certificado. 
                            //normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem

                        }
                    }
                    //++++ fim novo aditamentos +++


                    //procurar se um certificado desse tipo já foi criado
                    string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

                    DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                    if (foundRows.Length > 0) //o certificado já foi criado 
                    {
                        throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
                        //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
                        //isto é: antes da migração foi criado à mao a referência de um certificado. 
                        //normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.

                    }

                    strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
                    foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                    if (foundRows.Length > 0) //o certificado já foi criado 
                    {
                        throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

                    }

                    strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
                    foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
                        {
                            string idServico = dRow["idServico"].ToString();
                            string observacoes = dRow["obsWorkflowCertificado"].ToString();
                            string idEstadoActual = dRow["idEstadoCertificado"].ToString();

                            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
                            int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
                            data = null;

                            //aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
                            //aqui e nos b sitios todos onde essa funcao é chamada

                            dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
                            dRow["estadoCertificado"] = "Calibrado sem Validação";
                            DT.AcceptChanges();

                            fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
                            //filemode é sempre create, pelos vistos
                            break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
                        }
                    }
                }


                FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);

                PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

                DocumentoFinal.Open();

                PdfContentByte cb = writer.DirectContent;

                PdfImportedPage pageRelatorio;
                PdfImportedPage pageTemplatePagina;


                //ultima folha nao leva simbolo e leva template nao acreditado
                //as outras levam simbolo e template IPAQ
                


                int cont = 1;
                int nPages = readerRelatorio.NumberOfPages;
                while (cont < nPages + 1)

                //dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1

                //nao está programado, mas é subentendido que o documento tem sempre no minimo 2 paginas.
                {

                    DocumentoFinal.SetPageSize(PageSize.A4);
                    DocumentoFinal.NewPage();
                    pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);

                    //este template é adcionado em todas as paginas na 2 ultimas ??
                    if (cont < (nPages)) //
                    {
                        pageTemplatePagina = writer.GetImportedPage(readerTemplateIPAC, 1);
                    }
                    else
                    {
                        pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);
                    }

                    //if (cont == 1) //
                    //{
                    //    pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);

                    //}
                    //else
                    //{
                    //    pageTemplatePagina = writer.GetImportedPage(readerTemplateIPAC, 1);
                    //}

                    cb.AddTemplate(pageTemplatePagina, 0, 0); //na posicao 0-0

                    //inserir simbolo em todas as paginas menos a ultima
                    if (cont < (nPages))
                    {
                        if (caminhoSimboloAcreditacao != null)
                        {
                            imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                            //    cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
                            cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
                        }


                        //************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
                        // em todas menos nas duas ultimas
                        //*************************************************************************************************
                        iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

                        iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
                        //60, 300, 100, 500
                        //2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
                        //ct.SetSimpleColumn(myPhrase, 325, 640, 495, 770, 15, Element.ALIGN_LEFT);
                        //ct.SetSimpleColumn(myPhrase, 310, 630, 495, 760, 15, Element.ALIGN_LEFT);
                        ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
                        ct.Go();



                    }


                    

                    cb.AddTemplate(pageRelatorio, 0, 0);

                    cont++;
                }

                DocumentoFinal.Close();

                //3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
                if (!bAlteracaoTemplate)
                {
                    fileOriginal.CopyTo(caminhoDocumentoBackup, true);
                    fileOriginal.Delete();
                }
            }
            catch (Exception e)
            {
                DocumentoFinal.Close();
                if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
                {
                    errMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
                }
                else
                {
                    errMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
                }
            }

            return errMessage;
        }
        //FIM MARTELADA ECH
        
		//martelada de ultima hora
		//devolve uma string com msg de erro
        private string mergeFiles(string caminhoDocumentoDestino, string caminhoDocumentoOrigem, string caminhoTemplate, string caminhoSimboloAcreditacao, string caminhoDocumentoBackup, FileInfo fileOriginal, bool bAlteracaoTemplate, Chunk nomeLaboratorioHeader)
		{
			//Response.Write("entre no mergefiles com o documento "+caminhoDocumentoOrigem+"<br />"); 
			string errMessage =""; 

			Document DocumentoFinal = new Document(PageSize.A4); //fica aqui fora para ser fechado no catch
			
			try
			{
				//cria o pdfreader para ler o documento de origem e o template
				PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem); 
				PdfReader readerTemplate = new PdfReader(caminhoTemplate);
				iTextSharp.text.Image imgSimboloAcreditacao ; 

				//para ver se encontra a img do simbolo e dar mensagem se ha alguma problema. 
				//martelada num dia de dores de cabeça
				
				if(caminhoSimboloAcreditacao !=null)
				{
					try
					{
						imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
					}
					catch
					{
                        errMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
						return errMessage; 
					
					}
				}

				FileMode fm = new FileMode();

				if (bAlteracaoTemplate) //isto é uma condicao que indica se a template foi alterada, que faz com que um ficheiro é copiado para cima do outro na pasta construção. 
				{
					fm = FileMode.Create; //vai criar um novo
				}
				else
				{	//***************************************************************************************************
					//***************************************************************************************************
					//para todas as extensoes 1C, 1R, 2R, 1A etc. não pode ser validado um novo documento se já existe
					//um certificado com esse tipo. 

					//tb não podem entrar novos certificados que estejam à espera de serem validados pelo resp.tecn.
					//se ainda não existe a extensão, podem entrar todos menos os que estão no estado TRES (

					//tb nao devem entrar certificados para serviços com estados inferiores a 6...

					//PODEM ENTRAR TODOS MENOS OS QUE ESTÃO EM ESTADO 3 (VALIDADO PELO TECNICO)
					//***************************************************************************************************
					//***************************************************************************************************

					string nomeDocumento = fileOriginal.ToString(); 

					//quando o servico tem um pai, aqui tem de ser comparado com o servico pai
					//para isso, na datatable inicial tb teem de vir os pais....

					string refServico = nomeDocumento.Replace("-","/"); 
					refServico = refServico.Substring(0, refServico.Length-7); 
					
					string[] docIn = nomeDocumento.Split("-,.".ToCharArray()); 
					string siglaIn = docIn[2];  

					//***** novo, tem a ver com os aditamentos **********************fev.2008
					string letraSigla = siglaIn.Substring(1,1); 
					string s = refServico.Substring(0,1); 

					//correcao agosto 2008... nao vou mexer no resto para nao estragar
					if(s.ToUpper() == "A" && letraSigla.ToUpper() !="A") //O serviço é um aditamento
						//mas carregaram um ficheiro ".C"
					{
						throw new Exception(" - Aditamentos devem ser carregados com a extensao '1A', '2A' ou '3A'.");  //o certificado já existe, 
					}
					// fim correccao.============================


					if(letraSigla.ToUpper() =="A")
					{
						//so se pode carregar aditamentos para serviços aditamentos.
						if(s.ToUpper()!="A")
						{	
							throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Solicite a eliminação do ficheiro.");  //o certificado já existe, 
						}

						//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
						//strSearch = "refServico = 
						string sSearch = "refServico = '" + refServico +"'";
					
						DataRow[] fRows = DT.Select(sSearch,null, DataViewRowState.CurrentRows );
						string refPai = fRows[0]["refServicoPai"].ToString(); 

						sSearch = "refServico = '" + refPai +"' AND tipoCertificadoSigla ='"+siglaIn +"'";
												
						DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows );
						if(fR.Length >0) //o certificado já foi criado 
						{
							throw new Exception(" - O aditamento já existe. ("+refPai+"-"+siglaIn+")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
							//nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
							//isto é: antes da migração foi criado à mao a referência de um certificado. 
							//normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem 
						}	
					}
					

					//procurar se um certificado desse tipo já foi criado
					string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' "; 
					
					DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows );
					if(foundRows.Length >0) //o certificado já foi criado 
					{
						throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
						//nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
						//isto é: antes da migração foi criado à mao a referência de um certificado. 
						//normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem
						 
					}

					strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
					foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows );
					if(foundRows.Length >0) //o certificado já foi criado 
					{
						throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior
					}

					strSearch =  "refServico = '" + refServico + "' AND idEstadoCertificado <> 3"; //Calibrado com Validação
					foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows );
					if(foundRows.Length	> 0)
					{
						foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
						{
							string idServico = dRow["idServico"].ToString();
							string observacoes = dRow["obsWorkflowCertificado"].ToString();
							string idEstadoActual = dRow["idEstadoCertificado"].ToString();

							DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
							int iRes = data.UpdateEstadoCertificado(idServico,idEstadoActual, "2", observacoes, null); 
							data = null; 

							//aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
							//aqui e nos b sitios todos onde essa funcao é chamada
							if(iRes > -1) //pq essa parte la em cima pode dar erro
							{

								dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
								dRow["estadoCertificado"] = "Calibrado sem Validação";
								DT.AcceptChanges();
					
								fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
								//filemode é sempre create, pelos vistos
							}
							break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
						}
					}
				}

				FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);
				
				PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);
				//writer.SetEncryption(PdfWriter.STRENGTH40BITS, null, null, PdfWriter.AllowCopy);

				DocumentoFinal.Open();

				PdfContentByte cb = writer.DirectContent;

				PdfImportedPage pageRelatorio;
				PdfImportedPage pageTemplatePagina;

				int j = 0;
				while (j < readerRelatorio.NumberOfPages)
				{
					j++;
					DocumentoFinal.SetPageSize(PageSize.A4);
					DocumentoFinal.NewPage();
					pageRelatorio = writer.GetImportedPage(readerRelatorio, j);

					pageTemplatePagina = writer.GetImportedPage(readerTemplate, 1);
					cb.AddTemplate(pageTemplatePagina, 0, 0); //na posicao 0-0
					
					//se tem simbolo 
					if(caminhoSimboloAcreditacao !=null)
					{
						
						imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                        //cb.AddImage(imgSimboloAcreditacao,65.31f,0,0,62.37f,470,760); //
                        //cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
                        cb.AddImage(imgSimboloAcreditacao, 108f, 0, 0, 52.37f, 470, 760); //

                        //2º param o width em px, 5º param o height em px, os dois ultimos a posicao na pagina.
                        //cb.AddTemplate(pageRelatorio, 1, 0, 0, 0.95f, 0, 65);
                    }

                    //*************************************************************************************************
                    //************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
                    //se nao tem simbolo, escreve string vazia, era para escrever texto default, mas apaguei isso por 
                    //causa dos reps
                    //*************************************************************************************************
                    iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

                    iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
                    //60, 300, 100, 500
                    //2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
                    ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
                   
                    ct.Go();

                    //*************************************************************************************************
                    //*************************************************************************************************
                    //*************************************************************************************************
					cb.AddTemplate(pageRelatorio,0, 0); //o 3º param empurro de baixo para cima ////40 px : 1 cm e qq coisa
					//o 2º param empurro de esquerda para a direita
				}


				DocumentoFinal.Close();

				//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
				//pretender alterar o template. Esta opção não é válida se for para mudar de template
				if (!bAlteracaoTemplate) //ok faz sentido
				{
					fileOriginal.CopyTo(caminhoDocumentoBackup, true);
					fileOriginal.Delete();
				}
			}
			catch (Exception e)
			{
				DocumentoFinal.Close();

                //switch(e.m futuramente pôr um switch aqui quando estiver bem definda a mensagem de erro; 
                if(e.Message.ToString() == "Rebuild failed: trailer not found.; Original message: PDF startxref not found.")
                {

                   errMessage+= MSG_PDF_CORROMPIDO; //indicacao que falta criar pdf
                   
                }
                else if(e.Message.ToString().IndexOf("- O certificado já existe.")>=0)
                {
                    errMessage += fileOriginal.ToString().ToUpper() + e.Message.ToString()+"<br />";
                
                }
				else if(e.Message.ToString().IndexOf("The process cannot access the file") >=0)
				{
					errMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
				}
				else if (e.Message.ToString().IndexOf("O aditamento já existe") >=0)
				{

					errMessage += fileOriginal.ToString().ToUpper() + e.Message.ToString()+"<br />";
				}
				else
				{
					Response.Write(e.ToString()+"<br /><br />"); 
					errMessage += fileOriginal.ToString() + " " + e.Message.ToString()+"<br />";
				}
			}	

			return errMessage; 
		}
		#endregion


		# region BVERIFICACAOPASTASFINAISCONSTRUCAO
		//========================================================================================================
		//valida os ficheiros existentes na directoria FINAIS CONSTRUCAO
		//ja nao vai retornar qualquer mensagem de erro, apenas marcar as linhas correspondentes do ficheiro. vou retornar sempre true ou entao nao avaliar o bool
		//usa a DT sem viewstate pois é chamado antes de qq postback
		//========================================================================================================
		private bool bVerificacaoPastaFinaisConst()
		{
			
			DirectoryInfo dirInfopastaCertificadosConstrucao = new DirectoryInfo(pastaCertificadosFinaisConstrucao);

			Hashtable HTIdCert = HTIdCertificadoBySigla(); //contem sigla / idTipoCertificado
			Hashtable HTDescCert = HTDescCertificadoBySigla(); //contem sigla /tipoCertificado(descricao)


			//se encontra um ficheiro mas o idEstadoCertificado = 1 (n existente)  (devia estar a 2,5 ou 6)  (penso que os rejeitados tb ficam nessa pasta ate vir um novo documento)
			//ou se encontra algum certificado para serviços em estado  anulado, avariado ou suspenso
			//marca o nome do ficherio com "Certificado Inválido". NAO: já nao marca os que estão com idEstadoCertificado = 1 pois 
			//estes ficam marcados com "FALTA CRIAR PDF" (ou similar) 06.10.2008
			
			//vai procurar todos os rows onde não devia existir nenhum ficheiro na pasta de destino e que estejam nos estados 7,8,9,20,21
			

			//===========================================================================================================
			//resumindo: certificados para serviços em estado 7,8,9,20,21 serão automaticamente apagados e é feito um reset ao 
			//idEstadoCertificado do serviço. linhas marcadas com "MSG_CERTIFICADO_INVALIDO"
			//certificados para serviços em estado 10 tb devem ser apagado, é mais facil assim...(automatico, enquanto da outra
			//forma, documentos errados podem ficar na pasta
			//============================================================================================================

			//string strExpr = "idEstadoCertificado = 1 OR idEstadoServico IN (7,8,9,10,21)"; //DM: adicionado o 10 no dia 19-07-07
			//dm: o idEstadoCertificado = 1 tem de ficar fora disso, nao pode ser apagado. 06-10-2008
			string strExpr = "idEstadoServico IN (7,8,9,10,21)"; //DM: adicionado o 10 no dia 19-07-07 -- os estados nos quais nao devia
			//existir nenhum certificado. 




			//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
			//os que estão num estado em que não pode exisitir nenhum certificado (dentro dos possiveis estados que foram escolhidos para aqui constar)
			//out 2008: sao simplesmente apagados automaticamente e a datarow tb é apagada. 
			//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
			DataRow[] dRowsCertInvalidos = DT.Select(strExpr, null, DataViewRowState.CurrentRows );
			foreach (DataRow dRow in dRowsCertInvalidos)
			{
				FileInfo[] fi = dirInfopastaCertificadosConstrucao.GetFiles("*" + dRow["nomeDocumento"].ToString() + "*"); //procura ficheiro que n devia existir.

				if (fi.Length > 0) //encontrou
				{
					//dRow["nomeFichCalibracao"] = "APAGADO"; //MSG_CERTIFICADO_INVALIDO; //devia simplesmente apgar isto.  ....
					dRow.Delete(); //apago tb a datarow, tao simples assim. 
					foreach (FileInfo f in fi) //apago simplesmente os ficheiros que estao a mais. 
					{
						try
						{
							f.Delete(); 
						}
						catch(Exception ex)
						{
							GERAL.clsWriteError.WriteLog("linha 1072 gestdocs - apagar ficheiro" + ex.ToString()); 
						}
					}
				}
				dRow.Delete(); 
			}

			//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
			//vai procurar os serviços marcados como tendo certificado (idEstadoCertificado = 2 ) e verificar se o documento correspondente existe
			//caso contrário marca o nome doc com MSG_PDF_INEXISTENTE e a varival bFileExists com false
			//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
			string strExpr2 = "idEstadoCertificado IN(2,5,6) ";  //o 5 e o 6 tb deviam ter documentos na pasta construção, mas se faltar um doc para o 5 + 6 nao é critico. 
			DataRow[] dRowsCertValidos = DT.Select(strExpr2, null, DataViewRowState.CurrentRows );
           
			
			foreach (DataRow dRow in dRowsCertValidos) 
			{		
				
				//se nao encontra ficheiro, marca o nomeFichCalibracao como MSG_PDF_INEXISTENTE
				FileInfo[] fi = dirInfopastaCertificadosConstrucao.GetFiles("*" + dRow["nomeDocumento"].ToString() + "*");
				if (fi.Length == 0) //n encontrou
				{ 	
					dRow["nomeFichCalibracao"] = MSG_PDF_INEXISTENTE; 
					//bFileExists = false; //problema: aqui podem estar os inválidos de outro user, que eu NAO quero listar.							
				}
				else //encontrou ficheiro
				{
					if(dRow["idEstadoCertificado"].ToString() =="2") //se é 2 escreve no nome ficheiro , serve para poder abrir tb os rejeitados, pelo campo nomeDocumento, em vez do campo "nomeFichCalibracao"
					{
						string sigla = strExtraiSigla(fi[0].ToString()); //atencao que isto pode correr mal
						object x = HTIdCert[sigla];
						int idTipoCert = System.Convert.ToInt16(x);
						dRow["idTipoCertificado"] = idTipoCert.ToString(); //actualiza o campo "(ID) tipo de certificado" da DT
						dRow["nomeFichCalibracao"] = fi[0].ToString(); //actualiza o campo "nome do ficheiro" da DT
					
						object y = HTDescCert[sigla];
						string descTipoCert = System.Convert.ToString(y); 
						dRow["tipoCertificado"] = descTipoCert;
					}
					
					//em todos os casos, escreve no campo nomeDocumento out 2008
					dRow["nomeDocumento"] =fi[0].ToString(); 
				}
			}


			dirInfopastaCertificadosConstrucao = null; 

			//[apaguei aqui grandes pedaços de código]
			
			//fazer uma especie de select distinct //nao sei se ainda preciso disto.
			//pq como o inner join é feito com a tabela certificado, no caso de ja existirem duas linhas na tabela certificado para um serviço, entao aparecem 2 linhas tb ao utilizador, com os mesmo dados...
			string sIdServico =""; 
			
			
			foreach(DataRow dr in DT.Select(null, null, DataViewRowState.CurrentRows))
			{
				if(sIdServico == dr["idServico"].ToString())
				{
					sIdServico = dr["idServico"].ToString(); //tem que ficar aqui tb
					dr.Delete(); //isto faz uma especie de select distinct.

				}
				else
				{
					sIdServico = dr["idServico"].ToString(); //tem que ficar no else
					//senao dá mensagem de que deletedrowinformation cannot be accessed
				}
				
			}

			DT.AcceptChanges(); //just in case me tenha esquecido algures...


				


			//			if (bCertValido==false) return false;
			//			if (bFileExists==false) return false;
			return true; //agora devolvo sempre true 
		}

		#endregion


		private bool ColumnEqual(object A, object B)
		{
	
			// Compares two values to see if they are equal. Also compares DBNULL.Value.
			// Note: If your DataTable contains object fields, then you must extend this
			// function to handle them in a meaningful way if you intend to group on them.
			
			if ( A == DBNull.Value && B == DBNull.Value ) //  both are DBNull.Value
				return true; 
			if ( A == DBNull.Value || B == DBNull.Value ) //  only one is DBNull.Value
				return false; 
			return ( A.Equals(B) );  // value type standard comparison
		}
					
 
		//esta funcao existe na sua forma generica na pagina formfacturasap.
		
		private DataTable SelectDistinct(DataTable SourceTable, string FieldName)
		{	
			DataTable dt = new DataTable();
			dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

			object LastValue = null; 

			foreach (DataRow dr in SourceTable.Select("", FieldName))
			{
				if (  LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])) ) 
				{
					LastValue = dr[FieldName]; 
					dt.Rows.Add(new object[]{LastValue});
				}
			}
			
			return dt;

		}

		#region BINDGRID
		//BINDGRID DOS RESULTADOS 
		private void BindGrid()
		{
			if(Page.IsPostBack)	//da primeira vez a DT está em memoria pq nao houve nenhum postback
				//depois de haver um submit, tb devia recarregar tudo do principio.
			{
				DT = (DataTable)ViewState["DT"]; 
			}
			
			//Aqui vou apagar todos os que são de outro user!!!! (para não entrar nos loops das validações)
			//aqui nao posso fazer isto, pq pode sempre escolher ver todos...mas se nao tem essa opção, apago
			//para o users que teem essa opção acessivel, a dt será sempre maior que o datagrid (no caso de so
			//verem os registos deles proprios)

			//nova evolução no meio disto tduo.... DJIZAS
			//como pode ser outra pessoa a fazer a 1 revisao ou o 1 aditamento....
			//no caso desses, nao posso apagar
			//portanto aqui so posso apagar os que belongstouser = 0 AND idTipoCertificado = 1 //1 certificado
			//todos os que sao idTipoCertificado > 1 (revisoes, aditamentos) teem de ficar visiveis para todos
			//os users da mesma grandeza


			//mas aqui ainda ha outro problema, se o user logado for responsavel por pela grandeza do serviço
			//tem de ficar visivel para ele tb, pq os superiores podem sempre validaro todos os documentos das
			//suas grande

			//######################################################
			
			//se sao de outro user ficam disabled, mas se sao revisoes aditamentos etc ficam 
			//enabled para users da mesma grandeza
			//OU se o user é gestor de certificados tb enabled
			//OU se o user é RT e o serviço é de uma das suas grandedzes  TB ENABLED
			//mas se o idUtilizadorValidouCertificado nao for null, so fica enabled para esse user


			//antes, apagar alguns que possam estar na dt em estado diferente de 1 e 2 (por exemplo porque foi colocado um doc 
			//errado na directoria //para já só vou validar o estado 3, calibrado com validação, quando dps 
			//disso foi criado mais um documento e colocado na directoria.

			DataRow[] drEstado3 = DT.Select("idEstadoCertificado in (3)", null, DataViewRowState.CurrentRows );
			foreach(DataRow dr in drEstado3)
			{
				dr.Delete(); 
			}

			
			if(cbTodos.Checked==false) 
			{
				//PERFIS: 4	Responsável Técnico - 5	Técnico Especialista
				//se o user logado é de um desses 2 perfis, tenho de fazer uma validação linha à linha de todos 
				//os serviços que ele pode validar, e esses não podem ser apagado como na funcao a seguir.

				string sGrandezas =strGrandezasRespLogado();
                
				if(Session["idPerfil"].ToString() == "4"  || Session["idPerfil"].ToString() == "5")
				{

                    DataRow[] drow = DT.Select(null, null, DataViewRowState.CurrentRows);
                   
                    foreach(DataRow dr in drow)
					{

                        //nao funciona
                        //if (cbCertifsUser.Checked) //novo, apagar os que nao sao dele.
                        //{
                        //    if (dr["belongsToUser"].ToString() == "False")
                        //    {
                        //        try
                        //        {
                        //            dr.Delete();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            //Response.Write(ex.ToString()); 
                        //            GERAL.clsWriteError.WriteLog(ex.ToString());
                        //        }
                        //    }
                        //}
                     

						if(sGrandezas.IndexOf(dr["idGrandezaServico"].ToString())==-1)
						{

							if(dr["username"].ToString()!=User.Identity.Name.ToString()) //se por acaso o TE calibrou
								//algum equipamento de outra grandeza, nao se apaga, caso contrário, apaga-se
							{
								try
								{
									dr.Delete(); 
								}
								catch(Exception ex)
								{
									//Response.Write(ex.ToString()); 
									GERAL.clsWriteError.WriteLog(ex.ToString()); 
								}
							}
						}
					}

                    ////29/01/2018
                    //nao funciona, acho que algures sao apagdos 2 vezes os mesmos registos e estoira
                    ////o que se pretende aqui  é que caso ele so queira ver os proprios
                    //if (cbCertifsUser.Checked) //se a checkbox está checked quer dizer que o RT so quer ver os proprios certificados
                    //{
                    //    DataRow[] drOutros = DT.Select("belongsToUser = 0", null, DataViewRowState.ModifiedCurrent);
                    //    //ver se isto funciona com modified current
                    //    foreach (DataRow dr in drOutros)
                    //    {
                    //        dr.Delete();
                    //    }
                    //}
				}
				else
				{

					//************************************************************************************************
					//queremos que as revisões apareçam aos funcionarios da mesma grandeza
					//apaga os de outros users e do tipo 1 pq tudo o que é depois do 1c pode ser validado por todos. 
					//**************************************************************************************************
					DataRow[] drOutros = DT.Select("belongsToUser = 0 AND idTipoCertificado = 1", null, DataViewRowState.CurrentRows );
					foreach(DataRow dr in drOutros)
					{
						dr.Delete(); 
					}
					//out 2008: tb posso apagar estes, pois foi onde o outro user ainda nao criou um documento. com idestadocertificado 1 ou 2, onde o user ainda 
					//nao carregou ou validou nada. 
					DataRow[] drOutrosOutros = DT.Select("belongsToUser = 0 AND idEstadoCertificado IN (1)", null, DataViewRowState.CurrentRows );  //talvez acrescentar aqui o 2... 
					foreach(DataRow dr in drOutrosOutros)
					{
						dr.Delete(); 
					}
				}
			}
			


			
			//username aqui é o nome do user que efectuou o serviço

			//quando uma revisao ou aditamento é rejeitado, o fichTipCertificado é null ou vazio, 
			//quando é um é calibracao(1) tem de aparecer no user, quando é maior que um pode aparecer em todos os users da grandeza
			//acrescentado, depois de uma revisao validada por um user, ja so aparece a esse user.
		
			
			DV = new DataView(DT); 

			if(Session["idPerfil"].ToString() == "6" || (strGrau()=="0")) //mostra so registos do user logado
				//TECNICO (aos RT+TE mostra tudo). 
				//ou GESTOR CERTIFICADOS (pode ver tudo																						se checkar a caixa de ver todos).
			{
				string strSelect= " username = '"+User.Identity.Name.ToString()+"' OR ( isnull(idTipoCertificado,0) <>1 AND idUtilizadorValidouCertificado is null AND idGrandezaServico = '"+Session["idGrandezaUtilizador"].ToString()+"') ";
			
				if(!cbTodos.Checked) DV.RowFilter = strSelect; //isto estava fora do if e passou para dentro, testar bem!!!
			}

            //novo - se quer ver so os seus //nao apaguei nada como la em cima...

            if (cbCertifsUser.Checked) //se ele é rt ou tecnico especialisa
            {
                if (Session["idPerfil"].ToString() == "4" || Session["idPerfil"].ToString() == "5")
                {
                    string strSelect = "belongsToUser  = 1";

                    DV.RowFilter = strSelect; //isto estava fora do if e passou para dentro, testar bem!!!
                }
            
            }
			DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
			dgDocumentos.DataSource = DV;
			dgDocumentos.DataBind();

			if(DT.Rows.Count >0) //ver se chamei o acceptchanges antes de chegar aqui
			{//se ha registos
				
				dgDocumentos.Visible = true;
			}
			else
			{//se nao ha registos
				dgDocumentos.DataSource = null;
				dgDocumentos.DataBind(); 
				dgDocumentos.Visible = false;

                lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_NO_RECORDS; 
			}			
		}
		#endregion
		//devolve true se ha algo para gravar e false se nao ha nada
		public bool validacaoRegistos()
		{
			DT = (DataTable)ViewState["DT"];
			DV = new DataView(DT);
			
			foreach (DataRowView drv in DV) //LOOP
			{
				bool botaoAprovar = Convert.ToBoolean(drv.Row["cbAprovar"].ToString());
				bool botaoRejeitar = Convert.ToBoolean(drv.Row["cbRejeitar"].ToString());
				if (botaoAprovar || botaoRejeitar)
				{
					return true; 
				}
			}
			return false; 
		}

				
		//=======================================================================================
		// Apaga os documentos que foram mantidos para alteração do template quando se submete o
		// documento para validação pelo Responsável de Laboratório
		//=======================================================================================
		private void deleteDocs(string idsServico)
		{
			//1ºListar os ficheiros a apagar!!!

			DirectoryInfo dirInfoPastaBackupOrigem = new DirectoryInfo(pastaCertificadosOriginaisBackup);

			DT = (DataTable)ViewState["DT"];
			DV = new DataView(DT);

			DV.RowFilter = "idServico in (" + idsServico.ToString() + ")";

			foreach (DataRowView drv in DV)
			{

				FileInfo[] fileInfosOriginalBackup = dirInfoPastaBackupOrigem.GetFiles("*" + drv.Row["nomeDocumento"].ToString() + "*");
				if (fileInfosOriginalBackup.Length != 0)
				{
					fileInfosOriginalBackup[0].Delete();
				}
			}
		}


		//=======================================================================================
		//este autopostback tem de ficar pois cada vez que é alterado, um documento é copiado para
		//a pasta construcao.
		

		//ok: no fundo o que se quer aqui fazer é:
		//alterar esta informação na datatable
		//copiar um novo documento para a pasta CONSTRUCAO, COM OU SEM SIMBOLO, CONFORME ALTERACAO


		//=======================================================================================
		protected void cbTemplate_checked(object sender, System.EventArgs e)
		{
			CheckBox cb = (CheckBox)sender;
			DataGridItem dgi = (DataGridItem)cb.Parent.Parent;
			int ind = dgi.ItemIndex;

			//aqui acedo a celulas do datagrid pelo index...
			string nomeDoc = dgDocumentos.Items[ind].Cells[20].Text.ToString(); //testar se apanha
			string idServico = dgDocumentos.DataKeys[ind].ToString();  //estava num cell, mudei. DM
			string acreditado = convertBoolToBit(cb.Checked);


			//guarda a alteração na datatable
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT);
			DV.Sort = "idServico"; //para o find que vem a seguir. //posso fazer o sort pela dataview mas o update à datatable
			int index = DV.Find(dgDocumentos.DataKeys[ind]);
			DT.Rows[index]["cbTemplate"] = cb.Checked.ToString(); 
			ViewState["DT"] = DT;
			if(DT.Rows[index]["refServico"].ToString().IndexOf("R",0,1) > -1) //se é uma reparação, nao quero alterar template nenhuma pq é sempre igual
			{
				return; 
			}
			
			Hashtable ht = HTIdCertificadoBySigla(); //veerificar se me lembro para que isto serve. 

			// AQUI VAI COPIAR 1 DOCUMENTO COM OU SEM SIMBOLO (ALTERACAO) DA PASTA ORIGEM BACKUP PARA A PASTA CONSTRUCAO
			DirectoryInfo dirInfoPastaBackupOrigem = new DirectoryInfo(pastaCertificadosOriginaisBackup); //agora vai à pasta backup buscar uma copia do ficheiro original
			
			FileInfo[] fi = dirInfoPastaBackupOrigem.GetFiles("*" + DT.Rows[index]["nomeDocumento"].ToString() + "*"); //em princip.so 1 ficheiro
				
			for (int i = 0; i < fi.GetLength(0); i++)
			{
				string sigla = strExtraiSigla(fi[i].ToString());

				if (!ht.ContainsKey(sigla))//verificar se ok!
				{
                    lblMessage.Text += "<br />" + GERAL.clsGeral.ErrorMessage.ERR_FILE_EXTENSION;
				}
				else
				{
					string caminhoDestino = pastaCertificadosFinaisConstrucao + "\\" + fi[i].ToString().ToUpper();
					string caminhoOrigem = pastaCertificadosOriginaisBackup + "\\" + fi[i].ToString().ToUpper(); //aqui a origem aponta para a dir backup... 
					string caminhoBackup = pastaCertificadosOriginaisBackup + "\\" + fi[i].ToString().ToUpper(); //este param dps n vai ser usado. 
					string strSimboloAcreditacao = DT.Rows[index]["simbolo"].ToString().TrimEnd() +".gif"; 
					string caminhoSimbolo = pastaSimbolos + "\\" + strSimboloAcreditacao;

                    //martelada de ultima hora (março 2010!!!!), nao ha neste momento tempo para fazer isto com BO etc
                    //basicamente, o que se pretende é que conforme os simboloso de acredtiacao, apareça
                    //um nome de laboratorio diferente no cabeçalo do documento. nao existe bo para isso
                    //e nao vai existir tao depressa, dada a urgencia, vai ser hardcoded. 
                    
                    //aqui vi, via debug, que o simbolo inclui o .gif....
                    iTextSharp.text.Font ARIAL_NORMAL_FONT = FontFactory.GetFont("Arial", 11);

                    iTextSharp.text.Chunk nomeLaboratorioHeader = new Chunk();
                    nomeLaboratorioHeader.Font = ARIAL_NORMAL_FONT;

					if(cb.Checked) //se está checked, tem simbolo
					{
                        //passou aqui para dentro
                        //aqui ja tem sempre simbolo e so pode mostrar ou nao mostrar, se mostra, definir o nome do lab
                        switch (strSimboloAcreditacao)
                        {
                            case "M0046.gif":
                                nomeLaboratorioHeader.Append("Laboratório de Calibração em Metrologia Física");
                                break;
                            case "L0268.gif":
                                nomeLaboratorioHeader.Append("Laboratório de Ensaios Físicos");
                                break;
                            case "M0059.gif":
                                nomeLaboratorioHeader.Append("Laboratório de Calibração em Metrologia Electro-Física");
                                break;
                            case "L0342.gif":
                                nomeLaboratorioHeader.Append("Laboratório de Ensaios de Controlo Dimensional");
                                break;
                            case "M0009.gif":
                                nomeLaboratorioHeader.Append("Laboratório de Metrologia Dimensional");
                                break;
                            case "L0610.gif":
                                nomeLaboratorioHeader.Append("Labmetro Saúde \n");
                                nomeLaboratorioHeader.Append("Laboratório de Metrologia Equipamento Clínico Hospitalar");            
                                break;
                            default:
                                //nomeLaboratorioHeader.Append("Laboratório de Metrologia"); 
                                nomeLaboratorioHeader.Append("");
                                break;
                        }


						if(DT.Rows[index]["refServico"].ToString().IndexOf("R",0,1) > -1) 
						{
							//nao faz nada pq a reparação n. tem simbolo.... isso devia ter sido tratado logo noutro sitio, disable da cb no caso de reparações. 
						}

						else//se não é uma reparação
						{
                            if ((DT.Rows[index]["refServico"].ToString().IndexOf("CACV", 0) > -1) && (DT.Rows[index]["equipamento"].ToString() == "DOSÍMETRO SOM") && ((Convert.IsDBNull(DT.Rows[index]["idSubtipoServico"])) || DT.Rows[index]["idSubtipoServico"].ToString() =="32"))						
							{
								lblMessage.Text += mergeFilesDosimetro(caminhoDestino, caminhoOrigem,  caminhoSimbolo, caminhoBackup, fi[i], true,nomeLaboratorioHeader);//true=bAlteracaoTemplate
							}
       //                     else if (DT.Rows[index]["refServico"].ToString().IndexOf("VMLF",0) > -1)
							//{
       //                         lblMessage.Text += mergeFilesVMLF(caminhoDestino, caminhoOrigem, caminhoSimbolo, caminhoBackup, fi[i], true,nomeLaboratorioHeader);//true=bAlteracaoTemplate
							//}

                            //Novembro 2011
                            //novo ECH pode ser nao acreditado ou acreditado
                            //se acreditado, nao leva o simbolo nem a template acreditada na ultima pagina


                            else if (DT.Rows[index]["refServico"].ToString().IndexOf("ECH",1) > -1) 
                                //se é acreditado, e quando chega aqui, é, entao nao leva o simolo + template ipac na ultima pagina
                                //o mergefilesvmlf faz isso --> substituido pelo mergefilesECH
                            {
                                   lblMessage.Text += mergeFilesECH(caminhoDestino, caminhoOrigem, caminhoSimbolo, caminhoBackup, fi[i], true, nomeLaboratorioHeader);//false=bAlteracaoTemplate
                            }
							else
							{
								lblMessage.Text += mergeFiles(caminhoDestino, caminhoOrigem, caminhoTemplateIPAC, caminhoSimbolo, caminhoBackup, fi[i], true,nomeLaboratorioHeader);//true=bAlteracaoTemplate
							}
						}
					}
					else //se não está checked quero mandar para as templates sem simbolo  
					{

                        //sem simbolo e nome do lab vaziao
                        
                        nomeLaboratorioHeader.Append("");


						if(DT.Rows[index]["refServico"].ToString().IndexOf("R",0,1) > -1) 
						{
							//nao faz nada 		
						}

						else//se não é uma reparação
						{
                            if((DT.Rows[index]["refServico"].ToString().IndexOf("CACV", 0) > -1) && (DT.Rows[index]["equipamento"].ToString() == "DOSÍMETRO SOM") && ((Convert.IsDBNull(DT.Rows[index]["idSubtipoServico"])) || DT.Rows[index]["idSubtipoServico"].ToString() =="32"))	
											
							{
								lblMessage.Text += mergeFilesDosimetro(caminhoDestino, caminhoOrigem,  null, caminhoBackup, fi[i], true,nomeLaboratorioHeader);//true=bAlteracaoTemplate
                                //verificar, pois parece-me que os dosimetros sao hardcoded e teem sempre o simbole e template
							}
                            else if (DT.Rows[index]["refServico"].ToString().IndexOf("VMLF",0) > -1)
							{
                                lblMessage.Text += mergeFilesVMLF(caminhoDestino, caminhoOrigem, null, caminhoBackup, fi[i], true,nomeLaboratorioHeader);//true=bAlteracaoTemplate
                                //verificar, pois parece-me que os dosimetros sao hardcoded e teem sempre o simbole e template
                                //nov. 2011- nao sei se nesta situação chega até aqui
							}
							else
							{
								lblMessage.Text += mergeFiles(caminhoDestino, caminhoOrigem, caminhoTemplate,null, caminhoBackup, fi[i],  true,nomeLaboratorioHeader);//true=bAlteracaoTemplate
							}
						}
					}	
				}			
			}
		}

		
		//=================================================================================
		/// Acção da checkbox para escolha de todos os registos
		/// Sempre que o utilizador click na checkbox a página retorna 
		///	um postback com a DataGrid correspondente à opção escolhida
		//=================================================================================
		private void cbTodos_CheckedChanged(object sender, System.EventArgs e)
		{
			BindGrid();
		}

        private void cbCertifsUser_CheckedChanged(object sender, System.EventArgs e)
		{
			BindGrid();
		}

       
		//para todas as extensoes 1C, 1R, 2R, 1A etc. não pode ser validado um novo documento se já existe
		//um certificado com esse tipo. 

		//tb não podem entrar novos certificados que estejam à espera de serem validados pelo resp.tecn.
		//se ainda não existe a extensão, podem entrar todos menos os que estão no estado TRES (

		//PODEM ENTRAR TODOS MENOS OS QUE ESTÃO EM ESTADO 3 (VALIDADO PELO TECNICO)

		//GUARDA O VALORE DAS CHECKBOXES NA DATATABLE**************
		private void SaveCheckBoxValuesToDT()
		{
			
			DT = (DataTable)ViewState["DT"];
			DV = new DataView(DT);
			DV.Sort = "idServico";	//para o find que vem a seguir. 
			//posso fazer o sort pela dataview mas o update à datatable

			//vai buscar os valodres das checkboxes
			foreach (DataGridItem dgi in dgDocumentos.Items)
			{
				int index = DV.Find(dgDocumentos.DataKeys[dgi.ItemIndex]);

				CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
				CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
				TextBox txtObs = (TextBox)dgi.Cells[0].FindControl("txtObservacoes");

				DT.Rows[index]["cbAprovar"] = cbAprovar.Checked.ToString();
				DT.Rows[index]["cbRejeitar"] = cbRejeitar.Checked.ToString();
				DT.Rows[index]["obsWorkflowCertificado"] = txtObs.Text;
				
			}

			DT.AcceptChanges(); //suponho que quando chamo o acceptchanges já tudo o que foi feito antes (rowstates) 
			//ficam limpos, verificar

			ViewState["DT"] = DT;
		}


		//===========================================================================================
		//ACCÇÃO INVOCADA PELA BOTÃO SUBMIT. VERIFICAR AINDA
		//===========================================================================================
		private void updateBD()
		{

			//CÓDIGO PARA ALTERAR ESTADO DOS SERVIÇOS APROVADOS
			string strIds = "";
			string strIdsApagar = "";
			string strEstActual = ""; 
			string strEstNovo = "";
			string strtCert = "";
			

			DT = (DataTable)ViewState["DT"];
			
			string sSearch = "cbRejeitar = True"; //ele nao apanhar isto aqui... converter para algo!
			DataView DVR = new DataView(DT);
			DVR.RowFilter=sSearch;


			sSearch = "cbAprovar = True"; //ele nao apanhar isto aqui... converter para algo!
			DataView DVA = new DataView(DT);
			DVA.RowFilter=sSearch;

			//preciso de fazer aqui os search pq preciso de saber o tamanho de ambos para o array
			string[] obsWorkflow = new string[DVR.Count+DVA.Count]; //tem de ter o tamanho dos rejeitados e aprovados
			int n = 0; 

			foreach (DataRowView drvR in DVR) //LOOP
			{

			
				//O idestadoCertificado aqui já foi alterado
				//eu quero saber qual o idEstadoCertificado que veio originalmente da bd. 
				//mas quando faço acceptchanges, estes dados desaparecem, --
				//por isso, para cada datarow, tenho de guardar a info de qual o estado em veio.
				//eu faço sempre acceptchanges a) pq a situação de ter necessidade de visualizer os
				//orignalrows so surgio à ultima da hora e pq ao fazer acceptchanges corto a informação 
				//desnecessaria para nao sobrecarregar a DT em viewstate

				strIds +=  drvR.Row["idServico"].ToString() + ","; //ids dos serviços
				strEstActual += drvR.Row["idEstadoCertificado"].ToString() +","; 
				strEstNovo += 5 + ",";   //AQUI MUDA O ESTADO PARA 5 : REJEITADO PELO TÉCNICO
				strtCert += drvR.Row["idTipoCertificado"].ToString() + ",";
				obsWorkflow[n] = drvR.Row["obsWorkflowCertificado"].ToString();
				n+=1; 
			}
		
			//os arrays continuam a creser

			foreach (DataRowView drvA in DVA) //LOOP
			{
				strIds +=  drvA.Row["idServico"].ToString() + ","; //ids dos serviços
				strIdsApagar +=  drvA.Row["idServico"].ToString() + ",";
				strEstActual += drvA.Row["idEstadoCertificado"].ToString() +","; 
				strEstNovo += 3 + ",";  //AQUI MUDA O ESTADO PARA 3 : CALIBRADO COM VALIDAÇÃO
				strtCert += drvA.Row["idTipoCertificado"].ToString() + ",";
				obsWorkflow[n] = drvA.Row["obsWorkflowCertificado"].ToString();
				n+=1; 
			}

			char[] delimiter =  ",".ToCharArray();

			strIds = strIds.TrimEnd(delimiter);
			strIdsApagar = strIdsApagar.TrimEnd(delimiter);
			strEstActual = strEstActual.TrimEnd(delimiter);
			strEstNovo = strEstNovo.TrimEnd(delimiter);
			strtCert = strtCert.TrimEnd(delimiter);

			string[] idsServicos = strIds.Split(delimiter);
			string[] idsEstadosActuais = strEstActual.Split(delimiter);
			string[] idsEstadosNovos = strEstNovo.Split(delimiter);
			string[] idsTipoCertificado = strtCert.Split(delimiter);
			//obs ja está em array desde sempre....

			DATA.EstadoCertificadoBD estadoCertBD = new LabMetro.DATA.EstadoCertificadoBD();

			if(!estadoCertBD.UpdateEstadosCertificados(idsServicos,idsEstadosActuais,idsEstadosNovos, User.Identity.Name.ToString(), obsWorkflow, idsTipoCertificado))
			{
                lblMessage.Text += GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
			}
			else
			{
				//APAGA OS REGISTOS DA PARSTA ORIGINAIS BACKUP quando se aceita o documento
				if (strIdsApagar.ToString() != "")
				{
					deleteDocs(strIdsApagar); //idsServicosApagar);
				}
				Server.Transfer("GestDocs.aspx"); 
			}

		}


		#region FUNÇÕES SOBRE O DATAGRID
		
		//*******************************************************************************************
		//*******************************************************************************************
		//****************************FUNÇÕES SOBRE O DATAGRID **************************************
		//*******************************************************************************************
		//*******************************************************************************************

		//==================================================================================
		/// ITEMDATABOUND DO DGDOCUMENTOS
		//==================================================================================
		public void dgDocumentos_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			
			DataRowView DRV = (DataRowView) e.Item.DataItem;
			
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{

				LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];
				CheckBox cbAprovar = (CheckBox)e.Item.FindControl("cbAprovar");
				CheckBox cbRejeitar = (CheckBox)e.Item.FindControl("cbRejeitar");
				CheckBox cbTemplate = (CheckBox)e.Item.FindControl("cbTemplate"); 
				
				TextBox txtObs = (TextBox)e.Item.FindControl("txtObservacoes");
				txtObs.ToolTip = txtObs.Text.ToString();

				//novo, se não ha simbolo, cbtemplate sempre disabled
				if(DRV["simbolo"].ToString() =="") cbTemplate.Enabled=false; 

				//--------------------------------------------------------------------------------
				//independentemente de todo o resto, os anulados ficam sempre vermelhos e disabled
				//e os que não têm documento associado também. 
				//--------------------------------------------------------------------------------
                if (DRV["idEstadoCertificado"].ToString() == "5" || DRV["idEstadoCertificado"].ToString() == "6" || DRV["nomeFichCalibracao"].ToString() == MSG_PDF_INEXISTENTE || DRV["nomeFichCalibracao"].ToString() == MSG_PDF_CRIARNOVO || DRV["nomeFichCalibracao"].ToString() == MSG_PDF_CORROMPIDO) //diferente de rejeitado e diferentes de sem documentos....)
				{
					for (int i = 1; i < e.Item.Cells.Count; i++)
					{
						e.Item.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA380");
					}
					
					//e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA380"); isto so funciona para aqueles que 
					//já la estavam antes de eu ter adicionado novas colunas. bug? feature?
					cbAprovar.Enabled = false;
					cbRejeitar.Enabled = false;
					cbTemplate.Enabled = false;
					txtObs.Enabled=false;
				}
					//---------------------------------------------------------------------
					//independentemente de todo o resto, os serviços de outro user ficam sempre amarelos
					//a não ser que estejam vermelhos
					//---------------------------------------------------------------------
				else if(DRV["belongsToUser"].ToString() == "False") //nao tirar o else
				{
					for (int i = 1; i < e.Item.Cells.Count; i++)
					{
						e.Item.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE5BF");
					}
				}

				
				//if(DRV["idEstadoCertificado"].ToString() !="5" && DRV["idEstadoCertificado"].ToString() !="6" && DRV["idEstadoCertificado"].ToString() !="1")
				if(DRV["idEstadoCertificado"].ToString() !="1") //podem visualizar os rejeitados tb
				{
					//colocar o link em todos as celulas da linha (para abrir o doc)
					for (int i = 1; i < e.Item.Cells.Count; i++)
					{
						if(!e.Item.Cells[i].HasControls()) //para nao pôr o link nas cells que conteem checkboxes
						{							
							e.Item.Cells[i].ToolTip = "Click para visualisar o documento. ";// + e.Item.Cells[5].Text;
							e.Item.Cells[i].Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(button, ""));
						}
					}
	
				}
				
			}
		}
		
		
		private int toIntFichTipo(string fich)
		{
			if(fich =="") 
			{
				return 0; 
			}
			else
			{
				try
				{
					return System.Convert.ToInt16(fich); 
				}
				catch
				{
					return 0; 
				}
			}
		}
		//================================================================================
		//SORTGRID
		//================================================================================
		public void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{
			SaveCheckBoxValuesToDT(); 
			switch (ViewState["sortDirection"].ToString())
			{
				case "ASC":
					ViewState["sortDirection"] = "DESC";
					break;
				case "DESC":
					ViewState["sortDirection"] = "ASC";
					break;
			}

			ViewState["sortField"] = e.SortExpression;
			BindGrid(); 
		}

		#endregion

		# region ACÇÕES DOS BOTÕES
		//*******************************************************************************************
		//*******************************************************************************************
		//****************************ACÇÕES DOS BOTÕES *********************************************
		//*******************************************************************************************
		//*******************************************************************************************

		//================================================================================
		/// BOTÃO DE CHECK ALL (para aprovar os documentos)
		//================================================================================
		private void btnAprovarAll_Click(object sender, System.EventArgs e)
		{
			allEvento("aprovar");	
		}

		//================================================================================
		//BOTÃO DE CHECK ALL (para rejeitar os documentos)
		//================================================================================
		private void btnRejeitarAll_Click(object sender, System.EventArgs e)
		{
			allEvento("rejeitar");	
		}

		//================================================================================
		// BOTÃO DESELECT ALL - quando o utilizador pretende 
		// deseleccionar as selecções de aprovar ou anular
		//================================================================================
		private void btnDeselectAll_Click(object sender, System.EventArgs e)
		{
			allEvento("limpar"); 	
		}


		//=======================================================================================
		/// Função que executa as acções correspondentes aos botões de acção All
		//=======================================================================================
		private void allEvento(string accao) 
		{

			string sGrau = strGrau(); //para nao entrar em n loops abaixo.

			string[] aGrandezas = sGrandezasUserResp(); //para so chamar 1 vez

			DT = (DataTable)ViewState["DT"];
			DV = new DataView(DT);

//			dg1.DataSource = DV;
//			dg1.DataBind(); 

			bool bAprovar =false; //se é false, signfica que é para rejeitar (n. posso inicializar a null) 
			bool bLimpar = false;

			switch(accao)
			{
				case"aprovar": 
					bAprovar = true;
					break;
				case "rejeitar": 
					bAprovar = false; 
					break;
				case "limpar": 
					bLimpar = true;
					break;
			}
			
			
			foreach (DataGridItem dgi in dgDocumentos.Items)
			{
				DataRow[] dRow = DT.Select("idServico ="+dgi.Cells[1].Text.ToString()); 
				
				//DataRowView DRV = dgi.DataItem; //aqui n funcionar por isso tenho de aceder aos campos invisiveis em vez de aceder à datasource do datagrid. 
				
				CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
				CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
				TextBox txtObs = (TextBox)dgi.Cells[0].FindControl("txtObservacoes");
				CheckBox cbTemplate = (CheckBox)dgi.Cells[0].FindControl("cbTemplate");				


				//---------------------------------------------------------------------------------
				//primeiro avaliar os enabled e os disabled e depois verificar se esta parte é mesmo necessária aqui
				//---------------------------------------------------------------------------------
				//vou fazer por partes para ser de mais facil leitura. 
				//1. se serviços REJEITADOS, tudo disabled para toda a gente
				if(dgi.Cells[9].Text == "5" || dgi.Cells[9].Text == "6" || dgi.Cells[9].Text == "1" || dgi.Cells[15].Text=="...") //REJEITADOS SEMPRE DISABLED e inexistentes tb
				{
						cbAprovar.Enabled=false;
						cbRejeitar.Enabled=false;
						cbTemplate.Enabled=false;
						txtObs.Enabled=false;
						continue; 
				}
					
				//2. tem se ser gestor de Certif. ou responsavel pela grandeza ou técnico (nesse caso pode aceitar todos os que lhe aparecem,  independentemente de serem dele ou nao, pois ja aparecem filtrados à partida

				//aqui havia um else
				//if( sGrau =="0" || bGrandezaRT(dgi.Cells[1].Text) || Session["idPerfil"].ToString() =="6") 
				//nao consigo p^^or a grandeza para dentro do item, desisto e uso substring.... 
				if( sGrau =="0" || Array.IndexOf(aGrandezas,dgi.Cells[20].Text.Substring(1,3))>-1 || Session["idPerfil"].ToString() =="6") 
				{
					//Response.Write(Array.IndexOf(aGrandezas,dgi.Cells[20].Text.Substring(1,3)).ToString()); 

					cbAprovar.Enabled = true;
					cbRejeitar.Enabled = true;
					txtObs.Enabled=true;
					if (cbTemplate.Checked) cbTemplate.Enabled = true;

					//---------------------------------------------------------------------------------
					//DEPOIS AVALIAR OS QUE FICAM APROVADOS, REJEITADOS OU LIMPOS	
					//E SO NAS CONDIÇÕES DESTE IF 
					//---------------------------------------------------------------------------------
					if(bLimpar) //se é para limpar, ficam todos limpos
					{
						cbAprovar.Checked=false;
						cbRejeitar.Checked=false;
						foreach (DataRow dr in dRow) //actualizar a datasource logo ao mesmo tempo
						{
							dr["cbAprovar"] = false;
							dr["cbRejeitar"] = false;
						}
					}
					else//uma das outras duas condições há de ser true ou false
					{
						cbAprovar.Checked= bAprovar; 
						cbRejeitar.Checked= !bAprovar;
						foreach (DataRow dr in dRow) //actualizar a datasource logo ao mesmo tempo
						{
							dr["cbAprovar"] = bAprovar;
							dr["cbRejeitar"] = !bAprovar;
						}
					}
				}
			}
			ViewState["DT"] = DT;
		}



		//================================================================================
		// CLICK DO BOTÃO DE PESQUISA
		//================================================================================
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			DT = (DataTable)ViewState["DT"]; 
			DV = new DataView(DT); //dt está sempre a ser carregada no pageload
			
			string strSearch ="idServico is not null "; //para ter alguma coisa aqui...
			if(txtSearchEmpresa.Text !="") strSearch += " AND empresa LIKE '%"+txtSearchEmpresa.Text+"%'"; 
			if(txtSearchNServico.Text !="") strSearch += " AND refServico LIKE '%"+txtSearchNServico.Text+"%'"; 
			if(ddGrandeza.SelectedValue !="") strSearch += " AND idGrandezaServico = '"+ddGrandeza.SelectedValue+"'"; 
			if(cbTodos.Visible==false)
			{
				if(Session["idPerfil"].ToString() == "6") //partindo do principio que nenhum TL será jamais 
					//"gestor de certificados";
				{
					strSearch += " AND belongsToUser = 1 "; 
				}
			}

			DV.RowFilter=strSearch;

			dgDocumentos.DataSource = DV;
			dgDocumentos.DataBind(); 

		}

		//================================================================================
		//CLICK DO BOTÃO PARA LIMPAR OS CAMPOS DE PESQUISA
		//================================================================================
		private void btnLimparCampos_Click(object sender, System.EventArgs e)
		{
			LimpaCamposPesquisa();
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


		//================================================================================
		//BUTTON SUBMIT DA PÁGINA
		//================================================================================
		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			SaveCheckBoxValuesToDT(); 
			if(VerificaCheckBoxes()==false)
			{
				lblMessage.Text ="Verifique as checkboxes, não pode aprovar e rejeitar um certificado ao mesmo tempo."; 
				return; 
			}
			else
			{
				if(validacaoRegistos()) //futuramenet juntar com a funcao anterior.
				{
					updateBD();
				}
				else
				{
					lblMessage.Text ="Não escolheu nenhum Certificado."; 
				}
			}
		}
		#endregion

		#region FUNÇÕES MENORES

		//*******************************************************************************************
		//*******************************************************************************************
		//****************************FUNÇÕES MENORES ********************************************
		//*******************************************************************************************
		//*******************************************************************************************


		//================================================================================== 
		// guarda em viewstate a grandeza do utilizador registado. 
		//================================================================================== 
		private void getGrandezaUtilizadorLogado()
		{
			DATA.UtilizadoresBD user = new LabMetro.DATA.UtilizadoresBD();			
			Session["idGrandezaUtilizador"] = user.idGrandezeUser(User.Identity.Name.ToString());
			user = null; 
		}
		//verificar
		//================================================================================== 
		// Função para verificar se o serviço a validar é de uma das grandezas 
		// associadas ao responsável técnico (o segudno parametro (id do utilizador logado, 
		// e associado na funcaob GrandezaRT e vem da sessao. 
		//==================================================================================
		private bool bGrandezaRT(string idServico) 
		{
			DATA.EstadoCertificadoBD g = new LabMetro.DATA.EstadoCertificadoBD();
			bool b = g.bGrandezaRT(idServico);
			g = null;
			return b;
		}

		//array com as grandezas do user logado. 
		private string[] sGrandezasUserResp() //as grandezas dos users logados que sao tb responsáveis pelas grandezas, dos técncicos nao... 
		{
		
			
			DATA.EstadoCertificadoBD certificadoBD = new LabMetro.DATA.EstadoCertificadoBD(); 
			DataTable DT = certificadoBD.DTListGrandezas(User.Identity.Name.ToString(), "");
			string[] s = new string[DT.Rows.Count]; 
			int i = 0; 
			foreach (DataRow dr in DT.Rows)
			{								
				s[i]  = dr["idGrandeza"].ToString();  
				i+=1; 
			}		
			
			certificadoBD = null; 

			return s; //array; 
			
		}
	


		//isto existia em array, mas eu vou mandar em string, assim dps faço um find à string
		//uma vez em vez de um loop atraves do array...
		//devolve uma string com todas as grandezas do RT ou TL logado.
		private string strGrandezasRespLogado()
		{
			string sGrandezas="";
			
			DATA.EstadoCertificadoBD certificadoBD = new LabMetro.DATA.EstadoCertificadoBD(); 
			SqlDataReader DR = certificadoBD.DRListGrandezas(User.Identity.Name.ToString(), "");

			while (DR.Read())
			{
				sGrandezas +=  DR["idGrandeza"].ToString() + ",";
			}	
				
			DR.Close();
			certificadoBD = null; 

			return sGrandezas.TrimEnd(",".ToCharArray()); //virgula fica,pois nao incomoda
        }

		//================================================================================
		//CLICK DO BOTÃO UPLOAD //carrega um ficheiro
		//================================================================================

  //      protected void btnUpload_Click(object sender, System.EventArgs e)
  //      {
  //          uploadFile();
  //      }
        //=========================================================================
        // Função que retorna ou o user logado ou sem selecção de user
        // conforme o utilizador tenha ou não ligado a checkbox de visualizar todos
        //=========================================================================
        private string nomeUtilizador()
		{
			if (!cbTodos.Checked && strGrau() != "0")
			{
				return User.Identity.Name.ToString();
			}
			else
			{
				return "";
			}
		}

		//==============================================================================
		//recebe nome de ficheiro e extrai a sigla (1C, 1R ...)
		//==============================================================================
		public string strExtraiSigla(string nomeDocumento)
		{
			try
			{
				string[] docIn = nomeDocumento.Split("-,.".ToCharArray()); 
				string sigla = docIn[2];

				return sigla;
			}
			catch
			{
				return "";
			}

		}
		//=========================================================================================
		// Função que permite desabilitar todos os comandos usados pelo utilizador nas
		// situações em que é necessário corrigir alguma anomalia no workflow
		//=========================================================================================

		//================================================================================
		// LIMPA OS CAMPOS DE PESQUISA
		//================================================================================
		private void LimpaCamposPesquisa()
		{
			txtSearchNServico.Text = "";
			ddGrandeza.SelectedIndex=0; 
			txtSearchEmpresa.Text = "";
		}

		//================================================================================
		//Função usada na conversão de boolean para bit para alterar a tabela de acreditação em SQL
		//================================================================================
		public static string convertBoolToBit(bool x)
		{
			if (x == true)
			{
				return "1";
			}
			return "0";
		}

		//==================================================================================
		// Função que retorna o caminho completo de um ficheiro (parametro
		//==================================================================================
		public string downloadpath(object filename)
		{
			if (filename != null && filename.ToString() != "")
			{
				string myPath = (string)ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CONSTRUCAO"];
				myPath = myPath + "/" + filename.ToString();
				return myPath;
			}
			return "#";
		}

		//==================================================================================
		// Função que permite visualizar o documento pretendido pelo utilizador
		//==================================================================================
		public void visualisarDocumento(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName.ToString() == "Select")
			{
				string doc = e.Item.Cells[20].Text; //campo nomeDocumento
				string nome = downloadpath(doc);
				//Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");

                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language=JavaScript>");
                strScript.Append("window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');");
                strScript.Append("</script>");
                //RegisterClientScriptBlock("imprimefactura", strScript.ToString());
                ClientScript.RegisterClientScriptBlock(GetType(), "mostraCertif", strScript.ToString());
			}
		}

		//==================================================================================
		// Retorna o perfil do utilizador (antes era uma funciona
		//==================================================================================
		public string strPerfil()
		{
			//está na session
			return Session["idPerfil"].ToString(); 
		}


		//==================================================================================
		// Retorna o grau de hierarquia do utilizador //se é 0 é que interessas (merdas...)
		//==================================================================================
		public string strGrau()
		{
			if(ViewState["strGrau"]==null || ViewState["strGrau"].ToString()=="")
			{
				DATA.EstadoCertificadoBD gr = new LabMetro.DATA.EstadoCertificadoBD();
				ViewState["strGrau"] = gr.strGrauUtilizador();
				gr = null;
			}

			return ViewState["strGrau"].ToString(); 
		}


		//================================================================================== 
		//carrega numa hashtable sigla / idTipoCertificado
		//depois, em vez de num loop ir à bd busca-los um por um, vai ler à hashtable
		//isso nos loops do  createDTServicos
		//==================================================================================
		public static Hashtable HTIdCertificadoBySigla() //para nao ir tantas vezes à bd //pode ser public static, igual para todos
		{
			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
			SqlDataReader DR = data.DRTipoCertificado();

			Hashtable ht = new Hashtable();

			if (DR.HasRows)
			{
				while (DR.Read())
				{
					ht.Add(DR["sigla"], DR["idTipoCertificado"]);
				}
			}

			DR.Close();
			data = null;
			return ht;
		}
		//================================================================================== 
		//carrega numa hashtable sigla / idTipoCertificado
		//depois, em vez de num loop ir à bd busca-los um por um, vai ler à hashtable
		//isso nos loops do  createDTServicos
		//==================================================================================
		public static Hashtable HTDescCertificadoBySigla() //para nao ir tantas vezes à bd //pode ser public static, igual para todos
		{
			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
			SqlDataReader DR = data.DRTipoCertificado();

			Hashtable ht = new Hashtable();

			if (DR.HasRows)
			{
				while (DR.Read())
				{
					ht.Add(DR["sigla"], DR["tipoCertificado"]);
				}
			}

			DR.Close();
			data = null;
			return ht;
		}

		//================================================================================== 
		//carrega numa hashtable sigla / idTipoCertificado
		//depois, em vez de num loop ir à bd busca-los um por um, vai ler à hashtable
		//isso nos loops do  createDTServicos
		//==================================================================================
		public static Hashtable HTDescCertificadoIdTipo() //para nao ir tantas vezes à bd //pode ser public static, igual para todos
		{
			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
			SqlDataReader DR = data.DRTipoCertificado();

			Hashtable ht = new Hashtable();

			if (DR.HasRows)
			{
				while (DR.Read())
				{
					ht.Add(DR["idTipoCertificado"].ToString(), DR["tipoCertificado"]);
				}
			}

			DR.Close();
			data = null;
			return ht;
		}

//		//================================================================================== 
		//comentado dm 03-10-2008
//		//carrega numa hashtable idServico / (b)acreditado
//		//depois, em vez de num loop ir à bd busca-los um por um, vai ler à hashtable
//		//isso nos loops do  createDTServicos
//		//==================================================================================
//		private Hashtable HTAcreditadoByIdServico() // tem de ser private pq pode ser diferente para cada user, os dados q la foram carregados
//		{
//
//			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
//			SqlDataReader DR = data.DRGetCertTemplate();
//
//			Hashtable ht = new Hashtable();
//
//			if (DR.HasRows)
//			{
//				while (DR.Read())
//				{
//					ht.Add(DR["idServico"], DR["acreditado"]);
//				}
//			}
//			DR.Close();
//			data = null;
//			return ht;
//		}

		//================================================================================== 
//		//carrega numa hashtable idServico / (b)acreditado DA TABELA AUXILIAR!!!!!
//		//depois, em vez de num loop ir à bd busca-los um por um, vai ler à hashtable
//		//isso nos loops do  createDTServicos
//		//==================================================================================
//		public static Hashtable HTAcreditadoFromAuxTable() //IDEM
//		{
//			DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
//			SqlDataReader DR = data.DRGetCertTemplate();
//
//			Hashtable ht = new Hashtable();
//
//			if (DR.HasRows)
//			{
//				while (DR.Read())
//				{
//					ht.Add(DR["idServico"], DR["acreditado"]);
//				}
//			}
//
//			DR.Close();
//			data = null;
//
//			return ht;
//		}

		//==================================================================================
		// Javascript para as janelas de confirmação da opção do utilizador (RUI PRISMA)
		//==================================================================================
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
			if (ClientScript.IsClientScriptBlockRegistered("ConfirmarEscolha"))
			{
				ClientScript.RegisterClientScriptBlock(GetType(), "ConfirmarEscolha", js);
			}
		}

		//==================================================================================
		////FUNCAO QUE PERMITE FAZER UPLOAD DE FICHEIROS
		////==================================================================================
		//private void uploadFile()
		//{
		//	if (fileIn.PostedFile.FileName != "")
		//	{
		//		string strFileName;
		//		try
		//		{
		//			strFileName = System.IO.Path.GetFileName(fileIn.PostedFile.FileName).ToUpper();

		//			string path = (string)ConfigurationManager.AppSettings["PATHREL_CERT_ORIGINAIS"];
		//			string myPath = Server.MapPath("~/" + path);

  //                  //tirei isto 14/11/2017
  //                  //if (File.Exists(myPath + "/" + strFileName))
  //                  //{
  //                  //    lblMessage.Text += "<br />" + GERAL.clsGeral.ErrorMessage.ERR_JA_EXISTE_FICHEIRO_NOME;
  //                  //}
  //                  //else
  //                  //{
		//				fileIn.PostedFile.SaveAs(myPath + "/" + strFileName);

		//				lblMessage.Text += "<br />"+clsGeral.ErrorMessage.MSG_DOCS_UPLOAD;
		//				lblMessage.Text += clsGeral.ErrorMessage.MSG_DOCS_ALERTA_REFRESH;
		//			//}
		//		}
		//		catch (Exception ex)
		//		{
		//			GERAL.clsWriteError.WriteLog("Erro no carregamento de ficheiros." + ex.ToString());
  //                  lblMessage.Text += "<br />" + GERAL.clsGeral.ErrorMessage.ERR_CARREGAMENTO_FICHEIRO;
		//		}
		//	}
		//}



        //upload multiplo
        protected void btnupload_Click(object sender, EventArgs e)
        {


            string path = (string)ConfigurationManager.AppSettings["PATHREL_CERT_ORIGINAIS"];
            //string path = (string)System.Configuration.ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_PATH_REL"];
            //string savepath = Server.MapPath("~/" + path);
            string myPath = Server.MapPath("~/" + path);


            for (int i = 0; i < Request.Files.Count; i++)
            {
                //In IE8, this behavior has changed and it will ONLY pass the file name, not the full path. ;-)
                //em ie, ele vai buscar o camimho todo ho hpf.filename e logo dá erro a seguir
                //mesmo assim o upload multiplo nao funciona em ie.
                HttpPostedFile hpf = Request.Files[i];
                string filename = hpf.FileName.ToUpper();
                string filename2 = Path.GetFileName(hpf.FileName).ToUpper();

                try
                {
                    hpf.SaveAs(myPath + "/" + filename2);
                    //Response.Write(filename2 + " - OK" + "<br />");
                }
                catch (Exception ex)
                {

                    Response.Write("Error a carregar el fichero " + myPath + "/" + filename2 + ". " + ex.ToString());
                    clsWriteError.WriteLog(ex.ToString());
                }

            }

            BindGrid();
            //todos os ficheiros vao parar para uma pasta temporaria
            // a partir dai vou correr uma funcao que copia todos os certificados para a pasta final e que no meio disto actualiza a bd.
            //se é um 1c, 1R, 

        }
        //===============================================================================================
        //FILL DA DROPDOWN GRANDEZAS
        //===============================================================================================
        private void fillDDGrandeza()
		{
			string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
			SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			
			ddGrandeza.DataSource = dr;
			ddGrandeza.DataBind(); 
			ddGrandeza.Items.Insert(0,new System.Web.UI.WebControls.ListItem("","")); 
			
			dr.Close(); 
			
		}

		//===============================================================================================
		//TRUE SE FICHEIRO TEM UMA NOMENCLATURA CORRECTA; FALSE SE NÃO TEM
		//===============================================================================================
		bool isValidFileName(string strIn)
		{
			// Return true if strIn is in valid  format.
			//calibracao, aditamento, revisao, via
			//qq carcater depois 2 numeros um traco um numero e um caracatcer (c,a,r,v) e ponto pdf
			string myPattern = ".*\\-[0-9]{2}\\-[0-9]{1}[c|C|a|A|r|R|v|V]{1}\\.[pP][dD][fF]"; 
			return Regex.IsMatch(strIn,myPattern); 
		}


		//===============================================================================================
		//TRUE SE FICHEIRO É UM PDF; FALSE SE NÃO É
		//===============================================================================================
		bool isPdf(string strIn)
		{
			// Return true if strIn is in valid  format.
			string myPattern = "@^.+\\.(pdf)";
			return Regex.IsMatch(strIn,myPattern,RegexOptions.IgnoreCase);
		}


		//===============================================================================================
		//cria uma string com as refs calibração dos ficheiros que foram colocados na pasta ORIGEM
		//===============================================================================================
		private string stringDocs()
		{
			string strDocs =""; 

			DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOriginais);
			FileInfo[] files = dirInfoPastaOrigem.GetFiles();
			
			foreach(FileInfo f in files)
			{
				if(!f.Name.EndsWith("1C.pdf")) //todos os que são diferentes de 1C
				{
					string doc = f.Name;
					if(!(doc.Length <11 || doc.IndexOf("-")==-1)) //se tem menos de 11 caract, nao está bem.
					{
						doc = doc.Replace("-","/"); 
						doc = doc.Substring(0, doc.Length-7); 
						//strDocs+="'"+doc+"',";  //mudar isto para a nova funcao de sql, nao leva as plicas
						strDocs+=doc+","; 

					}
				}
			}

			strDocs = strDocs.TrimEnd(",".ToCharArray());			
			return strDocs; 

		}

		#endregion	

        //=================================================================================================
        //=================================================================================================
        protected string ConverteEstado(bool b)
        {
            if (b == true) return "CONCLUSIVO";
            else return "---";
        }
	}
}

