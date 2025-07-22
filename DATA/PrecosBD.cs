using System;
using System.Data; 
using System.Data.SqlClient;
using System.Data.SqlTypes; 
using System.Configuration; 
using System.Web; 
using System.Collections; 
using LabMetro.GERAL; 

namespace LabMetro.DATA
{
    /// <summary>
    /// Summary description for PrecosBD.
    /// </summary>
    public class PrecosBD
    {
        public PrecosBD()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        //=======================================================================================
        //RETORNA DATAREADER COM LISTA DE FAMILIAS    
        //=======================================================================================
        public SqlDataReader DRFamilias(string idGrandeza)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            
            arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
            arrParams[1] = new SqlParameter("@inDescricao", "");
         
            return GERAL.clsDataAccess.SPExecuteDRParams("stpGetListFamilias", arrParams); 
        }
        
        //=======================================================================================
        //RETORNA DATAREADER COM LISTA DE TIPOS DE PREÇO    
        //=======================================================================================
        public SqlDataReader DRTipoPreço()
        {
            string strSQL = "SELECT idTipoPreco, descricao FROM TipoPreco"; 

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
        }
        
        //=======================================================================================
        //RETORNA DATAREADER COM LISTA DE CLASSES    
        //=======================================================================================
        public SqlDataReader DRClasse()
        {
            string strSQL = "SELECT idClasse, descricao FROM Classe ORDER BY descricao ASC"; 

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 

        }

        //=======================================================================================
        //TIPO DE PREÇO POR ID EQUIPAMENTO
        //RETORNA UM DATAREADER COM UMA LINHA SO
        //=======================================================================================
        public SqlDataReader DRTipoPrecoByIdEquipamento(string idTipoEquipamento,string idTipoServico)
        {
            string strSQL = "SELECT TipoPreco.idTipoPreco, TipoPreco.descricao FROM TipoPreco INNER JOIN TipoEquip_TipoPreco ON TipoPreco.idTipoPreco = TipoEquip_TipoPreco.idTipoPreco WHERE TipoEquip_TipoPreco.idTipoEquipamento = "+ idTipoEquipamento +" AND TipoEquip_TipoPreco.idTipoServico = '"+idTipoServico+"'"; 

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
        }

        public SqlDataReader DRListaUnidadeAlcance()
        {
            string strSQL = "SELECT idUnidadeAlcance as ident, descricao FROM UnidadeAlcance WHERE activo = 1 ORDER BY descricao ASC";

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
        }
        
        //=======================================================================================
        //RETORNA DATAREADER COM LISTA DE EQUIPAMENTOS ACTIVOS ASSOCIADAS A UMA FAMILIA
        //=======================================================================================
        public SqlDataReader DRTipoEquipamento(string idFamilia)
        {
            string strSQL = "SELECT idTipoEquipamento, descricao FROM TipoEquipamento WHERE activo = 1 AND idFamilia ="+idFamilia+" ORDER BY descricao ASC"; 

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
        }

        //=======================================================================================
        //RETORNA DATAREADER COM LISTA DE TIPOS DE EQUIPAMENTOS QUE AINDA N TEEM NENHUM TIPO 
        //PREÇO ASSOCIADO
        //=======================================================================================

        public SqlDataReader DRTipoEquipamentoSemTipoPreço(string idFamilia,string idTipoServico)
        {
            string strSQL = "SELECT idTipoEquipamento,descricao FROM TipoEquipamento WHERE activo = 1 AND TipoEquipamento.idFamilia ="+ idFamilia +" AND idTipoEquipamento NOT IN (SELECT idTipoEquipamento FROM TipoEquip_TipoPreco WHERE idTipoServico = '"+idTipoServico +"') ORDER BY descricao ASC";  

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
        }

        
        //=======================================================================================
        //RETORNA DATAREADER COM LISTA DE TIPOS DE EQUIPAMENTOS 
        //PARA OS QUAIS JA FOI INTRODUZIDO UM TIPO DE PREÇO

        //agora ja nao é preciso qeu tenham os tipos de preço porque todos para ja teem os preços directos // janeiro de 2014
        //=======================================================================================
        public SqlDataReader DRTipoEquipamentoComTipoPreco(string idFamilia, string idTipoServico)
        {
            string strSQL ="SELECT idTipoEquipamento,descricao FROM TipoEquipamento WHERE activo = 1 AND TipoEquipamento.idFamilia ="+ idFamilia ; // AND idTipoEquipamento IN (SELECT idTipoEquipamento FROM TipoEquip_TipoPreco WHERE idTipoServico = '"+idTipoServico +"') ORDER BY descricao ASC";  
            
            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
            
        }

        //=======================================================================================
        //RETORNA DATATABLE PARA LISTAGEM DE EQUIPAMENTOS E TIPOS DE PREÇO ASSOCIADOS
        //=======================================================================================
        public DataTable DTTiposPrecoPorEquipamento()
        {
            string strSQL = "SELECT dbo.TipoEquip_TipoPreco.id, dbo.Grandeza.descricao AS Grandeza, dbo.Familia.descricao AS Familia, dbo.TipoEquipamento.descricao AS TipoEquipamento,  dbo.TipoServico.descricao AS TipoServico, dbo.TipoPreco.descricao AS TipoPreco,TipoEquip_TipoPreco.bFormula FROM dbo.TipoEquip_TipoPreco INNER JOIN  dbo.TipoEquipamento ON dbo.TipoEquip_TipoPreco.idTipoEquipamento = dbo.TipoEquipamento.idTipoEquipamento INNER JOIN dbo.TipoPreco ON dbo.TipoEquip_TipoPreco.idTipoPreco = dbo.TipoPreco.idTipoPreco INNER JOIN  dbo.TipoServico ON dbo.TipoEquip_TipoPreco.idTipoServico = dbo.TipoServico.idTipoServico INNER JOIN      dbo.Familia ON dbo.TipoEquipamento.idFamilia = dbo.Familia.idFamilia INNER JOIN dbo.Grandeza ON dbo.Familia.idGrandeza = dbo.Grandeza.idGrandeza"; 

            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        }

