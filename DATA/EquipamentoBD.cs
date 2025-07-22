using System;
using System.Data; 
using System.Data.SqlClient;
using System.Web; 
using System.Configuration; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for EquipamentoBD.
	/// </summary>
	///  
	public class EquipmentDetails
	{
		public string idEquipamento;
		public string idGrandeza; 
		public string idEmpresa; 
		public string nomeEmpresa;
		public string idTipoEquipamento; 
		public string tipoEquipamento;
		public string numSerie;  
		public string numIdentificacao; 
		public string alcanceInf;
        public string alcanceSup; 
        public string idUnidadeAlcance;
        public string alcance; 
		public string resolucao; 
		public string idmarca; 
		public string idmodelo; 
        public string idClasse; 
		public string classe; 
		public string forma; 
		public string fabricante; 
		public string refUltimaCalibracao; 
		public string dtUltimaCalibracao; 
		public string periodicidadeCalibracao; 
		public string activo;
		public string observacoes; 
		public string calibInt;
		public string certConclusivo;
		public string campo1; 
		public string campo2;
        public string marca;
        public string modelo;
        public string etiqueta1;
        public string etiqueta2;
        public string etiqueta3;
        public string criterios;
        public string codigoIPQ;
        public string idEstadoRelacaoCalibracao;
        
	}

	public class EquipamentoBD
	{
		public EquipamentoBD()
		{
		}

		// Funcao que devolve os detalhes de um Equipamento com base no seu ID
		public EquipmentDetails GetEquipmentDetails(string idEquipamento)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEquipamento", idEquipamento);
         
			DataTable EquipamentoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEquipamentoById", arrParams); 
           
			if(EquipamentoDT.Rows.Count > 0) 
			{ 

				EquipmentDetails myEquipmentDetails= new EquipmentDetails(); 
				myEquipmentDetails.idEquipamento = EquipamentoDT.Rows[0]["idEquipamento"].ToString();
				myEquipmentDetails.idEmpresa = EquipamentoDT.Rows[0]["idEmpresa"].ToString();
				myEquipmentDetails.nomeEmpresa = EquipamentoDT.Rows[0]["nomeEmpresa"].ToString();
				myEquipmentDetails.idTipoEquipamento = EquipamentoDT.Rows[0]["idTipoEquipamento"].ToString();
				myEquipmentDetails.tipoEquipamento = EquipamentoDT.Rows[0]["tipoEquipamento"].ToString();
				myEquipmentDetails.numSerie = EquipamentoDT.Rows[0]["numSerie"].ToString();
				myEquipmentDetails.numIdentificacao = EquipamentoDT.Rows[0]["numIdentificacao"].ToString();
				myEquipmentDetails.alcanceInf=  EquipamentoDT.Rows[0]["alcanceInf"].ToString();
				myEquipmentDetails.alcanceSup=  EquipamentoDT.Rows[0]["alcanceSup"].ToString();
				myEquipmentDetails.idUnidadeAlcance=  EquipamentoDT.Rows[0]["idUnidadeAlcance"].ToString();
				myEquipmentDetails.alcance = EquipamentoDT.Rows[0]["alcance"].ToString(); 
				myEquipmentDetails.resolucao= EquipamentoDT.Rows[0]["resolucao"].ToString();
				myEquipmentDetails.marca = EquipamentoDT.Rows[0]["marca"].ToString();
				myEquipmentDetails.modelo = EquipamentoDT.Rows[0]["modelo"].ToString();
                myEquipmentDetails.idmarca = EquipamentoDT.Rows[0]["idmarca"].ToString();
                myEquipmentDetails.idmodelo = EquipamentoDT.Rows[0]["idmodelo"].ToString();

				myEquipmentDetails.idClasse=  EquipamentoDT.Rows[0]["idClasse"].ToString();
				myEquipmentDetails.classe = EquipamentoDT.Rows[0]["classe"].ToString(); 
				myEquipmentDetails.forma = EquipamentoDT.Rows[0]["forma"].ToString();
				myEquipmentDetails.fabricante = EquipamentoDT.Rows[0]["fabricante"].ToString();
				myEquipmentDetails.refUltimaCalibracao = EquipamentoDT.Rows[0]["refUltimaCalibracao"].ToString();
				myEquipmentDetails.dtUltimaCalibracao = EquipamentoDT.Rows[0]["dtUltimaCalibracao"].ToString();
				myEquipmentDetails.periodicidadeCalibracao = EquipamentoDT.Rows[0]["periodicidadeCalibracao"].ToString();
				myEquipmentDetails.activo= GERAL.clsGeral.ConvertStringToBool(EquipamentoDT.Rows[0]["activo"].ToString()).ToString();
				myEquipmentDetails.observacoes = EquipamentoDT.Rows[0]["observacoes"].ToString();
				myEquipmentDetails.idGrandeza = EquipamentoDT.Rows[0]["idGrandeza"].ToString();
				myEquipmentDetails.calibInt = EquipamentoDT.Rows[0]["calibInt"].ToString();
				myEquipmentDetails.certConclusivo = EquipamentoDT.Rows[0]["bcertConclusivo"].ToString();
				myEquipmentDetails.campo1 = EquipamentoDT.Rows[0]["campo1"].ToString();
				myEquipmentDetails.campo2 = EquipamentoDT.Rows[0]["campo2"].ToString();
                myEquipmentDetails.etiqueta1 = EquipamentoDT.Rows[0]["etiqueta1"].ToString();
                myEquipmentDetails.etiqueta2 = EquipamentoDT.Rows[0]["etiqueta2"].ToString();
                myEquipmentDetails.etiqueta3 = EquipamentoDT.Rows[0]["etiqueta3"].ToString();
                myEquipmentDetails.criterios = EquipamentoDT.Rows[0]["criterios"].ToString();
                myEquipmentDetails.codigoIPQ = EquipamentoDT.Rows[0]["codigoIPQ"].ToString();
                myEquipmentDetails.idEstadoRelacaoCalibracao = EquipamentoDT.Rows[0]["idEstadoRelacaoCalibracao"].ToString();


				return myEquipmentDetails;
			}
			else
			{
				return null; 
			}
		}

		public DataTable DTEquipamentobyIDBO(string idEquipamento)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdEquipamento", idEquipamento);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEquipamentoByIDBO", arrParams); 

		}

	
		// Retorna DataTable com equipamentos --- ListaEquipamentos.aspx
		public DataTable DTEquipamento(string idEmpresa,string tipoEquipamento, string numSerie, string numIdentificacao, object AtivoInativo = null)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento);
			arrParams[2] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[3] = new SqlParameter("@inNumIdentificacao", numIdentificacao);
            arrParams[4] = new SqlParameter("@Estado", AtivoInativo?.ToString());


            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListEquipamentos", arrParams); 
           
		}

        // Retorna DataTable com equipamentos --- ListaEquipamentos.aspx
        public DataTable DTEquipamentoForIgae(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[1];

            arrParams[0] = new SqlParameter("@idEmpresa", idEmpresa);
        
            return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListEquipamentosIgae", arrParams);

        }

		// Retorna DataTable com equipamentos --- ListaEquipamentos.aspx
		public DataTable DTEquipamentoBO(string idEmpresa,string tipoEquipamento, string numSerie, string numIdentificacao, string tipoPesquisa, string bfiltro, string grandeza, string familia, string refUltEntrada, string idEquipamento )
		{
			SqlParameter[] arrParams = new SqlParameter[10];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento.Trim());
			arrParams[2] = new SqlParameter("@inNumSerie", numSerie.Trim());
			arrParams[3] = new SqlParameter("@inNumIdentificacao", numIdentificacao.Trim());
			arrParams[4] = new SqlParameter("@inTipoPesquisa", tipoPesquisa);
			arrParams[5] = new SqlParameter("@inbFiltro", bfiltro);
			arrParams[6] = new SqlParameter("@inGrandeza", grandeza);
			arrParams[7] = new SqlParameter("@inFamilia", familia);
			arrParams[8] = new SqlParameter("@inUltServico", refUltEntrada);
			arrParams[9] = new SqlParameter("@inIdEquipamento", idEquipamento);
       
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListEquipamentosBO", arrParams); 
           
           
		}


		// Retorna DataTable com equipamentos
		public DataTable DTEquipamentoBOCONC(string idEmpresa,string tipoEquipamento, string numSerie, string numIdentificacao,string grandeza, string refUltEntrada, string idEquipamento )
		{
			SqlParameter[] arrParams = new SqlParameter[7];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inTipoEquipamento", tipoEquipamento.Trim());
			arrParams[2] = new SqlParameter("@inNumSerie", numSerie.Trim());
			arrParams[3] = new SqlParameter("@inNumIdentificacao", numIdentificacao.Trim());
			arrParams[4] = new SqlParameter("@inGrandeza", grandeza);
			arrParams[5] = new SqlParameter("@inUltServico", refUltEntrada);
			arrParams[6] = new SqlParameter("@inIdEquipamento", idEquipamento);
       
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListEquipamentosBOCONC", arrParams); 
           
           
		}

	

		// Função que devolve a Grandeza de um dado Tipo de Equipamento
		public string GetGrandeza(string idTipoEquipamento)
		{
			string strSQL = "SELECT dbo.udfGetIdGrandezaByIdTipoEquipamento(" + idTipoEquipamento + ")"; 
			return (string)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
		}


		//====================================================================================

		// Função nova - transacção
		// faz insert e update do equipamento
		// escreve na tabela de auditoria
		// no insert, retorna uma string com o num id do equipamento, em todos os outros casos
		// retorna uma msg de erro/confirmação.
		// faz também as validações se o equipamento já existe ou não. 

		//====================================================================================
		public string InsertUpdateEquipamento(string idEquipamento,string idEmpresa, string idTipoEquipamento, string numSerie, string numIdentificacao, string alcanceInf, string alcanceSup, string idAlcance, string alcance, string resolucao, string modelo, string marca, string idClasse, string classe,string forma, string fabricante, string refUltimaCalibracao, string dtUltimaCalibracao, string periodicidadeCalibracao,string activo, string observacoes, string operation,string strCalibInt, string certConclusivo, string campo1, string campo2, string etiqueta1, string etiqueta2, string etiqueta3, string criterios  )
		{

			string retMsg =""; 

			string UserId = HttpContext.Current.Session["UserId"].ToString(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				
				objCmd.Connection = objConn; 

				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try
					{
				
						objCmd.CommandType = CommandType.Text; 
				
						//verificar se ja existe equipamento com a mesma identificao
						if(validaEquipamento(objConn,objCmd,idEmpresa,idEquipamento, idTipoEquipamento,numSerie,numIdentificacao,operation,marca, modelo) == false)
						{

							objTrans.Rollback(); 
							return "Já existe um equipamento do mesmo tipo com a mesma identificação (núm.Série / núm. Ident) dentro da mesma empresa."; 
						}
						else 
						{
							SqlParameter[] arrParamsEquipamento; 
							
							if(operation == "insert")
							{

								arrParamsEquipamento = arrParamsEquipamentoInsert(idEmpresa, idTipoEquipamento, numSerie,  numIdentificacao,  alcanceInf,  alcanceSup,  idAlcance,  alcance,  resolucao,  modelo,  marca,  idClasse,  classe, forma,  fabricante,  refUltimaCalibracao,  dtUltimaCalibracao,	  periodicidadeCalibracao, observacoes, strCalibInt, certConclusivo, campo1, campo2, etiqueta1, etiqueta2, etiqueta3, criterios  );

								objCmd.CommandType = CommandType.StoredProcedure; 

								retMsg =  GERAL.clsDataAccess.ExecuteNonQuerySPConn_retExNbr(objConn, objCmd,"stpInsertEquipamento",arrParamsEquipamento).ToString(); 

								//no update, se o num id ou o numSerie nao sao alterados, ele tb nao escreve no historico, por isso
                                if (retMsg == "2") retMsg = Resources.Resource.MSG_INSERT_DB; 
								if(retMsg =="8114") retMsg = "O erro pode ser causado pelo formato incorrecto da data. formato correcto: dd/mm/aaaa."; 
										
							}
							else if(operation =="update")
							{
								arrParamsEquipamento = arrParamsEquipamentoUpdate(idEquipamento,idEmpresa, idTipoEquipamento, numSerie,  numIdentificacao,  alcanceInf,  alcanceSup,  idAlcance,  alcance,  resolucao,  modelo,  marca,  idClasse,  classe, forma,  fabricante,  refUltimaCalibracao,  dtUltimaCalibracao,	  periodicidadeCalibracao,activo, observacoes,strCalibInt, certConclusivo, campo1, campo2, etiqueta1, etiqueta2, etiqueta3, criterios );

								objCmd.CommandType = CommandType.StoredProcedure; 
								retMsg =  GERAL.clsDataAccess.ExecuteNonQuerySPConn_retExNbr(objConn, objCmd,"stpUpdateEquipamento",arrParamsEquipamento).ToString(); 
								
								//2 porque insere uma vez na tabela equipamento e outra na tabela historico. 
								//aqui tb pode devolver apenas 1
                                if (retMsg == "1" || retMsg == "2") retMsg = Resources.Resource.MSG_UPDATE_DB;						
								if(retMsg =="8114") retMsg = "O erro pode ser causado pelo formato incorrecto da data. formato correcto: dd/mm/aaaa."; 

							}
							objTrans.Commit(); 
							return retMsg; 
						}
					}
					catch(Exception ex)
					{ 	
						if(operation =="insert")
						{
							retMsg = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
						}
						else if(operation =="update")
						{
							retMsg = GERAL.clsGeral.ErrorMessage.ERR_UPDATE; 
						}
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception exep)
						{
							GERAL.clsWriteError.WriteLog(exep); 
						}

						GERAL.clsWriteError.WriteLog(ex); 
						return retMsg; 
					}
				}
			}
		}



		//valida se o num de serie ou num de identificao ja existem para o tipo de equipamento e a empresa, isto pode ser alargado tb para a marca e o modelo, se necessario

		public bool validaEquipamento(SqlConnection objConn, SqlCommand objCmd, string idEmpresa, string idEquipamento, string idTipoEquipamento, string numSerie, string numIdentificacao, string operation, string marca, string modelo)
		{
			objCmd.CommandType = CommandType.Text; 

			//verificar se ja existe algum equipamento com o mesmo num serie ou num id
			string strSQLcmdGetEquipment = "SELECT idEquipamento,numSerie,numIdentificacao,marca FROM Equipamento WHERE idEmpresa = "+idEmpresa+" AND idTipoEquipamento = "+ idTipoEquipamento;
			if(marca!="")
			{
				strSQLcmdGetEquipment += " AND Marca = '"+marca+"' "; 
			}
//			if(modelo!="")
//			{
//				strSQLcmdGetEquipment += " AND MODELO = '"+modelo+"' ";
//			}
										  
			DataTable dt; 
			
			try
			{
				dt = GERAL.clsDataAccess.ExecuteDT(objConn,objCmd,strSQLcmdGetEquipment);
			}
			catch(Exception ex)

			{
				GERAL.clsWriteError.WriteLog(ex.ToString());
				return false; 
			}

			int rows; //resultados dos rows conforme as queries

			//==================================================
			//numSerie, numIdentificacao
			//nao pode haver numeros repetidos dentro da datatable 
			//nem no numId, nem no num de serie
			//mas tenho de excluir os "---" que podem ser iguais.
			//==================================================
			string filterExpr;
			
			//FAZER DE NOVO
			//testar um por um para nao haver ands e  ors a atrapalhar


			//se tudo ficou igual, ficar-se por aqui

			if(operation =="update")
			{
				//se o id ficou igual e o numId e numSerie tb, entao está td bem.
				string filtro = "idEquipamento = "+idEquipamento + " AND numSerie =  '"+numSerie+"' AND numIdentificacao ='"+numIdentificacao+"' AND marca ='"+marca+"'"; 

				DataRow[] r = dt.Select(filtro);
							
				if (r.Length == 1) 
				{
					return true; //sair da funcao
				}
			}

			if(numSerie!="---" && numSerie !="") //numero de serie está preenchido
			{
				filterExpr = "numSerie ='"+numSerie.ToString()+"'"; 
				if(idEquipamento != "") filterExpr += " AND idEquipamento <> "+idEquipamento;  
				DataRow[] dr = dt.Select(filterExpr); 
				rows = dr.Length; 
				if(rows >0) 
				{
					return false; //ja existe um numero de serie igual para um outro equipamento do mesmo tipo
				}
			}
			
			if(numIdentificacao!="---"&& numIdentificacao !="") //numero de serie está preenchido
			{
				filterExpr = "numIdentificacao ='"+numIdentificacao.ToString()+"'"; // AND idEquipamento <> "+idEquipamento;  
				if(idEquipamento != "") filterExpr += " AND idEquipamento <> "+idEquipamento;  
				DataRow[] dr = dt.Select(filterExpr); 
				rows = dr.Length; 
				if(rows >0) return false; //ja existe um numero de identificacao igual para um outro equipamento do mesmo tipo

			}
			return true; //se chegou ate aqui, deve estar tudo bem 
		}
		
		public SqlParameter[] arrParamsAuditoria(string inActionType,string inTableName, string inTableIds, string userName)
		{
			SqlParameter[] arrParams = new SqlParameter[4];
			
			arrParams[0] = new SqlParameter("@inActionType", inActionType);
			arrParams[1] = new SqlParameter("@inTableName ", inTableName);
			arrParams[2] = new SqlParameter("@inTableIds", inTableIds);
			arrParams[3] = new SqlParameter("@inUsername", userName);
			
			return arrParams; 
		}

		public SqlParameter[] arrParamsEquipamentoInBre(string idEmpresa,string idTipoEquipamento, string numSerie, string numIdentificacao, string userId)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
			
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[2] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[3] = new SqlParameter("@inNumIdentificacao", numIdentificacao);
			arrParams[4] = new SqlParameter("@inUserId", userId);
			
			return arrParams; 
		}

		public SqlParameter[] arrParamsEquipamentoInsert(string idEmpresa, string idTipoEquipamento, string numSerie, string numIdentificacao, string alcanceInf, string alcanceSup, string idAlcance, string alcance, string resolucao, string modelo, string marca, string idClasse, string classe,string forma, string fabricante, string refUltimaCalibracao, string dtUltimaCalibracao, string periodicidadeCalibracao, string observacoes,string strCalibInt,string certConclusivo,string campo1,string campo2, string etiqueta1, string etiqueta2, string etiqueta3, string criterios )
		{
		
			SqlParameter[] arrParams = new SqlParameter[28];

			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[2] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[3] = new SqlParameter("@inNumIdentificacao", numIdentificacao);
            
			arrParams[4] = new SqlParameter("@inAlcanceInf", GERAL.clsGeral.convertDecimalSeparator(alcanceInf));
			arrParams[5] = new SqlParameter("@inAlcanceSup", GERAL.clsGeral.convertDecimalSeparator(alcanceSup));
			arrParams[6] = new SqlParameter("@inIdAlcance", idAlcance);
			
			arrParams[7] = new SqlParameter("@inAlcance", alcance);
			arrParams[8] = new SqlParameter("@inResolucao", resolucao);
			arrParams[9] = new SqlParameter("@inModelo",modelo);
			arrParams[10] = new SqlParameter("@inMarca", marca);
			arrParams[11] = new SqlParameter("@inIdClasse", idClasse); 
			arrParams[12] = new SqlParameter("@inClasse", classe);
			arrParams[13] = new SqlParameter("@inForma", forma);
			arrParams[14] = new SqlParameter("@inFabricante", fabricante);
			arrParams[15] = new SqlParameter("@inRefUltimaCalibracao", refUltimaCalibracao);
			arrParams[16] = new SqlParameter("@inDtUltimaCalibracao", dtUltimaCalibracao);

			arrParams[17] = new SqlParameter("@inPeriodicidadeCalibracao", periodicidadeCalibracao);
			arrParams[18] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[19] = new SqlParameter("@inUserId", HttpContext.Current.Session["UserId"].ToString());
			arrParams[20] = new SqlParameter("@inCalibInt", strCalibInt);
			arrParams[21] = new SqlParameter("@certConclusivo", certConclusivo);
			arrParams[22] = new SqlParameter("@campo1", campo1);
			arrParams[23] = new SqlParameter("@campo2", campo2);
            arrParams[24] = new SqlParameter("@etiqueta1", etiqueta1);
            arrParams[25] = new SqlParameter("@etiqueta2", etiqueta2);
            arrParams[26] = new SqlParameter("@etiqueta3", etiqueta3);
            arrParams[27] = new SqlParameter("@criterios", criterios);
			return arrParams; 

	}

        public SqlParameter[] arrParamsEquipamentoUpdate(string idEquipamento, string idEmpresa, string idTipoEquipamento, string numSerie, string numIdentificacao, string alcanceInf, string alcanceSup, string idAlcance, string alcance, string resolucao, string modelo, string marca, string idClasse, string classe, string forma, string fabricante, string refUltimaCalibracao, string dtUltimaCalibracao, string periodicidadeCalibracao, string activo, string observacoes, string strCalibInt, string certConclusivo, string campo1, string campo2, string etiqueta1, string etiqueta2, string etiqueta3, string criterios)
		{
		
			SqlParameter[] arrParams = new SqlParameter[30];
 
			arrParams[0] = new SqlParameter("@inIdEquipamento", idEquipamento);
			arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[2] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[3] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[4] = new SqlParameter("@inNumIdentificacao", numIdentificacao);

			arrParams[5] = new SqlParameter("@inAlcanceInf", GERAL.clsGeral.convertDecimalSeparator(alcanceInf));
			arrParams[6] = new SqlParameter("@inAlcanceSup", GERAL.clsGeral.convertDecimalSeparator(alcanceSup));
			arrParams[7] = new SqlParameter("@inIdAlcance", idAlcance);            
			arrParams[8] = new SqlParameter("@inAlcance", alcance);
			arrParams[9] = new SqlParameter("@inResolucao", resolucao);
			arrParams[10] = new SqlParameter("@inModelo", modelo);
			arrParams[11] = new SqlParameter("@inMarca", marca);
			arrParams[12] = new SqlParameter("@inIdClasse", idClasse);
			arrParams[13] = new SqlParameter("@inClasse", classe);
			arrParams[14] = new SqlParameter("@inForma", forma);
			arrParams[15] = new SqlParameter("@inFabricante", fabricante);
			arrParams[16] = new SqlParameter("@inRefUltimaCalibracao", refUltimaCalibracao);
			arrParams[17] = new SqlParameter("@inDtUltimaCalibracao", dtUltimaCalibracao);
			arrParams[18] = new SqlParameter("@inPeriodicidadeCalibracao", periodicidadeCalibracao);
			arrParams[19] = new SqlParameter("@inActivo", activo);
			arrParams[20] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[21] = new SqlParameter("@inUserId", HttpContext.Current.Session["UserId"].ToString());
			arrParams[22] = new SqlParameter("@inCalibInt", strCalibInt);
			arrParams[23] = new SqlParameter("@certConclusivo", certConclusivo);
			arrParams[24] = new SqlParameter("@campo1", campo1);
			arrParams[25] = new SqlParameter("@campo2", campo2);
            arrParams[26] = new SqlParameter("@etiqueta1", etiqueta1);
            arrParams[27] = new SqlParameter("@etiqueta2", etiqueta2);
            arrParams[28] = new SqlParameter("@etiqueta3", etiqueta3);
             arrParams[29] = new SqlParameter("@criterios", criterios);

			return arrParams; 
		}

		//OVERLAOD
        // Função que insere um Equipamento (quick insert) recebendo apenas 3 campos 
        //o return value nao deve ser tratado no codigo.
        public string InsertEquipmentinBre(string idEmpresa, string idTipoEquipamento, string numSerie, string numIdentificacao,string marca, string modelo)
        {
			string retMsg =""; 

			string userId = HttpContext.Current.Session["UserId"].ToString(); 

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using (SqlCommand objCmd = new SqlCommand())
			{

				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
				
				objCmd.Connection = objConn; 
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())			
				{
					objCmd.Transaction =objTrans; 
					
					try
					{
						
						//verificar se ja existe equipamento com a mesma identificao
						objCmd.CommandType = CommandType.Text; 

						if(validaEquipamento(objConn,objCmd,idEmpresa,"",idTipoEquipamento,numSerie,numIdentificacao,"insert",marca, modelo) == false)
						{
							objTrans.Rollback(); 

							return "Já existe um equipamento c/ o mesmo núm.Série e/ou núm. Ident dentro da mesma empresa e marca."; 
						}
						else 
						{
							
							SqlParameter[] arrParams; 
						
							objCmd.CommandType = CommandType.StoredProcedure; 

							arrParams = arrParamsEquipamentoInBre(idEmpresa,idTipoEquipamento, numSerie,numIdentificacao,userId); 

							GERAL.clsDataAccess.ExecuteNonQuerySP(objConn, objCmd,"stpInsertEquipamentoInBRE",arrParams);
							
							objCmd.CommandType = CommandType.Text; 

							objCmd.CommandText = "SELECT @@IDENTITY"; //retorna um decimal

							int idEquipamento= Decimal.ToInt32((decimal)objCmd.ExecuteScalar()); 
							
							if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();

							arrParams = arrParamsAuditoria("I","Equipamento",idEquipamento.ToString(),HttpContext.Current.User.Identity.Name.ToString()); 
							
							objCmd.CommandType = CommandType.StoredProcedure; 

							GERAL.clsDataAccess.ExecuteNonQuerySP(objConn,objCmd, "stpRegistaAuditoria",arrParams);

                            retMsg = Resources.Resource.MSG_DB; 

							}
							objTrans.Commit(); 
							return retMsg; 				
					}
					
					catch(Exception ex)
					{ 	
						
						retMsg = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
							
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception exep)
						{
							GERAL.clsWriteError.WriteLog(exep); 
						}

						GERAL.clsWriteError.WriteLog(ex); 
						
						return retMsg; 
					}
				}
			}
		}


		//overload com +1 parametro: numSerie
		public DataTable DTGetEquipamentosActivosByEmpresa(string idEmpresa,string tipoEquipamento,string refUltimaCalibracao,string numIdentificacao,string numSerie,string idTipoEquipamento)
		{
			SqlParameter[] arrParams = new SqlParameter[6];
            
			arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
			arrParams[1] = new SqlParameter("@inTipoEquipamento", "");
			arrParams[2] = new SqlParameter("@inRefUltimaCalibracao", refUltimaCalibracao);
			arrParams[3] = new SqlParameter("@inNumIdentificacao", numIdentificacao);
			arrParams[4] = new SqlParameter("@inNumSerie", numSerie);
		    arrParams[5] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);	

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEquipamentosActivosByEmpresa", arrParams); 
           
			
		}
		public DataTable DTEmpresasActivas()
		{
			return GERAL.clsDataAccess.SPExecuteDT("stpGetEmpresasActivas"); 
		}

		

		
		public int updateEquipamentoInBO(string idTipoEquipamento ,string numSerie, string numIdentificacao, string periodCalib, string refUltCalib, string dtUltCalib, string userID, string idEquipamento)
		{
			SqlParameter[] arrParams = new SqlParameter[8];



			arrParams[0] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[1] = new SqlParameter("@inNumSerie", numSerie);
			arrParams[2] = new SqlParameter("@inNumIdentificacao", numIdentificacao);
			arrParams[3] = new SqlParameter("@inRefUltimaCalibracao", refUltCalib);
			arrParams[4] = new SqlParameter("@inDtUltimaCalibracao", dtUltCalib);
			arrParams[5] = new SqlParameter("@inPeriodicidadeCalibracao", periodCalib);
			arrParams[6] = new SqlParameter("@inUserId", userID);
			arrParams[7] = new SqlParameter("@inIdEquipamento", idEquipamento);
			
			return GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdEquipamentoInBO", arrParams); 
			
		}


		public int updateEquipamentoInBOConc(string periodCalib, string refUltCalib, string dtUltCalib, string userID, string idEquipamento,string obs)
		{
			SqlParameter[] arrParams = new SqlParameter[6];
			arrParams[0] = new SqlParameter("@inIdEquipamento", idEquipamento);
			arrParams[1] = new SqlParameter("@inRefUltimaCalibracao", refUltCalib);
			arrParams[2] = new SqlParameter("@inDtUltimaCalibracao", dtUltCalib);
			arrParams[3] = new SqlParameter("@inPeriodicidadeCalibracao", periodCalib);
			arrParams[4] = new SqlParameter("@inUserId", userID);
			arrParams[5] = new SqlParameter("@obs", obs);
			
			
			return GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdEquipamentoBOConc", arrParams); 
			
		}
		
	}
}
