<%@ Page Language="c#" CodeBehind="FormBRESub.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormBRESub" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

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
        <legend><%=Resources.Resource.BRESubcontrato %></legend>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="SingleParagraph"
            CssClass="lblMessage" ShowSummary="true" HeaderText="Preencha por favor os campos marcados com *.">
        </asp:ValidationSummary>
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
                    <%=Resources.Resource.EmpresaSubcontratada %>:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresaSub" runat="server" Width="360px" DataValueField="ident"
                        DataTextField="descricao" AutoPostBack="true" OnSelectedIndexChanged="ddEmpresaSub_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblEmpresaSub" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBRESub %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefBRESub" runat="server" Width="145px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.DataBRESub %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDtBRESub" runat="server" Width="145px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.FuncionarioRecepcao %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddFuncionarioRecepcao" runat="server" DataValueField="ident"
                        DataTextField="descricao">
                    </asp:DropDownList>
                </td>
                <td>
                   <%=Resources.Resource.EntreguePor %>:
                </td>
                <td>
                    <asp:TextBox ID="txtEntreguePor" runat="server" Width="145px"></asp:TextBox><asp:RequiredFieldValidator
                        ID="Requiredfieldvalidator2" runat="server" ControlToValidate="txtEntreguePor">*</asp:RequiredFieldValidator>
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
                <td colspan="4">
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
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, RefReq %>" DataField="refRequisicao" SortExpression="refRequisicao">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" DataField="codTipoEquipamento" SortExpression="codTipoEquipamento">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, NumIdentificacao %>" DataField="numIdentificacao" SortExpression="numIdentificacao">
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
                        Text="<%$ Resources:Resource, RecepcionarEquipamentos %>" OnClick="btnSubmitGrid_Click"></asp:Button>
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
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RefReq %>">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.idRequisicao") %>
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
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, LocalCalibracao %>" SortExpression="TipoServico">
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
                            <asp:EditCommandColumn EditText="<%$Resources:Resource, Editar %>" UpdateText="<%$ Resources:Resource, Alterar %>" CancelText="<%$ Resources:Resource, Cancelar %>"
                                ItemStyle-Width="100"></asp:EditCommandColumn>
                            <asp:ButtonColumn CommandName="Delete" Text="<%$ Resources:Resource, Remover %>"></asp:ButtonColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button class="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                        Text="<%$ Resources:Resource, Gravar %>" OnClick="btnSubmit_Click"></asp:Button>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td align="center" colspan="2">
                    <asp:Button class="button" ID="btnVerBRESub" CssClass="button" runat="server" Width="200"
                        Text="<%$ Resources:Resource, VerBRESub %>" OnClick="btnVerBRESub_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
