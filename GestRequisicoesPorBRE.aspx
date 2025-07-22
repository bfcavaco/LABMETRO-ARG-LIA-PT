<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@ Page language="c#" Codebehind="GestRequisicoesPorBRE.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestRequisicoesPorBRE" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - Requsições/BREs ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top" height="20"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top">
						<asp:label id="lblMessage" Runat="server"></asp:label>
						<!-- body -->
						<table class="text_normal" borderColor="darkgray" cellSpacing="2" cellPadding="2" width="700"
							border="10">
							<tr>
								<td style="WIDTH: 164px">Empresa:</td>
								<td style="WIDTH: 173px"><asp:textbox id="txtPesquisaEmpresa" Runat="server" AutoPostBack="true" ontextchanged="txtPesquisaEmpresa_TextChanged"></asp:textbox></td>
								<td>Nif:</td>
								<td><asp:textbox id="txtPesquisaNif" Runat="server" AutoPostBack="true" ontextchanged="txtPesquisaNif_TextChanged"></asp:textbox></td>
							</tr>
							<tr>
								<td style="WIDTH: 164px">Código Cliente SAP:</td>
								<td style="WIDTH: 173px"><asp:textbox id="txtPesquisaNumClienteSAP" Runat="server" AutoPostBack="true" ontextchanged="txtPesquisaNumClienteSAP_TextChanged"></asp:textbox></td>
								<td colSpan="2"><asp:Button class="button" id="btnEmpresas" CssClass="button" Runat="server" Text="Pesquisar Empresas" CausesValidation="false" onclick="btnEmpresas_Click"></asp:button></td>
							</tr>
							<tr id="trEmpresa" runat="server">
								<TD style="WIDTH: 164px">Empresa:</TD>
								<TD colSpan="3"><asp:dropdownlist id="ddEmpresa" Runat="server" AutoPostBack="true" DataTextField="nome" DataValueField="idEmpresa" onselectedindexchanged="ddEmpresa_SelectedIndexChanged"></asp:dropdownlist><asp:requiredfieldvalidator id="Requiredfieldvalidator1" Runat="server" ControlToValidate="ddEmpresa" ErrorMessage="Empresa">*</asp:requiredfieldvalidator>
									<asp:label id="lblEmpresa" Runat="server"></asp:label>
									<br>
									<br>
									<asp:label id="lblEmpresaDevedora" Runat="server" ForeColor="Red"></asp:label></TD>
							</tr>
							<TR>
								<TD style="WIDTH: 162px">BRE:</TD>
								<TD style="WIDTH: 207px"><asp:dropdownlist id="ddBRE" Runat="server" AutoPostBack="true" DataTextField="refBRE" DataValueField="idBRE" onselectedindexchanged="ddBRE_SelectedIndexChanged"></asp:dropdownlist><asp:label id="lblBRE" Runat="server"></asp:label>&nbsp;&nbsp;&nbsp;
								</TD>
								<TD colSpan="2"><asp:checkbox id="cbBRE" runat="server" AutoPostBack="True" Text="&nbsp;&nbsp;Só BRE's completos!"
										Checked="True"></asp:checkbox></TD>
							</TR>
						</table>
						<br>
						<br>
						<div class="SeparadorFactura">Requisições da Empresa</div>
						<asp:datagrid id="dgRequisicoes" CssClass="DG_cinzento" Runat="server"  AlternatingItemStyle-BackColor="WhiteSmoke"
							DataKeyField="idRequisicao" BorderWidth="2" GridLines="Both" BorderColor="#FFFFFF" PagerStyle-Mode="NumericPages"
							OnPageIndexChanged="DoPaging" OnSortCommand="SortGrid" AllowSorting="True" PageSize="10" AllowPaging="true"
							AutoGenerateColumns="false" CellPadding="2" Width="100%">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa"></asp:BoundColumn>
								<asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente" HeaderText="Ref. Req.<br>Cliente"
									ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Empresa<br>tem<br>contrato?" SortExpression="eContrato">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.eContrato"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="dtRequisicao" SortExpression="dtRequisicao" HeaderText="Data Req." DataFormatString="{0:dd/MM/yyyy}"
									ItemStyle-Wrap="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="dtValidade" SortExpression="dtValidade" HeaderText="Data Val." DataFormatString="{0:dd/MM/yyyy}"
									ItemStyle-Wrap="False"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="compl." SortExpression="completa">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.completa"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Contrato" SortExpression="bContrato">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bContrato"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Renovável" SortExpression="bRenovavel">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRenovavel"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Ficheiro" SortExpression="nomeFicheiro">
									<ItemTemplate>
										<asp:HyperLink Runat=server NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>' ID="Hyperlink1" Target=new>
											<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:HyperLinkColumn HeaderText="(Editar)<br>Requisição" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
									DataNavigateUrlField="idRequisicao" target="_new" DataTextField="refRequisicao" SortExpression="refRequisicao">
									<ItemStyle HorizontalAlign="center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn HeaderText="Ver<br>Serviços" DataNavigateUrlFormatString="ListaServicosRequisicao.aspx?btn=Doc&id={0}"
									DataNavigateUrlField="idRequisicao" target="_new" Text="ver">
									<ItemStyle HorizontalAlign="center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:TemplateColumn HeaderText="Dar Baixa">
									<ItemTemplate>
										<asp:CheckBox Runat="server" ID="chbCompleta" AutoPostBack="True" Checked='<%#DataBinder.Eval(Container.DataItem, "completa") %>' OnCheckedChanged="cb_SetComplete">
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
						<br>
						<br>
						<div class="SeparadorFactura">Serviços por BRE - para alterar requisição associada, 
							clique em "Editar"</div>
						<asp:datagrid id="dgOrigem" CssClass="DG_cinzento" Runat="server" AlternatingItemStyle-BackColor="#d3d3d3"
							DataKeyField="idServico" BorderWidth="2" GridLines="horizontal" BorderColor="#FFFFFF" OnSortCommand="SortGridOrigem"
							AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" OnEditCommand="editGridOrigem"
							OnCancelCommand="cancelGridOrigem" OnUpdateCommand="updateGridOrigem" OnItemDataBound="dgOrigem_ItemDataBound"
							Width="100%">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:CheckBox Runat="server" ID="checkbox"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Ref.Calib." SortExpression="refServico">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.refServico") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="N/Ref.<br>Req.">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.refRequisicao") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList ID="ddRequisicaoEdit" Runat="server" DataValueField="idRequisicao" DataTextField="refRequisicao"></asp:DropDownList>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Ref. Req.<br>Cliente" SortExpression="referenciaCliente">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.referenciaCliente") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Núm.Ident." SortExpression="numIdentificacao">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tipo Equip." SortExpression="tipoEquipamento">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Estado" SortExpression="estadoServico">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.estadoServico") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Dt.Estado" SortExpression="dtEstado">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.dtEstado") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tipo Serv." SortExpression="tipoServico">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.tipoServico") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Obs." SortExpression="observacoes">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.observacoes") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Calib.<br>Ext." SortExpression="calibracaoExterna">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.calibracaoExterna"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar" ItemStyle-Width="100"></asp:EditCommandColumn>
							</Columns>
						</asp:datagrid>
						<!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
