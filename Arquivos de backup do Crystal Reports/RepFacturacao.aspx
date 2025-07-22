<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepFacturacao.aspx.cs"
    Inherits="LabMetro.RepFacturacao" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnSubmit").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.Facturacao %></legend>
        
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblDtInicio" runat="server"><%=Resources.Resource.DataInicio %></asp:Label>
                    <asp:Label ID="lblAno" runat="server"><%=Resources.Resource.Ano %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDtInicio" runat="server"></asp:TextBox>
                    <asp:DropDownList ID="ddAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDtFim" runat="server"><%=Resources.Resource.DataFim %></asp:Label>
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
                    <asp:DataGrid ID="dgGrandeza" runat="server" Width="100%"
                        AlternatingItemStyle-BackColor="#d3d3d3" ShowFooter="false" BorderWidth="2" BorderColor="#FFFFFF"
                        GridLines="horizontal" DataKeyField="ident" AutoGenerateColumns="false">
                        <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="cbGrandeza"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, LaboratorioGrandeza %>" DataField="descricao"></asp:BoundColumn>
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
                        <asp:ListItem Text="<%$ Resources:Resource, FacturacaoPorTipoEquipamento %>"></asp:ListItem>

                        <asp:ListItem Text="<%$ Resources:Resource, FacturacaoPorConjuntoTiposEquipamento %>"></asp:ListItem>

                        <asp:ListItem Text="<%$ Resources:Resource, FacturacaoPorFamiliaEquipamentos %>"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, FacturacaoPorLaboratorioGrandeza %>"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, FacturacaoMensalPorLaboratorioGrandeza %>"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, FacturacaoAcumuladaUltimos3Anos %>"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, ValorFacturadoPorEquipamento %>"></asp:ListItem>
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
