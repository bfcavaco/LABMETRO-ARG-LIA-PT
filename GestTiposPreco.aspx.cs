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


namespace LabMetro
{
	/// <summary>
	/// Summary description for GestTiposPreco.
	/// </summary>
	public partial class GestTiposPreco : System.Web.UI.Page
	{

         private const string ID_PAG = "TIPOSPRECO_1";//NOME DA PAGINA
    
        protected void Page_Load(object sender, System.EventArgs e)
        {
            lblMessage.Text =""; 
            
            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
            if(ht == null) //sessao expirou:
            {
                Server.Transfer("Default.aspx?err=2");          
            }
            else
            {
                if(!ht.ContainsKey(ID_PAG))
                {
                    Server.Transfer("Default.aspx?err=1",true);//user n tem permissoes    
                }
                else
                {
                    if(!Page.IsPostBack)
                    {
				       ViewState["sortField"] = "TipoEquipamento";
                        ViewState["sortDirection"] = "ASC";

                        FillGrandezas(); 
                        FillFamilias(); 
                      
                        FillTipoPreco(); 
                        FillTipoServico(); 
                        //tem de ser preenchido em ultimo pois depende dos outros    
                        FillEquipamento(); 
                        BindGrid(txtGrandeza.Text, txtFamilia.Text, txtTipoEquipamento.Text); 
                    }
                }
            }
        }

        private void FillGrandezas()
        {
            
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaGrandezas(); 
            ddGrandeza.DataSource = DR;
            ddGrandeza.DataBind();  
      
            DR.Close(); 
			lista = null; 
        }

        private void FillFamilias()
        {
            DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();
            SqlDataReader DR = precos.DRFamilias(ddGrandeza.SelectedValue); 
            ddFamilia.DataSource = DR;
            ddFamilia.DataBind();  
            
            DR.Close(); 
			precos = null; 
        }

        private void FillTipoServico()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD(); 
            SqlDataReader DR = lista.DRListaTipoServico(); 
            ddTipoServico.DataSource = DR;
            ddTipoServico.DataBind();  
      
            DR.Close(); 
			lista = null; 
        }


        private void FillTipoPreco()
        {
            DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();
            SqlDataReader DR = precos.DRTipoPreço(); 
            ddTipoPreco.DataSource= DR; 
            ddTipoPreco.DataBind(); 
            
            DR.Close(); 

			if(ddTipoServico.SelectedValue!="4")
			{
				cbFormula.Enabled=false; 
			}

			precos = null; 

        }


