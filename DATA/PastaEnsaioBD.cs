using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for PastaEnsaioBD.
	/// </summary>
	public class PastaEnsaioBD
	{
		public PastaEnsaioBD()
		{
		}

		// Grava as alterações à Pasta de Ensaio numa única transacção e devolve a msg de erro
		//como preciso das mensagens de erro, nao posso devolver bool, por isso, se devolver algo diferente de ...é pq correu MAL

        //aqui recebo NULL no campo valor
		public string UpdatePastaEnsaio(string idServico, string idLocalActual, string calibracaoExterna, string observacoes, string idEstadoServico, string idEquipamento, string numIdentificacao, string numSerie, string forma,string idClasse, string classe,string alcanceInf, string alcanceSup, string idUnidadeAlcance, string alcance, string resolucao, string marca, string modelo, string idFuncionarioTreino, DataView dvLinhasServico, DataView dvCertificados, string username, string quantidade, string unidadeQuantidade, string valor, string idEmpresa, string idTipoEquipamento, string obsCliente, string idComentarioEstado,string idSubTipoServico, string idEstadoServicoOriginal, string campo1, string campo2, string conforme,string bDeslocacao, string idNivelPrioridade, string dtPrevisao, string etiqueta1, string etiqueta2, string etiqueta3, string linguaCertificado, string observacoesEquipamento, string tipoDeslocacao, string codigoIPQ, string idEstadoRelacaoCalibracao, string bRejeitado, string numEtiquetaIPQ, string idSubTipoEquipamento)
		{
		
			EquipamentoBD equipamento = new EquipamentoBD();
			ServicoBD servico = new ServicoBD(); 

			string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn = new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand()) 
			{
				
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn;
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
    
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction = objTrans; 
                    try
                    {			
                        // ***********************************************
                        // Actualizar o Equipamento
                        // ***********************************************

						objCmd.CommandType = CommandType.Text; 
						
						if(equipamento.validaEquipamento(objConn,objCmd,idEmpresa,idEquipamento,idTipoEquipamento,numSerie, numIdentificacao, "update",marca,modelo) ==false)
						{
							objTrans.Rollback(); 

							return "Já existe um equipamento c/ o mesmo núm.Série e/ou núm. Ident nessa empresa. Actualização não efectuada.<br />Verifique também a marca do equipamento"; 
						
						}

						objCmd.CommandType = CommandType.StoredProcedure;

                        UpdateEquipamentoInPastaEnsaio(objConn, objCmd, idEquipamento, numSerie, numIdentificacao,alcanceInf,alcanceSup,idUnidadeAlcance, alcance, resolucao, modelo, marca,idClasse,classe, forma,idTipoEquipamento,campo1, campo2,etiqueta1, etiqueta2, etiqueta3,observacoesEquipamento,codigoIPQ,idEstadoRelacaoCalibracao);
                       
                        // ***********************************************
                        // Actualizar o Serviço
                        // ***********************************************                        
						objCmd.CommandType = CommandType.StoredProcedure;

                        //aqui mando null no campo VALOR
						int iRows=   UpdateServicoInPastaEnsaio(objConn, objCmd, idServico, idLocalActual, calibracaoExterna, observacoes, quantidade, unidadeQuantidade, valor, idEstadoServico, idFuncionarioTreino, username,obsCliente, idComentarioEstado,idSubTipoServico, idEstadoServicoOriginal,conforme,bDeslocacao,idNivelPrioridade, dtPrevisao, linguaCertificado,tipoDeslocacao, bRejeitado, numEtiquetaIPQ,idSubTipoEquipamento); 

						if(iRows != 1) 
						{
							objTrans.Rollback();
							return "Pasta não actualizada. Feche a pasta de ensaio e volte e entrar."; 

						}


                        // ***********************************************
                        // Inserir / Actualizar / Apagar Linhas de Serviço
                        // ***********************************************                        

                        // Inserir Linhas de Serviço
                        dvLinhasServico.RowStateFilter = DataViewRowState.Added;
                        for(int i = 0;i < dvLinhasServico.Count ;i++)
                        {
							objCmd.CommandType = CommandType.StoredProcedure;
                            InsertServicoLinha(objConn, objCmd, idServico, dvLinhasServico[i]["numPecas"].ToString(), dvLinhasServico[i]["numPontos"].ToString(), dvLinhasServico[i]["pontosCalib"].ToString(), username); 
                           
                        }

                        // Actualizar Linhas de Serviço
                        dvLinhasServico.RowStateFilter = DataViewRowState.ModifiedCurrent;
                        for(int i = 0;i < dvLinhasServico.Count ;i++)
                        {
							objCmd.CommandType = CommandType.StoredProcedure;
                            UpdateServicoLinha(objConn, objCmd, dvLinhasServico[i]["idServicoLinha"].ToString(), idServico, dvLinhasServico[i]["numPecas"].ToString(), dvLinhasServico[i]["numPontos"].ToString(), dvLinhasServico[i]["pontosCalib"].ToString(), username); 
                         
                        }

                        // Apagar Linhas de Serviço
                        dvLinhasServico.RowStateFilter = DataViewRowState.Deleted;
                        for(int i = 0;i < dvLinhasServico.Count ;i++)
                        {
							objCmd.CommandType = CommandType.StoredProcedure;
                            DeleteServicoLinha(objConn, objCmd, dvLinhasServico[i]["idServicoLinha"].ToString(), username); 
                            
                        }

                        // ***********************************************
                        // Inserir / Actualizar / Apagar Certificados
                        // ***********************************************                        

                        // Inserir Certificados
                        dvCertificados.RowStateFilter = DataViewRowState.Added;
                        for(int i = 0;i < dvCertificados.Count ;i++)
                        {
							objCmd.CommandType = CommandType.StoredProcedure;
                            InsertCertificado(objConn, objCmd, idServico, dvCertificados[i]["ident"].ToString(), dvCertificados[i]["idFuncionario"].ToString(), dvCertificados[i]["dtCertificado"].ToString(), "", username); 
                            
                        }

                        // Actualizar Certificados
                        dvCertificados.RowStateFilter = DataViewRowState.ModifiedCurrent;
                        for(int i = 0;i < dvCertificados.Count ;i++)
                        {
							objCmd.CommandType = CommandType.StoredProcedure;
                            UpdateCertificado(objConn, objCmd, dvCertificados[i]["idCertificado"].ToString(), idServico, dvCertificados[i]["ident"].ToString(), dvCertificados[i]["idFuncionario"].ToString(), dvCertificados[i]["dtCertificado"].ToString(), "", username); 
                            
                        }

                        // Apagar Certificados
                        dvCertificados.RowStateFilter = DataViewRowState.Deleted;
                        for(int i = 0;i < dvCertificados.Count ;i++)
                        {
							objCmd.CommandType = CommandType.StoredProcedure;
                            DeleteCertificado(objConn, objCmd, dvCertificados[i]["idCertificado"].ToString(), username); 
                            
                        }

                        objTrans.Commit(); 
                        return GERAL.clsGeral.ErrorMessage.MSG_DB; 
					
                    }
                    catch(SqlException ex)
                    { 	
                        try
                        {	
                            objTrans.Rollback();
                        }
                        catch(Exception exep)
                        {
                            GERAL.clsWriteError.WriteLog(exep);
                            return "Erro ao actualizar a pasta de ensaio."; 
                        }
                        GERAL.clsWriteError.WriteLog(ex);
                        return "Erro ao actualizar a pasta de ensaio."; 
                    }
				}
			}
		}

		// Funcao que actualiza um Equipamento (na Pasta de Ensaio)
		public void UpdateEquipamentoInPastaEnsaio(SqlConnection myConnection, SqlCommand myCommand, string idEquipamento, string NumSerie, string NumIdentificacao, string alcanceInf, string alcanceSup, string idAlcance,string Alcance, string Resolucao, string Modelo, string Marca, string idClasse, string Classe, string Forma, string idTipoEquipamento, string campo1, string campo2,string etiqueta1, string etiqueta2, string etiqueta3, string observacoesEquipamento,string codigoIPQ, string idEstadoRelacaoCalibracao)

		{
			SqlParameter[] arrParams = new SqlParameter[23];

			arrParams[0] = new SqlParameter("@inIdEquipamento", idEquipamento);
			arrParams[1] = new SqlParameter("@inNumSerie", NumSerie);
			arrParams[2] = new SqlParameter("@inNumIdentificacao", NumIdentificacao);
			arrParams[3] = new SqlParameter("@inAlcanceInf", GERAL.clsGeral.convertDecimalSeparator(alcanceInf));
			arrParams[4] = new SqlParameter("@inAlcanceSup", GERAL.clsGeral.convertDecimalSeparator(alcanceSup));
			arrParams[5] = new SqlParameter("@inIdAlcance", idAlcance);
			arrParams[6] = new SqlParameter("@inAlcance", Alcance);
			arrParams[7] = new SqlParameter("@inResolucao", Resolucao);
			arrParams[8] = new SqlParameter("@inModelo", Modelo);
			arrParams[9] = new SqlParameter("@inMarca", Marca);
			arrParams[10] = new SqlParameter("@inIdClasse", idClasse);
			arrParams[11] = new SqlParameter("@inClasse", Classe);
			arrParams[12] = new SqlParameter("@inForma", Forma);
			arrParams[13] = new SqlParameter("@inUserId", HttpContext.Current.Session["UserId"].ToString());
			arrParams[14] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[15] = new SqlParameter("@campo1", campo1);
			arrParams[16] = new SqlParameter("@campo2", campo2);
            arrParams[17] = new SqlParameter("@etiqueta1", etiqueta1);
            arrParams[18] = new SqlParameter("@etiqueta2", etiqueta2);
            arrParams[19] = new SqlParameter("@etiqueta3", etiqueta3);
            arrParams[20] = new SqlParameter("@inObservacoes", observacoesEquipamento);
            arrParams[21] = new SqlParameter("@codigoIPQ", codigoIPQ);
            arrParams[22] = new SqlParameter("@idEstadoRelacaoCalibracao", idEstadoRelacaoCalibracao);
			
			GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection, myCommand, "stpUpdateEquipamentoInPastaEnsaio", arrParams); 
		}

		// Funcao que actualiza um Serviço (na Pasta de Ensaio)
		public int UpdateServicoInPastaEnsaio(SqlConnection myConnection, SqlCommand myCommand, string idServico, string idLocalActual, string calibracaoExterna, string observacoes, string quantidade, string unidadeQuantidade,string valor, string idEstadoServico, string idFuncionarioTreino, string username, string obsCliente, string idComentarioEstado, string idSubTipoServico, string idEstadoServicoOriginal, string conforme, string bDeslocacao, string idNivelPrioridade, string dtPrevisao, string linguaCertificado, string tipoDeslocacao, string bRejeitado, string numEtiquetaIPQ, string idSubTipoEquipamento)
		{
			SqlParameter[] arrParams = new SqlParameter[22];


            //foi removido o valor.
            //o valor será actualizado pela nova aplicação tinta digital.
            
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
            arrParams[2] = new SqlParameter("@inIdLocalActual", idLocalActual);
			arrParams[3] = new SqlParameter("@inCalibracaoExterna", GERAL.clsGeral.ConvertStringToBool(calibracaoExterna));
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inIdFuncionarioTreino", idFuncionarioTreino);
			arrParams[6] =new SqlParameter("@inQuantidade",quantidade); 
			arrParams[7] = new SqlParameter("@inUnidadeQuantidade",unidadeQuantidade);      
			//arrParams[8] = new SqlParameter("@inValor", GERAL.clsGeral.ConvertStringToDouble(valor));    
			arrParams[8] = new SqlParameter("@dtPrevisao", dtPrevisao);
            arrParams[9] = new SqlParameter("@inUsername", username);
			arrParams[10] = new SqlParameter("@inObsCliente", obsCliente);
			arrParams[11] = new SqlParameter("@idComentarioEstado", idComentarioEstado);
			arrParams[12] = new SqlParameter("@idSubTipoServico", idSubTipoServico);
			arrParams[13] = new SqlParameter("@inIdEstadoServicoOriginal", idEstadoServicoOriginal);
			arrParams[14] = new SqlParameter("@conforme", conforme);
			arrParams[15] = new SqlParameter("@bDeslocacao", bDeslocacao);
            arrParams[16] = new SqlParameter("@idNivelPrioridade", idNivelPrioridade);
            arrParams[17] = new SqlParameter("@linguaCertificado", linguaCertificado);
            arrParams[18] = new SqlParameter("@tipoDeslocacao", tipoDeslocacao);
            arrParams[19] = new SqlParameter("@bRejeitado", bRejeitado);
            arrParams[20] = new SqlParameter("@numEtiquetaIPQ", numEtiquetaIPQ);
			arrParams[21] = new SqlParameter("@idSubTipoEquipamento", idSubTipoEquipamento);


			//isto estava noutra funcao com return value, corteio o return value. dm jan.2008
			return GERAL.clsDataAccess.intExecuteNonQuerySP(myConnection, myCommand, "stpUpdServicoInPastaEnsaio", arrParams); 
		}

		// ==========================================================================================
		// Linhas de Serviço
		// ==========================================================================================

		// ==========================================================================================
		// Função que insere uma Linha de Serviço
		// ==========================================================================================
		public int InsertServicoLinha(SqlConnection myConnection, SqlCommand myCommand, string idServico, string numPecas, string numPontos, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inNumPecas", numPecas);
			arrParams[2] = new SqlParameter("@inNumPontos", numPontos);
			arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[4] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpInsServicoLinha", arrParams); 
		}

		// ==========================================================================================
		// Função que actualiza uma Linha de Serviço
		// ==========================================================================================
		public int UpdateServicoLinha(SqlConnection myConnection, SqlCommand myCommand, string idServicoLinha, string idServico, string numPecas, string numPontos, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[6];

			arrParams[0] = new SqlParameter("@inIdServicoLinha", idServicoLinha);
			arrParams[1] = new SqlParameter("@inIdServico", idServico);
			arrParams[2] = new SqlParameter("@inNumPecas", numPecas);
			arrParams[3] = new SqlParameter("@inNumPontos", numPontos);
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpUpdServicoLinha", arrParams); 
		}

		// ==========================================================================================
		// Função que apaga uma Linha de Serviço
		// ==========================================================================================
		public int DeleteServicoLinha(SqlConnection myConnection, SqlCommand myCommand, string idServicoLinha, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@inIdServicoLinha", idServicoLinha);
			arrParams[1] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpDelServicoLinha", arrParams); 
		}


		
		// ==========================================================================================
		// Certificados
		// ==========================================================================================

		// ==========================================================================================
		// Função que insere um Certificado
		// ==========================================================================================
		public int InsertCertificado(SqlConnection myConnection, SqlCommand myCommand, string idServico, string idTipoCertificado, string idFuncionarioEmitiu, string dtCertificado, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[6];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[2] = new SqlParameter("@inIdFuncionarioEmitiu", idFuncionarioEmitiu);
			try
			{
				arrParams[3] = new SqlParameter("@inDtCertificado", DateTime.Parse(dtCertificado));
			}
			catch
			{
				arrParams[3] = new SqlParameter("@inDtCertificado", null);
			}

			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpInsCertificado", arrParams);
		}

		// ==========================================================================================	
		// Função que actualiza um Certificado //deixará de ser usado
		// ==========================================================================================
		public int UpdateCertificado(SqlConnection myConnection, SqlCommand myCommand, string idCertificado, string idServico, string idTipoCertificado, string idFuncionarioEmitiu, string dtCertificado, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inIdCertificado", idCertificado);
			arrParams[1] = new SqlParameter("@inIdServico", idServico);
			arrParams[2] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[3] = new SqlParameter("@inIdFuncionarioEmitiu", idFuncionarioEmitiu);
			try
			{
				arrParams[4] = new SqlParameter("@inDtCertificado", DateTime.Parse(dtCertificado));
			}
			catch
			{
				arrParams[4] = new SqlParameter("@inDtCertificado", null);
			}
			arrParams[5] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[6] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpUpdCertificado", arrParams); 
		}

		// ==========================================================================================
		// Função que apaga um Certificado
		// ==========================================================================================
		public int DeleteCertificado(SqlConnection myConnection, SqlCommand myCommand, string idCertificado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@inIdCertificado", idCertificado);
			arrParams[1] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpDelCertificado", arrParams); 
		}

		// ==========================================================================================
		// Funcao que devolve todos os Certificados associados a um Serviço
		// ==========================================================================================
		public DataTable dtCertificadosByIdServico(string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetCertificadosByIdServico", arrParams); 
           
		}

		
		//*******************************************************************************************************
		//*************************************** LISTAS ********************************************************
		//*******************************************************************************************************

		

	}
}
