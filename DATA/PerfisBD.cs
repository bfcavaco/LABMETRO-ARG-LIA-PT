using System;
using System.Data; 
using System.Data.SqlClient;
using System.Web.UI; 
using System.Web; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for PerfisBD.
	/// </summary>
	public class PerfisBD
	{
		public PerfisBD()
		{
		}

		//=========================================================================================================
		// Função que devolve as permissões associadas a um determinado perfil
		//=========================================================================================================
        public  DataTable DTPermissions(string idPerfil)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@inIdPerfil", idPerfil);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetPermissoesPerfil",arrParams); 
           
        }
		//=========================================================================================================
		// Função que actualiza as permissões
		//=========================================================================================================
        public int UpdatePermissions(string idPerfil, string idPagina, string acesso, string acessoTotal)
        {
            SqlParameter[] arrParams = new SqlParameter[6];
            
            arrParams[0] = new SqlParameter("@inIdPerfil", idPerfil);
            arrParams[1] = new SqlParameter("@inIdPagina", idPagina);
            arrParams[2] = new SqlParameter("@inAcesso", GERAL.clsGeral.ConvertStringToBool(acesso));
            arrParams[3] = new SqlParameter("@inAcessoTotal", GERAL.clsGeral.ConvertStringToBool(acessoTotal));
            arrParams[4] = new SqlParameter("@userId", HttpContext.Current.Session["UserId"].ToString());
			arrParams[5] = new SqlParameter("@inUsername", HttpContext.Current.User.Identity.Name.ToString()); 
			
            
            try
            {
                GERAL.clsDataAccess.ExecuteNonQuerySP("stpInsUpdDelPermissoesPerfil",arrParams); 
                return 0; 
            }
            catch
            {
                return 1; 
            }
        }

		//=========================================================================================================
		// Funcao que devolve uma Lista de Utilizadores com um determinado Perfil
		//=========================================================================================================
		public SqlDataReader ListaUtilizadoresPerfil(string idPerfil)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
        	arrParams[0] = new SqlParameter("@inIdPerfil", idPerfil);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetFuncionariosByIdPerfil", arrParams); 
           
        }

		//=========================================================================================================
		// Funcao que devolve uma Lista de FUNCIONARIOS e as suas associações
		//=========================================================================================================
		public SqlDataReader ListaFuncionarioWorkflow(string idFuncionario)
		{       
			SqlParameter[] arrParams = new SqlParameter[1];    
			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetFuncionarios",arrParams); 
           
		}

		//=========================================================================================================
		// Funcao que devolve uma Lista de Funcionários responsáveis no workflow e as suas associações
		//=========================================================================================================
		public SqlDataReader ListaResponsaveisWorkflow(string grau)
		{       
			SqlParameter[] arrParams = new SqlParameter[1];
        	arrParams[0] = new SqlParameter("@inGrau", grau);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetResponsaveis",arrParams);    
		}

		//=========================================================================================================
		// Funcao que devolve uma Lista de Funcionários Responsáveis e as suas associações
		//=========================================================================================================
		public DataTable dtListaFuncionariosWorkflow(string grau)
		{       
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inGrau", grau);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetResponsaveis",arrParams); 
          
		}


		//=========================================================================================================
		//DATAREADER COM CHAVES COMBINADAS: IDFUNCIONARIO/IDGRANDEZA - CONTEM AS ASSOCIAÇÕES DOS FUNCIONÁRIOS
		//(RESPONSÁVEIS ÀS GRANDEZAS))
		//=========================================================================================================
		public SqlDataReader ListaGrandezasResponsaveisWorkflow(string idFuncionario)
		{       
			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetResponsaveisGrandezas", arrParams); 
           
		}

		//==================================================================================================
		//DATATABLE COM CHAVES COMBINADAS: IDFUNCIONARIO/IDGRANDEZA - CONTEM AS ASSOCIAÇÕES DOS FUNCIONÁRIOS
		//(RESPONSÁVEIS ÀS GRANDEZAS))
		//==================================================================================================
		public DataTable dtListaFuncionariosGrandezasWorkflow(string idFuncionario)
		{       
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpWGetResponsaveisGrandezas", arrParams);   
		}

		//==================================================================================================
		// DATAREADER - LISTA DE GRANDEZAS
		//==================================================================================================
		public SqlDataReader ListaGrandezasWorkflow()
		{       
			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpWGetGrandezas"); 
			
		}

		
		//==================================================================================================
		// DATAREADER - LISTA DE GRAUS
		//==================================================================================================
		public SqlDataReader ListaGrauWorkflow()
		{       
			return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpWGetGrau"); 
		}
		

		
		//==================================================================================================
		// DATAREADER - LISTA DE PERFIS
		//==================================================================================================
        public SqlDataReader ListaPerfis()
        {
//			DATA.ListasBD listas = new LabMetro.DATA.ListasBD(); 
////            return listas.DRLista("Perfil","","",""); 
//
//            //alteracao ultima hora: nao se pretende mostrar ou editar o perfil -1 que serve apenas para utilizadores externos poderem inserir pedidos de orçamento e pedidos de utlizador na bd, para ter uma chave, perfil -1, idUtilizador -1

            string strSQL = "SELECT  descricao, idPerfil AS ident, activo FROM Perfil WHERE idPerfil > 0"; 
            return GERAL.clsDataAccess.ExecuteDR(strSQL); 
            
        }

		//==================================================================================================
		// DATAREADER - LISTA DE PERFIS CONFORME O GRAU?
		//==================================================================================================

		public SqlDataReader ListaPerfisWorkflow(string grau)
		{

			SqlParameter[] arrParams = new SqlParameter[1];
			arrParams[0] = new SqlParameter("@inGrau", grau);

			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpWGetPerfil", arrParams); 
		}	
        
		//==================================================================================================
		// INSERE PERFIL NA BD E DEVOLVE MENSAGEM DE ERRO
		//==================================================================================================
		public string InsertPerfil(string descricao, string estado, string username)
		{          
				SqlParameter[] arrParams = new SqlParameter[4];

				arrParams[0] = new SqlParameter("@inTable", "Perfil");
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

		//==================================================================================================
		// UPDATE AO PERFIL NA BD E DEVOLVE MENSAGEM DE ERRO
		//==================================================================================================
		public string UpdatePerfis(string idPerfil, string descricao, string estado, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[5];
 
			arrParams[0] = new SqlParameter("@inTable", "Perfil");
            arrParams[1] = new SqlParameter("@inId", idPerfil);
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

		//=========================================================================================================
		// Função que insere um Perfil do Workflow e devolve a msg de erro
		//=========================================================================================================
		public string InsertPerfilWorkFlow(string idFuncionario, string idGrandeza, string idGrau, string idPerfil, string username)
		{          
			SqlParameter[] arrParams = new SqlParameter[5];

			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);
			arrParams[1] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[2] = new SqlParameter("@inIdGrau", idGrau);
			arrParams[3] = new SqlParameter("@inIdPerfil", idPerfil);
			arrParams[4] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpWInsResponsaveisGrandezas",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_INSERT;
		}

		//=========================================================================================================
		// Função que apaga uma Grandeza associada ao Funcionario do WorkFlow e devolve a msg de erro
		//=========================================================================================================
		public string DeleteGrandezaWorkFlow(string idResponsavel, string username)

		{
			SqlParameter[] arrParams = new SqlParameter[2];
 
			arrParams[0] = new SqlParameter("@inIdResponsavel", idResponsavel);
			arrParams[1] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpWDelGrandeza",arrParams); 	

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_DELETE_DB;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_DELETE;
		}

		//=========================================================================================================
		// Função que actualiza uma Grandeza associada ao Funcionario do WorkFlow e devolve a msg de erro
		//=========================================================================================================
		public string UpdateGrandezaWorkFlow(string idResponsavel, string idGrandeza, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[3];
 
			arrParams[0] = new SqlParameter("@inIdResponsaveis", idResponsavel);
			arrParams[1] = new SqlParameter("@inIdGrandeza", idGrandeza);
			arrParams[2] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpWUpdResponsaveisGrandezas",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}
		//=========================================================================================================
		// Função que actualiza um Perfil do WorkFlow e devolve a msg de erro
		//=========================================================================================================
		public string UpdatePerfilWorkFlow(string idFuncionario, string idGrau, string idPerfil, string username)
		{
			SqlParameter[] arrParams = new SqlParameter[4];
 
			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);
			arrParams[1] = new SqlParameter("@inIdGrau", idGrau);
			arrParams[2] = new SqlParameter("@inIdPerfil", idPerfil);
			arrParams[3] = new SqlParameter("@inUsername", username);

			int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpWUpdResponsaveisPerfis",arrParams); 

			if (retValue == 0)
				return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
			else if (retValue == 2)
				return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY;
			else
				return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
		}
	}
}
