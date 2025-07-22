<%@ Page Language="c#" CodeBehind="FormContacto.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.FormContacto"
    MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

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
        <legend><%=Resources.Resource.Contacto %></legend>
        <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
            ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaEmpresa" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaEmpresa_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.NIF %>:
                </td>
                <td>
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true" OnTextChanged="txtPesquisaNif_TextChanged"></asp:TextBox>&nbsp;
                    &nbsp;<asp:Button class="button" ID="btnEmpresas" CssClass="button" runat="server"
                        CausesValidation="false" Text="<%$ Resources:Resource, verEmpresas %>" OnClick="btnEmpresas_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <%=Resources.Resource.Empresa %>:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddEmpresa" runat="server" DataValueField="idEmpresa" DataTextField="nome">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Nome %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddtitulo" runat="server" DataValueField="ident" DataTextField="descricao">
                    </asp:DropDownList>
                    &nbsp;<asp:TextBox ID="txtNome" runat="server" MaxLength="100" Width="200px"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNome">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.Extensao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtExtensao" runat="server"></asp:TextBox>
                </td>
                <tr>
                    <td>
                        <%=Resources.Resource.TelefoneDirecto %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelefoneDirecto" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.Telemovel %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelemovel" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.EmailDirecto %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailDirecto" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.FaxDir %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFaxDirecto" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Departamento %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDepartamento" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.Cargo %>:
                </td>
                <td>
                    <asp:TextBox ID="txtCargo" runat="server" Width="200px" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.ContactoPrincipal %>:
                </td>
                <td>
                    <asp:RadioButtonList ID="rblContactoPrincipal" runat="server" RepeatLayout="Flow"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="rblContactoPrincipal">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.Estado%>
                </td>
                <td>
                    
                    <asp:DropDownList ID="ddEstado" runat="server">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, activo %> "></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, inactivo %>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td><%=Resources.Resource.Observacoes %>:</td>
                <td colspan="3">
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="520px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <fieldset>
            <legend><%=Resources.Resource.AreasResponsabilidade %></legend>
            <table>
                <tr>
                    <td>
                        <%=Resources.Resource.Facturacao %>:<br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chbFacturacao" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                        <%=Resources.Resource.Orcamento %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbOrcamento" runat="server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Qualidade %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbQualidade" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                        <%=Resources.Resource.ManutencaoAprovisionamento %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbManutencao" runat="server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Requisicoes %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbRequisicoes" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                      
                    </td>
                    <td>
                       
                    </td>
                </tr>
                </table>
            </fieldset>
        <fieldset>
            <legend></legend>
            <table>
                   <tr>
                    <td>
                        <%=Resources.Resource.AcessoCertificados %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbCertificados" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                        <%=Resources.Resource.GestaoEquipamentos %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbGestaoEquipamentos" runat="server" Enabled="true"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.AlertasLevantamentos %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbAlertasLevantamentos" runat="server" Enabled="false"></asp:CheckBox>
                    </td>
                    <td>
                        <%=Resources.Resource.AlertasNovosCertificados %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbAlertasNovosCertificados" runat="server" Enabled="false"></asp:CheckBox>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <%=Resources.Resource.AlertasPlanosCalibracao %>:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbAlertasPlanosCalibracao" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                      
                    </td>
                    <td>
                       
                    </td>
                </tr>
                
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button class="button_confirm" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Gravar %>" OnClick="btnSubmit_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </fieldset>
    </fieldset>
</asp:Content>
