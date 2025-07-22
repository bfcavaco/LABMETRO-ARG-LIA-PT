using System;
using System.Web; 



namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for clsGeral.
	/// </summary>
    public class clsGeral
    {
        public clsGeral()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        
		public class ErrorMessage
		{
            //========================================================================================
            //ERROS **********************************************************************************
            //========================================================================================

            //BD =====================================================================================
            public static string  ERR_INSERT = Resources.Resource.ERR_INSERT; // "Erro na inserção do registo. Registo não inserido";
            public static string  ERR_UPDATE = Resources.Resource.ERR_UPDATE; // "Erro na actualização do registo. Registo não actualizado";
            public static string  ERR_DELETE = Resources.Resource.ERR_DELETE; //"Erro ao apagar registo. Operação não efectuada.";
            public static string  ERR_DB = Resources.Resource.ERR_DB; // "Operação não efectuada.";
            public static string  ERR_DUPLICATE_KEY = Resources.Resource.ERR_INSERT; //"Chave duplicada.";

            //de accesso e gerais =====================================================================
            public static string  ERR_SESSION_TIMEOUT = Resources.Resource.ERR_SESSION_TIMEOUT; //"A sessão expirou. Por favor volte a registar-se.";
            public static string ERR_GENERAL_ERROR = Resources.Resource.ERR_GENERAL_ERROR; //"Erro de aplicação. Por favor tente mais tarde.";
            public static string  ERR_USER_REDIRECT = Resources.Resource.ERR_USER_REDIRECT; //"Tentou aceder a uma página para a qual não tem permissões. Por favor registe-se novamente";
            public static string  ERR_USERNAME = Resources.Resource.ERR_USERNAME; // "Já existe um utilizador com este userName.";
            public static string  ERR_LOGIN = Resources.Resource.ERR_LOGIN; //"Login inválido"; 

            //outros            
            public static string  ERR_CRIACAO_FICHEIRO = Resources.Resource.ERR_CRIACAO_FICHEIRO; //"Erro na criação do ficheiro. Ficheiro não foi criado.";
            public static string  ERR_JA_EXISTE_EMPRESA_NIF = Resources.Resource.ERR_JA_EXISTE_EMPRESA_NIF; //"Tem de associar a empresa a um Grupo pois já existe uma empresa com o NIF indicado.";
            public static string  ERR_NAO_TEM_MESMO_NIF = Resources.Resource.ERR_NAO_TEM_MESMO_NIF; //"A empresa não pode ser associada ao Grupo escolhido pois não tem o mesmo Nº de Contribuinte.";
            public static string  ERR_INDICAR_MAIL_FUNCIONARIO = Resources.Resource.ERR_INDICAR_MAIL_FUNCIONARIO; //"Tem de ter uma conta de email. Actualiza a informação do funcionário.";
            public static string  ERR_CLIENTE_SEM_NUMFAX = Resources.Resource.ERR_CLIENTE_SEM_NUMFAX; //"O cliente não tem nº de fax.";
            public static string  ERR_CLIENTE_SEM_EMAIL = Resources.Resource.ERR_CLIENTE_SEM_EMAIL; //"O cliente não tem endereço de email.";
            //valores incorrectos
            public static string  ERR_VALORES_INCORRECTOS = Resources.Resource.ERR_VALORES_INCORRECTOS; //"Valore(s) incorrecto(s).";
            public static string  ERR_DATA_FIM_SUPERIOR_DATA_INICIO = Resources.Resource.ERR_DATA_FIM_SUPERIOR_DATA_INICIO; //"A data de início não pode ser superior à data de fim.";
            public static string  ERR_DATAS_INCORRECTAS = Resources.Resource.ERR_DATAS_INCORRECTAS; //"Data(s) incorrecta(s=.";
            public static string  ERR_EMAIL_FORMATO_INVALIDO = Resources.Resource.ERR_EMAIL_FORMATO_INVALIDO; //"Email em formato inválido.";
            
            
            //ficheiros
            public static string  ERR_CARREGAMENTO_FICHEIRO = Resources.Resource.ERR_CARREGAMENTO_FICHEIRO; //"Erro no carregamento do ficheiro.";


            //=====================================================================================
            //CONFIRMAÇÔES*************************************************************************
            //=====================================================================================
            //BD
            public static string  MSG_INSERT_DB = Resources.Resource.MSG_INSERT_DB; //"Registo inserido com sucesso.";
            public static string MSG_UPDATE_DB = Resources.Resource.MSG_UPDATE_DB; //"Registo(s) actualizado(s) com sucesso.";
            public static string  MSG_DELETE_DB = Resources.Resource.MSG_DELETE_DB; //"Registo apagado com sucesso.";
            public static string  MSG_DB = Resources.Resource.MSG_DB; // "Operação efectuada com sucesso.";
            public static string  MSG_NO_RESULTS = Resources.Resource.MSG_NO_RESULTS; //"Não foram encontrados registos para a pesquisa efectuada.";
            public static string  MSG_NO_RECORDS = Resources.Resource.MSG_NO_RECORDS; //"Não existem registos";
            public static string  MSG_NO_DATA = Resources.Resource.MSG_NO_DATA; //"Não existem dados para este registo.";
            public static string  MSG_TROCA_EFECTUADA = Resources.Resource.MSG_TROCA_EFECTUADA; //"Troca efecutada com sucesso"; 

            

            //MENSAGENS
            //seleccionar
            public static string  MSG_SELECCIONE_GRANDEZA = Resources.Resource.MSG_SELECCIONE_GRANDEZA; //"Por favor seleccione uma grandeza.";
            public static string  MSG_SELECCIONE_ESTADO = Resources.Resource.MSG_SELECCIONE_ESTADO; //"Por favor seleccione um estado.";
            public static string  MSG_SELECCIONE_EMPRESA = Resources.Resource.MSG_SELECCIONE_EMPRESA; //"Por favor seleccione uma empresa";
            public static string  MSG_SELECCIONE_BRE = Resources.Resource.MSG_SELECCIONE_BRE; //"Por favor seleccione um BRE.";
            public static string  MSG_SELECCIONE_CONTACTO = Resources.Resource.MSG_SELECCIONE_CONTACTO; // "Tem de seleccionar um contacto.";
            public static string  MSG_SELECCIONE_TIPOEQUIPAMENTO = Resources.Resource.MSG_SELECCIONE_TIPOEQUIPAMENTO; // "Tem de seleccionar um tipo de equipamento.";

            //indicar
            public static string  MSG_INDICAR_FAX = Resources.Resource.MSG_INDICAR_FAX; //"Por favor indique um número de fax.";
            public static string  MSG_INDICAR_EMAIL = Resources.Resource.MSG_INDICAR_EMAIL; //"Por favor indique um endereço de email.";
            public static string  MSG_INDICAR_PARAMS = Resources.Resource.MSG_INDICAR_PARAMS; // "Por favor introduza os parâmetros seguintes:";
            public static string  MSG_INDICAR_EMPRESA = Resources.Resource.MSG_INDICAR_EMPRESA; // "Por favor indique uma empresa.";
            public static string  MSG_EMPRESA_SEDE_TIRAR_GRUPO = Resources.Resource.MSG_EMPRESA_SEDE_TIRAR_GRUPO; //"A empresa é sede, o Grupo tem de estar vazio.";
            public static string  MSG_INDICAR_TIPO_SERVICO = Resources.Resource.MSG_INDICAR_TIPO_SERVICO; //"Tem de indicar um tipo de Serviço";
            public static string  MSG_INDICAR_DESCRICAO = Resources.Resource.MSG_INDICAR_DESCRICAO; //"Tem de indicar a descrição.";
            public static string  ERR_MISSING_FIELDS = Resources.Resource.ERR_MISSING_FIELDS; //"Tem de preencher todos os campos.";
            
            //pesquisas
            public static string  MSG_SEARCH = Resources.Resource.MSG_SEARCH; //"Por favor preencha um critério de pesquisa.";
            public static string  MSG_MINIMUM_SEARCHCONDITION = Resources.Resource.MSG_MINIMUM_SEARCHCONDITION; //"Tem de preencher pelo menos um  critério de pesquisa.";
            public static string  MSG_PESQUISA_SERVICOS = Resources.Resource.MSG_PESQUISA_SERVICOS; //"Tem de preencher no mínimo 1 critério de Pesquisa. Se pesquisar por Marca e/ou Modelo, tem de juntar outro critério (ex. Grandeza)."; 

            //inserção de registos

            public static string  MSG_ASSOCIAR_EMPRESA_GRUPO = Resources.Resource.MSG_ASSOCIAR_EMPRESA_GRUPO; //"Tem de associar a empresa a um grupo.";
            public static string  MSG_PREENCHER_NUMSERIE_OU_NUMIDENT = Resources.Resource.MSG_PREENCHER_NUMSERIE_OU_NUMIDENT; //"Tem de preencher o número de série ou o número de identificação.";
            public static string  MSG_INSERIR_NUM_IDENTIFICACAO = Resources.Resource.MSG_INSERIR_NUM_IDENTIFICACAO; // "Tem de inserir o número de identificação do equipamento.";
            public static string  MSG_INSERIR_MIN_1EQUIPAMENTO = Resources.Resource.MSG_INSERIR_MIN_1EQUIPAMENTO; //"Tem de inserir pelo menos um equipamento na lista.";
            public static string  MSG_INSERIR_EMAIL = Resources.Resource.MSG_INSERIR_EMAIL; //"Tem de inserir um endereço de email";
            public static string MSG_DAR_ACESSO_LABMETRO_ONLINE = Resources.Resource.MSG_DAR_ACESSO_LABMETRO_ONLINE; //"Tem de inserir um endereço de email";

            public static string  MSG_INSERIR_LINHA_FACTURA = Resources.Resource.MSG_INSERIR_LINHA_FACTURA; //"Tem de inserir pelo menos uma linha na factura.";
            public static string  MSG_INSERIR_REGIAO_VENDAS = Resources.Resource.MSG_INSERIR_REGIAO_VENDAS; //"Existem calibrações externa. Actualize a região de vendas da empresa.<br />e carregue no botão 'Actualizar Região' para gravar as alterações.";
            public static string  MSG_NUMCLIENTE_SAP_INEXISTENTE = Resources.Resource.MSG_NUMCLIENTE_SAP_INEXISTENTE; //"Este Nº SAP não existe ainda nas nossas tabelas.";
            public static string  MSG_PERFIL_NAO_NECESSITA_GRANDEZAS = Resources.Resource.MSG_PERFIL_NAO_NECESSITA_GRANDEZAS; // "Este perfil não necessita de ser associado a grandezas!";
            public static string  MSG_NAO_EXISTEM_REQUISICOES = Resources.Resource.MSG_NAO_EXISTEM_REQUISICOES; //"Não existem requisições para a empresa seleccionada.";
            public static string  MSG_INDICAR_PAI_ADITAMENTO = Resources.Resource.MSG_INDICAR_PAI_ADITAMENTO; //"Para aditamentos tem de seleccionar um servico pai.";
            public static string  MSG_REMOVER_PAI = Resources.Resource.MSG_REMOVER_PAI; //"Somente deve indicar um serviço pai para os Aditamentos.";
            public static string  MSG_CONFIRMACAO_MARCACAO = Resources.Resource.MSG_CONFIRMACAO_MARCACAO; //"Marcação feita.";
            public static string  MSG_MARCACAO_APAGADA = Resources.Resource.MSG_MARCACAO_APAGADA; //"Marcação apagada";
            public static string  MSG_NAO_EXISTEM_CONTACTOS = Resources.Resource.MSG_NAO_EXISTEM_CONTACTOS; //"Não existem contactos inseridos nesta empresa.<br />Tem de inserir um contacto primeiro.";
            public static string  MSG_INSERIR_EQUIPAMENTO = Resources.Resource.MSG_INSERIR_EQUIPAMENTO; //"Tem de inserir um equipamento"; 

            //ficheiros

            public static string  MSG_FICH_FACTURA = Resources.Resource.MSG_FICH_FACTURA; //"Ficheiro de Facturação criado com sucesso.";
            public static string  MSG_DOCS_VALIDACAO = Resources.Resource.MSG_DOCS_VALIDACAO; //"Não escolheu nenhum documento.";
            public static string  MSG_DOCS_UPLOAD = Resources.Resource.MSG_DOCS_UPLOAD; //"Documento carregado.";
            public static string  MSG_DOCS_ALERTA_REFRESH = Resources.Resource.MSG_DOCS_ALERTA_REFRESH; // "Os documentos carregados só aparecem se carregar novamente no Item de Menu 'Validar Documentos.'";
            public static string  MSG_DOCS_PENDENTES = Resources.Resource.MSG_DOCS_PENDENTES; //"Não existem calibrações pendentes para validação.";
            public static string  ERR_JA_EXISTE_FICHEIRO_NOME = Resources.Resource.ERR_JA_EXISTE_FICHEIRO_NOME; //"Já existe um ficheiro com o mesmo nome.";
            public static string  ERR_FILE_EXTENSION = Resources.Resource.ERR_FILE_EXTENSION; //"Erro com alguma extensão de ficheiro.";

			


            //BLOQUEIOS EMPRESAS
            public static string  MSG_AUMENTAR_BLOQ_AMARELO_LARANJA = Resources.Resource.MSG_AUMENTAR_BLOQ_AMARELO_LARANJA; // "Tem de aumentar o nível de bloqueio interno para amarelo ou laranja.";
            public static string  MSG_BLOQ_DEVE_ESTAR_LIVRE = Resources.Resource.MSG_BLOQ_DEVE_ESTAR_LIVRE; //"O nível de bloqueio tem de estar livre.";
            public static string  MSG_LIMPAR_BLOQUEIO_INTERNO = Resources.Resource.MSG_LIMPAR_BLOQUEIO_INTERNO; //"Tem de limpar o nivel de bloqueio interno.";
            public static string  MSG_NIVEL_BLOQ_TEM_ESTAR_VERMELHO = Resources.Resource.MSG_NIVEL_BLOQ_TEM_ESTAR_VERMELHO; //"O nível de bloqueio tem de estar vermelho";
            public static string  MSG_NIVEL_NAO_PODE_ESTAR_VERMELHO = Resources.Resource.MSG_NIVEL_NAO_PODE_ESTAR_VERMELHO; //"O nível de bloqueio não pode estar a vermelho."; 


            
		}

		// recebe "false" ou "true" e devolve "0" ou "1" respectivamente
        public static byte ConvertStringToBool(string str) 
        { 	
            byte ret = 0; 
            if (String.Compare(str, "True") == 0) ret = 1;			
            return ret; 
        }

		// recebe "0" ou "1" e devolve "false" ou "true" respectivamente
 		public static bool ConvertStringToBoolean(string str) 
		{ 	 
			if ("1".Equals(str))
			{
				return true;
			}
			return false; 
			
		}

        // recebe "0" ou "1" e devolve "false" ou "true" respectivamente
        public static bool ConvertBStringToBoolean(string str) 
        { 	 
			if (str=="True")
			{
				return true;
			}
			
			return false; 
        }

		// recebe "0" ou "1" e devolve "false" ou "true" respectivamente
		public static bool ConvertObjectToBoolean(object o) 
		{ 	 
			string s = (string)o; 

			if (s=="True")
			{
				return true;
			}
			
			return false; 
		}

		// Converte uma string (com uma data longa) para uma string (com uma data curta)
		public static string ToShortDate(string str) 
		{ 	 
			try
			{
				return DateTime.Parse(str).ToShortDateString();
			}
			catch
			{
				return "";
			}

		}

        public static string ConverteEstado(bool b)
        {
            if (b==true) 
            {
                return Resources.Resource.Activo;
            }
            return Resources.Resource.Inactivo;
            
        }

        public static string ConverteBoolSimNao(bool b)
        {
            if (b==true) 
            {
                return Resources.Resource.Sim; 
            }
            return Resources.Resource.Nao;
        }

		public static double ConvertStringToDouble(string str)
		{
			if (str.Equals(""))
			{
				return 0;
			}
			return Double.Parse(str);
			
		}

        public static int ConvertStringToInt(string str)
        {
			if (str.Equals(""))
			{
				return 0;
			}
			return Int32.Parse(str);
        }


        //os campos money da bd sao recebidos nos datagrids como system.dbnull ou system.decimal
        //tenho de converte-los para double
        public static string ConvertDBMoneyToCurrencyString(object obj)
        {
        
            
            if(!Convert.IsDBNull(obj))
            {
                decimal dec = Decimal.Parse(obj.ToString()); 
                return dec.ToString("C"); 
            }
            return ""; 
        }
        
        public static string ConvertDBMoneyToString(object obj)
        {
        
           
			if(!Convert.IsDBNull(obj))
			{
				decimal dec = Decimal.Parse(obj.ToString()); 
				dec = Decimal.Round(dec,2);
				return dec.ToString(); 
			}
            return ""; 
        }

		public static bool invBool(bool x)
		{
			if (x==true)
			{
				return false;
			}
			return true;
		}
       
        public static string RoundString2(string s)
        {
            try
            {   
                decimal dec = Decimal.Parse(s); 
                dec = Decimal.Round(dec,2);
                return dec.ToString(); 
            }
            catch //se nao consegue, retorna a string q entrou
            {
                return s;     
            }
        }

        public static string convertDecimalSeparator(string x)//converts the decimalseparator "," to "."
        {
            return x.Replace(",",".");
        }
    }
}
