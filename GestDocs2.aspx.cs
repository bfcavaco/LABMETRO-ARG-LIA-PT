using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LabMetro.REPORTS;
using LabMetro.GERAL;
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
using System.Text;


// nota: dm, junho 2009: vou retirar as permissoes para o user labmetrousers poder read files na pasta origem!


namespace LabMetro
{
    /// <summary>
    /// Summary description for GestDocumentos.
    /// Página para a primeira fase de validação dos certificados.
    /// Os documentos deverão ser validados pelos técnicos que executaram os ensaios.
    /// </summary>
    public partial class GestDocs2 : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button btnHelp;
        protected System.Web.UI.WebControls.Label lblErro;

        //NOME DA PAGINA
        private const string ID_PAG = "CERTDOCUMENTOS_1";
        private string MSG_PDF_INEXISTENTE = "Criar/Carregar PDF.";
        private string MSG_PDF_CRIARNOVO = "Recarregar PDF corrigido.";
        private string MSG_PDF_CORROMPIDO = "PDF corrompido. Criar novo.";

        //PASTAS
        private static string pastaTemplates = (string)ConfigurationManager.AppSettings["PASTA_TEMPLATES"];
        private static string pastaSimbolos = (string)ConfigurationManager.AppSettings["PASTA_SIMBOLOS"];
        private static string pastaCertificadosOrigem = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGEM"];
        //MUDEI O NOME MAS SO PARA ESTA PAGNA, DUPL. WEB.CONFIG

        private static string pastaCertificadosOrigemBackup = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGEM_BACKUP"];
        //MUDEI O NOME MAS SO PARA ESTA PAGNA, DUPL. 

