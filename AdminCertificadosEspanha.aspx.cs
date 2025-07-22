using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using LabMetro.GERAL;
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
using System.Text;


namespace LabMetro
{
    /// <summary>
    /// Summary description for AdminSAPFiles.
    /// </summary>
    public partial class AdminCertificadosEspanha : System.Web.UI.Page
    {
        //223 utilizador chamaod "responsable tecnico" para a aplicacao de espanha para constar da tabela certificad apenas! id funcionario 237

        //idTipoCertificado	descricao	sigla
        //1	Primeiro Certificado	1C

        //3	1ª Revisão	1R

        //7	2ª Revisão	2R
        //8	3ª Revisão	3R
        //11	4ª Revisão	4R
     

        //a fazer:

        //todos os certificados na pasta ORIGEM, em que os serviços estejam nos estados equivalentes ao workflow
        //teem de ser lidos, um por um e tem de ser actualizado o estado do serviço para o estado seguinte, tem de ser criado um registo na tabela certificados e tem de ser copiado o ficheiro para a pasta de certificados finais.

        //tambem tem de haver um datagrid com todos os serviços que precisam de certificados. e colocar nesta pagina o botao para fazer upload dos certificados.

        private static string pastaCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];
        private static string pastaOrigem = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGINAIS"];
        //private static string pastaConstrucao = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CONSTRUCAO"];

        private const string ID_PAG = "IMP_CERTIF_0";//NOME DA PAGINA


         protected void Page_Load(object sender, System.EventArgs e)
        {
            Hashtable ht = (Hashtable)Session["HTPermissions"];
            if (ht == null) //session expired
            {
                Server.Transfer("Default.aspx?err=2", false);
            }


            lblMessage.Text = "";
            //if (!Page.IsPostBack)
            //{
                EspanhaApagaFicheirosErrados();
                BindGrid();
                
           // }
            //EspanhaLoopThroughFiles();
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        protected void btnupload_Click(object sender, EventArgs e)
        {

  
            string path = (string)ConfigurationManager.AppSettings["PATHREL_CERT_ORIGINAIS"];
            //string path = (string)System.Configuration.ConfigurationManager.AppSettings["UPLOAD_PEDIDOSORC_PATH_REL"];
            //string savepath = Server.MapPath("~/" + path);
            string myPath = Server.MapPath("~/" + path);


            for (int i = 0; i < Request.Files.Count; i++)
            {
                //In IE8, this behavior has changed and it will ONLY pass the file name, not the full path. ;-)
                //em ie, ele vai buscar o camimho todo ho hpf.filename e logo dá erro a seguir
                //mesmo assim o upload multiplo nao funciona em ie.
                HttpPostedFile hpf = Request.Files[i];
                string filename = hpf.FileName.ToUpper();
                string filename2 = Path.GetFileName(hpf.FileName).ToUpper();
               
                try
                {
                    hpf.SaveAs(myPath + "/" + filename2);
                    Response.Write(filename2 + " - OK" + "<br />");
                }
                catch (Exception ex)
                {

                    Response.Write("Error a carregar el fichero " + myPath + "/" + filename2 + ". " + ex.ToString());
                    clsWriteError.WriteLog(ex.ToString());
                }

            }

            BindGrid();
            //todos os ficheiros vao parar para uma pasta temporaria
            // a partir dai vou correr uma funcao que copia todos os certificados para a pasta final e que no meio disto actualiza a bd.
            //se é um 1c, 1R, 

        } 

        private void BindGrid()
        {

            DirectoryInfo dirInfo = new DirectoryInfo(pastaOrigem);

            FileInfo[] files = dirInfo.GetFiles("*.pdf");
            IComparer comp = new LabMetro.GERAL.CompareFileInfo(); //ordena por data
            Array.Sort(files, comp);

            dgFiles.DataSource = files;
            dgFiles.DataBind();
        }


       
        //===============================================================================================
		//TRUE SE FICHEIRO TEM UMA NOMENCLATURA CORRECTA; FALSE SE NÃO TEM
		//===============================================================================================
		bool isValidFileName(string strIn)
		{
			// Return true if strIn is in valid  format.
			//calibracao, aditamento, revisao, via
			//qq carcater depois 2 numeros um traco um numero e um caracatcer (c,a,r,v) e ponto pdf
			string myPattern = ".*\\-[0-9]{2}\\-[0-9]{1}[c|C|a|A|r|R|v|V]{1}\\.[pP][dD][fF]"; 
			return Regex.IsMatch(strIn,myPattern); 
		}

        private void EspanhaApagaFicheirosErrados()
        {
            DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaOrigem);
            FileInfo[] filesOrigem = dirInfoPastaOrigem.GetFiles();
            IComparer comp = new LabMetro.GERAL.CompareFileInfo(); //ordena por data(+hora)
            Array.Sort(filesOrigem, comp);

            if (filesOrigem.Length > 0)
            {

                for (int i = 0; i < filesOrigem.Length; i++)
                {
                    string[] documento = filesOrigem[i].ToString().Split("-,.".ToCharArray());

                    if (documento.Length != 4) //tem um nome mal formado
                    {
                        try
                        {
                            filesOrigem[i].Delete(); //apaga da pasta de origem!.[
                        }
                        catch
                        {
                        }
                        continue;
                    }

                    //podem fazer ambos coisas par
                    if (!isValidFileName(filesOrigem[i].ToString()))
                    {
                        try
                        {
                            filesOrigem[i].Delete(); //apaga da pasta de origem!.
                        }
                        catch (Exception e)
                        {
                            GERAL.clsWriteError.WriteLog("linha 144" + e.ToString());
                            if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
                            {
                                lblMessage.Text += filesOrigem[i].ToString().ToUpper() + " - Fichero en uso por otro programa.<br />";
                            }
                        }
                        continue;
                    }
                }
            }
        }


