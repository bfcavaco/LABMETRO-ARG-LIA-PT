<%@ Page Language="c#" CodeBehind="GestPerfis.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestPerfis"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">

    <fieldset>
        <legend><%=Resources.Resource.GestaoPerfis %></legend>
 
        <br />
        <br />
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><asp:DataGrid
            ID="DGPerfis" runat="server" Width="50%" GridLines="horizontal" BorderColor="#E0E0E0"
            HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True" HeaderStyle-ForeColor="#FFFFFF"
            AllowPaging="false" ShowFooter="True" DataKeyField="ident" OnUpdateCommand="DGPerfis_UpdateGrid"
            OnCancelCommand="DGPerfis_CancelGrid" OnEditCommand="DGPerfis_Edit" AutoGenerateColumns="false">
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
            <Columns>
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
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarRegisto %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid><br />
        <br />
        <%=Resources.Resource.VerPermissoesPerfil %>:<asp:DropDownList ID="ddPerfis" runat="server" DataValueField="ident"
            DataTextField="descricao" AutoPostBack="true">
        </asp:DropDownList>
        &nbsp;&nbsp;
        <br />
        <br />
        <asp:DataList ID="dlUtilizadores" CssClass="DG_branco" runat="server" Width="100%"
            GridLines="both" RepeatDirection="Vertical" BorderWidth="2" RepeatColumns="4"
            BorderColor="#E0E0E0" HeaderStyle-BackColor="#999999" HeaderStyle-Font-Bold="True"
            HeaderStyle-ForeColor="#FFFFFF">
            <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <HeaderTemplate>
                <%=Resources.Resource.UtilizadoresAssociadosAoPerfil %>
            </HeaderTemplate>
            <ItemTemplate>
                <%# DataBinder.Eval(Container, "DataItem.nome") %>&nbsp;&nbsp;
            </ItemTemplate>
        </asp:DataList><br />
        <br />
        <asp:DataGrid 
        ID="DGPaginas" 
        runat="server"
        AllowPaging="false"
        DataKeyField="idPagina" 
        AutoGenerateColumns="false" 
        AllowSorting="False">
            <Columns>
                <asp:BoundColumn HeaderText="<%$ Resources:Resource, Menu %>" DataField="Menu" ReadOnly="True"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Pagina %>">
                    <ItemTemplate>
                        <%#ConverteNome(Convert.ToString(DataBinder.Eval(Container, "DataItem.Pagina"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Acesso %>">
                    <ItemTemplate>
                        <asp:CheckBox ID="checkAcesso" Checked='<%# DataBinder.Eval(Container, "DataItem.acesso") %>'
                            runat="server" Enabled="true"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, EscritaMais %>">
                    <ItemTemplate>
                        <asp:CheckBox ID="checkAcessoTotal" Checked='<%# DataBinder.Eval(Container, "DataItem.acessoTotal") %>'
                            runat="server" BorderColor="red" Enabled='<%# DataBinder.Eval(Container, "DataItem.soLeitura") %>'>
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid><br />
        <asp:Button class="button" ID="btnUpdateDGPaginas" CssClass="button" runat="server"
            Text="<%$ Resources:Resource, Alterar %>"></asp:Button>
        <br />
        <br />
        <!--fim body -->
    </fieldset>
</asp:Content>
