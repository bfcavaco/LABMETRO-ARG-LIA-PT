<%@ Page Language="c#" CodeBehind="ListaEquipamentos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ListaEquipamentos" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarEquipamentosPorEmpresa %></legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" Text="<%$ Resources:Resource, verEmpresas %>" CssClass="button"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa" DataTextField="nome">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Button class="button" ID="btnSubmit" runat="server" Text="<%$Resources:Resource, PesquisarEquipamentos %>" CssClass="button"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <%=Resources.Resource.FiltrarEquipamentos %>:
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.EstadoEquipamento %>:
                </td>
                <td>
                    <asp:DropDownList ID="ComboAtivoInativo" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>ACTIVE</asp:ListItem>
                        <asp:ListItem>INACTIVE</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="3"></td>
               
            </tr>
            <tr>
             <td>
                    <%=Resources.Resource.NumSerie %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumSerie" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumIdent %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdent" runat="server"></asp:TextBox>
                </td>
                <td colspan="1">
                    <asp:Button class="button" ID="btnSearch" runat="server" Text="<%$Resources:Resource, Filtrar %>" CssClass="button"></asp:Button>&nbsp;<asp:Button ID="btnLimparCampos" runat="server" Text="<%$ Resources:Resource, LimparCampos %>" CssClass="button"></asp:Button>
                </td>
            </tr>
        </table>
        <asp:DataGrid 
        ID="DGEquipamentos" 
        runat="server" 
        AutoGenerateColumns="false" 
        AllowPaging="true" 
        PageSize="25" 
        AllowSorting="True" 
        OnPageIndexChanged="DoPaging" 
        DataKeyField="idEquipamento" 
        OnSortCommand="SortGrid">
            <Columns>
                <asp:BoundColumn DataField="idEquipamento" SortExpression="idEquipamento" HeaderText="<%$ Resources:Resource, ID_BD %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="TipoEquipamento" SortExpression="TipoEquipamento" HeaderText="<%$ Resources:Resource, TipoEquipamento %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="numIdentificacao" SortExpression="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdent %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="numSerie" SortExpression="numSerie" HeaderText="<%$ Resources:Resource, NumSerie %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="Marca" SortExpression="Marca" HeaderText="<%$ Resources:Resource, Marca %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtUltimaCalibracao" SortExpression="dtUltimaCalibracao" HeaderText="<%$ Resources:Resource, DataUltimaCalibracao %>" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="refUltimaCalibracao" SortExpression="refUltimaCalibracao" HeaderText="<%$ Resources:Resource, RefUltimaCalibracao %>"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="Estado">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Estado"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormEquipamento.aspx?btn=EMP&id={0}" DataNavigateUrlField="idEquipamento" Target="_self">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