        //=======================================================================================
        //INSERE UM REGISTO RELACIONADO TIPO EQUIPAMENTO A PREÇO
        //RETORNA UM INT: OU NUMERO DE ROWS AFECTADOS (SE CORRE BEM É 1)
        //OU O SQL ERROR NUMBER, TRATADO NO CODIGO DO CASE DE SER CHAVE DUPLICADA
        //=======================================================================================
        public int InsertRelTipoEquipPreco(string idTipoEquipamento, string idTipoPreco,string idTipoServico,string checkedString)
        {
            byte checkFormula = GERAL.clsGeral.ConvertStringToBool(checkedString); 

            string strSQL = "INSERT INTO TipoEquip_TipoPreco(idTipoEquipamento, idTipoServico,idTipoPreco,bFormula) VALUES("+idTipoEquipamento+", '"+idTipoServico+"',"+idTipoPreco+","+checkFormula+")";
 
            return GERAL.clsDataAccess.myExecuteNonQueryWithErrNumber(strSQL); 
        }

        //=======================================================================================
        //APAGA A RELACAO ENTRE TIPO DE EQUIPAMENTO E TIPO DE PREÇO
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //=======================================================================================
        public int DeleteRelation(string id) //id do REGISTO!!!

        {

            int ret = 0; 
            
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
				
						objCmd.CommandText = "SELECT * FROM TipoEquip_TipoPreco WHERE id = " + id; 
               
						SqlDataReader dr = objCmd.ExecuteReader(); 

						string idTipoEquipamento=""; 
						string idTipoServico=""; 
						string idTipoPreco =""; 
						if(dr.HasRows)
						{
							while(dr.Read()) 
							{
								idTipoEquipamento = dr["idTipoEquipamento"].ToString(); 
								idTipoServico = dr["idTipoServico"].ToString(); 
								idTipoPreco =dr["idTipoPreco"].ToString(); 
							}
						}
						dr.Close(); 

						objCmd.CommandText = "DELETE FROM TipoEquip_TipoPreco WHERE id = " + id; 
						objCmd.ExecuteNonQuery(); 
                
						//preços por tipo equipamento, tipo equipamento e quantidade, tipo equipamento e marca/modelo sao preços directos
						//os outros veem da tabela alcance simples ou alcances mistos e pontos.

						string strTableName ="";  
						switch(idTipoPreco)
						{
							case "1": //por tipo equipamento
								strTableName= "tbPrecosDirectos"; 
								break;
							case "2": //por tipo e quantidade
								strTableName= "tbPrecosDirectos"; 
								break;
							case "3": //tipo equip. e alcance

								strTableName= "tbAlcancesSimples";
								break;
							case "4": //tipo equip. e alc(%-ºC) e nº de pts/prs
								strTableName= "tbAlcancesMistos";  //atencao, apagar tb dos preços por alcances mistos 
								break;
							case "5"://tipo equip. e alcance + classe
								strTableName= "tbAlcancesSimples"; 
								break;
							case "6"://tipo equip. e marca e modelo
								strTableName= "tbPrecosDirectos"; 
								break;
						}
		                
						objCmd.CommandText = "DELETE FROM "+strTableName+" WHERE idTipoEquipamento = "+idTipoEquipamento +" AND idTipoServico ='"+idTipoServico+"' "; //está aqui activado um cascade delete para apagar todos os preços q contenham o tipo...


						objCmd.ExecuteNonQuery(); 
		                
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
							GERAL.clsWriteError.WriteLog(excep); 
						}
						GERAL.clsWriteError.WriteLog(ex); 
		                 
					}
					
					return ret; 	
				}
			}
        }
        //**************************************************************************************//
        //**************************ALCANCES MISTOS*********************************************//
        //**************************************************************************************//
        
        
        //======================================================================================
        //RETORNA UMA DATATABLE COM OS ALCANCES MISTOS POR TIPO DE EQUIPAMENTO
        //======================================================================================
        public DataTable DTAlcancesMistosByIdTipoEquipamento(string idTipoEquipamento,string idTipoServico)
        {
            string strSQL = "SELECT idAlcanceMisto, T1, T2, T3, T4, H1, H2, H3, H4 FROM tbAlcancesMistos WHERE idTipoEquipamento = "+idTipoEquipamento+" AND idTipoServico = '"+idTipoServico+"'"; 

            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        }

        
        //======================================================================================
        //INSERE NA TABELA ALCANCES MISTOS
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int InsertAlcanceMisto(string idTipoEquipamento,string idTipoServico, string t1, string t2, string t3, string t4, string h1, string h2, string h3, string h4,string username)
        {

           SqlParameter[] arrParams = new SqlParameter[11];       
            
           arrParams[0] = new SqlParameter("@idTipoEquipamento",idTipoEquipamento); 
           arrParams[1] = new SqlParameter("@idTipoServico",idTipoServico);  
			
			if(t1!="") 	
			{
				arrParams[2] = new SqlParameter("@t1",GERAL.clsGeral.convertDecimalSeparator(t1)); 
			}
			else
			{
				arrParams[2] = new SqlParameter("@t1",null); 
			}
			
			if(t2!="") 	
			{
				arrParams[3] = new SqlParameter("@t2",GERAL.clsGeral.convertDecimalSeparator(t2)); 
			}
			else
			{
				arrParams[3] = new SqlParameter("@t2",null); 
			}
			
			if(t3!="") 	
			{
				arrParams[4] = new SqlParameter("@t3",GERAL.clsGeral.convertDecimalSeparator(t3)); 
			}
			else
			{
				arrParams[4] = new SqlParameter("@t3",null); 
			}
			
			if(t4!="") 	
			{
				arrParams[5] = new SqlParameter("@t4",GERAL.clsGeral.convertDecimalSeparator(t4)); 
			}
			else
			{
				arrParams[5] = new SqlParameter("@t4",null); 
			}

			if(h1!="") 	
			{
				arrParams[6] = new SqlParameter("@h1",GERAL.clsGeral.convertDecimalSeparator(h1)); 
			}
			else
			{
				arrParams[6] = new SqlParameter("@h1",null); 
			}
			if(h2!="") 	
			{
				arrParams[7] = new SqlParameter("@h2",GERAL.clsGeral.convertDecimalSeparator(h2)); 
			}
			else
			{
				arrParams[7] = new SqlParameter("@h2",null); 
			}
			if(h3!="") 	
			{
				arrParams[8] = new SqlParameter("@h3",GERAL.clsGeral.convertDecimalSeparator(h3)); 
			}
			else
			{
				arrParams[8] = new SqlParameter("@h3",null); 
			}
			if(h4!="") 	
			{
				arrParams[9] = new SqlParameter("@h4",GERAL.clsGeral.convertDecimalSeparator(h4)); 
			}
			else
			{
				arrParams[9] = new SqlParameter("@h4",null); 
			}
           
			arrParams[10] = new SqlParameter("@username",username); 


			//convertdecimalseparator está a ser feito nas variaveis.
           string strSQL = "INSERT INTO tbAlcancesMistos(idTipoEquipamento,idTipoServico,T1,T2,T3,T4,H1,H2,H3,H4,dtCriacao,idUtilCriacao,dtAlteracao,idUtilAlteracao)VALUES (@idTipoEquipamento,@idTipoServico,@t1,@t2,@t3,@t4,@h1,@h2,@h3,@h4,getDate(),@username,getDate(),@userName)";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //UPDATE NA TABELA ALCANCES MISTOS
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int UpdateAlcanceMisto(string idAlcanceMisto, string t1, string t2, string t3, string t4, string h1, string h2, string h3, string h4,string username)
        {
            SqlParameter[] arrParams = new SqlParameter[10];       
            
            arrParams[0] = new SqlParameter("@idAlcanceMisto",idAlcanceMisto); 
          
			if(t1!="") 	
			{
				arrParams[1] = new SqlParameter("@t1",GERAL.clsGeral.convertDecimalSeparator(t1)); 
			}
			else
			{
				arrParams[1] = new SqlParameter("@t1",null); 
			}
			if(t2!="") 	
			{
				arrParams[2] = new SqlParameter("@t2",GERAL.clsGeral.convertDecimalSeparator(t2)); 
			}
			else
			{
				arrParams[2] = new SqlParameter("@t2",null); 
			}
			if(t3!="") 	
			{
				arrParams[3] = new SqlParameter("@t3",GERAL.clsGeral.convertDecimalSeparator(t3)); 
			}
			else
			{
				arrParams[3] = new SqlParameter("@t3",null); 
			}
			if(t4!="") 	
			{
				arrParams[4] = new SqlParameter("@t4",GERAL.clsGeral.convertDecimalSeparator(t4)); 
			}
			else
			{
				arrParams[4] = new SqlParameter("@t4",null); 
			}

			if(h1!="") 	
			{
				arrParams[5] = new SqlParameter("@h1",GERAL.clsGeral.convertDecimalSeparator(h1)); 
			}
			else
			{
				arrParams[5] = new SqlParameter("@h1",null); 
			}
			if(h2!="") 	
			{
				arrParams[6] = new SqlParameter("@h2",GERAL.clsGeral.convertDecimalSeparator((h2))); 
			}
			else
			{
				arrParams[6] = new SqlParameter("@h2",null); 
			}
			if(h3!="") 	
			{
				arrParams[7] = new SqlParameter("@h3",GERAL.clsGeral.convertDecimalSeparator(h3)); 
			}
			else
			{
				arrParams[7] = new SqlParameter("@h3",null); 
			}
			if(h4!="") 	
			{
				arrParams[8] = new SqlParameter("@h4",GERAL.clsGeral.convertDecimalSeparator((h4))); 
			}
			else
			{
				arrParams[8] = new SqlParameter("@h4",null); 
			}
            
            
            arrParams[9] = new SqlParameter("@username",username); 

			//convertdecimalseparator está a ser feito nas variaveis
            string strSQL = "UPDATE tbAlcancesMistos SET T1 = @t1,T2=@t2,T3=@t3,T4=@t4,H1=@h1,H2=@h2,H3=@h3,H4=@h4,dtAlteracao=getDate(),idUtilAlteracao=@username WHERE idAlcanceMisto = @idAlcanceMisto";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //APAGA UM ALCANCE MISTO E APAGA IMEDIATAMENTO OS PREÇOS RELACIONAODS COM ESTE ALCANCE 
        //RETORNA 0 SE CORREU MAL E 1 SE CORREU BEM
        //======================================================================================

        public int DeleteFromAlcancesMistos(string idAlcanceMisto)
        {

            int ret = 0; 
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
						objCmd.CommandText = "DELETE FROM tbPrecosAlcancesMistos WHERE idAlcanceMisto = "+idAlcanceMisto; 
						objCmd.ExecuteNonQuery(); 

						objCmd.CommandText = "DELETE FROM tbAlcancesMistos WHERE idAlcanceMisto = "+idAlcanceMisto;  
						objCmd.ExecuteNonQuery(); 

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
							GERAL.clsWriteError.WriteLog(excep); 
						}
						GERAL.clsWriteError.WriteLog(ex); 
					}
				
					return ret; 	
				}
			}
        }
        //======================================================================================
        //RETORNA UMA DATATABLE COM OS PREÇO POR ALCANCES MISTOS 
        //======================================================================================
        public DataTable DTPrecosAlcancesMistos(string idAlcanceMisto)
        {
            string strSQL = "SELECT * FROM tbPrecosAlcancesMistos WHERE idAlcanceMisto = "+idAlcanceMisto; 

            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        }

        
        //======================================================================================
        //INSERE NA TABELA PREÇOS POR ALCANCES MISTOS
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int InsertPrecoAlcanceMisto(string idAlcanceMisto, string p1, string p2, string preco, string precoExterior, string precoMovel,string username)
        {
           
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
                //preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }

            SqlParameter[] arrParams = new SqlParameter[7];       
            
            arrParams[0] = new SqlParameter("@idAlcanceMisto",idAlcanceMisto); 
            arrParams[1] = new SqlParameter("@p1",p1);
            arrParams[2] = new SqlParameter("@p2",p2); 
            arrParams[3] = new SqlParameter("@preco",dblPreco); 
            arrParams[4] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[5] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[6] = new SqlParameter("@username",username); 
			
            string strSQL = "INSERT INTO tbPrecosAlcancesMistos(idAlcanceMisto,P1,P2,preco, precoExterior,precoMovel,dtCriacao,idUtilCriacao,dtAlteracao,idUtilAlteracao) VALUES (@idAlcanceMisto,@p1,@p2,@preco,@precoExterior,@precoMovel,getDate(),@username,getDate(),@userName)";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //UPDATE NA TABELA PREÇOS POR ALCANCES MISTOS
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int UpdatePrecoAlcanceMisto(string idPrecoMisto, string p1, string p2, string preco, string precoExterior, string precoMovel,string username)
        {
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
              //  preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }

            SqlParameter[] arrParams = new SqlParameter[7];       
            
            arrParams[0] = new SqlParameter("@idPrecoMisto",idPrecoMisto); 
            arrParams[1] = new SqlParameter("@p1",p1);  
            arrParams[2] = new SqlParameter("@p2",p2); 
            arrParams[3] = new SqlParameter("@preco",dblPreco); 
            arrParams[4] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[5] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[6] = new SqlParameter("@username",username);  

            string strSQL = "UPDATE tbPrecosAlcancesMistos SET p1 = @p1,p2=@p2,preco=@preco,precoExterior=@precoExterior,precoMovel=@precoMovel,dtAlteracao=getDate(),idUtilAlteracao=@username WHERE idPrecoMisto = +@idPrecoMisto";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }
        
        //=======================================================================================
        //APAGA UM PREÇO POR ALCANCE MISTO
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //=======================================================================================
        public int DeleteFromPrecosAlcancesMistos(string idPrecoMisto)

        {
            string strSQL = "DELETE FROM tbPrecosAlcancesMistos WHERE idPrecoMisto = " + idPrecoMisto; 

            return GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
        }


        //**************************************************************************************//
        //**************************ALCANCES SIMPLES*********************************************//
        //**************************************************************************************//
            

        //======================================================================================
        //RETORNA UMA DATATABLE COM OS ALCANCES SIMPLES POR TIPO DE EQUIPAMENTO
        //======================================================================================
        public DataTable DTAlcancesSimplesByIdTipoEquipamento(string idTipoEquipamento,string idTipoServico)
        {
            string strSQL = "SELECT tbAlcancesSimples.idAlcanceSimples, tbAlcancesSimples.A1, tbAlcancesSimples.A2, tbAlcancesSimples.idUnidadeAlcance, Classe.descricao AS classe, UnidadeAlcance.descricao, tbAlcancesSimples.A2, tbAlcancesSimples.idClasse, Classe.descricao AS classe, UnidadeAlcance.descricao AS alcance,tbAlcancesSimples.preco,tbAlcancesSimples.precoExterior,tbAlcancesSimples.precoMovel FROM tbAlcancesSimples LEFT OUTER JOIN UnidadeAlcance ON tbAlcancesSimples.idUnidadeAlcance = UnidadeAlcance.idUnidadeAlcance LEFT OUTER JOIN Classe ON tbAlcancesSimples.idClasse = Classe.idClasse WHERE tbAlcancesSimples.idTipoEquipamento = "+idTipoEquipamento+" AND tbAlcancesSimples.idTipoServico = '"+idTipoServico+"'"; 

            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        }

        //======================================================================================
        //INSERE NA TABELA ALCANCES SIMPLES
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int InsertAlcanceSimples(string idTipoEquipamento,string idTipoServico, string a1, string a2, string idUnidadeAlcance,string idClasse,string preco, string precoExterior, string precoMovel,string username)
        {

            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

//            int alcInf = GERAL.clsGeral.ConvertStringToInt(a1); 
//            int alcSup = GERAL.clsGeral.ConvertStringToInt(a1); ; 
//            

            if(preco!="") 
            {
                //preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }

            SqlParameter[] arrParams = new SqlParameter[10];       
            
            arrParams[0] = new SqlParameter("@idTipoEquipamento",idTipoEquipamento); 
            arrParams[1] = new SqlParameter("@idTipoServico",idTipoServico); 

			if(a1!="")
			{
				arrParams[2] = new SqlParameter("@a1",Convert.ToDouble(a1));
			}
			else
			{
				arrParams[2] = new SqlParameter("@a1",null);
			}
			
			if(a2!="")
			{
				arrParams[3] = new SqlParameter("@a2",Convert.ToDouble(a2)); 
			}
			else
			{
				arrParams[3] = new SqlParameter("@a2",null); 
			}
  
            
            arrParams[4] = new SqlParameter("@idUnidadeAlcance",idUnidadeAlcance); 
            arrParams[5] = new SqlParameter("@idClasse",idClasse); 
            arrParams[6] = new SqlParameter("@preco",dblPreco); 
            arrParams[7] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[8] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[9] = new SqlParameter("@username",username); 

            string strSQL = "INSERT INTO tbAlcancesSimples(idTipoEquipamento,idTipoServico,A1,A2,idUnidadeAlcance,idClasse,preco,precoExterior,precoMovel,dtCriacao,idUtilCriacao,dtAlteracao,idUtilAlteracao) VALUES (@idTipoEquipamento,@idTipoServico,"+GERAL.clsGeral.convertDecimalSeparator(@a1)+","+GERAL.clsGeral.convertDecimalSeparator(@a2)+",@idUnidadeAlcance,@idClasse,@preco,@precoExterior,@precoMovel,getDate(),@username,getDate(),@username)";  
            
			//HttpContext.Current.Response.Write(strSQL); 

            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //UPDATE NA TABELA ALCANCES SIMPLES
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int UpdateAlcanceSimples(string idAlcanceSimples, string a1, string a2,string idUnidadeAlcance,string idClasse,string preco, string precoExterior, string precoMovel,string username)
        {
        
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
                //preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }

            SqlParameter[] arrParams = new SqlParameter[9];       
            
            arrParams[0] = new SqlParameter("@idAlcanceSimples",idAlcanceSimples);

			
			if(a1!="")
			{
				arrParams[1] = new SqlParameter("@a1",Convert.ToDouble(a1));
			}
			else
			{
				arrParams[1] = new SqlParameter("@a1",null);
			}
			
			if(a2!="")
			{
				arrParams[2] = new SqlParameter("@a2",Convert.ToDouble(a2)); 
			}
			else
			{
				arrParams[2] = new SqlParameter("@a2",null); 
			}
  
            
            arrParams[3] = new SqlParameter("@idUnidadeAlcance",idUnidadeAlcance); 
            arrParams[4] = new SqlParameter("@idClasse",idClasse); 
            arrParams[5] = new SqlParameter("@preco",dblPreco); 
            arrParams[6] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[7] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[8] = new SqlParameter("@username",username); 

            string strSQL = "UPDATE tbAlcancesSimples SET A1='"+GERAL.clsGeral.convertDecimalSeparator(@a1)+"',A2='"+GERAL.clsGeral.convertDecimalSeparator(@a2)+"',idUnidadeAlcance=@idUnidadeAlcance,idClasse=@idClasse ,preco=@preco,precoExterior=@precoExterior,precoMovel=@precoMovel,dtAlteracao=getDate(), idUtilAlteracao=@username WHERE idAlcanceSimples = @idAlcanceSimples";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //APAGA UM ALCANCE SIMPLES E APAGA IMEDIATAMENTO OS PREÇOS RELACIONAODS COM ESTE ALCANCE 
        //RETORNA 0 SE CORREU MAL E 1 SE CORREU BEM
        //======================================================================================

        public int DeleteFromAlcancesSimples(string idAlcanceSimples)
        {

            string strSQL = "DELETE FROM tbAlcancesSimples WHERE idAlcanceSimples = "+idAlcanceSimples;  

            return GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
            

            
        }

        //**************************************************************************************//
        //**************************PREÇOS SIMPLES*********************************************//
        //**************************************************************************************//
            

        //======================================================================================
        //RETORNA UMA DATATABLE COM OS PREÇOS DIRECTOS POR TIPO DE EQUIPAMENTO
        //======================================================================================
        public DataTable DTPrecosDirectos(string idTipoEquipamento, string idTipoServico)
        {
            string strSQL = "SELECT * FROM tbPrecosDirectos WHERE idTipoEquipamento = " +idTipoEquipamento + " AND idTipoServico = '" + idTipoServico +"'";  

            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        }

        //======================================================================================
        //INSERE NA TABELA PREÇOS DIRECTOS
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int InsertPrecoDirecto(string idTipoEquipamento,string idTipoServico, string preco, string precoExterior, string precoMovel,string username)
        {
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
                //preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }

            SqlParameter[] arrParams = new SqlParameter[9];       
            
            arrParams[0] = new SqlParameter("@idTipoEquipamento",idTipoEquipamento); 
            arrParams[1] = new SqlParameter("@idTipoServico",idTipoServico);  
            arrParams[2] = new SqlParameter("@quant1",null); 
            arrParams[3] = new SqlParameter("@quant2",null); 
            arrParams[4] = new SqlParameter("@unidade",null); 
            arrParams[5] = new SqlParameter("@preco",dblPreco); 
            arrParams[6] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[7] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[8] = new SqlParameter("@username",username); 


            string strSQL = "INSERT INTO tbPrecosDirectos(idTipoEquipamento,idTipoServico,quant1,quant2,unidQuant,preco,precoExterior,precoMovel,dtCriacao,idUtilCriacao,dtAlteracao, idUtilAlteracao) VALUES (@idTipoEquipamento,@idTipoServico,@quant1,@quant2,@unidade,@preco,@precoExterior,@precoMovel,getDate(),@username,getDate(),@username)";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //UPDATE NA TABELA PREÇOS DIRECTOS
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int UpdatePrecosDirectos(string idPrecoDirecto, string preco, string precoExterior, string precoMovel,string username)
        {
            
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
                //preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }

            SqlParameter[] arrParams = new SqlParameter[8];       
            
            arrParams[0] = new SqlParameter("@idPrecoDirecto",idPrecoDirecto); 
            arrParams[1] = new SqlParameter("@quant1",null); 
            arrParams[2] = new SqlParameter("@quant2",null); 
            arrParams[3] = new SqlParameter("@unidade",null); 
            arrParams[4] = new SqlParameter("@preco",dblPreco); 
            arrParams[5] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[6] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[7] = new SqlParameter("@username",username); 

            string strSQL = "UPDATE tbPrecosDirectos SET quant1 = @quant1,quant2=@quant2,preco=@preco,precoExterior=@precoExterior,precoMovel=@precoMovel,dtAlteracao=getDate(),idUtilAlteracao=@username WHERE idPrecoDirecto = @idPrecoDirecto";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //=======================================================================================
        //APAGA UM PREÇO DIRECTO 
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //=======================================================================================
        public int DeleteFromPrecosDirectos(string idPrecoDirecto)
        {
            string strSQL = "DELETE FROM tbPrecosDirectos WHERE idPrecoDirecto = " + idPrecoDirecto; 

            return GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
        }
        
        //**********************************************************************************************
        //***************************PREÇOS MARCA MODELO ****************************************
        //**********************************************************************************************
        
        public DataTable DTPrecoMarcaModelo(string idTipoEquipamento,string idTipoServico)

        {
            string strSQL = "SELECT tbPrecosDirectos.idPrecoDirecto, Marca.descricao AS marca, Modelo.descricao AS modelo, tbPrecosDirectos.preco, tbPrecosDirectos.precoExterior, tbPrecosDirectos.precoMovel FROM tbPrecosDirectos LEFT OUTER JOIN Modelo ON tbPrecosDirectos.idModelo = Modelo.idModelo LEFT OUTER JOIN Marca ON Modelo.idMarca = Marca.idMarca WHERE tbPrecosDirectos.idTipoEquipamento = "+idTipoEquipamento+" AND tbPrecosDirectos.idTipoServico = '"+idTipoServico+"'" ; 

            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        }


        //======================================================================================
        //INSERE NA TABELA PREÇOS DIRECTOS MAS PREÇOS POR MARCA MODELO
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        //falta pôr os useres mas primeiro pôr isto a funcionar
        public int InsertPrecoMarcaModelo(string idTipoEquipamento,string idTipoServico,string idModelo,string preco, string precoExterior, string precoMovel,string username)
        {
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
                //preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }


            SqlParameter[] arrParams = new SqlParameter[7];       
            
            arrParams[0] = new SqlParameter("@idTipoEquipamento",idTipoEquipamento); 
            arrParams[1] = new SqlParameter("@idTipoServico",idTipoServico); 
            arrParams[2] = new SqlParameter("@idModelo",idModelo); 
            arrParams[3] = new SqlParameter("@preco",dblPreco); 
            arrParams[4] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[5] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[6] = new SqlParameter("@username",username); 

            string strSQL = "INSERT INTO tbPrecosDirectos(idTipoEquipamento,idTipoServico, idModelo,preco,precoExterior,precoMovel,dtCriacao,idUtilCriacao,dtAlteracao,idUtilAlteracao) VALUES (@idTipoEquipamento,@idTipoServico,@idModelo,@preco,@precoExterior,@precoMovel,getDate(),@username,getDate(),@username)";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        //======================================================================================
        //UPDATE NA TABELA PREÇOS DIRECTOS MAS PREÇOS POR MARCA MODELO
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        
        public int UpdatePrecosMarcaModelo(string idPrecoDirecto,string preco, string precoExterior, string precoMovel,string username)
        {
           
            double dblPreco= 0; 
            double dblPrecoExterior= 0; 
            double dblPrecoMovel= 0; 

            if(preco!="") 
            {
               // preco=GERAL.clsGeral.convertDecimalSeparator(preco);
                dblPreco = Convert.ToDouble(preco); 
            }
            
            if(precoExterior!="") 
            {
                //precoExterior=GERAL.clsGeral.convertDecimalSeparator(precoExterior);
                dblPrecoExterior = Convert.ToDouble(precoExterior); 
            }
            
            if(precoMovel!="")
            {
                //precoMovel =GERAL.clsGeral.convertDecimalSeparator(precoMovel);
                dblPrecoMovel = Convert.ToDouble(precoMovel); 
            }



            SqlParameter[] arrParams = new SqlParameter[5];       
            
            arrParams[0] = new SqlParameter("@idPrecoDirecto",idPrecoDirecto); 
            arrParams[1] = new SqlParameter("@preco",dblPreco); 
            arrParams[2] = new SqlParameter("@precoExterior",dblPrecoExterior); 
            arrParams[3] = new SqlParameter("@precoMovel",dblPrecoMovel); 
            arrParams[4] = new SqlParameter("@username",username); 

            string strSQL = "UPDATE tbPrecosDirectos SET preco=@preco,precoExterior=@precoExterior,precoMovel=@precoMovel,dtAlteracao=getDate(),idUtilAlteracao=@username WHERE idPrecoDirecto = @idPrecoDirecto";  
            
            return GERAL.clsDataAccess.myExecuteNonQueryParams(strSQL,arrParams); 

        }

        
        //======================================================================================
        //APAGA TB DA TABELA PRECOS DIRECTOS VISTO A INFORMACAO SER GUARDADA NA MESMA TABELA
        //no fundo, esta função está repetida, mas pela coerencia, fica aqui com um nome proprio
        //RETORNA O NUMERO DE LINHAS AFACTADAS, SE CORREU BEM, DEVOLVE 1
        //======================================================================================
        public int DeleteFromPrecosMarcaModelo(string idPrecoDirecto)
        {
            try
            {
                string strSQL = "DELETE FROM tbPrecosDirectos WHERE idPrecoDirecto = " + idPrecoDirecto; 

                return GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
                return 0; 
            }            
        }

        //======================================================================================
        //FUNCOES QUE VAO BUSCAR PREÇOS
        //======================================================================================
        public double getPriceByTipoEquipamento(string idTipoEquipamento,string idTipoServico,string tipoPr)
        {
            try
            {
                string strSQL ="SELECT "+ tipoPr +" FROM tbPrecosDirectos WHERE idTipoEquipamento = "+idTipoEquipamento+" AND idTipoServico = '"+idTipoServico+"' "; 
            
                return Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
                return 0; 
            }
                
        }
            
        public double getPriceByIdModelo(string idTipoEquipamento,string idModelo,string idTipoServico,string tipoPr)
        {
            if(idModelo =="") return 0; 
            
            try
            {
                string strSQL ="SELECT "+ tipoPr +" FROM tbPrecosDirectos WHERE idTipoEquipamento = "+idTipoEquipamento+" AND idModelo = "+idModelo+" AND idTipoServico = '"+idTipoServico+"' "; 
        
                return Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
                return 0; 
            }
                
        }

        public double getPriceByTipoEquipamentoQuantidade(string idTipoEquipamento,string idTipoServico,string quantidade,string tipoPr)
        {
            if(quantidade=="") return 0; 
            
            try
            {
                string strSQL ="SELECT "+ tipoPr +"  FROM tbPrecosDirectos WHERE idTipoEquipamento = "+idTipoEquipamento+" AND idTipoServico = '"+idTipoServico+"' AND ( "+quantidade+" BETWEEN quant1 AND quant2) "; 

				double res = Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL)) * Convert.ToInt32(quantidade); 
                return res; 

            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
                return 0; 
            }
                
        }

        public double getPriceByTipoEquipamentoAlcanceSimples(string idTipoEquipamento,string idTipoServico,string idUnidadeAlcance, string alcanceInf, string alcanceSup,string tipoPr)
        {
            if(idUnidadeAlcance =="") return 0; 
            if(alcanceInf=="") return 0; 
            if(alcanceSup =="") alcanceSup = alcanceInf; 
            
            try
            {
                string strSQL = "SELECT tbAlcancesSimples."+tipoPr+" FROM tbAlcancesSimples WHERE     idTipoEquipamento = "+idTipoEquipamento+" AND idTipoServico = '"+idTipoServico+"' AND A1 < = '"+GERAL.clsGeral.convertDecimalSeparator(alcanceInf)+"'  AND A2 >='"+clsGeral.convertDecimalSeparator(alcanceSup)+"' AND idUnidadeAlcance ="+idUnidadeAlcance ; 

                return Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
                return 0; 
            }
        
        }

        public double getPriceByTipoEquipamentoAlcanceSimplesClasse(string idTipoEquipamento,string idTipoServico,string idUnidadeAlcance, string alcanceInf, string alcanceSup,string idClasse,string tipoPr)
        {
            if(idUnidadeAlcance =="") return 0; 
            if(alcanceInf=="") return 0; 
            if(idClasse =="") return 0; 
            if(alcanceSup =="") alcanceSup = alcanceInf; 

            try
            {
                string strSQL = "SELECT tbAlcancesSimples."+tipoPr+" FROM tbAlcancesSimples WHERE idTipoEquipamento = "+idTipoEquipamento +"  AND idTipoServico = '"+idTipoServico+"' AND A1 < = '"+clsGeral.convertDecimalSeparator(alcanceInf)+"'  AND A2 >='"+clsGeral.convertDecimalSeparator(alcanceSup)+"' AND idUnidadeAlcance ="+idUnidadeAlcance+" AND idClasse = "+idClasse; 

                return Convert.ToDouble(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
                return 0; 
            }
        
        }

        public double getPriceByTipoEquipamentoAlcancesMistos(string idTipoEquipamento, string idTipoServico, string pontosCalibracao,string alcance,string tipoPr, string quantidade, string formula)
        {
            try
            {
                string delimStr = ";"; 
                char [] delimiter = delimStr.ToCharArray();
                string [] pontos = pontosCalibracao.Split(delimiter); 

                //preciso de receber os ids
                string ids = ""; 

                switch(alcance)
                {
                    case "M": //misto
                
                        //fazer split
                        //par: retorna um array length 2 e as 2 posicoes com length >0
                        //temp: retorna um array length 2 com a primeira posicao com length =0
                        //hum: retorna um array com length 1
                        foreach (string str in pontos)
                        {
                            string delimstr = "("; 
                            char [] delim = delimstr.ToCharArray();
                            string[] s = str.Split(delim); 

                            if(s.Length == 1)//é uma humidade
                            {
                                int id = idHumidade(s[0],idTipoEquipamento, idTipoServico); 
                         
                                if(!Convert.IsDBNull(id))  
                                {
                                    ids += id.ToString()+";"; 
                                }
                            }
                            else
                            {
                               if(s[0].Length ==0) //é uma temperatura
                                {
                                    string ch = ")"; 
                                    char [] charEnd = ch.ToCharArray();

                                    string temp =  s[1].TrimEnd(charEnd); 

                                    int id = idTemperatura(temp,idTipoEquipamento, idTipoServico); 

                                    if(!Convert.IsDBNull(id))  
                                    {
                                        ids += id.ToString()+";"; 
                                    }
                                }
                                else //é um PAR
                                {           
                                    string hum = s[0].ToString(); 

                                    string ch = ")"; 
                                    char [] charEnd = ch.ToCharArray();

                                    //remove o caracter final ) no caso de ser temperatura
                                    string temp =  s[1].TrimEnd(charEnd); 
                                
                                    string strSQL = "SELECT idAlcanceMisto FROM tbAlcancesMistos  WHERE ( '"+clsGeral.convertDecimalSeparator(hum) +"'  BETWEEN H1 AND H2) OR ( '"+clsGeral.convertDecimalSeparator(hum)+ "' BETWEEN H3 AND H4)  AND ( '"+clsGeral.convertDecimalSeparator(temp) +"'  BETWEEN T1 AND T2) OR ( '"+clsGeral.convertDecimalSeparator(temp)+ "' BETWEEN T3 AND T4) AND idTipoEquipamento = "+idTipoEquipamento +" AND idTipoServico = '"+idTipoServico+"'" ; 

							   //HttpContext.Current.Response.Write(strSQL); 

                                    int id = Convert.ToInt32(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 

                                    if(!Convert.IsDBNull(id))  
                                        ids += id.ToString()+";"; 
                                }                            
                            }
                        
                            //HttpContext.Current.Response.Write("ids:"+ids+"<br />"); 
                        }
                        break; 
                    case "T"://temperatura
                        foreach (string str in pontos)
                        {
                            int id = idTemperatura(str,idTipoEquipamento, idTipoServico); 

                            if(!Convert.IsDBNull(id))  
                            {
                                ids += id.ToString()+";"; 
                            }
                        }
                        break; 
                    case "H": //humidade
                        foreach (string str in pontos)
                        {
                            int id = idHumidade(str,idTipoEquipamento, idTipoServico); 

                            if(!Convert.IsDBNull(id))  
                                ids += id.ToString()+";"; 
                        }
                        break; 
                }

                ids = ids.TrimEnd(delimiter); 
                string [] IDS = ids.Split(delimiter); 
          
                Hashtable ht = new Hashtable(); 
            
                foreach(string id in IDS)
                {
                    //adiciona tudo a uma hashtable, onde cada chave tem de ser unica
                    //fico com um par id/numero de occorencias. 
                    try
                    {
                        ht.Add(id,CountOccurrences(ids,id).ToString());  
                    }
                    catch
                    {
                        //nada, ele aqui nao adiciona os items repetidos, que é o q se pretende.
                    }
                }

                //calcula preco e devolve +para a página
                double preco = calculaPrecoMisto(ht,tipoPr,quantidade,formula); 
                return preco; 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex); 
				string strError = "Params : " + idTipoEquipamento + " " +idTipoServico + " "  + pontosCalibracao + " " + alcance + " " + tipoPr + " " +quantidade + " " + formula; 
				//GERAL.clsWriteError.WriteLog(strError); 
                return 0; 
            }
         
        }

        //recebe uma hashtable com os ids e numero de pontos em cada id, e retorna o preço
        //equivalente

        public double calculaPrecoMisto(Hashtable ht,string tipoPr,string quantidade, string formula)
        {
            double preco =0; 
            //recebe uma hashtable com o idAlcance e os numeros de pontos associados
            //por ex id= 2; num vezes q existe = 3
            //o = id; ht[o].ToString() = num de vezes
            foreach (Object o in ht.Keys) //ou ht.Keys ou ht.Values
            {
        
                int numPontos = Convert.ToInt16(ht[o]); //numero de pontos para cada idAlcance
                
                string strSQL = "SELECT * FROM tbPrecosAlcancesMistos WHERE idAlcanceMisto = "+o.ToString(); 
                DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL); 
                DataView dv = new DataView(dt); 
                dv.Sort = "p1 ASC"; //a ordenar pelo alcance mais pequeno
                
                int cont = 0; //row 0 da datatable que recebo.
                
                while(numPontos > 0)
                {
                    double preco_ = Convert.ToDouble(dt.Rows[cont][tipoPr]); 
                    int numpontos_ = Convert.ToInt16(dt.Rows[cont]["p2"]) - Convert.ToInt16(dt.Rows[cont]["p1"]) + 1; //numero de pontos que cabem no intervalos

                    //ver os pontos que ainda existem, se ja nao existem o suficiente, usar o total...
                    // nao consigo explicar melhor.
                    if(numPontos<numpontos_) numpontos_ = numPontos; 
                    
                    preco += preco_ * numpontos_; //preço unit. multiplicado pelo núm. de pontos
                
                    cont += 1; 
                    numPontos -=numpontos_; //abater os pontos ja calculados do total
                }

            }

            //este preço multiplicado pelo numero de peças:

            double numPecas =1; 
            if(quantidade!="")
            {
                numPecas = Convert.ToDouble(quantidade); 
                if(formula == "True")
                {
                    numPecas = numPecasAFacturar(numPecas); 
                }
                preco = preco*numPecas; 
            }
            
            return preco; 
        }

        //passar isto para double
        public double numPecasAFacturar(double numPecas)
        {
            int numMinPecasParaDesconto = Convert.ToInt32(ConfigurationManager.AppSettings["NUMPECASPARADESCONTO"]);

            if(numPecas >= numMinPecasParaDesconto) 
                return (((numPecas-1)/2)+1); 
            else return numPecas; 
            
            //formula = Num. Pecas a facturar = ((NumPecas -1)/2) + 1
        
        }
        //retorna o id de uma temperatura dentro de um certo alcance
        public int idTemperatura(string str, string idTipoEquipamento, string idTipoServico)
        {
            string strSQL =  "SELECT idAlcanceMisto FROM tbAlcancesMistos WHERE((H1 IS NULL) AND (H2 IS NULL) AND (H3 IS NULL) AND (H4 IS NULL)) AND (( '"+clsGeral.convertDecimalSeparator(str) +"'  BETWEEN T1 AND T2) OR ('" + clsGeral.convertDecimalSeparator(str) + "' BETWEEN T3 AND T4)) AND idTipoEquipamento = "+idTipoEquipamento +" AND idTipoServico = '"+idTipoServico+"'" ; 
            int id = Convert.ToInt32(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 
            return id; 
        }

        //retorna o id de uma temperatura dentro de um certo alcance
        public int idHumidade(string str, string idTipoEquipamento, string idTipoServico)
        {
            string strSQL = "SELECT idAlcanceMisto FROM tbAlcancesMistos WHERE idTipoEquipamento = "+idTipoEquipamento +" AND idTipoServico = '"+idTipoServico+"' AND ((T1 IS NULL) AND (T2 IS NULL) AND (T3 IS NULL) AND (T4 IS NULL)) AND (( '"+ clsGeral.convertDecimalSeparator(str) +"'  BETWEEN H1 AND H2) OR ( '"+ clsGeral.convertDecimalSeparator(str) + "' BETWEEN H3 AND H4))"; 

            int id = Convert.ToInt32(GERAL.clsDataAccess.myExecuteScalar(strSQL)); 
            return id;     
        }

        // Count the number of string occurrences
        public int CountOccurrences(string source, string search)                                  {
           
            System.Text.RegularExpressions.MatchCollection mc; 
            mc = System.Text.RegularExpressions.Regex.Matches(source, search, System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
            return mc.Count; 
        }


    }
}

