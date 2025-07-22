using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web; 
namespace LabMetro.DATA
{

	/// <summary>
	/// Summary description for FacturaData.
	/// </summary>
		// Classe que guarda nos seus atributos os detalhes de uma Factura
	 	


	// Classe que guarda nos seus atributos os detalhes de uma Factura
	//acho q ja nao guarda....

	public class FacturaDetails
	{
		public string idFactura;
		public string idBRE;
		public string refBRE;
		public string idEmpresa;
		public string nomeEmpresa;
		public string numFactura;
		public string ano;
		public string refFactura;
		public string dtFactura;
		public string valorDespesasEnvio;
		public string valorAjudasCustoDeslocacoes;
		public string valorTotalFactura;
		public string observacoes;
		public string obsEmpresa; 
		//public DateTime dtAlteracao;  //DateTime tostring tb tira os milliseconds -- não funciona
		public string idUtilAlteracao;
        public string numVD;
		
	}

	public class FacturaData
	{
		public FacturaData()
		{
		}



		//MUITO IMPORTANTE -- IR VER O ORIGINAL QUE ENTRETANTO MUDOU! ---------------------------
		//COPIAR AS FUNÇÕES IGUAIS DO ORIGINAL


		//****************************************************************************************
		//DETALHAS DA FACTURA (PARA O FILLFORM)===================================================
		//****************************************************************************************
	
		//========================================================================================	
		//DEVOLVE DADOS FACTURA PELO SEU ID
		//========================================================================================	
		public DataTable DTFacturaDetails(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdFactura", idFactura);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetFacturaById", arrParams); 

		}

		//****************************************************************************************
		//FICHEIRO FACTURAÇÃO ====================================================================
		//****************************************************************************************

		//========================================================================================
		//DATAREADER com os dados necessarios para criar o ficheiro de facturacao
		//necessitanto ainda das strings strDeslocacoes, e strDataFimServico, abaixo
		//========================================================================================
		public SqlDataReader drFicheiroFactura(string idFactura,string orderBy)
		{
			string strSQL =  "SELECT TE.descricao, ISNULL(E.numIdentificacao, '') AS numIdentificacao, ISNULL(E.numSerie, '') AS numSerie, S.refServico, CAST(ISNULL(S.valorFinal, 0) AS varchar) AS valorFinal FROM Servico S INNER JOIN Equipamento E ON S.idEquipamento = E.idEquipamento INNER JOIN TipoEquipamento TE ON E.idTipoEquipamento = TE.idTipoEquipamento WHERE S.idFactura = "+idFactura+" ORDER BY " +orderBy; 

			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}

		//========================================================================================
		//STRING- com o valor das deslocacoes por factura, ou string vazia
		//========================================================================================
		public string strDeslocacoes(string idFactura)
		{
			string strSQL = "SELECT CAST(ISNULL(valorAjudasCustoDeslocacoes, 0) AS varchar) AS valorAjudasCustoDeslocacoes FROM Factura WHERE idFactura ="+idFactura; 
            
			object valDesloc = GERAL.clsDataAccess.myExecuteScalar(strSQL); 
            
			if(!Convert.IsDBNull(valDesloc))    
			{
				return valDesloc.ToString().Replace(".",""); 
			}    
			else
			{
				return ""; 
			}
            
		}

		//========================================================================================
		//STRING- com o valor das Despesas de Envio e Transporte por factura, ou string vazia
		//========================================================================================
		public string strDespesasEnvio(string idFactura)
		{
			string strSQL = "SELECT CAST(ISNULL(valorDespesasEnvio, 0) AS varchar) AS valorDespesasEnvio FROM Factura WHERE idFactura ="+idFactura; 
            
			object valDesloc = GERAL.clsDataAccess.myExecuteScalar(strSQL); 
            
			if(!Convert.IsDBNull(valDesloc))    
			{
				return valDesloc.ToString().Replace(".",""); 
			}    
			else
			{
				return ""; 
			}
            
		}

		//========================================================================================
		//STRING- data de fim para o ficheiro de facturacao
		//========================================================================================
		public string strDataFimServico(string idFactura)
		{
			string strSQL = "SELECT ISNULL(MAX(dtBSE), MAX(dtFactura)) FROM Servico S LEFT OUTER JOIN BSE  ON S.idBSE = BSE.idBSE INNER JOIN Factura F ON S.idFactura = F.idFactura WHERE S.idFactura = "+idFactura; 

			object dataFim = GERAL.clsDataAccess.myExecuteScalar(strSQL); 
            
			if(!Convert.IsDBNull(dataFim))    
			{
				try
				{
					//para forçar o formato certo                   
					return DateTime.Parse(dataFim.ToString()).ToString("dd'/'MM'/'yyyy"); 
				}
				catch
				{
					return "??/??/????"; //.... nao devia fazer isto, mas....
				}
			}
			else
			{
				return "??/??/????"; 
			}
		}

