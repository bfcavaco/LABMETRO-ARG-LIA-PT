<%@ Page Language="c#" CodeBehind="FormRequisicao.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormRequisicao" MasterPageFile="~/mp.Master" MaintainScrollPositionOnPostback="true" %>

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
        <legend><%=Resources.Resource.Requisicao %></legend>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" CssClass="lblMessage"
            ShowSummary="true"></asp:ValidationSummary>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblNumRequisicao" runat="server"></asp:Label>
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
                    <asp:TextBox ID="txtPesquisaNif" runat="server" AutoPostBack="true"></asp:TextBox>&nbsp;
                    &nbsp;<asp:Button ID="btnEmpresas" CssClass="button" runat="server"
                        Width="80" CausesValidation="false" Text="<%$ Resources:Resource, VerEmpresas %>"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <%=Resources.Resource.Empresa %>:
                    <br />
                    <asp:DropDownList ID="ddEmpresa" runat="server" AutoPostBack="true"                     DataTextField="nome" DataValueField="idEmpresa">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="ddEmpresa">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Observacoes %>:
                </td>
                <td>
                    <asp:Label ID="lblObsEmpresa" runat="server"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblEmpresaDevedora" runat="server" CssClass="lblMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.RefCliente %>:
                </td>
                <td>
                    <asp:TextBox ID="txtRefCliente" runat="server" Width="218px" MaxLength="35"></asp:TextBox><asp:RequiredFieldValidator
                        ID="req1" runat="server" ControlToValidate="txtRefCliente">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.DtReq %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDataRequisicao" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator6" runat="server" ControlToValidate="txtDataRequisicao" Type="Date"
                        Operator="DataTypeCheck">dd-mm-aaaa!</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.NomeFicheiro %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeFicheiro" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <%=Resources.Resource.DataValidade %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDataValidade" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator5" runat="server" ControlToValidate="txtDataValidade" Type="Date"
                        Operator="DataTypeCheck">dd-mm-aaaa!</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <%=Resources.Resource.Ficheiro %>:<input id="fileIn" type="file" size="59" name="fileIn" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnUpload" CssClass="button" runat="server" CausesValidation="false" Text="<%$ Resources:Resource, CarregarFicheiro  %>"></asp:Button>
                </td>
                <td colspan="2" align="left">
                    <asp:Button ID="btnRemove" CssClass="button" runat="server" Text="<%$ Resources:Resource, RemoverFicheiro %>">
                    </asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Completa %>?
                </td>
                <td>
                    <asp:RadioButtonList ID="rblCompleta" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="1" Text="<%$ Resources:Resource, Sim %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:Resource, Nao %>"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" ControlToValidate="rblCompleta">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.Observacoes %>:
                </td>
                <td>
                    <asp:TextBox ID="txtObservacoes" runat="server" Width="368px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Contrato %>?
                </td>
                <td align="left">
                    <asp:CheckBox ID="chbContrato" runat="server"></asp:CheckBox>
                </td>
                <td >
                    <%=Resources.Resource.Renovavel %>?
                </td>
                <td align="left">
                    <asp:CheckBox ID="chbRenovavel" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button cssClass="button_confirm" ID="btnSubmit" runat="server" CausesValidation="true"
                        Text="<%$ Resources:Resource, Gravar %>"></asp:Button>
                </td>
                <td>
          
                </td>
                <td>
                </td>
            </tr>
        </table>
       
    </fieldset>
</asp:Content>
