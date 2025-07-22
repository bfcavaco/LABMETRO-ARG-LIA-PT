<%@ Page Language="c#" CodeBehind="FormBRE.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormBRE"  MasterPageFile="~/mp.Master" 
    MaintainScrollPositionOnPostback="false"  %>

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
        <legend>BRE</legend>
        <fieldset>
            <legend>
                <%=Resources.Resource.Empresa%></legend>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanelEmpresa">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label><br />
                    <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
                        ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
                    <table width="100%">
                        <tr>
                            <td>
                                <%=Resources.Resource.Empresa%>:
                                <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" OnTextChanged="txtEmpresaTextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <%=Resources.Resource.NIF%>:<asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"
                                    OnTextChanged="txtNifTextChanged"></asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Button class="button" ID="btnEmpresas" CssClass="button" runat="server" Width="80"
                                    CausesValidation="false" Text="<%$ Resources:Resource, verEmpresas %>" />
                            </td>
                        </tr>
                        <tr id="trEmpresa" runat="server">
                            <td colspan="3">
                                <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                                    DataTextField="nome" Width="100%">
                                </asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage" />
                            </td>
                        </tr>
                        <tr runat="server" id="Tr1">
                            <td>
                                <%=Resources.Resource.EmpresaContratante %>:
                                <asp:TextBox ID="txtEmpresaContratante" runat="server" AutoPostBack="true" OnTextChanged="txtEmpresaContratante_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <%=Resources.Resource.NIF%>: &nbsp;
                                <asp:TextBox ID="txtNifEmpresaContratante" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;
                            </td>
                            <td>
                                &nbsp;<asp:Button class="button" ID="pesquisaEmpresaContratante" CssClass="button"
                                    runat="server" Width="80" CausesValidation="false" Text="<%$ Resources:Resource, verEmpresas %>">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr id="trEmpresaContratante" runat="server">
                            <td  height="20">
                                <%=Resources.Resource.Empresa%>:

                            </td>
                           <td>                                <asp:DropDownList ID="ddEmpresaContratante" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                                    DataTextField="nome" OnSelectedIndexChanged="ddEmpresaContratante_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td>Cliente do BRE tem acesso aos certificados?</td>
                            <td>   <asp:RadioButtonList ID="rbEmpbrepodevercertificados" runat="server" RepeatDirection="Horizontal" Enabled="false">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>" Selected="true" ></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                        </asp:RadioButtonList>

                          </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Resources.Resource.ObservacoesEmpresa%>:&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblObsEmpresa" runat="server" CssClass="lblMessage"></asp:Label>
                            </td>
                            <td>
                                <%=Resources.Resource.CondicoesPagamentoEmpresa%>:
                            </td>
                            <td>
                                <asp:Label ID="lblCondPagamEmpresa" runat="server" CssClass="lblMessage"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                    <td>
                       <%=Resources.Resource.RefRequisicaoANG%>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtReferenciaRequisicao" runat="server" MaxLength="150"></asp:TextBox>
                    </td>
                    <td>
                          <%=Resources.Resource.RefOrcamentoANG%>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOrcamento" runat="server" DataTextField="refOrcamento" DataValueField="idOrcamento"></asp:DropDownList>
                          <asp:DropDownList ID="ddEstadoOrcamento" runat="server">
                              <asp:ListItem Value="0">--</asp:ListItem>
                              <asp:ListItem Value="4" Text="<%$ Resources:Resource, Aprovado %>"></asp:ListItem>
                              <asp:ListItem Value="9" Text="<%$ Resources:Resource, AprovadoParcialmente %>"></asp:ListItem>
                          </asp:DropDownList>
                    </td>
               <%-- </tr>
                         <tr>
                    <td>
                       Requisições incompletas: (só para associar a requisicao a todos os serviços do bre!)
                    </td>
                    <td>
                          <asp:DropDownList ID="ddRequisicao" runat="server" DataTextField="refRequisicao" DataValueField="idRequisicao"></asp:DropDownList>
                          <asp:DropDownList ID="ddRequisicaoCompleta" runat="server">
                              <asp:ListItem Value="0">--</asp:ListItem>
                              <asp:ListItem Value="1" Text="Completa"></asp:ListItem>
                              <asp:ListItem Value="0" Text="Incompleta"></asp:ListItem>
                          </asp:DropDownList>
                    </td>
                    <td>
                         
                    </td>
                    <td>
                    
                    </td>
                </tr>--%>

                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <legend>
                <%=Resources.Resource.DadosBRE %></legend>
            <table>
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
                        <%=Resources.Resource.EntreguePor %>:
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
                        <asp:RequiredFieldValidator ID="reqReqCompleta" runat="server" ControlToValidate="rbReqCompleta">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <%=Resources.Resource.Expedicao %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtExpedicao" runat="server" Width="145px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Observacoes %>:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtObservacoes" runat="server" Width="368px"></asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td>
                        <%=Resources.Resource.Notas %>:
                    </td>
                    <td colspan="3">
                       <asp:DropDownList ID="ddNota" runat="server" DataTextField="descricao" DataValueField="descricao">
                         

                       </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td>Taxa Urgência MET LEGAL:</td>
                            <td colspan="3">   <asp:RadioButtonList ID="rbTaxaUrgencia" runat="server" RepeatDirection="Horizontal" Enabled="true">
                                  
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>" ></asp:ListItem>
                                  <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>" Selected="True"></asp:ListItem>
                        
                        </asp:RadioButtonList></td>
