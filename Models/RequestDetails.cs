using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RequestDetails
/// </summary>
/// 

namespace LabMetro.GERAL
{
    
public class RequestDetails
{
    public RequestDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string detailDescription { get; set; } 
    // varchar	445(máximo 445)	Descrição da linha do pedido que o cliente fez no portal
    public string quantity { get; set; } 
    //varchar	18 (máximo 18)	quantidade pedida pelo cliente, sempre com duas casas decimais e com separador decimal ponto(.)
    public string serviceType { get; set; }
    //varchar	50 (máximo 50)	Tipo de serviço escolhido pelo cliente(ensaio, calibração, etc)

}
}