        private void EspanhaLoopThroughFiles()
        {
            string refServico = "";
            string idServico = "";
           
            string sigla = "";

            DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaOrigem);
            FileInfo[] filesOrigem = dirInfoPastaOrigem.GetFiles();
            IComparer comp = new CompareFileInfo(); //ordena por data(+hora)
            Array.Sort(filesOrigem, comp);

            if (filesOrigem.Length > 0)
            {

                for (int i = 0; i < filesOrigem.Length; i++)
                {
                    refServico = this.refServico(filesOrigem[i].ToString());
                    sigla = this.sigla(filesOrigem[i].ToString());
                    string caminhoDestino = pastaCertificados + "\\" + filesOrigem[i].ToString().ToUpper();
                    string caminhoOrigem = pastaOrigem + "\\" + filesOrigem[i].ToString().ToUpper();

                   
                   idServico = Convert.ToString (clsDataAccess.myExecuteScalar("SELECT idServico FROM servico where refServico = '" + refServico + "'"));
                   if(idServico==null)
                   {
                       lblMessage.Text += "No existe el servicio: " + refServico;
                       continue;
                   }
                  

                    switch (sigla)
                    {

                        case "1C":
                            trata1C(refServico, idServico, filesOrigem[i], caminhoDestino);
                            break;
                        case "1R":
                            trata1R(refServico, idServico, filesOrigem[i], caminhoDestino);
                            break;
                        case "2R":
                            trata2R(refServico, idServico, filesOrigem[i], caminhoDestino);
                            break;
                        case "3R":
                            trata3R(refServico, idServico, filesOrigem[i], caminhoDestino);
                            break;
                        case "4R":
                            trata4R(refServico, idServico, filesOrigem[i], caminhoDestino);
                            break;
                       
                    }

                    dirInfoPastaOrigem = null;
                                    }
            }
        }
        
        //se for um 1C o estado do Servico tem de estar em 15, 6 ou 25 e nao pode existir nenhum certificado na tabela dos certificados
        //neste caso, actualizo o serviço para estado com certificado e insiro na tabela certificado e movo o ficheiro.
        private bool trata1C(string refServico, string idServico, FileInfo filesOrigem,string caminhoDestino)
        {
            string strSQL;
            string strSQL2;
            string strSQL3;
            string strSQL4;

               string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
               using (SqlConnection objConn = new SqlConnection(connectionString))
               using (SqlCommand objCmd = new SqlCommand())
               {
                   objCmd.Connection = objConn;
                   objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                   objConn.Open();


                    objCmd.CommandText = "SELECT idServico FROM Servico where idServico = '" + idServico + "' AND idEstadoServico in (6,15,25,10,24)"; //verificar se está no estado correcto
					object idser = objCmd.ExecuteScalar();

                    
					if(idser == null) //se o idServico is null  O SERVICO ESTÁ NO ESTADO ERRADO
					{
                        lblMessage.Text += "Verifica el estado del Servicio" + refServico + ". Debe estar Calibrado o Entregue o Calibrado en el Exterior ou Subcontratación Calibrada ou Entregado (subcont)." + "<br />";
                        return false;
                    }
                
                    objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idServico = '" + idServico +"'"; //verificar se ja tem certificado
                    object idcert = objCmd.ExecuteScalar();

                    if (idcert != null) //se existe algum certificado //se existe algum certificado
                    {
                        lblMessage.Text += "Ya existe un certificado  1C para el Servicio " + refServico + "<br />";
                        return false;
                    }

                    using (SqlTransaction objTrans = objConn.BeginTransaction())
                    {
                        objCmd.Transaction = objTrans;

                        strSQL = "UPDATE servico SET idEstadoServico = 23, idEstadoCertificado = 4, idUtilAlteracao = " + HttpContext.Current.Session["UserId"].ToString() + " WHERE idservico = '" + idServico + "' AND servico.idEstadoServico in (15,25)";
                            objCmd.CommandText = strSQL;
                            int x1 = objCmd.ExecuteNonQuery();

                            strSQL2 = "UPDATE servico SET idEstadoServico = 12, idEstadoCertificado = 4, idUtilAlteracao = " + HttpContext.Current.Session["UserId"].ToString() + "  WHERE refServico = '" + refServico + "' AND servico.idEstadoServico = 6";
                            objCmd.CommandText = strSQL2;
                            int x2 = objCmd.ExecuteNonQuery();


                        //coisas novas à pressa
                            strSQL3 = "UPDATE servico SET idEstadoServico = 18, idEstadoCertificado = 4, idUtilAlteracao = " + HttpContext.Current.Session["UserId"].ToString() + " WHERE refServico = '" + refServico + "' AND servico.idEstadoServico = 10";
                            objCmd.CommandText = strSQL3;
                            int x3 = objCmd.ExecuteNonQuery();

                            strSQL4 = "UPDATE servico SET idEstadoServico = 23, idEstadoCertificado = 4,idUtilAlteracao = " + HttpContext.Current.Session["UserId"].ToString() + "  WHERE refServico = '" + refServico + "' AND servico.idEstadoServico = 24";
                            objCmd.CommandText = strSQL4;
                            int x4 = objCmd.ExecuteNonQuery();



                            if (x1 == 1 || x2 == 1 || x3 == 1 || x4 == 1 )//se uma ou outra correm bem entao crio o certificado na tabela certificado.
                            {
                                strSQL = "INSERT INTO certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) VALUES (" + idServico + ", 1, 237, getdate(),null,getdate(),223,getdate(),223)";

                                objCmd.CommandText = strSQL;

                                if (objCmd.ExecuteNonQuery() == 1) 
                                {
                                    //isto é o último passo. mover o ficheiro e se o ficheiro for movido, commit transaction
                                    try
                                    {

                                        filesOrigem.CopyTo(caminhoDestino, false);
                                        filesOrigem.Delete();
                                        objTrans.Commit();
                                        lblMessage.Text += "SUCESO al cargar el Certificado " + filesOrigem.ToString() + "<br />";
                                        return true;
                                       
                                    }
                                    catch(Exception ex)
                                    {
                                        objTrans.Rollback();
                                        lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString() + ex.ToString() + "<br />";
                                        return false;
                                    }
                                    
                                }
                                else
                                {  
                                    
                                    objTrans.Rollback();
                                    lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString() +". Verifique si existe algun servicio associado ou apague el fichero.<br />";
                                return false;
                                
                                }
                            }
                            else
                            {
                                objTrans.Rollback();
                                lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString() +". Verifique si existe algun servicio associado ou apague el fichero.<br />";
                                return false;

                            }
                      } //end using transaction
               }
        }

       

        
                            //        //aqui, tem de ter ja um certificado  1C primeiro e não pode ter nenhum outro R. portanto o servico tem de estar em estado 4
                            //        //insere um novo registo na tabela certificados e move o ficheiro

                            //        //verificar se já existe:
                            //        strSQL = "SELECT COUNT(idCertifcicado) FROM CERTIFICADO WHERE idServico = " + idServico + " AND idTipoCertificado in (3,7,8)"; //todas as revisoes

                            //        //se noa houver e se o estado do servico = 4 entao insere
        //        strSQL = "INSERT INTO certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) VALUES (" + idServico + ", 3, 237, getdate(),null,getdate(),223,getdate(),223)";

                            //        //depois move o ficheiro

        private bool trata1R(string refServico, string idServico, FileInfo filesOrigem,string caminhoDestino)
        {

             string strSQL;

               string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
               using (SqlConnection objConn = new SqlConnection(connectionString))
               using (SqlCommand objCmd = new SqlCommand())
               {
                   objCmd.Connection = objConn;
                   objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                   objConn.Open();


                    objCmd.CommandText = "SELECT idServico FROM Servico where idServico = " + idServico + " AND idEstadoCertificado = 4"; // in (6,15,25)"; //verificar se está no estado correcto

                    object idser = objCmd.ExecuteScalar();
					if(Convert.IsDBNull(idser)) //SE NAO HA NADA O SERVICO ESTÁ NO ESTADO ERRADO
                    {
                        lblMessage.Text += "Verifica el estado del Servicio" + refServico + " y si existe un certificado 1C. <br />";
                        return false;
                    }
                
                    objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 3 and idServico = " + idServico; //verificar se ja tem certificado
                    object idCert = objCmd.ExecuteScalar();

                    if (idCert!=null) //se existe algum certificado
					{
                        lblMessage.Text += "Ya existe un certificado  1R para el Servicio " + refServico + "<br />";
                        return false;
                    }

                    using (SqlTransaction objTrans = objConn.BeginTransaction())
                    {
                        objCmd.Transaction = objTrans;


                        strSQL = "INSERT INTO certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) VALUES (" + idServico + ", 3, 237, getdate(),null,getdate(),223,getdate(),223)";
                      objCmd.CommandText = strSQL;

                        if(objCmd.ExecuteNonQuery() != 1)
                        {
                            return false;
                        }

                        //isto é o último passo. mover o ficheiro e se o ficheiro for movido, commit transaction
                        try
                        {
                            
                            filesOrigem.CopyTo(caminhoDestino, false);
                            filesOrigem.Delete();
                            objTrans.Commit();
                            lblMessage.Text += "SUCESO al cargar el Certificado " + filesOrigem.ToString() + "<br />";
                            return true;
                            
                        }
                        catch
                        {
                            objTrans.Rollback();
                            lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString();
                            return false;
                        }
                       
                    }
                           
                } //end using transaction
              
        }

        private bool trata2R(string refServico, string idServico, FileInfo filesOrigem, string caminhoDestino)
        {

            string strSQL;

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objConn.Open();


                objCmd.CommandText = "SELECT idServico FROM Servico where idServico = " + idServico + " AND idEstadoCertificado = 4"; // in (6,15,25)"; //verificar se está no estado correcto

                object idser = objCmd.ExecuteScalar();
                if (Convert.IsDBNull(idser)) //SE NAO HA NADA O SERVICO ESTÁ NO ESTADO ERRADO
                {
                    lblMessage.Text += "Verifica el estado del Servicio" + refServico + " y si existe un certificado 1C. <br />";
                    return false;
                }

                //se ainda nao ha um 1R tem de criar um 1R primeiro
                objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 3 and idServico = " + idServico; //verificar se ja tem certificado
                object idCert = objCmd.ExecuteScalar();

                if (idCert == null) //se existe algum certificado
                {
                    lblMessage.Text += "Debe crear primero un 1R para el servicio " + refServico + "<br />";
                    return false;
                }

                objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 7 and idServico = " + idServico; //verificar se ja tem certificado
                idCert = objCmd.ExecuteScalar();

                if (idCert != null) //se existe algum certificado
                {
                    lblMessage.Text += "Ya existe un certificado  2R para el Servicio " + refServico + "<br />";
                    return false;
                }

                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;


                    strSQL = "INSERT INTO certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) VALUES (" + idServico + ", 7, 237, getdate(),null,getdate(),223,getdate(),223)";
                    objCmd.CommandText = strSQL;

                    if (objCmd.ExecuteNonQuery() != 1)
                    {
                        return false;
                    }

                    //isto é o último passo. mover o ficheiro e se o ficheiro for movido, commit transaction
                    try
                    {

                        filesOrigem.CopyTo(caminhoDestino, false);
                        filesOrigem.Delete();
                       objTrans.Commit();
                        lblMessage.Text += "SUCESO al cargar el Certificado " + filesOrigem.ToString() + "<br />";
                        return true;
                    }
                    catch
                    {
                        objTrans.Rollback();
                        lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString();
                        return false;
                    }
                   
                }

            } //end using transaction

        }


        private bool trata3R(string refServico, string idServico, FileInfo filesOrigem, string caminhoDestino)
        {
            string strSQL;

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objConn.Open();


                objCmd.CommandText = "SELECT idServico FROM Servico where idServico = " + idServico + " AND idEstadoCertificado = 4"; // in (6,15,25)"; //verificar se está no estado correcto

                object idser = objCmd.ExecuteScalar();
                if (Convert.IsDBNull(idser)) //SE NAO HA NADA O SERVICO ESTÁ NO ESTADO ERRADO
                {
                    lblMessage.Text += "Verifica el estado del Servicio" + refServico + " y si existe un certificado 1C. <br />";
                    return false;
                }

                //se ainda nao ha um 1R tem de criar um 1R primeiro
                objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 7 and idServico = " + idServico; //verificar se ja tem certificado
                object idCert = objCmd.ExecuteScalar();

                if (idCert == null) //se existe algum certificado
                {
                    lblMessage.Text += "Debe crear primero un 2R para el servicio " + refServico + "<br />";
                    return false;
                }

                objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 8 and idServico = " + idServico; //verificar se ja tem certificado
                idCert = objCmd.ExecuteScalar();

                if (idCert != null) //se existe algum certificado
                {
                    lblMessage.Text += "Ya existe un certificado  3R para el Servicio " + refServico + "<br />";
                    return false;
                }

                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;


                    strSQL = "INSERT INTO certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) VALUES (" + idServico + ", 8, 237, getdate(),null,getdate(),223,getdate(),223)";
                    objCmd.CommandText = strSQL;

                    if (objCmd.ExecuteNonQuery() != 1)
                    {
                        return false;
                    }

                    //isto é o último passo. mover o ficheiro e se o ficheiro for movido, commit transaction
                    try
                    {

                        filesOrigem.CopyTo(caminhoDestino, false);
                        filesOrigem.Delete();
                        objTrans.Commit();
                        lblMessage.Text += "SUCESO al cargar el Certificado " + filesOrigem.ToString() + "<br />";
                        return true;
                       
                    }
                    catch
                    {
                        objTrans.Rollback();
                        lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString();
                        return false;
                    }
                   
                }

            } //end using transaction

        }


        private bool trata4R(string refServico, string idServico, FileInfo filesOrigem, string caminhoDestino)
        {

            string strSQL;

            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];
            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {
                objCmd.Connection = objConn;
                objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut;
                objConn.Open();


                objCmd.CommandText = "SELECT idServico FROM Servico where idServico = " + idServico + " AND idEstadoCertificado = 4"; // in (6,15,25)"; //verificar se está no estado correcto

                object idser = objCmd.ExecuteScalar();
                if (Convert.IsDBNull(idser)) //SE NAO HA NADA O SERVICO ESTÁ NO ESTADO ERRADO
                {
                    lblMessage.Text += "Verifica el estado del Servicio" + refServico + " y si existe un certificado 1C. <br />";
                    return false;
                }

                //se ainda nao ha um 1R tem de criar um 1R primeiro
                objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 8 and idServico = " + idServico; //verificar se ja tem certificado
                object idCert = objCmd.ExecuteScalar();

                if (idCert == null) //se existe algum certificado
                {
                    lblMessage.Text += "Debe crear primeiro un 3R para el servicio " + refServico + "<br />";
                    return false;
                }

                objCmd.CommandText = "SELECT idCertificado FROM CERTIFICADO where idtipoCertificado = 11 and idServico = " + idServico; //verificar se ja tem certificado
                idCert = objCmd.ExecuteScalar();

                if (idCert != null) //se existe algum certificado
                {
                    lblMessage.Text += "Ya existe un certificado  4R para el Servicio " + refServico + "<br />";
                    return false;
                }

                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;


                    strSQL = "INSERT INTO certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) VALUES (" + idServico + ", 11, 237, getdate(),null,getdate(),223,getdate(),223)";
                    objCmd.CommandText = strSQL;

                    if (objCmd.ExecuteNonQuery() != 1)
                    {
                        return false;
                    }

                    //isto é o último passo. mover o ficheiro e se o ficheiro for movido, commit transaction
                    try
                    {

                        filesOrigem.CopyTo(caminhoDestino, false);
                        filesOrigem.Delete();
                        objTrans.Commit();
                        lblMessage.Text += "SUCESO al cargar el Certificado " + filesOrigem.ToString() + "<br />";
                        return true;
                        
                    }
                    catch
                    {
                        objTrans.Rollback();
                        lblMessage.Text += "ERROR al cargar el Certificado " + filesOrigem.ToString();
                        return false;
                    }
                   
                   
                }

            } //end using transaction

        }

       // private void trata1A(string refServico, string idServico, string caminhoOrigem,string caminhoDestino)
       // {
       // }
       // private void trata2A(string refServico, string idServico, string caminhoOrigem,string caminhoDestino)
       // {
       // }
        
        

        //extrai a sigla do nome do documento
        private string sigla(string strIn)
        {

            string[] docIn = strIn.Split("-,.".ToCharArray());
            return docIn[2].ToUpper() ;

        }

        //devolva a ref do serviço com base no nome do documento
        private string refServico(string strIn)
        {

            string refServico = strIn.Replace("-", "/");
            return refServico.Substring(0, refServico.Length - 7);

        }


        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            EspanhaLoopThroughFiles();
            BindGrid();
        }

        protected void ItemsGrid_Command(Object sender, DataGridCommandEventArgs e)
        {

            switch (((LinkButton)e.CommandSource).CommandName)
            {

                case "Delete":
                    //Response.Write("apagar ficheiro");
                    DeleteItem(e);
                    break;

                // Add other cases here, if there are multiple ButtonColumns in 
                // the DataGrid control.

                default:
                    // Do nothing.
                    break;

            }

        }

        void DeleteItem(DataGridCommandEventArgs e)
        {

            // e.Item is the table row where the command is raised. For bound
            // columns, the value is stored in the Text property of a TableCell.
            TableCell itemCellFileName = e.Item.Cells[1];
            string fileName = itemCellFileName.Text;

      
            // Rebind the data source to refresh the DataGrid control.

             DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaOrigem);
            FileInfo[] filesOrigem = dirInfoPastaOrigem.GetFiles(fileName);


            if (filesOrigem.Length > 0)
            {

                for (int i = 0; i < filesOrigem.Length; i++)
                {

                    filesOrigem[i].Delete(); //apaga da pasta de origem!.[
                }
            }

            BindGrid();

        }
      
    }
}