		//========================================================================================	
		//vai buscar a percentagem de desconto associada à Empresa,
		//devolve 0 se nao existir
		//========================================================================================	
		public string strPercentagemDesconto(string idEmpresa)
		{
			string strSQL = "SELECT percDesconto FROM Empresa WHERE idEmpresa = "+ idEmpresa; 
			object o = GERAL.clsDataAccess.myExecuteScalar(strSQL); 
			if(!Convert.IsDBNull(o))
			{
				return o.ToString(); 
			}
			else
			{
				return "0"; 
			}
		}

		
		//****************************************************************************************
		//LISTAS ETC. ====================================================================
		//****************************************************************************************

		//========================================================================================
		// DATATABLE- Empresas que teem equipamentos que podem ser facturados.
		//========================================================================================
		public DataTable DTListaEmpresasForFactura(string cbBRE, string strEmpresa, string strNIF, string numClienteSAP)
		{
			SqlParameter[] arrParams = new SqlParameter[4];
            
			arrParams[0] = new SqlParameter("@inSoCompletos", GERAL.clsGeral.ConvertStringToBool(cbBRE));
			arrParams[1] = new SqlParameter("@inNome", strEmpresa);
			arrParams[2] = new SqlParameter("@inNif", strNIF);
			arrParams[3] = new SqlParameter("@inNumClienteSAP", numClienteSAP);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForFactura",arrParams); 	
		}

        //========================================================================================
        // DATATABLE- Empresas que teem equipamentos que podem ser facturados.
        //========================================================================================
        public DataTable DTListaEmpresasForVD(string strEmpresa, string strNIF, string numClienteSAP)
        {
            SqlParameter[] arrParams = new SqlParameter[3];

           
            arrParams[0] = new SqlParameter("@inNome", strEmpresa);
            arrParams[1] = new SqlParameter("@inNif", strNIF);
            arrParams[2] = new SqlParameter("@inNumClienteSAP", numClienteSAP);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForVD", arrParams);
        }
        

		//========================================================================================
		// DATATABLE de Facturas com base nos critérios de pesquisa
		//========================================================================================	
		public DataTable DTFacturas(string empresa, string refBRE, string refFactura, string tipoEquipamento, string ano, string nomeFuncionario, string refServico)
		{
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inEmpresa", empresa);
			arrParams[1] = new SqlParameter("@inRefFactura", refFactura);
			arrParams[2] = new SqlParameter("@inRefBRE", refBRE);
			arrParams[3] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
			arrParams[4] = new SqlParameter("@inAno", ano);
			arrParams[5] = new SqlParameter("@inNomeFuncionario", nomeFuncionario);
            arrParams[6] = new SqlParameter("@inRefServico", refServico);
			
			
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListFacturas", arrParams); 
		}

		

