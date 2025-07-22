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
    public partial class GestEstadoRecepcao : System.Web.UI.Page
    {

        protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
        protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator4;
        protected System.Web.UI.HtmlControls.HtmlTable tblPesquisa;

        private const string ID_PAG = "ESTADOS_RECEP_PISTOLA_1";//NOME DA PAGINA

        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblMessage.Text = "";
            lblOK.Text = "";

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

                       
                        fillDDEstadoDestino();

                    }
                }
            }
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

        private void getPrateleira(string idServico)
        {
            string strSQL = "select refServico,expedicao, dbo.udfGetReferenciaBRE(servico.idBRE) as refBRE, bre.observacoes as obsBRE from servico inner join bre on servico.idbre = bre.idBre and servico.idServico = " + idServico;

            SqlDataReader DR = GERAL.clsDataAccess.ExecuteDR(strSQL); 
            if (DR.HasRows)
            {

                while (DR.Read())
                {
                    txtBRE.Text = DR["refBRE"].ToString();
                    txtPrateleira.Text = DR["expedicao"].ToString();
                    lblOK.Text = DR["refServico"].ToString();
                    ViewState["idServico"] = idServico.ToString();
                    txtObsBre.Text = DR["obsBre"].ToString();
                }
            }

            DR.Close();
            


        }

        protected void txtIdServico_TextChanged(object sender, EventArgs e)
        {
            string idEstadoServico = ddEstadoOrigem.SelectedValue;
            string idServico = txtIdServico.Text;

            if (idEstadoServico == "13") //equipamento na recepcao
            {
                getPrateleira(idServico);
                ViewState["idServico"] = idServico;
            }
            else
            {
                ViewState["idServico"] = null;
            }
            UpdateEstadoServico(idServico);
        }

        protected void btnPrateleira_Click(object sender, EventArgs e)
        {
            if (ViewState["idServico"] != null && ViewState["idServico"].ToString() != "")
            {
                string strSQL = "Update bre set expedicao = '" + txtPrateleira.Text + "' from  bre inner join servico on bre.idbre = servico.idbre where servico.idServico = " + ViewState["idServico"];
                if (GERAL.clsDataAccess.myExecuteNonQuery(strSQL) == 1)
                {
                    txtPrateleira.Text = "";
                    lblOK.Text = "Expedição Actualizada";
                }
                else
                {
                    lblOK.Text = "Expedição Não actualizado";
                }
            }
        }
    }
}
