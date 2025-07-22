<%@ Page Language="c#" CodeBehind="ListaOrcamentosTecnicos.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaOrcamentosTecnicos" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarOrcamentos %></legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><br />
        <br />
        <table>
           
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ReferenciaOrcamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefOrcamento" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
                <tr>
                    <td colspan="4">
                        <asp:Button class="button" ID="btnPesquisa" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CausesValidation="true"
                            CssClass="button" OnClick="btnPesquisa_Click"></asp:Button>
                    </td>
                </tr>
        </table>
        <br />
        <asp:DataGrid 
            ID="DG" 
            runat="server" 
            AutoGenerateColumns="false" 
            AllowPaging="true" 
            PageSize="25"
            AllowSorting="True" 
            OnSortCommand="SortGrid" 
            OnPageIndexChanged="DoPaging"
            DataKeyField="idOrcamento">
            <Columns>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refOrcamento" SortExpression="refOrcamento" HeaderText="<%$ Resources:Resource, RefOrc %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="versao" SortExpression="versao" HeaderText="<%$ Resources:Resource, versao %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtOrcamento" SortExpression="dtOrcamento" HeaderText="<%$ Resources:Resource, DataOrc %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="<%$ Resources:Resource, Estado %>" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                <asp:ButtonColumn CommandName="verOrcamento" HeaderText="<%$ Resources:Resource, Ver %>" Text="<%$ Resources:Resource, Ver %>" ItemStyle-Font-Size="8" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center">
                </asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
