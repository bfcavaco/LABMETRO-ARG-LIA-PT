using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace LabMetro.BO
{
	/// <summary>
	/// Summary description for BO_ApagarCertificado.
	/// </summary>
	public partial class BO_ApagarCertificado : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			//select idServico, refServico, idEstadoServico, idEstadoCertificado from servico where refServico IN ('CTEM2928/07' ,'CTEM3008/07')
			//fazer esta query e verificar qual o estado do serviço. 
			//se for 12,UO 13, OU 19,  passo outra vez para 6
			//SE JA ESTIVER ENTREGUE COM CERTIF OU SEMELHANTE, PASSO PARA ENTREGUE. 
//
//			select idServico, refServico, idEstadoServico, idEstadoCertificado from servico where refServico IN 
//('CTEM2928/07' ,'CTEM3008/07','chum2060/07')
//
//
//UPDATE SERVICO SET idEstadoServico = 6, idEstadoCertificado = 1 where
// idServico in (176945,
//177964,
//178702)
//
//select * from certificado where idServico in (176945,
//177964,
//178702)
//delete from certificado where idServico in (176945,
//177964,
//178702)
//atencao que tenho de ver o que apago da tabela certificado, se for o primeiro, o aditamento, etc.

			//no fim, apagar os ficheiros da pasta de certificados finais.

			//tenho de apagar tb do historico e pelos vistos tenho de fazer reset ao utilizador que efectuou o serviço (verificar). possivelmente!!!! sim





		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
