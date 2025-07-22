using System;
using System.Data.SqlClient; 
using System.Security.Cryptography; 
using System.Text; 
using System.Configuration; 
using System.Data; 
using System.Collections; 
using System.Web.SessionState; 
using System.Security; 
using System.Web.UI; 
using System.Web; 
using System.ComponentModel; 
using LabMetro.GERAL; 


namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for UtilizadoresBD.
	/// </summary>
	/// 
//    public class PermissionsBD
//    {
//        
//        public Hashtable HTPermissions; // = new Hashtable(); 
//        //public string strPerm; 
//
//    }

        
        
        public class UtilizadoresBD
        {
            public UtilizadoresBD()
            {
            }
            
            
            
            //chamado logo a seguir ao login, no default, retorna uma hashtable com as permissoes
            //correspondentes ao perfil do utilizador. hashtable é dps guardada numa variavel de 
            //sessao: Session["HTPermissions"]
            public Hashtable GetPermissions(int intProfileId)
            {

              //  HTPermissions.Clear(); 

                SqlDataReader DR = drPaginasComAcesso(intProfileId); 
                
                Hashtable ht = new Hashtable(); 

                if(DR.HasRows)
                {
                    
                    while(DR.Read())
                    {
                         ht.Add(DR["shortname"],DR["acessoTotal"]);  
                    }
        
                  }
                DR.Close();
                return ht; 
               
            }
     
//            public PermissionsBD GetPermissions(int intProfileId)
//            {
//                PermissionsBD myPermissions = new PermissionsBD(); 
//
//                DATA.UtilizadoresBD util = new LabMetro.DATA.UtilizadoresBD(); 
            //                SqlDataReader DR = util.drPaginasComAcesso(intProfileId); 
            //                if(DR.HasRows)
            //                {
            //                    PermissionsBD xxx = new PermissionsBD(); 
            //                    Hashtable ht = new Hashtable(); 
            //
            //                    while(DR.Read())
            //                    {
            //                        
            //                      //  xxx.HTPermissions.Add(DR["shortname"],DR["acessoTotal"]);      
            //                      ht.Add(DR["shortname"],DR["acessoTotal"]);  
            //                    }
            //                   xxx.HTPermissions=ht; 
            //                   return xxx; 
            //
            //                }
            //                else
            //                {
            //                    return null; 
            //                }
//
//            }

//            // Função que autentica um utilizador perante a base de dados
//            public bool authenticateUser(string strUserName, string strPassword)
//            {
//                string strSQL = "SELECT dbo.udfAuthenticateUser('" + strUserName + "','" + strPassword + "')"; 
//                return (bool)GERAL.clsDataAccess.myExecuteScalar(strSQL);
//				
//            }

			public bool bAuthenticateUser(string strUserName, string strPassword, string strIP)
			{
				SqlParameter[] arrParams = new SqlParameter[3];

				arrParams[0] = new SqlParameter("@inUsername", strUserName);
				arrParams[1] = new SqlParameter("@inPassword", strPassword);
				arrParams[2] = new SqlParameter("@strIP", strIP);

				int i = clsDataAccess.SPExecuteNonQueryRetVal("stpAuthenticateUser", arrParams); 
				if (i == 0) return false; 
				return true; 

			}
            //funcao  q indica se password tem de ser alterada, nao está bem testado ainda
//            public int checkPasswordStatus(string username)
//            {
//                string strSQL = "SELECT alterarPassword FROM Utilizador WHERE userName = '"+username; 
//
//                return (int)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
//                
//            }
            // Função que devolve o ID de um utilizador com base no seu username
            public int UserId(string userName)
            {
                string strSQL = "SELECT dbo.udfGetUserId('" + userName + "')"; 
                return (int)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
            }

			public string idGrandezeUser(string userName)
			{
				string strSQL = "SELECT dbo.udfGetIdGrandeza('" + userName + "')"; 
				return (string)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
			
			}

            public int ProfileId(string userName) 
            {
                string strSQL = "SELECT dbo.udfGetProfileId('" + userName + "')"; 
                return (int)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
            }

            public string registerExternalUser(string strUserName, string strPassword)
            {
                string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];

                //            MD5CryptoServiceProvider m5Hasher = new MD5CryptoServiceProvider(); 
                //            UTF8Encoding encoder = new UTF8Encoding(); 
                //            Byte[] hashedbytes = m5Hasher.ComputeHash(encoder.GetBytes(strPassword)); 
                //            string hashValue = BitConverter.ToString(hashedbytes).Replace("-",""); 
                //            
                //            m5Hasher = null; 
                //            encoder = null; 

                //o IDPErfil - fixo - do user externo tem de ser guardado nalgum sitio como constante
                //string strSQL = "INSERT INTO tbUtilizadores(NomeUtilizador,Password, IDPerfil, dtCriacao, dtAlteracao) VALUES('"+strUserName +"','"+hashValue+"',1,getDate(),getDate())";

                string strSQL = "INSERT INTO Utilizador(idPerfil,username,passwd, activo, alterarPassword, idUtilCriacao, dtCriacao, idUtilAlteracao,dtAlteracao) VALUES(2,'"+strUserName +"','"+strPassword+"',1,0,0,getDate(),0,getDate())";

                SqlConnection objConn =  new SqlConnection(connectionString); 
                SqlCommand objCmd  = new SqlCommand(strSQL, objConn); 
                objConn.Open();
   
                string strMsg =strSQL;
            
                try
                {
                    objCmd.ExecuteNonQuery(); 
                    
                }
                catch
                {
                    
                }
                finally
                {
                    objConn.Close();
                    objConn = null; 
                }
                return strMsg; 
            
                //ao mesmo tempo q ele se regista, tem de lhe ser atribuido o perfil de utilizador externo,  e os dados dele serem inseridos na tabela de clientes? empresas? entidades=?
            }

            // Função que devolve uma Lista com todos os Funcionários
            public DataTable DTListaFuncionarios()
            {
                SqlParameter[] arrParams = new SqlParameter[1];
            
                arrParams[0] = new SqlParameter("@inActiv", "1");
                
                DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListFuncionarios", arrParams); 
           
                return DT;
            }

            // Função que devolve uma Lista com todos Contactos de uma determinada Empresa
            public DataTable DTListaContactos(string idEmpresa)
            {
                SqlParameter[] arrParams = new SqlParameter[2];
            
                arrParams[0] = new SqlParameter("@inActiv", "1");
             
                arrParams[1] = new SqlParameter("@inIdEmpresa", idEmpresa);
         
                DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetListContactos", arrParams); 
           
                return DT;
            }

            // Funcao que insere/actualiza um Utilizador e devolve a msg de erro
            public string ExecuteUsers(string tableName, string id, string idPerfil, string SessionUser,string UserName, string Password)
            {
                SqlParameter[] arrParams = new SqlParameter[6];

                arrParams[0] = new SqlParameter("@inTable", tableName);
                arrParams[1] = new SqlParameter("@inId", id);
                arrParams[2] = new SqlParameter("@inIdPerfil", idPerfil);
                arrParams[3] = new SqlParameter("@inUsernameAccao", SessionUser);
                arrParams[4] = new SqlParameter("@inUsername", UserName);
                arrParams[5] = new SqlParameter("@inPassword", Password);

                int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpInsUpdUtilizador", arrParams); 

                if (retValue == 0)
                    return GERAL.clsGeral.ErrorMessage.MSG_DB;
                else if(retValue == 2601) //violacao....

                    return GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY; 
                    //else if (retValue == 2)
                    //return GERAL.clsGeral.ErrorMessage.ERR_USERNAME;
                else
                    return GERAL.clsGeral.ErrorMessage.ERR_DB;//+retValue.ToString();                                   
            } 

            //funcao para quem tiver permissoes para mudar as passwords alterar as passwords do utilizaor
            //usado na Gestao de Passwords
            public string UpdatePassword(string id, string SessionUser,string Password)
            {
                SqlParameter[] arrParams = new SqlParameter[3];

                arrParams[0] = new SqlParameter("@inId", id);
                arrParams[1] = new SqlParameter("@inPassword", Password);
                arrParams[2] = new SqlParameter("@inUsernameAccao", SessionUser);

                int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpUpdPassword", arrParams); 

                if (retValue == 0)
                    return GERAL.clsGeral.ErrorMessage.MSG_DB;
                else
                    return GERAL.clsGeral.ErrorMessage.ERR_DB;//+retValue.ToString();                                   
            } 
        
            //funcao para o próprio mudar a sua password
            public string ChangePassword(string username, string oldPassword, string newPassword)
            {
                SqlParameter[] arrParams = new SqlParameter[3];

                arrParams[0] = new SqlParameter("@inUsername", username);
                arrParams[1] = new SqlParameter("@inOldPassword", oldPassword);
                arrParams[2] = new SqlParameter("@inNewPassword", newPassword);

                int retValue = GERAL.clsDataAccess.SPExecuteNonQueryRV("stpChangePassword", arrParams); 

                if (retValue == 0)
                    return GERAL.clsGeral.ErrorMessage.MSG_DB;
                else
                    return GERAL.clsGeral.ErrorMessage.ERR_DB; 
          
            }
            public int validaPagina(string UserName, string ShortNamePagina)
            {
                string strSQL = "SELECT dbo.udfUserHasPagePermission('" + UserName + "','"+ShortNamePagina+"')"; 
                return (int)GERAL.clsDataAccess.myExecuteScalar(strSQL); 
            
            
            }



            //devolve os items de menu para os botoes. nao devolve os subitems.
            public string strListMenuItems(string idPerfil) //em principio ele ha de receber o userdId ou Username, isabel: depois mudar quando fizeres a funcao
            
            {
                string strSQL = "SELECT DISTINCT dbo.Menu.shortname AS menuItem FROM dbo.Perfil_Pagina INNER JOIN dbo.Pagina ON dbo.Perfil_Pagina.idPagina = dbo.Pagina.idPagina INNER JOIN dbo.Menu ON dbo.Pagina.idMenu = dbo.Menu.idMenu WHERE (dbo.Perfil_Pagina.idPerfil = "+idPerfil+")"; 
                SqlDataReader myReader = GERAL.clsDataAccess.ExecuteDR(strSQL); 
            
                string strLista = ""; 
            
           
                while (myReader.Read())
                {
                    strLista+= myReader.GetString(0); 
                    strLista+=",";  
                }
                string delimStr = ",";
                char [] delimiter = delimStr.ToCharArray();
                strLista = strLista.TrimEnd(delimiter); 
                
                myReader.Close(); 

                return strLista; 
                
            }

            public DataTable dtPaginas(string idPerfil)
            {
        
                SqlParameter[] arrParams = new SqlParameter[1];
            
                arrParams[0] = new SqlParameter("@inIdPerfil",idPerfil);
            
                DataTable DT = LabMetro.GERAL.clsDataAccess.SPExecuteDTParams("stpGetPermissoesPerfil", arrParams); 
                
                return DT;
            }


            public SqlDataReader drPaginasComAcesso(int idPerfil)
            {
        
                SqlParameter[] arrParams = new SqlParameter[1];
            
                arrParams[0] = new SqlParameter("@inIdPerfil",idPerfil);
            
                SqlDataReader DR = LabMetro.GERAL.clsDataAccess.SPExecuteDRParams("stpGetPagePermissions", arrParams); 
           
                return DR;
                
            }

            // public Hashtable myHT = new Hashtable(); 

//            public void FillHTPermissions()
//            {
//                DATA.UtilizadoresBD util = new LabMetro.DATA.UtilizadoresBD(); 
//                SqlDataReader DR = util.drPaginasComAcesso("0"); 
//                if(DR.HasRows)
//                {
//                    while(DR.Read())
//                    {
//                        HTPermissions.Add(DR["shortname"],DR["acessoTotal"]);           
//                    }
//                }
//            }
        }
    }
