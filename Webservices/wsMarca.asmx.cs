using System;
using System.Collections.Generic;

using System.Web;
using System.Web.Services;

using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;

namespace LabMetro.Webservices
{
    /// <summary>
    /// Summary description for wsMarca
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class wsMarca : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMarcas(string prefix)
        {
            List<string> marcas = new List<string>();
            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
               

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select idMarca, descricao from marca where " +
                    "descricao like @SearchText + '%' order by descricao";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            marcas.Add(string.Format("{0}-{1}", sdr["descricao"], sdr["idMarca"]));
                        }
                    }
                    conn.Close();
                }
                return marcas.ToArray();
            }
        }
    }
}
