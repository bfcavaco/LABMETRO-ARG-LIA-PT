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
using System.Linq;

namespace LabMetro
{
	/// <summary>
	/// Summary description for AdminSAPFiles.
	/// </summary>
	public partial class AdminCertificados : System.Web.UI.Page
	{

		private static string pastaCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];
        private static string pastaOrigem = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGINAIS"];
        private static string pastaConstrucao = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CONSTRUCAO"];
	
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text =""; 
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


        //desarquivar

        private void moveFilesVisteon()
        {

            DirectoryInfo dirInfoCertificadosFinais = new DirectoryInfo(pastaCertificados);

            DirectoryInfo dirInfoCertificadosFinais2007 = new DirectoryInfo(pastaCertificados + "\\2007\\");

            DirectoryInfo dirInfoCertificadosFinais2008 = new DirectoryInfo(pastaCertificados + "\\2008\\");

            DirectoryInfo dirInfoCertificadosFinais2009 = new DirectoryInfo(pastaCertificados + "\\2009\\");

            DirectoryInfo dirInfoCertificadosFinais2010 = new DirectoryInfo(pastaCertificados + "\\2010\\");

            DirectoryInfo dirInfoCertificadosFinais2011 = new DirectoryInfo(pastaCertificados + "\\2011\\"); ;

            string strSQL = "select replace (refServico,'/','-') as ref, servico.ano, dtCertificado FROM CERTIFICADO INNER JOIN SERVICO ON CERTIFICADO.idServico = SERVICO.idServico WHERE SERVICO.IDBRE IN (SELECT IDBRE  from bre where idEmpresa in (28116,28117,28118,28115,21,31,32,33,485,10331,35))  and servico.ano = 2007 order by servico.idServico asc";

            string fileloc = "";

            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            while (dr.Read())
            {

                foreach (FileInfo f in dirInfoCertificadosFinais2007.GetFiles(dr["ref"].ToString() + "*"))
                {
                    try
                    {
                        fileloc = pastaCertificados + "\\" + f.Name.ToString().ToUpper();
                        f.MoveTo(fileloc);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(fileloc + ex.ToString());
                    }
                }
            }


            //2008
            strSQL = "select replace (refServico,'/','-') as ref, servico.ano, dtCertificado FROM CERTIFICADO INNER JOIN SERVICO ON CERTIFICADO.idServico = SERVICO.idServico WHERE SERVICO.IDBRE IN (SELECT IDBRE  from bre where idEmpresa in (28116,28117,28118,28115,21,31,32,33,485,10331,35))  and servico.ano = 2008 order by servico.idServico asc";

            fileloc = "";

            dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            while (dr.Read())
            {

                foreach (FileInfo f in dirInfoCertificadosFinais2008.GetFiles(dr["ref"].ToString() + "*"))
                {
                    try
                    {
                        fileloc = pastaCertificados + "\\" + f.Name.ToString().ToUpper();
                        f.MoveTo(fileloc);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(fileloc + ex.ToString());
                    }
                }
            }


            //2009
            strSQL = "select replace (refServico,'/','-') as ref, servico.ano, dtCertificado FROM CERTIFICADO INNER JOIN SERVICO ON CERTIFICADO.idServico = SERVICO.idServico WHERE SERVICO.IDBRE IN (SELECT IDBRE  from bre where idEmpresa in (28116,28117,28118,28115,21,31,32,33,485,10331,35))  and servico.ano = 2009 order by servico.idServico asc";

            fileloc = "";

            dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            while (dr.Read())
            {

                foreach (FileInfo f in dirInfoCertificadosFinais2009.GetFiles(dr["ref"].ToString() + "*"))
                {
                    try
                    {
                        fileloc = pastaCertificados + "\\" + f.Name.ToString().ToUpper();
                        f.MoveTo(fileloc);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(fileloc + ex.ToString());
                    }
                }
            }

            //2010
            strSQL = "select replace (refServico,'/','-') as ref, servico.ano, dtCertificado FROM CERTIFICADO INNER JOIN SERVICO ON CERTIFICADO.idServico = SERVICO.idServico WHERE SERVICO.IDBRE IN (SELECT IDBRE  from bre where idEmpresa in (28116,28117,28118,28115,21,31,32,33,485,10331,35))  and servico.ano = 2010 order by servico.idServico asc";

            fileloc = "";

            dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            while (dr.Read())
            {

                foreach (FileInfo f in dirInfoCertificadosFinais2010.GetFiles(dr["ref"].ToString() + "*"))
                {
                    try
                    {
                        fileloc = pastaCertificados + "\\" + f.Name.ToString().ToUpper();
                        f.MoveTo(fileloc);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(fileloc + ex.ToString());
                    }
                }
            }

            //2011
            strSQL = "select replace (refServico,'/','-') as ref, servico.ano, dtCertificado FROM CERTIFICADO INNER JOIN SERVICO ON CERTIFICADO.idServico = SERVICO.idServico WHERE SERVICO.IDBRE IN (SELECT IDBRE  from bre where idEmpresa in (28116,28117,28118,28115,21,31,32,33,485,10331,35))  and servico.ano = 2011 order by servico.idServico asc";

            fileloc = "";

            dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            while (dr.Read())
            {

                foreach (FileInfo f in dirInfoCertificadosFinais2011.GetFiles(dr["ref"].ToString() + "*"))
                {
                    try
                    {
                        fileloc = pastaCertificados + "\\" + f.Name.ToString().ToUpper();
                        f.MoveTo(fileloc);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(fileloc + ex.ToString());
                    }
                }
            }


        }
    

