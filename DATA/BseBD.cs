using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for BseBD.
	/// </summary>
	/// 

	// Classe que guarda nos seus atributos os detalhes de um BSE
	public class BseDetails
	{
		public string idBSE;
		public string idEmpresa;
		public string nomeEmpresa;
		public string idFuncionarioSaida;
		public string nomeFuncionarioSaida;
		public string numBSE;
		public string ano;
		public string refBSE;
		public string dtBSE;
		public string recebidoPor;
		public string observacoes;
        public string refOrcamento;
        public string refRequisicao;
	}

	public class BseBD
	{
		public BseBD()
		{
		}
        // Funcao que devolve um BSE com base no seu ID
        public BseDetails GetBseDetails(string idBSE)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
            arrParams[0] = new SqlParameter("@inIdBSE", idBSE);
         
            DataTable BseDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetBseById", arrParams); 
           
            if(BseDT.Rows.Count > 0) 
            { 
                BseDetails myBseDetails= new BseDetails();
                SetBseDetails(BseDT, myBseDetails);
                return myBseDetails;
            }
            else
            {
                return null; 
            }
        }

        // Preenche todos os atributos da classe BseDetails a partir de uma DataTable
        private void SetBseDetails(DataTable BseDT, BseDetails myBseDetails)
        {
            myBseDetails.idBSE = BseDT.Rows[0]["idBSE"].ToString();
            myBseDetails.idEmpresa = BseDT.Rows[0]["idEmpresa"].ToString();
            myBseDetails.idFuncionarioSaida = BseDT.Rows[0]["idFuncionarioSaida"].ToString();
			myBseDetails.nomeEmpresa = BseDT.Rows[0]["empresa"].ToString();
			myBseDetails.nomeFuncionarioSaida = BseDT.Rows[0]["nomeFuncionarioSaida"].ToString();
			myBseDetails.numBSE = BseDT.Rows[0]["numBSE"].ToString();
            myBseDetails.ano = BseDT.Rows[0]["ano"].ToString();
            myBseDetails.refBSE = BseDT.Rows[0]["refBSE"].ToString();
            myBseDetails.dtBSE = BseDT.Rows[0]["dtBSE"].ToString();
            myBseDetails.recebidoPor = BseDT.Rows[0]["recebidoPor"].ToString();
            myBseDetails.observacoes = BseDT.Rows[0]["observacoes"].ToString();
            myBseDetails.refOrcamento = BseDT.Rows[0]["refOrcamento"].ToString();
            myBseDetails.refRequisicao = BseDT.Rows[0]["refRequisicao"].ToString();


        }

        // Lista Empresas que estao associadas à BSE's
        public SqlDataReader DRListaEmpresasForListaBSE()
        { 
            return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasForListBSE");            
        }

		public DataTable DTEmpresasForListaBSE(string strEmpresa, string strNIF)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inNome", strEmpresa);
			arrParams[1] = new SqlParameter("@inNif", strNIF);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForListBSE", arrParams); 
		}

        public DataTable DTBse(string idEmpresa, string refBSE)
        {
			SqlParameter[] arrParams = new SqlParameter[2];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inRefBSE", refBSE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListBSEs", arrParams); 
           
        }

        //insere um bse dentro de uma transaccao e retorna o id inserido.
        public int InsertBseConn(SqlConnection myConnection,SqlCommand myCommand, string idEmpresa, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, string refOrcamento, string refRequisicao)
        {
            SqlParameter[] arrParams = new SqlParameter[7];

            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[1] = new SqlParameter("@inIdFuncionarioSaida", idFuncionarioSaida);
            arrParams[2] = new SqlParameter("@inRecebidoPor", recebidoPor);
            arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
            arrParams[4] = new SqlParameter("@inUsername", username);
            arrParams[5] = new SqlParameter("@refOrcamento", refOrcamento);
            arrParams[6] = new SqlParameter("@refRequisicao", refRequisicao);
            
            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput(myConnection,myCommand,"stpInsBSE", arrParams); 

        }

        public void InsertBse(SqlConnection myConnection,SqlCommand myCommand, string idEmpresa, string idFuncionarioSaida, string recebidoPor, string observacoes, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[5];

            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[1] = new SqlParameter("@inIdFuncionarioSaida", idFuncionarioSaida);
            arrParams[2] = new SqlParameter("@inRecebidoPor", recebidoPor);
            arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
            arrParams[4] = new SqlParameter("@inUsername", username);

            GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection,myCommand,"stpInsBSE", arrParams); 

        }

        //Funcao que actualiza um BRE dentro de uma transaccao
        public void UpdateBseConn(SqlConnection myConnection,SqlCommand myCommand,string idBSE, string idEmpresa, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, string refOrcamento, string refRequisicao)
        {
            SqlParameter[] arrParams = new SqlParameter[8];

            arrParams[0] = new SqlParameter("@inIdBSE", idBSE);
            arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
            arrParams[2] = new SqlParameter("@inIdFuncionarioSaida", idFuncionarioSaida);
            arrParams[3] = new SqlParameter("@inRecebidoPor", recebidoPor);
            arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
            arrParams[5] = new SqlParameter("@inUsername", username);
            arrParams[6] = new SqlParameter("@refOrcamento", refOrcamento);
            arrParams[7] = new SqlParameter("@refRequisicao", refRequisicao);

            GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection, myCommand,"stpUpdBSE", arrParams); 

        }

        //transaccao, insere um bse com serviços associados, retorna o ID do BSE
        public int InsertBseWithServices(string idEmpresa, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, DataTable DTServices, string refOrcamento, string refRequisicao)

        {
            int retValue; 

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
                        int id = InsertBseConn(objConn,objCmd,idEmpresa,idFuncionarioSaida,recebidoPor,observacoes,username, refOrcamento,refRequisicao); 
                        
                        int rows = DTServices.Rows.Count; 
                        
                        for(int i = 0; i<rows; i++)

                        {                
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

                            string idServico= DTServices.Rows[i]["idServico"].ToString(); 
                            
                            string idEstadoServico= DTServices.Rows[i]["idEstadoServico"].ToString();  
                            
                            string obs= DTServices.Rows[i]["Observacoes"].ToString();  
               
							string idLocalCalibracao = DTServices.Rows[i]["idLocalCalibracao"].ToString(); 


                            SqlParameter[] arrParams = new SqlParameter[6];

                            arrParams[0] = new SqlParameter("@inIdBSE", id);
                            arrParams[1] = new SqlParameter("@inIdServico", idServico);
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);
							arrParams[5] = new SqlParameter("@inIdLocalCalibracao", idLocalCalibracao);

							//objCmd.CommandText = "stpUpdServicoInBSE"; 

                            objCmd.CommandText = "stpUpdServicoInBSE"; 
                            
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
						// Se não houver erro devolvemos o ID do BSE
						retValue = id;
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
							// Se houver erro devolvemos o ID do BSE a zero
							retValue = 0; 
                        }

                        GERAL.clsWriteError.WriteLog(ex); 
						// Se houver erro devolvemos o ID do BSE a zero
						retValue = 0;
                    }
                }
            }
            return retValue; 
        }

        //transaccao, altera um bse com serviços associados, retorna um valor conforme correu bem ou mal
        //1=correu bem - 0=correu mal
        public int UpdateBseWithServices(string idBSE, string idEmpresa, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, DataTable DTDestino, string refOrcamento, string refRequisicao)

        {
            int retValue; 

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
                    objCmd.Transaction=objTrans; 

                    try
                    {						
                        //UPDATE
                        UpdateBseConn(objConn,objCmd,idBSE,idEmpresa,idFuncionarioSaida,recebidoPor,observacoes,username, refOrcamento, refRequisicao); 
                        //


                        //remover as rows que foram removidas    
                        DataRow[] dRows = DTDestino.Select(null,null,DataViewRowState.Deleted); 
                          
                        //System.Data.DeletedRowInaccessibleException: Deleted row information cannot be accessed through the row
                        //aceder atraves de:
                        //DataRow.Item("itemname", DataRowVersion.Original)
                        foreach(DataRow dRow in dRows)
                        {
                            
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            string idServico=  dRow["idServico",DataRowVersion.Original].ToString(); 
                            string idEstadoServico= dRow["idEstadoServico",DataRowVersion.Original].ToString(); 
                            string obs= dRow["Observacoes",DataRowVersion.Original].ToString(); 
							string idLocalCalibracao = dRow["idLocalCalibracao",DataRowVersion.Original].ToString();
						
                            
                            SqlParameter[] arrParams = new SqlParameter[6];

                            arrParams[0] = new SqlParameter("@inIdBSE", "");
                            arrParams[1] = new SqlParameter("@inIdServico", idServico);
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);
							arrParams[5] = new SqlParameter("@inIdLocalCalibracao", idLocalCalibracao);

							//objCmd.CommandText = "stpUpdServicoInBSE"; 

                            objCmd.CommandText = "stpUpdServicoInBSE"; 
                            
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

                        //DTDestino = linhas  associadas ao BSE
                        //fazer update do idBSE para idBSE
                        DataRow[] dRws = DTDestino.Select(null,null,DataViewRowState.CurrentRows);                                    
                        foreach(DataRow dRw in dRws)
                        {
                                     
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            string idServico= dRw["idServico"].ToString(); 
                            string idEstadoServico= dRw["idEstadoServico"].ToString(); 
							string idLocalCalibracao = dRw["idLocalCalibracao"].ToString(); 
                            string obs= dRw["Observacoes"].ToString();  
               
                            SqlParameter[] arrParams = new SqlParameter[6];

                            arrParams[0] = new SqlParameter("@inIdBSE", idBSE);
                            arrParams[1] = new SqlParameter("@inIdServico", idServico);
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);
							arrParams[5] = new SqlParameter("@inIdLocalCalibracao", idLocalCalibracao);

                            objCmd.CommandText = "stpUpdServicoInBSE"; 
                            
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
                        objTrans.Commit(); 
                        
                        retValue = 1;
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
                        
                            retValue = 0; 
                        }

                        GERAL.clsWriteError.WriteLog(ex); 
                        
                        retValue = 0;
                    }
                }
            }
            return retValue; 
            //1=correu bem
        }

		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSBSE(string idBse)
		{
			LabMetro.DATASETS.DSBSE ds = new LabMetro.DATASETS.DSBSE(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 

				SqlDataAdapter da = new SqlDataAdapter(objCmd);
			
				objCmd.CommandText= "SELECT BSE.idBSE, dbo.udfGetReferenciaBSE(BSE.idBSE) AS refBSE, BSE.dtBSE, Empresa.nome AS empresa, Empresa.morada, Empresa.nif, Empresa.codigoPostal, empresa.localidadePostal, empresa.localidade, Funcionario.nome AS entreguePor, BSE.recebidoPor, BSE.observacoes, Empresa.numObra,Empresa.NIF, bse.refOrcamento, bse.refRequisicao FROM  Empresa INNER JOIN BSE ON bse.idBSE = " + idBse + " AND Empresa.idEmpresa = BSE.idEmpresa INNER JOIN Funcionario ON BSE.idFuncionarioSaida = Funcionario.idFuncionario"; 
				
				try
				{
					da.Fill(ds,"dtBse"); 
				}
				catch(Exception ex)
				{
                    GERAL.clsWriteError.WriteLog(ex.ToString());
				}

				objCmd.CommandText =  "SELECT Servico.idServico, Servico.idBRE, TipoEquipamento.descricao AS tipoEquipamento, Equipamento.numIdentificacao, Requisicao.referenciaCliente, Requisicao.dtRequisicao, Servico.observacoes, TipoServico.descricao AS tipoServico, Servico.refServico, CASE WHEN CAST(Servico.idEstadoServico AS VARCHAR(10)) = '15' THEN 'Não' WHEN CAST(Servico.idEstadoServico AS VARCHAR(10)) = '14' THEN 'Sim' END AS comCertificado, Servico.idBSE, Servico.idSubcontratoBRE, Servico.idSubcontratoBSE, Servico.idEstadoServico, Equipamento.numSerie, servico.acessorios, EstadoServico.descricao as estadoServico FROM Servico INNER JOIN Equipamento ON Servico.idEquipamento = Equipamento.idEquipamento INNER JOIN TipoEquipamento ON Equipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento INNER JOIN TipoServico ON Servico.idTipoServico = TipoServico.idTipoServico LEFT OUTER JOIN  Requisicao ON Servico.idRequisicao = Requisicao.idRequisicao INNER JOIN EstadoServico on servico.idestadoServico = estadoServico.idEstadoServico WHERE Servico.idEstadoServico <> (-1) AND Servico.idBse = " + idBse + " ORDER BY Servico.idServico"; 
				//quero q os anulados não apareçam.<>7
				 
				//objCmd.Connection = objConn; 

				try
				{
					da.Fill(ds,"dtServico");
				}
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex.ToString());
				}
				
				da.Dispose();
				
			}
			return ds; 
		}

	}
}
