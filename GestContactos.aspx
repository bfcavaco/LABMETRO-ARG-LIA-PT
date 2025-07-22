<%@ Page language="c#" Codebehind="GestContactos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestContactos" %>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GestContactos</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<script language="javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnSearch").focus();
		}
	}

		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top" borderColor="#999999">
						<!-- body -->
						<table class="text_normal" id="tblPesquisa" runat="server" border="1">
							<tr>
								<td colSpan="7"><asp:label id="lblMessage" Runat="server" CssClass="lblMessage"></asp:label></td>
							</tr>
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colSpan="7">Empresa:</td>
							</tr>
						</table>
						<!--fim body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