        private void moveFilesByYear(string ano)
        {
            

            DirectoryInfo dirInfoCertificados = new DirectoryInfo(pastaCertificados);
           
            string fileloc="";

            switch (ano)
            {
                case "2007":
                    foreach (FileInfo f in dirInfoCertificados.EnumerateFiles("*-07-*"))
                    {
                        try
                        {
                            fileloc = pastaCertificados + "\\2007\\" + f.Name.ToString().ToUpper();
                            f.MoveTo(fileloc);
                        }
                        catch (Exception ex)
                        {
                            Response.Write(fileloc + ex.ToString());
                        }

                    }
                    break;
                case "2008":
                    foreach (FileInfo f in dirInfoCertificados.EnumerateFiles("*-08-*"))
                    {
                        try
                        {
                            fileloc = pastaCertificados + "\\2008\\" + f.Name.ToString().ToUpper();
                            f.MoveTo(fileloc);
                        }
                        catch (Exception ex)
                        {
                            Response.Write(fileloc + ex.ToString());
                        }

                    }
                    break;
                case "2009":
                    foreach (FileInfo f in dirInfoCertificados.EnumerateFiles("*-09-*"))
                    {
                        try
                        {
                            fileloc = pastaCertificados + "\\2009\\" + f.Name.ToString().ToUpper();
                            f.MoveTo(fileloc);
                        }
                        catch (Exception ex)
                        {
                            Response.Write(fileloc + ex.ToString());
                        }

                    }
                    break;
                case "2010":
                    foreach (FileInfo f in dirInfoCertificados.EnumerateFiles("*-10-*"))
                    {
                        try
                        {
                            fileloc = pastaCertificados + "\\2010\\" + f.Name.ToString().ToUpper();
                            f.MoveTo(fileloc);
                        }
                        catch (Exception ex)
                        {
                            Response.Write(fileloc + ex.ToString());
                        }

                    }
                    break;
                case "2011":
                    foreach (FileInfo f in dirInfoCertificados.EnumerateFiles("*-11-*"))
                    {
                        try
                        {
                            fileloc = pastaCertificados + "\\2011\\" + f.Name.ToString().ToUpper();
                            f.MoveTo(fileloc);
                        }
                        catch (Exception ex)
                        {
                            Response.Write(fileloc + ex.ToString());
                        }

                    }
                    break;
            }
            }

