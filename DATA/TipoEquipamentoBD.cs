using System;
using System.Data;
using System.Data.SqlClient;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for TipoEquipamentoBD.
	/// </summary>
	public class TipoEquipamentoBD
	{
		public TipoEquipamentoBD()
		{
		}

		// Função que devolve uma lista de Tipos de Equipamentos com base nos critérios de pesquisa/ordenação
		public DataTable DTTipoEquipamento(string idFamilia, string codigo, string descricao, string idGrandeza)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
            
			arrParams[0] = new SqlParameter("@inActiv", ""); // Todos os Tipos de Equipamentos
			arrParams[1] = new SqlParameter("@inIdFamilia", idFamilia);
			arrParams[2] = new SqlParameter("@inCodigo", codigo);
			arrParams[3] = new SqlParameter("@inDescricao", descricao);
            arrParams[4] = new SqlParameter("@idGrandeza", idGrandeza);
         
			DataTable tipoEquipamentoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListTiposEquipamentos", arrParams); 
  
			return tipoEquipamentoDT;   
        }

		// Função que devolve uma lista de Tipos de Equipamentos com base nos critérios de pesquisa/ordenação (igual à anterior mas devolve um datareader)
		public SqlDataReader DRTipoEquipamento(string sortField, string sortDirection, string idFamilia, string codigo, string descricao, string activo)
		{
			SqlParameter[] arrParams = new SqlParameter[6];
            
			arrParams[0] = new SqlParameter("@inActiv", activo);
			arrParams[1] = new SqlParameter("@inSortField", sortField);
			arrParams[2] = new SqlParameter("@inSortDirection", sortDirection);
			arrParams[3] = new SqlParameter("@inIdFamilia", idFamilia);
			arrParams[4] = new SqlParameter("@inCodigo", codigo);
			arrParams[5] = new SqlParameter("@inDescricao", descricao);
         
			SqlDataReader tipoEquipamentoDR = LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListTiposEquipamentos", arrParams); 
  
			return tipoEquipamentoDR;   
		}

		public SqlDataReader DRTipoEquipamento()
		{
			return DRTipoEquipamento("", "", "", "", "", "");   
		}

		// Função que insere um Tipo de Equipamento e devolve a msg de erro
		public string InsertTipoEquipamento(string idFamilia, string codigo, string descricao, string acreditado, string simbolo, string estado, string username)
		{          
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inIdFamilia", idFamilia);
			arrParams[1] = new SqlParameter("@inCodigo", codigo);
			arrParams[2] = new SqlParameter("@inDescricao", descricao);
			arrParams[3] = new SqlParameter("@inAcreditado", acreditado);
			arrParams[4] = new SqlParameter("@inSimbolo", simbolo.Trim());
			arrParams[5] = new SqlParameter("@inActivo", estado);
			arrParams[6] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsTipoEquipamento",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
        }

		// Função que actualiza um Tipo de Equipamento e devolve a msg de erro
		public string UpdateTipoEquipamento(string idTipoEquipamento, string idFamilia, string codigo, string descricao, string acreditado, string simbolo, string estado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[8];
 
			arrParams[0] = new SqlParameter("@inIdTipoEquipamento", idTipoEquipamento);
			arrParams[1] = new SqlParameter("@inIdFamilia", idFamilia);
			arrParams[2] = new SqlParameter("@inCodigo", codigo);
			arrParams[3] = new SqlParameter("@inDescricao", descricao);
			arrParams[4] = new SqlParameter("@inAcreditado", acreditado);
			arrParams[5] = new SqlParameter("@inSimbolo", simbolo.Trim());
			arrParams[6] = new SqlParameter("@inActivo", estado);
			arrParams[7] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdTipoEquipamento",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}
	}
}