		//========================================================================================	
		// DATATABLE COM SO SERVIÇOS DE UM BRE QUE JÁ PODEM SER FACTURADOS
		//========================================================================================	
		public DataTable DTGetServicosForFactura(string idBRE)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoForFacturaByIdBRE", arrParams); 
		}

        //========================================================================================	
        // DATATABLE COM SO SERVIÇOS DE UM BRE QUE JÁ PODEM SER FACTURADOS
        //========================================================================================	
        public DataTable DTGetServicosForVD(string idBRE)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdBRE", idBRE);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoForVDByIdBRE", arrParams);
        }

		

		//========================================================================================	
		// DATATABLE COM TODOS OS SERVIÇOS DE UMA FACTURA - PARA O FILLFORM
		//========================================================================================	
		public DataTable DTGetServicoByFactura(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdFactura", idFactura);
         
			DataTable ServicoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicoByFactura", arrParams); 
           
			return ServicoDT;
		}

		//========================================================================================	
		// DATAREADER - que devolve todos os serviços de uma Factura
		//========================================================================================	
		public SqlDataReader DRGetServicoByFactura(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdFactura", idFactura);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetServicoByFactura", arrParams); 
           
		}

		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSFactura(string idFactura, string orderBy, string myApp)
		{
			LabMetro.DSFactura ds = new DSFactura(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.Connection = objConn; 	
	
				SqlDataAdapter DA = new SqlDataAdapter(objCmd);
                string str = "";
                if (myApp == "ANG_LABMETRO")
                {
                     str = "SELECT DISTINCT dbo.udfGetReferenciaBRE(dbo.udfGetIdBREByIdFactura (dbo.Factura.idFactura)) AS refBRE, dbo.udfGetReferenciaFactura(dbo.Factura.idFactura) AS refFactura,  dbo.Factura.dtFactura, dbo.Factura.idFactura,  dbo.Empresa.nome AS empresa, dbo.Empresa.numClienteSAP, dbo.Factura.observacoes,  dbo.Factura.valorDespesasEnvio,  dbo.Factura.valorAjudasCustoDeslocacoes, (cast(factura.idFactura as varchar)+'_'+CAST(factura.intVersaoFicheiro as varchar)+'.txt')   as nomeFicheiro, bre.referenciaRequisicao, orcamento.refOrcamento, empresa.numObra, factura.refBSE, ISNULL(Empresa.morada, '')   + ' ' +   ISNULL(Empresa.codigoPostal, '') + ' ' + ISNULL(Empresa.localidade, '') AS morada   from  dbo.Factura   left join bre on factura.idbre = bre.idbre   inner join empresa on isnull(bre.idEmpresaContratante, bre.idEmpresa) = empresa.idEMpresa   left join orcamento on bre.idOrcamento = orcamento.idOrcamento  WHERE idFactura = " + idFactura;
                }
                else
                {
                     str = "SELECT DISTINCT dbo.udfGetReferenciaBRE(dbo.udfGetIdBREByIdFactura (dbo.Factura.idFactura)) AS refBRE, dbo.udfGetReferenciaFactura(dbo.Factura.idFactura) AS refFactura,  dbo.Factura.dtFactura, dbo.Factura.idFactura, dbo.Empresa.nome AS empresa, dbo.Empresa.numClienteSAP, dbo.Factura.observacoes,   dbo.Factura.valorDespesasEnvio, dbo.Factura.valorAjudasCustoDeslocacoes, (cast(factura.idFactura as varchar)+'_'+CAST(factura.intVersaoFicheiro as varchar)+'.txt')  as nomeFicheiro, bre.referenciaRequisicao, orcamento.refOrcamento, empresa.numObra, factura.refBSE, ISNULL(Empresa.morada, '')   + ' ' + ISNULL(Empresa.codigoPostal, '') + ' ' + ISNULL(Empresa.localidade, '') AS morada FROM  dbo.Empresa INNER JOIN  dbo.Factura ON dbo.Empresa.idEmpresa = dbo.Factura.idEmpresa left join bre on factura.idbre = bre.idbre left join orcamento on bre.idOrcamento = orcamento.idOrcamento WHERE idFactura = " + idFactura;
                }

				objCmd.CommandType = CommandType.Text; 
				objCmd.CommandText= str;
				
				try
				{
					DA.Fill(ds,"dtFactura"); 
				}
				catch
				{	
				}

				string s =  "SELECT  Servico.idFactura, TipoEquipamento.descricao AS tipoEquipamento, Servico.refServico, Servico.valor, Servico.percDesconto, Servico.valorFinal,  Equipamento.numIdentificacao,  Equipamento.numSerie,TipoEquipamento.codigo as codTipoEquipameno, Servico.idServico, Requisicao.referenciaCliente as refRequisicao FROM Servico INNER JOIN Equipamento ON Servico.idEquipamento = Equipamento.idEquipamento INNER JOIN TipoEquipamento ON Equipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento LEFT JOIN Requisicao on Servico.idRequisicao = Requisicao.idRequisicao	WHERE Servico.idFactura = " + idFactura + " ORDER BY "+ orderBy; 
				
				objCmd.CommandText = s;

				try
				{
					DA.Fill(ds,"dtFacturaLinha");
				}
				catch
				{	
				}

				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Parameters.AddWithValue("@inIdFactura",idFactura);
				objCmd.CommandText= "stpGetTotaisCentroCustoByIdFactura"; 
				
				try
				{
					DA.Fill(ds,"dtTotaisCentroCustoByIdFactura"); 
				}
				catch
				{	
				}

				DA.Dispose(); 
				DA = null; 
				//				da1.Dispose();
				//				da2.Dispose(); 
				//				da3.Dispose(); 
			}
			return ds; 
		}

		//****************************************************************************************
		//OPERAÇÕES DE BD ========================================================================
		//****************************************************************************************

		//==============================================================================================
		//INSERT FACTURA COM LINHAS EM TRANSACÇÃO 
		//DEVOLVE o ID DA FACTURA SE CORREU BEM OU ZERO SE CORREU MAL 
		//==============================================================================================
		public int InsertFacturaWithLinhas(string idBRE, string idEmpresa, string valorDespesasEnvio, string valorAjudasCustoDeslocacoes, string valorTotalFactura, string observacoes, string username, DataTable dtLinhasFactura, string idTipoDocumento, string chCanalDistribuicao, string idEscritorioVendas, string idCondicoesPagamento,string refBSE, string dataFactura, string refPedidoCliente, string dtPedidoCliente)//datafactura novo nov.2014
		{
			int idFactura = 0; //se houver erro, este campo é retornado a zero
			
			string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{

				objCmd.Connection = objConn;
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction = objTrans; 
					
					try
					{						
						idFactura = InsertFactura(objConn, objCmd, idBRE, idEmpresa, valorDespesasEnvio, valorAjudasCustoDeslocacoes, valorTotalFactura, observacoes, username,idTipoDocumento,chCanalDistribuicao, idEscritorioVendas, idCondicoesPagamento, refBSE,dataFactura, refPedidoCliente, dtPedidoCliente); //debug ok

						int rows = dtLinhasFactura.Rows.Count; 
                        
						for(int i = 0; i<rows; i++)
						{                
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear(); 

							string idServico = dtLinhasFactura.Rows[i]["idServico"].ToString();
							string valor = dtLinhasFactura.Rows[i]["valor"].ToString();
							string percDesconto = dtLinhasFactura.Rows[i]["percDesconto"].ToString();
							string valorFinal = dtLinhasFactura.Rows[i]["valorFinal"].ToString();

							//actualizado no trigger.
//							string idRegiaoVendas;	//se calib.externa vem da empresa
//													//se nao, vem do laboratorio
//							string idCodigoPEP;		//se factura venda de projecto, vem do laboratório
//							string idCodigoServico; //vem da grandeza
							
	
							int rowCount = InsValoresFacturaInServico(objConn, objCmd, idServico, idFactura.ToString(), valor, percDesconto, valorFinal, username);  //count das linhas afectadas na BD

							//apanhar erro
							if(rowCount<=0) 
							{
								//ir buscar os dados de quem facturou serviço entretanto
								string strSQL = "SELECT S.refServico,dbo.udfGetReferenciaFactura(F.idFactura) AS refFactura ,dbo.udfGetNomeUtilizador(F.idUtilAlteracao) as Utilizador FROM Servico S INNER JOIN FACTURA F ON S.idFactura = F.idFactura AND S.idServico = " + idServico; 
								objCmd.CommandType = CommandType.Text; 

								SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(objConn,objCmd,strSQL); 
								if(dr.HasRows)
								{
									while(dr.Read())
									{
										HttpContext.Current.Response.Write("<div class='errorMessage'>" + dr["refServico"] + " já facturado por " + dr["Utilizador"] + " - Factura Nº " + dr["refFactura"]+"</div>"); 
									}
								}


								dr.Close(); //cuidado, nao sei se o using nao fecha por fora.
								objTrans.Rollback(); 
								return 0; 
							}
							
							
							

						}
						objTrans.Commit(); 
						return idFactura; 
					}
					catch(SqlException ex)
					{ 	
						try
						{	
							GERAL.clsWriteError.WriteLog(ex.Message); 
							objTrans.Rollback();
							return 0; 
						}
						catch(Exception exep)
						{
							GERAL.clsWriteError.WriteLog(exep.Message); 
							return 0; 
						}	
					}
				}
			}
		}


		//========================================================================================	
		// INSERE DADOS FACTURA - DEVOLVE ID FACTURA
		//========================================================================================	
		// Funcao que insere uma Factura e devolve o ID com que a mesma ficou registada
		public int InsertFactura(SqlConnection myConnection, SqlCommand myCommand, string idBRE, string idEmpresa, string valorDespesasEnvio, string valorAjudasCustoDeslocacoes, string valorTotalFactura, string observacoes, string username,string idTipoDocumento, string chCanalDistribuicao, string idEscritorioVendas,string idCondicoesPagamento, string refBSE, string dataFactura, string refPedidoCliente, string dtPedidoCliente)//datafactura novo 2015 - so em espanha é que podem alterar a data
		{
			SqlParameter[] arrParams = new SqlParameter[15];

			arrParams[0] = new SqlParameter("@inIdBRE", idBRE);
			arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[2] = new SqlParameter("@inValorDespesasEnvio", GERAL.clsGeral.ConvertStringToDouble(valorDespesasEnvio));
			arrParams[3] = new SqlParameter("@inValorAjudasCustoDeslocacoes", GERAL.clsGeral.ConvertStringToDouble(valorAjudasCustoDeslocacoes));
			arrParams[4] = new SqlParameter("@inValorTotalFactura", GERAL.clsGeral.ConvertStringToDouble(valorTotalFactura));
			arrParams[5] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[6] = new SqlParameter("@inUsername", username);
			arrParams[7] = new SqlParameter("@inIdTipoDocumento", idTipoDocumento);
			arrParams[8] = new SqlParameter("@inCanalDistribuicao", chCanalDistribuicao);
			arrParams[9] = new SqlParameter("@inIdEscritorioVendas", idEscritorioVendas);
			arrParams[10] = new SqlParameter("@inIdCondicoesPagamento", idCondicoesPagamento);
            arrParams[11] = new SqlParameter("@refBSE", refBSE);
            arrParams[12] = new SqlParameter("@dataFactura", dataFactura);
            arrParams[13] = new SqlParameter("@refPedidoCliente", refPedidoCliente);
            arrParams[14] = new SqlParameter("@dtPedidoCliente", dtPedidoCliente);

            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput(myConnection,myCommand,"stpInsFactura", arrParams); 
		}




		public int InsValoresFacturaInServico(SqlConnection myConnection, SqlCommand myCommand, string idServico, string idFactura, string valor, string percDesconto, string valorFinal, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[6];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdFactura", idFactura);
			arrParams[2] = new SqlParameter("@inValor", GERAL.clsGeral.ConvertStringToDouble(valor));
			arrParams[3] = new SqlParameter("@inPercDesconto", GERAL.clsGeral.ConvertStringToDouble(percDesconto));
			arrParams[4] = new SqlParameter("@inValorFinal", GERAL.clsGeral.ConvertStringToDouble(valorFinal));
			arrParams[5] = new SqlParameter("@inUsername", username);
			
			return GERAL.clsDataAccess.intExecuteNonQuerySP(myConnection, myCommand, "stpInsValoresFacturaInServico", arrParams); 


		}

		//UPDATE FACTURA COM LINHAS EM TRANSACÇÃO 
		//DEVOLVE 1 SE CORREU BEM OU 0(ZERO) SE CORREU MAL 
	
		//==============================================================================================
		public int UpdateFacturaWithLinhas(string idFactura, string valorDespesasEnvio, string valorAjudasCustoDeslocacoes, string valorTotalFactura, string observacoes, string username,DataTable DTOrigem, DataTable DTDestino, string idTipoDocumento, string chCanalDistribuicao, string idEscritorioVendas, string idCondicoesPagamento, string idUtilAlteracao, string refBSE, string dataFactura, string refPedidoCliente, string dtPedidoCliente)			
		{
			//DTorigem = nao associadas - DTDestino = associadas
			//int retValue;

			string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.Connection = objConn; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction = objTrans; 

					int rowcount; 
					try
					{						
						rowcount = UpdateFacturaConn(objConn, objCmd, idFactura, valorDespesasEnvio, valorAjudasCustoDeslocacoes, valorTotalFactura, observacoes, username,idTipoDocumento, chCanalDistribuicao, idEscritorioVendas, idCondicoesPagamento,idUtilAlteracao, refBSE, dataFactura, refPedidoCliente, dtPedidoCliente); 
						
						if(rowcount<=0)
						{

							//ir buscar os dados de quem facturou serviço entretanto
							string strSQL = "SELECT dbo.udfGetNomeUtilizador(F.idUtilAlteracao) as Utilizador FROM FACTURA F WHERE idFactura = " + idFactura;
							
							objCmd.CommandType = CommandType.Text; 

							SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(objConn,objCmd,strSQL); 

//When using a DataReader YOU control the rowset population. That is, until
//Dr.Read returns False there are rows still to be fetched on the Connection.
//Until all rows are fetched, the connection cannot be used for any other
//operations. You need to finish fetching the rows or close the DataReader.

 


							if(dr.HasRows)
							{
								while(dr.Read())
								{
									HttpContext.Current.Response.Write("<div class='errorMessage'>" + dr["refServico"] + " Factura entretanto alterada por " + dr["Utilizador"] + ".</div>"); 
								}
							}

							dr.Close(); //cuidado, nao sei se o using nao fecha por fora.
							//so fecha se eu uso:(CommandBehavior.CloseConnection);
							objTrans.Rollback(); 
							return 0; 

							
						}
						else
						{
							//auditoria - void 
							objCmd.CommandType = CommandType.StoredProcedure; 


                        
							DataRow[] dRows = DTOrigem.Select(null,null,DataViewRowState.Added); 
                                
							int count; 
							foreach(DataRow dRow in dRows)
							{   
								//remove o idFactura associado ao serviço e os valores!
								string idServico = dRow["idServico"].ToString();
								count = UpdateServicoInFactura(objConn, objCmd, idServico, "", "", "", "", username); 
								if(count == 0)  throw(new Exception());

							}

							DataRow[] dRws = DTDestino.Select(null,null,DataViewRowState.CurrentRows); 
                                    
							foreach(DataRow dRw in dRws)
							{
								string idServico = dRw["idServico"].ToString();
								string valor = dRw["valor"].ToString();
								string percDesconto = dRw["percDesconto"].ToString();
								string valorFinal = dRw["valorFinal"].ToString();

								count = UpdateServicoInFactura(objConn, objCmd, idServico, idFactura, valor, percDesconto, valorFinal, username); 
								

								if(count == 0)  throw(new Exception());
							}

							objTrans.Commit(); 
							return 1; 
						}
					}
					catch(Exception ex)
					{ 	
						GERAL.clsWriteError.WriteLog(ex.Message); 
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception exep)
						{
							GERAL.clsWriteError.WriteLog(exep.Message); 
							return 0;
						}
						return 0; 
					}
				}
			}
			//return retValue; 
		}
		
		//======================================================================================
		//UPDATE À FACTURA DENTRO DE UMA TRANSACÇÃO - VOID
		//tirei o idEmpresa pq nao se deve poder alterar a empresa dentro de uma factura
		//======================================================================================
		public int UpdateFacturaConn(SqlConnection myConnection, SqlCommand myCommand,string idFactura, string valorDespesasEnvio, string valorAjudasCustoDeslocacoes, string valorTotalFactura, string observacoes, string username, string idTipoDocumento, string chCanalDistribuicao, string idEscritorioVendas, string idCondicoesPagamento, string idUtilAlteracao, string refBSE, string dataFactura, string refPedidoCliente, string dtPedidoCliente)
		{
			SqlParameter[] arrParams = new SqlParameter[15];

			arrParams[0] = new SqlParameter("@inIdFactura", idFactura);
			arrParams[1] = new SqlParameter("@inValorDespesasEnvio", GERAL.clsGeral.ConvertStringToDouble(valorDespesasEnvio));
			arrParams[2] = new SqlParameter("@inValorAjudasCustoDeslocacoes", GERAL.clsGeral.ConvertStringToDouble(valorAjudasCustoDeslocacoes));
			arrParams[3] = new SqlParameter("@inValorTotalFactura", GERAL.clsGeral.ConvertStringToDouble(valorTotalFactura));
			arrParams[4] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[5] = new SqlParameter("@inUsername", username);
			arrParams[6] = new SqlParameter("@inIdTipoDocumento", idTipoDocumento);
			arrParams[7] = new SqlParameter("@inCanalDistribuicao", chCanalDistribuicao);
			arrParams[8] = new SqlParameter("@inIdEscritorioVendas", idEscritorioVendas);
			arrParams[9] = new SqlParameter("@inIdCondicoesPagamento", idCondicoesPagamento);
			arrParams[10] = new SqlParameter("@inIdUtilAlteracao", idUtilAlteracao);
            arrParams[11] = new SqlParameter("@refBSE", refBSE);
            arrParams[12] = new SqlParameter("@dataFactura", dataFactura);
            arrParams[13] = new SqlParameter("@refPedidoCliente", refPedidoCliente);
            arrParams[14] = new SqlParameter("@dtPedidoCliente", dtPedidoCliente);

            return GERAL.clsDataAccess.intExecuteNonQuerySP(myConnection, myCommand, "stpUpdFactura", arrParams); 
		}

		//=============================================================================
		// Funcao que actualiza um Serviço (na Factura) e devolve uma msg de erro
		//=============================================================================
		public int UpdateServicoInFactura(SqlConnection myConnection, SqlCommand myCommand, string idServico, string idFactura, string valor, string percDesconto, string valorFinal, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[6];

			arrParams[0] = new SqlParameter("@inIdServico", idServico);
			arrParams[1] = new SqlParameter("@inIdFactura", idFactura);
			arrParams[2] = new SqlParameter("@inValor", GERAL.clsGeral.ConvertStringToDouble(valor));
			arrParams[3] = new SqlParameter("@inPercDesconto", GERAL.clsGeral.ConvertStringToDouble(percDesconto));
			arrParams[4] = new SqlParameter("@inValorFinal", GERAL.clsGeral.ConvertStringToDouble(valorFinal));
			arrParams[5] = new SqlParameter("@inUsername", username);

			
			return GERAL.clsDataAccess.intExecuteNonQuerySP(myConnection, myCommand, "stpUpdServicoInFactura", arrParams); 
		}

		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		// COISAS NOVAS ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		//====================================================================================
		//==============================????????????==========================================
		//====================================================================================
		public DataTable DTFicheiroFacturaSAP(string idFactura) //usado???
		{
			string strSQL =  "SELECT TE.descricao, ISNULL(E.numIdentificacao, '') AS numIdentEquipamento, ISNULL(E.numSerie, '') AS numSerieEquipamento, S.refServico, CAST(ISNULL(S.valorFinal, 0) AS varchar) AS valorFinal, Requisicao.referenciaCliente, Requisicao.dtRequisicao FROM Servico S INNER JOIN Equipamento E ON S.idEquipamento = E.idEquipamento INNER JOIN TipoEquipamento TE ON E.idTipoEquipamento = TE.idTipoEquipamento LEFT JOIN Requisicao on s.idRequisicao = Requisicao.idRequisicao WHERE S.idFactura = "+idFactura+" ORDER BY S.idServico"; 

			return GERAL.clsDataAccess.ExecuteDT(strSQL); 
            
		}


		//====================================================================================
		//==============================LISTAS================================================
		//====================================================================================
		// DATAREADER CODIGOS DE BLOQUEIO SAP
		//====================================================================================
		public SqlDataReader drCodigoBloqueioSap()
		{
			string strSQL = "SELECT codigoBloqueio AS id, descCodigoBloqueio AS descricao FROM sap_CodigoBloqueio where activo = 1"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
			
		}
		  
		//====================================================================================
		// DATAREADER ESCRITÓRIO DE VENDAS
		//====================================================================================
		public SqlDataReader drEscritorioVendas()
		{
			string strSQL = "SELECT idCodigoEscritVendas AS id, descCodigoEscritVendas AS descricao FROM sap_CodigoEscritVendas where activo = 1"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
			
		}

		//====================================================================================
		// DATAREADER CONDIÇÕES DE PAGAMENTO
		//====================================================================================
		public SqlDataReader drCondicoesPagamento()
		{
			string strSQL = "SELECT idCodigoCondPagam AS id, descCodigoCondPagam AS descricao FROM sap_CodigoCondPagam where activo = 1"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
			
		}

        //====================================================================================
        // DATAREADER IVA
        //====================================================================================
        public SqlDataReader drIva()
        {
            string strSQL = "SELECT idIva, Local, Valor FROM TaxaServicoIva";
            return GERAL.clsDataAccess.ExecuteDR(strSQL);

        }
        
        //====================================================================================
        // DATAREADER TIPO DE DOCUMENTO
        //====================================================================================
        public SqlDataReader drTipoDocumento()
		{
			string strSQL = "SELECT idCodigoTipoDocumento AS id, descCodigoTipoDocumento AS descricao FROM sap_CodigoTipoDocumento where activo = 1"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 

		}

		//====================================================================================
		// DATAREADER REGIÃO DE VENDAS
		//====================================================================================
		public SqlDataReader drRegiaoVendas()
		{
			string strSQL = "SELECT idCodigoRegiaoVendas AS id, descCodigoRegiaoVendas AS descricao FROM sap_CodigoRegiaoVendas where activo = 1"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 			
		}

		//====================================================================================
		// DATATABLE QUE SERVE DE BASE PARA CRIAR O FICHEIRO DE FACTURAÇÃO PARA O SAP
		//====================================================================================
		public DataTable dtServicosParaFicheiroSAP(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@inIdFactura", idFactura);
			
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosForFicheiroSAP", arrParams); 
		}

        //====================================================================================
        // DATATABLE QUE SERVE DE BASE PARA CRIAR o cabecalho da factura para o webservice SAP
        //====================================================================================
        public DataTable dtCabecalhoFacturaParWebServiceSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetHeaderFacturaForWSSAP", arrParams);
        }

        //====================================================================================
        // DATAREADER QUE SERVE DE BASE PARA CRIAR o cabecalho da factura para o webservice SAP
        //====================================================================================
        public SqlDataReader drCabecalhoFacturaParWebServiceSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetHeaderFacturaForWSSAP", arrParams);
        }


        //====================================================================================
        // DATATABLE QUE SERVE DE BASE PARA CRIAR as linhas de serviço para o webservice SAP
        //====================================================================================
        public DataTable dtLinhasFacturaParWebServiceSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetLinhasFacturaForWSSAP", arrParams);
        }


		//====================================================================================
		// DATATABLE QUE SERVE DE BASE PARA CRIAR as linhas de serviço para o webservice SAP
		//====================================================================================
		public DataTable dtLinhasFacturaParWebServiceSAPInterno(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@idFactura", idFactura);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("[stpGetLinhasFacturaINTERNAForWSSAP]", arrParams);
		}
		//====================================================================================
		// DATATABLE QUE SERVE DE BASE PARA CRIAR as linhas de deslocacao para o webservice SAP
		//====================================================================================
		public DataTable dtDeslocacoesParWebServiceSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetLinhasDeslocacaoForWSSAP", arrParams);
        }

        //====================================================================================
        // DATATABLE QUE SERVE DE BASE PARA CRIAR as linhas de despesas envio para o webservice SAP
        //====================================================================================
        public DataTable dtDespesasEnvioParWebServiceSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetLinhasDespesasEnvioForWSSAP", arrParams);
        }

        //====================================================================================
        // DATATABLE QUE SERVE DE BASE PARA CRIAR O FICHEIRO DE FACTURAÇÃO PARA O AXAPTA
        //====================================================================================
        public DataTable dtServicosParaFicheiroAXAPTA(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@inIdFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosForFicheiroAXAPTA", arrParams);
        }

     

        public DataTable dtServicosParaVD(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosParaVD", arrParams); 
        
        }

        public string insertVD(DataTable dt)
        {
            string numVenda = ""; 
        
            DataView dv = new DataView(dt);
            foreach (DataRowView drv in dv)
            {

                SqlParameter[] arrParams = new SqlParameter[11];

                arrParams[0] = new SqlParameter("@numero_responsavel", drv["numFuncionario"]);
                arrParams[1] = new SqlParameter("@cliente", drv["cliente"]);
                arrParams[2] = new SqlParameter("@morada", drv["morada"]);
                arrParams[3] = new SqlParameter("@localidade", drv["localidade"]);
                arrParams[4] = new SqlParameter("@nr_fiscal", drv["nif"]);
                arrParams[5] = new SqlParameter("@descricao", drv["descServico"]);
                arrParams[6] = new SqlParameter("@tipo", null);
                arrParams[7] = new SqlParameter("@valor", drv["valor"]);
                arrParams[8] = new SqlParameter("@identificador", drv["identificador"]);
                arrParams[9] = new SqlParameter("@pep", drv["pep"]);
                arrParams[10] = new SqlParameter("@iva", drv["codigoIVA"]);

                numVenda = GERAL.clsDataAccess.ExecuteNonQuerySPOutput_BD_VD("[InsereVendaMET]", arrParams);
            }

            return numVenda; //isto ainda nao está terminado aqui porque tenho de colocar uma validação
            //para perceber se o idvenda é igual em todas as linhas
        }

		//====================================================================================
		// DATATABLE QUE SERVE DE BASE PARA CRIAR AS LINHAS DE DESLOCAÇÃO PARA O FICHEIRO SAP
		//====================================================================================
		public DataTable dtDeslocacoes(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@idFactura", idFactura);

            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetDTDeslocacoesByIdFactura", arrParams); 
        }


        //====================================================================================
        // DATATABLE QUE SERVE DE BASE PARA CRIAR AS LINHAS DE DESLOCAÇÃO PARA O FICHEIRO SAP
        //====================================================================================
        public DataTable dtDeslocacoesWSSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idFactura", idFactura);

            
            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetDTDeslocacoesByIdFactura", arrParams);
        }


        //====================================================================================
        // DATATABLE QUE SERVE DE BASE PARA CRIAR AS LINHAS DE DESP.ENVIO/TRANSPORTE PARA O FICHEIRO SAP
        //====================================================================================
        public DataTable dtDespesasEnvio(string idFactura)
		{
			SqlParameter[] arrParams = new SqlParameter[1];

			arrParams[0] = new SqlParameter("@idFactura", idFactura);

         
        
        return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetDTDespesasEnvioByIdFactura", arrParams); 
    }

        public DataTable dtDespesasEnvioWSSAP(string idFactura)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idFactura", idFactura);


            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetLinhasDespesasEnvioForWSSAP", arrParams);
          
        }

    }
}

