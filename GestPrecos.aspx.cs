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
using LabMetro.GERAL;
using LabMetro.REPORTS;
using System.Configuration;
using LabMetro.DataAccessLayer; 

namespace LabMetro
{
    /// <summary>
    /// Summary description for GestPrecos.
    /// </summary>
    public partial class GestPrecos : System.Web.UI.Page
    {

        protected System.Web.UI.WebControls.DataGrid dgPrecario;
        protected System.Web.UI.WebControls.TextBox TextBox5;
        protected System.Web.UI.WebControls.TextBox TextBox6;
        protected System.Web.UI.WebControls.TextBox TextBox7;

        protected System.Web.UI.WebControls.TextBox txtT1;
        protected System.Web.UI.WebControls.TextBox txtT2;
        protected System.Web.UI.WebControls.TextBox txtT3;
        protected System.Web.UI.WebControls.TextBox txtT4;
        protected System.Web.UI.WebControls.TextBox txtH1;
        protected System.Web.UI.WebControls.TextBox txtH2;
        protected System.Web.UI.WebControls.TextBox txtH3;
        protected System.Web.UI.WebControls.TextBox txtH4;
        //protected System.Web.UI.WebControls.DataGrid dgPrecosAlcancesSimples;


        private const string ID_PAG = "PRECOS_1";//NOME DA PAGINA

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


            //ddTipoPreco.SelectedIndexChanged += new EventHandler(ddTipoPreco_SelectedIndexChanged); 
            ddGrandeza.SelectedIndexChanged += new System.EventHandler(ddGrandeza_SelectedIndexChanged);
            ddFamilia.SelectedIndexChanged += new System.EventHandler(ddFamilia_SelectedIndexChanged);
            ddTipoEquipamento.SelectedIndexChanged += new System.EventHandler(ddTipoEquipamento_SelectedIndexChanged);
            ddTipoServico.SelectedIndexChanged += new System.EventHandler(ddTipoServico_SelectedIndexChanged);
            //dgAlcancesSimples.SelectedIndexChanged += new System.EventHandler(dgAlcancesSimples_SelectedIndexChanged);

            //dgAlcancesSimples.SelectedIndexChanged += new System.EventHandler(dgAlcancesSimples_SelectedIndexChanged);
            //dgAlcancesMistos.SelectedIndexChanged += new System.EventHandler(dgAlcancesMistos_SelectedIndexChanged);
            //dgPrecoMarcaModelo.SelectedIndexChanged += new System.EventHandler(dgPrecoMarcaModelo_SelectedIndexChanged);
            //dgPrecosDirectos.SelectedIndexChanged += new System.EventHandler(DataGrid1_SelectedIndexChanged);

            //quando é protected nao precisa de eventhandlers
            //            dgAlcancesMistos.ItemCommand += new DataGridCommandEventHandler(dgAlcancesMistos_ItemCommand);
            //            dgPrecosAlcancesMistos.ItemCommand += new DataGridCommandEventHandler(dgPrecosAlcancesMistos_ItemCommand);

            //  dgAlcancesMistos.SelectedIndexChanged += new System.EventHandler(dgAlcancesMistos_SelectedIndexChanged); 

        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            lblMessage.Text = "";

