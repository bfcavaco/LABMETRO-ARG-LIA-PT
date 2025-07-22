<%@ Page Language="c#" CodeBehind="GestUtilizadores.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestUtilizadores" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <!-- body -->
    <fieldset>
        <legend><%=Resources.Resource.GestaoUtilizadores %></legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
        <br />
        <br />
        <%=Resources.Resource.Pesquisar %>:
        <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
        <asp:Button class="button" CssClass="button" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" ID="btnSearch">
        </asp:Button>
        <br />
        <br />
        <asp:DataGrid 
        ID="dgFuncionario" 
        runat="server" 
        DataKeyField="idFuncionario" 
        AutoGenerateColumns="false"
        OnEditCommand="EditDgFuncionario" 
        OnCancelCommand="CancelDgFuncionario" 
        OnUpdateCommand="UpdateDgFuncionario"
        PageSize="25" 
        AllowPaging="true"
        ShowFooter="false" 
        AllowSorting="true" 
        OnPageIndexChanged="DoPagingFuncionarios"
        OnSortCommand="SortGridFuncionarios">
            
            <Columns>
                <asp:BoundColumn DataField="nome" HeaderText="<%$ Resources:Resource, Nome %>" SortExpression="Nome" ReadOnly="true">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, UserName %>" SortExpression="username">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.username")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUsernameFuncionario" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.username") %>'>
                        </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="funcao" HeaderText="<%$ Resources:Resource, Funcao %>" SortExpression="funcao" ReadOnly="true">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Perfil %>" SortExpression="perfil">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddPerfilFuncionario" runat="server" DataTextField="descricao"
                            DataValueField="ident">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.perfil")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Password %>">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.passwd")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPasswordFuncionario" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" CancelText="Cancelar" UpdateText="Alterar">
                </asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
        <!-- FIM body -->
    </fieldset>
</asp:Content>
