using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using Ionic.Zip;

namespace LabMetro
{
    public partial class ZipCertificados : Page
    {
        private const string ID_PAG = "ZIP_CERTIFICADOS_0"; //NOME DA PAGINA
    
        DataTable DT;
        DataView DV;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
    
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            
        }

       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnPesquisa.UniqueID;

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
                else if (!Page.IsPostBack)
                {
                    fillDDGrandeza();
                }
            }
        }

        protected void txtPesquisaEmpresa_TextChanged(object sender, EventArgs e)
        {
            fillDDEmpresa();
        }

        protected void btnPesquisaEmpresa_Click(object sender, EventArgs e)
        {
            fillDDEmpresa();
        }

        private void fillDDEmpresa()
        {
            DATA.EmpresasBD empresa = new DATA.EmpresasBD();
            SqlDataReader dr = empresa.DREmpresasPesquisaPorNome(txtPesquisaEmpresa.Text);
            ddEmpresa.DataSource = dr; ;
            ddEmpresa.DataBind();

            empresa = null;

            fillBRE();
        }

        private void fillBRE()
        {
            DATA.BreBD bre = new DATA.BreBD();
            SqlDataReader dr = bre.DRGetBreByIdEmpresa(ddEmpresa.SelectedValue);

            ddBRE.DataSource = dr; ;
            ddBRE.DataBind();

            bre = null;
        }

        private void fillDDGrandeza()
        {
            string strSQL = "SELECT idGrandeza, descricao FROM Grandeza "; //WHERE activo = 1"; 
            SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL);

            ddGrandeza.DataSource = dr;
            ddGrandeza.DataBind();
            ddGrandeza.Items.Insert(0, new ListItem("", ""));

            dr.Close();
        }

        protected void ddEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillBRE();
            gvCertificados.PageIndex = 0;
            BindGrid();
        }

        //==================================================================================
        // Função que permite visualizar o documento pretendido pelo utilizador
        //==================================================================================
        public void visualisarDocumento(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Select")
            {
                string doc = e.Item.Cells[6].Text;
                string nome = downloadpath(doc);
                Response.Write("<script language=javascript>window.open('" + nome + "','new_Win','toolbar=0,menubar=0,resizable=1');</script>");
            }
        }

        private void BindGrid()
        {
            DATA.EstadoCertificadoBD data = new DATA.EstadoCertificadoBD();
            DT = data.DTListCertificadoByEmpresa(ddEmpresa.SelectedValue, ddBRE.SelectedValue, ddGrandeza.SelectedValue);
            DV = new DataView(DT);

            gvCertificados.DataSource = DV;
            gvCertificados.DataBind();
            data = null;
        }

        public string downloadpath(object filename)
        {
            if (filename != null && filename.ToString() != "")
            {
                string myPath = ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CERTIFICADOS"];
                myPath = myPath + "/" + filename.ToString();
                return myPath;
            }
            else
            {
                return "#";
            }
        }

        public void dgCertificados_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                LinkButton button = (LinkButton)e.Item.Cells[0].Controls[0];

                int numCells = e.Item.Cells.Count;
                for (int i = 1; i < numCells; i++)
                {

                    if (!e.Item.Cells[i].HasControls())
                    {
                        e.Item.Cells[i].ToolTip = "Click para visualisar o documento " + e.Item.Cells[6].Text;
                        e.Item.Cells[i].Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(button, ""));
                    }
                }
            }
        }
        protected void btnDownloadNow_Click(object sender, EventArgs e)
        {
            string pastaCertificadosFinaisCertificados = (string)ConfigurationManager.AppSettings["PASTA_CERT_FINAIS_CERTIFICADOS"];

            // Tell the browser we're sending a ZIP file!
            var downloadFileName = string.Format("Certificados-{0}.zip", DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss"));
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "filename=" + downloadFileName);

            // Zip the contents of the selected files
            using (var zip = new  ZipFile())
            {
                
                // Construct the contents of the README.txt file that will be included in this ZIP
                var readMeMessage = string.Format("O ficheiro ZIP {0} contem os seguintes ficheiros:{1}{1}", downloadFileName, Environment.NewLine);

                // Add the checked files to the ZIP

                string fileName = "";
                foreach (GridViewRow gvrow in gvCertificados.Rows)
                {
                    CheckBox CheckBox1 = (CheckBox)gvrow.FindControl("chkSelected");
                    if (CheckBox1.Checked)
                    {
                        // Record the file that was included in readMeMessage
                        fileName =  gvCertificados.DataKeys[gvrow.RowIndex].Value.ToString();
                        string filePath = pastaCertificadosFinaisCertificados + "\\" + fileName;
                        readMeMessage += string.Concat("\t* ", fileName, Environment.NewLine);

                        zip.AddFile(filePath, "Certificados");
                    }
                }

                // Add the README.txt file to the ZIP
                zip.AddEntry("README.txt", readMeMessage, Encoding.ASCII);

                // Send the contents of the ZIP back to the output stream
                zip.Save(Response.OutputStream);
            }
        }

        protected void btnPesquisa_Click(object sender, EventArgs e)
        {
            //gvCertificados.PageIndex = 0;
            BindGrid();
        }
    }
}
