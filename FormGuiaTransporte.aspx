<%@ Page language="c#" Codebehind="FormGuiaTransporte.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormGuiaTransporte" %>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - GUIA DE TRANSPORTE ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body onkeydown="CheckKey(event);" MS_POSITIONING="GridLayout">
		<script language="javascript">
	function CheckKey() 
	{
		if (event.keyCode == 13) 
		{
			document.getElementById("btnSubmit").focus();
		}
	}

		</script>
		<form id="Form1" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top" height="60"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr borderColor="pink">
					<td class="text_normal" vAlign="top">
						<!-- body da pagina fica num td, o primeiro é rowspan 2 e contem o menu--><asp:validationsummary id="valSummary" runat="server" DisplayMode="SingleParagraph" CssClass="errorMsg"
							ShowSummary="true" HeaderText="Preencha por favor os campos marcados com *."></asp:validationsummary>
						<table class="tblBody" borderColor="darkgray" cellSpacing="2" cellPadding="2" border="1">
							<tr>
								<td class="tblTituloCinzaClaroLetraBranca" colSpan="4">Guia de Transporte</td>
							</tr>
							<tr>
								<td colSpan="4"><asp:label id="lblMessage" Runat="server"></asp:label></td>
							</tr>
							<tr>
								<td>Equipamentos<br />
									da Empresa:</td>
								<td><asp:textbox id="txtPesquisaEmpresa" Runat="server" AutoPostBack="true"></asp:textbox></td>
								<td style="WIDTH: 1px">Nif:</td>
								<td><asp:textbox id="txtPesquisaNif" Runat="server" AutoPostBack="true"></asp:textbox>&nbsp; 
									&nbsp;<asp:Button class="button" id="btnEmpresas" CssClass="button" Runat="server" Text="ver Empresas" CausesValidation="false"
										Width="80"></asp:button></td>
							</tr>
							<tr id="trEmpresa" runat="server">
								<TD colSpan="4" height="20">Empresa:
									<asp:dropdownlist id="ddEmpresa" Runat="server" AutoPostBack="true" ForeColor="#00cc00" DataTextField="nome"
										DataValueField="idEmpresa"></asp:dropdownlist><asp:requiredfieldvalidator id="Requiredfieldvalidator4" Runat="server" ControlToValidate="ddEmpresa">*</asp:requiredfieldvalidator></TD>
							</tr>
							<tr>
								<td>BRE's da Empresa</td>
								<td><asp:dropdownlist id="ddBre" Runat="server" AutoPostBack="True" DataTextField="refBRE" DataValueField="idBRE" onselectedindexchanged="ddBre_SelectedIndexChanged"></asp:dropdownlist></td>
								<td style="WIDTH: 1px">Guia&nbsp;criada por:</td>
								<td><asp:textbox id="txtUtilCriacao" runat="server" ReadOnly="True"></asp:textbox></td>
							</tr>
							<tr>
								<td><asp:Button class="button" id="btnSearch" runat="server" CssClass="button" Text="Ver Serviços" CausesValidation="False"></asp:button></td>
								<td></td>
								<td style="WIDTH: 1px">REF:</td>
								<td><asp:label id="lblRefGuia" Runat="server"></asp:label></td>
							</tr>
							<tr>
								<td>Destinatário:</td>
								<td style="WIDTH: 129px" colSpan="2"><asp:textbox id="txtDestinatario" Runat="server" Width="330px"></asp:textbox></td>
								<td>Local de Destino:<asp:textbox id="txtLocalDestino" Runat="server" Width="100%"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator5" Runat="server" ControlToValidate="txtLocalDestino">*</asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td>Data:<br />
									<asp:textbox id="txtData" Runat="server"></asp:textbox><asp:comparevalidator id="Comparevalidator1" runat="server" ControlToValidate="txtData" Operator="DataTypeCheck"
										Type="Date">data inválida!</asp:comparevalidator><asp:requiredfieldvalidator id="RequiredFieldValidator1" Runat="server" ControlToValidate="txtData">*</asp:requiredfieldvalidator></td>
								<td>Hora saída:<br />
									<asp:textbox id="txtHoraSaida" Runat="server" MaxLength="5"></asp:textbox></td>
								<td>Hora chegada(prevista):<br />
									<asp:textbox id="txtHoraChegada" Runat="server" MaxLength="5"></asp:textbox></td>
								<td>Num.contrib.:<br />
									<asp:textbox id="txtNumContribuinte" Runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td>Local Carregamento</td>
								<td><asp:textbox id="txtLocalCarregamento" Runat="server" Width="100%"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator2" Runat="server" ControlToValidate="txtLocalCarregamento">*</asp:requiredfieldvalidator></td>
								<td style="WIDTH: 1px">Local Descarregamento</td>
								<td><asp:textbox id="txtLocalDescarregamento" Runat="server" Width="100%"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator3" Runat="server" ControlToValidate="txtLocalDescarregamento">*</asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td>Matrícula:</td>
								<td><asp:textbox id="txtMatricula" Runat="server" Width="100%"></asp:textbox></td>
								<td style="WIDTH: 1px">Nome do condutor:</td>
								<td><asp:textbox id="txtNomeCondutor" Runat="server" Width="100%"></asp:textbox></td>
							</tr>
							<tr>
								<td style="HEIGHT: 16px" colSpan="4">Observacoes:<asp:textbox id="txtObservacoes" Runat="server" Width="550px" MaxLength="250"></asp:textbox></td>
							</tr>
							<tr>
								<td colSpan="4"><asp:datagrid id="dgOrigem" CssClass="DG_cinzento" Runat="server" AutoGenerateColumns="False"
										AllowPaging="False" AllowSorting="True" OnSortCommand="SortGrid" PagerStyle-Mode="NumericPages" BorderColor="#FFFFFF"
										GridLines="Both" BorderWidth="2" DataKeyField="idServico" AlternatingItemStyle-BackColor="LightGrey" BackColor="Gainsboro">
										<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
										<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:CheckBox Runat="server" ID="checkbox" AutoPostBack="false"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="refServico"></asp:BoundColumn>
											<asp:BoundColumn DataField="refBre"></asp:BoundColumn>
											<asp:BoundColumn DataField="tipoEquipamento"></asp:BoundColumn>
											<asp:BoundColumn DataField="numIdentificacao"></asp:BoundColumn>
											<asp:BoundColumn DataField="numSerie"></asp:BoundColumn>
											<asp:BoundColumn DataField="estadoServico"></asp:BoundColumn>
											<asp:BoundColumn DataField="localCalibracao"></asp:BoundColumn>
											<asp:BoundColumn DataField="obsServico"></asp:BoundColumn>
											<asp:BoundColumn DataField="obsBRE"></asp:BoundColumn>
										</Columns>
									</asp:datagrid><br />
									<br />
									<asp:Button class="button" id="btnAddLinesToDestino" CssClass="button" Runat="server" Text="Adicionar Serviços" CausesValidation="False"></asp:button><asp:datagrid id="dgDestino" CssClass="DG_branco" Runat="server" Width="100%" AutoGenerateColumns="false"
										AllowSorting="false" BorderColor="#E0E0E0" GridLines="both" BorderWidth="2" DataKeyField="idServico" ShowFooter="false" OnUpdateCommand="updateGrid" OnCancelCommand="cancelGrid" OnEditCommand="editGrid"
										HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True" HeaderStyle-ForeColor="#FFFFFF">
										<Columns>
											<asp:TemplateColumn HeaderText="NºBRE">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.refBRE") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Ref. Calibração">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.refServico") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Cód.Equip." SortExpression="tipoEquipamento">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Num.Ident." SortExpression="numIdentificacao">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Num.Série" SortExpression="numSerie">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.numSerie") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Estado">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.estadoServico") %>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:DropDownList ID="ddEstadoServicoEdit" Runat="server" DataTextField="descricao" DataValueField="idEstadoServico"></asp:DropDownList>
												</EditItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Tipo serviço" SortExpression="TipoServico">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.tipoServico") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Local Equip." SortExpression="TipoServico">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.localCalibracao") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:ButtonColumn CommandName="Delete" Text="Remover"></asp:ButtonColumn>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
								<td colspan=2 align=center><asp:Button class="button" id="btnSubmit" CssClass="button_red" Runat="server" Text="Submeter"></asp:button></td>
								
									<td></td>
								<td><asp:Button class="button" id="btnReport" CssClass="button" Runat="server" Text="Ver/Imprimir" Width="135px" onclick="btnReport_Click"></asp:button></td>
							
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<!--FIM tabela principal--></form>
		</ASP:DDEMPRESA>
	</body>
</HTML>
