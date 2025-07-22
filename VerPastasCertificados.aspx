<%@ Page language="c#" Codebehind="VerPastasCertificados.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.VerPastasCertificados" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
	<head>
		<title>CERTIFICADOS</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
			<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</head>
	<body>
		<form id="DocManager" method="post" encType="multipart/form-data" runat="server">
			<table cellpadding="10" cellspacing="0" border="0">
				<tr>
					<td>
						<asp:label id="ErrorMessages" runat="server" EnableViewState="False"></asp:label>
						<asp:label id="Heading" runat="server" EnableViewState="False"></asp:label>
						<asp:linkbutton Cssclass="DocMgrAdminLInk" id="CreateFolderLB" runat="server" EnableViewState="False" Visible="False" CommandArgument="CreateFolder" OnCommand="ShowAdminField"></asp:linkbutton><asp:linkbutton id="UploadFileLB" runat="server" Cssclass="DocMgrAdminLInk" EnableViewState="False" Visible="False" CommandArgument="UploadFile" OnCommand="ShowAdminField"></asp:linkbutton><br />
						<asp:label id="txtFolders" runat="server" EnableViewState="False"></asp:label>
						<asp:label id="txtFiles" runat="server" EnableViewState="False"></asp:label>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>