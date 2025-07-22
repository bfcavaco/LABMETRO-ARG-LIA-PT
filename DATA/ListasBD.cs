using System;
using System.Configuration;
using System.Data.SqlClient; 
using System.Data; 


namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for ListasBD.
	/// </summary>
	public class ListasBD
	{
		public ListasBD()
		{
		}

        //TODAS AS LISTAS RETORNAM SEMPRE OS CAMPOS ID E NOME.
        
        //*******************************************************************************************
        //*******************************************************************************************
        //*******************************************************************************************
        //*******************************************************************************************
        //atencao, ver se está sempre escolhido o WHERE Activo = 1!!!!!!!!!!!
        //===========================================================================================

		// Lista Grandezas
        public SqlDataReader DRListaGrandezas()
        {
            return DRLista("Grandeza","1"); 
        }

		// Lista Perfis
        public SqlDataReader DRListaPerfis()
        {
            return DRLista("Perfil","1"); 
        }

		// Lista Funcionarios
		public SqlDataReader DRListaFuncionarios()
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
            arrParams[0] = new SqlParameter("@inActiv", "1");
			arrParams[1] = new SqlParameter("@inIdFuncao", "");
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListFuncionarios", arrParams); 
         }

        // Lista Funcionarios
        public SqlDataReader DRListaFuncionariosTreino()
        {
           string strSQL ="SELECT f.idFuncionario, f.Nome from funcionario f inner join utilizador u on f.idUtilizador = u.idUtilizador where u.idPerfil in (4,5,6,10) and  f.activo = 1 and f.idFuncionario in (select idFuncionarioTreino from servico) order by f.nome";

           return GERAL.clsDataAccess.ExecuteDR(strSQL);

            //return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListFuncionarios", arrParams);
        }

		// Lista Funcionarios com uma determinada função - usado no FormOrcam.aspx
		public DataTable DTListaFuncionarios(string idFuncao)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inActiv", "1");
			arrParams[1] = new SqlParameter("@inIdFuncao", idFuncao);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListFuncionarios", arrParams);   
		}

		// Lista Contactos
		public SqlDataReader DRListaContactos(string idEmpresa, string activo)
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@inActiv", activo);

			arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
            
			
            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListContactos", arrParams); 
           
		}

        // Lista Contactos
        public SqlDataReader DRListaContactos(string idEmpresa)
        {
            SqlParameter[] arrParams = new SqlParameter[2];
            
            arrParams[0] = new SqlParameter("@inActiv", "1");
      
            arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
            
            
            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListContactos", arrParams); 
           
        }

		// Lista Empresas
		//usado no FormEmpresa.aspx
		//usado no FormRepListas.aspx
		//usado no GestUtilizadores.aspx

		public SqlDataReader DRListaEmpresas()
		{
			SqlParameter[] arrParams = new SqlParameter[4];
            
			arrParams[0] = new SqlParameter("@inNome", "");
			arrParams[1] = new SqlParameter("@inNif", "");
			arrParams[2] = new SqlParameter("@inIdEstadoEmpresa", "1"); // Activas
			arrParams[3] = new SqlParameter("@inIdTipoEmpresa", "");

			return  LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListEmpresas", arrParams); 
		}

        public SqlDataReader DRListaNivelPrioridade()
        {
            return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetListNivelPrioridade");
        }


		// Lista Localidades
		public SqlDataReader DRListaLocalidades()
		{
			return DRLista("Localidade","1");
		}

		// Lista Estados Empresas
		public SqlDataReader DRListaEstadosEmpresas()
		{
			return DRLista("EstadoEmpresa","1");
		}

//		// Lista Estados Orçamentos //desaparece daqui, pq nao pode levar com a ordenacao default
		//alfabetica, dem de ser ordenado por id, para aparecer o pedido em primeiro.
