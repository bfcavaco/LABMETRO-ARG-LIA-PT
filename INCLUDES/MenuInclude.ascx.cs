namespace LabMetro.INCLUDES
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Web.Security; 
    using System.Web.UI; 




    /// <summary>
    ///		Summary description for _Menu.
    /// </summary>
    public class MenuInclude : System.Web.UI.UserControl
    {
		protected System.Web.UI.HtmlControls.HtmlTable navcontainer;
		protected System.Web.UI.WebControls.Label lblTeste;

        protected System.Web.UI.HtmlControls.HtmlGenericControl navlistEmpresas;
        protected System.Web.UI.HtmlControls.HtmlGenericControl navlistEquipamentos;
        protected System.Web.UI.HtmlControls.HtmlGenericControl navlistGestao;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistOrcamentos;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistLaboratorios;
        protected System.Web.UI.HtmlControls.HtmlGenericControl navlistDocumentos;
        protected System.Web.UI.HtmlControls.HtmlGenericControl navlistFacturacao;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistVarios;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistEstatisticas;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistCertificados;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistMarcacoes;

        //protected System.Web.UI.WebControls.Button btnContratosCalibracao;
		//protected System.Web.UI.WebControls.Button btnVarios;
		protected System.Web.UI.WebControls.Button btnEmpresa;
		protected System.Web.UI.WebControls.Button btnOrcamento;
        protected System.Web.UI.WebControls.Button btnEquipamento;
        protected System.Web.UI.WebControls.Button btnGestao;
        protected System.Web.UI.WebControls.Button btnDocumentos;
        protected System.Web.UI.WebControls.Button btnFacturacao;
		protected System.Web.UI.WebControls.Button btnEstatisticas;
        protected System.Web.UI.WebControls.Button btnLaboratorios;
		protected System.Web.UI.WebControls.Button btnMarcacoes;
		protected System.Web.UI.WebControls.Button btnLogout;

        protected System.Web.UI.HtmlControls.HtmlTableCell tdEmp;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdEqu;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdGes;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdOrc;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdLab;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdDoc;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdFac;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdEst;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdCer;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdMar;

        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnEmp;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnEqu;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnGes;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnOrc;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnLab;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnDoc;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnFac;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnEst;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnMar;
		protected System.Web.UI.WebControls.Button btnCertificado;
		protected System.Web.UI.WebControls.Button btnAlerta;
		protected System.Web.UI.HtmlControls.HtmlTableCell Td1;
		protected System.Web.UI.HtmlControls.HtmlTableCell Td2;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnCon;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdCon;
		protected System.Web.UI.HtmlControls.HtmlGenericControl navlistConcessionarios;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnAdm;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdAdm;
		protected System.Web.UI.WebControls.Button btnConcessionarios;
		protected System.Web.UI.WebControls.Button btnBOAdmin;
		protected System.Web.UI.HtmlControls.HtmlTableCell t;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdBtnCer;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))
                {
                    Server.Transfer("Default.aspx",true);
                }    
                else
                {
                    DisableButtons(); //faz disable a tudo
                    setButtonPermissions(); //faz enable aos items para o qual existem permissoes;  
                    fillSubItems(); //preenche os links abaixo dos botoes
                }
              
            }
           
            if(Request.QueryString["btn"]!= null)
            {
                switch(Request.QueryString["btn"].ToString().ToUpper())
                {
                    case "EMP":
                        CloseNavLists(Page,1,"navlistEmpresas"); 
                        break; 
                    case "EQU":
                        CloseNavLists(Page,1,"navlistEquipamentos"); 
                        break; 
                    case "GES":
                        CloseNavLists(Page,1,"navlistGestao"); 
                        break; 
                    case "DOC":
                        CloseNavLists(Page,1,"navlistDocumentos"); 
                        break; 
                    case "FAC":
                        CloseNavLists(Page,1,"navlistFacturacao"); 
                        break; 
                    case "LAB":
                        CloseNavLists(Page,1,"navlistLaboratorios"); 
                        break; 
                    case "ORC":
                        CloseNavLists(Page,1,"navlistOrcamentos"); 
                        break; 
					case "EST":
						CloseNavLists(Page,1,"navlistEstatisticas"); 
						break;
					case "CER":
						CloseNavLists(Page,1,"navlistCertificados"); 
						break;
					case "CON":
						CloseNavLists(Page,1,"navlistConcessionarios"); 
						break;
					case "MAR":
						CloseNavLists(Page,1,"navlistMarcacoes"); 
						break;
					case "ADM": //É DIFERENTE
						CloseNavLists(Page,1,""); 
						break;
                    default:
                        CloseNavLists(Page,1,"");     
                        break;
                }
            }
            else //just in case
            {
                CloseNavLists(Page,1,"");     
            }

        }
                   
        private void DisableButtons()
        {
            btnDocumentos.Enabled=false;
            btnEmpresa.Enabled=false;
            btnEquipamento.Enabled=false; 
            btnFacturacao.Enabled=false; 
			btnEstatisticas.Enabled=false; 
            btnGestao.Enabled=false;
            btnLaboratorios.Enabled=false; 
            btnOrcamento.Enabled=false;
			btnCertificado.Enabled=false;
			btnConcessionarios.Enabled = false;
			btnMarcacoes.Enabled = false; 
			btnBOAdmin.Enabled = true;  //diferente. futuramente isto será resolvido com roles, mas até la...
            
            tdBtnDoc.Visible=false;
            tdBtnEmp.Visible=false;
            tdBtnFac.Visible=false; 
			tdBtnEst.Visible=false; 
            tdBtnGes.Visible=false; 
            tdBtnOrc.Visible=false;
            tdBtnLab.Visible=false; 
            tdBtnEqu.Visible=false;
			tdBtnCer.Visible=false;
			tdBtnCon.Visible=false;
			tdBtnMar.Visible=false;
			tdBtnAdm.Visible=true;  //diferente

        }

        private void setButtonPermissions()
        {  
            string delimStr = ",";
            char [] delimiter = delimStr.ToCharArray();
        	
			if(Session["sMenu"] == null)
			{
				string ProfileId = Session["idPerfil"].ToString(); 

				DATA.UtilizadoresBD users = new LabMetro.DATA.UtilizadoresBD();       

				Session["sMenu"] = users.strListMenuItems(ProfileId); //guardar em sessao
				//assim para cada user, so vai uma vez à bd
				users = null; 
			}
		 
			string  sMenuItems = Session["sMenu"].ToString(); //passar a sessão para string

			string[] strMenuItems = sMenuItems.Split(delimiter); //split da string

			foreach(string s in strMenuItems)
            {
                switch(s)
                {
                    case "EMP" : 
                       btnEmpresa.Enabled=true; 
                       tdBtnEmp.Visible=true;
                        break;
                    case "EQU" : 
                        btnEquipamento.Enabled=true;
                        tdBtnEqu.Visible=true;
                        break;
                    case "DOC" : 
                        btnDocumentos.Enabled=true; 
                        tdBtnDoc.Visible=true;
                        break;
                    case "FAC" : 
                        btnFacturacao.Enabled=true;
                        tdBtnFac.Visible=true;
                        break;
					case "EST" : 
						btnEstatisticas.Enabled=true;
						tdBtnEst.Visible=true;
						break;
                    case "GES" : 
                        btnGestao.Enabled=true; 
                        tdBtnGes.Visible=true;
                        break;
                    case "LAB" :
                        btnLaboratorios.Enabled=true; 
                        tdBtnLab.Visible=true; 
                        break; 
                    case "ORC" :
                        btnOrcamento.Enabled=true; 
                        tdBtnOrc.Visible=true; 
                        break; 
					case "CER" :
						btnCertificado.Enabled=true; 
						tdBtnCer.Visible=true; 
						break; 
					case "CON" :
						btnConcessionarios.Enabled=true; 
						tdBtnCon.Visible=true; 
						break; 
					case "MAR" :
						btnMarcacoes.Enabled=true; 
						tdBtnMar.Visible=true; 
						break; 
					case "ADM" :
						btnBOAdmin.Enabled=true; 
						tdBtnAdm.Visible=true; 
						break; 
                }
            }
        }   
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            InitializeComponent2();
            base.OnInit(e);
        }
		
        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			
			Load += new System.EventHandler(Page_Load);

		}

        private void InitializeComponent2()
        {
            btnLogout.Click += new System.EventHandler(btnLogout_Click);
            btnOrcamento.Click += new System.EventHandler(btnOrcamento_Click);
            btnEstatisticas.Click += new System.EventHandler(btnEstatisticas_Click);
            btnEmpresa.Click += new System.EventHandler(btnEmpresa_Click);
            btnEquipamento.Click += new System.EventHandler(btnEquipamento_Click);
            btnOrcamento.Click += new System.EventHandler(btnOrcamento_Click);
            btnLaboratorios.Click += new System.EventHandler(btnLaboratorios_Click);
            btnDocumentos.Click += new System.EventHandler(btnDocumentos_Click);
            btnLogout.Click += new System.EventHandler(btnLogout_Click);
            btnFacturacao.Click += new System.EventHandler(btnFacturacao_Click);
            btnGestao.Click += new System.EventHandler(btnGestao_Click);
			btnAlerta.Click += new System.EventHandler(btnAlerta_Click);
			btnEstatisticas.Click += new System.EventHandler(btnEstatisticas_Click);
			btnCertificado.Click += new System.EventHandler(btnCertificados_Click);
			btnConcessionarios.Click += new System.EventHandler(btnConcessionarios_Click);
			btnBOAdmin.Click += new System.EventHandler(btnBOAdmin_Click);
            btnMarcacoes.Click += new System.EventHandler(btnMarcacoes_Click);
        }
        #endregion

        private void CloseNavLists(Control parent,int level,string navcontainer)
        {
            foreach(Control ctrl in parent.Controls)
            {    
                if((ctrl.GetType().FullName.ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl") && (ctrl.ID.ToString() != navcontainer))
                {
                    ctrl.Visible=false;     
                }
                else if((ctrl.GetType().FullName.ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl") && (ctrl.ID.ToString() == navcontainer))
                {
                    ctrl.Visible=true;
                }                     
                if(ctrl.Controls.Count > 0)  CloseNavLists(ctrl, level + 1,navcontainer); 
            }
        }

        private void fillSubItems()
        {
            string myPage = currentPage(Request.RawUrl.ToString()); 

			if(Session["dtPages"] == null)
			{
				

				DATA.UtilizadoresBD users = new LabMetro.DATA.UtilizadoresBD();       

				Session["dtPages"] = users.dtPaginas(Session["idPerfil"].ToString()); //dataTatble
				//assim para cada user, so vai uma vez à bd
				users = null; 
			}

            
            
			DataTable DTPages = (DataTable) Session["dtPages"]; 
		
            DataView DVPages = new DataView(DTPages); 
            DVPages.Sort = "ordem ASC"; 
        	// =========================================================================
            //explicacao do rowfilter acessoTotal = soLeitura
            //as paginas com permissoes puras (sim/nao) teem o campo soLeitura a 0
            //e teem o campo acessototal a 0, pq nunca é preenchdido.
            //se aparecem como resultado, é pq o utilizador pode aceder.
            //as paginas com permissoes mista (leitura/escrita) teem o campo soLeitura a 1
            //se o acessoTotal = 0 (so leitura) sao acedidadas atraves de links e nao aparecem 
            //no menu, se aparecem como acessoTotal a 1, sao mostradas no menu.
            // =========================================================================

			// =========================================================================
			// EMPRESAS
			// =========================================================================
            DVPages.RowFilter = "shortname = 'EMP' AND (acesso = 1) AND (acessoTotal = soLeitura)"; 

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {
                    string id = drv["nomeFicheiro"].ToString(); 
                    int lenId = id.Length; 
                    //id = id.Substring(0,lenId-5); 
					
                    navlistEmpresas.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"' "; 
                    
                    if(id.ToString()==myPage) 
                    {
                        navlistEmpresas.InnerHtml+= "class=liSelected"; 
                    }
                    navlistEmpresas.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
                }
            }

			// =========================================================================
			// DOCUMENTOS
			// =========================================================================
            DVPages.RowFilter = "shortname = 'DOC' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {
                    string id = drv["nomeFicheiro"].ToString(); 
                    int lenId = id.Length; 
                    //id = id.Substring(0,lenId-5); 

                    navlistDocumentos.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"' "; 
                    
                    if(id.ToString()==myPage) 
                    {
                        navlistDocumentos.InnerHtml+= "class=liSelected"; 
                    }
                    navlistDocumentos.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
                }
            }

			// =========================================================================
			// GESTÃO
			// =========================================================================
            DVPages.RowFilter = "shortname = 'GES' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {

                    string id = drv["nomeFicheiro"].ToString(); 
                    int lenId = id.Length; 
                    //id = id.Substring(0,lenId-5); 

                    navlistGestao.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"' "; 
                    
                    if(id.ToString()==myPage) 
                    {
                        navlistGestao.InnerHtml+= "class=liSelected"; 
                    }
                    navlistGestao.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
                    
                }
            }

			// =========================================================================
			// FACTURAÇÃO
			// =========================================================================
            DVPages.RowFilter = "shortname = 'FAC' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {
                    string id = drv["nomeFicheiro"].ToString(); 
                    int lenId = id.Length; 
                    //id = id.Substring(0,lenId-5); 

                    navlistFacturacao.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"' "; 
                    
                    if(id.ToString()==myPage) 
                    {
                        navlistFacturacao.InnerHtml+= "class=liSelected"; 
                    }
                    navlistFacturacao.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
                }
            }

			// =========================================================================
			// ESTATÍSTICAS
			// =========================================================================
			DVPages.RowFilter = "shortname = 'EST' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

			if(DVPages.Table.Rows.Count > 0) 
			{
				foreach(DataRowView drv in DVPages)
				{
					string id = drv["nomeFicheiro"].ToString(); 
					int lenId = id.Length; 
					//id = id.Substring(0,lenId-5); 

					navlistEstatisticas.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"' "; 
                    
					if(id.ToString()==myPage) 
					{
						navlistEstatisticas.InnerHtml+= "class=liSelected"; 
					}
					navlistEstatisticas.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
				}
			}

			// =========================================================================
			// EQUIPAMENTOS
			// =========================================================================
            DVPages.RowFilter = "shortname = 'EQU' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {
                    string id = drv["nomeFicheiro"].ToString(); 
                    int lenId = id.Length; 
                    //id = id.Substring(0,lenId-5); 

                    navlistEquipamentos.InnerHtml+=  "<li "; 
					
                    
                    if(id.ToString()==myPage) 
                    {
                        navlistEquipamentos.InnerHtml+= "class=liSelected"; 
                    }
                    navlistEquipamentos.InnerHtml+="><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"'>"+drv["Pagina"]+"</a></li>"; 
                }
            }
    
			// =========================================================================
			// LABORATÓRIOS
            // o Form das pastas de ensaio nao deve aparecer no menu, por isso remove-lo 
            // aqui manualmente.
			// =========================================================================
            DVPages.RowFilter = "shortname = 'LAB' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {
                    string id = drv["nomeFicheiro"].ToString(); 
                    if(id!="FormPastaEnsaio.aspx") //par nao aparecer essa pagina no menu
                    {
                        int lenId = id.Length; 
                        //id = id.Substring(0,lenId-5); 
                    
                        navlistLaboratorios.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"' "; 
                    
                        if(id.ToString()==myPage) 
                        {
                            navlistLaboratorios.InnerHtml+= "class=liSelected"; 
                        }
                        navlistLaboratorios.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
                    }
                }
            }

			// =========================================================================
			// ORÇAMENTOS
			// =========================================================================
            DVPages.RowFilter = "shortname = 'ORC' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

            if(DVPages.Table.Rows.Count > 0) 
            {
                foreach(DataRowView drv in DVPages)
                {
                    string id = drv["nomeFicheiro"].ToString(); 
                    int lenId = id.Length; 
                    //id = id.Substring(0,lenId-5); 

                    navlistOrcamentos.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"'  "; 
                    
                    if(id.ToString()==myPage) 
                    {
                        navlistOrcamentos.InnerHtml+= "class=liSelected"; 
                    }
                    navlistOrcamentos.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
                }
            }           
			// =========================================================================
			// CERTIFICADOS
			// =========================================================================
			DVPages.RowFilter = "shortname = 'CER' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

			if(DVPages.Table.Rows.Count > 0) 
			{
				foreach(DataRowView drv in DVPages)
				{
					string id = drv["nomeFicheiro"].ToString(); 
					int lenId = id.Length; 
					//id = id.Substring(0,lenId-5); 

					navlistCertificados.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"'  "; 
                    
					if(id.ToString()==myPage) 
					{
						navlistCertificados.InnerHtml+= "class=liSelected"; 
					}
					navlistCertificados.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
				}
			}           


			// =========================================================================
			// CONCESSIONÁRIOS
			// =========================================================================
			DVPages.RowFilter = "shortname = 'CON' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

			if(DVPages.Table.Rows.Count > 0) 
			{
				foreach(DataRowView drv in DVPages)
				{
					string id = drv["nomeFicheiro"].ToString(); 
					int lenId = id.Length; 
					//id = id.Substring(0,lenId-5); 

					navlistConcessionarios.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"'  "; 
                    
					if(id.ToString()==myPage) 
					{
						navlistConcessionarios.InnerHtml+= "class=liSelected"; 
					}
					navlistConcessionarios.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
				}
			}          


			// =========================================================================
			// Marcações
			// =========================================================================
			DVPages.RowFilter = "shortname = 'MAR' AND (acesso = 1) AND (acessoTotal = soLeitura)";  

			if(DVPages.Table.Rows.Count > 0) 
			{
				foreach(DataRowView drv in DVPages)
				{
					string id = drv["nomeFicheiro"].ToString(); 
					int lenId = id.Length; 
					//id = id.Substring(0,lenId-5); 

					navlistMarcacoes.InnerHtml+=  "<li><a href='"+drv["nomeFicheiro"].ToString()+"?btn="+drv["shortname"].ToString()+"'  "; 
                    
					if(id.ToString()==myPage) 
					{
						navlistMarcacoes.InnerHtml+= "class=liSelected"; 
					}
					navlistMarcacoes.InnerHtml+=">"+drv["Pagina"]+"</a></li>"; 
				}
			}
			// =========================================================================
			// BO ADMIN NAO TEM NADA AQUI... ABRE O MENU NUMA JANELA À PARTE
			// =========================================================================
        }

		// =========================================================================
		// 
		// =========================================================================
        private string strNavList(string str)
        {
            string s=""; 
            switch(str)
            {
                
                case "EMP":
                    s= "navlistEmpresas"; 
                    break;
                case "LAB":
                     s=  "navlistLaboratorios"; 
                    break;
                case "GES": 
                     s=  "navlistGestao"; 
                    break; 
                case "EQU":
                     s=  "navlistEquipamentos"; 
                    break;
                case "DOC":
                     s= "navlistDocumentos"; 
                    break;
                case "FAC":
                     s=  "navlistFacturacao"; 
                    break;
				case "EST":
					s=  "navlistEstatisticas"; 
					break;
                case "ORC":
                     s=  "navlistOrcamentos"; 
                    break; 
				case "CER":
					s=  "navlistCertificados"; 
					break; 
				case "CON":
					s=  "navlistConcessionarios"; 
					break; 
				case "MAR":
					s=  "navlistMarcacoes"; 
					break; 
                }   
            return s; 
        }

        /// <summary>
        /// INCOMPLETO; NAO VOU USAR DE MOMENTO
        /// </summary>
        /// <param name="myUrl"></param>
        private string currentPage(string myUrl) 
        {
			try
			{
				
				string delimStr = "/?"; 
				char [] delimiter = delimStr.ToCharArray();
			
				string myExpression = myUrl; 
				string [] split = myExpression.Split(delimiter); 
			
				//por alguma razao, a primeira posicao do array vem vazia...
				//por consequencia, o len vem sempre com 1 a mais, len 2 = 1 item, sendo o primeiro um item "invisivel"... 
           			    
				//vou querer sempre a penultima posição, independentemente do que vier antes
				
				int len = split.Length; 
				
				//Response.Write("len do array:" + len.ToString() + "<br>"); 
				//foreach (string s in split)
				//{
				//	Response.Write("--" + s + "<br>"); 
				//}
				//faz a mesma coisa q as linhas acima
//				for(int n = 0; n < len; n++)
//				{
//					Response.Write("split"+n +" : " + split[n] + "<br>"); 
//				}

				
				string myPage = split[len-2]; //-1 por causa do zero based e mais -1 para remover a ultima posicao
				return myPage; 				
				
			}
			catch
			{
				//Response.Write(ex.ToString());
				return ""; 
			}
        }


        private void btnEmpresa_Click(object sender, System.EventArgs e)
        {
             if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Response.Redirect("Default.aspx",true);
            
            else CloseNavLists(Page,1,"navlistEmpresas"); 
        }

        private void btnEquipamento_Click(object sender, System.EventArgs e)
        {
			if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))
			{
				Server.Transfer("Default.aspx"); 
			}
			else 
			{
				CloseNavLists(Page,1,"navlistEquipamentos"); 
			}
        }

        private void btnOrcamento_Click(object sender, System.EventArgs e)
        {
            if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
            else CloseNavLists(Page,1,"navlistOrcamentos"); 
        }

        private void btnFacturacao_Click(object sender, System.EventArgs e)
        {
            if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
            else CloseNavLists(Page,1,"navlistFacturacao"); 
        }

		private void btnEstatisticas_Click(object sender, System.EventArgs e)
		{
            if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
			else CloseNavLists(Page,1,"navlistEstatisticas"); 
		}

        private void btnLaboratorios_Click(object sender, System.EventArgs e)
        {
            if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
            else CloseNavLists(Page,1,"navlistLaboratorios");
        }

        private void btnDocumentos_Click(object sender, System.EventArgs e)
        {
            if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
            else CloseNavLists(Page,1,"navlistDocumentos");
        }

        private void btnGestao_Click(object sender, System.EventArgs e)
        {
            if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
            else CloseNavLists(Page,1,"navlistGestao");
        }

		private void btnCertificados_Click(object sender, System.EventArgs e)
		{
			if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
			else CloseNavLists(Page,1,"navlistCertificados");
		}

		private void btnConcessionarios_Click(object sender, System.EventArgs e)
		{
			if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
			else CloseNavLists(Page,1,"navlistConcessionarios");
		}

		private void btnMarcacoes_Click(object sender, System.EventArgs e)
		{
			if((Session["idPerfil"] == null) ||(Session["idPerfil"].ToString()==""))Server.Transfer("Default.aspx"); 
            
			else CloseNavLists(Page,1,"navlistMarcacoes");
		
		}
	
		private void btnBOAdmin_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("BO/BODefault.aspx"); 
		}


        private void btnLogout_Click(object sender, System.EventArgs e)
        {
            
            FormsAuthentication.SignOut(); //removes authentication ticket
            //no web.config é indicado para negar acesso a users nao authenticados a todas as paginas
            //logo a partir dai ele nao deixa aceder mais.
            //para alem disso, é validado em cada pagina se a session está preenchida.
            
            //CloseNavLists(Page,1,""); 
            Session.Clear(); 
            Server.Transfer("Default.aspx");
            
        }

		private void btnAlerta_Click(object sender, System.EventArgs e)
		{
			Server.Transfer("Home.aspx"); 
		}

	


    }
 }
