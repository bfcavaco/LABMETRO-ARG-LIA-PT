<%@ Page language="c#" Codebehind="BO_MudarEstados.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_MudarEstados" %>
<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BO Mudar Estados</title>
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
				<tr>
					<td><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu><br />
						<br />
					</td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
						<table id="tblPesquisa" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" borderColor="darkgray"
							width="90%" border="2">
							<tr>
								<td colSpan="5">Se faltarem estados, por favor comunique-me (Daniela) para que eu 
									posso analizar as sequências pretendidas e adiciona-las caso façam sentido.
									<br />
									<br />
									Se um estado fôr alterado para calibrado, deverá de seguida actualizar o 
									Técncico que calibrou (BO próprio) para efeitos de Workflow, pois a Base de 
									Dados está preparada para assumir o utilizador que muda o estado para "<STRONG>calibrado</STRONG>
									" como o utilizador que efectua o serviço.
									<br />
									dm: falta fazer: quando anulo algo depois de calibrado, tenho de eliminas 
									possiveis ficheiros que possam exisitr em construcao....
									<br />
								</td>
							</tr>
							<tr>
								<td colSpan="4"><asp:label id="lblMessage" Runat="server" ForeColor="#ff0033"></asp:label></td>
								<td rowspan="5"><asp:datagrid id="dgHistorico" Runat="server" CssClass="DG_branco" Font-Size="8" Font-Name="Arial"
										BackColor="Gainsboro" AlternatingItemStyle-BackColor="LightGrey" DataKeyField="idServico" AutoGenerateColumns="false"
										GridLines="Both" BorderColor="#FFFFFF" AllowSorting="false" ShowFooter="false" BorderWidth="2">
										<Columns>
											<asp:BoundColumn HeaderText="Estado" DataField="estado"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="Data Estado" DataField="dataEstado"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="Funcionário" DataField="userEstado"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="Local" DataField="localCalibracao"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="Coment." DataField="comentario"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
							<tr>
								<td colSpan="4"><%=Resources.Resource.RefServ %>:
									<asp:textbox id="txtRefServico" Runat="server"></asp:textbox>&nbsp;<asp:Button class="button" id="btnPesquisa" Runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="Button" onclick="btnPesquisa_Click"></asp:button>
								</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Estado %></td>
								<td><asp:dropdownlist id="ddEstadoOrigem" runat="server" AutoPostBack="true"></asp:dropdownlist></td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td vAlign="top"><%=Resources.Resource.MudarPara %>:</td>
								<td><asp:dropdownlist id="ddEstadoDestino" runat="server" AutoPostBack="True" onselectedindexchanged="ddEstadoDestino_SelectedIndexChanged"></asp:dropdownlist></td>
								<td><%=Resources.Resource.Razao %>:</td>
								<td><asp:dropdownlist id="ddComentarioEstado" Runat="server" DataValueField="idComentarioEstado" DataTextField="comentario"></asp:dropdownlist></td>
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
