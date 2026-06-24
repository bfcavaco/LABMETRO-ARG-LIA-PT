<%@ Page Language="c#" CodeBehind="FormEquipamento.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormEquipamento" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">
 
<script type="text/javascript">



    $(document).ready(function () {
        $("#<%=txtSearchMarca.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("Webservices/wsMarca.asmx/GetMarcas") %>',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[0],
                                val: item.split('-')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("#<%=hfidMarca.ClientID %>").val(i.item.val);
            },
            minLength: 1
        });
    });


</script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>
            <%=Resources.Resource.Equipamento %></legend>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaNif_TextChanged"></asp:TextBox>&nbsp;
                    &nbsp;<asp:Button class="button" ID="btnEmpresas" CssClass="button" runat="server"
                        CausesValidation="false" Text="<%$ Resources:Resource, verEmpresas %>"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td colspan="3">
                    <asp:UpdatePanel runat="server" ID="updatePanelEmpresa" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnEmpresas" />
                            <asp:AsyncPostBackTrigger ControlID="txtPesquisaEmpresa" />
                            <asp:AsyncPostBackTrigger ControlID="txtPesquisaNif" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:DropDownList ID="ddEmpresa" runat="server" DataValueField="idEmpresa" DataTextField="nome">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ID_BD %>:
                </td>
                <td>
                    <asp:Label ID="lblIdEquipamento" runat="server"></asp:Label>
                </td>
                <td>
                    <%=Resources.Resource.Activo %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddEstado" runat="server">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Activo %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Inactivo %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddTipoEquipamento" runat="server" DataValueField="ident" DataTextField="descricao" AutoPostBack="true">
                    </asp:DropDownList>
                   <asp:Label ID="lblTipoEquipamento" runat="server"></asp:Label>
                      <asp:RequiredFieldValidator
                        ID="RequiredFieldValidatortipo" runat="server" ControlToValidate="ddTipoEquipamento" Enabled="true" CssClass="lblMessage">Indicar Tipo Actual!</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumSerie %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumSerie" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumIdentificacao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdentif" runat="server" MaxLength="150"></asp:TextBox><asp:CustomValidator
                        OnServerValidate="validaEquipamento" runat="server" ID="cv1" ErrorMessage="Tem de preencher o número de série ou o número de identificação.">*</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.GamaEntre %>:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAlcanceInf" runat="server" Width="50"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator2" runat="server" ControlToValidate="txtAlcanceInf" Operator="DataTypeCheck"
                        Type="Double" ErrorMessage="!num" Display="static"></asp:CompareValidator>&nbsp;-&nbsp;
                    <asp:TextBox ID="txtAlcanceSup" runat="server" Width="50"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator3" runat="server" ControlToValidate="txtAlcanceSup" Operator="DataTypeCheck"
                        Type="Double" ErrorMessage="!num" Display="static"></asp:CompareValidator><%=Resources.Resource.UnidadeGama %>:<asp:DropDownList
                            ID="ddUnidadeAlcance" runat="server" DataValueField="ident" DataTextField="descricao">
                        </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.OuGama %>:
                </td>
                <td>
                    <asp:TextBox ID="txtAlcance" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Divisao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtResolucao" runat="server"></asp:TextBox>
                </td>
            </tr>
        <%--    <asp:ObjectDataSource ID="OBJDS_Modelo_WS" runat="server" SelectMethod="wsDTModelo"
                TypeName="LabMetro.Webservices.wsMarcaModelo">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddMarcaForm" Name="idMarca" PropertyName="SelectedValue"
                        DefaultValue="0" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>
           <%-- <asp:ObjectDataSource ID="OBJDS_Marca_WS" runat="server" SelectMethod="wsDTMarca"
                TypeName="LabMetro.Webservices.wsMarcaModelo"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="OBJDS_Marca" runat="server" SelectMethod="GetData" TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.MarcaDDTableAdapter"
                OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>--%>
           <%-- <asp:ObjectDataSource ID="OBJDS_Modelo" runat="server" SelectMethod="GetDataByIdMarca"
                TypeName="LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter"
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfidMarca" PropertyName="Value" Name="idMarca" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>
            <tr>
                <td>
                    <%=Resources.Resource.Marca %>:
                </td>
                <td>
                  <%--  <asp:DropDownList ID="ddMarcaForm" runat="server" AutoPostBack="true" DataValueField="idMarca"
                        DataTextField="descricao" Width="175px" AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="---"></asp:ListItem>
                    </asp:DropDownList>--%>
                     <asp:DropDownList ID="txtSearchMarca" runat="server"></asp:DropDownList>
    <asp:HiddenField ID="hfidMarca" runat="server" />
                </td>
                <td>
                    <%=Resources.Resource.Modelo %>:
                </td>
                <td>
                   <%-- <asp:UpdatePanel runat="server" ID="updatePanelEquipamentos" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtSearchMarca" />
                        </Triggers>
                        <ContentTemplate>--%>
                            <asp:DropDownList ID="ddModeloForm" runat="server" DataValueField="idModelo" DataTextField="descricao"
                                Width="175px"  AppendDataBoundItems="false" OnDataBound="MyListDataBound">
                            </asp:DropDownList>
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Classe %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddClassse" runat="server" DataValueField="idClasse" DataTextField="descricao"
                        Width="120">
                    </asp:DropDownList>
                </td>
                <td>
                    <%=Resources.Resource.Indicacao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtForma" runat="server"></asp:TextBox>
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
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Fabricante %>:
                </td>
                <td>
                    <asp:TextBox ID="txtFabricante" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.PeriodicidadeCalib %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPeriodicidade" runat="server"></asp:TextBox>&nbsp;<%=Resources.Resource.Meses %>
                    <asp:RangeValidator runat="server" ControlToValidate="txtPeriodicidade" Type="Integer" MaximumValue="120" MinimumValue="1" ErrorMessage="val. period."></asp:RangeValidator>

                </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.ReferenciaUltimaCalibracao %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefUltimaCalib" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.DataUltimaCalibracao %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataUltimaCalib" runat="server"></asp:TextBox><asp:CompareValidator
                            ID="Comparevalidator1" runat="server" ControlToValidate="txtDataUltimaCalib"
                            Operator="DataTypeCheck" Type="Date"><%=Resources.Resource.DataInvalida %></asp:CompareValidator>
                    </td>
                </tr>
            <tr>
                <td>
                    <%=Resources.Resource.CalibradoInternamente %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbCalibInt" runat="server"></asp:CheckBox>
                </td>
                <td>
                    <%=Resources.Resource.CertificadoConclusivo %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbCertConclusivo" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Observacoes %>:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="526px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Campo Etiqueta1
                </td>
                <td>
                    <asp:TextBox ID="txtEtiqueta1" runat="server" MaxLength="30" Width="100"></asp:TextBox>
                </td>
                <td>
                    Campo Etiqueta2
                </td>
                <td>
                    <asp:TextBox ID="txtEtiqueta2" runat="server" MaxLength="30"  Width="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Campo Etiqueta3
                </td>
                <td>
                    <asp:TextBox ID="txtEtiqueta3" runat="server" MaxLength="30"  Width="100"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
             <tr>
                <td>
                    <%=Resources.Resource.CriteriosAceitacao %>:
                </td>
                <td colspan="3">
                    
                    <asp:TextBox ID="txtCriteriosAceitacao" runat="server" Width="100%" Height="150" TextMode="MultiLine" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Gravar %>">
                    </asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="4" width="80%">
                    <p>
                        <br />
                        <%=Resources.Resource.FraseInfoCampExtra %></p>
                </td>
            </tr>
        </table>
        <fieldset>
            <legend>
                <%=Resources.Resource.HistoricoEstados %>
            </legend>
            <br />
            <asp:DataGrid ID="dgHistorico" runat="server" ShowFooter="true" AllowSorting="False"
                AutoGenerateColumns="true" />
        </fieldset>
    </fieldset>
</asp:Content>
