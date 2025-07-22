<%@ Page Language="c#" CodeBehind="GestAvisoEmpresas.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestAvisoEmpresas" MasterPageFile="~/mp.Master" %>

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
        <legend><%=Resources.Resource.EnviarFaxAviso %></legend>
        <table cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <label>
                    <%=Resources.Resource.EmpresaComEquipCalib %></label
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ListBox ID="lbEmpresa" runat="server" Height="100" SelectionMode="Single" AutoPostBack="true"
                        OnSelectedIndexChanged="lbEmpresa_SelectedIndexChanged"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <label><%=Resources.Resource.BREsEmpresa%></label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="ddBRE" runat="server" AutoPostBack="true" DataValueField="idBRE"
                        DataTextField="refBRE" OnSelectedIndexChanged="ddBRE_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <label><%=Resources.Resource.Equipamento %></label><br />
                    <asp:DataGrid 
                    ID="dgEquipamento" 
                    runat="server" 
                    AutoGenerateColumns="false" >
                        <Columns>
                            <asp:BoundColumn DataField="tipoEquipamento" HeaderText="<%$ Resources:Resource, TipoEquipamento %>"></asp:BoundColumn>
                            <asp:BoundColumn DataField="numIdentificacao" HeaderText="<%$ Resources:Resource, NumIdentificacao %>"></asp:BoundColumn>
                            <asp:BoundColumn DataField="refServico" HeaderText="<%$ Resources:Resource, RefCalib %>"></asp:BoundColumn>
                            <asp:BoundColumn DataField="estadoServico" HeaderText="<%$ Resources:Resource, Estado %>"></asp:BoundColumn>
                            <asp:BoundColumn DataField="refBRE" HeaderText="<%$ Resources:Resource, NumBRE %>"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Contacto %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddContacto" runat="server" AutoPostBack="true" DataValueField="idContacto"
                        DataTextField="descricao">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.FaxContacto %>:
                </td>
                <td>
                    <asp:TextBox ID="txtFaxContacto" runat="server" Width="125px" ReadOnly="True"></asp:TextBox><asp:RadioButton
                        ID="rbFaxContacto" runat="server" AutoPostBack="True" Text="<%$ Resources:Resource, UsarNumero %>" OnCheckedChanged="rbFaxContacto_CheckedChanged">
                    </asp:RadioButton>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.FaxEmpresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtFaxEmpresa" runat="server" Width="125px" ReadOnly="True"></asp:TextBox><asp:RadioButton
                        ID="rbFaxEmpresa" runat="server" AutoPostBack="True" Text="<%$ Resources:Resource, UsarNumero %>" OnCheckedChanged="rbFaxEmpresa_CheckedChanged">
                    </asp:RadioButton>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.EnviarPara %>:
                </td>
                <td>
                    <asp:TextBox ID="txtFaxParaEnvio" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.EquipamentoASerLevantadoEM %>:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblLocalLavantamento" runat="server" RepeatLayout="Flow"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="Lisboa" Text="<%$ Resources:Resource, Lisboa %>"></asp:ListItem>
                        <asp:ListItem Value="Porto" Text="<%$ Resources:resource, Porto %>"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="req1" runat="server" ErrorMessage="Indique Local"
                        ControlToValidate="rblLocalLavantamento"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Enviar %>"
                        OnClick="btnSubmit_Click"></asp:Button>&nbsp;&nbsp;&nbsp;<asp:Button class="button"
                            ID="btnVer" runat="server" CssClass="button" Text="<%$ Resources:Resource, VerFax %>" OnClick="btnVer_Click">
                        </asp:Button>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend><%=Resources.Resource.FaxesEnviadosAnterior %></legend>
        <table>
            <tr>
                <td colspan="2">
                    <asp:ListBox ID="lbFaxesEnviados" runat="server" Height="100" SelectionMode="Single"
                        AutoPostBack="true" DataValueField="idFax" DataTextField="dtEnvio" Width="274px"
                        OnSelectedIndexChanged="lbFaxesEnviados_SelectedIndexChanged"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DataGrid 
                    ID="dgEquipamentosAvisados" 
                    runat="server" 
                    AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundColumn DataField="tipoEquipamento" HeaderText="Tipo de Equipamento"></asp:BoundColumn>
                            <asp:BoundColumn DataField="numIdentificacao" HeaderText="Núm.Ident."></asp:BoundColumn>
                            <asp:BoundColumn DataField="refServico" HeaderText="Ref.Calib."></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <a href="GestFaxesEnviados.aspx" target="_self"><%=Resources.Resource.VerFaxesEnviados %></a>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    </fieldset>
    <!-- FIM body -->
</asp:Content>
