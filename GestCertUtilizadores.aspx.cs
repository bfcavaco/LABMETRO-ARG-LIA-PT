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
using System.Configuration;



namespace LabMetro
{
	/// <summary>
	/// Summary description for GestCertUtilizadores.
	/// </summary>
	public partial class GestCertUtilizadores : System.Web.UI.Page
	{
        private const string ID_PAG = "CERTUTILIZADORES_1";//NOME DA PAGINA
		
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}

		private void InitializeComponent2()
		{             
			DGFuncionarios.ItemDataBound += new DataGridItemEventHandler(DGFuncionarios_ItemDataBound); 
			DGFuncionarios.ItemCommand +=new DataGridCommandEventHandler(DGFuncionarios_ItemCommand);
			DGGrandezas.ItemDataBound += new DataGridItemEventHandler(DGGrandezas_ItemDataBound); 
			DGGrandezas.ItemCommand +=new DataGridCommandEventHandler(DGGrandezas_ItemCommand);
		}
		#endregion



		protected void Page_Load(object sender, System.EventArgs e)
		{ 
			
            lblMessage.Text =""; 
            
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
						fillGrau(); //alterado, passa o grau para viewstate
						ViewState["sortFieldR"] = "funcionario";
						ViewState["sortDirectionR"] = "ASC";
						ViewState["sortFieldG"] = "grandeza";
						ViewState["sortDirectionG"] = "ASC";

                        BindGridFuncionarios();
                    }
                }
            }
        }

		//======================================================================================
		//BINDGRID DOS FUNCIONÁRIOS RELEVANTES PARA O WORKFLOW
		//======================================================================================
        private void BindGridFuncionarios()
        {
			string grau = ViewState["grau"].ToString();   
			//GRAU RETORNA 0 SE É GESTOR DE CERTIFICADO E 1 PARA OUTROS GRAUS.

			DATA.PerfisBD responsaveis = new LabMetro.DATA.PerfisBD();
			
            DataTable DT =  responsaveis.dtListaFuncionariosWorkflow(grau);
			DataView DV = new DataView(DT);
			DV.Sort =  ViewState["sortFieldR"].ToString()+ " " + ViewState["sortDirectionR"]; 

            DGFuncionarios.DataSource = DV;
            DGFuncionarios.DataBind(); 

			responsaveis = null; 

			if(grau == "0")
			{
				DGFuncionarios.Columns[6].Visible = true;	//Coluna de definição do grau do utilizador
			}

			DGGrandezas.Visible = false;
        }

		private void DGFuncionarios_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			DATA.PerfisBD responsaveis = new LabMetro.DATA.PerfisBD();

			if(e.Item.ItemType == ListItemType.EditItem)
			{
				//ddPerfil
				SqlDataReader DRPr =  responsaveis.ListaPerfisWorkflow(ViewState["grau"].ToString());
				DropDownList ddPerfil = (DropDownList)e.Item.FindControl("ddPerfilREdit");
				ddPerfil.DataSource = DRPr;
				ddPerfil.DataBind();
				DRPr.Close();

				//como a datasource é um reader e nao um dataset, tem de ser feito assim....
				//System.Data.Common.DbDataRecord dbPerfil = (System.Data.Common.DbDataRecord)e.Item.DataItem;
				DataRowView drv = (DataRowView)e.Item.DataItem;
				//string perfil = dbPerfil.GetValue(3).ToString(); 
				string perfil = drv["idPerfil"].ToString();
				ddPerfil.SelectedValue = perfil.ToString();

				//ddGrau
				SqlDataReader DRGr =  responsaveis.ListaGrauWorkflow();
				DropDownList ddGrau = (DropDownList)e.Item.FindControl("ddGrauEdit");
				ddGrau.DataSource = DRGr;
				ddGrau.DataBind();
				DRGr.Close();
				
				//como a datasource é um reader e nao um dataset, tem de ser feito assim....
				//System.Data.Common.DbDataRecord dbGrau = (System.Data.Common.DbDataRecord)e.Item.DataItem;
				//string grauFuncionario = dbGrau.GetValue(5).ToString(); 
				string grauFuncionario = drv["idGrau"].ToString();
				ddGrau.SelectedValue = grauFuncionario.ToString();
			}

			responsaveis = null; 
		}

		private void DGFuncionarios_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName.ToString() == "Select")
			{
				//Carregar o DGGrandezas do funcionário escolhido se for Responsável ou Substituto
				if(e.Item.Cells[1].Text == "0" || (e.Item.Cells[2].Text != "4" && e.Item.Cells[2].Text != "5"))
				{
					DGGrandezas.Visible = false;
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_PERFIL_NAO_NECESSITA_GRANDEZAS; 
				}
				else
				{
					DGGrandezas.Visible = true;
					string idFuncionario = DGFuncionarios.DataKeys[e.Item.ItemIndex].ToString();
					
					
					ViewState["idFuncionario"] = idFuncionario;
					BindGridGrandezas(idFuncionario);
				}
			}
		}

		protected void DGFuncionarios_Edit(Object sender, DataGridCommandEventArgs e)     
		{
			DGFuncionarios.EditItemIndex = e.Item.ItemIndex;	
			ViewState["idGrau"] = e.Item.Cells[1].Text;
			BindGridFuncionarios();
		}

		protected void DGFuncionarios_CancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DGGrandezas.Visible = false;
			DGFuncionarios.ShowFooter=true;  
			DGFuncionarios.EditItemIndex = -1;
			BindGridFuncionarios();
		}
		
		protected void DGFuncionarios_UpdateGrid(Object sender, DataGridCommandEventArgs e)
		{
			string idFuncionario = DGFuncionarios.DataKeys[e.Item.ItemIndex].ToString();
			DropDownList ddPerfil = (DropDownList)e.Item.FindControl("ddPerfilREdit");
			DropDownList ddGrau = (DropDownList)e.Item.FindControl("ddGrauEdit");

			//Verificação se já existe na tabela responsaveisGrandezas

			string grauFuncionario; 
				
			DATA.EstadoCertificadoBD fw = new LabMetro.DATA.EstadoCertificadoBD(); 
			bool bExiste = fw.bExisteFuncionarioWorkFlow(idFuncionario); 
			fw = null; 

			if(!bExiste) //false = não existe
			{                   
				//string grauFuncionario = grau() == "0" ? ddGrau.SelectedValue.ToString() : "1";
				string str; 
				str = ViewState["grau"].ToString()  == "0" ? grauFuncionario = ddGrau.SelectedValue.ToString() : grauFuncionario = "1";

				DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD(); 
				//INSERT
				lblMessage.Text = perfis.InsertPerfilWorkFlow(idFuncionario,"", grauFuncionario, ddPerfil.SelectedValue.ToString(), User.Identity.Name.ToString()); 
				perfis = null; 
				
			}
			else
			{                 
				string str; 
				str = ViewState["grau"].ToString() == "0" ? grauFuncionario = ddGrau.SelectedValue.ToString() : grauFuncionario = ViewState["idGrau"].ToString();

				DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD();
				//UPDATE 
				lblMessage.Text = perfis.UpdatePerfilWorkFlow(idFuncionario, grauFuncionario, ddPerfil.SelectedValue.ToString(), User.Identity.Name.ToString()); 
				perfis = null;

			}

			DGGrandezas.Visible = false;
			DGFuncionarios.EditItemIndex = -1;
			BindGridFuncionarios(); 
			DGFuncionarios.ShowFooter=true; 
		}

		//DGGrandezas

		private void BindGridGrandezas(string idFuncionario)
		{
			DATA.PerfisBD grandezas = new LabMetro.DATA.PerfisBD();
			DataTable DT =  grandezas.dtListaFuncionariosGrandezasWorkflow(idFuncionario);
			
			DataView DV = new DataView(DT);
			
			DV.Sort = ViewState["sortFieldG"].ToString()+ " " + ViewState["sortDirectionG"]; 

			DGGrandezas.DataSource = DV;
			DGGrandezas.DataBind(); 

			grandezas = null;
			
		}

		private void DGGrandezas_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			DATA.PerfisBD grandezas = new LabMetro.DATA.PerfisBD();
			if(e.Item.ItemType == ListItemType.EditItem)
			{				
				//ddGrandeza
				SqlDataReader DRG =  grandezas.ListaGrandezasWorkflow();
				DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaGEdit");
				ddGrandeza.DataSource = DRG;
				ddGrandeza.DataBind(); 
				ddGrandeza.Items.Insert(0,new ListItem("", "ALL"));
				DRG.Close();
				//como a datasource é um reader e nao um dataset, tem de ser feito assim....
				//System.Data.Common.DbDataRecord dbGrandeza = (System.Data.Common.DbDataRecord)e.Item.DataItem;
				DataRowView drv = (DataRowView)e.Item.DataItem;
				//string grandeza = dbGrandeza.GetValue(3).ToString(); 
				string grandeza = drv["idGrandeza"].ToString();
				ddGrandeza.SelectedValue = grandeza.ToString();				
			}
			if(e.Item.ItemType == ListItemType.Footer)
			{
				//ddFuncionario
				SqlDataReader DRF = grandezas.ListaFuncionarioWorkflow(ViewState["idFuncionario"].ToString());
				DropDownList ddFuncionario = (DropDownList)e.Item.FindControl("ddFuncionarioGFooter");
				ddFuncionario.DataSource = DRF;
				ddFuncionario.DataBind(); 
				DRF.Close();

				//ddGrandeza
				SqlDataReader DRG =  grandezas.ListaGrandezasWorkflow();
				DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaGFooter");
				ddGrandeza.DataSource = DRG;
				ddGrandeza.DataBind(); 
				DRG.Close();
			}

				grandezas = null;
		}

		private void DGGrandezas_ItemCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Insert")
			{
				if(e.Item.ItemType == ListItemType.Footer)
				{
					DropDownList ddFuncionario = (DropDownList)e.Item.FindControl("ddFuncionarioGFooter");
					DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaGFooter");
                    
					DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD(); 
				
					lblMessage.Text = perfis.InsertPerfilWorkFlow(ddFuncionario.SelectedValue.ToString(),ddGrandeza.SelectedValue.ToString(), "", "", User.Identity.Name.ToString()); 

					DGGrandezas.EditItemIndex = -1;
					BindGridGrandezas(ddFuncionario.SelectedValue.ToString()); 
					DGGrandezas.ShowFooter=true; 

					perfis = null; 
				}
			}
			if(e.CommandName=="Apagar")
			{
				string idResponsavel = DGGrandezas.DataKeys[e.Item.ItemIndex].ToString();

				DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD();

				lblMessage.Text = perfis.DeleteGrandezaWorkFlow(idResponsavel, User.Identity.Name.ToString()); 
				BindGridGrandezas(ViewState["idFuncionario"].ToString());

				perfis = null; 
			}
		}

		protected void DGGrandezas_Edit(Object sender, DataGridCommandEventArgs e)     
		{
			DGGrandezas.ShowFooter=false;     
			DGGrandezas.EditItemIndex = e.Item.ItemIndex;
			string idFuncionario = e.Item.Cells[0].Text;
			DGGrandezas.Columns[7].Visible = false;
			ViewState["idFuncionario"] = idFuncionario;
			BindGridGrandezas(idFuncionario);
		}

		protected void DGGrandezas_CancelGrid(Object sender, DataGridCommandEventArgs e)
		{
			DGGrandezas.ShowFooter=true;  
			DGGrandezas.EditItemIndex = -1;
			BindGridGrandezas(ViewState["idFuncionario"].ToString());
			DGGrandezas.Columns[7].Visible = true;
		}
		
		protected void DGGrandezas_UpdateGrid(Object sender, DataGridCommandEventArgs e)
		{
			string idResponsavel = DGGrandezas.DataKeys[e.Item.ItemIndex].ToString();
			            
			DropDownList ddGrandeza = (DropDownList)e.Item.FindControl("ddGrandezaGEdit");
                  
			DATA.PerfisBD perfis = new LabMetro.DATA.PerfisBD();

			lblMessage.Text = perfis.UpdateGrandezaWorkFlow(idResponsavel, ddGrandeza.SelectedValue.ToString(), User.Identity.Name.ToString()); 

			DGGrandezas.EditItemIndex = -1;		
			BindGridGrandezas(ViewState["idFuncionario"].ToString());
			DGGrandezas.Columns[7].Visible = true;
			DGGrandezas.ShowFooter=true; 

			perfis = null;
		}

		/// <summary>
		/// Função de ordenação da DataGrid conforme o parâmetro s
		/// e invertendo a direcção actual
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		public void SortGridG(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionG"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionG"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionG"]="ASC";
					break;
			}

			ViewState["sortFieldG"] = e.SortExpression;
			BindGridGrandezas(ViewState["idFuncionario"].ToString());
		}///Fim SortGridR

		/// <summary>
		/// Função de ordenação da DataGrid conforme o parâmetro s
		/// e invertendo a direcção actual
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		public void SortGridR(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirectionR"].ToString())
			{
				case "ASC":
					ViewState["sortDirectionR"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirectionR"]="ASC";
					break;
			}

			ViewState["sortFieldR"] = e.SortExpression;
			BindGridFuncionarios();
		}///Fim SortGridR
         
		

        protected string ConverteEstado(bool b)
        {
            if (b==true) 
            {
                return "activo";
            }
            return "inactivo";  
        }

//        protected string ConverteNome(string str)
//        {
//            if(str.IndexOf("Inserir") >=0)
//            {
//				return str.Replace("Inserir", "Form"); 
//            }
//			return str; 
//        }

		/// <summary>
		/// 
		/// Retorna o grau de hierarquia do utilizador
		/// DM - pelo que estou a ver, o único grau existente é o grau e gestor de certificados, cujo valor é 0 (zero)
		/// para conter essa informação, não teria sido necessário criar uma tabela
		/// como não existe nenhum backoffice para tal, suponho q nao está prevista a inserção de mais graus... 
		/// 
		/// </summary>
		/// <returns></returns>
//		public string existeFuncionario(string idFuncionario)
//		{
//			DATA.EstadoCertificadoBD fw = new LabMetro.DATA.EstadoCertificadoBD(); 
//			SqlDataReader DR = fw.DRExisteFuncionarioWorkflow(idFuncionario);
//
//			string utilizadorWorkflow = "";
//
//			while (DR.Read())
//			{
//				utilizadorWorkflow += DR["existe"].ToString();
//			}
//
//			DR.Close();
//
//			fw = null; 
//
//			return utilizadorWorkflow;
//		}///Fim grau

		/// <summary>
		/// Retorna o grau de hierarquia do utilizador
		/// DM - pelo que estou a ver, o único grau existente é o grau e gestor de certificados, cujo valor é 0 (zero)
		/// para conter essa informação, não teria sido necessário criar uma tabela
		/// como não existe nenhum backoffice para tal, suponho q nao está prevista a inserção de mais graus... 
		/// 
		/// </summary>
		/// <returns></returns>
		public void fillGrau()
		{
			DATA.EstadoCertificadoBD gr = new LabMetro.DATA.EstadoCertificadoBD(); 
			string grau = gr.strGrauUtilizador(); 
			gr = null; 	
			
			ViewState["grau"] = grau; 

		}///Fim grau

		/// <summary>
		/// Retorna o perfil (perfil normal) do utilizador
		/// </summary>
		/// <returns></returns>
		public string strPerfil()
		{
//			DATA.EstadoCertificadoBD pr = new LabMetro.DATA.EstadoCertificadoBD(); 
//			SqlDataReader DR = pr.DRListGrau(User.Identity.Name.ToString());
//
//			string perfilUtilizador = "";
//
//			while (DR.Read())
//			{
//				perfilUtilizador += DR["idPerfil"].ToString();
//			}
//
//			DR.Close();
//
//			pr  = null; 
//
//			return perfilUtilizador;
			
			DATA.EstadoCertificadoBD gr = new LabMetro.DATA.EstadoCertificadoBD(); 
			string perfil = Session["idPerfil"].ToString(); 
			gr = null; 
				
			return perfil; 


		}///Fim perfil

//		public string downloadpathHELP(object filename)
//		{
//			if(filename!=null && filename.ToString()!="")
//			{
//				string myPath = (string)ConfigurationManager.AppSettings["HELP"];
//				myPath = myPath + "/" + filename.ToString();
//				return myPath;
//			}
//			else
//			{
//				return "#"; 
//			}
//		}///Fim downloadpathHELP

//		private void btnHelp_Click(object sender, System.EventArgs e)
//		{
//			string doc = "HTMLCompleto/LabMetro.html";
//			string nome = downloadpathHELP(doc);
//			Response.Write("<script language=javascript>window.open('"+nome+"','new_Win','height=400,width=950,toolbar=0,menubar=0,resizable=1');</script>");
//		}

		protected void DGFuncionarios_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//isto nao vale a pena pq o desseleccionar dos nao seleccionados é demasiado chato/complicado
//			DGFuncionarios.BackColor=System.Drawing.Color.Gainsboro; 
//			DGFuncionarios.AlternatingItemStyle.BackColor=System.Drawing.Color.LightGray; 
//			
//
//			//e.Item.Attributes.Add("onmouseover", "style.backgroundColor='Silver'");
//					
//			DGFuncionarios.Items[DGFuncionarios.SelectedIndex].Cells[0].BackColor=System.Drawing.Color.Red; 
		}
	}
}
