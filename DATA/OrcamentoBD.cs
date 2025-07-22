using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using LabMetro.GERAL;
using System.Web;
using System.Collections.Generic;


namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for OrcamentoBD
	/// </summary>
	/// 

	
	public class OrcamentoDetails
	{
	}

	public class OrcamentoBD
	{
		public OrcamentoBD()
		{
		}

		//===================================================================================================
		//DETALHES DE UM ORÇAMENTO POR ID
		//===================================================================================================
		public DataTable DTOrcamentoDetails(string idOrcamento)
		{
			SqlParameter[] arrParams = new SqlParameter[1]; 
			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
         
			return clsDataAccess.SPExecuteDTParams("stpGetOrcamentoById", arrParams); 
		}

		//===================================================================================================
        // LISTA DE ORÇAMCAMENTOs  com base nos critérios de pesquisa
		//===================================================================================================
        public DataTable DTOrcamentos(string empresa, string tipoEquipamento, string idEstadoOrcamento, string refOrcamento, string idEmpresa, string idFuncionario)
        {
			if(refOrcamento != null) refOrcamento = refOrcamento.Trim(); 
            SqlParameter[] arrParams = new SqlParameter[6];
     
            arrParams[0] = new SqlParameter("@inEmpresa", empresa);
            arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
            arrParams[2] = new SqlParameter("@inIdEstadoOrcamento", idEstadoOrcamento);
			arrParams[3] = new SqlParameter("@inRefOrcamento", refOrcamento);
			arrParams[4] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[5] = new SqlParameter("@idFuncionario", idFuncionario);
         
            return clsDataAccess.SPExecuteDTParams("stpGetListOrcamentos", arrParams); 
        
        }

		//===================================================================================================
		// LISTA DE ORÇAMCAMENTOs  com base nos critérios de pesquisa mais alargados 
		//===================================================================================================
		public DataTable DTOrcamentos(string empresa, string tipoEquipamento, string idEstadoOrcamento, string refOrcamento, string idEmpresa, string dtInicio, string dtFim,string valorMinimo, string calibracaoExterna, string idFuncionario, string idRazaoCliente, string bFollowup)
		{
			if(refOrcamento != null) refOrcamento = refOrcamento.Trim(); 
			
			SqlParameter[] arrParams = new SqlParameter[12];
     
			arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
			arrParams[2] = new SqlParameter("@inIdEstadoOrcamento", idEstadoOrcamento);
			arrParams[3] = new SqlParameter("@inRefOrcamento", refOrcamento);
			arrParams[4] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[5] = new SqlParameter("@inDtInicio", dtInicio);
			arrParams[6] = new SqlParameter("@inDtFim", dtFim);
			if(valorMinimo != null && valorMinimo  != "")
			{
				arrParams[7] = new SqlParameter("@inValorMin", double.Parse(valorMinimo));	
			}
			else
			{	
				arrParams[7] = new SqlParameter("@inValorMin", "")	;
			}

            arrParams[8] = new SqlParameter("@inCalibracaoExterna", calibracaoExterna);
            arrParams[9] = new SqlParameter("@idFuncionario", idFuncionario);

            arrParams[10] = new SqlParameter("@idRazaoCliente", idRazaoCliente);
            arrParams[11] = new SqlParameter("@bFollowup", bFollowup);
         
			return clsDataAccess.SPExecuteDTParams("stpGetListOrcamentos", arrParams); 
        
		}
		// Funcao que devolve todas as Requisições de uma dada Empresa
		// que podem ser adicionadas a um Serviço de um BRE (incompletas)
		public SqlDataReader DRGetOrcamentosByEmpresa(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListOrcamentos", arrParams); 
           
			
		}

		//===================================================================================================
		// calcula a data de Validade de um orçamento com base na data em que o orçamento foi realizado
		//===================================================================================================
		public string strDataValidadeOrcamento(string dtOrcamento)
		{
			string strSQL = "SELECT dbo.udfGetDataValidadeOrcamento('" + dtOrcamento + "')"; 
			return clsDataAccess.myExecuteScalar(strSQL).ToString(); 
		}

		public object mesesValidadeOrcamento()
		{
			string strSQL = "SELECT mesesValidadeOrcamento FROM Parametrizacao"; 
			return clsDataAccess.myExecuteScalar(strSQL); 
		}
		
		//===================================================================================================
		// INSERE UM ORÇAMENTO, DEVOLVE O ID DO MESMO
		//===================================================================================================
		public int InsertOrcamento(string idEmpresa, string idContacto, string idFuncionarioRespTecnico, string idObsLocalExecucao, string idEstadoOrcamento, string referenciaCliente, string dtPedido, string tempoExecucao, string valorAjudasCustoDeslocacoes, string valorTotal, string dtOrcamento, string dtValidade, string calibracaoExterna, string localidadeCalibracao, string observacoes, string username,string bTotal, string mesesValidade, string bDesconto, string bDeslocacoes,string idCondicoesPagamento, string nomeFicheiro, string idPTComercial, string refPSI, string bFollowup)
		{
			SqlParameter[] arrParams = new SqlParameter[25];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdContacto", idContacto);
			arrParams[2] = new SqlParameter("@inIdFuncionarioRespTecnico", idFuncionarioRespTecnico);
			arrParams[3] = new SqlParameter("@inIdObsLocalExecucao", idObsLocalExecucao);
			arrParams[4] = new SqlParameter("@inIdEstadoOrcamento", idEstadoOrcamento);
			arrParams[5] = new SqlParameter("@inReferenciaCliente", referenciaCliente);
			try
			{
				arrParams[6] = new SqlParameter("@inDtPedido", DateTime.Parse(dtPedido));
			}
			catch
			{
				arrParams[6] = new SqlParameter("@inDtPedido", null);
			}
			arrParams[7] = new SqlParameter("@inTempoExecucao", tempoExecucao);
			arrParams[8] = new SqlParameter("@inValorAjudasCustoDeslocacoes", clsGeral.ConvertStringToDouble(valorAjudasCustoDeslocacoes));
			arrParams[9] = new SqlParameter("@inValorTotal", clsGeral.ConvertStringToDouble(valorTotal));
			try
			{
				arrParams[10] = new SqlParameter("@inDtOrcamento", DateTime.Parse(dtOrcamento));
			}
			catch
			{
				arrParams[10] = new SqlParameter("@inDtOrcamento", null);
			}
			try
			{
				arrParams[11] = new SqlParameter("@inDtValidade", DateTime.Parse(dtValidade));
			}
			catch
			{
				arrParams[11] = new SqlParameter("@inDtValidade", null);
			}
			arrParams[12] = new SqlParameter("@inCalibracaoExterna", clsGeral.ConvertStringToBool(calibracaoExterna));
			arrParams[13] = new SqlParameter("@inLocalidadeCalibracao", localidadeCalibracao);
			arrParams[14] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[15] = new SqlParameter("@inUsername", username);
			arrParams[16] = new SqlParameter("@bTotal", clsGeral.ConvertStringToBool(bTotal));
			arrParams[17] = new SqlParameter("@inMesesValidade", mesesValidade);
			arrParams[18] = new SqlParameter("@bDesconto", clsGeral.ConvertStringToBool(bDesconto));
			arrParams[19] = new SqlParameter("@bDeslocacoes", clsGeral.ConvertStringToBool(bDeslocacoes));
			arrParams[20] = new SqlParameter("@idCondicoesPagamento",idCondicoesPagamento);
            arrParams[21] = new SqlParameter("@nomeFicheiro", nomeFicheiro);
            arrParams[22] = new SqlParameter("@idPTComercial", idPTComercial);
            arrParams[23] = new SqlParameter("@refPSI", refPSI);
            arrParams[24] = new SqlParameter("@bFollowup", bFollowup);
			
			return clsDataAccess.ExecuteNonQuerySPOutput("stpInsOrcamento", arrParams);
		}
		
		public string refOrcamento(string idOrcamento)
		{
			string strSQL = "SELECT refOrcamento FROM orcamento WHERE idOrcamento = "+ idOrcamento; 
			return GERAL.clsDataAccess.myExecuteScalar(strSQL).ToString();
		}

		//===================================================================================================
		// UPDATE ORÇAMENTO RETORNA INT (???)
		//===================================================================================================
		public int UpdateOrcamento(string idOrcamento, string idEmpresa, string idContacto, string idFuncionarioRespTecnico, string idObsLocalExecucao, string idEstadoOrcamento, string referenciaCliente, string dtPedido, string tempoExecucao, string valorAjudasCustoDeslocacoes, string valorTotal, string dtOrcamento, string dtValidade, string calibracaoExterna, string localidadeCalibracao, string observacoes, string username,string bTotal, string mesesValidade, string bDesconto, string bDeslocacoes,string idCondicoesPagamento, string nomeFicheiro, string idPTComercial, string refPSI, string bFollowup)
		{
			
            //MARTELADA ACTUALIZAR ESTADO ORÇAMENTO LINHA CONFORME ESTADO ORÇAMENTO

            string strSQL = "";
            if (idEstadoOrcamento == "4") //ACEITE
            {
                strSQL = "UPDATE ORCAMENTOLINHA SET idEstadoOrcamentoLinha = 2 where idOrcamento = " + idOrcamento; //ACEITE
                GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            }

            if (idEstadoOrcamento == "5") //REPROVADO
            {
                strSQL = "UPDATE ORCAMENTOLINHA SET idEstadoOrcamentoLinha = 3 where idOrcamento = " + idOrcamento; //REPROVADO
                GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            }
            

            
            SqlParameter[] arrParams = new SqlParameter[26];

			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[2] = new SqlParameter("@inIdContacto", idContacto);
			arrParams[3] = new SqlParameter("@inIdFuncionarioRespTecnico", idFuncionarioRespTecnico);
			arrParams[4] = new SqlParameter("@inIdObsLocalExecucao", idObsLocalExecucao);
			arrParams[5] = new SqlParameter("@inIdEstadoOrcamento", idEstadoOrcamento);
			arrParams[6] = new SqlParameter("@inReferenciaCliente", referenciaCliente);
			try
			{
				arrParams[7] = new SqlParameter("@inDtPedido", DateTime.Parse(dtPedido));
			}
			catch
			{
				arrParams[7] = new SqlParameter("@inDtPedido", null);
			}
			arrParams[8] = new SqlParameter("@inTempoExecucao", tempoExecucao);
			arrParams[9] = new SqlParameter("@inValorAjudasCustoDeslocacoes", clsGeral.ConvertStringToDouble(valorAjudasCustoDeslocacoes));
			arrParams[10] = new SqlParameter("@inValorTotal", clsGeral.ConvertStringToDouble(valorTotal));
			try
			{
				arrParams[11] = new SqlParameter("@inDtOrcamento", DateTime.Parse(dtOrcamento));
			}
			catch
			{
				arrParams[11] = new SqlParameter("@inDtOrcamento", null);
			}
			try
			{
				arrParams[12] = new SqlParameter("@inDtValidade", DateTime.Parse(dtValidade));
			}
			catch
			{
				arrParams[12] = new SqlParameter("@inDtValidade", null);
			}
			arrParams[13] = new SqlParameter("@inCalibracaoExterna", clsGeral.ConvertStringToBool(calibracaoExterna));
			arrParams[14] = new SqlParameter("@inLocalidadeCalibracao", localidadeCalibracao);
			arrParams[15] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[16] = new SqlParameter("@inUsername", username);
			arrParams[17] = new SqlParameter("@bTotal", clsGeral.ConvertStringToBool(bTotal));
			arrParams[18] = new SqlParameter("@inMesesValidade", mesesValidade);
			arrParams[19] = new SqlParameter("@bDesconto", clsGeral.ConvertStringToBool(bDesconto));
			arrParams[20] = new SqlParameter("@bDeslocacoes", clsGeral.ConvertStringToBool(bDeslocacoes));
			arrParams[21] = new SqlParameter("@idCondicoesPagamento",idCondicoesPagamento);
            arrParams[22] = new SqlParameter("@nomeFicheiro", nomeFicheiro);
            arrParams[23] = new SqlParameter("@idPTComercial", idPTComercial);
            arrParams[24] = new SqlParameter("@refPSI", refPSI);
            arrParams[25] = new SqlParameter("@bFollowup", bFollowup);
			return clsDataAccess.ExecuteNonQuerySP("stpUpdOrcamento", arrParams); //sem return value
			//isto nao retorna o num de linhas pois a stored procedura baralha tudo...
			//se calhar tenho de tirar o nocoutn... ou entao n da mesmo

		}

		//===================================================================================================
		// DATAREADER COM HISTORICO DE ESTADOS POR ID ORÇAMENTO
		//===================================================================================================
		public SqlDataReader DRGetOrcamentoHistoricoEstados(string idOrcamento)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
         
			return clsDataAccess.SPExecuteDRParams("stpGetOrcamentoHistoricoEstadosById", arrParams); 
		}

		//===================================================================================================
		// CRIA NOVA VERSÃO DE ORÇAMENTO E DEVOVLE O ID DO MESMO
		//===================================================================================================
		public int InsertOrcamentoNovaVersao(string idOrcamento, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[1] = new SqlParameter("@inUsername", username);

			return clsDataAccess.ExecuteNonQuerySPOutput("stpInsOrcamentoNovaVersao", arrParams);
		}
		//===================================================================================================
		//=CRIA REPLICA DE ORÇAMENTO E DEVOVLE O ID DO MESMO
		//===================================================================================================
		public int InsertOrcamentoReplica(string idOrcamento, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[1] = new SqlParameter("@inUsername", username);

			return clsDataAccess.ExecuteNonQuerySPOutput("stpInsOrcamentoReplica", arrParams);
		}

		//===================================================================================================
		// ==================================================================================================
		// LINHAS DE ORÇAMENTO
		// ==================================================================================================
		//===================================================================================================


		//===================================================================================================
		// INSERE UMA LINHA DE ORÇAMENTO
		//===================================================================================================
		public int InsertOrcamentoLinha(string idOrcamento, string idTipoServico, string idTipoEquipamento, string quantidade, string descricaoEquipamento, string asterisco, string valorUnitario, string valorLinha, string username, string percDescontoLinha)
		{
			SqlParameter[] arrParams = new SqlParameter[10];

			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[1] = new SqlParameter("@inIdTipoServico", idTipoServico);
			arrParams[2] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[3] = new SqlParameter("@inQuantidade", quantidade);
			arrParams[4] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
			arrParams[5] = new SqlParameter("@inAsterisco", asterisco);
			

			arrParams[6] = new SqlParameter("@inValorUnitario", clsGeral.ConvertStringToDouble(valorUnitario));
			arrParams[7] = new SqlParameter("@inValorLinha", clsGeral.ConvertStringToDouble(valorLinha));
			arrParams[8] = new SqlParameter("@inUsername", username);
			arrParams[9] = new SqlParameter("@inPercDescontoLinha", GERAL.clsGeral.ConvertStringToDouble(percDescontoLinha));

			return clsDataAccess.ExecuteNonQuerySP("stpInsOrcamentoLinha", arrParams);
		}

		//===================================================================================================
		// UPDATE UMA LINHA DE ORÇAMENTO
		//===================================================================================================
		public int UpdateOrcamentoLinha(string idOrcamentoLinha, string idOrcamento, string idTipoServico, string idTipoEquipamento, string quantidade, string descricaoEquipamento, string asterisco,   string valorUnitario, string valorLinha, string username, string percDescontoLinha, string idEstadoOrcamentoLinha, string idRazaoOrcamentoLinha)
		{
			SqlParameter[] arrParams = new SqlParameter[13];

			arrParams[0] = new SqlParameter("@inIdOrcamentoLinha", idOrcamentoLinha);
			arrParams[1] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[2] = new SqlParameter("@inIdTipoServico", idTipoServico);
			arrParams[3] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[4] = new SqlParameter("@inQuantidade", quantidade);
			arrParams[5] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
			arrParams[6] = new SqlParameter("@inAsterisco", asterisco);
			
			arrParams[7] = new SqlParameter("@inValorUnitario", clsGeral.ConvertStringToDouble(valorUnitario));
			arrParams[8] = new SqlParameter("@inValorLinha", clsGeral.ConvertStringToDouble(valorLinha));
			arrParams[9] = new SqlParameter("@inUsername", username);
            arrParams[10] = new SqlParameter("@inPercDescontoLinha", GERAL.clsGeral.ConvertStringToDouble(percDescontoLinha));
            arrParams[11] = new SqlParameter("@idEstadoOrcamentoLinha", idEstadoOrcamentoLinha);
            arrParams[12] = new SqlParameter("@idRazaoOrcamentoLinha", idRazaoOrcamentoLinha);
			return clsDataAccess.ExecuteNonQuerySP("stpUpdOrcamentoLinha", arrParams);
		}

		//===================================================================================================
		// DELETE UMA LINHA DE ORÇAMENTO
		//===================================================================================================
		public int DeleteOrcamentoLinha(string idOrcamentoLinha, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@inIdOrcamentoLinha", idOrcamentoLinha);
			arrParams[1] = new SqlParameter("@inUsername", username);

			return clsDataAccess.ExecuteNonQuerySP("stpDelOrcamentoLinha", arrParams);
		}

		//===================================================================================================
		// DATATABLE COM TODAS AS LINHAS DE UM ORÇAMENTO
		//===================================================================================================
		public DataTable DTLinhasOrcamentoByIdOrcamento(string idOrcamento)
		{
			SqlParameter[] arrParams = new SqlParameter[1]; 
			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
			return clsDataAccess.SPExecuteDTParams("stpGetLinhasOrcamentoByIdOrcamento", arrParams); 
		}

		//===================================================================================================
		// ==========================================================================================
		// OrcamentoComentario
		// ==========================================================================================
		//===================================================================================================
		
		//===================================================================================================
		//INSERT DE UM COMENTÁRIO DE ORÇAMENTO
		//===================================================================================================

		public int InsertOrcamentoComentario(string idOrcamento, string descricao, string asterisco , string username)
		{
			SqlParameter[] arrParams = new SqlParameter[4];

			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inAsterisco", asterisco);
			arrParams[3] = new SqlParameter("@inUsername", username);

			return clsDataAccess.ExecuteNonQuerySP("stpInsOrcamentoComentario", arrParams);
		}

		//===================================================================================================
		//UPDATE DE UM COMENTÁRIO DE ORÇAMENTO
		//===================================================================================================
		public int UpdateOrcamentoComentario(string idOrcamentoComentario, string idOrcamento, string descricao, string asterisco , string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdOrcamentoComentario", idOrcamentoComentario);
			arrParams[1] = new SqlParameter("@inIdOrcamento", idOrcamento);
			arrParams[2] = new SqlParameter("@inDescricao", descricao);
			arrParams[3] = new SqlParameter("@inAsterisco", asterisco);
			arrParams[4] = new SqlParameter("@inUsername", username);

			return clsDataAccess.ExecuteNonQuerySP("stpUpdOrcamentoComentario", arrParams);
		}


		//=================================================================================================	
		// DELETE DE UM COMENTÁRIO DE ORÇAMENTO
		//===================================================================================================
		public int DeleteOrcamentoComentario(string idOrcamentoComentario, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
			arrParams[0] = new SqlParameter("@inIdOrcamentoComentario", idOrcamentoComentario);
			arrParams[1] = new SqlParameter("@inUsername", username);

			return clsDataAccess.ExecuteNonQuerySP("stpDelOrcamentoComentario", arrParams);
		}

		//===================================================================================================
		// DATATABLE DE TODOS OS COMENTÁRIOS DE UM ORÇAMENTO
		//===================================================================================================
		public DataTable DTComentariosOrcamentoByIdOrcamento(string idOrcamento)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
         
			return clsDataAccess.SPExecuteDTParams("stpGetComentariosOrcamentoByIdOrcamento", arrParams); 
		}
		//===================================================================================================
		// ==========================================================================================
		// COMENTÁRIO TIPO
		// ==========================================================================================
		//===================================================================================================

		//INSERT DE Comentário Tipo de Orçamentos e devolve a msg de erro
		//===================================================================================================
		public string InsertOrcamentoComentarioTipo(string descricao, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@inDescricao", descricao);
			arrParams[1] = new SqlParameter("@inUsername", username);

			int retValue = clsDataAccess.SPExecuteNonQueryRV("stpInsOrcamentoComentarioTipo", arrParams); 

			if (retValue == 0)
			{
				return clsGeral.ErrorMessage.MSG_INSERT_DB;
			}
			else
			{
				return clsGeral.ErrorMessage.ERR_INSERT;
			}
		}
		//===================================================================================================
		//=UPDATE DE UM COMENTÁRIO TIPO, DEVOVLE MSG DE ERRO
		//===================================================================================================
		public string UpdateOrcamentoComentarioTipo(string idOrcamentoComentarioTipo, string descricao, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[3];
			arrParams[0] = new SqlParameter("@inIdOrcamentoComentarioTipo", idOrcamentoComentarioTipo);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inUsername", username);

			int retValue = clsDataAccess.SPExecuteNonQueryRV("stpUpdOrcamentoComentarioTipo", arrParams); 

			if (retValue == 0)
			{
				return clsGeral.ErrorMessage.MSG_UPDATE_DB;
			}
			else
			{
				return clsGeral.ErrorMessage.ERR_UPDATE;
			}
		}
		//===================================================================================================
		// DELETE DE UM Comentário Tipo de Orçamentos e devolve a msg de erro
		//===================================================================================================
		public string DeleteOrcamentoComentarioTipo(string idOrcamentoComentarioTipo, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
			arrParams[0] = new SqlParameter("@inIdOrcamentoComentarioTipo", idOrcamentoComentarioTipo);
			arrParams[1] = new SqlParameter("@inUsername", username);

			int retValue = clsDataAccess.SPExecuteNonQueryRV("stpDelOrcamentoComentarioTipo", arrParams); 

			if (retValue == 0)
			{
				return clsGeral.ErrorMessage.MSG_DELETE_DB;
			}
			else
			{
				return clsGeral.ErrorMessage.ERR_DELETE;
			}
		}

		//===================================================================================================
		// DEVOLVE um Comentário Tipo  COM BASE NO SEU ID
		//===================================================================================================
		public SqlDataReader DROrcamentoComentarioTipoById(string idOrcamentoComentarioTipo)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inIdOrcamentoComentarioTipo", idOrcamentoComentarioTipo);
         
			return clsDataAccess.SPExecuteDRParams("stpGetOrcamentoComentarioTipoById", arrParams); 
		}

		//===================================================================================================
		// RETORNA UMA LISTA DE COMENTÁRIOS TIPO
		//===================================================================================================
		public DataTable FillComentariosTipo(string strSortField, string strSortDirection)
		{
			SqlParameter[] arrParams = new SqlParameter[2]; 
			arrParams[0] = new SqlParameter("@inSortField", strSortField);
			arrParams[1] = new SqlParameter("@inSortDirection", strSortDirection);

			return clsDataAccess.SPExecuteDTParams("stpGetListOrcamentoComentariosTipo", arrParams); 
           
		}

		//===================================================================================================
        //LISTA DE EQUIPAMENTOS POR CRITERIOS DE PESQUISA
		//===================================================================================================
        public DataTable DTEquipamentosDeOrcamento(string strEquipamento, string strEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[2];            
            arrParams[0] = new SqlParameter("@inDescricaoEquipamento", strEquipamento);
            arrParams[1] = new SqlParameter("@inNomeEmpresa", strEmpresa);

            return clsDataAccess.SPExecuteDTParams("stpGetEquipamentosInOrcamentos", arrParams); 
        }

		//===================================================================================================
        //MARCA UM ORÇAMENTO COMO ENVIADO
		//===================================================================================================
        public int setOrcamentoAsEnviado(string idOrcamento,string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inIdOrcamento", idOrcamento);
            arrParams[1] = new SqlParameter("@inUsername", username);

            return clsDataAccess.SPExecuteNonQueryRV("stpUpdOrcamentoParaEnviado", arrParams);
        }

        ////===================================================================================================
        ////DEVOLVE UMA LISTA DE EMPRESAS SEM FILTRO
        ////===================================================================================================
        //public DataTable DTEmpresas()
        //{
        //    return GERAL.clsDataAccess.SPExecuteDT("stpGetEmpresas"); 
        //}

		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSOrcamFax(string idOrcamento)
		{
			LabMetro.DSOrcamentoFax ds = new DSOrcamentoFax(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.CommandType = CommandType.Text; 

				objCmd.Connection = objConn; 	
				
				SqlDataAdapter DA = new SqlDataAdapter(objCmd);
				
				objCmd.CommandText= "SELECT  dbo.Orcamento.idOrcamento, dbo.Funcionario.nomeAbreviado AS respTecnico, dbo.Orcamento.refOrcamento + ' - ' + CAST(dbo.Orcamento.versao AS VARCHAR) AS refOrcamento, dbo.Orcamento.dtOrcamento,  dbo.Empresa.nome AS empresa, CASE WHEN dbo.Titulo.descricao IS NULL THEN dbo.Contacto.nome ELSE dbo.Titulo.descricao + ' ' + dbo.Contacto.nome END AS contacto, dbo.Contacto.departamento,  dbo.Contacto.faxEmpresa, dbo.Orcamento.referenciaCliente, dbo.Orcamento.validadeMeses as mesesValidadeOrcamento, dbo.Parametrizacao.percIVA,  dbo.ObsLocalExecucao.descricao AS obsLocalExecucao, dbo.Orcamento.valorAjudasCustoDeslocacoes, dbo.Orcamento.localidadeCalibracao, dbo.Orcamento.tempoExecucao, dbo.Orcamento.bTotal, dbo.Orcamento.bDesconto, dbo.Orcamento.bDeslocacoes,dbo.Orcamento.idcondicoesPagamento,sapCP.descCodigoCondPagam as condicoesPagamento FROM dbo.Orcamento INNER JOIN   dbo.Empresa ON dbo.Orcamento.idEmpresa = dbo.Empresa.idEmpresa LEFT JOIN  dbo.Funcionario ON dbo.Orcamento.idFuncionarioRespTecnico = dbo.Funcionario.idFuncionario INNER JOIN   dbo.ObsLocalExecucao ON dbo.Orcamento.idObsLocalExecucao = dbo.ObsLocalExecucao.idObsLocalExecucao LEFT OUTER JOIN   dbo.Contacto ON dbo.Orcamento.idContacto = dbo.Contacto.idContacto LEFT OUTER JOIN  dbo.Titulo ON dbo.Contacto.idTitulo = dbo.Titulo.idTitulo INNER JOIN sap_CodigoCondPagam sapCP ON orcamento.idCondicoesPagamento = sapCP.idCodigoCondPagam CROSS JOIN   dbo.Parametrizacao WHERE dbo.Orcamento.idOrcamento = "+idOrcamento;

				try
				{
					DA.Fill(ds,"vOrcamento"); 
				}
				catch
				{	
				}

				objCmd.CommandText =  "SELECT idOrcamentoLinha, quantidade, valorLinha, valorUnitario, idOrcamento, asterisco, descricaoEquipamento, percDescontoLinha FROM OrcamentoLinha WHERE OrcamentoLinha.idOrcamento = "+idOrcamento;
				

				try
				{
					DA.Fill(ds,"vOrcamentoLinha");
				}
				catch
				{	
				}

				objCmd.CommandText =  "SELECT * from OrcamentoComentario where idOrcamento = "+idOrcamento;
				

				try
				{
					DA.Fill(ds,"OrcamentoComentario"); 
				}
				catch
				{	
				}

				DA.Dispose();
				DA = null; 
			}
			return ds; 
		}



        //===================================================================================================
        //FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
        //===================================================================================================
        public DataSet myDsOrcamentoFax(int idOrcamento)
        {
            DataAccessLayer.dlOrcamento ds = new LabMetro.DataAccessLayer.dlOrcamento(); 

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objCmd.CommandType = CommandType.Text;

                objCmd.Connection = objConn;

                SqlDataAdapter DA = new SqlDataAdapter(objCmd);

                objCmd.CommandText = "SELECT  dbo.Orcamento.idOrcamento, dbo.Funcionario.nomeAbreviado AS respTecnico, dbo.Orcamento.refOrcamento + ' - ' + CAST(dbo.Orcamento.versao AS VARCHAR) AS refOrcamento, dbo.Orcamento.dtOrcamento,  dbo.Empresa.nome AS empresa, CASE WHEN dbo.Titulo.descricao IS NULL THEN dbo.Contacto.nome ELSE dbo.Titulo.descricao + ' ' + dbo.Contacto.nome END AS contacto, dbo.Contacto.departamento,  dbo.Contacto.faxEmpresa, dbo.Orcamento.referenciaCliente, dbo.Orcamento.validadeMeses as mesesValidadeOrcamento, dbo.Parametrizacao.percIVA,  dbo.ObsLocalExecucao.descricao AS obsLocalExecucao, dbo.Orcamento.valorAjudasCustoDeslocacoes, dbo.Orcamento.localidadeCalibracao, dbo.Orcamento.tempoExecucao, dbo.Orcamento.bTotal, dbo.Orcamento.bDesconto, dbo.Orcamento.bDeslocacoes,dbo.Orcamento.idcondicoesPagamento,sapCP.descCodigoCondPagam as condicoesPagamento, dbo.Empresa.numObra FROM dbo.Orcamento INNER JOIN   dbo.Empresa ON dbo.Orcamento.idEmpresa = dbo.Empresa.idEmpresa LEFT JOIN  dbo.Funcionario ON dbo.Orcamento.idFuncionarioRespTecnico = dbo.Funcionario.idFuncionario INNER JOIN   dbo.ObsLocalExecucao ON dbo.Orcamento.idObsLocalExecucao = dbo.ObsLocalExecucao.idObsLocalExecucao LEFT OUTER JOIN   dbo.Contacto ON dbo.Orcamento.idContacto = dbo.Contacto.idContacto LEFT OUTER JOIN  dbo.Titulo ON dbo.Contacto.idTitulo = dbo.Titulo.idTitulo INNER JOIN sap_CodigoCondPagam sapCP ON orcamento.idCondicoesPagamento = sapCP.idCodigoCondPagam CROSS JOIN   dbo.Parametrizacao WHERE dbo.Orcamento.idOrcamento = " + idOrcamento;

                try
                {
                    DA.Fill(ds.dtOrcamentoFax); 
                    //DA.Fill(ds, "vOrcamento");
                }
                catch
                {
                }

                objCmd.CommandText = "SELECT tipoServico.descricao as tiposervico, idOrcamentoLinha, quantidade, valorLinha, valorUnitario, idOrcamento, asterisco, descricaoEquipamento, percDescontoLinha, tiposervico.descricao as tipoServico, tipoEquipamento.descricao as tipoEquipamento FROM OrcamentoLinha left join tipoServico on orcamentolinha.idTipoServico = tipoServico.idTipoServico left join tipoEquipamento on orcamentoLinha.idTipoEquipamento = tipoEquipamento.idTipoEquipamento WHERE OrcamentoLinha.idOrcamento = " + idOrcamento + " ORDER BY idOrcamentoLinha";


                try
                {
                    DA.Fill(ds.dtOrcamentoLinhaFax); 
                    //DA.Fill(ds, "vOrcamentoLinha");
                }
                catch(Exception)
                {
                }

                //objCmd.CommandText = "SELECT * from OrcamentoComentario where idOrcamento = " + idOrcamento;


                //try
                //{
                //    DA.Fill(ds.dtOrcamentoLinhaFax); 
                //    //DA.Fill(ds, "OrcamentoComentario");
                //}
                //catch
                //{
                //}

                DA.Dispose();
                DA = null;
            }
            return ds; 
            
        }

        // Returns strongly typed (using Generics) collection of ListItems filtered by category, 
        // and paged using a startrow and page size index (both of these are automatically
        // calculated by the GridView).
        //

        public List<System.Web.UI.WebControls.ListItem> ListaEmpresasParaOrcamento(string searchClause)
        {

            // List<string> empresas = new List<string>();
            List<System.Web.UI.WebControls.ListItem> empresas = new List<System.Web.UI.WebControls.ListItem>();

            DataAccessLayer.dlEmpresasTableAdapters.EmpresaForOrcamentoTableAdapter adapter = new LabMetro.DataAccessLayer.dlEmpresasTableAdapters.EmpresaForOrcamentoTableAdapter();

            DataAccessLayer.dlEmpresas.EmpresaForOrcamentoDataTable results = adapter.GetEmpresasForOrcamentoByNome(searchClause);


            foreach (DataAccessLayer.dlEmpresas.EmpresaForOrcamentoRow row in results)
            {
                empresas.Add(new System.Web.UI.WebControls.ListItem(row.idEmpresa.ToString(), row.nome));
            }

            return empresas;
        }

        public void ApagaFicheiroPedidoOrcamento(string idOrcamento, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@idOrcamento", idOrcamento);
            arrParams[1] = new SqlParameter("@inUsername", username);

            GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdOrcamentoFicheiro", arrParams);
        }

	}
}
