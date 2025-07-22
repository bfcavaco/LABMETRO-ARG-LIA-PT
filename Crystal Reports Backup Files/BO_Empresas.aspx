<%@ Page language="c#" Codebehind="BO_Empresas.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_Empresas" %>
<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - BO Empresas ::</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Styles.css" type="text/css" rel="stylesheet">
		<style type="text/css">

		
#footer {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 10px;
	color: #666666;
	line-height: 17px;
	border-top: 1px solid #CCCCCC;
	padding-left: 10px;
	padding-right: 15px;
	padding-top: 5px;
}

#footer a {
	color: #666666;
	font-size: 11px;
	text-decoration: none;
}

#footer a:hover {
	text-decoration: underline;
	}
		</style>
	</HEAD>
	<body onkeydown="CheckKey(event);" MS_POSITIONING="GridLayout">
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
			
			<table id="tblMainBO">
			<tr><td><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu><br /><br /></td></tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
					 
							<table id="tblPesquisa" borderColor="darkgray" border="2" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" width="400">
							<tr>
								<td colSpan="4"><asp:label id="lblMessage" Runat="server" ForeColor=#ff0033></asp:label></td>
							</tr>
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colSpan="4"><%=Resources.Resource.PesquisarEmpresa %>:</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Empresa %>:<asp:textbox id="txtNomeEmpresa" Runat="server"></asp:textbox></td>
								<td><%=Resources.Resource.NIF %>:<asp:textbox id="txtNIF" Runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Tipo %>:
									<asp:DropDownList ID="ddTipoEmpresa" Runat="server" DataValueField="ident" DataTextField="descricao"></asp:DropDownList></td>
								<td><%=Resources.Resource.Estado %>:<asp:dropdownlist id="ddEstado" Runat="server" DataTextField="descricao" DataValueField="ident"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td colspan="4"><%=Resources.Resource.NumCliente %>:<asp:TextBox ID="txtNumClienteSAP" Runat="server"></asp:TextBox></td>
							</tr>
							<tr>
								<!--so tem 2 td's visto haver um rowspan em cima-->
								<td align="center" colSpan="2"><asp:Button class="button" id="btnPesquisa" CssClass="button" Runat="Server" Text="<%$Resources:Resource, ListarEmpresas %>"></asp:button></td>
							</tr>
						</table>
						<br />
						<br />
						
								<div class="lblVermelhaBO"><%=Resources.Resource.FraseMarcarCaixasEmpresasSubtiruir %></div>
						<asp:datagrid id="DGEmpresas" CssClass="DG_branco" Runat="server" BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey"
							DataKeyField="idEmpresa" BorderWidth="2" GridLines="Both" BorderColor="#FFFFFF" PagerStyle-Mode="NumericPages"
							OnSortCommand="SortGrid" OnPageIndexChanged="DoPaging" AllowSorting="True" PageSize="25" AllowPaging="true"
							AutoGenerateColumns="false" Font-Name="Arial" Font-Size="8" SelectedItemStyle-BackColor="#99cc99">
							<HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
							<FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
							<Columns>
								<asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:CheckBox Runat="server" ID="checkbox" AutoPostBack="false"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:textbox id="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'  Width="20" Height="20" Runat="server">
										</asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="idEmpresa" SortExpression="idEmpresa" HeaderText="idEmpresa "></asp:BoundColumn>
								<asp:BoundColumn DataField="nomeComprido" SortExpression="nomeComprido" HeaderText="Nome "></asp:BoundColumn>
									<asp:BoundColumn DataField="estadoEmpresa" SortExpression="estadoEmpresa" HeaderText="Estado"></asp:BoundColumn>
								<asp:BoundColumn DataField="numClienteSAP" SortExpression="numClienteSAP" HeaderText="Núm. Cliente"></asp:BoundColumn>
								<asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="morada"></asp:BoundColumn>
								<asp:BoundColumn DataField="localidadeEmpresa" SortExpression="localidadeEmpresa" HeaderText="Localidade"></asp:BoundColumn>
							
								<asp:BoundColumn DataField="descTipoEmpresa" SortExpression="descTipoEmpresa" HeaderText="Tipo"></asp:BoundColumn>
								<asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="telefone"></asp:BoundColumn>
								<asp:BoundColumn DataField="nif" SortExpression="nif" HeaderText="NIF"></asp:BoundColumn>
								<asp:BoundColumn DataField="dtEstado" SortExpression="dtEstado" HeaderText="Dt.<br />Estado" DataFormatString="{0:dd/MM/yyyy}"
									ItemStyle-Wrap="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="DataCriacao" SortExpression="DataCriacao" HeaderText="Data<br />	Criação"
									DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                <asp:BoundColumn DataField="idEmpresaCrm" SortExpression="idEmpresaCrm" HeaderText="Id.Crm"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
						<br />
						<br />
						<table width="452" border="0" align="left" cellpadding="0" cellspacing="0" id="footer">
							<tr>
								<td width="450"><asp:LinkButton ID="Linkbutton1" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False"><%=Resources.Resource.Contactos %></asp:LinkButton>&nbsp; 
									| &nbsp;
									<asp:LinkButton ID="Linkbutton2" Runat="server" CssClass="LinkVermelhoSemUnderline"  CausesValidation="False"><%=Resources.Resource.Equipamentos %></asp:LinkButton>&nbsp; 
									| &nbsp;
									<asp:LinkButton ID="Linkbutton3" Runat="server" CssClass="LinkVermelhoSemUnderline"  CausesValidation="False"><%=Resources.Resource.Servicos %></asp:LinkButton>&nbsp; 
									| &nbsp;
									<asp:LinkButton ID="Linkbutton4" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False" ><%=Resources.Resource.Orcamentos %></asp:LinkButton>&nbsp; 
									| &nbsp;
									<asp:LinkButton ID="Linkbutton5" Runat="server" CssClass="LinkVermelhoSemUnderline" CausesValidation="False"><%=Resources.Resource.Requisicoes %></asp:LinkButton>
								</td>
							</tr>
						</table>
						<br />
						<br /><br />
						<br />
				<div class="lblVermelhaBO"><%=Resources.Resource.FraseEmpresaManter %></div>
						<asp:DropDownList ID="ddEmpresaManter" Runat="server"></asp:DropDownList>
						<asp:Button class="button" ID="btnTrocar" Runat="server" Text="<%$ Resources:Resource, FrasetrocarEmpresasSelect %>"></asp:Button>
						<br />
						<br />
						<asp:DataGrid ID="dgGenerico" Runat="server" AutoGenerateColumns="True" CssClass="DG_branco" Runat="server"
							BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey" BorderWidth="2" GridLines="Both"
							BorderColor="#FFFFFF" Font-Name="Arial" Font-Size="8"></asp:DataGrid>
						<!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
