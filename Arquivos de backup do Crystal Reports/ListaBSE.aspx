<%@ Page Language="c#" CodeBehind="ListaBSE.aspx.cs" AutoEventWireup="false" Inherits="LabMetro.ListaBSE" MasterPageFile="~/mp.Master" %>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

 

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.Pesquisar %> BSE's</legend>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <table>
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
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" Text="<%$ Resources:Resource, VerEmpresas %>"
                        CssClass="button"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataTextField="nome" DataValueField="idEmpresa"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBSE %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumBse" runat="server" AutoPostBack="false"></asp:TextBox>
                </td>
                <td colspan="3">
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button">
                    </asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:DataGrid 
        ID="DGBSE" 
        runat="server"
        AutoGenerateColumns="false"
        AllowPaging="true" 
        PageSize="25" 
        AllowSorting="True" 
        OnSortCommand="SortGrid"
        OnPageIndexChanged="DoPaging" 
        DataKeyField="iDBSE">
            <Columns>
                <asp:BoundColumn DataField="refBSE" SortExpression="refBSE" HeaderText="<%$ Resources:Resource, Numero %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="dtBSE" SortExpression="dtBSE" HeaderText="<%$ Resources:Resource, Data %>" DataFormatString="{0:dd/MM/yyyy}">
                </asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormBSE.aspx?btn=DOC&id={0}"
                    DataNavigateUrlField="idBSE" Target="_self">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>

