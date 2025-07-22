<%@ Page Language="c#" CodeBehind="FormFacturaSAP.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormFacturaSAP" MasterPageFile="~/mp.Master" %>

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
        <legend>Factura</legend>
        <fieldset>
            <legend>Empresa</legend>
        <asp:Label ID="lblMessage" runat="server"></asp:Label><asp:ValidationSummary ID="valSummary"
            runat="server" HeaderText="Preencha por favor os seguintes campos:" ShowSummary="True"
            CssClass="lblMessage" DisplayMode="List" Font-Bold="True"></asp:ValidationSummary>
        <table>
            <tr>
                <td>
                   <label>Empresa:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td>
                   <label> Nif:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label> Código Cliente SAP:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNumClienteSAP" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:Button class="button" ID="btnEmpresas" CssClass="button" runat="server" Text="Pesquisar Empresas"
                        CausesValidation="false"></asp:Button>
                </td>
            </tr>
            <tr id="trEmpresa" runat="server">
                <td>
                   <label> Empresa BRE:</label> 
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataTextField="nome"
                        DataValueField="idEmpresa">
                    </asp:DropDownList>
                    <asp:Label ID="lblEmpresa" runat="server"></asp:Label><asp:RequiredFieldValidator
                        ID="Requiredfieldvalidator1" runat="server" ControlToValidate="ddEmpresa" ErrorMessage="Empresa">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                     <label>Localidade da Empresa BRE:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtLocalidade" runat="server" AutoPostBack="true" Width="224px"></asp:TextBox>
                </td>
                <td>
                   <label> Regiao Vendas Empresa BRE:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddRegiaoVendasEmpresa" runat="server">
                    </asp:DropDownList>
                    <br />
                    <asp:Button class="button" ID="btnUpdateRegiaoVendasEmpresa" CssClass="button" runat="server"
                        Text="Actualizar Região" CausesValidation="false"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                   <label> BRE:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddBRE" runat="server" AutoPostBack="true" DataTextField="refBRE"
                        DataValueField="idBRE">
                    </asp:DropDownList>
                    <asp:Label ID="lblBRE" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="cbBRE" runat="server" AutoPostBack="True" Text="&nbsp;&nbsp;Só BRE's completos!"
                        Checked="True"></asp:CheckBox>
                </td>
            </tr>
            <tr id="trEmpresaContratante" runat="server">
                <td>
                    <label> Facturar a:</label>
                </td>
                <td >
                    <asp:DropDownList ID="ddEmpresaContratante" runat="server" DataTextField="nomeEmpresaContratante"
                        DataValueField="idEmpresaContratante">
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                </td>
            </tr>
        </table>
    
    
        <table>
            <tr>
                <td>
                    <label> Bloqueio SAP:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddCodigoBloqueioSAP" runat="server" Enabled="False">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:Label ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage"></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <label> Núm. Cliente SAP:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtNumClienteSAP" runat="server" AutoPostBack="true" Width="224px"
                        Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <label> Condiçoes de Pagamento da Empresa:</label> 
                </td>
                <td <%if(ddCondPagamentoEmpresa.SelectedValue=="1") Response.Write("bgcolor = red");%>>
                    <asp:DropDownList ID="ddCondPagamentoEmpresa" runat="server" Enabled="False">
                    </asp:DropDownList>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <label> Não precisa de requisição para facturar:</label> 
                    <asp:CheckBox ID="cbNPrecisaReqPFacturar" runat="server" Enabled="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td><label> 
                    Obs. Empresa:</label> 
                </td>
                <td colspan="3">
                    <asp:Label ID="lblObsEmpresa" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Requisições</legend>
        <asp:Button class="button" ID="btnRequisicoes" runat="server" Text="Ver Requisições"
            CausesValidation="true"></asp:Button>
        <asp:DataGrid 
        ID="dgRequisicoes" 
        runat="server"
        DataKeyField="idRequisicao"
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid" 
        AllowSorting="True" 
        PageSize="10"
        AllowPaging="true" 
        AutoGenerateColumns="false">    
            <Columns>
                <asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente"
                    HeaderText="Ref.Req. Cliente"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Empresa<br />tem<br />contrato?" SortExpression="eContrato">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.eContrato"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="dtRequisicao" SortExpression="dtRequisicao" HeaderText="Data Req."
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtValidade" SortExpression="dtValidade" HeaderText="Data Val."
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="compl." SortExpression="completa">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.completa"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Renovável" SortExpression="bRenovavel">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRenovavel"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ficheiro" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeFicheiro"))%>'
                            ID="Hyperlink1" Target="new">
														<%# DataBinder.Eval(Container.DataItem, "nomeFicheiro")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="(Editar)<br />Requisição" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" DataTextField="refRequisicao"
                    SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:HyperLinkColumn HeaderText="Ver<br />Serviços" DataNavigateUrlFormatString="ListaServicosRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" Text="ver">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="Marcar<br />completa">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chbCompleta" AutoPostBack="True" Checked='<%#DataBinder.Eval(Container.DataItem, "completa") %>'
                            OnCheckedChanged="cb_SetComplete"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
    <fieldset>
        <legend>Serviços a facturar</legend>
        <asp:Label ID="lblEquipamentos" runat="server"></asp:Label>
        <asp:DataGrid 
        ID="dgOrigem"
        runat="server" 
        DataKeyField="idServico"
        OnSortCommand="SortGridOrigem"
        AllowSorting="true"
        AutoGenerateColumns="false" 
        ShowFooter="false" 
        OnEditCommand="editGridOrigem"
        OnCancelCommand="cancelGridOrigem" 
        OnUpdateCommand="updateGridOrigem" 
        OnItemDataBound="dgOrigem_ItemDataBound">
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="checkbox"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ref.Calib." SortExpression="refServico">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.refServico") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="N/Ref.<br />Req.">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.refRequisicao") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddRequisicaoEdit" runat="server" DataValueField="idRequisicao"
                            DataTextField="referenciaCliente">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ref. Req.<br />Cliente" SortExpression="referenciaCliente">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.referenciaCliente") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Núm.Ident." SortExpression="numIdentificacao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Tipo Equip." SortExpression="tipoEquipamento">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Estado" SortExpression="estadoServico">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.estadoServico") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Dt.Estado" SortExpression="dtEstado">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.dtEstado") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Tipo Serv." SortExpression="tipoServico">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.tipoServico") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Obs." SortExpression="observacoes">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.observacoes") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Calib.<br />Ext." SortExpression="calibracaoExterna">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.calibracaoExterna"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Button class="button" ID="btnSubmitGrid" runat="server" Text="Facturar equipamentos"
            CausesValidation="true"></asp:Button>
        <br />
    </fieldset>
    <fieldset>
        <legend>Dados da Factura</legend>
        <table>
            <tr>
                <td>
                    <label> Nº Factura:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtRefFactura" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    <label> Data Factura:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtDtFactura" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label> Forçar data:</label> 
                </td>
                <td>
                    <asp:CheckBox ID="cbForcarData" runat="server"></asp:CheckBox>
                </td>
                <td>
                    <label> Data a ser assumida:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtDataFacturaForcada" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator6" runat="server" ControlToValidate="txtDataFacturaForcada"
                        ErrorMessage="Data assuimida formato obrigatório 'dd-mm-aaaa'" Operator="DataTypeCheck"
                        Type="Date">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <label> Tipo de Doc:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddTipoDocumento" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                   <label>  Canal de Distrib.:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddCanalDistribuicao" runat="server">
                        <asp:ListItem Value="RE" Selected="True">Retalho</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label> Escrit.Vendas:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddEscritorioVendas" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label> Cond.Pagamento:</label> 
                </td>
                <td>
                    <asp:DropDownList ID="ddCondPagamentoFactura" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                   <label>  Nome Ficheiro:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtFicheiro" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                   <label>  Versão:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtVersao" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                   <label>  Observações:</label> 
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="100%"></asp:TextBox>
                </td>
            </tr>
        </table>
    
        <asp:Label ID="lblErroRequisicoes" CssClass="lblMessage" runat="server"></asp:Label><br />
        <asp:DataGrid 
        ID="dgDestino" 
        runat="server" 
        DataKeyField="idServico"
        OnSortCommand="SortGridDestino" 
        AllowSorting="True" 
        AutoGenerateColumns="false"
        ShowFooter="false" 
        OnEditCommand="editGrid"
        OnCancelCommand="cancelGrid" 
        OnUpdateCommand="updateGrid" 
        OnItemDataBound="dgDestino_ItemDataBound">
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <%# Container.ItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Id Serv.<br />(BD)" SortExpression="idServico">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.idServico") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Equip." SortExpression="codTipoEquipamento">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Num.Ident." SortExpression="numIdentificacao">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn HeaderText="Ref.Cal." DataNavigateUrlFormatString="FormPastaEnsaio.aspx?id={0}"
                    DataNavigateUrlField="idServico" Target="_new" DataTextField="refServico" SortExpression="refServico">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente"
                    HeaderText="Ref. Req.<br />Cliente" ReadOnly="True"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Valor" SortExpression="valor" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valor"))%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtValorEdit" Text='<%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valor")) %>'
                            runat="server" Width="80" />
                        <asp:CompareValidator ID="Comparevalidator2" runat="server" CssClass="errorMsg" Display="static"
                            ErrorMessage="!Formato" ControlToValidate="txtValorEdit" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="%<br /> Desc." SortExpression="percDesconto" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.percDesconto"))%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPercDescontoEdit" Text='<%#LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.percDesconto")) %>'
                            runat="server" Width="80" />
                        <asp:CompareValidator ID="Comparevalidator3" runat="server" CssClass="errorMsg" Display="static"
                            ErrorMessage="!Formato" ControlToValidate="txtPercDescontoEdit" Type="Double"
                            Operator="DataTypeCheck"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Valor Final" SortExpression="valorFinal" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valorFinal"))%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtValorFinalEdit" Text='<%# LabMetro.GERAL.clsGeral.ConvertDBMoneyToString(DataBinder.Eval(Container, "DataItem.valorFinal")) %>'
                            runat="server" ReadOnly="True" Width="80" />
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar"
                    ItemStyle-Width="100"></asp:EditCommandColumn>
                <asp:ButtonColumn CommandName="Delete" Text="Remover"></asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
    <fieldset>
        <legend>Valores da Factura</legend>
        <table>
            <tr>
                <td>
                    <label> Ajudas de Custo/Deslocações:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtValorAjudasCustoDeslocacoes" runat="server" AutoPostBack="true"
                        Width="100px"></asp:TextBox><asp:CompareValidator ID="Comparevalidator1" runat="server"
                            CssClass="lblMessage" ControlToValidate="txtValorAjudasCustoDeslocacoes" Display="static"
                            ErrorMessage="!Formato" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                </td>
                <td>
                    <label> Sub Total:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtValorSubTotal" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                   <label>  Despesas de Envio:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtValorDespesasEnvio" runat="server" AutoPostBack="true" Width="100px"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator4" runat="server" CssClass="lblMessage" ControlToValidate="txtValorDespesasEnvio"
                        Display="static" ErrorMessage="!Formato" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                </td>
                <td>
                    <label> Desconto Total:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtDescontoTotal" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td>
                    <label> Total:</label> 
                </td>
                <td>
                    <asp:TextBox ID="txtValorTotalFactura" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="center">
                    <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="Gravar!"
                        CausesValidation="true"></asp:Button>
                </td>
                <td>
                </td>
                <td align="center">
                    <asp:Button class="button" ID="btnFicheiro" CssClass="button" runat="server" Text="Gerar Ordem de Venda"
                        CausesValidation="true" Width="150"></asp:Button>
                    <br />
                    <asp:Button class="button" ID="btnVD" CssClass="button" runat="server" Text="Gerar Venda à Dinheiro"
                        CausesValidation="true" Width="150" onclick="btnVD_Click"></asp:Button>
                    <br />
                    
                    <asp:Button class="button" ID="btnVerFactura" CssClass="button" runat="server" Text="Ver Factura"
                        Width="150" />
                    <br />
                    
                    <asp:Button class="button" ID="btnReset" CssClass="button" runat="server" Text="Nova factura"
                        CausesValidation="false" Width="150"></asp:Button>
                    <br />
                    <br />
                  
                </td>
            </tr>
        </table>
    </fieldset>
    </fieldset>
</asp:Content>
