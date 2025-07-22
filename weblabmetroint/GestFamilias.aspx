<%@ Page Language="c#" CodeBehind="GestaoFamilias.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestaoFamilias"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">

</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnSubmit").focus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoFamilias %></legend>
        
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <%=Resources.Resource.Grandeza %>:<asp:DropDownList ID="ddGrandeza" runat="server" DataTextField="descricao"
            DataValueField="ident">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;<%=Resources.Resource.Familia %>:<asp:TextBox ID="txtFamilia" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Button
            ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button" OnClick="btnSubmit_Click">
        </asp:Button><br />
        <br />
        <asp:DataGrid
        ID="DGFamilias" 
        runat="server" 
        AutoGenerateColumns="false" 
        OnEditCommand="DGFamilias_Edit"
        OnCancelCommand="DGFamilias_CancelGrid" 
        OnUpdateCommand="DGFamilias_UpdateGrid"
        DataKeyField="idFamilia" 
        ShowFooter="True" 
        AllowPaging="true" 
        PageSize="25" 
        AllowSorting="True"
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid" 
        OnSelectedIndexChanged="DGFamilias_SelectedIndexChanged">
            <Columns>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Grandeza %>" SortExpression="grandeza">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.grandeza") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddGrandezaEdit" runat="server" DataTextField="descricao" DataValueField="ident">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddGrandezaFooter" runat="server" DataTextField="descricao"
                            DataValueField="ident">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Familia %>" SortExpression="descricao">
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
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource,Activo %>"></asp:ListItem>
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
                <asp:BoundColumn DataField="codMaterialSAP" HeaderText="MaterialSAP" SortExpression="codMaterialSAP"></asp:BoundColumn>
                <asp:BoundColumn DataField="idCRM" HeaderText="idCRM" SortExpression="idCrm"></asp:BoundColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText="">
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarFamilia %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
