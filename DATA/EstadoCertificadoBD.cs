using System;
using System.Data;
using System.Data.SqlClient; 
using System.Configuration; 
using System.Web; //para a session


namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for EstadoCertificadoBD.
	/// </summary>
	public class EstadoCertificadoBD
	{
		public EstadoCertificadoBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}
   
		//====================================================================================
        //========= retorna uma DataTable com documentos para validação
		//====================================================================================
        public DataTable DTDocumentsForValidation(string inDocs)
        {   
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inDocs", inDocs);	
			
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetCertificadosParaValidacao", arrParams); 
        }

		//nova query, feita em paralelo para nao estragar nada
		public DataTable DTDocumentsForValidation2(string inDocs)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inDocs", inDocs);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetCertificadosParaValidacao2", arrParams);
		}
		//====================================================================================
		//========= retorna uma DataTable de certificados assinados existentes
		//====================================================================================
		public DataTable DTListCertificado(string empresa, string grandeza, string refServico, string ano,string refBRE)
		{   
			SqlParameter[] arrParams = new SqlParameter[5];
            
			arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inGrandeza", grandeza);
			arrParams[2] = new SqlParameter("@inRefServico", refServico);
			arrParams[3] = new SqlParameter("@inAno", ano);
			arrParams[4] = new SqlParameter("@refBRE", refBRE);
			
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetListCertificados", arrParams); 
		}


        //====================================================================================
		//========= retorna uma DataTable de certificados  por empresa e bre (Opcional)
		//====================================================================================
		public DataTable DTListCertificadoByEmpresa(string idempresa, string idBre, string grandeza)		
        {   
			SqlParameter[] arrParams = new SqlParameter[3];
            
			arrParams[0] = new SqlParameter("@inGrandeza", grandeza);
			arrParams[1] = new SqlParameter("@idBre", idBre);
			arrParams[2] = new SqlParameter("@idEmpresa", idempresa);
			
			
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetListCertificadosByEmpresa", arrParams); 
		}

         
        //====================================================================================
        //========= retorna uma DataTable de certificados assinados existentes
        //====================================================================================
        public DataTable DTListCertificadoIgae(string idEmpresa, string ano,string tipoEquipamento)
        {
            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@idEmpresa", idEmpresa);
            arrParams[1] = new SqlParameter("@ano", ano);
            arrParams[2] = new SqlParameter("@tipoEquipamento", tipoEquipamento);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListCertificadosIgae", arrParams);
        }  


		//usado no BO
		//====================================================================================
		//========= retorna uma DataTable de certificados assinados existentes
		//====================================================================================
		public DataTable DTListCertificadoByEquipamento(string idEquipamento)
		{   
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEquipamento", idEquipamento);
		
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListCertificadosByIdEquipamento", arrParams); 
		}  
  
		

		//====================================================================================
		//========= Altera a tabela AUXILIAR auxServico_Acreditado com as acreditações 
		//=========	dos equipamentos  MAS NAO PERCEBO PORQUÊ
		//========= O AUTOPOSTBACK NA CHECKBOX DO DATAGRID PARA DATAGRIDS GRANDES É HORRÍVEL
		//====================================================================================
		public bool UpdCertTemplate(string listaIdServicos, string idServico, string acreditado, string estado)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
			        
			arrParams[0] = new SqlParameter("@inListaIdServico", listaIdServicos);
			arrParams[1] = new SqlParameter("@inIdServico", idServico);
			arrParams[2] = new SqlParameter("@inAcreditado", acreditado);
			arrParams[3] = new SqlParameter("@estado", estado);
			arrParams[4] = new SqlParameter("@UserID", HttpContext.Current.Session["UserId"].ToString());

			try
			{		
				GERAL.clsDataAccess.vExecuteNonQuerySP("stpWUpdCertTemplate",arrParams);
				return true; 
			}
			catch(Exception ex)
			{
				GERAL.clsWriteError.WriteLog(ex); 
				return false; 
			}		
		} 

		//feito à pressa, melhorar mais tarde
		public string InsCertTemplate(string listaIdServicos, string idUtilizador)
		{
			
			try
			{
				string strSQL =""; 
				string err =""; 	
				strSQL = "DELETE FROM auxServico_Acreditado WHERE idUtilizador = "+ idUtilizador; 
				err = GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL); 

				if(err==GERAL.clsGeral.ErrorMessage.MSG_DB) //correu bem, pode continuar, senao devolve logo
				{

					strSQL = "INSERT INTO auxServico_Acreditado SELECT S.idServico, TE.acreditado, "+ idUtilizador + " FROM servico S INNER JOIN equipamento E ON E.idEquipamento = S.idEquipamento  INNER JOIN tipoEquipamento TE ON TE.idTipoEquipamento = E.idTipoEquipamento WHERE S.idServico IN ("+listaIdServicos+") "; 

					return GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL); 	
				}
				return err; 
			}
			catch(Exception ex)
			{
				GERAL.clsWriteError.WriteLog(ex); 
				return GERAL.clsGeral.ErrorMessage.ERR_DB;
			}
		}
		

