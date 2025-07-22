using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for wsResponse
/// </summary>
/// 
namespace LabMetro.GERAL
{

    public class wsResponse
    {
        public wsResponse()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string lApp { get; set; }
        public string message { get; set; }
        public string quoteID { get; set; }

        //Em termos de respostas o ws devolve dois códigos de resposta: 200 (ok) 500 (erro) seguido de um body(é sempre devolvido neste formato) :


        //"lApp": "EXTCDSCreateQuoteFromRequest/08586187886775206802925931137CU32",

        //"message": "Quote created successfully [MET-20-02087.0]",

        //"quoteID": "MET-20-02087.0"

    }
}