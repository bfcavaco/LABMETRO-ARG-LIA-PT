using System;
using System.Data;
using System.Data.SqlClient;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for ServicoBD.
	/// </summary>
	/// 

	// Classe que guarda nos seus atributos os detalhes de um Serviço
	public class ServicoDetails
	{
		public string idServico;
		public string refServico;
		public string idBRE;
		public string refBRE;
        public string idBSE;
		public string refBSE;
		public string idRequisicao;
		public string refRequisicao;
		public string idEquipamento;
		public string idFactura;
		public string refFactura;
		public string idFuncionarioEfectuouServico;
		public string funcEfectuouServico;
		public string idFuncionarioTreino;
		public string FuncionarioTreino;
		public string idEstadoServico;
        public string estadoServico; 
		public string idLocalCalibracao;
        public string localCalibracao;
		public string idTipoServico;
		public string tipoServico;
		public string idGrandeza;
		public string grandeza;
		public string numServico;
		public string ano;
		public string valor;
        public string quantidade;
        public string unidadeQuantidade; 
		public string percDesconto;
		public string valorFinal;
		public string calibracaoExterna;
		public string dtEstado;
        public string observacoes;
        public string idTipoEquipamento;
        //public string idTipoPreco; 
        public string formula; 
		public string bDefinitivo; 
		public string idComentarioEstado;
		public string obsCliente; 
		public string nomeFicheiroReq;
		public string idSubTipoServico; 
		public string idServicoPai; 
		public string refServicoPai; 
		public string bConforme; 
		public string bDeslocacao;
        public string descEstadoServico;
        public string idNivelPrioridade;
        public string dtPrevisao;
        public string acessorios;
        public string bVariasGrandezas;
        public string linguaCertificado;
        public string tipoDeslocacao;
        public string acreditado;
        public string localActual;
        public string idLocalActual;
        public string bRejeitado;
        public string numEtiquetaIPQ;
	
		public string subTipoEquipamento;
		public string idSubTipoEquipamento;
		public string bactivoSubTipo;

	}

	public class ServicoBD
	{
		public ServicoBD()
		{
		}

		//=======================================================================================================
		// DETALHES DE UM SERVIÇO 
		//=======================================================================================================
		public ServicoDetails GetServicoDetails(string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
        
			DataTable ServicoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoById", arrParams); 
           
			if(ServicoDT.Rows.Count > 0) 
			{ 
				ServicoDetails myServicoDetails= new ServicoDetails();

				myServicoDetails.idServico = ServicoDT.Rows[0]["idServico"].ToString();
				myServicoDetails.idBRE = ServicoDT.Rows[0]["idBRE"].ToString();
				myServicoDetails.refBRE = ServicoDT.Rows[0]["refBRE"].ToString();
				myServicoDetails.idBSE = ServicoDT.Rows[0]["idBSE"].ToString();
				//myServicoDetails.refBSE = ServicoDT.Rows[0]["refBSE"].ToString();
				myServicoDetails.idRequisicao = ServicoDT.Rows[0]["idRequisicao"].ToString();
				myServicoDetails.refRequisicao = ServicoDT.Rows[0]["refRequisicao"].ToString();
				myServicoDetails.idEquipamento = ServicoDT.Rows[0]["idEquipamento"].ToString();
				myServicoDetails.idFactura = ServicoDT.Rows[0]["idFactura"].ToString();
				myServicoDetails.refFactura = ServicoDT.Rows[0]["refFactura"].ToString();
				myServicoDetails.idFuncionarioEfectuouServico = ServicoDT.Rows[0]["idFuncionarioEfectuouServico"].ToString();
				myServicoDetails.funcEfectuouServico = ServicoDT.Rows[0]["funcEfectuouServico"].ToString();
				
				myServicoDetails.idFuncionarioTreino = ServicoDT.Rows[0]["idFuncionarioTreino"].ToString();
				myServicoDetails.FuncionarioTreino = ServicoDT.Rows[0]["FuncionarioTreino"].ToString();
			
				myServicoDetails.idEstadoServico = ServicoDT.Rows[0]["idEstadoServico"].ToString();
				myServicoDetails.estadoServico = ServicoDT.Rows[0]["estadoServico"].ToString();
				myServicoDetails.idLocalCalibracao = ServicoDT.Rows[0]["idLocalCalibracao"].ToString();
				myServicoDetails.localCalibracao = ServicoDT.Rows[0]["localCalibracao"].ToString();

                myServicoDetails.idLocalActual = ServicoDT.Rows[0]["idLocalActual"].ToString();
                myServicoDetails.localActual = ServicoDT.Rows[0]["localActual"].ToString();


				myServicoDetails.idTipoServico = ServicoDT.Rows[0]["idTipoServico"].ToString();
				myServicoDetails.tipoServico = ServicoDT.Rows[0]["tipoServico"].ToString();
				myServicoDetails.idGrandeza = ServicoDT.Rows[0]["idGrandeza"].ToString();
				myServicoDetails.grandeza = ServicoDT.Rows[0]["grandeza"].ToString();
				myServicoDetails.numServico = ServicoDT.Rows[0]["numServico"].ToString();
				myServicoDetails.ano = ServicoDT.Rows[0]["ano"].ToString();
				myServicoDetails.refServico = ServicoDT.Rows[0]["refServico"].ToString();
				myServicoDetails.valor = ServicoDT.Rows[0]["valor"].ToString();
				myServicoDetails.percDesconto = ServicoDT.Rows[0]["percDesconto"].ToString();
				myServicoDetails.valorFinal = ServicoDT.Rows[0]["valorFinal"].ToString();
				myServicoDetails.calibracaoExterna = GERAL.clsGeral.ConvertStringToBool(ServicoDT.Rows[0]["calibracaoExterna"].ToString()).ToString();
				
				myServicoDetails.dtEstado = ServicoDT.Rows[0]["dtEstado"].ToString();
				myServicoDetails.observacoes = ServicoDT.Rows[0]["observacoes"].ToString();
				//myServicoDetails.idTipoPreco = ServicoDT.Rows[0]["idTipoPreco"].ToString(); 
				myServicoDetails.idTipoEquipamento = ServicoDT.Rows[0]["idTipoEquipamento"].ToString(); 
				myServicoDetails.quantidade= ServicoDT.Rows[0]["quantidade"].ToString(); 
				myServicoDetails.unidadeQuantidade= ServicoDT.Rows[0]["unidadeQuantidade"].ToString(); 
				//myServicoDetails.formula = ServicoDT.Rows[0]["bFormula"].ToString(); 
				myServicoDetails.bDefinitivo = ServicoDT.Rows[0]["bDefinitivo"].ToString(); 
				myServicoDetails.idComentarioEstado = ServicoDT.Rows[0]["idComentarioEstado"].ToString(); 
				myServicoDetails.obsCliente = ServicoDT.Rows[0]["obsCliente"].ToString(); 
				myServicoDetails.nomeFicheiroReq = ServicoDT.Rows[0]["nomeFicheiro"].ToString(); 
				myServicoDetails.idSubTipoServico = ServicoDT.Rows[0]["idSubTipoServico"].ToString(); 

				myServicoDetails.idServicoPai = ServicoDT.Rows[0]["idServicoPai"].ToString(); 
				myServicoDetails.refServicoPai= ServicoDT.Rows[0]["refServicoPai"].ToString(); 
				
				myServicoDetails.bConforme = ServicoDT.Rows[0]["bConforme"].ToString(); 
				myServicoDetails.bDeslocacao = ServicoDT.Rows[0]["bDeslocacao"].ToString();

                myServicoDetails.idNivelPrioridade = ServicoDT.Rows[0]["idNivelPrioridade"].ToString();
                myServicoDetails.dtPrevisao = ServicoDT.Rows[0]["dtPrevisao"].ToString();
                myServicoDetails.acessorios = ServicoDT.Rows[0]["acessorios"].ToString();
                myServicoDetails.bVariasGrandezas = GERAL.clsGeral.ConverteBoolSimNao(Convert.ToBoolean(ServicoDT.Rows[0]["bVariasGrandezas"]));
                myServicoDetails.linguaCertificado = ServicoDT.Rows[0]["linguaCertificado"].ToString();
                myServicoDetails.tipoDeslocacao = ServicoDT.Rows[0]["tipoDeslocacao"].ToString();
                myServicoDetails.acreditado = ServicoDT.Rows[0]["acreditado"].ToString();
                myServicoDetails.numEtiquetaIPQ = ServicoDT.Rows[0]["numEtiquetaIPQ"].ToString();
                myServicoDetails.bRejeitado = GERAL.clsGeral.ConvertStringToBool(ServicoDT.Rows[0]["bRejeitado"].ToString()).ToString();
				 myServicoDetails.subTipoEquipamento = ServicoDT.Rows[0]["subTipoEquipamento"].ToString();
				myServicoDetails.idSubTipoEquipamento = ServicoDT.Rows[0]["idSubTipoEquipamento"].ToString();
				myServicoDetails.bactivoSubTipo = ServicoDT.Rows[0]["bactivoSubTipo"].ToString(); // tratado como string


				return myServicoDetails;
			}
			else
			{
				return null; 
			}
		}




		// ==========================================================================================
		// Funcao que devolve todas as Linhas de Serviço associadas a um Serviço
		// ==========================================================================================
		public DataTable dtLinhasServicoByIdServico(string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetLinhasServicoByIdServico", arrParams);    
		}

		
		
		//========================================================================================	
		// DATATABLE COM LISTA DE SERVIÇOS PARA A PASTA DE EENSAIO COM BASE NOS CRITÉRIOS DE PESQUISA
		//========================================================================================	
		public DataTable DTGetServicos(string empresa, string refBRE, string refServico,string idEstado,string numIdent, string numSerie, string idLocal, string calibracaoExterna, string idGrandeza,string dtBRE,string tipoEquipamento, string idEquipamento, string numEtiquetaIPQ)
		{
			SqlParameter[] arrParams = new SqlParameter[13];

            arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inRefServico", refServico);
			arrParams[2] = new SqlParameter("@inRefBRE", refBRE);
			arrParams[3] = new SqlParameter("@inIdEstado", idEstado);
			arrParams[4] = new SqlParameter("@inNumIdentificacao", numIdent);
			arrParams[5] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[6] = new SqlParameter("@inLocal", idLocal);
			arrParams[7] = new SqlParameter("@inCalibracaoExterna", calibracaoExterna == "" ? null : calibracaoExterna);
			arrParams[8] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[9] = new SqlParameter("@inDtBre", dtBRE);         
			arrParams[10] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
			arrParams[11] = new SqlParameter("@inIdEquipamento", idEquipamento);
            arrParams[12] = new SqlParameter("@numEtiquetaIPQ", numEtiquetaIPQ);

            return GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicos", arrParams); 	               
        }

        // DATATABLE COM LISTA DE SERVIÇOS que inclui datas de calibracao, com certif. e previsao, conforme pedido pela fatima
		//========================================================================================	
		public DataTable DTGetServicosComDatas(string empresa, string refBRE, string refServico,string idEstado,string numIdent, string numSerie, string Local, string calibracaoExterna, string idGrandeza,string dtBRE,string tipoEquipamento, string idEquipamento)
		{
			SqlParameter[] arrParams = new SqlParameter[12];
            
			arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inRefServico", refServico);
			arrParams[2] = new SqlParameter("@inRefBRE", refBRE);
			arrParams[3] = new SqlParameter("@inIdEstado", idEstado);
			arrParams[4] = new SqlParameter("@inNumIdentificacao", numIdent);
			arrParams[5] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[6] = new SqlParameter("@inLocal", Local);

			string sCalibExterna = calibracaoExterna; 
			if(sCalibExterna =="") 
			{
				arrParams[7] = new SqlParameter("@inCalibracaoExterna",null);
			}
			else
			{
				arrParams[7] = new SqlParameter("@inCalibracaoExterna",sCalibExterna);
			}

			arrParams[8] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[9] = new SqlParameter("@inDtBre", dtBRE);
         
			arrParams[10] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
			arrParams[11] = new SqlParameter("@inIdEquipamento", idEquipamento);
        
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicosComDatas", arrParams); 
		
                
        }


  

		public DataTable DTServicosByIdEmpresa(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@idEmpresa", idEmpresa);
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicosByIdEmpresa", arrParams); 
		
		}

		public DataTable DTServicosByIdEmpresaCONCCTA(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@idEmpresa", idEmpresa);
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListServicosByIdEmpresaCONCCTA", arrParams); 
		
		}

		//========================================================================================	
		// DATATABLE COM OS SERViçOS DE UMA EMPRESA QUE SE ENCONTRAM SUBCONTRATADOS A UMA 
		// DETERMINADA OUTRA EMPRESA
		//========================================================================================	
		public DataTable DTGetServicosForSubcontratoBRE(string idEmpresa, string idEmpresaSubcontratada)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdEmpresaSubcontratada", idEmpresaSubcontratada);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoForSubcontratoBREByIdEmpresaAndIdEmpresaSub", arrParams); 
		}

		//========================================================================================	
		// DATATABLE COM OS SERVIÇOS DUMA EMPRESA QUE PODEM SER SUBCONTRATATOS
		//========================================================================================	
		public DataTable DTGetServicosForSubcontratoBSE(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoForSubcontratoBSEByIdEmpresa", arrParams); 
		}

		
		//========================================================================================	
		//DATATABLE COM OS SERVIÇOS DISPONIVEIS PARA BES, POR EMPRESA 
		//========================================================================================	
		public DataTable DTServicosForBSE(string idEmpresa,string refBRE, string idFuncionarioRecepcao)
		{
			SqlParameter[] arrParams = new SqlParameter[3];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inRefBre", refBRE);
			arrParams[2] = new SqlParameter("@inIdFuncionarioRecepcao",idFuncionarioRecepcao );

         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoForBSEByIdEmpresa", arrParams); 
        
		}

		////========================================================================================	
		//// DATATABLE COM INFORMAÇÃO NECESSÁRIA PARA CALCULAR O PREÇO DE UM SERVIÇO
		////========================================================================================	
		//public DataTable DTGetServicoPrecoDetails(string idServico)
		//{
		//	SqlParameter[] arrParams = new SqlParameter[1];
            
		//	arrParams[0] = new SqlParameter("@inIdServico", idServico);
                   
		//	return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoPrecoById", arrParams);
		//}

		//========================================================================================	
		// DATATABLE COM HISTÓRIO DE ESTATDOS DE UM SERVIÇO
		//========================================================================================	
		public DataTable DTGetServicoHistoricoEstados(string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoHistoricoEstadosById", arrParams); 
		}

		 
		//========================================================================================	
		// DATATABLE COM TODOS OS SERVIÇOS DE UM BRE
		//========================================================================================	
		public DataTable DTGetServicoByBRE(string idBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoByBRE", arrParams); 
		}


		//========================================================================================	
		// DATATABLE COM TODOS OS SERVIÇOS DE UM BSE
		//========================================================================================	
		public DataTable DTGetServicoByBSE(string idBSE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdBSE", idBSE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoByBSE", arrParams); 	
		}


		//========================================================================================	
		// DATATABLE COM TODOS OS SERVIÇOS DE UM BSE
		//========================================================================================	
		public DataTable DTGetCertificadosByBSE(string idBSE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdBSE", idBSE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetCertificadosByBSE", arrParams); 	
		}
		
		
		//========================================================================================	
		// DATATABLE COM TODOS OS SERVIÇOS DE UM BRE DE SUBCONTRATAÇÃO
		//========================================================================================	
		public DataTable DTGetServicoBySubcontratoBRE(string idSubcontratoBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdSubcontratoBRE", idSubcontratoBRE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoBySubcontratoBRE", arrParams); 
		}

		//========================================================================================	
		// DATATABLE COM TODOS OS SERVIÇOS DE UM BSE DE SUBCONTRATAÇÃO
		//========================================================================================	
		public DataTable DTGetServicoBySubcontratoBSE(string idSubcontratoBSE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdSubcontratoBSE", idSubcontratoBSE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoBySubcontratoBSE", arrParams); 
		}


		

		//========================================================================================	
		// DATAREADER -Estados de Serviços seguintes possíveis com base no ID do estado
		// se id estado vem vazio, devolve estados inciciais (acho eu)
		//========================================================================================	
		public SqlDataReader DRGetListaEstadosSeguintes(string idEstado)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEstado", idEstado);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListaEstadosSeguintes", arrParams); 
           
		}

		//========================================================================================	
		// DATAREADER - Comentarios dos estados de serviço com base no estado Seleccionado 
		// inicialmente isto existe para os estados ANULADO(7) E SUSPENSO(9)
		//========================================================================================	
		public SqlDataReader DRGetListaComentariosEstado(string idEstado)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEstado", idEstado);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListComentariosEstados", arrParams); 
           
		}

		//========================================================================================	
		// DATAREADER -Estados de Serviços seguintes possíveis com base no ID DO SERVIÇO
		//========================================================================================	
		public SqlDataReader DRGetListaEstadosServico(string idServico)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListaEstadosServicoByIdServico", arrParams); 
           
		}

	}
}
