using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Net.Http; //tive de add references para chamar o http client

using Newtonsoft.Json;
using LabMetro.GERAL;
using System.Text;
using System.IO;
using RestSharp;
using System.Net;

namespace LabMetro
{
    public partial class FormTesteOrcamento : System.Web.UI.Page
    {

        public FormTesteOrcamento()
        {
            _httpClient.BaseAddress = new Uri(url);
        }
        private static readonly HttpClient _httpClient = new HttpClient();
        // https://softwareengineering.stackexchange.com/questions/330364/should-we-create-a-new-single-instance-of-httpclient-for-all-requests

        //ISTO TEM DE IR PARA O WEB.CONFIG
        private static string url = "https://prod-00.northeurope.logic.azure.com:443/workflows/74ed197cabc845e28cac123c688130c6/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=vD_xUJrLTDZJw9AFB92OhxKliD0-2DEnNEyEuGje1iY";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void Unnamed1_Click(object sender, EventArgs e)

        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            //var client = new RestClient("https://prod-00.northeurope.logic.azure.com:443/workflows/74ed197cabc845e28cac123c688130c6/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=vD_xUJrLTDZJw9AFB92OhxKliD0-2DEnNEyEuGje1iY");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", "{\r\n  \r\n  \"requestID\": \"\",\r\n  \"resquestDate\": \"17/02/2020 09:30\",\r\n  \"accountGUID\": \"ecc8b2c0-bb41-ea11-a812-000d3ab40d73\",\r\n  \"contactFirstName\": \"daniela\",\r\n  \"contactLastName\": \"joao\",\r\n  \"contactEmail\": \"mail@mail.com\",\r\n  \"contactPhoneNumber\": \"\",\r\n  \"resquestDetails\": [\r\n    {\r\n      \"detailDescription\": \"sasd\",\r\n      \"quantity\": \"1\",\r\n      \"serviceType\": \"Calibração\"\r\n    },\r\n    {\r\n      \"detailDescription\": \"dasda\",\r\n      \"quantity\": \"1\",\r\n      \"serviceType\": \"Calibração\"\r\n    }\r\n  ]\r\n}", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Response.Write(response.Content);


            try
            {

                List<RequestDetails> det = new List<RequestDetails>() {
                new RequestDetails (){  detailDescription = "sasd", quantity="1",    serviceType= "Calibração"  },
                  new RequestDetails(){  detailDescription = "dasda", quantity="1",      serviceType = "Ensaio"  },
                };

                Contact obj = new Contact();
                obj.requestID = "";
                obj.resquestDate = "17/02/2020 09:30";
                obj.accountGUID = "ecc8b2c0-bb41-ea11-a812-000d3ab40d73";
                obj.contactFirstName = "joao";
                obj.contactLastName = "silva";
                obj.contactEmail = "mail@mail.com";
                obj.contactPhoneNumber = "";
                obj.resquestDetails = det;

                var json = JsonConvert.SerializeObject(obj);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("", stringContent);

                // var response = await _httpClient.PostAsync("", stringContent);


                if (response.StatusCode == System.Net.HttpStatusCode.OK)

                {

                    var stringResponse = response.Content.ReadAsStringAsync().Result;

                    var updateResponse = JsonConvert.DeserializeObject<wsResponse>(stringResponse);
                    lblMessage.Text = updateResponse.quoteID;

                }

                else

                {

                    //error

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = ex.ToString();
            }

        }
    }
}
