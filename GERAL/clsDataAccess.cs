using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration; 
using System.Data.OleDb; 


namespace LabMetro.GERAL
{
    /// <summary>
    /// Summary description for clsDataAccess.
    /// </summary>
    /// 

	
	//acho q nao pode ser static, pq uma classe statica noa pode conter instance constructors (new...)
	//When we use static keyword before a class name, we specify that the class will only have static member variables and methods. Such classes cannot be instantiated as they don’t need to: they cannot have instance variables. Also an important point to note is that such static classes are sealed by default, which means they cannot be inherited further. 
//	As already written above, we need static classes when we know that our class will not have any behavior as such. 
	//Suppose we have a set of helper or utility methods which we would like to wrap together in a class. Since these methods are generic in nature we can define them all inside a static class. Remember that helper or utility methods need to be called many times and since they are generic in nature there is no need to create instances. For e.g. suppose that you need a method that parses an int to a string. This method would come in the category of a utility or helper method.
//	So using static keyword will make your code a bit faster since on object creation is involved. 

	

    public sealed class clsDataAccess
    {


		public static int iCommandTimeOut =System.Convert.ToInt16(ConfigurationManager.AppSettings["commandTimeOut"].ToString());
		
		
		
//==============================================================================
        //recebe uma string de sql e retorna um datareader
		//==============================================================================
        public static SqlDataReader ExecuteDR(string strSQL)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			
            SqlConnection objConn = new SqlConnection(connectionString); 
            SqlCommand objCmd = new SqlCommand(strSQL,objConn); 
			objCmd.CommandTimeout = iCommandTimeOut; 

            try
            {
                objConn.Open();
				return objCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                objConn.Close();
                throw; 
            }
        }

        //recebe uma string de sql e retorna um datareader
        //==============================================================================
        public static SqlDataReader ExecuteDREspanha(string strSQL)
        {
            string connectionString = (string)ConfigurationManager.AppSettings["connectionstringESPANHA"];

            SqlConnection objConn = new SqlConnection(connectionString);
            SqlCommand objCmd = new SqlCommand(strSQL, objConn);
            objCmd.CommandTimeout = iCommandTimeOut;

            try
            {
                objConn.Open();
                return objCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                objConn.Close();
                throw;
            }
        }

		//==============================================================================
		//a mesma coisa mas recebe o commando e a connection
		//==============================================================================
		public static SqlDataReader ExecuteDR(SqlConnection myConnection, SqlCommand myCommand,string strSQL)
		{
			myCommand.CommandText= strSQL;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = CommandType.Text; 
			return  myCommand.ExecuteReader();
		}


		//==============================================================================
		//recebe uma string de sql e retorna um sclar OBJECT
		//==============================================================================
        public static object myExecuteScalar(string strSQL)
        {
            
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];

            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
            
