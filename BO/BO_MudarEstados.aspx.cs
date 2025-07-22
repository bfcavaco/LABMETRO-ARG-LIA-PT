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



namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_MudarEstados.
	/// </summary>
	public partial class BO_MudarEstados : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Session["UserID"] == null) Response.Redirect("../Default.aspx",true); 

			lblMessage.Text = ""; 
			// Put user code to initialize the page here
//			if(!Page.IsPostBack)
//			{
//				bindDDTecnicos(); 
//			}
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

		private void resetData()
		{			
			txtRefServico.Text=""; 
			ViewState["idServico"] =""; 
			ddEstadoOrigem.Items.Clear(); 
			ddEstadoDestino.Items.Clear(); 
		
		}

		//==============================================================================================
		private void fillDDEstadoOriginal()
		{

			DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD(); 
			SqlDataReader dr = estado.DRGetListaEstadosServico(); 
			ddEstadoOrigem.DataTextField = "descricao"; 
			ddEstadoOrigem.DataValueField = "ident"; 
			ddEstadoOrigem.DataSource=dr; 
			ddEstadoOrigem.DataBind(); 

			ddEstadoOrigem.Items.Insert(0,new ListItem("","")); 
			dr.Close(); 

			estado = null; 

		}

		//==============================================================================================
		//==============================================================================================
		private void fillDDEstadoDestino()
		{
			if(ddEstadoOrigem.SelectedValue!="")
			{
				DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD(); 
				SqlDataReader dr = estado.DRGetEstadosServicosEsubsequentesBO(ddEstadoOrigem.SelectedValue); 
				ddEstadoDestino.DataTextField = "descricao"; 
				ddEstadoDestino.DataValueField = "idEstadoServico"; 
				ddEstadoDestino.DataSource=dr; 
				ddEstadoDestino.DataBind(); 

				
				dr.Close(); 

				estado = null; 
				
			}
			else
			{
				ddEstadoDestino.Items.Clear();  
			}
		}


		private void fillRazoes()
		{
			DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
		
			SqlDataReader DR =  servico.DRGetListaComentariosEstado(ddEstadoDestino.SelectedValue); 
			if(DR.HasRows)
			{

				ddComentarioEstado.DataSource = DR; 
				ddComentarioEstado.DataBind(); 
			}
			else
			{
				ddComentarioEstado.Items.Clear(); 
				ddComentarioEstado.DataSource = null;
				ddComentarioEstado.DataBind(); 
				
			}

			DR.Close(); 

			ddComentarioEstado.Items.Insert(0,new ListItem("",""));
					
			servico = null;
		}


		//private void bindDDTecnicos()
