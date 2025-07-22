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
    public partial class GestEstadoPistola : System.Web.UI.Page
    {

        protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
        protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator4;
        protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;

        private const string ID_PAG = "ESTADOS_EQUIP_PISTOLA_1";//NOME DA PAGINA

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

                        fillDDEstadoOriginal();
                        fillDDEstadoDestino();

                    }
                }
            }
        }




        //==============================================================================================
        //==============================================================================================
        private void fillDDEstadoOriginal()
        {

            DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD();
            SqlDataReader dr = estado.DRGetListaEstadosServico();
            ddEstadoOrigem.DataTextField = "descricao";
            ddEstadoOrigem.DataValueField = "ident";
            ddEstadoOrigem.DataSource = dr;
            ddEstadoOrigem.DataBind();

            ddEstadoOrigem.Items.Insert(0, new ListItem("", ""));
            dr.Close();

            estado = null;

        }

        //==============================================================================================
        //==============================================================================================
        private void fillDDEstadoDestino()
        {
            if (ddEstadoOrigem.SelectedValue != "")
            {
                DATA.EstadoEquipBD estado = new LabMetro.DATA.EstadoEquipBD();
                SqlDataReader dr = estado.DRGetEstadosServicosEsubsequentes(ddEstadoOrigem.SelectedValue);
                ddEstadoDestino.DataTextField = "descricao";
                ddEstadoDestino.DataValueField = "idEstadoServico";
                ddEstadoDestino.DataSource = dr;
                ddEstadoDestino.DataBind();

                if (ddEstadoOrigem.SelectedValue == "1") // recepcionado
                {
                    ddEstadoDestino.SelectedValue = "3";
                }


                dr.Close();

                estado = null;
                int idPerfil = (int)Session["idPerfil"];
                if (idPerfil != 6 && idPerfil != 4 && idPerfil != 5) //todos os que noa podem calibrar
                {
                    ListItem foundItem6 = (ListItem)ddEstadoDestino.Items.FindByValue("6"); //calibrado
                    ListItem foundItem25 = (ListItem)ddEstadoDestino.Items.FindByValue("25"); //calibrado no ext
                    if (foundItem6 != null) ddEstadoDestino.Items.Remove(foundItem6);
                    if (foundItem25 != null) ddEstadoDestino.Items.Remove(foundItem25);
                }
            }
            else
            {
                ddEstadoDestino.Items.Clear();
            }
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
            ddEstadoOrigem.SelectedIndexChanged += new System.EventHandler(ddEstadoOrigem_SelectedIndexChanged);
        }
        #endregion


        //==============================================================================================
        //==============================================================================================
        private void ddEstadoOrigem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddEstadoOrigem.SelectedValue != "")
            {
                fillDDEstadoDestino();

            }
            else
            {
                ddEstadoDestino.Items.Clear();
            }
        }


        //==============================================================================================
        //==============================================================================================
        private void UpdateEstadoServico(string idServico)
        {


            if (ddEstadoOrigem.SelectedValue == "" || ddEstadoDestino.SelectedValue == "")
            {
                lblMessage.Text = "Seleccione estados antes de submeter.";
                return;
            }
            else
            {
                if (ddEstadoDestino.SelectedValue == "7" || ddEstadoDestino.SelectedValue == "9")
                {
                    lblMessage.Text = "Só pode anular ou suspender equipamento na pasta de Ensaio, indicado a razão.";
                    return;
                }
                else
                {

                    DATA.EstadoEquipBD data = new LabMetro.DATA.EstadoEquipBD();
                    lblMessage.Text = data.UpdateEstadosServico(ddEstadoOrigem.SelectedValue, ddEstadoDestino.SelectedValue, Session["UserId"].ToString(), idServico).ToString();
                    data = null;

                    txtIdServico.Text = "";
                    txtIdServico.Focus();



                }
            }
        }

        protected void txtIdServico_TextChanged(object sender, EventArgs e)
        {
            UpdateEstadoServico(txtIdServico.Text);
        }
    }
}