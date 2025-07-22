<%@ Page Language="c#" CodeBehind="FormBSESub.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormBSESub"  MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

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
        <legend><%=Resources.Resource.BSESubcontrato %></legend>
        <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
            ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" Width="360px" DataValueField="ident"
                        DataTextField="descricao" AutoPostBack="true" OnSelectedIndexChanged="ddEmpresa_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblEmpresa" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.EmpresaASubcontratar %>:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresaSub" runat="server" Width="360px" DataValueField="ident"
                        DataTextField="descricao" OnSelectedIndexChanged="ddEmpresaSub_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblEmpresaSub" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBSESub %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefBSESub" runat="server" Width="145px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.DataBSESub %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDtBSESub" runat="server" Width="145px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.FuncionarioSaida %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddFuncionarioSaida" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                    <asp:Label ID="lblFuncionarioSaida" runat="server"></asp:Label>
                </td>
                <td>
                    <%=Resources.Resource.RecebidoPor %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRecebidoPor" runat="server" Width="145px"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRecebidoPor">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Observacoes %>:
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="355px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                    Certificado em nome do Cliente Final:
                </td>
                <td colspan="4">
                  <asp:CheckBox ID="cbCertNomeCliente" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblEquipamentos" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataGrid 
                    ID="dgOrigem" 
                    runat="server" 
                    OnSortCommand="SortGridOrigem"
                    ShowFooter="false" 
                    AllowSorting="true"
                    DataKeyField="idServico"
                    AutoGenerateColumns="false">
                    <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="checkbox"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="RefServ." DataField="refServico" SortExpression="refServico">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" DataField="codTipoEquipamento" SortExpression="codTipoEquipamento">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, NumIdent %>" DataField="numIdentificacao" SortExpression="numIdentificacao">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Estado %>" DataField="estadoServico" SortExpression="estadoServico">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, DtEstado %>" DataField="dtEstado" SortExpression="dtEstado"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, TipoServico %>" DataField="tipoServico" SortExpression="tipoServico">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, LocalCalibracao %>" DataField="localCalibracao" SortExpression="localCalibracao">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Obs %>" DataField="observacoes" SortExpression="observacoes">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button class="button" ID="btnSubmitGrid" CssClass="button" runat="server" CausesValidation="false"
                        Text="<%$ Resources:Resource, SubcontratarEquipamentos %>" OnClick="btnSubmitGrid_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataGrid 
                    ID="dgDestino" 
                    runat="server"
                    ShowFooter="false"
                    AllowSorting="false"
                    DataKeyField="idServico"
                    AutoGenerateColumns="false" 
                    OnEditCommand="editGrid" 
                    OnCancelCommand="cancelGrid"
                    OnUpdateCommand="updateGrid">
                        <Columns>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, IDRequisicao %>" Visible="false">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.idRequisicao") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="RefServ.">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.refServico") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, CodigoEquipamento %>" SortExpression="codigoEquipamento">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.codTipoEquipamento") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumIdent %>" SortExpression="numIdentificacao">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.estadoServico") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddEstadoServicoEdit" runat="server" DataTextField="descricao"
                                        DataValueField="idEstadoServico">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, TipoServico %>" SortExpression="TipoServico">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.tipoServico") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:REsource, LocalCalibracao %>" SortExpression="TipoServico">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.localCalibracao") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Observacoes %>" SortExpression="observacoes">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.observacoes ")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtObservacoesEdit" Text='<%# DataBinder.Eval(Container, "DataItem.observacoes") %>'
                                        runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                                ItemStyle-Width="100"></asp:EditCommandColumn>
                            <asp:ButtonColumn CommandName="Delete" Text="<%$ Resources:Resource, Remover %>"></asp:ButtonColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button class="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                        Text="<%$ Resources:Resource, Gravar %>" OnClick="btnSubmit_Click"></asp:Button>
                </td>
                <td colspan="2" align="center">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td colspan="2" align="center">
                    <asp:Button class="button" ID="btnVerBSESub" runat="server" Text="<%$ Resources:Resource, VerBSESub %>"
                        Width="200" CssClass="button" OnClick="btnVerBSESub_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
