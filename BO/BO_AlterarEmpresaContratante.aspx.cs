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
	/// Summary description for BO_AlterarEmpresaContratante.
	/// </summary>
	public partial class BO_AlterarEmpresaContratante : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			lblMessage.Text = "";
			ddEmpresaContratante.DataSource = null;
			ddEmpresaContratante.DataBind();
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

		private void fillDDEmpresaContratante(string search)
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(search,"","1","","","","","",""); //activas
			DataView DV = new DataView(DT);		
			ddEmpresaContratanteNova.DataSource = DV; ; 
			ddEmpresaContratanteNova.DataBind();
			empresa = null; 
			ddEmpresaContratanteNova.Items.Insert(0,new ListItem("","")); 


		}

		private void fillDDEmpresaSoActual(string idEmpresa)
		{
			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
			DataTable DT = empresa.DTEmpresas(idEmpresa); //so a actual

			DataView DV = new DataView(DT);

			ddEmpresaContratante.DataSource = DV; ;
			ddEmpresaContratante.DataBind();

            ddEmpresaContratante.DataSource = DV; //faço tb o bind da caixa da nova (depois podem mudar, para o caso de so quererem mudar a barte do tem acesso ou nao tem acesso certificados)
			ddEmpresaContratante.DataBind();


			empresa = null;
		}

		private void accaoBotaoPesquisa()
		{
			string strSQL = "SELECT empresa.idEmpresa, empresa.nome, idEmpresaContratante, idBRE, bEmpBrePodeverCertificados FROM BRE INNER JOIN EMPRESA on bre.idEmpresa = empresa.idEmpresa WHERE dbo.udfGetReferenciaBRE(BRE.idBRE) = '" + txtRefBRE.Text+"' "; 
			//fillDDEmpresaContratante(txtPesquisaEmpresaContratante.Text); 


			SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			string idEmpresaContratante = "";
			string bEmpBrePodeverCertificados = "";
			string idBRE = ""; 
			
			if(DR.HasRows)
			{
				
				while (DR.Read())
				{
			
					idEmpresaContratante = DR["idEmpresaContratante"].ToString();
					bEmpBrePodeverCertificados = GERAL.clsGeral.ConvertStringToBool(DR["bEmpBrePodeverCertificados"].ToString()).ToString();  ;
					idBRE = DR["idBRE"].ToString(); 
					ViewState["idBRE"] = idBRE; 
					lblEmpresa.Text = DR["nome"].ToString(); 
					
				}
			}

            DR.Close();
			
			try
			{
				//fillDDEmpresaContratante(txtSearchEmpresa.Text);
				fillDDEmpresaSoActual(idEmpresaContratante);
				
				ddEmpresaContratante.SelectedValue = idEmpresaContratante; 
			}
			catch

			{
				ddEmpresaContratante.SelectedValue = ""; 
			}
			rbEmpbrepodevercertificados.SelectedValue = bEmpBrePodeverCertificados;
			strSQL ="select refServico, estadoServico.descricao as Estado, idFactura from servico inner join estadoServico on servico.idEstadoServico = estadoServico.idEstadoServico where idBRE = "+idBRE; 
			DataTable DT = GERAL.clsDataAccess.ExecuteDT(strSQL); 
			dgServicosBRE.DataSource = DT; 
			dgServicosBRE.DataBind(); 
		
		}
		protected void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			accaoBotaoPesquisa(); 
			
		}

		protected void dgServicosBRE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void UpdateBD()
		{

			string idEmpresaContratante = ddEmpresaContratanteNova.SelectedValue.ToString(); 
			string strSQL; 

			if(idEmpresaContratante !="")
			{
				strSQL = "UPDATE BRE SET idEmpresaContratante = "+idEmpresaContratante + ", bEmpBrePodeverCertificados = "+rbEmpbrepodevercertificados.SelectedValue+"  WHERE idBRE =" + ViewState["idBRE"].ToString()   ; 
			}
			else
			{
				strSQL = "UPDATE BRE SET idEmpresaContratante = null, bEmpBrePodeverCertificados = 1 WHERE idBRE =" + ViewState["idBRE"].ToString()   ; 
			}
			
			
			lblMessage.Text = GERAL.clsDataAccess.myExecuteNonQuery(strSQL).ToString() + " Registo(s) alterado(s)";
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			UpdateBD(); 
			accaoBotaoPesquisa(); 
			
		}

		protected void btnSearchEmpresa_Click(object sender, System.EventArgs e)
		{
			//accaoBotaoPesquisa(); 
			fillDDEmpresaContratante(txtPesquisaEmpresaContratante.Text);
		}
	}
}
