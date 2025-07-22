using System;
using System.Data; 
using System.Data.SqlClient;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for TabelasBD.
	/// </summary>
	public class TabelasBD
	{
		public TabelasBD()
		{
		}

		// Função que devolve uma lista de Registos com base nos critérios de pesquisa/ordenação
		public DataTable DTTabela(string tableName)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inTable", tableName);
			arrParams[1] = new SqlParameter("@inActiv", "0");
			
         
			DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetTableRecords", arrParams); 
  
			return DT;
        }

		// Função que insere um Registo na Tabela especificada e devolve a msg de erro
		public string InsertTable(string tableName, string descricao, string estado, string username)
		{          
			SqlParameter[] arrParams = new SqlParameter[4];

			arrParams[0] = new SqlParameter("@inTable", tableName);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsTableRecords",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
        }

		// Função que actualiza um Registo na Tabela especificada e devolve a msg de erro
		public string UpdateTable(string tableName, string id, string descricao, string estado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inTable", tableName);
			arrParams[1] = new SqlParameter("@inId", id);
			arrParams[2] = new SqlParameter("@inDescricao", descricao);
			arrParams[3] = new SqlParameter("@inActivo", estado);
			arrParams[4] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdTableRecords",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
        }

		// Função que devolve uma lista de Países com base nos critérios de pesquisa/ordenação
		public DataTable DTTabelaPais()
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inTable", "Pais");
			arrParams[1] = new SqlParameter("@inActiv", "0");
			
			DataTable PaisDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetTableRecords", arrParams); 
  
			return PaisDT;
		}

		// Função que insere um Pais e devolve a msg de erro
		public string InsertPais(string idPais, string descricao, string estado, string username)
		{          
			SqlParameter[] arrParams = new SqlParameter[4];

			arrParams[0] = new SqlParameter("@inIdPais", idPais);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsPais",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
		}

		// Função que actualiza um Pais e devolve a msg de erro
		public string UpdatePais(string idPais, string descricao, string estado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[4];
 
			arrParams[0] = new SqlParameter("@inIdPais", idPais);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdPais",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}

		// Função que devolve uma lista de Tipos de Certificados com base nos critérios de pesquisa/ordenação
		public DataTable DTTabelaTipoCertificado()
		{
			SqlParameter[] arrParams = new SqlParameter[4];
                    
			DataTable TipoCertificadoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDT("stpGetTipoCertificado"); 
  
			return TipoCertificadoDT;
		}

		// Função que insere um TipoCertificado e devolve a msg de erro
		public string InsertTipoCertificado(string descricao, string sigla, string estado, string username)
		{          
			SqlParameter[] arrParams = new SqlParameter[4];

			arrParams[0] = new SqlParameter("@inDescricao", descricao);
			arrParams[1] = new SqlParameter("@inSigla", sigla);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsTipoCertificado",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
		}

		// Função que actualiza um TipoCertificado e devolve a msg de erro
		public string UpdateTipoCertificado(string idTipoCertificado, string descricao, string sigla, string estado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inIdTipoCertificado", idTipoCertificado);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inSigla", sigla);
			arrParams[3] = new SqlParameter("@inActivo", estado);
			arrParams[4] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdTipoCertificado",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}
	}
}
