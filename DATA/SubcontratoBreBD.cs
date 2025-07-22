using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for SubcontratoBreBD.
	/// </summary>
	/// 

	// Classe que guarda nos seus atributos os detalhes de um BRE de um Subcontrato
	public class SubcontratoBreDetails
	{
		public string idBRE;
		public string refBRE;
		public string idEmpresa;
		public string nomeEmpresa;
		public string idSubcontratoBRE;
		public string idEmpresaSubcontratada;
		public string nomeEmpresaSubcont;
		public string idFuncionarioRecepcao;
		public string nomeFuncionarioRecepcao;
		public string numSubcontratoBRE;
		public string ano;
		public string refSubcontratoBRE;
		public string dtSubcontratoBRE;
		public string entreguePor;
		public string observacoes;
	}

	public class SubcontratoBreBD
	{
		public SubcontratoBreBD()
		{
		}

		//===========================================================================
        // DEVOLVE OS DETALHES DE UM BRE DE SUBCONTRATAÇÃO
		//===========================================================================
        public SubcontratoBreDetails GetSubcontratoBreDetails(string idSubcontratoBRE)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
            arrParams[0] = new SqlParameter("@inIdSubcontratoBRE", idSubcontratoBRE);
         
            DataTable SubcontratoBreDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetSubcontratoBreById", arrParams); 
           
            if(SubcontratoBreDT.Rows.Count > 0) 
            { 
                SubcontratoBreDetails mySubcontratoBreDetails= new SubcontratoBreDetails();
				mySubcontratoBreDetails.idBRE = SubcontratoBreDT.Rows[0]["idBRE"].ToString();
				mySubcontratoBreDetails.refBRE = SubcontratoBreDT.Rows[0]["refBRE"].ToString();
				mySubcontratoBreDetails.idEmpresa = SubcontratoBreDT.Rows[0]["idEmpresa"].ToString();
				mySubcontratoBreDetails.nomeEmpresa = SubcontratoBreDT.Rows[0]["empresa"].ToString();
				mySubcontratoBreDetails.idSubcontratoBRE = SubcontratoBreDT.Rows[0]["idSubcontratoBRE"].ToString();
				mySubcontratoBreDetails.idEmpresaSubcontratada = SubcontratoBreDT.Rows[0]["idEmpresaSubcontratada"].ToString();
				mySubcontratoBreDetails.nomeEmpresaSubcont = SubcontratoBreDT.Rows[0]["nomeEmpresaSubcont"].ToString();
				mySubcontratoBreDetails.idFuncionarioRecepcao = SubcontratoBreDT.Rows[0]["idFuncionarioRecepcao"].ToString();
				mySubcontratoBreDetails.nomeFuncionarioRecepcao = SubcontratoBreDT.Rows[0]["nomeFuncionarioRecepcao"].ToString();
				mySubcontratoBreDetails.numSubcontratoBRE = SubcontratoBreDT.Rows[0]["numSubcontratoBRE"].ToString();
				mySubcontratoBreDetails.ano = SubcontratoBreDT.Rows[0]["ano"].ToString();
				mySubcontratoBreDetails.refSubcontratoBRE = SubcontratoBreDT.Rows[0]["refSubcontratoBRE"].ToString();
				mySubcontratoBreDetails.dtSubcontratoBRE = SubcontratoBreDT.Rows[0]["dtSubcontratoBRE"].ToString();
				mySubcontratoBreDetails.entreguePor = SubcontratoBreDT.Rows[0]["entreguePor"].ToString();
				mySubcontratoBreDetails.observacoes = SubcontratoBreDT.Rows[0]["observacoes"].ToString();
                return mySubcontratoBreDetails;
            }
            else
            {
                return null; 
            }
        }

		//=================================================================================================
        // DATATABLE - LISTA de BREs de SUBCONTRATO com base nos critérios de pesquisa
		//=================================================================================================
        public DataTable FillListaSubBRE(string empresa, string refBRE, string refSubcontratoBRE)
        {
            SqlParameter[] arrParams = new SqlParameter[3];
            
            arrParams[0] = new SqlParameter("@inEmpresa", empresa);
            arrParams[1] = new SqlParameter("@inRefSubcontratoBRE", refSubcontratoBRE);
            arrParams[2] = new SqlParameter("@inRefBRE", refBRE);
         
            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListSubBREs", arrParams); 
        }

		//=================================================================================================
		// Função que insere um BRE (subcontrato) e as linhas correspondentes dentro de uma única transacção 
		// devolve o ID com que o BRE (subcontrato) ficou registado
		//=================================================================================================
		public int InsertSubBREWithLinhas(string idEmpresaSubcontratada, string idFuncionarioRecepcao, string entreguePor, string observacoes, string username, DataTable dtLinhasSubBRE)
		{
			int retValue;
			
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
						int idSubcontratoBRE = InsertSubcontratoBre(objConn, objCmd, idEmpresaSubcontratada, idFuncionarioRecepcao, entreguePor, observacoes, username); 
                        
						int rows = dtLinhasSubBRE.Rows.Count; 
                        
						for(int i = 0; i<rows; i++)
						{                
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							string idServico = dtLinhasSubBRE.Rows[i]["idServico"].ToString();
							string idEstadoServico = dtLinhasSubBRE.Rows[i]["idEstadoServico"].ToString();
							string servicoObservacoes = dtLinhasSubBRE.Rows[i]["observacoes"].ToString();

							retValue = UpdateServicoInSubcontratoBRE(objConn, objCmd, idServico, idSubcontratoBRE.ToString(), idEstadoServico, servicoObservacoes, username); 

							// Se houver erro devolvemos o ID do Subcontrato BRE a zero
							if (retValue != 0) retValue = 0;
						}

						objTrans.Commit(); 
						return idSubcontratoBRE;
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
							// Se houver erro devolvemos o ID do Subcontrato BRE a zero
							return 0; 
						}

						GERAL.clsWriteError.WriteLog(ex); 
						// Se houver erro devolvemos o ID do Subcontrato BRE a zero
						return 0; 
					}
				}
			}
			//return retValue; 
		}

		//=================================================================================================
		// Funcao que insere um BRE de um Subcontrato e devolve o ID com que o mesmo ficou registado
		//=================================================================================================
		public int InsertSubcontratoBre(SqlConnection myConnection, SqlCommand myCommand, string idEmpresaSubcontratada, string idFuncionarioRecepcao, string entreguePor, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdEmpresaSubcontratada", idEmpresaSubcontratada);
			arrParams[1] = new SqlParameter("@inIdFuncionarioRecepcao", idFuncionarioRecepcao);
			arrParams[2] = new SqlParameter("@inEntreguePor", entreguePor);
			arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[4] = new SqlParameter("@inUsername", username);
   
			return GERAL.clsDataAccess.ExecuteNonQuerySPOutput(myConnection,myCommand,"stpInsSubcontratoBRE", arrParams);
		}

		//=================================================================================================
		//TIREI DO SERVIÇO E PUS AQUI - DM JULHO 2006	
		//FUNÇÃO QUE ACTUALIZA UM SERVIÇO COM A INFORMAÇÃO DO BRE DE SUBCONTRATAÇÃO
		//=================================================================================================
		public int UpdateServicoInSubcontratoBRE(SqlConnection myConnection, SqlCommand myCommand, string idServico, string idSubcontratoBRE, string idEstadoServico, string observacoes, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdSubcontratoBRE", idSubcontratoBRE);
			arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
			arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[4] = new SqlParameter("@inUsername", username);

			return GERAL.clsDataAccess.SPExecuteNonQueryRV(myConnection, myCommand, "stpUpdServicoInSubcontratoBRE", arrParams); 
		}


		//=================================================================================================
        //Funcao que actualiza um SubContrato BRE dentro de uma transaccao
		//=================================================================================================
        public void UpdateSubBREConn(SqlConnection myConnection,SqlCommand myCommand,string idSubcontratoBRE, string idFuncionarioRecepcao, string entreguePor, string observacoes, string username)
        {
            SqlParameter[] arrParams = new SqlParameter[5];

            arrParams[0] = new SqlParameter("@inIdSubcontratoBRE", idSubcontratoBRE);
            
            arrParams[1] = new SqlParameter("@inIdFuncionarioRecepcao", idFuncionarioRecepcao);
            arrParams[2] = new SqlParameter("@inEntreguePor", entreguePor);
            arrParams[3] = new SqlParameter("@inObservacoes", observacoes);
            arrParams[4] = new SqlParameter("@inUsername", username);


            GERAL.clsDataAccess.ExecuteNonQuerySP(myConnection, myCommand,"stpUpdSubcontratoBRE", arrParams); 

        }

		//=================================================================================================
        //transaccao, altera um SUB-BRE com serviços associados, retorna um valor conforme correu bem ou mal
        //1=correu bem - 0=correu mal
		//=================================================================================================
        public int UpdateSubBREWithLinhas(string idBreSub, string idFuncionarioRecepcao, string entreguePor, string observacoes, string username, DataTable DTOrigem, DataTable DTDestino)

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

                        UpdateSubBREConn(objConn,objCmd, idBreSub,  idFuncionarioRecepcao,  entreguePor,  observacoes,  username); 

                        //DTOrigem = linhas que nao sao associadas ao BSE, fazer update do idBreSub para ""

                        //os outros nao estao associados,mas so nos added tem de se pôr o idBreSub a null
                        
                        DataRow[] dRows = DTOrigem.Select(null,null,DataViewRowState.Added); 
                                    
                        foreach(DataRow dRow in dRows)
                        {
                        
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            string idServico= dRow["idServico"].ToString(); 
                            string idEstadoServico= dRow["idEstadoServico"].ToString(); 
                            string obs= dRow["Observacoes"].ToString();  
                            
                            SqlParameter[] arrParams = new SqlParameter[5];

                            arrParams[0] = new SqlParameter("@inIdServico", idServico);
                            arrParams[1] = new SqlParameter("@inIdSubcontratoBRE", "");
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);

                            objCmd.CommandText = "stpUpdServicoInSubcontratoBRE"; 
                            
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

                        //DTDestino = linhas  associadas ao BSE, fazer update do idBreSub para idBreSub
                        DataRow[] dRws = DTDestino.Select(null,null,DataViewRowState.CurrentRows); 
                                    
                        foreach(DataRow dRw in dRws)
                        {
                                     
                            if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 
                            string idServico= dRw["idServico"].ToString(); 
                            string idEstadoServico= dRw["idEstadoServico"].ToString(); 
                            string obs= dRw["Observacoes"].ToString();  
               
                            
                            SqlParameter[] arrParams = new SqlParameter[5];

                            arrParams[0] = new SqlParameter("@inIdServico", idServico);
                            arrParams[1] = new SqlParameter("@inIdSubcontratoBRE", idBreSub);
                            arrParams[2] = new SqlParameter("@inIdEstadoServico", idEstadoServico);
                            arrParams[3] = new SqlParameter("@inObservacoes", obs);
                            arrParams[4] = new SqlParameter("@inUsername", username);

                            objCmd.CommandText = "stpUpdServicoInSubcontratoBRE"; 
                            
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
		public DataSet DSBRESub(string idBRESub) 
		{
			LabMetro.DATASETS.DSBRE ds = new LabMetro.DATASETS.DSBRE(); //uso o mesmo dataset que no bre
		
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
				objCmd.Connection = objConn; 
				

				SqlDataAdapter DA = new SqlDataAdapter(objCmd);
			
				objCmd.CommandText= "SELECT dbo.SubcontratoBRE.idSubcontratoBRE, dbo.udfGetReferenciaSubcontratoBRE(dbo.SubcontratoBRE.idSubcontratoBRE) AS refBRE,  dbo.SubcontratoBRE.dtSubcontratoBRE, dbo.Empresa.nome AS empresa, dbo.Empresa.morada, dbo.Empresa.codigoPostal, dbo.empresa.localidade, dbo.Funcionario.nome AS recebidoPor, dbo.SubcontratoBRE.entreguePor,  dbo.SubcontratoBRE.observacoes FROM    dbo.SubcontratoBRE INNER JOIN dbo.Empresa ON dbo.SubcontratoBRE.idSubcontratoBRE = " + idBRESub + " and dbo.SubcontratoBRE.idEmpresaSubcontratada = dbo.Empresa.idEmpresa INNER JOIN dbo.Funcionario ON dbo.SubcontratoBRE.idFuncionarioRecepcao = dbo.Funcionario.idFuncionario"; 
				
				try
				{
					DA.Fill(ds,"dtSubcontratoBRE"); 
				}
				catch
				{	
				}

				objCmd.CommandText =  "SELECT dbo.Servico.idServico, dbo.Servico.idBRE, dbo.TipoEquipamento.descricao AS tipoEquipamento, dbo.Equipamento.numIdentificacao, dbo.Requisicao.referenciaCliente, dbo.Requisicao.dtRequisicao, dbo.Servico.observacoes, dbo.TipoServico.descricao AS tipoServico, dbo.Servico.refServico, CASE WHEN CAST(dbo.Servico.idEstadoServico AS VARCHAR(10)) = '15' THEN 'Não' WHEN CAST(dbo.Servico.idEstadoServico AS VARCHAR(10)) = '14' THEN 'Sim' END AS comCertificado, dbo.Servico.idBSE, dbo.Servico.idSubcontratoBRE, dbo.Servico.idSubcontratoBSE, dbo.Servico.idEstadoServico FROM dbo.Servico INNER JOIN          dbo.Equipamento ON dbo.Servico.idEquipamento = dbo.Equipamento.idEquipamento INNER JOIN dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.TipoServico ON dbo.Servico.idTipoServico = dbo.TipoServico.idTipoServico LEFT OUTER JOIN  dbo.Requisicao ON dbo.Servico.idRequisicao = dbo.Requisicao.idRequisicao WHERE dbo.Servico.idSubcontratoBRE IS NOT NULL AND dbo.Servico.idEstadoServico <>(-1) AND dboServico.idSubcontratoBRE = " + idBRESub + " ORDER BY dbo.Servico.idServico"; 
				 //quero q os anulados não apareçam.<>7 nem os anulados definitivos -1
				 
				//objCmd.Connection = objConn; 

				try
				{
					DA.Fill(ds,"dtServico");
				}
				catch
				{	
				}
				
				DA.Dispose();
				DA = null; 
				
			}
			return ds; 
		}
	}
}