        protected void Button2_Click(object sender, EventArgs e)
        {
            moveFilesByYear(txtAno.Text);
        }


	
		
		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			//fillDBNomesDocs(); 
            fillDBNomesDocsFaster();
		}

		
			//****************************************************************

        private void fillDBNomesDocsFaster()
        {


            string strSQL = "SELECT MAX (dataCertificado) FROM DocsCertificados where nomepasta = 'certificados'";
            
            System.DateTime maxdatacertificadosfinais = System.Convert.ToDateTime(GERAL.clsDataAccess.myExecuteScalar(strSQL));

            
            //Response.Write(maxdatacertificadosfinais.ToString());
            DirectoryInfo dirInfoCertificados = new DirectoryInfo(pastaCertificados);

           
            FileInfo[] latestFiles = new DirectoryInfo(pastaCertificados)//new DirectoryInfo(@"c:\foo\bar")
                         .EnumerateFiles()
                         .Select( x => {
                            x.Refresh();
                            return x;
                         })
                         .Where(x => x.CreationTime.Date > maxdatacertificadosfinais || x.LastWriteTime > maxdatacertificadosfinais)
                         .ToArray()
                         ;


            if (latestFiles.Length > 0)
            {

                foreach (FileInfo f in latestFiles)
                {

                    Response.Write(f.Name + " " + f.CreationTime + "<br />");
                    strSQL = "INSERT INTO DocsCertificados (nomeCertificado,tipoCertificado,dataCertificado,nomePasta,refServico) VALUES ('" + f.Name + "','" + sigla(f.Name) + "','" + f.CreationTime + "','Certificados','" + refServico(f.Name) + "')";
                    GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL);

                }

                strSQL = "SELECT MAX (dataCertificado) FROM DocsCertificados where nomepasta = 'certificados'";
                maxdatacertificadosfinais = System.Convert.ToDateTime(GERAL.clsDataAccess.myExecuteScalar(strSQL));

                Response.Write(maxdatacertificadosfinais.ToString());

            }
        }

      
        //**********************************************************************
        private void fillDBNomesDocs()
        {

            string strSQL = "SELECT MAX (dataCertificado) FROM DocsCertificados where nomepasta = 'certificados'"; 
            System.DateTime maxdatacertificadosfinais = System.Convert.ToDateTime(GERAL.clsDataAccess.myExecuteScalar(strSQL)) ; 

            Response.Write(maxdatacertificadosfinais.ToString());
			
            DirectoryInfo dirInfoCertificados = new DirectoryInfo(pastaCertificados);
            FileInfo[] files = dirInfoCertificados.GetFiles();

            IComparer comp = new LabMetro.GERAL.CompareFileInfo(); //ordena por data(+hora)
            Array.Sort(files, comp); 

            if(files.Length >0) 
            {

                    foreach(FileInfo f in files)
                    {

                        if (f.CreationTime > maxdatacertificadosfinais)
                        {
                            Response.Write(f.Name + " " + f.CreationTime + "<br />");
                            strSQL = "INSERT INTO DocsCertificados (nomeCertificado,tipoCertificado,dataCertificado,nomePasta,refServico) VALUES ('" + f.Name + "','" + sigla(f.Name) + "','" + f.CreationTime + "','Certificados','" + refServico(f.Name) + "')";
                            GERAL.clsDataAccess.ExecuteNonQueryDB(strSQL); 
                        }
                    }


                    strSQL = "SELECT MAX (dataCertificado) FROM DocsCertificados where nomepasta = 'certificados'";
                    maxdatacertificadosfinais = System.Convert.ToDateTime(GERAL.clsDataAccess.myExecuteScalar(strSQL));

                    Response.Write(maxdatacertificadosfinais.ToString());

                       
            }
        }


		//extrai a sigla do nome do documento
		private string sigla (string strIn)
		{
			
			string[] docIn = strIn.Split("-,.".ToCharArray()); 
			return docIn[2];  

		}

		//devolva a ref do serviço com base no nome do documento
		private string refServico (string strIn)
		{
			
			string refServico = strIn.Replace("-","/"); 
			return refServico.Substring(0, refServico.Length-7); 

		}

		protected void btnVerMaus_Click(object sender, System.EventArgs e)
		{

			//isto é o caso em que nao foram criados os certificados
			//será que nao existem tb casos em que foram criados os certificados e aparecem na pasta, 
			//mas o estado nao foi actualizador?! aconteceu 1 vez... uma vez nao é nenhuma vez, mas...
			
			//A VER AINDA

			//=====================================

            //verifcar se isto tb apanha as revisoes etc... acho que nao!
			string strSQL = "SELECT docs.*, ser.idServico, ser.idEstadoCertificado, ser.idTipoCertificadoEmValidacao, ser.idUtilizadorValidouCertificado, ser.idEstadoServico FROM docsCertificados docs INNER JOIN servico ser ON ser.refServico = docs.refServico WHERE ser.idEstadoServico <> -1 and docs.refServico NOT IN (SELECT s.refServico FROM servico s INNER JOIN certificado c ON s.idServico = c.idServico)"; 

			dgErros.DataSource = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			dgErros.DataBind(); 
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			corrigirBD(); 
		}


		private void corrigirBD()
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				string strSQL; 
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try
					{
                
						objCmd.CommandType = CommandType.Text; 


                        //nao sei se isto funcioa bem com as revisoes....



//						//ir buscar os que estao mal para dentro de uma tabela temporaria
//						strSQL ="select docs.*, ser.idServico, ser.idEstadoCertificado, ser.idTipoCertificadoEmValidacao, ser.idUtilizadorValidouCertificado, ser.idEstadoServico into #ttt from docsCertificados docs inner join servico ser on ser.refServico = docs.refServico where docs.refServico not in (select s.refServico from servico s inner join certificado c on s.idServico = c.idServico) and ser.idTipoCertificadoEmValidacao is not null";  //esta ultima parte, nao sei se é pq os dados de teste estao ligeiramente corruptos, ou se existe outro problema na bd.... 
						strSQL = "SELECT docs.*, ser.idServico, ser.idEstadoCertificado, tc.idTipoCertificado, ser.idTipoCertificadoEmValidacao, ser.idUtilizadorValidouCertificado, ser.idEstadoServico into #ttt FROM docsCertificados docs INNER JOIN servico ser ON docs.refServico = ser.refServico INNER JOIN tipoCertificado tc on docs.tipoCertificado = tc.sigla WHERE ser.idEstadoServico <> -1 and ser.idServico not in (select idServico from certificado where idTipoCertificado = tc.idTipoCertificado)"; // --onde não existe nenhum registo na tabela certificado para --aquele tipo de certificado" 


						objCmd.CommandText = strSQL; 
						objCmd.ExecuteNonQuery(); 

						//actulizar estados (1a parte)
						strSQL = "update servico set idEstadoCertificado = 4, idEstadoServico = 12 where idServico in (select idServico from #ttt) and idEstadoServico =6"; 
						objCmd.CommandText = strSQL; 
						objCmd.ExecuteNonQuery(); 

						//actulizar estados (2a parte)
						strSQL = "update servico set idEstadoCertificado = 4, idEstadoServico = 23 where idServico in (select idServico from #ttt) and idEstadoServico in (25,15)"; 
						objCmd.CommandText = strSQL; 
						objCmd.ExecuteNonQuery(); 
				
						//actulizar estados (3a parte)n //martelada para actualizar os estados dos restantes.
						//aqui o problema pode ser se é detectado muito tarde um problema, eu mudo o estado para 4, 
						//mas ha um novo certificado posterior em fila de espera para aprovação.
						//tenho deps de rever isto, mas para já, deve funcionar assim.
						strSQL = "update servico set idEstadoCertificado = 4 where idServico in (select idServico from #ttt) and idEstadoServico not in (6,25,15)"; 
						objCmd.CommandText = strSQL; 
						objCmd.ExecuteNonQuery(); 

						//actulizar tabela certificados
						strSQL = "insert into certificado (idServico,idTipoCertificado,idFuncionarioEmitiu, dtCertificado,observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao ) select idServico,idTipoCertificado,0,dataCertificado,'dados actualizados por ADMIN',getDate(),0,getDate(),0 from #ttt"; 

						objCmd.CommandText = strSQL; 
						objCmd.ExecuteNonQuery(); 

						//drop tabela temporaria.
						strSQL = "drop table #ttt"; 
						objCmd.CommandText = strSQL; 
						objCmd.ExecuteNonQuery(); 

						objTrans.Commit(); 
						lblMessage.Text +=	GERAL.clsGeral.ErrorMessage.MSG_DB; 
					}
					catch(Exception ex)
					{ 	
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); 
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text+= GERAL.clsGeral.ErrorMessage.ERR_DB; 
					}
				}
			}
		}

        protected void Buttonvisteon_Click(object sender, EventArgs e)
        {
            moveFilesVisteon();
        }

       
	}
}
