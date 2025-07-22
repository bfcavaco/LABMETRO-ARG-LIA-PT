<%@ Page Language="c#" CodeBehind="GestPrecos.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.GestPrecos"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GestaoPrecos %></legend> 
        <asp:GridView id="gvPrecos" runat="server" AllowSorting="False" 
         AutoGenerateColumns="true" Visible="false" />
        <asp:Button 
                ID="btnExportGrid" runat="server" 
                Text="Ver Precario en Excel" OnClick="BtnExportGrid_Click" />


        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%=Resources.Resource.Grandeza %>:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="ddGrandeza" runat="server" DataValueField="ident" DataTextField="descricao"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%=Resources.Resource.Familia %>:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="ddFamilia" runat="server" DataValueField="idFamilia" DataTextField="descricao"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%=Resources.Resource.TipoEquipamento %><br />
         
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="ddTipoEquipamento" runat="server" DataValueField="idTipoEquipamento"
                        DataTextField="descricao" AutoPostBack="true" ></asp:DropDownList>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%=Resources.Resource.TipoServico %>:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="ddTipoServico" runat="server"
                        AutoPostBack="true">
                              <asp:ListItem Text="C" Value="C"></asp:ListItem>
                        <asp:ListItem Text="E" Value="E"></asp:ListItem>
                        <asp:ListItem Text="V" Value="V"></asp:ListItem>
                         <asp:ListItem Text="R" Value="R"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                </td>
            </tr>
            
            <!--===================== PREÇOS directos  ou directos + quantidade==========-->
            <tr>
                <td colspan="2" align="left">
                    <asp:Table ID="tblPrecosDirecto" runat="server" CssClass="msgboxPrecario">
                        <asp:TableRow>
                            <asp:TableCell>Insira aqui os preços para preços directos pelo tipo de equipamento seleccionado, ou por preço e quantidade.</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:DataGrid ID="dgPrecosDirectos" runat="server" SelectedItemStyle-BackColor="#cccccc"
                        CssClass="DG_branco_pq" AutoGenerateColumns="false" PagerStyle-Mode="NumericPages"
                        HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-Font-Bold="True" HeaderStyle-BackColor="#999999"
                        BorderColor="#E0E0E0" DataKeyField="idPrecoDirecto" OnItemCommand="dgPrecosDirectos_ItemCommand"
                        OnEditCommand="dgPrecosDirectos_Edit" OnCancelCommand="dgPrecosDirectos_CancelGrid"
                        OnUpdateCommand="dgPrecosDirectos_UpdateGrid" OnDeleteCommand="dgPrecosDirectos_DeleteCommand"
                        GridLines="Both" ShowFooter="true">
                        <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
                        <FooterStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="#999999"></FooterStyle>
                        <Columns>
                            
                            <asp:TemplateColumn SortExpression="preco" HeaderText="Preço">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrecoEdit" Text='<%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.preco")) %>'
                                        runat="server" Width="30" />
                                    <asp:CompareValidator ID="Comparevalidator15" runat="server" ControlToValidate="txtPrecoEdit"
                                        Type="Double" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.preco")) %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPrecoFooter" runat="server" Width="30" />
                                    <asp:CompareValidator ID="Comparevalidator16" runat="server" ControlToValidate="txtPrecoFooter"
                                        Type="Double" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Preço Exterior">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrecoExteriorEdit" Text='<%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.precoExterior")) %>'
                                        runat="server" Width="30" />
                                    <asp:CompareValidator ID="Comparevalidator17" runat="server" ControlToValidate="txtPrecoExteriorEdit"
                                        Type="Double" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.precoExterior")) %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPrecoExteriorFooter" runat="server" Width="30" />
                                    <asp:CompareValidator ID="Comparevalidator18" runat="server" ControlToValidate="txtPrecoExteriorFooter"
                                        Type="Double" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Preço Móvel">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrecoMovelEdit" Text='<%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.precoMovel")) %>'
                                        runat="server" Width="30" />
                                    <asp:CompareValidator ID="Comparevalidator19" runat="server" ControlToValidate="txtPrecoMovelEdit"
                                        Type="Double" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.precoMovel")) %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPrecoMovelFooter" runat="server" Width="30" />
                                    <asp:CompareValidator ID="Comparevalidator20" runat="server" ControlToValidate="txtPrecoMovelFooter"
                                        Type="Double" Operator="DataTypeCheck"> ! </asp:CompareValidator>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn EditText="editar" UpdateText="alterar" CancelText="cancelar"
                                ItemStyle-Width="30"></asp:EditCommandColumn>
                            <asp:ButtonColumn CommandName="Delete" Text="Apagar"></asp:ButtonColumn>
                            <asp:TemplateColumn HeaderText="">
                                <FooterTemplate>
                                    <asp:LinkButton ID="Linkbutton1" CommandName="insertPreco" runat="server" Text="Adicionar Preço"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <!--=====================FIM PREÇOS directos  ou directos + quantidade=====================-->
                            <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Submeter %>">
                            </asp:Button>
                        </td>
                    </tr>
        </table>
    </fieldset>
</asp:Content>
