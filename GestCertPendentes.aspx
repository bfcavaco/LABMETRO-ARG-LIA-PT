<%@ Page language="c#" Codebehind="GestCertPendentes.aspx.cs" AutoEventWireup="false" Inherits="LabMetro.GestCertPendentes" EnableViewState="True" %>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - Listar Documentos para Aprovação ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body onbeforeunload="doHourglass();" onunload="doHourglass();" MS_POSITIONING="GridLayout">
		<script language="javascript">
		function doHourglass()
		{
			document.body.style.cursor = 'wait';
		}
		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body --><asp:label id="lblMessage" Runat="server"></asp:label><br>
						<table class="text_normal">
							<tr>
								<td vAlign="top">
									<table class="text_normal">
										<tr>
											<td colSpan="7"></td>
										</tr>
										<tr>
											<td>Empresa:</td>
											<td><asp:textbox id="txtSearchEmpresa" runat="server"></asp:textbox></td>
											<td>Nº Serviço:</td>
											<td><asp:textbox id="txtSearchNServico" Runat="server"></asp:textbox></td>
											<td colSpan="2"><asp:button id="btnSearch" Runat="server" CssClass="button" Text="Pesquisar"></asp:button>&nbsp;<asp:button id="btnLimparCampos" Runat="server" CssClass="button" Text="Limpar Campos"></asp:button></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						<br>
						<hr>
						<span class="errorMessage">Clique em qualquer parte da linha para ver o documento.</span><br>
						<br>
						<asp:datagrid id="dgDocumentos" Runat="server" CssClass="DG_cinzento" AutoGenerateColumns="False"
							BorderWidth="2px" OnSortCommand="SortGrid" OnItemDataBound="dgDocumentos_ItemDataBound" AllowSorting="True"
							OnItemCommand="visualisarDocumento" AllowPaging="False" PagerStyle-Mode="NumericPages" HeaderStyle-BackColor="#999999"
							GridLines="Both" BorderColor="#FFFFFF" DataKeyField="idServico" ForeColor="Transparent" AlternatingItemStyle-BackColor="#d3d3d3">
							<FooterStyle Font-Bold="True" ForeColor="White" BackColor="#999999" Width="800"></FooterStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#999999"></HeaderStyle>
							<Columns>
								<asp:ButtonColumn Visible="false" Text="visualisarDocumento" DataTextField="nomeDocumento" HeaderText="Visualisar Documento"
									CommandName="Select">
									<HeaderStyle Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
								</asp:ButtonColumn>
								<asp:BoundColumn DataField="idServico" Visible="False" SortExpression="idServico" HeaderText="idServico"></asp:BoundColumn>
								<asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa"></asp:BoundColumn>
								<asp:BoundColumn DataField="equipamento" SortExpression="equipamento" HeaderText="Equipamento"></asp:BoundColumn>
								<asp:BoundColumn DataField="responsavelValidar" SortExpression="responsavelValidar" HeaderText="Aprovar"></asp:BoundColumn>
								<asp:BoundColumn DataField="nomeFicheiro" SortExpression="nomeFicheiro" HeaderText="Nome do Documento"
									Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="nomeDocumento" SortExpression="nomeDocumento" HeaderText="" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="Ref.Serv." Visible="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="tipoCertificado" SortExpression="tipoCertificado" HeaderText="Tipo Cert."
									Visible="True"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Aprovar">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox ID="cbAprovar" Runat="server" AutoPostBack="False" Checked='<%#DataBinder.Eval(Container, "DataItem.cbAprovar")%>'>
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Rejeitar">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox ID="cbRejeitar" Runat="server" AutoPostBack="False" Checked='<%#DataBinder.Eval(Container, "DataItem.cbRejeitar")%>'>
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Observações">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox ID="txtObservacoes" Runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.obsWorkflowCertificado")%>'>
										</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="belongsToUser" SortExpression="belongsToUser" HeaderText="Do User" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="funcionarioValidou" SortExpression="funcionarioValidou" HeaderText="Validado Por"></asp:BoundColumn>
							</Columns>
							<PagerStyle Mode="NumericPages"></PagerStyle>
						</asp:datagrid><br>
						<asp:button id="btnAprovarAll" Runat="server" CssClass="button" Text="Aprovar todos"></asp:button>&nbsp;
						<asp:button id="btnRejeitarAll" Runat="server" CssClass="button" Text="Rejeitar todos"></asp:button>&nbsp;
						<asp:button id="btnDeselectAll" Runat="server" CssClass="button" Text="Limpar todos"></asp:button>&nbsp;
						<asp:button id="btnSubmit" Runat="server" CssClass="button_red" Text="Submeter"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:checkbox id="cbTodos" Runat="server" Text="    Ver todos" AutoPostBack="True" Font-Bold="True"></asp:checkbox>
						<table class="text_normal">
							<tr>
								<td></td>
							</tr>
						</table>
						&nbsp;&nbsp;&nbsp; 
						<!--body--></td>
				</tr>
			</table>
			<asp:datagrid id="dg1" Runat="server" AutoGenerateColumns="True"></asp:datagrid></form>
	</body>
</HTML>
