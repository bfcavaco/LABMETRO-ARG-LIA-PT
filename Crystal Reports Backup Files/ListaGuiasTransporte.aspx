<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@ Page language="c#" Codebehind="ListaGuiasTransporte.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ListaGuiasTransporte" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - Guias de Transporte ::</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout" onkeydown="CheckKey(event);">
		<script type="text/javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnPesquisa").focus();
		}
	}

		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu>
					</td>
					<td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" valign="top"><!-- body -->
						<asp:Label ID="lblMessage" Runat="server" CssClass="lblMessage"></asp:Label>
						<table class="text_normal" runat="server" id="tblPesquisa" cellSpacing="2" cellPadding="2"
							border="1" borderColor="darkgray">
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colspan="5">Pesquisar Guias Transporte</td>
							</tr>
							<tr>
								<td>Empresa<br />do Equipamento:</td>
								<td><asp:TextBox ID="txtPesquisaEmpresa" Runat="server" AutoPostBack="true" ontextchanged="txtPesquisaEmpresa_TextChanged"></asp:TextBox></td>
								<td>Nif:<asp:TextBox ID="txtPesquisaNif" Runat="server" AutoPostBack="true" ontextchanged="txtPesquisaNif_TextChanged"></asp:TextBox></td>
								<td><asp:Button class="button" id="btnPesquisaEmpresa" Runat="server" Text="ver Empresas" CssClass="button" onclick="btnPesquisaEmpresa_Click"></asp:Button></td>
							</tr>
							<tr>
								<td colspan="4"><asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa" AutoPostBack="true" onselectedindexchanged="ddEmpresa_SelectedIndexChanged"></asp:DropDownList></td>
							</tr>
							<tr>
								<td>Ref. Guia:</td>
								<td><asp:TextBox ID="txtRefGuia" Runat="server" AutoPostBack="false"></asp:TextBox></td>
								<td>Destinatario:</td>
								<td><asp:TextBox ID="txtDestinatario" Runat="server" AutoPostBack="false"></asp:TextBox></td>
							
							</tr>
							<tr>
								<td>Local Carregamento:</td>
								<td><asp:TextBox ID="txtLocalCarregamento" Runat="server" AutoPostBack="false"></asp:TextBox></td>
								<td>Local Descarregamento:</td>
								<td><asp:TextBox ID="txtLocalDesCarregamento" Runat="server" AutoPostBack="false"></asp:TextBox></td>
							</tr>
							<tr>
								<td colspan="4"><asp:Button class="button" id="btnPesquisa" Runat="server" Text='Pesquisar' CssClass="button" onclick="btnPesquisa_Click"></asp:Button></td>
							</tr>
						</table>
						<br />
						<br />
						<asp:DataGrid 
						ID="DGGT" 
						Runat="server" 
						AutoGenerateColumns="false" 
						AllowPaging="true"
						PageSize="25" 
						AllowSorting="True" 
						OnSortCommand="SortGrid" 
						OnPageIndexChanged="DoPaging" 
						DataKeyField="idGuiaTransporte">
							<Columns>
								<asp:BoundColumn DataField="refGuiaTransporte" SortExpression="refGuiaTransporte" HeaderText="Nº Guia."></asp:BoundColumn>
								<asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa"></asp:BoundColumn>
								<asp:BoundColumn DataField="dataGuia" SortExpression="dataGuia" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
								
								<asp:BoundColumn DataField="destinatario" SortExpression="destinatario" HeaderText="Destinatário"></asp:BoundColumn>
								<asp:BoundColumn DataField="localCarregamento" SortExpression="localCarregamento" HeaderText="Local<br />Carregamento"></asp:BoundColumn>
								<asp:BoundColumn DataField="localDescarregamento" SortExpression="localDescarregamento" HeaderText="Local<br />Descarregamento"></asp:BoundColumn>
								
								
								<asp:HyperLinkColumn HeaderText="Detalhes" Text="ver" DataNavigateUrlFormatString="FormGuiaTransporte.aspx?btn=DOC&id={0}"
									DataNavigateUrlField="idGuiaTransporte" target="_self">
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
