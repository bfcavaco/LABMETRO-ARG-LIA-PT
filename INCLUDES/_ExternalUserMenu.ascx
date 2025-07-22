<%@ Control Language="c#" AutoEventWireup="false" Codebehind="_ExternalUserMenu.ascx.cs" Inherits="LabMetro.INCLUDES.__ExternalUserMenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table id="navcontainer" border="0" bordercolor="red" cellpadding="0" cellspacing="0" align="top"
    height="600">
    <tr>
        <td class="tdLabMetro" height="70">
            <asp:Image ImageUrl="../IMAGES/banner_isq.jpg" Runat="server" id="Image2"></asp:Image></td>
    </tr>
    <tr>
        <td class="tdLabMetro">LabMetro</td>
    </tr>
    <tr>
        <td><ul id="navlist1" runat="server">
                <li>
                    <a href="PedidoOrcamento.aspx">Pedido de Orçamento</a>
                <li>
                    <a href="EstadoEquipamentos.aspx">Estado dos Equipamentos</a>
                <li>
                    <a href="#">Alterar Dados</a>
                <li>
                    <a href="#">Mudar Password</a>
                <li>
                    <a href="#">Pedir Registo</a></li>
            </ul>
        </td>
    </tr>
    <tr>
        <td class="tdBtnMenu"><asp:Button ID="btnLogout" Runat="server" Text="Terminar Sessão" CssClass="btnLogout" BorderStyle="none"></asp:Button></td>
    </tr>
</table>
