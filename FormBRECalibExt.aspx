<%@ Page Language="c#" CodeBehind="FormBRECalibExt.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormBRECalibExt" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey(event) {
            if (event.Key == 'Enter') {
                document.getElementById("btnSubmit").focus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.BREExterno %></legend>
        <fieldset>
            <legend><%=Resources.Resource.Empresa %></legend>
            <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="<%$ Resources:Resource, AvisoCampoAsterisco %>" ShowSummary="true" DisplayMode="SingleParagraph" />
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
                    <td>
                        <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.NIF %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;
                        &nbsp;<asp:Button class="button" ID="btnEmpresas" CssClass="button" runat="server"
                            Width="80" CausesValidation="false" Text="<%$ Resources:Resource, VerEmpresas %>"></asp:Button>
                    </td>
                </tr>
                <tr id="trEmpresa" runat="server">
                    <td colspan="4" height="20">
                        <%=Resources.Resource.Empresa %>:
                        <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                            DataTextField="nome">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="Tr1">
                    <td>
                        <%=Resources.Resource.EmpresaContratante %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmpresaContratante" runat="server" AutoPostBack="true" OnTextChanged="txtEmpresaContratante_TextChanged1"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.NIF %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNifEmpresaContratante" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;
                        &nbsp;<asp:Button class="button" ID="pesquisaEmpresaContratante" CssClass="button"
                            runat="server" Width="80" CausesValidation="false" Text="<%$ Resources:Resource, VerEmpresas %>" OnClick="pesquisaEmpresaContratante_Click"></asp:Button>
                    </td>
                </tr>
                <tr id="trEmpresaContratante" runat="server">
                    <td height="20">
                        <%=Resources.Resource.Empresa%>:

                    </td>
                    <td>
                        <asp:DropDownList ID="ddEmpresaContratante" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                            DataTextField="nome" OnSelectedIndexChanged="ddEmpresaContratante_SelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td>Cliente do BRE tem acesso aos certificados?</td>
                    <td>
                        <asp:RadioButtonList ID="rbEmpbrepodevercertificados" runat="server" RepeatDirection="Horizontal" Enabled="false">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>" Selected="true"></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.ObservacoesEmpresa %>:&nbsp;
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
                        <%=Resources.Resource.NumBRE %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefBRE" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.DataBRE %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataBRE" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.RecebidoPor %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddFuncionarioRecepcao" runat="server" DataValueField="ident"
                            DataTextField="descricao">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%=Resources.Resource.PedidoPor %>/<br />
                        <%=Resources.Resource.NomeTecnico %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEntreguePor" runat="server" Width="145px"></asp:TextBox><asp:RequiredFieldValidator
                            ID="Requiredfieldvalidator3" runat="server" ControlToValidate="txtEntreguePor">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.TodosComRequisicao %>:
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbReqCompleta" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rbReqCompleta">*</asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2">
                        <%=Resources.Resource.Observacoes %>:
                        <asp:TextBox ID="txtObservacoes" runat="server" Width="368px"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                <td>Taxa Urgência:</td>
                            <td colspan="3">   <asp:RadioButtonList ID="rbTaxaUrgencia" runat="server" RepeatDirection="Horizontal" Enabled="true">
                                    <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>" ></asp:ListItem>
                        
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                            Text="<%$ Resources:Resource, Gravar %>"></asp:Button>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend><%=Resources.Resource.RequisicoesEmpresa %></legend>
            <asp:Label ID="lblRequisicao" runat="server" />
            <asp:DataGrid
                ID="DGRequisicoes"
                runat="server"
                ShowHeader="true"
                ShowFooter="false"
                AllowSorting="false"
                DataKeyField="idRequisicao"
                AutoGenerateColumns="False"
                PageSize="5"
                AllowPaging="True"
                OnPageIndexChanged="DoPagingRequisicoes">
                <Columns>
                    <asp:ButtonColumn CommandName="Select" Text="seleccionar >>"></asp:ButtonColumn>
                    <asp:BoundColumn DataField="idRequisicao" HeaderText="ID"></asp:BoundColumn>
                    <asp:BoundColumn DataField="refRequisicao" HeaderText="<%$ Resources:Resource, RefReq %>"></asp:BoundColumn>
                    <asp:BoundColumn DataField="referenciaCliente" HeaderText="<%$ Resources:Resource, RefCliente %>"></asp:BoundColumn>
                    <asp:BoundColumn DataField="dtRequisicao" HeaderText="<%$ Resources:Resource, DtReq %>" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="dtValidade" HeaderText="<%$ Resources:Resource, DtVal %>" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                                ID="Hyperlink1" Target="new">
														<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </fieldset>
        <fieldset>
            <legend><%=Resources.Resource.EquipamentosDaEmpresa %></legend>
            <table id="tblPesquisaEquipamento">
                <tr>
                    <td>
                        <%=Resources.Resource.EquipamentosDaEmpresa %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddTipoEquipamentoPesquisa" runat="server" DataValueField="idTipoEquipamento"
                            DataTextField="descricao">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%=Resources.Resource.NumIdentificacao %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesquisaNumIdentificacao" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.NumSerie %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesquisaNumSerie" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.RefCalib %>.
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesquisaRefCalibracao" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button class="button" ID="btnPesquisaEquipamentoEmpresa" CssClass="button" runat="server"
                            CausesValidation="false" Text="<%$ Resources:Resource, PesquisarListarEquipamentos %>"></asp:Button>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
            <asp:DataGrid
                ID="DGEquipamentos"
                runat="server"
                ShowFooter="true"
                AllowSorting="true"
                DataKeyField="idEquipamento"
                AutoGenerateColumns="false"
                PageSize="15" AllowPaging="false"
                OnSortCommand="SortGridEquipamento">
                <Columns>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="checkbox"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" SortExpression="tipoEquipamento">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddTipoEquipamentoFooter" runat="server" DataTextField="descricao"
                                DataValueField="ident">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn HeaderText="<%$ Resources:Resource, Codigo %>" ReadOnly="True" DataField="codigoEquipamento"
                        SortExpression="codigoEquipamento"></asp:BoundColumn>
                    <asp:BoundColumn HeaderText="<%$ Resources:Resource, ReferenciaUltimaCalibracao %>" ReadOnly="false" DataField="refUltimaCalibracao"
                        SortExpression="refUltimaCalibracao"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumSerie %>" SortExpression="numSerie">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox runat="server" ID="txtNumSerieFoter"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="numSerie" SortExpression="numSerie"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumIdentificacao %>" SortExpression="numIdentificacao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNumIdentificacaoFooter" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="numIdentificacao"></asp:BoundColumn>
                    <asp:BoundColumn HeaderText="" DataField="idTipoEquipamento" Visible="false"></asp:BoundColumn>
                    <asp:TemplateColumn>
                        <FooterTemplate>
                            <asp:LinkButton ID="Linkbutton1" CommandName="Insert" runat="server" Text="<%$ Resources:REsource, InserirEquipamento %>"
                                CausesValidation="false" />
                        </FooterTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </fieldset>
        <fieldset>
            <legend><%=Resources.Resource.ServicosBRE %></legend>
            <table>
                <tr>
                    <td>
                        <%=Resources.Resource.NumVezes %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumVezes" runat="server" Width="32px">1</asp:TextBox><asp:CompareValidator
                            ID="Comparevalidator32" runat="server" ControlToValidate="txtNumVezes" Operator="DataTypeCheck"
                            Type="Integer">int!</asp:CompareValidator><asp:RequiredFieldValidator ID="Requiredfieldvalidator2"
                                runat="server" ControlToValidate="txtNumVezes">*</asp:RequiredFieldValidator><span style="font-weight: normal; font-size: 9px"><%=Resources.Resource.NotaUsarCampo %>
                                </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.TipoServico %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddTipoServico" runat="server" DataValueField="ident" DataTextField="descricao">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.LocalDestino %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddLocalDestino" runat="server" DataValueField="ident" DataTextField="descricao">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Estado %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddEstado" runat="server" DataValueField="idEstadoServico" DataTextField="descricao">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button class="button" ID="btnInserirServicos" CssClass="button" runat="server"
                            CausesValidation="false" Text="<%$ Resources:Resource, AdicionarEquipBRE %>"></asp:Button>
                    </td>
                </tr>
            </table>
            <asp:DataGrid ID="DGServicosBRE" runat="server"
                ShowFooter="false" AllowSorting="false" DataKeyField="idServico"
                AutoGenerateColumns="false" OnUpdateCommand="DGServicosBRE_UpdateGrid" OnCancelCommand="DGServicosBRE_CancelGrid"
                OnEditCommand="DGServicosBRE_Edit" BorderWidth="2">

                <Columns>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <%# Container.ItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="idEquipamento" HeaderText="<%$ Resources:Resource, IDEquipamentoBD %>" ReadOnly="true"
                        Visible="false"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, IDRequisicao %>" Visible="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.idRequisicao") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RefReq %>">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.RefRequisicao") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddRequisicaoEdit" runat="server" DataValueField="idRequisicao"
                                DataTextField="refRequisicao">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, IDEquipamentoBD %>" SortExpression="Equipamento" Visible="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.idEquipamento") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, CodigoEquipamento %>" SortExpression="codTipoEquipamento">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.codTipoEquipamento") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddCodTipoEquipamentoEdit" runat="server" DataValueField="idEquipamento"
                                DataTextField="codigoEquipamento" OnSelectedIndexChanged="dd_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumIdent %>" SortExpression="numIdentificacao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddNumIdentEdit" runat="server" DataTextField="numIdentificacao"
                                DataValueField="idEquipamento" OnSelectedIndexChanged="dd_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, ReferenciaUltimaCalibracao %>" SortExpression="refUltimaCalibracao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddRefUltimaCalibracaoEdit" runat="server" DataTextField="refUltimaCalibracao"
                                DataValueField="idEquipamento" OnSelectedIndexChanged="dd_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumSerie %>" SortExpression="numSerie">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddNumSerieEdit" runat="server" DataValueField="idEquipamento"
                                DataTextField="numSerie" OnSelectedIndexChanged="dd_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Estado %>">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EstadoServico") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEstadoServicoEdit" runat="server" DataTextField="descricao"
                                DataValueField="idEstadoServico">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="idEstadoServico" Visible="false"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, TipoServico %>" SortExpression="TipoServico">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.TipoServico") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, LocalDestino %>" SortExpression="TipoServico">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddLocalDestinoEdit" runat="server" DataTextField="descricao"
                                DataValueField="ident">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.LocalDestino") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, observacoes %>" SortExpression="Observacoes">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.observacoes ")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtObservacoesEdit" Text='<%# DataBinder.Eval(Container, "DataItem.observacoes") %>'
                                runat="server" />
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:ButtonColumn CommandName="Delete" Text="Remover"></asp:ButtonColumn>
                    <asp:EditCommandColumn EditText="<%$ Resources:Resource, Editar %>" UpdateText="<%$ Resources:Resource, Alterar %>" CancelText="<%$ Resources:Resource, Cancelar %>"
                        ItemStyle-Width="100"></asp:EditCommandColumn>
                    <asp:BoundColumn DataField="idTipoEquipamento" Visible="False"></asp:BoundColumn>
                    <asp:BoundColumn DataField="refServico" Visible="true" ReadOnly="true"></asp:BoundColumn>
                    <asp:TemplateColumn>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddMarteladaIdTipoEquipamento" runat="server" DataTextField="idTipoEquipamento"
                                DataValueField="idEquipamento" Visible="False" AutoPostBack="False">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            <table>
                <tr>
                    <td>
                        <%=Resources.Resource.EquipamentosDefinitivos %>:<asp:CheckBox ID="chbDadosDefinitivos" runat="server"></asp:CheckBox>
                    </td>
                    <td colspan="3">
                        <asp:Button class="button_confirm" ID="btnGravar" runat="server" Text="<%$ Resources:Resource, Gravar %>"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button class="button" ID="btnVerBRE" CssClass="button" runat="server" Width="170"
                            Text="<%$ Resources:Resource, VerBRE %>"></asp:Button>
                    </td>
                    <td align="center" colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button class="button" ID="btnVerEtiquetas" CssClass="button" runat="server"
                            Width="170" Text="<%$ Resources:Resource, verEtiquetas %>"></asp:Button>
                    </td>
                    <td align="center" colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button class="button" ID="btnListaEquipamentos" CssClass="button" runat="server"
                            Width="170" Text="<%$ Resources:Resource, ListaEquipamentos %>"></asp:Button>
                    </td>
                    <td align="center" colspan="2"></td>
                </tr>
            </table>
        </fieldset>
    </fieldset>
</asp:Content>
