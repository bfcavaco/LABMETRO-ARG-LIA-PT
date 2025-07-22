<%@ Page Language="c#" CodeBehind="GestGrandezas.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestGrandezas" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoGrandezas %></legend>
        <!-- body -->
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><br />
        <br />
        <asp:DataGrid 
        ID="DGGrandeza" 
        runat="server" 
        AllowPaging="False" 
        ShowFooter="True"
        DataKeyField="idGrandeza" 
        OnUpdateCommand="DGGrandeza_UpdateGrid" 
        OnCancelCommand="DGGrandeza_CancelGrid"
        OnEditCommand="DGGrandeza_Edit" 
        AutoGenerateColumns="false" 
        AllowSorting="True"
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid"
        OnItemDataBound="DGGrandeza_ItemDataBound">
            <Columns>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Codigo %>" SortExpression="idGrandeza">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.idGrandeza") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtIdGrandezaFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Descricao %>" SortExpression="descricao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.descricao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescricao" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>'
                            runat="server" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDescricaoFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="activo">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.activo"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEstado" runat="server">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Activo %>"></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Inactivo %>"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddEstadoFooter" runat="server">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Activo %>"></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Inactivo %>"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, CodServico %>" SortExpression="codigoServico">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.codigoServico") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCodServico" runat="server">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCodServicoFooter" runat="server">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="Linkbutton1" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarGrandeza %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