//		//====================================================================================
//		//========= DATATABLE com os serviços e se estão acreditados - RUI
//		//========= vai buscar o id e o campo "acreditado" à tabela auxServico_Acreditado - DM
//		//====================================================================================
//		public DataTable DTGetCertTemplate()
//		{   
//			return LabMetro.GERAL.clsDataAccess.SPExecuteDT("stpWGetCertTemplate"); 
//		}

		//MESMA COISA EM DATAREADER
		public SqlDataReader DRGetCertTemplate()
		{   
			SqlParameter[] arrParams = new SqlParameter[1];
  			arrParams[0] = new SqlParameter("@UserID", HttpContext.Current.Session["UserId"].ToString());
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetCertTemplate", arrParams);
		}

		//====================================================================================
		//========= retorna uma DataTable de equipamentos com um estado especifico
		//====================================================================================
		public DataTable DTCertificadoByEstadoCertificado(string empresa, string grandeza, string NServico, string ano, string utilizador)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
            
			arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inGrandeza", grandeza);
			arrParams[2] = new SqlParameter("@inNServico", NServico);
			arrParams[3] = new SqlParameter("@inAno", ano);
			//arrParams[4] = new SqlParameter("@inGrupo", grupo);
			arrParams[4] = new SqlParameter("@inUserName", utilizador);
        	
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetCertificadosPorAprovar", arrParams); //esta stp so é chamada uma vez, aqui
		}


		//====================================================================================
		//========= retorna uma DataTable de serviços para uma determinada pessoa (param user) aprovar
		//====================================================================================
		public DataTable DTCertificadosPorAprovar(string empresa, string refServico, string username, bool admin)
		{
			SqlParameter[] arrParams = new SqlParameter[4];
            
			arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inRefServico", refServico);
			arrParams[2] = new SqlParameter("@inUsername", username);
			arrParams[3] = new SqlParameter("@bVerTodos", admin);
        	
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetCertificadosPorAprovarRevisto", arrParams); 
		}


	
		//====================================================================================
		//====================================================================================
		public SqlDataReader DRAnoCertificado()
		{
			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpWGetAnoCertificados");
		}

		//====================================================================================
		//====================================================================================
		public SqlDataReader DRTipoCertificado()
		{
			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpWGetTipoCertificado");
		}

		//====================================================================================
		//substitui o datareader acima, retorna unicamente o que interessa.
		//====================================================================================
		public string idTipoCertificado(string sigla)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inSigla", sigla);
			
			object o = LabMetro.GERAL.clsDataAccess.myExecuteScalarSP("stpWGetTipoCertificadoBySigla", arrParams);
			if(!Convert.IsDBNull(o))return System.Convert.ToString(o); 
			return ""; //se está a null retorna empty string			
		}

		//====================================================================================
		//recebe uma sigla e verifica se ela existe na bd. retorna true se existe e false se nao existe
		//====================================================================================
		public bool bSiglaExists(string sigla)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inSigla", sigla);
			
			int i = (int)LabMetro.GERAL.clsDataAccess.myExecuteScalarSP("stpWGetSiglaBySigla", arrParams);
			if (i > 0) return true; 
			return false; 
			
		}

		//====================================================================================
		//substituir por um boolean e um executescalar
		//====================================================================================
		public SqlDataReader DRListGrandezas(string utilizador, string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@UserID", HttpContext.Current.Session["UserId"].ToString());
			arrParams[1] = new SqlParameter("@inIdServico", idServico);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetGrandezaUtilizador", arrParams);
		}
		//====================================================================================
		//mesma coisa em DT -- out 2008
		//====================================================================================
		public DataTable DTListGrandezas(string utilizador, string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@UserID", HttpContext.Current.Session["UserId"].ToString());
			arrParams[1] = new SqlParameter("@inIdServico", idServico);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetGrandezaUtilizador", arrParams);
		}

		//====================================================================================
		//verificar se o serviço a validar é da grandeza do responsável técnico
		//true = é, false não é
		//====================================================================================
		public bool bGrandezaRT(string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
			
			arrParams[0] = new SqlParameter("@UserID", HttpContext.Current.Session["UserId"].ToString());
			arrParams[1] = new SqlParameter("@inIdServico", idServico);
			
			//retorna o idGrandeza se existe, se nao, null
																		     
			SqlDataReader dr = LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetGrandezaByUserIdServicoID", arrParams); 			
			bool b = false; 
			if(dr.HasRows)b= true;  //muito mal feito, a melhorar depois, se contem dados, independentenmente do que é... está bem
			dr.Close(); 
			return b; 
			
		}

