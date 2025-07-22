using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;


namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for BreBD.
	/// </summary>
	/// 

	// Classe que guarda nos seus atributos os detalhes de um BRE
    public class BreDetails
    {
		public string idBRE;
		public string idEmpresa;
		public string nomeEmpresa;
		public string idEmpresaContratante;
		public string nomeEmpresaContratante;
		public string nomeCompridoEmpresa; 
		public string obsEmpresa;
		public string morada; 
		public string cp; 
		public string localidade; 
		public string idFuncionarioRecepcao;
		public string funcionarioRecepcao;
		public string numBRE;
		public string ano;
		public string refBRE;
		public string dtBRE;
		public string requisicaoCompleta;
		public string entreguePor;
		public string observacoes;
        public string bDefinitivo;   
		public string expedicao;
        public string idOrcamento;
        public string refRequisicao;
        public string concelho; //pedido de espanha
        public string nota;
        public string bEmpBrePodeverCertificados;
        public string bTaxaUrgencia;
    }
    
	public class BreBD
	{
		public BreBD()
		{
		}

		//********************************************************************************
		// Lista BRE's de uma Empresa que têm serviços por facturar
		//********************************************************************************
		public SqlDataReader DRGetBrePorFacturarByIdEmpresa(string idEmpresa, string cbBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inSoCompletos", GERAL.clsGeral.ConvertStringToBool(cbBRE));
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetBrePorFacturarByIdEmpresa", arrParams);  
		}


        //********************************************************************************
        // Lista BRE's de uma Empresa que têm serviços por facturar
        //********************************************************************************
        public SqlDataReader DRGetBreByIdEmpresa(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idEmpresa", idEmpresa);
           
            
            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetBreByIdEmpresa", arrParams);
        }

		//********************************************************************************
		// Lista BRE's de uma Empresa que têm serviços por facturar
		//********************************************************************************
		public DataTable DTGetBrePorFacturarByIdEmpresa(string idEmpresa, string cbBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inSoCompletos", GERAL.clsGeral.ConvertStringToBool(cbBRE));
         
            
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetBrePorFacturarByIdEmpresa", arrParams);  
		}


        //********************************************************************************
        // Lista BRE's de uma Empresa PARA VD (Inclui serviços que possam ainda nao estar em estado facturavel
        //********************************************************************************
        public DataTable DTGetBreParaVDByIdEmpresa(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);


            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetBreForVDByIdEmpresa", arrParams);
        }

		public SqlDataReader DRGetIdEmpresaContratantePorBRE(string idBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@idBRE", idBRE);
		
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetEmpresaContratantePorBRE", arrParams);  
		}

		//********************************************************************************
		// Lista BRE's que têm serviços por facturar
		//********************************************************************************
		public SqlDataReader DRGetBrePorFacturar()
		{
			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetBrePorFacturar"); 
		}

		// Lista Empresas que estao associadas à BRE's
		public DataTable DTListaEmpresasForListaBRE(string strEmpresa, string strNIF)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inNome", strEmpresa);
			arrParams[1] = new SqlParameter("@inNif", strNIF);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForListBRE", arrParams); 
		}


		//********************************************************************************
		// Funcao que devolve um BRE com base no seu ID
		//********************************************************************************
		public BreDetails GetBreDetails(string idBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
         
			DataTable BreDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetBreById", arrParams); 
           
			if(BreDT.Rows.Count > 0) 
			{ 
				BreDetails myBreDetails= new BreDetails();
				myBreDetails.idBRE = BreDT.Rows[0]["idBRE"].ToString();
				myBreDetails.idEmpresa = BreDT.Rows[0]["idEmpresa"].ToString();
				myBreDetails.nomeEmpresa = BreDT.Rows[0]["empresa"].ToString();
				myBreDetails.idEmpresaContratante = BreDT.Rows[0]["idEmpresaContratante"].ToString();
				myBreDetails.nomeEmpresaContratante = BreDT.Rows[0]["empresaContratante"].ToString();
				myBreDetails.nomeCompridoEmpresa = BreDT.Rows[0]["nomeCompridoEmpresa"].ToString();
				myBreDetails.obsEmpresa = BreDT.Rows[0]["obsEmpresa"].ToString();
				myBreDetails.morada = BreDT.Rows[0]["morada"].ToString();
				myBreDetails.cp = BreDT.Rows[0]["cp"].ToString();
				myBreDetails.localidade =BreDT.Rows[0]["localidade"].ToString();
				myBreDetails.idFuncionarioRecepcao = BreDT.Rows[0]["idFuncionarioRecepcao"].ToString();
				myBreDetails.funcionarioRecepcao = BreDT.Rows[0]["funcionarioRecepcao"].ToString();
				myBreDetails.numBRE = BreDT.Rows[0]["numBRE"].ToString();
				myBreDetails.ano = BreDT.Rows[0]["ano"].ToString();
				myBreDetails.refBRE = BreDT.Rows[0]["refBRE"].ToString();
				myBreDetails.dtBRE = BreDT.Rows[0]["dtBRE"].ToString();
				myBreDetails.requisicaoCompleta = GERAL.clsGeral.ConvertStringToBool(BreDT.Rows[0]["requisicaoCompleta"].ToString()).ToString();
				myBreDetails.entreguePor = BreDT.Rows[0]["entreguePor"].ToString();
				myBreDetails.observacoes = BreDT.Rows[0]["observacoes"].ToString();
				myBreDetails.bDefinitivo = BreDT.Rows[0]["bDefinitivo"].ToString();  
				myBreDetails.expedicao = BreDT.Rows[0]["expedicao"].ToString();
                myBreDetails.idOrcamento = BreDT.Rows[0]["idOrcamento"].ToString();
                myBreDetails.refRequisicao = BreDT.Rows[0]["referenciaRequisicao"].ToString();
                myBreDetails.concelho = BreDT.Rows[0]["concelho"].ToString();
                myBreDetails.nota = BreDT.Rows[0]["nota"].ToString();
                myBreDetails.bEmpBrePodeverCertificados = GERAL.clsGeral.ConvertStringToBool(BreDT.Rows[0]["bEmpBrePodeverCertificados"].ToString()).ToString();
                myBreDetails.bTaxaUrgencia = GERAL.clsGeral.ConvertStringToBool(BreDT.Rows[0]["bTaxaUrgencia"].ToString()).ToString(); 


                return myBreDetails; 
			}
			else
			{
				return null; 
			}
		}

		//********************************************************************************r
		//********************************************************************************
		public DataTable DTBRE(string idEmpresa, string refBRE,string bDefintivo, string refRequisicao, string bCompletos)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inRefBRE", refBRE);
			arrParams[2] = new SqlParameter("@inBDefinitivo", bDefintivo);
            arrParams[3] = new SqlParameter("@inSoCompletos", GERAL.clsGeral.ConvertStringToBool(bCompletos));
            arrParams[4] = new SqlParameter("@refRequisicao", refRequisicao);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListBREs", arrParams);   
		}

		//********************************************************************************
		//insere um bre dentro de uma transaccao e retorna o id inserido.
		//********************************************************************************
		public int InsertBreConnn(SqlConnection myConnection,SqlCommand myCommand, string idEmpresa, string idFuncionarioRecepcao, string requisicaoCompleta, string entreguePor, string observacoes, string username, string bDefinitivo, string expedicao, string idEmpresaContratante, string idOrcamento, string refRequisicao, string nota, string bEmpBrePodeverCertificados, string bTaxaUrgencia)
		{
			SqlParameter[] arrParams = new SqlParameter[14];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdFuncionarioRecepcao", idFuncionarioRecepcao);
			arrParams[2] = new SqlParameter("@inRequisicaoCompleta", requisicaoCompleta);
			arrParams[3] = new SqlParameter("@inEntreguePor", entreguePor);
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", username);
			arrParams[6] = new SqlParameter("@inBDefinitivo", bDefinitivo);
			arrParams[7] = new SqlParameter("@inExpedicao", expedicao);
			arrParams[8] = new SqlParameter("@inIdEmpresaContratante", idEmpresaContratante);
            arrParams[9] = new SqlParameter("@idOrcamento", idOrcamento);
            arrParams[10] = new SqlParameter("@refRequisicao", refRequisicao);
            arrParams[11] = new SqlParameter("@nota", nota);
            arrParams[12] = new SqlParameter("@bEmpBrePodeverCertificados", bEmpBrePodeverCertificados);
            arrParams[13] = new SqlParameter("@bTaxaUrgencia", bTaxaUrgencia);


            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput(myConnection,myCommand,"stpInsBRE", arrParams); 

		}

		//********************************************************************************
		//insere um bre sem transaccao e retorna o ID do BRE
		//********************************************************************************
		public int InsertBre(string idEmpresa, string idFuncionarioRecepcao, string requisicaoCompleta, string entreguePor, string observacoes, string username, string bDefinitivo, string expedicao,string idEmpresaContratante, string nota, string bEmpBrePodeverCertificados, string bTaxaUrgencia)
		{
			SqlParameter[] arrParams = new SqlParameter[12];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdFuncionarioRecepcao", idFuncionarioRecepcao);
			arrParams[2] = new SqlParameter("@inRequisicaoCompleta", requisicaoCompleta);
			arrParams[3] = new SqlParameter("@inEntreguePor", entreguePor);
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", username);
			arrParams[6] = new SqlParameter("@inBDefinitivo", bDefinitivo);
			arrParams[7] = new SqlParameter("@inExpedicao", expedicao);
			arrParams[8] = new SqlParameter("@inIdEmpresaContratante", idEmpresaContratante);
            arrParams[9] = new SqlParameter("@nota", nota);
            arrParams[10] = new SqlParameter("@bEmpBrePodeverCertificados", bEmpBrePodeverCertificados);
            arrParams[11] = new SqlParameter("@bTaxaUrgencia", bTaxaUrgencia);


            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput("stpInsBRE", arrParams); 

		}
		//********************************************************************************
		//Funcao que actualiza um BRE 
		//********************************************************************************
		public void UpdateBreConn(SqlConnection myConnection,SqlCommand myCommand,string idBRE,  string requisicaoCompleta, string entreguePor, string observacoes, string bDefinitivo, string expedicao, string idOrcamento, string refRequisicao, string nota, string bEmpBrePodeverCertificados, string bTaxaUrgencia)
		{
			SqlParameter[] arrParams = new SqlParameter[12];

			arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
			arrParams[1] = new SqlParameter("@inRequisicaoCompleta", requisicaoCompleta);
			arrParams[2] = new SqlParameter("@inEntreguePor", entreguePor);
			arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[4] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
			arrParams[5] = new SqlParameter("@inBDefinitivo", bDefinitivo);
			arrParams[6] = new SqlParameter("@inExpedicao", expedicao);
            arrParams[7] = new SqlParameter("@idOrcamento", idOrcamento);
            arrParams[8] = new SqlParameter("@refRequisicao", refRequisicao);
            arrParams[9] = new SqlParameter("@nota", nota);
            arrParams[10] = new SqlParameter("@bEmpBrePodeverCertificados", bEmpBrePodeverCertificados);
            arrParams[11] = new SqlParameter("@bTaxaUrgencia", bTaxaUrgencia);
            GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection, myCommand,"stpUpdBRE", arrParams); 

		}


		

		//********************************************************************************
		//transaccao, insere um bre com serviços associados, retorna uma msg de erro ou confirmacao
		//********************************************************************************
		public int InsertBreWithServices(string idEmpresa, string idFuncionarioRecepcao, string requisicaoCompleta, string entreguePor, string observacoes, string username,DataView DV, string bDefinitivo, string expedicao, string idEmpresaContratante,string idOrcamento, string refRequisicao, string nota, string bEmpBrePodeverCertificados, string bTaxaUrgencia)
		{
			int idBRE =0; 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using (SqlCommand objCmd = new SqlCommand())
			{
             
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 
			

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 

					try
					{	
						idBRE = InsertBreConnn(objConn,objCmd,idEmpresa,idFuncionarioRecepcao,requisicaoCompleta,entreguePor,observacoes,username,bDefinitivo, expedicao, idEmpresaContratante, idOrcamento,refRequisicao, nota, bEmpBrePodeverCertificados,bTaxaUrgencia); 
    
						DV.RowStateFilter= DataViewRowState.CurrentRows;
                        
						foreach(DataRowView drv in DV)
						{
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							string idRequisicao= drv["idRequisicao"].ToString(); 
							string idEquipamento= drv["idEquipamento"].ToString(); 
							string idEstadoServico= drv["idEstadoServico"].ToString();
                            string idLocalDestino = drv["idLocalDestino"].ToString();  
							string idTipoServico= drv["idTipoServico"].ToString();  
							string obs= drv["Observacoes"].ToString();  
							string idServicoPai= drv["idServicoPai"].ToString();  
							string refServicoPai= drv["refServicoPai"].ToString();
                            string acessorios = drv["acessorios"].ToString();
                            string refServicoCertificado = drv["refServicoCertificado"].ToString();
                            
							SqlParameter[] arrParams = new SqlParameter[24];

							arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
							arrParams[1] = new SqlParameter("@inIdBSE", "");
							arrParams[2] = new SqlParameter("@inIdRequisicao", idRequisicao);
							arrParams[3] = new SqlParameter("@inIdEquipamento", idEquipamento);
							arrParams[4] = new SqlParameter("@inIdFactura", "");
							arrParams[5] = new SqlParameter("@inIdFuncionarioEfectuouServico", "");
							arrParams[6] = new SqlParameter("@inIdFuncionarioEstimativa", "");
							arrParams[7] = new SqlParameter("@inIdSubcontratoBRE", "");
							arrParams[8] = new SqlParameter("@inIdSubcontratoBSE", "");
							arrParams[9] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
							arrParams[10] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
							arrParams[11] = new SqlParameter("@inIdTipoServico", idTipoServico);
							arrParams[12] = new SqlParameter("@inValor", 0);
							arrParams[13] = new SqlParameter("@inPercDesconto", 0);
							arrParams[14] = new SqlParameter("@inValorFinal",0);
							arrParams[15] = new SqlParameter("@inCalibracaoExterna", drv["calibExt"].ToString());
							arrParams[16] = new SqlParameter("@inObservacoes", obs);
							arrParams[17] = new SqlParameter("@inUsername", username);
							arrParams[18] = new SqlParameter("@inBDefinitivo", bDefinitivo);
							arrParams[19] = new SqlParameter("@idServicoPai", idServicoPai);
							arrParams[20] = new SqlParameter("@refServicoPai", refServicoPai);
                            arrParams[21] = new SqlParameter("@acessorios", acessorios);
                            arrParams[22] = new SqlParameter("@bVariasGrandezas", drv["bVariasGrandezas"].ToString());
                            arrParams[23] = new SqlParameter("@refServicoCertificado", refServicoCertificado);
                   
							objCmd.CommandText = "stpInsServico"; 
                            
							foreach (SqlParameter p in arrParams)
							{
								//check for derived output value with no value assigned
								if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
								{
									p.Value = DBNull.Value;
								}
				
								objCmd.Parameters.Add(p);
							}
                           
							objCmd.ExecuteNonQuery(); 
						}
						objTrans.Commit(); 
					}
					catch(SqlException ex)
					{ 	
						idBRE = 0; //ser correu mal id volta a 0

						try
						{	
							objTrans.Rollback();
						}
						catch(Exception exep)
						{
							GERAL.clsWriteError.WriteLog(exep); 
						}
						GERAL.clsWriteError.WriteLog(ex); 
					}
				}
			}
			return idBRE; 
		}

		//********************************************************************************
		//transaccao, insere um bre com serviços associados, retorna uma msg de erro ou confirmacao
		//foi alterada (adaptei, nao fiz de novo, para inserir, apagar, o alterar....)DM
		//********************************************************************************
		public string UpdateBreWithServices(string idBRE, string idEmpresa, string idFuncionarioRecepcao, string requisicaoCompleta, string entreguePor, string observacoes, string username, DataView DV,string bDefinitivo, string expedicao, string idOrcamento, string refRequisicao, string nota, string bEmpBrePodeverCertificados, string bTaxaUrgencia)
		{
			string result; 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using (SqlCommand objCmd = new SqlCommand())
			{
             
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans;  

					try
					{	
						UpdateBreConn(objConn,objCmd,idBRE,requisicaoCompleta,entreguePor,observacoes,bDefinitivo,expedicao,idOrcamento,refRequisicao, nota, bEmpBrePodeverCertificados, bTaxaUrgencia); 

						//se as rows nao foram alteradas, podem no entanto mudar
						//devido a checkbox estar checkada (bDefinitivo) o q se reflecte
						//em todos os serviços
						DV.RowStateFilter = DataViewRowState.Unchanged; 
						foreach(DataRowView drv in DV)
						{
							//UPDATE
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            
							string idServico= drv["idServico"].ToString(); 
							string idRequisicao= drv["idRequisicao"].ToString();
                            string idLocalDestino = drv["idLocalDestino"].ToString();
							string obs = drv["observacoes"].ToString();
                            string acessorios = drv["acessorios"].ToString();
                            string refServicoCertificado = drv["refServicoCertificado"].ToString();
    
							SqlParameter[] arrParams = new SqlParameter[9];
                
							arrParams[0] = new SqlParameter("@inIdServico", idServico);
							arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao);
                            arrParams[2] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
							arrParams[3] = new SqlParameter("@inObservacoes", obs);
							arrParams[4] = new SqlParameter("@inUsername", username);
							arrParams[5] = new SqlParameter("@inBDefinitivo", bDefinitivo);
                            arrParams[6] = new SqlParameter("@acessorios", acessorios);
                            arrParams[7] = new SqlParameter("@bVariasGrandezas", drv["bVariasGrandezas"].ToString());
                            arrParams[8] = new SqlParameter("@refServicoCertificado", refServicoCertificado);
                
							objCmd.CommandText = "stpUpdServicoInBRE"; 
                            
							foreach (SqlParameter p in arrParams)
							{
								if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
								{
									p.Value = DBNull.Value;
								}
								objCmd.Parameters.Add(p);
							}
							objCmd.ExecuteNonQuery(); 
						}

						DV.RowStateFilter= DataViewRowState.ModifiedCurrent; 
                        
						foreach(DataRowView drv in DV)
						{
							//UPDATE
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            
							string idServico= drv["idServico"].ToString(); 
							string idRequisicao= drv["idRequisicao"].ToString();
                            string idLocalDestino = drv["idLocalDestino"].ToString();
                            string obs = drv["observacoes"].ToString();
                            string acessorios = drv["acessorios"].ToString();
                            string refServicoCertificado = drv["refServicoCertificado"].ToString();
    
							SqlParameter[] arrParams = new SqlParameter[9];
                
							arrParams[0] = new SqlParameter("@inIdServico", idServico);
							arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao);
                            arrParams[2] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
							arrParams[3] = new SqlParameter("@inObservacoes", obs);
							arrParams[4] = new SqlParameter("@inUsername", username);
							arrParams[5] = new SqlParameter("@inBDefinitivo", bDefinitivo);
                            arrParams[6] = new SqlParameter("@acessorios", acessorios);
                            arrParams[7] = new SqlParameter("@bVariasGrandezas", drv["bVariasGrandezas"].ToString());
                            arrParams[8] = new SqlParameter("@refServicoCertificado", refServicoCertificado);
							
                
							objCmd.CommandText = "stpUpdServicoInBRE"; 
                            
							foreach (SqlParameter p in arrParams)
							{
								if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
								{
									p.Value = DBNull.Value;
								}
								objCmd.Parameters.Add(p);
							}
							objCmd.ExecuteNonQuery(); 
						}

						DV.RowStateFilter= DataViewRowState.Added;
						foreach(DataRowView drv in DV)
						{
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							string idRequisicao= drv["idRequisicao"].ToString(); 
							string idEquipamento= drv["idEquipamento"].ToString(); 
							string idEstadoServico= drv["idEstadoServico"].ToString();
                            string idLocalDestino = drv["idLocalDestino"].ToString();  
							string idTipoServico= drv["idTipoServico"].ToString();  
							string obs= drv["Observacoes"].ToString();  
							string idServicoPai= drv["idServicoPai"].ToString();  
							string refServicoPai= drv["refServicoPai"].ToString();
                            string acessorios = drv["acessorios"].ToString();
                            string refServicoCertificado = drv["refServicoCertificado"].ToString();

							SqlParameter[] arrParams = new SqlParameter[24];

							arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
							arrParams[1] = new SqlParameter("@inIdBSE", "");
							arrParams[2] = new SqlParameter("@inIdRequisicao", idRequisicao);
							arrParams[3] = new SqlParameter("@inIdEquipamento", idEquipamento);
							arrParams[4] = new SqlParameter("@inIdFactura", "");
							arrParams[5] = new SqlParameter("@inIdFuncionarioEfectuouServico", "");
							arrParams[6] = new SqlParameter("@inIdFuncionarioEstimativa", "");
							arrParams[7] = new SqlParameter("@inIdSubcontratoBRE", "");
							arrParams[8] = new SqlParameter("@inIdSubcontratoBSE", "");
							arrParams[9] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[10] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
							arrParams[11] = new SqlParameter("@inIdTipoServico", idTipoServico);
							arrParams[12] = new SqlParameter("@inValor", 0);
							arrParams[13] = new SqlParameter("@inPercDesconto", 0);
							arrParams[14] = new SqlParameter("@inValorFinal",0);
							arrParams[15] = new SqlParameter("@inCalibracaoExterna", "");
							arrParams[16] = new SqlParameter("@inObservacoes", obs);
							arrParams[17] = new SqlParameter("@inUsername", username);
							arrParams[18] = new SqlParameter("@inBDefinitivo", bDefinitivo);
							arrParams[19] = new SqlParameter("@idServicoPai", idServicoPai); //nao sei se é necessario no update
							arrParams[20] = new SqlParameter("@refServicoPai", refServicoPai);//idem
                            arrParams[21] = new SqlParameter("@acessorios", acessorios);
                            arrParams[22] = new SqlParameter("@bVariasGrandezas", drv["bVariasGrandezas"].ToString());
                            arrParams[23] = new SqlParameter("@refServicoCertificado", refServicoCertificado);

							objCmd.CommandText = "stpInsServico"; 
                            
							foreach (SqlParameter p in arrParams)
							{
								//check for derived output value with no value assigned
								if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
								{
									p.Value = DBNull.Value;
								}
				
								objCmd.Parameters.Add(p);
							}
                           
							objCmd.ExecuteNonQuery(); 

						}
                        
						DV.RowStateFilter= DataViewRowState.Deleted;
						foreach(DataRowView drv in DV)
						{
                           
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							string idServico= drv["idServico"].ToString(); 
							string idEstadoServico = drv["idEstadoServico"].ToString(); 
				
							SqlParameter[] arrParams = new SqlParameter[3];

							arrParams[0] = new SqlParameter("@inIdServico", idServico);
							arrParams[1] = new SqlParameter("@inUsername", username);
							arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);

							objCmd.CommandText = "SetServiceInBREDisabled"; 
                            
							foreach (SqlParameter p in arrParams)
							{
								//check for derived output value with no value assigned
								if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
								{
									p.Value = DBNull.Value;
								}
				
								objCmd.Parameters.Add(p);
							}
                           
							objCmd.ExecuteNonQuery(); 
						}

						objTrans.Commit(); 
						result = "0"; 
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
							result = "1"; 
						}

						GERAL.clsWriteError.WriteLog(ex); 
                
						result = "1"; 
					}
				}
			}
			return result; 
		}        
        

		//********************************************************************************
		//Igual à funcao superior so q chama uma stored procedure que permite alterar o idEquipamento para alterar o Serviço e como nao quero alterar todas as chamadas à funcao, 
		//criei isto de novo
		//o idEquipamento vem dentro da dataview
		//********************************************************************************
		public string UpdateBreWithServicesComEquipamento(string idBRE, string idEmpresa,  string idFuncionarioRecepcao, string requisicaoCompleta, string entreguePor, string observacoes, string username, DataView DV,string bDefinitivo, string expedicao, string nota, string bEmpBrePodeverCertificados, string bTaxaUrgencia)
		{
			string result; 
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using (SqlCommand objCmd = new SqlCommand())
			{
             
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 


				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans;

					try
					{	
            
						UpdateBreConn(objConn,objCmd,idBRE,requisicaoCompleta,entreguePor,observacoes,bDefinitivo,expedicao,"","",nota, bEmpBrePodeverCertificados, bTaxaUrgencia); 

						//se as rows nao foram alteradas, podem no entanto mudar
						//devido a checkbox estar checkada (bDefinitivo) o q se reflecte
						//em todos os serviços
						if(DV !=null)
						{
							DV.RowStateFilter = DataViewRowState.Unchanged; 
							foreach(DataRowView drv in DV)
							{
								//UPDATE
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            
								string idServico= drv["idServico"].ToString(); 
								string idRequisicao= drv["idRequisicao"].ToString();
                                string idLocalDestino = drv["idLocalDestino"].ToString();
								string obs = drv["observacoes"].ToString();  
    
								SqlParameter[] arrParams = new SqlParameter[7];
                
								arrParams[0] = new SqlParameter("@inIdServico", idServico);
								arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao);
								arrParams[2] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
								arrParams[3] = new SqlParameter("@inObservacoes", obs);
								arrParams[4] = new SqlParameter("@inUsername", username);
								arrParams[5] = new SqlParameter("@inBDefinitivo", bDefinitivo);
								arrParams[6] = new SqlParameter("@inExpedicao", expedicao);
                
								objCmd.CommandText = "stpUpdServicoInBRE"; 
                            
								foreach (SqlParameter p in arrParams)
								{
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
									objCmd.Parameters.Add(p);
								}
								objCmd.ExecuteNonQuery(); 
							}

							DV.RowStateFilter= DataViewRowState.ModifiedCurrent; 
                        
							foreach(DataRowView drv in DV)
							{
								//UPDATE
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            
								string idServico= drv["idServico"].ToString(); 
								string idRequisicao= drv["idRequisicao"].ToString();
                                string idLocalDestino = drv["idLocalDestino"].ToString();
								string idEquipamento= drv["idEquipamento"].ToString();
								string idEstadoServico = drv["idEstadoServico"].ToString(); 

								string obs = drv["observacoes"].ToString();  
    
								SqlParameter[] arrParams = new SqlParameter[8];
                
								arrParams[0] = new SqlParameter("@inIdServico", idServico);
								arrParams[1] = new SqlParameter("@inIdEquipamento", idEquipamento);
								arrParams[2] = new SqlParameter("@inIdRequisicao", idRequisicao);
                                arrParams[3] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
								arrParams[4] = new SqlParameter("@inObservacoes", obs);
								arrParams[5] = new SqlParameter("@inUsername", username);
								arrParams[6] = new SqlParameter("@inBDefinitivo", bDefinitivo);
								arrParams[7] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                
								objCmd.CommandText = "stpUpdServicoInBREComIdEquipamento"; 
                            
								foreach (SqlParameter p in arrParams)
								{
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
									objCmd.Parameters.Add(p);
								}
								objCmd.ExecuteNonQuery(); 
							}

							DV.RowStateFilter= DataViewRowState.Added;
                        
							foreach(DataRowView drv in DV)
							{
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

								string idRequisicao= drv["idRequisicao"].ToString(); 
								string idEquipamento= drv["idEquipamento"].ToString(); 
								string idEstadoServico= drv["idEstadoServico"].ToString();
                                string idLocalDestino = drv["idLocalDestino"].ToString();  
								string idTipoServico= drv["idTipoServico"].ToString();  
								string idServicoPai= drv["idServicoPai"].ToString();  
								string obs= drv["Observacoes"].ToString();  
								
                   
								SqlParameter[] arrParams = new SqlParameter[19];

								arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
								arrParams[1] = new SqlParameter("@inIdBSE", "");
								arrParams[2] = new SqlParameter("@inIdRequisicao", idRequisicao);
								arrParams[3] = new SqlParameter("@inIdEquipamento", idEquipamento);
								arrParams[4] = new SqlParameter("@inIdFactura", "");
								arrParams[5] = new SqlParameter("@inIdFuncionarioEfectuouServico", "");
								arrParams[6] = new SqlParameter("@inIdFuncionarioEstimativa", "");
								arrParams[7] = new SqlParameter("@inIdSubcontratoBRE", "");
								arrParams[8] = new SqlParameter("@inIdSubcontratoBSE", "");
								arrParams[9] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                                arrParams[10] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
								arrParams[11] = new SqlParameter("@inIdTipoServico", idTipoServico);
								arrParams[12] = new SqlParameter("@inValor", 0);
								arrParams[13] = new SqlParameter("@inPercDesconto", 0);
								arrParams[14] = new SqlParameter("@inValorFinal",0);
								arrParams[15] = new SqlParameter("@inCalibracaoExterna", "");
								
								arrParams[17] = new SqlParameter("@inObservacoes", obs);
								arrParams[18] = new SqlParameter("@inUsername", username);
								arrParams[19] = new SqlParameter("@inBDefinitivo", bDefinitivo);
								arrParams[20] = new SqlParameter("@idServicoPai", idServicoPai);

								objCmd.CommandText = "stpInsServico"; 
                            
								foreach (SqlParameter p in arrParams)
								{
									//check for derived output value with no value assigned
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
				
									objCmd.Parameters.Add(p);
								}
                           
								objCmd.ExecuteNonQuery(); 

							}
                        
							DV.RowStateFilter= DataViewRowState.Deleted;
							foreach(DataRowView drv in DV)
							{
                           
								if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

								string idServico= drv["idServico"].ToString(); 
								string idEstadoServico= drv["idEstadoServico"].ToString(); 	

								SqlParameter[] arrParams = new SqlParameter[3];

								arrParams[0] = new SqlParameter("@inIdServico", idServico);
								arrParams[1] = new SqlParameter("@inUsername", username);
								arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);

								objCmd.CommandText = "SetServiceInBREDisabled"; 
                            
								foreach (SqlParameter p in arrParams)
								{
									//check for derived output value with no value assigned
									if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
									{
										p.Value = DBNull.Value;
									}
				
									objCmd.Parameters.Add(p);
								}   
								objCmd.ExecuteNonQuery(); 
							}
						}
						objTrans.Commit(); 
						result = GERAL.clsGeral.ErrorMessage.MSG_DB;
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
							result = GERAL.clsGeral.ErrorMessage.ERR_DB; 
						}

						GERAL.clsWriteError.WriteLog(ex); 
						result = GERAL.clsGeral.ErrorMessage.ERR_DB; 
					}
				}
			}
			return result; 
		}

        //===================================================================================================
        //FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS -- velho, substitutido pelo proximo
        //===================================================================================================
        public DataSet DSBRE_MarcacaoOVM(string idMarcacao)
        {
            LabMetro.DATASETS.DSMarcacoesOVM ds = new LabMetro.DATASETS.DSMarcacoesOVM();


            ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                //objCmd.CommandType = CommandType.StoredProcedure;

                //objCmd.CommandText = "stpGetLinhasMarcacaoOVM";
                objCmd.CommandText = "select ROW_NUMBER() OVER(ORDER BY s.idServico ) AS linha,marca.descricao as marca, modelo.descricao as modelo , e.numSerie, e.refUltimaCalibracao,null as TipoVerificacao, null As disponibilidade from marcacao inner join bre on marcacao.idBre = bre.idBre inner join servico s on bre.idBre = s.idBre inner join equipamento e on  s.idEquipamento = e.idEquipamento inner join tipoEquipamento te on e.idTipoEquipamento = te.idTipoEquipamento left join marca on e.idmarca = marca.idMarca left join modelo on e.idModelo = modelo.idModelo where idMarcacao = " + idMarcacao; 
                //objCmd.Parameters.AddWithValue("@idMarcacao", idMarcacao); 

                try
                {
                    da.Fill(ds, "DTLinhasOVM");
                }
                catch (Exception exep)
                {
                    GERAL.clsWriteError.WriteLog(exep + objCmd.CommandText);

                }
                //nova query
                
              
                da.Dispose();
            }


            return ds;
        }

        public DataSet DSBRE_EquipsMarcacaoOVM(string idEmpresa)
        {
            LabMetro.DATASETS.DSMarcacoesOVM ds = new LabMetro.DATASETS.DSMarcacoesOVM(); //uso o dataset que ja tinha feito para receber os equipamentos do bre da marcacao, e preencho com equipamentos AGE E OPC dessa empresa. -- maio 2015


            ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                SqlDataAdapter da = new SqlDataAdapter(objCmd);

                //objCmd.CommandType = CommandType.StoredProcedure;

                //objCmd.CommandText = "stpGetLinhasMarcacaoOVM";
                objCmd.CommandText = "select ROW_NUMBER() OVER(ORDER BY e.idEquipamento ) AS linha,marca.descricao as marca, modelo.descricao as modelo , e.numSerie, e.refUltimaCalibracao,null as TipoVerificacao, null As disponibilidade from equipamento e inner join tipoEquipamento te on e.idTipoEquipamento = te.idTipoEquipamento inner join familia on te.idfamilia = familia.idfamilia and familia.idGrandeza In ('age','opc') left join marca on e.idmarca = marca.idMarca left join modelo on e.idModelo = modelo.idModelo where e.activo = 1 and e.idEmpresa = " + idEmpresa;
                //objCmd.Parameters.AddWithValue("@idMarcacao", idMarcacao); 

                try
                {
                    da.Fill(ds, "DTLinhasOVM");
                }
                catch (Exception exep)
                {
                    GERAL.clsWriteError.WriteLog(exep + objCmd.CommandText);

                }
                //nova query


                da.Dispose();
            }


            return ds;
        }


		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSBRE(string idBRE)
		{
			LabMetro.DATASETS.DSBRE ds = new LabMetro.DATASETS.DSBRE(); 
			ds.EnforceConstraints = false; 	//muito importante, senão dá me um erro no fill!!!!
	
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				SqlDataAdapter da = new SqlDataAdapter(objCmd);

                objCmd.CommandText = "SELECT dbo.BRE.idBRE, dbo.udfGetReferenciaBRE(dbo.BRE.idBRE) AS refBRE, dbo.BRE.dtBRE, dbo.Empresa.nome AS empresa, dbo.Empresa.morada, Empresa.NIF, dbo.Empresa.codigoPostal,dbo.empresa.localidadePostal,  dbo.empresa.localidade,dbo.Funcionario.nomeAbreviado AS recebidoPor, dbo.BRE.entreguePor,dbo.BRE.observacoes, dbo.Empresa.telefone, isnull(EC.nome,EC.nomeAbreviado) AS EmpresaContratante, Empresa.numObra, BRE.referenciaRequisicao, Orcamento.refOrcamento, BRE.nota FROM dbo.BRE INNER JOIN dbo.Empresa ON dbo.BRE.idBRE = " + idBRE + " AND dbo.BRE.idEmpresa = dbo.Empresa.idEmpresa LEFT JOIN Empresa EC ON BRE.idEmpresaContratante = EC.idEmpresa  INNER JOIN dbo.Funcionario ON dbo.BRE.idFuncionarioRecepcao = dbo.Funcionario.idFuncionario left join orcamento on bre.idORcamento = Orcamento.idOrcamento";
				try
				{
					da.Fill(ds,"dtBre"); 
				}
				catch(Exception exep)
				{
					GERAL.clsWriteError.WriteLog(exep + objCmd.CommandText); 
                           
				}
				//nova query

				


				objCmd.CommandText =  "SELECT dbo.Servico.idServico, dbo.Servico.idBRE, dbo.TipoEquipamento.descricao AS tipoEquipamento, dbo.Equipamento.numIdentificacao, dbo.Requisicao.referenciaCliente, dbo.Requisicao.dtRequisicao, dbo.Servico.observacoes, CASE WHEN dbo.Servico.idTipoServico = 'A' THEN dbo.TipoServico.descricao + '- ' +dbo.Servico.refServico ELSE  dbo.TipoServico.descricao  END AS tipoServico, dbo.Servico.refServico, CASE WHEN CAST(dbo.Servico.idEstadoServico AS VARCHAR(10)) = '15' THEN 'Não' WHEN CAST(dbo.Servico.idEstadoServico AS VARCHAR(10)) = '14' THEN 'Sim' END AS comCertificado, dbo.Servico.idBSE, dbo.Servico.idSubcontratoBRE, dbo.Servico.idSubcontratoBSE, dbo.Servico.idEstadoServico, dbo.Equipamento.numSerie, dbo.Servico.acessorios FROM dbo.Servico INNER JOIN dbo.Equipamento ON dbo.Servico.idEquipamento = dbo.Equipamento.idEquipamento INNER JOIN dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.TipoServico ON dbo.Servico.idTipoServico = dbo.TipoServico.idTipoServico LEFT OUTER JOIN  dbo.Requisicao ON dbo.Servico.idRequisicao = dbo.Requisicao.idRequisicao WHERE dbo.Servico.idEstadoServico NOT IN(-1,20)AND dbo.Servico.idBRE = " + idBRE + " ORDER BY dbo.Servico.idServico"; 
				//quero q os anulados não apareçam. -1: número anulado, 20: anulado no ext(vai desaparecer) 
				//os anulados ficam pq ainda podem ser calibrados.
				
				try
				{
					da.Fill(ds,"dtServico");
				}
				catch(Exception exep)
				{
					GERAL.clsWriteError.WriteLog(exep + objCmd.CommandText); 
                           
				}
				da.Dispose();
			}
			return ds; 
		}


		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS -- recebe o iddo bre
		//===================================================================================================
		public DataSet DSEtiqueta(string idBRE)
		{
			LabMetro.DATASETS.DSEtiquetaCal ds = new LabMetro.DATASETS.DSEtiquetaCal(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{

				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				SqlDataAdapter da1 = new SqlDataAdapter(objCmd);
				objCmd.CommandType = CommandType.StoredProcedure; 
				
				objCmd.Parameters.AddWithValue("@idBRE",idBRE); 
				//[stpGetEtiquetaBREbyIdBre]
				objCmd.CommandText= "stpGetEtiquetaBREbyIdBre"; 
				//"SELECT dbo.BRE.idBRE, dbo.udfGetReferenciaBRE(dbo.BRE.idBRE) AS refBRE, dbo.BRE.dtBRE, dbo.Servico.observacoes, dbo.Servico.idServico, dbo.Servico.refServico, dbo.TipoEquipamento.codigo AS tipoEquipamento, dbo.Empresa.nomeAbreviado AS empresa FROM  dbo.BRE INNER JOIN  dbo.Servico ON dbo.BRE.idBRE = dbo.Servico.idBRE INNER JOIN dbo.Equipamento ON dbo.Servico.idBRE = "+idBRE+" AND dbo.Servico.idEquipamento = dbo.Equipamento.idEquipamento INNER JOIN dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.Empresa ON dbo.BRE.idEmpresa = dbo.Empresa.idEmpresa "; 

				try
				{
					da1.Fill(ds,"dtEtiquetaCal"); //usa  dtEtiquetaCal
				}
				catch
				{	
				}
			}
			return ds; 
		}
		
		//********************************************************************************
		//etiqueta que é impressa no BRE, antes de efectuado o serviço
		//********************************************************************************
		public DataSet DSEtiquetaServico(string idServico)
		{
			LabMetro.DATASETS.DSEtiquetaCal ds = new LabMetro.DATASETS.DSEtiquetaCal(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				SqlDataAdapter da1 = new SqlDataAdapter(objCmd);
			
				objCmd.CommandType = CommandType.StoredProcedure; 
				
				objCmd.Parameters.AddWithValue("@idServico",idServico); 
				//[stpGetEtiquetaBRE]
				objCmd.CommandText= "stpGetEtiquetaBRE";
				//"SELECT dbo.BRE.idBRE, dbo.udfGetReferenciaBRE(dbo.BRE.idBRE) AS refBRE, dbo.BRE.dtBRE, dbo.Servico.observacoes, dbo.Servico.idServico, dbo.Servico.refServico, dbo.TipoEquipamento.codigo AS tipoEquipamento, dbo.Empresa.nomeAbreviado AS empresa FROM dbo.BRE INNER JOIN  dbo.Servico ON  dbo.Servico.idServico = " + idServico + " AND dbo.BRE.idBRE  = dbo.Servico.idBRE INNER JOIN dbo.Equipamento ON dbo.Servico.idEquipamento = dbo.Equipamento.idEquipamento INNER JOIN dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.Empresa ON dbo.BRE.idEmpresa = dbo.Empresa.idEmpresa"; 

				try
				{
					da1.Fill(ds,"dtEtiquetaCal"); //usa  dtEtiquetaCal
				}
				catch
				{	
				}		
			}
			return ds; 
		}


		//*******************************************************************************************
		//etiqueta que é na pasta de ensaio e colada no equipamento depois de este ter sido calibrado
		//*******************************************************************************************
		public DataSet DSEtiquetaCalibracao(string idServico)
		{
			LabMetro.DATASETS.DSEtiquetaCal ds = new LabMetro.DATASETS.DSEtiquetaCal(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				SqlDataAdapter da1 = new SqlDataAdapter(objCmd);
				//stpGetEtiquetaCalibracao
				objCmd.CommandType = CommandType.StoredProcedure; 
				
				objCmd.Parameters.AddWithValue("@idServico",idServico); 

				objCmd.CommandText= "stpGetEtiquetaCalibracao"; 

				try
				{
					da1.Fill(ds,"dtEtiquetaPosCalib");  //faz fill da datatable referida (Outra que a etiqueta de bre... podia usar a																mesma mas ficou assim para nao ter de mexer em coisas ja feitas
				}
				catch
				{	
				}
				
			}
		
			return ds; 
		}

        //*******************************************************************************************
        //etiqueta que é na pasta de ensaio e colada no equipamento depois de este ter sido calibrado
        //por multiplos idServicos (diferença para com o anterior está no nome, etiqueta, etiquetaS) 
        //e no param que recebem.
        //*******************************************************************************************
        public DataSet DSEtiquetasCalibracao(string idsServicos)
        {
            LabMetro.DATASETS.DSEtiquetaCal ds = new LabMetro.DATASETS.DSEtiquetaCal();

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;

                SqlDataAdapter da1 = new SqlDataAdapter(objCmd);
                //stpGetEtiquetaCalibracao
                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.AddWithValue("@idsServicos", idsServicos);

                objCmd.CommandText = "stpGetEtiquetasCalibracao";

                //"SELECT dbo.BRE.idBRE, dbo.udfGetReferenciaBRE(dbo.BRE.idBRE) AS refBRE, dbo.BRE.dtBRE, dbo.Servico.observacoes, dbo.Servico.idServico, dbo.Servico.refServico, dbo.TipoEquipamento.codigo AS tipoEquipamento, dbo.Empresa.nomeAbreviado AS empresa FROM dbo.BRE INNER JOIN  dbo.Servico ON  dbo.Servico.idServico = " + idServico + " AND dbo.BRE.idBRE  = dbo.Servico.idBRE INNER JOIN dbo.Equipamento ON dbo.Servico.idEquipamento = dbo.Equipamento.idEquipamento INNER JOIN dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.Empresa ON dbo.BRE.idEmpresa = dbo.Empresa.idEmpresa"; 

                try
                {
                    da1.Fill(ds, "dtEtiquetaPosCalib");  //faz fill da datatable referida (Outra que a etiqueta de bre... podia usar a																mesma mas ficou assim para nao ter de mexer em coisas ja feitas
                }
                catch
                {
                }

            }

            return ds;
        }


		//********************************************************************************
		//********************************************************************************
		public DataSet DSEquipamentos(string idBRE)
		{
			LabMetro.DATASETS.DSBRE ds = new LabMetro.DATASETS.DSBRE();

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				objCmd.CommandType = CommandType.StoredProcedure; 

				SqlDataAdapter da = new SqlDataAdapter(objCmd);
			
				objCmd.Parameters.AddWithValue("@idBRE",idBRE);
				objCmd.CommandText = "stpEquipamentosPorBRE"; 

				try
				{
					da.Fill(ds,"dtEquipamentos");
				}
				catch(Exception exep)
				{
					GERAL.clsWriteError.WriteLog(exep);            
				}
			}

			return ds; 
		}

		//********************************************************************************
		//********************************************************************************
		public SqlDataReader DRTiposEquipamentoByEmpresa(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@idEmpresa",idEmpresa); 
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetTipoEquipamentoByIdEmpresa", arrParams); 
		}



		//********************************************************************************
		//insere os equipamentos de um bre de calibração externa. num loop
		//se o bDefinitivo está marcado, tem de actualizar o respectivo bre com bdefinitivo = true
		//devolve a mensagem de erro/confirmacao
		//********************************************************************************

        public bool InsertServicesInBRECalibExt(int iNumVezes, string idBRE, string idRequisicao, string strIdsEquipamentos, string bDefinitivo, string idEstadoServico, string idTipoServico, string idLocalDestino, string requisicaoCompleta, string entreguePor, string observacoes, string expedicao)
		{
			
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using (SqlCommand objCmd = new SqlCommand())
			{
             
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans;  

					
					SqlParameter[] arrParams = new SqlParameter[19];
					string delimStr = ",";
					char[] delimiter = delimStr.ToCharArray();
					strIdsEquipamentos = strIdsEquipamentos.TrimEnd(delimiter);
					string[] idsEquipamentos = strIdsEquipamentos.Split(delimiter); 

								
					try
					{
						foreach(string idEquipamento in idsEquipamentos)
						{
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
							arrParams[1] = new SqlParameter("@inIdBSE", "");
							arrParams[2] = new SqlParameter("@inIdRequisicao", idRequisicao);
							arrParams[3] = new SqlParameter("@inIdEquipamento", idEquipamento);
							arrParams[4] = new SqlParameter("@inIdFactura", "");
							arrParams[5] = new SqlParameter("@inIdFuncionarioEfectuouServico", "");
							arrParams[6] = new SqlParameter("@inIdFuncionarioEstimativa", "");
							arrParams[7] = new SqlParameter("@inIdSubcontratoBRE", "");
							arrParams[8] = new SqlParameter("@inIdSubcontratoBSE", "");
							arrParams[9] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[10] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
							arrParams[11] = new SqlParameter("@inIdTipoServico", idTipoServico);
							arrParams[12] = new SqlParameter("@inValor", 0);
							arrParams[13] = new SqlParameter("@inPercDesconto", 0);
							arrParams[14] = new SqlParameter("@inValorFinal",0);
							arrParams[15] = new SqlParameter("@inCalibracaoExterna", "");
						    arrParams[16] = new SqlParameter("@inObservacoes", "");
							arrParams[17] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
							arrParams[18] = new SqlParameter("@inBDefinitivo", bDefinitivo);

							objCmd.CommandText = "stpInsServico"; 
                        
							foreach (SqlParameter p in arrParams)
							{
								//check for derived output value with no value assigned
								if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
								{
									p.Value = DBNull.Value;
								}
				
								objCmd.Parameters.Add(p);
							}

							for(int i = 0; i<iNumVezes;i++)
							{
								try
								{
									objCmd.ExecuteNonQuery().ToString();
								}
								catch (Exception ex)
                                { 
                                GERAL.clsWriteError.WriteLog(ex.ToString());
                                }
							}
						}


//						//se por acaso está a definitivo, tenho de alterar tb o BRE e passa-lo para definitivo
						//ha um trigger que tb passa todos os serviços a definitivos que façam parte de um bre. 
						if(bDefinitivo =="1")
						{
							UpdateBreConn(objConn, objCmd, idBRE,requisicaoCompleta, entreguePor, observacoes,bDefinitivo,expedicao,"","","","","");  
							
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
						}
						GERAL.clsWriteError.WriteLog(ex); 
						return false;
					}
				}
			}
		}


		public void disableServiceInBRE(string idServico, string idEstadoServico)
		{	
	
			SqlParameter[] arrParams = new SqlParameter[3];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
			arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);

