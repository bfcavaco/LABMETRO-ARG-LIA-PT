<%@ Page Language="c#" CodeBehind="FormPastaEnsaio.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormPastaEnsaio" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>
            <asp:Label ID="lblRefServico" runat="server"></asp:Label></legend>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.BRE%>:
                    <asp:Label ID="lblRefBRE" runat="server"></asp:Label>
                </td>
                <td>
                    <%=Resources.Resource.DataBRE%>:
                    <asp:Label ID="lblDtBRE" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%=Resources.Resource.Observacoes%>:<asp:Label ID="lblObservacoes" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td id="tdReq" runat="server"></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">Acessórios:<asp:Label ID="lblAcessorios" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Múltiplos Serviços:<asp:Label ID="lblMultiplosServicos" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
    </fieldset>
    <!-- body da pagina fica num td, o primeiro é rowspan 2 e contem o menu-->
    <br />
    <fieldset>
        <legend>
            <%=Resources.Resource.DadosServico %></legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblRefServico2" runat="server"></asp:Label>&nbsp;
                    <asp:Label ID="lblGAS" runat="server" CssClass=""></asp:Label>
                    <asp:DropDownList ID="ddlSubtipoServico" runat="server" Font-Size="10px" Font-Name="Verdana"
                        Font-Bold="true" ForeColor="#666666" BackColor="gainsboro" Visible="False">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSubtipoServico" Enabled="false" CssClass="lblMessage">Indicar Subtipo!</asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="lblFacturado" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.LocalActual %>
                </td>
                <td>
                    <asp:DropDownList ID="ddLocalActual" runat="server" DataTextField="descricao"
                        DataValueField="ident">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.CalibExterna %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbCalExterna" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Quantidade %>:
                </td>
                <td>
                    <asp:TextBox ID="txtQuantidade" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator8" runat="server" Operator="DataTypeCheck" Type="Integer"
                        ControlToValidate="txtQuantidade">*</asp:CompareValidator>Unidade:
                    <asp:TextBox ID="txtUnidadeQuantidade" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.FuncionarioTreino %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddFuncTreino" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ObservacoesInternas %>:
                </td>
                <td>
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="80%" MaxLength="150"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.ObservacoesCliente %>:
                </td>
                <td>
                    <asp:TextBox ID="txtObsCliente" runat="server" Width="100%" MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Estado %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server" DataTextField="descricao" DataValueField="idEstadoServico"
                        AutoPostBack="True" CssClass="combo">
                    </asp:DropDownList>
                    &nbsp;<%=Resources.Resource.Razao %>:<asp:DropDownList ID="ddComentarioEstado" runat="server"
                        DataTextField="comentario" DataValueField="idComentarioEstado">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Conformidade %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddConforme" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, NaoConforme %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Conforme %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.ComDeslocacao %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbDeslocacao" runat="server"></asp:CheckBox>

                </td>
            </tr>
            <tr>
                <td>Lingua do Certificado
                </td>
                <td>
                    <asp:DropDownList ID="ddLinguaCertificado" runat="server">

                        <asp:ListItem Value="PT" Text="PT"></asp:ListItem>
                        <asp:ListItem Value="EN" Text="EN"></asp:ListItem>
                        <asp:ListItem Value="FR" Text="FR"></asp:ListItem>
                        <asp:ListItem Value="ES" Text="ES"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoDeslocacao" runat="server">
                        <asp:ListItem Value="0" Text="Seleccione tipo de Deslocação"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Desloc. 1º Equip."></asp:ListItem>
                        <asp:ListItem Value="2" Text="Desloc. + 2º Equip "></asp:ListItem>
                        <asp:ListItem Value="3" Text="Outra"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Núm. Etiqueta IPQ
                </td>
                <td>
                    <asp:TextBox ID="txtNumEtiquetaIPQ" runat="server" Width="80%" MaxLength="150"></asp:TextBox>

                </td>
                <td>Rejeitado:
                </td>
                <td>
                    <asp:RadioButtonList ID="rbRejeitado" runat="server" RepeatDirection="Horizontal" Enabled="true">


                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>

                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <br />
    <fieldset>
        <legend>
            <%=Resources.Resource.Prioridade %></legend>
        <asp:DropDownList ID="ddNivelPrioridade" runat="server" DataTextField="descricao"
            DataValueField="idNivelPrioridade">
        </asp:DropDownList>
        <br />
        <%=Resources.Resource.DataPrevisao %>
        <asp:TextBox ID="txtDataPrevisao" runat="server"></asp:TextBox>
        <asp:CompareValidator ID="Comparevalidator4" runat="server" ControlToValidate="txtDataPrevisao"
            Type="Date" Operator="DataTypeCheck">dd-mm-aaaa</asp:CompareValidator>
    </fieldset>
    <fieldset>
        <legend>
            <%=Resources.Resource.Cliente %></legend>
        <asp:Label ID="lblEmpresa" Width="100%" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblEmpresaDevedora" Width="100%" runat="server"></asp:Label>
        Criterios de Aceitacao (Empresa):<asp:TextBox ID="txtCriteriosAceitacaoEmpresa" runat="server" Width="100%" Height="150" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
        <table>
            <tr>
                <td id="tdFicheiroCriterios" runat="server"></td>
            </tr>
        </table>


    </fieldset>
    <br />
    <fieldset>
        <legend>
            <%=Resources.Resource.DadosEquipamento %></legend>
        <asp:Label ID="lblIdEquipanmento" runat="server"></asp:Label>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoEquipamento" runat="server" DataTextField="descricao"
                        DataValueField="ident" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:Label ID="lblTipoEquipamento" runat="server"></asp:Label>
                      <asp:RequiredFieldValidator
                        ID="RequiredFieldValidatortipo" runat="server" ControlToValidate="ddTipoEquipamento" Enabled="true" CssClass="lblMessage">Indicar Tipo Actual!</asp:RequiredFieldValidator>
                    <br />

                </td>
                <td>
                    <%=Resources.Resource.codigoIPQ %>:
                </td>
                <td>
                    <asp:TextBox ID="txtCodigoIPQ" runat="server" Width="175px" MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumIdentificacao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdentificacao" runat="server" Width="175px" MaxLength="150"></asp:TextBox><asp:CustomValidator
                        ID="cv1" runat="server" ErrorMessage="Tem de preencher o número de série ou o número de identificação."
                        OnServerValidate="validaEquipamento"></asp:CustomValidator>
                </td>
                <td>
                    <%=Resources.Resource.NumSerie %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumSerie" runat="server" Width="175px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Marca %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddMarca" runat="server" DataTextField="descricao" DataValueField="idMarca"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.Modelo %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddModelo" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Indicacao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtForma" runat="server" Width="175px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Classe %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddClassse" Width="120" runat="server" DataTextField="descricao"
                        DataValueField="idClasse">
                    </asp:DropDownList>
                    &nbsp;<asp:TextBox ID="txtClasse" Width="60" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.GamaEntre %>:
                </td>
                <td>
                    <asp:TextBox ID="txtAlcanceInf" Width="50" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator6" runat="server" CssClass="lblMessage" ErrorMessage="!"
                        Operator="DataTypeCheck" Type="Double" ControlToValidate="txtAlcanceInf" Display="static"></asp:CompareValidator>&nbsp;-&nbsp;
                    <asp:TextBox ID="txtAlcanceSup" Width="50" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator7" runat="server" CssClass="lblMessage" ErrorMessage="!numérico"
                        Operator="DataTypeCheck" Type="Double" ControlToValidate="txtAlcanceSup" Display="static"></asp:CompareValidator>
                </td>
                <td>
                    <%=Resources.Resource.UnidadeGama %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddUnidadeAlcance" runat="server" DataTextField="descricao"
                        DataValueField="ident">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ObsGama %>:
                </td>
                <td>
                    <asp:TextBox ID="txtAlcance" runat="server" Width="175px"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Divisao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtResolucao" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CampExtra1 %>:
                </td>
                <td>
                    <asp:TextBox ID="txtCampo1" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.CampExtra2 %>:
                </td>
                <td>
                    <asp:TextBox ID="txtCampo2" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                conf
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CertificadoConclusivo %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbCertConclusivo" runat="server" Enabled="False"></asp:CheckBox>
                </td>
                <td>Obs. Equipamento    
                </td>
                <td>
                    <asp:TextBox ID="txtObsEquipamento" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Criterios (Equipamento):
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCriteriosEquipamento" runat="server" Width="100%" Height="150" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td>Campo Etiqueta1
                </td>
                <td>
                    <asp:TextBox ID="txtEtiqueta1" runat="server" MaxLength="30" Width="100"></asp:TextBox>
                </td>
                <td>Campo Etiqueta2
                </td>
                <td>
                    <asp:TextBox ID="txtEtiqueta2" runat="server" MaxLength="30" Width="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Campo Etiqueta3
                </td>
                <td>
                    <asp:TextBox ID="txtEtiqueta3" runat="server" MaxLength="30" Width="100"></asp:TextBox>
                </td>
                <td>ESPANA:<%=Resources.Resource.EstadoRelacaoCalibracao %>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddEstadoRelacaoCalibracao" DataTextField="descricao"
                        DataValueField="idEstadoRelacaoCalibracao" AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="---"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>
            <%=Resources.Resource.PontosCalibracao %></legend>
        <table>
            <tr>
                <td colspan="4">
                    <asp:DataGrid ID="DGLinhasServico" runat="server" ShowFooter="true" AllowSorting="False"
                        AutoGenerateColumns="false" DataKeyField="idServicoLinha">
                        <Columns>
                            <asp:TemplateColumn HeaderText="<%$Resources:Resource, NumPecas %>" ItemStyle-Width="80">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNumPecas" runat="Server" Width="80" />
                                    <asp:CompareValidator ID="Comparevalidator2" runat="server" CssClass="lblMessage"
                                        Display="static" ErrorMessage="!Formato" ControlToValidate="txtNumPecas" Type="Integer"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.numPecas") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNumPecasEdit" Text='<%# DataBinder.Eval(Container, "DataItem.numPecas") %>'
                                        runat="server" Width="80" />
                                    <asp:CompareValidator ID="Comparevalidator3" runat="server" CssClass="lblMessage"
                                        Display="static" ErrorMessage="!Formato" ControlToValidate="txtNumPecasEdit"
                                        Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumPontos %>" ItemStyle-Width="80">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNumPontos" runat="Server" Width="80" />
                                    <asp:CompareValidator ID="Comparevalidator4" runat="server" CssClass="lblMessage"
                                        Display="static" ErrorMessage="!Formato" ControlToValidate="txtNumPontos" Type="Integer"
                                        Operator="DataTypeCheck"></asp:CompareValidator>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.numPontos") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNumPontosEdit" Text='<%# DataBinder.Eval(Container, "DataItem.numPontos") %>'
                                        runat="server" Width="80" />
                                    <asp:CompareValidator ID="Comparevalidator5" runat="server" CssClass="lblMessage"
                                        Display="static" ErrorMessage="!Formato" ControlToValidate="txtNumPontosEdit"
                                        Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, PontosCalibracao %>">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPontosCalib" runat="Server" Width="300" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.pontosCalib") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPontosCalibEdit" Text='<%# DataBinder.Eval(Container, "DataItem.pontosCalib") %>'
                                        runat="server" Width="300" />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                                ItemStyle-Width="100"></asp:EditCommandColumn>
                            <asp:ButtonColumn CommandName="Delete" Text="Remover"></asp:ButtonColumn>
                            <asp:TemplateColumn HeaderText="">
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkLinhaServicoAdd" CommandName="Insert" runat="server" Text="<%$ Resources:Resource, AdicionarLinha %>" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                  
                </td>
            </tr>
        </table>
    </fieldset>
      <fieldset>
        <legend>
           "SUBTIPO"</legend>
        <table>
            <tr>
                <td colspan="4">
                  Seleccionar: <asp:DropDownList ID="ddSubtipoEquipamento" DataValueField="idSubTipoEquipamento" DataTextField="descricao" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center">  <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Gravar %>"
                        CausesValidation="true"></asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>
            <%= Resources.Resource.HistoricoEstados %></legend>
        <table width="100%">
            <tr>
                <td colspan="4">
                    <asp:DataGrid ID="dgHistorico" runat="server" ShowFooter="false" AllowSorting="false"
                        AutoGenerateColumns="false" DataKeyField="idServico">
                        <Columns>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Estado %>" DataField="estado"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, DtEstado %>" DataField="dataEstado"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Funcionario %>" DataField="userEstado"></asp:BoundColumn>

                            <asp:BoundColumn HeaderText="<%$ Resources:Resource, Comentario %>" DataField="comentario"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>
            <%=Resources.Resource.Certificados %></legend>
        <asp:DataGrid ID="dgCertificados" runat="server" AutoGenerateColumns="False" DataKeyField="nomeDocumento"
            OnItemDataBound="dgCertificados_ItemDataBound" OnItemCommand="visualisarDocumento">
            <Columns>
                <asp:ButtonColumn Text="visualisarDocumento" ButtonType="LinkButton" DataTextField="nomeDocumento"
                    Visible="False" SortExpression="nomeDocumento" HeaderText="Visualisar Documento"
                    CommandName="Select">
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonColumn>
                <asp:BoundColumn DataField="refServico" SortExpression="refServico" HeaderText="<%$ Resources:Resource, RefCalib %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtCertificado" SortExpression="dtCertificado" HeaderText="<%$ Resources:Resource, DataEmissao %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="responsavelLaboratorio" SortExpression="responsavelLaboratorio"
                    HeaderText="<%$ Resources:Resource, AprovadoPor %>" ItemStyle-Width="20%"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpathCert(DataBinder.Eval(Container,"DataItem.nomeDocumento"))%>'
                            ID="Hyperlink2" Target="new">
														<%# DataBinder.Eval(Container.DataItem, "nomeDocumento")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </fieldset>
    <fieldset>
        <legend>
            <%=Resources.Resource.OutrosServicosEquipamento %></legend>
        <asp:DataGrid ID="dgServicosAnteriores" runat="server" AutoGenerateColumns="false"
            DataKeyField="nomeDocumento" OnItemCommand="visualisarDocumento">
            <Columns>
                <asp:BoundColumn DataField="refServico" HeaderText="<%$ Resources:Resource, RefCalib %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="estadoServico" HeaderText="<%$ Resources:Resource, Estado %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtEntrada" HeaderText="<%$ Resources:Resource, DataEntrada %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeDocumento">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpathCert(DataBinder.Eval(Container,"DataItem.nomeDocumento"))%>'
                            ID="Hyperlink3" Target="new">
														<%# DataBinder.Eval(Container.DataItem, "nomeDocumento")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="responsavelLaboratorio" HeaderText="<%$ Resources:Resource, AprovadoPor %>"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
    <!-- FIM body -->
    <br />
    <br />
    <asp:Button class="button" ID="btnRequisicoes" runat="server" CssClass="button" Text="<%$ Resources:Resource, VerOcultarRequisicoes %>"></asp:Button>&nbsp;&nbsp;<asp:Button class="button" ID="btnOrcamentos" runat="server"
        CssClass="button" Text="<%$ Resources:Resource, VerOcultarOrcamentos %>"></asp:Button>&nbsp;&nbsp;
    <asp:Button class="button" ID="btnEtiqueta" Width="200" runat="server" CssClass="button"
        Text="<%$ Resources:Resource, EtiquetaRecepcao %>"></asp:Button>&nbsp;&nbsp;
    <asp:Button class="button" ID="btnEtiquetaCal" Width="200" runat="server" CssClass="button"
        Text="<%$ Resources:Resource, EtiquetaCalibracao %>"></asp:Button>
    <br />
    <br />
    <div style="float: right; width: 50%">
        *
        <%=Resources.Resource.AvisoDataEtiquetaCalibracao %>.
    </div>
    <!--*********DATAGRID REQUISICOES***********************************************-->
    <asp:DataGrid ID="DGRequisicoes" runat="server" ShowFooter="false" AllowSorting="false"
        GridLines="both" AutoGenerateColumns="False" DataKeyField="idRequisicao" Visible="False"
        ShowHeader="true" PageSize="10" AllowPaging="True" OnPageIndexChanged="DoPagingRequisicoes">
        <Columns>
            <asp:BoundColumn DataField="refRequisicao" HeaderText="<%$ Resources:Resource, RefReq %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="referenciaCliente" HeaderText="<%$ Resources:resource, RefCliente %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="dtRequisicao" HeaderText="<%$ Resources:Resource, DtReq %>"
                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="dtValidade" HeaderText="<%$ Resources:Resource, DtVal %>"
                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Completo %>" SortExpression="completa">
                <ItemTemplate>
                    <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.completa"))) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
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
    <!--*********DATAGRID ORÇAMENTOS***********************************************-->
    <asp:DataGrid ID="DGOrcamentos" runat="server" AllowSorting="True" AutoGenerateColumns="false"
        DataKeyField="idOrcamento" Visible="False" PageSize="5" AllowPaging="true" OnPageIndexChanged="DoPaging"
        OnSortCommand="SortGrid">
        <Columns>
            <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="refOrcamento" SortExpression="refOrcamento" HeaderText="<%$ Resources:Resource, RefOrc %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="versao" SortExpression="versao" HeaderText="<%$ Resources:Resource, versao %>"></asp:BoundColumn>
            <asp:BoundColumn DataField="dtOrcamento" SortExpression="dtOrcamento" HeaderText="<%$ Resources:Resource, DataOrc %>"
                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="<%$ Resources:Resource, Estado %>"
                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
            <asp:ButtonColumn CommandName="select" HeaderText="Ver<br />Linhas" Text="<%$ Resources:Resource, VerLinhas %>"
                ItemStyle-Font-Size="8"></asp:ButtonColumn>

            <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%#downloadpathOrcamento(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                        ID="Hyperlink1" Target="new">
														<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <!--********-->
    <div style="float: left; width: 50%">
        <asp:DataGrid ID="dgLinhasOrcamento" runat="server" ShowFooter="true" AllowSorting="False"
            AutoGenerateColumns="false" DataKeyField="idOrcamentoLinha">
            <Columns>
                <asp:TemplateColumn HeaderText="Qtd">
                    <ItemStyle Width="10"></ItemStyle>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.quantidade")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Descricao %>">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.descricaoEquipamento")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Notas %>">
                    <ItemStyle Width="30"></ItemStyle>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.asterisco")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <div style="float: right; width: 50%">
        <asp:DataGrid ID="dgComentariosOrcamento" runat="server" ShowFooter="true" AllowSorting="False"
            AutoGenerateColumns="false">
            <Columns>
                <asp:BoundColumn DataField="asterisco" HeaderText="<%$ Resources:Resource, Asterisco %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="descricao" HeaderText="<%$ Resources:Resource, Descricao %>"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </div>
</asp:Content>
