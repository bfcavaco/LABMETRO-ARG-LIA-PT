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
    /// Summary description for TaxaServicoBD
    /// </summary>
    /// 


    public class TaxaServicoDetails
    {
    }

    public class TaxaServicoBD
    {
        public TaxaServicoBD()
        {
        }

        //===================================================================================================
        //DETALHES DE UM Taxas de Serviço POR ID
        //===================================================================================================
        public DataTable DTTaxaServicoDetails(string idTaxaServico)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);

            return clsDataAccess.SPExecuteDTParams("stpGetTaxaServicoById", arrParams);
        }

        //===================================================================================================
        // LISTA DE ORÇAMCAMENTOs  com base nos critérios de pesquisa
        //===================================================================================================
        public DataTable DTTaxaServicos(string empresa, string tipoEquipamento, string idEstadoTaxaServico, string refTaxaServico, string idEmpresa, string idFuncionario)
        {
            if (refTaxaServico != null) refTaxaServico = refTaxaServico.Trim();
            SqlParameter[] arrParams = new SqlParameter[6];

            arrParams[0] = new SqlParameter("@inEmpresa", empresa);
            arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
            arrParams[2] = new SqlParameter("@inIdEstadoTaxaServico", idEstadoTaxaServico);
            arrParams[3] = new SqlParameter("@inRefTaxaServico", refTaxaServico);
            arrParams[4] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[5] = new SqlParameter("@idFuncionario", idFuncionario);

            return clsDataAccess.SPExecuteDTParams("stpGetListTaxaServicos", arrParams);

        }

        //===================================================================================================
        // LISTA DE ORÇAMCAMENTOs  com base nos critérios de pesquisa mais alargados 
        //===================================================================================================
        public DataTable DTTaxaServicos(string empresa, string tipoEquipamento, string idEstadoTaxaServico, string refTaxaServico, string idEmpresa, string dtInicio, string dtFim, string valorMinimo, string calibracaoExterna)
        {
            if (refTaxaServico != null) refTaxaServico = refTaxaServico.Trim();

            SqlParameter[] arrParams = new SqlParameter[9];

            arrParams[0] = new SqlParameter("@inEmpresa", empresa);
            arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
            arrParams[2] = new SqlParameter("@inIdEstadoTaxaServico", idEstadoTaxaServico);
            arrParams[3] = new SqlParameter("@inRefTaxaServico", refTaxaServico);
            arrParams[4] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[5] = new SqlParameter("@inDtInicio", dtInicio);
            arrParams[6] = new SqlParameter("@inDtFim", dtFim);
            if (valorMinimo != null && valorMinimo != "")
            {
                arrParams[7] = new SqlParameter("@inValorMin", double.Parse(valorMinimo));
            }
            else
            {
                arrParams[7] = new SqlParameter("@inValorMin", "");
            }

            arrParams[8] = new SqlParameter("@inCalibracaoExterna", calibracaoExterna);


            return clsDataAccess.SPExecuteDTParams("stpGetListTaxaServicos", arrParams);

        }
        // Funcao que devolve todas as Requisições de uma dada Empresa
        // que podem ser adicionadas a um Serviço de um BRE (incompletas)
        public SqlDataReader DRGetTaxaServicosByEmpresa(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListTaxaServicos", arrParams);


        }

        //===================================================================================================
        // calcula a data de Validade de um Taxas de Serviço com base na data em que o Taxas de Serviço foi realizado
        //===================================================================================================
        public string strDataValidadeTaxaServico(string dtTaxaServico)
        {
            return ""; //isto nao se aplica aqui nem sei se ainda se aplica nos orçaments
        }

        public object mesesValidadeTaxaServico()
        {
            return "";//nao se aplica
        }

        //===================================================================================================
        // INSERE UM Taxas de Serviço, DEVOLVE O ID DO MESMO
        //===================================================================================================
        public int InsertTaxaServico(string idEmpresa, string idContacto, string idFuncionarioRespTecnico, string idObsLocalExecucao, string idEstadoTaxaServico, string referenciaCliente, string dtPedido, string tempoExecucao, string valorAjudasCustoDeslocacoes, string valorTotal, string dtTaxaServico, string dtValidade, string calibracaoExterna, string localidadeCalibracao, string observacoes, string username, string bTotal,  string bDesconto, string bDeslocacoes, string idCondicoesPagamento, string nomeFicheiro, string idPTComercial, string refPSI, string ccMail, string ValorIva)
        {
            SqlParameter[] arrParams = new SqlParameter[25];

            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[1] = new SqlParameter("@inIdContacto", idContacto);
            arrParams[2] = new SqlParameter("@inIdFuncionarioRespTecnico", idFuncionarioRespTecnico);
            arrParams[3] = new SqlParameter("@inIdObsLocalExecucao", idObsLocalExecucao);
            arrParams[4] = new SqlParameter("@inIdEstadoTaxaServico", idEstadoTaxaServico);
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
                arrParams[10] = new SqlParameter("@inDtTaxaServico", DateTime.Parse(dtTaxaServico));
            }
            catch
            {
                arrParams[10] = new SqlParameter("@inDtTaxaServico", null);
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
            arrParams[17] = new SqlParameter("@refPSI", refPSI);
            arrParams[18] = new SqlParameter("@bDesconto", clsGeral.ConvertStringToBool(bDesconto));
            arrParams[19] = new SqlParameter("@bDeslocacoes", clsGeral.ConvertStringToBool(bDeslocacoes));
            arrParams[20] = new SqlParameter("@idCondicoesPagamento", idCondicoesPagamento);
            arrParams[21] = new SqlParameter("@nomeFicheiro", nomeFicheiro);
            arrParams[22] = new SqlParameter("@idPTComercial", idPTComercial);
            arrParams[23] = new SqlParameter("@ccMail", ccMail);
            arrParams[24] = new SqlParameter("@ValorIva", ValorIva);


            return clsDataAccess.ExecuteNonQuerySPOutput("stpInsTaxaServico", arrParams);
        }

        public string refTaxaServico(string idTaxaServico)
        {
            string strSQL = "SELECT refTaxaServico FROM TaxaServico WHERE idTaxaServico = " + idTaxaServico;
            return GERAL.clsDataAccess.myExecuteScalar(strSQL).ToString();
        }

        //===================================================================================================
        // UPDATE Taxas de Serviço RETORNA INT (???)
        //===================================================================================================
        public int UpdateTaxaServico(string idTaxaServico, string idEmpresa, string idContacto, string idFuncionarioRespTecnico, string idObsLocalExecucao, string idEstadoTaxaServico, string referenciaCliente, string dtPedido, string tempoExecucao, string valorAjudasCustoDeslocacoes, string valorTotal, string dtTaxaServico, string dtValidade, string calibracaoExterna, string localidadeCalibracao, string observacoes, string username, string bTotal, string bDesconto, string bDeslocacoes, string idCondicoesPagamento, string nomeFicheiro, string idPTComercial, string refPSI, string ccMail, string ValorIva)
        {

            //MARTELADA ACTUALIZAR ESTADO Taxas de Serviço LINHA CONFORME ESTADO Taxas de Serviço

            string strSQL = "";
            if (idEstadoTaxaServico == "4") //ACEITE
            {
                strSQL = "UPDATE TaxaServicoLINHA SET idEstadoTaxaServicoLinha = 2 where idTaxaServico = " + idTaxaServico; //ACEITE
                GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            }

            if (idEstadoTaxaServico == "5") //REPROVADO
            {
                strSQL = "UPDATE TaxaServicoLINHA SET idEstadoTaxaServicoLinha = 3 where idTaxaServico = " + idTaxaServico; //REPROVADO
                GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);
            }



            SqlParameter[] arrParams = new SqlParameter[26];

            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[2] = new SqlParameter("@inIdContacto", idContacto);
            arrParams[3] = new SqlParameter("@inIdFuncionarioRespTecnico", idFuncionarioRespTecnico);
            arrParams[4] = new SqlParameter("@inIdObsLocalExecucao", idObsLocalExecucao);
            arrParams[5] = new SqlParameter("@inIdEstadoTaxaServico", idEstadoTaxaServico);
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
                arrParams[11] = new SqlParameter("@inDtTaxaServico", DateTime.Parse(dtTaxaServico));
            }
            catch
            {
                arrParams[11] = new SqlParameter("@inDtTaxaServico", null);
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
            arrParams[18] = new SqlParameter("@refPSI", refPSI);
            arrParams[19] = new SqlParameter("@bDesconto", clsGeral.ConvertStringToBool(bDesconto));
            arrParams[20] = new SqlParameter("@bDeslocacoes", clsGeral.ConvertStringToBool(bDeslocacoes));
            arrParams[21] = new SqlParameter("@idCondicoesPagamento", idCondicoesPagamento);
            arrParams[22] = new SqlParameter("@nomeFicheiro", nomeFicheiro);
            arrParams[23] = new SqlParameter("@idPTComercial", idPTComercial);
            arrParams[24] = new SqlParameter("@ccMail", ccMail);
            arrParams[25] = new SqlParameter("@ValorIva", ValorIva);

            return clsDataAccess.ExecuteNonQuerySP("stpUpdTaxaServico", arrParams); //sem return value
            //isto nao retorna o num de linhas pois a stored procedura baralha tudo...
            //se calhar tenho de tirar o nocoutn... ou entao n da mesmo

        }

        //===================================================================================================
        // DATAREADER COM HISTORICO DE ESTADOS POR ID Taxas de Serviço
        //===================================================================================================
        public SqlDataReader DRGetTaxaServicoHistoricoEstados(string idTaxaServico)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);

            return clsDataAccess.SPExecuteDRParams("stpGetTaxaServicoHistoricoEstadosById", arrParams);
        }

        //===================================================================================================
        // CRIA NOVA VERSÃO DE Taxas de Serviço E DEVOVLE O ID DO MESMO
        //===================================================================================================
        public int InsertTaxaServicoNovaVersao(string idTaxaServico, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inUsername", username);

            return clsDataAccess.ExecuteNonQuerySPOutput("stpInsTaxaServicoNovaVersao", arrParams);
        }
        //===================================================================================================
        //=CRIA REPLICA DE Taxas de Serviço E DEVOVLE O ID DO MESMO
        //===================================================================================================
        public int InsertTaxaServicoReplica(string idTaxaServico, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inUsername", username);

            return clsDataAccess.ExecuteNonQuerySPOutput("stpInsTaxaServicoReplica", arrParams);
        }

        //===================================================================================================
        // ==================================================================================================
        // LINHAS DE Taxas de Serviço
        // ==================================================================================================
        //===================================================================================================


        //===================================================================================================
        // INSERE UMA LINHA DE Taxas de Serviço
        //===================================================================================================
        public int InsertTaxaServicoLinha(string idTaxaServico, string idTipoServico, string idTipoEquipamento, string quantidade, string descricaoEquipamento, string asterisco, string valorUnitario, string valorLinha, string username, string percDescontoLinha)
        {
            SqlParameter[] arrParams = new SqlParameter[10];

            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inIdTipoServico", idTipoServico);
            arrParams[2] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
            arrParams[3] = new SqlParameter("@inQuantidade", quantidade);
            arrParams[4] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
            arrParams[5] = new SqlParameter("@inAsterisco", asterisco);
          

            arrParams[6] = new SqlParameter("@inValorUnitario", clsGeral.ConvertStringToDouble(valorUnitario));
            arrParams[7] = new SqlParameter("@inValorLinha", clsGeral.ConvertStringToDouble(valorLinha));
            arrParams[8] = new SqlParameter("@inUsername", username);
            arrParams[9] = new SqlParameter("@inPercDescontoLinha", percDescontoLinha);

            return clsDataAccess.ExecuteNonQuerySP("stpInsTaxaServicoLinha", arrParams);
        }

        //===================================================================================================
        // UPDATE UMA LINHA DE Taxas de Serviço
        //===================================================================================================
        public int UpdateTaxaServicoLinha(string idTaxaServicoLinha, string idTaxaServico, string idTipoServico, string idTipoEquipamento, string quantidade, string descricaoEquipamento, string asterisco,   string valorUnitario, string valorLinha, string username, string percDescontoLinha, string idEstadoTaxaServicoLinha, string idRazaoTaxaServicoLinha)
        {
            SqlParameter[] arrParams = new SqlParameter[13];

            arrParams[0] = new SqlParameter("@inIdTaxaServicoLinha", idTaxaServicoLinha);
            arrParams[1] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[2] = new SqlParameter("@inIdTipoServico", idTipoServico);
            arrParams[3] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
            arrParams[4] = new SqlParameter("@inQuantidade", quantidade);
            arrParams[5] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
            arrParams[6] = new SqlParameter("@inAsterisco", asterisco);
            arrParams[7] = new SqlParameter("@inValorUnitario", clsGeral.ConvertStringToDouble(valorUnitario));
            arrParams[8] = new SqlParameter("@inValorLinha", clsGeral.ConvertStringToDouble(valorLinha));
            arrParams[9] = new SqlParameter("@inUsername", username);
            arrParams[10] = new SqlParameter("@inPercDescontoLinha", percDescontoLinha);
            arrParams[11] = new SqlParameter("@idEstadoTaxaServicoLinha", idEstadoTaxaServicoLinha);
            arrParams[12] = new SqlParameter("@idRazaoTaxaServicoLinha", idRazaoTaxaServicoLinha);
           
            
            return clsDataAccess.ExecuteNonQuerySP("stpUpdTaxaServicoLinha", arrParams);
        }

        //===================================================================================================
        // DELETE UMA LINHA DE Taxas de Serviço
        //===================================================================================================
        public int DeleteTaxaServicoLinha(string idTaxaServicoLinha, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@inIdTaxaServicoLinha", idTaxaServicoLinha);
            arrParams[1] = new SqlParameter("@inUsername", username);

            return clsDataAccess.ExecuteNonQuerySP("stpDelTaxaServicoLinha", arrParams);
        }

        //===================================================================================================
        // DATATABLE COM TODAS AS LINHAS DE UM Taxas de Serviço
        //===================================================================================================
        public DataTable DTLinhasTaxaServicoByIdTaxaServico(string idTaxaServico)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            return clsDataAccess.SPExecuteDTParams("stpGetLinhasTaxaServicoByIdTaxaServico", arrParams);
        }

        //===================================================================================================
        // ==========================================================================================
        // TaxaServicoComentario
        // ==========================================================================================
        //===================================================================================================

        //===================================================================================================
        //INSERT DE UM COMENTÁRIO DE Taxas de Serviço
        //===================================================================================================

        public int InsertTaxaServicoComentario(string idTaxaServico, string descricao, string asterisco, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[4];

            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inDescricao", descricao);
            arrParams[2] = new SqlParameter("@inAsterisco", asterisco);
            arrParams[3] = new SqlParameter("@inUsername", username);

            return clsDataAccess.ExecuteNonQuerySP("stpInsTaxaServicoComentario", arrParams);
        }

        //===================================================================================================
        //UPDATE DE UM COMENTÁRIO DE Taxas de Serviço
        //===================================================================================================
        public int UpdateTaxaServicoComentario(string idTaxaServicoComentario, string idTaxaServico, string descricao, string asterisco, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[5];

            arrParams[0] = new SqlParameter("@inIdTaxaServicoComentario", idTaxaServicoComentario);
            arrParams[1] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[2] = new SqlParameter("@inDescricao", descricao);
            arrParams[3] = new SqlParameter("@inAsterisco", asterisco);
            arrParams[4] = new SqlParameter("@inUsername", username);

            return clsDataAccess.ExecuteNonQuerySP("stpUpdTaxaServicoComentario", arrParams);
        }


        //=================================================================================================	
        // DELETE DE UM COMENTÁRIO DE Taxas de Serviço
        //===================================================================================================
        public int DeleteTaxaServicoComentario(string idTaxaServicoComentario, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inIdTaxaServicoComentario", idTaxaServicoComentario);
            arrParams[1] = new SqlParameter("@inUsername", username);

            return clsDataAccess.ExecuteNonQuerySP("stpDelTaxaServicoComentario", arrParams);
        }

        //===================================================================================================
        // DATATABLE DE TODOS OS COMENTÁRIOS DE UM Taxas de Serviço
        //===================================================================================================
        public DataTable DTComentariosTaxaServicoByIdTaxaServico(string idTaxaServico)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);

            return clsDataAccess.SPExecuteDTParams("stpGetComentariosTaxaServicoByIdTaxaServico", arrParams);
        }
        //===================================================================================================
        // ==========================================================================================
        // COMENTÁRIO TIPO
        // ==========================================================================================
        //===================================================================================================

        //INSERT DE Comentário Tipo de Taxas de Serviços e devolve a msg de erro
        //===================================================================================================
        public string InsertTaxaServicoComentarioTipo(string descricao, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@inDescricao", descricao);
            arrParams[1] = new SqlParameter("@inUsername", username);

            int retValue = clsDataAccess.SPExecuteNonQueryRV("stpInsTaxaServicoComentarioTipo", arrParams);

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
        public string UpdateTaxaServicoComentarioTipo(string idTaxaServicoComentarioTipo, string descricao, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inIdTaxaServicoComentarioTipo", idTaxaServicoComentarioTipo);
            arrParams[1] = new SqlParameter("@inDescricao", descricao);
            arrParams[2] = new SqlParameter("@inUsername", username);

            int retValue = clsDataAccess.SPExecuteNonQueryRV("stpUpdTaxaServicoComentarioTipo", arrParams);

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
        // DELETE DE UM Comentário Tipo de Taxas de Serviços e devolve a msg de erro
        //===================================================================================================
        public string DeleteTaxaServicoComentarioTipo(string idTaxaServicoComentarioTipo, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inIdTaxaServicoComentarioTipo", idTaxaServicoComentarioTipo);
            arrParams[1] = new SqlParameter("@inUsername", username);

            int retValue = clsDataAccess.SPExecuteNonQueryRV("stpDelTaxaServicoComentarioTipo", arrParams);

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
        public SqlDataReader DRTaxaServicoComentarioTipoById(string idTaxaServicoComentarioTipo)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdTaxaServicoComentarioTipo", idTaxaServicoComentarioTipo);

            return clsDataAccess.SPExecuteDRParams("stpGetTaxaServicoComentarioTipoById", arrParams);
        }

        //===================================================================================================
        // RETORNA UMA LISTA DE COMENTÁRIOS TIPO
        //===================================================================================================
        public DataTable FillComentariosTipo(string strSortField, string strSortDirection)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inSortField", strSortField);
            arrParams[1] = new SqlParameter("@inSortDirection", strSortDirection);

            return clsDataAccess.SPExecuteDTParams("stpGetListTaxaServicoComentariosTipo", arrParams);

        }

        //===================================================================================================
        //LISTA DE EQUIPAMENTOS POR CRITERIOS DE PESQUISA
        //===================================================================================================
        public DataTable DTEquipamentosDeTaxaServico(string strEquipamento, string strEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inDescricaoEquipamento", strEquipamento);
            arrParams[1] = new SqlParameter("@inNomeEmpresa", strEmpresa);

            return clsDataAccess.SPExecuteDTParams("stpGetEquipamentosInTaxaServicos", arrParams);
        }

        //===================================================================================================
        //MARCA UM Taxas de Serviço COMO ENVIADO
        //===================================================================================================
        public int setTaxaServicoAsEnviado(string idTaxaServico, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            arrParams[0] = new SqlParameter("@inIdTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inUsername", username);

            return clsDataAccess.SPExecuteNonQueryRV("stpUpdTaxaServicoParaEnviado", arrParams);
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
        public DataSet DSOrcamFax(string idTaxaServico)
        {
            LabMetro.DSTaxaServico ds = new DSTaxaServico();


            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objCmd.CommandType = CommandType.Text;

                objCmd.Connection = objConn;

                SqlDataAdapter DA = new SqlDataAdapter(objCmd);

                objCmd.CommandText = "SELECT  dbo.TaxaServico.idTaxaServico, dbo.Funcionario.nomeAbreviado AS respTecnico, dbo.TaxaServico.refTaxaServico + ' - ' + CAST(dbo.TaxaServico.versao AS VARCHAR) AS refTaxaServico, dbo.TaxaServico.dtTaxaServico,  dbo.Empresa.nome AS empresa, CASE WHEN dbo.Titulo.descricao IS NULL THEN dbo.Contacto.nome ELSE dbo.Titulo.descricao + ' ' + dbo.Contacto.nome END AS contacto, dbo.Contacto.departamento,  dbo.Contacto.faxEmpresa, dbo.TaxaServico.referenciaCliente,  dbo.Parametrizacao.percIVA,  dbo.ObsLocalExecucao.descricao AS obsLocalExecucao, dbo.TaxaServico.valorAjudasCustoDeslocacoes, dbo.TaxaServico.localidadeCalibracao, dbo.TaxaServico.tempoExecucao, dbo.TaxaServico.bTotal, dbo.TaxaServico.bDesconto, dbo.TaxaServico.bDeslocacoes,dbo.TaxaServico.idcondicoesPagamento,sapCP.descCodigoCondPagam as condicoesPagamento FROM dbo.TaxaServico INNER JOIN   dbo.Empresa ON dbo.TaxaServico.idEmpresa = dbo.Empresa.idEmpresa LEFT JOIN  dbo.Funcionario ON dbo.TaxaServico.idFuncionarioRespTecnico = dbo.Funcionario.idFuncionario INNER JOIN   dbo.ObsLocalExecucao ON dbo.TaxaServico.idObsLocalExecucao = dbo.ObsLocalExecucao.idObsLocalExecucao LEFT OUTER JOIN   dbo.Contacto ON dbo.TaxaServico.idContacto = dbo.Contacto.idContacto LEFT OUTER JOIN  dbo.Titulo ON dbo.Contacto.idTitulo = dbo.Titulo.idTitulo INNER JOIN sap_CodigoCondPagam sapCP ON TaxaServico.idCondicoesPagamento = sapCP.idCodigoCondPagam CROSS JOIN   dbo.Parametrizacao WHERE dbo.TaxaServico.idTaxaServico = " + idTaxaServico;

                try
                {
                    DA.Fill(ds, "TaxaServico");
                }
                catch
                {
                }

                objCmd.CommandText = "SELECT idTaxaServicoLinha, quantidade, valorLinha, valorUnitario, idTaxaServico, asterisco, descricaoEquipamento, percDescontoLinha FROM TaxaServicoLinha WHERE TaxaServicoLinha.idTaxaServico = " + idTaxaServico;


                try
                {
                    DA.Fill(ds, "TaxaServicoLinha");
                }
                catch
                {
                }

                objCmd.CommandText = "SELECT * from TaxaServicoComentario where idTaxaServico = " + idTaxaServico;


                try
                {
                    DA.Fill(ds, "TaxaServicoComentario");
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
        public DataSet myDsTaxaServicoFax(int idTaxaServico)
        {




            DataAccessLayer.TaxaServicoFax ds = new LabMetro.DataAccessLayer.TaxaServicoFax();
            



            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objCmd.CommandType = CommandType.Text;

                objCmd.Connection = objConn;

                SqlDataAdapter DA = new SqlDataAdapter(objCmd);

                objCmd.CommandText = "SELECT  dbo.TaxaServico.idTaxaServico, dbo.Funcionario.nomeAbreviado AS respTecnico, dbo.TaxaServico.refTaxaServico + ' - ' + CAST(dbo.TaxaServico.versao AS VARCHAR) AS refTaxaServico, dbo.TaxaServico.dtTaxaServico,  dbo.Empresa.nome AS empresa, CASE WHEN dbo.Titulo.descricao IS NULL THEN dbo.Contacto.nome ELSE dbo.Titulo.descricao + ' ' + dbo.Contacto.nome END AS contacto, dbo.Contacto.departamento,  dbo.Contacto.faxEmpresa, dbo.TaxaServico.referenciaCliente, dbo.Parametrizacao.percIVA,  dbo.ObsLocalExecucao.descricao AS obsLocalExecucao, dbo.TaxaServico.valorAjudasCustoDeslocacoes, dbo.TaxaServico.localidadeCalibracao, dbo.TaxaServico.tempoExecucao, dbo.TaxaServico.bTotal, dbo.TaxaServico.bDesconto, dbo.TaxaServico.bDeslocacoes,dbo.TaxaServico.idcondicoesPagamento,sapCP.descCodigoCondPagam as condicoesPagamento, dbo.Empresa.numObra,  taxaservico.ValorIva FROM dbo.TaxaServico INNER JOIN   dbo.Empresa ON dbo.TaxaServico.idEmpresa = dbo.Empresa.idEmpresa LEFT JOIN  dbo.Funcionario ON dbo.TaxaServico.idFuncionarioRespTecnico = dbo.Funcionario.idFuncionario INNER JOIN   dbo.ObsLocalExecucao ON dbo.TaxaServico.idObsLocalExecucao = dbo.ObsLocalExecucao.idObsLocalExecucao LEFT OUTER JOIN   dbo.Contacto ON dbo.TaxaServico.idContacto = dbo.Contacto.idContacto LEFT OUTER JOIN  dbo.Titulo ON dbo.Contacto.idTitulo = dbo.Titulo.idTitulo INNER JOIN sap_CodigoCondPagam sapCP ON TaxaServico.idCondicoesPagamento = sapCP.idCodigoCondPagam CROSS JOIN   dbo.Parametrizacao WHERE dbo.TaxaServico.idTaxaServico = " + idTaxaServico;

                try
                {
                    DA.Fill(ds.TaxaServico);
                    
                }
                catch
                {
                }

                objCmd.CommandText = "SELECT tipoServico.descricao as tiposervico, idTaxaServicoLinha, quantidade, valorLinha, valorUnitario, idTaxaServico, asterisco, descricaoEquipamento, percDescontoLinha, tiposervico.descricao as tipoServico, tipoEquipamento.descricao as tipoEquipamento FROM TaxaServicoLinha left join tipoServico on TaxaServicolinha.idTipoServico = tipoServico.idTipoServico left join tipoEquipamento on TaxaServicoLinha.idTipoEquipamento = tipoEquipamento.idTipoEquipamento WHERE TaxaServicoLinha.idTaxaServico = " + idTaxaServico + " ORDER BY idTaxaServicoLinha";


                try
                {
                    DA.Fill(ds.TaxaServicoLinha);
                    //DA.Fill(ds, "vTaxaServicoLinha");
                }
                catch (Exception)
                {

                   
                }


              

                DA.Dispose();
                DA = null;
            }
            return ds;

        }

        // Returns strongly typed (using Generics) collection of ListItems filtered by category, 
        // and paged using a startrow and page size index (both of these are automatically
        // calculated by the GridView).
        //

        public List<System.Web.UI.WebControls.ListItem> ListaEmpresasParaTaxaServico(string searchClause)
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

        public void ApagaFicheiroPedidoTaxaServico(string idTaxaServico, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];

            arrParams[0] = new SqlParameter("@idTaxaServico", idTaxaServico);
            arrParams[1] = new SqlParameter("@inUsername", username);

            GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdTaxaServicoFicheiro", arrParams);
        }

    }
}
