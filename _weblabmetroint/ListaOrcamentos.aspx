<%@ Page Language="c#" CodeBehind="ListaOrcamentos.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.ListaOrcamentos" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">


</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.PesquisarOrcamentos %></legend>
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
                    <%=Resources.Resource.EstadoOrcamento %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddEstadoOrcamento" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                </td>

                </tr>
            <tr>
                <td>Funcionario</td>
                <td>
                    <asp:DropDownList ID="ddFuncionario" runat="server" DataTextField="descricao" DataValueField="idFuncionario">
                    </asp:DropDownList></td>
            
            <td></td>
            <td></td>
            </tr>
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
        PagerStyle-Mode="NumericPages"
        DataKeyField="idOrcamento">
        <Columns>
            <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="refOrcamento" SortExpression="refOrcamento" HeaderText="<%$ Resources:Resource, RefOrc %>">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="versao" SortExpression="versao" HeaderText="<%$ Resources:Resource, Versao %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="dtPedido" SortExpression="dtPedido" HeaderText="Dt. Pedido"
                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="dtOrcamento" SortExpression="dtOrcamento" HeaderText="<%$ Resources:Resource, DataOrc %>"
                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
         
                <asp:BoundColumn DataField="valorTotal" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"  SortExpression="valorTotal"></asp:BoundColumn>
            <asp:BoundColumn DataField="Funcionario" SortExpression="Funcionario" HeaderText="<%$ Resources:Resource, Funcionario %>"></asp:BoundColumn>
             <asp:BoundColumn DataField="requisicaoOrdemCompra" SortExpression="requisicaoOrdemCompra" HeaderText="Req. Cliente"></asp:BoundColumn>
            
            <asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="<%$ Resources:Resource, Estado %>" ItemStyle-HorizontalAlign="Center"
                HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
            <asp:HyperLinkColumn HeaderText="Detalhes" Text="<%$ Resources:Resource, Ver %>" DataNavigateUrlFormatString="FormOrcam.aspx?btn=ORC&id={0}"
                DataNavigateUrlField="idOrcamento" Target="_self">
                <ItemStyle HorizontalAlign="center"></ItemStyle>
            </asp:HyperLinkColumn>
        </Columns>
    </asp:DataGrid>
    </fieldset>
</asp:Content>
