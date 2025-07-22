<%@ Page Language="c#" CodeBehind="GestCodigosSAP.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestCodigosSAP" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>Gestão de Códigos SAP</legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label><br />
        <br />
        <br />
        <asp:DropDownList ID="ddTabelas" runat="server" DataValueField="idTabela" DataTextField="nomeTabela"
            AutoPostBack="True" OnSelectedIndexChanged="ddTabelas_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        <asp:DataGrid 
            ID="DG" 
            runat="server" 
            AutoGenerateColumns="false" 
            OnEditCommand="DG_Edit"
            OnCancelCommand="DG_CancelGrid" 
            OnUpdateCommand="DG_UpdateGrid" 
            DataKeyField="id"
            ShowFooter="True" 
            AllowPaging="false" 
            PageSize="25" 
            AllowSorting="True" 
            OnPageIndexChanged="DoPaging"
            OnSortCommand="SortGrid">
            <Columns>
                <asp:TemplateColumn HeaderText="Valor" SortExpression="valor">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.codigo") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtValor" Text='<%# DataBinder.Eval(Container, "DataItem.codigo") %>'
                            runat="server" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtValorFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Descrição" SortExpression="descricao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.descricao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescricao" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>'
                            runat="server" Width="100%" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDescricaoFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Estado" SortExpression="activo">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.activo"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEstado" runat="server">
                            <asp:ListItem Value="1">activo</asp:ListItem>
                            <asp:ListItem Value="0">inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddEstadoFooter" runat="server">
                            <asp:ListItem Value="1">activo</asp:ListItem>
                            <asp:ListItem Value="0">inactivo</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="Adicionar Registo"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <div class="errorMessage" runat="server" id="Div1">
            Nota: esta tabela não precisa de ser editada, a não ser que seja adicionada uma
            nova grandeza ou haja outra alteração nas grandezas. Ainda a definir se mostro aqui
            todas as grandezas ou só as activas.</div>
    </fieldset>
</asp:Content>
