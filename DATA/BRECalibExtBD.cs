using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for BRECalibExtBD.
	/// </summary>
	public class BRECalibExtBD
	{
		public BRECalibExtBD()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //ha muitas coisas do bre calibração externa que sao tratadas no BREBD.cs "Normal"

        //aqui so sao tratadas as coisas que sao diferentes ou novas.

        //pesquisa dentro dos equipamentos de uma empresa por numeros de série que pertencem a equipamentos do mesmo tipo
        public DataTable DTEquipamentosParaSubstituicao(string idTipoEquipamento, string idEmpresa)
        {
            //2389-52
            string strSQL = "SELECT EQ.idEquipamento, TE.descricao AS tipoEquipamento, TE.codigo AS codigoEquipamento, EQ.numSerie, EQ.numIdentificacao, EQ.refUltimaCalibracao  FROM Equipamento EQ INNER JOIN  Empresa E ON EQ.idEmpresa = E.idEmpresa INNER JOIN      TipoEquipamento TE ON EQ.idTipoEquipamento = TE.idTipoEquipamento WHERE (EQ.idEmpresa = "+idEmpresa+") AND (EQ.idTipoEquipamento ="+idTipoEquipamento+") AND (EQ.activo = 1)"; 
                
            return GERAL.clsDataAccess.ExecuteDT(strSQL); 
        
        }
       
        //datatable com equipamentos que estao dentro da mesma grandeza q o equipamento original, e que podem ser trocados.

        public DataTable DTGetEquipamentosActivosByEmpresaPorGrandeza(string idEmpresa,string idTipoEquipamento,string refServico)
        {
            string idGrandeza; 
            string strSQL; 
            if(refServico != "")
            {
                idGrandeza = refServico.Substring(1,3); 

                //isto so funciona para as novas referencias de calibracao.
                strSQL ="SELECT EQ.idEquipamento,EQ.idTipoEquipamento, TE.descricao AS tipoEquipamento, TE.codigo AS codigoEquipamento, EQ.numSerie, EQ.numIdentificacao, EQ.refUltimaCalibracao, Familia.idGrandeza, Grandeza.idGrandeza FROM Equipamento EQ INNER JOIN Empresa E ON EQ.idEmpresa = E.idEmpresa INNER JOIN TipoEquipamento TE ON EQ.idTipoEquipamento = TE.idTipoEquipamento INNER JOIN Familia ON TE.idFamilia = Familia.idFamilia INNER JOIN TipoEquipamento TipoEquipamento_1 ON EQ.idTipoEquipamento = TipoEquipamento_1.idTipoEquipamento INNER JOIN  Grandeza ON Familia.idGrandeza = Grandeza.idGrandeza WHERE (EQ.idEmpresa = "+idEmpresa+") AND (EQ.activo = 1) AND (Grandeza.idGrandeza = '"+idGrandeza+"')ORDER BY TipoEquipamento, numIdentificacao,numSerie ASC"; 
            }
            else
            {
                //aqui so mostra equipamentos do mesmo tipo
                 strSQL = "SELECT EQ.idEquipamento,EQ.idTipoEquipamento, TE.descricao AS tipoEquipamento, TE.codigo AS codigoEquipamento, EQ.numSerie, EQ.numIdentificacao, EQ.refUltimaCalibracao  FROM Equipamento EQ INNER JOIN  Empresa E ON EQ.idEmpresa = E.idEmpresa INNER JOIN TipoEquipamento TE ON EQ.idTipoEquipamento = TE.idTipoEquipamento WHERE (EQ.idEmpresa = "+idEmpresa+") AND (EQ.idTipoEquipamento ="+idTipoEquipamento+") AND (EQ.activo = 1)"; 
            }
            
            return GERAL.clsDataAccess.ExecuteDT(strSQL);   
            
         }
            
                
               
		public DataTable DTListaEmpresasForListaBRECalibExt(string strEmpresa, string strNIF)
		{
            
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inNome", strEmpresa);
			arrParams[1] = new SqlParameter("@inNif", strNIF);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetEmpresasForListBRECalibExt", arrParams); 
            
		}                     
            
            
              
            
        
	}
}
