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
using System.Data.SqlClient; 
using System.IO; 
using System.Configuration;

namespace LabMetro
{
	/// <summary>
	/// Summary description for ConsultaEstadoCertificado.
	/// </summary>
	public partial class ConsultaEstadoCertificado : System.Web.UI.Page
	{
	
		private static string pastaOrigem = (string)ConfigurationManager.AppSettings["PASTA_CERT_ORIGINAIS"];
		private static string pastaConstrucao = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CONSTRUCAO"];
		private string pastaCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];
		private const string ID_PAG = "CERTCONSESTADO_0";//NOME DA PAGINA
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
					// Put user code to initialize the page here
					ViewState["sortField"] = "refServico";
					ViewState["sortDirection"] = "DESC";
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

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			dgResult.CurrentPageIndex=0; 
			BindGrid(); 
		}

		private void BindGrid()
		{

            SqlParameter[] arrParams = new SqlParameter[1];
            arrParams[0] = new SqlParameter("@refServico", txtRefServico.Text);

            DataTable DT = GERAL.clsDataAccess.SPExecuteDTParams("stpWetEstadosServicosCertificados", arrParams);
            DataView DV = new DataView(DT);
            DV.Sort = ViewState["sortField"].ToString() + " " + ViewState["sortDirection"];
            dgResult.DataSource = DV;
            dgResult.DataBind(); 

		}

		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			dgResult.CurrentPageIndex = e.NewPageIndex; 
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

		protected void dgResult_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			findFilesForRefServico(); 
		}


		//aqui fazer tb algo em que possam ver e abrir os documentos dentro das varias pastas em que se encontram.
		//fazer tb uma validacao de... existe mas nao devia, devia existir em determindada pasta mas nao existe...
		//Mas isso demora mais tempo a fazer. 


		//importente, se existe um documento na pasta final nao pode existir nenhum com o mesmo nome em nenhuma das outras 
		//pastas inclusive backup, apagar automaticamente. 
		private void findFilesForRefServico()
		{
			string refServico = dgResult.DataKeys[dgResult.SelectedIndex].ToString(); 
			string nomeDocLinha =""; 

			DirectoryInfo dirInfoOrigem = new DirectoryInfo(pastaOrigem);
			DirectoryInfo dirInfoConstrucao = new DirectoryInfo(pastaConstrucao);
			DirectoryInfo dirInfoCertificados = new DirectoryInfo(pastaCertificados);
			
			
			foreach(DataGridItem dgi in dgResult.Items)

			{

				nomeDocLinha = refServico.Replace("/","-")+"-"+dgi.Cells[6].Text + ".pdf";

			
				FileInfo[] filesOrigem = dirInfoOrigem.GetFiles(nomeDocLinha);
				if(filesOrigem.Length >0) 
				{
			
					lblResult.Text = "Ficheiro " + nomeDocLinha + " : Verifique os dados do Ficheiro/Serviço."; 
				}
				
				FileInfo[] filesConstrucao = dirInfoConstrucao.GetFiles(nomeDocLinha);
				if(filesConstrucao.Length >0)
				{
			
					lblResult.Text ="Ficheiro " + nomeDocLinha + " : Documento está à espera de ser validado/aprovado."; 
				}

				FileInfo[] filesCertificados = dirInfoCertificados.GetFiles(nomeDocLinha);
				if(filesCertificados.Length >0) 
				{
			
					lblResult.Text ="Ficheiro " + nomeDocLinha + " : Certificado está criado e aprovado.<br />"; 
				}


			}

		}

		
	}
}
