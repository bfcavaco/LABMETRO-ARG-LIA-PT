<%@ Page Language="c#" CodeBehind="GestLaboratorios.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestLaboratorios" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoLaboratorios %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <asp:DataGrid ID="DGLaboratorios" runat="server" AutoGenerateColumns="false" OnEditCommand="DGLaboratorios_Edit "
            OnCancelCommand="DGLaboratorios_CancelGrid" OnUpdateCommand="DGLaboratorios_UpdateGrid"
            OnItemDataBound="DGLaboratorios_ItemDataBound" OnItemCommand="DGLaboratorios_ItemCommand"
            DataKeyField="idLaboratorio" ShowFooter="True" AllowPaging="False" AllowSorting="True"
            OnPageIndexChanged="DoPaging" OnSortCommand="SortGrid">
            
            <Columns>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Laboratorio %>" SortExpression="grandeza" ItemStyle-Width="30%">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.grandeza") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddGrandezaFooter" runat="server" DataTextField="descricao"
                            DataValueField="ident">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Local %>" SortExpression="LocalLaboratorio">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.LocalLaboratorio") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddLocalLaboratorioFooter" runat="server" DataTextField="descricao"
                            DataValueField="ident">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RegiaoVendas %>" SortExpression="codigoRegiaoVendas">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.codigoRegiaoVendas") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddRegiaoVendas" runat="server">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddRegiaoVendasFooter" runat="server">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, PEP3ProjRetalho %>" SortExpression="codigoPEP">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.codigoPEP") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCodPEP" runat="server">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCodPEPFooter" runat="server">
                        </asp:DropDownList>
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
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="50"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarLaboratorio %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