//			if(GERAL.clsDataAccess.ExecuteNonQuerySP("SetServiceInBREDisabled",arrParams)>0) return true;
//			return false;
			GERAL.clsDataAccess.ExecuteNonQuerySP("SetServiceInBREDisabled",arrParams); //quando tento apanhar se correu bem ou nao, da-me erro de sqlparameter, mas depois o resultado é correcto, porm isso, nao percebo bem qualo problema. 


		}

		public void updateServicoInBRECalibExt(string idServico, string idRequisicao, string idLocalDestino, string observacoes,string idEquipamento, string idEstadoServico, string bDefinitivo)
		{
			
			SqlParameter[] arrParams = new SqlParameter[8];
                
			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdEquipamento", idEquipamento);
			arrParams[2] = new SqlParameter("@inIdRequisicao", idRequisicao);
            arrParams[3] = new SqlParameter("@inIdLocalDestino", idLocalDestino);
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", System.Web.HttpContext.Current.User.Identity.Name.ToString());
			arrParams[6] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
			arrParams[7] = new SqlParameter("@inBDefinitivo", bDefinitivo);
            
			
                
			GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdServicoInBREComIdEquipamento",arrParams); //quando tento apanhar se correu bem ou nao, da-me erro de sqlparameter, mas depois o resultado é correcto, porm isso, nao percebo bem qualo problema. 

		
		}
	}
}



