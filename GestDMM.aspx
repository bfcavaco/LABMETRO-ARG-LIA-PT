<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestDMM.aspx.cs" Inherits="LabMetro.GestDMM"
    MasterPageFile="~/mp.Master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .formLabel
        {
            width: 100em;
            text-align: left;
            margin-left: 0.5em;
            font-weight: bold;
        }
        .submit input
        {
            margin-left: 1.5em;
        }
        .fvForm label
        {
            width: 15em;
            float: left;
            text-align: right;
            margin-right: 0.5em;
            display: block;
        }
        .fvForm input, select, textarea
        {
            margin-left: 1.5em;
        }
        .fvForm textarea
        {
            width: 250px;
            height: 150px;
        }
        .fvForm .boxes
        {
            width: 1em;
        }
        br
        {
            clear: left;
        }
    </style>
</asp:Content>
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
    <asp:ObjectDataSource ID="OBJDS_ListaEquipamentosByEmpresa" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetDataByIdEmpresa" TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.DMMTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddEmpresa" Name="idEmpresa" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_DetalheEquipamento" runat="server" InsertMethod="BLLInsertEquipamento"
        SelectMethod="BLLGetEquipamentoById" TypeName="LabMetro.BusinessLogicLayer.EquipamentoBLL"
        UpdateMethod="BLLUpdateEquipamento" OnInserted="OBJDS_Inserted" OnUpdated="OBJDS_Updated">
        <UpdateParameters>
            <asp:ControlParameter Name="idEmpresa" Type="Int32" ControlID="ddEmpresa" PropertyName="SelectedValue" />
            <asp:Parameter Name="idTipoEquipamento" Type="Int32" />
            <asp:Parameter Name="numSerie" Type="String" />
            <asp:Parameter Name="numIdentificacao" Type="String" />
            <asp:Parameter Name="alcanceInf" Type="Double" />
            <asp:Parameter Name="alcanceSup" Type="Double" />
            <asp:Parameter Name="idUnidadeAlcance" Type="Int32" />
            <asp:Parameter Name="alcance" Type="String" />
            <asp:Parameter Name="resolucao" Type="String" />
            <asp:Parameter Name="idClasse" Type="Int32" />
            <asp:Parameter Name="classe" Type="String" />
            <asp:Parameter Name="forma" Type="String" />
            <asp:Parameter Name="fabricante" Type="String" />
            <asp:Parameter Name="refUltimaCalibracao" Type="String" />
            <asp:Parameter Name="dtUltimaCalibracao" Type="DateTime" />
            <asp:Parameter Name="periodicidadeCalibracao" Type="Int16" />
            <asp:Parameter Name="activo" Type="Boolean" />
            <asp:Parameter Name="observacoes" Type="String" />
            <asp:Parameter Name="calibInt" Type="Boolean" />
            <asp:Parameter Name="bCertConclusivo" Type="Boolean" />
            <asp:Parameter Name="campo1" Type="String" />
            <asp:Parameter Name="campo2" Type="String" />
            <asp:Parameter Name="idMarca" Type="Int32" />
            <asp:Parameter Name="idModelo" Type="Int32" />
            <asp:Parameter Name="entidadeUltimaCalibracao" Type="String" />
            <asp:Parameter Name="idEstadoUtilizacao" Type="Int32" />
            <asp:Parameter Name="idEstadoEquipamento" Type="Int32" />
            <asp:Parameter Name="dtEntradaServico" Type="DateTime" />
            <asp:Parameter Name="fornecedor" Type="String" />
            <asp:Parameter Name="responsavelEquipamento" Type="String" />
            <asp:Parameter Name="precoClienteEuros" Type="Decimal" />
            <asp:Parameter Name="custoClienteEuros" Type="Decimal" />
            <asp:Parameter Name="localizacao" Type="String" />
            <asp:Parameter Name="criterios" Type="String" />
            <asp:Parameter Name="pontosCalibracao" Type="String" />
            <asp:Parameter Name="texto" Type="String" />
            <asp:Parameter Name="idEstadoRelacaoCalibracao" Type="Int32" />
            <asp:Parameter Name="idTipoIntervencao" Type="Int32" />
            <asp:Parameter Name="idEquipamento" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="gvEquipamentos" Name="idEquipamento" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:ControlParameter Name="idEmpresa" Type="Int32" ControlID="ddEmpresa" PropertyName="SelectedValue" />
            <asp:Parameter Name="idTipoEquipamento" Type="Int32" />
            <asp:Parameter Name="numSerie" Type="String" />
            <asp:Parameter Name="numIdentificacao" Type="String" />
            <asp:Parameter Name="alcanceInf" Type="Double" />
            <asp:Parameter Name="alcanceSup" Type="Double" />
            <asp:Parameter Name="idUnidadeAlcance" Type="Int32" />
            <asp:Parameter Name="alcance" Type="String" />
            <asp:Parameter Name="resolucao" Type="String" />
            <asp:Parameter Name="idClasse" Type="Int32" />
            <asp:Parameter Name="classe" Type="String" />
            <asp:Parameter Name="forma" Type="String" />
            <asp:Parameter Name="fabricante" Type="String" />
            <asp:Parameter Name="refUltimaCalibracao" Type="String" />
            <asp:Parameter Name="dtUltimaCalibracao" Type="DateTime" />
            <asp:Parameter Name="periodicidadeCalibracao" Type="Int16" />
            <asp:Parameter Name="activo" Type="Boolean" />
            <asp:Parameter Name="observacoes" Type="String" />
            <asp:Parameter Name="calibInt" Type="Boolean" />
            <asp:Parameter Name="bCertConclusivo" Type="Boolean" />
            <asp:Parameter Name="campo1" Type="String" />
            <asp:Parameter Name="campo2" Type="String" />
            <asp:Parameter Name="idMarca" Type="Int32" />
            <asp:Parameter Name="idModelo" Type="Int32" />
            <asp:Parameter Name="entidadeUltimaCalibracao" Type="String" />
            <asp:Parameter Name="idEstadoUtilizacao" Type="Int32" />
            <asp:Parameter Name="idEstadoEquipamento" Type="Int32" />
            <asp:Parameter Name="dtEntradaServico" Type="DateTime" />
            <asp:Parameter Name="fornecedor" Type="String" />
            <asp:Parameter Name="responsavelEquipamento" Type="String" />
            <asp:Parameter Name="precoClienteEuros" Type="Decimal" />
            <asp:Parameter Name="custoClienteEuros" Type="Decimal" />
            <asp:Parameter Name="localizacao" Type="String" />
            <asp:Parameter Name="criterios" Type="String" />
            <asp:Parameter Name="pontosCalibracao" Type="String" />
            <asp:Parameter Name="texto" Type="String" />
            <asp:Parameter Name="idEstadoRelacaoCalibracao" Type="Int32" />
            <asp:Parameter Name="idTipoIntervencao" Type="Int32" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_tipoEquipamentoActivo" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetActivos" TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.TipoEquipamentoDDTableAdapter">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_TipoEquipamentoByGrandeza" runat="server" SelectMethod="GetByIdGrandeza"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.TipoEquipamentoDDTableAdapter"
        OldValuesParameterFormatString="original_{0}" OnSelecting="dsEquipsByGrandezaSelecting">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvEquipamentos" Name="idGrandeza" PropertyName="SelectedDataKey.Values[idGrandeza]"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_EstadoEquipamento" runat="server" SelectMethod="GetData"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.EstadoEquipamentoDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_TipoEquipamento" runat="server" SelectMethod="GetData"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.TipoEquipamentoDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_UnidadeAlcance" runat="server" SelectMethod="GetData"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.UnidadeAlcanceDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_Classe" runat="server" SelectMethod="GetData" TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.ClasseDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_EstadoUtilizacao" runat="server" SelectMethod="GetData"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.EstadoUtilizacaoEquipamentoDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_EstadoRelacaoCalibracao" runat="server" SelectMethod="GetData"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.EstadoRelacaoCalibracaoDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_TipoIntervencao" runat="server" SelectMethod="GetData"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.TipoIntervencaoDDTableAdapter"
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OBJDS_Empresa" runat="server" TypeName="LabMetro.DataAccessLayer.dlEmpresasTableAdapters.EmpresaDDTableAdapter"
        SelectMethod="GetEmpresaDDActivas" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="objds_Marca" runat="server" SelectMethod="GetData" TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.MarcaDDTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="objds_Modelo" runat="server" SelectMethod="GetDataByIdMarca"
        TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter"
        OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter Name="idMarca" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <fieldset>
        <legend>
            <%=Resources.Resource.GestaoEquipamentosClientes %></legend>
        <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label><br />
        <table width="90%">
            <tr>
                <td>
                    <%=Resources.Resource.NomeEmpresa %>:
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" OnTextChanged="txtEmpresaTextChanged"></asp:TextBox>
                </td>
                <td>
                    <asp:Button class="button" ID="btnEmpresas" CssClass="button" runat="server" Width="80"
                        CausesValidation="false" Text="<%$ Resources:Resource, verEmpresas %>" OnClick="btnEmpresas_Click" />
                </td>
            </tr>
            <tr id="trEmpresa" runat="server">
                <td colspan="2">
                    <div>
                        <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                            DataTextField="nome" Width="90%">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvEquipamentos" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="idEquipamento,idGrandeza" DataSourceID="OBJDS_ListaEquipamentosByEmpresa"
            PagerStyle-BackColor="White" PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Right"
            PagerStyle-Font-Bold="true" PagerStyle-Font-Size="0.9em" OnRowDataBound="CoresLinhas_RowDataBound">
            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
            <Columns>
                <asp:ButtonField CommandName="Select" Text="<%$ Resources:Resource, Ver %>" />
                <asp:BoundField DataField="TipoEquipamento" HeaderText="<%$ Resources:Resource, Tipo %>"
                    SortExpression="TipoEquipamento" />
                <asp:BoundField DataField="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdent %>"
                    SortExpression="numIdentificacao" />
                <asp:BoundField DataField="numSerie" HeaderText="<%$ Resources:Resource, NumSerie %>"
                    SortExpression="numSerie" />
                <asp:BoundField DataField="periodicidadeCalibracao" HeaderText="<%$ Resources:Resource, per_m %>"
                    SortExpression="periodicidadeCalibracao" />
                <asp:BoundField DataField="dtUltimaCalibracao" HeaderText="<%$ Resources:Resource, DataUltimaCalibracao %>"
                    SortExpression="dtUltimaCalibracao" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="estadoRelacaoCalibracao" HeaderText="<%$ Resources:Resource, EstadoRelacaoCalibracao %>"
                    SortExpression="estadoRelacaoCalibracao" />
                <asp:BoundField DataField="localizacao" HeaderText="<%$ Resources:Resource, Localizacao %>"
                    SortExpression="localizacao" />
                <asp:BoundField DataField="estadoUtilizacao" HeaderText="<%$ Resources:Resource, EstadoUtilizacao %>"
                    SortExpression="estadoUtilizacao" />
                <asp:BoundField DataField="estadoEquipamento" HeaderText="<%$ Resources:Resource, EstadoEquipamento %>"
                    SortExpression="estadoEquipamento" />
                <asp:BoundField DataField="observacoes" HeaderText="<%$ Resources:Resource, Observacoes %>"
                    SortExpression="observacoes" />
                <asp:TemplateField SortExpression="activo" HeaderText="<%$ Resources:Resource, Activo %>">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColor" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.activo")))%>'
                            Width="10" Height="10" BorderStyle="None" BorderWidth="0" runat="server">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:HyperLinkField DataNavigateUrlFields="idEquipamento" DataNavigateUrlFormatString="FormIntervencao.aspx?id={0}"
                    Text="Intervencoes" />
                <asp:TemplateField AccessibleHeaderText="Edit">
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnEdit" PostBackUrl="~/FormIntervencao.aspx"
                            Text="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle HorizontalAlign="Right" BackColor="White" Font-Bold="True" Font-Size="0.9em">
            </PagerStyle>
        </asp:GridView>
        <table cellspacing="0" cellpadding="0" width="584" align="left" border="0">
            <tr>
                <td>
                    <asp:LinkButton ID="lbHistorico" runat="server" CausesValidation="False" OnClick="lbHistorico_Click"><%=Resources.Resource.Historico %></asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="lbServicos" runat="server" CausesValidation="False" OnClick="lbServicos_Click"><%=Resources.Resource.Servicos %></asp:LinkButton>&nbsp;
                    | &nbsp;
                    <asp:LinkButton ID="lbCertificados" runat="server" CausesValidation="False" OnClick="lbCertificados_Click"><%=Resources.Resource.Certificados %></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvGenerico" runat="server" AutoGenerateColumns="true">
        </asp:GridView>
        <asp:GridView ID="gvCertificados" runat="server" AutoGenerateColumns="false" DataKeyNames="nomeDocumento"
            PagerSettings-Mode="NumericFirstLast" OnRowCommand="visualisarDocumento" OnDataBound="gvCertificados_databound">
            <Columns>
                <asp:ButtonField Text="visualisarDocumento" ButtonType="Link" DataTextField="nomeDocumento"
                    Visible="False" SortExpression="nomeDocumento" HeaderText="<%$ Resources:Resource, VerDoc %>"
                    CommandName="Select">
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle"></ItemStyle>
                </asp:ButtonField>
                <asp:BoundField DataField="refServico" SortExpression="refServico" HeaderText="<%$ Resources:Resource, RefCalib %>">
                </asp:BoundField>
                <asp:BoundField DataField="dtCertificado" SortExpression="dtCertificado" HeaderText="<%$ Resources:Resource,DataEmissao %>"
                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                <asp:BoundField DataField="responsavelLaboratorio" SortExpression="responsavelLaboratorio"
                    HeaderText="<%$ resources:Resource, AprovadoPor %>" ItemStyle-Width="20%"></asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:Resource, Ficheiro %>" SortExpression="nomeFicheiro">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#downloadpath(DataBinder.Eval(Container,"DataItem.nomeDocumento"))%>'
                            ID="Hyperlink1" Target="new">
											<%# DataBinder.Eval(Container.DataItem, "nomeDocumento")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:DataGrid ID="dgIntervencoes" runat="server">
        </asp:DataGrid>
        <asp:Button ID="btnExportGrid" runat="server" Text="<%$ Resources:Resource, ExportarParaExcel %>"
            OnClick="BtnExportGrid_Click" />
        <asp:FormView ID="fvEquipqmantos" CssClass="fvForm" runat="server" AllowPaging="True"
            DataKeyNames="idEquipamento" DataSourceID="OBJDS_DetalheEquipamento" 
            OnPageIndexChanging="fv_PageIndexChanging"
            OnItemInserted="fv_Inserted" OnItemInserting="fv_Inserting" OnItemUpdated="fv_Updated"
            OnItemUpdating="fv_Updating" OnItemCommand="fv_ItemCommand" PagerSettings-Mode="NumericFirstLast"
            OnDataBound="fv_DataBound">
            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
            <EditItemTemplate>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.DetalhesEquipamento %></legend>
                    <table>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.IDEquipamentoBD %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:Label CssClass="formLabel" ID="idEquipamentoLabel1" runat="server" Text='<%# Eval("idEquipamento") %>'
                                    Font-Italic="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Empresa %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList runat="server" ID="ddEmpresa" DataTextField="nome" DataValueField="idEmpresa"
                                    DataSourceID="OBJDS_Empresa" SelectedValue='<%# Bind("idEmpresa") %>' Enabled="false">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.TipoEquipamento %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList runat="server" ID="ddTipoEquipamento" DataValueField="idTipoEquipamento"
                                    DataTextField="descricao" DataSourceID="OBJDS_TipoEquipamentoByGrandeza" SelectedValue='<%# Bind("idTipoEquipamento") %>'>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.NumSerie %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="numSerieTextBox" runat="server" Text='<%# Bind("numSerie") %>' Width="75"
                                    MaxLength="50" />
                                <asp:RequiredFieldValidator ID="reqNumSerie" Display="Static" ErrorMessage="*" ControlToValidate="numSerieTextBox"
                                    runat="server" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.NumIdentificacao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="numIdentificacaoTextBox" runat="server" Text='<%# Bind("numIdentificacao") %>'
                                    Width="200" MaxLength="150" />
                                <asp:RequiredFieldValidator ID="reqNumIdent" Display="Static" ErrorMessage="*" ControlToValidate="numIdentificacaoTextBox"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.UnidMedicaoInf %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="alcanceInfTextBox" runat="server" Text='<%# Bind("alcanceInf") %>' /><asp:CompareValidator
                                    ID="CompareValidator4" runat="server" ControlToValidate="alcanceInfTextBox" Operator="DataTypeCheck"
                                    Type="Double" Text="núm!" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.UnidMedicaoSup %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="alcanceSupTextBox" runat="server" Text='<%# Bind("alcanceSup") %>' /><asp:CompareValidator
                                    ID="CompareValidator5" runat="server" ControlToValidate="alcanceSupTextBox" Operator="DataTypeCheck"
                                    Type="Double" Text="núm!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Unidade %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddUnidadeAlcance" DataValueField="idUnidadeAlcance"
                                    DataTextField="descricao" DataSourceID="OBJDS_UnidadeAlcance" SelectedValue='<%# Bind("idUnidadeAlcance") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.AlcanceAlt %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="alcanceTextBox" runat="server" Text='<%# Bind("alcance") %>' Width="75"
                                    MaxLength="60" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Resolucao %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="resolucaoTextBox" runat="server" Text='<%# Bind("resolucao") %>'
                                    Width="75" MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Classe %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddClasse" DataTextField="descricao" DataValueField="idClasse"
                                    DataSourceID="OBJDS_Classe" SelectedValue='<%# Bind("idClasse") %>' AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.ClasseAlt %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="classeTextBox" runat="server" Text='<%# Bind("classe") %>' Width="40"
                                    MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Forma %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="formaTextBox" runat="server" Text='<%# Bind("forma") %>' Width="40"
                                    MaxLength="30" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.Fabricante %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="fabricanteTextBox" runat="server" Text='<%# Bind("fabricante") %>'
                                    Width="75" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.RefUltimaCalibracao %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="refUltimaCalibracaoTextBox" runat="server" Text='<%# Bind("refUltimaCalibracao") %>'
                                    Width="75" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.DataUltimaCalibracao %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="dtUltimaCalibracaoTextBox" runat="server" Text='<%# Bind("dtUltimaCalibracao","{0:dd/MM/yyyy}") %>' />
                                dd-mm-aaaa<asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="dtUltimaCalibracaoTextBox"
                                    Operator="DataTypeCheck" Type="Date" Text="data!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.PeriodCalibMeses %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="periodicidadeCalibracaoTextBox" runat="server" Text='<%# Bind("periodicidadeCalibracao") %>' />
                                <asp:CompareValidator ID="compvalPeriod" runat="server" ControlToValidate="periodicidadeCalibracaoTextBox"
                                    Operator="DataTypeCheck" Type="Integer" Text="int!" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.Activo %>:</label>
                            </td>
                            <td>
                                <asp:CheckBox ID="activoCheckBox" runat="server" Checked='<%# Bind("activo") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.CampExtra1 %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="campo1TextBox" runat="server" Text='<%# Bind("campo1") %>' Width="75"
                                    MaxLength="50" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CampExtra2 %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="campo2TextBox" runat="server" Text='<%# Bind("campo2") %>' Width="75"
                                    MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Marca %>:</label>
                            </td>
                            <td>
                                <asp:ObjectDataSource ID="OBJDS_MarcaEdit" runat="server" SelectMethod="GetData"
                                    TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.MarcaDDTableAdapter"
                                    OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                                <asp:DropDownList runat="server" ID="ddMarcaEdit" DataTextField="descricao" DataValueField="idMarca"
                                    DataSourceID="OBJDS_MarcaEdit" SelectedValue='<%# Bind("idMarca") %>' AppendDataBoundItems="true"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddMarcaEdit_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.Modelo %>:</label>
                            </td>
                            <td>
                                <asp:ObjectDataSource ID="OBJDS_ModeloEdit" runat="server" SelectMethod="GetDataByIdMarca"
                                    TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="0" Name="idMarca" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:DropDownList runat="server" ID="ddModeloEdit" AppendDataBoundItems="true" DataTextField="descricao"
                                    DataValueField="idModelo" DataSourceID="OBJDS_ModeloEdit">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Observacoes %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="observacoesTextBox" runat="server" Text='<%# Bind("observacoes") %>'
                                    Width="300" MaxLength="250" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.DadosOperacionais %></legend>
                    <table>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.DataEntrada %></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="dtEntradaServicoTextBox" runat="server" Text='<%# Bind("dtEntradaServico","{0:dd/MM/yyyy}") %>' />
                                dd-mm-aaaa<asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="dtEntradaServicoTextBox"
                                    Operator="DataTypeCheck" Type="Date" Text="data!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Fornecedor %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="fornecedorTextBox" runat="server" Text='<%# Bind("fornecedor") %>'
                                    Width="150" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.ResponsavelEquipamento %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="responsavelEquipamentoTextBox" runat="server" Text='<%# Bind("responsavelEquipamento") %>'
                                    Width="150" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.PrecoCompraEuros %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="precoClienteEurosTextBox" runat="server" Text='<%# Bind("precoClienteEuros","{0:0.00}") %>' />
                                <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="precoClienteEurosTextBox"
                                    Operator="DataTypeCheck" Type="Currency" Text="!" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CustoEmEuros %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="custoClienteEurosTextBox" runat="server" Text='<%# Bind("custoClienteEuros","{0:0.00}") %>' />
                                <asp:CompareValidator ID="CompareValidator9" runat="server" ControlToValidate="custoClienteEurosTextBox"
                                    Operator="DataTypeCheck" Type="Currency" Text="!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Localizacao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="localizacaoTextBox" runat="server" Text='<%# Bind("localizacao") %>'
                                    Width="150" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.EstadoUtilizacao %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddEstadoUtilizacao" DataValueField="idEstadoUtilizacao"
                                    DataTextField="descricao" DataSourceID="OBJDS_EstadoUtilizacao" SelectedValue='<%# Bind("idEstadoUtilizacao") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.EstadoEquipamento %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddEstadoEquipamento" DataTextField="descricao"
                                    DataValueField="idEstadoEquipamento" DataSourceID="OBJDS_EstadoEquipamento" SelectedValue='<%# Bind("idEstadoEquipamento") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.TipoIntervencao %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="idTipoIntervencao" DataTextField="descricao"
                                    DataValueField="idTipoIntervencao" DataSourceID="OBJDS_TipoIntervencao" SelectedValue='<%# Bind("idTipoIntervencao") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.DadosRelativosCalibracao %></legend>
                    <table>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.CalibradoInternamente %>:</label>
                            </td>
                            <td>
                                <asp:CheckBox ID="calibIntCheckBox" runat="server" Checked='<%# Bind("calibInt") %>' />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CertificadoConclusivo %>:</label>
                            </td>
                            <td>
                                <asp:CheckBox ID="bCertConclusivoCheckBox" runat="server" Checked='<%# Bind("bCertConclusivo") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.EntidadeUltimaCalibracao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="entidadeUltimaCalibracaoTextBox" runat="server" Text='<%# Bind("entidadeUltimaCalibracao") %>'
                                    Width="" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CriteriosAceitacao %>:</label>
                            </td>
                            <td>
                               <asp:TextBox ID="criteriosTextBox" runat="server" Text='<%# Bind("criterios") %>' TextMode="MultiLine" Width="500" MaxLength="1500" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.PontosCalibracao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="pontosCalibracaoTextBox" runat="server" Text='<%# Bind("pontosCalibracao") %>'
                                    Width="" MaxLength="150" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.EstadoRelacaoCalibracao %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddEstadoRelacaoCalibracao" DataTextField="descricao"
                                    DataValueField="idEstadoRelacaoCalibracao" DataSourceID="OBJDS_EstadoRelacaoCalibracao"
                                    SelectedValue='<%# Bind("idEstadoRelacaoCalibracao") %>' AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.Notas %></legend>
                    <label>
                        <%=Resources.Resource.TextoLivre %>:</label>
                    <asp:TextBox ID="textoTextBox" runat="server" Text='<%# Bind("texto") %>' TextMode="MultiLine"
                        Width="500" />
                    <br />
                </fieldset>
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                    Text="<%$ Resources:Resource, Actualizar %>" />
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="<%$ Resources:Resource, Cancelar %>" />
            </EditItemTemplate>
            <InsertItemTemplate>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.DetalhesEquipamento %></legend>
                    <table>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.TipoEquipamento %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList runat="server" ID="ddTipoEquipamento" DataValueField="idTipoEquipamento"
                                    DataTextField="descricao" DataSourceID="OBJDS_TipoEquipamento" SelectedValue='<%# Bind("idTipoEquipamento") %>'>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.NumSerie %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="numSerieTextBox" runat="server" Text='<%# Bind("numSerie") %>' Width="75"
                                    MaxLength="50" /><asp:RequiredFieldValidator ID="reqNumSerie" Display="Static" ErrorMessage="*"
                                        ControlToValidate="numSerieTextBox" runat="server" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.NumIdentificacao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="numIdentificacaoTextBox" runat="server" Text='<%# Bind("numIdentificacao") %>'
                                    Width="200" MaxLength="150" />
                                <asp:RequiredFieldValidator ID="reqNumIdent" Display="Static" ErrorMessage="*" ControlToValidate="numIdentificacaoTextBox"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.UnidMedicaoInf %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="alcanceInfTextBox" runat="server" Text='<%# Bind("alcanceInf") %>' /><asp:CompareValidator
                                    ID="CompareValidator1" runat="server" ControlToValidate="alcanceInfTextBox" Operator="DataTypeCheck"
                                    Type="Double" Text="núm!" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.UnidMedicaoSup %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="alcanceSupTextBox" runat="server" Text='<%# Bind("alcanceSup") %>' />
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="alcanceSupTextBox"
                                    Operator="DataTypeCheck" Type="Double" Text="núm!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Unidade %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddUnidadeAlcance" DataValueField="idUnidadeAlcance"
                                    DataTextField="descricao" DataSourceID="OBJDS_UnidadeAlcance" SelectedValue='<%# Bind("idUnidadeAlcance") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.AlcanceAlt %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="alcanceTextBox" runat="server" Text='<%# Bind("alcance") %>' Width="75"
                                    MaxLength="60" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Resolucao %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="resolucaoTextBox" runat="server" Text='<%# Bind("resolucao") %>'
                                    Width="40" MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Classe %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddClasse" DataTextField="descricao" DataValueField="idClasse"
                                    DataSourceID="OBJDS_Classe" SelectedValue='<%# Bind("idClasse") %>' AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.ClasseAlt %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="classeTextBox" runat="server" Text='<%# Bind("classe") %>' Width="40"
                                    MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Forma %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="formaTextBox" runat="server" Text='<%# Bind("forma") %>' Weith="40"
                                    MaxLength="30" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.Fabricante %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="fabricanteTextBox" runat="server" Text='<%# Bind("fabricante") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.ReferenciaUltimaCalibracao %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="refUltimaCalibracaoTextBox" runat="server" Text='<%# Bind("refUltimaCalibracao") %>'
                                    Width="60" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.DataUltimaCalibracao %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="dtUltimaCalibracaoTextBox" runat="server" Text='<%# Bind("dtUltimaCalibracao") %>' />
                                dd-mm-aaaa
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.PeriodCalibMeses %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="periodicidadeCalibracaoTextBox" runat="server" Text='<%# Bind("periodicidadeCalibracao") %>' />
                                <asp:CompareValidator ID="compvalPeriod" runat="server" ControlToValidate="periodicidadeCalibracaoTextBox"
                                    Operator="DataTypeCheck" Type="Integer" Text="int!" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.Activo %>:</label>
                            </td>
                            <td>
                                <asp:CheckBox ID="activoCheckBox" runat="server" Checked='<%# Bind("activo") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.CampExtra1 %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="campo1TextBox" runat="server" Text='<%# Bind("campo1") %>' Width="60"
                                    MaxLength="50" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CampExtra2 %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="campo2TextBox" runat="server" Text='<%# Bind("campo2") %>' Width="60"
                                    MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Marca %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddMarca" DataTextField="descricao" DataValueField="idMarca"
                                    DataSourceID="OBJDS_Marca" SelectedValue='<%# Bind("idMarca") %>' AppendDataBoundItems="true"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.Modelo %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddModelo" DataTextField="descricao" DataValueField="idModelo"
                                    DataSourceID="OBJDS_Modelo" SelectedValue='<%# Bind("idModelo") %>' AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Observacoes %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="observacoesTextBox" runat="server" Text='<%# Bind("observacoes") %>'
                                    Width="300" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.DadosOperacionais %></legend>
                    <table>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.DtEntradaServico %>:</label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="dtEntradaServicoTextBox" runat="server" Text='<%# Bind("dtEntradaServico") %>' />
                                dd-mm-aaaa
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Fornecedor %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="fornecedorTextBox" runat="server" Text='<%# Bind("fornecedor") %>'
                                    Width="150" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.ResponsavelEquipamento %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="responsavelEquipamentoTextBox" runat="server" Text='<%# Bind("responsavelEquipamento") %>'
                                    Width="150" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.PrecoCompraEuros %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="precoClienteEurosTextBox" runat="server" Text='<%# Bind("precoClienteEuros","{0:0.00}") %>' /><asp:CompareValidator
                                    ID="CompareValidator3" runat="server" ControlToValidate="precoClienteEurosTextBox"
                                    Operator="DataTypeCheck" Type="Currency" Text="!" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CustoEmEuros %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="custoClienteEurosTextBox" runat="server" Text='<%# Bind("custoClienteEuros","{0:0.00}") %>' /><asp:CompareValidator
                                    ID="CompareValidator4" runat="server" ControlToValidate="custoClienteEurosTextBox"
                                    Operator="DataTypeCheck" Type="Currency" Text="!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.Localizacao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="localizacaoTextBox" runat="server" Text='<%# Bind("localizacao") %>'
                                    Width="150" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.EstadoUtilizacao %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddEstadoUtilizacao" DataValueField="idEstadoUtilizacao"
                                    DataTextField="descricao" DataSourceID="OBJDS_EstadoUtilizacao" SelectedValue='<%# Bind("idEstadoUtilizacao") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.EstadoEquipamento %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddEstadoEquipamento" DataTextField="descricao"
                                    DataValueField="idEstadoEquipamento" DataSourceID="OBJDS_EstadoEquipamento" SelectedValue='<%# Bind("idEstadoEquipamento") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.TipoIntervencao %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="idTipoIntervencao" DataTextField="descricao"
                                    DataValueField="idTipoIntervencao" DataSourceID="OBJDS_TipoIntervencao" SelectedValue='<%# Bind("idTipoIntervencao") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.DadosRelativosCalibracao %></legend>
                    <table>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.CalibradoInternamente %>:</label>
                            </td>
                            <td>
                                <asp:CheckBox ID="calibIntCheckBox" runat="server" Checked='<%# Bind("calibInt") %>' />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CertificadoConclusivo %>:</label>
                            </td>
                            <td>
                                <asp:CheckBox ID="bCertConclusivoCheckBox" runat="server" Checked='<%# Bind("bCertConclusivo") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.EntidadeUltimaCalibracao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="entidadeUltimaCalibracaoTextBox" runat="server" Text='<%# Bind("entidadeUltimaCalibracao") %>'
                                    Width="" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.CriteriosAceitacao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="criteriosTextBox" runat="server" Text='<%# Bind("criterios") %>' TextMode="MultiLine" Width="500" MaxLength="1500"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=Resources.Resource.PontosCalibracao %>:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="pontosCalibracaoTextBox" runat="server" Text='<%# Bind("pontosCalibracao") %>'
                                    Width="" MaxLength="100" />
                            </td>
                            <td>
                                <label>
                                    <%=Resources.Resource.EstadoRelacaoCalibracao %>:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddEstadoRelacaoCalibracao" DataTextField="descricao"
                                    DataValueField="idEstadoRelacaoCalibracao" DataSourceID="OBJDS_EstadoRelacaoCalibracao"
                                    SelectedValue='<%# Bind("idEstadoRelacaoCalibracao") %>' AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text="---"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <%=Resources.Resource.Notas %>:</legend>
                    <label>
                        <%=Resources.Resource.TextoLivre %>:</label>
                    <asp:TextBox ID="textoTextBox" TextMode="Multiline" Width="300" runat="server" Text='<%# Bind("texto") %>' />
                    <br />
                </fieldset>
                <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                    Text="Insert" />
                <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" />
            </InsertItemTemplate>
            <ItemTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="TipoEquipamentoLabel" runat="server" Text='<%# Bind("TipoEquipamento") %>' /><br />
                    </legend>
                    <fieldset>
                        <legend>
                            <%=Resources.Resource.Identificacao %></legend>
                        <table>
                            <tr>
                                <td>
                                    <%=Resources.Resource.ID_BD %>:
                                </td>
                                <td colspan="3">
                                    <asp:Label CssClass="formLabel" ID="idEquipamentoLabel" runat="server" Text='<%# Eval("idEquipamento") %>'
                                        Font-Italic="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.NumIdent %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="numIdentificacaoLabel" runat="server" Text='<%# Bind("numIdentificacao") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.NumSerie %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="numSerieLabel" runat="server" Text='<%# Bind("numSerie") %>'
                                        Width="75" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.Familia %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="FamiliaLabel" runat="server" Text='<%# Bind("Familia") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.Grandeza %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="GrandezaLabel" runat="server" Text='<%# Bind("grandeza") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.Activo %>:
                                </td>
                                <td>
                                    <asp:CheckBox ID="activoCheckBox" runat="server" Checked='<%# Bind("activo") %>'
                                        Enabled="false" />
                                </td>
                                <td colspan="2">
                                    <i>
                                        <%=Resources.Resource.FraseCaixaDesmarcada %></i>
                                </td>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <%=Resources.Resource.Especificacoes %></legend>
                        <table>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.IntervaloMedicaoInferior %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="alcanceInfLabel" runat="server" Text='<%# Bind("alcanceInf") %>' />
                                </td>
                                <td>
                                    Sup.:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="alcanceSupLabel" runat="server" Text='<%# Bind("alcanceSup") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.Unidade %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="idUnidadeAlcanceLabel" runat="server" Text='<%# Bind("sAlcance") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.Ou %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="alcanceLabel" runat="server" Text='<%# Bind("alcance") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=Resources.Resource.Resolucao %>:
                                </td>
                                <td colspan="6">
                                    <asp:Label CssClass="formLabel" ID="resolucaoLabel" runat="server" Text='<%# Bind("resolucao") %>'
                                        Width="40" MaxLength="30" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=Resources.Resource.Marca %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="marcaLabel" runat="server" Text='<%# Bind("marca") %>'
                                        Width="75" MaxLength="50" />
                                </td>
                                <td colspan="2">
                                    <%=Resources.Resource.Modelo %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="modeloLabel" runat="server" Text='<%# Bind("modelo") %>'
                                        Width="75" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=Resources.Resource.Classe %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="idClasseLabel" runat="server" Text='<%# Bind("sClasse") %>'
                                        Width="40" MaxLength="30" />
                                </td>
                                <td colspan="2">
                                    <%=Resources.Resource.Ou %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="classeLabel" runat="server" Text='<%# Bind("classe") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=Resources.Resource.Forma %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="formaLabel" runat="server" Text='<%# Bind("forma") %>'
                                        Width="40" MaxLength="30" />
                                </td>
                                <td colspan="2">
                                    <%=Resources.Resource.Fabricante %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="fabricanteLabel" runat="server" Text='<%# Bind("fabricante") %>'
                                        Width="75" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=Resources.Resource.CampExtra1 %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="campo1Label" runat="server" Text='<%# Bind("campo1") %>'
                                        Width="75" MaxLength="50" />
                                </td>
                                <td colspan="2">
                                    <%=Resources.Resource.CampExtra2 %>:
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="formLabel" ID="campo2Label" runat="server" Text='<%# Bind("campo2") %>'
                                        Width="75" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=Resources.Resource.Observacoes %>:
                                </td>
                                <td colspan="6">
                                    <asp:Label CssClass="formLabel" ID="observacoesLabel" runat="server" Text='<%# Bind("observacoes") %>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <%=Resources.Resource.DadosOperacionais %></legend>
                        <table>
                            <tr>
                                <td>
                                    <%=Resources.Resource.DtEntradaServico %>:
                                </td>
                                <td colspan="3">
                                    <asp:Label CssClass="formLabel" ID="dtEntradaServicoLabel" runat="server" Text='<%# Bind("dtEntradaServico","{0:dd/MM/yyyy}" )%>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.Fornecedor %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="fornecedorLabel" runat="server" Text='<%# Bind("fornecedor") %>'
                                        Width="150" MaxLength="100" />
                                </td>
                                <td>
                                    <%=Resources.Resource.ResponsavelEquipamento %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="responsavelEquipamentoLabel" runat="server" Text='<%# Bind("responsavelEquipamento") %>'
                                        Width="150" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.PrecoCompraEuros %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="precoClienteEurosLabel" runat="server" Text='<%# Bind("precoClienteEuros","{0:C2}") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.CustoEquipamento %>(euros):
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="custoClienteEurosLabel" runat="server" Text='<%# Bind("custoClienteEuros","{0:C2}") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.Localizacao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="localizacaoLabel" runat="server" Text='<%# Bind("localizacao") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.EstadoUtilizacao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="estadoUtilizacaoLabel" runat="server" Text='<%# Bind("estadoUtilizacao") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.EstadoEquipamento %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="estadoEquipamentoLabel" runat="server" Text='<%# Bind("estadoEquipamento") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.TipoIntervencao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="Label1" runat="server" Text='<%# Bind("tipoIntervencao") %>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <%=Resources.Resource.DadosRelativosCalibracao %>(<%=Resources.Resource.Ensaio %>
                            etc.)</legend>
                        <table>
                            <tr>
                                <td>
                                    <%=Resources.Resource.DataUltimaCalibracao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="dtUltimaCalibracaoLabel" runat="server" Text='<%# Bind("dtUltimaCalibracao","{0:dd/MM/yyyy}") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.PeriodCalibMeses %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="periodicidadeCalibracaoLabel" runat="server"
                                        Text='<%# Bind("periodicidadeCalibracao") %>' />
                                </td>
                            </tr>
                          <tr>
                                <td>
                                    <%=Resources.Resource.PontosCalibracao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="pontosCalibracaoLabel" runat="server" Text='<%# Bind("pontosCalibracao") %>' />
                                </td>
                                <td></td><td></td>
                            </tr>
                              <tr>
                                <td>
                                    <%=Resources.Resource.CriteriosAceitacao %>:
                                </td>
                                <td colspan="3">
                                    <asp:Label CssClass="formLabel" ID="criteriosLabel" runat="server" Text='<%# Eval("criterios").ToString().Replace("\n","<br />")%>' />                                        
                                </td>
                                
                                </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.CalibradoInternamente %>:
                                </td>
                                <td>
                                    <asp:CheckBox ID="calibIntCheckBox" runat="server" Checked='<%# Bind("calibInt") %>'
                                        Enabled="false" />
                                </td>
                                <td>
                                    <%=Resources.Resource.RequerCertificadoConclusivo %>:
                                </td>
                                <td>
                                    <asp:CheckBox ID="bCertConclusivoCheckBox" runat="server" Checked='<%# Bind("bCertConclusivo") %>'
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.ResultadoUltimaCalibracao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="estadoRelacaoCalibracaoLabel" runat="server"
                                        Text='<%# Bind("estadoRelacaoCalibracao") %>' />
                                </td>
                                <td colspan="2">
                                    <i>
                                        <%=Resources.Resource.FraseNotaDesenvolvimento %></i>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.ReferenciaUltimaCalibracao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="refUltimaCalibracaoLabel" runat="server" Text='<%# Bind("refUltimaCalibracao") %>' />
                                </td>
                                <td>
                                    <%=Resources.Resource.EntidadeUltimaCalibracao %>:
                                </td>
                                <td>
                                    <asp:Label CssClass="formLabel" ID="entidadeUltimaCalibracaoLabel" runat="server"
                                        Text='<%# Bind("entidadeUltimaCalibracao") %>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <%=Resources.Resource.Notas %>
                        </legend>
                        <asp:Label CssClass="formLabel" ID="textoLabel" runat="server" Text='<%# Eval("texto").ToString().Replace("\n","<br />") %>' />
                        <br />
                    </fieldset>
                    <asp:LinkButton ID="LinkButton1" Text="<%$ Resources:Resource, Editar %>" CommandName="Edit"
                        runat="server" />
                    <asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New"
                        Text="<%$ Resources:Resource, CriarNovo %>" /><br />
                </fieldset>
                </fieldset>
            </ItemTemplate>
            <PagerTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="FirstButton" CommandName="Page" CommandArgument="First" Text="<<"
                                runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton ID="PrevButton" CommandName="Page" CommandArgument="Prev" Text="<"
                                runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton ID="NextButton" CommandName="Page" CommandArgument="Next" Text=">"
                                runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton ID="LastButton" CommandName="Page" CommandArgument="Last" Text=">>"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </PagerTemplate>
        </asp:FormView>
        <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="<%$ Resources:Resource, FraseAtencaoCamposMarc %>"
            ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
    </fieldset>
</asp:Content>
