<%@ Page Language="c#" CodeBehind="GestaoEquipamentos.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestaoEquipamentos" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headerContent">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="scriptContent">

    <script type="text/javascript" language="javascript">
        function CheckKey() {
            if (event.keyCode == 13) {
                document.getElementById("btnPesquisaEmpresa").focus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <fieldset>
        <legend>
            <%=Resources.Resource.GestaoEquipamentos %></legend>PER
        <asp:ObjectDataSource ID="OBJDS_Marca_WS" runat="server" SelectMethod="wsDTMarca"
            TypeName="LabMetro.Webservices.wsMarcaModelo"></asp:ObjectDataSource>
        <table>
            <tr>
                <td colspan="7">
                    <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>
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
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Button class="button" ID="btnPesquisaEmpresa" runat="server" CssClass="button"
                        CausesValidation="False" Text="<%$ Resources:Resource, verEmpresas %>"></asp:Button>
                </td>
                <td>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true" DataValueField="idEmpresa"
                        DataTextField="nome">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <%=Resources.Resource.FiltrarEquipamentos %>:
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.TipoEquipamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtTipoEquipamento" runat="server"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NumSerie %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNSerie" runat="server"></asp:TextBox>
                </td>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NumIdent %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdent" runat="server"></asp:TextBox>
                </td>
                <td colspan="5">
                    <%=Resources.Resource.SoEquipActivos %>:&nbsp;<asp:CheckBox ID="cbActivos" runat="server">
                    </asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:Button class="button" ID="btnSearch" runat="server" Text="<%$ Resources:Resource, PesquisarEquipamentos %>"
                        CssClass="button" CausesValidation="false"></asp:Button>&nbsp;<asp:Button class="button"
                            ID="btnLimparCampos" runat="server" Text="<%$ Resources:Resource, LimparCampos %>"
                            CssClass="button" CausesValidation="false"></asp:Button>
                </td>
            </tr>
        </table>
        <%=Resources.Resource.OrdenarResultados %>:
        <asp:DropDownList ID="ddFiltro" runat="server" AutoPostBack="True">
            <asp:ListItem Text="<%$ Resources:Resource, TipoEquipamento %>" Value="TipoEquipamento"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, NumIdentificacao %>" Value="numIdentificacao"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, NumSerie %>" Value="numSerie"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, Marca %>" Value="marca"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, ReferenciaUltimaCalibracao %>" Value="modelo"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, DataUltimaCalibracao %>" Value="dtUltimaCalibracao"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, Estado %>" Value="Estado"></asp:ListItem>
            <asp:ListItem Text="<%$ Resources:Resource, DtCriacao %>" Value="dtCriacao"></asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Button class="button" ID="bntShowHeader" runat="Server" Text="<%$ Resources:Resource, InserirNovoEquipamento %>"
            CausesValidation="False" CssClass="button"></asp:Button>
        <br />
        <br />
        <table bgcolor="white" bordercolor="gainsboro" border="1" cellpadding="3" cellspacing="0">
            <tr>
                <td colspan="8" bgcolor="#999999">
                    <div>
                        <asp:TextBox ID="Textbox1" Enabled="False" BackColor="white" Width="10" Height="10"
                            runat="server"></asp:TextBox>&nbsp;<%=Resources.Resource.EquipamentosActivos %><br />
                        <asp:TextBox ID="Textbox2" Enabled="False" BackColor="red" Width="10" Height="10"
                            runat="server"></asp:TextBox>&nbsp;<%=Resources.Resource.EquipamentosInactivos %><br />
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#999999">
                    <div>
                        &nbsp;&nbsp;<%=Resources.Resource.ID_BD %></div>
                </td>
                <td bgcolor="#999999" width="20%">
                    <div>
                        &nbsp;&nbsp;<%=Resources.Resource.Equipamento %></div>
                </td>
                <td width="15%" bgcolor="#999999">
                    <div>
                        &nbsp;&nbsp;<%=Resources.Resource.NumIdent %></div>
                </td>
                <td width="15%" bgcolor="#999999">
                    <div>
                        &nbsp;&nbsp;<%=Resources.Resource.NumSerie %></div>
                </td>
                <td width="10%" bgcolor="#999999">
                    <div>
                        <%=Resources.Resource.Marca %>:</div>
                </td>
                <td width="10%" bgcolor="#999999">
                    <div>
                        <%=Resources.Resource.DataUltimaCalibracao %>:</div>
                </td>
                <td width="15%" bgcolor="#999999">
                    <div>
                        <%=Resources.Resource.RefUltimaCalibracao %>:</div>
                </td>
                <td width="5%" bgcolor="#999999">
                    <div>
                        &nbsp;</div>
                </td>
            </tr>
        </table>
        <asp:DataList ID="DLEquipamentos" runat="server" DataKeyField="idEquipamento" OnDeleteCommand="Delete_Command"
            OnUpdateCommand="Update_Command" OnCancelCommand="Cancel_Command" OnEditCommand="Edit_Command"
            RepeatColumns="1" RepeatLayout="Table" RepeatDirection="Horizontal" ExtractTemplateRows="False"
            OnItemDataBound="DLEquipamentos_ItemDataBound" ShowHeader="False" OnItemCommand="DL_ItemCommand"
            BackColor="#cccccc" GridLines="Horizontal" BorderColor="white">
            <HeaderStyle Font-Bold="False" ForeColor="#FFFFFF" BackColor="#999999"></HeaderStyle>
            <ItemTemplate>
                <table bgcolor="White" border="1px" cellpadding="2" cellspacing="0" style="table-layout: fixed">
                    <tr>
                        <td bgcolor="Gainsboro">
                            &nbsp;<b><%# DataBinder.Eval(Container.DataItem, "idEquipamento") %></b>
                        </td>
                        <td bgcolor="Gainsboro" width="20%">
                            <asp:TextBox ID="txtColor" Enabled="False" BackColor='<%# ConvertColor(Convert.ToInt16(DataBinder.Eval(Container, "DataItem.Estado")))%>'
                                Width="10" Height="10" runat="server">
                            </asp:TextBox>&nbsp;
                            <%# DataBinder.Eval(Container.DataItem, "TipoEquipamento")%>
                            <br />
                        </td>
                        <td width="15%" bgcolor="Gainsboro" style="word-wrap: break-word;">
                            &nbsp;<b><%# DataBinder.Eval(Container.DataItem, "numIdentificacao") %></b>
                        </td>
                        <td width="15%" bgcolor="Gainsboro">
                            &nbsp;<b><%# DataBinder.Eval(Container.DataItem, "numSerie") %></b>
                        </td>
                        <td width="10%" bgcolor="Gainsboro">
                            &nbsp;<b><%# DataBinder.Eval(Container.DataItem, "Marca") %></b>
                        </td>
                        <td width="10%" bgcolor="Gainsboro" align="right" style="white-space: nowrap;">
                            &nbsp;<%#LabMetro.GERAL.clsGeral.ToShortDate(DataBinder.Eval(Container.DataItem, "dtUltimaCalibracao").ToString())%>
                        </td>
                        <td width="15%" bgcolor="Gainsboro" style="white-space: nowrap;">
                            &nbsp;<%# DataBinder.Eval(Container, "DataItem.refUltimaCalibracao") %>
                        </td>
                        <td width="5%" align="center">
                            <asp:LinkButton Text="Editar" CommandName="Edit" runat="server" ID="edit" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <EditItemTemplate>
                <table cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <%=Resources.Resource.Activo %>:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddEstado" runat="server">
                                <asp:ListItem Value="1">activo</asp:ListItem>
                                <asp:ListItem Value="0">inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.TipoEquipamento %>:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddTipoEquipamento" runat="server" AutoPostBack="false" DataValueField="ident"
                                DataTextField="descricao">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddTipoEquipamento"
                                ID="reqTipoEquipamento">*</asp:RequiredFieldValidator>
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
                            <%=Resources.Resource.NumIdent %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumIdentif" runat="server" MaxLength="150"></asp:TextBox>
                            <asp:CustomValidator OnServerValidate="validaEquipamento" runat="server" ID="custValidaEquipamento"
                                ErrorMessage="Indique NºSérie ou NºIdent." Display="Dynamic">*Indique NºSérie ou NºIdent.EDIT</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.GamaEntre %>:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAlcanceInf" runat="server" Width="50"></asp:TextBox>
                            <asp:CompareValidator ID="comAlcanceInf" runat="server" CssClass="lblMessage" ControlToValidate="txtAlcanceInf"
                                Operator="DataTypeCheck" Type="Double" ErrorMessage="!num" Display="static"></asp:CompareValidator>&nbsp;-&nbsp;
                            <asp:TextBox ID="txtAlcanceSup" runat="server" Width="50"></asp:TextBox>
                            <asp:CompareValidator ID="compAlcanceSup" runat="server" CssClass="lblMessage" ControlToValidate="txtAlcanceSup"
                                Operator="DataTypeCheck" Type="Double" ErrorMessage="!num" Display="static"></asp:CompareValidator><%=Resources.Resource.UnidadeGama %>:
                            <asp:DropDownList ID="ddUnidadeAlcance" runat="server" DataValueField="ident" DataTextField="descricao">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 23px">
                            <%=Resources.Resource.OuGama %>:
                        </td>
                        <td style="height: 23px">
                            <asp:TextBox ID="txtAlcance" runat="server"></asp:TextBox>
                        </td>
                        <td style="height: 23px">
                            <%=Resources.Resource.Divisao%>:
                        </td>
                        <td style="height: 23px">
                            <asp:TextBox ID="txtResolucao" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.Marca %>:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddMarca" runat="server" AutoPostBack="true" DataValueField="idMarca"
                                DataTextField="descricao" Width="175px"  DataSourceID="OBJDS_Marca_WS" OnDataBound="ddMarcaEditDatabound">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <%=Resources.Resource.Modelo %>:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="OBJDS_ModeloEdit" runat="server" SelectMethod="wsDTModelo"
                                TypeName="LabMetro.Webservices.wsMarcaModelo">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddMarca" Name="idMarca" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:UpdatePanel ID="updateModeloEdit" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddMarca" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddModelo" runat="server" DataValueField="idModelo" DataTextField="descricao" Width="175px" DataSourceID="OBJDS_ModeloEdit" OnDataBound="ddModeloEditDatabound">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.Classe %>:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddClasse" runat="server" DataValueField="idClasse" DataTextField="descricao"
                                Width="120">
                            </asp:DropDownList>
                            &nbsp;<%=Resources.Resource.Ou %>
                            <asp:TextBox ID="txtClasse" runat="server" Width="60"></asp:TextBox>
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
                           Etiqueta 1
                        </td>
                        <td>
                            <asp:TextBox ID="txtEtiqueta1" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                        <td>
                            Etiqueta 2
                        </td>
                        <td>
                            <asp:TextBox ID="txtEtiqueta2" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Etiqueta 3
                        </td>
                        <td>
                            <asp:TextBox ID="txtEtiqueta3" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            
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
                            <%=Resources.Resource.PeriodicidadeCalib%>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodicidade" runat="server"></asp:TextBox>&nbsp;(meses)
                             <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPeriodicidade" MaximumValue="120" MinimumValue="1" Type="Integer" ErrorMessage="val.per."></asp:RangeValidator>
                        </td>
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
                                <asp:TextBox ID="txtDataUltimaCalib" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="compDataUltimaCalib" runat="server" ControlToValidate="txtDataUltimaCalib"
                                    Operator="DataTypeCheck" Type="Date">dd-mm-aaaa!</asp:CompareValidator>
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
                                <%=Resources.Resource.RequerCertificadoConclusivo %>:
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
                </table>
                <div align="right">
                    <asp:LinkButton Text="<%$ Resources:Resource, Alterar %>" CommandName="Update" runat="server"
                        ID="update" CausesValidation="True" />
                    <asp:LinkButton Text="<%$ Resources:Resource, Cancelar %>" CommandName="Cancel" runat="server"
                        ID="cancel" CausesValidation="False" /></div>
            </EditItemTemplate>
            <HeaderTemplate>
                <table bordercolor="#b90000" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <%=Resources.Resource.Activo %>:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddEstadoHeader" runat="server">
                                <asp:ListItem Value="1">activo</asp:ListItem>
                                <asp:ListItem Value="0">inactivo</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.TipoEquipamento %>:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddTipoEquipamentoHeader" runat="server" DataValueField="ident"
                                DataTextField="descricao">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddTipoEquipamentoHeader"
                                ID="reqTipoEquipamentoHeader">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.NumSerie %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumSerieHeader" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <%=Resources.Resource.NumIdent %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumIdentifHeader" runat="server"></asp:TextBox>
                            <asp:CustomValidator OnServerValidate="validaEquipamentoHeader" runat="server" ID="custValidaEquipamentoHeader"
                                ErrorMessage="Indique NºSérie ou NºIdent.HEADER" Display="Dynamic" Enabled="False">*Indique NºSérie ou NºIdent.</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.GamaEntre %>:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAlcanceInfHeader" runat="server" Width="50"></asp:TextBox>
                            <asp:CompareValidator ID="compAlcanceInfHeader" runat="server" CssClass="lblMessage"
                                ControlToValidate="txtAlcanceInfHeader" Operator="DataTypeCheck" Type="Double"
                                ErrorMessage="!num" Display="static" Enabled="False"></asp:CompareValidator>&nbsp;-&nbsp;
                            <asp:TextBox ID="txtAlcanceSupHeader" runat="server" Width="50"></asp:TextBox>
                            <asp:CompareValidator ID="comlcanceSupHeader" runat="server" CssClass="lblMessage"
                                ControlToValidate="txtAlcanceSupHeader" Operator="DataTypeCheck" Type="Double"
                                ErrorMessage="!num" Display="static" Enabled="False"></asp:CompareValidator>Unid.
                            <%=Resources.Resource.Gama %>:
                            <asp:DropDownList ID="ddUnidadeAlcanceHeader" runat="server" DataValueField="ident"
                                DataTextField="descricao">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 23px">
                            <%=Resources.Resource.OuGama %>:
                        </td>
                        <td style="height: 23px">
                            <asp:TextBox ID="txtAlcanceHeader" runat="server"></asp:TextBox>
                        </td>
                        <td style="height: 23px">
                            <%=Resources.Resource.Divisao %>:
                        </td>
                        <td style="height: 23px">
                            <asp:TextBox ID="txtResolucaoHeader" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.Marca %>:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddMarcaHeader" runat="server" AutoPostBack="true" DataValueField="idMarca"
                                DataTextField="descricao" Width="175px"  DataSourceID="OBJDS_Marca_WS" OnDataBound="ddMarcaEditDatabound">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <%=Resources.Resource.Modelo %>:
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="OBJDS_ModeloEdit" runat="server" SelectMethod="wsDTModelo"
                                TypeName="LabMetro.Webservices.wsMarcaModelo">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddMarcaHeader" Name="idMarca" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:UpdatePanel ID="updateModeloInsert" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddMarcaHeader" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddModeloHeader" runat="server" DataValueField="idModelo" DataTextField="descricao" Width="175px" DataSourceID="OBJDS_ModeloEdit" OnDataBound="ddModeloEditDatabound">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 24px">
                            <%=Resources.Resource.Classe %>:
                        </td>
                        <td style="height: 24px">
                            <asp:DropDownList ID="ddClasseHeader" runat="server" DataValueField="idClasse" DataTextField="descricao"
                                Width="120">
                            </asp:DropDownList>
                            &nbsp;<%=Resources.Resource.Ou %>:
                            <asp:TextBox ID="txtClasseHeader" runat="server" Width="60"></asp:TextBox>
                        </td>
                        <td style="height: 24px">
                            <%=Resources.Resource.Indicacao %>:
                        </td>
                        <td style="height: 24px">
                            <asp:TextBox ID="txtFormaHeader" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 29px">
                            <%=Resources.Resource.Fabricante %>:
                        </td>
                        <td style="height: 29px">
                            <asp:TextBox ID="txtFabricanteHeader" runat="server"></asp:TextBox>
                        </td>
                        <td style="height: 29px">
                            <%=Resources.Resource.PeriodicidadeCalib %>:
                        </td>
                        <td style="height: 29px">
                            <asp:TextBox ID="txtPeriodicidadeHeader" runat="server"></asp:TextBox>&nbsp;(meses)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.CampExtra1 %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCampo1Header" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <%=Resources.Resource.CampExtra2 %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCampo2Header" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          Etiqueta 1
                        </td>
                        <td>
                            <asp:TextBox ID="txtEtiqueta1Header" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                        <td>
                            Etiqueta 2
                        </td>
                        <td>
                            <asp:TextBox ID="txtEtiqueta2Header" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Etiqueta 3
                        </td>
                        <td>
                            <asp:TextBox ID="txtEtiqueta3Header" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                        <td>
                           
                        </td>
                        <td>
                           
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <%=Resources.Resource.ReferenciaUltimaCalibracao %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefUltimaCalibHeader" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <%=Resources.Resource.DataUltimaCalibracao %>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataUltimaCalibHeader" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="comDataUltimaCalibHeader" runat="server" ControlToValidate="txtDataUltimaCalibHeader"
                                Operator="DataTypeCheck" Type="Date" Enabled="false">dd-mm-aaaa!</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.CalibradoInternamente %>:
                        </td>
                        <td>
                            <asp:CheckBox ID="cbCalibIntHeader" runat="server"></asp:CheckBox>
                        </td>
                        <td>
                            <%=Resources.Resource.RequerCertificadoConclusivo %>:
                        </td>
                        <td>
                            <asp:CheckBox ID="cbCertConclusivoHeader" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=Resources.Resource.Observacoes %>:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtObservacoesHeader" runat="server" Width="526px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div align="right">
                    <asp:LinkButton Text="<%$ Resources:Resource, Inserir %>" CommandName="insereEquipamento"
                        runat="server" ID="insert" CausesValidation="True" />
                    <asp:LinkButton Text="<%$ Resources:Resource,Cancelar %>" CommandName="cancelInsert"
                        runat="server" ID="cancelHeader" CausesValidation="False" /></div>
            </HeaderTemplate>
        </asp:DataList>
    </fieldset>
</asp:Content>
