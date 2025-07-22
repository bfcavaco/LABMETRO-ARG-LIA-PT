using System;
using System.Data;
using System.Data.SqlClient; 
using System.Web;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for MarcasModelosBD.
	/// </summary>
	public class MarcasModelosBD
	{
		public MarcasModelosBD()
		{
		}


		// ==========================================================================================
		// Marcas
		// ==========================================================================================


		// Função que devolve uma lista de Marcas com base nos critérios de pesquisa/ordenação
		public DataTable DTMarcas()
		{
//			SqlParameter[] arrParams = new SqlParameter[2];
//            
//			arrParams[0] = new SqlParameter("@inTable", "Marca");
//			arrParams[1] = new SqlParameter("@inActiv", "0");
//			
//			DataTable MarcasDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetTableRecords", arrParams); 
			string strSQL = "SELECT Marca.idMarca, Marca.activo, Marca.descricao, Marca.idEmpresaManutCTA, Marca.activo, Empresa.nomeAbreviado as empresaManutencao, marca.dtcriacao, utilizador.username FROM Marca inner join Utilizador on marca.idUtilCriacao = utilizador.idUtilizador LEFT JOIN Empresa ON Marca.idEmpresaManutCTA = Empresa.idEmpresa ORDER BY marca.descricao"; 
			return GERAL.clsDataAccess.ExecuteDT(strSQL); 
		}

		// Função que insere uma Marca e devolve a msg de erro
		public string InsertMarcas(string descricao, string idEmpresaManut)
		{
			SqlParameter[] arrParams = new SqlParameter[3];

			arrParams[0] = new SqlParameter("@inDescricao", descricao);
			
			arrParams[1] = new SqlParameter("@inUserId",  HttpContext.Current.Session["UserId"].ToString());

			arrParams[2] = new SqlParameter("@inIdEmpresaManut", idEmpresaManut);
			int i = GERAL.clsDataAccess.ExecuteNonQuerySP("stpInsMarca",arrParams); 
			return i.ToString(); 
//			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsMarca",arrParams); 
//
//			if (retValue == 0)
//				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
//			else if (retValue == 2)
//				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
//			else
//				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
		}

		// Função que actualiza uma Marca e devolve a msg de erro
		public string UpdateMarcas(string idMarca, string descricao, string estado, string idEmpresaManut)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inIdMarca", idMarca);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUserId",  HttpContext.Current.Session["UserId"].ToString());
			arrParams[4] = new SqlParameter("@inIdEmpresaManut", idEmpresaManut);

			int i = GERAL.clsDataAccess.ExecuteNonQuerySP("stpUpdMarca",arrParams); 
			return i.ToString(); 
		}



		// ==========================================================================================
		// Modelos
		// ==========================================================================================



		// Função que devolve uma lista de Modelos com base nos critérios de pesquisa/ordenação
		public DataTable DTModelos(string idMarca, string descricao)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inIdMarca", idMarca);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
         
			DataTable ModelosDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListModelos", arrParams); 
  
			return ModelosDT;
        }

		// Função que insere um Modelo de Equipamentos e devolve a msg de erro
		public string InsertModelos(string idMarca, string descricao, string estado, string username)
		{          
			SqlParameter[] arrParams = new SqlParameter[4];

			arrParams[0] = new SqlParameter("@inIdMarca", idMarca);
			arrParams[1] = new SqlParameter("@inDescricao", descricao);
			arrParams[2] = new SqlParameter("@inActivo", estado);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsModelo",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
		}

		// Função que actualiza um Modelo de Equipamentos e devolve a msg de erro
		public string UpdateModelos(string idModelo, string descricao, string estado, string username)
		{
			return UpdateModelos(idModelo, "", descricao, estado, username);
		}

		// Função que actualiza um Modelo de Equipamentos e devolve a msg de erro
		public string UpdateModelos(string idModelo, string idMarca, string descricao, string estado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inIdModelo", idModelo);
			arrParams[1] = new SqlParameter("@inIdMarca", idMarca);
			arrParams[2] = new SqlParameter("@inDescricao", descricao);
			arrParams[3] = new SqlParameter("@inActivo", estado);
			arrParams[4] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdModelo",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}

		public SqlDataReader DREmpresasManutencao()
		{
			string strSQL = "SELECT idEmpresa, nomeAbreviado as empresaManutencao FROM EMPRESA WHERE				idTipoEmpresa = 4 AND idEstadoEmpresa = 1"; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}

		
	}
}
