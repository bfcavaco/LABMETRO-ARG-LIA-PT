<%@ Page Language="c#" CodeBehind="FormRepFacturacao.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormRepFacturacao" MasterPageFile="~/mp.Master" UICulture="pt-PT"%>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">

<!-- %@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %  isto estava no topo -->

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnReport").focus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Estatísticas Facturação</legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server">Data Início:</asp:Label>
                    <asp:Label ID="lblAno" runat="server">Ano:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox>
                    <asp:DropDownList ID="ddAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server">Data Fim:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtFim" runat="server"></asp:TextBox><asp:CheckBox ID="cbPrevisao"
                        Text="   Previsão de Valores" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="4">
                    <asp:DataGrid 
                    ID="dgGrandeza" 
                    runat="server"
                    ShowFooter="false"
                    DataKeyField="ident" 
                    AutoGenerateColumns="false">
                    
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="cbGrandeza"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="Laboratório / Grandeza" DataField="descricao"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="4">
                    <asp:DataList RepeatColumns="3" runat="server" ID="dlTipoEquipamento" 
                        Width="100%" BackColor="#d3d3d3" BorderWidth="2" BorderColor="#FFFFFF"
                        GridLines="both" DataKeyField="ident" RepeatDirection="Horizontal">
                        <ItemStyle Font-Size="7"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chb" runat="server"></asp:CheckBox><%# DataBinder.Eval(Container, "DataItem.descricao") %>&nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="4">
                    <asp:RadioButtonList ID="rblReports" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OnSelectedIndexChangedMethod"  >
                        <asp:ListItem>Facturação por tipo de equipamento</asp:ListItem>
                        <asp:ListItem>Facturação por conjunto de tipos de equipamento</asp:ListItem>
                        <asp:ListItem>Facturação por famílias de equipamentos</asp:ListItem>
                        <asp:ListItem>Facturação por laboratório / grandeza</asp:ListItem>
                        <asp:ListItem>Facturação mensal por laboratório / grandeza</asp:ListItem>
                        <asp:ListItem>Facturação acumulada dos últimos três anos</asp:ListItem>
                        <asp:ListItem>Valores Facturados por Equipamento</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center">
                    <asp:Button class="button" ID="btnReport" runat="server" Text="Ver Report" OnClick="btnReport_Click">
                    </asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