            {      
				objCmd.CommandTimeout = iCommandTimeOut; 
                objConn.Open();
                return objCmd.ExecuteScalar(); 
            }
        }

        //==============================================================================
        //recebe uma string de sql e retorna um sclar OBJECT
        //==============================================================================
        public static object myExecuteScalarBDEspanha(string strSQL)
        {

            string connectionString = (string)ConfigurationManager.AppSettings["connectionstringESPANHA"];

            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand(strSQL, objConn))
            {
                objCmd.CommandTimeout = iCommandTimeOut;
                objConn.Open();
                return objCmd.ExecuteScalar();
            }
        }

		
		//==============================================================================
		//recebe uma stored procedure e um arrParams e retorna um sclar OBJECT
		//==============================================================================
		public static object myExecuteScalarSP(string commandText, params SqlParameter[] commandParameters)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];

			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			  using (SqlCommand objCmd = new SqlCommand(commandText,objConn))
			{      
				objCmd.CommandTimeout = iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				

				foreach (SqlParameter p in commandParameters)
				{
					objCmd.Parameters.Add(p);
				}

				objConn.Open(); 
				return objCmd.ExecuteScalar(); 
			}
		}


		//==============================================================================
		//recebe uma string de sql e retorna um dataset
		//==============================================================================
        public static DataSet ExecuteDS(string strSQL) 
        {

            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
            {
                    objCmd.CommandTimeout = iCommandTimeOut; 
                    //objConn.Open();
                    SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
                    DataSet objDS = new DataSet(); 

                    objDA.Fill(objDS);
                                    
                    return objDS; 
                
            }
        }

		//==============================================================================
		//recebe uma string de sql e o nome do dataset e da table que é para ser preenchida 
		//e retorna um dataset
		//==============================================================================
		public static DataSet DSFillDS(string strSQL, DataSet DS,string TableName) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
			{
                        
				objCmd.CommandTimeout = iCommandTimeOut; 
				//objConn.Open();
				SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
				objDA.Fill(DS,TableName);
                                    
				return DS; 
                
			}

		}

		//==============================================================================
		//recebe uma stored procedure e o nome do dataset e da table que é para ser preenchida 
		//e retorna um dataset
		//==============================================================================
		public static DataSet DSFillDS_SP(string commandText, DataSet DS,string TableName) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
                
				objCmd.Connection = objConn; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.CommandText = commandText; 
				objCmd.CommandTimeout = iCommandTimeOut; 

				//objConn.Open();

				SqlDataAdapter objDA = new SqlDataAdapter(objCmd);

				try
				{
					objDA.Fill(DS,TableName);
					return DS; 
				}
				catch(Exception ex)
				{
					GERAL.clsWriteError.WriteLog(ex.Message); 
					return null;
				}
			}
		}

		//==============================================================================
		//recebe uma stored procedure, parametros, e o nome do dataset e da table que é para ser preenchida 
		//e retorna um dataset
		//==============================================================================
		public static DataSet DSFillDS_SP_Params(string commandText, DataSet DS,string TableName,params SqlParameter[] commandParameters) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
                
				objCmd.Connection = objConn; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				objCmd.CommandText = commandText; 
				objCmd.CommandTimeout = iCommandTimeOut; 

				foreach (SqlParameter p in commandParameters)
				{
					 if (((p.Value == null)  || (p.Value.ToString()=="")))
                    {
                        p.Value = DBNull.Value;
                    }
					objCmd.Parameters.Add(p);
				}

				SqlDataAdapter objDA = new SqlDataAdapter(objCmd);

				try
				{
					objDA.Fill(DS,TableName);
					return DS; 
				}
				catch(Exception ex)
				{
					GERAL.clsWriteError.WriteLog(ex.Message); 
					return null;
				}
			}
		}
        
		//==============================================================================
        //recebe ums string de sql e retorna uma datatable
		//==============================================================================
        public static DataTable ExecuteDT(string strSQL) 
        {

            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
            {
				objCmd.CommandTimeout = iCommandTimeOut; 

                SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
                DataTable objDT = new DataTable(); 
                
                try
                {
//                    objConn.Open();
                
                    objDA.Fill(objDT);
                                    
                    return objDT; 
                }
                catch(Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex); 
                    return null; 
                }
            }
        }

		//==============================================================================
		//recebe ums string de sql + parametros e retorna uma datatable
		//==============================================================================
		public static DataTable ExecuteDT(string strSQL,  params SqlParameter[] commandParameters) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
			{
				objCmd.CommandTimeout = iCommandTimeOut; 
				SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
				DataTable objDT = new DataTable(); 
                
				try
				{
					//objConn.Open();

					objCmd.CommandType = CommandType.Text; 

					foreach (SqlParameter p in commandParameters)
					{
							
						objCmd.Parameters.Add(p);
					}
                
					objDA.Fill(objDT);
                                    
					return objDT; 
				}
				catch(Exception ex)
				{
					GERAL.clsWriteError.WriteLog(ex); 
					return null; 
				}
			}
		}

		//==============================================================================
		//recebe ums string de sql e retorna uma datatable
		//==============================================================================
		public static DataTable csvSAPExecuteDT(string strSQL) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["csvConnSAP"];
    
			using (OleDbConnection objConn = new OleDbConnection(connectionString)) 
			using (OleDbCommand objCmd = new OleDbCommand(strSQL,objConn))
			{
				OleDbDataAdapter objDA = new OleDbDataAdapter(objCmd);
				DataTable objDT = new DataTable(); 
                
				try
				{
					objConn.Open();
                
					objDA.Fill(objDT);
                                    
					return objDT; 
				}
				catch(Exception ex)
				{
					GERAL.clsWriteError.WriteLog(ex); 
					return null; 
				}
			}
		}


		//==============================================================================
		//recebe ums string de sql e retorna uma datatable
		//==============================================================================
		public static DataTable csvSAPLOGSExecuteDT(string strSQL) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["csvConnSAPLOGS"];
    
			using (OleDbConnection objConn = new OleDbConnection(connectionString)) 
			using (OleDbCommand objCmd = new OleDbCommand(strSQL,objConn))
			{
				OleDbDataAdapter objDA = new OleDbDataAdapter(objCmd);
				DataTable objDT = new DataTable(); 
                
				try
				{
					objConn.Open();
                
					objDA.Fill(objDT);
                                    
					return objDT; 
				}
				catch(Exception ex)
				{
					GERAL.clsWriteError.WriteLog(ex); 
					return null; 
				}
			}
		}


		//==============================================================================
		//a mesma coisa mas recebe o commando e a connection
		//==============================================================================
		public static DataTable  ExecuteDT(SqlConnection myConnection, SqlCommand myCommand,string strSQL)
		{
			myCommand.CommandText= strSQL;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = CommandType.Text; 


			SqlDataAdapter objDA = new SqlDataAdapter(myCommand);
			DataTable objDT = new DataTable(); 
            
			objDA.Fill(objDT);                        
			return objDT; 
		}

		//==============================================================================
        //recebe o nome de uma stored procedure e retorna uma datatable
		//==============================================================================
        public static DataTable SPExecuteDT(string commandText)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))
            {
				
				objCmd.CommandTimeout = iCommandTimeOut; 

                objCmd.CommandType = CommandType.StoredProcedure; 
                SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
                DataTable objDT = new DataTable(); 

                //objConn.Open();
                objDA.Fill(objDT);
                                    
                return objDT;       
            }
        }

		//==============================================================================
        //recebe o nome de uma stored procedure e um array de parametros e retorna uma datatable
		//==============================================================================
        public static DataTable SPExecuteDTParams(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))
            {
            	objCmd.CommandTimeout = iCommandTimeOut; 
                objCmd.CommandType = CommandType.StoredProcedure; 

                foreach (SqlParameter p in commandParameters)
                {
                    //check for derived output value with no value assigned
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null || p.Value.ToString() == ""))
                        {
                            p.Value = DBNull.Value;
                        }
                        objCmd.Parameters.Add(p);
                    }
                }
                SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
                DataTable objDT = new DataTable(); 

                //objConn.Open();
                
                objDA.Fill(objDT);
                                    
                return objDT;                   
            }
        }

		//==============================================================================
        //recebe o nome de uma stored procedure e um array de parametros e retorna um DataSet
		//==============================================================================
        public static DataSet SPExecuteDSParams(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))
            {
            
				objCmd.CommandTimeout = iCommandTimeOut; 
                objCmd.CommandType = CommandType.StoredProcedure; 

                foreach (SqlParameter p in commandParameters)
                {
                    //check for derived output value with no value assigned
                    //                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    //                    {
                    //                        p.Value = DBNull.Value;
                    //                    }
                    //				
                    objCmd.Parameters.Add(p);
                }
                SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
                DataSet objDS = new DataSet(); 

                //objConn.Open();
                objDA.Fill(objDS);
                                    
                return objDS;                  
            }
        }

		//==============================================================================
		//recebe o nome de uma stored procedure e um array de parametros e retorna um datareader
		//==============================================================================
		public static SqlDataReader SPExecuteDRParams(string commandText, params SqlParameter[] commandParameters)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			SqlConnection objConn = new SqlConnection(connectionString); 
			SqlCommand objCmd = new SqlCommand(commandText,objConn); 
			{
            
				objCmd.CommandTimeout = iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 
				foreach (SqlParameter p in commandParameters)
				{
					objCmd.Parameters.Add(p);
				}
				try
				{
					objConn.Open();
					SqlDataReader myReader = objCmd.ExecuteReader(CommandBehavior.CloseConnection);
					return myReader;
				}
				catch
				{
					objConn.Close();
					throw; 
				}               
			}
		}

		//==============================================================================
		//recebe o nome de uma stored procedure e retorna um datareader
		//==============================================================================
		public static SqlDataReader SPExecuteDR(string commandText)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			SqlConnection objConn = new SqlConnection(connectionString); 
			SqlCommand objCmd = new SqlCommand(commandText,objConn); 
		    {
            	objCmd.CommandTimeout = iCommandTimeOut; 
				objCmd.CommandType = CommandType.StoredProcedure; 

				try
				{
					objConn.Open();
					SqlDataReader myReader = objCmd.ExecuteReader(CommandBehavior.CloseConnection);
					return myReader;
				}
				catch
				{
					objConn.Close();
					throw; 
				}               
		    }
		}

		//==============================================================================
        //recebe uma string de sql, executa a string e retorna um int
		//==============================================================================
        public static int myExecuteNonQuery(string strSQL)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
            {
				objCmd.CommandTimeout = iCommandTimeOut; 
                objConn.Open();
                try
                {
                    return objCmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex.ToString()+"-"+strSQL);

                    return 0; 

                }

            }

        }
		
		//==============================================================================
		public static int myExecuteNonQuery(SqlConnection myConnection, SqlCommand myCommand,string myCommandText)
		{
			myCommand.CommandText= myCommandText;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = CommandType.Text; 
			
			return myCommand.ExecuteNonQuery(); 
		}

		//==============================================================================
		public static int myExecuteNonQuery(SqlConnection myConnection, SqlCommand myCommand,string myCommandText,CommandType myCommandType,params SqlParameter[] commandParameters)
		{
			myCommand.CommandText= myCommandText;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = myCommandType;//CommandType.StoredProcedure;
			
			if(myCommand.Parameters.Count >0) myCommand.Parameters.Clear(); 

			foreach (SqlParameter p in commandParameters)
			{
				myCommand.Parameters.Add(p);
			}
			
			return myCommand.ExecuteNonQuery(); 
		}

		//==============================================================================
		//==============================================================================
		//==============================================================================
        public static int myExecuteNonQueryParams(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
   
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))
            {
                objCmd.CommandType = CommandType.Text; 
				objCmd.CommandTimeout = iCommandTimeOut; 
 
                    foreach (SqlParameter p in commandParameters)
                    {
                        if ((p.Value == null) || (p.Value.ToString() == ""))// || (p.Value.ToString() == "0"))
                        {
                            p.Value = DBNull.Value; 
                        }
				
                        objCmd.Parameters.Add(p);
                    }
               
                    objConn.Open();
                
                try
                {
                    return objCmd.ExecuteNonQuery();
                }

                catch(Exception ex)
                {
                    GERAL.clsWriteError.WriteLog(ex.ToString()+"-"+commandText);
                    return 0; 
                }   
            }

        }

		//==============================================================================
        //a mesma coisa mas recebe o commando e a connection
		//==============================================================================
        public static void  myExecuteNonQueryParams(SqlConnection myConnection, SqlCommand myCommand,string myCommandText, params SqlParameter[] commandParameters)
        {
            myCommand.CommandText= myCommandText;
            myCommand.Connection = myConnection; 
            myCommand.CommandType = CommandType.Text; 
            myCommand.Parameters.Clear();
                
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
				
                myCommand.Parameters.Add(p);
            }

            myCommand.ExecuteNonQuery(); 
        }

		//==============================================================================
		//a mesma coisa mas recebe o commando e a connection
		//==============================================================================
		public static int  intMyExecuteNonQueryParams(SqlConnection myConnection, SqlCommand myCommand,string myCommandText, params SqlParameter[] commandParameters)
		{
			myCommand.CommandText= myCommandText;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = CommandType.Text; 
			myCommand.Parameters.Clear();
                
			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}
				
				myCommand.Parameters.Add(p);
			}

			return myCommand.ExecuteNonQuery(); 
		}

		//==============================================================================
		//a mesma coisa mas recebe o commando e a connection SEM PARAMS
		//==============================================================================
		public static int  intMyExecuteNonQuery(SqlConnection myConnection, SqlCommand myCommand,string myCommandText)
		{
			myCommand.CommandText= myCommandText;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = CommandType.Text; 
			
            
			return myCommand.ExecuteNonQuery(); 
		}

		//==============================================================================
		
		//==============================================================================
		public static int intMyExecuteNonQuery(string strSQL,  params SqlParameter[] commandParameters) 
		{

			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
		
			
			
			{

				if(objCmd.Parameters.Count > 0) objCmd.Parameters.Clear();
				objCmd.CommandTimeout = iCommandTimeOut; 
				
				try
				{
					objCmd.CommandType = CommandType.Text; 

					foreach (SqlParameter p in commandParameters)
					{			
						objCmd.Parameters.Add(p);
					}
                
					objConn.Open();
					return objCmd.ExecuteNonQuery();                 
					
				}
				catch(Exception ex)
				{
					GERAL.clsWriteError.WriteLog(ex); 
					return -1; 
				}
			}
		}



        public static int myExecuteNonQueryWithErrNumber(string strSQL)
        {
               
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(strSQL,objConn))
            {
				objCmd.CommandTimeout = iCommandTimeOut; 
                objConn.Open();
              
                try
                {
                    return objCmd.ExecuteNonQuery();
                }
                catch(SqlException ex)
                {
                    GERAL.clsWriteError.WriteLog(ex.ToString()+"-"+strSQL);

                    int exNbr = ex.Number; 
                    return exNbr; 
                    //if(exNbr == "2627") ret =objReadXML.ReadErrorFile("ERR_DUPLIC_KEY"); 

                }

            }

        }


		//==============================================================================
		//recebe a connection e o command - int. passou de void para int 05.03.2007
		//==============================================================================
		public static int ExecuteNonQuerySPConn_retExNbr(SqlConnection myConnection,SqlCommand myCommand, string myCommandText, params SqlParameter[] commandParameters)
		{
         
			if(myCommand.Parameters.Count > 0) myCommand.Parameters.Clear();
			myCommand.CommandText= myCommandText;
               
			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}	
				myCommand.Parameters.Add(p);
			}
			try
			{
				return myCommand.ExecuteNonQuery();
			}
			catch(SqlException ex)
			{

				GERAL.clsWriteError.WriteLog(ex.ToString());
				System.Web.HttpContext.Current.Response.Write(ex.Message.ToString()); 
				System.Web.HttpContext.Current.Response.Write(ex.InnerException.ToString()); 

				return ex.Number; 
				
			}
		}

		//==============================================================================
        //recebe ums string de sql, executa a string, e retorna uma string com a msg de erro
		//==============================================================================
        public static string ExecuteNonQueryDB(string strSQL)
        {		
            string ret = ""; 
			
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			
            SqlConnection objConn = new SqlConnection(connectionString); 
            SqlCommand objCmd = new SqlCommand(strSQL,objConn); 
            objCmd.CommandTimeout = iCommandTimeOut; 
			objConn.Open();
            try
            {
                objCmd.ExecuteNonQuery();
                ret = GERAL.clsGeral.ErrorMessage.MSG_DB; 
            }
				
            catch(System.Data.SqlClient.SqlException ex) 
            {
                string exNbr = ex.Number.ToString(); 
                ret= GERAL.clsGeral.ErrorMessage.ERR_DB; 
                if(exNbr == "2627") ret =GERAL.clsGeral.ErrorMessage.ERR_DUPLICATE_KEY; 
            }
			
            catch(Exception ex)
            {
                ret=ex.ToString();
            }
			
            finally 
            {
                objConn.Close(); 
            
            }
                return ret;
        }

        //==============================================================================
        //recebe ums string de sql, executa a string, e retorna uma string com a msg de erro
        //==============================================================================
        public static int ExecuteNonQueryDB_ESPANHA(string strSQL)
        {
            int ret = -1;

            string connectionString = (string)ConfigurationManager.AppSettings["connectionstringESPANHA"];

            SqlConnection objConn = new SqlConnection(connectionString);
            SqlCommand objCmd = new SqlCommand(strSQL, objConn);
            objCmd.CommandTimeout = iCommandTimeOut;
            objConn.Open();
            try
            {
               ret= objCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GERAL.clsWriteError.WriteLog(ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
            return ret;
        }

		
		//==============================================================================
        //recebe o nome de uma stored procedure e um array de parametros e executa uma query, retornando um inteiro
		//==============================================================================
        public static int ExecuteNonQuerySP(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))
            {
				objCmd.Parameters.Clear();  //sem isto tenho tido problemas estranhos.
				
				objCmd.CommandTimeout = iCommandTimeOut; 
                objCmd.CommandType= CommandType.StoredProcedure; 
				
				try
				{
					foreach (SqlParameter p in commandParameters)
					{
						//check for derived output value with no value assigned
						//isso devia ser apenas .Input, mas como isso tb se reflecte nos selects, agora é tarde								para mudar. 
						if ((p.Direction == ParameterDirection.InputOutput) && ((p.Value == null) ||(p.Value.ToString()=="")) )
						{
							p.Value = DBNull.Value;
						}
				
						objCmd.Parameters.Add(p);
					}

					objConn.Open();
					return objCmd.ExecuteNonQuery(); 
					

				}
				catch(ArgumentException ex) //isto provoca um erro mas depois funciona /caso de ser chamado no brecalibext, set servico disabled....
				{
					
					System.Web.HttpContext.Current.Response.Write(ex.ToString()); 
					return -1;
				}
				catch(Exception ex) //isto provoca um erro mas depois funciona /caso de ser chamado no brecalibext, set servico disabled....Exception exe
				{

                    //System.Web.HttpContext.Current.Response.Write(ex.ToString()); 
                    GERAL.clsWriteError.WriteLog(ex.ToString());
                    return -1;
				}
            }
        }

		//==============================================================================
		//recebe a connection e o command - int. passou de void para int 05.03.2007
		//==============================================================================
		public static int ExecuteNonQuerySP(SqlConnection myConnection,SqlCommand myCommand, string myCommandText, params SqlParameter[] commandParameters)
		{
         
			if(myCommand.Parameters.Count > 0) myCommand.Parameters.Clear();
			myCommand.CommandText= myCommandText;
               
			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}	
				myCommand.Parameters.Add(p);
			}
			return myCommand.ExecuteNonQuery(); 
		}


		//==============================================================================
		// VOID
		//==============================================================================
		public static void vExecuteNonQuerySP(string commandText, params SqlParameter[] commandParameters)
		{	   
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand(commandText,objConn))

			{
				objCmd.CommandTimeout = iCommandTimeOut; 
				objCmd.CommandType= CommandType.StoredProcedure; 
                
				foreach (SqlParameter p in commandParameters)
				{
					//check for derived output value with no value assigned
					if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
					{
						p.Value = DBNull.Value;
					}
				
					objCmd.Parameters.Add(p);
				}
               
				objConn.Open();

				objCmd.ExecuteNonQuery(); 
			}
		}
		

		//==============================================================================
		//recebe a connection e o command - INT
		//==============================================================================
		public static int intExecuteNonQuerySP(SqlConnection myConnection,SqlCommand myCommand, string myCommandText, params SqlParameter[] commandParameters)
		{
         
			myCommand.CommandText= myCommandText;
			myCommand.Connection = myConnection; 
			myCommand.CommandType = CommandType.StoredProcedure; 
			myCommand.Parameters.Clear();
                
			foreach (SqlParameter p in 	commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}
				
				myCommand.Parameters.Add(p);
			}

			return myCommand.ExecuteNonQuery(); 
            
		}

        public static int ExecuteNonQuerySPOutput(SqlConnection myConnection,SqlCommand myCommand, string myCommandText, params SqlParameter[] commandParameters)
        {
            myCommand.CommandText= myCommandText;
            myCommand.Connection = myConnection; 
            myCommand.CommandType = CommandType.StoredProcedure; 
			myCommand.Parameters.Clear();
                
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
				
                myCommand.Parameters.Add(p);
            }

            SqlParameter outParam = new SqlParameter("@out", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output; 
            myCommand.Parameters.Add(outParam); 

            myCommand.ExecuteNonQuery(); 
            int id = (int)outParam.Value; 
           
            return id; 
            
        }


        public static string ExecuteNonQuerySPOutput_BD_PTCOMERCIAL(string myCommandText, params SqlParameter[] commandParameters)
        {
            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionStringPTCOMERCIAL"];

            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand(myCommandText, objConn))
            {
                objCmd.CommandTimeout = iCommandTimeOut;
                objCmd.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter p in commandParameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    objCmd.Parameters.Add(p);
                }

                objConn.Open();



                SqlParameter outParam = new SqlParameter("@out", SqlDbType.NVarChar, 150);
                outParam.Direction = ParameterDirection.Output;
                objCmd.Parameters.Add(outParam);

                objCmd.ExecuteNonQuery();
                string id = (string)outParam.Value;

                return id;
                
            }
        }

        public static string ExecuteNonQuerySPOutput_BD_VD(string myCommandText, params SqlParameter[] commandParameters)
        {
            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionStringVD"];

            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand(myCommandText, objConn))
            {
                objCmd.CommandTimeout = iCommandTimeOut;
                objCmd.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter p in commandParameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    objCmd.Parameters.Add(p);
                }

                objConn.Open();

                SqlParameter outParam = new SqlParameter("@out", SqlDbType.NVarChar,150);
                outParam.Direction = ParameterDirection.Output;
                objCmd.Parameters.Add(outParam);

                objCmd.ExecuteNonQuery();
                string ordem = (string)outParam.Value;

                return ordem;
            }
        }

        public static int ExecuteScalarSPRetVal_BD_VD(string myCommandText, params SqlParameter[] commandParameters)
        { 
            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionStringVD"];

            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand(myCommandText, objConn))
            {
				objCmd.CommandTimeout = iCommandTimeOut; 
                objCmd.CommandType= CommandType.StoredProcedure; 
                
                foreach (SqlParameter p in commandParameters)
                {
                   
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
				
                    objCmd.Parameters.Add(p);
                }
               
                SqlParameter returnvalue = new SqlParameter("@ReturnValue", SqlDbType.Int);
                returnvalue.Direction = ParameterDirection.ReturnValue; 
                objCmd.Parameters.Add(returnvalue); 

                objConn.Open();
            
                objCmd.ExecuteNonQuery(); 

                return (int)returnvalue.Value; 
            }
        }
        



		public static int ExecuteNonQuerySPOutput(string myCommandText, params SqlParameter[] commandParameters)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand(myCommandText,objConn))

			{
				objCmd.CommandTimeout = iCommandTimeOut; 
				objCmd.CommandType= CommandType.StoredProcedure; 
                
				foreach (SqlParameter p in commandParameters)
				{
					//check for derived output value with no value assigned
					if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
					{
						p.Value = DBNull.Value;
					}
				
					objCmd.Parameters.Add(p);
				}
               
				objConn.Open();

				SqlParameter outParam = new SqlParameter("@out", SqlDbType.Int);
				outParam.Direction = ParameterDirection.Output; 
				objCmd.Parameters.Add(outParam); 

				objCmd.ExecuteNonQuery(); 
				int id = (int)outParam.Value; 
           
				return id; 
			}
		}


        

        //o return value foi cortado pq quando estoira pelo meio nao retorna o return value certo
        //agora retorna 0 quando corre bem e o ex.number quando corre mal
        //ver se isto está bem em todo o sitio
        //retorna 0 quando corre bem, 2 quando ha uma chave duplicada, e outra coisa qq quando corre mal.
        public static int SPExecuteNonQueryRV(string commandText, params SqlParameter[] commandParameters)
        {
            
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))

            {
				objCmd.CommandTimeout = iCommandTimeOut; 
                objCmd.CommandType= CommandType.StoredProcedure; 
                
                foreach (SqlParameter p in commandParameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
				
                    objCmd.Parameters.Add(p);
                }
               
                SqlParameter returnvalue = new SqlParameter("@ReturnValue", SqlDbType.Int);
                returnvalue.Direction = ParameterDirection.ReturnValue; 
                objCmd.Parameters.Add(returnvalue); 

                objConn.Open();
            
                try
                {
                    objCmd.ExecuteNonQuery(); 
                    return 0;
                }

                
                catch(SqlException ex)
                {

                    GERAL.clsWriteError.WriteLog(ex);
                    int ret = 1; 
                    if(ex.ToString()== "2627") ret = 2;  //retorna 2 quando é chave duplicada
                    return ret; 

                }
            }
        }
        
		//==============================================================================
        //vou criar esta para a empresa, pq é preciso apanhar o retvalue....
		//==============================================================================
        public static int SPExecuteNonQueryRetVal(string commandText, params SqlParameter[] commandParameters)
        {
            
            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
            using (SqlConnection objConn = new SqlConnection(connectionString)) 
            using (SqlCommand objCmd = new SqlCommand(commandText,objConn))

            {
				objCmd.CommandTimeout = iCommandTimeOut; 
                objCmd.CommandType= CommandType.StoredProcedure; 
                
                foreach (SqlParameter p in commandParameters)
                {
                   
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
				
                    objCmd.Parameters.Add(p);
                }
               
                SqlParameter returnvalue = new SqlParameter("@ReturnValue", SqlDbType.Int);
                returnvalue.Direction = ParameterDirection.ReturnValue; 
                objCmd.Parameters.Add(returnvalue); 

                objConn.Open();
            
                objCmd.ExecuteNonQuery(); 

                return (int)returnvalue.Value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myConnection"></param>
        /// <param name="myCommand"></param>
        /// <param name="myCommandText"></param>
        ///         /// <param name="commandParameters"></param>
        /// <returns></returns>
        // recebe a connection e o command
        public static int SPExecuteNonQueryRV(SqlConnection myConnection,SqlCommand myCommand, string myCommandText, params SqlParameter[] commandParameters)
        {
           
            myCommand.CommandText = myCommandText; 
            myCommand.Connection = myConnection; 
            myCommand.CommandType= CommandType.StoredProcedure; 
			myCommand.Parameters.Clear();
                
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
			
                myCommand.Parameters.Add(p);
            }
            
            SqlParameter returnvalue = new SqlParameter("@ReturnValue", SqlDbType.Int);
            returnvalue.Direction = ParameterDirection.ReturnValue; 
            myCommand.Parameters.Add(returnvalue); 

            myCommand.ExecuteNonQuery(); 

            return (int)returnvalue.Value; 
            
        }

		//isto não tem muito a ver aqui mas vai ficar aqui//
		//chamada à stp de insert na tabela de auditoria, dentro de uma transaccao

		

    }
}
