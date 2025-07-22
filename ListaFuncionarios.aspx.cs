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

namespace LabMetro
{
	/// <summary>
	/// Summary description for ListaFuncionarios.
	/// </summary>
	public partial class ListaFuncionarios : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList ddCargo;
        private const string ID_PAG = "FUNCIONARIOS_0";//NOME DA PAGINA

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            Page.Form.DefaultButton = btnSearch.UniqueID;

            
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
                        ViewState["sortField"] = "nome";
                        ViewState["sortDirection"] = "ASC";
						fillDropDowns(); 
                        BindGrid(); 
                    }

                    if(!ht.ContainsKey("FUNCIONARIOS_1")) //se n tem permissoes para ver os detalhes dos funcionarios, desactivar o link
                    {
                        DGFuncionarios.Columns[5].Visible=false; 
                    }
                }
            }
        }
        

		private void fillDropDowns()
		{
			DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
          
		
			SqlDataReader DR4 = lista.DRListaFuncoes();
			ddFuncao.DataSource = DR4; 
			ddFuncao.DataBind(); 
			ddFuncao.Items.Insert(0,new ListItem("",""));
			DR4.Close(); 
        
			SqlDataReader DR5 = lista.DRListaLocalCalibracao(); 
			ddLocalCalibracao.DataSource = DR5; 
			ddLocalCalibracao.DataBind(); 
			ddLocalCalibracao.Items.Insert(0,new ListItem("",""));
			DR5.Close(); 
            
			SqlDataReader DR6 =  lista.DRListaLaboratorios(); 
			ddLaboratorio.DataSource = DR6;  
			ddLaboratorio.DataBind();
			ddLaboratorio.Items.Insert(0,new ListItem("",""));

			DR6.Close(); 

			lista = null; 

			DATA.PerfisBD perfil = new LabMetro.DATA.PerfisBD(); 
			SqlDataReader dr =  perfil.ListaPerfis(); 
			ddPerfil.DataSource = dr;
			ddPerfil.DataBind(); 
			ddPerfil.Items.Insert(0,new ListItem("",""));
			perfil = null; 
		}


        private void BindGrid()
        {
            DATA.FuncionariosBD funcionario = new LabMetro.DATA.FuncionariosBD(); 

            DataTable DT = funcionario.DTFillFuncionarios(); 
			DataView DV = new DataView(DT);

			string strRowfilter ="1=1"; // "idFuncionario <> 0 "; 

			if(txtNome.Text.Length >0)
			{
				strRowfilter +=" AND nome LIKE '"+txtNome.Text+"%' ";  
			}
			if(ddFuncao.SelectedIndex > 0)
			{
				strRowfilter +="AND idFuncao = "+ddFuncao.SelectedValue+" ";  
			}

			if(ddLaboratorio.SelectedIndex > 0)
			{
				strRowfilter +=" AND idLaboratorio = "+ddLaboratorio.SelectedValue+" ";  
			}

			if(ddPerfil.SelectedIndex > 0)
			{
				strRowfilter +=" AND idPerfil = "+ddPerfil.SelectedValue+" ";  
			}

			if(ddLocalCalibracao.SelectedIndex > 0)
			{
				strRowfilter +=" AND idLocalCalibracao = "+ddLocalCalibracao.SelectedValue+" ";  
			}

			if(ddEstado.SelectedValue != "")
			{
				strRowfilter +=" AND  estado= "+ddEstado.SelectedValue+" ";  
			}

			DV.RowFilter =  strRowfilter;
			DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
            
            DGFuncionarios.DataSource = DV; 
            DGFuncionarios.DataBind(); 
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

        protected void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {
            DGFuncionarios.CurrentPageIndex = e.NewPageIndex;
            BindGrid(); 
        }


        protected void SortGrid(Object s, DataGridSortCommandEventArgs e)
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

		protected string ConverteEstado(bool b)
		{
			if (b==true) 
			{
				return "activo";
			}
			else
			{
				return "inactivo"; 
			}
		}

        protected void DGFuncionarios_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        
        }

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			DGFuncionarios.CurrentPageIndex=0; 
			BindGrid(); 
			
		}
    }
}
