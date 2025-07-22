using System;
using System.Data.SqlClient; 
using System.Data; 
using System.Configuration; 
using System.Web; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for FaxAvisoBD.
	/// </summary>
	public class FaxAvisoBD
	{
		public FaxAvisoBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        // Lista Empresas que têm equipamentos que podem entrar num fax de aviso
        public SqlDataReader DREmpresas()
        {
     

			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasForFaxAviso"); 
            
        }
                
        public SqlDataReader DREmpresasComFax()
        {
		

			

			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetEmpresasComFaxAviso"); 
            
        }


		//DataReader com todos os BRES de uma determinada empresa com equipamentos calibrados e que ainda nao foram avisados ao cliente por fax (mau portugues, eu sei...)
		public SqlDataReader DRBre(string idEmpresa)
		{
			//string strSQL = "SELECT DISTINCT idBRE, refBRE FROM vFaxAvisoLinha WHERE idEmpresa = "+idEmpresa; 

			//novo, descomentar dps. 
			string strSQL = "SELECT DISTINCT idBRE, refBRE FROM vFaxAvisoLinha WHERE isnull(idEmpresaContratante, idEmpresa ) = "+idEmpresa; 
 
			return LabMetro.GERAL.clsDataAccess.ExecuteDR(strSQL); 

		}  
        //DataTable com todos os equipamentos de uma determinada empresa com estados calibrados e que ainda nao foram avisados ao cliente por fax (mau portugues, eu sei...)
        public DataTable DTEquipamentos(string idBre)
        {
            string strSQL = "SELECT * FROM vFaxAvisoLinha WHERE idBre = "+idBre; 
         
            return LabMetro.GERAL.clsDataAccess.ExecuteDT(strSQL); 

            
        }  
 

        
        //DataReader com todos os faxes enviados para uma determindada empresa
        public SqlDataReader DRFaxesEnviados(string idEmpresa)
        {
            string strSQL = "SELECT idFax,dtEnvio FROM FaxEquipamentosCalibrados WHERE idEmpresa = "+ idEmpresa; 
            return LabMetro.GERAL.clsDataAccess.ExecuteDR(strSQL); 
                   
           
        }

        //DataTable com todos os equipamentos que foram incluidos num determinado fax
        public DataTable DTEquipamentosPorFax(string idFax)
        {
            string strSQL = "SELECT Equipamento.idEmpresa, TipoEquipamento.descricao AS tipoEquipamento, Equipamento.numIdentificacao, Servico.idServico,Servico.refServico,EstadoServico.descricao AS Estado FROM  Equipamento INNER JOIN TipoEquipamento ON Equipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento INNER JOIN  Servico ON Equipamento.idEquipamento = Servico.idEquipamento  INNER JOIN EstadoServico ON Servico.idEstadoServico = EstadoServico.idEstadoServico WHERE SERVICO.idFax ="+ idFax; 
         
            return LabMetro.GERAL.clsDataAccess.ExecuteDT(strSQL); 

            
        }   

        //dados de um contacto, neste momento so o fax directo e o fax da empresa
        //pode tb retornar os emails.
        public SqlDataReader DRContactInfo(string idContacto)
        {
            string strSQL = "SELECT * FROM vFaxAviso where idContacto = "+idContacto; 

            return LabMetro.GERAL.clsDataAccess.ExecuteDR(strSQL); 
                
            
        }        

        
        //retorna 0 se correu mal e 1 se correu bem
        //insere na tabela FaxEquipamentosCalibrados e faz update aos serviços com o numero de fax inserido, idFax = idFax...

        public int updateFaxData(string idFax, string idUtilizador, string localLevantamento)

        {
            string strSQL = "UPDATE FaxEquipamentosCalibrados SET dtEnvio = getDate(), idUtilizadorEnvio = " + idUtilizador + ", localLevantamento ='"+localLevantamento+"' WHERE idFax = " + idFax; 
            try
            {
                GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
                return 1;
           }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); //escrever para ficheiro de log
                return 0;
            }

        }

        public int insertFaxData(string idEmpresa, string idContacto, string numFax, string idUtilizador, string localLevantamento, DataView dv)
        {
            int ret = 0; 
            
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.Connection = objConn; 	
				objConn.Open(); 
	
    
				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction = objTrans; 	

					try
					{	 
		             
						SqlParameter[] arrParams = new SqlParameter[5]; 																							
						arrParams[0] = new SqlParameter("@idContacto",idContacto); 
						arrParams[1] = new SqlParameter("@idUtilizadorEnvio",idUtilizador);  
						arrParams[2] = new SqlParameter("@numFax",numFax); 
						arrParams[3] = new SqlParameter("@idEmpresa",idEmpresa);     
						arrParams[4] = new SqlParameter("@localLevantamento",localLevantamento);   

						string cmdText = "INSERT INTO FaxEquipamentosCalibrados(idEmpresa,idContacto,numFax,idUtilizadorEnvio,dtEnvio,localLevantamento) VALUES (@idEmpresa,@idContacto,@numFax,@idUtilizadorEnvio,getDate(),@localLevantamento)"; 

						objCmd.CommandType = CommandType.Text; 

						GERAL.clsDataAccess.myExecuteNonQueryParams(objConn,objCmd,cmdText,arrParams); 
		                  
						//objCmd.CommandText = "SELECT SCOPE_IDENTITY()"; --isto aqui nao funciona
						objCmd.CommandText = "SELECT @@IDENTITY"; //retorna um decimal

						int id= Decimal.ToInt32((decimal)objCmd.ExecuteScalar()); 
		                
						foreach(DataRowView drv in dv)
						{
							objCmd.CommandText = "UPDATE SERVICO SET idFax = " + id.ToString() + " WHERE idServico = "+ drv["idServico"]; 
							objCmd.ExecuteNonQuery(); 
						}
		               
						objTrans.Commit(); 
						ret = 1; 	
					}
					catch(Exception ex)
					{ 	 
						try
						{	
							objTrans.Rollback();
		                    
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); //escrever para ficheiro de log
						}
		               
						GERAL.clsWriteError.WriteLog(ex); //escrever para ficheiro de log
							
					
					}
					return ret;    
				}
			}
        }

        public string nomeFuncionario(string username)
        {
            string strSQL = "SELECT Funcionario.nomeAbreviado FROM Funcionario INNER JOIN Utilizador ON Funcionario.idUtilizador = Utilizador.idUtilizador WHERE Utilizador.userName = '"+username+"'";
            return GERAL.clsDataAccess.myExecuteScalar(strSQL).ToString(); 
        }

		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSFaxAviso(string idEmpresa, string idContacto, string idBRE)
		{
			LabMetro.DATASETS.DSFaxAviso ds = new LabMetro.DATASETS.DSFaxAviso(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.Connection = objConn; 	
				//objConn.Open(); 
	
				SqlDataAdapter DA = new SqlDataAdapter(objCmd);
			
				objCmd.CommandText= "SELECT  dbo.Empresa.idEmpresa, dbo.Empresa.nome AS empresa, CASE WHEN dbo.Titulo.descricao IS NULL  THEN dbo.Contacto.nome ELSE dbo.Titulo.descricao + ' ' + dbo.Contacto.nome COLLATE Latin1_General_CI_AS END AS contacto, dbo.Empresa.fax, dbo.Contacto.departamento, dbo.Empresa.email, dbo.Contacto.faxEmpresa AS faxContacto, dbo.Contacto.emailEmpresa AS emailContacto,  dbo.Contacto.idContacto,  ISNULL(sap.descCodigoCondPagam, 'Pronto Pagamento') as condicoesPagamento FROM  dbo.Empresa LEFT JOIN dbo.Contacto ON dbo.Empresa.idEmpresa = dbo.Contacto.idEmpresa LEFT JOIN dbo.Titulo ON dbo.Contacto.idTitulo = dbo.Titulo.idTitulo left join sap_CodigoCondPagam sap on empresa.idCondicoesPagamento = sap.idCodigoCondPagam WHERE dbo.Empresa.idEmpresa = "+ idEmpresa + " AND dbo.Contacto.idContacto = " + idContacto; 

				
				try
				{
					DA.Fill(ds,"dtFaxAviso"); 
				}
				catch(Exception ex)
				{	
					GERAL.clsWriteError.WriteLog(ex); 
				}

				objCmd.CommandText =  "SELECT  dbo.Equipamento.idEmpresa, dbo.TipoEquipamento.descricao AS tipoEquipamento, dbo.Equipamento.numIdentificacao, dbo.Servico.idServico, dbo.Servico.refServico, dbo.EstadoServico.descricao AS estadoServico,dbo.Servico.idBRE, dbo.udfGetReferenciaBRE(dbo.Servico.idBRE)  AS refBRE FROM dbo.Equipamento INNER JOIN  dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.Servico ON dbo.Equipamento.idEquipamento = dbo.Servico.idEquipamento INNER JOIN  dbo.EstadoServico ON dbo.Servico.idEstadoServico = dbo.EstadoServico.idEstadoServico WHERE  dbo.Servico.idBre = " + idBRE + "  AND (dbo.Servico.idEstadoServico IN (21,18,19,6,10)) AND (dbo.Servico.idBSE IS NULL) AND (dbo.Servico.idFax = '' OR  dbo.Servico.idFax IS NULL)"; 

				try
				{
					DA.Fill(ds,"dtFaxAvisoLinha");
				}
				catch(Exception ex)
				{	
					GERAL.clsWriteError.WriteLog(ex); 
				}
				
				DA.Dispose();
				DA = null; 
				
			}
			return ds; 
		}


		
		//===================================================================================================
		//FAZ FILL DO DATASET PARA OS CRYSTAL REPORTS
		//===================================================================================================
		public DataSet DSFaxAvisoEnviados(string idEmpresa, string idContacto, string idFax)
		{
			LabMetro.DATASETS.DSFaxAviso ds = new LabMetro.DATASETS.DSFaxAviso(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				objCmd.Connection = objConn; 	
				
	
				SqlDataAdapter DA = new SqlDataAdapter(objCmd);

				objCmd.CommandText= "SELECT  dbo.Empresa.idEmpresa, dbo.Empresa.nome AS empresa, CASE WHEN dbo.Titulo.descricao IS NULL  THEN dbo.Contacto.nome ELSE dbo.Titulo.descricao + ' ' + dbo.Contacto.nome COLLATE Latin1_General_CI_AS END AS contacto, dbo.Empresa.fax, dbo.Contacto.departamento, dbo.Empresa.email, dbo.Contacto.faxEmpresa AS faxContacto, dbo.Contacto.emailEmpresa AS emailContacto,  dbo.Contacto.idContacto FROM  dbo.Empresa LEFT JOIN dbo.Contacto ON dbo.Empresa.idEmpresa = dbo.Contacto.idEmpresa LEFT JOIN dbo.Titulo ON dbo.Contacto.idTitulo = dbo.Titulo.idTitulo WHERE dbo.Empresa.idEmpresa = "+ idEmpresa + " AND dbo.Contacto.idContacto = " + idContacto; 


				
				try
				{
					DA.Fill(ds,"dtFaxAviso"); 
				}
				catch(Exception ex)
				{	
					GERAL.clsWriteError.WriteLog(ex + objCmd.CommandText.ToString() ); 
				}
				objCmd.CommandText =  "SELECT   dbo.Equipamento.idEmpresa, dbo.TipoEquipamento.descricao AS tipoEquipamento, dbo.Equipamento.numIdentificacao, dbo.Servico.idServico, dbo.Servico.refServico, dbo.EstadoServico.descricao AS estadoServico, dbo.Servico.idFax FROM dbo.Equipamento INNER JOIN  dbo.TipoEquipamento ON dbo.Equipamento.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN  dbo.Servico ON dbo.Equipamento.idEquipamento = dbo.Servico.idEquipamento INNER JOIN  dbo.EstadoServico ON dbo.Servico.idEstadoServico = dbo.EstadoServico.idEstadoServico WHERE dbo.Servico.idFax = "+idFax; 
			
				//objCmd.Connection = objConn; 

				//faz fill da mesma datatable q acima, so com outro select.
				try
				{
					DA.Fill(ds,"dtFaxAvisoLinha");
				}
				catch(Exception ex)
				{	
					GERAL.clsWriteError.WriteLog(ex + objCmd.CommandText.ToString()); 
				}
				
				DA.Dispose();
				DA = null; 
				
			}
			return ds; 
		}
 
	}
}

