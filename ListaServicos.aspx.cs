using System;
using System.Collections;
using System.Collections.Specialized; // para iorderdictionary
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient; 

namespace LabMetro
{
    public partial class ListaServicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
                SqlDataReader DR = lista.DRListaEstadosServico();
                ddEstado.DataSource = DR;
                ddEstado.DataBind();
                ddEstado.Items.Insert(0, new ListItem("", ""));
                DR.Close();

                SqlDataReader DR2 = lista.DRListaLocalCalibracao();
                ddLocalEquipamento.DataSource = DR2;
                ddLocalEquipamento.DataBind();
                ddLocalEquipamento.Items.Insert(0, new ListItem("", ""));
                DR2.Close();

                lista = null;

                // Fill da DropDownList das Grandezas
                DATA.ListasBD listaGrandezas = new LabMetro.DATA.ListasBD();
                SqlDataReader DR3 = listaGrandezas.DRListaGrandezas();
                ddGrandeza.DataSource = DR3;
                ddGrandeza.DataBind();
                ddGrandeza.Items.Insert(0, new ListItem("", ""));
                DR3.Close();

                listaGrandezas = null;

            }

        }        

      


        protected void selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
          
            //DataTable dt = (DataTable)e.ReturnValue; //o returnavalue do select é uma datatable; 
            //Response.Write(dt.Rows.Count.ToString() + "<br />");

            if (e.Exception != null)
            {
                Response.Write(e.Exception.ToString());
            }
            else
            {
            }
        }
        //Handle the Selecting event to perform additional initialization that is specific to your application, to validate the values of parameters, or to change the parameter values before the ObjectDataSource control performs the data retrieval operation. The parameters are available as an IDictionary collection that is accessed by the InputParameters property, which is exposed by the ObjectDataSourceMethodEventArgs object.
        protected void selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {


            //if (e.ExecutingSelectCount == false) //nao está a executar um rowcount
            //{
       
            //}
            //else
            //{

            //}
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            gvServicos.DataBind(); 
        }

        protected void gvServicos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


      
       

    //    //http://www.asp.net/LEARN/data-access/tutorial-45-cs.aspx
    //    //Set the drop-down lists in the UPDATE, INSERT, and DELETE tabs to “(None)” and then click the Next button. The Configure Data Source wizard now prompts for the sources of the GetProductsPaged method’s startRowIndex  and maximumRows input parameters. In actuality, these input parameters are ignored. Instead, the startRowIndex  and maximumRows values will be passed in through the Arguments property in the ObjectDataSource’s Selecting event handler, just like how we specified the sortExpression in this tutorial’s first demo. Therefore, leave the parameter source drop-down lists in the wizard set at “None”.
    }
}
