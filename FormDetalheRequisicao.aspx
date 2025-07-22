<%@ Page language="c#" Codebehind="FormDetalheRequisicao.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormDetalheRequisicao" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>:: LabMetro - Detalhe Requisição ::</title>
        <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
        <meta content="C#" name="CODE_LANGUAGE">
        <meta content="JavaScript" name="vs_defaultClientScript">
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <LINK href="Styles.css" type="text/css" rel="stylesheet">
    </HEAD>
    <body>
        <form id="Form1" method="post" runat="server">
            <table class="tblBody" borderColor="#b90000" cellSpacing="0" cellPadding="0" width="700"
                border="0">
                <tr>
                    <td colSpan="4"><asp:label id="lblMessage" Runat="server"></asp:label></td>
                </tr>
                <tr>
                    <td colSpan="4">
                        <asp:datagrid id="DGLinhasRequisicao" CssClass="DG_branco" Runat="server" Width="100%" AllowSorting="false"
                            HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999"
                            BorderWidth="2" BorderColor="#E0E0E0" GridLines="both" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Tipo Serviço">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.TipoServico ")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Tipo Equip.">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.equipamento ")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Desc. Equip.">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.descricaoEquipamento ")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Quant.">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.quantidade ")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Preço.">
                                    <ItemTemplate>
                                        <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToCurrencyString(DataBinder.Eval(Container, "DataItem.preco "))%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Obs.">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.observacoes ")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:datagrid>
                    </td>
                </tr>
            </table>
            <!--FIM tabela principal--></form>
    </body>
</HTML>
