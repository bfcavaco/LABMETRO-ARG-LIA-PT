using System;
using System.Data.SqlClient; 
using System.Configuration;
using System.Data; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for ContactosBD.
	/// </summary>
	/// 
    public class ContactDetails
    {
		public string idContacto;
		public string nome;
        public string idEmpresa; 
        public string nomeEmpresa; 
        public string idTitulo; 
		public string titulo;
        public string contactoPrincipal;
        public string cargo;
        public string departamento;
        public string extensaoEmpresa;
        public string telefoneEmpresa;
        public string faxEmpresa;
        public string emailEmpresa;
		public string activo;
        public string observacoes; 
		public string telemovel;
		public bool bFacturacao;
		public bool bOrcamento;
		public bool bQualidade;
		public bool bManutencao;
		public bool bCertificados;
		public bool bRequisicoes;
        public bool bGestaoEquipamentos;
        public bool bAlertasCertificados;
        public bool bAlertasLevantamentos;
        public bool bPlanosCalibracao;
    }

        public class ContactosBD
        {
            public ContactosBD()
            {
            }

			// Função que devolve uma lista de contactos com base nos critérios de pesquisa
			//nao sei se ainda é usada, fiz uma nova para a ListaContactos.aspx, mais simples, 
			//no final da pagina.
            public DataTable DTFillContacts(string idEmpresa,string nomeContacto,string activo, string email)
			{
				SqlParameter[] arrParams = new SqlParameter[4];
            
				arrParams[0] = new SqlParameter("@inActiv", activo); // Todos os contactos
				arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
                arrParams[2] = new SqlParameter("@inNome", nomeContacto); 
				arrParams[3] = new SqlParameter("@inEmail", email); 
             
         
				return LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListContactos", arrParams); 
           
			}

			public ContactDetails GetContactDetails(string idContacto)
			{
				SqlParameter[] arrParams = new SqlParameter[1];
            
				arrParams[0] = new SqlParameter("@inIdContacto", idContacto);
         
				DataTable ContactoDT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetContactoById", arrParams); 
           
				if(ContactoDT.Rows.Count > 0) 
				{ 
					ContactDetails myContactDetails = new ContactDetails();
					SetContactDetails(ContactoDT, myContactDetails);
					return myContactDetails;
				}
				else
				{
					return null; 
				}
			}

			// Preenche todos os atributos da classe ContactDetails a partir de uma DataTable
			private void SetContactDetails(DataTable ContactoDT, ContactDetails myContactDetails)
			{
                myContactDetails.idContacto = ContactoDT.Rows[0]["idContacto"].ToString();
				myContactDetails.idEmpresa = ContactoDT.Rows[0]["idEmpresa"].ToString(); 
                myContactDetails.nomeEmpresa = ContactoDT.Rows[0]["nomeEmpresa"].ToString();
				myContactDetails.idTitulo = ContactoDT.Rows[0]["idTitulo"].ToString();
				myContactDetails.titulo = ContactoDT.Rows[0]["titulo"].ToString();
                myContactDetails.nome = ContactoDT.Rows[0]["nome"].ToString(); 
                myContactDetails.contactoPrincipal = GERAL.clsGeral.ConvertStringToBool(ContactoDT.Rows[0]["contactoPrincipal"].ToString()).ToString();
                myContactDetails.cargo = ContactoDT.Rows[0]["cargo"].ToString();
                myContactDetails.departamento = ContactoDT.Rows[0]["departamento"].ToString(); 
                myContactDetails.extensaoEmpresa =ContactoDT.Rows[0]["extensaoEmpresa"].ToString(); 
                myContactDetails.telefoneEmpresa = ContactoDT.Rows[0]["telefoneEmpresa"].ToString();
                myContactDetails.faxEmpresa = ContactoDT.Rows[0]["faxEmpresa"].ToString(); 
                myContactDetails.emailEmpresa = ContactoDT.Rows[0]["emailEmpresa"].ToString();
                myContactDetails.activo = GERAL.clsGeral.ConvertStringToBool(ContactoDT.Rows[0]["activo"].ToString()).ToString();
                myContactDetails.observacoes = ContactoDT.Rows[0]["observacoes"].ToString();
				myContactDetails.telemovel = ContactoDT.Rows[0]["observacoes"].ToString();
				myContactDetails.bCertificados = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bCertificados"].ToString());
				myContactDetails.bFacturacao = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bFacturacao"].ToString());
				myContactDetails.bManutencao = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bManutencao"].ToString());
				myContactDetails.bOrcamento = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bOrcamento"].ToString());
				myContactDetails.bQualidade = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bQualidade"].ToString());
				myContactDetails.bRequisicoes = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bRequisicoes"].ToString());
			
                myContactDetails.bGestaoEquipamentos = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bGestaoEquipamentos"].ToString());GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bGestaoEquipamentos"].ToString());
                myContactDetails.bAlertasCertificados = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bAlertasCertificados"].ToString());
                myContactDetails.bAlertasLevantamentos = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bAlertasLevantamentos"].ToString());
                myContactDetails.bPlanosCalibracao = GERAL.clsGeral.ConvertBStringToBoolean(ContactoDT.Rows[0]["bAlertaPlanoCalibracao"].ToString());

            }

			// Funcao que insere um Contacto e retorna o id inserido
            public string InsertContact(string idEmpresa, string idTitulo, string nome, string contactoPrincipal, string cargo, string departamento, string extensaoEmpresa, string telefoneEmpresa, string faxEmpresa, string emailEmpresa, string observacoes, string telemovel, string bFacturacao, string bOrcamento, string bQualidade, string bManutencao, string bCertificados, string bRequisicoes, string activo, string username, string bGestaoEquipamentos, string bAlertaPlanoCalibracao)
            {
				SqlParameter[] arrParams = new SqlParameter[22];

				arrParams[0] = new SqlParameter("@inIdEmpresa", idEmpresa);
				
				arrParams[1] = new SqlParameter("@inIdTitulo", idTitulo);
				
				arrParams[2] = new SqlParameter("@inNome", nome);
				arrParams[3] = new SqlParameter("@inContactoPrincipal", contactoPrincipal);
				arrParams[4] = new SqlParameter("@inCargo", cargo);
				arrParams[5] = new SqlParameter("@inDepartamento", departamento);
				arrParams[6] = new SqlParameter("@inExtensaoEmpresa", extensaoEmpresa);
				arrParams[7] = new SqlParameter("@inTelefoneEmpresa", telefoneEmpresa);
				arrParams[8] = new SqlParameter("@inFaxEmpresa", faxEmpresa);
				arrParams[9] = new SqlParameter("@inEmailEmpresa", emailEmpresa);
				arrParams[10] = new SqlParameter("@inTelemovel", telemovel);
				arrParams[11] = new SqlParameter("@inActivo", activo);
				arrParams[12] = new SqlParameter("@inObservacoes", observacoes);
				arrParams[13] = new SqlParameter("@inUsername", username);
				arrParams[14] = new SqlParameter("@bFacturacao", bFacturacao);
				arrParams[15] = new SqlParameter("@bOrcamento", bOrcamento);
				arrParams[16] = new SqlParameter("@bQualidade", bQualidade);
				arrParams[17] = new SqlParameter("@bManutencao", bManutencao);
				arrParams[18] = new SqlParameter("@bCertificados", bCertificados);
				arrParams[19] = new SqlParameter("@bRequisicoes", bRequisicoes);
                arrParams[20] = new SqlParameter("@bGestaoEquipamentos", bGestaoEquipamentos);
                arrParams[21] = new SqlParameter("@bAlertaPlanoCalibracao", bAlertaPlanoCalibracao);

                 return GERAL.clsDataAccess.ExecuteNonQuerySPOutput("stpInsContacto", arrParams).ToString(); 

            }

			// Funcao que actualiza um Contacto e retorna um codigo de erro
            //retorna 0 (em string) se correu bem e outra coisa qq se correu mal
            public string UpdateContact(string idContacto, string idEmpresa, string idTitulo, string nome, string contactoPrincipal, string cargo, string departamento, string extensaoEmpresa, string telefoneEmpresa, string faxEmpresa, string emailEmpresa, string observacoes, string telemovel, string bFacturacao, string bOrcamento, string bQualidade, string bManutencao, string bCertificados, string bRequisicoes, string activo, string username, string bGestaoEquipamentos, string bAlertaPlanoCalibracao)
			{
				SqlParameter[] arrParams = new SqlParameter[23];

				arrParams[0] = new SqlParameter("@inIdContacto", idContacto);
				arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
				
				arrParams[2] = new SqlParameter("@inIdTitulo", idTitulo);
			
				arrParams[3] = new SqlParameter("@inNome", nome);
				arrParams[4] = new SqlParameter("@inContactoPrincipal", contactoPrincipal);
				arrParams[5] = new SqlParameter("@inCargo", cargo);
				arrParams[6] = new SqlParameter("@inDepartamento", departamento);
				arrParams[7] = new SqlParameter("@inExtensaoEmpresa", extensaoEmpresa);
				arrParams[8] = new SqlParameter("@inTelefoneEmpresa", telefoneEmpresa);
				arrParams[9] = new SqlParameter("@inFaxEmpresa", faxEmpresa);
				arrParams[10] = new SqlParameter("@inEmailEmpresa", emailEmpresa);
				
				arrParams[11] = new SqlParameter("@inActivo", activo);
				arrParams[12] = new SqlParameter("@inObservacoes", observacoes);
				arrParams[13] = new SqlParameter("@inUsername", username);

				arrParams[14] = new SqlParameter("@bFacturacao", bFacturacao);
				arrParams[15] = new SqlParameter("@bOrcamento", bOrcamento);
				arrParams[16] = new SqlParameter("@bQualidade", bQualidade);
				arrParams[17] = new SqlParameter("@bManutencao", bManutencao);
				arrParams[18] = new SqlParameter("@bCertificados", bCertificados);

				arrParams[19] = new SqlParameter("@inTelemovel", telemovel);
				arrParams[20] = new SqlParameter("@bRequisicoes", bRequisicoes);
				
                arrParams[21] = new SqlParameter("@bGestaoEquipamentos", bGestaoEquipamentos);
                arrParams[22] = new SqlParameter("@bAlertaPlanoCalibracao", bAlertaPlanoCalibracao);

				int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdContacto", arrParams); 
                return retValue.ToString(); 

//				if (retValue == 0)
//					return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
//				else
//					return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;    
			}

			// Funcao que actualiza um Contacto e retorna um codigo de erro
			//retorna 0 (em string) se correu bem e outra coisa qq se correu mal
			public string UpdateContactCurto(string idContacto, string nome,  string cargo, string telefoneEmpresa, string faxEmpresa, string emailEmpresa, string bFacturacao, string bOrcamento, string bQualidade,string bManutencao, string bCertificados, string bRequisicoes, string activo, string username, string departamento,string bGestaoEquipamentos)
			{
				SqlParameter[] arrParams = new SqlParameter[16];

				arrParams[0] = new SqlParameter("@inIdContacto", idContacto);
				arrParams[1] = new SqlParameter("@inNome", nome);
				
				arrParams[2] = new SqlParameter("@inCargo", cargo);
			
				arrParams[3] = new SqlParameter("@inTelefoneEmpresa", telefoneEmpresa);
				arrParams[4] = new SqlParameter("@inFaxEmpresa", faxEmpresa);
				arrParams[5] = new SqlParameter("@inEmailEmpresa", emailEmpresa);
				
				arrParams[6] = new SqlParameter("@inActivo", activo);
				
				arrParams[7] = new SqlParameter("@inUsername", username);

				arrParams[8] = new SqlParameter("@bFacturacao", bFacturacao);
				arrParams[9] = new SqlParameter("@bOrcamento", bOrcamento);
				arrParams[10] = new SqlParameter("@bQualidade", bQualidade);
				arrParams[11] = new SqlParameter("@bManutencao", bManutencao);
				arrParams[12] = new SqlParameter("@bCertificados", bCertificados);
				arrParams[13] = new SqlParameter("@bRequisicoes", bRequisicoes);
				arrParams[14] = new SqlParameter("@inDepartamento", departamento);
                arrParams[15] = new SqlParameter("@bGestaoEquipamentos", bGestaoEquipamentos);


				int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdContactoCurto", arrParams); //retorna zero se corre bem
				return retValue.ToString(); 

				//				if (retValue == 0)
				//					return GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
				//				else
				//					return GERAL.clsGeral.ErrorMessage.ERR_UPDATE;    
			}

			public DataTable DTEmpresas()
			{
				return GERAL.clsDataAccess.SPExecuteDT("stpGetEmpresas"); 
			}

//			public DataTable DTContactos()
//			{
//				return GERAL.clsDataAccess.SPExecuteDT("stpGetContactos"); 
//			}
        }
    }
