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
	/// <summary>
	/// Summary description for ListaPastasEnsaio.
	/// </summary>
	public partial class ListaPastasEnsaio : System.Web.UI.Page
	{
        DataView DV;
        private const string ID_PAG = "PASTASENSAIO_0";//NOME DA PAGINA

	
		protected void Page_Load(object sender, System.EventArgs e)
		{

            Page.Form.DefaultButton = btnPesquisa.UniqueID;

             

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
                    if(!ht.ContainsKey("PASTASENSAIO_1")) //se n tem permissoes para ver os detalhes das pastas de ensaio, desactivar o link
                    {
                        DGpastaEnsaio.Columns[7].Visible=false; 
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


		private void BindGrid()
		{

		
				DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
				DataTable DT = servico.DTGetServicos(
					txtNomeEmpresa.Text,
					txtNumBRE.Text,
					txtRefServico.Text,
					ddEstado.SelectedValue.ToString(),
					txtNumIdentificacao.Text,
					txtNumSerie.Text,
					ddLocalEquipamento.SelectedValue,
					rblCalibracaoExterna.SelectedValue,
					ddGrandeza.SelectedValue,
					txtDtBRE.Text,
					txtTipoEquipamento.Text,
					txtIdEquipamento.Text,
					txtNumEtiquetaIPQ.Text);  
            
				DV = new DataView(DT);

                string rowfilter = " 1 = 1 ";

                if (txtMarca.Text != "") rowfilter += "and marca is not null and marca like '" + txtMarca.Text + "%' ";
                if (txtModelo.Text != "") rowfilter += "and modelo is not null and modelo like '" + txtModelo.Text + "%' ";
            if (txtCampoExtra.Text != "") rowfilter += "and (etiqueta1 is not null and etiqueta1 like '" + txtCampoExtra.Text + "%') or (etiqueta2 is not null and etiqueta1 like '" + txtCampoExtra.Text + "%') or (etiqueta3 is not null and etiqueta1 like '" + txtCampoExtra.Text + "%') ";

            DV.RowFilter = rowfilter; 

				//DV.RowStateFilter = DataViewRowState.CurrentRows; 
				lblRecordCount.Text = DV.Count.ToString() + " Registos encontrados"; 

				if(ViewState["sortField"].ToString()=="refBRE")
				{
					DV.Sort = ViewState["sortExpression"].ToString(); 
				}        
				else
				{
					DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
				}

				DGpastaEnsaio.DataSource = DV; 
			    DGpastaEnsaio.DataBind();
        
                servico = null; 
		
			if(DV.Table.Rows.Count > 0)
			{
				DGpastaEnsaio.Visible=true;
			}
			else
			{
				DGpastaEnsaio.Dispose();
				DGpastaEnsaio.Visible=false; 
				lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
			}
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			InitializeComponent2();
			base.OnInit(e);
		}
	
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}

		private void InitializeComponent2()
		{    
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
		}
		#endregion

		public void DoPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DGpastaEnsaio.CurrentPageIndex = e.NewPageIndex;
			BindGrid(); 
		}

		public void SortGrid(Object s, DataGridSortCommandEventArgs e)
		{
			switch (ViewState["sortDirection"].ToString())
			{
				case "ASC":
					ViewState["sortDirection"]="DESC"; 
					break;
				case "DESC":
					ViewState["sortDirection"]="ASC";
					break;
			}

            ViewState["sortField"] = e.SortExpression;
            
			if(ViewState["sortField"].ToString()=="refBRE")
            {
                ViewState["sortExpression"] = "ano "+ViewState["sortDirection"].ToString()+", numBRE "+ViewState["sortDirection"].ToString(); 
            }

           BindGrid(); 
		}

		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
            if((txtNomeEmpresa.Text =="")&&(txtNumBRE.Text =="")&&(txtRefServico.Text=="")&&(ddEstado.SelectedIndex==0)&&(txtNumSerie.Text=="") && (txtNumIdentificacao.Text=="") && (ddLocalEquipamento.SelectedIndex==0)&&(ddGrandeza.SelectedValue=="")&&(rblCalibracaoExterna.SelectedValue=="") && (txtTipoEquipamento.Text =="") && (txtIdEquipamento.Text=="")) //&& (txtMarca.Text =="") && (txtModelo.Text==""))
            {
                lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_PESQUISA_SERVICOS;
            }
            else
            {
                DGpastaEnsaio.CurrentPageIndex=0; 
                BindGrid(); 
            }
		}

		protected System.Drawing.Color ConvertColor(int i, string codigoBloqueioSAP)
		{
			System.Drawing.ColorConverter colConvert = new ColorConverter();
			 


			System.Drawing.Color colorName; 
			switch(i)
			{
				case 0:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
				case 1:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Gold");
					break;
				case 2:
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("DarkOrange");
					break;
				case 3: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("Crimson");
					break;
				default: 
					colorName= (System.Drawing.Color)colConvert.ConvertFromString("White");
					break;
			}

			if(codigoBloqueioSAP =="03") //se a empresa está inactiva em SAP (n tem a ver com o nivelBloqueiolabmetro, martelada...)
			{
				colorName = (System.Drawing.Color)colConvert.ConvertFromString("PowderBlue");
			}
			return colorName; 
		}

		protected void dgPE_itemCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Select")
			{
				
				


				LinkButton button = (LinkButton)e.Item.Cells[e.Item.Cells.Count-1].Controls[0];
				string fName = button.Text;
				//é um campo invisivel pq nao consigo aceder ao valor q está dentro de uma templatecolumn

				string fPath = (string) ConfigurationManager.AppSettings["PATHREL_CERT_FINAIS_CERTIFICADOS"].ToString();
				string filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + fPath + "/" + fName);
				
				try
				{
					if (File.Exists(filePath))
					{


						Response.Clear();
						Response.Buffer = true;
						//' x-mxdownload forces automatic download no matter what the content type

						Response.ContentType = "Application/x-msdownload";
						Response.ContentEncoding = System.Text.Encoding.Default;
						//força o dialog "save as"; 
						Response.AddHeader("content-disposition", "attachment; filename=" + fName);

						Response.WriteFile(filePath);
						Response.End();

					}
				}
				catch
				{

					//acho que este erro nao é grave.... 
					//	Erro : System.Threading.ThreadAbortException: Thread was being aborted.
					//   at System.Threading.Thread.AbortInternal()
					//   at System.Threading.Thread.Abort(Object stateInfo)
					//   at System.Web.HttpResponse.End()
					//   at LabMetro.FormBSE.dg_itemCommand(Object sender, DataGridCommandEventArgs e)

					//clsWriteError.WriteLog("Erro no abrir certificado no bse. Erro : " + ex.ToString());
				}
			}
		
		}

        protected void Button1_Click(object sender, EventArgs e)
        {

            DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD();
            DataTable DT = servico.DTGetServicos(txtNomeEmpresa.Text, txtNumBRE.Text, txtRefServico.Text, ddEstado.SelectedValue.ToString(), txtNumIdentificacao.Text, txtNumSerie.Text, ddLocalEquipamento.SelectedValue, rblCalibracaoExterna.SelectedValue, ddGrandeza.SelectedValue, txtDtBRE.Text, txtTipoEquipamento.Text, txtIdEquipamento.Text, txtNumEtiquetaIPQ.Text);

            DV = new DataView(DT);

            string rowfilter = " 1 = 1 ";

            if (txtMarca.Text != "") rowfilter += "and marca is not null and marca like '%" + txtMarca.Text + "%' ";
            if (txtModelo.Text != "") rowfilter += "and modelo is not null and modelo like '%" + txtModelo.Text + "%' ";

            //rowfilter += " ORDER BY idServico desc ";
            DV.RowFilter = rowfilter;
            DV.Sort = " idServico DESC ";
            GridView1.DataSource = DV;
            GridView1.DataBind();
            GERAL.GridViewExportUtil.Export("servicos.xls", GridView1);

        }

	}
}

