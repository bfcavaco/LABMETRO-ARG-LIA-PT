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
using System.IO;
using System.Configuration;


namespace LabMetro
{
    public partial class GestPastaAssinaturas : System.Web.UI.Page
    {
        private static string assinaturasPath = (string)ConfigurationManager.AppSettings["ASSINATURAS_PATH_REL"];

        private static string assinaturasFolder = System.Web.HttpContext.Current.Server.MapPath("~/" + assinaturasPath);

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        private const string ID_PAG = "ASSINATURAS_1";//NOME DA PAGINA

        
        override protected void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            lblStatus.Text = "";
      
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
                        GridView1.DataBind();
                    }
                }
            }
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Button1.Click += Button1_Click;
        }

        protected string[] GetUploadList()
        {
         
            string[] files = Directory.GetFiles(assinaturasFolder);
            string[] fileNames = new string[files.Length];
            Array.Sort(files);

            for (int i = 0; i < files.Length; i++)
            {
                fileNames[i] = Path.GetFileName(files[i]);
            }

            return fileNames;
        }

        protected void UploadThisFile(FileUpload upload)
        {
            if (upload.HasFile)
            {
                string theFileName = Path.Combine(assinaturasFolder, upload.FileName);
                upload.SaveAs(theFileName);
                labelStatus.Text = "<b>Ficheiro carregado com sucesso.</b>";
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            e.Cancel = true;
            string fileName = ((HyperLink)GridView1.Rows[e.RowIndex].FindControl("FileLink")).Text;

            fileName = Path.Combine(assinaturasFolder, fileName);
            File.Delete(fileName);
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            UploadThisFile(FileUpload1);
            GridView1.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //FileInfo TheFile = new FileInfo(MapPath(".") + "\\" + txtOldFile.Text);

                FileInfo TheFile = new FileInfo(Path.Combine(assinaturasFolder, txtOldFile.Text));
                if (TheFile.Exists)
                {
                    File.Move(Path.Combine(assinaturasFolder, txtOldFile.Text), Path.Combine(assinaturasFolder, txtNewFile.Text));
                }
                else
                {
                    throw new FileNotFoundException();
                }
                lblStatus.Text = "Nome do ficheiro alterado com sucesso.";
                txtNewFile.Text = "";
                txtOldFile.Text = "";
                GridView1.DataBind();
            }
            catch (FileNotFoundException ex)
            {
                lblStatus.Text += ex.Message;
            }
            catch (Exception ex)
            {
                lblStatus.Text += ex.Message;
            }
        }

    }
} 
