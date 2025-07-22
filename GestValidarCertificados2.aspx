

<%@ Page Language="c#" CodeBehind="GestValidarCertificados2.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestValidarCertificados2" EnableEventValidation="false" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.AprovacaoCertificados %></legend>
        <asp:Label ID="lblMessage" runat="server"></asp:Label><br />
        <table>
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td colspan="7">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Resources.Resource.Empresa %>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEmpresa" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <%=Resources.Resource.RefServico %>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchNServico" runat="server"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:Button class="button" ID="btnSearch" runat="server" CssClass="button" Text="<%$ Resources:Resource, Pesquisar %>">
                                </asp:Button>&nbsp;<asp:Button class="button" ID="btnLimparCampos" runat="server"
                                    CssClass="button" Text="<%$ Resources:Resource, LimparCampos %>"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    
        <br />
        <asp:DataGrid 
        ID="dgDocumentos" 
        runat="server" 
        AutoGenerateColumns="False" 
        OnSortCommand="SortGrid" 
        OnItemDataBound="dgDocumentos_ItemDataBound" 
        AllowSorting="True"
        OnItemCommand="visualisarDocumento" 
        AllowPaging="False" 
        DataKeyField="idServico"><Columns>
                <asp:ButtonColumn Visible="false" Text="visualisarDocumento" DataTextField="nomeDocumentoIn"
                    HeaderText="Visualisar Documento" CommandName="Select">
                    <HeaderStyle Width="100px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonColumn>
                <asp:BoundColumn DataField="idServico" Visible="False" SortExpression="idServico"
                    HeaderText="idServico"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="equipamento" SortExpression="equipamento" HeaderText="<%$ Resources:Resource, Equipamento %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="nomeFicheiro" SortExpression="nomeFicheiro" HeaderText="<%$ Resources:Resource, NomeDocumento %>"
                    Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="nomeDocumentoIn" SortExpression="nomeDocumentoIn" HeaderText=""
                    Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="<%$ Resources:Resource, RefServ %>"
                    Visible="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="tipoCertificado" SortExpression="tipoCertificado" HeaderText="<%$ Resources:Resource, TipoCertif %>"
                    Visible="True"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Aprovar %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbAprovar" runat="server" AutoPostBack="False" Checked='<%#DataBinder.Eval(Container, "DataItem.cbAprovar")%>'>
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Rejeitar %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbRejeitar" runat="server" AutoPostBack="False" Checked='<%#DataBinder.Eval(Container, "DataItem.cbRejeitar")%>'>
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Observacoes %>">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <asp:TextBox ID="txtObservacoes" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.obsWorkflowCertificado")%>'>
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="funcionarioValidou" SortExpression="funcionarioValidou"
                    HeaderText="<%$ Resources:Resource, ValidadoPor %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="linguaCertificado" HeaderText="Lingua" ItemStyle-BackColor="Gainsboro"></asp:BoundColumn>
            </Columns>
            <PagerStyle Mode="NumericPages"></PagerStyle>
        </asp:DataGrid><br />
        <asp:Button class="button" ID="btnAprovarAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, AprovarTodos %>">
        </asp:Button>&nbsp;
        <asp:Button class="button" ID="btnRejeitarAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, RejeitarTodos %>">
        </asp:Button>&nbsp;
        <asp:Button class="button" ID="btnDeselectAll" runat="server" CssClass="button" Text="<%$ Resources:Resource, LimparTodos %>">
        </asp:Button>&nbsp;
        
        <asp:CheckBox ID="cbTodos" runat="server" Text="<%$ Resources:Resource, VerTodos %>" AutoPostBack="True"
            Font-Bold="True"></asp:CheckBox>
            
            <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Confirmar %>" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </fieldset>
</asp:Content>
