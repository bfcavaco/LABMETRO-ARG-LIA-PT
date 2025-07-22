<%@ Page Language="c#" CodeBehind="GestTiposEquipamento.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestTiposEquipamento" MasterPageFile="~/mp.Master" %>

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
        <legend><%=Resources.Resource.GestaoTipoEquipamentos %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
        <br />
        <br />
        <%=Resources.Resource.Grandeza %>:&nbsp;
        <asp:DropDownList ID="ddGrandeza" runat="server" DataTextField="descricao" DataValueField="ident"
            AutoPostBack="True" OnSelectedIndexChanged="ddGrandeza_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        <%=Resources.Resource.Familia %>:&nbsp;
        <asp:DropDownList ID="ddFamilia" runat="server" DataTextField="familia" DataValueField="idFamilia">
        </asp:DropDownList>
        <br />
        <br />
        <%=Resources.Resource.Codigo %>:<asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<%=Resources.Resource.Descricao %>:<asp:TextBox
            ID="txtTipoEquipamento" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Pesquisar %>" CssClass="button" OnClick="btnSubmit_Click">
            </asp:Button><br />
        <br />
        <asp:DataGrid 
        ID="DGTipoEquip" 
        runat="server" 
        AutoGenerateColumns="false" 
        OnEditCommand="DGTipoEquip_Edit"
        OnCancelCommand="DGTipoEquip_CancelGrid" 
        OnUpdateCommand="DGTipoEquip_UpdateGrid"
        DataKeyField="idTipoEquipamento" 
        ShowFooter="True" 
        AllowPaging="true" 
        PageSize="25"
        AllowSorting="True" 
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid" 
        OnSelectedIndexChanged="DGTipoEquip_SelectedIndexChanged">
            <Columns>
            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Familia %>" SortExpression="familia" ItemStyle-Width="300">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.familia") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddFamiliaEdit" runat="server" DataTextField="familia" DataValueField="idFamilia">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFamiliaFooter" runat="server" DataTextField="familia" DataValueField="idFamilia">
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" SortExpression="descricao" ItemStyle-Width="550">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.descricao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescricaoEdit" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>'
                            runat="server" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDescricaoFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Codigo %>" SortExpression="codigo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.codigo") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCodigoEdit" Text='<%# DataBinder.Eval(Container, "DataItem.codigo") %>'
                            runat="server" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCodigoFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Acreditado %>" SortExpression="activo">
                    <ItemTemplate>
                        <%# ConverteEstadoAC(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.acreditado"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddAcreditadoEdit" runat="server">
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, NaoAcreditado %>"></asp:ListItem>
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Acreditado %>"></asp:ListItem>
                          
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddAcreditadoFooter" runat="server">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Acreditado %>"></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, NaoAcreditado %>"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Simbolo %>" SortExpression="simbolo" ItemStyle-Width="100">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.simbolo") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSimboloEdit" Text='<%# DataBinder.Eval(Container, "DataItem.simbolo") %>'
                            runat="server" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtSimboloFooter" runat="server" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>" SortExpression="activo">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.activo"))) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEstadoEdit" runat="server">
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
                        <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarTipo %>"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
               
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
