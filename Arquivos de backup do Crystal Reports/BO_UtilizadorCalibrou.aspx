<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<%@ Page language="c#" Codebehind="BO_UtilizadorCalibrou.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_UtilizadorCalibrou" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>:: LabMetro - BO Utilizador Calibrou ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="..\Styles.css" type="text/css" rel="stylesheet">
		<style type="text/css">

		
#footer { PADDING-RIGHT: 15px; BORDER-TOP: #cccccc 1px solid; PADDING-LEFT: 10px; FONT-SIZE: 10px; COLOR: #666666; LINE-HEIGHT: 17px; PADDING-TOP: 5px; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }

		
#footer A { FONT-SIZE: 11px; COLOR: #666666; TEXT-DECORATION: none }

		
#footer A:hover { TEXT-DECORATION: underline }

		
		</style>
	</head>
	<body onkeydown="CheckKey(event);" ms_positioning="GridLayout">
		<script type="text/javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnSubmit").focus();
		}
	}

		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMainBO">
				<tr>
					<td><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu><br />
						<br />
					</td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
						<%=Resources.Resource.FraseAvisoBOUtilCalib%>
						<br /><br />
						<asp:Label id="lblMessage" runat="server" CssClass="errorMessage"></asp:Label><br />
						<br />
						<%=Resources.Resource.ReferenciaServicoCompleta %>:<br /><asp:textbox id="txtRefServico" runat="server"></asp:textbox>
						<br />
						<%=Resources.Resource.CalibradoPor %>:<br />
						<asp:dropdownlist id="ddTecnicoLaboratorio" Runat="server" DataValueField="idFuncionario" DataTextField="nomeAbreviado"></asp:dropdownlist>
						<br /><br />
						<asp:Button class="button" id="btnSubmit" runat="server" Text="<%$Resources:Resource, Actualizar %>" onclick="btnSubmit_Click"></asp:button></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
						<!-- FIM body --> <!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
