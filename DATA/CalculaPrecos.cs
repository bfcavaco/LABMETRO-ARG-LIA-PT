using System;
using System.Data.SqlClient; 
using System.Configuration;
using System.Data; 

namespace LabMetro.DATA
{
	/// <summary>
	/// Summary description for CalculaPrecos.
	/// </summary>
    /// 
    public class DetalhesServico
    {
       
        
    }

	public class CalculaPrecos
	{
		public CalculaPrecos()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
        //adaptado da funccao calculatePrice da pasta de ensaio.
        
            public double calculaPreco(string idServico)
            {

            //            //vai buscar detalhes do serviço
            //            LabMetro.DATA.ServicoBD servico = new LabMetro.DATA.ServicoBD(); 
            //            DataTable dt = servico.DTGetServicoPrecoDetails(idServico); 

            //            string idTipoPreco = dt.Rows[0]["idTipoPreco"].ToString();
            //string grandeza = dt.Rows[0]["grandeza"].ToString();                 
            //            string idTipoEquipamento = dt.Rows[0]["idTipoEquipamento"].ToString(); 
            //            string idTipoServico = dt.Rows[0]["idTipoServico"].ToString(); 
            //            string quantidade = dt.Rows[0]["quantidade"].ToString();
            //bool bCalibExterna = (bool)dt.Rows[0]["bCalibExterna"];
            //string unidadeAlcance = dt.Rows[0]["unidadeAlcance"].ToString();
            //string alcanceInf = dt.Rows[0]["alcanceInf"].ToString();
            //string alcanceSup = dt.Rows[0]["alcanceSup"].ToString();
            //string classe = dt.Rows[0]["idClasse"].ToString();
            //string modelo = dt.Rows[0]["modelo"].ToString();
            //string bFormula = dt.Rows[0]["bFormula"].ToString();

            //            double precoServico = 0;  //se conseguir calcular algum preco, muda, senao, fica a 0

            //            //retorna algo a dizer se é calibração interna ou externa, para móvel, não ha nada
            //            //movel, nao sei onde se vê; por isso so vou distinguir entre externa e interna
            //            string tipoPr = "preco";  

            //            if(bCalibExterna == true)
            //            {
            //                tipoPr = "precoExterior"; 
            //            }


            //            DATA.PrecosBD preco = new LabMetro.DATA.PrecosBD(); 

            //            switch(idTipoPreco)
            //            {
            //                case "1": //preço directo por tipo de equipamento.

            //                    precoServico = preco.getPriceByTipoEquipamento(idTipoEquipamento, idTipoServico,tipoPr); 
            //                    break; 

            //                case "2":  //tipo de equipamento e quantidade

            //                    //se nao tiver quantidade, nao calcula o preço
            //                    if(quantidade != "")
            //                    {
            //                        precoServico = preco.getPriceByTipoEquipamentoQuantidade(idTipoEquipamento, idTipoServico,quantidade,tipoPr);
            //                    }

            //                    break;                 

            //                case "3": //tipo de equipamento e alcance

            //                    precoServico = preco.getPriceByTipoEquipamentoAlcanceSimples(idTipoEquipamento,idTipoServico,unidadeAlcance, alcanceInf, alcanceSup, tipoPr); 
            //                    break; 


            //                case "4": //tipo de equipamento + alcance + numPontos/pares

            //                    //ir buscar as linhas de servicos para um servico e pôr nesta datatable
            //		LabMetro.DATA.ServicoBD s = new LabMetro.DATA.ServicoBD(); 
            //		DataTable dtLinhasServico = s.dtLinhasServicoByIdServico(idServico);
            //                    //dtLinhasServico = (DataTable)ViewState["dtLinhasServico"]; 
            //                    DataView dv = new DataView(dtLinhasServico); 

            //                    foreach(DataRowView drv in dv)
            //                    {
            //                        string strAlcance=""; 
            //                        string strPontos =""; 
            //                        string numPecas = "1"; 
            //                        try
            //                        {
            //                            //aqui recebe os pontos do datagrid. //e pode receber varias linhas
            //                            string p = drv["pontosCalib"].ToString(); 
            //                            numPecas = drv["numPecas"].ToString() ; 
            //                            int strHum = p.IndexOf("%"); 
            //                            int strTemp = p.IndexOf("ºC"); 
            //                            int strMisto = p.IndexOf("%(ºC)"); 

            //                            int indexOf = 0;  

            //                            if(strMisto >=0) //alcance Misto
            //                            {
            //                                strAlcance="M"; 
            //                                indexOf = strMisto; 
            //                            }
            //                            else if(strHum >=0) //alcance Humidade
            //                            {
            //                                strAlcance = "H"; 
            //                                indexOf = strHum; 
            //                            }
            //                            else if(strTemp >=0) //alcance Temperatura
            //                            {
            //                                strAlcance = "T"; 
            //                                indexOf = strTemp; 
            //                            }

            //                            //remover o alcance para receber a string so com pontos/pares
            //                            strPontos = p.Substring(0,indexOf);
            //                        }

            //                        catch(Exception ex)
            //                        {
            //                            GERAL.clsWriteError.WriteLog(ex.ToString()); 
            //                        }

            //                        if(strPontos!="")
            //                        {
            //                            precoServico +=  preco.getPriceByTipoEquipamentoAlcancesMistos(idTipoEquipamento,idTipoServico,strPontos,strAlcance,tipoPr,numPecas,bFormula);
            //                            //Response.Write(precoServico); 
            //                        }
            //                    }   

            //                    break; 

            //                case "5": //tipo equipa + alcance + classe

            //                    precoServico = preco.getPriceByTipoEquipamentoAlcanceSimplesClasse(idTipoEquipamento,idTipoServico, unidadeAlcance, alcanceInf, alcanceSup, classe, tipoPr); 
            //                    break; 

            //                case "6"://marca-modelo 
            //                    if(grandeza.ToString() == "ELE") //preços por marca modelo

            //                        //so para electricos

            //			//jan.2008 foi mudado para CTA tambem terem marcas e modelos mas 
            //			//o calculo do preço nao foi alterado ate agora.
            //			//alias, isto nunca foi testado (ou utilizado) desde a implementação, embore o calculo dos preços funcione nalguns caso... 
            //                    {
            //                        precoServico = preco.getPriceByIdModelo(idTipoEquipamento, modelo,idTipoServico,tipoPr); 

            //                    }
            //                    break;                           
            //            }

            return 0; // precoServico; 
            }
        }
}
