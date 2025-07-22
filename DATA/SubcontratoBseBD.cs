using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for SubcontratoBseBD.
	/// </summary>
	/// 

	// Classe que guarda nos seus atributos os detalhes de um BSE de um Subcontrato
	public class SubcontratoBseDetails
	{
		public string idBRE;
		public string refBRE;
		public string nomeEmpresaSubcont;
		public string idEmpresa;
		public string nomeEmpresa;
		public string idSubcontratoBSE;
		public string idEmpresaSubcontratada;
		public string idFuncionarioSaida;
		public string nomeFuncionarioSaida;
		public string numSubcontratoBSE;
		public string ano;
		public string refSubcontratoBSE;
		public string dtSubcontratoBSE;
		public string recebidoPor;
		public string observacoes;
        public bool bCertNomeCliente; 
	}

	public class SubcontratoBseBD
	{
		public SubcontratoBseBD()
		{
		}

        // Funcao que devolve um BSE de um Subcontrato com base no seu ID
        public SubcontratoBseDetails GetSubcontratoBseDetails(string idSubcontratoBSE)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
            arrParams[0] = new SqlParameter("@inIdSubcontratoBSE", idSubcontratoBSE);
         
            DataTable SubcontratoBseDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetSubcontratoBseById", arrParams); 
           
            if(SubcontratoBseDT.Rows.Count > 0) 
            { 
                SubcontratoBseDetails mySubcontratoBseDetails= new SubcontratoBseDetails();
                SetSubcontratoBseDetails(SubcontratoBseDT, mySubcontratoBseDetails);
                return mySubcontratoBseDetails;
            }
            else
            {
                return null; 
            }
        }

        // Preenche todos os atributos da classe SubcontratoBseDetails a partir de uma DataTable
        private void SetSubcontratoBseDetails(DataTable SubcontratoBseDT, SubcontratoBseDetails mySubcontratoBseDetails)
        {
            mySubcontratoBseDetails.idBRE = SubcontratoBseDT.Rows[0]["idBRE"].ToString();
            mySubcontratoBseDetails.refBRE = SubcontratoBseDT.Rows[0]["refBRE"].ToString();
            mySubcontratoBseDetails.idEmpresa = SubcontratoBseDT.Rows[0]["idEmpresa"].ToString();
            mySubcontratoBseDetails.nomeEmpresa = SubcontratoBseDT.Rows[0]["empresa"].ToString();
            mySubcontratoBseDetails.idSubcontratoBSE = SubcontratoBseDT.Rows[0]["idSubcontratoBSE"].ToString();
            mySubcontratoBseDetails.idEmpresaSubcontratada = SubcontratoBseDT.Rows[0]["idEmpresaSubcontratada"].ToString();
            mySubcontratoBseDetails.nomeEmpresaSubcont = SubcontratoBseDT.Rows[0]["nomeEmpresaSubcont"].ToString();
            mySubcontratoBseDetails.idFuncionarioSaida = SubcontratoBseDT.Rows[0]["idFuncionarioSaida"].ToString();
            mySubcontratoBseDetails.nomeFuncionarioSaida = SubcontratoBseDT.Rows[0]["nomeFuncionarioSaida"].ToString();
            mySubcontratoBseDetails.numSubcontratoBSE = SubcontratoBseDT.Rows[0]["numSubcontratoBSE"].ToString();
            mySubcontratoBseDetails.ano = SubcontratoBseDT.Rows[0]["ano"].ToString();
            mySubcontratoBseDetails.refSubcontratoBSE = SubcontratoBseDT.Rows[0]["refSubcontratoBSE"].ToString();
            mySubcontratoBseDetails.dtSubcontratoBSE = SubcontratoBseDT.Rows[0]["dtSubcontratoBSE"].ToString();
            mySubcontratoBseDetails.recebidoPor = SubcontratoBseDT.Rows[0]["recebidoPor"].ToString();
            mySubcontratoBseDetails.observacoes = SubcontratoBseDT.Rows[0]["observacoes"].ToString();
            mySubcontratoBseDetails.bCertNomeCliente = GERAL.clsGeral.ConvertBStringToBoolean(SubcontratoBseDT.Rows[0]["bCertNomeCliente"].ToString());
        }
                

   


        // Função que devolve uma lista de BSE's de Subcontratos com base nos critérios de pesquisa
        public DataTable FillListaSubBSE(string empresa, string refBRE, string refSubcontratoBSE)
        {
            SqlParameter[] arrParams = new SqlParameter[3];
            
            //			arrParams[0] = new SqlParameter("@inSortField", strSortField);
            //			arrParams[1] = new SqlParameter("@inSortDirection", strSortDirection);
            arrParams[0] = new SqlParameter("@inEmpresa", empresa);
            arrParams[1] = new SqlParameter("@inRefSubcontratoBSE", refSubcontratoBSE);
            arrParams[2] = new SqlParameter("@inRefBRE", refBRE);
         
            DataTable SubBSEsDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListSubBSEs", arrParams); 
           
            return SubBSEsDT;
        }
		// Função que insere um BSE (subcontrato) e as linhas correspondentes dentro de uma única transacção e devolve o ID com que o BSE (subcontrato) ficou registado
		public int InsertSubBSEWithLinhas(string idEmpresaSubcontratada, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, DataTable dtLinhasSubBSE, string bCertNomeCliente)
		{
			int retValue;
			//ServicoBD servico = new ServicoBD();

			string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
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
						int idSubcontratoBSE = InsertSubcontratoBse(objConn, objCmd, idEmpresaSubcontratada, idFuncionarioSaida, recebidoPor, observacoes, username, bCertNomeCliente); 
                        
						int rows = dtLinhasSubBSE.Rows.Count; 
                        
						for(int i = 0; i<rows; i++)
						{                
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							string idServico = dtLinhasSubBSE.Rows[i]["idServico"].ToString();
							string idEstadoServico = dtLinhasSubBSE.Rows[i]["idEstadoServico"].ToString();
							string servicoObservacoes = dtLinhasSubBSE.Rows[i]["observacoes"].ToString();

							retValue = UpdateServicoInSubcontratoBSE(objConn, objCmd, idServico, idSubcontratoBSE.ToString(), idEstadoServico, servicoObservacoes, username); 

							// Se houver erro devolvemos o ID do Subcontrato BSE a zero
							if (retValue != 0)
								retValue = 0;
						}

						objTrans.Commit(); 
						// Se não houver erro devolvemos o ID do Subcontrato BSE
						retValue = idSubcontratoBSE;
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
							// Se houver erro devolvemos o ID do Subcontrato BSE a zero
							retValue = 0;
						}

						GERAL.clsWriteError.WriteLog(ex); 
						// Se houver erro devolvemos o ID do Subcontrato BSE a zero
						retValue = 0;
					}
				}
			}
			return retValue; 
		}


		// ==========================================================================================
		// Funcao que actualiza um Serviço (no Subcontrato BSE) e devolve a msg de erro
		// ==========================================================================================
		public int UpdateServicoInSubcontratoBSE(SqlConnection myConnection, SqlCommand myCommand, string idServico, string idSubcontratoBSE, string idEstadoServico, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdSubcontratoBSE", idSubcontratoBSE);
			arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
			arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[4] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpUpdServicoInSubcontratoBSE", arrParams); 
		}
		

		// Funcao que insere um BSE de um Subcontrato e devolve o ID com que o mesmo ficou registado
		public int InsertSubcontratoBse(SqlConnection myConnection, SqlCommand myCommand, string idEmpresaSubcontratada, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, string bCertNomeCliente)
		{
			SqlParameter[] arrParams = new SqlParameter[6];

			arrParams[0] = new SqlParameter("@inIdEmpresaSubcontratada", idEmpresaSubcontratada);
			arrParams[1] = new SqlParameter("@inIdFuncionarioSaida", idFuncionarioSaida);
			arrParams[2] = new SqlParameter("@inRecebidoPor", recebidoPor);
			arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[4] = new SqlParameter("@inUsername", username);
            arrParams[5] = new SqlParameter("@bCertNomeCliente", bCertNomeCliente);

			return GERAL.clsDataAccess.ExecuteNonQuerySPOutput(myConnection,myCommand,"stpInsSubcontratoBSE", arrParams);
		}

		// Funcao que actualiza um BSE de um Subcontrato e devolve a msg de erro
		public int UpdateSubcontratoBse(string idSubcontratoBSE, string idEmpresaSubcontratada, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, string bCertNomeCliente)
		{
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inIdSubcontratoBSE", idSubcontratoBSE);
			arrParams[1] = new SqlParameter("@inIdEmpresaSubcontratada", idEmpresaSubcontratada);
			arrParams[2] = new SqlParameter("@inIdFuncionarioSaida", idFuncionarioSaida);
			arrParams[3] = new SqlParameter("@inRecebidoPor", recebidoPor);
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", username);
            arrParams[6] = new SqlParameter("@bCertNomeCliente", bCertNomeCliente);
			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdSubcontratoBSE", arrParams); 
            return retValue; 

//            if (retValue == 0)
//                return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
//            else
//                return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}

        //Funcao que actualiza um SubContrato BRE dentro de uma transaccao
        public void UpdateSubBSEConn(SqlConnection myConnection,SqlCommand myCommand,string idSubcontratoBSE, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, string bCertNomeCliente)
        {
            SqlParameter[] arrParams = new SqlParameter[6];

            arrParams[0] = new SqlParameter("@inIdSubcontratoBSE", idSubcontratoBSE);
            //arrParams[1] = new SqlParameter("@inIdEmpresaSubcontratada", idEmpresaSubcontratada);
            arrParams[1] = new SqlParameter("@inIdFuncionarioSaida", idFuncionarioSaida);
            arrParams[2] = new SqlParameter("@inRecebidoPor", recebidoPor);
            arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
            arrParams[4] = new SqlParameter("@inUsername", username);
            arrParams[5] = new SqlParameter("@bCertNomeCliente", bCertNomeCliente);

            //mudar nome da sp chamada
            //remover a empresa dos parametros e da sp
            GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection, myCommand,"stpUpdSubcontratoBSE", arrParams); 

        }

        //transaccao, altera um SUB-BRE com serviços associados, retorna um valor conforme correu bem ou mal
        //1=correu bem - 0=correu mal
        public int UpdateSubBSEWithLinhas(string idBseSub, string idFuncionarioSaida, string recebidoPor, string observacoes, string username, DataTable DTOrigem, DataTable DTDestino, string bCertNomeCliente)

        {
            int retValue; 

            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
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
                        //UPDATE
                        UpdateSubBSEConn(objConn,objCmd, idBseSub,  idFuncionarioSaida,  recebidoPor,  observacoes,  username, bCertNomeCliente); 
                        //


                        //DTOrigem = linhas que nao sao associadas ao BSE
                        //fazer update do idBreSub para ""

                        //os outros nao estao associados, 
                        //mas so nos added tem de se pôr o idBreSub a null
                        
                        DataRow[] dRows = DTOrigem.Select(null,null,DataViewRowState.Added); 
                                    
                        foreach(DataRow dRow in dRows)
                        {
                        
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            string idServico= dRow["idServico"].ToString(); 
                            string idEstadoServico= dRow["idEstadoServico"].ToString(); 
                            string obs= dRow["Observacoes"].ToString();  
                            
                            SqlParameter[] arrParams = new SqlParameter[5];

                            arrParams[0] = new SqlParameter("@inIdServico", idServico);
                            arrParams[1] = new SqlParameter("@inIdSubcontratoBSE", "");
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);

                            objCmd.CommandText = "stpUpdServicoInSubcontratoBSE"; 
                            
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
                        //fazer update do idBreSub para idBreSub
                        DataRow[] dRws = DTDestino.Select(null,null,DataViewRowState.CurrentRows); 
                                    
                        foreach(DataRow dRw in dRws)
                        {
                                     
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            string idServico= dRw["idServico"].ToString(); 
                            string idEstadoServico= dRw["idEstadoServico"].ToString(); 
                            string obs= dRw["Observacoes"].ToString();  
               
                            
                            SqlParameter[] arrParams = new SqlParameter[5];

                            arrParams[0] = new SqlParameter("@inIdServico", idServico);
                            arrParams[1] = new SqlParameter("@inIdSubcontratoBSE", idBseSub);
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);

                            objCmd.CommandText = "stpUpdServicoInSubcontratoBSE"; 
                            
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
		public DataSet DSBSE(string idBSESub)
		{
			LabMetro.DATASETS.DSBSE ds = new LabMetro.DATASETS.DSBSE(); //usa o mesmo dataset que o bse

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.Connection = objConn; 
				
				SqlDataAdapter da1 = new SqlDataAdapter(objCmd);
			
				//objCmd.CommandText= "SELECT dbo.SubcontratoBSE.idSubcontratoBSE, dbo.udfGetReferenciaSubcontratoBSE(dbo.SubcontratoBSE.idSubcontratoBSE) AS refBSE, dbo.SubcontratoBSE.dtSubcontratoBSE, dbo.Empresa.nome AS empresa, dbo.Empresa.morada, dbo.Empresa.codigoPostal, dbo.empresa.localidade, dbo.Funcionario.nome AS entreguePor, dbo.SubcontratoBSE.recebidoPor, dbo.SubcontratoBSE.observacoes FROM dbo.SubcontratoBSE INNER JOIN dbo.Empresa ON dbo.SubcontratoBSE.idSubcontratoBSE = " + idBSESub + "  AND  dbo.SubcontratoBSE.idEmpresaSubcontratada = dbo.Empresa.idEmpresa  INNER JOIN dbo.Funcionario ON dbo.SubcontratoBSE.idFuncionarioSaida = dbo.Funcionario.idFuncionario"; 


                objCmd.CommandText = "     SELECT distinct dbo.SubcontratoBSE.idSubcontratoBSE, dbo.udfGetReferenciaSubcontratoBSE(dbo.SubcontratoBSE.idSubcontratoBSE) AS refBSE, dbo.SubcontratoBSE.dtSubcontratoBSE, e.nome AS empresa, e.morada, e.codigoPostal, e.localidade,  dbo.Funcionario.nome AS entreguePor, dbo.SubcontratoBSE.recebidoPor, dbo.SubcontratoBSE.observacoes  , empbre.nome as nomeClienteCertificado, dbo.SubcontratoBSE.bCertNomeCliente, empBRE.morada as moradaCliente, empBRE.codigoPostal as codigoPostalCliente , empBRE.localidade as localidadeCliente FROM dbo.SubcontratoBSE  INNER JOIN dbo.Empresa E ON dbo.SubcontratoBSE.idSubcontratoBSE = " + idBSESub + "  AND   dbo.SubcontratoBSE.idEmpresaSubcontratada = E.idEmpresa  INNER JOIN dbo.Funcionario ON dbo.SubcontratoBSE.idFuncionarioSaida = dbo.Funcionario.idFuncionario INNER JOIN dbo.Servico S  ON dbo.SubcontratoBSE.idSubcontratoBSE = S.idSubcontratoBSE INNER JOIN dbo.BRE  ON S.idBRE = dbo.BRE.idBRE INNER JOIN dbo.Empresa EmpBre   ON dbo.BRE.idEmpresa = EmpBre.idEmpresa";

				try
				{
					da1.Fill(ds,"dtSubcontratoBSE"); 
				}
				catch(Exception)

				{	
				}

				objCmd.CommandText =  "SELECT dbo.Servico.idServico, dbo.Servico.idBRE, dbo.TipoEquipamento.descricao AS tipoEquipamento, dbo.Equipamento.numIdentificacao, dbo.Requisicao.referenciaCliente, dbo.Requisicao.dtRequisicao, dbo.Servico.observacoes, dbo.TipoServico.descricao AS tipoServico, dbo.Servico.refServico, CASE WHEN CAST(dbo.Servico.idEstadoServico AS VARCHAR(10)) = '15' THEN 'Não' WHEN CAST(dbo.Servico.idEstadoServico AS VARCHAR(10)) = '14' THEN 'Sim' END AS comCertificado, dbo.Servico.idBSE, dbo.Servico.idSubcontratoBRE, dbo.Servico.idSubcontratoBSE, dbo.Servico.idEstadoServico FROM dbo.Servico INNER JOIN dbo.Equipamento ON dbo.Servico.idEquipamento = dbo.Equipamento.idEquipamento INNER JOIN dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.TipoServico ON dbo.Servico.idTipoServico = dbo.TipoServico.idTipoServico LEFT OUTER JOIN  dbo.Requisicao ON dbo.Servico.idRequisicao = dbo.Requisicao.idRequisicao WHERE dbo.Servico.idEstadoServico <> (-1) AND dbo.Servico.idSubcontratoBse = " + idBSESub + " ORDER BY dbo.Servico.idServico"; 
				//quero q os anulados não apareçam.<>7
				 
				//objCmd.Connection = objConn; 

				try
				{
					da1.Fill(ds,"dtServico");
				}
				catch
				{	
				}
				
				da1.Dispose();
				da1 = null; 
				
			}
			return ds; 
		}
	}
}
