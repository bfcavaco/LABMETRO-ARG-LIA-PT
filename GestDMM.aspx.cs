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
    public partial class GestDMM : System.Web.UI.Page
    {
        private const string ID_PAG = "GESTDMM_0";//NOME DA PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            //DataAccessLayer.dlEquipamentoTableAdapters.TipoEquipamentoTableAdapter ta = new LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.TipoEquipamentoTableAdapter();
            //dlEquipamento.TipoEquipamentoDataTable dtTiposEquipamento =  new dlEquipamento.TipoEquipamentoDataTable();
            //dtTiposEquipamento = ta.GetAllTiposEquipamento();
            
            //foreach (dlEquipamento.TipoEquipamentoRow dr in dtTiposEquipamento)
            //{
            //    //Response.Write(dr.descricao.ToString()); 
            //}

            //DataAccessLayer.dlEquipamentoTableAdapters.EquipamentosCompletosTableAdapter ta_equips = new LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.EquipamentosCompletosTableAdapter(); 

            //Response.Write("Hello"); 

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
            }
            
        }

        protected void txtEmpresaTextChanged(object sender, System.EventArgs e)
		{
			fillDDEmpresa();
            gvEquipamentos.DataBind();
		}

        protected void btnEmpresas_Click(object sender, EventArgs e)
        {
            fillDDEmpresa();
            gvEquipamentos.DataBind();
        }

        private void fillDDEmpresa()
        {
            if (txtPesquisaEmpresa.Text.Length < 1)
            {
                lblMessage.Text = "Indique no mínimo a primeira letra da empresa.";
                return;
            }

            DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD();
            DataTable DT = empresa.DTEmpresas(txtPesquisaEmpresa.Text, "", "1", "", "", "", "", "", ""); //activas
            DataView DV = new DataView(DT);
            ddEmpresa.DataSource = DV; ;
            ddEmpresa.DataBind();
            empresa = null;

            try //... marteladas posteriores
            {
                if (ViewState["idEmpresa"] != null)
                {
                    if (ViewState["idEmpresa"].ToString() != "")
                    {
                        ddEmpresa.SelectedValue = ViewState["idEmpresa"].ToString();
                    }
                }
            }
            catch
            {            
            }

        }
        //==========================================================================================
        //==========================================================================================
        //FUNÇÕES SOBRE A FORMVIEW
        //==========================================================================================
          protected void ddMarcaEdit_SelectedIndexChanged(object sender, EventArgs e)
        {

            //http://www.mikepope.com/blog/DisplayBlog.aspx?permalink=1629
              //ler isto tb: http://msdn.microsoft.com/en-us/library/bb386452.aspx
              //isto é mais ou menos o mesmo que o meu codigo abaixo
            DropDownList ddmarca = (DropDownList)fvEquipqmantos.FindControl("ddMarcaEdit");
            DropDownList ddmod = (DropDownList)fvEquipqmantos.FindControl("ddModeloEdit");

            ObjectDataSource dsModelo = (ObjectDataSource)fvEquipqmantos.FindControl("OBJDS_ModeloEdit");
              dsModelo.SelectParameters["idMarca"].DefaultValue = ddmarca.SelectedValue;

              //ddmod.DataTextField = "descricao";
              //ddmod.DataValueField = "idModelo";
            
              ddmod.DataBind();
              //Response.Write("idMarca" + ddmarca.SelectedItem.Text + " idModelo " + ddmod.SelectedItem.Text);
     
            
        }
          //==========================================================================================
          //FORMVIEW DATABOUND - Occurs after the server control binds to a data source. (Inherited from BaseDataBoundControl.)
          //==========================================================================================
        protected void fv_DataBound(object sender, EventArgs e)
        {
            // DataTextField="descricao" DataValueField="idModelo"  SelectedValue='<%# Bind("idModelo") %>' 
            if (fvEquipqmantos.CurrentMode == FormViewMode.Edit) 
            {
                try
                {
                    DataRowView drv = (DataRowView)fvEquipqmantos.DataItem;
                    DropDownList ddmar = (DropDownList)fvEquipqmantos.FindControl("ddMarcaEdit");

                    //string idMarca = drv["idMarca"].ToString();
                    string idMarca = ddmar.SelectedValue; 
                    string idModelo = drv["idModelo"].ToString();

                    ObjectDataSource dsModelo = (ObjectDataSource)fvEquipqmantos.FindControl("OBJDS_ModeloEdit");
                    dsModelo.SelectParameters["idMarca"].DefaultValue = idMarca;

                    DropDownList ddmod = (DropDownList)fvEquipqmantos.FindControl("ddModeloEdit");
                    //ddmod.DataTextField = "descricao";
                    //ddmod.DataValueField = "idModelo";
     
                    ddmod.DataBind();

                    ddmod.SelectedValue = idModelo;
                }
                catch 
                { 
                
                }
            }                     

        }
        //ocurrs when the value of the pageindexproperty changes before a paging operation. 
        //==========================================================================================
        protected void fv_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {

        }
        //==========================================================================================
        //FORMVIEW INSERTING EVENT - occurs when an insert button in fv is clicked, but before the 
        //                          insert operation
        //==========================================================================================
        protected void fv_Inserting(object sender, FormViewInsertEventArgs e)
        {
            //nao vou usar isto pq ainda nao encontrei maneira de fazer o decode
            //// Iterate though the values entered by the user and HTML encode 
            //// the values. This helps prevent malicious values from being 
            //// stored in the data source.
            //for (int i = 0; i < e.Values.Count; i++)
            //{
            //    if (e.Values[i] != null)
            //    {
            //        e.Values[i] = Server.HtmlEncode(e.Values[i].ToString());
            //    }
            //}
        }

        //==========================================================================================
        //FORMVIEW UPDATING EVENT - occurs when an update button in fv is clicked, but before the 
        //                          update operation
        //==========================================================================================
        protected void fv_Updating(object sender, FormViewUpdateEventArgs e)
        {
            //// Iterate though the values entered by the user and HTML encode 
            //// the values. This helps prevent malicious values from being 
            //// stored in the data source.
            //for (int i = 0; i < e.NewValues.Count; i++)
            //{
            //    if (e.NewValues[i] != null)
            //    {
            //        e.NewValues[i] = Server.HtmlEncode(e.NewValues[i].ToString());
            //    }
            //}
        }


        //==========================================================================================
        //FORMVIEW INSERTED EVENT - occurs when an insert button in fv is clicked, but after the 
        //                          insert operation
        //==========================================================================================
        protected void fv_Inserted(object sender, FormViewInsertedEventArgs e)
        {
            ////nao consigo apanhar isto aqui!//com as funcoes anteriores feitas, ja consigo.
            //if (e.AffectedRows > -1)
            //{
            //    rebindGridViewAfterAction();
            //}
            //else
            //{
            //    lblMessage.Text = "Erro na inserção do registo.";
            //}
            // Use the Exception property to determine whether an exception
            // occurred during the insert operation.


            //==================================================================================================
            //If the business object throws an exception, the ExceptionHandled property is set to false and the exception is 
            //wrapped by the Exception property. If you use an ObjectDataSourceStatusEventHandler object, 
            //you can check the Exception property and handle the exception. If you handle the exception, 
            //set the ExceptionHandled property to true or the ObjectDataSource control will throw an exception. 
            //==================================================================================================
            if (e.Exception == null)
            {
                // Use the AffectedRows property to determine whether the
                // record was inserted. Sometimes an error might occur that 
                // does not raise an exception, but prevents the insert
                // operation from completing.
                if (e.AffectedRows > 0) //normalmente seria == 1 mas no nosso caso com os triggers tem de ser maior a 0 
                    //pq sao afectadas sempre mais de 1 linha
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_INSERT_DB; 
                    rebindGridViewAfterAction();
                }
                else
                {
                    lblMessage.Text = GERAL.clsGeral.ErrorMessage.ERR_INSERT; 

                    // Use the KeepInInsertMode property to remain in insert mode
                    // when an error occurs during the insert operation.
                    e.KeepInInsertMode = true;
                }
            }
            else
            {
                // Insert the code to handle the exception.
                lblMessage.Text = e.Exception.Message + "...";

                // Use the ExceptionHandled property to indicate that the 
                // exception has already been handled.
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        //==========================================================================================
        //FORMVIEW UPDATED EVENT  - occurs when an update button in fv is clicked, but after the 
        //                          update operation
        //==========================================================================================
        protected void fv_Updated(object sender, FormViewUpdatedEventArgs e)
        {
            //nao consigo apanhar isto aqui!
            if (e.AffectedRows > -1)
            {
                rebindGridViewAfterAction();
            }
            else
            {
                lblMessage.Text = "Erro na alteração do registo.";
            }
        }

        //==========================================================================================
        //FORMVIEW ITEMCOMMAND  - Occurs when a button within a FormView control is clicked. 
        //==========================================================================================
        protected void fv_ItemCommand(object sender, FormViewCommandEventArgs e)
        {

            if (e.CommandName == "Edit")
            {

                int index = gvEquipamentos.SelectedIndex;
                int ID = Convert.ToInt32(gvEquipamentos.DataKeys[index].Values["idEquipamento"]);
                string idGrand = Convert.ToString(gvEquipamentos.DataKeys[index].Values["idGrandeza"]);
                //Response.Write(ID + idGrand);


                //  if (e.CommandName == "DELETE")
                //{           
                //    int ID = Convert.ToInt32(gridview.DataKeys[gridview.SelectedIndex].Values[1]);            

                //}
            }
        }

        
        //==========================================================================================
        //==========================================================================================
        //FUNÇÕES SOBRE A OBJECTDATASOURCE DA FORMVIEW
        //==========================================================================================
        //==========================================================================================


        //==========================================================================================
        //OBJECTDATASOURCE INSERTED EVENT - ocurrs when an insert operation hast completd
        //==========================================================================================
        //isto é necessario para fazer manualmente o set dos e.AffectedRows, senao nao conseguem ser apanhados...
        //porque nao estou a usar um dataadapter pelo meio- COPIADO DO PGS, VERIFICAR SE NECESSÁRIO
        protected void OBJDS_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            e.AffectedRows = (int)e.ReturnValue;
            if (e.Exception != null) lblMessage.Text ="..."+ e.ToString();
        }



        //==========================================================================================
        //OBJECTDATASOURCE UPDATED EVENT - ocurrs when an update operation hast completd 
        //==========================================================================================
        //isto é necessario para fazer manualmente o set dos e.AffectedRows, senao nao conseguem ser apanhados...
        //porque nao estou a usar um dataadapter pelo meio- COPIADO DO PGS, VERIFICAR SE NECESSÁRIO
        protected void OBJDS_Updated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            e.AffectedRows = (int)e.ReturnValue;
            if (e.Exception != null) lblMessage.Text = "..." + e.ToString();
        }



        //==========================================================================================
        //==========================================================================================
        //OUTRAS FUNÇÕES 
        //==========================================================================================
        //==========================================================================================


        protected void rebindGridViewAfterAction(object sender, EventArgs e) //remove o selectedindex
        {
            gvEquipamentos.DataBind();
            gvEquipamentos.SelectedIndex = -1;
        }

        protected void rebindGridViewAfterAction() //remove o selectedindex
        {
            gvEquipamentos.DataBind();
            gvEquipamentos.SelectedIndex = -1;
        }

        protected void rebindGridView(object sender, EventArgs e) //Mantem o selectedIndex
        {
            gvEquipamentos.DataBind();
        }

        protected System.Drawing.Color ConvertColor(int i)
        {
            System.Drawing.ColorConverter colConvert = new ColorConverter();

            System.Drawing.Color colorName;
            switch (i)
            {
                case 0:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");//(System.Drawing.Color)colConvert.ConvertFromString("Red");
                    break;
                case 1:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");//(System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
                default:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");//(System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
            }

            return colorName;
        }

	

       


  

        protected void dsEquipsByGrandezaSelecting(object source, ObjectDataSourceMethodEventArgs e)
        {
           //string idGrand = Convert.ToString(gvEquipamentos.DataKeys[gvEquipamentos.SelectedIndex].Values["idGrandeza"]);
           //e.InputParameters["idGrandeza"] = idGrand;
        }

        protected void lbIntervencoes_Click(object sender, EventArgs e)
        {

        }

        protected void lbHistorico_Click(object sender, EventArgs e)
        {
            {//Histórico
                if (gvEquipamentos.SelectedIndex < 0)
                {
                    lblMessage.Text = "Tem de seleccionar um equipamento primeiro.";
                    return;
                }

                gvGenerico.AutoGenerateColumns = true;
                string strSQL = "SELECT     TipoEquipamento.descricao AS tipoEquipamento, HistoricoEquipamento.numIdentificacao, HistoricoEquipamento.numSerie, HistoricoEquipamento.refUltimaCalibracao, HistoricoEquipamento.dtAlteracao, Funcionario.nomeAbreviado FROM HistoricoEquipamento INNER JOIN Funcionario ON HistoricoEquipamento.idUtilAlteracao = Funcionario.idUtilizador INNER JOIN TipoEquipamento ON HistoricoEquipamento.idTipoEquipamento = TipoEquipamento.idTipoEquipamento WHERE idEquipamento = " + gvEquipamentos.DataKeys[gvEquipamentos.SelectedIndex].Values["idEquipamento"].ToString()+ " ORDER BY HistoricoEquipamento.dtAlteracao desc ";

                //Response.Write(strSQL); 

                DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL);
                gvGenerico.DataSource = dt;
                gvGenerico.DataBind();

                gvCertificados.Visible = false;
                gvGenerico.Visible = true;
            }

        }

        protected void lbServicos_Click(object sender, EventArgs e)
        {
            if (gvEquipamentos.SelectedIndex < 0)
            {
                lblMessage.Text = "Tem de seleccionar um equipamento primeiro.";
                return;
            }

            gvGenerico.AutoGenerateColumns = true;
            string strSQL = "SELECT S.idEquipamento, S.refServico, ESTADOsERVICO.DESCRICAO AS ESTADO, s.dtCriacao as dtEntrada FROM SERVICO  S INNER JOIN ESTADOSERVICO ON S.IDESTADOSERVICO = ESTADOSERVICO.IDESTADOSERVICO 	WHERE S.idEquipamento = " + gvEquipamentos.DataKeys[gvEquipamentos.SelectedIndex].Values["idEquipamento"].ToString() + " ORDER BY s.dtCriacao";
            //Response.Write(strSQL); 

            DataTable dt = GERAL.clsDataAccess.ExecuteDT(strSQL);
            gvGenerico.DataSource = dt;
            gvGenerico.DataBind();

            gvCertificados.Visible = false;
            gvGenerico.Visible = true; 
        }


        protected void lbCertificados_Click(object sender, EventArgs e)
        {
            if (gvEquipamentos.SelectedIndex < 0)
            {
                lblMessage.Text = "Tem de seleccionar um equipamento primeiro.";
                return;
            }
            //certificados
            DATA.EstadoCertificadoBD data = new LabMetro.DATA.EstadoCertificadoBD();
            DataTable DT = data.DTListCertificadoByEquipamento(gvEquipamentos.DataKeys[gvEquipamentos.SelectedIndex].Values["idEquipamento"].ToString());
            data = null;

            gvCertificados.DataSource = DT;
            gvCertificados.DataBind();

            gvCertificados.Visible = true;
            gvGenerico.Visible = false; 
        }

        //============================================================================	
        //ITEM DATABOUND DO GRID CERTIFICADOS.
        //============================================================================	
        public void gvCertificados_databound(Object sender, System.EventArgs e)
        {

            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            //{
            //    LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];

            //    int numCells = e.Item.Cells.Count;
            //    for (int i = 1; i < numCells; i++)
            //    {

            //        //Response.Write("cell" + i + "-"+e.Item.Cells[i].Text+"<br />"); 
            //        if (!e.Item.Cells[i].HasControls()) //para nao pôr o link nas cells que conteem checkboxes
            //        {
            //            e.Item.Cells[i].ToolTip = "Click para visualisar o documento " + e.Item.Cells[3].Text;
            //            e.Item.Cells[i].Attributes.Add("onclick", Page.GetPostBackClientHyperlink(button, ""));
            //        }
            //    }
            //}
        }

        //==================================================================================
        // Função que permite visualizar o documento pretendido pelo utilizador
        //==================================================================================
        public void visualisarDocumento(Object sender, GridViewCommandEventArgs e)
        {
        //    if (e.CommandName.ToString() == "Select")
        //    {
        //        string doc = e.Item.Cells[4].Text;
        //        string nome = downloadpath(doc);
        //        Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");
        //    }
        //}

        //public string downloadpath(object filename)
        //{
        //    if (filename != null && filename.ToString() != "")
        //    {
        //        string myPath = (string)ConfigurationManager.AppSettings["PATHABS_CERT_FINAIS_CERTIFICADOS"];
        //        myPath = myPath + "/" + filename.ToString();

        //        return myPath;
        //    }
        //    else
        //    {
        //        return "#";
        //    }
        }

        public string downloadpath(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = (string)ConfigurationManager.AppSettings["PATHABS_CERT_FINAIS_CERTIFICADOS"];
                myPath = myPath + "/" + filename.ToString();

                return myPath;
            }
            else
            {
                return "#";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void BtnExportGrid_Click(object sender, EventArgs args)
        {
            //if (rdoBtnListExportOptions.SelectedIndex == 1)
            //{
            //    //  the user wants all rows exported, turn off paging
            //    //  and rebing the grid before sending it to the export
            //    //  utility
            gvEquipamentos.AllowPaging = false;
            gvEquipamentos.Columns[10].Visible = false;
            gvEquipamentos.DataBind();
            //}
            //else if (rdoBtnListExportOptions.SelectedIndex == 2)
            //{
            //    //  the user wants just the first 100,
            //    //  adjust the PageSize and rebind
            //    gvEquipamentos.PageSize = 100;
            //    gvEquipamentos.DataBind();
            //}

            //  pass the grid that for exporting ...
            GridViewExportUtil.Export("Customers.xls", gvEquipamentos);

        }


        //mara em cores diferentes os conformes, nao conformes e aceites com restrições
        protected void CoresLinhas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Make sure we are working with a DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataAccessLayer.dlEquipamento.DMMRow equipamento = (DataAccessLayer.dlEquipamento.DMMRow)((System.Data.DataRowView)e.Row.DataItem).Row;

              
                if (!equipamento.IsidEstadoRelacaoCalibracaoNull() && equipamento.idEstadoRelacaoCalibracao.ToString() == "2") //nao conforme
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#D90C15"); 
                }

                if (!equipamento.IsidEstadoRelacaoCalibracaoNull() && equipamento.idEstadoRelacaoCalibracao.ToString() == "3") //aceite com restricoes
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF6600");
                }
            }
        }
      
      
    }

}
