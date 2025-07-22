<%@ Page language="c#" Codebehind="AdminCertificados.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.AdminCertificados" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Admin Certificados Ficheiros</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid ID="dgErros" Runat="server" AutoGenerateColumns="True"></asp:DataGrid>
			<asp:Button class="button" ID="btnSubmit" Runat="server" Text="Actualizar BD com dados dos Ficheiros" onclick="btnSubmit_Click"></asp:Button><br />
			<br />
			<asp:Button class="button" ID="btnVerMaus" Runat="server" Text="Ver Erros" onclick="btnVerMaus_Click"></asp:Button><br />
			<br />
			<asp:Button class="button" ID="Button1" Runat="server" Text="Corrigir" onclick="Button1_Click"></asp:Button><br />
			<br />
            <asp:TextBox runat="server" ID="txtAno" MaxLength="4" ></asp:TextBox>
            <asp:Button class="button" ID="Button2" Runat="server" Text="Mover ANO" onclick="Button2_Click"></asp:Button><br />
			<br />
			<asp:Button class="button" ID="Button3" Runat="server" Text="desarquivar" onclick="Buttonvisteon_Click" Enabled="true"></asp:Button><br />
			<br />
			<asp:Label ID="lblMessage" Runat="server" ></asp:Label>
		</form>
	</body>
</HTML>
