using System;
using System.Data.SqlClient; 
using System.Configuration;
using System.Data;


namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for PedidosUtilizadores.
	/// </summary>
	public class PedidosUtilizadores
	{
		public PedidosUtilizadores()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //esta parte passou para a parte externa
//        public int InsertPedido(string nomeEmpresa, string numClienteEmpresa, string cp1, string cp2,string idLocalidade, string idPais, string moradaEmpresa, string telefoneEmpresa, string faxEmpresa, string emailEmpresa,string nif, string nomeContacto,string idTitulo, string cargo,string departamento,string extensaoEmpresa,string telefoneContacto,string faxcontacto,string emailcontacto,string observacoes)
//        
//        {
//            
//
//            SqlParameter[] arrParams = new SqlParameter[21];
//
//            arrParams[0] = new SqlParameter("@nomeEmpresa", nomeEmpresa);
//            arrParams[1] = new SqlParameter("@numClienteEmpresa", numClienteEmpresa);
//            arrParams[2] = new SqlParameter("@inCp1", cp1);
//            arrParams[3] = new SqlParameter("@inCp2", cp2);
//            arrParams[4] = new SqlParameter("@idLocalidade", idLocalidade);
//            arrParams[5] = new SqlParameter("@idPais", idPais);
//            arrParams[6] = new SqlParameter("@moradaEmpresa", moradaEmpresa);
//        
//            arrParams[7] = new SqlParameter("@telefoneEmpresa", telefoneEmpresa);
//            arrParams[8] = new SqlParameter("@faxEmpresa", faxEmpresa);
//            arrParams[9] = new SqlParameter("@emailEmpresa", emailEmpresa);
//            arrParams[10] = new SqlParameter("@nomeContacto", nomeContacto);
//            arrParams[11] = new SqlParameter("@idTitulo", idTitulo);
//            arrParams[12] = new SqlParameter("@cargo", cargo);
//
//            arrParams[13] = new SqlParameter("@departamento", departamento);
//            arrParams[14] = new SqlParameter("@extensaoEmpresa", extensaoEmpresa);
//            arrParams[15] = new SqlParameter("@telefoneContacto", telefoneContacto);
//            arrParams[16] = new SqlParameter("@faxcontacto", faxcontacto);
//            arrParams[17] = new SqlParameter("@emailcontacto", emailcontacto);
//            arrParams[18] = new SqlParameter("@observacoes", observacoes);
//            arrParams[19] = new SqlParameter("@bEstadoPedido", 1);
//            arrParams[20] = new SqlParameter("@nif", nif);
//        
//            try
//            {
//                int x =  GERAL.clsDataAccess.ExecuteNonQuerySP("stpInsPedidosUtilizadores", arrParams); 
//                return x; 
//            }
//            catch(Exception ex)
//            {
//                GERAL.clsWriteError.WriteLog(ex.ToString()); 
//                return 0; 
//            }
//
//
//        }

        public DataTable DTListaPedidos()
        {
            string strSQL = "SELECT * FROM dbo.PedidosUtilizadores "; 
         
            DataTable DT = LabMetro.GERAL.clsDataAccess.ExecuteDT(strSQL); 
           
            return DT;
        }

        public SqlDataReader DRDetalhesPedidos(string id)
        {
            string strSQL = "SELECT * FROM PedidosUtilizadores WHERE idPedidoUtilizador = " +id;  
            return LabMetro.GERAL.clsDataAccess.ExecuteDR(strSQL); 
           
        }

        public int DeletePedido(string id)

        {
            string strSQL = "DELETE FROM PedidosUtilizadores WHERE idPedidoUtilizador = " + id; 

            return GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 
            
        }
	}
}