        //depende da dropdown FAMILIA, mas tb das dropdowns TIPO DE PREÇO e TIPO DE SERVIÇO
        private void FillEquipamento()
        {
            DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();
            SqlDataReader DR = precos.DRTipoEquipamentoSemTipoPreço(ddFamilia.SelectedValue,ddTipoServico.SelectedValue); 
       
            lbTipoEquipamento.DataSource = DR; 
            lbTipoEquipamento.DataBind(); 
            DR.Close(); 

			precos = null; 
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {    

        }

        private void InitializeComponent2()
        {    
           
            btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            ddTipoPreco.SelectedIndexChanged += new EventHandler(ddTipoPreco_SelectedIndexChanged); 
            ddGrandeza.SelectedIndexChanged += new System.EventHandler(ddGrandeza_SelectedIndexChanged);
            ddFamilia.SelectedIndexChanged += new System.EventHandler(ddFamilia_SelectedIndexChanged);
            dgPrecario.SelectedIndexChanged += new System.EventHandler(dgPrecario_SelectedIndexChanged);
            btnSearch.Click += new System.EventHandler(btnSearch_Click);
            btnLimparCampos.Click += new System.EventHandler(btnLimparCampos_Click);
            dgPrecario.DeleteCommand += new  DataGridCommandEventHandler(dgPrecario_DeleteCommand);
            ddTipoServico.SelectedIndexChanged += new System.EventHandler(ddTipoServico_SelectedIndexChanged);
		
        }
        #endregion

        private void ddGrandeza_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillFamilias(); 
            FillEquipamento(); 
        }

		
        private void ddFamilia_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillEquipamento(); 
        }

        private void ddTipoPreco_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(ddTipoPreco.SelectedValue=="4")
                cbFormula.Enabled=true; 
            else
                cbFormula.Enabled=false; 
			  
        }


        private void BindGrid(string txtGrandeza, string txtFamilia, string txtTipoEquipamento)
        {
            //fazer selects diferentes conforme o tipo de preço que é.

            //dgPrecario.CurrentPageIndex=0;
            DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD(); 
            DataTable DT = precos.DTTiposPrecoPorEquipamento(); 

            DataView DV = new DataView(DT);
            DV.RowFilter = "Grandeza LIKE '"+txtGrandeza+"%' AND Familia LIKE '"+txtFamilia+"%' AND TipoEquipamento LIKE '"+txtTipoEquipamento+"%'"; 
            if(Page.IsPostBack)
            {
                string strSort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
                DV.Sort = strSort; 
            }
            
            dgPrecario.DataSource =DV; 
            dgPrecario.DataBind(); 

			precos = null; 

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
            BindGrid(txtGrandeza.Text,txtFamilia.Text,txtTipoEquipamento.Text); 

        }

        public void DoPaging(Object s,DataGridPageChangedEventArgs e)
        {   
            dgPrecario.CurrentPageIndex = e.NewPageIndex;
            BindGrid(txtGrandeza.Text,txtFamilia.Text,txtTipoEquipamento.Text); 
        }

        private void InsertBD()
        {
            //ver onde vai parar o tipo de serviço, vejo dps.
            DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 
            
            int ok = 1; //pode inserir, se está a 0 é pq algum campo nao está seleccionado
            if(lbTipoEquipamento.SelectedIndex == -1)
            {
                lblMessage.Text ="Tem de escolher um tipo de equipamento."; 
                ok = 0; 
            }
            if(ddTipoPreco.SelectedIndex == -1)
            {
                lblMessage.Text ="Tem de escolher um tipo de preço."; 
                ok = 0; 
            }
            if(ddTipoServico.SelectedIndex == -1)
            {
                lblMessage.Text ="Tem de escolher um tipo de serviço."; 
                ok = 0; 
            }
            if(ok == 1)
            {
                int i = preco.InsertRelTipoEquipPreco(lbTipoEquipamento.SelectedValue.ToString(),ddTipoPreco.SelectedValue.ToString(),ddTipoServico.SelectedValue.ToString(),cbFormula.Checked.ToString()); 

                switch(i)
                {
                    
                    case 0: //nenhum row affected: correu mal
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 
                        break; 
                    case 1: //one row affected, correu bem

                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB; 
                        //PREENCHER DE NOVO O TIPO DE EQUIPAMENTO PARA DEIXAR DE APARECER
                        //O QUE ACABOU DE SER INSERIDO
                        FillEquipamento(); 
                        break; 
                    case 2627: 
                        lblMessage.Text = "O tipo de equipamento já tem um tipo de preço associado.<br />Para alterar, apague o registo da tabela em baixo e volte a inserir."; 
                        break; 
                    default:
                        lblMessage.Text =""; //nada ...
                        break; 
                }
            }
			preco = null; 
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            InsertBD(); 
            BindGrid(txtGrandeza.Text, txtFamilia.Text, txtTipoEquipamento.Text); 
        }

        private void dgPrecario_SelectedIndexChanged(object sender, System.EventArgs e)
        {
		
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            dgPrecario.CurrentPageIndex = 0; 

            BindGrid(txtGrandeza.Text, txtFamilia.Text, txtTipoEquipamento.Text); 
        }

        private void btnLimparCampos_Click(object sender, System.EventArgs e)
        {
            LimpaCamposPesquisa(); 
        }

        private void LimpaCamposPesquisa()
        {
            txtFamilia.Text=""; 
            txtGrandeza.Text="";
            txtTipoEquipamento.Text=""; 
        }
        
        private void dgPrecario_DeleteCommand(object source,System.Web.UI.WebControls.DataGridCommandEventArgs e)		
        {
            DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 

            string id = dgPrecario.DataKeys[e.Item.ItemIndex].ToString();
            int i = preco.DeleteRelation(id);  
            preco.DeleteRelation(id);  
            if (i > 0) lblMessage.Text = "Apagado com sucesso."; 
            FillEquipamento(); //o equipamento apagado já deve ter passado para cima
            BindGrid(txtGrandeza.Text, txtFamilia.Text, txtTipoEquipamento.Text); 
             
			preco = null;    
        }

        
        private void ddTipoServico_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillEquipamento(); 
            //mostrar os equipamentos que ainda nao estao associados a este tipo de serviço.
        }

    }
}
