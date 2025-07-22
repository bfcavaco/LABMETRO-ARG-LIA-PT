<%@ Control Language="c#" AutoEventWireup="false" Codebehind="MenuInclude.ascx.cs" Inherits="LabMetro.INCLUDES.MenuInclude" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table id="navcontainer" height="600" cellSpacing="0" cellPadding="0" border="0">
	<tr>
		<td class="tdLabMetro" height="70" runat="server" style="BORDER-TOP:#999999 1px solid">
			<P align="center"><IMG src="IMAGES/simbolo-isq-webpeq_tratado_P.gif"><a href="Home.aspx" target="_self"></a></P>
		</td>
	</tr>
	<tr>
		<td class="tdLabMetro">LabMetro</td>
	</tr>
	<tr>
		<td class="tdLabMetro" id="Td1" runat="server" height="15"><asp:button id="btnAlerta" runat="server" Text="Alertas" BorderStyle="none" CausesValidation="false"
				CssClass="btnAlerta"></asp:button></td>
	</tr>
	<tr>
		<td class="tdLabMetro" id="Td2" runat="server" height="15"><a href="IMAGES/Visio-sequenciaestados_Nova_ComSetas_PDF.pdf" target="_blank" class="LinkVermelhoSemUnderline">
				Sequência<br>
				de Estados</a></td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnEmp" runat="server"><asp:button id="btnEmpresa" runat="server" Text="Empresa" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdEmp" runat="server">
			<ul id="navlistEmpresas" runat="server" class="navlist">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnEqu" runat="server"><asp:button id="btnEquipamento" runat="server" Text="Equipamento" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdEqu" runat="server">
			<ul id="navlistEquipamentos" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnGes" runat="server"><asp:button id="btnGestao" runat="server" Text="Gestão" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdGes" runat="server">
			<ul id="navlistGestao" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnOrc" runat="server"><asp:button id="btnOrcamento" runat="server" Text="Orçamento" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdOrc" runat="server">
			<ul id="navlistOrcamentos" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnLab" runat="server"><asp:button id="btnLaboratorios" runat="server" Text="Laboratórios" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdLab" runat="server">
			<ul id="navlistLaboratorios" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnDoc" runat="server"><asp:button id="btnDocumentos" runat="server" Text="Documentos" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdDoc" runat="server">
			<ul id="navlistDocumentos" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnFac" runat="server"><asp:button id="btnFacturacao" runat="server" Text="Facturação" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdFac" runat="server">
			<ul id="navlistFacturacao" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnEst" runat="server"><asp:button id="btnEstatisticas" runat="server" Text="Estatísticas" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdEst" runat="server">
			<ul id="navlistEstatisticas" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnCer" runat="server"><asp:button id="btnCertificado" runat="server" Text="Certificação" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdCer" runat="server">
			<ul id="navlistCertificados" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnCon" runat="server"><asp:button id="btnConcessionarios" runat="server" Text="CTA" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdCon" runat="server">
			<ul id="navlistConcessionarios" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnMar" runat="server"><asp:button id="btnMarcacoes" runat="server" Text="Marcações" CssClass="btnMenu" BorderStyle="none" CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdMar" runat="server">
			<ul id="navlistMarcacoes" runat="server">
			</ul>
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu" id="tdBtnAdm" runat="server"><asp:button id="btnBOAdmin" runat="server" Text="BO Admin" CssClass="btnMenu" BorderStyle="none"
				CausesValidation="false"></asp:button></td>
	</tr>
	<tr>
		<td id="tdAdm" runat="server">
		</td>
	</tr>
	<tr>
		<td class="tdBtnMenu">
			<P><asp:button id="btnLogout" Runat="server" Text="Terminar Sessão" CssClass="btnLogout" BorderStyle="none"
					CausesValidation="false"></asp:button></P>
			<P>&nbsp;</P>
		</td>
	</tr>
	<tr>
		<td align="center" style="BORDER-RIGHT: #cc0000 1px solid; BORDER-TOP: #cc0000 1px solid; FONT-WEIGHT: bold; BORDER-LEFT: #cc0000 1px solid; COLOR: #cc0000; BORDER-BOTTOM: #cc0000 1px solid; FONT-FAMILY: Arial, Verdana; BACKGROUND-COLOR: white">
			<%Response.Write(HttpContext.Current.User.Identity.Name.ToString());%>
		</td>
	</tr>
</table>
<asp:label id="lblTeste" Runat="server"></asp:label>
