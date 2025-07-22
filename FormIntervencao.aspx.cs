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
    public partial class FormIntervencao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = ""; 
            if (!Page.IsPostBack)
            {
                gvIntervencoes.DataBind();
                if (gvIntervencoes.Rows.Count == 0)
                {
                    lblMessage.Text = "Não foram registadas intevenções sobre este equipamento."; 
                    fvIntervencao.ChangeMode(FormViewMode.Insert);
                }

            }
        }

        protected void linkInsert_Click(object sender, EventArgs e)
        {
            fvIntervencao.ChangeMode(FormViewMode.Insert);
            fvIntervencao.DataBind();
            //string rep = ""; //nao serve para nada, so para testar o controlo de versoes
        }


        //==========================================================================================
        //==========================================================================================
        //FUNÇÕES SOBRE A FORMVIEW
        //==========================================================================================
       
        //==========================================================================================
        //FORMVIEW DATABOUND - Occurs after the server control binds to a data source. (Inherited from BaseDataBoundControl.)
        //==========================================================================================
        protected void fv_DataBound(object sender, EventArgs e)
        {
            // DataTextField="descricao" DataValueField="idModelo"  SelectedValue='<%# Bind("idModelo") %>' 
            if (fvIntervencao.CurrentMode == FormViewMode.Edit)
            {
                
                //    DataRowView drv = (DataRowView)fvIntervencao.DataItem;
                //    DropDownList ddmar = (DropDownList)fvIntervencao.FindControl("ddMarcaEdit");

               
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
            
        }

        //==========================================================================================
        //FORMVIEW UPDATING EVENT - occurs when an update button in fv is clicked, but before the 
        //                          update operation
        //==========================================================================================
        protected void fv_Updating(object sender, FormViewUpdateEventArgs e)
        {
           
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

                int index = gvIntervencoes.SelectedIndex;
                int ID = Convert.ToInt32(gvIntervencoes.DataKeys[index].Values["idIntervencao"]);
                //string idGrand = Convert.ToString(gvIntervencoes.DataKeys[index].Values["idGrandeza"]);
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
            if (e.Exception != null) lblMessage.Text = "..." + e.ToString();
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
            gvIntervencoes.DataBind();
            gvIntervencoes.SelectedIndex = -1;
        }

        protected void rebindGridViewAfterAction() //remove o selectedindex
        {
            gvIntervencoes.DataBind();
            gvIntervencoes.SelectedIndex = -1;
        }

        protected void rebindGridView(object sender, EventArgs e) //Mantem o selectedIndex
        {
            gvIntervencoes.DataBind();
        }
    }
}
