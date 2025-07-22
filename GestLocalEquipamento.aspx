<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@ Page language="c#" Codebehind="GestLocalEquipamento.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestLocalEquipamento" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Gestão Localização Equipamento</title>
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
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body --><asp:label id="lblMessage" Runat="server"></asp:label><br />
						<br />
						<table class="text_normal" border="1">
							<tr>
								<td vAlign="top">
									<asp:Label ID="Label1" Runat="server" CssClass="lblMessage"></asp:Label>
									<table class="text_normal" runat="server" id="tblPesquisa" cellSpacing="2" cellPadding="2"
										border="1" borderColor="darkgray">
										<tr class="tblTituloCinzaClaroLetraBranca">
											<td colspan="5">Pesquisar Guias Transporte</td>
										</tr>
										<tr>
											<td>Empresa<br />
												do Equipamento:</td>
											<td><asp:TextBox ID="txtPesquisaEmpresa" Runat="server" AutoPostBack="true"></asp:TextBox></td>
											<td>Nif:<asp:TextBox ID="txtPesquisaNif" Runat="server" AutoPostBack="true"></asp:TextBox></td>
											<td><asp:Button class="button" id="btnPesquisaEmpresa" Runat="server" Text="ver Empresas" CssClass="button"></asp:Button></td>
										</tr>
										<tr>
											<td colspan="4"><asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa" AutoPostBack="true"></asp:DropDownList></td>
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
											<td>Ref BRE:</td>
											<td><asp:TextBox ID="txtBRE" Runat="server" AutoPostBack="false"></asp:TextBox></td>
											<td>Local.Equip.:</td>
											<td><asp:dropdownlist id="ddLocal" runat="server" DataTextField="descricao" DataValueField="ident"></asp:dropdownlist></td>
										</tr>
										<tr>
											<td colspan="4"><asp:Button class="button" id="btnPesquisa" Runat="server" Text='Pesquisar' CssClass="button"></asp:Button></td>
										</tr>
									</table>
									<br />
									<br />
									<asp:datagrid 
									id="dgServicos" 
									Runat="server"
									AllowSorting="True" 
									DataKeyField="idServico" 
									OnSortCommand="SortGrid" 
									AutoGenerateColumns=False>									
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:CheckBox Runat="server" ID="checkbox" AutoPostBack="false"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="NºGuia" SortExpression="refGuiaTransporte">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.refGuiaTransporte") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="NºBRE"  SortExpression="refBRE">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.refBRE") %>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Ref. Calibração"  SortExpression="refServico">
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
											<asp:TemplateColumn HeaderText="Estado"  SortExpression="estadoServico">
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
											<asp:TemplateColumn HeaderText="Local Equip." SortExpression="localCalibracao">
												<ItemTemplate>
													<%# DataBinder.Eval(Container, "DataItem.localCalibracao") %>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
							<tr>
								<td vAlign="top"><br />
									Mudar todos os seleccionados<br />
									como estando localizados em:
									<asp:dropdownlist id="ddLocalNovo" runat="server" DataTextField="descricao" DataValueField="ident"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</tr>
						</table>
						<br />
						<hr>
						<br />
						<br />
						<asp:Button class="button" id="btnSelectAll" Runat="server" CssClass="button" Text="Seleccionar todos"></asp:button>&nbsp;&nbsp;&nbsp;<asp:Button class="button" id="btnDeselectAll" Runat="server" CssClass="button" Text="Limpar todos"></asp:button>
						&nbsp;&nbsp;&nbsp;
						<asp:Button class="button" id="btnSubmit" Runat="server" CssClass="button_red" Text="Alterar Localização dos Serviços Seleccionados"
							BackColor="#B22222" ForeColor="#ffffff" Font-Bold="True"></asp:button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