        private static string pastaCertificadosFinaisConstrucao = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CONSTRUCAO"];
        //private static string caminhoPastaCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];
        private static string pastaCertificadosFinaisValidados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];
        //TEMPLATES
        private static string templatePaginaNaoAcreditada = (string)ConfigurationManager.AppSettings["TEMPLATE_PAGINA"];
        private static string templatePaginaAcreditada = (string)ConfigurationManager.AppSettings["TEMPLATE_PAGINA_IPAC"];
        private static string templatePaginaREP = (string)ConfigurationManager.AppSettings["TEMPLATE_PAGINA_REP"];

        private static string caminhoTemplateNaoAcreditada = pastaTemplates + "\\" + templatePaginaNaoAcreditada;
        private static string caminhoTemplateAcreditada = pastaTemplates + "\\" + templatePaginaAcreditada;
        private static string caminhoTemplateREP = pastaTemplates + "\\" + templatePaginaREP;

        private DataTable DT;
        private DataView DV;
        DataTable dt;



        //private static DirectoryInfo dirInfoPastaBackupOrigem = new DirectoryInfo(pastaCertificadosOrigemBackup);
        //private static DirectoryInfo dirInfopastaCertificadosConstrucao = new DirectoryInfo(pastaCertificadosFinaisConstrucao);
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();

            base.OnInit(e);
        }
        private void InitializeComponent()
        {

            btnSearch.Click += new System.EventHandler(btnSearch_Click);
            btnLimparCampos.Click += new System.EventHandler(btnLimparCampos_Click);
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            btnAprovarAll.Click += new System.EventHandler(btnAprovarAll_Click);
            btnRejeitarAll.Click += new System.EventHandler(btnRejeitarAll_Click);
            btnDeselectAll.Click += new System.EventHandler(btnDeselectAll_Click);
            //btnUpload.Click += new System.EventHandler(btnUpload_Click);
            //cbTodos.CheckedChanged += new System.EventHandler(cbTodos_CheckedChanged);
            cbShowOnlyCertifsFromLoggedUser.CheckedChanged += new System.EventHandler(cbShowOnlyCertifsFromLoggedUser_CheckedChanged);
            dgCertificados.ItemDataBound += new DataGridItemEventHandler(dgCertificados_ItemDataBound);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Expires = 0;   //nao tirar!!!
            lblMessage.Text = "";

            Hashtable ht = (Hashtable)Session["HTPermissions"];
            if (ht == null)
            {
                Server.Transfer("Default.aspx?err=2", false);
            }
            else
            {
                if (!ht.ContainsKey(ID_PAG))
                {
                    Server.Transfer("Default.aspx?err=1", false);
                }
                else
                {
                    if (!Page.IsPostBack)
                    {
                        setupClientScript();
                        btnSubmit.Attributes.Add("onClick", "return ConfirmarEscolha(this);");
                        ViewState["sortField"] = "dtEstadoServico";
                        ViewState["sortDirection"] = "DESC";
                        ViewState["idGrandezaUtilizadorLogado"] = sGrandezaUtilizadorLogado();
                        ViewState["userNameUserLogado"] = User.Identity.Name.ToString();
                        FillDDGrandezas();

                        createDTServicos();
                        ValidaAndApagaFilesOrigem();
                        MergePDFInicial(); //ja nao devolve string, se tem ficheiros errados, tem de se apagar la dentro
                        VerificacaoPastaFinaisConst(); 
                        BindGrid();

                        DT.AcceptChanges(); //já é chamado antes mas ju

                        ViewState["DT"] = DT;

                    }
                }
            }
        }
        private void createDTServicos()
        {
            
            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();

            DT = data.DTDocumentsForValidation2(stringDocs()); //strDocs deve servir por causa das revisoes
            DT.Columns.Add(new DataColumn("cbAprovar", typeof(bool)));
            DT.Columns.Add(new DataColumn("cbRejeitar", typeof(bool)));
            DT.Columns.Add(new DataColumn("msgColunaFicheiro", typeof(string)));
            DT.Columns.Add(new DataColumn("belongsToUser", typeof(bool))); //diz se doc pertence ao user 
            DT.Columns.Add(new DataColumn("podeAlterarTemplate", typeof(bool))); //marca se é possivel alterar a template.nas excepcoes nao podemos alterar 

            Hashtable HTDescCertByID = HTDescCertificadoIdTipo();   //contem idTipo(string) /tipoCertificado(descricao)

            //preenhce os valores dos campos acima criados, nomeadamente se o serviço pertence ao user logado, se o equip. é acreditado
            foreach (DataRow dRow in DT.Rows)
            {
                dRow["cbAprovar"] = false;  //inicializado a false
                dRow["cbRejeitar"] = false; //inicializado a false
                dRow["tipoCertificado"] = ""; //vem da bd
                dRow["belongsToUser"] = false;
                dRow["podeAlterarTemplate"] = true;

                if (dRow["idTipoServico"].ToString() == "R") //reparacao// podia passar o tipo de serviço da bd
                {
                    dRow["simbolo"] = null;
                    dRow["acreditado"] = false; //SEMPRE
                    dRow["podeAlterarTemplate"] = false; //template fixa - mas ha outros que tb nao podem alterar a template
                }

                dRow["msgColunaFicheiro"] = dRow["nomeFicheiroCertificado"]; //inicializa com o nome do documento (menos extensao); 
                if (dRow["idEstadoCertificado"].ToString() == "1")
                {
                    dRow["msgColunaFicheiro"] = MSG_PDF_INEXISTENTE;
                }
                if (dRow["userNameEfectuouServico"].ToString() == ViewState["userNameUserLogado"].ToString())
                {
                    dRow["belongsToUser"] = true;
                }
                if (dRow["idEstadoCertificado"].ToString() == "5" || dRow["idEstadoCertificado"].ToString() == "6") //REJEITADOS
                {
                    dRow["msgColunaFicheiro"] = MSG_PDF_CRIARNOVO; //indicacao que falta criar NOVO pdf

                    string idTipoCertificadoEmValidacao = dRow["idTipoCertificadoEmValidacao"].ToString();
                    //substituir o campo tipo certificado pelo tipocertificado em validacao		

                    if (idTipoCertificadoEmValidacao != "")
                    {
                        dRow["idTipoCertificado"] = idTipoCertificadoEmValidacao;
                        object y = HTDescCertByID[idTipoCertificadoEmValidacao];
                        string descTipoCert = System.Convert.ToString(y);
                        dRow["tipoCertificado"] = descTipoCert;
                    }

                    //substituir o campo userefectuouServiço pelo user que validou o serviço em último,porque...?
                    if (idTipoCertificadoEmValidacao != "1") //isto agora so é valido para tudo menos as primeiras calibracoes!!!dm 28-08-2007	
                    {
                        dRow["userNameEfectuouServico"] = dRow["userNameValidouServico"];
                        dRow["nomeTecnicoEfectuouServico"] = dRow["nomeTecnicoValidouServico"];
                    }
                }
            }


            HTDescCertByID = null;
            data = null;

            if (Page.IsPostBack) ViewState["DT"] = DT;

        }
        private void ValidaAndApagaFilesOrigem()
        {
           
            DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOrigem);
            FileInfo[] fileOrigem = dirInfoPastaOrigem.GetFiles();

            if (fileOrigem.Length > 0)
            {
                for (int i = 0; i < fileOrigem.Length; i++)
                {
                    if (!isValidFileName(fileOrigem[i].ToString()))
                    {
                        try
                        {
                            fileOrigem[i].Delete();
                        }
                        catch
                        {
                        }
                        continue;
                    }

                    string refServico = fileOrigem[i].ToString().Replace("-", "/");
                    refServico = refServico.Substring(0, refServico.Length - 7);

                    string sigla = strExtraiSigla(fileOrigem[i].ToString()); //atencao que isto pode correr mal
                    string strSQL = "";

                    switch (sigla)
                    {
                        case "1C":
                            strSQL = "select s.refServico from servico s inner join certificado c " +
                                "on s.idServico = c.idServico " +
                                "where s.refServico = '" + refServico + "' and c.idtipoCertificado = 1";
                            break;
                        case "1R":
                            strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 3";
                            break;
                        case "2R":
                            strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 7";
                            break;
                        case "3R":
                            strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 8";
                            break;
                        case "4R":
                            strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 11";
                            break;
                        case "5R":
                            strSQL = "select s.refServico from servico s inner join certificado c on s.idServico = c.idServico where s.refServico = '" + refServico + "' and c.idtipoCertificado = 12";
                            break;

                            
                    }
                    if (strSQL != "")
                    {
                        dt = GERAL.clsDataAccess.ExecuteDT(strSQL);
                    }
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                            try
                            {
                                fileOrigem[i].Delete();
                            }
                            catch
                            {
                            }
                    }

                    // eu devia apagar aqui quaisquer ficheiros que nao teem correspondencia na dt "nomeFicheiroValidacao"

                }
            }
            dirInfoPastaOrigem = null;
        }
        private string MergePDFInicial()
        {
            string resultMessage = "";
            DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOrigem);
            DV = new DataView(DT); //DT EM MEMÓRIA, AINDA NÃO HOUVE POSTBACK, 
            string idServico = "";
            foreach (DataRowView drv in DV) // para cada datarow, vai tentar buscar um certificado e no fim apaga tudo da pasta origem que nao corresponde
            {
                if (idServico != drv.Row["idServico"].ToString()) //PODE RECEBER VÁRIAS LINHAS DE UM IDSERVICO - TLVZ MELHORAR A QUERY + TARDE
                {
                    idServico = drv.Row["idServico"].ToString();
                    
                    FileInfo[] fileCertificado = dirInfoPastaOrigem.GetFiles("*" + drv.Row["nomeFicheiroCertificado"].ToString() + "*");
                    for (int i = 0; i < fileCertificado.GetLength(0); i++) //pode encontrar varios ficheiros, i guess...
                    {
                        string nomeFicheiroEncontrado = fileCertificado[i].ToString().ToUpper(); //variavel para +facil leitura
                        string refServicoEmValidacao = drv["refServico"].ToString();

                        //nao vou tratar validacao de aditamentos

                        //SE EU ENCONTRO UM FICHEIRO PARA OS OUTROS ESTADOS QUE PODEM SER 1,5,6 MUDO PARA 2: 
                        //CALIBRACO SEM VALIDACAO = o utilizador pode validar
                  
                        //Apenas os SERVIÇOS COM idEstadoCertificado = 2 É SAO  PARA APROVAR OU REJEITAR
                        string strSearch = "refServico = '" + refServicoEmValidacao + "'"; 
                        DataRow[]  foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
                        if (foundRows.Length > 0)
                        {
                            foreach (DataRow dRow in foundRows) 
                            {
                                string observacoes = dRow["obsWorkflowCertificado"].ToString();
                                string idEstadoActual = dRow["idEstadoCertificado"].ToString();
                                DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
                                int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
                                data = null;
                                if (iRes > -1) //pq essa parte la em cima pode dar erro
                                {
                                    dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
                                    dRow["estadoCertificado"] = "Calibrado sem Validação";
                                    DT.AcceptChanges();
                                }
                                break; 
                            }
                        }

                       

                        // se chegou ate aqui vai encaminhar para os diversos Mergefiles
                        if (drv["idTipoServico"].ToString() == "R") //se é uma reparação (tem uma template diferente e nunca tem simbolo
                        {
                            drv["podeAlterarTemplate"] = false;
                            resultMessage += mergeFilesReparacao(refServicoEmValidacao, fileCertificado[i], null, false, false);
                        }
                        else//não é uma reparação							
                        {
                            //CACV + DOSIMETRO SOM (nao deviam permitir alteraco template)	
                            if ((drv["refServico"].ToString().IndexOf("CACV", 0) > -1)
                                && (drv["equipamento"].ToString() == "DOSÍMETRO SOM")
                                && ((Convert.IsDBNull(drv["idSubtipoServico"]) || drv["idSubtipoServico"].ToString() == "32")))
                                {
                                    drv["podeAlterarTemplate"] = false;
                                    resultMessage += mergeFilesDosimetro(refServicoEmValidacao, fileCertificado[i], null, false, false);
                                }
                            //VACV COM CONDICOES
                            else if (drv["refServico"].ToString().IndexOf("VACV", 0) > -1)
                            {
                                drv["podeAlterarTemplate"] = false;
                                resultMessage += mergeFilesVACV(refServicoEmValidacao, fileCertificado[i], null, false, false);
                            }
                            //ECH
                            else if (drv["refServico"].ToString().IndexOf("ECH", 1) > -1)
                            {

                                drv["podeAlterarTemplate"] = false; //aqui nao sei se pode alterar?
                                resultMessage += mergeFilesECH(refServicoEmValidacao, fileCertificado[i], null, false, false);
                            }
                            //TODOS OS OUTROS
                            else
                            {
                                resultMessage += mergeFiles(refServicoEmValidacao, fileCertificado[i], nomeFicheiroEncontrado, drv["simbolo"].ToString(), System.Convert.ToBoolean(drv["acreditado"].ToString()), false);
                            }
                        }
                        if (resultMessage.IndexOf(MSG_PDF_CORROMPIDO) > -1)//Janeiro 2009
                        {
                            drv["msgColunaFicheiro"] = MSG_PDF_CORROMPIDO;
                            drv["idEstadoCertificado"] = "1"; //é como se fosse não existente, e fica disabled no loop que faz enable/disable das checkboxes
                        }
                    }
                }
            }

            resultMessage = resultMessage.Replace(MSG_PDF_CORROMPIDO, "");
            lblMessage.Text += resultMessage; //.... tenho de remover daqui agora a msg do pdfCorrompido.
            return resultMessage;

        }


        private void trataAditamentos()
        {

            ////::::::::::::::::::::::::::::::::::::::::::::
            ////REVER ISTO!!! MAIS TARDE - testar e ver que linhas estao no grid 
            ////estou a fazer loop pelos ficheiros
            //if (drv["idTipoServico"].ToString() == "A" && (!nomeFicheiroEncontrado.EndsWith("A")))
            //{

            //    //vamos apagar sem mensagem mas nao sei sequer se chega aqui, os aditamentos teem de ser testados à parte
            //    fileCertificado[i].Delete();
            //    //throw new Exception(" - Aditamentos devem ser carregados com a extensao '1A', '2A' ou '3A'.");  //o certificado já existe?
            //}

            //if (nomeFicheiroEncontrado.EndsWith("A"))
            //{
            //    string sigla = nomeFicheiroEncontrado.Substring(nomeFicheiroEncontrado.Length - 2); //validar

            //    if (drv["idTipoServico"].ToString() != "A")//so se pode carregar aditamentos para serviços aditamentos.
            //    {
            //        throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Solicite a eliminação do ficheiro.");  //o certificado já existe, 
            //    }

            //    //se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.		
            //    string sSearch = "refServico = '" + refServicoEmValidacao + "'"; //nao estoua perceber?
            //    DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //    string refPai = fRows[0]["refServicoPai"].ToString();
            //    sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + sigla + "'";
            //    DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //    if (fR.Length > 0) //o certificado já foi criado 
            //    {
            //        throw new Exception(" - O aditamento já existe. (" + refPai + "-" + sigla + ")");
            //    }
            //}

            ////acho que a validacao inicial apanha este caso
            //////procurar se um certificado desse tipo já foi criado
            ////string strSearch = "refServico = '" + refServicoEmValidacao + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";
            ////DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            ////if (foundRows.Length > 0) //o certificado já foi criado 
            ////{
            ////    //testar e apagar este certificado
            ////    throw new Exception(" - O certificado já existe. ");
            ////}
        }
        private string mergeFiles(string refServico, FileInfo fileCertificado, string nomeFicheiroEmValidacao, string simboloAcreditacao, bool bAcreditado, bool bAlteracaoTemplate)
        {

            //sem contemplar ainda a alteracao da template neste codigo todo
            string resultMessage = "";

            string ficheiroPastaOrigem = pastaCertificadosOrigem + "\\" + nomeFicheiroEmValidacao;
            string ficheiroPastaConstrucao = pastaCertificadosFinaisConstrucao + "\\" + nomeFicheiroEmValidacao;
            string ficheiroPastaBackup = pastaCertificadosOrigemBackup + "\\" + nomeFicheiroEmValidacao;
            string caminhoSimboloAcreditacao = pastaSimbolos + "\\" + simboloAcreditacao.Trim() + ".gif";
            Document DocumentoFinal = new Document(PageSize.A4);

            try
            {

                iTextSharp.text.Image imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
                iTextSharp.text.Chunk nomeLaboratorioHeader = new Chunk();
                iTextSharp.text.Font ARIAL_NORMAL_FONT = FontFactory.GetFont("Arial", 11);
                nomeLaboratorioHeader.Font = ARIAL_NORMAL_FONT;
                string nomeLaboratorio = strNomeLaboratorio(simboloAcreditacao);

                PdfReader readerRelatorio = new PdfReader(ficheiroPastaOrigem);
                PdfReader readerTemplate = new PdfReader(caminhoTemplateNaoAcreditada);
                if (bAcreditado == true)
                {
                    readerTemplate = new PdfReader(caminhoTemplateAcreditada);
                }

                FileStream fileST = new FileStream(ficheiroPastaConstrucao, FileMode.Create); //--> de ORIGEM --> CONSTRUCAO

                PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);
                DocumentoFinal.Open();

                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage pageRelatorio;
                PdfImportedPage pageTemplate;

                int j = 0;
                while (j < readerRelatorio.NumberOfPages)
                {
                    j++;
                    DocumentoFinal.SetPageSize(PageSize.A4);
                    DocumentoFinal.NewPage();
                    pageRelatorio = writer.GetImportedPage(readerRelatorio, j);
                    pageTemplate = writer.GetImportedPage(readerTemplate, 1);

                    cb.AddTemplate(pageTemplate, 0, 0); //na posicao 0-0

                    if (caminhoSimboloAcreditacao != null)
                    {
                        cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760);
                    }

                    nomeLaboratorioHeader.Append(nomeLaboratorio);

                    iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);
                    iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
                    ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);

                    ct.Go();

                    cb.AddTemplate(pageRelatorio, 0, 0);
                    //o 3º param empurro de baixo para cima ////40 px : 1 cm e qq coisa
                    //o 2º param empurro de esquerda para a direita
                }


                DocumentoFinal.Close();

                //GUARDAR UMA COPIA NA PASTA BACKUP E APAGAR
                fileCertificado.CopyTo(ficheiroPastaBackup, true);
                fileCertificado.Delete();

                ////3ºMover o ficheiro Orginal para a pasta backup para guardar uma versao original do ficheiro para ir buscar mais tarde caso for ecessario.						
                //if (bAlteracaoTemplate == false) //ok faz sentido
                //{
                //	fileCertificado.CopyTo(ficheiroPastaBackup, true);
                //	fileCertificado.Delete();
                //}
            }
            catch (Exception e)
            {
                DocumentoFinal.Close();

                //switch(e.m futuramente pôr um switch aqui quando estiver bem definda a mensagem de erro; 
                if (e.Message.ToString() == "Rebuild failed: trailer not found.; Original message: PDF startxref not found.")
                {

                    resultMessage += MSG_PDF_CORROMPIDO; //indicacao que falta criar pdf

                }
                else if (e.Message.ToString().IndexOf("- O certificado já existe.") >= 0)
                {
                    resultMessage += fileCertificado.ToString().ToUpper() + e.Message.ToString() + "<br />";

                }
                else if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
                {
                    resultMessage += fileCertificado.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
                }
                else if (e.Message.ToString().IndexOf("O aditamento já existe") >= 0)
                {

                    resultMessage += fileCertificado.ToString().ToUpper() + e.Message.ToString() + "<br />";
                }
                else
                {
                    Response.Write(e.ToString() + "<br /><br />");
                    resultMessage += fileCertificado.ToString() + " " + e.Message.ToString() + "<br />";
                }
            }

            return resultMessage;
        }
        private string mergeFilesReparacao(string refServico, FileInfo fileCertificado, string simboloAcreditacao, bool bAcreditado, bool bAlteracaoTemplate)
        {
            return "ainda nao implementado";
        }
        private string mergeFilesDosimetro(string refServico, FileInfo fileCertificado, string simboloAcreditacao, bool bAcreditado, bool bAlteracaoTemplate)
        {

            ////ultimas 2 folhas nao levam simbolo e levam template nao acreditado
            ////as outras levam simbolo e template IPAQ
            string resultMessage = "";

            //Document DocumentoFinal = new Document(PageSize.A4);

            //try
            //{

            //	PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);
            //	PdfReader readerTemplate = new PdfReader(caminhoTemplateNaoAcreditada);
            //	PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateAcreditada);

            //	iTextSharp.text.Image imgSimboloAcreditacao;

            //	if (caminhoSimboloAcreditacao != null)
            //	{
            //		try
            //		{
            //			imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //		}
            //		catch
            //		{
            //			resultMessage += fileOriginal.ToString() + "Simbólo inválido. Fale com o RT" + caminhoSimboloAcreditacao;
            //			return resultMessage;

            //		}
            //	}

            //	FileMode fm = new FileMode();

            //	if (bAlteracaoTemplate)
            //	{
            //		fm = FileMode.Create; //vai criar um novo
            //	}
            //	else
            //	{
            //		string nomeFicheiro = fileOriginal.ToString();

            //		string refServico = nomeFicheiro.Replace("-", "/");
            //		refServico = refServico.Substring(0, refServico.Length - 7);

            //		string[] docIn = nomeFicheiro.Split("-,.".ToCharArray());
            //		string siglaIn = docIn[2];

            //		//***** novo, tem a ver com os aditamentos **********************fev.2008
            //		string letraSigla = siglaIn.Substring(1, 1);
            //		if (letraSigla.ToUpper() == "A")
            //		{
            //			//so se pode carregar aditamentos para serviços aditamentos.
            //			string s = refServico.Substring(0, 1);
            //			if (s.ToUpper() != "A")
            //			{
            //				throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Por favor elimine o ficheiro.");  //o certificado já existe, 
            //			}


            //			//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
            //			//strSearch = "refServico = 
            //			string sSearch = "refServico = '" + refServico + "'";
            //			DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			string refPai = fRows[0]["refServicoPai"].ToString();

            //			sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
            //			DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			if (fR.Length > 0) //o certificado já foi criado 
            //			{
            //				throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");

            //			}
            //		}



            //		//procurar se um certificado desse tipo já foi criado
            //		string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

            //		DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - O certificado já existe. ");

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3";
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0)
            //		{
            //			foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
            //			{
            //				string idServico = dRow["idServico"].ToString();
            //				string observacoes = dRow["obsWorkflowCertificado"].ToString();
            //				string idEstadoActual = dRow["idEstadoCertificado"].ToString();

            //				DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            //				int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null, null);
            //				data = null;

            //				dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
            //				dRow["estadoCertificado"] = "Calibrado sem Validação";
            //				DT.AcceptChanges();

            //				fm = FileMode.Create;
            //				break;
            //			}
            //		}
            //	}

            //	FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);
            //	PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);
            //	DocumentoFinal.Open();

            //	PdfContentByte cb = writer.DirectContent;

            //	PdfImportedPage pageRelatorio;
            //	PdfImportedPage pageTemplate;

            //	//ultimas 2 folhas nao levam simbolo e levam template nao acreditado
            //	//as outras levam simbolo e template IPAQ
            //	int cont = 1;
            //	int nPages = readerRelatorio.NumberOfPages;
            //	while (cont < nPages + 1) //dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1
            //	{

            //		DocumentoFinal.SetPageSize(PageSize.A4);
            //		DocumentoFinal.NewPage();
            //		pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);

            //		//este template é adcionado em todas as paginas menos as 2 ultimas
            //		if (cont < (nPages - 1)) //
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplateIPAC, 1);
            //		}
            //		else
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplate, 1);
            //		}

            //		cb.AddTemplate(pageTemplate, 0, 0); //na posicao 0-0

            //		//inserir simbolo em todas as paginas menos as duas ultimas
            //		if (cont < (nPages - 1))
            //		{
            //			if (caminhoSimboloAcreditacao != null)
            //			{
            //				imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //				//cb.AddImage(imgSimboloAcreditacao,65.31f,0,0,62.37f,470,760); //
            //				cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
            //			}

            //			//*************************************************************************************************
            //			//************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
            //			//em todas tambem menos nas 2 ultimas
            //			//*************************************************************************************************
            //			iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

            //			iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
            //			//60, 300, 100, 500
            //			//2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha
            //			//ct.SetSimpleColumn(myPhrase, 325, 640, 495, 770, 15, Element.ALIGN_LEFT);
            //			//ct.SetSimpleColumn(myPhrase, 310, 630, 495, 760, 15, Element.ALIGN_LEFT);
            //			ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
            //			ct.Go();

            //			//*************************************************************************************************
            //			//*************************************************************************************************
            //			//*************************************************************************************************
            //		}
            //		cb.AddTemplate(pageRelatorio, 0, 0);
            //		cont++;
            //	}
            //	DocumentoFinal.Close();

            //	//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
            //	if (!bAlteracaoTemplate)
            //	{
            //		fileOriginal.CopyTo(caminhoDocumentoBackup, true);
            //		fileOriginal.Delete();
            //	}
            //}
            //catch (Exception e)
            //{
            //	DocumentoFinal.Close();
            //	if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
            //	{
            //		resultMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
            //	}
            //	else
            //	{
            //		resultMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
            //	}
            //}

            return resultMessage;
        }
        private string mergeFilesVMLF(string refServico, FileInfo fileCertificado, string simboloAcreditacao, bool bAcreditado, bool bAlteracaoTemplate)
        {        //INICIO VMLF . SEMPRE ACREDITADO COM SIMBOLO E TEMPLATE MENOS NA ULTIMA PAGINA. 
                 // USA AS DUAS TEMPLATES, EM TODAS AS PAGNAS MENOS na ultima, SIMBOLO E TEMPLATE ACREDITADOS
            /// NA ULTIMA SEM SIMBOLO E TEMPLATE NAO ACREDITADO
            /// copiado do mergefilesdosimetros
            /// 
            /// 
            /// 2018: agora é ao contrario, é na primeira sem simbolo e template nao acreditado
            /// </summary>

            //a ultima pagina nao leva simbolo e leva template nao acreditada
            //as outras levam simbolo e template IPAQ (sempre)
            //Ha sempre mais de uma pagina, pq é sempre feito o anexo de um documento ao certificado. 

            //2018 agora é ao contrario, a primeira é que nao leva nem simbolo nem template acreditada
            string resultMessage = "";

            //Document DocumentoFinal = new Document(PageSize.A4);

            //try
            //{

            //	PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);
            //	PdfReader readerTemplate = new PdfReader(caminhoTemplateNaoAcreditada);
            //	PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateAcreditada);
            //	iTextSharp.text.Image imgSimboloAcreditacao;

            //	if (caminhoSimboloAcreditacao != null)
            //	{
            //		try
            //		{
            //			imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //		}
            //		catch
            //		{
            //			resultMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
            //			return resultMessage;

            //		}
            //	}

            //	FileMode fm = new FileMode();

            //	if (bAlteracaoTemplate)
            //	{
            //		fm = FileMode.Create; //vai criar um novo
            //	}
            //	else
            //	{
            //		string nomeFicheiro = fileOriginal.ToString();

            //		string refServico = nomeFicheiro.Replace("-", "/");
            //		refServico = refServico.Substring(0, refServico.Length - 7);

            //		string[] docIn = nomeFicheiro.Split("-,.".ToCharArray());
            //		string siglaIn = docIn[2];

            //		//***** novo, tem a ver com os aditamentos **********************fev.2008
            //		string letraSigla = siglaIn.Substring(1, 1);
            //		if (letraSigla.ToUpper() == "A")
            //		{
            //			//so se pode carregar aditamentos para serviços aditamentos.
            //			string s = refServico.Substring(0, 1);
            //			if (s.ToUpper() != "A")
            //			{
            //				throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Elimine ficheiro.");  //o certificado já existe, 
            //			}


            //			//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
            //			//strSearch = "refServico = 
            //			string sSearch = "refServico = '" + refServico + "'";
            //			DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			string refPai = fRows[0]["refServicoPai"].ToString();

            //			sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
            //			DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			if (fR.Length > 0) //o certificado já foi criado 
            //			{
            //				throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
            //																									  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
            //																									  //isto é: antes da migração foi criado à mao a referência de um certificado. 
            //																									  //normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem

            //			}
            //		}
            //		//++++ fim novo aditamentos +++


            //		//procurar se um certificado desse tipo já foi criado
            //		string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

            //		DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
            //																  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
            //																  //isto é: antes da migração foi criado à mao a referência de um certificado. 
            //																  //normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0)
            //		{
            //			foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
            //			{
            //				string idServico = dRow["idServico"].ToString();
            //				string observacoes = dRow["obsWorkflowCertificado"].ToString();
            //				string idEstadoActual = dRow["idEstadoCertificado"].ToString();

            //				DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            //				int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
            //				data = null;

            //				//aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
            //				//aqui e nos b sitios todos onde essa funcao é chamada

            //				dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
            //				dRow["estadoCertificado"] = "Calibrado sem Validação";
            //				DT.AcceptChanges();

            //				fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
            //									  //filemode é sempre create, pelos vistos
            //				break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
            //			}
            //		}
            //	}


            //	FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);

            //	PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

            //	DocumentoFinal.Open();

            //	PdfContentByte cb = writer.DirectContent;

            //	PdfImportedPage pageRelatorio;
            //	PdfImportedPage pageTemplate;


            //	//ultima folha nao leva simbolo e leva template nao acreditado
            //	//as outras levam simbolo e template IPAQ
            //	//2018: agora é ao contrario: a primeira é que nao leva nada disso, o resto leva.


            //	int cont = 1;
            //	int nPages = readerRelatorio.NumberOfPages;
            //	while (cont < nPages + 1)

            //	//dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1

            //	//nao está programado, mas é subentendido que o documento tem sempre no minimo 2 paginas.
            //	{

            //		DocumentoFinal.SetPageSize(PageSize.A4);
            //		DocumentoFinal.NewPage();
            //		pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);


            //		if (cont == 1) //
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplate, 1);

            //		}
            //		else
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplateIPAC, 1);
            //		}

            //		cb.AddTemplate(pageTemplate, 0, 0); //na posicao 0-0

            //		//inserir simbolo em todas as paginas menos a primeira
            //		if (cont > 1)
            //		{
            //			if (caminhoSimboloAcreditacao != null)
            //			{
            //				imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //				//cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
            //				cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
            //			}

            //			//*************************************************************************************************
            //			//************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
            //			// em todas menos nas duas ultimas
            //			//*************************************************************************************************
            //			iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

            //			iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
            //			//60, 300, 100, 500
            //			//2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha

            //			ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);

            //			ct.Go();

            //		}

            //		cb.AddTemplate(pageRelatorio, 0, 0);

            //		cont++;
            //	}

            //	DocumentoFinal.Close();

            //	//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
            //	if (!bAlteracaoTemplate)
            //	{
            //		fileOriginal.CopyTo(caminhoDocumentoBackup, true);
            //		fileOriginal.Delete();
            //	}
            //}
            //catch (Exception e)
            //{
            //	DocumentoFinal.Close();
            //	if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
            //	{
            //		resultMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
            //	}
            //	else
            //	{
            //		resultMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
            //	}
            //}

            return resultMessage;
        }
        private string mergeFilesVACV(string refServico, FileInfo fileCertificado, string simboloAcreditacao, bool bAcreditado, bool bAlteracaoTemplate)
        {

            string resultMessage = "";

            //Document DocumentoFinal = new Document(PageSize.A4);

            //try
            //{

            //	PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);
            //	PdfReader readerTemplate = new PdfReader(caminhoTemplateNaoAcreditada);
            //	PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateAcreditada);
            //	iTextSharp.text.Image imgSimboloAcreditacao;

            //	if (caminhoSimboloAcreditacao != null)
            //	{
            //		try
            //		{
            //			imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //		}
            //		catch
            //		{
            //			resultMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
            //			return resultMessage;

            //		}
            //	}

            //	FileMode fm = new FileMode();

            //	if (bAlteracaoTemplate)
            //	{
            //		fm = FileMode.Create; //vai criar um novo
            //	}
            //	else
            //	{
            //		string nomeFicheiro = fileOriginal.ToString();

            //		string refServico = nomeFicheiro.Replace("-", "/");
            //		refServico = refServico.Substring(0, refServico.Length - 7);

            //		string[] docIn = nomeFicheiro.Split("-,.".ToCharArray());
            //		string siglaIn = docIn[2];

            //		//***** novo, tem a ver com os aditamentos **********************fev.2008
            //		string letraSigla = siglaIn.Substring(1, 1);
            //		if (letraSigla.ToUpper() == "A")
            //		{
            //			//so se pode carregar aditamentos para serviços aditamentos.
            //			string s = refServico.Substring(0, 1);
            //			if (s.ToUpper() != "A")
            //			{
            //				throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Elimine ficheiro.");  //o certificado já existe, 
            //			}


            //			//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
            //			//strSearch = "refServico = 
            //			string sSearch = "refServico = '" + refServico + "'";
            //			DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			string refPai = fRows[0]["refServicoPai"].ToString();

            //			sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
            //			DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			if (fR.Length > 0) //o certificado já foi criado 
            //			{
            //				throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");
            //				//o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
            //				//nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
            //				//isto é: antes da migração foi criado à mao a referência de um certificado. 
            //				//normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem

            //			}
            //		}
            //		//++++ fim novo aditamentos +++


            //		//procurar se um certificado desse tipo já foi criado
            //		string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

            //		DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
            //																  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
            //																  //isto é: antes da migração foi criado à mao a referência de um certificado. 
            //																  //normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0)
            //		{
            //			foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
            //			{
            //				string idServico = dRow["idServico"].ToString();
            //				string observacoes = dRow["obsWorkflowCertificado"].ToString();
            //				string idEstadoActual = dRow["idEstadoCertificado"].ToString();

            //				DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            //				int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
            //				data = null;

            //				//aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
            //				//aqui e nos b sitios todos onde essa funcao é chamada

            //				dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
            //				dRow["estadoCertificado"] = "Calibrado sem Validação";
            //				DT.AcceptChanges();

            //				fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
            //									  //filemode é sempre create, pelos vistos
            //				break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
            //			}
            //		}
            //	}


            //	FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);

            //	PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

            //	DocumentoFinal.Open();

            //	PdfContentByte cb = writer.DirectContent;

            //	PdfImportedPage pageRelatorio;
            //	PdfImportedPage pageTemplate;


            //	//ultima folha nao leva simbolo e leva template nao acreditado
            //	//as outras levam simbolo e template IPAQ



            //	int cont = 1;
            //	int nPages = readerRelatorio.NumberOfPages;
            //	while (cont < nPages + 1)

            //	//dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1
            //	//nao está programado, mas é subentendido que o documento tem sempre no minimo 2 paginas.
            //	{

            //		DocumentoFinal.SetPageSize(PageSize.A4);
            //		DocumentoFinal.NewPage();
            //		pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);

            //		//este template é SO NAS PRIMERIAS DUAS
            //		if (cont < 3) //
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplateIPAC, 1);
            //		}
            //		else
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplate, 1);
            //		}

            //		cb.AddTemplate(pageTemplate, 0, 0); //na posicao 0-0

            //		//O SIMBOLO é SO NAS PRIMERIAS DUAS

            //		if (cont < 3)
            //		{
            //			if (caminhoSimboloAcreditacao != null)
            //			{
            //				imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //				//    cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
            //				cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
            //			}

            //			//*************************************************************************************************
            //			iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

            //			iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
            //			//60, 300, 100, 500
            //			//2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha

            //			ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
            //			ct.Go();

            //		}

            //		cb.AddTemplate(pageRelatorio, 0, 0);

            //		cont++;
            //	}

            //	DocumentoFinal.Close();

            //	//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
            //	if (!bAlteracaoTemplate)
            //	{
            //		fileOriginal.CopyTo(caminhoDocumentoBackup, true);
            //		fileOriginal.Delete();
            //	}
            //}
            //catch (Exception e)
            //{
            //	DocumentoFinal.Close();
            //	if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
            //	{
            //		resultMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
            //	}
            //	else
            //	{
            //		resultMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
            //	}
            //}

            return resultMessage;
        }
        private string mergeFilesECH(string refServico, FileInfo fileCertificado, string simboloAcreditacao, bool bAcreditado, bool bAlteracaoTemplate)
        {
            /// NA ULTIMA SEM SIMBOLO E TEMPLATE NAO ACREDITADO
            //ECH COM CONDICOES (SIMBOLO VEM HARDCODED DA BD??- VERIFICAR)
            //PODE SER ACREDITADO OU NAO, MAS SE É ACREDITADO, NAO LEVA NEM SIMBOLO NEM TEMPLATE NA ULTIMA PAGINA
            //o ECH NAO ACREDITADO USA O MERGEFILES NORMAL
            //======================================================================================================================================
            //a ultima pagina nao leva simbolo e leva template nao acreditada
            //as outras levam simbolo e template IPAQ (sempre)
            //Ha sempre mais de uma pagina, pq é sempre feito o anexo de um documento ao certificado. 


            string resultMessage = "";

            //Document DocumentoFinal = new Document(PageSize.A4);

            //try
            //{
            //	PdfReader readerRelatorio = new PdfReader(caminhoDocumentoOrigem);
            //	PdfReader readerTemplate = new PdfReader(caminhoTemplateNaoAcreditada);
            //	PdfReader readerTemplateIPAC = new PdfReader(caminhoTemplateAcreditada);

            //	iTextSharp.text.Image imgSimboloAcreditacao;

            //	if (caminhoSimboloAcreditacao != null)
            //	{
            //		try
            //		{
            //			imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //		}
            //		catch
            //		{
            //			resultMessage += fileOriginal.ToString() + " Problema com o simbolo. Verifique a nomenclatura do simbolo na página de gestão de tipos de equipamento (falar com Resp.Técn).<br />" + caminhoSimboloAcreditacao;
            //			return resultMessage;
            //		}
            //	}

            //	FileMode fm = new FileMode();

            //	if (bAlteracaoTemplate)
            //	{
            //		fm = FileMode.Create; //vai criar um novo
            //	}
            //	else
            //	{
            //		string nomeFicheiro = fileOriginal.ToString();

            //		string refServico = nomeFicheiro.Replace("-", "/");
            //		refServico = refServico.Substring(0, refServico.Length - 7);

            //		string[] docIn = nomeFicheiro.Split("-,.".ToCharArray());
            //		string siglaIn = docIn[2];

            //		//***** novo, tem a ver com os aditamentos **********************fev.2008
            //		string letraSigla = siglaIn.Substring(1, 1);
            //		if (letraSigla.ToUpper() == "A")
            //		{
            //			//so se pode carregar aditamentos para serviços aditamentos.
            //			string s = refServico.Substring(0, 1);
            //			if (s.ToUpper() != "A")
            //			{
            //				throw new Exception(" - So pode carregar aditamentos para serviços de aditamento. Elimine ficheiro.");  //o certificado já existe, 
            //			}


            //			//se tem um pai e já existe um certificado do mesmo estilo para esse pai, tb tenho de mandar para tras.
            //			//strSearch = "refServico = 
            //			string sSearch = "refServico = '" + refServico + "'";
            //			DataRow[] fRows = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			string refPai = fRows[0]["refServicoPai"].ToString();

            //			sSearch = "refServico = '" + refPai + "' AND tipoCertificadoSigla ='" + siglaIn + "'";
            //			DataRow[] fR = DT.Select(sSearch, null, DataViewRowState.CurrentRows);
            //			if (fR.Length > 0) //o certificado já foi criado 
            //			{
            //				throw new Exception(" - O certificado já existe. (" + refPai + "-" + siglaIn + ")");  //o certificado já existe, verificar se vai para à mensagem de erro la em baixo, com
            //																									  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
            //																									  //isto é: antes da migração foi criado à mao a referência de um certificado. 
            //																									  //normalmente os certificados que ja existem sao automaticamente apagados no apagaDocsIncorrectosPastaOrigem

            //			}
            //		}
            //		//++++ fim novo aditamentos +++


            //		//procurar se um certificado desse tipo já foi criado
            //		string strSearch = "refServico = '" + refServico + "' AND tipoCertificadoSigla ='" + siglaIn + "' ";

            //		DataRow[] foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - O certificado já existe. ");  //o certificado já existe, verificar se vai para a mensagem de erro la em baixo, com
            //																  //nota: isto tb provoca erro quando o certificado ja existe na tabela de certificados!!! 
            //																  //isto é: antes da migração foi criado à mao a referência de um certificado. 
            //																  //normalmente, os certificados que já existem na pasta destino sao automaticamente apagados.

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado =3"; //à espera de ser validado pelo sup
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0) //o certificado já foi criado 
            //		{
            //			throw new Exception(" - Não pode subsituir o documento; apague o documento e aguarde a validação do RT.");  //o certificado está à espera de ser validado pelo superior

            //		}

            //		strSearch = "refServico = '" + refServico + "' AND idEstadoCertificado <> 3";
            //		foundRows = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            //		if (foundRows.Length > 0)
            //		{
            //			foreach (DataRow dRow in foundRows) //so vou fazer uma actualizacao!!! ao serviço, nao uma por cada row encontrada....
            //			{
            //				string idServico = dRow["idServico"].ToString();
            //				string observacoes = dRow["obsWorkflowCertificado"].ToString();
            //				string idEstadoActual = dRow["idEstadoCertificado"].ToString();

            //				DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            //				int iRes = data.UpdateEstadoCertificado(idServico, idEstadoActual, "2", observacoes, null);
            //				data = null;

            //				//aqui so posso actualizar se correu bem..... portanto tenho de validar se correu bem, 
            //				//aqui e nos b sitios todos onde essa funcao é chamada

            //				dRow["idEstadoCertificado"] = "2"; //actualizar o estado do certificado dentro da DT tb
            //				dRow["estadoCertificado"] = "Calibrado sem Validação";
            //				DT.AcceptChanges();

            //				fm = FileMode.Create; //cria novo ficheiro; se já existe, overwrite, não dá erro
            //									  //filemode é sempre create, pelos vistos
            //				break; //para nao percorrer as outras linhas possiveis, que já nao interssam para o assunto
            //			}
            //		}
            //	}


            //	FileStream fileST = new FileStream(caminhoDocumentoDestino, fm);

            //	PdfWriter writer = PdfWriter.GetInstance(DocumentoFinal, fileST);

            //	DocumentoFinal.Open();

            //	PdfContentByte cb = writer.DirectContent;
            //	PdfImportedPage pageRelatorio;
            //	PdfImportedPage pageTemplate;

            //	//ultima folha nao leva simbolo e leva template nao acreditado
            //	//as outras levam simbolo e template IPAQ

            //	int cont = 1;
            //	int nPages = readerRelatorio.NumberOfPages;
            //	while (cont < nPages + 1)

            //	//dm. contador começa a 1 e tem que estar mais pequeno qu pagecount +1

            //	//nao está programado, mas é subentendido que o documento tem sempre no minimo 2 paginas.
            //	{

            //		DocumentoFinal.SetPageSize(PageSize.A4);
            //		DocumentoFinal.NewPage();
            //		pageRelatorio = writer.GetImportedPage(readerRelatorio, cont);

            //		//este template é adcionado em todas as paginas na 2 ultimas ??
            //		if (cont < (nPages)) //
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplateIPAC, 1);
            //		}
            //		else
            //		{
            //			pageTemplate = writer.GetImportedPage(readerTemplate, 1);
            //		}


            //		cb.AddTemplate(pageTemplate, 0, 0); //na posicao 0-0

            //		//inserir simbolo em todas as paginas menos a ultima
            //		if (cont < (nPages))
            //		{
            //			if (caminhoSimboloAcreditacao != null)
            //			{
            //				imgSimboloAcreditacao = iTextSharp.text.Image.GetInstance(caminhoSimboloAcreditacao);
            //				//    cb.AddImage(imgSimboloAcreditacao, 65.31f, 0, 0, 62.37f, 470, 760); //
            //				cb.AddImage(imgSimboloAcreditacao, 118f, 0, 0, 62.37f, 470, 760); //
            //			}


            //			//************escrever texto, nesto caso o nome do laboratório associado ao simbolo de acreditacao
            //			// em todas menos nas duas ultimas
            //			//*************************************************************************************************
            //			iTextSharp.text.Phrase myPhrase = new Phrase(nomeLaboratorioHeader);

            //			iTextSharp.text.pdf.ColumnText ct = new ColumnText(cb);
            //			//60, 300, 100, 500
            //			//2º param mostra na horizontal onde começa e o 4ç onde acaba, o terceiro na vertical e o quinto, o penultimo acho   q  a altura da linha

            //			ct.SetSimpleColumn(myPhrase, 310, 625, 485, 770, 12, Element.ALIGN_LEFT);
            //			ct.Go();

            //		}

            //		cb.AddTemplate(pageRelatorio, 0, 0);

            //		cont++;
            //	}

            //	DocumentoFinal.Close();

            //	//3ºMover o ficheiro Orginal para uma pasta que guarda os ficheiros para o caso de o utilizador					
            //	if (!bAlteracaoTemplate)
            //	{
            //		fileOriginal.CopyTo(caminhoDocumentoBackup, true);
            //		fileOriginal.Delete();
            //	}
            //}
            //catch (Exception e)
            //{
            //	DocumentoFinal.Close();
            //	if (e.Message.ToString().IndexOf("The process cannot access the file") >= 0)
            //	{
            //		resultMessage += fileOriginal.ToString().ToUpper() + " - Alguém tem este ficheiro em pdf aberto na Pasta Certificados; por favor, feche o mesmo.<br />";
            //	}
            //	else
            //	{
            //		resultMessage += fileOriginal.ToString() + " " + e.Message.ToString() + "<br />";
            //	}
            //}

            return resultMessage;
        }
        protected void cbAcreditado_checkedChanged(object sender, System.EventArgs e)
        {
            //=======================================================================================
            //que se quer aqui fazer é:
            //alterar esta informação na datatable
            //copiar um novo documento para a pasta CONSTRUCAO, COM OU SEM SIMBOLO, CONFORME ALTERACAO
            //isto é parecido com o mergepdfinicial so que ao contrario....
            //atanção que ha uns que sao hardcoded e onde isto nao funciona nem devia funcionar
            //ex os vacv + dosimetro e os cacv + ....
            //isto é chamado um por um, quer dizer por um servico a nao para a grid toda
            //=======================================================================================
            //// isto aqui está tudo mal!

            //CheckBox cb = (CheckBox)sender;
            //DataGridItem dgi = (DataGridItem)cb.Parent.Parent;
            //CheckBox cbAcreditado = (CheckBox)sender;
            //int gridIndex = dgi.ItemIndex;

            ////aqui acedo a celulas do datagrid pelo index...
            //string nomeDoc = dgCertificados.Items[gridIndex].Cells[20].Text.ToString(); 
            //string idServico = dgCertificados.DataKeys[gridIndex].ToString();  	
            //string acreditado = convertBoolToBit(cbAcreditado.Checked);

            ////guarda a alteração na datatable
            //DT = (DataTable)ViewState["DT"];
            //DV = new DataView(DT);
            //DV.Sort = "idServico"; //para o find que vem a seguir. //posso fazer o sort pela dataview mas o update à datatable
            //int index = DV.Find(dgCertificados.DataKeys[gridIndex]);
            //DT.Rows[index]["acreditado"] = cbAcreditado.Checked.ToString();
            //ViewState["DT"] = DT;
            //if (DT.Rows[index]["refServico"].ToString().IndexOf("R", 0, 1) > -1) 
            //	//se é uma reparação, nao quero alterar template nenhuma pq é sempre igual
            //{
            //	return;
            //}

            //Hashtable ht = HTIdCertificadoBySigla(); 

            //// AQUI VAI COPIAR 1 DOCUMENTO COM OU SEM SIMBOLO (ALTERACAO) DA PASTA ORIGEM BACKUP PARA A PASTA CONSTRUCAO
            ////DirectoryInfo dirInfoPastaBackupOrigem = new DirectoryInfo(pastaCertificadosOrigemBackup); 
            ////agora vai à pasta backup buscar uma copia do ficheiro original

            //FileInfo[] fi = dirInfoPastaBackupOrigem.GetFiles("*" + DT.Rows[index]["nomeFicheiroCertificado"].ToString() + "*"); //em princip.so 1 ficheiro
            //for (int i = 0; i < fi.GetLength(0); i++)
            //{
            //	string sigla = strExtraiSigla(fi[i].ToString());

            //	if (!ht.ContainsKey(sigla))//verificar se ok!
            //	{
            //		lblMessage.Text += "<br />" + GERAL.clsGeral.ErrorMessage.ERR_FILE_EXTENSION;
            //	}
            //	else
            //	{
            //		string caminhoDocumentoDestino = pastaCertificadosFinaisConstrucao + "\\" + fi[i].ToString().ToUpper();
            //		string caminhoDocumentoOrigem = pastaCertificadosOrigemBackup + "\\" + fi[i].ToString().ToUpper(); 
            //		//aqui a origem aponta para a dir backup... 
            //		string caminhoDocumentoBackup = pastaCertificadosOrigemBackup + "\\" + fi[i].ToString().ToUpper(); 
            //		//este param dps n vai ser usado. aponta tb para o backup
            //		string strSimboloAcreditacao = DT.Rows[index]["simbolo"].ToString().TrimEnd() + ".gif";
            //		string caminhoSimbolo = pastaSimbolos + "\\" + strSimboloAcreditacao;

            //		iTextSharp.text.Font ARIAL_NORMAL_FONT = FontFactory.GetFont("Arial", 11);
            //		iTextSharp.text.Chunk nomeLaboratorioHeader = new Chunk();
            //		nomeLaboratorioHeader.Font = ARIAL_NORMAL_FONT;

            //		if (cbAcreditado.Checked)
            //		//===========================================================================================
            //		//acreditado, tem template acreditada e tem simbolo (o caminho normal)
            //		//===========================================================================================
            //		{
            //			string nomeLaboratorio = strNomeLaboratorio(strSimboloAcreditacao);
            //			nomeLaboratorioHeader.Append(nomeLaboratorio);

            //			if (DT.Rows[index]["refServico"].ToString().IndexOf("R", 0, 1) > -1)
            //			{
            //				//nao faz nada pq a reparação n. tem simbolo.... 
            //			}

            //			else
            //			//se não é uma reparação
            //			{
            //				if ((DT.Rows[index]["refServico"].ToString().IndexOf("CACV", 0) > -1) && (DT.Rows[index]["equipamento"].ToString() == "DOSÍMETRO SOM") && ((Convert.IsDBNull(DT.Rows[index]["idSubtipoServico"])) || DT.Rows[index]["idSubtipoServico"].ToString() == "32"))
            //				{
            //					lblMessage.Text += mergeFilesDosimetro(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoSimbolo, caminhoDocumentoBackup, fi[i], true, nomeLaboratorioHeader);//true=bAlteracaoTemplate
            //				}

            //				else if (DT.Rows[index]["refServico"].ToString().IndexOf("ECH", 1) > -1)
            //				//se é acreditado, e quando chega aqui, é, entao nao leva o simolo + template ipac na ultima pagina
            //				//o mergefilesvmlf faz isso --> substituido pelo mergefilesECH
            //				{
            //					lblMessage.Text += mergeFilesECH(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoSimbolo, caminhoDocumentoBackup, fi[i], true, nomeLaboratorioHeader);//false=bAlteracaoTemplate
            //				}
            //				else
            //				{

            //					lblMessage.Text +=mergeFiles(DT.Rows[index]["refServico"].ToString(), fi[i], null, false, false);

            //					///lblMessage.Text += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplateAcreditada, caminhoSimbolo, caminhoDocumentoBackup, fi[i], true, nomeLaboratorioHeader);//true=bAlteracaoTemplate
            //					//isto seria o mmergefile NORMAL COM SIMBOLO E TEMPLATE IPAC
            //					////	resultMessage += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplateAcreditada, caminhoSimboloAcreditacao, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
            //					////}

            //					////isto seria o mergefile normal sem simbolo e template sem ipac
            //					////{
            //					////	resultMessage += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplateNaoAcreditada, null, caminhoDocumentoBackup, fileInfosPastaOrigem[i], false, nomeLaboratorioHeader);//false=bAlteracaoTemplate
            //					////}
            //				}
            //			}
            //		}
            //		else
            //		//===========================================================================================
            //		//nao acreditado
            //		//===========================================================================================
            //		{
            //			nomeLaboratorioHeader.Append("");
            //			if (DT.Rows[index]["refServico"].ToString().IndexOf("R", 0, 1) > -1)
            //			{
            //				//nao faz nada 		
            //			}
            //			else
            //			//se não é uma reparação
            //			{
            //				if ((DT.Rows[index]["refServico"].ToString().IndexOf("CACV", 0) > -1) && (DT.Rows[index]["equipamento"].ToString() == "DOSÍMETRO SOM") && ((Convert.IsDBNull(DT.Rows[index]["idSubtipoServico"])) || DT.Rows[index]["idSubtipoServico"].ToString() == "32"))
            //				{
            //					lblMessage.Text += mergeFilesDosimetro(caminhoDocumentoDestino, caminhoDocumentoOrigem, null, caminhoDocumentoBackup, fi[i], true, nomeLaboratorioHeader);//true=bAlteracaoTemplate
            //																																																			//verificar, pois parece-me que os dosimetros sao hardcoded e teem sempre o simbole e template
            //				}
            //				else if (DT.Rows[index]["refServico"].ToString().IndexOf("VMLF", 0) > -1)
            //				{
            //					lblMessage.Text += mergeFilesVMLF(caminhoDocumentoDestino, caminhoDocumentoOrigem, null, caminhoDocumentoBackup, fi[i], true, nomeLaboratorioHeader);//true=bAlteracaoTemplate																									//verificar, pois parece-me que os dosimetros sao hardcoded e teem sempre o simbole e template
            //				}
            //				else
            //				{
            //					//nesta alteracao, caminho origem e caminho backup apontam os 2 para a pasta backup e alteracaotemplate = true
            //					lblMessage.Text += mergeFiles(caminhoDocumentoDestino, caminhoDocumentoOrigem, caminhoTemplateNaoAcreditada, null, caminhoDocumentoBackup, fi[i], true, nomeLaboratorioHeader);//true=bAlteracaoTemplate
            //				}
            //			}
            //		}
            //	}
            //}
        }


        private void VerificacaoPastaFinaisConst()
        {//========================================================================================================
         //valida os ficheiros existentes na directoria FINAIS CONSTRUCAO
         //usa a DT sem viewstate pois é chamado antes de qq postback
         //é chamado dps do MergeFiles.....???
         //========================================================================================================

            DirectoryInfo dirInfopastaCertificadosConstrucao = new DirectoryInfo(pastaCertificadosFinaisConstrucao);

            Hashtable HTIdCert = HTIdCertificadoBySigla(); //contem sigla / idTipoCertificado
            Hashtable HTDescCert = HTDescCertificadoBySigla(); //contem sigla /tipoCertificado(descricao)



            //nao deviam estar aqui certificados 1C de serviços com idEstadoCertificado = 1 --> apagar so o certificado
            //nao deviam estar aqui certificados 1C de serviços que nao estejam explicitamente nos estados:(6, 10,15,25) --> apagar so o certificado
            
            //nao deviam estar aqui sequer certificados que nao correspondem a nenhuma linha da datatable


            //nao deviam estar certificados de serviços anulados (mesmos estados da stored procedure, mas nao acho que apareçama qui)
            // sao  apagados automaticamente e a datarow tb é apagada. 
            string strSearchForServicosAnulados = "idEstadoServico IN (7,21,22,-1,20)";
            DataRow[] dRowsCertServicosAnulados = DT.Select(strSearchForServicosAnulados, null, DataViewRowState.CurrentRows);
            foreach (DataRow dRow in dRowsCertServicosAnulados)
            {
                FileInfo[] ficheiroApagar = dirInfopastaCertificadosConstrucao.GetFiles("*" + dRow["nomeFicheiroCertificado"].ToString() + "*");
                if (ficheiroApagar.Length > 0) 
                {
                    foreach (FileInfo f in ficheiroApagar) //apago  os ficheiros que estao a mais. 
                    {
                        try
                        {
                            f.Delete();
                        }
                        catch (Exception ex)
                        {
                            GERAL.clsWriteError.WriteLog("linha 1634 gestdocs - apagar ficheiro" + ex.ToString());
                        }
                    }
                }
                dRow.Delete();
            }
        
            //vai procurar os serviços marcados como tendo certificado e verificar se o certificado existe
            //caso contrário marca o nome doc com MSG_PDF_INEXISTENTE --> e a varival bFileExists com false NAO <--
            //o 5 e o 6 tb deviam ter documentos na pasta construção, mas se faltar um doc para o 5 + 6 nao é critico.         
            string strSearch = "idEstadoCertificado IN(2,5,6) ";  
            DataRow[] dRowsCertValidos = DT.Select(strSearch, null, DataViewRowState.CurrentRows);
            foreach (DataRow dRow in dRowsCertValidos)
            {
                FileInfo[] fi = dirInfopastaCertificadosConstrucao.GetFiles("*" + dRow["nomeFicheiroCertificado"].ToString() + "*");
                if (fi.Length == 0) //n encontrou
                {
                    dRow["msgColunaFicheiro"] = MSG_PDF_INEXISTENTE;
                    //bFileExists = false; //problema: aqui podem estar os inválidos de outro user, que eu NAO quero listar.				
                }
                else //encontrou ficheiro
                {
                    if (dRow["idEstadoCertificado"].ToString() == "2") //se é 2 escreve no nome ficheiro , serve para poder abrir tb os rejeitados, pelo campo nomeFicheiroCertificado (BD), em vez do campo "msgColunaFicheiro"
                        //quando mudar o estado para 3????
                    {
                        string sigla = strExtraiSigla(fi[0].ToString()); //atencao que isto pode correr mal
                        object x = HTIdCert[sigla];
                        int idTipoCert = System.Convert.ToInt16(x);
                        dRow["idTipoCertificado"] = idTipoCert.ToString(); //actualiza o campo "(ID) tipo de certificado" da DT
                        dRow["msgColunaFicheiro"] = fi[0].ToString(); //actualiza o campo "nome do ficheiro" da DT

                        object y = HTDescCert[sigla];
                        string descTipoCert = System.Convert.ToString(y);
                        dRow["tipoCertificado"] = descTipoCert;
                    }

                    //em todos os casos, escreve no campo nomeFicheiroCertificado out 2008
                    dRow["nomeFicheiroCertificado"] = fi[0].ToString();
                }
            }
            dirInfopastaCertificadosConstrucao = null;

            //[apaguei aqui grandes pedaços de código]

            //fazer uma especie de select distinct //nao sei se ainda preciso disto.
            //pq como o inner join é feito com a tabela certificado, no caso de ja existirem duas linhas na tabela certificado para um serviço, entao aparecem 2 linhas tb ao utilizador, com os mesmo dados...
            string sIdServico = "";
            foreach (DataRow dr in DT.Select(null, null, DataViewRowState.CurrentRows))
            {
                if (sIdServico == dr["idServico"].ToString())
                {
                    sIdServico = dr["idServico"].ToString(); //tem que ficar aqui tb
                    dr.Delete(); //isto faz uma especie de select distinct.

                }
                else
                {
                    sIdServico = dr["idServico"].ToString(); //tem que ficar no else, senao dá mensagem de que deletedrowinformation cannot be accessed
                }
            }
            DT.AcceptChanges(); //just in case me tenha esquecido algures...
        }
        private void BindGrid()
        {
            if (Page.IsPostBack)
            //da primeira vez a DT está em memoria pq nao houve nenhum postback, dps de um submit, tb devia recarregar tudo do principio.
            {
                DT = (DataTable)ViewState["DT"];
            }
            if (Session["idPerfil"].ToString() == "0") //se ele nao pode ver todos, logo nao é do perfil admin
            {
                //se ele pode ver tudo, nao se aplica simplesmente filtro nenhum e devolve-se tudo
                //----> RETIRAR O GRAU DA PAGINA ONDE SE ASSOCIA AS GRANDEZAS.
            }
            //4 RT + 5 TE MOSTRAM OS SERVIOS DAS GRANDEZAS ASSOCIADAS QUE LHES SAO ASSOCIADAS           
            if (Session["idPerfil"].ToString() == "4" || Session["idPerfil"].ToString() == "5")
            {
                string sGrandezas = strGrandezasRespLogado();
                DataRow[] drow = DT.Select(null, null, DataViewRowState.CurrentRows); //DIF ENTRE TRATAR A DATATABLE OU DATAVIEW?
                foreach (DataRow dr in drow)
                {
                    if (sGrandezas.IndexOf(dr["idGrandezaServico"].ToString()) == -1) //apaga os de outros users mas so de outras grandezas
                    {
                        if (dr["userNameEfectuouServico"].ToString() != ViewState["userNameUserLogado"].ToString())
                        {
                            try
                            {
                                dr.Delete();
                            }
                            catch (Exception ex)
                            {
                                GERAL.clsWriteError.WriteLog(ex.ToString());
                            }
                        }
                    }
                }
            }
            DV = new DataView(DT);
            //agora é sobre a dataview, que pode mudar a cada postback nquando os rows apagados da DT ja nao aparecem
            if (Session["idPerfil"].ToString() == "4" || Session["idPerfil"].ToString() == "5")
            {
                if (cbShowOnlyCertifsFromLoggedUser.Checked)
                {
                    DV.RowFilter = "belongsToUser = 1";
                }
            }
            if (Session["idPerfil"].ToString() == "6") //TECNICO, SO OS PROPRIOS OU REVISOES
            {
                // se pudera aqui isnull IttipoCertificado = 1 ??
                string rowfilter = " userNameEfectuouServico = '" + ViewState["userNameUserLogado"] + "' OR (isnull(idTipoCertificado,1) > 1 AND idUtilizadorValidouCertificado is null AND idGrandezaServico = '" + ViewState["idGrandezaUtilizadorLogado"] + "') ";
                DV.RowFilter = rowfilter;
            }
            DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
            dgCertificados.DataSource = DV;
            dgCertificados.DataBind();

            if (DT.Rows.Count > 0) //ver se chamei o acceptchanges antes de chegar aqui
            {//se ha registos

                dgCertificados.Visible = true;
            }
            else
            {
                //se nao ha registos
                dgCertificados.DataSource = null;
                dgCertificados.DataBind();
                dgCertificados.Visible = false;

                lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_NO_RECORDS;
            }
        }
        private void SaveCheckBoxValuesToDT()
        {
            //para todas as extensoes 1C, 1R, 2R, 1A etc. não pode ser validado um novo documento se já existe
            //um certificado com esse tipo. 

            //tb não podem entrar novos certificados que estejam à espera de serem validados pelo resp.tecn.
            //se ainda não existe a extensão, podem entrar todos menos os que estão no estado TRES (

            //PODEM ENTRAR TODOS MENOS OS QUE ESTÃO EM ESTADO 3 (VALIDADO PELO TECNICO)

            //GUARDA O VALORE DAS CHECKBOXES NA DATATABLE**************
            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT);
            DV.Sort = "idServico";  //para o find que vem a seguir. 
                                    //posso fazer o sort pela dataview mas o update à datatable

            //vai buscar os valodres das checkboxes
            foreach (DataGridItem dgi in dgCertificados.Items)
            {
                int index = DV.Find(dgCertificados.DataKeys[dgi.ItemIndex]);

                CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
                CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
                TextBox txtObs = (TextBox)dgi.Cells[0].FindControl("txtObservacoes");

                DT.Rows[index]["cbAprovar"] = cbAprovar.Checked.ToString();
                DT.Rows[index]["cbRejeitar"] = cbRejeitar.Checked.ToString();
                DT.Rows[index]["obsWorkflowCertificado"] = txtObs.Text;

            }

            DT.AcceptChanges(); //suponho que quando chamo o acceptchanges já tudo o que foi feito antes (rowstates) 
                                //ficam limpos, verificar

            ViewState["DT"] = DT;
        }
        private void updateBD()
        {
            //CÓDIGO PARA ALTERAR ESTADO DOS SERVIÇOS APROVADOS
            string strIds = "";
            string strIdsApagar = "";
            string strEstActual = "";
            string strEstNovo = "";
            string strtCert = "";

            DT = (DataTable)ViewState["DT"];

            string sSearch = "cbRejeitar = True"; //ele nao apanhar isto aqui... converter para algo!
            DataView DVR = new DataView(DT);
            DVR.RowFilter = sSearch;

            sSearch = "cbAprovar = True"; //ele nao apanhar isto aqui... converter para algo!
            DataView DVA = new DataView(DT);
            DVA.RowFilter = sSearch;

            //preciso de fazer aqui os search pq preciso de saber o tamanho de ambos para o array
            string[] obsWorkflow = new string[DVR.Count + DVA.Count]; //tem de ter o tamanho dos rejeitados e aprovados
            int n = 0;

            foreach (DataRowView drvR in DVR) //LOOP
            {
                //O idestadoCertificado aqui já foi alterado
                //eu quero saber qual o idEstadoCertificado que veio originalmente da bd. 
                //mas quando faço acceptchanges, estes dados desaparecem, --
                //por isso, para cada datarow, tenho de guardar a info de qual o estado em veio.
                //eu faço sempre acceptchanges a) pq a situação de ter necessidade de visualizer os
                //orignalrows so surgio à ultima da hora e pq ao fazer acceptchanges corto a informação 
                //desnecessaria para nao sobrecarregar a DT em viewstate

                strIds += drvR.Row["idServico"].ToString() + ","; //ids dos serviços
                strEstActual += drvR.Row["idEstadoCertificado"].ToString() + ",";
                strEstNovo += 5 + ",";   //AQUI MUDA O ESTADO PARA 5 : REJEITADO PELO TÉCNICO
                strtCert += drvR.Row["idTipoCertificado"].ToString() + ",";
                obsWorkflow[n] = drvR.Row["obsWorkflowCertificado"].ToString();
                n += 1;
            }

            //os arrays continuam a creser

            foreach (DataRowView drvA in DVA) //LOOP
            {
                strIds += drvA.Row["idServico"].ToString() + ","; //ids dos serviços
                strIdsApagar += drvA.Row["idServico"].ToString() + ",";
                strEstActual += drvA.Row["idEstadoCertificado"].ToString() + ",";
                strEstNovo += 3 + ",";  //AQUI MUDA O ESTADO PARA 3 : CALIBRADO COM VALIDAÇÃO
                strtCert += drvA.Row["idTipoCertificado"].ToString() + ",";
                obsWorkflow[n] = drvA.Row["obsWorkflowCertificado"].ToString();
                n += 1;
            }

            char[] delimiter = ",".ToCharArray();

            strIds = strIds.TrimEnd(delimiter);
            strIdsApagar = strIdsApagar.TrimEnd(delimiter);
            strEstActual = strEstActual.TrimEnd(delimiter);
            strEstNovo = strEstNovo.TrimEnd(delimiter);
            strtCert = strtCert.TrimEnd(delimiter);

            string[] idsServicos = strIds.Split(delimiter);
            string[] idsEstadosActuais = strEstActual.Split(delimiter);
            string[] idsEstadosNovos = strEstNovo.Split(delimiter);
            string[] idsTipoCertificado = strtCert.Split(delimiter);
            //obs ja está em array desde sempre....

            DATA.EstadoCertificadoBD estadoCertBD = new LabMetro.DATA.EstadoCertificadoBD();

            if (!estadoCertBD.UpdateEstadosCertificados(idsServicos, idsEstadosActuais, idsEstadosNovos, User.Identity.Name.ToString(), obsWorkflow, idsTipoCertificado))
            {
                lblMessage.Text += GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
            }
            else
            {
                //APAGA OS REGISTOS DA PARSTA ORIGINAIS BACKUP quando se aceita o documento
                if (strIdsApagar.ToString() != "")
                {
                    deleteDocs(strIdsApagar); //idsServicosApagar);
                }

                Server.Transfer("GestDocs2.aspx");
            }

        }
        public void dgCertificados_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            DataRowView DRV = (DataRowView)e.Item.DataItem;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];
                CheckBox cbAprovar = (CheckBox)e.Item.FindControl("cbAprovar");
                CheckBox cbRejeitar = (CheckBox)e.Item.FindControl("cbRejeitar");
                CheckBox cbAcreditado = (CheckBox)e.Item.FindControl("cbAcreditado"); //cbacreditado é filled inicialmente com o valor "acreditado da bd, mas dps pode ser desmarcado

                TextBox txtObs = (TextBox)e.Item.FindControl("txtObservacoes");
                txtObs.ToolTip = txtObs.Text.ToString();

                //novo, se não ha simbolo, acreditado sempre disabled
                if (DRV["simbolo"].ToString() == "") cbAcreditado.Enabled = false;

                //--------------------------------------------------------------------------------
                //independentemente de todo o resto, os anulados ficam sempre vermelhos e disabled
                //e os que não têm documento associado também. 
                //--------------------------------------------------------------------------------
                if (DRV["idEstadoCertificado"].ToString() == "5" || DRV["idEstadoCertificado"].ToString() == "6" || DRV["msgColunaFicheiro"].ToString() == MSG_PDF_INEXISTENTE || DRV["msgColunaFicheiro"].ToString() == MSG_PDF_CRIARNOVO || DRV["msgColunaFicheiro"].ToString() == MSG_PDF_CORROMPIDO) //diferente de rejeitado e diferentes de sem documentos....)
                {
                    for (int i = 1; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA380");
                    }

                    cbAprovar.Enabled = false;
                    cbRejeitar.Enabled = false;
                    cbAcreditado.Enabled = false;
                    txtObs.Enabled = false;
                }
                //---------------------------------------------------------------------
                //a não ser que estejam vermelhos
                //---------------------------------------------------------------------
                else if (DRV["belongsToUser"].ToString() == "False") //nao tirar o else
                {
                    for (int i = 1; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE5BF");
                    }
                }

                if (DRV["idEstadoCertificado"].ToString() != "1") //podem visualizar os rejeitados tb
                {
                    //colocar o link em todos as celulas da linha (para abrir o doc)
                    for (int i = 1; i < e.Item.Cells.Count; i++)
                    {
                        if (!e.Item.Cells[i].HasControls()) //para nao pôr o link nas cells que conteem checkboxes
                        {
                            e.Item.Cells[i].ToolTip = "Click para visualisar o documento. ";// + e.Item.Cells[5].Text;
                            e.Item.Cells[i].Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(button, ""));
                        }
                    }
                }
            }
        }
        public void SortGrid(Object s, DataGridSortCommandEventArgs e)
        {
            SaveCheckBoxValuesToDT();
            switch (ViewState["sortDirection"].ToString())
            {
                case "ASC":
                    ViewState["sortDirection"] = "DESC";
                    break;
                case "DESC":
                    ViewState["sortDirection"] = "ASC";
                    break;
            }

            ViewState["sortField"] = e.SortExpression;
            BindGrid();
        }
        private void btnAprovarAll_Click(object sender, System.EventArgs e)
        {
            // checkAll("aprovar");
            approveAll();
        }
        private void btnRejeitarAll_Click(object sender, System.EventArgs e)
        {
            checkAll("rejeitar");
        }
        private void btnDeselectAll_Click(object sender, System.EventArgs e)
        {
            checkAll("limpar");
        }

        private void approveAll()
        {
            //eu devia ver toda a grid que me é apresentada, e que posso aprovar
            //eu ao devia ter de validar mais nada, ou sou tecnic e aprovo todos os que estao à minha frente
            //ou sou RT e aprovo todos os que estao à minha frente, sendo que teem se der linhas que estao enabled
            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT);

            foreach (DataGridItem dgi in dgCertificados.Items)
            {
                CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");

                cbAprovar.Checked = true;

                DataRow[] dRow = DT.Select("idServico =" + dgi.Cells[1].Text.ToString());

                foreach (DataRow dr in dRow) //actualizar a datasource logo ao mesmo tempo
                {
                    dr["cbAprovar"] = true;
                    dr["cbRejeitar"] = false;
                  
                }
            }
            ViewState["DT"] = DT;
            dgteste.DataSource = DT;

        }
        private void checkAll(string accao)
        {
            string[] aGrandezas = sGrandezasUserResp(); //para so chamar 1 vez

            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT);

            bool bAprovar = false; //se é false, signfica que é para rejeitar (n. posso inicializar a null) 
            bool bLimpar = false;

            switch (accao)
            {
                case "aprovar":
                    bAprovar = true;
                    break;
                case "rejeitar":
                    bAprovar = false;
                    break;
                case "limpar":
                    bLimpar = true;
                    break;
            }

            foreach (DataGridItem dgi in dgCertificados.Items)
            {
                DataRow[] dRow = DT.Select("idServico =" + dgi.Cells[1].Text.ToString());

                //DataRowView DRV = dgi.DataItem; //aqui n funcionar por isso tenho de aceder aos campos invisiveis em vez de aceder à datasource do datagrid. 

                CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
                CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");
                TextBox txtObs = (TextBox)dgi.Cells[0].FindControl("txtObservacoes");
                CheckBox acreditado = (CheckBox)dgi.Cells[0].FindControl("acreditado");


                //1. se serviços REJEITADOS ou ainda nao carregados, tudo disabled para toda a gente
                if (dgi.Cells[9].Text == "5" || dgi.Cells[9].Text == "6" || dgi.Cells[9].Text == "1" || dgi.Cells[15].Text == "...") 
                {
                    cbAprovar.Enabled = false;
                    cbRejeitar.Enabled = false;
                    acreditado.Enabled = false;
                    txtObs.Enabled = false;
                    continue;
                }

                //2. tem se ser gestor de Certif. ou responsavel pela grandeza ou técnico (nesse caso pode aceitar todos os que lhe aparecem,  independentemente de serem dele ou nao, pois ja aparecem filtrados à partida

                //aqui havia um else
                //if( sGrau =="0" || bGrandezaRT(dgi.Cells[1].Text) || Session["idPerfil"].ToString() =="6") 
                //nao consigo p^^or a grandeza para dentro do item, desisto e uso substring.... 

                //tirei o strgray
                //if( sGrau =="0" || Array.IndexOf(aGrandezas,dgi.Cells[20].Text.Substring(1,3))>-1 || Session["idPerfil"].ToString() =="6") 

                if (Session["idPerfil"].ToString() == "0" || Array.IndexOf(aGrandezas, dgi.Cells[20].Text.Substring(1, 3)) > -1 || Session["idPerfil"].ToString() == "6")
                {
                    //Response.Write(Array.IndexOf(aGrandezas,dgi.Cells[20].Text.Substring(1,3)).ToString()); 

                    cbAprovar.Enabled = true;
                    cbRejeitar.Enabled = true;
                    txtObs.Enabled = true;
                    if (acreditado.Checked) acreditado.Enabled = true;

                    //---------------------------------------------------------------------------------
                    //DEPOIS AVALIAR OS QUE FICAM APROVADOS, REJEITADOS OU LIMPOS	
                    //E SO NAS CONDIÇÕES DESTE IF 
                    //---------------------------------------------------------------------------------
                    if (bLimpar) //se é para limpar, ficam todos limpos
                    {
                        cbAprovar.Checked = false;
                        cbRejeitar.Checked = false;
                        foreach (DataRow dr in dRow) //actualizar a datasource logo ao mesmo tempo
                        {
                            dr["cbAprovar"] = false;
                            dr["cbRejeitar"] = false;
                        }
                    }
                    else//uma das outras duas condições há de ser true ou false
                    {
                        cbAprovar.Checked = bAprovar;
                        cbRejeitar.Checked = !bAprovar;
                        foreach (DataRow dr in dRow) //actualizar a datasource logo ao mesmo tempo
                        {
                            dr["cbAprovar"] = bAprovar;
                            dr["cbRejeitar"] = !bAprovar;
                        }
                    }
                }
            }
            ViewState["DT"] = DT;

        }
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT); //dt está sempre a ser carregada no pageload

            string strSearch = "idServico is not null "; //para ter alguma coisa aqui...
            if (txtSearchEmpresa.Text != "") strSearch += " AND empresa LIKE '%" + txtSearchEmpresa.Text + "%'";
            if (txtSearchNServico.Text != "") strSearch += " AND refServico LIKE '%" + txtSearchNServico.Text + "%'";
            if (ddGrandeza.SelectedValue != "") strSearch += " AND idGrandezaServico = '" + ddGrandeza.SelectedValue + "'";
            if (Session["idPerfil"].ToString() != "0") //se ele NAO PODE VER TODOS
                                                            //se ele puder ver todos pq é do perfil Admin (ate podia ser o gestor de certicicados.... mas so que ng usa....
            {
                if (Session["idPerfil"].ToString() == "6") //partindo do principio que nenhum TL será jamais "gestor de certificados";
                {
                    strSearch += " AND belongsToUser = 1 ";
                }
            }

            DV.RowFilter = strSearch;

            dgCertificados.DataSource = DV;
            dgCertificados.DataBind();

        }
        private void btnLimparCampos_Click(object sender, System.EventArgs e)
        {
            LimpaCamposPesquisa();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //nao vou optimizar agora, too much work, concentrar o essencial
            SaveCheckBoxValuesToDT();
            if (checkRows() == false)
            {
                lblMessage.Text = "Verifique as checkboxes, não pode aprovar e rejeitar um certificado ao mesmo tempo.";
                return;
            }
            else
            {
                if (validacaoRegistos()) //futuramenet juntar com a funcao anterior.
                {
                    updateBD();
                }
                else
                {
                    lblMessage.Text = "Não escolheu nenhum Certificado.";
                }
            }
        }
        public bool checkRows()
        {
            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT);

            foreach (DataGridItem dgi in dgCertificados.Items)
            {
                CheckBox cbAprovar = (CheckBox)dgi.Cells[0].FindControl("cbAprovar");
                CheckBox cbRejeitar = (CheckBox)dgi.Cells[0].FindControl("cbRejeitar");

                if (cbAprovar.Checked && cbRejeitar.Checked)
                {
                    return false;
                }
            }
            return true;
        }
        public bool validacaoRegistos()
        {
            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT);

            foreach (DataRowView drv in DV) //LOOP
            {
                bool botaoAprovar = Convert.ToBoolean(drv.Row["cbAprovar"].ToString());
                bool botaoRejeitar = Convert.ToBoolean(drv.Row["cbRejeitar"].ToString());
                if (botaoAprovar || botaoRejeitar)
                {
                    return true;
                }
            }
            return false;
        }

        private void deleteDocs(string idsServico)
        {
            //1ºListar os ficheiros a apagar!!!

            DirectoryInfo dirInfoPastaBackupOrigem = new DirectoryInfo(pastaCertificadosOrigemBackup);

            DT = (DataTable)ViewState["DT"];
            DV = new DataView(DT);

            DV.RowFilter = "idServico in (" + idsServico.ToString() + ")";

            foreach (DataRowView drv in DV)
            {

                FileInfo[] fileInfosOriginalBackup = dirInfoPastaBackupOrigem.GetFiles("*" + drv.Row["nomeFicheiroCertificado"].ToString() + "*");
                if (fileInfosOriginalBackup.Length != 0)
                {
                    fileInfosOriginalBackup[0].Delete();
                }
            }
        }
        private bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
        private DataTable SelectDistinct(DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;

            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }

            return dt;

        }
        private string sGrandezaUtilizadorLogado()
        {
            DATA.UtilizadoresBD user = new LabMetro.DATA.UtilizadoresBD();
            string s = user.idGrandezeUser(User.Identity.Name.ToString());
            user = null;
            return s;
        }
        private bool bGrandezaRT(string idServico)
        {
            DATA.EstadoCertificadoBD g = new LabMetro.DATA.EstadoCertificadoBD();
            bool b = g.bGrandezaRT(idServico);
            g = null;
            return b;
        }
        private string[] sGrandezasUserResp()
        {//as grandezas dos users logados que sao tb responsáveis pelas grandezas, dos técncicos nao... 
            DATA.EstadoCertificadoBD certificadoBD = new LabMetro.DATA.EstadoCertificadoBD();
            DataTable DT = certificadoBD.DTListGrandezas(User.Identity.Name.ToString(), "");
            string[] s = new string[DT.Rows.Count];
            int i = 0;
            foreach (DataRow dr in DT.Rows)
            {
                s[i] = dr["idGrandeza"].ToString();
                i += 1;
            }
            certificadoBD = null;
            return s; //array;
        }
        //isto existia em array, mas eu vou mandar em string, assim dps faço um find à string
        private string strGrandezasRespLogado()
        {
            string sGrandezas = "";

            DATA.EstadoCertificadoBD certificadoBD = new LabMetro.DATA.EstadoCertificadoBD();
            SqlDataReader DR = certificadoBD.DRListGrandezas(User.Identity.Name.ToString(), "");

            while (DR.Read())
            {
                sGrandezas += DR["idGrandeza"].ToString() + ",";
            }

            DR.Close();
            certificadoBD = null;

            return sGrandezas.TrimEnd(",".ToCharArray()); //virgula fica,pois nao incomoda
        }
        public string strExtraiSigla(string nomeFicheiro)
        {
            try
            {
                string[] docIn = nomeFicheiro.Split("-,.".ToCharArray());
                string sigla = docIn[2];

                return sigla;
            }
            catch
            {
                return "";
            }
        }
        private void LimpaCamposPesquisa()
        {
            txtSearchNServico.Text = "";
            ddGrandeza.SelectedIndex = 0;
            txtSearchEmpresa.Text = "";
        }
        public static string convertBoolToBit(bool x)
        {
            if (x == true)
            {
                return "1";
            }
            return "0";
        }
        public string downloadpath(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = (string)ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CONSTRUCAO"];
                myPath = myPath + "/" + filename.ToString();
                return myPath;
            }
            return "#";
        }
        public void visualisarCertificado(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Select")
            {
                string doc = e.Item.Cells[20].Text; //campo nomeFicheiroCertificado
                string nome = downloadpath(doc);
                //Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");

                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language=JavaScript>");
                strScript.Append("window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');");
                strScript.Append("</script>");
                //RegisterClientScriptBlock("imprimefactura", strScript.ToString());
                ClientScript.RegisterClientScriptBlock(GetType(), "mostraCertif", strScript.ToString());
            }
        }
        public static Hashtable HTIdCertificadoBySigla()
        {
            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            SqlDataReader DR = data.DRTipoCertificado();

            Hashtable ht = new Hashtable();

            if (DR.HasRows)
            {
                while (DR.Read())
                {
                    ht.Add(DR["sigla"], DR["idTipoCertificado"]);
                }
            }
            DR.Close();
            data = null;
            return ht;
        }
        public static Hashtable HTDescCertificadoBySigla()
        {
            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            SqlDataReader DR = data.DRTipoCertificado();

            Hashtable ht = new Hashtable();

            if (DR.HasRows)
            {
                while (DR.Read())
                {
                    ht.Add(DR["sigla"], DR["tipoCertificado"]);
                }
            }
            DR.Close();
            data = null;
            return ht;
        }
        public static Hashtable HTDescCertificadoIdTipo()
        {

            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            SqlDataReader DR = data.DRTipoCertificado();
            Hashtable ht = new Hashtable();

            if (DR.HasRows)
            {
                while (DR.Read())
                {
                    ht.Add(DR["idTipoCertificado"].ToString(), DR["tipoCertificado"]);
                }
            }
            DR.Close();
            data = null;
            return ht;
        }

        private void setupClientScript()
        {
            string js = @"
			<script language=JavaScript> 
			function ConfirmarEscolha(btnWaiter) 
			{
				if (confirm('Tem a certeza dos registos seleccionados?'))
				{
					document.body.style.cursor=""wait"";
					return true;
				} 
				return false; 
			}
			</script>";
            //btnWaiter.setAttribute(""value"", ""Please Wait..."");
            //Register the script
            if (ClientScript.IsClientScriptBlockRegistered("ConfirmarEscolha"))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ConfirmarEscolha", js);
            }
        }
        //usado pelo grid
        protected void btnupload_Click(object sender, EventArgs e)
        {
            string path = (string)ConfigurationManager.AppSettings["PATHREL_CERT_ORIGINAIS"];

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
                    //Response.Write(filename2 + " - OK" + "<br />");
                }
                catch (Exception ex)
                {
                    Response.Write("Error a carregar el fichero " + myPath + "/" + filename2 + ". " + ex.ToString());
                    clsWriteError.WriteLog(ex.ToString());
                }
            }
            BindGrid();
        }

        private void cbShowOnlyCertifsFromLoggedUser_CheckedChanged(object sender, System.EventArgs e)
        {
            BindGrid();
        }

        private void FillDDGrandezas()
        {
            string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddGrandeza.DataSource = dr;
            ddGrandeza.DataBind();
            ddGrandeza.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            dr.Close();
        }

        bool isValidFileName(string strIn)
        {
            
                // o problema é que deixa passr coisas erradas tipo isto: Copy of CPRE3838 - 20 - 1C
            //qq carcater depois 2 numeros um traco um numero e um caracatcer (c,a,r,v) e ponto pdf
            string myPattern = ".*\\-[0-9]{2}\\-[0-9]{1}[c|C|a|A|r|R|v|V]{1}\\.[pP][dD][fF]";
            if (Regex.IsMatch(strIn, myPattern) == true)
            {
                if (strIn.EndsWith("C.pdf"))//se é um certificado tipo C so pode ser 1C (às vezes carregam 2c)
                {
                    if (strIn.EndsWith("1C.pdf")) return true;
                    return false;
                }
                return true;
            }
            return false;
        }

        private string stringDocs()
        {     //===============================================================================================
              //cria uma string com as refs calibração dos ficheiros que foram colocados na pasta ORIGEM
              //nao percebo para que serve.... eu queria validar os serviços e ver se eles teem algum certificado
              // mas eu nao quero ir buscar serviços por algum certificado mal carregad0
              //será que servem para os pais?
              //===============================================================================================
            string strDocs = "";

            DirectoryInfo dirInfoPastaOrigem = new DirectoryInfo(pastaCertificadosOrigem);
            FileInfo[] files = dirInfoPastaOrigem.GetFiles();

            foreach (FileInfo f in files)
            {
                if (!f.Name.EndsWith("1C.pdf")) //todos os que são diferentes de 1C
                {
                    string doc = f.Name;
                    if (!(doc.Length < 11 || doc.IndexOf("-") == -1)) //se tem menos de 11 caract, nao está bem.
                    {
                        doc = doc.Replace("-", "/");
                        doc = doc.Substring(0, doc.Length - 7);
                        //strDocs+="'"+doc+"',";  //mudar isto para a nova funcao de sql, nao leva as plicas
                        strDocs += doc + ",";
                    }
                }
            }
            strDocs = strDocs.TrimEnd(",".ToCharArray());
            return strDocs;
        }

        //usado pelo datagrid
        protected string ConverteEstado(bool b)
        {
            if (b == true) return "CONCLUSIVO";
            else return "---";
        }
        private string strNomeLaboratorio(string strSimboloAcreditacao)
        {
            switch (strSimboloAcreditacao)
            {
                case "M0046":
                case "M0046.gif":
                    return "Laboratório de Calibração em Metrologia Física";
                case "L0268":
                case "L0268.gif":
                    return "Laboratório de de Ensaios Físicos";
                case "M0059":
                case "M0059.gif":
                    return "Laboratório de Calibração em Metrologia Electro-Física";
                case "L0342":
                case "L0342.gif":
                    return "Laboratório de Ensaios de Controlo Dimensional";
                case "M0009":
                case "M0009.gif":
                    return "Laboratório de Metrologia Dimensional";
                case "L0610":
                case "L0610.gif":
                    return "Laboratório de Calibração em Metrologia FísicaLabmetro Saúde \n Laboratório de Metrologia Equipamento Clínico Hospitalar";
                default:
                    return "";

            }
        }
    }
}

