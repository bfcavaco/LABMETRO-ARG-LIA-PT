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
using System.Text.RegularExpressions; 

namespace LabMetro.BO
{
    public partial class BO_Marcas_Modelos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillMarcas();
                fillModelosManter(ddMarca.SelectedValue);
                fillModelosApagar(ddMarca.SelectedValue);
                fillMarcasApagar();
                fillMarcasManter(); 

                
            }
        }

        private void fillMarcas()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaMarcas();
            ddMarca.DataSource = DR;
            ddMarca.DataBind();
            ddMarca.Items.Insert(0, new ListItem("", ""));
            DR.Close();
            lista = null;

        }

        private void fillMarcasApagar()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaMarcas();
            ddMarcaApagar.DataSource = DR;
            ddMarcaApagar.DataBind();
           
            DR.Close();
            lista = null;

        }


        private void fillMarcasManter()
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaMarcas();
            ddMarcaManter.DataSource = DR;
            ddMarcaManter.DataBind();
            ddMarcaManter.Items.Insert(0, new ListItem("", ""));
            DR.Close();
            lista = null;

        }


        private void fillModelosManter(string idMarca)
        {
            
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaModelos(idMarca);
            ddModeloManter.DataSource = DR;
            ddModeloManter.DataBind();
            ddModeloManter.Items.Insert(0, new ListItem("null",""));

            DR.Close();
            lista = null;
            
        }

        private void fillModelosApagar(string idMarca)
        {
            DATA.ListasBD lista = new LabMetro.DATA.ListasBD();
            SqlDataReader DR = lista.DRListaModelos(idMarca);
            ddModeloApagar.DataSource = DR;
            ddModeloApagar.DataBind();

            DR.Close();
            lista = null;
            
        }

        protected void ddMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillModelosManter(ddMarca.SelectedValue);
            fillModelosApagar(ddMarca.SelectedValue); 
        }

        protected void btnModelos_Click(object sender, EventArgs e)
        {
            TrocarModelos();
            fillMarcas();
            fillModelosManter(ddMarca.SelectedValue);
            fillModelosApagar(ddMarca.SelectedValue);
            fillMarcasApagar();
            fillMarcasManter(); 
        }

        private void TrocarModelos()
        {


            string connectionString = (string )ConfigurationManager.AppSettings["ConnectionString2"];
    
			using(SqlConnection objConn =  new SqlConnection(connectionString))
			using(SqlCommand objCmd = new SqlCommand())
			{
				
				objCmd.Connection = objConn; 

				objConn.Open(); 

				using (SqlTransaction objTrans = objConn.BeginTransaction())
				{
					objCmd.Transaction =objTrans; 
					objCmd.CommandType = CommandType.Text; 
					try
					{
						objCmd.CommandText = "update equipamento set modelo = '" + ddModeloManter.SelectedValue + "' from equipamento e inner join tipoequipamento te on e.idTipoEquipamento = te.idTipoEquipamento inner join familia f on te.idfamilia = f.idFamilia where f.idGrandeza in ('ele','age','conc','cta') and e.marca = '" + ddMarca.SelectedValue + "' and e.modelo =  '" + ddModeloApagar.SelectedValue + "'";
						objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "delete from modelo where idModelo = " + ddModeloApagar.SelectedValue; 
						objCmd.ExecuteNonQuery(); 
						objTrans.Commit();
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DB;
						
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
							
						}
						GERAL.clsWriteError.WriteLog(ex); 
						lblMessage.Text +=ex.Message.ToString()+"<br />";
						
					}
				}	
			}
		}


        private void TrocarMarcas()
        {


            string connectionString = (string)ConfigurationManager.AppSettings["ConnectionString2"];

            using (SqlConnection objConn = new SqlConnection(connectionString))
            using (SqlCommand objCmd = new SqlCommand())
            {

                objCmd.Connection = objConn;

                objConn.Open();

                using (SqlTransaction objTrans = objConn.BeginTransaction())
                {
                    objCmd.Transaction = objTrans;
                    objCmd.CommandType = CommandType.Text;
                    try
                    {

                        objCmd.CommandText = "update modelo set idMarca = " + ddMarcaManter.SelectedValue + " where idMarca =" + ddMarcaApagar.SelectedValue; 

                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "update equipamento set marca = '" + ddMarcaManter.SelectedValue + "' from equipamento e inner join tipoequipamento te on e.idTipoEquipamento = te.idTipoEquipamento inner join familia f on te.idfamilia = f.idFamilia where f.idGrandeza in ('ele','age','conc','cta') and e.marca = '" + ddMarcaApagar.SelectedValue +"'"; 

                        objCmd.ExecuteNonQuery();

                        objCmd.CommandText = "delete from marca where idMarca = " + ddMarcaApagar.SelectedValue;
                        objCmd.ExecuteNonQuery();
                        objTrans.Commit();
                        lblMessage.Text = GERAL.clsGeral.ErrorMessage.MSG_DB;

                    }

                    catch (Exception ex)
                    {
                        try
                        {
                            objTrans.Rollback();
                        }
                        catch (Exception excep)
                        {
                            GERAL.clsWriteError.WriteLog(excep);
                            lblMessage.Text += excep.Message.ToString() + "<br />";

                        }
                        GERAL.clsWriteError.WriteLog(ex);
                        lblMessage.Text += ex.Message.ToString() + "<br />";

                    }
                }
            }
        }

        protected void btnMarcas_Click(object sender, EventArgs e)
        {
            TrocarMarcas();
            fillMarcas();
            fillModelosManter(ddMarca.SelectedValue);
            fillModelosApagar(ddMarca.SelectedValue);
            fillMarcasApagar();
            fillMarcasManter(); 
        }
		
    }
}