//		 {
//				   string strSQL = "SELECT Funcionario.idFuncionario, Funcionario.nomeAbreviado FROM         Funcionario INNER JOIN Utilizador ON Funcionario.idUtilizador = Utilizador.idUtilizador WHERE     (Funcionario.activo = 1) AND (Utilizador.idPerfil IN (4, 5, 6)) ORDER BY Funcionario.nome";
//
//				   ddTecnicoLaboratorio.DataSource = GERAL.clsDataAccess.ExecuteDR(strSQL); 
//				   ddTecnicoLaboratorio.DataBind(); 
//				   ddTecnicoLaboratorio.Items.Insert(0,new ListItem("","")); 
//		 }

		protected void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			accaoBotaoPesquisa(); 

		}

		private void accaoBotaoPesquisa()
		{
			string strSQL= "SELECT idServico, idEstadoServico from Servico WHERE refServico = '"+txtRefServico.Text+"'"; 
			SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			string idEstado = ""; 
			string idServico = ""; 
			if(DR.HasRows)
			{
				
				while (DR.Read())
				{
					idEstado = DR["idEstadoServico"].ToString(); 
					idServico = DR["idServico"].ToString(); 
					ViewState["idServico"] = idServico.ToString(); 
					
				}
			}

            DR.Close();

			fillDDEstadoOriginal(); 
			try
			{
				ddEstadoOrigem.SelectedValue = idEstado; 
				ddEstadoOrigem.Enabled=false;
				fillDDEstadoDestino(); 
			}
			catch

			{
			
			}

			if(ViewState["idServico"] != null )
			{
				BindGridHistorico(ViewState["idServico"].ToString()); 
			}
		}


		protected void ddEstadoDestino_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fillRazoes(); 
		}

		private void UpdateBD()

		{

			string idEstadoActual = ddEstadoOrigem.SelectedValue.ToString(); 
			string idEstadoNovo = ddEstadoDestino.SelectedValue.ToString(); 
			string idRazao = ddComentarioEstado.SelectedValue.ToString(); 
		

			
			string strSQL = "UPDATE Servico SET idEstadoServico = "+idEstadoNovo ; 

			if(idEstadoNovo == "7" || idEstadoNovo =="9")
			{
				if(idRazao=="") 
				{
					lblMessage.Text ="Indique a razão";
					return; 
				}
				strSQL +=" , idComentarioEstado = "+ idRazao; 
			}
			else
			{
					strSQL +=" , idComentarioEstado = null ";
			}
			//isto nao pode ser feito aqui pq o trigger escrever por cima!

//			if(idEstadoNovo == "6" || idEstadoNovo =="25")
//			{
//				if(idFuncionario=="") 
//				{
//					lblMessage.Text ="Indique o funcionário.";
//					return; 
//				}
//				strSQL +=" , idFuncionarioEfectuouServico = "+ idFuncionario; 
//			}
			
			if((idEstadoActual =="6" || idEstadoActual =="15"))
 
			{
				if(idEstadoNovo =="6" || idEstadoNovo =="15")
				{
					//nada	-martelada porque nao consigo pensar em ifs e elses...
				}
				else

				//se passa de calibrado para outro estado fora do workflow, tenho de fazer reset ao estado do certificado. 
				{
					strSQL +=" , idEstadoCertificado = 1 " ; 
				}
			}

			if(idEstadoActual =="15" && idEstadoNovo =="6") 
			{
				strSQL +=" , idBSE  = null " ; 
			}

			if(idEstadoActual =="14" && idEstadoNovo =="19") 
			{
				strSQL +=" , idBSE  = null " ; 
			}
			
			if(idEstadoActual =="23" && idEstadoNovo =="19") 
			{
				strSQL +=" , idBSE  = null " ; 
			}

			if(idEstadoActual =="14" && idEstadoNovo =="18") 
			{
				strSQL +=" , idBSE  = null " ; 
			}

			if(idEstadoActual =="22" && idEstadoNovo =="21") 
			{
				strSQL +=" , idBSE  = null " ; 
			}
			strSQL +=", dtAlteracao = getDate(), idUtilAlteracao = "+HttpContext.Current.Session["UserId"].ToString()+" WHERE idServico = "+ViewState["idServico"].ToString()+" AND idEstadoServico ="+idEstadoActual; 
			//Response.Write(strSQL); 

			//se o estado passa para qq um anterior à entrega, tem de se eliminar tb o bse. 
			
			
			lblMessage.Text = GERAL.clsDataAccess.myExecuteNonQuery(strSQL).ToString() + " Registo(s) alterado(s)";


		}
		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			UpdateBD(); 
			accaoBotaoPesquisa(); //para fazer reset.
			
		}

		private void BindGridHistorico(string idServico)
		{   

			DATA.ServicoBD servicos = new LabMetro.DATA.ServicoBD(); 
			try
			{
				//dgHistorico.DataSource = servicos.DTGetServicoHistoricoEstados(ViewState["idServico"].ToString()); 
				dgHistorico.DataSource = servicos.DTGetServicoHistoricoEstados(idServico); 
				dgHistorico.DataBind();

			}
			catch
			{
				servicos = null; 
				dgHistorico.DataSource = null;
				dgHistorico.DataBind(); 
			}
		}

	}
}
