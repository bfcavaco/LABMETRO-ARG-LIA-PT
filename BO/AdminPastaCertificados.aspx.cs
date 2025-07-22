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
using System.Linq;
using System.Collections.Generic;

namespace LabMetro
{
    public partial class AdminPastaCertificados : System.Web.UI.Page
    {
        private static string certificadosPath = (string)ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CERTIFICADOS"];

        private static string certificadosFolder = System.Web.HttpContext.Current.Server.MapPath("~/" + certificadosPath);

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
      

        
        override protected void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            lblStatus.Text = "";




            //if (!Page.IsPostBack)
            //{
            //    GridView1.DataBind();
            //}


        }

        override protected void OnInit(EventArgs e)
        {
            //base.OnInit(e);
            //Button1.Click += Button1_Click;
        }



        protected void UploadThisFile(FileUpload upload)
        {
            if (upload.HasFile)
            {
                string theFileName = Path.Combine(certificadosFolder, upload.FileName);
                upload.SaveAs(theFileName);
                lblMessage.Text = "<b>Ficheiro carregado com sucesso.</b>";
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            e.Cancel = true;
            string fileName = ((HyperLink)GridView1.Rows[e.RowIndex].FindControl("FileLink")).Text;

            fileName = Path.Combine(certificadosFolder, fileName);
            File.Delete(fileName);
            BindGrid();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //UploadThisFile(FileUpload1);
            //BindGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //FileInfo TheFile = new FileInfo(MapPath(".") + "\\" + txtOldFile.Text);

                FileInfo TheFile = new FileInfo(Path.Combine(certificadosFolder, txtOldFile.Text));
                if (TheFile.Exists)
                {
                    File.Move(Path.Combine(certificadosFolder, txtOldFile.Text), Path.Combine(certificadosFolder, txtNewFile.Text.ToUpper()));
                }
                else
                {
                    throw new FileNotFoundException();
                }
                lblMessage.Text = "Nome do ficheiro alterado com sucesso.";
                txtNewFile.Text = "";
                txtOldFile.Text = "";
                GridView1.DataBind();
            }
            catch (FileNotFoundException ex)
            {
                lblMessage.Text += ex.Message;
            }
            catch (Exception ex)
            {
                lblMessage.Text += ex.Message;
            }
        }

        private void BindGrid()
        {
            string searchpattern = txtSearch.Text.ToString();
           
               
            DirectoryInfo dirInfo = new DirectoryInfo(certificadosFolder);
            FileInfo[] f = dirInfo.GetFiles(searchpattern + "*");
            //  IEnumerable<string> files = Directory.GetFiles(certificadosFolder).Where(file => file.Contains(searchpattern.ToUpper())); //CASE SENSITIVE

         
            GridView1.DataSource = f;
            GridView1.DataBind();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindGrid();
          
        }
    }
} 
