<%@ Page Language="c#" CodeBehind="GestPasswords.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestPasswords" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoPasswords %></legend>
        <asp:Label ID="lblMessage" CssClass="lblMessage" runat="server"></asp:Label>
        <br />
        <br />
        <%=Resources.Resource.Pesquisar %>:
        <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
        <asp:Button class="button" CssClass="button" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" ID="btnSearch"
            OnClick="btnSearch_Click"></asp:Button>
        <br />
        <br />
        <asp:DataGrid 
        ID="dgFuncionario" 
        runat="server" 
        DataKeyField="idUtilizador" 
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
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Password %>">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPasswordFuncionario" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" CancelText="Cancelar" UpdateText="Alterar">
                </asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
