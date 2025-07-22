<%@ Page language="c#" Codebehind="ListaObras.aspx.cs" AutoEventWireup="false" Inherits="LabMetro.ListaObras" %>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - Obras ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout" onkeydown="CheckKey(event);">
		<script language="javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnPesquisa").focus();
		}
	}

		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMain" width="800">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu>
					</td>
					<td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" valign="top"><!-- body -->
						<asp:Label ID="lblMessage" Runat="server" CssClass="lblMessage"></asp:Label>
						 <table class="text_normal" id="tblPesquisa" borderColor="darkgray" width="100%" border="1">
							<tr>
								<td>Ref.Obra:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtRefObra" Runat="server"></asp:TextBox></td>
								<td>Empresa:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox></td>
							</tr>
							<tr>
								<td>Estado da Obra:&nbsp;&nbsp;<asp:DropDownList ID="ddEstadoObra" Runat="server" DataTextField="descricao" DataValueField="ident"></asp:DropDownList></td>
								<td>NIF:&nbsp;&nbsp;<asp:TextBox ID="txtNif" runat="server"></asp:TextBox></td>
							<tr>
							<tr>
								<td>
									<asp:Button ID="btnPesquisa" Runat="server" Text="Pesquisar" CssClass="button"></asp:Button></td>
								<td></td>
							</tr>
						</table>
						<br>
						<br>
						<asp:DataGrid CssClass="DG_cinzento" ID="DG" Runat="server" AutoGenerateColumns="false" AllowPaging="true"
							PageSize="25" AllowSorting="True" OnSortCommand="SortGrid" OnPageIndexChanged="DoPaging" PagerStyle-Mode="NumericPages"
							BorderColor="#FFFFFF" GridLines="horizontal" BorderWidth="2" DataKeyField="idObra" AlternatingItemStyle-BackColor="WhiteSmoke">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="refObra" SortExpression="refObra" HeaderText="Ref.Obra"></asp:BoundColumn>
								<asp:BoundColumn DataField="estadoObra" SortExpression="estadoObra" HeaderText="Estado"></asp:BoundColumn>
								<asp:BoundColumn DataField="dtAbertura" SortExpression="dtAbertura" HeaderText="Data Abert." DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
								<asp:BoundColumn DataField="dtFecho" SortExpression="dtFecho" HeaderText="Data Fecho" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
								<asp:BoundColumn DataField="observacoes" SortExpression="observacoes" HeaderText="Obs."></asp:BoundColumn>
								<asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="1ºa emp." ItemStyle-Width="300px"></asp:BoundColumn>
								<asp:HyperLinkColumn HeaderText="Detalhes" Text="ver" DataNavigateUrlFormatString="FormObra.aspx?btn=FAC&id={0}"
									DataNavigateUrlField="idObra" target="_self">
									<ItemStyle HorizontalAlign="center"></ItemStyle>
								</asp:HyperLinkColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
