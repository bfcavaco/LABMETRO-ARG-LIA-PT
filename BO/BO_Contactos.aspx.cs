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
using LabMetro.REPORTS;
using LabMetro.GERAL;
using System.Configuration;


namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_Contactos.
	/// </summary>
	public partial class BO_Contactos : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btnSubmit;
		
		#region Web Form Designer generated code


		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			InitializeComponent2();
			base.OnInit(e);
		}
	
		private void InitializeComponent()
		{    	

		}
		private void InitializeComponent2()
		{    
			btnPesquisa.Click += new System.EventHandler(btnPesquisa_Click);
			btnTrocar.Click += new System.EventHandler(btnTrocar_Click);
			btnPesquisaEmpresa.Click += new System.EventHandler(btnPesquisaEmpresa_Click);
		}

		#endregion
		

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text =""; 
			
			btnTrocar.Attributes.Add("onClick","javascript:if(confirm('Confirma troca de contactos?')== false) return false;");

			if(!Page.IsPostBack)
			{
				ViewState["sortField"] = "nome";
				ViewState["sortDirection"] = "ASC";
				
			}
		}

	

		
		private void BindDDContactos()
		{
			
			if(ddEmpresa.SelectedValue=="")
			{
				lblMessage.Text =Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}
				 
			string strSQL = "SELECT idContacto, (CAST(idContacto as varchar) +' - ' + Cast(nome as varchar)) as descricao FROM contacto WHERE nome like'%"+txtNomeContacto.Text+"%' "; 
			if(ddEmpresa.SelectedValue!="")
			{
				strSQL +=" AND idEmpresa = " +ddEmpresa.SelectedValue;
			}
			strSQL += " ORDER BY NOME"; 
			

			SqlDataReader dr = GERAL.clsDataAccess.ExecuteDR(strSQL); 
			ddContactoManter.DataTextField = "descricao";
			ddContactoManter.DataValueField="idContacto"; 
			ddContactoManter.DataSource = dr; 
			ddContactoManter.DataBind(); 		
			ddContactoManter.Items.Insert(0,new ListItem("",""));
            dr.Close();
		}

		private string sede(bool b)
		{
			if (b==true) return " ** Sede"; 
			else return ""; 
		
		}

		private void fillDDEmpresa()
		{

			DATA.EmpresasBD empresa = new LabMetro.DATA.EmpresasBD(); 
			DataTable DT =  empresa.DTEmpresas(txtPesquisaEmpresa.Text,"","","","","","","",""); 
            
			DataColumn dc = new DataColumn(); 
			
			dc.ColumnName="Nome_"; 
			
			DT.Columns.Add(dc); 
			foreach (DataRow dr in DT.Rows)
			{
				dr["Nome_"] = dr["nome"].ToString() + sede(System.Convert.ToBoolean(dr["sede"]));
			}
			 

			DataView DV = new DataView(DT);


			string strSort = "nome ASC"; 
			DV.Sort = strSort; 
			
			ddEmpresa.DataSource = DV; ; 
			ddEmpresa.DataBind();

			empresa = null; 

			if((txtPesquisaEmpresa.Text == ""))ddEmpresa.Items.Insert(0, new ListItem("Todas",""));
		}

		private void BindGrid()
		{  
			if(ddEmpresa.SelectedValue=="")
			{
				lblMessage.Text =Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return; 
			}

			DATA.ContactosBD contactos = new LabMetro.DATA.ContactosBD();
			try
			{
           
				//se quero receber todos, tenho de meter o ZERO como param
				DataTable DT = contactos.DTFillContacts(ddEmpresa.SelectedValue,txtNomeContacto.Text,"0",txtEmail.Text); 
				DataView DV = new DataView(DT); 
				DV.Sort = ViewState["sortField"].ToString()+ " " + ViewState["sortDirection"]; 
	        
				DGContactos.DataSource = DV; 
				DGContactos.DataBind(); 
				
				
				if(DV.Table.Rows.Count > 0)
				{
					DGContactos.Visible=true;
				}
				else
				{
					DGContactos.Dispose();
					DGContactos.Visible=false; 
					lblMessage.Text= GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS; 
					
				}
			}
			catch(Exception ex)
			{

				DGContactos.Dispose();
				DGContactos.Visible=false; 
				lblMessage.Text= ex.ToString() + "-" + GERAL.clsGeral.ErrorMessage.MSG_NO_RESULTS;
			}
			
			contactos = null; 
		}
		

		


		private void btnPesquisa_Click(object sender, System.EventArgs e)
		{
			DGContactos.CurrentPageIndex=0; 
			BindGrid(); 
			BindDDContactos(); 
		}
		
		public void doPaging(Object s,DataGridPageChangedEventArgs e)
		{
			DGContactos.CurrentPageIndex = e.NewPageIndex; 
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
			BindGrid(); 
		}


		
		private void btnPesquisaEmpresa_Click(object sender, System.EventArgs e)
		{
			fillDDEmpresa(); 
		}
		
		private void btnTrocar_Click(object sender, System.EventArgs e)
		{
			string strIds =""; 
			foreach(DataGridItem dgi in DGContactos.Items) 
			{ 
				CheckBox myCheckBox =(CheckBox)dgi.Cells[0].FindControl("checkbox"); 
				
				if(myCheckBox.Checked == true)
				{
					
					strIds+= DGContactos.DataKeys[dgi.ItemIndex].ToString();
					strIds+=",";
				}   
			}
			strIds = strIds.TrimEnd(",".ToCharArray());//tem de ser senao manda um vazio no ultimo item

			if(ddContactoManter.SelectedValue =="" || strIds.Length ==0)
			{
				lblMessage.Text =Resources.Resource.MSG_SELECCIONE_EMPRESA; 
				return;
			}

			if(trocarContactos(ddContactoManter.SelectedValue,strIds)) 
			{
				BindGrid();  
				BindDDContactos(); 
			}
			
		}


		private bool trocarContactos(string idContactoManter, string idsContactosApagar)
		{
			string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
			using (SqlConnection objConn = new SqlConnection(connectionString)) 
			using (SqlCommand objCmd = new SqlCommand())
			{
				objCmd.Connection = objConn; 
				objCmd.CommandTimeout = GERAL.clsDataAccess.iCommandTimeOut; 
			
				objConn.Open(); 
				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					
					try


					{

						//VALIDAR QUE SO TROQUEM CONTACTOS DA MESMA EMPRESA
						//--as tabelas no from nao precisam de estar directamente relacionados com os valores do uptade
						//-- podem ser uma especie de subquery, e os valores veem à mesma em params.
						objCmd.CommandText="UPDATE orcamento SET orcamento.idContacto = "+idContactoManter+" FROM contacto cM,contacto cA WHERE orcamento.idContacto IN ("+idsContactosApagar+") AND cA.idContacto IN ("+idsContactosApagar+") AND cM.idContacto ="+idContactoManter+ " AND cM.idEmpresa = cA.idEmpresa"; 

						objCmd.ExecuteNonQuery();


                        //VALIDAR QUE SO TROQUEM CONTACTOS DA MESMA EMPRESA
                        //--as tabelas no from nao precisam de estar directamente relacionados com os valores do uptade
                        //-- podem ser uma especie de subquery, e os valores veem à mesma em params.
                        objCmd.CommandText = "UPDATE taxaservico SET taxaservico.idContacto = " + idContactoManter + " FROM contacto cM,contacto cA WHERE taxaservico.idContacto IN (" + idsContactosApagar + ") AND cA.idContacto IN (" + idsContactosApagar + ") AND cM.idContacto =" + idContactoManter + " AND cM.idEmpresa = cA.idEmpresa";

                        objCmd.ExecuteNonQuery();

                        //VALIDAR QUE SO TROQUEM CONTACTOS DA MESMA EMPRESA
                        objCmd.CommandText = "UPDATE FaxEquipamentosCalibrados SET FaxEquipamentosCalibrados.idContacto = "+idContactoManter+" FROM contacto cM,contacto cA WHERE FaxEquipamentosCalibrados.idContacto IN ("+idsContactosApagar+") AND cA.idContacto IN ("+idsContactosApagar+") AND cM.idContacto ="+idContactoManter+ " AND cM.idEmpresa = cA.idEmpresa"; 

						objCmd.ExecuteNonQuery();

						int numIds = idsContactosApagar.Split(",".ToCharArray()).Length; 

						
						//se a tabela contacto tiver ligações às tabelas orcamento e faxequipamentoscalibrados, nao deixa apagar foreign keys aqui se nao tiver trocado os ids anteriormente. -- verificar que estas ligações existem. mas caso alguem as apague... que fazer?
						//objCmd.CommandText = "DELETE FROM CONTACTO WHERE idContacto in ("+idsContactosApagar+")"; 

						//para não correr esse risco:
						objCmd.CommandText = "DELETE FROM CONTACTO WHERE idContacto IN ("+idsContactosApagar+")	and not exists (Select idContacto from orcamento where idContacto IN ("+idsContactosApagar+")) and not exists (Select idContacto from FaxEquipamentosCalibrados where idContacto IN ("+idsContactosApagar+"))"; 	
						
						if(objCmd.ExecuteNonQuery()!=numIds) throw new Exception("Contactos não foram trocados/apagados."); 

						
						objTrans.Commit();

                        lblMessage.Text += GERAL.clsGeral.ErrorMessage.MSG_TROCA_EFECTUADA;
						return true;


					}
					catch(Exception ex)
					{ 	
						try
						{	
							objTrans.Rollback();
						}
						catch(Exception excep)
						{
							GERAL.clsWriteError.WriteLog(excep); 
							lblMessage.Text +=excep.Message.ToString()+"<br />";
							return false;
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text +=ex.Message.ToString()+"<br />";
						return false;
					}
				}
			}
		}

        protected System.Drawing.Color ConvertColor(int i)
        {
            System.Drawing.ColorConverter colConvert = new ColorConverter();

            System.Drawing.Color colorName;
            switch (i)
            {
                case 0:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#CC0000");//(System.Drawing.Color)colConvert.ConvertFromString("Red");
                    break;
                case 1:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");//(System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
                default:
                    colorName = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");//(System.Drawing.Color)colConvert.ConvertFromString("White");
                    break;
            }

            return colorName;
        }

	}
}
