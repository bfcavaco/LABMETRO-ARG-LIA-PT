<%@ Page Language="c#" CodeBehind="ListaFuncionarios.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaFuncionarios" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
    <br />
    <br />
    <fieldset><legend><%=Resources.Resource.PesquisarFuncionarios%></legend>
        
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Nome %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Funcao %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddFuncao" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Perfil %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddPerfil" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Laboratorio %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddLaboratorio" runat="server" DataTextField="laboratorio" DataValueField="idLaboratorio">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.LocalCalibracao %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddLocalCalibracao" runat="server" DataTextField="descricao"
                        DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Estado %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server">
                        <asp:ListItem Value="" Text="<%$ Resources:Resource, Todos %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Activo %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Inactivo %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button class="button" CssClass="button" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" ID="btnSearch"
                        OnClick="btnSearch_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:DataGrid 
        ID="DGFuncionarios" 
        runat="server"
        DataKeyField="idFuncionario"
        OnPageIndexChanged="DoPaging" 
        AllowSorting="True" 
        PageSize="25" 
        AllowPaging="true"
        AutoGenerateColumns="false" 
        OnSortCommand="SortGrid" 
        OnSelectedIndexChanged="DGFuncionarios_SelectedIndexChanged">
            <Columns>
                <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="<%$ Resources:Resource, Nome %>" ItemStyle-Width="200px">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Laboratorio" SortExpression="Laboratorio" HeaderText="<%$ Resources:Resource, Lab %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="LocalCalibracao" SortExpression="LocalCalibracao" HeaderText="<%$ Resources:Resource, Local %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="funcao" SortExpression="funcao" HeaderText="<%$ Resources:Resource, Funcao %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="numFuncionario" SortExpression="extensao" HeaderText="Num. Funcionário">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="telefoneDirecto" SortExpression="telefoneDirecto" HeaderText="<%$ Resources:resource, TelDir %>">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="Estado">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Estado"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormFuncionario.aspx?btn=GES&id={0}"
                    DataNavigateUrlField="idFuncionario" Target="_self">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
