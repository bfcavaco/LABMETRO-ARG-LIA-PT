using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 
namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for GuiaTransporteBD.
	/// </summary>
	public class GuiaTransporteBD
	{
		public GuiaTransporteBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int UpdateBD(string command,string idGuiaTransporte,string idEmpresa, string localCarregamento, string localDescarregamento, string localDestino, string destinatario, string dataGuia, string horaSaida, string horaChegada, string matricula, string nomeCondutor, string observacoes, string idUtilizador, DataTable DT, string numContribuinte)
		{
			
			int iRet = 1 ; //se devolvo 0 correu mal, se devolvo 1, o update correu bem, e se devolvo > 1 é o id do registo

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];

			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				
				objCmd.Connection = objConn; 
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					SqlParameter[] arrParams; 
					try
					{	
						if(command =="insert")
						{
							
							arrParams = arrParamsGuiaTransporteInsert(idEmpresa, localCarregamento, localDescarregamento,localDestino, destinatario, dataGuia,  horaSaida, horaChegada, matricula, nomeCondutor, observacoes, idUtilizador, numContribuinte); 

							objCmd.CommandType = CommandType.StoredProcedure; 

							if(GERAL.clsDataAccess.intExecuteNonQuerySP(objConn, objCmd,"stpInsGuiaTransporte",arrParams) != 1)
							{
								iRet = 0; 
							}																									
					
							objCmd.CommandType = CommandType.Text; 
							objCmd.CommandText = "SELECT @@IDENTITY"; //retorna um decimal

							string idGT = objCmd.ExecuteScalar().ToString(); //este não é validado

							DataTable DTServices = DT; 

							int rows = DTServices.Rows.Count; 
							for(int i = 0; i<rows; i++)
							{                
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

								string idServico= DTServices.Rows[i]["idServico"].ToString(); 
	                            	
								arrParams = arrParamsGuiaTransporteLinhaInsert(idGT,idServico, idUtilizador); 
								
								objCmd.CommandType = CommandType.StoredProcedure;

								if(GERAL.clsDataAccess.intExecuteNonQuerySP(objConn, objCmd,"stpInsGuiaTransporteLinha",arrParams) !=1)
								{
									 iRet = 0; 
								}
							}

							if (iRet != 0) iRet = System.Convert.ToInt16(idGT); 
						}
						else if(command =="update")
						{
							//alteração dos dados do cabeçalho da guia de transporte
							arrParams = arrParamsGuiaTransporteUpdate(idGuiaTransporte, localCarregamento,  localDescarregamento, localDestino, destinatario, dataGuia,  horaSaida, horaChegada, matricula, nomeCondutor, observacoes,idUtilizador,numContribuinte);  
							
							objCmd.CommandType = CommandType.StoredProcedure; 

							if(GERAL.clsDataAccess.intExecuteNonQuerySP(objConn, objCmd,"stpUpdGuiaTransporte",arrParams) != 1)
							{
								iRet = 0; 
							}

							//serviços : só há 2 acções possíveis: inserir ou apagar. as rows em si não são alteradas. 
							DataTable DTServices = DT; 

							//apagar as rows que foram removidas    
							DataRow[] dRows = DTServices.Select(null,null,DataViewRowState.Deleted); 
							foreach(DataRow dRow in dRows)
							{
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
								string idGuiaTransporteDel=  dRow["idGuiaTransporte",DataRowVersion.Original].ToString(); 
								string idServico=  dRow["idServico",DataRowVersion.Original].ToString(); 
								
								arrParams = new SqlParameter[2];

								arrParams[0] = new SqlParameter("@idGuiaTransporte", idGuiaTransporteDel);
								arrParams[1] = new SqlParameter("@idServico", idServico);
								
								objCmd.CommandType = CommandType.StoredProcedure; 
								
								//aqui não sei bem quantas associações podem existir, por isso valido == 0
								if(GERAL.clsDataAccess.intExecuteNonQuerySP(objConn, objCmd,"stpDelGuiaTransporteLinha",arrParams) ==0)
								{
									iRet = 0; 
								}
							}
							
							//adicionar alguma nova row que tenha sido adicionada
							dRows = DTServices.Select(null,null,DataViewRowState.Added);                                 
							foreach(DataRow dRow in dRows)
							{
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
								
								string idServico= dRow["idServico"].ToString(); 
	                            	
								arrParams = arrParamsGuiaTransporteLinhaInsert(idGuiaTransporte,idServico, idUtilizador); 
								objCmd.CommandType = CommandType.StoredProcedure;
								if(GERAL.clsDataAccess.intExecuteNonQuerySP(objConn, objCmd,"stpInsGuiaTransporteLinha",arrParams)!=1)
								{
									iRet = 0; 
								}
							}    
						}
					
						objTrans.Commit(); 
						return iRet; 
					}
					catch(Exception ex)
					{ 	
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception exep)
						{
							GERAL.clsWriteError.WriteLog(exep); 
						}
						
						GERAL.clsWriteError.WriteLog(ex); 
						return 0; //case chegue aqui...

					}
				}
			}
		}

		
		private SqlParameter[] arrParamsGuiaTransporteInsert(string idEmpresa,  string localCarregamento, string localDescarregamento, string localDestino, string destinatario, string dataGuia, string horaSaida, string horaChegada, string matricula, string nomeCondutor, string observacoes, string idUtilizador, string numContribuinte)
		{

			SqlParameter[] arrParams = new SqlParameter[13];

			
			arrParams[0] = new SqlParameter("@localCarregamento", localCarregamento);
			arrParams[1] = new SqlParameter("@idEmpresa", idEmpresa); 
			arrParams[2] = new SqlParameter("@localDescarregamento", localDescarregamento);
			arrParams[3] = new SqlParameter("@localDestino", localDestino);
            
			arrParams[4] = new SqlParameter("@destinatario", destinatario);
			arrParams[5] = new SqlParameter("@dataGuia", dataGuia);
			arrParams[6] = new SqlParameter("@horaSaida", horaSaida);
			
			arrParams[7] = new SqlParameter("@matricula", matricula);
			arrParams[8] = new SqlParameter("@nomeCondutor", nomeCondutor);
			arrParams[9] = new SqlParameter("@observacoes",observacoes);
			
			arrParams[10] = new SqlParameter("@idUtilizador", idUtilizador); 
			arrParams[11] = new SqlParameter("@numContribuinte", numContribuinte); 
			
			arrParams[12] = new SqlParameter("@horaChegada", horaChegada);	
			return arrParams; 

		}

		private SqlParameter[] arrParamsGuiaTransporteUpdate(string idGuiaTransporte, string localCarregamento, string localDescarregamento, string localDestino, string destinatario, string dataGuia, string horaSaida, string horaChegada, string matricula, string nomeCondutor, string observacoes, string idUtilizador, string numContribuinte)
		{
			SqlParameter[] arrParams = new SqlParameter[13];

			arrParams[0] = new SqlParameter("@idGuiaTransporte", idGuiaTransporte);
			arrParams[1] = new SqlParameter("@localCarregamento", localCarregamento);
			arrParams[2] = new SqlParameter("@localDescarregamento", localDescarregamento);
			arrParams[3] = new SqlParameter("@localDestino", localDestino);
            
			arrParams[4] = new SqlParameter("@destinatario", destinatario);
			arrParams[5] = new SqlParameter("@dataGuia", dataGuia);
			arrParams[6] = new SqlParameter("@horaSaida", horaSaida);
			
			arrParams[7] = new SqlParameter("@matricula", matricula);
			arrParams[8] = new SqlParameter("@nomeCondutor", nomeCondutor);
			arrParams[9] = new SqlParameter("@observacoes",observacoes);
			arrParams[10] = new SqlParameter("@idUtilizador", idUtilizador); 
			arrParams[11] = new SqlParameter("@numContribuinte", numContribuinte); 
			arrParams[12] = new SqlParameter("@horaChegada", horaChegada);	
			return arrParams; 

		}

		private SqlParameter[] arrParamsGuiaTransporteLinhaInsert(string idGuiaTransporte, string idServico, string idUtilizador)
		{

			SqlParameter[] arrParams = new SqlParameter[3];

			arrParams[0] = new SqlParameter("@idGuiaTransporte", idGuiaTransporte);
			arrParams[1] = new SqlParameter("@idServico", idServico);
			arrParams[2] = new SqlParameter("@idUtilizador", idUtilizador); 
		
			return arrParams; 

		}

		private SqlParameter[] arrParamsGuiaTransporteLinhaUpdate(string idGuiaTransporte, string idServico,  string idUtilizador)
		{

		

			SqlParameter[] arrParams = new SqlParameter[3];

			arrParams[0] = new SqlParameter("@idGuiaTransporte", idGuiaTransporte);
			arrParams[1] = new SqlParameter("@idServico", idServico);
			arrParams[2] = new SqlParameter("@idUtilizador", idUtilizador); 
		
			return arrParams; 

		}


		//funcao nova
		public string UpdateLocalizacaoServicos(string idLocalNovo,string userId, string idsServicos)
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
                
						string strSQL = "UPDATE SERVICO SET idLocalCalibracao   = '"+idLocalNovo+"' , idUtilAlteracao ='"+ userId+"', dtAlteracao = CONVERT(DATETIME, GETDATE(), 102) WHERE idServico IN ("+idsServicos+")"; 
						
						objCmd.CommandType = CommandType.Text; 
						objCmd.CommandText = strSQL; 
						
						int i = objCmd.ExecuteNonQuery(); 
						if(i > 0 )
						{
							string strSQLAudit =""; 
							try
							{

								if(idsServicos.Length > 8000)
								{
									idsServicos = idsServicos.Substring(0,7500)+"... ver ficheiro de log desta data."; 
									GERAL.clsWriteError.WriteLog("String demasiado comprida para ser inserida na tabela de auditoria. ---" + strSQLAudit); 
								}
								strSQLAudit =  "INSERT INTO Auditoria(username, tabela, data,tipoAccao, dados) VALUES ('"+userId+"','Serviço(Local)',getDate(),'U','idLocalCalibracao:"+idLocalNovo+";Servicos:"+idsServicos+"')"; 
								objCmd.CommandText = strSQLAudit; 
								objCmd.ExecuteNonQuery(); 

							}
							catch //em principio nao ha de acontecer agora
							{
		                    
								GERAL.clsWriteError.WriteLog("Erro ao tentar introduzir a string: "+ strSQLAudit); 
							}

							objTrans.Commit(); 
							
							return GERAL.clsGeral.ErrorMessage.MSG_DB;     
						}
						else
						{
							objTrans.Rollback(); 
							return GERAL.clsGeral.ErrorMessage.ERR_DB; 
						}
					}
	
					catch(Exception ex)
					{ 	
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); 
						}
						GERAL.clsWriteError.WriteLog(ex); 
		                
						return GERAL.clsGeral.ErrorMessage.ERR_DB; 
					}
				}
			}
		}

	}
}
