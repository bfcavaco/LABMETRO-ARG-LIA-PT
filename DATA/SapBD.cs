using System;
using System.Configuration;
using System.Data.SqlClient; 
using System.Data; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for SapBD.
	/// </summary>
	public class SapBD
	{
		public SapBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// Lista Funcionarios
		public SqlDataReader DRCodigoBloqueioSAP()
		{
			//+para a dropdown escolho o codigoBloqueio e a descCodigoBloqueio 
			//Nao sei se o id continua a ser usado
			string strSQL = "SELECT idCodigoBloqueio, codigoBloqueio, descCodigoBloqueio FROM sap_CodigoBloqueio WHERE activo = 1"; 

			return LabMetro.GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}
	}
}
