using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 
using System.Web; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for RequisicaoBD.
	/// 
	/// Contém as classes referentes às tabelas "Requisicao" e "RequisicaoLinha"
	/// </summary>
	/// 

	// Classe que guarda nos seus atributos os detalhes de uma Requisição
	public class RequisicaoDetails
	{
		public string idRequisicao;
		public string idEmpresa;
		public string nomeEmpresa; 
		public string numRequisicao;
		public string ano;
		public string refRequisicao;
		public string referenciaCliente;
		public string dtRequisicao;
		public string dtValidade;
		public string completa;
		public string nomeFicheiro;
		public string observacoes;
		public bool  bContrato; 
		public bool  bRenovavel; 
	}

	public class RequisicaoBD
	{
		public RequisicaoBD()
		{
		}

        // Função que devolve uma Requisição com base no seu ID
        public RequisicaoDetails GetRequisicaoDetails(string idRequisicao)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
            arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
         
            DataTable RequisicaoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetRequisicaoById", arrParams); 
           
            if(RequisicaoDT.Rows.Count > 0) 
            { 

                RequisicaoDetails myRequisicaoDetails= new RequisicaoDetails();
                SetRequisicaoDetails(RequisicaoDT, myRequisicaoDetails);
                return myRequisicaoDetails;
            }
            else
            {
                return null; 
            }
        }

        // Preenche todos os atributos da classe RequisicaoDetails a partir de uma DataTable
        private void SetRequisicaoDetails(DataTable RequisicaoDT, RequisicaoDetails myRequisicaoDetails)
        {
            myRequisicaoDetails.idRequisicao = RequisicaoDT.Rows[0]["idRequisicao"].ToString();
            myRequisicaoDetails.idEmpresa = RequisicaoDT.Rows[0]["idEmpresa"].ToString();
            myRequisicaoDetails.nomeEmpresa = RequisicaoDT.Rows[0]["nomeAbreviado"].ToString();
            myRequisicaoDetails.numRequisicao = RequisicaoDT.Rows[0]["numRequisicao"].ToString();
            myRequisicaoDetails.ano = RequisicaoDT.Rows[0]["ano"].ToString();
            myRequisicaoDetails.refRequisicao = RequisicaoDT.Rows[0]["refRequisicao"].ToString();
            myRequisicaoDetails.referenciaCliente = RequisicaoDT.Rows[0]["referenciaCliente"].ToString();
            myRequisicaoDetails.dtRequisicao = RequisicaoDT.Rows[0]["dtRequisicao"].ToString();
            myRequisicaoDetails.dtValidade = RequisicaoDT.Rows[0]["dtValidade"].ToString();
            myRequisicaoDetails.completa = GERAL.clsGeral.ConvertStringToBool(RequisicaoDT.Rows[0]["completa"].ToString()).ToString();
            myRequisicaoDetails.nomeFicheiro = RequisicaoDT.Rows[0]["nomeFicheiro"].ToString();
            myRequisicaoDetails.observacoes = RequisicaoDT.Rows[0]["observacoes"].ToString();
			myRequisicaoDetails.bContrato = GERAL.clsGeral.ConvertBStringToBoolean(RequisicaoDT.Rows[0]["bContrato"].ToString());
			myRequisicaoDetails.bRenovavel =GERAL.clsGeral.ConvertBStringToBoolean(RequisicaoDT.Rows[0]["bRenovavel"].ToString());
			 
			
        }

        // Funcao que devolve todas as Linhas de uma Requisição
        public DataTable GetLinhasRequisicaoByIdRequisicao(string idRequisicao)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
            arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
         
            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetLinhasRequisicaoByIdRequisicao", arrParams); 
           
        }        


		// Lista Empresas que estão  associadas  a requisicoes e q estao em estado 1
		public DataTable DTListaEmpresasForListRequisicao(string strEmpresa, string strNIF)
		{ 
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inNome", strEmpresa);
			arrParams[1] = new SqlParameter("@inNif", strNIF);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForListRequisicoes", arrParams); 
		}


