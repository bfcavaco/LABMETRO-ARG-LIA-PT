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
using System.Configuration;
using System.IO;

namespace LabMetro
{
    public partial class RepExcel : System.Web.UI.Page
    { 
        
        DataView DV;
        private const string ID_PAG = "PASTASENSAIO_0";//NOME DA PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {


            Page.Form.DefaultButton = Button1.UniqueID;

             

            lblMessage.Text = "";
			lblRecordCount.Text =""; 
            
            Hashtable ht = (Hashtable)Session["HTPermissions"]; 
			if(ht == null) //session expired
			{
				Server.Transfer("Default.aspx?err=2",false); 
			}
			else
			{
				if(!ht.ContainsKey(ID_PAG))
				{
					Server.Transfer("Default.aspx?err=1",false);
				}
                else
                {
			        if(!Page.IsPostBack)
			        {

						// Preencher valores default
						DateTime dt = DateTime.Now; 
						
						//Response.Write(dt.ToString()); 
						DateTime dtLastYear; 
						try
						{
							dtLastYear = new DateTime(dt.Year-1,dt.Month,dt.Day); 
						}
						catch
						{
							dtLastYear = new DateTime(dt.Year-1,dt.Month+1,1); 
						}
						//Response.Write(dtLastYear.ToString()); 

						txtDtBRE.Text = dtLastYear.ToShortDateString(); 
				
				        ViewState["sortField"] = "dtBRE";
				        ViewState["sortDirection"] = "DESC";
                        
                        FillDropDowns(); 
				        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_SEARCH;
                        
			        }
                   
                }
            }
        }

        private void FillDropDowns()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaEstadosServico(); 
            ddEstado.DataSource=DR;
            ddEstado.DataBind(); 
            ddEstado.Items.Insert(0,new ListItem("",""));  
            DR.Close(); 

			SqlDataReader DR2 = lista.DRListaLocalCalibracao(); 
			ddLocalEquipamento.DataSource=DR2 ;
			ddLocalEquipamento.DataBind(); 
			ddLocalEquipamento.Items.Insert(0,new ListItem("",""));  
			DR2.Close(); 
			
			lista = null; 

			// Fill da DropDownList das Grandezas
			DATA.ListasBD listaGrandezas = new LabMetro.DATA.ListasBD(); 
			SqlDataReader DR3 = listaGrandezas.DRListaGrandezas(); 
			ddGrandeza.DataSource = DR3;
			ddGrandeza.DataBind(); 
			ddGrandeza.Items.Insert(0,new ListItem("",""));
			DR3.Close(); 

			listaGrandezas = null; 

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
	
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}

		
		#endregion

		
		

		  protected void Button1_Click(object sender, EventArgs e)
        {
	
       

            DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
            DataTable DT = servico.DTGetServicosComDatas(txtNomeEmpresa.Text, txtNumBRE.Text, txtRefServico.Text, ddEstado.SelectedValue.ToString(), txtNumIdentificacao.Text, txtNumSerie.Text, ddLocalEquipamento.SelectedValue, rblCalibracaoExterna.SelectedValue, ddGrandeza.SelectedValue, txtDtBRE.Text, txtTipoEquipamento.Text, txtIdEquipamento.Text);

            DV = new DataView(DT);

            string rowfilter = " 1 = 1 ";

          
            //rowfilter += " ORDER BY idServico desc ";
            DV.RowFilter = rowfilter;
            DV.Sort = " idServico DESC ";
            GridView1.DataSource = DV;
            GridView1.DataBind();
            GERAL.GridViewExportUtil.Export("servicos.xls", GridView1);

        }

		


	}
}

