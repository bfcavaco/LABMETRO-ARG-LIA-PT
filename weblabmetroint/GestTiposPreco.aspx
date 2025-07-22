<%@ Page Language="c#" CodeBehind="GestTiposPreco.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestTiposPreco" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoTiposPrecos %></legend>
        <!--tabela lado esquerdo-->
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <table id="Table2">
            <tr>
                <td>
                    <%=Resources.Resource.Grandeza %>:
                </td>
                <td style="height: 19px">
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataValueField="ident" DataTextField="descricao"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Familia %>:
                </td>
                <td style="height: 20px">
                    <asp:DropDownList ID="ddFamilia" runat="server" DataValueField="idFamilia" DataTextField="descricao"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:ListBox ID="lbTipoEquipamento" runat="server" DataValueField="idTipoEquipamento"
                        DataTextField="descricao" SelectionMode="Single"></asp:ListBox>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoServico %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoServico" runat="server" DataValueField="ident" DataTextField="descricao"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoPreco %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoPreco" runat="server" DataValueField="idTipoPreco" DataTextField="descricao"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    &nbsp;&nbsp;<asp:Label ID="lblFormula" runat="server"><%=Resources.Resource.Formula %>:</asp:Label>&nbsp;&nbsp;<asp:CheckBox
                        ID="cbFormula" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td style="height: 21px" align="center" colspan="2">
                    <asp:Button class="button" ID="btnSubmit" runat="server" CssClass="button" Text="<%$ Resources:Resource, Submeter %>">
                    </asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend><%=Resources.Resource.FiltrarTiposDef %></legend>
        <table id="Table3">
            <tr>
                <td>
                    <%=Resources.Resource.Grandeza %>:
                </td>
                <td>
                    <asp:TextBox ID="txtGrandeza" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Familia %>:
                </td>
                <td>
                    <asp:TextBox ID="txtFamilia" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="btnSearch" runat="server" CssClass="button" Text="<%$ Resources:Resource, Filtrar %>">
                    </asp:Button>&nbsp;<asp:Button class="button" ID="btnLimparCampos" runat="server"
                        CssClass="button" Text="<%$ Resources:Resource, LimparCampos %>"></asp:Button>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:DataGrid ID="dgPrecario" runat="server" ShowFooter="true" DataKeyField="id"
            OnSortCommand="SortGrid" OnPageIndexChanged="DoPaging" AllowSorting="True" AllowPaging="true"
            AutoGenerateColumns="false" PagerStyle-Mode="NumericPages" HeaderStyle-ForeColor="#FFFFFF"
            HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999" BorderColor="#E0E0E0"
            GridLines="both">
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
            <Columns>
                <asp:BoundColumn DataField="Grandeza" SortExpression="Grandeza" HeaderText="<%$ Resources:Resource, Grandeza %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Familia" SortExpression="Familia" HeaderText="<%$ Resources:Resource, Familia %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="TipoEquipamento" SortExpression="TipoEquipamento" HeaderText="<%$ Resources:Resource, TipoEquipamento %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="TipoServico" SortExpression="TipoServico" HeaderText="<%$ Resources:resource, TipoServico %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="TipoPreco" SortExpression="TipoPreco" HeaderText="<%$ Resources:Resource, TipoPreco %>">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Formula %>">
                    <ItemTemplate>
                        <%# LabMetro.GERAL.clsGeral.ConverteBoolSimNao(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bFormula"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:ButtonColumn CommandName="Delete" Text="<%$ Resources:Resource, Apagar %>"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
