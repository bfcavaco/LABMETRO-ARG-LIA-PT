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

namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaRequisicoes.
	/// </summary>
	public partial class ListaRequisicoes : System.Web.UI.Page
	{
		
		
		DataTable DT; 
		DataView DV;
		
        
        private const string ID_PAG = "REQUISICOES_0";//NOME DA PAGINA

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Page.Form.DefaultButton = btnPesquisa.UniqueID;

            
            lblMessage.Text = ""; 

            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					Server.Transfer("Default.aspx?err=1",false);
				}
                else
                {
                    if(!Page.IsPostBack)
                    {
						ViewState["sortField"] = "idRequisicao";
						ViewState["sortDirection"] = "DESC";

						if(Request.QueryString["id"]!= null) 
						{
							fillDDEmpresa();  
							ddEmpresa.SelectedValue=Request.QueryString["id"].ToString(); 
						}
                    }
                    if(!ht.ContainsKey("REQUISICOES_1")) 
                    //se n tem permissoes para ver os detalhes das pastas de ensaio, desactivar o link
                    {
                        DGRequisicoes.Columns[7].Visible=false; 
                    }
                }
            }
        }

		private void fillDDEmpresa()
		{
			DATA.RequisicaoBD empresa = new LabMetro.DATA.RequisicaoBD(); 
			DataTable DT = empresa.DTListaEmpresasForListRequisicao(txtPesquisaEmpresa.Text, txtPesquisaNif.Text); 
			DataView DV = new DataView(DT);
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaNif.Text == "") &&(txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}


        private void BindGrid()
        {
			try
			{
				DATA.RequisicaoBD req = new LabMetro.DATA.RequisicaoBD(); 
				DT = req.DTListaRequisicoes(ddEmpresa.SelectedValue.ToString(),txtRefReq.Text.ToString(),ddValidade.SelectedValue.ToString(), ddCompletas.SelectedValue.ToString(),cbFicheiro.Checked,chbContrato.Checked);
            
				DV = new DataView(DT);
				DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 

				DGRequisicoes.DataSource =DV; 
				DGRequisicoes.DataBind(); 

				if(DV.Table.Rows.Count > 0)
				{
					DGRequisicoes.Visible=true;
				}
				else
				{
					DGRequisicoes.Dispose();
					DGRequisicoes.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
				}

				req = null; 
			}
			catch(Exception ex)
			{
				//Response.Write(ex.ToString()); 
				GERAL.clsWriteError.WriteLog(ex.ToString()); 
			}

			

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

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGRequisicoes.CurrentPageIndex = e.NewPageIndex;
            BindGrid(); 

        }   

        public void SortGrid(Object s, DataGridSortCommandEventArgs e)
        {
            switch (ViewState["sortDirection"].ToString())
            {
                case "ASC":
                    ViewState["sortDirection"]="DESC"; 
                    break;
                case "DESC":
                    ViewState["sortDirection"]="ASC";
                    break;
            }

            ViewState["sortField"] = e.SortExpression;
	
            BindGrid(); 

        }

        protected void btnPesquisa_Click(object sender, System.EventArgs e)
        {
            DGRequisicoes.CurrentPageIndex=0; 
            BindGrid(); 
			dgServicos.DataSource=null;
			dgServicos.DataBind(); 
        }

		protected string ConverteEstado(bool b)
        {
            if (b==true) 
            {
                return "sim";
            }
            return "não"; 
        }

        public string downloadpath(object filename)
        {
            if(filename!=null && filename.ToString()!="")
            {
                string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_URL"];
                
                myPath = myPath + "/" + filename.ToString(); 
                return myPath;
            }
            return "#"; 
 
        }

		private void BindGridServicos()
		{
		
				SqlParameter[] arrParams = new SqlParameter[1];
				arrParams[0] = new SqlParameter("@inIdRequisicao", DGRequisicoes.DataKeys[DGRequisicoes.SelectedIndex].ToString());
				
				DataTable dt = GERAL.clsDataAccess.SPExecuteDTParams("stpGetServicosByIdRequisicao",arrParams); 
				if(dt.Rows.Count > 0)
				{
					dgServicos.DataSource=dt; 
					dgServicos.DataBind(); 
					lblMessageServicos.Text="";
				}
				else
				{
					dgServicos.DataSource=null; 
					dgServicos.DataBind(); 
					lblMessageServicos.Text ="Esta requisição ainda não foi associada a quaisquer serviços na Base de Dados."; //<br />(Nota: isto não significa que não tenha sido já utilizada.)"; 
				}
			
			Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(!ht.ContainsKey("REQUISICOES_1")) //se n tem permissoes para ver os detalhes da empresas, desactivar o link
			{

				dgServicos.Columns[6].Visible=false;     
			}
		}


		protected void txtPesquisaEmpresa_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGRequisicoes.CurrentPageIndex=0; 
			BindGrid(); 
			dgServicos.DataSource=null;
			dgServicos.DataBind(); 
		}

		protected void txtPesquisaNif_TextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGRequisicoes.CurrentPageIndex=0; 
			BindGrid(); 
			dgServicos.DataSource=null;
			dgServicos.DataBind(); 
		}

		protected void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
			DGRequisicoes.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void ddEmpresa_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DGRequisicoes.CurrentPageIndex=0; 
			BindGrid(); 
		}

		protected void DGRequisicoes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			BindGridServicos(); 
			

			
		}

		protected System.Drawing.Color ConvertColor(int i, string codigoBloqueioSAP)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();
			 


			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Gold");
					break;
				case 2:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("DarkOrange");
					break;
				case 3: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Crimson");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}

			if(codigoBloqueioSAP =="03") //se a empresa está inactiva em SAP (n tem a ver com o nivelBloqueiolabmetro, martelada...)
			{
				colorName = (System.Drawing.Color)colConvert.ConvertFromString("PowderBlue");
			}
			return colorName; 
		}

		protected void desassociarRequisicao(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
		
			if(e.CommandName == "desassociarReq")
			{
				string strSQL = "UPDATE servico SET idRequisicao = null WHERE idServico =" +dgServicos.DataKeys[e.Item.ItemIndex]; 
				GERAL.clsDataAccess.myExecuteNonQuery(strSQL); 

			

                strSQL = "INSERT INTO [dbo].[HistoricoRequisicoes]   ([idServico],    [idRequisicao]  ,[accao],    [idUtilizador] ,[data])  VALUES (" + dgServicos.DataKeys[e.Item.ItemIndex] + ",null,'D'," + Session["UserID"] + ", getDate()) ";
                
                    GERAL.clsDataAccess.myExecuteNonQuery(strSQL);



                BindGridServicos();
			}
		}

    }
}