//        public int InsertRequisicaoConnn(SqlConnection myConnection,SqlCommand myCommand,
//            string idEmpresa, string referenciaCliente,  string dtRequisicao, string dtValidade,string completa,string nomeFicheiro, string observacoes, string username)
//        {
//            SqlParameter[] arrParams = new SqlParameter[10];
//    
//            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
//            arrParams[1] = new SqlParameter("@inReferenciaCliente", referenciaCliente);
//			
//			arrParams[2] = new SqlParameter();
//            arrParams[2].ParameterName = "@inDtRequisicao"; 
//            if(dtRequisicao!="")arrParams[2].Value =DateTime.Parse(dtRequisicao);
//            else arrParams[2].Value =dtRequisicao;  
//
//			arrParams[3] = new SqlParameter();
//            arrParams[3].ParameterName = "@inDtValidade"; 
//            
//            if(dtValidade!="")arrParams[3].Value =DateTime.Parse(dtValidade);
//            else arrParams[3].Value =dtValidade;  
//			
//            arrParams[4] = new SqlParameter("@inNomeFicheiro", nomeFicheiro);
//            arrParams[5] = new SqlParameter("@inCompleta", GERAL.clsGeral.ConvertStringToBool(completa));
//            arrParams[6] = new SqlParameter("@inObservacoes", observacoes);
//            arrParams[7] = new SqlParameter("@inUsername", username);
//
//			
//
//            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput(myConnection,myCommand,"stpInsRequisicao", arrParams); 
//        }



		public int InsertRequisicao(string idEmpresa, string referenciaCliente,  string dtRequisicao, string dtValidade,string completa,string nomeFicheiro, string observacoes, string username, byte bContrato, byte bRenovavel)
		{
			SqlParameter[] arrParams = new SqlParameter[10];
    
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inReferenciaCliente", referenciaCliente);
			
			arrParams[2] = new SqlParameter();
			arrParams[2].ParameterName = "@inDtRequisicao"; 
			if(dtRequisicao!="")arrParams[2].Value =DateTime.Parse(dtRequisicao);
			else arrParams[2].Value =dtRequisicao;  

			arrParams[3] = new SqlParameter();
			arrParams[3].ParameterName = "@inDtValidade"; 
            
			if(dtValidade!="")arrParams[3].Value =DateTime.Parse(dtValidade);
			else arrParams[3].Value =dtValidade;  
			
			arrParams[4] = new SqlParameter("@inNomeFicheiro", nomeFicheiro);
			arrParams[5] = new SqlParameter("@inCompleta", GERAL.clsGeral.ConvertStringToBool(completa));
			arrParams[6] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[7] = new SqlParameter("@inUsername", username);

			arrParams[8] = new SqlParameter("@bContrato", bContrato);
			arrParams[9] = new SqlParameter("@bRenovavel", bRenovavel);

			return GERAL.clsDataAccess.ExecuteNonQuerySPOutput("stpInsRequisicao", arrParams); 
		}

		public int UpdateRequisicao(string idRequisicao, string idEmpresa, string referenciaCliente, string dtRequisicao, string dtValidade, string completa, string nomeFicheiro, string observacoes, string username, byte bContrato, byte bRenovavel)
		{
             
			SqlParameter[] arrParams = new SqlParameter[11];

			arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
			arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[2] = new SqlParameter("@inReferenciaCliente", referenciaCliente);
			try
			{
				arrParams[3] = new SqlParameter("@inDtRequisicao", DateTime.Parse(dtRequisicao));
				arrParams[4] = new SqlParameter("@inDtValidade", DateTime.Parse(dtValidade));
			}
			catch
			{
				arrParams[3] = new SqlParameter("@inDtRequisicao", null);
				arrParams[4] = new SqlParameter("@inDtValidade", null);
			}
			arrParams[5] = new SqlParameter("@inCompleta",completa); // GERAL.clsGeral.ConvertStringToBool(completa));
			arrParams[6] = new SqlParameter("@inNomeFicheiro", nomeFicheiro);
			arrParams[7] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[8] = new SqlParameter("@inUsername", username);
			arrParams[9] = new SqlParameter("@bContrato", bContrato);
			arrParams[10] = new SqlParameter("@bRenovavel", bRenovavel);


			return GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdRequisicao", arrParams);  
		}

        public void UpdateRequisicaoConn(SqlConnection myConnection,SqlCommand myCommand,string idRequisicao, string idEmpresa, string referenciaCliente, string dtRequisicao, string dtValidade, string completa, string nomeFicheiro, string observacoes, string username)
		{
             
			SqlParameter[] arrParams = new SqlParameter[9];

			arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
			arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[2] = new SqlParameter("@inReferenciaCliente", referenciaCliente);
			try
			{
				arrParams[3] = new SqlParameter("@inDtRequisicao", DateTime.Parse(dtRequisicao));
				arrParams[4] = new SqlParameter("@inDtValidade", DateTime.Parse(dtValidade));
			}
			catch
			{
				arrParams[3] = new SqlParameter("@inDtRequisicao", null);
				arrParams[4] = new SqlParameter("@inDtValidade", null);
			}
			arrParams[5] = new SqlParameter("@inCompleta",completa); // GERAL.clsGeral.ConvertStringToBool(completa));
			arrParams[6] = new SqlParameter("@inNomeFicheiro", nomeFicheiro);
			arrParams[7] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[8] = new SqlParameter("@inUsername", username);

			GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection,myCommand,"stpUpdRequisicao", arrParams);  
		}
    
