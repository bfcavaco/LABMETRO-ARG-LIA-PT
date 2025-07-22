<%@ Page Language="c#" CodeBehind="ListaEmpresasSAP.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaEmpresasSAP" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarEmpresasSAP%></legend>
        <table>
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <%=Resources.Resource.FraseProjectoRetalhado %>:
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNIF" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CodClienteSAP%>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumClienteSAP" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumClienteAntigo %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumClienteAntigo" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CodigoBloqueio %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddCodigoBloqueioSap" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.GrupoContas %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddGrupoContas" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="V001" Text="<%$ Resources:Resource, ClientesNacionaisSedes %>"></asp:ListItem>
                        <asp:ListItem Value="V002" Text="<%$ Resources:Resource, ClientesUE %>"></asp:ListItem>
                        <asp:ListItem Value="V003" Text="<%$ Resources:Resource, ClientesOutrosPaises %>"></asp:ListItem>
                        <asp:ListItem Value="V005" Text="<%$ Resources:Resource, FiliaisOutraMorada %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;<asp:Button class="button" ID="btnPesquisa" CssClass="button" runat="server" Text="<%$ Resources:Resource, verEmpresas %>">
                    </asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <br />
    <asp:DataGrid 
    ID="DGEmpresas" 
    runat="server" 
    PagerStyle-Mode="NumericPages"
    OnSortCommand="SortGrid" 
    OnPageIndexChanged="DoPaging" 
    AllowSorting="True" 
    PageSize="25"
    AllowPaging="true" 
    AutoGenerateColumns="True">
    
    </asp:DataGrid>
    <br />
    <br />
    <%=Resources.Resource.Legenda %>:<br />
    <%=Resources.Resource.LegV001 %><br />
    <%=Resources.Resource.LegV002 %><br />
    <%=Resources.Resource.LegV003 %><br />
    <%=Resources.Resource.LegV005 %>
    <!-- FIM body -->
</asp:Content>
