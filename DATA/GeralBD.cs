using System;
using System.Data;
using System.Data.SqlClient;

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for GeralBD.
	/// </summary>
	public class GeralBD
	{
		public GeralBD()
		{
		}

		// Função que devolve o ID do Funcionário correspondente a um dado Username
		public static int GetIdFuncionarioByUsername(string username)
		{
			string strSQL = "SELECT dbo.udfGetIdFuncionarioByUsername('" + username + "')"; 
           
			return (int)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
		}

//		// Função que devolve a Referência da Obra correspondente a um ID de um BRE
//		public string GetReferenciaObraByIdBRE(string idBRE)
//		{
//			string strSQL = "SELECT dbo.udfGetReferenciaObraByIdBRE(" + idBRE + ")"; 
//           
//			return (string)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
//		}
	}
}
