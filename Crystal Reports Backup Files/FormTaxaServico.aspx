
<%@ Page Language="c#" CodeBehind="FormTaxaServico.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormTaxaServico" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>



<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
    <script type="text/javascript">

        var allcheckBoxSelectorAA = '#<%=dgLinhasTaxaServico.ClientID%> input[id*="chkSelectAll"]:checkbox';
        var checkBoxSelectorA = '#<%=dgLinhasTaxaServico.ClientID%> input[id*="chkSelected"]:checkbox';

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

    <style type="text/css">
        .auto-style1 {
            height: 32px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>
           Taxa de Servico</legend>
        <fieldset>
            <legend>
               Detalhes Taxa de Servico</legend>
            <table>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Ref. Taxa Serviço:</label>
                    </td>
                    <td>
                        <asp:Label ID="lblRefTaxaServico" runat="server"></asp:Label>
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
                            Data da Taxa de Serviço</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDtTaxaServico" runat="server" Width="125px"></asp:TextBox>
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
                        
                    </td>
                    <td>
                        
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
                 <tr>
                    <td>
                        <label>
                            CC PARA:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCCMail" runat="server" Width="175px"></asp:TextBox>
                        
                     
                    </td>
                    <td>
                        
                    </td>
                    <td>
                      
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>
               Detalhes</legend>
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
                    <td><label>
                        <%=Resources.Resource.NomeFicheiro %>:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeFicheiro" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                       <label>Ficheiro:</label> 
                    </td>
                    <td><input id="fileIn" type="file" size="59" name="fileIn" runat="server" />
                    </td>
                </tr>
                <tr>
                <td>  <asp:HyperLink runat="server" id="linkFicheiro" Target="new">ver ficheiro
                        </asp:HyperLink></td>
                <td align="right"> 
                
                <asp:Button class="button" ID="btnRemove" 
                        CssClass="button" runat="server" 
                        Text="<%$ Resources:Resource, RemoverFicheiro %>">
                        </asp:Button></td>
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
                    <td>
                        <asp:TextBox ID="txtObservacoes" runat="server" Width="150px"></asp:TextBox>
                    </td>
                     <td>
                        <label>
                            Ref.PSI:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefPSI" runat="server" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>
                            ID PT Comercial:</label>
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtIdPtComercial" runat="server" Width="150px"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator9" runat="server" Operator="DataTypeCheck" Type="Integer" 
 ControlToValidate="txtIdPtComercial" ErrorMessage="Inteiro!" />
                    </td>
                     <td class="auto-style1">
                        <label>
                            Dt. Inser.PT Comercial:</label>
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtDtPTComercial" runat="server" Width="544px" ReadOnly="true"></asp:TextBox>
                        <
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button class="button" ID="btnHistorico" runat="server" CssClass="button" Text="<%$ Resources:Resource, VerHistoricoEstados %>">
                        </asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:DataGrid ID="dgHistorico" runat="server" DataKeyField="idTaxaServico" AutoGenerateColumns="false"
                            AllowSorting="false" ShowFooter="false">
                            <Columns>
                                <asp:BoundColumn HeaderText="<%$ Resources:Resource, Estado %>" DataField="estado">
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="<%$ Resources:Resource, DtEstado %>" DataField="dataEstado">
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="<%$ Resources:Resource, UserEstado %>" DataField="userEstado">
                                </asp:BoundColumn>
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
                            <%=Resources.Resource.Equipamento %>:<asp:TextBox ID="txtPesquisaEquipamento" runat="server"></asp:TextBox><%=Resources.Resource.Empresa %>:</label>
                            <asp:TextBox ID="txtPesquisaEmpresa" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Button class="button"
                                    ID="btnPesquisaEquip" runat="server" CssClass="button" Text="<%$Resources:Resource, Pesquisar %>">
                                </asp:Button>&nbsp;&nbsp;
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
                Linhas:</legend>
            <table>
                <tr>
                    <td colspan="4">
                        <asp:DataGrid ID="dgLinhasTaxaServico" runat="server" Width="800" DataKeyField="idTaxaServicoLinha"
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
                                            DataValueField="ident" Width="220">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddTipoEquipamentoEdit" runat="server" DataTextField="descricao"
                                            DataValueField="ident" Width="222" Font-Size="7">
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
                                        <asp:TextBox ID="txtValorUnitario" runat="Server" Width="30" />
                                        <asp:CompareValidator ID="compareValUnitario" runat="server" ControlToValidate="txtValorUnitario"
                                            Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString( DataBinder.Eval(Container, "DataItem.valorUnitario"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtValorUnitarioEdit" Text='<%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valorUnitario")) %>'
                                            runat="server" Width="40" />
                                        <asp:CompareValidator ID="Comparevalidator1" runat="server" ControlToValidate="txtValorUnitarioEdit"
                                            Type="double" Operator="DataTypeCheck">!</asp:CompareValidator>
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
                                <asp:TemplateColumn HeaderText="Estado">
                                    <FooterTemplate>
                                       
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.EstadoTaxaServicoLinha") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddEstadoTaxaServicoLinhaEdit" runat="server" DataTextField="descricao"
                                            DataValueField="idEstadoLinha" Width="85" Font-Size="7">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                
                                <asp:TemplateColumn HeaderText="Razao">
                                    <FooterTemplate>
                                   
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.RazaoTaxaServicoLinha") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddRazaoTaxaServicoLinhaEdit" runat="server" DataTextField="descricao"
                                            DataValueField="idRazaoLinha" Width="85" Font-Size="7">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>

                                
                          
                                <asp:EditCommandColumn EditText="Edit." UpdateText="Alter." CancelText="Canc." ItemStyle-Width="100">
                                </asp:EditCommandColumn>
                                <asp:ButtonColumn CommandName="Delete" Text="Apag."></asp:ButtonColumn>
                                <asp:TemplateColumn HeaderText="">
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkTaxaServicoLinhaAdd" CommandName="Insert" runat="server" Text="<%$Resources:Resource, AdicionarLinha %>" />
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
                    <td align="right" colspan="4">  <asp:Button ID="btnAprovarTodos" runat="server" Text="Aprovar marcados" 
                            onClick="btnAprovarTodos_Click" />
                        <asp:Button ID="btnRejeitarTodos" runat="server" Text="Rejeitar marcados"  OnClick="btnRejeitarTodos_Click"/></td></tr>
                <tr>
                    <td align="left" colspan="4">
                        <!--outra tabela -->
                        <table>
                            <tr>
                                <td align="right">
                                    <%=Resources.Resource.ComTotal %>:<asp:CheckBox ID="chbComTotal" runat="server">
                                    </asp:CheckBox>
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
                                    <%=Resources.Resource.ComDesconto %>:<asp:CheckBox ID="chbDesconto" runat="server" Enabled="false">
                                    </asp:CheckBox>
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
                                <td align="right">
                                    IVA:
                                </td>
                                <td align="left">                                    
                                    <asp:DropDownList ID="ddIva"  runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td align="left">                                    
                                   
                                </td>
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
                        <asp:DataGrid ID="dgComentariosTaxaServico" runat="server" DataKeyField="idTaxaServicoComentario"
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
                                        <asp:LinkButton ID="lnkTaxaServicoComentarioAdd" CommandName="Insert" runat="server"
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
               Acções:</legend>
            <table>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                            Text="<%$ Resources:Resource, Gravar %>"></asp:Button>
                    </td>
                    <td>
                    </td>
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
                            CausesValidation="false" Width="200" ></asp:Button>
                    </td>
                </tr>

                 <tr>
                    <td>
                         <asp:Button ID="btnPTComercial" runat="server" OnClick="btnPTComercial_Click" Text="Inserir na PT Comercial"></asp:Button>
                    </td>
                    <td>
                  
                      
                    </td>
                     <td>
                         <asp:Label ID="lblMessagePTComercial" runat="server"></asp:Label>
                    </td>
                       
                </tr>
                <tr>
                    <td>
                      
                    </td>
                    <td>
                     
                    </td>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <!-- fim outra tabela -->
            <asp:DataGrid ID="teste" runat="server" AutoGenerateColumns="true">
            </asp:DataGrid>
        </fieldset>
    </fieldset>
           
</asp:Content>
