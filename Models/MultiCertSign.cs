using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Labmetro
{
	public class MultiCertSign
	{
		public class Document
		{
			public DocumentRequest documentRequest { get; set; }
			public SignatureInfo signatureInfo { get; set; }
		}

		public class DocumentRequest
		{
			public string base64Content { get; set; }
			public string contentType { get; set; }
			public string externalReference { get; set; }
			public string name { get; set; }
		}

		public class Root
		{
			public string description { get; set; }
			public string externalReference { get; set; }
			public List<Document> documents { get; set; }
		}

		public class SignatureInfo
		{
			public string signatureType { get; set; }
			public bool applyTimestamp { get; set; }
			public string location { get; set; }
			public string reason { get; set; }
			public VisibleSignatureInfo visibleSignatureInfo { get; set; }
		}

		public class VisibleSignatureInfo
		{
			public string locationType { get; set; }
			public string locationValue { get; set; }
			public string area { get; set; }
			public string font { get; set; }
			public string oneOffTemplate { get; set; }
			public string templateType { get; set; }
		}

		/*
			Esta função serve para fazer o pedido à REST API.
			Para fazer o pedido é necessário o utilizador ter o token de acesso e passar mais alguns parâmetros no Header(Content-type, Auth, etc).
			O body (JSON) do pedido é composto pelas classes Document, DocumentRequest, SigntureInfo, VisibleSigantureInfo e a classe Pai (ROOT).
			Returns signed Base64String file.
		*/
		public static async Task<string> DigitalSignMulticert(string filePath, string filename)
		{
			try
			{
				string token = await Task.Run(() => Login());
				String base64 = Convert.ToBase64String(File.ReadAllBytes(filePath));

				Document document = new Document()
				{
					documentRequest = new DocumentRequest()
					{
						base64Content = base64,
						contentType = "application/pdf",
						externalReference = "Ext_Ref4132asigyuhasjiddio1asbhdjio81113402d",
						name = filename,
					},
					signatureInfo = new SignatureInfo()
					{
						signatureType = "PAdES",
						applyTimestamp = true,
						location = "Oeiras",
						reason = "API",
						visibleSignatureInfo = new VisibleSignatureInfo()
						{
							locationType = "COORDINATE",
							locationValue = "page=1,x=140,y=35",
							area = "width=100,height=30",
							font = "color=#000000,size=8,name=arial",
							oneOffTemplate = "Digitally signed by $certificate.getCommonName()\nDate: " + DateTime.Now.ToString("yyyy/MM/dd") + "\nTime: " + DateTime.Now.ToString("HH:mm") + " UTC",
							templateType = "ONE_OFF",
						},
					},
				};

				Root root = new Root()
				{
					description = "Some context.",
					externalReference = "Ext_Ref" + filename + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HH:mm:ss"),
					documents = new List<Document>(),
				};

				root.documents.Add(document);

				string url = "http://10.92.150.141:8081/signstash/einvoice-integration-client-ws/api/v0/document/sign/base64//";
				RestClient client = new RestClient(url + "/");

				RestRequest request = new RestRequest() { Method = Method.Post };
				request.AddHeader("Content-Type", "application/json");
				request.AddHeader("Accept", "*/*");
				request.AddHeader("Authorization", "Bearer " + token);

				var body = JsonConvert.SerializeObject(root);
				request.AddParameter("application/json", body, ParameterType.RequestBody);
				RestResponse response = await client.ExecuteAsync(request);
				var obj = response.Content == null ? null : JObject.Parse(response.Content);

				return response.Content == null ? null : response.Content.ToString().Contains("error") ? null : (string)obj["documentList"][0]["signedContent"];
			}
			catch (Exception ex)
            {
				return "ERRO:\n" + ex.Message;
            }
		}

		/*
			Função de autenticação que serve para gerar o token de acesso.
			Retorna o token de acesso como String.
		*/
		public static string Login()
		{
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            String username = "PRT.505767457-PRT.500140022-f2d578b3-a3ca-4a0f-be74-fda268406404";
            String password = "9PmQrEJVfKd8%%n6l0RJ";
            String encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msignstash.multicert.com/oauth2/authorization-server/oauth/token?grant_type=client_credentials");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + encoded);
            request.PreAuthenticate = true;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(responseString);
            string token = data["access_token"].Value<string>();

            return token;
        }

		//Create file from Base64String
		public static void Base64ToPdf(string base64BinaryStr, string strFile)
		{
			FileStream stream = File.Create(strFile);
			Byte[] byteArray = Convert.FromBase64String(base64BinaryStr);
			stream.Write(byteArray, 0, byteArray.Length);
			stream.Close();
		}
	}
}
