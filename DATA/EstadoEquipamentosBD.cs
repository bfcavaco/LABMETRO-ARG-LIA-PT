using System;
using System.Data; 
using System.Data.SqlClient; 
namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for EstadoEquipamentosBD.
	/// </summary>
	public class EstadoEquipamentosBD
	{
		public EstadoEquipamentosBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public DataTable ListaEquipmentos(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            
			
            arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
            DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEstadoEquipamentosByEmpresa", arrParams); 
           
            return DT;
        }
	}
}