//		//substituir por um boolean e um executescalar
//		public SqlDataReader DRExisteFuncionarioWorkflow(string idFuncionario)
//		{
//			SqlParameter[] arrParams = new SqlParameter[1];
//
//			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);
//
//			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetExisteFuncionario", arrParams);
//		}

		//====================================================================================
		//====================================================================================
		public bool bExisteFuncionarioWorkFlow(string idFuncionario)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);

			int i =  (int)LabMetro.GERAL.clsDataAccess.myExecuteScalarSP("stpWGetExisteFuncionario", arrParams); //retorna um int (select count...)
			if(i == 0) return false; //nao existe
			else return true; 
		}
		
//		//==========================================================================================================
//		//substituido pelas funções a seguir, uma para receber o grau, outra para receber o perfil
//		//==========================================================================================================
//		public SqlDataReader DRListGrau(string utilizador)
//		{
//			SqlParameter[] arrParams = new SqlParameter[1];
//
//			arrParams[0] = new SqlParameter("@inUser", utilizador);
//
//			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetGrauPerfilUtilizador", arrParams);
//		}


		//==========================================================================================================
		//coma a funcao anterior devolve apenas um valor, vou substituir o reader por um executescalar e devolver logo a string
		//==========================================================================================================
		public string strGrauUtilizador()//usa o id que está na sessão como param directamente em baixo
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@UserID", HttpContext.Current.Session["UserId"].ToString());

			object o = LabMetro.GERAL.clsDataAccess.myExecuteScalarSP("stpWGetGrauUtilizador", arrParams);
			if(!Convert.IsDBNull(o))return System.Convert.ToString(o); 
			return ""; //se está a null retorna empty string
		}

		//====================================================================================
		//"com pai"
		//====================================================================================
		public int UpdateEstadoCertificado(string idServico, string idEstadoActualCertificado, string idEstadoNovoCertificado,  string observacoes, string idTipoCertificado,string idServicoPai)
		{
			SqlParameter[] arrParams = new SqlParameter[8];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdEstadoActualCertificado", idEstadoActualCertificado);
			arrParams[2] = new SqlParameter("@inIdEstadoNovoCertificado", idEstadoNovoCertificado);
			arrParams[3] = new SqlParameter("@inObs", observacoes);
			arrParams[4] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[5] = new SqlParameter("@userId",System.Web.HttpContext.Current.Session["UserId"]);
			arrParams[6] = new SqlParameter("@userName",System.Web.HttpContext.Current.User.Identity.Name.ToString()); 
			arrParams[7] = new SqlParameter("@inIdServicoPai", idServicoPai);

			return GERAL.clsDataAccess.ExecuteNonQuerySP("stpWUpdEstadoCertificado",arrParams); 

		}

		//====================================================================================
		//"sem pai"
		//====================================================================================
		public int UpdateEstadoCertificado(string idServico, string idEstadoActualCertificado, string idEstadoNovoCertificado,  string observacoes, string idTipoCertificado)
		{
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdEstadoActualCertificado", idEstadoActualCertificado);
			arrParams[2] = new SqlParameter("@inIdEstadoNovoCertificado", idEstadoNovoCertificado);
			arrParams[3] = new SqlParameter("@inObs", observacoes);
			arrParams[4] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[5] = new SqlParameter("@userId",System.Web.HttpContext.Current.Session["UserId"]);
			arrParams[6] = new SqlParameter("@userName",System.Web.HttpContext.Current.User.Identity.Name.ToString()); 
			
			return GERAL.clsDataAccess.ExecuteNonQuerySP("stpWUpdEstadoCertificado",arrParams); 

		}


		//====================================================================================
		//com connexao, com pai
		//====================================================================================
		public int UpdateEstadoCertificado(SqlConnection myConnection, SqlCommand myCommand,string idServico, string idEstadoActualCertificado, string idEstadoNovoCertificado,  string observacoes, string idTipoCertificado,string idServicoPai)
		{
	
			SqlParameter[] arrParams = new SqlParameter[8];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdEstadoActualCertificado", idEstadoActualCertificado);
			arrParams[2] = new SqlParameter("@inIdEstadoNovoCertificado", idEstadoNovoCertificado);
			arrParams[3] = new SqlParameter("@inObs", observacoes);
			arrParams[4] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[5] = new SqlParameter("@userId",System.Web.HttpContext.Current.Session["UserId"]);
			arrParams[6] = new SqlParameter("@userName",System.Web.HttpContext.Current.User.Identity.Name.ToString()); 
			arrParams[7] = new SqlParameter("@inIdServicoPai", idServicoPai);

			return GERAL.clsDataAccess.myExecuteNonQuery(myConnection, myCommand,"stpWUpdEstadoCertificado",CommandType.StoredProcedure,arrParams); 

		}

		//====================================================================================
		//com connexao, sem pai
		//====================================================================================
		public int UpdateEstadoCertificado(SqlConnection myConnection, SqlCommand myCommand,string idServico, string idEstadoActualCertificado, string idEstadoNovoCertificado,  string observacoes, string idTipoCertificado)
		{
	
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdEstadoActualCertificado", idEstadoActualCertificado);
			arrParams[2] = new SqlParameter("@inIdEstadoNovoCertificado", idEstadoNovoCertificado);
			arrParams[3] = new SqlParameter("@inObs", observacoes);
			arrParams[4] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[5] = new SqlParameter("@userId",System.Web.HttpContext.Current.Session["UserId"]);
			arrParams[6] = new SqlParameter("@userName",System.Web.HttpContext.Current.User.Identity.Name.ToString()); 
			

			return GERAL.clsDataAccess.myExecuteNonQuery(myConnection, myCommand,"stpWUpdEstadoCertificado",CommandType.StoredProcedure,arrParams); 

		}


		//====================================================================================
		//faz update e retorna true se correu bem e false se correu mal - com pai
		//====================================================================================
		public bool UpdateEstadosCertificados(string[] idsServicos, string[] idsEstadosActuais,  string[] idsEstadosNovos, string username, string[] observacoes, string[] idsTipoCertificado, string[] idsServicoPai)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
              
					//pôr aqui um foreach ou algo assim
					try
					{	
						for(int i = 0;i<idsServicos.Length;i++)
						{

							//aqui alguns podem estar bem e outros não.
							
							
							
							int iRes = UpdateEstadoCertificado(objConn, objCmd, idsServicos[i], idsEstadosActuais[i], idsEstadosNovos[i], observacoes[i], idsTipoCertificado[i],idsServicoPai[i]);  //nao sei se aqui vou avaliar o resultado ou nao...
						
							
							//GERAL.clsWriteError.WriteLog("iRES = " + iRes.ToString());

						
						}
                                  
						objTrans.Commit(); 
						
						return true;
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
							return false;
						}
						GERAL.clsWriteError.WriteLog(ex); 
						return false; 
					}
				}
			}
            
		}


		//====================================================================================
		//faz update e retorna true se correu bem e false se correu mal - sem pai
		//====================================================================================
		public bool UpdateEstadosCertificados(string[] idsServicos, string[] idsEstadosActuais,  string[] idsEstadosNovos, string username, string[] observacoes, string[] idsTipoCertificado)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
              
					//pôr aqui um foreach ou algo assim
					try
					{	
						for(int i = 0;i<idsServicos.Length;i++)
						{

							//aqui alguns podem estar bem e outros não.
							
							
							
							int iRes = UpdateEstadoCertificado(objConn, objCmd, idsServicos[i], idsEstadosActuais[i], idsEstadosNovos[i], observacoes[i], idsTipoCertificado[i]);  //nao sei se aqui vou avaliar o resultado ou nao...
						
							
							//GERAL.clsWriteError.WriteLog("iRES = " + iRes.ToString());

						
						}
                                  
						objTrans.Commit(); 
						
						return true;
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
							return false;
						}
						GERAL.clsWriteError.WriteLog(ex); 
						return false; 
					}
				}
			}
            
		}
		//====================================================================================
		//========= retorna uma DataTable com documentos para validação
		//====================================================================================
		public DataTable DTGetServicosParaValidacaoPastas(string inDocs)
		{   
			
			SqlParameter[] arrParams = new SqlParameter[1];
            
			
			arrParams[0] = new SqlParameter("@inRefs", inDocs);	
			
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetServicosParaValidacaoPastas", arrParams); 
		}  

	}
}