//        //transaccao, insere uma requisição com linhas associados, retorna o ID da requisição ou zero em caso de erro
//        public int InsertRequisicaoWithLinhas(string idEmpresa, string referenciaCliente, string dtRequisicao, string dtValidade, string completa,string nomeFicheiro, string observacoes, string username, DataTable DTLinhas)
//        {
//            int result; 
//
//            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
//    
//            using(SqlConnection objConn =  new SqlConnection(connectionString))
//            {
//                SqlCommand cmdReq = new SqlCommand();  
//                cmdReq.CommandType = CommandType.StoredProcedure; 
//                cmdReq.Connection = objConn; 
//			
//                SqlCommand cmdLinha = new SqlCommand(); 
//                cmdLinha.CommandType = CommandType.StoredProcedure; 
//                cmdLinha.Connection = objConn; 
//    
//                objConn.Open(); 
//
//                using (SqlTransaction objTrans = objConn.BeginTransaction())
//                {
//                    cmdReq.Transaction =objTrans; 
//                    cmdLinha.Transaction=objTrans; 
//
//                    try
//                    {						
//                        int idRequisicao = InsertRequisicaoConnn(objConn,cmdReq,idEmpresa, referenciaCliente, dtRequisicao,dtValidade,completa, nomeFicheiro, observacoes,username); 
//
//                        int rows = DTLinhas.Rows.Count; 
//                        
//                        for(int i = 0; i<rows; i++)
//                        {                
//                            if(cmdLinha.Parameters.Count > 0) cmdLinha.Parameters.Clear(); 
//
//                            string idTipoServico= DTLinhas.Rows[i]["idTipoServico"].ToString(); 
//                            string idTipoEquipamento= DTLinhas.Rows[i]["idTipoEquipamento"].ToString();
//                            int quantidade= GERAL.clsGeral.ConvertStringToInt(DTLinhas.Rows[i]["quantidade"].ToString());  
//                            string descricaoEquipamento= DTLinhas.Rows[i]["descricaoEquipamento"].ToString();
//                            double preco= GERAL.clsGeral.ConvertStringToDouble(DTLinhas.Rows[i]["preco"].ToString());  
//                            string obs= DTLinhas.Rows[i]["observacoes"].ToString();
//               
//                            SqlParameter[] arrParams = new SqlParameter[8];
//
//                            arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
//                            arrParams[1] = new SqlParameter("@inIdTipoServico", idTipoServico);
//                            arrParams[2] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
//                            arrParams[3] = new SqlParameter("@inQuantidade", quantidade);
//                            arrParams[4] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
//                            arrParams[5] = new SqlParameter("@inPreco", preco);
//                            arrParams[6] = new SqlParameter("@inObservacoes", obs);
//                            arrParams[7] = new SqlParameter("@inUsername", username);
//
//
//                            //LabMetro.GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection,myCommand,"stpInsServico", arrParams);
//                            //   
//                            cmdLinha.CommandText = "stpInsRequisicaoLinha"; 
//                            
//                            foreach (SqlParameter p in arrParams)
//                            {
//                                //check for derived output value with no value assigned
//                                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
//                                {
//                                    p.Value = DBNull.Value;
//                                }
//				
//                                cmdLinha.Parameters.Add(p);
//                            }
//                            cmdLinha.ExecuteNonQuery(); 
//                        }
//
//                        objTrans.Commit(); 
//                        result = idRequisicao;	
//                    }
//                    catch(SqlException ex)
//                    { 	
//                        try
//                        {	
//                            objTrans.Rollback();
//                        }
//                        catch(Exception exep)
//                        {
//                            GERAL.clsWriteError.WriteLog(exep); 
//                            result = 0; 
//                        }
//
//                        GERAL.clsWriteError.WriteLog(ex); 
//                        result = 0; 
//                    }
//                }
//            }
//            return result; 
//        }


