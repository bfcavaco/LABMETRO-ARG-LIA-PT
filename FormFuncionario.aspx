<%@ Page Language="c#" CodeBehind="FormFuncionario.aspx.cs" AutoEventWireup="True"
    Inherits="LabMetro.FormFuncionario" MasterPageFile="~/mp.Master" %>

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
    <!-- body -->
    <fieldset>
        <legend><%=Resources.Resource.Funcionario %> </legend>
        <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="Preencha por favor os campos marcados com *."
            ShowSummary="true" CssClass="lblMessage" DisplayMode="SingleParagraph"></asp:ValidationSummary>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Nome %> :
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtNome" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" ControlToValidate="txtNome">*</asp:RequiredFieldValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                   <%=Resources.Resource.NomeAbreviado %>:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeAbreviado" runat="server" MaxLength="25" Width="200px"></asp:TextBox><asp:RequiredFieldValidator
                        ID="Requiredfieldvalidator1" runat="server" ControlToValidate="txtNomeAbreviado">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.Funcao %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddFuncao" runat="server" DataTextField="descricao" DataValueField="ident">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="ddFuncao">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Laboratorio %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddLaboratorio" runat="server" DataTextField="laboratorio" DataValueField="idLaboratorio">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" ControlToValidate="ddLaboratorio">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <%=Resources.Resource.LocalCalibracao %>:
                </td>
                <td>
                    <asp:DropDownList ID="ddLocalCalibracao" runat="server" DataTextField="descricao"
                        DataValueField="ident">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator5" runat="server" ControlToValidate="ddLocalCalibracao">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.DataAdmissao %>:
                </td>
                <td>
                    <asp:TextBox ID="txtDataAdmissao" runat="server"></asp:TextBox><asp:CompareValidator
                        ID="Comparevalidator2" runat="server" ControlToValidate="txtDataAdmissao" Type="Date"
                        Operator="DataTypeCheck">dd-mm-aaaa!</asp:CompareValidator>
                </td>
                <td>
                    <%=Resources.Resource.CTA %>:
                </td>
                <td>
                    <asp:CheckBox ID="cbCTA" runat="server"></asp:CheckBox>
                </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Telefone %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelefone" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <%=Resources.Resource.EXT%>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtExtensao" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Email %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    </td>
                    <td>Num. Funcionário (VD)
                    </td>
                    <td><asp:TextBox ID="txtNumFuncionario" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Resources.Resource.Activo %>:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddEstado" runat="server">
                            <asp:ListItem Value="1" Text="<%$ Resources:Resource, Activo %>"></asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources:Resource, Inactivo %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%=Resources.Resource.Observacoes %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtObservacoes" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button class="button" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, Enviar %>" 
                            CausesValidation="true" OnClick="btnSubmit_Click"></asp:Button>
                    </td>
                </tr>
        </table>
    </fieldset>
</asp:Content>
