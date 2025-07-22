using System;
using System.Data;
using System.Data.SqlClient; 
using System.Configuration; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for EstadoEquipBD.
	/// </summary>
	public class EstadoEquipBD
	{
		public EstadoEquipBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //***********retorna todos os estados possiveis***********************
        public SqlDataReader DRGetListaEstadosServico()
        {
            return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetListaEstadosServico"); 
            
        }

		
		public SqlDataReader DRGetListaEstadosServicoRejeitados(string grupos)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@grupos", grupos);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListaEstadosServicoRejeitados", arrParams); 
			
		}

		public SqlDataReader DRGetDetalhesServico(string refServico)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inRefServico", refServico);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetDetalhesServico", arrParams); 


		}
        //===========quero retornar um dataset com uma datatable com todos os estados servicos
        //===========para popular uma dropdownlist, 
        //===========e outra datatable com todos os estados seguintes ao 
        //===========ao estado seleccionado
        public SqlDataReader DRGetEstadosServicosEsubsequentes(string idEstado)
        {
           string strSQL = "SELECT ES.idEstadoServico, ES.descricao FROM EstadoServico ES INNER JOIN  EstadoServicoSequencias ESS ON ES.idEstadoServico = ESS.idEstadoServicoFinal WHERE     ESS.idEstadoServicoInicial = "+idEstado+" UNION ALL SELECT ES.idEstadoServico, ES.descricao FROM EstadoServico ES WHERE ES.idEstadoServico = "+idEstado; 
            return GERAL.clsDataAccess.ExecuteDR(strSQL); 


        }

		//===========quero retornar um dataset com uma datatable com todos os estados servicos
		//===========para popular uma dropdownlist, 
		//===========e outra datatable com todos os estados seguintes ao 
		//===========ao estado seleccionado
		public SqlDataReader DRGetEstadosServicosEsubsequentesBO(string idEstado)
		{
			///string strSQL = "SELECT ES.idEstadoServico, ES.descricao FROM EstadoServico ES INNER JOIN  EstadoServicoSequencias ESS ON ES.idEstadoServico = ESS.idEstadoServicoFinal WHERE ESS.idEstadoServicoInicial= "+idEstado+" UNION SELECT ES.idEstadoServico, ES.descricao FROM EstadoServico ES WHERE ES.idEstadoServico = "+idEstado+" UNION SELECT ES.idEstadoServico, ES.descricao FROM estadoServico ES INNER JOIN  EstadoServicoSequenciasBO ESSBO ON ES.idEstadoServico = ESSBO.idEstadoServicoFinal WHERE ESSBO.idEstadoServicoInicial = "+idEstado+" ORDER BY 2 "; 

            //podem mudar para qq estado menos os estados resultantes do workflow
            string strSQL = "SELECT * FROM estadoServico where idEstadoServico not in (6,12,25,23)";
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 


		}

        //========= retorna uma lista de equipamentos com um estado especifico
        public DataTable DTEquipamentoByEstado(string idEstado, string idEmpresa, string equipamento, string bre)
        {   
            
          string strSQL =   "SELECT Servico.refServico, Empresa.nome AS Empresa, TipoEquipamento.descricao AS Equipamento, Equipamento.numSerie AS 'núm.Série', Equipamento.numIdentificacao AS 'núm.Ident.', CONVERT(VARCHAR, BRE.numBRE) + '/' + RIGHT(CONVERT(VARCHAR, dbo.BRE.ano), 2) AS NumBRE, ES.descricao as Estado, Servico.idServico, TipoEquipamento.acreditado  FROM Servico  INNER JOIN  Equipamento ON Servico.idEquipamento= Equipamento.idEquipamento "; 
			strSQL +=" INNER JOIN TipoEquipamento ON Equipamento.idTipoEquipamento = TipoEquipamento .idTipoEquipamento "; 
			strSQL +=" INNER JOIN BRE ON Servico.idBRE = BRE.idBRE "; 
			strSQL +=" INNER JOIN Empresa ON dbo.Equipamento.idEmpresa = Empresa.idEmpresa "; 
			strSQL +=" INNER JOIN EstadoServico ES ON Servico.idEstadoServico = ES.idEstadoServico "; 
			strSQL +=" AND BRE.idEmpresa = dbo.Empresa.idEmpresa "; 
			strSQL +=" WHERE Servico.bDefinitivo = 1 "; 
			strSQL +=" AND (TipoEquipamento.descricao LIKE '"+equipamento+"%') "; 
			if(bre !="") strSQL += " AND dbo.udfGetReferenciaBRE(BRE.idBRE) = '" + bre + "' "; 
			if(idEmpresa !="")	strSQL +="AND  Empresa.idEmpresa = " + idEmpresa +" " ; 
			if(idEstado != "") 	strSQL +="AND  Servico.idEstadoServico = " + idEstado +" " ; 
			strSQL +="ORDER BY Servico.refServico, tipoEquipamento.descricao ASC "; 
			//GERAL.clsWriteError.WriteLog(strSQL); 
            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        
        }
        

		//string numRegistos é o numero de id's que vem
        public string UpdateEstadosServicos(string idEstado, string idEstadoNovo, string userId, string idsServicos, int numRegistos)
        {

			
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try
					{
                   
						string strSQL = "UPDATE SERVICO SET idEstadoServico = "+idEstadoNovo+" , idUtilAlteracao ='"+ userId+"', dtAlteracao = CONVERT(DATETIME, GETDATE(), 102) "; 
						
						if(idEstadoNovo =="6" || idEstadoNovo == "25" || idEstadoNovo == "10" || idEstadoNovo =="26") 
							//-se passou para calibrado actualiza o estado do certificado
						{
							strSQL += " , idFuncionarioEfectuouServico = funcionario.idFuncionario, servico.idLocalCalibracao = ISNULL(funcionario.idLocalCalibracao, 1) "; 
							if(idEstadoNovo =="6" || idEstadoNovo == "10" ) //calibrado com certificado ou subcontratacao calibrada
							{
								strSQL +=" , idEstadoCertificado = 1, calibracaoExterna = 0, servico.dtEstado = CONVERT(DATETIME, GETDATE(), 102)  ";
							}
							else // ="25" 							+ 26
							{
								strSQL +=" , idEstadoCertificado = 1 , calibracaoExterna = 1, servico.dtEstado = CONVERT(DATETIME, GETDATE(), 102)  "; 
							}
						}

						strSQL +="	FROM funcionario WHERE funcionario.idUtilizador ='"+ userId+"' AND Servico.idServico IN ("+idsServicos+") AND Servico.idEstadoServico = "+ idEstado; 
						
						GERAL.clsWriteError.WriteLog(strSQL); 
						objCmd.CommandType = CommandType.Text; 
						objCmd.CommandText = strSQL; 


						if(objCmd.ExecuteNonQuery() != numRegistos) 
						{
							objTrans.Rollback();
							throw(new Exception("erro1")); //para ir catch e sair da ordem de execução


						}

						//o trigger para actualizar o funcionario que efectuou o serviço na tabela serviço
						//e a referencia da ultima calibracao na tabela equipamento nao funcionao no trigger quando
						//os ids veem dentro da sintaxe IN (...)
                      
						if(idEstadoNovo =="6" || idEstadoNovo == "25" || idEstadoNovo == "10" || idEstadoNovo =="26") 
							//-se passou para calibrado actualiza o estado do certificado
						{
                            //julho 2011 - acho está esquecido que quando é uma marcacao, deve aparecer a data da marcacao e nao a data da calibracao
                            string strSQL_Equipamento = "  UPDATE equipamento SET equipamento.refUltimaCalibracao = servico.refServico,equipamento.dtUltimaCalibracao = isnull(marcacao.dtMarcacao, CONVERT(DATETIME, GETDATE(), 102)) FROM equipamento inner join servico on servico.idEquipamento = equipamento.idEquipamento left join marcacao on servico.idBRE = marcacao.idBRE WHERE servico.idServico IN ( " + idsServicos + ") and servico.idTipoServico <> 'A' ";
                                
                                
                                //ERRO quando nao ha marcacao "UPDATE equipamento SET equipamento.refUltimaCalibracao = servico.refServico,equipamento.dtUltimaCalibracao = isnull(marcacao.dtMarcacao, CONVERT(DATETIME, GETDATE(), 102)) FROM equipamento,servico, marcacao WHERE servico.idServico IN ( " + idsServicos + ") AND servico.idEquipamento = equipamento.idEquipamento and marcacao.idBRE = servico.idbre";


                            //"UPDATE equipamento SET equipamento.refUltimaCalibracao = servico.refServico,equipamento.dtUltimaCalibracao = CONVERT(DATETIME, GETDATE(), 102) FROM equipamento,servico WHERE servico.idServico IN ( "+idsServicos+") AND servico.idEquipamento = equipamento.idEquipamento"; 
						
						
							GERAL.clsWriteError.WriteLog(strSQL_Equipamento); 
							objCmd.CommandType = CommandType.Text; 
							objCmd.CommandText = strSQL_Equipamento; 
							objCmd.ExecuteNonQuery(); //nao ha maneira de validar a nao ser que eu faça antes um count ao "select distinct idEquipamento from servico where idServico in (.....))
						}
						
//						if(objCmd.ExecuteNonQuery() != numRegistos)  //às vezes n funciona pq existem equipamento repetids nos serviços,
//							// dentro do inner join, isto dá um resultado igual ao num de equipamentos dentro dos serviços e nao aos								// numeros de servicos
//						{
//							objTrans.Rollback();
//							throw(new Exception("erro2")); //para ir catch e sair da ordem de execução
//						}

						

						objTrans.Commit(); 

						return GERAL.clsGeral.ErrorMessage.MSG_DB;     
					}	


					catch(Exception ex)
					{ 	
						try
						{	
							if(objTrans.Connection != null) //como sao fechadas la em cima, aqui ja podem chegar com nulls...

							{
								objTrans.Rollback();
							}
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); 
						}

						GERAL.clsWriteError.WriteLog(ex); 
						
						return GERAL.clsGeral.ErrorMessage.ERR_DB; 
					}
				}
			}
        }


        //SO RECEBE UM SERVIÇO
        public string UpdateEstadosServico(string idEstado, string idEstadoNovo, string userId, string idServico)
        {
            
            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())

            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objConn.Open();

                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;

                    try
                    {

                        string strSQL = "UPDATE SERVICO SET idEstadoServico = " + idEstadoNovo + " , idUtilAlteracao ='" + userId + "', dtAlteracao = CONVERT(DATETIME, GETDATE(), 102) ";

                        if (idEstadoNovo == "6" || idEstadoNovo == "25" || idEstadoNovo == "10" || idEstadoNovo == "26")
                        //-se passou para calibrado actualiza o estado do certificado
                        {
                            strSQL += " , idFuncionarioEfectuouServico = funcionario.idFuncionario, servico.idLocalCalibracao = ISNULL(funcionario.idLocalCalibracao, 1) ";
                            if (idEstadoNovo == "6" || idEstadoNovo == "10") //calibrado com certificado ou subcontratacao calibrada
                            {
                                strSQL += " , idEstadoCertificado = 1, calibracaoExterna = 0, servico.dtEstado = CONVERT(DATETIME, GETDATE(), 102)  ";
                            }
                            else // ="25" 							+ 26
                            {
                                strSQL += " , idEstadoCertificado = 1 , calibracaoExterna = 1, servico.dtEstado = CONVERT(DATETIME, GETDATE(), 102)  ";
                            }
                        }

                        strSQL += "	FROM funcionario WHERE funcionario.idUtilizador ='" + userId + "' AND Servico.idServico = " +idServico + " AND Servico.idEstadoServico = " + idEstado;

                        GERAL.clsWriteError.WriteLog(strSQL);
                        objCmd.CommandType = CommandType.Text;
                        objCmd.CommandText = strSQL;
                        if (objCmd.ExecuteNonQuery() != 1)
                        {
                            objTrans.Rollback();
                            throw (new Exception("erro1 " + strSQL)); //para ir catch e sair da ordem de execução

                        }

                        //o trigger para actualizar o funcionario que efectuou o serviço na tabela serviço
                        //e a referencia da ultima calibracao na tabela equipamento nao funcionao no trigger quando
                        //os ids veem dentro da sintaxe IN (...)
                        if (idEstadoNovo == "6" || idEstadoNovo == "25" || idEstadoNovo == "10" || idEstadoNovo == "26")
                        //-se passou para calibrado actualiza o estado do certificado
                        {
                            string strSQL_Equipamento = "UPDATE equipamento SET equipamento.refUltimaCalibracao = servico.refServico,equipamento.dtUltimaCalibracao = CONVERT(DATETIME, GETDATE(), 102) FROM equipamento,servico WHERE servico.idServico = " + idServico + " AND servico.idEquipamento = equipamento.idEquipamento";

                            GERAL.clsWriteError.WriteLog(strSQL_Equipamento);
                            objCmd.CommandType = CommandType.Text;
                            objCmd.CommandText = strSQL_Equipamento;
                            objCmd.ExecuteNonQuery(); //nao ha maneira de validar a nao ser que eu faça antes um count ao "select distinct idEquipamento from servico where idServico in (.....))
                        }

                     
                        objTrans.Commit();

                        return GERAL.clsGeral.ErrorMessage.MSG_DB;
                         
                    }

                    catch (Exception ex)
                    {
                        try
                        {
                            if (objTrans.Connection != null) //como sao fechadas la em cima, aqui ja podem chegar com nulls...
                            {
                                objTrans.Rollback();
                            }
                        }
                        catch (Exception excep)
                        {
                            GERAL.clsWriteError.WriteLog(excep);
                        }

                        GERAL.clsWriteError.WriteLog(ex);

                        return GERAL.clsGeral.ErrorMessage.ERR_DB;
                    }
                }
            }
        }

        public string updateLocalServico(string idServico, string idLocalNovo, string UserID)
        {
            string strSQL = "Update Servico set idLocalActual = " + idLocalNovo + ", idUtilAlteracao = '" + UserID + "', dtAlteracao = getdate() where idServico = " + idServico;

            return GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);


        }

	}
}
