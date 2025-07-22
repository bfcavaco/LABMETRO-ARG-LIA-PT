using System;
using CrystalDecisions.CrystalReports.Engine; 
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO;
using System.Configuration;
using System.Web;
using LabMetro.REPORTS;
//using System.Web.Mail; obsolete

using System.Collections; 
using System.Web.UI.WebControls; 
using System.Drawing.Printing; 
using System.Threading;
using System.Net.Mail;


namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for clsReport.
	/// </summary>
    /// 
   
	public class clsReport
	{

        private string myApp = (string)ConfigurationManager.AppSettings["Application_Country"].ToUpper();



		public clsReport()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// Fornece ao report a informação necessária para o mesmo se ligar à BD

		//acho q se podia usar tb (ReportDocument report)
		public void setReportConnectionInfo(ReportClass report)
		{
			ConnectionInfo conInfo = new ConnectionInfo();

			conInfo.ServerName = (string)ConfigurationManager.AppSettings["ServerName"];
			conInfo.DatabaseName = (string)ConfigurationManager.AppSettings["DatabaseName"];
			conInfo.UserID = (string)ConfigurationManager.AppSettings["UserID"];
			conInfo.Password = (string)ConfigurationManager.AppSettings["Password"];

			foreach(CrystalDecisions.CrystalReports.Engine.Table table in report.Database.Tables)
			{
				TableLogOnInfo tableInfo = new TableLogOnInfo() ;
				tableInfo.ConnectionInfo = conInfo;
				table.ApplyLogOnInfo(tableInfo);
			}
		}


        //a mesma coisa mas recebe um ReportDocument
        public void setReportConnectionInfo(ReportDocument report)
        {
            ConnectionInfo conInfo = new ConnectionInfo();

            conInfo.ServerName = (string)ConfigurationManager.AppSettings["ServerName"];
            conInfo.DatabaseName = (string)ConfigurationManager.AppSettings["DatabaseName"];
            conInfo.UserID = (string)ConfigurationManager.AppSettings["UserID"];
            conInfo.Password = (string)ConfigurationManager.AppSettings["Password"];

            foreach (CrystalDecisions.CrystalReports.Engine.Table table in report.Database.Tables)
            {
                TableLogOnInfo tableInfo = new TableLogOnInfo();
                tableInfo.ConnectionInfo = conInfo;
                table.ApplyLogOnInfo(tableInfo);
            }
        }

     


        //OVERLOAD QUE RECEBE O NOME DO FICHEIRO
        //nao sei se isto funciona pois recebi um nome com uma barra
        // Exporta o report para PDF
        public void exportReportToPDF(ReportClass report,string fileName)
        {
            HttpContext.Current.Response.Buffer = false; //?? o que é isso
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            try //isto aqui so existe para poder fechar os objectos no final, senao ha uma especie de response.end e nada mais é executado.
            {
                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, fileName);

                
            }
            catch(Exception ex)
            { 
                
                HttpContext.Current.Response.Write(ex.ToString()); 
            
            }

            finally
            {
                report.Close();
                report.Dispose();
                report = null;
            }
            
            
            //// Enviar para PDF
            //MemoryStream oStream;

            //oStream = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Buffer= true;
            //HttpContext.Current.Response.ContentType = "application/pdf"; 
            //fileName = fileName.Replace("/","_"); 
            //if(fileName!="")
            //{
            //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename="+fileName+".pdf;");
            //}
            //else
            //{
            //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=myFile.pdf");
            //}
            //HttpContext.Current.Response.BinaryWrite(oStream.ToArray());
            //HttpContext.Current.Response.End(); 
            //oStream.Close(); 
            //report.Close();
            //report.Dispose(); 
        }

        //OVERLOAD QUE RECEBE O NOME DO FICHEIRO
        //nao sei se isto funciona pois recebi um nome com uma barra
        // Exporta o report para PDF
        public void exportReportToPDF_NEW(ReportDocument report, string fileName)
        {
            HttpContext.Current.Response.Buffer = false; //?? o que é isso
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            try //isto aqui so existe para poder fechar os objectos no final, senao ha uma especie de response.end e nada mais é executado.
            {
                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, fileName);
            }
            catch
            { }
            finally
            {
                report.Close();
                report.Dispose();
                report = null;
            }


            //// Enviar para PDF
            //MemoryStream oStream;

            //oStream = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Buffer= true;
            //HttpContext.Current.Response.ContentType = "application/pdf"; 
            //fileName = fileName.Replace("/","_"); 
            //if(fileName!="")
            //{
            //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename="+fileName+".pdf;");
            //}
            //else
            //{
            //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=myFile.pdf");
            //}
            //HttpContext.Current.Response.BinaryWrite(oStream.ToArray());
            //HttpContext.Current.Response.End(); 
            //oStream.Close(); 
            //report.Close();
            //report.Dispose(); 
        }

        //grava para disco e retorna o nome do ficheiro
        //report_prefix: ORC_, FACT_, etc.
        //id: para tornar o nome do documento único, recebo um id
        //as pdf
        public string saveReportToDisk(ReportClass report,string reportPrefix, string id)
        {
			id = id.Replace("/","_"); 	
            
			DiskFileDestinationOptions dfdo = new DiskFileDestinationOptions(); 
            
			string strFileName= reportPrefix + id+".pdf";  //+ "_" + DateTime.Now.ToShortDateString() + ".pdf";
            string pathFicheiro = (string)ConfigurationManager.AppSettings["TEMP_PATH"];     
            string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName);            
            dfdo.DiskFileName = path; 
            
            report.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile; 
            report.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat; 
            report.ExportOptions.DestinationOptions = dfdo; 
            
            report.Export();

			report.Close();
			report.Dispose(); 
            return path; 
        }

        //guarda em disco em word doc
        private string saveReportToDiskAsDoc(ReportClass report,string reportPrefix, string id)
        {
        
            DiskFileDestinationOptions dfdo = new DiskFileDestinationOptions(); 
            string strFileName= reportPrefix + id + "_" + DateTime.Now.ToShortDateString() + ".pdf";
            string pathFicheiro = (string)ConfigurationManager.AppSettings["TEMP_PATH_REL"];     
            string path = System.Web.HttpContext.Current.Server.MapPath("~/"+pathFicheiro+ "/" + strFileName);            
            dfdo.DiskFileName = path; 
            
            report.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile; 
            report.ExportOptions.ExportFormatType = ExportFormatType.WordForWindows; 
            report.ExportOptions.DestinationOptions = dfdo; 
            report.Export();
			report.Close();
			report.Dispose(); 
            return path; 
        }


		// Exporta o report para WORD
		public void exportReportToWORD(ReportClass report)
		{
			// Enviar para WORD
			MemoryStream oStream;

			oStream = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
            
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ContentType = "application/doc"; 
			HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
			//força o dialog "save as"; 
			HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=myFile.doc");  
			HttpContext.Current.Response.BinaryWrite(oStream.ToArray());
			HttpContext.Current.Response.End();
			oStream.Close(); 
			report.Close();
			report.Dispose(); 

		}

		// Usado para os envios para o LanFax 
		public void printReport(ReportClass report, string printerName, int numberCopies)
		{			
            report.PrintOptions.PrinterName = printerName; 
            //To print all pages, set the startPageN and endPageN parameters to zero. 
            report.PrintToPrinter(numberCopies,true,0,0);
			report.Close();
			report.Dispose(); 
		}

        public void printReportEtiquetas(ReportClass report, string printerName, int numberCopies)
        {
            report.PrintOptions.PrinterName = printerName;
			report.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperEnvelope10;
          
            
          //tentar configurar aqui as coisas de forma a poder imprimir bem as etiquetas

            PageMargins margins;

            // Get the PageMargins structure and set the 
            // margins for the report.
            margins = report.PrintOptions.PageMargins;
            margins.bottomMargin = 350;
            margins.leftMargin = 350;
            margins.rightMargin = 350;
            margins.topMargin = 350;

            // Apply the page margins.
            report.PrintOptions.ApplyPageMargins(margins);

            //CONFIGURAR ESTA SECCAO!

//=====================
//PageContentHeight Int32. Gets the height of the pages content. 
//PageContentWidth Int32. Gets the width of the pages content. 
//PageMargins PageMargins. Gets the reports page margins. Use the ApplyPageMargins method to apply the changes. 
//PaperOrientation PaperOrientation. Gets or sets the current printer paper orientation. For the default printer, DefaultPaperOrientation is returned. 
//PaperSize PaperSize. Gets or sets the current printer paper size. For the default printer, DefaultPaperSize is returned. 
//PaperSource PaperSource. Gets or sets the current printer paper source. 
//PrinterDuplex PrinterDuplex. Gets or sets the current printer duplex option. 
//PrinterName String. Gets or sets the printer name used by the report. Gets an empty string if the default printer is used. Once set the report can be printed by clicking the Printer button on the Windows Forms Viewer or through code using the PrintToPrinter method. 

//=====================


            // Print the report. Set the startPageN and endPageN
            // parameters to 0 to print all pages.
            report.PrintToPrinter(numberCopies,true,0,0);
			report.Close();
			report.Dispose(); 


        }

        
        //o fax manda para o servidor de email.


        

        public void sendMail(string mailTo,string mailFrom,string mailSubject,string FilePath,string mailBody,string mailOrFax, string cc, string bcc)
        {

           
            SmtpClient smtp = new SmtpClient();
            smtp.Host = (ConfigurationManager.AppSettings["IPSERVIDORMAIL"].ToString());
           

            //para espanha precisamos de username e password
            if (myApp == "ES_LABMETRO")
            {
                string pass = (ConfigurationManager.AppSettings["PASSMAILESPANHA"].ToString()); // "Wot83420"
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("isq@unceta.es",pass ); // passar para variaveis dps
                smtp.Port = 587;
                
                smtp.EnableSsl = true;
            }

          
          
            MailMessage msg = new MailMessage(); 
            
            msg.To.Add(mailTo);
            msg.From = new MailAddress(mailFrom);//new MailAddress("isq@unceta.es"); tentei colocar o mesmo mail que a conta de outloo
            msg.Subject = mailSubject;

            if (cc != "")
            {
                msg.CC.Add(cc); 
            }
            if (bcc != "")
            {
                msg.Bcc.Add(bcc);
            }

            if (mailBody != "")
            {
                msg.Body = mailBody;
            }

            if (FilePath != "")
            {
                using (Attachment myAttachment = new Attachment(FilePath))
                {
                    msg.Attachments.Add(myAttachment);
                    try
                    {
                        smtp.Send(msg);
                        GERAL.clsWriteError.WriteLog("Mail -" + mailSubject + "- enviado para : " + mailTo);
                    }
                    catch (Exception ex)
                    {
                        GERAL.clsWriteError.WriteLog("Erro na função de envio do email parte 1 : " + ex.ToString()+smtp.Credentials.ToString());
                    }

                }
            }

            else //no caso de nao ter attachment, mas eu acho que tem sempre.
            {
                try
                {
                    smtp.Send(msg);
                    GERAL.clsWriteError.WriteLog("Mail -" + mailSubject + "- enviado para : " + mailTo);
                }
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog("Erro na função de envio do email parte 2: " + ex.ToString() + smtp.Credentials.ToString());
                }
            }

            msg.Dispose();

        }
        
        ////o faxNumber recebido corresponde ao mailTo
        ////id foi substituido pelo ref do orcamento
        //public void faxReport(ReportClass report, string faxNumber,string mailFrom,string reportPrefix, string id)
        //{   
        //    string faxIn = faxNumber.Replace(" ","").Replace("-","").Replace("+","");
        //    string numFax = ConfigurationManager.AppSettings["NUMFAX"].ToString();         
        //    numFax = numFax.Replace("000",faxIn); 
            
        //    string filePath = saveReportToDisk(report,reportPrefix,id);
           
        //    try
        //    {
        //        sendMail(numFax,mailFrom,"Confirmação de Envio de fax",filePath,"","fax","","");
        //        //System.IO.File.Delete(filePath);
        //    }
        //    catch(Exception ex)
        //    {
        //        GERAL.clsWriteError.WriteLog("Erro na função de envio do fax : " + ex.ToString()); 
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            report.Dispose();
        //            report.Close();
        //            System.IO.File.Delete(filePath);
        //        }
        //        catch (Exception ex)
        //        {
        //            GERAL.clsWriteError.WriteLog("Erro na função de apgar o ficheiro após envio do fax : " + ex.ToString()); 
        //        }
        //    }
        //}

        //id foi substituido pelo ref do orcamento
        public void sendFaxNovo(ReportClass report, string fax, string reportPrefix, string id)
        {
            //string faxIn = faxNumber.Replace(" ", "").Replace("-", "").Replace("+", "");
            //string numFax = "000@efax.vodafone.pt"; //uso o metodo antigo para nao ter de alterar muito agora.
            //numFax = numFax.Replace("000", faxIn);

            string faxIn = fax.Replace(" ", "").Replace("-", "").Replace("+", "");
            string numFax = "000@efax.vodafone.pt"; //uso o metodo antigo para nao ter de alterar muito agora.
            numFax = numFax.Replace("000", faxIn);

            string fName = saveReportToDisk(report, reportPrefix, id);

            
            //string myPath = (string)ConfigurationManager.AppSettings["TEMP_PATH_PTCOMERCIAL"];
            //myPath = myPath + "/" + "teste.pdf";//filename.ToString();

            //converter o ficheiro para byte
            byte[] file;
            using (var stream = new FileStream(fName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            try
            {
                //REMOVER O NUMERO HARD CODED. CORRIGIR TUDO NO FIM

                sendFaxWS(numFax, "214228102@efax.vodafone.pt", file,  "-.pdf"); 
               // sendFax(numFax, "Confirmação de Envio de fax", filePath, "", "fax", "", "");
                //System.IO.File.Delete(filePath);
            }
            catch (Exception ex)
            {
                GERAL.clsWriteError.WriteLog("Erro na função de envio do fax : " + ex.ToString());
            }
            finally
            {
                try
                {
                    report.Dispose();
                    report.Close();
                    System.IO.File.Delete(fName);
                }
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog("Erro na função de apgar o ficheiro após envio do fax : " + ex.ToString());
                }
            }
        }





//string destination //numero de fax – “yyyyyyyyy”
//string remetente    //endereço remetente –  “yyyyyyyyy@efax.vodafone.pt”
//byte[] attach //filestream do attach (apenas um por fax)
//string extension    //extensão do attach (com ponto) – “.pdf”

//)
        private void sendFaxWS(string numFaxDestino, string numFaxRemetente, byte[] attach,  string extension)
        {

//            Subject – É necessário. Surge no corpo do mail/1ª página quando este é transformado em eFax. Quando foi vazio, deu erro!

//Body – Pode ir vazio. Provavelmente não terá qualquer utilidade neste caso.

//User – Necessário : “número” do vosso utilizador de fax atribuído (provavelmente igual ao numero do vosso fax).

//Pass – Necessário :  Corresponde ao user anterior.

            // BRUNO PODES COLOCAR AQUI EM BAIXO HARDCODED O USER E PASS, DEPOIS PASSAMOS PARA VARIAVEL MAIS TARDE!
            pt.isq.wservices.comm x = new pt.isq.wservices.comm();
            string user = "214228102";
            string pass = "DvqZC5?uJRtTFX3a";
            x.sendFAX("Envio de fax Labmetro", "", numFaxDestino, "214228102@efax.vodafone.pt", attach, user, pass, extension); 



        }

        //nova funcao de enviar fax por email Julho 2017 por vodafone
        // usa outro servidor smpt
//        Endereço de correio eletrónico (remetente):
	

//214228102@efax.vodafone.pt

//Endereço de correio eletrónico (destinatário):
	

//<nºfax_destino>@efax.vodafone.pt

//Servidor envio (SMTP):
	

//efax.vodafone.pt

//Porta:
	

//465

//Tipo de ligação encriptada:
	

//SSL

//Utilizador:
	

//214228102

//Palavra-passe:
	

//DvqZC5?uJRtTFX3a

 

//Mais informo que o recebido de entrega dos faxes será enviado automaticamente para a mailbox que se encontra associada a este número: faturacao.labmetro@isq.pt

        public void sendFax(string mailTo,  string mailSubject, string FilePath, string mailBody, string mailOrFax, string cc, string bcc)
        {
            string mailFrom = "214228102@efax.vodafone.pt";
            SmtpClient smtp = new SmtpClient("efax.vodafone.pt");
            MailMessage msg = new MailMessage();

            msg.To.Add(mailTo);
            msg.From = new MailAddress(mailFrom);
            msg.Subject = mailSubject;

            if (cc != "")
            {
                msg.CC.Add(cc);
            }
            if (bcc != "")
            {
                msg.Bcc.Add(bcc);
            }

            if (mailBody != "")
            {
                msg.Body = mailBody;
            }

            if (FilePath != "")
            {
                using (Attachment myAttachment = new Attachment(FilePath))
                {
                    msg.Attachments.Add(myAttachment);
                    try
                    {
                        smtp.Send(msg);
                        GERAL.clsWriteError.WriteLog("Mail -" + mailSubject + "- enviado para : " + mailTo);
                    }
                    catch (Exception ex)
                    {
                        GERAL.clsWriteError.WriteLog("Erro na função de envio do fax parte 1 : " + ex.ToString());
                    }

                }
            }

            else //no caso de nao ter attachment, mas eu acho que tem sempre.
            {
                try
                {
                    smtp.Send(msg);
                    GERAL.clsWriteError.WriteLog("Fax -" + mailSubject + "- enviado para : " + mailTo);
                }
                catch (Exception ex)
                {
                    GERAL.clsWriteError.WriteLog("Erro na função de envio do Fax parte 2: " + ex.ToString());
                }
            }

            msg.Dispose();

        }


        public void mailReport(ReportClass report,string mailTo,string mailFrom,string mailSubject,string reportPrefix, string id,string mailBody,string cc, string bcc)
        {
            string filePath = saveReportToDisk(report,reportPrefix,id);
            try
            {
                sendMail(mailTo,mailFrom,mailSubject,filePath,mailBody,"mail",cc,bcc); 
            }
            catch(Exception ex)
            {
                GERAL.clsWriteError.WriteLog("Erro na função de envio do email : " + ex.ToString()); 
            }
            finally
            {
                try
                {
                    report.Dispose();
                    report.Close();
                    System.IO.File.Delete(filePath);
                    
                }
                catch
                { 
                
                }
            }
        }
	}
}
