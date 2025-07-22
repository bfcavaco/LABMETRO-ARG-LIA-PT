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
    public partial class ListaServicosCalibrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillDDGrandezas();
                fillLocalCalibracao();
                lblCountServicos.Text = ""; 
            }
        }

        protected void ddGrandeza_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvServicos.DataBind();
            lblCountServicos.Text = "Núm. Serv.: "+ gvServicos.Rows.Count.ToString();
        }

        protected void ddLocalCalibracao_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvServicos.DataBind();
            lblCountServicos.Text = "Núm. Serv.: " + gvServicos.Rows.Count.ToString();
        }

        private void fillDDGrandezas()
        {
            // Fill da DropDownList das Grandezas
            DATA.ListasBD listaGrandezas = new LabMetro.DATA.ListasBD();
            SqlDataReader DR3 = listaGrandezas.DRListaGrandezas();
            ddGrandeza.DataSource = DR3;
            ddGrandeza.DataBind();
            ddGrandeza.Items.Insert(0, new ListItem("", ""));
            DR3.Close();
        }
        private void fillLocalCalibracao()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaLocalCalibracao();
            ddLocalCalibracao.DataSource = DR;
            ddLocalCalibracao.DataBind();
            DR.Close();

            lista = null;
            ddLocalCalibracao.Items.Insert(0,(new ListItem("todos","-1")));
        }

        public string downloadpath(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = (string)ConfigurationManager.AppSettings["UPLOAD_REQ_URL"];

                myPath = myPath + "/" + filename.ToString();
                return myPath;
            }
            return "#";

        }

        
        protected void BtnExportGrid_Click(object sender, EventArgs args)
        {
       
            //  pass the grid that for exporting ...
            GridViewExportUtil.Export("ACalibrar.xls", gvServicos);

        }
      
    }
  
}
