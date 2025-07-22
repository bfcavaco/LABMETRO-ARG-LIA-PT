<%@ Page language="c#" Codebehind="BODefault.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BODefault" %>
<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BODefault</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href=../Styles.css type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
	<div class="lblVermelhaBO">Backoffice de Administração do Labmetro</div>
		<form id="Form1" method="post" runat="server">
		<menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu>
		</form>
	</body>
</HTML>
