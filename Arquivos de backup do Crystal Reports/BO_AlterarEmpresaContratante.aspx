<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<%@ Page language="c#" Codebehind="BO_AlterarEmpresaContratante.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_AlterarEmpresaContratante" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BO Alterar Empresa Contratante</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Styles.css" type="text/css" rel="stylesheet">
		<style type="text/css">#footer { PADDING-RIGHT: 15px; BORDER-TOP: #cccccc 1px solid; PADDING-LEFT: 10px; FONT-SIZE: 10px; COLOR: #666666; LINE-HEIGHT: 17px; PADDING-TOP: 5px; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
	#footer A { FONT-SIZE: 11px; COLOR: #666666; TEXT-DECORATION: none }
	#footer A:hover { TEXT-DECORATION: underline }
		</style>
	</HEAD>
	<body onkeydown="CheckKey(event);">
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
				<tr>
					<td><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu><br />
						<br />
					</td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
						<table id="tblPesquisa" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" width="90%" borderColor="darkgray" border="2">
							<tr>
								<td colSpan="5"><%=Resources.Resource.MSG_BackofficeAdministrativo %></td>
							</tr>
							<tr>
								<td colSpan="4"><asp:label id="lblMessage" Runat="server" ForeColor="#ff0033"></asp:label></td>
							</tr>
							<tr>
								<td colSpan="4"><%=Resources.Resource.NumBRE %>&nbsp;<%=Resources.Resource.Completa%>:
									<asp:textbox id="txtRefBRE" Runat="server"></asp:textbox>&nbsp;
									<asp:Button class="button" id="btnPesquisa" Runat="server" Text="<%$Resources:Resource, Pesquisar %>" CssClass="Button" onclick="btnPesquisa_Click"></asp:button>
								</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Empresa %></td>
								<td>
									<asp:Label id="lblEmpresa" runat="server"></asp:Label></td>
								<td><%=Resources.Resource.EmpresaContratante %></td>
								<td>
									
										<asp:dropdownlist id="ddEmpresaContratante" Runat="server" AutoPostBack="true" DataValueField="idEmpresa" DataTextField="nome"></asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td>Nova empresa contratante:(pesquisar)</td>
								<td>
									<asp:TextBox ID="txtPesquisaEmpresaContratante" runat="server"></asp:TextBox><asp:Button class="button" ID="Button1" Runat="server" Text="<%$ Resources:Resource, Procurar %>" onclick="btnSearchEmpresa_Click"></asp:Button></td>
								<td>Mudar para <%=Resources.Resource.EmpresaContratante %></td>
								<td>
									
										
									<P> 
										<asp:dropdownlist id="ddEmpresaContratanteNova" Runat="server" AutoPostBack="true" DataValueField="idEmpresa" DataTextField="nome"></asp:dropdownlist></P>
									  Cliente do BRE tem acesso aos certificados?
                           <asp:RadioButtonList ID="rbEmpbrepodevercertificados" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>" Selected="true" ></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
								</asp:RadioButtonList>
							</tr>

							<tr>
								<td colspan="4">
									<asp:datagrid id="dgServicosBRE" Runat="server" Width="50%" CssClass="DG_branco" AutoGenerateColumns="true"
										GridLines="horizontal" BorderColor="#E0E0E0" AllowSorting="False" ShowFooter="true" BorderWidth="2"
										HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True" HeaderStyle-ForeColor="#FFFFFF" onselectedindexchanged="dgServicosBRE_SelectedIndexChanged"></asp:datagrid></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
								<td><asp:Button class="button" id="btnSubmit" Runat="server" Text="<%$ Resources:Resource, Alterar %>" CssClass="Button" onclick="btnSubmit_Click"></asp:button></td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
						</table>
						<!-- FIM body --></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