</tr>
            </table>
            <!--****************************************************************-->
            <asp:Label ID="lblRequisicao" runat="server" />
            <asp:DataGrid ID="DGRequisicoes" runat="server" ShowFooter="true" ShowHeader="true"
                AllowSorting="false" DataKeyField="idRequisicao" AutoGenerateColumns="False"
                OnPageIndexChanged="DoPagingRequisicoes" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages">
                <Columns>
                    <asp:ButtonColumn CommandName="SelectReq" Text="seleccionar >>"></asp:ButtonColumn>
                    <asp:BoundColumn DataField="idRequisicao" HeaderText="ID"></asp:BoundColumn>
                    <asp:BoundColumn DataField="refRequisicao" HeaderText="ref.Requis."></asp:BoundColumn>
                    <asp:BoundColumn DataField="referenciaCliente" HeaderText="ref.Cliente"></asp:BoundColumn>
                    <asp:BoundColumn DataField="dtRequisicao" HeaderText="Dt.Req." DataFormatString="{0:dd/MM/yyyy}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="dtValidade" HeaderText="Dt.Val." DataFormatString="{0:dd/MM/yyyy}">
                    </asp:BoundColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                                ID="Hyperlink1" Target="new">
														<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid><br />
            <br />
            <fieldset>
                <legend>
                    <%=Resources.Resource.EquipamentosDoCliente %></legend>
                <table id="tblPesquisaEquipamento" runat="server">
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
                            <%=Resources.Resource.RefCalib %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPesquisaRefCalibracao" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.NumIdent %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPesquisaNumIdentificacao" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <%=Resources.Resource.NumSerie %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPesquisaNumSerie" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <p>
                                <asp:Button class="button" ID="btnPesquisaEquipamentoEmpresa" CssClass="button" runat="server"
                                    CausesValidation="false" Text="<%$ Resources:Resource, PesquisarListarEquipamentos %>"
                                    Enabled="false"></asp:Button><%=Resources.Resource.SeleccionarSetaCodEquip %>
                            </p>
                        </td>
                    </tr>
                </table>
                <asp:DataGrid ID="DGEquipamentos" runat="server" ShowFooter="true" AllowSorting="true"
                    GridLines="both" DataKeyField="idEquipamento" AutoGenerateColumns="false" OnPageIndexChanged="DoPagingEquipamentos"
                    OnSortCommand="SortGridEquipamento" Width="100%" OnItemCommand="DGEquipamentos_ItemCommand">
                    <Columns>
                        <asp:ButtonColumn CommandName="Select" Text="<%$ Resources:Resource, Seleccionar %>">
                        </asp:ButtonColumn>
                        <asp:TemplateColumn HeaderText="<%$ Resources:Resource, TipoEquipamento %>" SortExpression="tipoEquipamento">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddTipoEquipamentoFooter" runat="server" DataTextField="descricao"
                                    DataValueField="ident" AutoPostBack="False" Width="300">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="<%$ Resources:Resource, Codigo %>" ReadOnly="True" DataField="codigoEquipamento"
                            SortExpression="codigoEquipamento"></asp:BoundColumn>
                        <asp:BoundColumn HeaderText="<%$ Resources:Resource, ReferenciaUltimaCalibracao %>"
                            ReadOnly="True" DataField="refUltimaCalibracao" SortExpression="refUltimaCalibracao">
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="<%$ Resources:Resource, NumSerie %>" DataField="numSerie"
                            Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumSerie %>" SortExpression="numSerie">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtNumSerieFoter" Width="100"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="<%$ Resources:Resource, IdCliente %>" DataField="numIdentificacao"
                            Visible="false"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumIdentificacao %>" SortExpression="numIdentificacao">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtNumIdentificacaoFooter" runat="server" Width="100"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <FooterTemplate>
                                <asp:LinkButton ID="Linkbutton1" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarEquipamento %>"
                                    CausesValidation="false" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </fieldset>
            <fieldset>
                <legend>
                    <%=Resources.Resource.ServicosBRE %></legend>
                <asp:DataGrid ID="DGServicosBRE" runat="server" ShowFooter="true" AllowSorting="false"
                    AutoGenerateColumns="false" OnUpdateCommand="DGServicosBRE_UpdateGrid" OnCancelCommand="DGServicosBRE_CancelGrid"  OnEditCommand="DGServicosBRE_Edit">
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <%# Container.ItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" HeaderText="Id.Req.">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.idRequisicao") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtIdRequisicaoFooter" runat="server" Enabled="False" Width="25"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RefReq %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.RefRequisicao") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtRefRequisicaoFooter" runat="server" Enabled="False" Width="60"></asp:TextBox><br />
                                <asp:Button class="button" ID="btnAddRequisicao" runat="server" Text=">>" OnClick="BindGridRequisicoes"
                                    CausesValidation="false"></asp:Button>
                            </FooterTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddRequisicaoEdit" runat="server" DataValueField="idRequisicao"
                                    DataTextField="refRequisicao">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" SortExpression="Equipamento" HeaderText="<%$ Resources:Resource, IDEquipamentoBD %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.idEquipamento") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtIdEquipamentoFooter" runat="server" Enabled="False" Width="60"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="codTipoEquipamento" HeaderText="<%$ Resources:Resource, CodigoEquipamento %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.codTipoEquipamento") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCodigoEquipamentoFooter" runat="server" Enabled="False" Width="60"></asp:TextBox><br />
                                <asp:Button class="button" ID="Button1" runat="server" Text=">>" OnClick="BindGridEquipamentos"
                                    CausesValidation="false"></asp:Button>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdent %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtNumIdFooter" runat="server" Enabled="False" Width="60"></asp:TextBox><br />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="numSerie" HeaderText="<%$ Resources:Resource, NumSerie %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtNumSerieFooter" runat="server" Enabled="False" Width="60"></asp:TextBox><br />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="<%$ Resources:resource, Estado %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.EstadoServico") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddEstadoServicoFooter" runat="server" DataTextField="descricao"
                                    DataValueField="idEstadoServico">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="TipoServico" HeaderText="<%$ Resources:Resource, TipoServico %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.TipoServico") %>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddTipoServicoEdit" runat="server" DataTextField="descricao"
                                    DataValueField="ident">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddTipoServicoFooter" runat="server" DataTextField="descricao"
                                    DataValueField="ident">
                                </asp:DropDownList>
                                
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="Pai" HeaderText="<%$ Resources:Resource, ServicoPai %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.refServicoPai") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddServicopaiEdit" runat="server" DataTextField="refServico"
                                    DataValueField="idServico">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddServicopai" runat="server" DataTextField="refServico" DataValueField="idServico">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="localDestino" HeaderText="<%$ Resources:Resource, LocalDestino %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.LocalDestino") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddLocalDestinoFooter" runat="server" DataTextField="descricao"
                                    DataValueField="ident">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddLocalDestinoEdit" runat="server" DataTextField="descricao"
                                    DataValueField="ident">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="Observacoes" HeaderText="<%$ Resources:Resource, Observacoes %>">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.Observacoes ")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtObservacoesFooter" runat="Server"></asp:TextBox>
                            </FooterTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtObservacoesEdit" Text='<%# DataBinder.Eval(Container, "DataItem.observacoes") %>'
                                    runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="bVariasGrandezas" HeaderText="MS*">
                            <ItemTemplate>
                               <asp:CheckBox id="cbVariasGrandezas" runat="server" Checked='<%#DataBinder.Eval(Container, "DataItem.bVariasGrandezas")%>' Enabled="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox id="cbVariasGrandezasFooter" runat="server" />
                            </FooterTemplate>
                            <EditItemTemplate>
                            
                                <asp:CheckBox id="cbVariasGrandezasEdit" runat="server" Checked='<%#DataBinder.Eval(Container, "DataItem.bVariasGrandezas")%>' />
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        
                        
                        <asp:TemplateColumn SortExpression="acessorios" HeaderText="Acessorios">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.acessorios ")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtAcessoriosFooter" runat="Server" MaxLength="150"></asp:TextBox>
                            </FooterTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAcessoriosEdit" Text='<%# DataBinder.Eval(Container, "DataItem.acessorios") %>'
                                    runat="server" MaxLength="150" />
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        
                        
                        <asp:TemplateColumn SortExpression="refServicoCertificado" HeaderText="Ref.ESP.">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.refServicoCertificado ")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddServicoCertificadoEdit" runat="server" DataTextField="refServico"
                                    DataValueField="refServico">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddServicoCertificado" runat="server" DataTextField="refServico" DataValueField="refServico">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:ButtonColumn Text="<%$ Resources:Resource, Remover %>" CommandName="Delete">
                        </asp:ButtonColumn>
                        <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="<%$ Resources:Resource, Alterar %>"
                            CancelText="<%$ Resources:Resource, Cancelar %>" EditText="<%$ Resources:Resource, Editar %>"
                            CausesValidation="false">
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:EditCommandColumn>
                        <asp:TemplateColumn>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkButtonAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarLinha %>"
                                    CausesValidation="false" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="refServico" Visible="true" ReadOnly="true"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                    <asp:Label ID="lblMessageServicos" runat="server"></asp:Label>
            </fieldset>
            <table>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                            Text="<%$ Resources:Resource, Gravar %>"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnVerBRE" CssClass="button" runat="server" Width="150"
                            Text="<%$ Resources:Resource, VerBRE %>"></asp:Button>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnVerEtiquetas" CssClass="button" runat="server"
                            Width="150" Text="<%$ Resources:Resource, VerEtiquetas %>"></asp:Button>
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnListaEquipamentos" CssClass="button" runat="server"
                            Width="150" Text="<%$ Resources:Resource, ListaEquipamentos %>"></asp:Button>
                    </td>
                </tr>
            </table>
        </fieldset>
    </fieldset>
</asp:Content>

