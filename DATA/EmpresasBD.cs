using System;
using System.Data.SqlClient; 
using System.Configuration;
using System.Data;

using System.Web;



namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for EmpresasBD.
	/// </summary>
	/// 
    public class CompanyDetails
    {													
		public string idEmpresa; 
        
		public string idEmpresaPai; 
        public string empresaPai; 
        public string localidade;
        public string localidadePostal;
        public string latitude;
        public string longitude;
        public string idConcelho;
        public string idActividade; 
        public string idTipoEmpresa; 
        public string tipoEmpresa; 
        public string idPais;
        public string pais; 
        public string idEstadoEmpresa; 
		public string estadoEmpresa; 
        public string nome;
        public string nomeAbreviado;
        public string nif; 
        public string morada; 
		public string cp;
        public string telefone;
        public string telefone2; 
        public string fax; 
        public string email; 
        public string sede; 
        public string pagamentoAtraso; 
        public string percDesconto; 
		public string dtEstado; 
        public string observacoes;  
		public string codigoBloqueioSAP; 
		public string descCodigoBloqueioSAP; 
		public string nivelBloqueioLabmetro; 
		public string requisicaoAtraso;
		public string bContrato; 
		public string idCodigoRegiaoVendas;
		public string regiaoVendas;
		public string idCondicoesPagamento;
		public string condicoesPagamento; 
		public string numClienteSAP; 
		public bool bCertifsSemReq; 
		public bool bPodeFacturarSemRequisicao;
        public bool bGestaoEquipamentos;
        public string idFuncionarioGestaoEquipamentos;
        public string FuncionarioGestaoEquipamentos;
        public string numObra;
        public string website;
        public string modoPagamento;
        public string modoEntrega;
        public string creditoMax;
        public string criteriosAceitacao;
        public string ficheiroCriterios;
        public bool bRequerDocEntrada;
        public string documentacaoEntrada;

    }

	public class EmpresasBD
	{
		public EmpresasBD()
		{ 
		}


		// Função que devolve um DataReader com os detalhes de uma Empresa com base no seu ID
		public SqlDataReader DRGetCompanyDetails(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
			SqlDataReader EmpresaDR = LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetEmpresaById", arrParams); 
           
			return EmpresaDR;
		}


		// Funcao que devolve os dados de uma Empresa com base no seu ID
		public CompanyDetails GetCompanyDetails(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
			DataTable EmpresaDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresaById", arrParams); 
           
			if(EmpresaDT.Rows.Count > 0) 
			{ 
				CompanyDetails dt= new CompanyDetails();

				dt.idEmpresa = EmpresaDT.Rows[0]["idEmpresa"].ToString();
				dt.idEmpresaPai = EmpresaDT.Rows[0]["idEmpresaPai"].ToString(); 
				dt.empresaPai =  EmpresaDT.Rows[0]["empresaPai"].ToString(); 
				dt.localidade= EmpresaDT.Rows[0]["localidade"].ToString();
                dt.localidadePostal = EmpresaDT.Rows[0]["localidadePostal"].ToString(); 
				dt.pais = EmpresaDT.Rows[0]["pais"].ToString();
                dt.idConcelho = EmpresaDT.Rows[0]["idConcelho"].ToString();
                dt.idActividade = EmpresaDT.Rows[0]["idConcelho"].ToString();
                dt.latitude = EmpresaDT.Rows[0]["latitude"].ToString();
                dt.longitude = EmpresaDT.Rows[0]["longitude"].ToString();
				dt.idTipoEmpresa = EmpresaDT.Rows[0]["idTipoEmpresa"].ToString();
				dt.tipoEmpresa = EmpresaDT.Rows[0]["tipoEmpresa"].ToString();
				dt.idPais = EmpresaDT.Rows[0]["idPais"].ToString();
                dt.idConcelho = EmpresaDT.Rows[0]["idConcelho"].ToString();
                dt.idActividade = EmpresaDT.Rows[0]["idActividade"].ToString(); 
				dt.idEstadoEmpresa = EmpresaDT.Rows[0]["idEstadoEmpresa"].ToString();
				dt.estadoEmpresa = EmpresaDT.Rows[0]["estadoEmpresa"].ToString();
				dt.nome = EmpresaDT.Rows[0]["nome"].ToString(); 
				dt.nomeAbreviado = EmpresaDT.Rows[0]["nomeAbreviado"].ToString();
				dt.nif = EmpresaDT.Rows[0]["nif"].ToString(); 
				dt.morada = EmpresaDT.Rows[0]["morada"].ToString(); 
				dt.cp = EmpresaDT.Rows[0]["cp"].ToString(); 
				dt.telefone = EmpresaDT.Rows[0]["telefone"].ToString().Trim(); 
				dt.telefone2 = EmpresaDT.Rows[0]["telefone2"].ToString();
				dt.fax = EmpresaDT.Rows[0]["fax"].ToString();
				dt.email = EmpresaDT.Rows[0]["email"].ToString(); 
				dt.sede = GERAL.clsGeral.ConvertStringToBool(EmpresaDT.Rows[0]["sede"].ToString()).ToString();
				dt.pagamentoAtraso =  GERAL.clsGeral.ConvertStringToBool(EmpresaDT.Rows[0]["pagamentoAtraso"].ToString()).ToString();
				dt.percDesconto = EmpresaDT.Rows[0]["percDesconto"].ToString(); 
                dt.dtEstado = EmpresaDT.Rows[0]["dtEstado"].ToString(); 
				dt.observacoes = EmpresaDT.Rows[0]["observacoes"].ToString();     
				dt.codigoBloqueioSAP = EmpresaDT.Rows[0]["codigoBloqueioSAP"].ToString();     
				dt.descCodigoBloqueioSAP = EmpresaDT.Rows[0]["descCodigoBloqueio"].ToString();   
				dt.nivelBloqueioLabmetro = EmpresaDT.Rows[0]["nivelBloqueioLabmetro"].ToString();   
				dt.requisicaoAtraso = GERAL.clsGeral.ConvertStringToBool(EmpresaDT.Rows[0]["bRequisicaoAtraso"].ToString()).ToString();
				dt.bContrato = GERAL.clsGeral.ConvertStringToBool(EmpresaDT.Rows[0]["bContrato"].ToString()).ToString();
				dt.numClienteSAP = EmpresaDT.Rows[0]["numClienteSAP"].ToString(); 
				dt.idCondicoesPagamento = EmpresaDT.Rows[0]["idCondicoesPagamento"].ToString(); 
				dt.condicoesPagamento = EmpresaDT.Rows[0]["condicoesPagamento"].ToString(); 
				dt.idCodigoRegiaoVendas = EmpresaDT.Rows[0]["idCodigoRegiaoVendas"].ToString();
				dt.regiaoVendas = EmpresaDT.Rows[0]["regiaoVendas"].ToString();
				dt.bCertifsSemReq = GERAL.clsGeral.ConvertBStringToBoolean(EmpresaDT.Rows[0]["bMostraCertSemReq"].ToString());
				dt.bPodeFacturarSemRequisicao = GERAL.clsGeral.ConvertBStringToBoolean(EmpresaDT.Rows[0]["bPodeFacturarSemRequisicao"].ToString());
                dt.bGestaoEquipamentos = GERAL.clsGeral.ConvertBStringToBoolean(EmpresaDT.Rows[0]["bGestaoEquipamentos"].ToString());
                dt.idFuncionarioGestaoEquipamentos = EmpresaDT.Rows[0]["idFuncionarioGestaoEquipamentos"].ToString();
                dt.FuncionarioGestaoEquipamentos = EmpresaDT.Rows[0]["FuncionarioGestaoEquipamentos"].ToString();
                dt.numObra = EmpresaDT.Rows[0]["numObra"].ToString();
                dt.website = EmpresaDT.Rows[0]["website"].ToString();
                dt.modoEntrega = EmpresaDT.Rows[0]["modoEntrega"].ToString();
                dt.modoPagamento = EmpresaDT.Rows[0]["modoPagamento"].ToString();
                dt.creditoMax = EmpresaDT.Rows[0]["creditoMax"].ToString();
                dt.ficheiroCriterios = EmpresaDT.Rows[0]["ficheiroCriterios"].ToString();
                dt.criteriosAceitacao = EmpresaDT.Rows[0]["criteriosAceitacao"].ToString();
                dt.bRequerDocEntrada = GERAL.clsGeral.ConvertBStringToBoolean(EmpresaDT.Rows[0]["bRequerDocEntrada"].ToString());
                dt.documentacaoEntrada = EmpresaDT.Rows[0]["documentacaoEntrada"].ToString(); 

				return dt;
			}
			else
			{
				return null; 
			}
		}

	

		// DataTable que retorna as Empresas que têm serviços que podem dar saída no BSE
		public DataTable DTGetEmpresasForBSE(string strEmpresa, string strNIF)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inNome", strEmpresa);
			arrParams[1] = new SqlParameter("@inNif", strNIF);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForBSE", arrParams); 
		}

		// Lista Empresas que têm serviços que podem ser subcontratados
		public SqlDataReader DRGetEmpresasForBSESub()
		{
			SqlDataReader DR = LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasForBSESub");
			return DR;
		}

		// Lista Empresas que têm serviços subcontratados
		public SqlDataReader DRGetEmpresasForBRESub()
		{
			SqlDataReader DR = LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasForBRESub");
			return DR;
		}

		// Lista Empresas que têm serviços por facturar
		public SqlDataReader DRGetEmpresasPorFacturar()
		{
			SqlDataReader DR = LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasPorFacturar");
			return DR;
		}

		// Lista Empresas do tipo "Subcontratada"
		public SqlDataReader DRGetEmpresasSubcontratadas()
		{
			SqlDataReader DR = LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasSubcontratadas");
			return DR;
		}

		// Lista Empresas do tipo "Subcontratada" que têm equipamentos de uma dada empresa
		// a si subcontratados
		public SqlDataReader DRGetEmpresasSubcontratadasByIdEmpresa(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresaCliente", idEmpresa);

			SqlDataReader DR = LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetEmpresasSubcontratadasByIdEmpresa", arrParams);
			return DR;
		}

		// Funcao que devolve o histórico de estados da Empresa
		public SqlDataReader DRGetEmpresaHistoricoEstados(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetEmpresaHistoricoEstadosById", arrParams); 
           

		}

        public SqlDataReader DREmpresasForIgae()
        {

            SqlParameter[] arrParams = new SqlParameter[3];
            arrParams[0] = new SqlParameter("@inNome", "");
            arrParams[1] = new SqlParameter("@inNif", "");
            arrParams[2] = new SqlParameter("@idConcelho","");

        	return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListEmpresasIgae", arrParams); 
        }

        public SqlDataReader DREmpresasPesquisaPorNome(string search)
        {

            SqlParameter[] arrParams = new SqlParameter[9];
            arrParams[0] = new SqlParameter("@inNome", search);
            arrParams[1] = new SqlParameter("@inNif", null);

            arrParams[2] = new SqlParameter("@inIdEstadoEmpresa", "1"); //activas
            arrParams[3] = new SqlParameter("@inIdTipoEmpresa", null);

            arrParams[4] = new SqlParameter("@inCodigoBloqueioSAP", null);
            arrParams[5] = new SqlParameter("@inPagamentoAtraso", null);
            arrParams[6] = new SqlParameter("@inNivelBloqueioLabmetro", null);
            arrParams[7] = new SqlParameter("@inRequisicaoAtraso", null);
            arrParams[8] = new SqlParameter("@inNumCliente", null);


            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListEmpresas", arrParams); 
        }
        
		// Função que devolve uma DataTable com empresas com base nos critérios de pesquisa
        public DataTable DTEmpresas(string strSearchEmpresa,string strSearchNIF, string idEstado, string idTipoEmpresa, string codigoBloqueioSAP, string pagamentoAtraso,string nivelBloqueioLabmetro, string requisicaoAtraso, string numCliente)
        {
			SqlParameter[] arrParams = new SqlParameter[9];
			arrParams[0] = new SqlParameter("@inNome", strSearchEmpresa);
			arrParams[1] = new SqlParameter("@inNif", strSearchNIF);
			
			arrParams[2] = new SqlParameter("@inIdEstadoEmpresa", idEstado); 
			arrParams[3] = new SqlParameter("@inIdTipoEmpresa", idTipoEmpresa); 

			arrParams[4] = new SqlParameter("@inCodigoBloqueioSAP",codigoBloqueioSAP); 
			arrParams[5] = new SqlParameter("@inPagamentoAtraso", pagamentoAtraso); 
			arrParams[6] = new SqlParameter("@inNivelBloqueioLabmetro", nivelBloqueioLabmetro);
			arrParams[7] = new SqlParameter("@inRequisicaoAtraso",requisicaoAtraso); 
			arrParams[8] = new SqlParameter("@inNumCliente",numCliente);
			
		
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListEmpresas", arrParams); 
           
		}


		// para fazer fill da dropdown so com a empresa em questao, que nao se pode alterar!
		public DataTable DTEmpresas(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresaById", arrParams); //uso a stp get empresa by id
		}



		// Funcao que insere uma Empresa e retorna o id inserido.
		public string InsertCompany(string idEmpresaPai, string localidade, string localidadePostal, string idConcelho, string idActividade, string latitude, string longitude, string idTipoEmpresa, string idPais, string idEstadoEmpresa, string nome, string nomeAbreviado, string nif, string morada, string cp, string telefone1, string telefone2, string fax, string email, string sede, string pagamentoAtraso, string percDesconto, string observacoes, string username, string nivelBloqueioLabmetro, string codigoBloqueioSAP, string requisicaoAtraso, string contrato, string numClienteSAP, string idCodigoRegiaoVendas, string idCondicoesPagamento, string bMostraCertSemReq, string bPodeFacturarSemRequisicao,string bGestaoEquipamentos, string idFuncionarioGestaoEquipamentos, string numObra, string website, string modoEntrega, string modoPagamento, string creditoMax, string criteriosAceitacao, string ficheiroCriterios, string bRequerDocEntrada, string documentacaoEntrada)
		{
			SqlParameter[] arrParams = new SqlParameter[44];

			arrParams[0] = new SqlParameter("@inIdEmpresaPai", idEmpresaPai);
			arrParams[1] = new SqlParameter("@localidade", localidade);
			arrParams[2] = new SqlParameter("@inIdTipoEmpresa", idTipoEmpresa);
			arrParams[3] = new SqlParameter("@inIdPais", idPais);
			arrParams[4] = new SqlParameter("@inIdEstadoEmpresa", idEstadoEmpresa);
			arrParams[5] = new SqlParameter("@inNome", nome);
			arrParams[6] = new SqlParameter("@inNomeAbreviado", nomeAbreviado);
			arrParams[7] = new SqlParameter("@inNif", nif);
			arrParams[8] = new SqlParameter("@inMorada", morada);
			arrParams[9] = new SqlParameter("@inCp", cp);
            arrParams[10] = new SqlParameter("@inIdCondicoesPagamento", idCondicoesPagamento); 
			arrParams[11] = new SqlParameter("@inTelefone", telefone1);
			arrParams[12] = new SqlParameter("@inTelefone2", telefone2);
			arrParams[13] = new SqlParameter("@inFax", fax);
			arrParams[14] = new SqlParameter("@inEmail", email);
			arrParams[15] = new SqlParameter("@inSede", sede);
            arrParams[16] = new SqlParameter("@bPodeFacturarSemRequisicao", bPodeFacturarSemRequisicao); 
			arrParams[17] = new SqlParameter("@inPagamentoAtraso", GERAL.clsGeral.ConvertStringToBool(pagamentoAtraso));
			arrParams[18] = new SqlParameter("@inPercDesconto", GERAL.clsGeral.ConvertStringToDouble(percDesconto));
			arrParams[19] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[20] = new SqlParameter("@inUsername", username);
			arrParams[21] = new SqlParameter("@inNivelBloqueioLabmetro",nivelBloqueioLabmetro); 
			arrParams[22] = new SqlParameter("@inCodigoBloqueioSAP",codigoBloqueioSAP); 
			arrParams[23] = new SqlParameter("@inRequisicaoAtraso",requisicaoAtraso); 
			arrParams[24] = new SqlParameter("@inContrato",contrato); 
			arrParams[25] = new SqlParameter("@inNumClienteSAP",numClienteSAP); 
			arrParams[26] = new SqlParameter("@inIdRegiaoVendas",idCodigoRegiaoVendas); 
			arrParams[27] = new SqlParameter("@inCertifsSemReq",bMostraCertSemReq);
            arrParams[28] = new SqlParameter("@localidadePostal", localidadePostal);
            arrParams[29] = new SqlParameter("@idConcelho", idConcelho);
            arrParams[30] = new SqlParameter("@idActividade", idActividade);
            arrParams[31] = new SqlParameter("@latitude", latitude);
            arrParams[32] = new SqlParameter("@longitude", longitude);
            arrParams[33] = new SqlParameter("@bGestaoEquipamentos", bGestaoEquipamentos);
            arrParams[34] = new SqlParameter("@idFuncionarioGestaoEquipamentos", idFuncionarioGestaoEquipamentos);
            arrParams[35] = new SqlParameter("@numObra", numObra);
            arrParams[36] = new SqlParameter("@website", website);
            arrParams[37] = new SqlParameter("@modoEntrega", modoEntrega);
            arrParams[38] = new SqlParameter("@modoPagamento", modoPagamento);
            arrParams[39] = new SqlParameter("@creditoMax", creditoMax);
            arrParams[40] = new SqlParameter("@criteriosAceitacao", criteriosAceitacao);
            arrParams[41] = new SqlParameter("@ficheiroCriterios", ficheiroCriterios);
            arrParams[42] = new SqlParameter("@bRequerDocEntrada", GERAL.clsGeral.ConvertStringToBool(bRequerDocEntrada));
            arrParams[43] = new SqlParameter("@documentacaoEntrada", documentacaoEntrada);


            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput("stpInsEmpresa", arrParams).ToString();             
		}

		// Funcao que actualiza uma Empresa 
        public string UpdateCompany(string idEmpresa, string idEmpresaPai, string localidade, string localidadePostal, string idConcelho, string idActividade, string latitude, string longitude, string idTipoEmpresa, string idPais, string idEstadoEmpresa, string nome, string nomeAbreviado, string nif, string morada, string cp, string telefone1, string telefone2, string fax, string email, string sede, string pagamentoAtraso, string percDesconto, string observacoes, string username, string nivelBloqueioLabmetro, string codigoBloqueioSAP, string requisicaoAtraso, string contrato, string numClienteSAP, string idCodigoRegiaoVendas, string idCondicoesPagamento, string bMostraCertSemReq, string bPodeFacturarSemRequisicao, string bGestaoEquipamentos, string idFuncionarioGestaoEquipamentos, string numObra, string website, string modoEntrega, string modoPagamento, string creditoMax, string criteriosAceitacao, string ficheiroCriterios, string bRequerDocEntrada, string documentacaoEntrada)
		{
			SqlParameter[] arrParams = new SqlParameter[45];

            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdEmpresaPai", idEmpresaPai);
			arrParams[2] = new SqlParameter("@localidade", localidade);
			arrParams[3] = new SqlParameter("@inIdTipoEmpresa", idTipoEmpresa);
			arrParams[4] = new SqlParameter("@inIdPais", idPais);
			arrParams[5] = new SqlParameter("@inIdEstadoEmpresa", idEstadoEmpresa);
			arrParams[6] = new SqlParameter("@inNome", nome);
			arrParams[7] = new SqlParameter("@inNomeAbreviado", nomeAbreviado);
			arrParams[8] = new SqlParameter("@inNif", nif);
			arrParams[9] = new SqlParameter("@inMorada", morada);
			arrParams[10] = new SqlParameter("@inCp", cp);
            arrParams[11] = new SqlParameter("@inCertifsSemReq", bMostraCertSemReq); 
			arrParams[12] = new SqlParameter("@inTelefone", telefone1);
			arrParams[13] = new SqlParameter("@inTelefone2", telefone2);
			arrParams[14] = new SqlParameter("@inFax", fax);
			arrParams[15] = new SqlParameter("@inEmail", email);
			
			arrParams[16] = new SqlParameter("@inSede", sede);
            arrParams[17] = new SqlParameter("@bPodeFacturarSemRequisicao", bPodeFacturarSemRequisicao); 
			arrParams[18] = new SqlParameter("@inPagamentoAtraso", pagamentoAtraso);
			arrParams[19] = new SqlParameter("@inPercDesconto", GERAL.clsGeral.ConvertStringToDouble(percDesconto));
			arrParams[20] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[21] = new SqlParameter("@inUsername", username);
			arrParams[22] = new SqlParameter("@inNivelBloqueioLabmetro",nivelBloqueioLabmetro); 
			arrParams[23] = new SqlParameter("@inCodigoBloqueioSAP",codigoBloqueioSAP); 
			arrParams[24] = new SqlParameter("@inRequisicaoAtraso",requisicaoAtraso); 
			arrParams[25] = new SqlParameter("@inContrato",contrato); 
			arrParams[26] = new SqlParameter("@inNumClienteSAP",numClienteSAP); 
			arrParams[27] = new SqlParameter("@inIdRegiaoVendas",idCodigoRegiaoVendas); 
			arrParams[28] = new SqlParameter("@inIdCondicoesPagamento",idCondicoesPagamento);

            arrParams[29] = new SqlParameter("@localidadePostal", localidadePostal);
            arrParams[30] = new SqlParameter("@idConcelho", idConcelho);
            arrParams[31] = new SqlParameter("@idActividade", idActividade);
            arrParams[32] = new SqlParameter("@latitude", latitude);
            arrParams[33] = new SqlParameter("@longitude", longitude);
            arrParams[34] = new SqlParameter("@bGestaoEquipamentos", bGestaoEquipamentos);
            arrParams[35] = new SqlParameter("@idFuncionarioGestaoEquipamentos", idFuncionarioGestaoEquipamentos);
            arrParams[36] = new SqlParameter("@numObra", numObra);
            arrParams[37] = new SqlParameter("@website", website);
            arrParams[38] = new SqlParameter("@modoEntrega", modoEntrega);
            arrParams[39] = new SqlParameter("@modoPagamento", modoPagamento);
            arrParams[40] = new SqlParameter("@creditoMax", creditoMax);
            arrParams[41] = new SqlParameter("@criteriosAceitacao", criteriosAceitacao);
            arrParams[42] = new SqlParameter("@ficheiroCriterios", ficheiroCriterios);
            arrParams[43] = new SqlParameter("@bRequerDocEntrada", GERAL.clsGeral.ConvertStringToBool(bRequerDocEntrada));
            arrParams[44] = new SqlParameter("@documentacaoEntrada", documentacaoEntrada);

            int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRetVal("stpUpdEmpresa", arrParams);
            return retValue.ToString();   
        
		}

		// Funcao que faz reset às empresas com pagamentos em atraso
		public string ResetEmpresasDevedoras(string username)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpResetEmpresasDevedoras", arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;    
		}

		//teste!
		public DataSet DSEmpresa(string idEmpresa)
		{
			//tenho q fazer um fill do dataset
			DSRepEmpresa ds = new DSRepEmpresa(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
             
				objCmd.CommandType = CommandType.Text; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				//objConn.Open(); 
			
				SqlDataAdapter DA = new SqlDataAdapter(objCmd);

				objCmd.CommandText= "SELECT Empresa.*,  Pais.descricao as Pais FROM Empresa LEFT JOIN PAIS on Empresa.idPais = Pais.idPais WHERE Empresa.idEmpresa = "+idEmpresa;
				
				DA.Fill(ds,"Empresa"); 

				objCmd.CommandText =  "SELECT Contacto.*, Titulo.descricao as Titulo FROM Contacto LEFT JOIN Titulo ON Contacto.idTitulo = Titulo.idTitulo WHERE Contacto.idEmpresa = "+idEmpresa;

				DA.Fill(ds,"Contacto"); 

							
				DA.Dispose(); 
				DA = null;
			}
			return ds; 

		}
		

		public DataTable DTEmpresasSAP(string nome, string numClienteSAP,string numClienteAntigo, string numContribuinte, string codigoBloqueioSAP, string grupoContas)
		{
			string strSQL = "SELECT sap_Empresas.nomeCompleto AS nome, sap_Empresas.nif, sap_Empresas.grupoContas as 'Grp.Contas', sap_Empresas.codigoClienteSAP AS 'Núm.Cliente', sap_Empresas.numAntigo as 'Núm.Antigo',  sap_Empresas.rua1, sap_Empresas.codigoPostal AS 'CP', sap_Empresas.localidade, sap_Empresas.pais,  sap_Empresas.telefone, sap_Empresas.fax,  sap_CodigoBloqueio.descCodigoBloqueio AS codBloq FROM sap_Empresas INNER JOIN  sap_CodigoBloqueio ON sap_Empresas.codBloqueio = sap_CodigoBloqueio.codigoBloqueio WHERE sap_Empresas.canalDistribuicao = 'RE' "; 
			//passar para parametros.
			//ou passar para stored procedura para poder modificar em runtime

			if(nome!="") strSQL +=" AND sap_Empresas.nomeCompleto LIKE '"+nome+"%' "; 
			
			if(numClienteSAP!="") strSQL +=" AND CAST(sap_Empresas.codigoClienteSAP as int) = CAST('"+numClienteSAP+"' as int) "; 
			if(numContribuinte!="") strSQL +=" AND sap_Empresas.nif LIKE '%"+numContribuinte+"%' "; 
			if(numClienteAntigo!="") strSQL +=" AND RIGHT(sap_Empresas.numAntigo,5) LIKE '"+numClienteAntigo+"%' "; 
			if(codigoBloqueioSAP!="") strSQL +=" AND sap_CodigoBloqueio.codigoBloqueio = '"+codigoBloqueioSAP+"' "; 
			if(grupoContas!="") strSQL +=" AND sap_Empresas.grupoContas = '"+grupoContas+"' "; 

			//HttpContext.Current.Response.Write(strSQL); 
			return GERAL.clsDataAccess.ExecuteDT(strSQL); 

		}	
		// Funcao que actualiza uma Empresa da "CARLA".
		public int UpdateEmpresaConc(string idEmpresa, string tipoContrato,string dtUltimaVisita, string bCTA, string bConcessionario, string bSistemaPesagem, string bCentroB, string telefone, string email, string fax,string bAGE)
		{
//
//			actualiza a emrpesa com os novos campos
//												 
//			bCTA	bit	Checked
//			bConcessionario	bit	Checked
//			bCentroB	bit	Checked
//			intTipoContrato	int	Checked
//			dtUltimaVisita	datetime	Checked
//			bSistemaPesagem

											
			SqlParameter[] arrParams = new SqlParameter[11];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@tipoContrato", tipoContrato);
			arrParams[2] = new SqlParameter("@dtUltimaVisita", dtUltimaVisita);
			arrParams[3] = new SqlParameter("@bCTA", bCTA);
			arrParams[4] = new SqlParameter("@bConcessionario", bConcessionario);
			arrParams[5] = new SqlParameter("@bSistemaPesagem", bSistemaPesagem);
			arrParams[6] = new SqlParameter("@bCentroB", bCentroB);
			arrParams[7] = new SqlParameter("@telefone", telefone);
			arrParams[8] = new SqlParameter("@email", email);
			arrParams[9] = new SqlParameter("@fax", fax);
			arrParams[10] = new SqlParameter("@bAGE", bAGE);
			
			return GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdateEmpresaConcessionarios", arrParams); 
			
		}
	}
}