//
//        public string UpdateRequisicaoWithLinhas(string idRequisicao, string idEmpresa, string referenciaCliente, string dtRequisicao, string dtValidade, string completa, string nomeFicheiro, string observacoes, string username, DataView DV)
//
//        {
//            string result; 
//
//            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
//    
//            using(SqlConnection objConn =  new SqlConnection(connectionString))
//            {
//                SqlCommand cmdReq = new SqlCommand();  
//                cmdReq.CommandType = CommandType.StoredProcedure; 
//                cmdReq.Connection = objConn; 
//			
//                SqlCommand cmdLinha = new SqlCommand(); 
//                cmdLinha.CommandType = CommandType.StoredProcedure; 
//                cmdLinha.Connection = objConn; 
//    
//                objConn.Open(); 
//
//                using (SqlTransaction objTrans = objConn.BeginTransaction())
//                {
//                    cmdReq.Transaction =objTrans; 
//                    cmdLinha.Transaction=objTrans; 
//
//                    try
//                    {						
//                        
//                      //aqui fazer updateconn InsertRequisicaoConnn(objConn,cmdReq,idEmpresa, referenciaCliente, dtRequisicao, "", nomeFicheiro, observacoes,  username); 
//
//                        UpdateRequisicaoConn(objConn,cmdReq,idRequisicao,idEmpresa,referenciaCliente, dtRequisicao,dtValidade,completa,nomeFicheiro,observacoes,username); 
//                        
//                        
//                        DV.RowStateFilter= DataViewRowState.ModifiedCurrent; 
//                        
//                        foreach(DataRowView drv in DV)
//                        {
//                            //UPDATE
//    
//                            if(cmdLinha.Parameters.Count > 0) cmdLinha.Parameters.Clear(); 
//
//                            //????
//                            string idRequisicaoLinha= drv["idRequisicaoLinha"].ToString(); 
//                            string idTipoServico= drv["idTipoServico"].ToString(); 
//                            string idTipoEquipamento= drv["idTipoEquipamento"].ToString();
//                            int quantidade= GERAL.clsGeral.ConvertStringToInt(drv["quantidade"].ToString());  
//                            string descricaoEquipamento= drv["descricaoEquipamento"].ToString();
//                            double preco= GERAL.clsGeral.ConvertStringToDouble(drv["preco"].ToString());  
//                            string obs= drv["observacoes"].ToString();
//               
//                            SqlParameter[] arrParams = new SqlParameter[9];
//
//                            arrParams[0] = new SqlParameter("@inIdRequisicaoLinha", idRequisicaoLinha);
//                            arrParams[1] = new SqlParameter("@inIdRequisicao", idRequisicao);
//                            arrParams[2] = new SqlParameter("@inIdTipoServico", idTipoServico);
//                            arrParams[3] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
//                            arrParams[4] = new SqlParameter("@inQuantidade", quantidade);
//                            arrParams[5] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
//                            arrParams[6] = new SqlParameter("@inPreco", preco);
//                            arrParams[7] = new SqlParameter("@inObservacoes", obs);
//                            arrParams[8] = new SqlParameter("@inUsername", username);
//
//                              
//                            cmdLinha.CommandText = "stpUpdRequisicaoLinha"; 
//                            
//                            foreach (SqlParameter p in arrParams)
//                            {
//                                //check for derived output value with no value assigned
//                                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
//                                {
//                                    p.Value = DBNull.Value;
//                                }
//				
//                                cmdLinha.Parameters.Add(p);
//                            }
//
//                            cmdLinha.ExecuteNonQuery(); 
//
//                        }
//
//                        DV.RowStateFilter= DataViewRowState.Added;
//                        foreach(DataRowView drv in DV)
//                        {
//                            if(cmdLinha.Parameters.Count > 0) cmdLinha.Parameters.Clear(); 
//
//                            string idTipoServico= drv["idTipoServico"].ToString(); 
//                            string idTipoEquipamento= drv["idTipoEquipamento"].ToString();
//                            int quantidade= GERAL.clsGeral.ConvertStringToInt(drv["quantidade"].ToString());  
//                            string descricaoEquipamento= drv["descricaoEquipamento"].ToString();
//                            double preco= GERAL.clsGeral.ConvertStringToDouble(drv["preco"].ToString());  
//                            string obs= drv["observacoes"].ToString();
//               
//                            SqlParameter[] arrParams = new SqlParameter[8];
//
//                            arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
//                            arrParams[1] = new SqlParameter("@inIdTipoServico", idTipoServico);
//                            arrParams[2] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
//                            arrParams[3] = new SqlParameter("@inQuantidade", quantidade);
//                            arrParams[4] = new SqlParameter("@inDescricaoEquipamento", descricaoEquipamento);
//                            arrParams[5] = new SqlParameter("@inPreco", preco);
//                            arrParams[6] = new SqlParameter("@inObservacoes", obs);
//                            arrParams[7] = new SqlParameter("@inUsername", username);
//
//
//                            //LabMetro.GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection,myCommand,"stpInsServico", arrParams);
//                            //   
//                            cmdLinha.CommandText = "stpInsRequisicaoLinha"; 
//                            
//                            foreach (SqlParameter p in arrParams)
//                            {
//                                //check for derived output value with no value assigned
//                                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
//                                {
//                                    p.Value = DBNull.Value;
//                                }
//				
//                                cmdLinha.Parameters.Add(p);
//                            }
//
//                            cmdLinha.ExecuteNonQuery(); 
//                        }
//
//                        DV.RowStateFilter= DataViewRowState.Deleted;
//                        foreach(DataRowView drv in DV)
//                        {
//                           
//                            if(cmdLinha.Parameters.Count > 0) cmdLinha.Parameters.Clear(); 
//
//                            string idRequisicaoLinha= drv["idRequisicaoLinha"].ToString(); 
//                            SqlParameter[] arrParams = new SqlParameter[2];
//
//                            arrParams[0] = new SqlParameter("@inIdRequisicaoLinha", idRequisicaoLinha);
//                            arrParams[1] = new SqlParameter("@inUsername", username);
//
//                            cmdLinha.CommandText = "stpDelRequisicaoLinha"; 
//                            
//                            foreach (SqlParameter p in arrParams)
//                            {
//                                //check for derived output value with no value assigned
//                                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
//                                {
//                                    p.Value = DBNull.Value;
//                                }
//				
//                                cmdLinha.Parameters.Add(p);
//                            }
//                           
//                            cmdLinha.ExecuteNonQuery(); 
//                        }
//
//                        objTrans.Commit(); 
//                        //result =  GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB; 	
//                        result ="0"; 
//                    }
//
//                    catch(SqlException ex)
//                    { 	
//                        try
//                        {	
//                            objTrans.Rollback();
//                        }
//                        catch(Exception exep)
//                        {
//                            GERAL.clsWriteError.WriteLog(exep); 
//                            //result =  GERAL.clsGeral.ErrorMessage.ERR_UPDATE; 
//                            result = "1"; 
//                        }
//
//                        GERAL.clsWriteError.WriteLog(ex); 
//                
//                        //result= GERAL.clsGeral.ErrorMessage.ERR_UPDATE; 
//                        result = "1"; 
//                    }
//                }
//            }
//            return result; 
//        }

        public void ApagaFicheiroRequisicao(string idRequisicao, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            
            arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
            arrParams[1] = new SqlParameter("@inUsername", username);

            GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdRequisicaoFicheiro",arrParams); 
        }


		// Funcao que devolve todas as Requisições de uma dada Empresa
		// que podem ser adicionadas a um Serviço de um BRE (incompletas)
		public SqlDataReader DRGetRequisicoesIncompletasByEmpresa(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
			SqlDataReader EquipamentosDR = LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetRequisicoesIncompletasByEmpresa", arrParams); 
           
			return EquipamentosDR;
		}

		


		public DataTable DTGetRequisicoesIncompletasByEmpresa(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetRequisicoesIncompletasByEmpresa", arrParams); 
         }

		public DataTable DTGetRequisicoesIncompletasByEmpresaServico(string idEmpresa, string refServico)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inRefServico", refServico);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetRequisicoesIncompletasByEmpresaServico", arrParams); 
		}

        public DataTable DTListaRequisicoes(string idEmpresa, string refReq, string validade, string completa,bool ficheiro, bool contrato)
        {
            SqlParameter[] arrParams = new SqlParameter[6];
            
            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[1] = new SqlParameter("@inRefRequisicao", refReq);
			arrParams[2] = new SqlParameter("@inValidade", validade);//Não é usado como param
			arrParams[3] = new SqlParameter("@inCompletas", completa);
			arrParams[4] = new SqlParameter("@inFicheiro", ficheiro);
			arrParams[5] = new SqlParameter("@bContrato", contrato);
			
			string strSQL = "SELECT idRequisicao, dtRequisicao,dtValidade,numRequisicao,ano, referenciaCliente,completa, nomeFicheiro , R.dtCriacao,  dbo.udfGetReferenciaRequisicao(idRequisicao) AS refRequisicao  , ISNULL(E.nomeAbreviado, E.nome) AS empresa, E.observacoes as observacoes,E.nivelBloqueioLabmetro,E.codigoBloqueioSAP, ISNULL(R.bContrato,0) as bContrato,ISNULL(R.bRenovavel,0) as bRenovavel, ISNULL(E.bContrato,0) as eContrato, R.observacoes as obsReq FROM Requisicao R INNER JOIN Empresa E ON R.idEmpresa = E.idEmpresa  WHERE 1 = 1 "; 

			if(contrato == true)
			{
				strSQL += " AND R.bContrato = 1 "; 
			}

			if(ficheiro == true)
			{
				strSQL += " AND nomeFicheiro is not null "; 
			}

			if(completa != "")
			{
				strSQL += " AND R.completa = @inCompletas "; 
			}
			
			if(idEmpresa != "")
			{
				strSQL += " AND R.idEmpresa = @inIdEmpresa "; 
			}

			if(refReq != "")
			{
				strSQL += " AND dbo.udfGetReferenciaRequisicao(R.idRequisicao) LIKE '" +refReq + "%' "; 
			}

			if(validade != "")
			{
				 if(validade == "1") 
					strSQL +=" AND ((R.dtValidade IS NOT NULL AND R.dtValidade > getDate()) OR R.dtValidade IS NULL) "; 
				else if(validade == "0") 
					strSQL +=" AND ((R.dtValidade IS NOT NULL AND R.dtValidade < getDate()) OR R.dtValidade IS NULL) "; 
			}
			
			 
			strSQL +=" ORDER BY idRequisicao DESC "; 

			//HttpContext.Current.Response.Write(strSQL); 
			
			return GERAL.clsDataAccess.ExecuteDT(strSQL,arrParams); 
			
			//quando me ocorrer qq coisa, passo para storedprocedure
            //return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListRequisicoes", arrParams); 

			
           
        }


		public DataTable DTListaRequisicoesByIdEmpresa(string idEmpresa)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			
			string strSQL = "SELECT idRequisicao, dtRequisicao,dtValidade,numRequisicao,ano, referenciaCliente,completa, nomeFicheiro , R.dtCriacao,  dbo.udfGetReferenciaRequisicao(idRequisicao) AS refRequisicao  , ISNULL(E.nomeAbreviado, E.nome) AS empresa, E.observacoes as observacoes,E.nivelBloqueioLabmetro,E.codigoBloqueioSAP, ISNULL(R.bContrato,0) as bContrato,ISNULL(R.bRenovavel,0) as bRenovavel, ISNULL(E.bContrato,0) as eContrato  FROM Requisicao R INNER JOIN Empresa E ON R.idEmpresa = E.idEmpresa  WHERE R.idEmpresa = @inIdEmpresa  ORDER BY idRequisicao DESC "; 

			//HttpContext.Current.Response.Write(strSQL); 
			
			return GERAL.clsDataAccess.ExecuteDT(strSQL,arrParams); 
			
			//quando me ocorrer qq coisa, passo para storedprocedure
			//return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListRequisicoes", arrParams); 

			
           
		}
        public void UpdateRequisicaoCompleta(string idRequisicao,  string completa, string username)
        {
             
            SqlParameter[] arrParams = new SqlParameter[3];

            arrParams[0] = new SqlParameter("@inIdRequisicao", idRequisicao);
            arrParams[1] = new SqlParameter("@inCompleta", GERAL.clsGeral.ConvertStringToBool(completa));
            arrParams[2] = new SqlParameter("@inUsername", username);

            GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdRequisicaoSetComplete", arrParams);  
        }

		public DataTable DTEmpresasActivas()
		{
			return GERAL.clsDataAccess.SPExecuteDT("stpGetEmpresasActivas"); 
		}


		public DataSet DSServicosSemRequisicao(string strNumBRE,string strAno, string strEmpresa, string strRefServico,bool bFactura, bool bSemFactura)
		{
			DATASETS.DSServicosSemRequisicao DS = new LabMetro.DATASETS.DSServicosSemRequisicao();

			string strSQL = "SELECT s.idServico,bre.numBRE,bre.idBRE, bre.idEmpresa as idEmpresa, bre.dtBre, dbo.udfGetReferenciaBRE(BRE.idBRE) AS refBRE, s.idFactura, s.refServico ,es.descricao AS estado, e.nomeAbreviado AS empresa, e.observacoes as obsEmpresa, tipoEquipamento.descricao as equipamento, EC.nomeAbreviado as EmpContrat FROM servico s INNER JOIN bre ON s.idBre = bre.idBre INNER JOIN EstadoServico ES ON s.idEstadoServico = es.idEstadoServico INNER JOIN empresa e ON bre.idEmpresa = e.idEmpresa  LEFT JOIN Empresa EC ON BRE.idEmpresaContratante = EC.idEmpresa INNER JOIN equipamento on s.idEquipamento = equipamento.idEquipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento  WHERE s.idRequisicao IS NULL AND es.idEstadoServico NOT IN (-1,20,21,22,7) AND (s.ano > 2006 or s.idFactura is null) "; 

			//SqlParameter[] arrParams = new SqlParameter[4];       

			if(bFactura == true)
			{
				strSQL += " AND s.idfactura IS NOT NULL";  
			}

			if(bSemFactura == true)
			{
				strSQL += " AND s.idfactura IS NULL and  ISNULL(EC.bPodeFacturarSemRequisicao,E.bPodeFacturarSemRequisicao) = 0";  //nao esquecer que pode haver aqui uma empresa contratante....
			}

			if(strNumBRE !="") strSQL += " AND  dbo.udfGetReferenciaBRE(BRE.idBRE) LIKE '"+strNumBRE+"%'"; 
			if(strAno !="") strSQL += " AND s.ano = '"+strAno+"'"; 
			if(strEmpresa !="") strSQL += " AND e.nome LIKE '"+strEmpresa+"%'"; 
			if(strRefServico !="") strSQL += " AND s.refServico LIKE '"+strRefServico+"%'"; 

			strSQL += " ORDER BY BRE.dtBRE "; 
			//HttpContext.Current.Response.Write(strSQL); 
		
			return GERAL.clsDataAccess.DSFillDS(strSQL,DS,"dtServicosSemRequisicao"); 
			
			
		}

		public DataSet DSServicosSemRequisicao2(string idEmpresa, string idBre, bool bFactura, bool bSemFactura)
		{
			DATASETS.DSServicosSemRequisicao DS = new LabMetro.DATASETS.DSServicosSemRequisicao();

			string strSQL = "SELECT s.idServico,bre.numBRE,bre.idBRE, bre.idEmpresa as idEmpresa, bre.dtBre, dbo.udfGetReferenciaBRE(BRE.idBRE) AS refBRE, s.idFactura, s.refServico ,es.descricao AS estado, e.nomeAbreviado AS empresa, e.observacoes as obsEmpresa, tipoEquipamento.descricao as equipamento, EC.nomeAbreviado as EmpContrat FROM servico s INNER JOIN bre ON s.idBre = bre.idBre INNER JOIN EstadoServico ES ON s.idEstadoServico = es.idEstadoServico INNER JOIN empresa e ON bre.idEmpresa = e.idEmpresa  LEFT JOIN Empresa EC ON BRE.idEmpresaContratante = EC.idEmpresa INNER JOIN equipamento on s.idEquipamento = equipamento.idEquipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento  WHERE s.idRequisicao IS NULL AND es.idEstadoServico NOT IN (-1,20,21,22,7) AND (s.ano > 2006 or s.idFactura is null) ";

			//SqlParameter[] arrParams = new SqlParameter[4];       

			if (bFactura == true)
			{
				strSQL += " AND s.idfactura IS NOT NULL";
			}

			if (bSemFactura == true)
			{
				strSQL += " AND s.idfactura IS NULL and  ISNULL(EC.bPodeFacturarSemRequisicao,E.bPodeFacturarSemRequisicao) = 0";  //nao esquecer que pode haver aqui uma empresa contratante....
			}

			if (idBre != "") strSQL += " AND  bre.idbre = '" + idBre + "'";
			if (idEmpresa != "") strSQL += " AND bre.idEmpresa= '" + idEmpresa + "'";
			

			strSQL += " ORDER BY BRE.dtBRE ";
			//HttpContext.Current.Response.Write(strSQL); 

			return GERAL.clsDataAccess.DSFillDS(strSQL, DS, "dtServicosSemRequisicao");


		}


		public SqlDataReader drEmpresasComServicosSemRequisicao( bool bFactura, bool bSemFactura)///mesma query que acima com um distinct empresa

		{
			string strSQL = "SELECT distinct  bre.idEmpresa as idEmpresa, e.nomeAbreviado AS empresa FROM servico s INNER JOIN bre ON s.idBre = bre.idBre INNER JOIN EstadoServico ES ON s.idEstadoServico = es.idEstadoServico INNER JOIN empresa e ON bre.idEmpresa = e.idEmpresa  LEFT JOIN Empresa EC ON BRE.idEmpresaContratante = EC.idEmpresa INNER JOIN equipamento on s.idEquipamento = equipamento.idEquipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento  WHERE s.idRequisicao IS NULL AND es.idEstadoServico NOT IN (-1,20,21,22,7) AND (s.ano > 2006 or s.idFactura is null) ";

			//SqlParameter[] arrParams = new SqlParameter[4];       

			if (bFactura == true)
			{
				strSQL += " AND s.idfactura IS NOT NULL";
			}

			if (bSemFactura == true)
			{
				strSQL += " AND s.idfactura IS NULL and  ISNULL(EC.bPodeFacturarSemRequisicao,E.bPodeFacturarSemRequisicao) = 0";  //nao esquecer que pode haver aqui uma empresa contratante....
			}


			strSQL += " ORDER BY e.nomeAbreviado";

			return GERAL.clsDataAccess.ExecuteDR(strSQL);


		}



		public SqlDataReader drBREdeEmpresasComServicosSemRequisicao(bool bFactura, bool bSemFactura,string idEmpresa)///mesma query que acima com um distinct empresa

		{
			string strSQL = "SELECT distinct  bre.idBRE,  dbo.udfGetReferenciaBRE(BRE.idBRE) AS refBRE FROM servico s INNER JOIN bre ON s.idBre = bre.idBre INNER JOIN EstadoServico ES ON s.idEstadoServico = es.idEstadoServico INNER JOIN empresa e ON bre.idEmpresa = e.idEmpresa  LEFT JOIN Empresa EC ON BRE.idEmpresaContratante = EC.idEmpresa INNER JOIN equipamento on s.idEquipamento = equipamento.idEquipamento inner join tipoEquipamento on equipamento.idTipoEquipamento = tipoEquipamento.idTipoEquipamento  WHERE bre.idEmpresa = '" + idEmpresa+ "' AND s.idRequisicao IS NULL AND es.idEstadoServico NOT IN (-1,20,21,22,7) AND (s.ano > 2006 or s.idFactura is null) ";

			//SqlParameter[] arrParams = new SqlParameter[4];       

			if (bFactura == true)
			{
				strSQL += " AND s.idfactura IS NOT NULL";
			}

			if (bSemFactura == true)
			{
				strSQL += " AND s.idfactura IS NULL and  ISNULL(EC.bPodeFacturarSemRequisicao,E.bPodeFacturarSemRequisicao) = 0";  //nao esquecer que pode haver aqui uma empresa contratante....
			}


			strSQL += " ORDER BY bre.idbre desc";

			return GERAL.clsDataAccess.ExecuteDR(strSQL);


		}
		
	}
}
