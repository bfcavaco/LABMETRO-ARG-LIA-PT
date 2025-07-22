using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace LabMetro
{
    public partial class ListaVDs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //Lista de duplicados eu ainda tenho de fazer algumas correções no stored procedure, mas em termos de input serão os seguintes dados:
            //http://bdedifica.isq.pt/ISQ_vendas/Reports/VendaADinheiro_Lista.aspx?numero_responsavel=5862&dataini=01-11-2006&datafim=23-11-2011

            //Para impressão da listagem das Vds por técnico e data a enviar semanalmente para a DAF,  devemos passar os seguintes parametros:
            //Numero_responsavel – Numero de funcionário que emitiu as VDS
            //Dataini – Data de Inicio
            //Datafim – Data de fim
            //URL de exemplo:
            //http://bdedifica.isq.pt/isq_Vendas/reports/vendasResponsavel.aspx?numero_responsavel=9999&dataini=01-01-2011&datafim=01-01-2012 

            
            string url = ""; 
            
            if(rblTipoLista.SelectedValue=="1") //duplicados



            {
                
                //duplicados




                    url = "http://sgao.isq.pt/Reports/VendaADinheiro_Lista3.aspx?numero_responsavel=" + txtNumFuncionario.Text + "&dataini=" + txtDataInicio.Text + "&datafim=" + txtDataFim.Text;
                
            }
            else //vds
            {
               
           
                url = "http://sgao.isq.pt/Reports/ListagemVendas.aspx?login="+txtNumFuncionario.Text+"&datainicial="+txtDataInicio.Text+"&datafinal="+txtDataFim.Text; 


            }
            
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=JavaScript>");
            strScript.Append("window.open('" + url + "','new_Win','toolbar=0,menubar=0,resizable=1');");
            strScript.Append("</script>");
            //RegisterClientScriptBlock("imprimefactura", strScript.ToString());
            ClientScript.RegisterClientScriptBlock(GetType(), "imprimefactura", strScript.ToString());
        }
    }
}

