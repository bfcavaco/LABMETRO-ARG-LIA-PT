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
    /// Summary description for GestEstadoEquip.
    /// </summary>
    public partial class GestLocalPistola : System.Web.UI.Page
    {

        protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
        protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator4;
        protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;

        private const string ID_PAG = "LOCAL_EQUIP_PISTOLA_1";//NOME DA PAGINA

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

                        
                        fillDDLocalNovo();

                    }
                }
            }
        }




     
        //==============================================================================================
        //==============================================================================================
        private void fillDDLocalNovo()
        {
            string strSQL = "select idLocalCalibracao as id , descricao as local from LocalCalibracao";

            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);
            ddLocalNovo.DataTextField = "local";
            ddLocalNovo.DataValueField = "id";
            ddLocalNovo.DataSource = dr;
            ddLocalNovo.DataBind();

            ddLocalNovo.Items.Insert(0, new ListItem("", ""));
            dr.Close();
            dr = null;
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
        private void InitializeComponent()
        {
           
        }
        #endregion


       

        //==============================================================================================
        //==============================================================================================
        private void UpdateLocal(string idServico)
        {


            if (ddLocalNovo.SelectedValue == "")
            {
                lblMessage.Text = "Seleccione o Local  antes de submeter.";
                return;
            }
            else
            {
                

                    DATA.EstadoEquipBD data = new LabMetro.DATA.EstadoEquipBD();
                    lblMessage.Text = data.updateLocalServico(idServico, ddLocalNovo.SelectedValue, Session["UserId"].ToString()).ToString();
                    data = null;

                    txtIdServico.Text = "";
                    txtIdServico.Focus();



                
            }
        }

        protected void txtIdServico_TextChanged(object sender, EventArgs e)
        {
            UpdateLocal(txtIdServico.Text);
        }
    }
}
