<%@ Page Language="c#" CodeBehind="ListaBSESub.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.ListaBSESub"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarBSEsSubcontrato %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumBSESub %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumBSESub" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBRE%>
                </td>
                <td>
                    <asp:TextBox ID="txtNumBRE" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button"
                        OnClick="btnPesquisa_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:DataGrid 
        ID="DGBSESub" 
        runat="server" 
        AutoGenerateColumns="false"
        AllowPaging="true" 
        PageSize="25" 
        AllowSorting="True" 
        OnSortCommand="SortGrid"
        OnPageIndexChanged="DoPaging" 
        DataKeyField="idSubcontratoBSE" >
            <Columns>
                <asp:BoundColumn DataField="refSubcontratoBSE" SortExpression="refSubcontratoBSE"
                    HeaderText="<%$ Resources:Resource, NumBSESub %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtSubcontratoBSE" SortExpression="dtSubcontratoBSE" HeaderText="<%$ Resources:Resource, Data %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refBRE" SortExpression="refBRE" HeaderText="<%$ Resources:Resource, NumBRE%>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="empresaSubcont" SortExpression="empresaSubcont" HeaderText="<%$ Resources:Resource, EmpresaSubcontratada %>">
                </asp:BoundColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, Detalhes %>" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormBSESub.aspx?btn=DOC&id={0}"
                    DataNavigateUrlField="idSubcontratoBSE" Target="_self">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