//		public SqlDataReader DRListaEstadosOrcamentos()
//		{
//			return DRLista("EstadoOrcamento","1");
//		}


		public DataView DVListaEstadosOrcamentos()
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@intable", "EstadoOrcamento");
			arrParams[1] = new SqlParameter("@inactiv", "1");

			DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetTableRecords", arrParams);
			
			DataView DV = new DataView(DT); 
			DV.Sort = "ident ASC"; 
			
			return DV; 
		}


        public DataView DVRazaoClienteOcamentos()
        {
            SqlParameter[] arrParams = new SqlParameter[2];


            string strSQL = "Select idRazaoCliente, razaoCliente from RazaoCliente order by 2";
            DataTable DT = LabMetro.GERAL.clsDataAccess.ExecuteDT(strSQL); 

            DataView DV = new DataView(DT);
           
            return DV;
        }


		// Lista Estados Obras
		public SqlDataReader DRListaEstadosObras()
		{
			return DRLista("EstadoObra","1");
		}

        //// Lista Marcas
        //public SqlDataReader DRListaMarcas()
        //{
        //    return DRLista("Marca","1");
        //}

		// Lista Marcas
		public SqlDataReader DRListaMarcas()
		{
			
			string strSQL = "SELECT idMarca, descricao FROM Marca ORDER BY DESCRICAO "; 
			return GERAL.clsDataAccess.ExecuteDR(strSQL); 
		}


		// Lista Modelos
		public SqlDataReader DRListaModelos(string idMarca)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdMarca", idMarca);
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetListModelos", arrParams); 
           
        }

		// Lista Países
        public SqlDataReader DRListaPaises()
        {
            return DRLista("Pais","1");
        }

        // Lista Departamentos
        public SqlDataReader DRListaDepartamentos()
        {
            return DRLista("Departamento","1");
        }

		// Lista Cargos
        public SqlDataReader DRListaCargos()
        {
            return DRLista("Cargo","1");
        }

		// Lista Funções
        public SqlDataReader DRListaFuncoes()
        {
            return DRLista("Funcao","1");
        }

		// Lista Laboratórios
        public SqlDataReader DRListaLaboratorios()
        {
			return  LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetListLaboratorios");   
        }

		// Lista Locais de Calibração
        public SqlDataReader DRListaLocalCalibracao()
        {
            return DRLista("LocalCalibracao","1");
        }

		// Lista Tipos de Serviço
        public SqlDataReader DRListaTipoServico()
        {
            return DRLista("TipoServico",""); 
        }

		// Lista Famílias
        public SqlDataReader DRListaFamilias()
        {
            return DRLista("Familia","1");
        }

        // Lista Títulos
		public SqlDataReader DRListaTitulos()
        {
            return DRLista("Titulo","1");
        }

		// Lista Tipos de Empresas
        public SqlDataReader DRListaTiposEmpresa()
        {
            return DRLista("TipoEmpresa","1");
        }

		// Lista Tipos de Equipamentos
        public SqlDataReader DRListaTiposEquipamento()
        {
             return DRLista("TipoEquipamento","1");
        }

        // Lista Tipos de Equipamentos
        public SqlDataReader DRListaEquipamentosMesmaGrandeza(string idTipoEquipamento)
        {
            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@idTipoEquipamento",idTipoEquipamento);
         
            return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetEquipamentosDaMesmaGrandeza", arrParams); 
            
        }

//		// Lista Tipos de Certificados
//		public SqlDataReader DRListaTiposCertificado()
//		{
//			return DRLista("TipoCertificado","1");
//		}

		// Lista Tipos de Certificados
		public DataView DVListaTiposCertificado()
		{
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@intable", "TipoCertificado");
			arrParams[1] = new SqlParameter("@inactiv", "1");

			DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetTableRecords", arrParams);
			
			DataView DV = new DataView(DT); 
			DV.Sort = "ident ASC"; 
			
			return DV; 
		}

		// Lista Observações do Local de Execução
		public SqlDataReader DRListaObsLocalExecucao()
		{
			return DRLista("ObsLocalExecucao","1");
		}
        
      

        // Lista de estados de serviço
        public SqlDataReader DRListaEstadosServico()
        {
            return LabMetro.GERAL.clsDataAccess.SPExecuteDR("stpGetListaEstadosServico"); 

        }
		// Funcao que devolve todos os Registos de uma dada Tabela de Manutenção com base no nome desta
		// table          - Nome da Tabela cujos registos pretendemos devolver
		// activ          - Flag que indica se pretendemos retornar 
		//                  (0 - Todos os registos; 1 - Apenas os Registos que se encontram activos)
		// orderField     - Campo pelo qual pretendemos ordenar os registos
		// orderDirection - Direcção pela qual pretendemos ordenar os registos (ASC ; DESC)
		public SqlDataReader DRLista(string table, string activ)
		{
            
			SqlParameter[] arrParams = new SqlParameter[2];
            
			arrParams[0] = new SqlParameter("@intable", table);
			arrParams[1] = new SqlParameter("@inactiv", activ);
		
			return LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetTableRecords", arrParams); 
           
		}
	}
}

