<%@ Page language="c#" Codebehind="BO_Contactos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_Contactos" %>
<%@Register TagPrefix="menu" TagName="inc_menu" src="boMenu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>:: LabMetro - BO Contactos ::</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../labmetro.css" type="text/css" rel="stylesheet">
		<style type="text/css">

		
#footer { PADDING-RIGHT: 15px; BORDER-TOP: #cccccc 1px solid; PADDING-LEFT: 10px; FONT-SIZE: 10px; COLOR: #666666; LINE-HEIGHT: 17px; PADDING-TOP: 5px; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }

		
#footer A { FONT-SIZE: 11px; COLOR: #666666; TEXT-DECORATION: none }

		
#footer A:hover { TEXT-DECORATION: underline }

		
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
					<td valign="top"><!-- body -->
						<table id="tblPesquisa" bordercolor="darkgray" border="2" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" width="400">
							<tr>
								<td colspan="4"><asp:label id="lblMessage" Runat="server" ForeColor="#ff0033"></asp:label></td>
							</tr>
							<tr>
								<td colspan="4"><%=Resources.Resource.PesquisarContactos %>:</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Empresa %>:</td>
								<td><asp:textbox id="txtPesquisaEmpresa" Runat="server"></asp:textbox></td>
								<td colspan="2"><asp:Button class="button" id="btnPesquisaEmpresa" Runat="server" CssClass="button" Text="<%$ Resources:Resource, verEmpresas %>"></asp:Button></td>
							</tr>
							<tr>
								<td colspan="4"><asp:dropdownlist id="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa" DataTextField="Nome_"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td colspan="4"><%=Resources.Resource.Contactos %>:</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Nome %>:</td>
								<td ><asp:textbox id="txtNomeContacto" runat="server"></asp:textbox></td>
								<td><%=Resources.Resource.Email %>:</td>
								<td ><asp:textbox id="txtEmail" runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td colspan="4"><asp:Button class="button" id="btnPesquisa" Runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>"></asp:Button></td>
							</tr>
						</table>
						<br />
						<br />
						
				<div><%=Resources.Resource.FraseMarcarCaixasContactosSubstituir %></div>
						<asp:datagrid id="DGContactos" Runat="server" CssClass="DG_branco" AlternatingItemStyle-BackColor="LightGrey"
							BackColor="Gainsboro" DataKeyField="idContacto" BorderWidth="2" GridLines="Both" BorderColor="#FFFFFF"
							PagerStyle-Mode="NumericPages" OnPageIndexChanged="doPaging" OnSortCommand="SortGrid" AllowSorting="True"
							PageSize="25" AllowPaging="true" AutoGenerateColumns="false" 
							Font-Name="Arial" Font-Size="8">
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:CheckBox Runat="server" ID="checkbox" AutoPostBack="false"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="activo" HeaderText="Activo.">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:textbox id="txtColor" Enabled="False" BackColor=<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.activo")))%>  Width="10" Height="10" Runat="server">
										</asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="idContacto" SortExpression="idContacto" HeaderText="idContacto"></asp:BoundColumn>
								<asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="Nome"></asp:BoundColumn>
								<asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="Empresa" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="idEmpresa" SortExpression="idEmpresa" HeaderText="IdEmpresa"
									ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="departamento" SortExpression="departamento" HeaderText="Departamento"
									ItemStyle-Width="50"></asp:BoundColumn>
								<asp:BoundColumn DataField="cargo" SortExpression="cargo" HeaderText="Função"></asp:BoundColumn>
								<asp:BoundColumn DataField="telefoneEmpresa" SortExpression="telefoneEmpresa" HeaderText="Tel."></asp:BoundColumn>
								<asp:BoundColumn DataField="faxEmpresa" SortExpression="faxEmpresa" HeaderText="Fax"></asp:BoundColumn>
								<asp:BoundColumn DataField="emailEmpresa" SortExpression="emailEmpresa" HeaderText="Email" ItemStyle-Wrap="True"
									ItemStyle-Width="80"></asp:BoundColumn>
                                <asp:BoundColumn DataField="idCrm" SortExpression="idCrm" HeaderText="idCrm" ItemStyle-Wrap="True"
									ItemStyle-Width="80"></asp:BoundColumn>
									<asp:BoundColumn DataField="userId" SortExpression="userId" HeaderText="userId" ItemStyle-Wrap="True"
									ItemStyle-Width="80"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
						<br />
						<br />
						<br />
				<div><%=Resources.Resource.FraseContactoMantar %></div>
						<asp:DropDownList ID="ddContactoManter" Runat="server"></asp:DropDownList>
						<asp:Button class="button" ID="btnTrocar" Runat="server" Text="<%$Resources:Resource, FrasetrocarContactosSelect %>"></asp:Button>
						<br />
						<br />
						<asp:DataGrid ID="dgGenerico" Runat="server" AutoGenerateColumns="True" BackColor="Gainsboro"	AlternatingItemStyle-BackColor="LightGrey" BorderWidth="2" GridLines="Both" BorderColor="#FFFFFF"
							Font-Name="Arial" Font-Size="8"></asp:DataGrid>
						<!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
