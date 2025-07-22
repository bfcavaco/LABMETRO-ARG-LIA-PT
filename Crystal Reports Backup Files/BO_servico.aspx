<%@ Page language="c#" Codebehind="BO_servico.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.BO.BO_servico" %>
<%@Register TagPrefix=menu TagName=inc_menu src="boMenu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:: LabMetro - BO Serviços ::</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
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
				<tr>
					<td><menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu><br />
						<br />
					</td>
				</tr>
				<tr>
					<td class="text_normal" vAlign="top"><!-- body -->
						<table id="tblPesquisa" borderColor="darkgray" border="2" style="FONT-SIZE: 8pt; FONT-FAMILY: Arial"
							width="400">
							<tr>
								<td colSpan="4"><asp:label id="lblMessage" Runat="server" ForeColor="#ff0033"></asp:label></td>
							</tr>
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colSpan="4"><%=Resources.Resource.PesquisarServicos%>:</td>
							</tr>
							<tr class="tblTituloCinzaClaroLetraBranca">
								<td colSpan="4"><%=Resources.Resource.Servicos %>:</td>
							</tr>
							<tr>
								<td><%=Resources.Resource.RefServ %>:</td>
								<td><asp:textbox id="txtRefServico" runat="server"></asp:textbox></td>
								<td><%=Resources.Resource.NumBRE %>:</td>
								<td><asp:textbox id="txtRefBRE" runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td><%=Resources.Resource.Empresa %>:</td>
								<td><asp:textbox id="txtEmpresa" runat="server"></asp:textbox></td>
								<td colSpan="2"><asp:Button class="button" id="btnPesquisa" Runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>" onclick="btnPesquisa_Click"></asp:button></td>
							</tr>
						</table>
						<br />
						<br />
						<div class="lblVermelhaBO"><%=Resources.Resource.FraseBOServico %></div>
						<asp:datagrid id="DGServicos" Runat="server" CssClass="DG_branco" AlternatingItemStyle-BackColor="LightGrey"
							BackColor="Gainsboro" DataKeyField="idServico" BorderWidth="2" GridLines="Both" BorderColor="#FFFFFF"
							PagerStyle-Mode="NumericPages" OnPageIndexChanged="doPaging" OnSortCommand="SortGrid" AllowSorting="True"
							PageSize="25" AllowPaging="true" AutoGenerateColumns="false" Font-Name="Arial" Font-Size="8"
							SelectedItemStyle-BackColor="#99cc99" Width="600" onselectedindexchanged="DGServicos_SelectedIndexChanged">
							<Columns>
								<asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
								<asp:BoundColumn DataField="idServico" SortExpression="idServico" HeaderText="idServico"></asp:BoundColumn>
								<asp:BoundColumn DataField="idgrandeza" SortExpression="idgrandeza" HeaderText="idgrandeza"></asp:BoundColumn>
								<asp:BoundColumn DataField="idequipamento" SortExpression="idequipamento" HeaderText="idequipamento"></asp:BoundColumn>
								<asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="refServico" ReadOnly="True" ItemStyle-Font-Bold="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="estado" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="tipoEquipamento" SortExpression="tipoEquipamento" HeaderText="tipoEquipamento" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="numIdentificacao" SortExpression="numIdentificacao" HeaderText="numIdentificacao" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="numSerie" SortExpression="numSerie" HeaderText="numSerie" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn DataField="idfactura" SortExpression="idfactura" HeaderText="idfactura" ItemStyle-Width="50"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
						<P style="WIDTH:600px">
							<STRONG><FONT face="Verdana" color="#ff0033" size="2"><%=Resources.Resource.FraseBOAtencaoServCalib %>. </FONT></STRONG></P>
						<P style="WIDTH:600px"><FONT face="Verdana" color="#000000" size="2"><%=Resources.Resource.FraseBOServTextoAtencao %>
						</P>
						<div class="lblVermelhaBO"><%=Resources.Resource.FraseEquipSelect %></div>
						<asp:DropDownList ID="ddEquipamentoAManter" Runat="server"></asp:DropDownList>
						<asp:Button class="button" ID="btnTrocar" Runat="server" Text="<%$ Resources:Resource,TrocarEquipamentos %>" onclick="btnTrocar_Click"></asp:Button>
						<br />
						<br />
						<!-- FIM body -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
