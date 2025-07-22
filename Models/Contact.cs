using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace LabMetro.GERAL
{
    /// <summary>
    /// Summary description for Contact
    /// </summary>
    public class Contact
    {
        public Contact()
        {
            //
            // TODO: Add constructor logic here
            //

        }


        public string requestID { get; set; }
        public string resquestDate { get; set; } //varchar 16 (exatamente 16)
        public string accountGUID { get; set; }// 36 (exatamente 36)
        public string contactFirstName { get; set; } // varchar 50(máximo 50)
        public string contactLastName { get; set; } //varchar 50(máximo 50)
        public string contactEmail { get; set; }//varchar 100(máximo 100)
        public string contactPhoneNumber { get; set; } //varchar 30(máximo 30)
        public List<RequestDetails> resquestDetails { get; set; }

    }


}

