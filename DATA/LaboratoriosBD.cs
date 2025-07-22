using System;
using System.Data; 
using System.Data.SqlClient;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for LaboratoriosBD.
	/// </summary>
	public class LaboratoriosBD
	{
		public LaboratoriosBD()
		{
		}

		// Função que devolve uma lista de Laboratórios (Centros de Custo)
		//so quero os activos. DM 12-12-2006 (nao adianta apagar um laboratorio para criar um novo com um novo centro de custo
		//a propria stored procedure devolve apenas os activos pois os inactivos nao interessam para nada!
		public DataTable DTLaboratorios()
		{
			
			return  LabMetro.GERAL.clsDataAccess.SPExecuteDT("stpGetListLaboratorios"); 
  
        }

		// Função que insere um Laboratório (Centro de Custo) e devolve a msg de erro
		public string InsertLaboratorio(string idGrandeza, string idLocalLaboratorio, string codigoCentroCusto, string activo, string username, string idCodigoPEP, string idCodigoRegiaoVendas)
		{          
			SqlParameter[] arrParams = new SqlParameter[7];

			arrParams[0] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[1] = new SqlParameter("@inIdLocalLaboratorio", idLocalLaboratorio);
			arrParams[2] = new SqlParameter("@inCodigoCentroCusto", codigoCentroCusto);
			arrParams[3] = new SqlParameter("@inActivo", activo);
			arrParams[4] = new SqlParameter("@inUsername", username);
			arrParams[5] = new SqlParameter("@inIdCodigoPEP", idCodigoPEP);
			arrParams[6] = new SqlParameter("@inIdCodigoRegiaoVendas", idCodigoRegiaoVendas);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsLaboratorio",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
        }

		// Função que actualiza um Laboratório (Centro de Custo) e devolve a msg de erro
		public string UpdateLaboratorio(string idLaboratorio, string activo, string username, string idCodigoPEP, string idCodigoRegiaoVendas)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inIdLaboratorio", idLaboratorio);
			//arrParams[1] = new SqlParameter("@inIdGrandeza", idGrandeza);
			//arrParams[2] = new SqlParameter("@inIdLocalLaboratorio", idLocalLaboratorio);
			//arrParams[3] = new SqlParameter("@inCodigoCentroCusto", codigoCentroCusto);
			arrParams[1] = new SqlParameter("@inActivo", activo);
			arrParams[2] = new SqlParameter("@inUsername", username);
			arrParams[3] = new SqlParameter("@inIdCodigoPEP", idCodigoPEP);
			arrParams[4] = new SqlParameter("@inIdCodigoRegiaoVendas", idCodigoRegiaoVendas);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdLaboratorio",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}
	}
}
