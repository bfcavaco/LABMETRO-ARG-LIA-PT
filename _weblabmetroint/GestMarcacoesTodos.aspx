<%@ Page Language="c#" CodeBehind="GestMarcacoesTodos.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestMarcacoesTodos" MasterPageFile="~/mp.Master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisa").focus();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend><%=Resources.Resource.GerirMarcacoesGerais %></legend>
        <fieldset>
            <legend><%=Resources.Resource.Empresas %></legend>
            <table>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMessage" ForeColor="#ff0033" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%=Resources.Resource.Empresa %>:&nbsp;<asp:TextBox ID="txtNomeEmpresa" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <%=Resources.Resource.NIF %>:&nbsp;<asp:TextBox ID="txtNIF" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="4">
                        <%=Resources.Resource.NumCliente %>:&nbsp;<asp:TextBox ID="txtNumClienteSAP" runat="server"></asp:TextBox>
                        &nbsp;<asp:Button class="button" ID="btnPesquisa" runat="Server" CssClass="button"
                            Text="<%$ Resources:Resource, ListarEmpresas %>" CausesValidation="False"></asp:Button>
                    </td>
                </tr>
            </table>
            <br />
            <!-- datagrid empresas -->
            <asp:DataGrid 
            ID="DGEmpresas" 
            runat="server" 
            OnItemDataBound="DGEmpresas_DataBound"
            OnEditCommand="editEmpresa" 
            OnCancelCommand="cancelEmpresa" 
            OnUpdateCommand="updateEmpresa"
            AutoGenerateColumns="false"
            AllowPaging="true" 
            PageSize="5" 
            AllowSorting="True" 
            OnPageIndexChanged="DoPagingEmpresas"
            OnSortCommand="SortGridEmpresas" 
            DataKeyField="idEmpresa">
                <Columns>
                    <asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:TextBox ID="txtItem" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.nivelBloqueioLabmetro")),Convert.ToString(DataBinder.Eval (Container, "DataItem.codigoBloqueioSAP")))%>'
                                Width="20" Height="20" runat="server">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="<%$ Resources:Resource, Nome %>" ReadOnly="True"
                        ItemStyle-Width="300"></asp:BoundColumn>
                    <asp:BoundColumn DataField="nif" SortExpression="nif" HeaderText="<%$ Resources:Resource, NIF %>" ReadOnly="True">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="localidadeEmpresa" SortExpression="localidadeEmpresa"
                        HeaderText="<%$Resources:Resource, Localidade %>" ReadOnly="True"></asp:BoundColumn>
                    <asp:BoundColumn DataField="morada" SortExpression="morada" HeaderText="<%$ Resources:Resource, Morada %>" ReadOnly="True">
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Telefone %>" SortExpression="telefone" ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.telefone") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTelefone" runat="server" Width="75" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.telefone") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="condicoesPagamento" SortExpression="condicoesPagamento"
                        HeaderText="<%$Resources:Resource, CondicoesPagamento %>" ReadOnly="True"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnContactos" runat="server" CausesValidation="False"><%=Resources.Resource.Contactos %></asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnEquipamentos" runat="server" CausesValidation="False"><%=Resources.Resource.Equipamentos %></asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnMarcacoes" runat="server" CausesValidation="False"><%=Resources.Resource.Marcacoes %></asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnRequisicao" runat="server" CausesValidation="False"><%=Resources.Resource.Requisicoes %></asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="btnOrcamentos" runat="server" CausesValidation="False"><%=Resources.Resource.Orcamentos %></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:DataGrid 
        ID="dgContactos" 
        runat="server" 
        OnEditCommand="editContacto"
        OnCancelCommand="cancelContacto" 
        OnUpdateCommand="updateContacto"
        AutoGenerateColumns="false" 
        AllowPaging="true" 
        PageSize="25"
        AllowSorting="True" 
        OnPageIndexChanged="DoPagingContactos" 
        OnSortCommand="SortGridContactos"
        DataKeyField="idContacto">
            <Columns>
                <asp:ButtonColumn CommandName="Select" Text=">>>" ItemStyle-Width="30"></asp:ButtonColumn>
                <asp:BoundColumn DataField="nome" SortExpression="nome" HeaderText="<%$ Resources:Resource, Nome %>" ReadOnly="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="cargo" SortExpression="cargo" HeaderText="<%$ Resources:Resource, Cargo %>" ReadOnly="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="departamento" SortExpression="departamento" HeaderText="<%$ Resources:Resource, Departamento %>"
                    ReadOnly="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="telefoneEmpresa" SortExpression="telefoneEmpresa" HeaderText="<%$ Resources:Resource, Telefone %>"
                    ReadOnly="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="faxEmpresa" SortExpression="faxEmpresa" HeaderText="<%$ Resources:Resource, Fax %>"
                    ReadOnly="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="emailEmpresa" SortExpression="emailEmpresa" HeaderText="<%$ Resources:Resource, Email %>"
                    ReadOnly="False"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        </fieldset>
        <fieldset>
            <legend><%=Resources.Resource.FazerMarcacao %>:</legend>
            <table>
                <tr>
                    <td>
                        <%=Resources.Resource.Tecnico %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddTecnicoExterior" runat="server" DataValueField="idFuncionario"
                            DataTextField="nomeAbreviado">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%=Resources.Resource.Tecnico %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddTecnicoExterior2" runat="server" DataValueField="idFuncionario"
                            DataTextField="nomeAbreviado">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.DataVisita %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataProximaVisita" runat="server" MaxLength="10"></asp:TextBox><asp:CompareValidator
                            ID="Comparevalidator7" runat="server" Operator="DataTypeCheck" Type="Date" ControlToValidate="txtDataProximaVisita">dd-mm-aaaa</asp:CompareValidator><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDataProximaVisita"> ! </asp:RequiredFieldValidator>&nbsp;
                    </td>
                    <td>
                        <%=Resources.Resource.DtUltDia %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataUltDiaMarcacao" runat="server" MaxLength="10"></asp:TextBox><asp:CompareValidator
                            ID="Comparevalidator4" runat="server" Operator="DataTypeCheck" Type="Date" ControlToValidate="txtDataUltDiaMarcacao">dd-mm-aaaa</asp:CompareValidator>
                    </td>
                </tr>
                  <tr>
                  <td>
                      Morada:
                    </td>
                    <td>
                   
                       <asp:TextBox  TextMode="MultiLine" ID="txtMorada" runat="server" ></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMorada"> ! </asp:RequiredFieldValidator>&nbsp;
                    </td>
                    <td>
                        Periodo:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPeriodoMarcacao" runat="server" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPeriodoMarcacao"> ! </asp:RequiredFieldValidator>&nbsp;
                    </td>
                    
                </tr>
                 <tr>
                    <td>
                       Obs.Cliente:
                    </td>
                    <td colspan="3">
                       <asp:TextBox ID="txtObsCliente" runat="server" Width="300" MaxLength="350"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Requisicao %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddRequisicao" runat="server" DataValueField="idRequisicao"
                            DataTextField="referenciaCliente">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Obs.Internas:
                    </td>
                    <td>
                        <asp:TextBox ID="txtObservacoes" runat="server" Width="200" MaxLength="245"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Orcamento %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOrcamento" runat="server" DataValueField="idOrcamento" DataTextField="refOrcamento">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%=Resources.Resource.LocalCalibracao %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddLocalCalibracao" runat="server" DataValueField="ident" DataTextField="descricao">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" id="Tr1">
                    <td>
                        <%=Resources.Resource.EmpresaContratante %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmpresaContratante" runat="server" AutoPostBack="true" OnTextChanged="txtEmpresaContratante_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.NIF %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNifEmpresaContratante" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;
                        &nbsp;<asp:Button class="button" ID="pesquisaEmpresaContratante" CssClass="button"
                            runat="server" Width="80" CausesValidation="false" Text="<%$ Resources:Resource, VerEmpresas %>" OnClick="pesquisaEmpresaContratante_Click">
                        </asp:Button>
                    </td>
                </tr>
               <tr id="trEmpresaContratante" runat="server">
                            <td  height="20">
                                <%=Resources.Resource.Empresa%>:

                            </td>
                           <td>                                <asp:DropDownList ID="ddEmpresaContratante" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                                    DataTextField="nome">
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
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button class="button_confirm" ID="btnFazerMarcacao" runat="server" Text="<%$ Resources:Resource, Marcar %>">
                        </asp:Button>
                    </td>
                    <td>
                        <p>
                            <asp:CheckBox ID="cbEnviarFax" runat="server" AutoPostBack="True" Enabled="false"></asp:CheckBox>&nbsp;<%=Resources.Resource.EnviarFax %></p>
                        <p>
                            <asp:CheckBox ID="cbEnviarMail" runat="server" AutoPostBack="True"></asp:CheckBox>&nbsp;<%=Resources.Resource.EnviarMail %></p>
                        <p>
                           

                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend><%=Resources.Resource.EquipamentosAIncluir %></legend>
            <p>
                <%=Resources.Resource.FiltrarGrandeza %>:
                <asp:DropDownList ID="ddGrandeza" runat="server" Width="300px" DataValueField="ident"
                    DataTextField="descricao">
                </asp:DropDownList>
                <asp:Button class="button" ID="btnFiltrarEquips" runat="server" Text="<%$ Resources:Resource, Filtrar %>" CausesValidation="False">
                </asp:Button></p>
            <asp:Button class="button" ID="bntC" runat="server" Text="<%$ Resources:Resource, TodosC %>" CausesValidation="False">
            </asp:Button><asp:Button class="button" ID="bntE" runat="server" Text="<%$ Resources:Resource, TodosE %>" CausesValidation="False">
            </asp:Button><asp:Button class="button" ID="btnV" runat="server" Text="<%$ Resources:Resource, TodosV %>" CausesValidation="False">
            </asp:Button>
            <asp:DataGrid 
            ID="DGEquipamentos" 
            runat="server" 
            OnItemDataBound="DGEquipamentos_DataBound"
            OnEditCommand="editEquipamento" 
            OnCancelCommand="cancelGridEquipamento" 
            OnUpdateCommand="updateEquipamento"
            AutoGenerateColumns="false"
            AllowPaging="false" 
            PageSize="15" 
            AllowSorting="True" 
            OnSortCommand="SortGridEquipamentos"
            DataKeyField="idEquipamento">
                <Columns>
                    <asp:TemplateColumn HeaderText="C" ItemStyle-BackColor="#99cc00">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="cbCalibracao" AutoPostBack="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="E" ItemStyle-BackColor="#ffcc00">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="cbEnsaio" AutoPostBack="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="V" ItemStyle-BackColor="#cccc99">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="cbVerificacao" AutoPostBack="false"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="idGrandeza" SortExpression="idGrandeza" HeaderText="GR."
                        ReadOnly="true"></asp:BoundColumn>
                    <asp:BoundColumn DataField="familia" SortExpression="familia" HeaderText="<%$ Resources:Resource, Familia %>"
                        ReadOnly="true" Visible="False"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Tipo %>" SortExpression="tipoEquipamento">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.tipoEquipamento") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumIdent %>" SortExpression="numIdentificacao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numIdentificacao") %>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, NumSerie %>" SortExpression="numSerie">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.numSerie") %>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Per." SortExpression="periodicidadeCalibracao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.periodicidadeCalibracao") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPC" runat="server" Width="50" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.periodicidadeCalibracao") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, RefUltimaCalibracao %>" SortExpression="refUltimaCalibracao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRUC" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, DataUltimaCalibracao %>" SortExpression="dtUltimaCalibracao">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.dtUltimaCalibracao","{0:dd/MM/yyyy}") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDUC" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.dtUltimaCalibracao","{0:dd/MM/yyyy}") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Obs %>" SortExpression="observacoes">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.observacoes") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtObs" runat="server" Width="80" Font-Size="8" Text='<%# DataBinder.Eval(Container, "DataItem.observacoes") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:EditCommandColumn EditText="Editar" UpdateText="Alterar" CancelText="Cancelar">
                    </asp:EditCommandColumn>
                </Columns>
            </asp:DataGrid>
            <asp:Label ID="lblLegenda" runat="server">
						<%=Resources.Resource.Legenda %>: <br />
						<%=Resources.Resource.FraseLegendaMarcacoes %><br />
							<br />
            </asp:Label></fieldset>
        <asp:DataGrid 
        ID="dgGenerico" 
        runat="server" 
        AutoGenerateColumns="True">
        </asp:DataGrid>
        <asp:DataGrid 
        ID="dgMarcacoes" 
        runat="server" 
        AutoGenerateColumns="False">
            <Columns>
                <asp:BoundColumn DataField="dtMarcacao" SortExpression="dtMarcacao" HeaderText="<%$ Resources:Resource, Data %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="funcionario" SortExpression="funcionario" HeaderText="<%$ Resources:Resource, Tecnico %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="refmarcacao" SortExpression="refmarcacao" HeaderText="<%$ Resources:Resource, RefMarcacao %>"
                    ReadOnly="True"></asp:BoundColumn>
                <asp:BoundColumn DataField="refBRE" SortExpression="refBRE" HeaderText="<%$ Resources:Resource, NumBRE %>"
                    ReadOnly="True"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid 
        ID="dgRequisicoes" 
        runat="server" 
        AutoGenerateColumns="false" 
        AllowPaging="true" 
        PageSize="10" 
        AllowSorting="True"
        OnPageIndexChanged="DoPaging" 
        OnSortCommand="SortGrid" 
        DataKeyField="idRequisicao">
            <Columns>
                <asp:BoundColumn DataField="referenciaCliente" SortExpression="referenciaCliente"
                    HeaderText="Ref.Req. Cliente"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Empresa<br />tem<br />contrato?" SortExpression="eContrato">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.eContrato"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="dtRequisicao" SortExpression="dtRequisicao" HeaderText="<%$ Resources:Resource, DtReq %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtValidade" SortExpression="dtValidade" HeaderText="<%$ Resources:Resource, DtVal %>"
                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Completa %>" SortExpression="completa">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.completa"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, Renovavel %>" SortExpression="bRenovavel">
                    <ItemTemplate>
                        <%# ConverteEstado(Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.bRenovavel"))) %>
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
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, EditarRequisicao %>" DataNavigateUrlFormatString="FormRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" DataTextField="refRequisicao"
                    SortExpression="refRequisicao">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:HyperLinkColumn HeaderText="<%$ Resources:Resource, VerServicos %>" DataNavigateUrlFormatString="ListaServicosRequisicao.aspx?btn=Doc&id={0}"
                    DataNavigateUrlField="idRequisicao" Target="_new" Text="<%$ Resources:Resource, Ver %>">
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:TemplateColumn HeaderText="<%$ Resources:Resource, MarcarCompleta %>">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chbCompleta" AutoPostBack="True" Checked='<%#DataBinder.Eval(Container.DataItem, "completa") %>'
                            OnCheckedChanged="cb_SetComplete"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid 
        ID="dgOrcamentos" 
        runat="server" 
        AutoGenerateColumns="false"
        AllowPaging="true" 
        PageSize="25" 
        AllowSorting="True" 
        OnPageIndexChanged="DoPagingOrc"
        OnSortCommand="SortGridOrc" 
        DataKeyField="idOrcamento">
            <Columns>
                <asp:BoundColumn DataField="empresa" SortExpression="empresa" HeaderText="<%$ Resources:Resource, Empresa %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="refOrcamento" SortExpression="refOrcamento" HeaderText="<%$ Resources:Resource, RefOrc %>">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="versao" SortExpression="versao" HeaderText="<%$ Resources:Resource, Versao %>"></asp:BoundColumn>
                <asp:BoundColumn DataField="dtOrcamento" SortExpression="dtOrcamento" HeaderText="<%$ Resources:Resource, DataOrcamento %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                <asp:BoundColumn DataField="estado" SortExpression="estado" HeaderText="<%$ Resources:Resource, Estado %>" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                <asp:ButtonColumn CommandName="verOrcamento" HeaderText="<%$ Resources:Resource, Ver %>" Text="<%$ Resources:Resource, Ver %>" ItemStyle-Font-Size="8">
                </asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
    </fieldset>
</asp:Content>
