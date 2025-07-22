<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@ Page language="c#" Codebehind="ConsultaEstadoServico.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ConsultaEstadoServico" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>::LabMetro - Consulta Ref.Calibração::</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout" onkeydown="CheckKey(event);">
	<script language="javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnSubmit").focus();
		}
	}

	</script>
		<form id="Form2" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top" height="20"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top">
						<!-- body -->
							<table class="text_normal" id="tblPesquisa" borderColor="darkgray" width="100%" border="1"
							cellpadding="2" cellspacing="1">
							<tr>
								<td colSpan="4" class="tblTituloCinzaClaroLetraBranca">Pesquisa por Referência de Calibração</td>
							</tr>
							
							<tr>
								<td colSpan="4" bgcolor="#d3d3d3">
								<asp:Label ID="lblMessage" Runat="server" CssClass="lblMessage"></asp:Label><br><br><br>
									Ref. Serviço (completa):
									<asp:TextBox ID="txtRefServico" Runat="server"></asp:TextBox>
									<asp:Button Runat="server" ID="btnSubmit" Text="Pesquisar" CssClass="button_red" onclick="btnSubmit_Click"></asp:Button>
								</td>
							</tr>
							<tr>
								<td colSpan="4">
									<asp:Label ID="lblDetalhesServico" Runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td colSpan="4">
									<asp:datagrid id="dgHistorico" Runat="server" CssClass="DG_cinzento" AlternatingItemStyle-BackColor="#d3d3d3"
										DataKeyField="idServico" AutoGenerateColumns="false" GridLines="horizontal" BorderColor="#FFFFFF"
										AllowSorting="false" ShowFooter="false" BorderWidth="2" Width=100%>
										<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
										<Columns>
											<asp:BoundColumn HeaderText="Estado" DataField="estado"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="Data Estado" DataField="dataEstado"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="Funcionário" DataField="userEstado"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
						</table>
						<!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
