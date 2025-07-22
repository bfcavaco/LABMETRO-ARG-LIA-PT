using System;
using System.Data;
using System.Data.SqlClient; 
using System.Configuration; 



namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for FuncionariosBD.
	/// </summary>
	/// 
    public class FuncionarioDetails
    {
	    public string idFuncionario; 
        public string idUtilizador; 
        public string idLaboratorio; 
		public string laboratorio;
        public string idFuncao;
		public string funcao;
        public string idLocalCalibracao; 
		public string localCalibracao;
        public string nome;
        public string nomeAbreviado;
        public string extensao;
        public string telefoneDirecto;
        public string dataAdmissao;
		public string estado; // activo
        public string observacoes; 
		public string bCta; 
		public string email;
        public string numFuncionario; 
    }

	public class FuncionariosBD
	{
		public FuncionariosBD()
		{
		}

		// Função que devolve uma lista de Funcionários com base nos critérios de pesquisa
		public DataTable DTFillFuncionarios()
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inActiv", ""); // Todos os funcionarios
         
			return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListFuncionarios", arrParams); 
           
        }

		// Funcao que devolve um Funcionário com base no seu ID
		public FuncionarioDetails GetFuncionarioDetails(string idFuncionario)
		{
			SqlParameter[] arrParams = new SqlParameter[1];
            
			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);
         
			DataTable FuncionarioDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetFuncionarioById", arrParams); 
           
			if(FuncionarioDT.Rows.Count > 0) 
			{ 
				FuncionarioDetails myFuncionarioDetails = new FuncionarioDetails();
				SetFuncionarioDetails(FuncionarioDT, myFuncionarioDetails);
				return myFuncionarioDetails;
			}
			else
			{
				return null; 
			}
		}

		// Preenche todos os atributos da classe FuncionarioDetails a partir de uma DataTable
		private void SetFuncionarioDetails(DataTable FuncionarioDT, FuncionarioDetails myFuncionarioDetails)
		{
			myFuncionarioDetails.idFuncionario = FuncionarioDT.Rows[0]["idFuncionario"].ToString();  
            myFuncionarioDetails.idUtilizador = FuncionarioDT.Rows[0]["idUtilizador"].ToString();  
			myFuncionarioDetails.idLaboratorio = FuncionarioDT.Rows[0]["idLaboratorio"].ToString();
			myFuncionarioDetails.laboratorio = FuncionarioDT.Rows[0]["laboratorio"].ToString();
			myFuncionarioDetails.idFuncao = FuncionarioDT.Rows[0]["idFuncao"].ToString(); 
			myFuncionarioDetails.funcao = FuncionarioDT.Rows[0]["funcao"].ToString(); 
			myFuncionarioDetails.idLocalCalibracao = FuncionarioDT.Rows[0]["idLocalCalibracao"].ToString();
			myFuncionarioDetails.localCalibracao = FuncionarioDT.Rows[0]["localCalibracao"].ToString();
			myFuncionarioDetails.nome = FuncionarioDT.Rows[0]["nome"].ToString(); 
			myFuncionarioDetails.nomeAbreviado = FuncionarioDT.Rows[0]["nomeAbreviado"].ToString(); 
			myFuncionarioDetails.extensao = FuncionarioDT.Rows[0]["extensao"].ToString(); 
			myFuncionarioDetails.telefoneDirecto = FuncionarioDT.Rows[0]["telefoneDirecto"].ToString();   
			myFuncionarioDetails.dataAdmissao = FuncionarioDT.Rows[0]["dtAdmissao"].ToString(); 
			myFuncionarioDetails.estado = GERAL.clsGeral.ConvertStringToBool(FuncionarioDT.Rows[0]["activo"].ToString()).ToString();
            myFuncionarioDetails.observacoes = FuncionarioDT.Rows[0]["observacoes"].ToString();      
			 myFuncionarioDetails.bCta = FuncionarioDT.Rows[0]["bCta"].ToString();
			 myFuncionarioDetails.email = FuncionarioDT.Rows[0]["email"].ToString();
             myFuncionarioDetails.numFuncionario = FuncionarioDT.Rows[0]["numFuncionario"].ToString();

        }

		// Funcao que insere um Contacto e o ultimo id inserido
		public string InsertFuncionario(string idLaboratorio, string idFuncao, string idLocalCalibracao, string nome, string nomeAbreviado, string extensao, string telefoneDirecto, string dataAdmissao, string estado, string observacoes, string username,string bCta,string email, string numFuncionario)
		{
			SqlParameter[] arrParams = new SqlParameter[14];

            arrParams[0] = new SqlParameter("@inEmail", email);
			arrParams[1] = new SqlParameter("@inIdLaboratorio", idLaboratorio);
			arrParams[2] = new SqlParameter("@inIdFuncao", idFuncao);
			arrParams[3] = new SqlParameter("@inIdLocalCalibracao", idLocalCalibracao);
			arrParams[4] = new SqlParameter("@inNome", nome);
			arrParams[5] = new SqlParameter("@inNomeAbreviado", nomeAbreviado);
			arrParams[6] = new SqlParameter("@inExtensao", extensao);
			arrParams[7] = new SqlParameter("@inTelefoneDirecto", telefoneDirecto);
			arrParams[8] = new SqlParameter("@inDtAdmissao", dataAdmissao);
			arrParams[9] = new SqlParameter("@inActivo", estado);
			arrParams[10] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[11] = new SqlParameter("@inUsername", username);
			arrParams[12] = new SqlParameter("@inBCta", bCta);
            arrParams[13] = new SqlParameter("@inNumFuncionario", numFuncionario);

            return GERAL.clsDataAccess.ExecuteNonQuerySPOutput("stpInsFuncionario", arrParams).ToString(); 

        }

	
		public string UpdateFuncionario(string idFuncionario,string idLaboratorio, string idFuncao, string idLocalCalibracao, string nome, string nomeAbreviado, string extensao, string telefoneDirecto, string dataAdmissao, string estado, string observacoes, string username, string bCta, string email, string numFuncionario)
		{
			SqlParameter[] arrParams = new SqlParameter[15];

			arrParams[0] = new SqlParameter("@inIdFuncionario", idFuncionario);

			arrParams[1] = new SqlParameter("@inIdLaboratorio", idLaboratorio);
			arrParams[2] = new SqlParameter("@inIdFuncao", idFuncao);
			arrParams[3] = new SqlParameter("@inIdLocalCalibracao", idLocalCalibracao);
			arrParams[4] = new SqlParameter("@inNome", nome);
			arrParams[5] = new SqlParameter("@inNomeAbreviado", nomeAbreviado);
			arrParams[6] = new SqlParameter("@inExtensao", extensao);
			arrParams[7] = new SqlParameter("@inTelefoneDirecto", telefoneDirecto);
			arrParams[8] = new SqlParameter("@inActivo", estado);
			arrParams[9] = new SqlParameter("@inObservacoes", observacoes);
			arrParams[10] = new SqlParameter("@inUsername", username);	
			arrParams[11] = new SqlParameter("@inBCta", bCta);
			arrParams[12] = new SqlParameter("@inEmail", email);
            arrParams[13] = new SqlParameter("@inDtAdmissao", dataAdmissao);
            arrParams[14] = new SqlParameter("@inNumFuncionario", numFuncionario);

            int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdFuncionario", arrParams);

            //retorna 0 se corre bem, nao sei pq tenho de chamar esta funcao, mas so funciona assim 
            if (retValue == 0)
                return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
            else
                return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;    

   
		}
	}
}
