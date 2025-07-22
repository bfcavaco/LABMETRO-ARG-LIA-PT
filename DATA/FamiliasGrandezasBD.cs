using System;
using System.Data; 
using System.Data.SqlClient;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for FamiliasGrandezasBD.
	/// </summary>
	public class FamiliasGrandezasBD
	{
		public FamiliasGrandezasBD()
		{
		}


		// ==========================================================================================
		// Grandezas
		// ==========================================================================================

		// Função que devolve uma lista de Grandezas alargada em relação à função anterior com os dados 
		//do sap associados às grandazes devolvidas.
		public DataTable DTListaGrandezas()
		{
				return LabMetro.GERAL.clsDataAccess.SPExecuteDT("stpGetListGrandezas");
		
		}

		// Função que devolve uma lista de Grandezas com base nos critérios de pesquisa/ordenação
		public DataTable DTGrandezas()
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
            arrParams[0] = new SqlParameter("@inTable", "Grandeza");
			arrParams[1] = new SqlParameter("@inActiv", "0");
		
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetTableRecords", arrParams); 
 		}

        // Função que insere uma Grandeza e devolve a msg de erro
        public string InsertGrandeza(string idGrandeza, string descricao, string estado, string username,  string idCodigoServico)
        {
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);
			arrParams[4] = new SqlParameter("@inIdCodigoServico", idCodigoServico);

            //nao apan
			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsGrandeza",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
        }

		// Função que actualiza uma Grandeza e devolve a msg de erro
		//depois de preenchidos, já não podem ser alterados, tem de se fazer disable a esta funcionalidade
		public string UpdateGrandezas(string idGrandeza, string descricao, string estado, string username, string idCodigoServico)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);
			arrParams[4] = new SqlParameter("@inIdCodigoServico", idCodigoServico);
			
			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdGrandeza",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}

		// ==========================================================================================
		// Famílias
		// ==========================================================================================

		// Função que devolve uma lista de Famílias com base nos critérios de pesquisa/ordenação
		public DataTable DTFamilias(string idGrandeza, string descricao)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListFamilias", arrParams); 
        }
		
		
		// Função que devolve um datareader com a familia por grandeza
		public SqlDataReader DRFamilias(string idGrandeza)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
			
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListFamilias", arrParams); 
		}

        // Função que insere uma Familia de Equipamentos e devolve a msg de erro
        public string InsertFamilia(string idGrandeza, string descricao, string estado, string username)
        {          
			SqlParameter[] arrParams = new SqlParameter[4];

			arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsFamilia",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
		}

		// Função que actualiza uma Familia de Equipamentos e devolve a msg de erro
        public string UpdateFamilia(string idFamilia, string idGrandeza, string descricao, string estado, string username)
        {
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inIdFamilia", idFamilia);
			arrParams[1] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[2] = new SqlParameter("@inDescricao", descricao);
			arrParams[3] = new SqlParameter("@inActivo", estado);
			arrParams[4] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdFamilia",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
        }

        public SqlDataReader drListaFamilias()

        {
            string strSQL = "Select idFamilia, descricao as familia from familia order by descricao";

            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
        }
	}
}
