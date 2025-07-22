<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<%@ Page language="c#" Codebehind="MarcacoesSemanaTodos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.MarcacoesSemanaTodos" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>MarcacoesSemana</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		
		function CheckKey() 
		{
			if (event.keyCode == 13) 
			{
				document.getElementById("btnPesquisa").focus();
			}
		}

		</script>
</HEAD>
	<body onkeydown="CheckKey(event);" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table id="tblMain">
				<tr>
					<td vAlign="top" align="left" rowSpan="2"><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
					<td vAlign="top"><header:inc_header id="inc_header" runat="server"></header:inc_header></td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top" borderColor="#999999">
						<!-- body --> <!-- body -->
						<table id="tblPesquisa" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" borderColor="darkgray"
							width="100%" border="2">
							<TBODY>
								<tr>
									<td colSpan="4"><asp:label id="lblMessage" Runat="server" ForeColor="#ff0033"></asp:label></td>
								</tr>
								<tr class="tblTituloCinzaClaroLetraBranca">
									<td colSpan="4">Pesquisar Marcações</td>
								</tr>
								<tr>
									<td>Técnico:</td>
									<td><asp:dropdownlist id="ddTecnicoExterior" Runat="server" DataTextField="nomeAbreviado" DataValueField="idFuncionario"
											AutoPostBack="True" onselectedindexchanged="ddTecnicoExterior_SelectedIndexChanged"></asp:dropdownlist></td>
									<td></td>
									<td></td>
								</tr>
								<tr>
									<td>De:</td>
									<td><asp:TextBox ID="txtStart" Runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator Runat="server" ControlToValidate="txtStart" ID="REQ1">*</asp:RequiredFieldValidator>
										<asp:comparevalidator id="Comparevalidator6" runat="server" ControlToValidate="txtStart" Type="Date" Operator="DataTypeCheck">formato errado</asp:comparevalidator></td>
									<td>A:</td>
									<td><asp:TextBox ID="txtEnd" Runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator Runat="server" ControlToValidate="txtEnd" ID="Requiredfieldvalidator1">*</asp:RequiredFieldValidator>
										<asp:comparevalidator id="Comparevalidator1" runat="server" ControlToValidate="txtEnd" Type="Date" Operator="DataTypeCheck">formato errado</asp:comparevalidator></td>
								</tr>
								<tr>
									<td colspan="4"><asp:Button class="button" id="btnPesquisa" Runat="Server" CausesValidation="true" Text="Ver" CssClass="button"
											Width="100" onclick="btnPesquisa_Click"></asp:button></td>
								</tr>
							</TBODY>
						</table>
						<br>
						<asp:DataGrid id="DataGrid1" OnSortCommand="SortGrid" AllowSorting="True" AllowPaging="false"
							AutoGenerateColumns="false" runat="server" OnItemDataBound="dg_databound" Width="100%" CssClass="DG_branco"
							BorderColor="#FFFFFF" GridLines="Both" BorderWidth="2" DataKeyField="idMarcacao" BackColor="Gainsboro"
							AlternatingItemStyle-BackColor="LightGrey" CellPadding="2" CellSpacing="0">
							<Columns>
								<asp:TemplateColumn HeaderText="Dia(s)" SortExpression="Weekday">
									<ItemTemplate>
										<%# diaSemana(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.Weekday"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn HeaderText="Data" DataField="date" SortExpression="date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="empresa" ReadOnly="True"></asp:BoundColumn>
<asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="Morada" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="localidade" SortExpression="localidade" HeaderText="Localidade" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="telefone" SortExpression="telefone" HeaderText="Telefone" ReadOnly="True"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Dt.Marcação" SortExpression="dtMarcacao" ItemStyle-Wrap="False">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.dtMarcacao","{0:dd/MM/yyyy}") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtStartEdit" Text='	<%# DataBinder.Eval(Container, "DataItem.dtMarcacao","{0:dd/MM/yyyy}") %>' Runat="server" />
									</EditItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn HeaderText="Técnico" SortExpression="funcionario">
									<ItemTemplate>
										<%# DataBinder.Eval(Container, "DataItem.funcionario") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList ID="ddFuncionarioEdit" DataValueField="idFuncionario" DataTextField="nome" Runat="server"></asp:DropDownList>
									</EditItemTemplate>
								</asp:TemplateColumn>
								
								<asp:HyperLinkColumn HeaderText="BRE" DataNavigateUrlField="idBRE" target="_self" DataTextField="BRE" SortExpression="bre"></asp:HyperLinkColumn>
									<asp:TemplateColumn HeaderText="BRE<br>def." SortExpression="bdefinitivo">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bdefinitivo"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Requisicao" SortExpression="nomeFicheiro">
									<ItemTemplate>
										<asp:HyperLink Runat=server NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>' ID="Hyperlink1" Target=new>
											<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn HeaderText="Camiao" SortExpression="bCamiao" ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bCamiao"))) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn CommandName="verDadosEmpresaManutencao" HeaderText="Emp.<br>Mant." Text="ver" ItemStyle-Font-Size="8"></asp:ButtonColumn>
								
							</Columns>
						</asp:DataGrid><br >
						<asp:DataGrid ID="dgEmpresasManutencao" runat="server" AutoGenerateColumns="True"></asp:DataGrid>
					</td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