            Hashtable ht = (Hashtable)Session["HTPermissions"];
            if (ht == null) //session expired
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
                        //  hideControls(); 
                        FillGrandezas();
                        FillFamilias();
                        //FillTipoServico();
                        FillEquipamento(); //tem de vir depois da familia e do tipo de servico
                        BindGridPrecosDirectos();
                        BindGridPrecosExcel();
                        

                    }
                }
            }
        }


        //================================================================================
        //preenchimento das dropdowns
        //GRANDEZAS,FAMILIAS,TIPOS DE EQUIP.,TIPOS DE SERVIÇO,TIPOS DE PREÇO 
        //================================================================================
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

        private void FillEquipamento()
        {
            DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();

            if ((ddFamilia.SelectedIndex == -1) ||  (ddFamilia.Items.Count == 0))
            {
                lblMessage.Text = "Tem de seleccionar uma familia.";
                
            }
            else
            {
                SqlDataReader DR = precos.DRTipoEquipamentoComTipoPreco(ddFamilia.SelectedValue, ddTipoServico.SelectedValue);
                //agora ja nao é com tipo de preço apenas

                ddTipoEquipamento.DataSource = DR;
                ddTipoEquipamento.DataBind();
                DR.Close();
            }

            precos = null;
        }

       

        //private void FillTipoPreco()
        //{
        //    if(lbTipoEquipamento.SelectedIndex ==-1) 
        //    {
        //        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_SELECCIONE_TIPOEQUIPAMENTO; 
        //    }

        //    else if(ddTipoServico.SelectedValue=="") 
        //    {
        //        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_INDICAR_TIPO_SERVICO;
        //    } 
        //    else
        //    {
        //        DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();
        //        SqlDataReader DR = precos.DRTipoPrecoByIdEquipamento(lbTipoEquipamento.SelectedValue.ToString(),ddTipoServico.SelectedValue.ToString()); 
        //        ddTipoPreco.DataSource= DR; 
        //        ddTipoPreco.DataBind(); 

        //        DR.Close(); 

        //        precos = null; 

        //        controlAction(ddTipoPreco.SelectedValue.ToString()); 

        //    }
        //}


        //================================================================================
        //fim preenchimento das dropdowns
        //================================================================================

        private void ddGrandeza_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillFamilias();
            FillEquipamento();
            // DisposeDataGrids(Page,1); 

            BindGridPrecosDirectos();
        }


        private void ddFamilia_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillEquipamento();
            //DisposeDataGrids(Page,1); 
            BindGridPrecosDirectos();
        }


        private void ddTipoEquipamento_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //DisposeDataGrids(Page,1); 
            //FillTipoServico();
            //if(dgPrecosAlcancesMistos.HasControls()) dgPrecosAlcancesMistos.Controls.Clear(); 
            BindGridPrecosDirectos();
        }

        private void ddTipoServico_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //DisposeDataGrids(Page,1);
            FillEquipamento();
            BindGridPrecosDirectos();
        }


      
        #region **** PREÇOS DIRECTOS*********************************************************

        //************************************************************************************
        //PREÇOS DIRECTOS
        //************************************************************************************

        //====================================================================================
        //BIND GRID PREÇOS DIRECTOS
        //====================================================================================
        private void BindGridPrecosDirectos()
        {
            DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();
            DataTable DT = precos.DTPrecosDirectos(ddTipoEquipamento.SelectedValue.ToString(), ddTipoServico.SelectedValue.ToString());

            DataView DV = new DataView(DT);

            dgPrecosDirectos.DataSource = DV;
            dgPrecosDirectos.DataBind();

            precos = null;

        }


        private void BindGridPrecosExcel()
        {

           string strSQL = "SELECT     Familia.idGrandeza as Grandeza, Familia.descricao AS Familia,  TipoEquipamento.descricao as tipoEquipamento, tbPrecosDirectos.idTipoServico as tipoServico, tbPrecosDirectos.preco, tbPrecosDirectos.precoExterior, tbPrecosDirectos.precoMovel FROM  TipoEquipamento INNER JOIN Familia ON TipoEquipamento.idFamilia = Familia.idFamilia LEFT OUTER JOIN tbprecosDirectos ON TipoEquipamento.idTipoEquipamento = tbPrecosDirectos.idTipoEquipamento ORDER BY 1,2,3";
            gvPrecos.DataSource = GERAL.clsDataAccess.ExecuteDT(strSQL);
            gvPrecos.DataBind(); 

        }


        protected void BtnExportGrid_Click(object sender, EventArgs args)
        {

            //  pass the grid that for exporting ...
            GridViewExportUtil.Export("Precos.xls", gvPrecos);

        }
        //====================================================================================
        //ITEMDATABOUND DOS PREÇOS DIRECTOS
        //SERVE PARA PREENCHER AS DROPDOWNS DDUNIDADE NO EDITITEM E NO FOOTER
        //====================================================================================
        protected void dgPrecosDirectos_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

            }
        }
        //====================================================================================
        //ITEM COMMAND DOS PREÇOS DIRECTOS
        //--edita preços para cada alcance seleccionado
        //--insere alcances novos atraves do footer
        //====================================================================================
        protected void dgPrecosDirectos_ItemCommand(object sender, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "insertPreco")
            {

                if (e.Item.ItemType == ListItemType.Footer)
                {
                   
                    TextBox txtPreco = (TextBox)e.Item.FindControl("txtPrecoFooter");
                    TextBox txtPrecoExterior = (TextBox)e.Item.FindControl("txtPrecoExteriorFooter");
                    TextBox txtPrecoMovel = (TextBox)e.Item.FindControl("txtPrecoMovelFooter");
                    if (ddTipoEquipamento.SelectedIndex == -1)
                    {
                        lblMessage.Text = "Tem de seleccionar um tipo de equipamento primeiro.";
                    }
                    else
                    {
                        DATA.PrecosBD precos = new LabMetro.DATA.PrecosBD();
                        //fazer isto
                        //unidade devia ser uma dropDown
                        int x = precos.InsertPrecoDirecto(ddTipoEquipamento.SelectedValue, ddTipoServico.SelectedValue,txtPreco.Text, txtPrecoExterior.Text, txtPrecoMovel.Text, Session["UserId"].ToString());

                        if (x == 1)
                        {
                            lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB;
                            BindGridPrecosDirectos();
                        }
                        else
                        {
                            lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT;
                        }

                        precos = null;
                    }
                }

                //                else if(e.CommandName=="Delete")
                //                {
                //                    string id = dgPrecosDirectos.DataKeys[e.Item.ItemIndex].ToString(); 
                //                    DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 
                //                    int i = preco.DeleteFromPrecosDirectos(id); 
                //                    
                //                    if(i==1) 
                //                    {
                //                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DELETE_DB; 
                //                        BindGridPrecosDirectos();
                //                    }
                //
                //                    else
                //                    {
                //                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DELETE; 
                //                    }
                //                }
            }
        }


        //====================================================================================
        //APAGA PREÇOS ALCANCES SIMPLES
        //====================================================================================
        protected void dgPrecosDirectos_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = dgPrecosDirectos.DataKeys[e.Item.ItemIndex].ToString();
            DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD();
            int i = preco.DeleteFromPrecosDirectos(id);

            if (i == 1)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DELETE_DB;
                BindGridPrecosDirectos();
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_DELETE;
            }

        }
        //====================================================================================
        //EDITA PREÇOS DIRECTOS
        //====================================================================================
        protected void dgPrecosDirectos_Edit(Object sender, DataGridCommandEventArgs e)
        {
            dgPrecosDirectos.ShowFooter = false;
            dgPrecosDirectos.EditItemIndex = e.Item.ItemIndex;
            BindGridPrecosDirectos();
        }

        //====================================================================================
        //CANCELA PREÇOS DIRECTOS
        //====================================================================================
        protected void dgPrecosDirectos_CancelGrid(Object sender, DataGridCommandEventArgs e)
        {
            dgPrecosDirectos.ShowFooter = true;
            dgPrecosDirectos.EditItemIndex = -1;
            BindGridPrecosDirectos();
        }

        //====================================================================================
        //ALTERA PREÇOS DIRECTOS
        //====================================================================================
        protected void dgPrecosDirectos_UpdateGrid(Object sender, DataGridCommandEventArgs e)
        {
            string id = dgPrecosDirectos.DataKeys[e.Item.ItemIndex].ToString();

       
            TextBox txtPreco = (TextBox)e.Item.FindControl("txtPrecoEdit");
            TextBox txtPrecoExterior = (TextBox)e.Item.FindControl("txtPrecoExteriorEdit");
            TextBox txtPrecoMovel = (TextBox)e.Item.FindControl("txtPrecoMovelEdit");

            DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD();


            int i = preco.UpdatePrecosDirectos(id, txtPreco.Text, txtPrecoExterior.Text, txtPrecoMovel.Text, Session["UserId"].ToString());

            if (i == 1)
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_UPDATE_DB;
            }
            else
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_UPDATE;
            }
            dgPrecosDirectos.EditItemIndex = -1;
            BindGridPrecosDirectos();
            dgPrecosDirectos.ShowFooter = true;

            preco = null;
        }

        #endregion

    }}
