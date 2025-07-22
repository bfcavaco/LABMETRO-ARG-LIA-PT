<%@ Page language="c#" Codebehind="DadosSap.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.DadosSap" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DadosSap</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid 
			ID="DG" 
			Runat="server" 
			AutoGenerateColumns="True" 
			ShowHeader="True" 
			AllowPaging=False 
			AllowSorting=True 
			OnSortCommand="sortGrid"></asp:DataGrid>
			
		</form>
	</body>
</HTML>
