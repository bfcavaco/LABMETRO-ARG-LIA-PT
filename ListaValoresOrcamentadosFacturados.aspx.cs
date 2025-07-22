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
    public partial class ListaValoresOrcamentadosFacturados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnExportGrid_Click(object sender, EventArgs args)
        {

            //  pass the grid that for exporting ...
            GridViewExportUtil.Export("ACalibrar.xls", gvResultados);

        }
    }
}
