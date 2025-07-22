<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormOrcamentoES.aspx.cs" Inherits="LabMetro.FormOrcamentoES" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>



<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    <script type="text/javascript">

        var allcheckBoxSelectorAA = '#<%=dgLinhasOrcamento.ClientID%> input[id*="chkSelectAll"]:checkbox';
        var checkBoxSelectorA = '#<%=dgLinhasOrcamento.ClientID%> input[id*="chkSelected"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxSelectorA),
         checkedCheckboxes = totalCheckboxes.filter(":checked"),
         noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
         allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

            $(allcheckBoxSelectorAA).attr('checked', allCheckboxesAreChecked);
        }

        $(document).ready(function () {
            $(allcheckBoxSelectorAA).live('click', function () {
                $(checkBoxSelectorA).attr('checked', $(this).is(':checked'));

                ToggleCheckUncheckAllOptionAsNeeded();
            });

            $(checkBoxSelectorA).live('click', ToggleCheckUncheckAllOptionAsNeeded);

            ToggleCheckUncheckAllOptionAsNeeded();
        });





    </script>
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
        <legend>
            <%=Resources.Resource.Orcamento %></legend>
        <fieldset>
            <legend>
                <%=Resources.Resource.DetOrcamento %></legend>
            <table>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.ReferenciaOrcamento %>:</label>
                    </td>
                    <td>
                        <asp:Label ID="lblRefOrcamento" runat="server"></asp:Label>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.DataPedido %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDtPedido" runat="server" Width="125px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.Versao %>:</label>
                    </td>
                    <td>
                        <asp:Label ID="lblVersao" runat="server"></asp:Label><br />
                        <asp:Button class="button" ID="btnNovaVersao" runat="server" Width="90" Text="<%$ Resources:Resource, NovaVersao %>"
                            CssClass="button" CausesValidation="false"></asp:Button>&nbsp;<asp:Button class="button"
                                ID="btnReplica" runat="server" Width="90" Text="<%$ Resources:Resource, Replica %>"
                                CssClass="button" CausesValidation="false"></asp:Button>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.DataOrcamento %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDtOrcamento" runat="server" Width="125px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.Estado %>:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddEstado" runat="server" DataTextField="descricao" DataValueField="ident">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.ValMeses %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMesesValidade" runat="server" Width="50" AutoPostBack="true"
                            MaxLength="2"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.DataValidade %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDtValidade" runat="server" Width="125px"></asp:TextBox><asp:RequiredFieldValidator
                            ID="reqdata" runat="server" ControlToValidate="txtDtValidade">*</asp:RequiredFieldValidator>
                    </td>
                     <td>FollowUP:
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cbFollowup" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
                <%=Resources.Resource.DetalhesEmpresa %></legend>
            <table>
                <tr>
                    <td>
                        <%=Resources.Resource.PesquisarEmpresa %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesqEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.NIF %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button class="button" ID="btnEmpresas" runat="server" CausesValidation="false"
                            CssClass="button" Text="<%$ Resources:Resource, PesquisarEmpresa %>"></asp:Button>
                    </td>
                </tr>
                <tr id="trEmpresa" runat="server">
                    <td>
                        <label>
                            <%=Resources.Resource.Empresa %>:</label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                            DataTextField="Nome_">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.ObservacoesEmpresa %>:</label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblObsEmpresa" runat="server" CssClass="lblMessage"></asp:Label>&nbsp:<asp:Label
                            ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.CondicoesPagamentoEmpresa %>:</label>
                    </td>
                    <td>
                        <asp:Label ID="lblCondPagamEmpresa" runat="server" CssClass="lblMessage"></asp:Label>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.DescontoEmPercentagem %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesconto" runat="server" Width="125px" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.Contacto %>:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddContacto" runat="server" DataTextField="descricao" DataValueField="idContacto"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.Departamento %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDepartamento" runat="server" Width="125px" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.Email %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="175px" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.Fax %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax" runat="server" Width="125px" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
                <%=Resources.Resource.DetOrcamento %></legend>
            <table>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.RefCliente %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefCliente" runat="server" Width="175px" MaxLength="85"></asp:TextBox>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.De %>:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddFuncionario" runat="server" DataTextField="descricao" DataValueField="idFuncionario">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.NomeFicheiro %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeFicheiro" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <label>Ficheiro:</label>
                    </td>
                    <td>
                        <input id="fileIn" type="file" size="59" name="fileIn" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HyperLink runat="server" ID="linkFicheiro" Target="new">ver ficheiro
                        </asp:HyperLink></td>
                    <td align="right">

                        <asp:Button class="button" ID="btnRemove"
                            CssClass="button" runat="server"
                            Text="<%$ Resources:Resource, RemoverFicheiro %>"></asp:Button></td>
                    <td align="right" colspan="2">
                        <asp:Button class="button" ID="btnUpload" CssClass="button" runat="server" CausesValidation="false"
                            Text="Carregar Ficheiro"></asp:Button>

                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.TempoExecucao %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTempoExecucao" runat="server" Width="125px" MaxLength="15"></asp:TextBox>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.TrabalhoAEfectuar %>:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddLocalExecucao" runat="server" DataTextField="descricao" DataValueField="ident">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.CalibExt %>:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbCalExterna" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                        <label>
                            <%=Resources.Resource.LocalCalibracao %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLocalidadeCalib" runat="server" Width="125px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=Resources.Resource.Observacoes %>:</label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtObservacoes" runat="server" Width="544px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button class="button" ID="btnHistorico" runat="server" CssClass="button" Text="<%$ Resources:Resource, VerHistoricoEstados %>"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:DataGrid ID="dgHistorico" runat="server" DataKeyField="idOrcamento" AutoGenerateColumns="false"
                            AllowSorting="false" ShowFooter="false">
                            <Columns>
                                <asp:BoundColumn HeaderText="<%$ Resources:Resource, Estado %>" DataField="estado"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="<%$ Resources:Resource, DtEstado %>" DataField="dataEstado"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="<%$ Resources:Resource, UserEstado %>" DataField="userEstado"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <label>
                            <%=Resources.Resource.PesquisarPorEquipamento %></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <label>
                            <%=Resources.Resource.Equipamento %>:<asp:TextBox ID="txtPesquisaEquipamento" runat="server"></asp:TextBox><%=Resources.Resource.Empresa %>:
                            <asp:TextBox ID="txtPesquisaEmpresa" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Button class="button"
                                ID="btnPesquisaEquip" runat="server" CssClass="button" Text="<%$Resources:Resource, Pesquisar %>"></asp:Button>&nbsp;&nbsp;
                            <asp:Button class="button" ID="btnResetPesquisaEquips" runat="server" CssClass="button"
                                Text="<%$ Resources:Resource, LimparResultados %>"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:DataGrid ID="DGEq" runat="server" AutoGenerateColumns="True" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
                <%= Resources.Resource.LinhasOrcamPrecos %></legend>
            <table>
                <tr>
                    <td colspan="4">



                        <asp:DataGrid ID="dgLinhasOrcamento" runat="server" Width="800" DataKeyField="idOrcamentoLinha"
                            AutoGenerateColumns="false" AllowSorting="False" ShowFooter="true">
                            <Columns>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Qtd %>">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtQuantidade" runat="Server" Width="20" />
                                        <asp:CompareValidator ID="Comparevalidator4" runat="server" ControlToValidate="txtQuantidade"
                                            Type="integer" Operator="DataTypeCheck">!</asp:CompareValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.quantidade")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQuantidadeEdit" Text='<%# DataBinder.Eval(Container, "DataItem.quantidade") %>'
                                            runat="server" Width="30" />
                                        <asp:CompareValidator ID="Comparevalidator5" runat="server" ControlToValidate="txtQuantidadeEdit"
                                            Type="integer" Operator="DataTypeCheck">!</asp:CompareValidator>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="<%$ resources:Resource, TipoServico %>">
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddTipoServico" runat="server" DataTextField="descricao" DataValueField="ident"
                                            Font-Size="7" Width="85">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipoServico" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.tipoServico") %>'></asp:Label>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddTipoServicoEdit" runat="server" DataTextField="descricao"
                                            DataValueField="ident" Width="85" Font-Size="7">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Descricao %>">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtDescricao" runat="Server" Width="280" />
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.descricaoEquipamento")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescricaoEdit" Text='<%# DataBinder.Eval(Container, "DataItem.descricaoEquipamento") %>'
                                            runat="server" Width="280" TextMode="MultiLine" />
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" ItemStyle-Width="200">
                                    <FooterTemplate>

                                        <asp:DropDownList ID="ddTipoEquipamento" runat="server" DataTextField="descricao"
                                            DataValueField="ident" Width="220" AutoPostBack="true" OnSelectedIndexChanged="fillPrice">
                                        </asp:DropDownList>

                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddTipoEquipamentoEdit" runat="server" DataTextField="descricao"
                                            DataValueField="ident" Width="222" Font-Size="7" AutoPostBack="true" OnSelectedIndexChanged="fillPriceEdit">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Asterisco %>">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAsteriscoLinha" runat="Server" Width="20" />
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.asterisco")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAsteriscoLinhaEdit" Text='<%# DataBinder.Eval(Container, "DataItem.asterisco") %>'
                                            runat="server" Width="20" />
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$Resources:Resource, ValorUnit %>" ItemStyle-HorizontalAlign="Right"
                                    HeaderStyle-HorizontalAlign="Right">
                                    <FooterTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtValorUnitario" runat="Server" Width="30" />
                                                <asp:CompareValidator ID="compareValUnitario" runat="server" ControlToValidate="txtValorUnitario"
                                                    Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </FooterTemplate>

                                    <ItemTemplate>
                                        <%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString( DataBinder.Eval(Container, "DataItem.valorUnitario"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                        <asp:TextBox ID="txtValorUnitarioEdit" Text='<%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valorUnitario")) %>'
                                            runat="server" Width="40" />
                                        <asp:CompareValidator ID="Comparevalidator1" runat="server" ControlToValidate="txtValorUnitarioEdit"
                                            Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>
                                                  </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, DescontoEmPercentagem %>"
                                    SortExpression="percDescontoLinha" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtPercDesconto" runat="Server" Width="25" />
                                        <asp:CompareValidator ID="Comparevalidator8" runat="server" ControlToValidate="txtPercDesconto"
                                            Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.percDescontoLinha"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPercDescontoEdit" Text='<%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.percDescontoLinha")) %>'
                                            runat="server" Width="25" />
                                        <asp:CompareValidator ID="Comparevalidator7" runat="server" CssClass="lblMessage"
                                            Display="static" ErrorMessage="!Formato" ControlToValidate="txtPercDescontoEdit"
                                            Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, ValorLinhaBR %>" ItemStyle-HorizontalAlign="Right"
                                    HeaderStyle-HorizontalAlign="Right">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtValorLinha" runat="Server" ReadOnly="True" Width="40" />
                                        <asp:CompareValidator ID="Comparevalidator2" runat="server" ControlToValidate="txtValorLinha"
                                            Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString( DataBinder.Eval(Container, "DataItem.valorLinha"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtValorLinhaEdit" Text='<%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valorLinha")) %>'
                                            runat="server" ReadOnly="True" Width="40" />
                                        <asp:CompareValidator ID="Comparevalidator3" runat="server" ControlToValidate="txtValorLinhaEdit"
                                            Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                               
                                <asp:TemplateColumn HeaderText="Estado">
                                    <FooterTemplate>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.EstadoOrcamentoLinha") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddEstadoOrcamentoLinhaEdit" runat="server" DataTextField="descricao"
                                            DataValueField="idEstadoOrcamentoLinha" Width="85" Font-Size="7">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Razao">
                                    <FooterTemplate>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.RazaoOrcamentoLinha") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddRazaoOrcamentoLinhaEdit" runat="server" DataTextField="descricao"
                                            DataValueField="idRazaoOrcamentoLinha" Width="85" Font-Size="7">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn EditText="Edit." UpdateText="Alter." CancelText="Canc." ItemStyle-Width="100"></asp:EditCommandColumn>
                                <asp:ButtonColumn CommandName="Delete" Text="Apag."></asp:ButtonColumn>
                                <asp:TemplateColumn HeaderText="">
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkOrcamentoLinhaAdd" CommandName="Insert" runat="server" Text="<%$Resources:Resource, AdicionarLinha %>" />
                                    </FooterTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        Marcar<asp:CheckBox runat="server" ID="chkSelectAll" />
                                        <%--<asp:CheckBox runat="server" ID="headerSelectCheckbox" ClientIDMode="Static" /> --%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkSelected" />
                                        <%-- <asp:CheckBox runat="server" ID="rowSelectCheckbox" /> --%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="4">
                        <asp:Button ID="btnAprovarTodos" runat="server" Text="Aprovar marcados"
                            OnClick="btnAprovarTodos_Click" />
                        <asp:Button ID="btnRejeitarTodos" runat="server" Text="Rejeitar marcados" OnClick="btnRejeitarTodos_Click" /></td>
                </tr>
                <tr>
                    <td align="left" colspan="4">
                        <!--outra tabela -->
                        <table>
                            <tr>
                                <td align="right">
                                    <%=Resources.Resource.ComTotal %>:<asp:CheckBox ID="chbComTotal" runat="server"></asp:CheckBox>
                                </td>
                                <td align="right">
                                    <%=Resources.Resource.SubTotal %>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorSubTotal" runat="server" Width="43px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%=Resources.Resource.ComDeslocacao %>:
                                    <asp:CheckBox ID="chbDeslocacoes" runat="server"></asp:CheckBox>
                                </td>
                                <td align="right">
                                    <%=Resources.Resource.AjudasCustoDeslocacoes %>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorAjudasCustoDeslocacoes" runat="server" Width="64px" AutoPostBack="True"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="reqv" runat="server" ControlToValidate="txtValorAjudasCustoDeslocacoes"> * </asp:RequiredFieldValidator><asp:CompareValidator
                                            ID="Comparevalidator6" runat="server" ControlToValidate="txtValorAjudasCustoDeslocacoes"
                                            Operator="DataTypeCheck" Type="double">!</asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%=Resources.Resource.ComDesconto %>:<asp:CheckBox ID="chbDesconto" runat="server"></asp:CheckBox>
                                </td>
                                <td align="right">
                                    <%=Resources.Resource.Total %>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorTotal" runat="server" Width="64px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <!--fim outra tabela -->
                                    <%=Resources.Resource.CondicoesPagamento %>:
                                    <asp:DropDownList ID="ddCondicoesPagamento" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="ddCondicoesPagamento">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="right"></td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
                <%=Resources.Resource.Comentario %></legend>
            <table>
                <tr>
                    <td colspan="4">
                        <asp:DataGrid ID="dgComentariosOrcamento" runat="server" DataKeyField="idOrcamentoComentario"
                            AutoGenerateColumns="false" AllowSorting="False" ShowFooter="true">
                            <Columns>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Comentario %>">
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddComentarioTipo" runat="server" DataTextField="descricao"
                                            DataValueField="descricao" AutoPostBack="True" OnSelectedIndexChanged="ddComentarioTipo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtComentario" runat="Server" Width="80%" />
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.descricao") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddComentarioTipoEdit" runat="server" DataTextField="descricao"
                                            DataValueField="descricao" AutoPostBack="True" OnSelectedIndexChanged="ddComentarioTipoEdit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="txtComentarioEdit" Text='<%# DataBinder.Eval(Container, "DataItem.descricao") %>'
                                            runat="server" Width="80%" />
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Asterisco %>">
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAsterisco" runat="Server" Width="75%" />
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.asterisco") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAsteriscoEdit" Text='<%# DataBinder.Eval(Container, "DataItem.asterisco") %>'
                                            runat="server" Width="50%" />
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn EditText="Edit." UpdateText="Alter." CancelText="Canc."></asp:EditCommandColumn>
                                <asp:ButtonColumn CommandName="Delete" Text="Remov."></asp:ButtonColumn>
                                <asp:TemplateColumn HeaderText="">
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkOrcamentoComentarioAdd" CommandName="Insert" runat="server"
                                            Text="<%$ Resources:Resource, AdicionarLinha %>" />
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
                <%=Resources.Resource.AccoesOrcamento%></legend>
            <table>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                            Text="<%$ Resources:Resource, Gravar %>"></asp:Button>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Fax %>:<asp:TextBox ID="txtFaxAlternativo" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnFax" runat="server" Width="200" CausesValidation="false"
                            CssClass="button" Text="<%$ Resources:Resource, EnviarFax %>"></asp:Button>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnVerFax" runat="server" Width="200" CausesValidation="false"
                            CssClass="button" Text="<%$ Resources:Resource, VerFax %>"></asp:Button><asp:Button
                                class="button" ID="btnCarta" runat="server" Width="200" CausesValidation="true"
                                CssClass="button" Text="Ver Carta" Enabled="false" Visible="False"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Email %>:<asp:TextBox ID="txtMailAlternativo" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnEmail" runat="server" Width="200" CausesValidation="false"
                            CssClass="button" Text="<%$ Resources:Resource, EnviarMail %>"></asp:Button>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnVerMail" runat="server" CssClass="button" Text="<%$ Resources:Resource, VerMail %>"
                            CausesValidation="false" Width="200"></asp:Button>
                    </td>
                </tr>
            </table>
            <!-- fim outra tabela -->
            <asp:DataGrid ID="teste" runat="server" AutoGenerateColumns="true">
            </asp:DataGrid>
        </fieldset>
    </fieldset>
</asp:Content>
