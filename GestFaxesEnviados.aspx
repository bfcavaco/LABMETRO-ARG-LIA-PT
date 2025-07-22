<%@ Page Language="c#" CodeBehind="GestFaxesEnviados.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.GestFaxesEnviados" MasterPageFile="~/mp.Master" %>

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
        <legend>Faxes de aviso Enviados</legend>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <table>
            <tr>
                <td colspan="2">
                    Empresa com faxes enviados anteriormente:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ListBox ID="lbEmpresasComFax" runat="server" Height="100" SelectionMode="Single"
                        AutoPostBack="true"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td>
                    Contacto:
                </td>
                <td>
                    <asp:DropDownList ID="ddContacto" runat="server" AutoPostBack="true" DataTextField="descricao"
                        DataValueField="idContacto">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Fax Contacto:
                </td>
                <td>
                    <asp:TextBox ID="txtFaxContacto" runat="server" Width="125px" ReadOnly="True"></asp:TextBox><asp:RadioButton
                        ID="rbFaxContacto" runat="server" AutoPostBack="True" Text="usar número"></asp:RadioButton>
                </td>
            </tr>
            <tr>
                <td>
                    Fax Empresa:
                </td>
                <td>
                    <asp:TextBox ID="txtFaxEmpresa" runat="server" Width="125px" ReadOnly="True"></asp:TextBox><asp:RadioButton
                        ID="rbFaxEmpresa" runat="server" AutoPostBack="True" Text="usar número"></asp:RadioButton>
                </td>
            </tr>
            <tr>
                <tr>
                    <td>
                        Enviar para:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFaxParaEnvio" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Equipamento a ser Levantado em:
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblLocalLavantamento" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="Lisboa">Lisboa</asp:ListItem>
                            <asp:ListItem Value="Porto">Porto</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="rblLocalLavantamento"
                            ErrorMessage="Indique Local"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button class="button" ID="btnSubmit" runat="server" CssClass="button" Text="Enviar Fax">
                        </asp:Button>&nbsp;&nbsp;&nbsp;<asp:Button class="button" ID="btnVer" runat="server"
                            CssClass="button_red" Text="Ver Fax"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:DataGrid ID="dgFaxesEnviados" runat="server" Width="60%" CssClass="DG_branco"
                            GridLines="horizontal" BorderColor="#E0E0E0" HeaderStyle-BackColor="#999999"
                            HeaderStyle-Font-Bold="True" HeaderStyle-ForeColor="#FFFFFF" AutoGenerateColumns="false"
                            DataKeyField="idFax" OnSelectedIndexChanged="dgFaxesEnviados_SelectedIndexChanged"
                            SelectedItemStyle-BackColor="RosyBrown">
                            <Columns>
                                <asp:BoundColumn DataField="dtEnvio" HeaderText="Data de envio"></asp:BoundColumn>
                                <asp:ButtonColumn CommandName="select" runat="server" Text="seleccionar"></asp:ButtonColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:DataGrid 
                        ID="dgEquipamentosAvisados" 
                        runat="server" 
                        Width="60%" 
                        AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundColumn DataField="tipoEquipamento" HeaderText="Tipo de Equipamento"></asp:BoundColumn>
                                <asp:BoundColumn DataField="numIdentificacao" HeaderText="Núm.Ident."></asp:BoundColumn>
                                <asp:BoundColumn DataField="refServico" HeaderText="Ref.Calib."></asp:BoundColumn>
                                <asp:BoundColumn DataField="Estado" HeaderText="Estado"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
        </table>
    </fieldset>
</asp:Content>
