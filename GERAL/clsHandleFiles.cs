using System;
using System.IO;
using System.Data; 
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections; 

//classes a user: File e FileInfo dentro do System.IO
//File: Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects.
//FileInfo:Provides instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects.

using System.Configuration; 
using System.Text;

namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for clsHandleFiles.
	/// </summary>
	/// 

	//tirado daqui : http://www.shanebauer.com/Weblog/PermaLink,guid,b9cd286d-e147-4a25-aa49-bde888a11433.aspx

	//Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.
	//return values:
//	Less than zero x is less than y. 
//	Zero x equals y. 
//	Greater than zero x is greater than y. 

	public class CompareFileInfo: IComparer
	{
		public int Compare(object x, object y)
		{
			FileInfo file = (FileInfo)x;
			FileInfo file2 = (FileInfo)y;
			return DateTime.Compare(file2.CreationTime,file.CreationTime); //aqui tenho de pôr o parametro que me interessa. 
		}

		//inacabado
//		public int CompareFileInfo(object x, object y)
//		{
//			FileInfo file = (FileInfo)x;
//			string strServico = (string)y; 
//			//Remove .pdf Extention 
//			int ix = file.FullName.LastIndexOf(@".pdf"); 
//			string fName = file.FullName.Substring(0,ix); 
//			
//			return String.Compare(fileName.CreationTime, file2.CreationTime); //aqui tenho de pôr o parametro que me interessa. 
//		}
		
	}


	public class clsHandleFiles
	{

		private string FileName;

		public clsHandleFiles()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//===========================================================================
		//tem de dentro do schema.ini uma definição para CADA ficheiro de texto
		//tenho de apagar o schema.ini existente e criar um novo antes de ler os ficheiros
		//===========================================================================
		public static void WriteSchemaIni()
		{
			//nome do ficheiro
			string strFileName= "Schema.ini";
			string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_PATH_REL_DM"];     
			string file = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName); 
			
			StreamWriter sw = new StreamWriter(file,false,System.Text.Encoding.GetEncoding(1252)); //false na segunda posição faz overwrite ao ficheiro já existente

			string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro); 
			
			DirectoryInfo dir = new DirectoryInfo(path); 
			FileInfo[] files = dir.GetFiles("*.txt");
			Array.Sort(files, new CompareFileInfo()); 
			
			foreach(FileInfo f in files)
			{ 
				//isto tem de ser feito para cada ficheiro existente na directoria

				// Use the MaxScanRows option to indicate how many rows should be scanned when determining the column types. If you set MaxScanRows to 0, the entire file is scanned. The MaxScanRows setting in Schema.ini overrides the setting in the Windows Registry on a file-by-file basis.

//The ColNameHeader indicates that Microsoft Jet should use the data in the first row of the table to determine field names and should examine the entire file to determine the data types used:


				sw.WriteLine("["+f.Name+"]"); 
				sw.WriteLine("ColNameHeader=FALSE"); 
				sw.WriteLine("Format=TabDelimited"); 
				//sw.WriteLine("MaxScanRows=0");
				sw.WriteLine("CharacterSet=ANSI");
				sw.WriteLine("Col1=codigoClienteSAP Char Width 10");
				sw.WriteLine("Col2=grupoContas Char Width 4 ");
				sw.WriteLine("Col3=sectorActividade Char Width 2");
				sw.WriteLine("Col4=canalDistribuicao Char Width 2");
				sw.WriteLine("Col5=nome1 Char Width 40");
				sw.WriteLine("Col6=nome2 Char Width 40");
				sw.WriteLine("Col7=conceitoPesquisa Char Width 20");
				sw.WriteLine("Col8=rua1 Char Width 60");
				sw.WriteLine("Col9=rua2 Char Width 40 ");
				sw.WriteLine("Col10=codigoPostal Char Width 10");
				sw.WriteLine("Col11=localidade Char Width 40");
				sw.WriteLine("Col12=pais Char Width 3");
				sw.WriteLine("Col13=regiao Char Width 3");
				sw.WriteLine("Col14=idioma Char Width 1");
				sw.WriteLine("Col15=telefone Char Width 30");
				sw.WriteLine("Col16=fax Char Width 30");
				sw.WriteLine("Col17=email Char Width 241");
				sw.WriteLine("Col18=url Char Width 132");
				sw.WriteLine("Col19=nif Char Width 20");
				sw.WriteLine("Col20=sede Char Width 10");
				sw.WriteLine("Col21=numAntigo Char Width 10");
				sw.WriteLine("Col22=grupoClientes Char Width 2");
				sw.WriteLine("Col23=codBloqueio Char Width 2");
				sw.WriteLine();
			}
			sw.Close(); 
		}

		//aqui tenho de:
		//1.ir buscar as empresas que não existem ainda na tabela sap_empresas e inseri-las nessa mesma tabela
		//2.depois tenho de fazer update ao campo codigobloqueio, tanto na tabela sap_empresas como na tabela Empresas
		//3.depois tenho de mover o ficheiro ja lido para dentro da pasta dos ficheiros ja lidos
		//so preciso de saber se os dados das empresas sap teem pais e filhos, se me posso reger pelo nif, codCliente, ou whatever...ha um codCliente por nif ou pode haver varios?

		//a chave na tabela sap_empresas é: codigoClienteSAP + canalDistribuicao
		
		public void UpdateEmpresasSAP()
		{
		
			string pathFicheiro = (string)ConfigurationManager.AppSettings["SAP_PATH_REL_DM"];     
			string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro); 
			
			string pathMoveTo = (string)ConfigurationManager.AppSettings["SAP_LIDOS_PATH_REL"];     

			DirectoryInfo dir = new DirectoryInfo(path); 
			FileInfo[] files = dir.GetFiles("*.txt");
			Array.Sort(files, new CompareFileInfo());

			string fName; 
			string strSQL; 
			string fNameNovo; 

			DataSet ds = new DataSet(); 

			string connectionString = (string )ConfigurationManager.AppSettings["csvConnSAP"];
    
			using (OleDbConnection objConn = new OleDbConnection(connectionString)) 
			using (OleDbCommand objCmd = new OleDbCommand())
			{
				objCmd.Connection =objConn; 				
				objConn.Open(); 

				OleDbDataAdapter objDA = new OleDbDataAdapter(objCmd);

				using (OleDbTransaction objTrans = objConn.BeginTransaction())
				{
					try
					{
						foreach(FileInfo f in files)
						{	 
							//2020: se os valores dos campos conteem " na string, entao vai dar erro na importacao
                            //no fill do dataset, alias, esses campos nao serio preenchidos no dataset sendo que normalmente quandos e chega ao fim da linha, o codigo de bloqueia fica vazio.
                            //CORRIGIR COM TEMPO - NAO ACONTECE MUITAS VEZES!


							fName = f.Name; 
							strSQL = "SELECT  *, '" + fName +"' as Ficheiro FROM " + fName; 		
							objCmd.CommandText = strSQL; 
							objCmd.Transaction =objTrans;
							objDA.Fill(ds);  //vai juntando rows.

							//isto nao devia acontecer ja aqui, so depois de ter actualizado tudo como deve ser é q o ficheiro devia ser movido.
							//para mover o ficheiro depois de lido 
							fNameNovo = System.Web.HttpContext.Current.Server.MapPath("~/"+pathMoveTo+ "/" + f.Name); //caminho+nome
							
							//NAO USAR O MOVE ==================================================================================
							//f.MoveTo(fNameNovo);  //-- funciona - nao faz overwrite (no caso de algo ter corrido mal, causa um problema)
							//===================================================================================================
							f.CopyTo(fNameNovo,true); 
							//copia para a dir, e faz overwrite se ja existir la um ficheiro com o mm nome
							f.Delete(); 
						}
					}
					catch(Exception ex)
					{
						GERAL.clsWriteError.WriteLog(ex); 
					}
				}
			}					
			updateEmpresas(ds); 
		}

		private void updateEmpresas(DataSet ds)
		{

			DataView dv = new DataView(ds.Tables[0]);

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection =objConn; 
				objConn.Open(); 
				
				using (SqlTransaction objTrans = objConn.BeginTransaction()) 
				//eu aqui ate quero que quando estoire, o seguinte esteja activado? ou nao? 
				//--> não, pq preciso da ordem de chegada
				{
					
					objCmd.Transaction = objTrans; 
//					try
//					{

						
						//actualizar ROW POR ROW
						foreach(DataRowView dr in dv)
						{
							if(objCmd.Transaction.Connection == null)
							{
								GERAL.clsWriteError.WriteSapLog("transaction is null"); //pq entrou no rollbac
								break; 
							}

							string codigoClienteSAP = dr["codigoClienteSAP"].ToString(); 
							string grupoContas = dr["grupoContas"].ToString(); 
							string sectorActividade = dr["sectorActividade"].ToString(); 
							string canalDistribuicao = dr["canalDistribuicao"].ToString(); 
							string nome1 = dr["nome1"].ToString(); 
							string nome2 = dr["nome2"].ToString(); 
							string conceitoPesquisa = dr["conceitoPesquisa"].ToString(); 
							string rua1 = dr["rua1"].ToString(); 
							string rua2 = dr["rua2"].ToString(); 
							string codigoPostal = dr["codigoPostal"].ToString(); 
							string localidade = dr["localidade"].ToString(); 
							string pais = dr["pais"].ToString(); 
							string regiao = dr["regiao"].ToString(); 
							string idioma = dr["idioma"].ToString(); 
							string telefone = dr["telefone"].ToString(); 
							string fax = dr["fax"].ToString(); 
							string email = dr["email"].ToString(); 
							string url = dr["url"].ToString(); 
							string nif = dr["nif"].ToString(); 
							string sede = dr["sede"].ToString(); 
							string numAntigo = dr["numAntigo"].ToString(); 
							string grupoClientes = dr["grupoClientes"].ToString(); 
							string codBloqueio = dr["codBloqueio"].ToString(); 
							
							FileName = dr["ficheiro"].ToString(); 
							
							objCmd.CommandText = "SELECT count(*) FROM sap_Empresas WHERE cast(codigoClienteSAP as int) = cast("+codigoClienteSAP.Trim()+" as int)  AND canalDistribuicao = '"+canalDistribuicao.Trim()+"' " ;
				
							int i = (int)objCmd.ExecuteScalar(); 

							if(i == 0) //se não existe, insiro, se existe, actualizo.
							{
								
								try
								{
									insertEmpresaSAP(objConn, objCmd, codigoClienteSAP, grupoContas, sectorActividade, canalDistribuicao, nome1, nome2, conceitoPesquisa, rua1, rua2, codigoPostal, localidade, pais, regiao, idioma, telefone, fax, email, url, nif, sede, numAntigo, grupoClientes, codBloqueio); 
								}
								catch(Exception ex)
								{
									GERAL.clsWriteError.WriteSapLog("InsertEmpresasSAP:: Erro na leitura/actualização ref. ao ficheiro " + FileName + "."); 
															
									GERAL.clsWriteError.WriteSapLog(ex.ToString()); 
								}
							}
							else
							{
								try
								{
									updateEmpresaSAP(objConn, objCmd, codigoClienteSAP, grupoContas, sectorActividade, canalDistribuicao, nome1, nome2, conceitoPesquisa, rua1, rua2, codigoPostal, localidade, pais, regiao, idioma, telefone, fax, email, url, nif, sede, numAntigo, grupoClientes, codBloqueio); //actualizdo tudo para ter na tabela das empresas sap sempre os dados mais actuais. 

									
								}
								catch(Exception ex)
								{
									GERAL.clsWriteError.WriteSapLog("UpdateEmpresasSAP:: Erro na leitura/actualização ref. ao ficheiro " + FileName + "."); 
															
									GERAL.clsWriteError.WriteSapLog(ex.ToString()); 
								}

								try
								{
									updateEmpresasLabMetro(objConn, objCmd, codigoClienteSAP, codBloqueio); 
								}
								catch(Exception ex)
								{
									GERAL.clsWriteError.WriteSapLog("InsertEmpresasLabmetro::  Erro na leitura/actualização ref. ao ficheiro " + FileName + "."); 
															
									GERAL.clsWriteError.WriteSapLog(ex.ToString()); 
								}
							}		
						}
					objTrans.Commit();					
				}
			}
		}

		//================================================================================================================
		//INSERE EMPRESAS NA TABELA SAP_EMPRESAS
		//================================================================================================================
		private void insertEmpresaSAP(SqlConnection conn, SqlCommand cmd, string codigoClienteSAP, string  grupoContas, string  sectorActividade, string  canalDistribuicao, string  nome1, string  nome2, string  conceitoPesquisa, string  rua1, string  rua2, string  codigoPostal, string  localidade, string  pais, string  regiao, string  idioma, string  telefone, string  fax, string  email, string  url, string  nif, string  sede, string  numAntigo, string  grupoClientes, string  codBloqueio)
		{

			SqlParameter[] arrParams = new SqlParameter[23];       
            
			arrParams[0] = new SqlParameter("@codigoClienteSAP",codigoClienteSAP); 
			arrParams[1] = new SqlParameter("@grupoContas",grupoContas);  
			arrParams[2] = new SqlParameter("@sectorActividade",sectorActividade); 
			arrParams[3] = new SqlParameter("@canalDistribuicao",canalDistribuicao);  
			arrParams[4] = new SqlParameter("@nome1",nome1); 
			arrParams[5] = new SqlParameter("@nome2",nome2);  
			arrParams[6] = new SqlParameter("@conceitoPesquisa",conceitoPesquisa); 
			arrParams[7] = new SqlParameter("@rua1",rua1);  
			arrParams[8] = new SqlParameter("@rua2",rua2); 
			arrParams[9] = new SqlParameter("@codigoPostal",codigoPostal);  
			arrParams[10] = new SqlParameter("@localidade",localidade); 
			arrParams[11] = new SqlParameter("@pais",pais);  
			arrParams[12] = new SqlParameter("@regiao",regiao); 
			arrParams[13] = new SqlParameter("@idioma",idioma);  
			arrParams[14] = new SqlParameter("@telefone",telefone); 
			arrParams[15] = new SqlParameter("@fax",fax); 
			arrParams[16] = new SqlParameter("@email",email); 
			arrParams[17] = new SqlParameter("@url",url);  
			arrParams[18] = new SqlParameter("@nif",nif); 
			arrParams[19] = new SqlParameter("@sede",sede);  
			arrParams[20] = new SqlParameter("@numAntigo",numAntigo); 
			arrParams[21] = new SqlParameter("@grupoClientes",grupoClientes); 
			arrParams[22] = new SqlParameter("@codBloqueio",codBloqueio); 
			
			
			
			string strSQL = "INSERT INTO sap_Empresas(codigoClienteSAP, grupoContas, sectorActividade, canalDistribuicao, nome1, nome2, conceitoPesquisa, rua1, rua2, codigoPostal, localidade, pais, regiao, idioma, telefone, fax, email, url, nif, sede, numAntigo, grupoClientes, codBloqueio) VALUES (@codigoClienteSAP,@grupoContas,@sectorActividade,@canalDistribuicao,@nome1, @nome2,@conceitoPesquisa,@rua1,@rua2,@codigoPostal,@localidade,@pais,@regiao,@idioma,@telefone,@fax,@email,@url,@nif,@sede,@numAntigo,@grupoClientes,@codBloqueio)";
			//escrever para o log antes de executar, para ficar tb com as que deram erro
			//GERAL.clsWriteError.WriteSapLog("--" + FileName + "--"+ strSQL);
			GERAL.clsDataAccess.myExecuteNonQuery(conn, cmd,strSQL, CommandType.Text,arrParams); 			
			
		}


		//================================================================================================================
		//actualiza as empresas da tabela SAP_EMPRESAS com os codigos de bloqueio vindos do SAP
		//================================================================================================================
		private void updateEmpresaSAP(SqlConnection conn, SqlCommand cmd,string codigoClienteSAP, string  grupoContas, string  sectorActividade, string  canalDistribuicao, string  nome1, string  nome2, string  conceitoPesquisa, string  rua1, string  rua2, string  codigoPostal, string  localidade, string  pais, string  regiao, string  idioma, string  telefone, string  fax, string  email, string  url, string  nif, string  sede, string  numAntigo, string  grupoClientes, string  codBloqueio)
		{

			SqlParameter[] arrParams = new SqlParameter[23];       
            
			arrParams[0] = new SqlParameter("@codigoClienteSAP",codigoClienteSAP); 
			arrParams[1] = new SqlParameter("@grupoContas",grupoContas);  
			arrParams[2] = new SqlParameter("@sectorActividade",sectorActividade); 
			arrParams[3] = new SqlParameter("@canalDistribuicao",canalDistribuicao);  
			arrParams[4] = new SqlParameter("@nome1",nome1); 
			arrParams[5] = new SqlParameter("@nome2",nome2);  
			arrParams[6] = new SqlParameter("@conceitoPesquisa",conceitoPesquisa); 
			arrParams[7] = new SqlParameter("@rua1",rua1);  
			arrParams[8] = new SqlParameter("@rua2",rua2); 
			arrParams[9] = new SqlParameter("@codigoPostal",codigoPostal);  
			arrParams[10] = new SqlParameter("@localidade",localidade); 
			arrParams[11] = new SqlParameter("@pais",pais);  
			arrParams[12] = new SqlParameter("@regiao",regiao); 
			arrParams[13] = new SqlParameter("@idioma",idioma);  
			arrParams[14] = new SqlParameter("@telefone",telefone); 
			arrParams[15] = new SqlParameter("@fax",fax); 
			arrParams[16] = new SqlParameter("@email",email); 
			arrParams[17] = new SqlParameter("@url",url);  
			arrParams[18] = new SqlParameter("@nif",nif); 
			arrParams[19] = new SqlParameter("@sede",sede);  
			arrParams[20] = new SqlParameter("@numAntigo",numAntigo); 
			arrParams[21] = new SqlParameter("@grupoClientes",grupoClientes); 
			arrParams[22] = new SqlParameter("@codBloqueio",codBloqueio); 
			
			string strSQL = "UPDATE sap_Empresas SET grupoContas =   @grupoContas, sectorActividade =  @sectorActividade,   nome1 =  @nome1, nome2 =  @nome2, conceitoPesquisa =  @conceitoPesquisa, rua1 =  @rua1, rua2 =  @rua2, codigoPostal =  @codigoPostal,   localidade =  @localidade, pais =  @pais, regiao =  @regiao, idioma =  @idioma, telefone =  @telefone, fax =  @fax, email =  @email, url =  @url, nif =  @nif, sede =  @sede, numAntigo =  @numAntigo, grupoClientes =  @grupoClientes, codBloqueio =@codBloqueio WHERE codigoClienteSAP =  @codigoClienteSAP AND canalDistribuicao =  @canalDistribuicao"; 
			//escrever para o log antes de executar, para ficar tb com as que deram erro
			//GERAL.clsWriteError.WriteSapLog("--" + FileName + "--"+ strSQL);
			GERAL.clsDataAccess.myExecuteNonQuery(conn, cmd,strSQL, CommandType.Text,arrParams); 
		}

		//================================================================================================================
		//actualiza as empresas da tabela EMPRESA com os codigos de bloqueio vindos do SAP
		//================================================================================================================
		private void updateEmpresasLabMetro(SqlConnection conn, SqlCommand cmd,string codigoClienteSAP, string codBloqueio)
		{		

			//nivelbloqueioLabmetro : 0: livre, 1: amarelo, 2: laranja, 3: vermelho - vem do sap.
			
			//escrever para o log antes de executar, para ficar tb com as que deram erro

			string strSQL = ""; 
			switch(codBloqueio)
			{
				case "00": //desbloqueado 		//--se a empresa estava bloqueada em SAP e fica desbloqueada

					strSQL= "UPDATE EMPRESA SET codigoBloqueioSAP = '00', nivelBloqueioLabmetro = CASE  WHEN (pagamentoatraso = 1 or bRequisicaoAtraso = 1) THEN 1 ELSE 0 END WHERE cast (numClienteSAP as int) = cast("+codigoClienteSAP+" as int) AND codigoBloqueioSAP <>'00'"; 

					//se está agora a 00 e já estava a zero, nao se mexe
				
					break;

				case "03" : //clientes inactivos, o outros codigos de bloqueio são removidos, pois está inactivo.
				
			
					strSQL= "UPDATE EMPRESA SET codigoBloqueioSAP = '03', nivelBloqueioLabmetro = 0 WHERE cast (numClienteSAP as int) = cast("+codigoClienteSAP+" as int)"; 

					//se está agora a 00 e já estava a zero, nao se mexe
				
					break;

                case "14": //14	Pós-Cobrança Coerciva, vamos ignorar este codigo pois nao se trata de nenhum codigo em que a empresa esteja realmente bloqueada

                    //nota daniela: isto nao vai funcioanr porque depois nao podemos facturar a esta emrpesa

                    strSQL = "UPDATE EMPRESA SET codigoBloqueioSAP = '00', nivelBloqueioLabmetro = 0 WHERE cast (numClienteSAP as int) = cast(" + codigoClienteSAP + " as int)";

                    //se está agora a 00 e já estava a zero, nao se mexe

                    break;

				default: // todos os bloqueados em sap passam para vermelho menos os clientes inactivos. 

					strSQL = "UPDATE EMPRESA SET codigoBloqueioSAP = '"+codBloqueio+"',  nivelBloqueioLabmetro = 3  WHERE cast (numClienteSAP as int) = cast("+codigoClienteSAP+" as int) "; 
					break;


			}
			//GERAL.clsWriteError.WriteSapLog("--" + FileName + "--"+ strSQL);
			GERAL.clsDataAccess.myExecuteNonQuery(conn, cmd,strSQL); 

		}
	}
}
