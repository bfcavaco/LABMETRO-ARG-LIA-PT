<%@ Page Language="c#" CodeBehind="FormBSE.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormBSE"  MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

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
        <legend>BSE</legend>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="SingleParagraph"
            CssClass="lblMessage" ShowSummary="true" HeaderText="Preencha por favor os campos marcados com *.">
        </asp:ValidationSummary>
        <br />
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <table>
            <tr>
                <td colspan="4">
                    <%=Resources.Resource.Empresa %>:&nbsp;
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;&nbsp;
                    <%=Resources.Resource.NIF %>:&nbsp;
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;
                    &nbsp;<asp:Button class="button" ID="btnEmpresas" runat="server" Text="<%$ Resources:Resource, VerEmpresas %>"
                        CssClass="button" CausesValidation="false" Width="80"></asp:Button>
                </td>
            </tr>
            <tr runat="server" id="trEmpresa">
                <td colspan="4">
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataTextField="nome"
                        DataValueField="idEmpresa">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ObservacoesEmpresa %>:
                </td>
                <td>
                    <asp:Label ID="lblObsEmpresa" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
                <td>
                    <%=Resources.Resource.CondicoesPagamentoEmpresa %>:
                </td>
                <td>
                    <asp:Label ID="lblCondPagamEmpresa" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumBSE %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefBSE" runat="server" ReadOnly="True" Width="145px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.DataBSE %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDtBSE" runat="server" ReadOnly="True" Width="145px"></asp:TextBox>
                </td>
                <td>
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
                    <asp:TextBox ID="txtRecebidoPor" runat="server" Width="145px"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Observacoes %>:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="368px"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.FiltrarEquipamentosPorBRE %>:
                </td>
                <td>
                    <%--<asp:TextBox ID="txtNumBRE" runat="server"></asp:TextBox>--%>
                     <asp:DropDownList ID="ddBRE" runat="server" AutoPostBack="true" DataTextField="refBRE"
                            DataValueField="idBRE" 
                        onselectedindexchanged="ddBRE_SelectedIndexChanged">
                        </asp:DropDownList>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="pesquisaPorBRE" CssClass="button" runat="server" Text="<%$ Resources:Resource, MostrarEquipamentos %>">
                    </asp:Button>
                </td>
            </tr>
            <tr>
                <td>Ref.Orç.(ANG):</td>   
                <td><asp:TextBox ID="txtRefOrcamento" MaxLength="150" runat="server"></asp:TextBox></td>
                <td>Ref.Req.Cliente (ANG):</td>
                <td><asp:TextBox ID="txtRefRequisicao" MaxLength="150" runat="server"></asp:TextBox></td>
             </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblEquipamentos" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataGrid ID="dgOrigem" runat="server" AutoGenerateColumns="false" DataKeyField="idServico"
                        AllowSorting="false" ShowFooter="false">
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="checkbox"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, NumBRE %>" DataNavigateUrlFormatString="FormBRE.aspx?btn=DOC&id={0}"
                                DataNavigateUrlField="idBRE" DataTextField="refBRE" SortExpression="refBRE" Target="_blank">
                                <ItemStyle Font-Bold="True" HorizontalAlign="Left"></ItemStyle>
                            </asp:HyperLinkColumn>
                            <asp:BoundColumn HeaderText="<%$ resources:Resource, RefReq %>" DataField="refRequisicao"
                                SortExpression="refRequisicao"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, RefCalib %>" DataField="refServico"
                                SortExpression="refServico"></asp:BoundColumn>
                           <asp:TemplateColumn HeaderText="M.S.*" SortExpression="bVariasGrandezas">
                                <ItemTemplate>
                                    <%# LabMetro.GERAL.clsGeral.ConverteBoolSimNao(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bVariasGrandezas")))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" DataField="codTipoEquipamento"
                                SortExpression="codTipoEquipamento"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Estado %>" DataField="estadoServico"
                                SortExpression="estadoServico"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, LocalCalibracao %>" DataField="localCalibracao"
                                SortExpression="localCalibracao"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Obs %>" DataField="observacoes"
                                SortExpression="observacoes"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Acessorios" DataField="acessorios" SortExpression="acessorios" />
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, EXP %>" DataField="expedicao"
                                SortExpression="expedicao"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button class="button" ID="btnSelectAll" CssClass="button" runat="server" Text="<%$ Resources:Resource, SeleccionarTodos %>">
                    </asp:Button>&nbsp;&nbsp;
                    <asp:Button class="button" ID="btnDeselectAll" CssClass="button" runat="server" Text="<%$ Resources:Resource, LimparTodos %>">
                    </asp:Button>&nbsp;&nbsp;
                    <asp:Button class="button" ID="btnSubmitGrid" CssClass="button" runat="server" Text="<%$ Resources:Resource, MoverEquipamentos %>"
                        CausesValidation="false"></asp:Button>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataGrid ID="dgDestino" runat="server" AutoGenerateColumns="false" DataKeyField="idServico"
                        AllowSorting="false" ShowFooter="false" OnUpdateCommand="updateGrid" OnCancelCommand="cancelGrid"
                        OnEditCommand="editGrid" OnItemCommand="dg_itemCommand">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Id.Req." Visible="false">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.idRequisicao") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RefReq %>">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.refRequisicao") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumBRE%>">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.refBRE") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RefCalib %>">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.refServico") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="M.S.*" SortExpression="bVariasGrandezas">
                                <ItemTemplate>
                                    <%# LabMetro.GERAL.clsGeral.ConverteBoolSimNao(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bVariasGrandezas")))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, CodigoEquipamento %>" SortExpression="codigoEquipamento">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.codTipoEquipamento") %>
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
                            <asp:TemplateColumn HeaderText="Acessorios" SortExpression="acessorios">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.acessorios ")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, EXP %>" SortExpression="expedicao">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.expedicao") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                                ItemStyle-Width="100"></asp:EditCommandColumn>
                            <asp:ButtonColumn CommandName="Delete" Text="<%$ Resources:Resource, Remover %>">
                            </asp:ButtonColumn>
                            <asp:ButtonColumn Text="&lt;img src=&quot;images/pdf_small.gif&quot; border=&quot;0&quot; /&gt;"
                                CommandName="Select" />
                            <asp:ButtonColumn DataTextField="nomeDocumento" CommandName="Select" />
                        </Columns>
                    </asp:DataGrid>
                    <br /> * Ao dar saíde de Serviços assinalados com M.S., verificar por favor se foram efectuados os outros serviços sobre o mesmo equipamento (consultar BRE/serviços).
                    * 
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                </td>
                <td align="center">
                    <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Gravar %>"
                        CausesValidation="true"></asp:Button>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="btnVerBSE" CssClass="button" runat="server" Text="Ver BSE">
                    </asp:Button>&nbsp;
        
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
