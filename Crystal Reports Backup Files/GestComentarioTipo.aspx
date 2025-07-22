<%@ Page Language="c#" CodeBehind="GestComentarioTipo.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestComentarioTipo" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    Labmetro - Listar Empresas
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.ComentariosTipoOrcamento %></legend>
        <!-- body -->
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <asp:DataGrid ID="DG" runat="server" AutoGenerateColumns="false" OnEditCommand="DG_Edit"
            OnCancelCommand="DG_CancelGrid" OnUpdateCommand="DG_UpdateGrid" DataKeyField="ident"
            ShowFooter="True" AllowPaging="true" PageSize="25" AllowSorting="True" OnPageIndexChanged="DoPaging"
            OnSortCommand="SortGrid" PagerStyle-Mode="NumericPages" HeaderStyle-ForeColor="#FFFFFF"
            HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999" BorderColor="#E0E0E0"
            GridLines="horizontal">
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Descricao %>" SortExpression="descricao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.descricao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescricao" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>'
                            runat="server" Width="400" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDescricaoFooter" runat="server" Width="400" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:ButtonColumn CommandName="Delete" Text="<%$ Resources:Resource, Remover %>"></asp:ButtonColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarRegisto %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
